using DB_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DB_Project.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public DB db;
        public DataTable dt { get; set; }
        public IndexModel(ILogger<IndexModel> logger , DB DB)
        {
            _logger = logger;
            db = DB;
        }

        public void OnGet()
        {
            dt = db.RecentTrips();
        }
    }
}