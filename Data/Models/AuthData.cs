

using MongoDB.Bson.Serialization.Attributes;

namespace LinuxCourses.Models;

/// <summary>
/// [Owned] by User (but stored separately).<br/>
/// All the data that should under no circumstance leave API.
/// </summary>
public class AuthData
{
	[BsonId]
	public Guid User_Id { get; set; }
	/// <summary> Password hash encoded in base64 </summary>
	public string Password { get; set; }
}