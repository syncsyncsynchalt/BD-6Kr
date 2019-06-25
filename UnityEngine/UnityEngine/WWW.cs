using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine.Internal;

namespace UnityEngine
{
	public sealed class WWW : IDisposable
	{
		internal IntPtr m_Ptr;

		public Dictionary<string, string> responseHeaders
		{
			get
			{
				if (!isDone)
				{
					throw new UnityException("WWW is not finished downloading yet");
				}
				return ParseHTTPHeaderString(responseHeadersString);
			}
		}

		private string responseHeadersString
		{
			get;
		}

		public string text
		{
			get
			{
				if (!isDone)
				{
					throw new UnityException("WWW is not ready downloading yet");
				}
				byte[] bytes = this.bytes;
				return GetTextEncoder().GetString(bytes, 0, bytes.Length);
			}
		}

		internal static Encoding DefaultEncoding => Encoding.ASCII;

		[Obsolete("Please use WWW.text instead")]
		public string data => text;

		public byte[] bytes
		{
			get;
		}

		public int size
		{
			get;
		}

		public string error
		{
			get;
		}

		public Texture2D texture => GetTexture(markNonReadable: false);

		public Texture2D textureNonReadable => GetTexture(markNonReadable: true);

		public AudioClip audioClip => GetAudioClip(threeD: true);

		public bool isDone
		{
			get;
		}

		public float progress
		{
			get;
		}

		public float uploadProgress
		{
			get;
		}

		public int bytesDownloaded
		{
			get;
		}

		[Obsolete("Property WWW.oggVorbis has been deprecated. Use WWW.audioClip instead (UnityUpgradable).", true)]
		public AudioClip oggVorbis => null;

		public string url
		{
			get;
		}

		public AssetBundle assetBundle
		{
			get;
		}

		public ThreadPriority threadPriority
		{
			get;
			set;
		}

		public WWW(string url)
		{
			InitWWW(url, null, null);
		}

		public WWW(string url, WWWForm form)
		{
			string[] iHeaders = FlattenedHeadersFrom(form.headers);
			InitWWW(url, form.data, iHeaders);
		}

		public WWW(string url, byte[] postData)
		{
			InitWWW(url, postData, null);
		}

		[Obsolete("This overload is deprecated. Use UnityEngine.WWW.WWW(string, byte[], System.Collections.Generic.Dictionary<string, string>) instead.", true)]
		public WWW(string url, byte[] postData, Hashtable headers)
		{
			Debug.LogError("This overload is deprecated. Use UnityEngine.WWW.WWW(string, byte[], System.Collections.Generic.Dictionary<string, string>) instead");
		}

		public WWW(string url, byte[] postData, Dictionary<string, string> headers)
		{
			string[] iHeaders = FlattenedHeadersFrom(headers);
			InitWWW(url, postData, iHeaders);
		}

		internal WWW(string url, Hash128 hash, uint crc)
		{
			INTERNAL_CALL_WWW(this, url, ref hash, crc);
		}

		public void Dispose()
		{
			DestroyWWW(cancel: true);
		}

		~WWW()
		{
			DestroyWWW(cancel: false);
		}

		private void DestroyWWW(bool cancel) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void InitWWW(string url, byte[] postData, string[] iHeaders) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		internal bool enforceWebSecurityRestrictions() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static string EscapeURL(string s)
		{
			Encoding uTF = Encoding.UTF8;
			return EscapeURL(s, uTF);
		}

		public static string EscapeURL(string s, [DefaultValue("System.Text.Encoding.UTF8")] Encoding e)
		{
			if (s == null)
			{
				return null;
			}
			if (s == string.Empty)
			{
				return string.Empty;
			}
			if (e == null)
			{
				return null;
			}
			return WWWTranscoder.URLEncode(s, e);
		}

		[ExcludeFromDocs]
		public static string UnEscapeURL(string s)
		{
			Encoding uTF = Encoding.UTF8;
			return UnEscapeURL(s, uTF);
		}

		public static string UnEscapeURL(string s, [DefaultValue("System.Text.Encoding.UTF8")] Encoding e)
		{
			if (s == null)
			{
				return null;
			}
			if (s.IndexOf('%') == -1 && s.IndexOf('+') == -1)
			{
				return s;
			}
			return WWWTranscoder.URLDecode(s, e);
		}

