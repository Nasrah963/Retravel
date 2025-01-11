using DB_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DB_Project.Pages
{
    public class travel_destinationModel : PageModel
    {
        public DB db;
        public DataTable dtt { get; set; }
        public travel_destinationModel(DB db)
        {
            this.db = db;
        }
        public void OnGet()
        {
            dtt = db.UpToDataTrips();
        }
    }
}
