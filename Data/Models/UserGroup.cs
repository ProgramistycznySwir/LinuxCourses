
namespace LinuxCourses.Models;

/// <summary>
/// [Owned] by Course
/// </summary>
public class UserGroup
{
	public string Name { get; set; }
	public List<UserPerms> Users { get; set; }
}