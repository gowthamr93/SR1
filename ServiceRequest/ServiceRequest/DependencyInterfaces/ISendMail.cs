using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceRequest.DependencyInterfaces
{
    public interface ISendMail
    {
        Task<bool> Send(string message, string Subject);
    }
}
