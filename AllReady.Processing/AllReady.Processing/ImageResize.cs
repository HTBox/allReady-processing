using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace AllReady.Processing
{
    public class ImageResize
    {
        [FunctionName("ImageResize")]
        public static void Run(
            [BlobTrigger("images/{name}")] Stream imageBlob,
            string name,
            [Blob("images/{name}", FileAccess.Write)] Stream outputBlob,
            TraceWriter log)
        {
            int maxDimension = Configuration.GetEnvironmentVariableAsInt("maxDimension", 800);
            using (var img = Image.Load<Rgba32>(imageBlob, out IImageFormat imageFormat))
            {
                int bigDimension = Math.Max(img.Width, img.Height);
                if (bigDimension <= maxDimension)
                {
                    log.Info($"Image {name} does not need resizing.");
                    return;
                }

                double ratio = bigDimension == img.Width
                    ? (double) maxDimension / img.Width
                    : (double) maxDimension / img.Height;
                int width = (int) (img.Width * ratio);
                int height = (int) (img.Height * ratio);

                log.Info($"Resizing image {name} to {width}x{height}");
                img.Mutate(ctx => ctx.Resize(width, height));

                img.Save(outputBlob, imageFormat);
            }
        }
    }
}