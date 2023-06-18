using DB_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DB_Project.Pages
{
    public class filteredModel : PageModel
    {
        public DB db;
        public DataTable dt { get; set; }
        public DataTable dtt { get; set; }
        public string s;
        public filteredModel(DB db)
        {
            this.db = db;
        }
        public void OnGet(string s)
        {
            this.s = s;
            dtt = db.filiteredtrips(s);
            dt = db.RecentTrips();
        }

    }
}
