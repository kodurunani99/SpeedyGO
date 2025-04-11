namespace TransportQuotation_Service.IRepository
{
    public interface IImageRepository
    {
        string GenerateImageUrl(IFormFile file);
    }
}
