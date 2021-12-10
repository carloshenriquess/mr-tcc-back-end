using Azure.Storage.Blobs;
using System.Text.RegularExpressions;

namespace doe.rapido.api.Services
{
    public class ImageService
    {
        public async Task<string> UploadBase64Image(string base64Image)
        {
            var fileName = Guid.NewGuid().ToString() + ".jpg";
            var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(base64Image, "");
            byte[] imageBytes = Convert.FromBase64String(data);
            var blobClient = new BlobClient(Settings.ConnectionStringStorageAzure, Settings.ConteinerImages, fileName);
            using (var stream = new MemoryStream(imageBytes))
            {
                await blobClient.UploadAsync(stream);
            }
            return blobClient.Uri.AbsoluteUri;
        }
    }

}
