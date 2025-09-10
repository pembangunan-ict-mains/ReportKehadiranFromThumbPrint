
namespace Utilities
{
    public interface ImyUtils
    {
        byte[] ExportToCSV<T>(IEnumerable<T> data);
        byte[] ExportToExcel<T>(IEnumerable<T> data);
        byte[] ExportMultipleToExcel(Dictionary<string, object> datasets);
        Task DownloadFile(string fileName, byte[] content, string contentType);
    }
}