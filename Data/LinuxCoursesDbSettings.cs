namespace LinuxCourses.Data;

public class LinuxCoursesDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;

    public string Quizes { get; set; } = null!;
    public string Courses { get; set; } = null!;
    public string Users { get; set; } = null!;
    public string AuthData { get; set; } = null!;
    public string CourseCategories { get; set; } = null!;
}