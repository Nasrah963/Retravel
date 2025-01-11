using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DB_Project.Models;

namespace DB_Project.Pages
{
    public class Delete_TourModel : PageModel
    {
        private readonly DB _db;

        public Delete_TourModel(DB db)
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
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        public void OnGet()
        {
            // Fetch the details of the tour to display on the page
            var tour = _db.GetTourById(Id);
            if (tour != null)
            {
                Destination = tour["Destination"].ToString();
                Price = Convert.ToDecimal(tour["Price"]);
                StartDate = Convert.ToDateTime(tour["Start__date"]);
                EndDate = Convert.ToDateTime(tour["End__date"]);
            }
        }

        public IActionResult OnPost()
        {
            // Delete the tour from the database
            _db.DeleteTour(Id);

            // Redirect to the tour list page after deletion
            return RedirectToPage("/Employee");
        }
    }
}
