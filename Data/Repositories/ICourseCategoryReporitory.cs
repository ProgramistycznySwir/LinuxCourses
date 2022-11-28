using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LinuxCourses.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

using static LinuxCourses.Prelude;
namespace LinuxCourses.Data.Services;

public interface ICourseCategoryReporitory
{
	Task Create(CourseCategory category);

	Task<List<CourseCategory>> GetAll();
	/// <returns> All subCategories of this category. </returns>
	Task<List<CourseCategory>> GetAllIn(Guid? parentId= null);
	Task<CourseCategory?> Get(Guid id);

	Task Update(Guid id, CourseCategory category);
	Task AddCourse(Guid categoryId, Guid courseId);
	Task RemoveCourse(Guid categoryId, Guid courseId);
	Task MoveToCategory(Guid categoryId, Guid? parentCategoryId);

	Task Remove(Guid id);
}

public class CourseCategoryReporitory : ICourseCategoryReporitory
{
	private readonly IMongoCollection<CourseCategory> _categories;
	private readonly IMongoCollection<Course> _courses;

	public CourseCategoryReporitory(IMongoDb mongo)
	{
		_categories = mongo.Categories();
		_courses = mongo.Courses();
	}

	public async Task AddCourse(Guid categoryId, Guid courseId)
	{
		// First: Change category of course.
		var courseQuery = Query<Course>.WithId(courseId);
		Course course = await _courses.GetFirst(courseQuery);

		if(course.CategoryId is Guid oldCategoryId)
		{
			// Remove course from category.
			var remove = Query<CourseCategory>
				.WithId(oldCategoryId)
				.WithUpdate(b => b.Pull(nameof(CourseCategory.Courses), course.Id));
			
			await _categories.FindOneAndUpdateAsync(remove, remove);
		}

		// Add course to category.
		// TODO: Convert to Fluent Queries.
		var add_filter_ = Builders<CourseCategory>.Filter.Eq(nameof(CourseCategory.Id), categoryId);
		var add_update_ = Builders<CourseCategory>.Update.Push(nameof(CourseCategory.Courses), course.Id);
		await _categories.FindOneAndUpdateAsync(add_filter_, add_update_);

		// And last: Add category to course.
		var addToCourse_update_ = Builders<Course>.Update.Set(nameof(Course.CategoryId), categoryId);
		await _courses.FindOneAndUpdateAsync(courseQuery, addToCourse_update_);
	}

	public async Task Create(CourseCategory category)
	{
		await _categories.InsertOneAsync(category);
	}


	public async Task<List<CourseCategory>> GetAll()
	{
		return await _categories.GetAllAsList();
	}
	public async Task<List<CourseCategory>> GetAllIn(Guid? parentId = null)
	{
		var query = Query<CourseCategory>
			.WithEq(e => e.Parent, parentId);
		return await _categories.GetAllAsList(query);
	}

	public async Task<CourseCategory?> Get(Guid id)
	{
		var query = Query<CourseCategory>
			.WithId(id);
		return await _categories.GetFirst(query);
	}

	public Task Remove(Guid id)
	{
		throw new NotImplementedException();
	}

	public Task RemoveCourse(Guid categoryId, Guid courseId)
	{
		throw new NotImplementedException();
	}

	public Task Update(Guid id, CourseCategory category)
	{
		throw new NotImplementedException();
	}

	/// <param name="parentCategoryId">Set to null to make it root category.</param>
	public async Task MoveToCategory(Guid categoryId, Guid? parentCategoryId)
	{
		var remove = Query<CourseCategory>
			.WithFilter(b => b.AnyEq(e => e.SubCategories, categoryId))
			.WithUpdate(b => b.Pull(e => e.SubCategories, categoryId));
		// INFO: Idk, but, This one may throw errors if currentParent does not exist, beware.
		await _categories.FindOneAndUpdateAsync(remove, remove);

		var update = Query<CourseCategory>
			.WithId(categoryId)
			.WithUpdate(b => b.Set(e => e.Parent, parentCategoryId));

		if(parentCategoryId is null)
			return;
		var add = Query<CourseCategory>
			.WithId(parentCategoryId.Value)
			.WithUpdate(b => b.Push(nameof(CourseCategory.SubCategories), categoryId));
		await _categories.FindOneAndUpdateAsync(add, add);
	}
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