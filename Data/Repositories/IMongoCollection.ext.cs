
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Elasticsearch.Net;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;

namespace LinuxCourses.Data.Services;

public static class IMongoCollection_
{
	public static async Task<IAsyncCursor<T>> GetAll<T>(this IMongoCollection<T> self, FilterDefinition<T> filter)
		=> await self.FindAsync(filter);
	public static async Task<IAsyncCursor<T>> GetAll<T>(this IMongoCollection<T> self)
		=> await IMongoCollection_.GetAll(self, new BsonDocument());
		// => await self.FindAsync(new BsonDocument());

	public static async Task<List<T>> GetAllAsList<T>(this IMongoCollection<T> self, FilterDefinition<T> filter)
		=> (await IMongoCollection_.GetAll(self, filter)).ToList();
	public static async Task<List<T>> GetAllAsList<T>(this IMongoCollection<T> self)
		=> (await IMongoCollection_.GetAll(self)).ToList();

	public static async Task<T?> GetFirst<T>(this IMongoCollection<T> self, FilterDefinition<T> filter)
		=> await(await self.FindAsync(filter)).FirstOrDefaultAsync();
	public static async Task<T?> GetFirst<T, T_id>(this IMongoCollection<T> self, T_id id) where T : IHasId<T_id>
		=> await(await self.FindAsync(Query<T, T_id>.WithId(id))).FirstOrDefaultAsync();
}