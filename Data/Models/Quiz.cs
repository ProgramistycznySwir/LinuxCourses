using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LinuxCourses.Models;

public class Quiz
{
	[BsonId]
    // [BsonRepresentation(BsonType.UUID)]
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string Description { get; set; }
	public List<Guid> Quizes_Ids { get; set; } = new();
}