using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Support.Library
{
    public class ImageTools
    {
        public System.Drawing.Image loadImage(Uri uri)
        {
            IBinaryObject ret = BinaryManager.ReadFromUri(uri, true);
            return System.Drawing.Image.FromStream(ret.DataStream);
        }

        
        public MemoryStream encodeImageAsJPEG(System.Drawing.Image img, long quality = 100L)
        {
            ImageCodecInfo jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            //
            MemoryStream stream = new MemoryStream();
            img.Save(stream, jgpEncoder, myEncoderParameters);
            //stream.Close();
            stream.Position = 0;
            return stream;
        }

        public MemoryStream encodeImageAsPNG(System.Drawing.Image img)
        {
            MemoryStream stream = new MemoryStream();
            img.Save(stream, ImageFormat.Png);
            //stream.Close();
            stream.Position = 0;
            return stream;
        }

        // ***********************************************************
        // con crop
        // ***********************************************************
        public System.Drawing.Image cropImage(Uri uri, int w, int h)
        {
            return cropImage(loadImage(uri),w,h);
        }

        public System.Drawing.Image cropImage(System.Drawing.Image img, int w, int h)
        {
            if (w == -1 && h == -1)
            {
                w = img.Width;
                h = img.Height;
            }
            else if (h == -1)
            {
                double r1 = ((double)img.Width) / ((double)img.Height);
                h = (int)((double)w / r1);
            }
            else if (w == -1)
            {
                double r1 = ((double)img.Width) / ((double)img.Height);
                w = (int)((double)h * r1);
            }
            //
            // faccio il resize se ilcaso
            if (img.Width != w || img.Height != h)
            {
                double r = Math.Max((double)w / (double)img.Width, (double)h / (double)img.Height);
                int W = (int)(img.Width * r);
                int H = (int)(img.Height * r);
                //
                int x = (w - W) / 2;
                int y = (h - H) / 2;
                System.Drawing.Bitmap dest = new System.Drawing.Bitmap(w, h);
                dest.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dest);
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(img, x, y, W, H);
                //
                return dest;
            }
            //
            return img;
        }

        // ***********************************************************
        // con resize
        // ***********************************************************
        public System.Drawing.Image resizeImage(Uri uri, int w, int h, uint bkColor = 0xFFFFFF)
        {
            return resizeImage(loadImage(uri), w, h, bkColor);
        }
        public System.Drawing.Image resizeImage(System.Drawing.Image img, int w, int h, uint bkColor = 0xFFFFFF)
        {
            if (w == -1 && h == -1)
            {
                w = img.Width;
                h = img.Height;
            }
            else if (h == -1)
            {
                double r1 = ((double)img.Width) / ((double)img.Height);
                h = (int)((double)w / r1);
            }
            else if (w == -1)
            {
                double r1 = ((double)img.Width) / ((double)img.Height);
                w = (int)((double)h * r1);
            }
            // faccio il resize se ilcaso
            if (img.Width != w || img.Height != h)
            {
                double r = Math.Min((double)w / (double)img.Width, (double)h / (double)img.Height);
                int W = (int)(img.Width * r);
                int H = (int)(img.Height * r);
                //
                int x = (w - W) / 2;
                int y = (h - H) / 2;
                System.Drawing.Bitmap dest = new System.Drawing.Bitmap(w, h);
                dest.SetResolution(img.HorizontalResolution, img.VerticalResolution);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dest);
                //
                SolidBrush bk = new System.Drawing.SolidBrush(Color.FromArgb((int)(bkColor >> 16) & 0xFF, (int)(bkColor >> 8) & 0xFF, (int)(bkColor) & 0xFF));
                //
                g.FillRectangle(bk /*Brushes.White*/, 0, 0, w, h);
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawImage(img, x, y, W, H);
                //
                return dest;
            }
            //
            return img;
        }

        // *********************************************************
        // PRIVATE
        // *********************************************************
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

    }
}
