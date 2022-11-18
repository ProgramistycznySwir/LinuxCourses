

using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace LinuxCourses.Models;

[CollectionName("Roles")]
public class AppRole : MongoIdentityRole<Guid>
{
}