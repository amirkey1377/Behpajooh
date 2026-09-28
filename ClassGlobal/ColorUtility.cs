using System;
using System.Drawing;

public static class ColorUtility
{
    private static readonly Random _random = new Random();

    /// <summary>
    /// تولید یک رنگ تصادفی که به خوبی روی زمینه سفید دیده می‌شود
    /// (رنگ تیره، نه سفید و نه کم‌رنگ)
    /// </summary>
    public static int GenerateHighContrastColor()
    {
        // برای اطمینان از تیره بودن رنگ، مقدار روشنایی (Brightness) را زیر ۱۲۰ نگه می‌داریم
        // محدوده روشنایی ۰ تا ۲۵۵ – هرچه عدد کمتر باشد رنگ تیره‌تر است
        const double maxBrightness = 120.0;

        while (true)
        {
            int r = _random.Next(0, 256);
            int g = _random.Next(0, 256);
            int b = _random.Next(0, 256);

            // محاسبه روشنایی درک شده با وزن استاندارد برای چشم انسان
            double brightness = 0.299 * r + 0.587 * g + 0.114 * b;

            if (brightness < maxBrightness)
            {
                // بررسی می‌کنیم که رنگ خیلی خاکستری و کدر هم نباشد (اختیاری)
                // اگر اشباع خیلی کم باشد (رنگ نزدیک به خاکستری تیره) ممکن است کنتراست مطلوب نباشد
                // می‌توان مقدار اشباع را هم محدود کرد، ولی برای سادگی این شرط کافی است
                Color color = Color.FromArgb(r, g, b);
                return color.ToArgb(); // خروجی int
            }
            // در غیر این صورت دوباره امتحان می‌کنیم تا یک رنگ تیره به دست آید
        }
    }
}