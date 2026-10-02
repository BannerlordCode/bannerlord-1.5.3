using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.KillFeed.General
{
	// Token: 0x02000100 RID: 256
	public class SingleplayerGeneralKillFeedWidget : Widget
	{
		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06000DC9 RID: 3529 RVA: 0x00025C5A File Offset: 0x00023E5A
		// (set) Token: 0x06000DCA RID: 3530 RVA: 0x00025C62 File Offset: 0x00023E62
		public float VerticalPaddingAmount { get; set; } = 3f;

		// Token: 0x06000DCB RID: 3531 RVA: 0x00025C6B File Offset: 0x00023E6B
		public SingleplayerGeneralKillFeedWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x00025C8C File Offset: 0x00023E8C
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
				child.PositionYOffset = Mathf.Lerp(child.PositionYOffset, this.GetVerticalPositionOfChildByIndex(i), 0.35f);
			}
		}

		// Token: 0x06000DCD RID: 3533 RVA: 0x00025CFC File Offset: 0x00023EFC
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.PositionYOffset = this.GetVerticalPositionOfChildByIndex(child.GetSiblingIndex());
			this.UpdateSpeedModifiers();
		}

		// Token: 0x06000DCE RID: 3534 RVA: 0x00025D1D File Offset: 0x00023F1D
		private float GetVerticalPositionOfChildByIndex(int indexOfChild)
		{
			return (this._normalWidgetHeight + this.VerticalPaddingAmount) * (float)(base.ChildCount - indexOfChild - 1);
		}

		// Token: 0x06000DCF RID: 3535 RVA: 0x00025D38 File Offset: 0x00023F38
		private void UpdateSpeedModifiers()
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				SingleplayerGeneralKillFeedItemWidget singleplayerGeneralKillFeedItemWidget = base.GetChild(i) as SingleplayerGeneralKillFeedItemWidget;
				float num = MathF.Pow((float)(base.ChildCount - i), 0.33f);
				singleplayerGeneralKillFeedItemWidget.SetSpeedModifier(num);
			}
		}

		// Token: 0x04000641 RID: 1601
		private float _normalWidgetHeight = -1f;
	}
}
