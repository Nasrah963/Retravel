using DB_Project.Models;
using Moq;
using Xunit;

namespace DB_Testing.UnitTesting
{
    public class Add_Trip_Test
    {
        [Fact]
        public void Add_Trip_ValidData_ExecutesSuccessfully()
        {
            var mockExecutor = new Mock<IDatabaseExecutor>();
            var db = new DB(mockExecutor.Object);

            string destination = "Cairo";
            int price = 1000;
            int max_no = 20;
            int min_no = 10;
            string start_date = "2025-01-15";
            string end_date = "2025-01-20";

            db.Add_Trip(destination, price, max_no, min_no, start_date, end_date);

            mockExecutor.Verify(executor => executor.ExecuteQuery(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Add_Trip_InvalidData_ThrowsArgumentException()
        {
            var mockExecutor = new Mock<IDatabaseExecutor>();
            var db = new DB(mockExecutor.Object);

            string destination = "";
            int price = 0; 
            int max_no = 0; 
            int min_no = -1; 
            string start_date = ""; 
            string end_date = "";

            Assert.Throws<ArgumentException>(() => db.Add_Trip(destination, price, max_no, min_no, start_date, end_date));
        }

        [Fact]
        public void Add_Trip_SqlException_HandlesGracefully()
        {
            var mockExecutor = new Mock<IDatabaseExecutor>();
            mockExecutor
                .Setup(executor => executor.ExecuteQuery(It.IsAny<string>()))
                .Throws(new ApplicationException("Simulated SQL Exception"));

            var db = new DB(mockExecutor.Object);

            string destination = "Cairo";
            int price = 1000;
            int max_no = 20;
            int min_no = 10;
            string start_date = "2025-01-15";
            string end_date = "2025-01-20";

            var exception = Assert.Throws<ApplicationException>(() => db.Add_Trip(destination, price, max_no, min_no, start_date, end_date));
            Assert.Equal("Simulated SQL Exception", exception.Message);
        }
    }
}
