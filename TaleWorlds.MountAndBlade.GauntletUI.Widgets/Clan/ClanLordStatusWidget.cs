using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Clan
{
	// Token: 0x02000179 RID: 377
	public class ClanLordStatusWidget : Widget
	{
		// Token: 0x060013CB RID: 5067 RVA: 0x00035F81 File Offset: 0x00034181
		public ClanLordStatusWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00035F94 File Offset: 0x00034194
		private void SetVisualState(int type)
		{
			switch (type)
			{
			case 0:
				this.SetState("Dead");
				return;
			case 1:
				this.SetState("Married");
				return;
			case 2:
				this.SetState("Pregnant");
				return;
			case 3:
				this.SetState("InBattle");
				return;
			case 4:
				this.SetState("InSiege");
				return;
			case 5:
				this.SetState("Child");
				return;
			case 6:
				this.SetState("Prisoner");
				return;
			case 7:
				this.SetState("Sick");
				return;
			default:
				return;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x060013CD RID: 5069 RVA: 0x00036027 File Offset: 0x00034227
		// (set) Token: 0x060013CE RID: 5070 RVA: 0x0003602F File Offset: 0x0003422F
		[Editor(false)]
		public int StatusType
		{
			get
			{
				return this._statusType;
			}
			set
			{
				if (this._statusType != value)
				{
					this._statusType = value;
					base.OnPropertyChanged(value, "StatusType");
					this.SetVisualState(value);
				}
			}
		}

		// Token: 0x04000901 RID: 2305
		private int _statusType = -1;
	}
}
