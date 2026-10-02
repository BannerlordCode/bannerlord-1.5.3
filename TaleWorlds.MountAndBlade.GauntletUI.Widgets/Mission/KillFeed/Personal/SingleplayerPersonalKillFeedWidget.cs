using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.KillFeed.Personal
{
	// Token: 0x020000FE RID: 254
	public class SingleplayerPersonalKillFeedWidget : Widget
	{
		// Token: 0x06000D99 RID: 3481 RVA: 0x00025643 File Offset: 0x00023843
		public SingleplayerPersonalKillFeedWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x00025658 File Offset: 0x00023858
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._normalWidgetHeight <= 0f && base.ChildCount > 1)
			{
				this._normalWidgetHeight = base.GetChild(0).SuggestedHeight;
			}
			for (int i = 0; i < base.ChildCount; i++)
			{
				Widget child = base.GetChild(i);
				child.PositionYOffset = Mathf.Lerp(child.PositionYOffset, this.GetVerticalPositionOfChildByIndex(i), 0.2f);
			}
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x000256C8 File Offset: 0x000238C8
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.PositionYOffset = this.GetVerticalPositionOfChildByIndex(child.GetSiblingIndex());
			this.UpdateSpeedModifiers();
		}

		// Token: 0x06000D9C RID: 3484 RVA: 0x000256E9 File Offset: 0x000238E9
		private float GetVerticalPositionOfChildByIndex(int indexOfChild)
		{
			return -1f * this._normalWidgetHeight * (float)(base.ChildCount - indexOfChild - 1);
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x00025704 File Offset: 0x00023904
		private void UpdateSpeedModifiers()
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				SingleplayerPersonalKillFeedItemWidget singleplayerPersonalKillFeedItemWidget = base.GetChild(i) as SingleplayerPersonalKillFeedItemWidget;
				float num = MathF.Pow((float)(base.ChildCount - i), 0.33f);
				singleplayerPersonalKillFeedItemWidget.SetSpeedModifier(num);
			}
		}

		// Token: 0x0400062B RID: 1579
		private float _normalWidgetHeight = -1f;
	}
}
