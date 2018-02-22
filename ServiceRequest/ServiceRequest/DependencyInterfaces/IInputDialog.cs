using System.Threading.Tasks;

namespace ServiceRequest.DependencyInterfaces
{
    public interface IInputDialog
    {
        Task<string> ShowDialog(string inputText);
    }
}
