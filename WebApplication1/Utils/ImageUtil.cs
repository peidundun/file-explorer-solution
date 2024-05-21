using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace WebApplication1.Utils
{
    public class ImageUtil
    {
        static public Image GenThumbImage(Image image, int destW, int destH)
        {
            var w = image.Width;
            var h = image.Height;
            var ratioW = destW * 1.0 / w;
            var ratioH = destH * 1.0 / h;

            var ratio = System.Math.Min(ratioW, ratioH);
            var realW = image.Width * ratio;
            var realH = image.Height * ratio;

            return image.Clone(z => z.Resize((int)realW, (int)realH));
        }

        static public byte[] GenThumbImageData(byte[] originalImageData, int destW, int destH)
        {
            var thumbImageData = (byte[])null;
            using (var ms = new System.IO.MemoryStream(originalImageData))
            {
                var image = Image.Load(ms);

                var w = image.Width;
                var h = image.Height;
                var ratioW = destW * 1.0 / w;
                var ratioH = destH * 1.0 / h;

                var ratio = System.Math.Min(ratioW, ratioH);
                var realW = image.Width * ratio;
                var realH = image.Height * ratio;

                var image2 = image.Clone(z => z.Resize((int)realW, (int)realH));

                using (var ms2 = new System.IO.MemoryStream())
                {
                    image2.SaveAsPng(ms2);
                    thumbImageData = ms2.ToArray();
                }
            }
            return thumbImageData;
        }
    }
}
