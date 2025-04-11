using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using TransportQuotation_Service.IRepository;

namespace TransportQuotation_Service.Repository
{
    // Repository class responsible for handling image-related actions, 
    // such as uploading images to Cloudinary
    public class ImageRepository : IImageRepository
    {
        // Private readonly field to hold Cloudinary client instance
        private readonly Cloudinary _cloudinary;

        // Constructor to initialize the Cloudinary client using environment variables
        public ImageRepository()
        {
            // Load environment variables from the .env file (if present)
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));

            // Initialize Cloudinary using the CLOUDINARY_URL from environment variables
            _cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));

            // Ensure the API uses secure (HTTPS) links when accessing Cloudinary
            _cloudinary.Api.Secure = true;
        }

        // Method to upload an image to Cloudinary and return the URL of the uploaded image
        public string GenerateImageUrl(IFormFile file)
        {
            // Setting up the upload parameters for the image file
            var uploadParams = new ImageUploadParams()
            {
                // Providing the file and its stream to be uploaded
                File = new FileDescription(file.FileName, file.OpenReadStream()),

                // Specifying the Cloudinary folder where the image should be uploaded
                Folder = "SpeedyGo-Images",

                // Ensuring the uploaded file uses its original filename
                UseFilename = true,

                // Setting to false so the filename will not be changed
                UniqueFilename = false,

                // Overwriting any existing file with the same name in the folder
                Overwrite = true
            };

            // Upload the image to Cloudinary and get the result
            var uploadResult = _cloudinary.Upload(uploadParams);

            // Log the JSON response from Cloudinary for debugging purposes
            Console.WriteLine(uploadResult.JsonObj);

            // Return the URL of the uploaded image
            return uploadResult.Url.ToString();
        }
    }
}
