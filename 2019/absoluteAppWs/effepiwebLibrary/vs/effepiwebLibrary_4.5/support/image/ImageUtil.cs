using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;

namespace Support.Library
{
    public class ImageUtil
    {
        public ImageUtil() { }
       
        /// <summary>
        /// Metodo che restituisce le dimensioni dell'immagine
        /// </summary>
        /// <param name="_sPathImage">path del file</param>
        /// <returns>restituisce un array di 2 con [width][heigth]</returns>
        public String[] GetImageSize(String _sPathImage)
        {
            String[] sReturn = new String[2];

            sReturn[0] = "";
            sReturn[1] = "";

            try
            {
                System.Drawing.Image objImage = System.Drawing.Image.FromFile(_sPathImage);

                sReturn[0] = objImage.Width.ToString();
                sReturn[1] = objImage.Height.ToString();
            }
            catch (Exception ex)
            {

            }

            return sReturn;
        }

        /// <summary>
        /// Metodo che restituisce un Bolean in caso di estensione valida o meno
        /// </summary>
        /// <param name="_extentionFile">estensione del file</param>
        /// <returns>restituisce il boleano</returns>
        public Boolean CheckImageExtention(String _extentionFile)
        {
            Boolean bResult = false;

            if (_extentionFile.ToLower() == ".jpg")
                bResult = true;
            if (_extentionFile.ToLower() == ".jpeg")
                bResult = true;
            else if (_extentionFile.ToLower() == ".gif")
                bResult = true;
            else if (_extentionFile.ToLower() == ".bmp")
                bResult = true;
            else if (_extentionFile.ToLower() == ".png")
                bResult = true;
            else if (_extentionFile.ToLower() == ".tiff")
                bResult = true;

            return bResult;
        }        

        /// <summary>
        /// Metodo che restituisce un Bolean in caso di resize effetuato correttamente
        /// </summary>
        /// <param name="_imageSRC">path immagine sorgente</param>
        /// <param name="_pathDEST">path immagine di destinazione</param>
        /// <param name="_imageWidth">dimensione larghezza</param>
        /// <param name="_imageHeight">dimensione altezza</param>
        /// <returns>restituisce il boleano</returns>

