using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJson;
using UnityEngine.Networking.Types;

namespace UnityEngine.Networking.Match;

public class NetworkMatch : MonoBehaviour
{
	public delegate void ResponseDelegate<T>(T response);

	private const string kMultiplayerNetworkingIdKey = "CloudNetworkingId";

	private Uri m_BaseUri = new Uri("https://mm.unet.unity3d.com");

	public Uri baseUri
	{
		get
		{
			return m_BaseUri;
		}
		set
		{
			m_BaseUri = value;
		}
	}

	public void SetProgramAppID(AppID programAppID)
	{
		Utility.SetAppID(programAppID);
	}

	public Coroutine CreateMatch(string matchName, uint matchSize, bool matchAdvertise, string matchPassword, ResponseDelegate<CreateMatchResponse> callback)
	{
		return CreateMatch(new CreateMatchRequest
		{
			name = matchName,
			size = matchSize,
			advertise = matchAdvertise,
			password = matchPassword
		}, callback);
	}

	public Coroutine CreateMatch(CreateMatchRequest req, ResponseDelegate<CreateMatchResponse> callback)
	{
		Uri uri = new Uri(baseUri, "/json/reply/CreateMatchRequest");
		Debug.Log("MatchMakingClient Create :" + uri);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("projectId", Application.cloudProjectId);
		wWWForm.AddField("sourceId", Utility.GetSourceID().ToString());
		wWWForm.AddField("appId", Utility.GetAppID().ToString());
		wWWForm.AddField("accessTokenString", 0);
		wWWForm.AddField("domain", 0);
		wWWForm.AddField("name", req.name);
		wWWForm.AddField("size", req.size.ToString());
		wWWForm.AddField("advertise", req.advertise.ToString());
		wWWForm.AddField("password", req.password);
		wWWForm.headers["Accept"] = "application/json";
		WWW client = new WWW(uri.ToString(), wWWForm);
		return StartCoroutine(ProcessMatchResponse(client, callback));
	}

	public Coroutine JoinMatch(NetworkID netId, string matchPassword, ResponseDelegate<JoinMatchResponse> callback)
	{
		return JoinMatch(new JoinMatchRequest
		{
			networkId = netId,
			password = matchPassword
		}, callback);
	}

	public Coroutine JoinMatch(JoinMatchRequest req, ResponseDelegate<JoinMatchResponse> callback)
	{
		Uri uri = new Uri(baseUri, "/json/reply/JoinMatchRequest");
		Debug.Log("MatchMakingClient Join :" + uri);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("projectId", Application.cloudProjectId);
		wWWForm.AddField("sourceId", Utility.GetSourceID().ToString());
		wWWForm.AddField("appId", Utility.GetAppID().ToString());
		wWWForm.AddField("accessTokenString", 0);
		wWWForm.AddField("domain", 0);
		wWWForm.AddField("networkId", req.networkId.ToString());
		wWWForm.AddField("password", req.password);
		wWWForm.headers["Accept"] = "application/json";
		WWW client = new WWW(uri.ToString(), wWWForm);
		return StartCoroutine(ProcessMatchResponse(client, callback));
	}

	public Coroutine DestroyMatch(NetworkID netId, ResponseDelegate<BasicResponse> callback)
	{
		return DestroyMatch(new DestroyMatchRequest
		{
			networkId = netId
		}, callback);
	}

	public Coroutine DestroyMatch(DestroyMatchRequest req, ResponseDelegate<BasicResponse> callback)
	{
		Uri uri = new Uri(baseUri, "/json/reply/DestroyMatchRequest");
		Debug.Log("MatchMakingClient Destroy :" + uri.ToString());
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("projectId", Application.cloudProjectId);
		wWWForm.AddField("sourceId", Utility.GetSourceID().ToString());
		wWWForm.AddField("appId", Utility.GetAppID().ToString());
		wWWForm.AddField("accessTokenString", Utility.GetAccessTokenForNetwork(req.networkId).GetByteString());
		wWWForm.AddField("domain", 0);
		wWWForm.AddField("networkId", req.networkId.ToString());
		wWWForm.headers["Accept"] = "application/json";
		WWW client = new WWW(uri.ToString(), wWWForm);
		return StartCoroutine(ProcessMatchResponse(client, callback));
	}

