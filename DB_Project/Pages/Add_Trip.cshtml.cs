using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DB_Project.Models;

namespace DB_Project.Pages
{
    public class Add_TripModel : PageModel
    {
        private readonly DB _db;

        public Add_TripModel(DB db)
        {
            _db = db;
        }

        [BindProperty]
        public string Destination { get; set; }
        [BindProperty]
        public int Price { get; set; }
        [BindProperty]
        public int MaxNo { get; set; }
        [BindProperty]
        public int MinNo { get; set; }
        [BindProperty]
        public string Photo { get; set; }
        [BindProperty]
        public string StartDate { get; set; }
        [BindProperty]
        public string EndDate { get; set; }
        [BindProperty]
        public int NumTickets { get; set; }

        public IActionResult OnPost()
        {
            
            int nextTripId = _db.getnexttripID();

            _db.Add_Trip(Destination, Price, MaxNo, MinNo,StartDate,EndDate );

            return RedirectToPage("/Employee");

            //return RedirectToPage("/Add_Activity", new { tripId = nextTripId });
        }
    }
}
