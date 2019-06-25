using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniRx
{
	public static class ObservableWWW
	{
		public static IObservable<string> Get(string url, Dictionary<string, string> headers = null, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<string> observer, CancellationToken cancellation) => FetchText(new WWW(url, (byte[])null, headers ?? new Dictionary<string, string>()), observer, progress, cancellation));
		}

		public static IObservable<byte[]> GetAndGetBytes(string url, Dictionary<string, string> headers = null, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<byte[]> observer, CancellationToken cancellation) => FetchBytes(new WWW(url, (byte[])null, headers ?? new Dictionary<string, string>()), observer, progress, cancellation));
		}

		public static IObservable<WWW> GetWWW(string url, Dictionary<string, string> headers = null, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<WWW> observer, CancellationToken cancellation) => Fetch(new WWW(url, (byte[])null, headers ?? new Dictionary<string, string>()), observer, progress, cancellation));
		}

		public static IObservable<string> Post(string url, byte[] postData, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<string> observer, CancellationToken cancellation) => FetchText(new WWW(url, postData), observer, progress, cancellation));
		}

		public static IObservable<string> Post(string url, byte[] postData, Dictionary<string, string> headers, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<string> observer, CancellationToken cancellation) => FetchText(new WWW(url, postData, headers), observer, progress, cancellation));
		}

		public static IObservable<string> Post(string url, WWWForm content, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<string> observer, CancellationToken cancellation) => FetchText(new WWW(url, content), observer, progress, cancellation));
		}

		public static IObservable<string> Post(string url, WWWForm content, Dictionary<string, string> headers, IProgress<float> progress = null)
		{
			Dictionary<string, string> contentHeaders = content.headers;
			return Observable.FromCoroutine((IObserver<string> observer, CancellationToken cancellation) => FetchText(new WWW(url, content.data, MergeHash(contentHeaders, headers)), observer, progress, cancellation));
		}

		public static IObservable<byte[]> PostAndGetBytes(string url, byte[] postData, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<byte[]> observer, CancellationToken cancellation) => FetchBytes(new WWW(url, postData), observer, progress, cancellation));
		}

		public static IObservable<byte[]> PostAndGetBytes(string url, byte[] postData, Dictionary<string, string> headers, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<byte[]> observer, CancellationToken cancellation) => FetchBytes(new WWW(url, postData, headers), observer, progress, cancellation));
		}

		public static IObservable<byte[]> PostAndGetBytes(string url, WWWForm content, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<byte[]> observer, CancellationToken cancellation) => FetchBytes(new WWW(url, content), observer, progress, cancellation));
		}

		public static IObservable<byte[]> PostAndGetBytes(string url, WWWForm content, Dictionary<string, string> headers, IProgress<float> progress = null)
		{
			Dictionary<string, string> contentHeaders = content.headers;
			return Observable.FromCoroutine((IObserver<byte[]> observer, CancellationToken cancellation) => FetchBytes(new WWW(url, content.data, MergeHash(contentHeaders, headers)), observer, progress, cancellation));
		}

		public static IObservable<WWW> PostWWW(string url, byte[] postData, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<WWW> observer, CancellationToken cancellation) => Fetch(new WWW(url, postData), observer, progress, cancellation));
		}

		public static IObservable<WWW> PostWWW(string url, byte[] postData, Dictionary<string, string> headers, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<WWW> observer, CancellationToken cancellation) => Fetch(new WWW(url, postData, headers), observer, progress, cancellation));
		}

		public static IObservable<WWW> PostWWW(string url, WWWForm content, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<WWW> observer, CancellationToken cancellation) => Fetch(new WWW(url, content), observer, progress, cancellation));
		}

		public static IObservable<WWW> PostWWW(string url, WWWForm content, Dictionary<string, string> headers, IProgress<float> progress = null)
		{
			Dictionary<string, string> contentHeaders = content.headers;
			return Observable.FromCoroutine((IObserver<WWW> observer, CancellationToken cancellation) => Fetch(new WWW(url, content.data, MergeHash(contentHeaders, headers)), observer, progress, cancellation));
		}

		public static IObservable<AssetBundle> LoadFromCacheOrDownload(string url, int version, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<AssetBundle> observer, CancellationToken cancellation) => FetchAssetBundle(WWW.LoadFromCacheOrDownload(url, version), observer, progress, cancellation));
		}

		public static IObservable<AssetBundle> LoadFromCacheOrDownload(string url, int version, uint crc, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<AssetBundle> observer, CancellationToken cancellation) => FetchAssetBundle(WWW.LoadFromCacheOrDownload(url, version, crc), observer, progress, cancellation));
		}

		public static IObservable<AssetBundle> LoadFromCacheOrDownload(string url, Hash128 hash128, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<AssetBundle> observer, CancellationToken cancellation) => FetchAssetBundle(WWW.LoadFromCacheOrDownload(url, hash128), observer, progress, cancellation));
		}

		public static IObservable<AssetBundle> LoadFromCacheOrDownload(string url, Hash128 hash128, uint crc, IProgress<float> progress = null)
		{
			return Observable.FromCoroutine((IObserver<AssetBundle> observer, CancellationToken cancellation) => FetchAssetBundle(WWW.LoadFromCacheOrDownload(url, hash128, crc), observer, progress, cancellation));
		}

		private static Dictionary<string, string> MergeHash(Dictionary<string, string> wwwFormHeaders, Dictionary<string, string> externalHeaders)
		{
			foreach (KeyValuePair<string, string> externalHeader in externalHeaders)
			{
				wwwFormHeaders[externalHeader.Key] = externalHeader.Value;
			}
			return wwwFormHeaders;
		}

		private static IEnumerator Fetch(WWW www, IObserver<WWW> observer, IProgress<float> reportProgress, CancellationToken cancel)
		{
			try
			{
				while (!www.isDone && !cancel.IsCancellationRequested)
				{
					if (reportProgress != null)
					{
						try
						{
							reportProgress.Report(www.progress);
						}
						catch (Exception ex3)
						{
							Exception ex2 = ex3;
							observer.OnError(ex2);
							yield break;
						}
					}
					yield return null;
				}
				if (!cancel.IsCancellationRequested)
				{
					if (reportProgress != null)
					{
						try
						{
							reportProgress.Report(www.progress);
						}
						catch (Exception ex4)
						{
							Exception ex = ex4;
							observer.OnError(ex);
							yield break;
						}
					}
					if (!string.IsNullOrEmpty(www.error))
					{
						observer.OnError(new WWWErrorException(www));
					}
					else
					{
						observer.OnNext(www);
						observer.OnCompleted();
					}
				}
			}
			finally
			{
				((IDisposable)www)?.Dispose();
			}
		}

		private static IEnumerator FetchText(WWW www, IObserver<string> observer, IProgress<float> reportProgress, CancellationToken cancel)
		{
			try
			{
				while (!www.isDone && !cancel.IsCancellationRequested)
				{
					if (reportProgress != null)
					{
						try
						{
							reportProgress.Report(www.progress);
						}
						catch (Exception ex3)
						{
							Exception ex2 = ex3;
							observer.OnError(ex2);
							yield break;
						}
					}
					yield return null;
				}
				if (!cancel.IsCancellationRequested)
				{
					if (reportProgress != null)
					{
						try
						{
							reportProgress.Report(www.progress);
						}
						catch (Exception ex4)
						{
							Exception ex = ex4;
							observer.OnError(ex);
							yield break;
						}
					}
					if (!string.IsNullOrEmpty(www.error))
					{
						observer.OnError(new WWWErrorException(www));
					}
					else
					{
						observer.OnNext(www.text);
						observer.OnCompleted();
					}
				}
			}
			finally
			{
				((IDisposable)www)?.Dispose();
			}
		}

		private static IEnumerator FetchBytes(WWW www, IObserver<byte[]> observer, IProgress<float> reportProgress, CancellationToken cancel)
		{
			try
			{
				while (!www.isDone && !cancel.IsCancellationRequested)
				{
					if (reportProgress != null)
					{
						try
						{
							reportProgress.Report(www.progress);
						}
						catch (Exception ex3)
						{
							Exception ex2 = ex3;
							observer.OnError(ex2);
							yield break;
						}
					}
					yield return null;
				}
				if (!cancel.IsCancellationRequested)
				{
					if (reportProgress != null)
					{
						try
						{
							reportProgress.Report(www.progress);
						}
						catch (Exception ex4)
						{
							Exception ex = ex4;
							observer.OnError(ex);
							yield break;
						}
					}
					if (!string.IsNullOrEmpty(www.error))
					{
						observer.OnError(new WWWErrorException(www));
					}
					else
					{
						observer.OnNext(www.bytes);
						observer.OnCompleted();
					}
				}
			}
			finally
			{
				((IDisposable)www)?.Dispose();
			}
		}

		private static IEnumerator FetchAssetBundle(WWW www, IObserver<AssetBundle> observer, IProgress<float> reportProgress, CancellationToken cancel)
		{
			try
			{
				while (!www.isDone && !cancel.IsCancellationRequested)
				{
					if (reportProgress != null)
					{
						try
						{
							reportProgress.Report(www.progress);
						}
						catch (Exception ex3)
						{
							Exception ex2 = ex3;
							observer.OnError(ex2);
							yield break;
						}
					}
					yield return null;
				}
				if (!cancel.IsCancellationRequested)
				{
					if (reportProgress != null)
					{
						try
						{
							reportProgress.Report(www.progress);
						}
						catch (Exception ex4)
						{
							Exception ex = ex4;
							observer.OnError(ex);
							yield break;
						}
					}
					if (!string.IsNullOrEmpty(www.error))
					{
						observer.OnError(new WWWErrorException(www));
					}
					else
					{
						observer.OnNext(www.assetBundle);
						observer.OnCompleted();
					}
				}
			}
			finally
			{
				((IDisposable)www)?.Dispose();
			}
		}
	}
}