		private Encoding GetTextEncoder()
		{
			string value = null;
			if (responseHeaders.TryGetValue("CONTENT-TYPE", out value))
			{
				int num = value.IndexOf("charset", StringComparison.OrdinalIgnoreCase);
				if (num > -1)
				{
					int num2 = value.IndexOf('=', num);
					if (num2 > -1)
					{
						string text = value.Substring(num2 + 1).Trim().Trim('\'', '"')
							.Trim();
						int num3 = text.IndexOf(';');
						if (num3 > -1)
						{
							text = text.Substring(0, num3);
						}
						try
						{
							return Encoding.GetEncoding(text);
						}
						catch (Exception)
						{
							Debug.Log("Unsupported encoding: '" + text + "'");
						}
					}
				}
			}
			return Encoding.UTF8;
		}

		private Texture2D GetTexture(bool markNonReadable) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public AudioClip GetAudioClip(bool threeD)
		{
			return GetAudioClip(threeD, stream: false);
		}

		public AudioClip GetAudioClip(bool threeD, bool stream)
		{
			return GetAudioClip(threeD, stream, AudioType.UNKNOWN);
		}

		public AudioClip GetAudioClip(bool threeD, bool stream, AudioType audioType)
		{
			return GetAudioClipInternal(threeD, stream, compressed: false, audioType);
		}

		public AudioClip GetAudioClipCompressed()
		{
			return GetAudioClipCompressed(threeD: true);
		}

		public AudioClip GetAudioClipCompressed(bool threeD)
		{
			return GetAudioClipCompressed(threeD, AudioType.UNKNOWN);
		}

		public AudioClip GetAudioClipCompressed(bool threeD, AudioType audioType)
		{
			return GetAudioClipInternal(threeD, stream: false, compressed: true, audioType);
		}

		private AudioClip GetAudioClipInternal(bool threeD, bool stream, bool compressed, AudioType audioType) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		public void LoadImageIntoTexture(Texture2D tex) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("All blocking WWW functions have been deprecated, please use one of the asynchronous functions instead.", true)]
		public static string GetURL(string url) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[Obsolete("All blocking WWW functions have been deprecated, please use one of the asynchronous functions instead.", true)]
		public static Texture2D GetTextureFromURL(string url)
		{
			return new WWW(url).texture;
		}

		[Obsolete("LoadUnityWeb is no longer supported. Please use javascript to reload the web player on a different url instead", true)]
		public void LoadUnityWeb()
		{
		}

		private static void INTERNAL_CALL_WWW(WWW self, string url, ref Hash128 hash, uint crc) { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		[ExcludeFromDocs]
		public static WWW LoadFromCacheOrDownload(string url, int version)
		{
			uint crc = 0u;
			return LoadFromCacheOrDownload(url, version, crc);
		}

		public static WWW LoadFromCacheOrDownload(string url, int version, [DefaultValue("0")] uint crc)
		{
			Hash128 hash = new Hash128(0u, 0u, 0u, (uint)version);
			return LoadFromCacheOrDownload(url, hash, crc);
		}

		[ExcludeFromDocs]
		public static WWW LoadFromCacheOrDownload(string url, Hash128 hash)
		{
			uint crc = 0u;
			return LoadFromCacheOrDownload(url, hash, crc);
		}

		public static WWW LoadFromCacheOrDownload(string url, Hash128 hash, [DefaultValue("0")] uint crc)
		{
			return new WWW(url, hash, crc);
		}

		private static string[] FlattenedHeadersFrom(Dictionary<string, string> headers)
		{
			if (headers == null)
			{
				return null;
			}
			string[] array = new string[headers.Count * 2];
			int num = 0;
			foreach (KeyValuePair<string, string> header in headers)
			{
				array[num++] = header.Key.ToString();
				array[num++] = header.Value.ToString();
			}
			return array;
		}

		internal static Dictionary<string, string> ParseHTTPHeaderString(string input)
		{
			if (input == null)
			{
				throw new ArgumentException("input was null to ParseHTTPHeaderString");
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			StringReader stringReader = new StringReader(input);
			int num = 0;
			while (true)
			{
				string text = stringReader.ReadLine();
				if (text == null)
				{
					break;
				}
				if (num++ == 0 && text.StartsWith("HTTP"))
				{
					dictionary["STATUS"] = text;
					continue;
				}
				int num3 = text.IndexOf(": ");
				if (num3 != -1)
				{
					string key = text.Substring(0, num3).ToUpper();
					string text3 = dictionary[key] = text.Substring(num3 + 2);
				}
			}
			return dictionary;
		}
	}
}
