using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAdmin.ServicesIntegration.Kivra
{
    public interface IKivraCommunicator
    {
        Task<bool> SendContent(string data, string invoiceId);

    }
}
