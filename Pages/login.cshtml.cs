using DB_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DB_Project.Pages
{
    public class loginModel : PageModel
    {
        private readonly DB DB;
        [BindProperty]
        public string username { get; set; }
        [BindProperty]
        public string password { get; set; }
        public string msg { get; set; }
        public int userType { get; set; }

        public loginModel(DB db)
        {
            DB = db;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPostLogin() 
        {
            string uu = username + "@" + password;
            if (DB.checkpassword(username, password))
            {
                userType = (int)DB.getusertyper(username);
                if (userType == 1)
                {
                    return RedirectToPage("/Employee");
                }
                else
                {
                    return RedirectToPage("/Index");
                }
            }
            else
            {
                msg = "Incorrect Password " +
                    "Do you want to sign up";
                return RedirectToPage("/login");
            }

        }
        public IActionResult OnPostSignup()
        {
            return RedirectToPage("/sign_up");
        }
            
    }
}
