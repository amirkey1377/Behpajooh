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
            message += "\n\nآیا می‌خواهید پوشه بروزرسانی را دستی انتخاب کنید؟";

            DialogResult answer = MessageBox.Show(
                message, "بروزرسانی",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (answer != DialogResult.Yes) return;

            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description =
                    "پوشه‌ای را انتخاب کنید که فایل releases.win.json و بسته‌های آپدیت داخل آن قرار دارند.";

                if (dialog.ShowDialog() != DialogResult.OK)
                    return;

                string releaseFile = Path.Combine(
                    dialog.SelectedPath, "releases.win.json");

                if (!File.Exists(releaseFile))
                {
                    MessageBox.Show(
                        "فایل releases.win.json در پوشه انتخاب‌شده پیدا نشد.",
                        "خطای بروزرسانی",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                await CheckUpdate(dialog.SelectedPath, true);
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

            // بررسی این‌که پاسخ واقعاً JSON است نه صفحه HTML خطا
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

            // نوشتن هر دو نام، برای سازگاری با هر نسخه‌ای از Velopack
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
                    return "نسخه فعلی: " + v.ToString(3);
            }
            catch { }

            try
            {
                var mgr = new UpdateManager(UpdateCacheFolder);
                if (mgr.IsInstalled && mgr.CurrentVersion != null)
                    return "نسخه فعلی: " + mgr.CurrentVersion;
            }
            catch { }

            return "نسخه فعلی: نامشخص";
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

                // IsInstalled را دیگر قبل از CheckForUpdatesAsync چک نمی‌کنیم
                UpdateInfo update =
                    await manager.CheckForUpdatesAsync();

                if (update == null)
                {
                    MessageBox.Show(
                        "برنامه شما به‌روز است.\n\n" +
                        "نسخه فعلی: " +
                        (manager.CurrentVersion?.ToString() ?? "نامشخص"),
                        "بروزرسانی",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return UpdateResult.NoUpdate;
                }

                string currentVersion =
                    manager.CurrentVersion?.ToString() ?? "نامشخص";
                string newVersion =
                    update.TargetFullRelease.Version.ToString();

                DialogResult answer = MessageBox.Show(
                    "نسخه جدید پیدا شد.\n\n" +
                    "نسخه فعلی: " + currentVersion + "\n" +
                    "نسخه جدید: " + newVersion + "\n\n" +
                    "آیا می‌خواهید بروزرسانی انجام شود؟",
                    "نسخه جدید",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (answer != DialogResult.Yes)
                    return UpdateResult.Cancelled;

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
                throw; // در حالت عادی، خطا را به بالا منتقل کن
            }
        }

        private enum UpdateResult
        {
            Updated, NoUpdate, SourceUnavailable, Cancelled
        }
    }
}