using System;

namespace Contentful.Essential.Sample.Http
{
	public interface IHttpClient : IDisposable
	{
		T Get<T>(string path, object query = null, Action<Exception> onError = null) where T : class;
		T Post<T>(string path, object query = null, Action<Exception> onError = null) where T : class;
	}
}
