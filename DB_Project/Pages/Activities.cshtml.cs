using DB_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DB_Project.Pages
{
    public class ActivitiesModel : PageModel
    {
        public DB db;
        public DataTable dt { get; set; }
        public ActivitiesModel(DB db)
        {
            this.db = db;
        }
        public void OnGet()
        {
            dt = db.RecentTrips();

        }
    }
}
