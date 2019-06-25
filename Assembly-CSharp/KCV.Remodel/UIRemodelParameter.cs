using UnityEngine;

namespace KCV.Remodel
{
	public class UIRemodelParameter : MonoBehaviour
	{
		private enum ValueStatus
		{
			DEFAULT,
			UP,
			DOWN,
			MAX
		}

		[SerializeField]
		private UISprite mSprite_ParamType;

		[SerializeField]
		private UILabel mLabel_Value;

		[SerializeField]
		private UISprite mSprite_Status;

		private int mDefaultValue;

		private ParameterType mPrameterType;

		private void OnDestroy()
		{
			if (mSprite_ParamType != null)
			{
				mSprite_ParamType.RemoveFromPanel();
			}
			mSprite_ParamType = null;
			if (mLabel_Value != null)
			{
				mLabel_Value.RemoveFromPanel();
			}
			mLabel_Value = null;
			if (mSprite_Status != null)
			{
				mSprite_Status.RemoveFromPanel();
				mSprite_Status.atlas = null;
			}
			mSprite_Status = null;
		}

		public void Initialize(ParameterType parameterType, int defaultValue)
		{
			mDefaultValue = defaultValue;
			mPrameterType = parameterType;
			mLabel_Value.text = ParamToString(mPrameterType, mDefaultValue);
			ChangeVirtualUpdateStatusColor(ValueStatus.DEFAULT);
		}

		public void StatusReset()
		{
			mLabel_Value.text = ParamToString(mPrameterType, mDefaultValue);
			ChangeVirtualUpdateStatusColor(ValueStatus.DEFAULT);
		}

		private void ChangeVirtualUpdateStatusColor(ValueStatus status)
		{
			switch (status)
			{
			case ValueStatus.DEFAULT:
				mSprite_Status.spriteName = string.Empty;
				mSprite_Status.transform.localScale = Vector3.zero;
				mSprite_Status.alpha = 0.01f;
				break;
			case ValueStatus.UP:
				mSprite_Status.spriteName = "status_up";
				mSprite_Status.transform.localScale = Vector3.one;
				mSprite_Status.alpha = 1f;
				break;
			case ValueStatus.DOWN:
				mSprite_Status.spriteName = "status_down";
				mSprite_Status.transform.localScale = Vector3.one;
				mSprite_Status.alpha = 1f;
				break;
			case ValueStatus.MAX:
				mSprite_Status.spriteName = "status_max";
				mSprite_Status.transform.localScale = Vector3.one;
				mSprite_Status.alpha = 1f;
				break;
			}
		}

		public void PreviewVirtualUpdatedValue(int nextParam, bool isMAX)
		{
			mLabel_Value.text = ParamToString(mPrameterType, nextParam);
			if (nextParam < mDefaultValue)
			{
				ChangeVirtualUpdateStatusColor(ValueStatus.DOWN);
			}
			else if (mDefaultValue < nextParam)
			{
				if (isMAX)
				{
					ChangeVirtualUpdateStatusColor(ValueStatus.MAX);
				}
				else
				{
					ChangeVirtualUpdateStatusColor(ValueStatus.UP);
				}
			}
			else
			{
				ChangeVirtualUpdateStatusColor(ValueStatus.DEFAULT);
			}
		}

		private string ParamToString(ParameterType paramType, int value)
		{
			switch (paramType)
			{
			case ParameterType.Soku:
				return GetSoukuText(value);
			case ParameterType.Leng:
				return GetLengText(value);
			default:
				return value.ToString();
			}
		}

		private string GetLengText(int value)
		{
			switch (value)
			{
			case 0:
				return "無";
			case 1:
				return "短";
			case 2:
				return "中";
			case 3:
				return "長";
			case 4:
				return "超長";
			default:
				return string.Empty;
			}
		}

		private string GetSoukuText(int value)
		{
			if (value == 10)
			{
				return "高速";
			}
			return "低速";
		}

		internal void Release()
		{
			if (mSprite_ParamType != null)
			{
				mSprite_ParamType.RemoveFromPanel();
				mSprite_ParamType.Clear();
				NGUITools.Destroy(mSprite_ParamType);
			}
			mSprite_ParamType = null;
			NGUITools.Destroy(mLabel_Value);
			mLabel_Value = null;
			if (mSprite_Status != null)
			{
				mSprite_Status.Clear();
				NGUITools.Destroy(mSprite_Status);
			}
			mSprite_Status = null;
		}
	}
}
