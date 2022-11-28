


using LinuxCourses.DTOs.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

namespace LinuxCourses;

public static class Prelude
{
	public static NotImplementedException TODO_(string message= null) => new NotImplementedException(message);
	public static NotImplementedException TODO => TODO_();
}