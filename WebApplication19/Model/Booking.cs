using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication19.Model
{
    public class Booking
    {
       
        public string bookingId { get; set; }
        public string userId { get; set; }
        public string busNumber { get; set; }
        public int seatNumber { get; set; }
        public DateTime bookingDate { get; set; }
        public string status { get; set; } = "confirmed";
    }
}
