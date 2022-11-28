

using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;

namespace LinuxCourses.Models;

public class CourseCategory : IHasId<Guid>
{
	[BsonId]
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }

	// INFO: Self is organized in tree-structure.
	public Guid? Parent { get; set; }
	// TODO: Create index for this!
	public ICollection<Guid> SubCategories { get; set; }
	public ICollection<Guid> Courses { get; set; }
}