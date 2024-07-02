using cobach_api.Infrastructure.Interfaces;
using QRCoder;

namespace cobach_api.Infrastructure.Services
{
    public class QRService : IQRService
    {
        public byte[] GetQRCode(string url)
        {
            QRCodeGenerator qrGenerator = new();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCore = new(qrCodeData);
            byte[] qrCodeImage = qrCore.GetGraphic(20);
            return qrCodeImage;
        }
    }
}
