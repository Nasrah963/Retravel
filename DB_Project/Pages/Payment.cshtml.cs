using DB_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace DB_Project.Pages
{
    public class PaymentModel : PageModel
    {
        public string CurrentDate { get; set; }
        public string ReceiptID { get; set; }

        public int HotelRooms { get; set; }
        public decimal RoomPrice { get; set; }
        public int NumTickets { get; set; }

        public decimal TotalForHotels { get; set; }
        public decimal TotalForTrnas { get; set; }

        public decimal Total { get; set; }


        private readonly DB db;

        public PaymentModel(DB db)
        {
            db = db;
        }
        public void OnGet(int tourId, int hotelId)
        {
            CurrentDate = DateTime.Now.ToString("dd MMMM, yyyy");
            ReceiptID = db.GetPaymentID();

            DataTable hotelRoomDetails = db.GetHotelRoomDetails(tourId, hotelId);

            if (hotelRoomDetails.Rows.Count > 0)
            {
                HotelRooms = Convert.ToInt32(hotelRoomDetails.Rows[0]["Rooms"]);
                RoomPrice = Convert.ToDecimal(hotelRoomDetails.Rows[0]["RoomPrice"]);
            }

            NumTickets = db.GetNumTickets(tourId);


            TotalForHotels = (RoomPrice * HotelRooms);


            TotalForTrnas = (100 * NumTickets);

            Total = (RoomPrice * HotelRooms) + (100 * NumTickets);
        }
    }
}
