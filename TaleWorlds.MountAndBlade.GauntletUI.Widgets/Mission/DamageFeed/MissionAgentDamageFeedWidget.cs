using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.DamageFeed
{
	// Token: 0x02000105 RID: 261
	public class MissionAgentDamageFeedWidget : Widget
	{
		// Token: 0x06000E0F RID: 3599 RVA: 0x00026B2A File Offset: 0x00024D2A
		public MissionAgentDamageFeedWidget(UIContext context)
			: base(context)
		{
			this._feedItemQueue = new Queue<MissionAgentDamageFeedItemWidget>();
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00026B48 File Offset: 0x00024D48
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			MissionAgentDamageFeedItemWidget missionAgentDamageFeedItemWidget = (MissionAgentDamageFeedItemWidget)child;
			this._feedItemQueue.Enqueue(missionAgentDamageFeedItemWidget);
			this.UpdateSpeedModifiers();
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00026B75 File Offset: 0x00024D75
		protected override void OnBeforeChildRemoved(Widget child)
		{
			this._activeFeedItem = null;
			base.OnBeforeChildRemoved(child);
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00026B88 File Offset: 0x00024D88
		protected override void OnUpdate(float dt)
		{
			if (this._activeFeedItem == null && this._feedItemQueue.Count > 0)
			{
				MissionAgentDamageFeedItemWidget missionAgentDamageFeedItemWidget = this._feedItemQueue.Dequeue();
				this._activeFeedItem = missionAgentDamageFeedItemWidget;
				this._activeFeedItem.ShowFeed();
			}
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00026BCC File Offset: 0x00024DCC
		private void UpdateSpeedModifiers()
		{
			if (base.ChildCount > this._speedUpWidgetLimit)
			{
				float num = (float)(base.ChildCount - this._speedUpWidgetLimit) / 3f + 1f;
				for (int i = 0; i < base.ChildCount - this._speedUpWidgetLimit; i++)
				{
					((MissionAgentDamageFeedItemWidget)base.GetChild(i)).SetSpeedModifier(num);
				}
			}
		}

		// Token: 0x04000665 RID: 1637
		private int _speedUpWidgetLimit = 1;

		// Token: 0x04000666 RID: 1638
		private readonly Queue<MissionAgentDamageFeedItemWidget> _feedItemQueue;

		// Token: 0x04000667 RID: 1639
		private MissionAgentDamageFeedItemWidget _activeFeedItem;
	}
}
