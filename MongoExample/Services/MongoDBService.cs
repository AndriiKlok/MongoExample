using MongoExample.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Microsoft.Extensions.Options;

namespace MongoExample.Services;

public class MongoDBService
{
	private readonly IMongoCollection<Playlist> _playlistCollection;

	public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
	{
		MongoClient client = new(mongoDBSettings.Value.ConnectionURI);
		IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
		_playlistCollection = database.GetCollection<Playlist>(mongoDBSettings.Value.CollectionName);
	}

	public async Task CreateAsync(Playlist playlist)
	{
		await _playlistCollection.InsertOneAsync(playlist);
		return;
	}

	public async Task<List<Playlist>> GetAsync()
	{
		return await _playlistCollection.Find(new BsonDocument()).ToListAsync();
	}

	public async Task AddMovieList(string id, string username)
	{
		FilterDefinition<Playlist> filter = Builders<Playlist>.Filter.Eq("id", id);
		UpdateDefinition<Playlist> update = Builders<Playlist>.Update.AddToSet<string>("username", username);

		await _playlistCollection.UpdateOneAsync(filter, update);
		return;
	}

	public async Task DeleteAsync(string id)
	{
		FilterDefinition<Playlist> filter = Builders<Playlist>.Filter.Eq("id", id);
		await _playlistCollection.DeleteOneAsync(filter);
		return;
	}
}