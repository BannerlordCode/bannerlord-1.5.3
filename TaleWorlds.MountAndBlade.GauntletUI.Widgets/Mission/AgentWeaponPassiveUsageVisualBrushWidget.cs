using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000D9 RID: 217
	public class AgentWeaponPassiveUsageVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000B1E RID: 2846 RVA: 0x0001F4A1 File Offset: 0x0001D6A1
		public AgentWeaponPassiveUsageVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0001F4B4 File Offset: 0x0001D6B4
		private void UpdateVisualState()
		{
			if (this._firstUpdate)
			{
				this.RegisterBrushStatesOfWidget();
				this._firstUpdate = false;
			}
			switch (this.CouchLanceState)
			{
			case 0:
				base.IsVisible = false;
				return;
			case 1:
				base.IsVisible = true;
				this.SetState("ConditionsNotMet");
				return;
			case 2:
				base.IsVisible = true;
				this.SetState("Possible");
				return;
			case 3:
				this.SetState("Active");
				base.IsVisible = true;
				return;
			default:
				return;
			}
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x0001F534 File Offset: 0x0001D734
		// (set) Token: 0x06000B21 RID: 2849 RVA: 0x0001F53C File Offset: 0x0001D73C
		[Editor(false)]
		public int CouchLanceState
		{
			get
			{
				return this._couchLanceState;
			}
			set
			{
				if (this._couchLanceState != value)
				{
					this._couchLanceState = value;
					base.OnPropertyChanged(value, "CouchLanceState");
					this.UpdateVisualState();
				}
			}
		}

		// Token: 0x04000509 RID: 1289
		private bool _firstUpdate;

		// Token: 0x0400050A RID: 1290
		private int _couchLanceState = -1;
	}
}
