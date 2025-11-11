using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication19.Model
{
    public class Bus
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string busId { get; set; } 
        public string userId { get; set; }
        public string BusName { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int noOfSeats { get; set; }
        //public int? totalSeats { get; set; }
        public decimal TicketPrice { get; set; }
        public int seatNumber { get; set; }
        public List<int> rating { get; set; } = new List<int>();
        public double averageRating { get; set; } = 0;
        //public List<User>? userDetails { get; set; }
        //public List<Booking> bookingDetails { get; set; }

    }
}
