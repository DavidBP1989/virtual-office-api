using cobach_api.Infrastructure.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;
using cobach_api.Infrastructure.Extensions;

namespace cobach_api.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _user;
        public FileService(IConfiguration configuration, IUserService user)
        {
            _configuration = configuration;
            _user = user;
        }

        public byte[] GetImageAsByteArray(string fileName, string size)
        {
            #pragma warning disable
            //solo funciona para windows

            var m = new MemoryStream();

            if (!string.IsNullOrEmpty(fileName))
            {
                string userId = _user.GetCurrentUser();

                string filePath = Path.Combine(_configuration["DigitalFileRootPath"]!, "empleados", userId, fileName);

                if (!string.IsNullOrEmpty(size))
                {
                    string thumbnailPath = string.Concat(_configuration["DigitalFileRootPath"], "\\assets\\thumbnails");

                    string thumbnailDirectoryPath = string.Concat(thumbnailPath, "\\", userId);

                    if (!Directory.Exists(thumbnailDirectoryPath))
                        Directory.CreateDirectory(thumbnailDirectoryPath);

                    string thumbnailFilePath = Path.Combine(thumbnailDirectoryPath, $"{Path.GetFileNameWithoutExtension(fileName)}-{size}{Path.GetExtension(fileName)}");

                    if (!File.Exists(thumbnailFilePath))
                    {
                        int mW = int.Parse(size.ToLower().Split('x')[0]);
                        int mH = int.Parse(size.ToLower().Split('x')[1]);

                        ImageCodecInfo imageCodecInfo = GetEncoderInfo("image/jpeg");
                        System.Drawing.Imaging.Encoder encoder;
                        EncoderParameter encoderParameter;
                        EncoderParameters encoderParameters;

                        encoder = System.Drawing.Imaging.Encoder.ColorDepth;
                        encoderParameters = new EncoderParameters(1);
                        encoderParameter = new EncoderParameter(encoder, 24L);
                        encoderParameters.Param[0] = encoderParameter;

                        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                        System.Drawing.Image image = Image.FromStream(stream);
                        if (image.Width > mW || image.Height > mH)
                        {
                            image.GetThumbnailWebQualityImage(mW, mH, ImageExtension.ScaleMode.Scale)
                                .Save(thumbnailFilePath, imageCodecInfo, encoderParameters);
                            image.GetThumbnailWebQualityImage(mW, mH, ImageExtension.ScaleMode.Scale)
                                .Save(m, imageCodecInfo, encoderParameters);
                        }
                        else
                        {
                            image.GetWebQualityImage().Save(thumbnailFilePath, imageCodecInfo, encoderParameters);
                            image.GetWebQualityImage().Save(m, imageCodecInfo, encoderParameters);
                        }
                    }
                    else
                    {
                        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                        Image image = Image.FromStream(stream);
                        image.Save(m, ImageFormat.Jpeg);
                    }
                }
                else
                {
                    using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                    Image image = Image.FromStream(stream);
                    image.Save(m, ImageFormat.Jpeg);
                }
            }

            return m.ToArray();
        }

        private ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            int j;
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType) return encoders[j];
            }
            return null;
        }
    }
}
