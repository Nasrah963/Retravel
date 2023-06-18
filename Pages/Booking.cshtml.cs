using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DB_Project.Models;
using System.Data;

namespace DB_Project.Pages
{
    public class BookingModel : PageModel
    {
        public int book_tourID;
        public DB DB;
        public DataTable dt;
        public BookingModel(DB dB)
        {
            this.DB = dB;
        }
        public void OnGet()
        {
            
            int ii = DB.selectedtripid;
			dt = DB.getHotel(ii);
		}
        public IActionResult OnPost()
        {
            return RedirectToPage("/Reciet");
        }
    }
}
