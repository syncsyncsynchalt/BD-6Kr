using KCV;
using KCV.Utils;
using KCV.View.Scroll;
using KCV.View.Scroll.Mission;
using local.models;

public class UIMissionScrollListParent : UIScrollListParent<MissionModel, UIMissionScrollListChild>
{
	public new void Initialize(MissionModel[] missionModels)
	{
		base.Initialize(missionModels);
	}

	protected override void OnAction(ActionType actionType, UIScrollListParent<MissionModel, UIMissionScrollListChild> calledObject, UIMissionScrollListChild actionChild)
	{
		base.OnAction(actionType, calledObject, actionChild);
		switch (actionType)
		{
		case ActionType.OnChangeFirstFocus:
			break;
		case ActionType.OnChangeFocus:
			SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			break;
		case ActionType.OnButtonSelect:
		case ActionType.OnTouch:
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			break;
		case ActionType.OnBack:
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			break;
		}
	}

	protected override void OnUpdate()
	{
	}
}
