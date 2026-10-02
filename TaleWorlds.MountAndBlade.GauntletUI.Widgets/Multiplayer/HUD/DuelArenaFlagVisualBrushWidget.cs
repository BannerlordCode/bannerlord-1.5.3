using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.HUD
{
	// Token: 0x020000C3 RID: 195
	public class DuelArenaFlagVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000A44 RID: 2628 RVA: 0x0001CEC8 File Offset: 0x0001B0C8
		public DuelArenaFlagVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0001CED8 File Offset: 0x0001B0D8
		private void UpdateVisual()
		{
			switch (this.ArenaType)
			{
			case 0:
				this.SetState("Infantry");
				return;
			case 1:
				this.SetState("Archery");
				return;
			case 2:
				this.SetState("Cavalry");
				return;
			default:
				this.SetState("Infantry");
				return;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x0001CF2F File Offset: 0x0001B12F
		// (set) Token: 0x06000A47 RID: 2631 RVA: 0x0001CF37 File Offset: 0x0001B137
		[Editor(false)]
		public int ArenaType
		{
			get
			{
				return this._arenaType;
			}
			set
			{
				if (this._arenaType != value)
				{
					this._arenaType = value;
					base.OnPropertyChanged(value, "ArenaType");
					this.UpdateVisual();
				}
			}
		}

		// Token: 0x040004A3 RID: 1187
		private int _arenaType = -1;
	}
}