	public Coroutine DropConnection(NetworkID netId, NodeID dropNodeId, ResponseDelegate<BasicResponse> callback)
	{
		return DropConnection(new DropConnectionRequest
		{
			networkId = netId,
			nodeId = dropNodeId
		}, callback);
	}

	public Coroutine DropConnection(DropConnectionRequest req, ResponseDelegate<BasicResponse> callback)
	{
		Uri uri = new Uri(baseUri, "/json/reply/DropConnectionRequest");
		Debug.Log("MatchMakingClient DropConnection :" + uri);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("projectId", Application.cloudProjectId);
		wWWForm.AddField("sourceId", Utility.GetSourceID().ToString());
		wWWForm.AddField("appId", Utility.GetAppID().ToString());
		wWWForm.AddField("accessTokenString", Utility.GetAccessTokenForNetwork(req.networkId).GetByteString());
		wWWForm.AddField("domain", 0);
		wWWForm.AddField("networkId", req.networkId.ToString());
		wWWForm.AddField("nodeId", req.nodeId.ToString());
		wWWForm.headers["Accept"] = "application/json";
		WWW client = new WWW(uri.ToString(), wWWForm);
		return StartCoroutine(ProcessMatchResponse(client, callback));
	}

	public Coroutine ListMatches(int startPageNumber, int resultPageSize, string matchNameFilter, ResponseDelegate<ListMatchResponse> callback)
	{
		return ListMatches(new ListMatchRequest
		{
			pageNum = startPageNumber,
			pageSize = resultPageSize,
			nameFilter = matchNameFilter
		}, callback);
	}

	public Coroutine ListMatches(ListMatchRequest req, ResponseDelegate<ListMatchResponse> callback)
	{
		Uri uri = new Uri(baseUri, "/json/reply/ListMatchRequest");
		Debug.Log("MatchMakingClient ListMatches :" + uri);
		WWWForm wWWForm = new WWWForm();
		wWWForm.AddField("projectId", Application.cloudProjectId);
		wWWForm.AddField("sourceId", Utility.GetSourceID().ToString());
		wWWForm.AddField("appId", Utility.GetAppID().ToString());
		wWWForm.AddField("includePasswordMatches", req.includePasswordMatches.ToString());
		wWWForm.AddField("accessTokenString", 0);
		wWWForm.AddField("domain", 0);
		wWWForm.AddField("pageSize", req.pageSize);
		wWWForm.AddField("pageNum", req.pageNum);
		wWWForm.AddField("nameFilter", req.nameFilter);
		wWWForm.headers["Accept"] = "application/json";
		WWW client = new WWW(uri.ToString(), wWWForm);
		return StartCoroutine(ProcessMatchResponse(client, callback));
	}

	private IEnumerator ProcessMatchResponse<JSONRESPONSE>(WWW client, ResponseDelegate<JSONRESPONSE> callback) where JSONRESPONSE : Response, new()
	{
		yield return client;
		JSONRESPONSE jsonInterface = (JSONRESPONSE)null;
		if (string.IsNullOrEmpty(client.error))
		{
			if (global::SimpleJson.SimpleJson.TryDeserializeObject(client.text, out var o) && o is IDictionary<string, object>)
			{
				try
				{
					jsonInterface = new JSONRESPONSE();
					object obj = o;
					jsonInterface.Parse(obj);
				}
				catch (FormatException ex)
				{
					FormatException exception = ex;
					Debug.Log(exception);
				}
			}
			if (jsonInterface == null)
			{
				Debug.LogError("Could not parse: " + client.text);
			}
			else
			{
				Debug.Log("JSON Response: " + jsonInterface.ToString());
			}
		}
		else
		{
			Debug.LogError("Request error: " + client.error);
			Debug.LogError("Raw response: " + client.text);
		}
		if (jsonInterface == null)
		{
			jsonInterface = new JSONRESPONSE();
		}
		callback(jsonInterface);
	}
}
