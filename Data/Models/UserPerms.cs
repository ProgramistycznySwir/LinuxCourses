
namespace LinuxCourses.Models;

/// <summary>
/// Relations: Course -< Self, User--Self
/// </summary>
public class UserPerms
{
	public Guid User_Id { get; set; }
	public CoursePerms Perms { get; set; }
}

[Flags]
public enum CoursePerms {
	/// <summary> Can view content. </summary>
	View,
	/// <summary> Can solve quizes and upload solutions to tasks. </summary>
	Attend,
	/// <summary> Can invite users to course. </summary>
	Invite,
	/// <summary> Can add, modify and delete content from course. </summary>
	Modify,
	/// <summary> Can do all things with course. (This claim gives automatically all other claims) </summary>
	Admin
}

public static class CoursePerms_
{
	public static class Roles
	{
		public static CoursePerms Attendee => CoursePerms.View | CoursePerms.Attend;
		public static CoursePerms Moderator => Attendee | CoursePerms.Invite | CoursePerms.Modify;
		public static CoursePerms Admin => Moderator | CoursePerms.Admin;
	}
	// public static bool CanView(CoursePerms self)
	// 	=> self
}