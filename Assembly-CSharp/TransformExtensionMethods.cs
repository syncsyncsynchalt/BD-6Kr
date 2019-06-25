using System;
using System.Collections;
using UnityEngine;

public static class TransformExtensionMethods
{
	private static Vector3 _vVec;

	public static T AddComponent<T>(this Transform transform) where T : Component
	{
		return transform.gameObject.AddComponent<T>();
	}

	public static T GetComponent<T>(this Transform transform) where T : Component
	{
		return ((Component)transform).GetComponent<T>() ?? transform.AddComponent<T>();
	}

	public static Component GetComponent(this Transform transform, Type componentType)
	{
		return transform.GetComponent(componentType) ?? transform.gameObject.AddComponent(componentType);
	}

	public static Transform Sync(this Transform transform, Transform target)
	{
		return transform = target;
	}

	public static Transform Sync(this Transform transform, GameObject target)
	{
		return transform = target.transform;
	}

	public static Transform LookAt2D(this Transform transform, Transform target)
	{
		Vector3 vector = target.transform.position - transform.position;
		float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		return transform;
	}

	public static void LookAt2D(this Transform self, Transform target, Vector2 forward)
	{
		self.LookAt2D(target.position, forward);
	}

	public static void LookAt2D(this Transform self, Vector3 target, Vector2 forward)
	{
		float forwardDiffPoint = GetForwardDiffPoint(forward);
		Vector3 vector = target - self.position;
		float num = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		self.rotation = Quaternion.AngleAxis(num - forwardDiffPoint, Vector3.forward);
	}

	private static float GetForwardDiffPoint(Vector2 forward)
	{
		if (object.Equals(forward, Vector2.up))
		{
			return 90f;
		}
		if (object.Equals(forward, Vector2.right))
		{
			return 0f;
		}
		return 0f;
	}

	public static Vector3 ScreenPoint(this Transform transform, Camera cam)
	{
		return cam.WorldToScreenPoint(transform.position);
	}

	public static Vector3 position(this Transform transform, float x, float y, float z)
	{
		_vVec.Set(x, y, z);
		return transform.position = _vVec;
	}

	public static Vector3 position(this Transform transform, Vector3 vpos)
	{
		return transform.position(vpos.x, vpos.y, vpos.z);
	}

