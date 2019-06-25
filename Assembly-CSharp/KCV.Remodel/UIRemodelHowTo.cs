using UnityEngine;

namespace KCV.Remodel
{
	[SelectionBase]
	[RequireComponent(typeof(UIWidget))]
	public class UIRemodelHowTo : UIHowTo, UIRemodelView
	{
        //virtual 
        void UIRemodelView.Show()
		{
			Show();
		}

        // virtual 
        void UIRemodelView.Hide()
		{
			Hide();
		}
	}
}
