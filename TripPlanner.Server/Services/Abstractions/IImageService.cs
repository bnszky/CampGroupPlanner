namespace TripPlanner.Server.Services.Abstractions
{
    public interface IImageService
    {
        public Task<string> UploadImage(IFormFile image);
        public Task<string> UploadImage(string imageUrl);
        public Task<IFormFile> DownloadImage(string imageUrl);
        public Task DeleteImage(string imageUrl);
        public bool IsJpgOrPng(IFormFile file);
    }
}
