using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DB_Project.Models;
using System.Data.SqlClient;
using System.Data;

namespace DB_Project.Pages
{
    
    public class destination_detailsModel : PageModel
    {
        public DB db;
        public destination_detailsModel(DB db) {  this.db = db; }
        public DataTable dt { get; set; }
        public DataTable dtt1 { get; set; }
        public DataTable dtt2 { get; set; }
        public DataTable dtt3 { get; set; }

        public int ID { get; set; }

        public void OnGet(int id)
        {
            dt = db.GetRowTour(id);
            dtt1 = db.GetActivity(id, 1);
            dtt2 = db.GetActivity(id, 2);
            dtt3 = db.GetActivity(id, 3);
			ID = id;
            db.selectedtripid = id;
		}
		public IActionResult OnPost() 
        {
            int iiiii = db.selectedtripid;
            return RedirectToPage ("/Booking", new { book_tourID = ID});
        }
    }
}
