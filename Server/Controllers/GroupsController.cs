using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Models;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public GroupsController(ApplicationDbContext db) => _db = db;

        // GET api/groups — список всех групп
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _db.Groups.ToListAsync());

        // POST api/groups — добавить группу
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Group group)
        {
            if (await _db.Groups.AnyAsync(g => g.Name == group.Name))
                return BadRequest("Группа с таким названием уже существует");

            _db.Groups.Add(group);
            await _db.SaveChangesAsync();
            return Ok(group);
        }
    }
}