        public Boolean ResizeImage(String _imageSRC, String _pathDEST, Int32 _imageWidth, Int32 _imageHeight)
        {
            return ResizeImage(_imageSRC, _pathDEST, _imageWidth, _imageHeight, false, false);
        }
        /// <summary>
        /// Metodo che restituisce un Bolean in caso di resize effetuato correttamente
        /// </summary>
        /// <param name="_imageSRC">path immagine sorgente</param>
        /// <param name="_pathDEST">path immagine di destinazione</param>
        /// <param name="_imageWidth">dimensione larghezza</param>
        /// <param name="_imageHeight">dimensione altezza</param>
        /// <param name="_enlarge">forza dimensioni se più grande</param>
        /// <param name="_strech">foza lo strech della foto</param>
        /// <returns>restituisce il boleano</returns>
        public Boolean ResizeImage(String _imageSRC, String _pathDEST, Int32 _imageWidth, Int32 _imageHeight, Boolean _enlarge, Boolean _strech)
        {
            try
            {
                int thumbWidth = 0;
                int thumbHeight = 0;

                System.Drawing.Image image = System.Drawing.Image.FromFile(_imageSRC);
                int srcWidth = image.Width;
                int srcHeight = image.Height;
                
                // Controllo i parametri passati e imposto le eventuali prporzioni
                if ((_imageWidth > 0) && (_imageHeight > 0))
                {
                    if (_strech)
                    {
                        // Se abilito lo strech forzo le misure come da parametri
                        thumbWidth = _imageWidth;
                        thumbHeight = _imageHeight;
                    }
                    else
                    {
                        // Entrambi i parametri passati, calcolo prima la larghezza
                        thumbWidth = _imageWidth;

                        thumbWidth = _imageWidth;
                        thumbHeight = (srcHeight * _imageWidth) / srcWidth;

                        if (thumbHeight > _imageHeight)
                        {
                            // Se l'altezza è ancora superiore ridimensione per l'altezza passata
                            thumbHeight = _imageHeight;
                            thumbWidth = (srcWidth * _imageHeight) / srcHeight;
                        }
                    }
                }
                else if ((_imageWidth > 0) && (_imageHeight.Equals(0)))
                {
                    // Passata solo la larghezza calcolo l'altezza in proporzione
                    thumbWidth = _imageWidth;
                    thumbHeight = (srcHeight * _imageWidth) / srcWidth;
                }
                else if ((_imageWidth.Equals(0)) && (_imageHeight > 0))
                {
                    // Passata solo l'altezza calcolo la larghezza in proporzione
                    thumbHeight = _imageHeight;
                    thumbWidth = (srcWidth * _imageHeight) / srcHeight;
                }


                // Controllo se allargare 
                if ((!_enlarge) && ((_imageWidth > srcWidth) || (_imageHeight > srcHeight)))
                {
                    if (_strech)
                    {
                        thumbWidth = srcWidth;
                        thumbHeight = srcHeight;
                    }
                }

                Bitmap bmp = new Bitmap(thumbWidth, thumbHeight);

                System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumbWidth, thumbHeight);
                gr.DrawImage(image, rectDestination, 0, 0, srcWidth, srcHeight, GraphicsUnit.Pixel);


                if (System.IO.Path.GetExtension(_imageSRC).ToLower() == ".jpg")
                    bmp.Save(_pathDEST, System.Drawing.Imaging.ImageFormat.Jpeg);
                else if (System.IO.Path.GetExtension(_imageSRC).ToLower() == ".jpeg")
                    bmp.Save(_pathDEST, System.Drawing.Imaging.ImageFormat.Jpeg);
                else if (System.IO.Path.GetExtension(_imageSRC).ToLower() == ".gif")
                    bmp.Save(_pathDEST, System.Drawing.Imaging.ImageFormat.Gif);
                else if (System.IO.Path.GetExtension(_imageSRC).ToLower() == ".bmp")
                    bmp.Save(_pathDEST, System.Drawing.Imaging.ImageFormat.Bmp);
                else if (System.IO.Path.GetExtension(_imageSRC).ToLower() == ".png")
                    bmp.Save(_pathDEST, System.Drawing.Imaging.ImageFormat.Png);
                else if (System.IO.Path.GetExtension(_imageSRC).ToLower() == ".tiff")
                    bmp.Save(_pathDEST, System.Drawing.Imaging.ImageFormat.Tiff);

                bmp.Dispose();
                image.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Metodo che restituisce un Bolean in caso di contentype di tipo immagine
        /// </summary>
        /// <param name="_fileContentType">contenttype</param>
        /// <returns>restituisce il boleano</returns>
        public Boolean FileContentTypeImage(String _fileContentType)
        {
            if (_fileContentType.Equals("image/jpeg") || _fileContentType.Equals("image/bmp") || _fileContentType.Equals("image/gif") || _fileContentType.Equals("image/png") || _fileContentType.Equals("image/tiff"))
                return true;
            return false;
        }

        /// <summary>
        /// Metodo che restituisce un Image con l'area selezionata per il crop
        /// </summary>
        /// <param name="img">Image dell'immagine sorgente</param>
        /// <param name="cropArea">Rectangle con area di crop</param>
        /// <returns>restituisce Image</returns>
        private static System.Drawing.Image cropImage(System.Drawing.Image img, Rectangle cropArea)
        {
            System.Drawing.Bitmap bmpImage = new System.Drawing.Bitmap(img);
            System.Drawing.Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return (System.Drawing.Image)(bmpCrop);
        }

        public void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[0x1000];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                output.Write(buffer, 0, read);
        }
        public IBinaryObject ResizeImage(IBinaryObject _imageSRC, Int32 _imageWidth, Int32 _imageHeight)
        {
            return ResizeImage(_imageSRC, _imageWidth, _imageHeight, false, false);
        }
        public IBinaryObject ResizeImage(IBinaryObject _imageSRC, Int32 _imageWidth, Int32 _imageHeight, Boolean _enlarge, Boolean _strech)
        {
            try
            {
                int thumbWidth = 0;
                int thumbHeight = 0;

                System.Drawing.Image image = System.Drawing.Image.FromStream(_imageSRC.DataStream);
                int srcWidth = image.Width;
                int srcHeight = image.Height;

                // Controllo i parametri passati e imposto le eventuali prporzioni
                if ((_imageWidth > 0) && (_imageHeight > 0))
                {
                    if (_strech)
                    {
                        // Se abilito lo strech forzo le misure come da parametri
                        thumbWidth = _imageWidth;
                        thumbHeight = _imageHeight;
                    }
                    else
                    {
                        // Entrambi i parametri passati, calcolo prima la larghezza
                        thumbWidth = _imageWidth;

                        thumbWidth = _imageWidth;
                        thumbHeight = (srcHeight * _imageWidth) / srcWidth;

                        if (thumbHeight > _imageHeight)
                        {
                            // Se l'altezza è ancora superiore ridimensione per l'altezza passata
                            thumbHeight = _imageHeight;
                            thumbWidth = (srcWidth * _imageHeight) / srcHeight;
                        }
                    }
                }
                else if ((_imageWidth > 0) && (_imageHeight.Equals(0)))
                {
                    // Passata solo la larghezza calcolo l'altezza in proporzione
                    thumbWidth = _imageWidth;
                    thumbHeight = (srcHeight * _imageWidth) / srcWidth;
                }
                else if ((_imageWidth.Equals(0)) && (_imageHeight > 0))
                {
                    // Passata solo l'altezza calcolo la larghezza in proporzione
                    thumbHeight = _imageHeight;
                    thumbWidth = (srcWidth * _imageHeight) / srcHeight;
                }

                // Controllo se allargare 
                if ((!_enlarge) && ((_imageWidth > srcWidth) || (_imageHeight > srcHeight)))
                {
                    if (_strech)
                    {
                        thumbWidth = srcWidth;
                        thumbHeight = srcHeight;
                    }
                }

                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(thumbWidth, thumbHeight);

                System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, thumbWidth, thumbHeight);
                gr.DrawImage(image, rectDestination, 0, 0, srcWidth, srcHeight, System.Drawing.GraphicsUnit.Pixel);

                MemoryStream outStream = new MemoryStream();

                if (_imageSRC.ContentType.ToLower().IndexOf("jpg") != -1)
                    bmp.Save(outStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                else if (_imageSRC.ContentType.ToLower().IndexOf("jpeg") != -1)
                    bmp.Save(outStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                else if (_imageSRC.ContentType.ToLower().IndexOf("gif") != -1)
                    bmp.Save(outStream, System.Drawing.Imaging.ImageFormat.Gif);
                else if (_imageSRC.ContentType.ToLower().IndexOf("png") != -1)
                    bmp.Save(outStream, System.Drawing.Imaging.ImageFormat.Png);

                outStream.Position = 0;
                return new BinaryObject(outStream);

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IBinaryObject Crop(IBinaryObject Image, Int32 Width, Int32 Height, Int32 X, Int32 Y)
        {

            try
            {

                using (System.Drawing.Image OriginalImage = System.Drawing.Image.FromStream(Image.DataStream))
                {

                    using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(Width, Height))
                    {

                        bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);

                        using (System.Drawing.Graphics Graphic = System.Drawing.Graphics.FromImage(bmp))
                        {

                            Graphic.SmoothingMode = SmoothingMode.AntiAlias;

                            Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

                            Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;

                            Graphic.DrawImage(OriginalImage, new System.Drawing.Rectangle(0, 0, Width, Height), X, Y, Width, Height, System.Drawing.GraphicsUnit.Pixel);

                            MemoryStream ms = new MemoryStream();

                            bmp.Save(ms, OriginalImage.RawFormat);

                            ms.Position = 0;

                            BinaryObject myobj = new BinaryObject(ms, ms.Length);

                            myobj.ContentType = Image.ContentType;

                            myobj.cache();

                            return myobj;

                        }

                    }

                }

            }

            catch (Exception Ex)
            {

                throw (Ex);

            }

        }
        
    }
}
