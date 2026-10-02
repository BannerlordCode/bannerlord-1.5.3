using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.Order
{
	// Token: 0x020000E7 RID: 231
	public class OrderFormationClassVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000C05 RID: 3077 RVA: 0x000215A3 File Offset: 0x0001F7A3
		public OrderFormationClassVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x000215B4 File Offset: 0x0001F7B4
		private void UpdateVisual()
		{
			switch (this.FormationClassValue)
			{
			case 0:
				this.SetState("Infantry");
				return;
			case 1:
				this.SetState("Ranged");
				return;
			case 2:
				this.SetState("Cavalry");
				return;
			case 3:
				this.SetState("HorseArcher");
				return;
			default:
				this.SetState("Infantry");
				return;
			}
		}

		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06000C07 RID: 3079 RVA: 0x0002161B File Offset: 0x0001F81B
		// (set) Token: 0x06000C08 RID: 3080 RVA: 0x00021623 File Offset: 0x0001F823
		[Editor(false)]
		public int FormationClassValue
		{
			get
			{
				return this._formationClassValue;
			}
			set
			{
				if (this._formationClassValue != value)
				{
					this._formationClassValue = value;
					base.OnPropertyChanged(value, "FormationClassValue");
					this.UpdateVisual();
				}
			}
		}

		// Token: 0x04000573 RID: 1395
		private int _formationClassValue = -1;
	}
}
