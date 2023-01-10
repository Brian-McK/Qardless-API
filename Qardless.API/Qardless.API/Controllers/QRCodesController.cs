using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using QRCoder;
using System.Drawing;
using System.IO;

namespace Qardless.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QRCodesController : ControllerBase
    {
        public byte[] ImageToByteArray(System.Drawing.Image img)
        {
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
    }
}
