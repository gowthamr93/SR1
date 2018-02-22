using System.Threading.Tasks;

namespace ServiceRequest.DependencyInterfaces
{
    public interface IFileViewer
    {
        Task OpenFile(string FilePath);
    }
}
