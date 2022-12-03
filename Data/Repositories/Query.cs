

using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace LinuxCourses.Data.Services;

public record struct MongoQuery<T>(FilterDefinition<T> Filter)
{
	public UpdateDefinition<T>? Update { get; set; } = null;
	public ProjectionDefinition<T>? Projection { get; set; } = null;

	public MongoQuery<T> WithUpdate(Func<UpdateDefinitionBuilder<T>, UpdateDefinition<T>> builder)
	{
		Update= builder(Builders<T>.Update);
		return this;
	}

	/// <summary> Beware, it overrides the projection. </summary>
	public MongoQuery<T> WithProjection(Func<ProjectionDefinitionBuilder<T>, ProjectionDefinition<T>> builder)
	{
		Projection= builder(Builders<T>.Projection);
		return this;
	}
	// TODO LOW: I dislike like this type-erasure.
	public MongoQuery<T> Include(Expression<Func<T, object>> field)
	{
		if(Projection is null)
			Projection = Builders<T>.Projection.Include(field);
		else
			Projection.Include(field);
		return this;
	}

	public static implicit operator FilterDefinition<T>(MongoQuery<T> self)
		=> self.Filter;
	public static implicit operator UpdateDefinition<T>(MongoQuery<T> self)
		=> self.Update;
	public static implicit operator ProjectionDefinition<T>(MongoQuery<T> self)
		=> self.Projection;
}

public static class Query<T> where T : IHasId<Guid>
{
	public static MongoQuery<T> WithFilter(Func<FilterDefinitionBuilder<T>, FilterDefinition<T>> builder)
		=> new(builder(Builders<T>.Filter));
	public static MongoQuery<T> WithId(Guid id)
		=> new(Builders<T>.Filter.Eq(nameof(IHasId<Guid>.Id), id));
	public static MongoQuery<T> WithEq<TField>(Expression<Func<T, TField>> field, TField value)
		=> new(Builders<T>.Filter.Eq(field, value));
}

public static class Query<T, T_Id> where T : IHasId<T_Id>
{
	public static MongoQuery<T> WithId(T_Id id)
		=> new(Builders<T>.Filter.Eq(nameof(IHasId<T_Id>.Id), id));
}