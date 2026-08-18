using System.ComponentModel.DataAnnotations;

namespace SimpleSIS.Models;

public class Student
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Student Number is required.")]
    [Display(Name = "Student Number")]
    public string StudentNumber { get; set; } = "";

    [Required(ErrorMessage = "Full Name is required.")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = "";

    [Range(7, 12, ErrorMessage = "Grade Level must be from 7 to 12.")]
    [Display(Name = "Grade Level")]
    public int GradeLevel { get; set; }

    [Required(ErrorMessage = "Section is required.")]
    public string Section { get; set; } = "";

    [Required(ErrorMessage = "Strand is required.")]
    public string Strand { get; set; } = "";
}
