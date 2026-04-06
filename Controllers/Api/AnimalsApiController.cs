using Animatch.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Animatch.Controllers.Api
{
    [ApiController]
    [Route("api/animals")]
    public class AnimalsApiController : ControllerBase
    {
        private readonly AnimalManagerDbContext context;

        public AnimalsApiController(AnimalManagerDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            string? searchTerm = null,
            int? categoryId = null,
            string? town = null,
            bool withCoordinates = false,
            int page = 1,
            int pageSize = 20)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 100 ? 20 : pageSize;

            var query = context.Animals
                .Include(a => a.Category)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalized = searchTerm.Trim().ToLower();
                query = query.Where(a => a.Name.ToLower().Contains(normalized));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(a => a.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(town))
            {
                var normalizedTown = town.Trim().ToLower();
                query = query.Where(a => a.Town != null && a.Town.ToLower() == normalizedTown);
            }

            if (withCoordinates)
            {
                query = query.Where(a => a.Latitude.HasValue && a.Longitude.HasValue);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(a => a.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new
                {
                    a.Id,
                    a.Name,
                    a.Species,
                    a.Town,
                    CategoryName = a.Category != null ? a.Category.Name : null,
                    a.Latitude,
                    a.Longitude,
                    a.ImageUrl
                })
                .ToListAsync();

            return Ok(new
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            });
        }
    }
}
