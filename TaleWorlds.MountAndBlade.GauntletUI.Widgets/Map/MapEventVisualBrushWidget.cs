using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map
{
	// Token: 0x0200011A RID: 282
	public class MapEventVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000F02 RID: 3842 RVA: 0x00029709 File Offset: 0x00027909
		public MapEventVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00029720 File Offset: 0x00027920
		private void UpdateVisual(int type)
		{
			if (this._initialUpdate)
			{
				this.RegisterBrushStatesOfWidget();
				this._initialUpdate = false;
			}
			switch (type)
			{
			case 1:
				this.SetState("Raid");
				return;
			case 2:
				this.SetState("Siege");
				return;
			case 3:
				this.SetState("Battle");
				return;
			case 4:
				this.SetState("Rebellion");
				return;
			case 5:
				this.SetState("SallyOut");
				return;
			default:
				this.SetState("None");
				return;
			}
		}

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x06000F04 RID: 3844 RVA: 0x000297A7 File Offset: 0x000279A7
		// (set) Token: 0x06000F05 RID: 3845 RVA: 0x000297AF File Offset: 0x000279AF
		[Editor(false)]
		public int MapEventType
		{
			get
			{
				return this._mapEventType;
			}
			set
			{
				if (this._mapEventType != value)
				{
					this._mapEventType = value;
					this.UpdateVisual(value);
				}
			}
		}

		// Token: 0x040006D9 RID: 1753
		private bool _initialUpdate = true;

		// Token: 0x040006DA RID: 1754
		private int _mapEventType = -1;
	}
}
