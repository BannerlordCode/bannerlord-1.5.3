using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Kingdom
{
	// Token: 0x02000137 RID: 311
	public class KingdomDecisionFactionTypeVisualBrushWidget : BrushWidget
	{
		// Token: 0x06001044 RID: 4164 RVA: 0x0002CFD1 File Offset: 0x0002B1D1
		public KingdomDecisionFactionTypeVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001045 RID: 4165 RVA: 0x0002CFE5 File Offset: 0x0002B1E5
		private void SetVisualState(string type)
		{
			this.RegisterBrushStatesOfWidget();
			this.SetState(type);
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x0002CFF4 File Offset: 0x0002B1F4
		// (set) Token: 0x06001047 RID: 4167 RVA: 0x0002CFFC File Offset: 0x0002B1FC
		[Editor(false)]
		public string FactionName
		{
			get
			{
				return this._factionName;
			}
			set
			{
				if (this._factionName != value)
				{
					this._factionName = value;
					base.OnPropertyChanged<string>(value, "FactionName");
					if (value != null)
					{
						this.SetVisualState(value);
					}
				}
			}
		}

		// Token: 0x0400076B RID: 1899
		private string _factionName = "";
	}
}
