using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace UnityEngine.Experimental.Networking
{
	[StructLayout(LayoutKind.Sequential)]
	public sealed class UnityWebRequest : IDisposable
	{
		internal enum UnityWebRequestMethod
		{
			Get,
			Post,
			Put,
			Head,
			Custom
		}

		internal enum UnityWebRequestError
		{
			OK,
			Unknown,
			SDKError,
			UnsupportedProtocol,
			MalformattedUrl,
			CannotResolveProxy,
			CannotResolveHost,
			CannotConnectToHost,
			AccessDenied,
			GenericHTTPError,
			WriteError,
			ReadError,
			OutOfMemory,
			Timeout,
			HTTPPostError,
			SSLCannotConnect,
			Aborted,
			TooManyRedirects,
			ReceivedNoData,
			SSLNotSupported,
			FailedToSendData,
			FailedToReceiveData,
			SSLCertificateError,
			SSLCipherNotAvailable,
			SSLCACertError,
			UnrecognizedContentEncoding,
			LoginFailed,
			SSLShutdownFailed
		}

		public const string kHttpVerbGET = "GET";

		public const string kHttpVerbHEAD = "HEAD";

		public const string kHttpVerbPOST = "POST";

		public const string kHttpVerbPUT = "PUT";

		public const string kHttpVerbCREATE = "CREATE";

		public const string kHttpVerbDELETE = "DELETE";

		[NonSerialized]
		internal IntPtr m_Ptr;

		private static readonly string[] forbiddenHeaderKeys = new string[22]
		{
			"accept-charset",
			"accept-encoding",
			"access-control-request-headers",
			"access-control-request-method",
			"connection",
			"content-length",
			"cookie",
			"cookie2",
			"date",
			"dnt",
			"expect",
			"host",
			"keep-alive",
			"origin",
			"referer",
			"te",
			"trailer",
			"transfer-encoding",
			"upgrade",
			"user-agent",
			"via",
			"x-unity-version"
		};

		public bool disposeDownloadHandlerOnDispose
		{
			get;
			set;
		}

		public bool disposeUploadHandlerOnDispose
		{
			get;
			set;
		}

		public static byte[] SerializeFormSections(List<IMultipartFormSection> multipartFormSections, byte[] boundary)
		{
			byte[] bytes = Encoding.UTF8.GetBytes("\r\n");
			int num = 0;
			foreach (IMultipartFormSection multipartFormSection in multipartFormSections)
			{
				num += 64 + multipartFormSection.sectionData.Length;
			}
			List<byte> list = new List<byte>(num);
			foreach (IMultipartFormSection multipartFormSection2 in multipartFormSections)
			{
				string str = "form-data";
				string sectionName = multipartFormSection2.sectionName;
				string fileName = multipartFormSection2.fileName;
				if (!string.IsNullOrEmpty(fileName))
				{
					str = "file";
				}
				string str2 = "Content-Disposition: " + str;
				if (!string.IsNullOrEmpty(sectionName))
				{
					str2 = str2 + "; name=\"" + sectionName + "\"";
				}
				if (!string.IsNullOrEmpty(fileName))
				{
					str2 = str2 + "; filename=\"" + fileName + "\"";
				}
				str2 += "\r\n";
				string contentType = multipartFormSection2.contentType;
				if (!string.IsNullOrEmpty(contentType))
				{
					str2 = str2 + "Content-Type: " + contentType + "\r\n";
				}
				list.AddRange(boundary);
				list.AddRange(bytes);
				list.AddRange(Encoding.UTF8.GetBytes(str2));
				list.AddRange(bytes);
				list.AddRange(multipartFormSection2.sectionData);
			}
			list.TrimExcess();
			return list.ToArray();
		}

		public static byte[] GenerateBoundary()
		{
			byte[] array = new byte[40];
			for (int i = 0; i < 40; i++)
			{
				int num = Random.Range(48, 110);
				if (num > 57)
				{
					num += 7;
				}
				if (num > 90)
				{
					num += 6;
				}
				array[i] = (byte)num;
			}
			return array;
		}

		public static byte[] SerializeSimpleForm(Dictionary<string, string> formFields)
		{
			string text = string.Empty;
			foreach (KeyValuePair<string, string> formField in formFields)
			{
				if (text.Length > 0)
				{
					text += "&";
				}
				text = text + Uri.EscapeDataString(formField.Key) + "=" + Uri.EscapeDataString(formField.Value);
			}
			return Encoding.UTF8.GetBytes(text);
		}

		internal void InternalDestroy() { throw new NotImplementedException("‚È‚É‚±‚ê"); }

		private void InternalSetDefaults()
		{
			disposeDownloadHandlerOnDispose = true;
			disposeUploadHandlerOnDispose = true;
		}

		~UnityWebRequest()
		{
			InternalDestroy();
		}

		public void Dispose()
		{
		}

		private static bool ContainsForbiddenCharacters(string s, int firstAllowedCharCode)
		{
			foreach (char c in s)
			{
				if (c < firstAllowedCharCode || c == '\u007f')
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsHeaderNameLegal(string headerName)
		{
			if (string.IsNullOrEmpty(headerName))
			{
				return false;
			}
			headerName = headerName.ToLower();
			if (ContainsForbiddenCharacters(headerName, 33))
			{
				return false;
			}
			if (headerName.StartsWith("sec-") || headerName.StartsWith("proxy-"))
			{
				return false;
			}
			string[] array = forbiddenHeaderKeys;
			foreach (string b in array)
			{
				if (string.Equals(headerName, b))
				{
					return false;
				}
			}
			return true;
		}

		private static bool IsHeaderValueLegal(string headerValue)
		{
			if (string.IsNullOrEmpty(headerValue))
			{
				return false;
			}
			if (ContainsForbiddenCharacters(headerValue, 32))
			{
				return false;
			}
			return true;
		}

		private static string GetErrorDescription(UnityWebRequestError errorCode)
		{
			switch (errorCode)
			{
			case UnityWebRequestError.OK:
				return "No Error";
			case UnityWebRequestError.SDKError:
				return "Internal Error With Transport Layer";
			case UnityWebRequestError.UnsupportedProtocol:
				return "Specified Transport Protocol is Unsupported";
			case UnityWebRequestError.MalformattedUrl:
				return "URL is Malformatted";
			case UnityWebRequestError.CannotResolveProxy:
				return "Unable to resolve specified proxy server";
			case UnityWebRequestError.CannotResolveHost:
				return "Unable to resolve host specified in URL";
			case UnityWebRequestError.CannotConnectToHost:
				return "Unable to connect to host specified in URL";
			case UnityWebRequestError.AccessDenied:
				return "Remote server denied access to the specified URL";
			case UnityWebRequestError.GenericHTTPError:
				return "Unknown/Generic HTTP Error - Check HTTP Error code";
			case UnityWebRequestError.WriteError:
				return "Error when transmitting request to remote server - transmission terminated prematurely";
			case UnityWebRequestError.ReadError:
				return "Error when reading response from remote server - transmission terminated prematurely";
			case UnityWebRequestError.OutOfMemory:
				return "Out of Memory";
			case UnityWebRequestError.Timeout:
				return "Timeout occurred while waiting for response from remote server";
			case UnityWebRequestError.HTTPPostError:
				return "Error while transmitting HTTP POST body data";
			case UnityWebRequestError.SSLCannotConnect:
				return "Unable to connect to SSL server at remote host";
			case UnityWebRequestError.Aborted:
				return "Request was manually aborted by local code";
			case UnityWebRequestError.TooManyRedirects:
				return "Redirect limit exceeded";
			case UnityWebRequestError.ReceivedNoData:
				return "Received an empty response from remote host";
			case UnityWebRequestError.SSLNotSupported:
				return "SSL connections are not supported on the local machine";
			case UnityWebRequestError.FailedToSendData:
				return "Failed to transmit body data";
			case UnityWebRequestError.FailedToReceiveData:
				return "Failed to receive response body data";
			case UnityWebRequestError.SSLCertificateError:
				return "Failure to authenticate SSL certificate of remote host";
			case UnityWebRequestError.SSLCipherNotAvailable:
				return "SSL cipher received from remote host is not supported on the local machine";
			case UnityWebRequestError.SSLCACertError:
				return "Failure to authenticate Certificate Authority of the SSL certificate received from the remote host";
			case UnityWebRequestError.UnrecognizedContentEncoding:
				return "Remote host returned data with an unrecognized/unparseable content encoding";
			case UnityWebRequestError.LoginFailed:
				return "HTTP authentication failed";
			case UnityWebRequestError.SSLShutdownFailed:
				return "Failure while shutting down SSL connection";
			default:
				return "Unknown error";
			}
		}
	}
}
