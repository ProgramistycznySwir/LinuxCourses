using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;

namespace LinuxCourses.Models;

public class User{
	[BsonId]
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
}