using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DB_Project.Models;
using System.Data;

namespace DB_Project.Pages
{
    public class Private_TripModel : PageModel
    {
        public DB db;
        public DataTable dt { get; set; }

        public Private_TripModel(DB db) 
        {
            this.db = db;
        }
        public void OnGet()
        {
            dt = db.RecentTrips();
        }
        public IActionResult OnPost()
        {
            return RedirectToPage("/Activities");
        }
    }
}
