using LinuxCourses.Models;

namespace LinuxCourses.Features.Auth;


[AttributeUsage(
		AttributeTargets.Class |
		AttributeTargets.Struct |
		AttributeTargets.Method
	)
]
public class NeedPermsAttribute : Attribute  
{
    public CoursePerms perms;

    public NeedPermsAttribute(CoursePerms perms = CoursePerms.Admin)  
    {
        this.perms = perms;
    }
} 