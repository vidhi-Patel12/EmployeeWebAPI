using EmployeeWebAPI.Models;

namespace EmployeeWebAPI.Repository
{
    internal class FileModel : EmployeeClass
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public DateTime UploadDate { get; set; }
    }
}