using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x0200008B RID: 139
	public class MultiplayerIntermissionNextMapImageWidget : Widget
	{
		// Token: 0x060007CF RID: 1999 RVA: 0x00016DBA File Offset: 0x00014FBA
		public MultiplayerIntermissionNextMapImageWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00016DC3 File Offset: 0x00014FC3
		private void UpdateMapImage()
		{
			if (string.IsNullOrEmpty(this.MapID))
			{
				return;
			}
			base.Sprite = base.Context.SpriteData.GetSprite(this.MapID);
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x00016DEF File Offset: 0x00014FEF
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x00016DF7 File Offset: 0x00014FF7
		[DataSourceProperty]
		public string MapID
		{
			get
			{
				return this._mapID;
			}
			set
			{
				if (value != this._mapID)
				{
					this._mapID = value;
					base.OnPropertyChanged<string>(value, "MapID");
					this.UpdateMapImage();
				}
			}
		}

		// Token: 0x0400036B RID: 875
		private string _mapID;
	}
}
