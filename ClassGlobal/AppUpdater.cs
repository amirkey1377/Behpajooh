using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Velopack;

namespace Skydat
{
    public static class AppUpdater
    {
        private const string WebUpdateBaseUrl =
            "https://www.behpajooh.net/wp-content/uploads/2026/10/";

        private const string WebFeedFileName = "releases.json";

        private static readonly string UpdateCacheFolder =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "BHP.Updates",
                "WebFeedCache");

        private static readonly HttpClient Http = CreateHttpClient();

        private static HttpClient CreateHttpClient()
        {
            var c = new HttpClient();
            c.Timeout = TimeSpan.FromMinutes(5);
            c.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Skydat-Updater/1.0 (Windows NT; Velopack)");
            c.DefaultRequestHeaders.Accept.ParseAdd(
                "application/json, */*;q=0.1");
            return c;
        }

        public static async Task CheckAndUpdateAsync()
        {
            string onlineError = null;

            try
            {
                string onlineFolder =
                    await PrepareWebReleaseFolderAsync();

                UpdateResult result =
                    await CheckUpdate(onlineFolder);

                if (result != UpdateResult.SourceUnavailable)
                    return;
            }
            catch (Exception ex)
            {
                onlineError = ex.Message;
            }
            try
            {
                string localReleases = FindLocalReleases();
                if (!string.IsNullOrEmpty(localReleases))
                {
                    UpdateResult result =
                        await CheckUpdate(localReleases);
                    if (result != UpdateResult.SourceUnavailable)
                        return;
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(onlineError))
                    onlineError = ex.Message;
            }


            string message =
                "منبع بروزرسانی آنلاین یا محلی در دسترس نیست.";
            if (!string.IsNullOrEmpty(onlineError))
                message += "\n\nجزئیات: " + onlineError;
            message += "\n\nآیا می‌خواهید فایل بروزرسانی را دستی انتخاب کنید؟";

            DialogResult answer = MessageBox.Show(
                message, "بروزرسانی",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (answer == DialogResult.Yes)
            {
                await PromptForManualUpdateAsync();
            }
        }

        // متد انتخاب فایل به جای انتخاب پوشه
        private static async Task PromptForManualUpdateAsync()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "فایل بروزرسانی (releases.win.json) را انتخاب کنید";
                dialog.Filter = "فایل بروزرسانی (releases.win.json)|releases.win.json|فایل‌های JSON (*.json)|*.json|همه فایل‌ها (*.*)|*.*";
                dialog.CheckFileExists = true;

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                // بررسی اینکه آیا کاربر دقیقاً فایل درست را انتخاب کرده است یا خیر
                if (!Path.GetFileName(dialog.FileName).Equals("releases.win.json", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "لطفاً دقیقاً فایل 'releases.win.json' را که همراه بسته‌های آپدیت است انتخاب کنید.",
                        "فایل نامعتبر",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Velopack به پوشه‌ی حاوی این فایل نیاز دارد، پس مسیر پوشه را استخراج می‌کنیم
                string selectedFolder = Path.GetDirectoryName(dialog.FileName);

                await CheckUpdate(selectedFolder, true);
            }
        }

        private static async Task<string> PrepareWebReleaseFolderAsync()
        {
            string baseUrl = WebUpdateBaseUrl.TrimEnd('/');
            string feedUrl = baseUrl + "/" + WebFeedFileName;

            Directory.CreateDirectory(UpdateCacheFolder);

            string json;
            using (HttpResponseMessage response =
                await Http.GetAsync(feedUrl))
            {
                json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(
                        "دریافت releases.json ناموفق بود.\nURL: " + feedUrl +
                        "\nHTTP: " + (int)response.StatusCode +
                        " " + response.ReasonPhrase);
                }
            }

            string trimmed = json.TrimStart();
            if (trimmed.Length == 0 || trimmed[0] != '{')
            {
                string preview = json.Length > 300
                    ? json.Substring(0, 300) : json;
                throw new InvalidDataException(
                    "پاسخ سرور JSON نیست (احتمالاً صفحه HTML).\nURL: " +
                    feedUrl + "\nپیش‌نمایش: " + preview);
            }

            JObject feed;
            try { feed = JObject.Parse(json); }
            catch (Exception ex)
            {
                throw new InvalidDataException(
                    "فایل releases.json معتبر نیست: " + ex.Message +
                    "\nURL: " + feedUrl);
            }

            JArray assets =
                GetPropertyIgnoreCase(feed, "Assets") as JArray;
            if (assets == null || assets.Count == 0)
            {
                throw new InvalidDataException(
                    "در فایل releases.json فهرست Assets پیدا نشد.\nURL: " +
                    feedUrl);
            }

            var packages = new List<JToken>();
            var packageNames = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

            foreach (JToken asset in assets)
            {
                string fileName = Convert.ToString(
                    GetPropertyIgnoreCase(asset, "FileName"));

                if (string.IsNullOrWhiteSpace(fileName)) continue;
                if (!fileName.EndsWith(".nupkg",
                    StringComparison.OrdinalIgnoreCase)) continue;

                if (!string.Equals(
                    Path.GetFileName(fileName), fileName,
                    StringComparison.Ordinal))
                    throw new InvalidDataException(
                        "نام فایل بسته معتبر نیست: " + fileName);

                if (packageNames.Add(fileName))
                    packages.Add(asset);
            }

            if (packages.Count == 0)
                throw new InvalidDataException(
                    "هیچ فایل پکیج با پسوند nupkg در releases.json پیدا نشد.");

            foreach (JToken asset in packages)
            {
                string fileName = Convert.ToString(
                    GetPropertyIgnoreCase(asset, "FileName"));

                JToken sizeToken = GetPropertyIgnoreCase(asset, "Size");
                long expectedSize = -1;
                if (sizeToken != null)
                {
                    long parsed;
                    if (long.TryParse(sizeToken.ToString(), out parsed) &&
                        parsed > 0)
                        expectedSize = parsed;
                }

                string localPackage = Path.Combine(
                    UpdateCacheFolder, fileName);

                if (File.Exists(localPackage) &&
                    (expectedSize <= 0 ||
                     new FileInfo(localPackage).Length == expectedSize))
                    continue;

                string packageUrl =
                    baseUrl + "/" + Uri.EscapeDataString(fileName);
                string tmp = localPackage + ".download";
                if (File.Exists(tmp)) File.Delete(tmp);

                using (HttpResponseMessage response =
                    await Http.GetAsync(packageUrl,
                        HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                        throw new HttpRequestException(
                            "دانلود بسته ناموفق بود: " + fileName +
                            "\nURL: " + packageUrl +
                            "\nHTTP: " + (int)response.StatusCode);

                    using (Stream input =
                        await response.Content.ReadAsStreamAsync())
                    using (var output = new FileStream(
                        tmp, FileMode.Create,
                        FileAccess.Write, FileShare.None))
                    {
                        await input.CopyToAsync(output);
                    }
                }

                long actualSize = new FileInfo(tmp).Length;
                if (actualSize <= 0 ||
                    (expectedSize > 0 && actualSize != expectedSize))
                {
                    File.Delete(tmp);
                    throw new InvalidDataException(
                        "اندازه فایل دانلودشده صحیح نیست: " + fileName +
                        " (انتظار: " + expectedSize +
                        "، دریافت: " + actualSize + ")");
                }

                if (File.Exists(localPackage)) File.Delete(localPackage);
                File.Move(tmp, localPackage);
            }

            string winFeed = Path.Combine(
                UpdateCacheFolder, "releases.win.json");
            string plainFeed = Path.Combine(
                UpdateCacheFolder, "releases.json");
            var utf8 = new UTF8Encoding(false);
            File.WriteAllText(winFeed, json, utf8);
            File.WriteAllText(plainFeed, json, utf8);

            return UpdateCacheFolder;
        }

        private static JToken GetPropertyIgnoreCase(
            JToken token, string propertyName)
        {
            if (token is JObject obj)
            {
                foreach (JProperty prop in obj.Properties())
                {
                    if (string.Equals(prop.Name, propertyName,
                        StringComparison.OrdinalIgnoreCase))
                        return prop.Value;
                }
            }
            return null;
        }

        public static string GetCurrentVersionText()
        {
            try
            {
                Version v = typeof(AppUpdater).Assembly
                    .GetName().Version;
                if (v != null)
                    return "Current Version : " + v.ToString(3);
            }
            catch { }

            try
            {
                var mgr = new UpdateManager(UpdateCacheFolder);
                if (mgr.IsInstalled && mgr.CurrentVersion != null)
                    return "Current Version : " + mgr.CurrentVersion;
            }
            catch { }

            return "Current Version : Unspacified";
        }

        private static string FindLocalReleases()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string updatesRoot = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "BHP.Updates");

