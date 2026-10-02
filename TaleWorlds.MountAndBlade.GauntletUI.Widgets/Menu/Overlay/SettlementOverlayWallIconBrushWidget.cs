using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.Overlay
{
	// Token: 0x02000116 RID: 278
	public class SettlementOverlayWallIconBrushWidget : BrushWidget
	{
		// Token: 0x06000EF1 RID: 3825 RVA: 0x00029433 File Offset: 0x00027633
		public SettlementOverlayWallIconBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0002943C File Offset: 0x0002763C
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.SetState(this.WallsLevel.ToString());
		}

		// Token: 0x17000557 RID: 1367
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x00029464 File Offset: 0x00027664
		// (set) Token: 0x06000EF4 RID: 3828 RVA: 0x0002946C File Offset: 0x0002766C
		[Editor(false)]
		public int WallsLevel
		{
			get
			{
				return this._wallsLevel;
			}
			set
			{
				if (this._wallsLevel != value)
				{
					this._wallsLevel = value;
					base.OnPropertyChanged(value, "WallsLevel");
				}
			}
		}

		// Token: 0x040006D2 RID: 1746
		private int _wallsLevel;
	}
}
