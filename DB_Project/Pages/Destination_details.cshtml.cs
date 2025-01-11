using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Collections.Generic;
using DB_Project.Models;

namespace DB_Project.Pages
{
    public class Destination_detailsModel : PageModel
    {
        private readonly DB _db;

        public DataRow TourDetails { get; set; } // Details about the tour
        public Dictionary<int, List<DataRow>> ActivitiesByDay { get; set; } // Grouped activities by day
        public DataTable RecentTrips { get; set; } // Data for recent trips

        public Destination_detailsModel(DB db)
        {
            _db = db;
        }

        public void OnGet(int id)
        {
            // Fetch the tour details
            TourDetails = _db.GetTourById(id);

            // Fetch all activities for the tour
            var allActivities = _db.GetActivitiesByTourId(id);

            // Organize activities into days
            ActivitiesByDay = GroupActivitiesByDay(allActivities);

            // Fetch recent trips
            RecentTrips = _db.RecentTrips();
        }

        public IActionResult OnPost()
        {
            // Handle the book button click
            return RedirectToPage("/Booking");
        }

        /// <summary>
        /// Groups activities by day and returns a dictionary.
        /// </summary>
        private Dictionary<int, List<DataRow>> GroupActivitiesByDay(DataTable allActivities)
        {
            var activitiesByDay = new Dictionary<int, List<DataRow>>();
            int day = 1; // Initialize the day counter

            foreach (DataRow activity in allActivities.Rows)
            {
                if (!activitiesByDay.ContainsKey(day))
                {
                    activitiesByDay[day] = new List<DataRow>();
                }

                activitiesByDay[day].Add(activity);

                // Assign activities to the next day after every 3 activities (example logic)
                if (activitiesByDay[day].Count == 3)
                {
                    day++;
                }
            }

            return activitiesByDay;
        }
    }
}
