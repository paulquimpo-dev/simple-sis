using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleSIS.Data;
using SimpleSIS.Models;

namespace SimpleSIS.Pages.Students;

public class DeleteModel(AppDbContext context) : PageModel
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
        var student = context.Students.Find(Student.Id);

        if (student is null)
        {
            return NotFound();
        }

        context.Students.Remove(student);
        context.SaveChanges();

        return RedirectToPage("Index");
    }
}
