using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace EmployeeWebAPI.Models
{
    public class EmployeeClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
        public string? BirthDate { get; set; }
        public string? Gender { get; set; }
     
        public string? Email { get; set; }
        public string? Password { get; set; }

        public string? Image {  get; set; }

        // public string FileName { get; set; }
        //public string FilePath { get; set; }
    

    }
}
