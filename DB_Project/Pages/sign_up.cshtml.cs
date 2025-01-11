using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DB_Project.Models;

namespace DB_Project.Pages
{
    public class sign_upModel : PageModel
    {
        [BindProperty]
        public string username { get; set; }
        [BindProperty]
        public string email { get; set; }
        [BindProperty]
        public string password { get; set; }
        [BindProperty]
        public string tel { get; set; } 
        [BindProperty]
        public int age { get; set; } 
        [BindProperty]
        public string city { get; set; } 

        public DB DB;

        public sign_upModel(DB db)
        {
            this.DB = db;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            DB.signup(username, email, password, tel, age, city);
            return RedirectToPage("/login");
        }
    }
}