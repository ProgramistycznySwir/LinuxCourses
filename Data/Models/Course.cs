namespace LinuxCourses.Models;

public record Course(
	Guid Id,
	string Name,
	string Description,
	ICollection<Guid> Quizes_Ids
);