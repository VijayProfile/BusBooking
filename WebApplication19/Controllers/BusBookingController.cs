using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using WebApplication19.Model;
using WebApplication19.MongoDbService;

namespace WebApplication19.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusBookingController : ControllerBase
    {
        private readonly BusService _busService;
       
        public BusBookingController(BusService busService)
        {
            _busService = busService;

        }

        #region Busbooking
        [HttpPost]
        [Route("BookingBus")]
        public async Task<IActionResult> bookingBus(Bus bus)
        {
            await _busService.bookingBus(bus);
            if (bus != null)
            {
                return Ok(new { message = bus });
            }
            return NotFound(new { message = "something went wrong" });

        }
        #endregion

        #region AvailableBooking
        [HttpGet]
        [Route("AvailableSeats")]
        public async Task<IActionResult> availablebus(string bus) 
        {
            var result=await _busService.AvailableSeats(bus);

            if (result== "no datas available") 
            {
                return NotFound(new { message = "something went wrong" });
            }
            return Ok(new { message = result });
        }
        #endregion

        #region CancelTicket
        [HttpPost]
        [Route("CancelTicket")]
        public async Task<IActionResult> cancelTicket(string busId, string userId) 
        {
          var cancelTicket = await _busService.CancelTicket(busId,userId);

            if (cancelTicket!=null) 
            {
                return Ok(cancelTicket);
            }
            return NotFound(new {message="something error"});
        }
        #endregion

        #region SortedBus
        [HttpGet]
        [Route("SortBus")]
        public async Task<IActionResult> sortedBus(string from,string to) 
        {
           var searchBus= await _busService.searchBus(from,to);
            
            if (searchBus!=null) 
            {
                return Ok(new {searchBus });
            }
            return NotFound(new {message="no bus found"});

        }
        #endregion

        #region GetAllBus
        [HttpGet]
        [Route("GetAllBus")]
        public async Task<IActionResult> getAllBus() 
        {
           var result= await _busService.getAllBus();
            if (result!=null) 
            {
                return Ok(new { result });
            }
            return NotFound("not found");
        }
        #endregion

        #region BangloreTopTrichy
        [HttpGet]
        [Route("BlrToTpj")]
        public async Task<IActionResult> BlgToTpj(string from,string to) 
        {
         var serachBus=await _busService.filterBus(from, to);
            
            if (serachBus != null) 
            {
                return Ok(new {message= serachBus });
            }
            return NotFound("no datas");
        }
        #endregion

        #region GetBookingDetails
        [HttpGet]
        [Route("GetBookingDetails/{busId}")]
        public async Task<IActionResult> GetBookingDetails(string userId, string busId) 
        {
            var result=await _busService.getById(userId,busId);

            if (result!=null) 
            {
                return Ok(result);
            }
            return NotFound("no datas fonud");
        }
        #endregion

        #region AddRating
        [HttpPost]
        [Route("RatingBus")]
        public async Task<IActionResult> RatingBus(string busId, int rating) 
        {
           var addRating= await _busService.Rating(busId,rating);

            if (addRating!=null) 
            {
                return Ok(addRating);
            }
            return NotFound(new {message="spmeth8ing went wrong"});
        }
        #endregion

        #region GetRatings
        [HttpGet]
        [Route("GetRatingOfBus/{busId}")]
        public async Task<IActionResult> getRating(string busId) 
        {
            var getRatings=await _busService.GetRating(busId);

            if (getRatings != null) 
            {
                return Ok(getRatings);
            }
            return NotFound("not found");

        }
        #endregion
    }
}

