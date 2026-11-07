using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Skydat
{
    public partial class GrayToolStripButton : ToolStripButton
    {
        private Image _originalImage;
        private Image _grayImage;

        public GrayToolStripButton()
        {
            InitializeComponent();
            CheckOnClick = true;
        }

        public new Image Image
        {
            get => _originalImage;
            set
            {
                _originalImage = value;

                if (_grayImage != null)
                    _grayImage.Dispose();

                _grayImage = value == null ? null : CreateGrayImage(value);

                UpdateImage();
            }
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            UpdateImage();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            UpdateImage();
        }

        private void UpdateImage()
        {
            base.Image = (Checked && Enabled)
                ? _originalImage
                : _grayImage;
        }

        private Bitmap CreateGrayImage(Image image)
        {
            Bitmap bmp = new Bitmap(image.Width, image.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                ColorMatrix matrix = new ColorMatrix(new float[][]
                {
                new float[] {0.299f,0.299f,0.299f,0,0},
                new float[] {0.587f,0.587f,0.587f,0,0},
                new float[] {0.114f,0.114f,0.114f,0,0},
                new float[] {0,0,0,0.45f,0},   // شفافیت 45٪
                new float[] {0,0,0,0,1}
                });

                using (ImageAttributes ia = new ImageAttributes())
                {
                    ia.SetColorMatrix(matrix);

                    g.DrawImage(
                        image,
                        new Rectangle(0, 0, bmp.Width, bmp.Height),
                        0,
                        0,
                        image.Width,
                        image.Height,
                        GraphicsUnit.Pixel,
                        ia);
                }
            }

            return bmp;
        }
    }
}
