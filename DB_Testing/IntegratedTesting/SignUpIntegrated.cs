using Xunit;
using System;
using System.Data.SqlClient;
using DB_Project.Models;

namespace DB_Testing.IntegratedTesting
{
    public class SignUpIntegrated
    {
        private readonly DB _db;

        public SignUpIntegrated()
        {
            string connectionString = "Data Source=DESKTOP-RCNAMN0;Initial Catalog=SWDB_Test;Integrated Security=True;Encrypt=False";
            _db = new DB(new SqlConnection(connectionString));
        }

        [Fact]
        public void SignUp_ValidData_ShouldPass()
        {
            // Arrange
            string name = "Valid User";
            string email = "validuser@example.com";
            string pass = "securepassword";
            string tel = "123456789";
            int age = 30;
            string city = "New York";

            try
            {
                // Act
                _db.signup(name, email, pass, tel, age, city);

                // Assert: Check if the user exists in the database
                bool userExists = CheckIfUserExists(name, email);
                Assert.True(userExists);
            }
            finally
            {
                // Clean up test data
                CleanUpTestData(name, email);
            }
        }

        [Fact]
        public void SignUp_InvalidData_ShouldThrowArgumentException()
        {
            // Arrange
            string name = ""; // Invalid name
            string email = ""; // Invalid email
            string pass = ""; // Invalid password
            string tel = ""; // Invalid telephone
            int age = -1; // Invalid age
            string city = ""; // Invalid city

            // Act & Assert: Ensure the method throws an ArgumentException
            Assert.Throws<ArgumentException>(() => _db.signup(name, email, pass, tel, age, city));
        }

        /// <summary>
        /// Checks if a user with the given name and email exists in the database.
        /// </summary>
        private bool CheckIfUserExists(string name, string email)
        {
            string query = @"
                SELECT COUNT(*) FROM PERSON
                WHERE name = @Name AND email = @Email";

            SqlCommand cmd = new SqlCommand(query, _db.con);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Email", email);

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

        /// <summary>
        /// Cleans up test data by removing the user from the database.
        /// </summary>
        private void CleanUpTestData(string name, string email)
        {
            string query = @"
                DELETE FROM PERSON
                WHERE name = @Name AND email = @Email";

            SqlCommand cmd = new SqlCommand(query, _db.con);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Email", email);

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
