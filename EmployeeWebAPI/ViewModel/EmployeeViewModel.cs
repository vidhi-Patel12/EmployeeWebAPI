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
        [Required, MinLength(1), DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress), EmailAddress, MaxLength(50), Display(Name = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        public string? Email { get; set; }
        public string? Password { get; set; }
        
        public IFormFile? Image { get; set; }

       

        //public string FileName { get; set; }
        //public string FilePath { get; set; }


    }
}
