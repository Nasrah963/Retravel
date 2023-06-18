using DB_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data;

namespace DB_Project.Pages.Shared
{
    public class Edit_EmployeeModel : PageModel
    {
        private readonly DB db;
        public DataTable dt { get; set; }

        [BindProperty]
        public int Employee_ID { get; set; }

        [BindProperty]
        public string Role { get; set; }

        [BindProperty]
        public string Trip { get; set; }

        [BindProperty]
        public DateTime Start_Date { get; set; }

        [BindProperty]
        public DateTime End_Date { get; set; }

        [BindProperty]
        public int Num_of_Children { get; set; }

        [BindProperty]
        public int Num_of_Adults { get; set; }

        public Edit_EmployeeModel(DB db)
        {
            this.db = db;
        }


        public void OnGet(int id)
        {
            // Retrieve the employee data based on the given ID
            Employee employee = db.GetEmployeeById(id);
            Employee_ID = id;
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/Employee");
        }
    }
}
