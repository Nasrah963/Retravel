using DB_Project.Models;
using Moq;
using Xunit;

namespace DB_Testing.UnitTesting
{
    public class Signup_Test
    {
        [Fact]
        public void Signup_ValidData_ExecutesSuccessfully()
        {
            var mockExecutor = new Mock<IDatabaseExecutor>();
            var db = new DB(mockExecutor.Object);

            string name = "John Doe";
            string email = "john.doe@example.com";
            string pass = "password123";
            string tel = "123456789";
            int age = 25;
            string city = "Cairo";

            db.signup(name, email, pass, tel, age, city);

            mockExecutor.Verify(executor => executor.ExecuteQuery(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Signup_InvalidData_ThrowsArgumentException()
        {
            var mockExecutor = new Mock<IDatabaseExecutor>();
            var db = new DB(mockExecutor.Object);

            string name = ""; 
            string email = ""; 
            string pass = ""; 
            string tel = ""; 
            int age = 0; 
            string city = ""; 

            Assert.Throws<ArgumentException>(() => db.signup(name, email, pass, tel, age, city));
        }

        [Fact]
        public void Signup_SqlException_HandlesGracefully()
        {
            var mockExecutor = new Mock<IDatabaseExecutor>();
            mockExecutor
                .Setup(executor => executor.ExecuteQuery(It.IsAny<string>()))
                .Throws(new ApplicationException("Simulated SQL Exception"));

            var db = new DB(mockExecutor.Object);

            string name = "Jane Doe";
            string email = "jane.doe@example.com";
            string pass = "password123";
            string tel = "987654321";
            int age = 30;
            string city = "Alexandria";

            var exception = Assert.Throws<ApplicationException>(() =>
                db.signup(name, email, pass, tel, age, city));
            Assert.Equal("Simulated SQL Exception", exception.Message);
        }
    }
}
