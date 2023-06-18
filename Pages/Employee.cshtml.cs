using DB_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;


namespace DB_Project.Pages
{

    public class EmployeeModel : PageModel
    {
        public DB db;
        public DataTable dt { get; set; }

        public List<Employee> Employees { get; set; }
        public EmployeeModel(DB db)
        {
            this.db = db;
        }
        
        public void OnGet(int id, string action)
        {
            dt = db.ReadTableEmployee();

        }
    }
}