using PaymentAdmin.ServicesIntegration.Kivra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class SendDocumentToCustomer
    {
        public async void SendDocument(string data, string invoiceId)
        {
            var kivraCommunicator = new KivraCommunicator("https://sender.sandbox-api.kivra.com");
            await kivraCommunicator.SendContent(data, invoiceId).ConfigureAwait(false);
        }
    }
}
