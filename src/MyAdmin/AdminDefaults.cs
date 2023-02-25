using Microsoft.AspNetCore.Http;

namespace MyAdmin.Admin;

/// <summary>
/// Default values related to cookie-based authentication
/// </summary>
public class AdminDefaults
{
	/// <summary>
	/// The default value used for CookieAuthenticationOptions.AuthenticationScheme
	/// </summary>
	public const string AuthenticationScheme = "MyAdmin__Cookies";

	/// <summary>
	/// The prefix used to provide a default CookieAuthenticationOptions.CookieName
	/// </summary>
	public static readonly string CookiePrefix = ".MyAdmin.";

	/// <summary>
	/// The default value used by CookieAuthenticationMiddleware for the
	/// CookieAuthenticationOptions.LoginPath
	/// </summary>
	public static readonly PathString LoginPath = new PathString("/Account/Login");

	/// <summary>
	/// The default value used by CookieAuthenticationMiddleware for the
	/// CookieAuthenticationOptions.LogoutPath
	/// </summary>
	public static readonly PathString LogoutPath = new PathString("/Account/Logout");

	/// <summary>
	/// The default value used by CookieAuthenticationMiddleware for the
	/// CookieAuthenticationOptions.AccessDeniedPath
	/// </summary>
	public static readonly PathString AccessDeniedPath = new PathString("/Account/AccessDenied");
}
