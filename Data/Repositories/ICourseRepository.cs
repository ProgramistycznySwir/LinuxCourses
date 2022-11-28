using System.Threading.Tasks;
using LinuxCourses.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace LinuxCourses.Data.Services;

public interface ICourseRepository
{
	Task Create(Course course);
	Task<List<Course>> GetAllIn(Guid categoryId);
	Task<Quiz?> Get(Guid id);
	Task Remove(Guid id);
	Task Update(Guid id, Course course);
}

public class CourseRepository : ICourseRepository
{
	private readonly IMongoCollection<Quiz> _quizes;
	private readonly IMongoCollection<Course> _courses;
	private readonly IMongoCollection<CourseCategory> _categories;

	public CourseRepository(IMongoDb mongo)
	{
		_quizes = mongo.Quizes();
		_courses = mongo.Courses();
		_categories = mongo.Categories();
	}

	public async Task Create(Course course)
	{
		course.Id = Guid.NewGuid();
		var query = Query<CourseCategory>
			.WithEq(e => e.Name, "BRAK")
			.WithUpdate(b => b.Push(e => e.Courses, course.Id ));
		var filter = query.Filter;
		var updatedCategory = await _categories.FindOneAndUpdateAsync(filter, query);
		course.CategoryId = updatedCategory.Id;
		await _courses.InsertOneAsync(course);
	}

	public Task<Quiz?> Get(Guid id)
	{
		throw new NotImplementedException();
	}

	public Task<List<Course>> GetAllIn(Guid categoryId)
	{
		throw new NotImplementedException();
	}

	public Task Remove(Guid id)
	{
		throw new NotImplementedException();
	}

	public Task Update(Guid id, Course course)
	{
		throw new NotImplementedException();
	}
}