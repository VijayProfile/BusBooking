using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using WebApplication19.DbConnection;
using WebApplication19.Model;

namespace WebApplication19.MongoDbService
{
    public class BusService
    {
        private readonly IMongoCollection<Bus> collection;
        private readonly IMongoCollection<User> Usercollection;
        private readonly IMongoCollection<Booking> Bookingcollection;
        private readonly IMongoCollection<BookingResult> BookingResult;

        public BusService(IOptions<BusBookingDatabaseSettings> busBookingSettings, IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase(busBookingSettings.Value.DatabaseName);
            collection = database.GetCollection<Bus>("Buses");
            Usercollection = database.GetCollection<User>("Users");
            Bookingcollection = database.GetCollection<Booking>("Bookings");
        }

        #region Bookingbus
        public async Task bookingBus(Bus bus)
        {
            bus.busId = ObjectId.GenerateNewId().ToString();
            await collection.InsertOneAsync(bus);

            await Usercollection.InsertOneAsync(new User
            {
                userId = bus.userId,
                fullName = bus.fullName,
                email = bus.email,
                passwordHash = bus.passwordHash,
                role = "User"
            });

        }
        #endregion

        #region AvailableSeats
        public async Task<string> AvailableSeats(string busName)
        {
            var filter = Builders<Bus>.Filter.Eq(p => p.BusName, busName);
            var availableSeats = await collection.Find(filter).ToListAsync();

            if (availableSeats.Count == 0)
            { 
                return "no datas available";
            }
            int seatsAvailable = availableSeats.Sum(p => p.noOfSeats);

            int totalSeats = 100;
            int seats = totalSeats - seatsAvailable;

            return $"{seats} seats available";
        }
        #endregion

        #region CancelTicket
        public async Task<string> CancelTicket(string busId, string userId)
        {
            var filter = Builders<Bus>.Filter.And
                    (
                        Builders<Bus>.Filter.Eq(p => p.busId, busId),
                        Builders<Bus>.Filter.Eq(p => p.userId, userId)
                    );
            var newDatas = await collection.Find(filter).FirstOrDefaultAsync();

            if (newDatas == null)
            {
                return "No booking found for this user and bus.";
            }
            await collection.DeleteOneAsync(filter);
            return "cancelled successfully";
        }
        #endregion

        #region FindingBus
        public async Task<List<BookingResult>> searchBus(string from, string to)
        {
            var bus = await collection.Find(p => true).ToListAsync();
            var averagePrice = bus.Average(p => p.TicketPrice);
            var totalPrice = bus.Sum(p => p.TicketPrice);
            var sortedBus = bus
                           .Where(p => p.From == from && p.To == to)
                           .OrderByDescending(p => p.TicketPrice)
                           .Select(b => new BookingResult
                           {
                               BusName = b.BusName,
                               TicketPrice = b.TicketPrice,
                               TotalPrice = totalPrice,
                               AveragePrice = averagePrice

                           }).ToList();

            return sortedBus;

        }
        #endregion


        #region getALlBus
        public async Task<List<Bus>> getAllBus()
        {
            var getbus = await collection.Find(p => true).ToListAsync();
            if (getbus != null)
            {
                return getbus;
            }
            return null;
        }

        #endregion

        #region BangloreToTrichy
        public async Task<List<Bus>> filterBus(string from, string to)
        {
            var filterBus = await collection.Find(p => true).ToListAsync();

            var result = filterBus
                        .Where(p => p.From == from && p.To == to)
                        .OrderByDescending(p => p.TicketPrice)
                        .ToList();

            return result;
        }
        #endregion


        #region GetBookingDetails
        public async Task<List<GetBookingDetails>> getById(string userId, string busId)
        {
            var getDetails = Builders<Bus>.Filter.And
                (
                    Builders<Bus>.Filter.Eq(p => p.busId, busId),
                    Builders<Bus>.Filter.Eq(p => p.userId, userId)
                );

            var result = await collection.Find(getDetails).ToListAsync();

            var details = result.Select(p => new GetBookingDetails
            {
                FullName = p.fullName,
                BusName = p.BusName,
                From = p.From,
                To = p.To,
                DepartureTime = p.DepartureTime.ToString(),
                ArrivalTime = p.ArrivalTime.ToString()
            }).ToList();
            return details;
        }
        #endregion

        #region Rating
        public async Task<Bus> Rating(string busId,int rating) 
        {
            var bus = await collection.Find(p => p.busId == busId).FirstOrDefaultAsync();
            
            if (bus==null) 
            {
                throw new Exception($"no {busId} is found");
            }
           bus.rating.Add(rating);
            bus.averageRating = bus.rating.Average();
          

            await collection.ReplaceOneAsync(p => p.busId == busId,bus);
            return bus;
        }
        #endregion

        #region GetRating
        public async Task<Bus> GetRating(string busId) 
        {
            var bus = await collection.Find(p => p.busId == busId).FirstOrDefaultAsync();

            if (bus==null) 
            {
                throw new Exception("check api");
            }
            bus.averageRating = bus.rating.Average();
            return bus;
        }
        #endregion
    }
}
