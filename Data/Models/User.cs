using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace LinuxCourses.Models;

[CollectionName("Users")]
public class User : MongoIdentityUser<Guid>
{
	// [BsonId]
	// public Guid Id { get; set; }
	// public string Name { get; set; }
	public string Description { get; set; }
}

public static class A{
	public static void B()
	{
		var usr = new User();
		// usr.

	}
}