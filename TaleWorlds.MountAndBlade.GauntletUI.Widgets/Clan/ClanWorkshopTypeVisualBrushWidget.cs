using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Clan
{
	// Token: 0x0200017B RID: 379
	public class ClanWorkshopTypeVisualBrushWidget : BrushWidget
	{
		// Token: 0x060013D0 RID: 5072 RVA: 0x0003605D File Offset: 0x0003425D
		public ClanWorkshopTypeVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x00036071 File Offset: 0x00034271
		private void SetVisualState(string type)
		{
			this.RegisterBrushStatesOfWidget();
			this.SetState(type);
		}

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x060013D2 RID: 5074 RVA: 0x00036080 File Offset: 0x00034280
		// (set) Token: 0x060013D3 RID: 5075 RVA: 0x00036088 File Offset: 0x00034288
		[Editor(false)]
		public string WorkshopType
		{
			get
			{
				return this._workshopType;
			}
			set
			{
				if (this._workshopType != value)
				{
					this._workshopType = value;
					base.OnPropertyChanged<string>(value, "WorkshopType");
					this.SetVisualState(value);
				}
			}
		}

		// Token: 0x04000902 RID: 2306
		private string _workshopType = "";
	}
}
