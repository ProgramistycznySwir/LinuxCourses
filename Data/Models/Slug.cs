
using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace LinuxCourses;

// TODO: Use slugs instead of raw Guids at api layer.
/// <summary>
/// Helper class for interop between strings and Guids
/// </summary>
public struct Slug : IEquatable<Slug>
{
	public string Text { get; init; } = string.Empty;

	public Slug() {}
	public Slug(string slug)
		=> this.Text = slug;
	public Slug(Guid guid)
		=> this.Text = Slug_.ToSlug(guid);

	public static implicit operator Slug(Guid guid)
		=> new(guid.ToSlug());
	public static implicit operator Guid?(Slug slug)
		=> slug.Text.ToGuid();

	public override int GetHashCode() => this.Text.GetHashCode();
	public override string? ToString() => this.Text;
	public bool Equals(Slug other) => this.Text == other.Text;
}

public static class Slug_
{
	public static string ToSlug(this Guid guid)
		=> Convert.ToBase64String(guid.ToByteArray()).TrimEnd('=').Replace('+', '-').Replace('/', '_');
	public static Guid? ToGuid(this string slug)
	{
		Span<byte> bytes = stackalloc byte[16];
		if(Convert.TryFromBase64String(slug.Replace('_', '/').Replace('-', '+').PadRight(24, '='), bytes, out _) is false)
			return null;
		return bytes.Length is 16
			? new(bytes)
			: null;
	}
}