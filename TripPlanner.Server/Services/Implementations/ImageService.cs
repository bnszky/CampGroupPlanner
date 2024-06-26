﻿using Microsoft.AspNetCore.StaticFiles;
using System;
using TripPlanner.Server.Services.Abstractions;

namespace TripPlanner.Server.Services.Implementations
{
    public class ImageService : IImageService
    {
        public static readonly string DIRECTORY_PATH = "wwwroot\\images";

        private string DecodeUrl(string url)
        {
            url = "wwwroot" + url;
            return url;
        }

        private string EncodeUrl(string url)
        {
            url = url.Substring(7);
            return url;
        }
        public bool IsJpgOrPng(IFormFile file)
        {
            if (file == null)
            {
                return false;
            }

            // Check the content type
            var contentType = file.ContentType.ToLower();
            if (contentType == "image/jpeg" || contentType == "image/png")
            {
                return true;
            }

            // Alternatively, check the file extension
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
            {
                return true;
            }

            return false;
        }
        public async Task DeleteImage(string imagePath)
        {
            imagePath = DecodeUrl(imagePath);
            if (!File.Exists(imagePath))
            {
                return;
            }

            try
            {
                File.Delete(imagePath);
            }
            catch (IOException ex)
            {
                throw new Exception("An error occurred while trying to delete the image.", ex);
            }
        }

        public async Task<IFormFile> DownloadImage(string imagePath)
        {
            imagePath = DecodeUrl(imagePath);
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("The file does not exist.");
            }

            byte[] imageBytes = await File.ReadAllBytesAsync(imagePath);

            var imageMemoryStream = new MemoryStream(imageBytes);

            var formFile = new FormFile(imageMemoryStream, 0, imageBytes.Length, "name", imagePath)
            {
                Headers = new HeaderDictionary(),
                ContentType = GetMimeType(imagePath)
            };

            return formFile;
        }

        public async Task<string> UploadImage(IFormFile image)
        {
            string uniqueFilename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

            string imagePath = Path.Combine(DIRECTORY_PATH, uniqueFilename);
            while (File.Exists(imagePath))
            {
                uniqueFilename = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                imagePath = Path.Combine(DIRECTORY_PATH, uniqueFilename);
            }

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return EncodeUrl(imagePath);
        }

        public async Task<string> UploadImage(string imageUrl)
        {
            string uniqueFilename = Guid.NewGuid().ToString() + Path.GetExtension(new Uri(imageUrl).LocalPath);
            string imagePath = Path.Combine(DIRECTORY_PATH, uniqueFilename);

            while (File.Exists(imagePath))
            {
                uniqueFilename = Guid.NewGuid().ToString() + Path.GetExtension(new Uri(imageUrl).LocalPath);
                imagePath = Path.Combine(DIRECTORY_PATH, uniqueFilename);
            }

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(imageUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        using (Stream imageStream = await response.Content.ReadAsStreamAsync())
                        {
                            using (FileStream fileStream = new FileStream(imagePath, FileMode.Create))
                            {
                                await imageStream.CopyToAsync(fileStream);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception($"Failed to download image. Status code: {response.StatusCode}");
                    }
                }
            }

            return EncodeUrl(imagePath);
        }
        private string GetMimeType(string filePath)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(filePath, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }
    }
}
