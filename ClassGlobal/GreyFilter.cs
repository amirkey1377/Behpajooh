using System.Drawing;
using System.Drawing.Imaging;

public static class ImageExtensions
{
    // تعریف ماتریس تبدیل به خاکستری با وزن‌های استاندارد
    static ColorMatrix grayMatrix = new ColorMatrix(new float[][]
    {
        new float[] { 0.2126f, 0.2126f, 0.2126f, 0, 0 },
        new float[] { 0.7152f, 0.7152f, 0.7152f, 0, 0 },
        new float[] { 0.0722f, 0.0722f, 0.0722f, 0, 0 },
        new float[] { 0, 0, 0, 1, 0 },
        new float[] { 0, 0, 0, 0, 1 }
    });

    // متد کمکی برای اعمال فیلتر روی یک تصویر
    public static Bitmap ApplyGrayscaleFilter(this Image originalImage)
    {
        // ایجاد یک بیت‌مپ جدید برای خروجی
        var grayscaleImage = new Bitmap(originalImage.Width, originalImage.Height, originalImage.PixelFormat);
        grayscaleImage.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);

        // رسم تصویر اصلی روی تصویر جدید با استفاده از ماتریس رنگی
        using (var graphics = Graphics.FromImage(grayscaleImage))
        using (var attributes = new ImageAttributes())
        {
            attributes.SetColorMatrix(grayMatrix);
            graphics.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                               0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, attributes);
            return grayscaleImage;
        }
    }
}