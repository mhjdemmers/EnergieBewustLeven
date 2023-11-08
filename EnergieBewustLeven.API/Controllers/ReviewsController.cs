using EnergieBewustLeven.API.Data;
using EnergieBewustLeven.API.Models.Domain;
using EnergieBewustLeven.API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnergieBewustLeven.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly EnergieBewustLevenDbContext dbContext;

        public ReviewsController(EnergieBewustLevenDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //GET REVIEW BY ID
        //GET:
        [HttpGet]
        [Route("ById/{id:Guid}")]
        public IActionResult GetReview([FromRoute] Guid id)
        {
            // Get from database - Domain Model
            var reviewDomain = dbContext.Reviews.FirstOrDefault(x => x.Id == id);

            if (reviewDomain == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            var reviewDTO = new ReviewDTO()
            {
                Id = reviewDomain.Id,
                ApplianceId = reviewDomain.ApplianceId,
                ReviewScore = reviewDomain.ReviewScore,
                ReviewText = reviewDomain.ReviewText
            };

            // Return DTO
            return Ok(reviewDTO);
        }

        //GET REVIEW BY APPLIANCE
        //GET:
        [HttpGet]
        [Route("ByAppliance/{id:Guid}")]
        public async Task<IActionResult> GetReviewsByAppliance([FromRoute] Guid applianceId)
        {
            // Get from database - Domain Model
            var reviewsDomain = await dbContext.Reviews.Where(m => m.ApplianceId == applianceId).ToListAsync();

            // Map Domain Models to DTOs
            var reviewsDTO = new List<ReviewDTO>();
            foreach (var reviewDomain in reviewsDomain)
            {
                reviewsDTO.Add(new ReviewDTO()
                {
                    Id = reviewDomain.Id,
                    ApplianceId = reviewDomain.ApplianceId,
                    ReviewScore = reviewDomain.ReviewScore,
                    ReviewText = reviewDomain.ReviewText
                });
            }

            // Return DTOs
            return Ok(reviewsDTO);
        }

        // POST CREATE NEW REVIEW
        // POST:
        [HttpPost]
        public IActionResult CreateReview([FromBody] AddReviewRequestDTO addReviewRequestDTO)
        {
            // Map DTO to Domain Model
            var reviewDomain = new Review
            {
                ApplianceId = addReviewRequestDTO.ApplianceId,
                ReviewScore = addReviewRequestDTO.ReviewScore,
                ReviewText = addReviewRequestDTO.ReviewText
            };

            // Use Domain Model to create appliance
            dbContext.Reviews.Add(reviewDomain);
            dbContext.SaveChanges();

            // Map Domain back to DTO
            var reviewDTO = new ReviewDTO()
            {
                Id = reviewDomain.Id,
                ApplianceId = addReviewRequestDTO.ApplianceId,
                ReviewScore = addReviewRequestDTO.ReviewScore,
                ReviewText= addReviewRequestDTO.ReviewText
            };

            return CreatedAtAction(nameof(GetReview), new { id = reviewDomain.Id }, reviewDTO);
        }

        // DELETE REVIEW
        // DELETE:
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult DeleteReview([FromRoute] Guid id)
        {
            // Get from database - Domain Model
            var reviewDomain = dbContext.Reviews.FirstOrDefault(x => x.Id == id);

            if (reviewDomain == null)
            {
                return NotFound();
            }

            // Delete from database
            dbContext.Reviews.Remove(reviewDomain);
            dbContext.SaveChanges();

            return NoContent();
        }
    }
}
