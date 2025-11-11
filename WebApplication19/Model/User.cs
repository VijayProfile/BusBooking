using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplication19.Model
{
    public class User
    {
        
        public string userId { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
        public string role { get; set; } = "User";
    }
}
