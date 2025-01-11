using DB_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DB_Project.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly DB _db;

        public ProfileModel(DB db)
        {
            _db = db;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; } // User ID

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public int Age { get; set; }

        public void OnGet(int id)
        {
            Id = id;
            // Fetch user data by ID
            var userRow = _db.GetUserById(Id);
            if (userRow != null)
            {
                Name = userRow["name"].ToString();
                Email = userRow["email"].ToString();
                Phone = userRow["tel"]?.ToString() ?? "N/A";
                City = userRow["city"]?.ToString() ?? "N/A";
                Age = userRow["age"] != DBNull.Value ? Convert.ToInt32(userRow["age"]) : 0;
            }
        }
    }
}
