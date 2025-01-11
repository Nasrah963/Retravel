using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DB_Project.Models;

namespace DB_Project.Pages
{
    public class Edit_TourModel : PageModel
    {
        private readonly DB _db;

        public Edit_TourModel(DB db)
        {
            _db = db;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; } // Tour ID

        [BindProperty]
        public string Destination { get; set; }

        [BindProperty]
        public decimal Price { get; set; }

        [BindProperty]
        public int MaxNo { get; set; }

        [BindProperty]
        public int MinNo { get; set; }

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        public int NumTickets { get; set; }
        public void OnGet(int id)
        {
            Id = id;
            var tour = _db.GetTourById(Id);
            if (tour != null)
            {
                Destination = tour["Destination"].ToString();
                Price = Convert.ToInt32(tour["Price"]);
                MaxNo = Convert.ToInt32(tour["Max_no"]);
                MinNo = Convert.ToInt32(tour["Min_no"]);
                StartDate = Convert.ToDateTime(tour["Start__date"]);
                EndDate = Convert.ToDateTime(tour["End__date"]);
                NumTickets = Convert.ToInt32(tour["Num_tickets"]);
            }
        }

        public IActionResult OnPost()
        {
            _db.Edit_Trip(Id,Destination, Convert.ToInt32(Price), MaxNo,MinNo, StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"), NumTickets);

            return RedirectToPage("/Employee"); 
        }
    }
}
