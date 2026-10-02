using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x02000139 RID: 313
	public class KingdomDecisionPopupWidget : Widget
	{
		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001064 RID: 4196 RVA: 0x0002D2D5 File Offset: 0x0002B4D5
		// (set) Token: 0x06001065 RID: 4197 RVA: 0x0002D2DD File Offset: 0x0002B4DD
		public int DelayAfterKingsDecision { get; set; } = 5;

		// Token: 0x06001066 RID: 4198 RVA: 0x0002D2E6 File Offset: 0x0002B4E6
		public KingdomDecisionPopupWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x0002D301 File Offset: 0x0002B501
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._kingDecisionDoneTime != -1f && base.EventManager.Time - this._kingDecisionDoneTime > (float)this.DelayAfterKingsDecision)
			{
				this.ExecuteFinalDone();
			}
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x0002D338 File Offset: 0x0002B538
		private void ExecuteFinalDone()
		{
			base.EventFired("FinalDone", Array.Empty<object>());
			this._kingDecisionDoneTime = -1f;
			List<Widget> allChildrenRecursive = base.GetAllChildrenRecursive(null);
			for (int i = 0; i < allChildrenRecursive.Count; i++)
			{
				KingdomDecisionOptionWidget kingdomDecisionOptionWidget;
				if ((kingdomDecisionOptionWidget = allChildrenRecursive[i] as KingdomDecisionOptionWidget) != null)
				{
					kingdomDecisionOptionWidget.OnFinalDone();
				}
			}
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x0002D390 File Offset: 0x0002B590
		private void OnKingsDecisionDone()
		{
			this._kingDecisionDoneTime = base.EventManager.Time;
			List<Widget> allChildrenRecursive = base.GetAllChildrenRecursive(null);
			for (int i = 0; i < allChildrenRecursive.Count; i++)
			{
				KingdomDecisionOptionWidget kingdomDecisionOptionWidget;
				if ((kingdomDecisionOptionWidget = allChildrenRecursive[i] as KingdomDecisionOptionWidget) != null)
				{
					kingdomDecisionOptionWidget.OnKingsDecisionDone();
				}
			}
		}

		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x0600106A RID: 4202 RVA: 0x0002D3DD File Offset: 0x0002B5DD
		// (set) Token: 0x0600106B RID: 4203 RVA: 0x0002D3E5 File Offset: 0x0002B5E5
		[Editor(false)]
		public bool IsKingsDecisionDone
		{
			get
			{
				return this._isKingsDecisionDone;
			}
			set
			{
				if (this._isKingsDecisionDone != value)
				{
					this._isKingsDecisionDone = value;
					base.OnPropertyChanged(value, "IsKingsDecisionDone");
					if (value)
					{
						this.OnKingsDecisionDone();
					}
				}
			}
		}

		// Token: 0x0400077A RID: 1914
		private float _kingDecisionDoneTime = -1f;

		// Token: 0x0400077B RID: 1915
		private bool _isKingsDecisionDone;
	}
}
