using Xunit;
using System;
using System.Data.SqlClient;
using DB_Project.Models;

namespace DB_Testing.IntegratedTesting
{
    public class Add_TripIntegrated
    {
        private readonly DB _db;

        public Add_TripIntegrated()
        {
            string connectionString = "Data Source=DESKTOP-RCNAMN0;Initial Catalog=SWDB_Test;Integrated Security=True;Encrypt=False";
            _db = new DB(new SqlConnection(connectionString));
        }

        [Fact]
        public void Add_Trip_ValidData_ShouldPass()
        {
            string destination = "Valid Test Destination";
            int price = 1500;
            int max_no = 30;
            int min_no = 5;
            string start_date = "2025-08-01";
            string end_date = "2025-08-10";

            try
            {
                _db.Add_Trip(destination, price, max_no, min_no, start_date, end_date);

                bool tripExists = CheckIfTripExists(destination, start_date);
                Assert.True(tripExists);
            }
            finally
            {
                CleanUpTestData(destination, start_date);
            }
        }

        [Fact]
        public void Add_Trip_InvalidData_ShouldThrowArgumentException()
        {
            // Arrange
            string destination = ""; // Invalid empty destination
            int price = -1000;       // Invalid negative price
            int max_no = 0;          // Invalid zero max number
            int min_no = -5;         // Invalid negative min number
            string start_date = "";  // Invalid empty start date
            string end_date = "";    // Invalid empty end date

            // Act & Assert: Expect an ArgumentException for invalid input data
            Assert.Throws<ArgumentException>(() =>
                _db.Add_Trip(destination, price, max_no, min_no, start_date, end_date));
        }

        private bool CheckIfTripExists(string destination, string start_date)
        {
            string query = $@"
                SELECT COUNT(*) FROM TOUR
                WHERE Destination = @Destination AND Start__date = @StartDate";

            SqlCommand cmd = new SqlCommand(query, _db.con);
            cmd.Parameters.AddWithValue("@Destination", destination);
            cmd.Parameters.AddWithValue("@StartDate", start_date);

            try
            {
                _db.con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
            finally
            {
                _db.con.Close();
            }
        }
        private void CleanUpTestData(string destination, string start_date)
        {
            string query = $@"
                DELETE FROM TOUR
                WHERE Destination = @Destination AND Start__date = @StartDate";

            SqlCommand cmd = new SqlCommand(query, _db.con);
            cmd.Parameters.AddWithValue("@Destination", destination);
            cmd.Parameters.AddWithValue("@StartDate", start_date);

            try
            {
                _db.con.Open();
                cmd.ExecuteNonQuery();
            }
            finally
            {
                _db.con.Close();
            }
        }
    }
}
