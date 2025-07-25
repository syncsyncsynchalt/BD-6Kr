using System;
using System.Text;

namespace UnityEngine.Experimental.Networking;

public class MultipartFormDataSection : IMultipartFormSection
{
	private string name;

	private byte[] data;

	private string content;

	public string sectionName => name;

	public byte[] sectionData => data;

	public string fileName => null;

	public string contentType => content;

	public MultipartFormDataSection(string name, byte[] data, string contentType)
	{
		if (data == null || data.Length < 1)
		{
			throw new ArgumentException("Cannot create a multipart form data section without body data");
		}
		this.name = name;
		this.data = data;
		content = contentType;
	}

	public MultipartFormDataSection(string name, byte[] data)
		: this(name, data, null)
	{
	}

	public MultipartFormDataSection(byte[] data)
		: this(null, data)
	{
	}

	public MultipartFormDataSection(string name, string data, Encoding encoding, string contentType)
	{
		if (data == null || data.Length < 1)
		{
			throw new ArgumentException("Cannot create a multipart form data section without body data");
		}
		byte[] bytes = encoding.GetBytes(data);
		this.name = name;
		this.data = bytes;
		if (contentType != null && !contentType.Contains("encoding="))
		{
			contentType = contentType.Trim() + "; encoding=" + encoding.WebName;
		}
		content = contentType;
	}

	public MultipartFormDataSection(string name, string data, string contentType)
		: this(name, data, Encoding.UTF8, contentType)
	{
	}

	public MultipartFormDataSection(string name, string data)
		: this(name, data, "text/plain")
	{
	}

	public MultipartFormDataSection(string data)
		: this(null, data)
	{
	}
}