            string[] paths =
            {
                Path.Combine(basePath, "Releases"),
                Path.Combine(Application.StartupPath, "Releases"),
                Path.Combine(updatesRoot, "Releases"),
                Path.Combine(updatesRoot, "WebFeedCache"),
                updatesRoot
            };

            foreach (string path in paths)
            {
                if (string.IsNullOrEmpty(path)) continue;
                string releaseFile = Path.Combine(
                    path, "releases.win.json");
                if (Directory.Exists(path) && File.Exists(releaseFile))
                    return path;
            }
            return null;
        }

        private static async Task<UpdateResult> CheckUpdate(
            string source, bool manual = false)
        {
            try
            {
                var manager = new UpdateManager(source);

                UpdateInfo update =
                    await manager.CheckForUpdatesAsync();

                if (update == null)
                {
                    // اگر دستی فراخوانی شده بود، فقط بگوییم فایل به‌روز است
                    if (manual)
                    {
                        MessageBox.Show(
                            "This File is Not Containing A New Update.",
                            "Update",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return UpdateResult.NoUpdate;
                    }

                    // اگر برنامه آنلاین به‌روز بود
                    DialogResult manualAnswer = MessageBox.Show(
                        "App Is Up To Date.\n\n" +
                        "Current Version : " +
                        (manager.CurrentVersion?.ToString() ?? "Unspacified") + "\n\n" +
                        "Do You Want To Do Manual Update ?",
                        "System Update",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (manualAnswer == DialogResult.Yes)
                    {
                        await PromptForManualUpdateAsync();
                    }

                    return UpdateResult.NoUpdate;
                }

                string currentVersion =
                    manager.CurrentVersion?.ToString() ?? "Unspacified";
                string newVersion =
                    update.TargetFullRelease.Version.ToString();

                if (!manual)
                {
                    // ★★★ سه دکمه: Yes (خودکار) - No (انصراف) - Cancel (دستی) ★★★
                    DialogResult answer = MessageBox.Show(
                        "New Version Found.\n\n" +
                        "Current Version : " + currentVersion + "\n" +
                        "New Version : " + newVersion + "\n\n" +
                        "For Automated Install Press [Yes] \n" +
                        "If You Regreat Your Choice Press [No] .\n" +
                        "For Manual Install Press [Cancel] .",
                        "New Version",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                    if (answer == DialogResult.Cancel)
                    {
                        // فراخوانی انتخاب فایل دستی
                        await PromptForManualUpdateAsync();
                        return UpdateResult.Cancelled;
                    }
                    if (answer == DialogResult.No)
                    {
                        return UpdateResult.Cancelled;
                    }
                    // اگر Yes بود، از اینجا رد می‌شود و دانلود شروع می‌شود
                }

                // دانلود و نصب
                await manager.DownloadUpdatesAsync(update);

                if (!manager.IsInstalled)
                {
                    MessageBox.Show(
                        "بروزرسانی دانلود شد ولی برنامه با Velopack نصب نشده، " +
                        "بنابراین اعمال خودکار ممکن نیست.\n\n" +
                        "برای اعمال، برنامه را از نسخه Setup نصب‌شده اجرا کنید.",
                        "بروزرسانی",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return UpdateResult.Cancelled;
                }

                manager.ApplyUpdatesAndRestart(update);
                return UpdateResult.Updated;
            }
            catch (Exception ex)
            {
                if (manual)
                {
                    MessageBox.Show(
                        "خطا در بررسی یا نصب بروزرسانی:\n\n" +
                        ex.Message, "خطای بروزرسانی",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return UpdateResult.SourceUnavailable;
                }
                throw;
            }
        }

        private enum UpdateResult
        {
            Updated, NoUpdate, SourceUnavailable, Cancelled
        }
    }
}