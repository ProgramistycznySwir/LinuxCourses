

using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace LinuxCourses.Models;

[CollectionName("Roles")]
public class AppRole : MongoIdentityRole<Guid>
{

	public const string CanCreateCourses = "CanCreateCourses";
	public const string CanCreateCourses_AndAbove = $"{CanCreateCourses}, {Admin}";
	public const string Admin = "Admin";
}