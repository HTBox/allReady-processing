using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Azure.WebJobs.Host;
using Moq;
using Shouldly;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using Xunit;

namespace AllReady.Processing.UnitTest
{
    public class ImageResizeShould
    {
        private const string BlobName = "/container/image.png";
        private readonly TraceWriter _log = new Mock<TraceWriter>(TraceLevel.Verbose).Object;

        public ImageResizeShould()
        {
            Environment.SetEnvironmentVariable("maxDimension", "400");
        }

        [Fact]
        public void Not_resize_an_image_within_maxDimensions_in_both_directions()
        {
            using (var src = ImageHelpers.GetImageStream(400, 400))
            using (var result = new MemoryStream())
            {
                ImageResize.Run(src, BlobName, result, _log);

                result.Seek(0, SeekOrigin.Begin);
                result.ReadByte().ShouldBe(-1);
            }
        }

        [Fact]
        public void Resize_an_image_with_width_larger_than_maxDimensions()
        {
            using (var src = ImageHelpers.GetImageStream(700, 400))
            using (var result = new MemoryStream())
            {
                ImageResize.Run(src, BlobName, result, _log);

                result.Seek(0, SeekOrigin.Begin);
                result.ShouldBeImageWithDimensions(400, 228);
            }
        }

        [Fact]
        public void Resize_an_image_with_height_larger_than_maxDimensions()
        {
            using (var src = ImageHelpers.GetImageStream(250, 500))
            using (var result = new MemoryStream())
            {
                ImageResize.Run(src, BlobName, result, _log);

                result.Seek(0, SeekOrigin.Begin);
                result.ShouldBeImageWithDimensions(200, 400);
            }
        }

        [Fact]
        public void Resize_an_image_with_both_dimensions_larger_than_maxDimensions()
        {
            using (var src = ImageHelpers.GetImageStream(500, 650))
            using (var result = new MemoryStream())
            {
                ImageResize.Run(src, BlobName, result, _log);

                result.Seek(0, SeekOrigin.Begin);
                result.ShouldBeImageWithDimensions(307, 400);
            }
        }

        public static readonly IEnumerable<object[]> TestImageFormats =
            new[]
            {
                new object[] {ImageFormats.Png},
                new object[] {ImageFormats.Gif},
                new object[] {ImageFormats.Bmp},
                new object[] {ImageFormats.Jpeg}
            };

        [Theory]
        [MemberData(nameof(TestImageFormats))]
        public void Maintain_the_same_image_format_when_resizing(IImageFormat expected)
        {
            using (var src = ImageHelpers.GetImageStream(500, 650, expected))
            using (var result = new MemoryStream())
            {
                ImageResize.Run(src, BlobName, result, _log);

                result.Seek(0, SeekOrigin.Begin);
                var format = Image.DetectFormat(result);
                format.ShouldBe(format);
            }
        }
    }
}