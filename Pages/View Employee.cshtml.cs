using DB_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data;

namespace DB_Project.Pages
{
    public class View_EmployeeModel : PageModel
    {
        private readonly DB db;

        public DataTable EmployeeTable { get; set; }

        public View_EmployeeModel(DB db)
        {
            this.db = db;
        }
        public int Employee_ID { get; set; }
        public string Role { get; set; }
        public string Trip { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_Date { get; set; }
        public int Num_of_Children { get; set; }
        public int Num_of_Adults { get; set; }

        public void OnGet(int id)
        {

            EmployeeTable = db.ReadTableEmployee();
            Employee_ID = id;
        }
    }
}
