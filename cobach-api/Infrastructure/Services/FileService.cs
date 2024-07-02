using cobach_api.Infrastructure.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;
using cobach_api.Infrastructure.Extensions;
using iTextSharp.text.pdf;
using cobach_api.Features.Common.Enums;

namespace cobach_api.Infrastructure.Services
{
    public class FileService : IFileService
    {
        readonly IConfiguration _configuration;
        readonly IQRService _qrService;
        public FileService(IConfiguration configuration, IQRService qrService)
        {
            _configuration = configuration;
            _qrService = qrService;
        }

        public byte[] GetImageAsByteArray(string userId, string fileName, string size)
        {
            #pragma warning disable
            //solo funciona para windows

            var m = new MemoryStream();

            if (!string.IsNullOrEmpty(fileName))
            {
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

        public byte[] GetPDFPermission(TipoPermisosLaborales tipoPermiso, Dictionary<string, string> workPermit)
        {
            string fileName = tipoPermiso switch
            { 
                TipoPermisosLaborales.CorteTiempo => "CorteDeTiempo.pdf",
                TipoPermisosLaborales.PermisoEconomico => "PermisoEconomico.pdf",
                _ => ":v"
            };
            string pdfPath = Path.Combine(_configuration["PdfPath"], fileName);
            if (!File.Exists(pdfPath))
                return new byte[0];

            var output = new MemoryStream();
            var reader = new PdfReader(pdfPath);
            var stamper = new PdfStamper(reader, output);
            var formFields = stamper.AcroFields;

            foreach (var field in workPermit.Keys)
            {
                switch (field.ToString())
                {
                    case "imagenQR":
                        PushbuttonField btnQR = formFields.GetNewPushbuttonFromField("imagenQR");
                        btnQR.Image = iTextSharp.text.Image.GetInstance(_qrService.GetQRCode("https://www.cobachbcs.edu.mx/"));
                        formFields.ReplacePushbuttonField("imagenQR", btnQR.Field);
                        break;
                    default:
                        formFields.SetField(field, workPermit[field]);
                        break;
                }
            }
            stamper.FormFlattening = true;
            stamper.Close();
            reader.Close();

            return output.ToArray();
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

        Dictionary<string, string> GetFormFieldNames(string pdfPath)
        {
            var fields = new Dictionary<string, string>();
            var reader = new PdfReader(pdfPath);
            foreach (KeyValuePair<string, AcroFields.Item> kvp in reader.AcroFields.Fields) fields.Add(kvp.Key, "");
            reader.Close();

            return fields;
        }
    }
}