	public static Vector3 position(this Transform transform, ExtensionUtils.Axis iaxis, float pos)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
		{
			Vector3 position5 = transform.position;
			float y = position5.y;
			Vector3 position6 = transform.position;
			_vVec = transform.position(pos, y, position6.z);
			break;
		}
		case ExtensionUtils.Axis.AxisY:
		{
			Vector3 position3 = transform.position;
			float x2 = position3.x;
			Vector3 position4 = transform.position;
			_vVec = transform.position(x2, pos, position4.z);
			break;
		}
		case ExtensionUtils.Axis.AxisZ:
		{
			Vector3 position = transform.position;
			float x = position.x;
			Vector3 position2 = transform.position;
			_vVec = transform.position(x, position2.y, pos);
			break;
		}
		case ExtensionUtils.Axis.AxisAll:
			_vVec = transform.position(pos, pos, pos);
			break;
		default:
			_vVec = transform.position;
			break;
		}
		return transform.position(_vVec);
	}

	public static Vector3 positionX(this Transform transform, float x = 0f)
	{
		Vector3 position = transform.position;
		float y = position.y;
		Vector3 position2 = transform.position;
		return transform.position(x, y, position2.z);
	}

	public static Vector3 positionY(this Transform transform, float y = 0f)
	{
		Vector3 position = transform.position;
		float x = position.x;
		Vector3 position2 = transform.position;
		return transform.position(x, y, position2.z);
	}

	public static Vector3 positionZ(this Transform transdorm, float z = 0f)
	{
		Vector3 position = transdorm.position;
		float x = position.x;
		Vector3 position2 = transdorm.position;
		return transdorm.position(x, position2.y, z);
	}

	public static Vector3 positionZero(this Transform transform)
	{
		return transform.position = Vector3.zero;
	}

	public static Vector3 AddPos(this Transform transform, float x = 0f, float y = 0f, float z = 0f)
	{
		Vector3 position = transform.position;
		float new_x = position.x + x;
		Vector3 position2 = transform.position;
		float new_y = position2.y + y;
		Vector3 position3 = transform.position;
		_vVec.Set(new_x, new_y, position3.z + z);
		return transform.position(_vVec);
	}

	public static Vector3 AddPos(this Transform transform, Vector3 vpos)
	{
		return transform.AddPos(vpos.x, vpos.y, vpos.z);
	}

	public static Vector3 AddPos(this Transform transform, ExtensionUtils.Axis iaxis, float pos)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
			_vVec.Set(pos, 0f, 0f);
			break;
		case ExtensionUtils.Axis.AxisY:
			_vVec.Set(0f, pos, 0f);
			break;
		case ExtensionUtils.Axis.AxisZ:
			_vVec.Set(0f, 0f, pos);
			break;
		case ExtensionUtils.Axis.AxisAll:
			_vVec.Set(pos, pos, pos);
			break;
		default:
			_vVec.Set(0f, 0f, 0f);
			break;
		}
		return transform.AddPos(_vVec);
	}

	public static Vector3 AddPosX(this Transform transform, float x)
	{
		return transform.AddPos(x);
	}

	public static Vector3 AddPosY(this Transform transform, float y)
	{
		return transform.AddPos(0f, y);
	}

	public static Vector3 AddPosZ(this Transform transform, float z)
	{
		return transform.AddPos(0f, 0f, z);
	}

	public static Vector3 localPosition(this Transform transform, float x, float y, float z)
	{
		_vVec.Set(x, y, z);
		return transform.localPosition = _vVec;
	}

	public static Vector3 localPosition(this Transform transform, Vector3 vpos)
	{
		return transform.localPosition(vpos.x, vpos.y, vpos.z);
	}

	public static Vector3 localPosition(this Transform transform, ExtensionUtils.Axis iaxis, float pos)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
		{
			Vector3 localPosition5 = transform.localPosition;
			float y = localPosition5.y;
			Vector3 localPosition6 = transform.localPosition;
			_vVec.Set(pos, y, localPosition6.z);
			break;
		}
		case ExtensionUtils.Axis.AxisY:
		{
			Vector3 localPosition3 = transform.localPosition;
			float x2 = localPosition3.x;
			Vector3 localPosition4 = transform.localPosition;
			_vVec.Set(x2, pos, localPosition4.z);
			break;
		}
		case ExtensionUtils.Axis.AxisZ:
		{
			Vector3 localPosition = transform.localPosition;
			float x = localPosition.x;
			Vector3 localPosition2 = transform.localPosition;
			_vVec.Set(x, localPosition2.y, pos);
			break;
		}
		case ExtensionUtils.Axis.AxisAll:
			_vVec.Set(pos, pos, pos);
			break;
		default:
			_vVec = transform.localPosition;
			break;
		}
		return transform.localPosition(_vVec);
	}

	public static Vector3 localPositionX(this Transform transform, float x)
	{
		return transform.localPosition(ExtensionUtils.Axis.AxisX, x);
	}

	public static Vector3 localPositionY(this Transform transform, float y)
	{
		return transform.localPosition(ExtensionUtils.Axis.AxisY, y);
	}

	public static Vector3 localPositionZ(this Transform transform, float z)
	{
		return transform.localPosition(ExtensionUtils.Axis.AxisZ, z);
	}

	public static Vector3 localPositionZero(this Transform transform)
	{
		return transform.localPosition(0f, 0f, 0f);
	}

	public static Vector3 AddLocalPosition(this Transform transform, float x = 0f, float y = 0f, float z = 0f)
	{
		Vector3 localPosition = transform.localPosition;
		float new_x = localPosition.x + x;
		Vector3 localPosition2 = transform.localPosition;
		float new_y = localPosition2.y + y;
		Vector3 localPosition3 = transform.localPosition;
		_vVec.Set(new_x, new_y, localPosition3.z + z);
		return transform.localPosition = _vVec;
	}

	public static Vector3 AddLocalPosition(this Transform transform, Vector3 vpos)
	{
		return transform.AddLocalPosition(vpos.x, vpos.y, vpos.z);
	}

	public static Vector3 AddLocalPosition(this Transform transform, ExtensionUtils.Axis iaxis = ExtensionUtils.Axis.None, float pos = 0f)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
			_vVec.Set(pos, 0f, 0f);
			break;
		case ExtensionUtils.Axis.AxisY:
			_vVec.Set(0f, pos, 0f);
			break;
		case ExtensionUtils.Axis.AxisZ:
			_vVec.Set(0f, 0f, pos);
			break;
		case ExtensionUtils.Axis.AxisAll:
			_vVec.Set(pos, pos, pos);
			break;
		default:
			_vVec.Set(0f, 0f, 0f);
			break;
		}
		return transform.AddLocalPosition(_vVec);
	}

	public static Vector3 AddLocalPositionX(this Transform transform, float x = 0f)
	{
		return transform.AddLocalPosition(x);
	}

	public static Vector3 AddLocalPositionY(this Transform transform, float y = 0f)
	{
		return transform.AddLocalPosition(0f, y);
	}

	public static Vector3 AddLocalPositionZ(this Transform transform, float z = 0f)
	{
		return transform.AddLocalPosition(0f, 0f, z);
	}

	public static Vector3 localScale(this Transform transform, float x, float y, float z)
	{
		_vVec.Set(x, y, z);
		return transform.localScale = _vVec;
	}

	public static Vector3 localScale(this Transform transform, Vector3 scale)
	{
		return transform.localScale(scale.x, scale.y, scale.z);
	}

	public static Vector3 localScale(this Transform transform, ExtensionUtils.Axis iaxis, float scale)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
		{
			Vector3 localScale8 = transform.localScale;
			float y2 = localScale8.y;
			Vector3 localScale9 = transform.localScale;
			_vVec.Set(scale, y2, localScale9.z);
			break;
		}
		case ExtensionUtils.Axis.AxisY:
		{
			Vector3 localScale6 = transform.localScale;
			float x3 = localScale6.x;
			Vector3 localScale7 = transform.localScale;
			_vVec.Set(x3, scale, localScale7.z);
			break;
		}
		case ExtensionUtils.Axis.AxisZ:
		{
			Vector3 localScale4 = transform.localScale;
			float x2 = localScale4.x;
			Vector3 localScale5 = transform.localScale;
			_vVec.Set(x2, localScale5.y, scale);
			break;
		}
		case ExtensionUtils.Axis.AxisAll:
			_vVec.Set(scale, scale, scale);
			break;
		default:
		{
			Vector3 localScale = transform.localScale;
			float x = localScale.x;
			Vector3 localScale2 = transform.localScale;
			float y = localScale2.y;
			Vector3 localScale3 = transform.localScale;
			_vVec.Set(x, y, localScale3.z);
			break;
		}
		}
		return transform.localScale(_vVec);
	}

	public static Vector3 localScaleX(this Transform transform, float x)
	{
		Vector3 localScale = transform.localScale;
		float y = localScale.y;
		Vector3 localScale2 = transform.localScale;
		return transform.localScale(x, y, localScale2.z);
	}

	public static Vector3 localScaleY(this Transform transform, float y)
	{
		Vector3 localScale = transform.localScale;
		float x = localScale.x;
		Vector3 localScale2 = transform.localScale;
		return transform.localScale(x, y, localScale2.z);
	}

	public static Vector3 localScaleZ(this Transform transform, float z)
	{
		Vector3 localScale = transform.localScale;
		float x = localScale.x;
		Vector3 localScale2 = transform.localScale;
		return transform.localScale(x, localScale2.y, z);
	}

	public static void localScaleZero(this Transform transform)
	{
		transform.localScale = Vector3.zero;
	}

	public static void localScaleOne(this Transform transform)
	{
		transform.localScale = Vector3.one;
	}

	public static Vector3 AddLocalScale(this Transform transform, float x, float y, float z)
	{
		Vector3 localScale = transform.localScale;
		float new_x = localScale.x + x;
		Vector3 localScale2 = transform.localScale;
		float new_y = localScale2.y + y;
		Vector3 localScale3 = transform.localScale;
		_vVec.Set(new_x, new_y, localScale3.z + z);
		return transform.localScale = _vVec;
	}

	public static Vector3 AddLocalScale(this Transform transform, Vector3 vscale)
	{
		return transform.AddLocalScale(vscale.x, vscale.y, vscale.z);
	}

	public static Vector3 AddLocalScale(this Transform transform, ExtensionUtils.Axis iaxis, float fscale)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
			_vVec.Set(fscale, 0f, 0f);
			break;
		case ExtensionUtils.Axis.AxisY:
			_vVec.Set(0f, fscale, 0f);
			break;
		case ExtensionUtils.Axis.AxisZ:
			_vVec.Set(0f, 0f, fscale);
			break;
		case ExtensionUtils.Axis.AxisAll:
			_vVec.Set(fscale, fscale, fscale);
			break;
		default:
			_vVec = transform.localScale;
			break;
		}
		return transform.AddLocalScale(_vVec);
	}

	public static Vector3 AddLocalScaleX(this Transform transform, float fx)
	{
		return transform.AddLocalScale(fx, 0f, 0f);
	}

	public static Vector3 AddLocalScaleY(this Transform transform, float fy)
	{
		return transform.AddLocalScale(0f, fy, 0f);
	}

	public static Vector3 AddLocalScaleZ(this Transform transform, float fz)
	{
		return transform.AddLocalScale(0f, 0f, fz);
	}

	public static Vector3 eulerAngles(this Transform transform, float x, float y, float z)
	{
		_vVec.Set(x, y, z);
		return transform.eulerAngles = _vVec;
	}

	public static Vector3 eulerAngles(this Transform transform, Vector3 vrot)
	{
		return transform.eulerAngles(vrot.x, vrot.y, vrot.z);
	}

	public static Vector3 eulerAngles(this Transform transform, ExtensionUtils.Axis iaxis, float frot)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
		{
			Vector3 localEulerAngles8 = transform.localEulerAngles;
			float y2 = localEulerAngles8.y;
			Vector3 localEulerAngles9 = transform.localEulerAngles;
			_vVec.Set(frot, y2, localEulerAngles9.z);
			break;
		}
		case ExtensionUtils.Axis.AxisY:
		{
			Vector3 localEulerAngles6 = transform.localEulerAngles;
			float x3 = localEulerAngles6.x;
			Vector3 localEulerAngles7 = transform.localEulerAngles;
			_vVec.Set(x3, frot, localEulerAngles7.z);
			break;
		}
		case ExtensionUtils.Axis.AxisZ:
		{
			Vector3 localEulerAngles4 = transform.localEulerAngles;
			float x2 = localEulerAngles4.x;
			Vector3 localEulerAngles5 = transform.localEulerAngles;
			_vVec.Set(x2, localEulerAngles5.y, frot);
			break;
		}
		case ExtensionUtils.Axis.AxisAll:
			_vVec.Set(frot, frot, frot);
			break;
		default:
		{
			Vector3 localEulerAngles = transform.localEulerAngles;
			float x = localEulerAngles.x;
			Vector3 localEulerAngles2 = transform.localEulerAngles;
			float y = localEulerAngles2.y;
			Vector3 localEulerAngles3 = transform.localEulerAngles;
			_vVec.Set(x, y, localEulerAngles3.z);
			break;
		}
		}
		return transform.eulerAngles(_vVec);
	}

	public static Vector3 eulerAnglesX(this Transform transform, float fx)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		float y = localEulerAngles.y;
		Vector3 localEulerAngles2 = transform.localEulerAngles;
		return transform.eulerAngles(fx, y, localEulerAngles2.z);
	}

	public static Vector3 eulerAnglesY(this Transform transform, float fy)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		float x = localEulerAngles.x;
		Vector3 localEulerAngles2 = transform.localEulerAngles;
		return transform.eulerAngles(x, fy, localEulerAngles2.z);
	}

	public static Vector3 eulerAnglesZ(this Transform transform, float fz)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		float x = localEulerAngles.x;
		Vector3 localEulerAngles2 = transform.localEulerAngles;
		return transform.eulerAngles(x, localEulerAngles2.y, fz);
	}

	public static Vector3 AddEulerAngles(this Transform transform, float x, float y, float z)
	{
		Vector3 eulerAngles = transform.eulerAngles;
		float new_x = eulerAngles.x + x;
		Vector3 eulerAngles2 = transform.eulerAngles;
		float new_y = eulerAngles2.y + y;
		Vector3 eulerAngles3 = transform.eulerAngles;
		_vVec.Set(new_x, new_y, eulerAngles3.z + z);
		return transform.eulerAngles = _vVec;
	}

	public static Vector3 AddEulerAngles(this Transform transform, Vector3 vrot)
	{
		return transform.AddEulerAngles(vrot.x, vrot.y, vrot.z);
	}

	public static Vector3 AddEulerAngles(this Transform transform, ExtensionUtils.Axis iaxis, float frot)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
			_vVec.Set(frot, 0f, 0f);
			break;
		case ExtensionUtils.Axis.AxisY:
			_vVec.Set(0f, frot, 0f);
			break;
		case ExtensionUtils.Axis.AxisZ:
			_vVec.Set(0f, 0f, frot);
			break;
		case ExtensionUtils.Axis.AxisAll:
			_vVec.Set(frot, frot, frot);
			break;
		default:
			_vVec.Set(0f, 0f, 0f);
			break;
		}
		return transform.AddEulerAngles(_vVec);
	}

	public static Vector3 AddEulerAnglesX(this Transform transform, float fx)
	{
		return transform.AddEulerAngles(fx, 0f, 0f);
	}

	public static Vector3 AddEulerAnglesY(this Transform transform, float fy)
	{
		return transform.AddEulerAngles(0f, fy, 0f);
	}

	public static Vector3 AddEulerAnglesZ(this Transform transform, float fz)
	{
		return transform.AddEulerAngles(0f, 0f, fz);
	}

	public static Vector3 localEulerAngles(this Transform transform, float x, float y, float z)
	{
		_vVec.Set(x, y, z);
		return transform.localEulerAngles = _vVec;
	}

	public static Vector3 localEulerAngles(this Transform transform, Vector3 vrot)
	{
		return transform.localEulerAngles(vrot.x, vrot.y, vrot.z);
	}

	public static Vector3 localEulerAngles(this Transform transform, ExtensionUtils.Axis iaxis, float frot)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
		{
			Vector3 localEulerAngles8 = transform.localEulerAngles;
			float y2 = localEulerAngles8.y;
			Vector3 localEulerAngles9 = transform.localEulerAngles;
			_vVec.Set(frot, y2, localEulerAngles9.z);
			break;
		}
		case ExtensionUtils.Axis.AxisY:
		{
			Vector3 localEulerAngles6 = transform.localEulerAngles;
			float x3 = localEulerAngles6.x;
			Vector3 localEulerAngles7 = transform.localEulerAngles;
			_vVec.Set(x3, frot, localEulerAngles7.z);
			break;
		}
		case ExtensionUtils.Axis.AxisZ:
		{
			Vector3 localEulerAngles4 = transform.localEulerAngles;
			float x2 = localEulerAngles4.x;
			Vector3 localEulerAngles5 = transform.localEulerAngles;
			_vVec.Set(x2, localEulerAngles5.y, frot);
			break;
		}
		case ExtensionUtils.Axis.AxisAll:
			_vVec.Set(frot, frot, frot);
			break;
		default:
		{
			Vector3 localEulerAngles = transform.localEulerAngles;
			float x = localEulerAngles.x;
			Vector3 localEulerAngles2 = transform.localEulerAngles;
			float y = localEulerAngles2.y;
			Vector3 localEulerAngles3 = transform.localEulerAngles;
			_vVec.Set(x, y, localEulerAngles3.z);
			break;
		}
		}
		return transform.localEulerAngles(_vVec);
	}

	public static Vector3 localEulerAnglesX(this Transform transform, float fx)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		float y = localEulerAngles.y;
		Vector3 localEulerAngles2 = transform.localEulerAngles;
		return transform.localEulerAngles(fx, y, localEulerAngles2.z);
	}

	public static Vector3 localEulerAnglesY(this Transform transform, float fy)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		float x = localEulerAngles.x;
		Vector3 localEulerAngles2 = transform.localEulerAngles;
		return transform.localEulerAngles(x, fy, localEulerAngles2.z);
	}

	public static Vector3 localEulerAnglesZ(this Transform transform, float fz)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		float x = localEulerAngles.x;
		Vector3 localEulerAngles2 = transform.localEulerAngles;
		return transform.localEulerAngles(x, localEulerAngles2.y, fz);
	}

	public static Vector3 AddLocalEulerAngles(this Transform transform, float x, float y, float z)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		float new_x = localEulerAngles.x + x;
		Vector3 localEulerAngles2 = transform.localEulerAngles;
		float new_y = localEulerAngles2.y + y;
		Vector3 localEulerAngles3 = transform.localEulerAngles;
		_vVec.Set(new_x, new_y, localEulerAngles3.z + z);
		return transform.localEulerAngles = _vVec;
	}

	public static Vector3 AddLocalEulerAngles(this Transform transform, Vector3 vrot)
	{
		Vector3 localEulerAngles = transform.localEulerAngles;
		float x = localEulerAngles.x + vrot.x;
		Vector3 localEulerAngles2 = transform.localEulerAngles;
		float y = localEulerAngles2.y + vrot.y;
		Vector3 localEulerAngles3 = transform.localEulerAngles;
		return transform.AddLocalEulerAngles(x, y, localEulerAngles3.z + vrot.z);
	}

	public static Vector3 AddLocalEulerAngles(this Transform transform, ExtensionUtils.Axis iaxis, float frot)
	{
		switch (iaxis)
		{
		case ExtensionUtils.Axis.AxisX:
			_vVec.Set(frot, 0f, 0f);
			break;
		case ExtensionUtils.Axis.AxisY:
			_vVec.Set(0f, frot, 0f);
			break;
		case ExtensionUtils.Axis.AxisZ:
			_vVec.Set(0f, 0f, frot);
			break;
		case ExtensionUtils.Axis.AxisAll:
			_vVec.Set(frot, frot, frot);
			break;
		default:
			_vVec.Set(0f, 0f, 0f);
			break;
		}
		return transform.AddLocalEulerAngles(_vVec);
	}

	public static Vector3 AddLocalEulerAnglesX(this Transform transform, float frot)
	{
		return transform.AddLocalEulerAngles(frot, 0f, 0f);
	}

	public static Vector3 AddLocalEulerAnglesY(this Transform transform, float frot)
	{
		return transform.AddLocalEulerAngles(frot, 0f, 0f);
	}

	public static Vector3 AddLocalEulerAnglesZ(this Transform transform, float frot)
	{
		return transform.AddLocalEulerAngles(frot, 0f, 0f);
	}

	public static void LookFrom(this Transform transform, Hashtable hash)
	{
		iTween.LookFrom(transform.gameObject, hash);
	}

	public static void LookFrom(this Transform transform, Vector3 target, float time)
	{
		iTween.LookFrom(transform.gameObject, target, time);
	}

	public static void LookTo(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash["oncomplete"] == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.LookTo(transform.gameObject, hash);
	}

	public static void LookTo(this Transform transform, Vector3 lookTarget, float time)
	{
		iTween.LookTo(transform.gameObject, lookTarget, time);
	}

	public static void LookTo(this Transform transform, Vector3 lookTarget, float time, Action onComplate)
	{
		transform.LookTo(lookTarget, time, iTween.EaseType.easeOutExpo, onComplate);
	}

	public static void LookTo(this Transform transform, Vector3 lookTarget, float time, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("looktarget", lookTarget);
		hashtable.Add("time", time);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.LookTo(hashtable);
	}

	public static void LookUpdate(this Transform transform, Hashtable hash)
	{
		iTween.LookUpdate(transform.gameObject, hash);
	}

	public static void LookUpdate(this Transform transform, Vector3 target, float time)
	{
		iTween.LookUpdate(transform.gameObject, target, time);
	}

	public static void MoveAdd(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash["oncomplete"] == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.MoveAdd(transform.gameObject, hash);
	}

	public static void MoveAdd(this Transform transform, Vector3 amount, float time)
	{
		iTween.MoveAdd(transform.gameObject, amount, time);
	}

	public static void MoveAdd(this Transform transform, Vector3 amount, float time, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("amount", amount);
		hashtable.Add("time", time);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.MoveAdd(hashtable);
	}

	public static void MoveBy(this Transform transform, Hashtable hash)
	{
		iTween.MoveBy(transform.gameObject, hash);
	}

	public static void MoveBy(this Transform transform, Vector3 amount, float time)
	{
		iTween.MoveBy(transform.gameObject, amount, time);
	}

	public static void MoveFrom(this Transform transform, Hashtable hash)
	{
		iTween.MoveFrom(transform.gameObject, hash);
	}

	public static void MoveFrom(this Transform transform, Vector3 pos, float time)
	{
		iTween.MoveFrom(transform.gameObject, pos, time);
	}

	public static void MoveTo(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash["oncomplete"] == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.MoveTo(transform.gameObject, hash);
	}

	public static void MoveTo(this Transform transform, Vector3 target, float time)
	{
		iTween.MoveTo(transform.gameObject, target, time);
	}

	public static void MoveTo(this Transform transform, Vector3 target, float time, Action onComplate)
	{
		transform.MoveTo(target, time, iTween.EaseType.easeOutExpo, onComplate);
	}

	public static void MoveTo(this Transform transform, Vector3 target, float time, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("position", target);
		hashtable.Add("time", time);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.MoveTo(hashtable);
	}

	public static void LocalMoveTo(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash["oncomplete"] == null)
		{
			hash.Remove("oncomplete");
		}
		hash.Add("islocal", true);
		transform.MoveTo(hash);
	}

	public static void LocalMoveTo(this Transform transform, Vector3 target, float time)
	{
		transform.LocalMoveTo(target, time, null);
	}

	public static void LocalMoveTo(this Transform transform, Vector3 target, float time, Action onComplate)
	{
		transform.LocalMoveTo(target, time, iTween.EaseType.easeOutExpo, onComplate);
	}

	public static void LocalMoveTo(this Transform transform, Vector3 target, float time, iTween.EaseType easeType, Action onComplate)
	{
		transform.LocalMoveTo(target, time, 0f, easeType, onComplate);
	}

	public static void LocalMoveTo(this Transform transform, Vector3 target, float time, float delay, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("position", target);
		hashtable.Add("time", time);
		hashtable.Add("delay", delay);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.LocalMoveTo(hashtable);
	}

	public static void MoveUpdate(this Transform transform, Hashtable hash)
	{
		iTween.MoveUpdate(transform.gameObject, hash);
	}

	public static void MoveUpdate(this Transform transform, Vector3 pos, float time)
	{
		iTween.MoveUpdate(transform.gameObject, pos, time);
	}

	public static void RotateAdd(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash["oncomplete"] == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.RotateAdd(transform.gameObject, hash);
	}

	public static void RotateAdd(this Transform transform, Vector3 amount, float time)
	{
		iTween.RotateAdd(transform.gameObject, amount, time);
	}

	public static void RotateAdd(this Transform transform, Vector3 amount, float time, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("amount", amount);
		hashtable.Add("time", time);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.RotateAdd(hashtable);
	}

	public static void RotateBy(this Transform transform, Hashtable hash)
	{
		iTween.RotateBy(transform.gameObject, hash);
	}

	public static void RotateBy(this Transform transform, Vector3 amount, float time)
	{
		iTween.RotateBy(transform.gameObject, amount, time);
	}

	public static void RotateFrom(this Transform transform, Hashtable hash)
	{
		iTween.RotateFrom(transform.gameObject, hash);
	}

	public static void RotateFrom(this Transform transform, Vector3 rot, float time)
	{
		iTween.RotateFrom(transform.gameObject, rot, time);
	}

	public static void RotateTo(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash["oncomplete"] == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.RotateTo(transform.gameObject, hash);
	}

	public static void RotateTo(this Transform transform, Vector3 rot, float time)
	{
		iTween.RotateTo(transform.gameObject, rot, time);
	}

	public static void RotateTo(this Transform transform, Vector3 rot, float time, Action onComplate)
	{
		transform.RotateTo(rot, time, iTween.EaseType.easeOutExpo, onComplate);
	}

	public static void RotateTo(this Transform transform, Vector3 rot, float time, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("rotation", rot);
		hashtable.Add("time", time);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.RotateTo(hashtable);
	}

	public static void RotateUpdate(this Transform transform, Hashtable hash)
	{
		iTween.RotateUpdate(transform.gameObject, hash);
	}

	public static void RotateUpadte(this Transform transform, Vector3 rot, float time)
	{
		iTween.RotateUpdate(transform.gameObject, rot, time);
	}

	public static void ScaleAdd(this Transform transform, Hashtable hash)
	{
		iTween.ScaleAdd(transform.gameObject, hash);
	}

	public static void ScaleAdd(this Transform transform, Vector3 amount, float time)
	{
		iTween.ScaleAdd(transform.gameObject, amount, time);
	}

	public static void ScaleBy(this Transform transform, Hashtable hash)
	{
		iTween.ScaleBy(transform.gameObject, hash);
	}

	public static void ScaleBy(this Transform transform, Vector3 amount, float time)
	{
		iTween.ScaleBy(transform.gameObject, amount, time);
	}

	public static void ScaleFrom(this Transform transform, Hashtable hash)
	{
		iTween.ScaleFrom(transform.gameObject, hash);
	}

	public static void ScaleFrom(this Transform transform, Vector3 scale, float time)
	{
		iTween.ScaleFrom(transform.gameObject, scale, time);
	}

	public static void ScaleTo(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash["oncomplete"] == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.ScaleTo(transform.gameObject, hash);
	}

	public static void ScaleTo(this Transform transform, Vector3 scale, float time)
	{
		iTween.ScaleTo(transform.gameObject, scale, time);
	}

	public static void ScaleTo(this Transform transform, Vector3 scale, float time, Action onComplate)
	{
		transform.ScaleTo(scale, time, iTween.EaseType.easeOutExpo, onComplate);
	}

	public static void ScaleTo(this Transform transform, Vector3 scale, float time, iTween.EaseType easeType, Action onComplate)
	{
		transform.ScaleTo(scale, time, 0f, easeType, onComplate);
	}

	public static void ScaleTo(this Transform transform, Vector3 target, float time, float delay, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("scale", target);
		hashtable.Add("time", time);
		hashtable.Add("delay", delay);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.ScaleTo(hashtable);
	}

	public static void ScaleUpdate(this Transform transform, Hashtable hash)
	{
		iTween.ScaleUpdate(transform.gameObject, hash);
	}

	public static void ScaleUpdate(this Transform transform, Vector3 scale, float time)
	{
		iTween.ScaleUpdate(transform.gameObject, scale, time);
	}

	public static void ColorTo(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash["oncomplete"] == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.ColorTo(transform.gameObject, hash);
	}

	public static void ColorTo(this Transform transform, Color color, float time)
	{
		iTween.ColorTo(transform.gameObject, color, time);
	}

	public static void ColorTo(this Transform transform, Color color, float time, float delay, iTween.EaseType easeType, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("color", color);
		hashtable.Add("time", time);
		hashtable.Add("delay", delay);
		hashtable.Add("easetype", easeType);
		hashtable.Add("oncomplete", onComplate);
		transform.ColorTo(hashtable);
	}

	public static void ShakePosition(this Transform transform, Hashtable hash)
	{
		iTween.ShakePosition(transform.gameObject, hash);
	}

	public static void ShakePosition(this Transform transform, Vector3 amount, float time)
	{
		iTween.ShakePosition(transform.gameObject, amount, time);
	}

	public static void ShakeRotation(this Transform transform, Hashtable hash)
	{
		iTween.ShakeRotation(transform.gameObject, hash);
	}

	public static void ShakeRotation(this Transform transform, Vector3 amount, float time)
	{
		iTween.ShakeRotation(transform.gameObject, amount, time);
	}

	public static void ShakeScale(this Transform transform, Hashtable hash)
	{
		iTween.ShakeScale(transform.gameObject, hash);
	}

	public static void ShakeScale(this Transform transform, Vector3 amount, float time)
	{
		iTween.ShakeScale(transform.gameObject, amount, time);
	}

	public static void ValueTo(this Transform transform, Hashtable hash)
	{
		if (hash.ContainsKey("oncomplete") && hash["oncomplete"] == null)
		{
			hash.Remove("oncomplete");
		}
		iTween.ValueTo(transform.gameObject, hash);
	}

	public static void ValueTo(this Transform transform, Hashtable hash, Action onComplate)
	{
		hash.Add("oncomplate", onComplate);
		transform.ValueTo(hash);
	}

	public static void ValueTo(this Transform transform, object from, object to, float time, Action<object> onUpdate, Action onComplate)
	{
		transform.ValueTo(from, to, time, iTween.EaseType.linear, onUpdate, onComplate);
	}

	public static void ValueTo(this Transform transform, object from, object to, float time, iTween.EaseType easeType, Action<object> onUpdate, Action onComplate)
	{
		transform.ValueTo(from, to, time, 0f, easeType, onUpdate, onComplate);
	}

	public static void ValueTo(this Transform transform, object from, object to, float time, float delay, iTween.EaseType easeType, Action<object> onUpdate, Action onComplate)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("from", from);
		hashtable.Add("to", to);
		hashtable.Add("time", time);
		hashtable.Add("delay", delay);
		hashtable.Add("easetype", easeType);
		hashtable.Add("onupdate", onUpdate);
		hashtable.Add("oncomplete", onComplate);
		transform.ValueTo(hashtable);
	}

	public static void iTweenPause(this Transform transform)
	{
		iTween.Pause(transform.gameObject);
	}

	public static void iTweenPause(this Transform transform, bool includechildren)
	{
		iTween.Pause(transform.gameObject, includechildren);
	}

	public static void iTweenStop(this Transform transform)
	{
		iTween.Stop(transform.gameObject);
	}

	public static void iTweenStop(this Transform transform, bool includechildren)
	{
		iTween.Stop(transform.gameObject, includechildren);
	}

	public static void iTweenResume(this Transform transform)
	{
		iTween.Resume(transform.gameObject);
	}

	public static void iTweenResume(this Transform transform, bool includechildren)
	{
		iTween.Resume(transform.gameObject, includechildren);
	}
}
