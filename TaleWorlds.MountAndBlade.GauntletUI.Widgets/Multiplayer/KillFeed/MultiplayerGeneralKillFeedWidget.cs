using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.KillFeed
{
	// Token: 0x020000C0 RID: 192
	public class MultiplayerGeneralKillFeedWidget : Widget
	{
		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x0001C605 File Offset: 0x0001A805
		// (set) Token: 0x06000A18 RID: 2584 RVA: 0x0001C60D File Offset: 0x0001A80D
		public float VerticalPaddingAmount { get; set; } = 3f;

		// Token: 0x06000A19 RID: 2585 RVA: 0x0001C616 File Offset: 0x0001A816
		public MultiplayerGeneralKillFeedWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0001C638 File Offset: 0x0001A838
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

		// Token: 0x06000A1B RID: 2587 RVA: 0x0001C6A8 File Offset: 0x0001A8A8
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.PositionYOffset = this.GetVerticalPositionOfChildByIndex(child.GetSiblingIndex());
			this.UpdateSpeedModifiers();
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0001C6C9 File Offset: 0x0001A8C9
		private float GetVerticalPositionOfChildByIndex(int indexOfChild)
		{
			return (this._normalWidgetHeight + this.VerticalPaddingAmount) * (float)(base.ChildCount - indexOfChild - 1);
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0001C6E4 File Offset: 0x0001A8E4
		private void UpdateSpeedModifiers()
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				MultiplayerGeneralKillFeedItemWidget multiplayerGeneralKillFeedItemWidget = base.GetChild(i) as MultiplayerGeneralKillFeedItemWidget;
				float num = MathF.Pow((float)(base.ChildCount - i), 0.33f);
				multiplayerGeneralKillFeedItemWidget.SetSpeedModifier(num);
			}
		}

		// Token: 0x04000491 RID: 1169
		private float _normalWidgetHeight = -1f;
	}
}
