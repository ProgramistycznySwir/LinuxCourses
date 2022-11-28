using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;

namespace LinuxCourses.Models;

public class Course : IHasId<Guid>
{
	[BsonId]
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }

	public Guid? CategoryId { get; set; }

	public ICollection<Guid> Quizes_Ids { get; set; }

	public ICollection<UserGroup> Groups { get; set; }
}