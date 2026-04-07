using Animatch.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Animatch.Controllers.Api
{
    [ApiController]
    [Route("api/events")]
    public class EventsApiController : ControllerBase
    {
        private readonly AnimalManagerDbContext context;

        public EventsApiController(AnimalManagerDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            string? searchTerm = null,
            string? location = null,
            bool withCoordinates = false,
            int page = 1,
            int pageSize = 10)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 100 ? 10 : pageSize;

            var query = context.Events.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var normalized = searchTerm.Trim().ToLower();
                query = query.Where(e => e.Title.ToLower().Contains(normalized));
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                var normalizedLocation = location.Trim().ToLower();
                query = query.Where(e => e.Location.ToLower().Contains(normalizedLocation));
            }

            if (withCoordinates)
            {
                query = query.Where(e => e.Latitude.HasValue && e.Longitude.HasValue);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(e => e.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new
                {
                    e.Id,
                    e.Title,
                    e.Description,
                    e.Date,
                    e.Location,
                    e.Latitude,
                    e.Longitude
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
