using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;

namespace DB_Project.Models
{
    public class DB
    {
        public readonly SqlConnection con;

        //public DB()
        //{
        //    con = new SqlConnection("Data Source=DESKTOP-RCNAMN0;Initial Catalog=SWDB;Integrated Security=True;Encrypt=False");
        //}
        public DB(SqlConnection connection = null)
        {
            //con = connection ?? new SqlConnection("Data Source=DESKTOP-RCNAMN0;Initial Catalog=SWDB;Integrated Security=True;Encrypt=False");
            con = connection ?? new SqlConnection("Data Source=DESKTOP-RCNAMN0;Initial Catalog=SWDB_Test;Integrated Security=True;Encrypt=False");
        }


        //public DB(SqlConnection connection)
        //{
        //    con = connection;
        //}

        private readonly IDatabaseExecutor _databaseExecutor;

        public DB(IDatabaseExecutor databaseExecutor)
        {
            _databaseExecutor = databaseExecutor;
        }

        //public void Add_Trip(string destination, int price, int max_no, int min_no, string start_date, string end_date)
        //{
        //    if (string.IsNullOrEmpty(destination) || price <= 0 || max_no <= 0 || min_no <= 0 || string.IsNullOrEmpty(start_date) || string.IsNullOrEmpty(end_date))
        //    {
        //        throw new ArgumentException("Invalid input data for adding a trip.");
        //    }

        //    string query = $@"
        //    INSERT INTO TOUR 
        //    VALUES ((SELECT MAX(Tour_ID) + 1 FROM TOUR), '{destination}', {price}, {max_no}, {min_no}, '{destination}.png', '{start_date}', '{end_date}', 0)";
        //    ExecuteQuery(query);

        //}
        public void Add_Trip(string destination, int price, int max_no, int min_no, string start_date, string end_date)
        {
            if (string.IsNullOrEmpty(destination) || price <= 0 || max_no <= 0 || min_no <= 0 || string.IsNullOrEmpty(start_date) || string.IsNullOrEmpty(end_date))
            {
                throw new ArgumentException("Invalid input data for adding a trip.");
            }

            string checkQuery = $@"
        SELECT COUNT(*) FROM TOUR
        WHERE Destination = @Destination AND Start__date = @StartDate";

            SqlCommand checkCmd = new SqlCommand(checkQuery, con);
            checkCmd.Parameters.AddWithValue("@Destination", destination);
            checkCmd.Parameters.AddWithValue("@StartDate", start_date);

            try
            {
                con.Open();
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                con.Close();

                if (count > 0)
                {
                    throw new InvalidOperationException("A trip with the same destination and start date already exists.");
                }

                string query = $@"
            INSERT INTO TOUR (Tour_ID, Destination, Price, Max_no, Min_no, Photo, Start__date, End__date, Num_tickets)
            VALUES ((SELECT COALESCE(MAX(Tour_ID), 0) + 1 FROM TOUR), @Destination, @Price, @MaxNo, @MinNo, @Photo, @StartDate, @EndDate, 0)";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Destination", destination);
                cmd.Parameters.AddWithValue("@Price", price);
                cmd.Parameters.AddWithValue("@MaxNo", max_no);
                cmd.Parameters.AddWithValue("@MinNo", min_no);
                cmd.Parameters.AddWithValue("@Photo", destination + ".png");
                cmd.Parameters.AddWithValue("@StartDate", start_date);
                cmd.Parameters.AddWithValue("@EndDate", end_date);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("Database operation failed.", ex);
            }
            finally
            {
                con.Close();
            }
        }



        public static bool Islogged { get; set; } = false;
        public static int UserTypeAOU { get; set; } = 0;
       
