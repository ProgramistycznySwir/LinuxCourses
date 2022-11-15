using System.Threading.Tasks;
using LinuxCourses.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace LinuxCourses.Data.Services;

public interface IQuizRepository
{
	Task CreateAsync(Quiz newBook);
	Task<List<Quiz>> GetAllAsync();
	Task<Quiz?> GetAsync(Guid id);
	Task RemoveAsync(Guid id);
	Task UpdateAsync(Guid id, Quiz quiz);
}

public class QuizRepository : IQuizRepository
{
	private readonly IMongoCollection<Quiz> _quizes;

	public QuizRepository(
		IOptions<LinuxCoursesDatabaseSettings> bookStoreDatabaseSettings)
	{
		var settings = bookStoreDatabaseSettings.Value;
		var mongoClient_ = new MongoClient(settings.ConnectionString);
		var mongoDatabase_ = mongoClient_.GetDatabase(settings.DatabaseName);
		_quizes = mongoDatabase_.GetCollection<Quiz>(settings.Quizes);
	}

	public async Task<List<Quiz>> GetAllAsync() =>
		await _quizes.Find(_ => true).ToListAsync();

	public async Task<Quiz?> GetAsync(Guid id) =>
		await _quizes.Find(x => x.Id == id).FirstOrDefaultAsync();

	public async Task CreateAsync(Quiz newBook) =>
		await _quizes.InsertOneAsync(newBook);

	public async Task UpdateAsync(Guid id, Quiz quiz) =>
		await _quizes.ReplaceOneAsync(x => x.Id == id, quiz);

	public async Task RemoveAsync(Guid id) =>
		await _quizes.DeleteOneAsync(x => x.Id == id);
}