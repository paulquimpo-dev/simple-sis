using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleSIS.Data;
using SimpleSIS.Models;

namespace SimpleSIS.Pages.Students;

public class EditModel(AppDbContext context) : PageModel
{
    [BindProperty]
    public Student Student { get; set; } = new();

    public IActionResult OnGet(int id)
    {
        var student = context.Students.Find(id);

        if (student is null)
        {
            return NotFound();
        }

        Student = student;
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var studentToUpdate = context.Students.Find(Student.Id);

        if (studentToUpdate is null)
        {
            return NotFound();
        }

        studentToUpdate.StudentNumber = Student.StudentNumber;
        studentToUpdate.FullName = Student.FullName;
        studentToUpdate.GradeLevel = Student.GradeLevel;
        studentToUpdate.Section = Student.Section;
        studentToUpdate.Strand = Student.Strand;

        context.SaveChanges();
        return RedirectToPage("Index");
    }
}
