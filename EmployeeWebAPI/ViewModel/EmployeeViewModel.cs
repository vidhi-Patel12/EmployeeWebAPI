using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeWebAPI.ViewModel
{
    public class EmployeeViewModel
    {
        //public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }

        [DataType(DataType.Date)]
        public string? BirthDate { get; set; }
        public string? Gender { get; set; }

        public string? Email { get; set; }
        public string? Password { get; set; }
        
        public IFormFile Image { get; set; }

        //public string FileName { get; set; }
        //public string FilePath { get; set; }

       
    }
}
