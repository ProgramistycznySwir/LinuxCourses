using System.Threading.Tasks;
using LinuxCourses.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace LinuxCourses.Data.Services;

public interface ICourseRepository
{
	Task CreateAsync(Quiz newBook);
	Task<List<Quiz>> GetAllAsync();
	Task<Quiz?> GetAsync(Guid id);
	Task RemoveAsync(Guid id);
	Task UpdateAsync(Guid id, Quiz quiz);
}

// public class CourseRepository : ICourseRepository
// {
// 	private readonly IMongoCollection<Quiz> _quizes;
// 	private readonly IMongoCollection<Course> _course;

// 	public CourseRepository(IMongoDb mongo)
// 	{
// 		_quizes = mongo.Quizes();
// 		_course = mongo.Courses();
// 	}

// 	public async Task<List<Course>> GetAllAsync()
// 	{
// 		var fields_ = Builders<Course>.Projection.Include(e => e.Id).Include(e => e.Name);
// 		return await _course.Find(_ => true).Project<Course>(fields_).ToListAsync();
// 	}

// 	public async Task<Quiz?> GetAsync(Guid id) =>
// 		await _course.Find(x => x.Id == id).FirstOrDefaultAsync();

// 	public async Task CreateAsync(Quiz newBook) =>
// 		await _quizes.InsertOneAsync(newBook);

// 	public async Task UpdateAsync(Guid id, Quiz quiz) =>
// 		await _quizes.ReplaceOneAsync(x => x.Id == id, quiz);

// 	public async Task RemoveAsync(Guid id) =>
// 		await _quizes.DeleteOneAsync(x => x.Id == id);
// }