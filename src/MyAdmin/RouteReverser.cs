using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MyAdmin.Admin;

public partial class RouteReverser
{
	private readonly IEnumerable<EndpointDataSource> _endpointSources;
	private readonly Dictionary<string, string> _urls = new Dictionary<string, string>();

	private string GetPathFromDisplayName(string displayName)
	{
		return displayName.Split(" ")[2];
	}

	public RouteReverser(IEnumerable<EndpointDataSource> endpointSources)
	{
		_endpointSources = endpointSources;

		IEnumerable<Endpoint> endpoints = _endpointSources.SelectMany(e => e.Endpoints).OfType<RouteEndpoint>();
		foreach (Endpoint endpoint in endpoints)
		{
			RouteNameMetadata? metadata = endpoint.Metadata.OfType<RouteNameMetadata>().FirstOrDefault();
			if (metadata != null && metadata.RouteName != null)
			{
				_urls.Add(metadata.RouteName, GetPathFromDisplayName(endpoint.DisplayName!));
			}
		}
	}

	public string Reverse(string name, object? parameters = null)
	{
		string path;

		bool exists = _urls.TryGetValue(name, out path!);

		if (!exists)
		{
			throw new KeyNotFoundException();
		}

		int paramCount = SearchParameterRegex().Count(path);

		if (parameters == null)
		{
			if (paramCount > 0)
				throw new InvalidOperationException("Missing url parameter(s).");
		}
		else
		{
			if (paramCount > 0)
			{
				PropertyInfo[] props = parameters.GetType().GetProperties();
				foreach (PropertyInfo prop in props)
				{
					object? val = prop.GetValue(parameters, null);
					if (val != null)
					{
						path = Regex.Replace(path, "{" + prop.Name + "}", val.ToString()!);
					}
				}
			}
		}

		return path;
	}

	[GeneratedRegex("{.*?}")]
	private static partial Regex SearchParameterRegex();
}
