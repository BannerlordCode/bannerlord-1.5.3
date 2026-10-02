using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.KillFeed
{
	// Token: 0x020000C2 RID: 194
	public class MultiplayerPersonalKillFeedWidget : Widget
	{
		// Token: 0x06000A3F RID: 2623 RVA: 0x0001CDC4 File Offset: 0x0001AFC4
		public MultiplayerPersonalKillFeedWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0001CDD8 File Offset: 0x0001AFD8
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

		// Token: 0x06000A41 RID: 2625 RVA: 0x0001CE48 File Offset: 0x0001B048
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			child.PositionYOffset = this.GetVerticalPositionOfChildByIndex(child.GetSiblingIndex());
			this.UpdateSpeedModifiers();
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0001CE69 File Offset: 0x0001B069
		private float GetVerticalPositionOfChildByIndex(int indexOfChild)
		{
			return -1f * this._normalWidgetHeight * (float)(base.ChildCount - indexOfChild - 1);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0001CE84 File Offset: 0x0001B084
		private void UpdateSpeedModifiers()
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				MultiplayerPersonalKillFeedItemWidget multiplayerPersonalKillFeedItemWidget = base.GetChild(i) as MultiplayerPersonalKillFeedItemWidget;
				float num = MathF.Pow((float)(base.ChildCount - i), 0.33f);
				multiplayerPersonalKillFeedItemWidget.SetSpeedModifier(num);
			}
		}

		// Token: 0x040004A2 RID: 1186
		private float _normalWidgetHeight = -1f;
	}
}
