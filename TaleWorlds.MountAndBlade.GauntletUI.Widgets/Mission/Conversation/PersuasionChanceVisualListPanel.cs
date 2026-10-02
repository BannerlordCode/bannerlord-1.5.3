using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.Conversation
{
	// Token: 0x02000106 RID: 262
	public class PersuasionChanceVisualListPanel : ListPanel
	{
		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06000E14 RID: 3604 RVA: 0x00026C2C File Offset: 0x00024E2C
		// (set) Token: 0x06000E15 RID: 3605 RVA: 0x00026C34 File Offset: 0x00024E34
		public bool IsFailChance { get; set; }

		// Token: 0x06000E16 RID: 3606 RVA: 0x00026C3D File Offset: 0x00024E3D
		public PersuasionChanceVisualListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00026C46 File Offset: 0x00024E46
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			base.IsVisible = !this.IsFailChance && this.ChanceValue > 0;
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x00026C69 File Offset: 0x00024E69
		// (set) Token: 0x06000E19 RID: 3609 RVA: 0x00026C71 File Offset: 0x00024E71
		public int ChanceValue
		{
			get
			{
				return this._chanceValue;
			}
			set
			{
				if (this._chanceValue != value)
				{
					this._chanceValue = value;
					base.OnPropertyChanged(value, "ChanceValue");
				}
			}
		}

		// Token: 0x04000669 RID: 1641
		private int _chanceValue;
	}
}
