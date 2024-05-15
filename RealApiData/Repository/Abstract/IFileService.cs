namespace RealApiData.Repository.Abstract
{
    public interface IFileService
    {
        public Tuple<int, string> SaveImage(IFormFile imageFile);
        public Task DeleteImage(string imageFileName);
    }
}