        public object ExecuteTable(string func)
        {
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(func, con);
                dt.Load(cmd.ExecuteReader());
                con.Close();
                return dt;
            }
            catch (SqlException ex)
            {
                con.Close();
                return null;
            }
        }
        public object ExecuteScalar(string func)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(func, con);
                object dt = (object)cmd.ExecuteScalar();
                con.Close();
                return dt;
            }
            catch (SqlException ex)
            {
                con.Close();
                return null;
            }
        }
        public void ExecuteQuery(string func)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(func, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (SqlException ex)
            {
                con.Close();
            }
        }
        public static bool CheckLogged() //Login 
        {
            return Islogged;
        }
        public static int GetUserTypeAOU() //Login
        {
            return UserTypeAOU;
        }
        public static void LogIn() //Login
        {
            Islogged = true;
        }
        public static void EditUserTypeAOU(int value)  //Login
        {
            UserTypeAOU = value;
        }
        public int getusertyper(string username) //Login
        {
            string Q = " select userType from PERSON where name = '" + username + "'";
            int usertype = (int)ExecuteScalar(Q);
            return usertype;
        }
        public bool checkpassword(string username, string password) //Login
        {
            string Q = " select Pass from PERSON where name = '" + username + "'";
            object userpass = ExecuteScalar(Q);
            string userpassss = (string)userpass;
            if (userpassss == password) return true;
            else return false;
        }
        public int GetUserIdByUsername(string username) // Profile
        {
            string Q = "SELECT ID FROM PERSON WHERE name ='"+username+"'";
            object userId = ExecuteScalar(Q);  
            return (int)userId;
        }
        public void signup(string name, string email, string pass, string tel, int age, string city)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass) ||
                string.IsNullOrEmpty(tel) || age <= 0 || string.IsNullOrEmpty(city))
            {
                throw new ArgumentException("Invalid input data for signup.");
            }

            string query = $@"
        INSERT INTO PERSON (ID, name, email, pass, UserType, tel, age, city)
        VALUES ((SELECT COALESCE(MAX(ID), 0) + 1 FROM PERSON), '{name}', '{email}', '{pass}', 2, '{tel}', {age}, '{city}')";
            ExecuteQuery(query);
        }

        //public void signup(string name, string email, string pass, string tel, int age, string city) // Sign up
        //{
        //    string Q = "INSERT INTO PERSON (ID, name, email, pass, UserType, tel, age, city) " +
        //               "VALUES ((SELECT COALESCE(MAX(ID), 0) + 1 FROM PERSON), '" + name + "', '" + email + "', '" + pass + "', 2, '" + tel + "', " + age + ", '" + city + "');";
        //    ExecuteQuery(Q);
        //}
        public DataRow GetUserById(int id)
        {
            string query = "SELECT name, email, tel, city, age FROM PERSON WHERE ID = @Id";
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    con.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error fetching user data: {ex.Message}");
                }
                finally
                {
                    con.Close();
                }
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }


        //public void Add_Trip(string destination ,int price,int max_no, int min_no, string Start_date, string end_date ) //Add_Trip
        //{
        //    if (string.IsNullOrEmpty(destination) || price <= 0 || max_no <= 0 || min_no <= 0 || string.IsNullOrEmpty(Start_date) || string.IsNullOrEmpty(end_date))
        //    {
        //        throw new ArgumentException("Invalid input data for adding a trip.");
        //    }
        //    String Q = "insert into TOUR values((select max(Tour_ID) + 1 from TOUR) ,'{destination}'," + price + "," + max_no + "," + min_no + ",'" + destination + ".Png','" + Start_date + "','" + end_date + "',0)";
        //    ExecuteQuery(Q);
        //}
        public DataTable ReadTableEmployee()// Employee 
        {
            DataTable dt = new DataTable();
            string query = "SELECT t.Tour_ID, t.Destination, t.Price,  t.Max_no, t.Min_no, t.Start__date, t.End__date, t.Num_tickets  FROM TOUR t WHERE t.Start__date > CAST(GETDATE() AS DATE);";

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                dt.Load(cmd.ExecuteReader());
            }
            catch (SqlException ex)
            {

            }
            finally
            {
                con.Close();
            }

            return dt;
        }
        //Edit tour
        public DataRow GetTourById(int id)
        {
            string query = "SELECT * FROM TOUR WHERE Tour_ID = @Id";
            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                try
                {
                    con.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                finally
                {
                    con.Close();
                }
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
        // Edit Tour
        public void Edit_Trip(int id, string destination, int price, int max_no, int min_no, string Start_date, string end_date, int Numoftickets)
        {
            string Q = $@"
        UPDATE TOUR 
        SET 
            Destination = '{destination}', 
            Price = {price}, 
            Max_no = {max_no}, 
            Min_no = {min_no}, 
            Photo = '{destination}.png', 
            Start__date = '{Start_date}', 
            End__date = '{end_date}', 
            Num_tickets = {Numoftickets} 
        WHERE Tour_ID = {id}";

            ExecuteQuery(Q);
        }
        // Delete Trip
        public void DeleteTour(int id)
        {
            string q = "DELETE FROM TOUR WHERE Tour_ID ="+id;
            ExecuteQuery(q);
        }
        public int getnexttripID() //Add_Trip
        {
            string Q = " SELECT ISNULL(MAX(Tour_ID), 0) + 1 FROM TOUR";
            int next_ID = (int)ExecuteScalar(Q);
            return next_ID;
        }

        public DataTable RecentTrips() //Home
        {
            string func = "SELECT TOP 3 * FROM TOUR WHERE Start__date >= CAST(GETDATE() AS DATE) ORDER BY ABS(DATEDIFF(DAY, Start__date, GETDATE())) ASC;";
            DataTable dt = new DataTable();
            dt = (DataTable)ExecuteTable(func);
            return dt;
        }
        //Destination_details
        public DataTable GetActivitiesByTourId(int tourId)
        {
            string query = @"
        SELECT 
            a.Name AS ActivityName,
            a.Type,
            a.Requirement,
            a.Capacity,
            a.Duration,
            a.Price,
            a.Description As ActivityDescription
        FROM 
            TOUR_ACTIV ta
        INNER JOIN 
            ACTIVITIES a ON ta.Activ_Name = a.Name
        WHERE 
            ta.Tour_ID ="+tourId;

            DataTable dt = new DataTable();

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@TourId", tourId);

                try
                {
                    con.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                finally
                {
                    con.Close();
                }
            }

            return dt;
        }


        public DataTable GetRowTour(int id)
        {
            string func= "select * from TOUR where Tour_ID = " + id;
            DataTable dt = new DataTable();
            dt= (DataTable)ExecuteTable(func);
            return dt;
        }
        public DataTable GetActivity(int id, int day)
        {
            string func = "select Activ_name , description from TOUR_ACTIV join ACTIVITIES on Activ_Name = Name where Tour_ID ="+id;
            DataTable dt = new DataTable();
            dt = (DataTable)ExecuteTable(func);
            return dt;
        }
        
        public DataTable UpToDataTrips()
        {
            string func = "SELECT * FROM TOUR where Start__date > (SELECT CAST( GETDATE() AS Date ) );";
            DataTable dt = new DataTable();
            dt = (DataTable)ExecuteTable(func);
            return dt;
        }
        public static List<string> GetCities()
        {
            string connectionString = "Data Source=DESKTOP-6IINIIQ;Initial Catalog=TravelAgencyDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT DISTINCT Destination FROM TOUR";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                List<string> cities = new List<string>();
                while (reader.Read())
                {
                    string city = reader["Destination"].ToString();
                    cities.Add(city);
                }

                return cities;
            }
        }

        public string GetDate(int id)
        {
            string connectionString = "Data Source=DESKTOP-6IINIIQ;Initial Catalog=TravelAgencyDB;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string func = "select Start__date FRIM TOUR where Tour_ID = " + id;
                string dt;
                dt = (string)ExecuteTable(func);
                return dt;
            }
        }

        public static DataTable SearchTour(string city, DateTime date, string travelType)
        {
            string connectionString = "Data Source=DESKTOP-6IINIIQ;Initial Catalog=TravelAgencyDB;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            string query = "SELECT * FROM TOUR WHERE Destination = @city AND Date = @date AND TravelType = @travelType";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@City", city);
            command.Parameters.AddWithValue("@Date", date);
            command.Parameters.AddWithValue("@TravelType", travelType);
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            connection.Close();
            return dt;
        }

        public string GetPaymentID()
        {
            string query = "SELECT Payment_ID FROM PAYMENT";
            SqlCommand command = new SqlCommand(query, con);

            try
            {
                con.Open();
                object result = command.ExecuteScalar();
                string paymentID = result != null ? result.ToString() : string.Empty;
                con.Close();
                return paymentID;
            }
            catch (SqlException ex)
            {
                con.Close();
                return string.Empty;
            }
        }
        public DataTable GetHotelRoomDetails(int tourId, int hotelId)
        {
            string query = "SELECT Rooms, RoomPrice FROM TOUR_HOTEL WHERE Tour_Id = @tourId AND Hotel_Id = @hotelId";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@tourId", tourId);
            command.Parameters.AddWithValue("@hotelId", hotelId);

            DataTable dt = new DataTable();

            try
            {
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            catch (SqlException ex)
            {
                con.Close();
            }

            return dt;
        }

        public int GetNumTickets(int tourId)
        {
            string query = "SELECT Num_tickets FROM TOUR WHERE Tour_ID = @tourId";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@tourId", tourId);

            try
            {
                con.Open();
                object result = command.ExecuteScalar();
                int numTickets = result != null ? Convert.ToInt32(result) : 0;
                con.Close();
                return numTickets;
            }
            catch (SqlException ex)
            {
                con.Close();
                return 0;
            }
        }

        
        public Employee GetEmployeeById(int id)
        {

            var employees = GetEmployees();
            return employees.SingleOrDefault(e => e.Employee_ID == id);
        }

        private List<Employee> GetEmployees()
        {
            return new List<Employee>();
        }
    }
}


