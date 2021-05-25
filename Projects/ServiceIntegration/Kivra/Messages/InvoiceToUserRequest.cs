using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAdmin.ServicesIntegration.Kivra.Messages
{
   public class ContentRequest
    {
        public string ssn { get; set; }
        public string subject { get; set; }
        public DateTime generated_at { get; set; }
        public string type { get; set; }
        public bool retain { get; set; } = false;
        public FileRequest[] files { get; set; }
    }

    public class TokenRequest
    {
        public string grant_type { get; set; }
    }

    public class AuthenticationResponse
    {
        public string state { get; set; }
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
    }

    public class FileRequest
   {
       public string name { get; set; }
       public string data { get; set; }
       public string content_type { get; set; } = "application/pdf";
   }

   public class ContextRequest
   {
       public InvoiceRequest invoice { get; set; }
   }

   public class InvoiceRequest
   {
       public string payment { get; set; }//To be changed to payment type
       public string invoice_reference { get; set; }//Tenant's own invoice info, invoice number for us
   }
}
