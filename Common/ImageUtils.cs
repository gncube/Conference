﻿using DotNetNuke.Services.FileSystem;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Connect.DNN.Modules.Conference.Common
{
    public class ImageUtils
    {
        public static void CreateThumbnails(int fileId)
        {
            CreateThumbnail(fileId, "l", 64, 64);
            CreateThumbnail(fileId, "s", 50, 50);
            CreateThumbnail(fileId, "xs", 32, 32);
        }

        public static void CreateThumbnail(int fileId, string type, int width, int height)
        {
            var file = FileManager.Instance.GetFile(fileId);
            if (file != null)
            {
                var folder = FolderManager.Instance.GetFolder(file.FolderId);
                var extension = "." + file.Extension;
                var sizedPhoto = file.FileName.Replace(extension, "_" + type + extension);
                if (!FileManager.Instance.FileExists(folder, sizedPhoto))
                {
                    using (var content = FileManager.Instance.GetFileContent(file))
                    {
                        var sizedContent = DotNetNuke.Common.Utilities.ImageUtils.CreateImage(content, height, width, extension);

                        FileManager.Instance.AddFile(folder, sizedPhoto, sizedContent);
                    }
                }
            }
        }

        public static MemoryStream CreateImage(Stream stream, float[] crop, string extension)
        {
            var original = new Bitmap(stream);
            PixelFormat format = original.PixelFormat;
            if (format.ToString().Contains("Indexed"))
            {
                format = PixelFormat.Format24bppRgb;
            }

            int newHeight = (int)(crop[3] - crop[1]);
            int newWidth = (int)(crop[2] - crop[0]);
            var newImg = new Bitmap(newWidth, newHeight, format);
            newImg.SetResolution(original.HorizontalResolution, original.VerticalResolution);
            Graphics canvas = Graphics.FromImage(newImg);
            canvas.SmoothingMode = SmoothingMode.None;
            canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
            canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;

            if (extension.ToLowerInvariant() != ".png")
            {
                canvas.Clear(Color.White);
                canvas.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
            }

            canvas.DrawImage(original, -crop[0], -crop[1], original.Width, original.Height);

            //newImg.Save
            var content = new MemoryStream();
            ImageFormat imgFormat = ImageFormat.Bmp;
            if (extension.ToLowerInvariant() == ".png")
            {
                imgFormat = ImageFormat.Png;
            }
            else if (extension.ToLowerInvariant() == ".gif")
            {
                imgFormat = ImageFormat.Gif;
            }
            else if (extension.ToLowerInvariant() == ".jpg")
            {
                imgFormat = ImageFormat.Jpeg;
            }

            newImg.Save(content, imgFormat);

            newImg.Dispose();
            original.Dispose();
            canvas.Dispose();

            return content;
        }
    }
}