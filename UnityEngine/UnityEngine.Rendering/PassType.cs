namespace UnityEngine.Rendering;

public enum PassType
{
	Normal = 0,
	Vertex = 1,
	VertexLM = 2,
	VertexLMRGBM = 3,
	ForwardBase = 4,
	ForwardAdd = 5,
	LightPrePassBase = 6,
	LightPrePassFinal = 7,
	ShadowCaster = 8,
	Deferred = 10,
	Meta = 11
}
