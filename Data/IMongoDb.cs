

using LinuxCourses.Data;
using LinuxCourses.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LinuxCourses.Data;

public interface IMongoDb
{
	IMongoCollection<Quiz> Quizes();
	IMongoCollection<Course> Courses();
	IMongoCollection<User> Users();
	IMongoCollection<AuthData> AuthData();
}

public class MongoDb : IMongoDb
{
	private readonly LinuxCoursesDatabaseSettings _settings;
	private readonly IMongoDatabase _mongo;

	public MongoDb(
		IOptions<LinuxCoursesDatabaseSettings> dbSettingsOption)
	{
		if(dbSettingsOption is null)
			throw new ArgumentException("Error when configuring MongoDb: Cannot read settings!");
		_settings = dbSettingsOption.Value;
		var mongoClient_ = new MongoClient(_settings.ConnectionString);
		_mongo = mongoClient_.GetDatabase(_settings.DatabaseName);
	}

	public IMongoCollection<Quiz> Quizes()
		=> _mongo.GetCollection<Quiz>(_settings.Quizes);

	public IMongoCollection<Course> Courses()
		=> _mongo.GetCollection<Course>(_settings.Courses);

	public IMongoCollection<User> Users()
		=> _mongo.GetCollection<User>(_settings.Users);

	public IMongoCollection<AuthData> AuthData()
		=> _mongo.GetCollection<AuthData>(_settings.AuthData);
}
