using BusinessLogic;
using System.Threading.Tasks;
using System.Web.Http;

namespace KivraIntegration.Controllers
{
    //[RoutePrefix("documents")]
    public class DocumentController : ApiController
    {
        [HttpPost]
        public async Task<IHttpActionResult> SendDocument(SendDocumentRequest request)
        {
            var sendDocumentToCustomer = new SendDocumentToCustomer();
            sendDocumentToCustomer.SendDocument(request.Data, request.InvoiceId);
            return Ok();
        }
    }

    public class SendDocumentRequest
    {
        public string Data { get; set; }
        public string InvoiceId { get; set; }
    }
}
