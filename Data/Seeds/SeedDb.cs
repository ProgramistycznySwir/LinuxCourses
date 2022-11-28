

using System.Runtime.InteropServices;
using System.Threading.Tasks;
using LinuxCourses.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LinuxCourses.Data.Seeding;


// TODO: Finish or throw out!
// REF: I suggest using repositories to manage data, as otherwise relations are not preserved!
public static class SeedDb
{
	public static async Task Seed(LinuxCoursesDatabaseSettings settings, bool deleteExistingData= false)
	{
		IMongoDb mongo = new MongoDb(Options.Create(settings));
		if(deleteExistingData)
			await mongo.DropCollectionAsync(settings.CourseCategories);
		SeedCourseCategories(mongo.Categories());
	}

	private static async Task DropCollectionAsync(this IMongoDb mongo, string name)
		=> await mongo.GetMongoDatabase().DropCollectionAsync(name);

	public static void SeedCourseCategories(IMongoCollection<CourseCategory> mongo)
	{
		var root = new CourseCategory[] {
			new() { Id= Guid.Empty, Name= "BRAK", Description= "Domyślna kategoria oznaczająca brak przypisania konkretnej kategorii" },
			new() { Id= Guid.NewGuid(), Name= "Linux", Description= "" },
		};
		mongo.InsertManyAsync(root);
	}
}