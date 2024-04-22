using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;

namespace cobach_api.Infrastructure.Extensions
{
    public static class ImageExtension
    {
        public enum ScaleMode
        {
            Stretch,
            Scale,
            Centered
        }

        public static Image GetThumbnailWebQualityImage(this Image image, int thumnWidth, int thumbHeight, ScaleMode scale)
        {
            #pragma warning disable
            //solo funciona en windows!!!

            Bitmap bmp = null;
            Size newSize = new Size(thumnWidth, thumbHeight);
            double ratio = 0d;
            double myThumbWidth = 0d;
            double myThumbHeight = 0d;

            if (scale == ScaleMode.Scale)
            {
                if ((image.Width / Convert.ToDouble(newSize.Width)) > (image.Height / Convert.ToDouble(newSize.Height)))
                {
                    ratio = Convert.ToDouble(image.Width) / Convert.ToDouble(newSize.Width);
                }
                else ratio = Convert.ToDouble(image.Height) / Convert.ToDouble(newSize.Height);

                myThumbHeight = Math.Ceiling(image.Height / ratio);
                myThumbWidth = Math.Ceiling(image.Width / ratio);
            }
            else
            {
                myThumbHeight = (thumbHeight > image.Height) ? image.Height : thumbHeight;
                myThumbWidth = (thumnWidth > image.Width) ? image.Width : thumnWidth;
            }

            Size thumbSize = new Size((int)myThumbWidth, (int)myThumbHeight);
            bmp = new Bitmap(thumbSize.Width, thumbSize.Height);

            Graphics g = Graphics.FromImage(bmp);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle rectangle = new Rectangle(0, 0, thumbSize.Width, thumbSize.Height);

            float x = 0, y = 0, xw = (float)image.Width, yh = (float)image.Height;
            if (scale == ScaleMode.Centered)
            {
                float rw = image.Width / thumbSize.Width;
                float rh = image.Height / thumbSize.Height;

                if (rw < rh)
                {
                    xw = image.Width;
                    yh = thumbSize.Height * rw;
                    x = 0;
                    y = (image.Height - yh) / 2;
                }
                else
                {
                    xw = thumbSize.Width * rh;
                    yh = image.Height;
                    y = 0;
                    x = (image.Width - xw) / 2;
                }
            }

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetWrapMode(WrapMode.Clamp, Color.White);

            g.DrawImage(image, rectangle, x, y, xw, yh, GraphicsUnit.Pixel, attributes);

            return bmp;
        }

        public static Image GetWebQualityImage(this Image image)
        {
            #pragma warning disable
            //solo funciona en windows!!!

            Bitmap bitmap = new Bitmap(image.Width, image.Height);

            Graphics g = Graphics.FromImage(bitmap);
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetWrapMode(WrapMode.Clamp, Color.White);

            g.DrawImage(image, rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            return bitmap;
        }
    }
}
