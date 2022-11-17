namespace LinuxCourses.Data;

public class LinuxCoursesDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;

    public string Quizes { get; set; } = null!;
    public string Courses { get; set; } = null!;
}