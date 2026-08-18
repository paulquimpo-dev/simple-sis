using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleSIS.Data;
using SimpleSIS.Models;

namespace SimpleSIS.Pages.Students;

public class IndexModel(AppDbContext context) : PageModel
{
    public List<Student> Students { get; private set; } = [];

    public void OnGet()
    {
        Students = context.Students.ToList();
    }
}
