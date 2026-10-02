using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission
{
	// Token: 0x020000E2 RID: 226
	public class FormationMarkerTeamTypeBrushWidget : BrushWidget
	{
		// Token: 0x06000BC6 RID: 3014 RVA: 0x00020F4D File Offset: 0x0001F14D
		public FormationMarkerTeamTypeBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x00020F56 File Offset: 0x0001F156
		private void UpdateState()
		{
			this.RegisterBrushStatesOfWidget();
			if (this.TeamType == 0)
			{
				this.SetState("Player");
				return;
			}
			if (this.TeamType == 1)
			{
				this.SetState("Ally");
				return;
			}
			this.SetState("Enemy");
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x00020F92 File Offset: 0x0001F192
		// (set) Token: 0x06000BC9 RID: 3017 RVA: 0x00020F9A File Offset: 0x0001F19A
		public int TeamType
		{
			get
			{
				return this._teamType;
			}
			set
			{
				if (this._teamType != value)
				{
					this._teamType = value;
					base.OnPropertyChanged(value, "TeamType");
					this.UpdateState();
				}
			}
		}

		// Token: 0x04000555 RID: 1365
		private int _teamType;
	}
}
