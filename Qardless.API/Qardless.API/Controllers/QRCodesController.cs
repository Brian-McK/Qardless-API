using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using QRCoder;
using System.Drawing;

namespace Qardless.API.Controllers
{
    [Route("api/qrcode")]
    [ApiController]
    public class QRCodesController : ControllerBase
    {
        public byte[] ImageToByteArray(System.Drawing.Bitmap img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }

        [HttpGet("GenerateQRCode")]
        public async Task<ActionResult> GenerateQRCode(string QRCodeText)
        {
            QRCodeGenerator _qrCode = new QRCodeGenerator();
            QRCodeData qRCodeData = _qrCode.CreateQrCode(QRCodeText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qRCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            //Set colors
            //Bitmap qrCodeImage = qrCode.GetGraphic(20, "#000ff0", "#0ff000");

            var bytes = ImageToByteArray(qrCodeImage);
            return File(bytes, "image/bmp");
        }
    }
}
