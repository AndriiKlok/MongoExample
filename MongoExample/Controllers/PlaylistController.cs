using Microsoft.AspNetCore.Mvc;
using MongoExample.Models;
using MongoExample.Services;

namespace MongoExample.Controllers
{
	[Controller]
	[Route("api/[controller]")]
	public class PlaylistController : Controller
	{
		private readonly MongoDBService _mongoDBService;

		public PlaylistController(MongoDBService mongoDBService) => _mongoDBService = mongoDBService;

		[HttpGet]
		public async Task<List<Playlist>> Get()
		{
			return await _mongoDBService.GetAsync();
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Playlist playList)
		{
			await _mongoDBService.CreateAsync(playList);
			return CreatedAtAction(nameof(Get), new { id = playList.Id }, playList);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> AddToMovieList(string id, [FromBody] string username)
		{
			await _mongoDBService.AddMovieList(id, username);

			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			await _mongoDBService.DeleteAsync(id);
			return NoContent();
		}
	}
}

