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
            if (DB.checkpassword(username, password))
            {
                DB.LogIn();
                userType = DB.getusertyper(username); 

                if (userType == 1) 
                {
                    DB.EditUserTypeAOU(1);
                    return RedirectToPage("/Employee");
                }
                else if (userType == 2) 
                {
                    DB.EditUserTypeAOU(2);
                    int userId = DB.GetUserIdByUsername(username);

                    return RedirectToPage("/Profile", new { id = userId });
                }
            }
            else
            {
                msg = "Incorrect Password. Do you want to sign up?";
                return RedirectToPage("/login");
            }

            return Page();
        }

        public IActionResult OnPostSignup()
        {
            return RedirectToPage("/sign_up");
        }
    }
}
