using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x02000136 RID: 310
	public class KingdomClanTypeVisualBrushWidget : BrushWidget
	{
		// Token: 0x06001040 RID: 4160 RVA: 0x0002CF32 File Offset: 0x0002B132
		public KingdomClanTypeVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001041 RID: 4161 RVA: 0x0002CF44 File Offset: 0x0002B144
		private void UpdateTypeVisual()
		{
			if (this.Type == 0)
			{
				this.SetState("Normal");
				return;
			}
			if (this.Type == 1)
			{
				this.SetState("Leader");
				return;
			}
			if (this.Type == 2)
			{
				this.SetState("Mercenary");
				return;
			}
			Debug.FailedAssert("This clan type is not defined in widget", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Kingdom\\KingdomClanTypeVisualBrushWidget.cs", "UpdateTypeVisual", 37);
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x0002CFA5 File Offset: 0x0002B1A5
		// (set) Token: 0x06001043 RID: 4163 RVA: 0x0002CFAD File Offset: 0x0002B1AD
		[Editor(false)]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (this._type != value)
				{
					this._type = value;
					base.OnPropertyChanged(value, "Type");
					this.UpdateTypeVisual();
				}
			}
		}

		// Token: 0x0400076A RID: 1898
		private int _type = -1;
	}
}
