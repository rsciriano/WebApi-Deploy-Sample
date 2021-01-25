using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace Microsoft.AspNetCore.TestHost
{
	public static class RequestBuilderExtensions
	{
		public static RequestBuilder WithJsonContent(this RequestBuilder requestBuilder, object content)
		{
			requestBuilder.And((Action<HttpRequestMessage>)delegate (HttpRequestMessage x)
			{
				x.Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
			});
			return requestBuilder;
		}
	}
}