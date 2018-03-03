using System.IO;
using Shouldly;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace AllReady.Processing.UnitTest
{
    public static class ImageHelpers
    {
        public static Stream GetImageStream(int width, int height, IImageFormat format = null)
        {
            var stream = new MemoryStream();
            using (var img = new Image<Rgba32>(width, height))
            {
                img.Save(stream, format ?? ImageFormats.Png);
            }

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static void ShouldBeImageWithDimensions(this Stream stream, int width, int height)
        {
            using (var img = Image.Load<Rgba32>(stream))
            {
                img.Width.ShouldBe(width);
                img.Height.ShouldBe(height);
            }
        }
    }
}