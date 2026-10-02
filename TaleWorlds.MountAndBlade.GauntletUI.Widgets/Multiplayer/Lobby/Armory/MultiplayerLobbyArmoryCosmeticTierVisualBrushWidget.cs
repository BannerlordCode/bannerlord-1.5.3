using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000BA RID: 186
	public class MultiplayerLobbyArmoryCosmeticTierVisualBrushWidget : BrushWidget
	{
		// Token: 0x060009E3 RID: 2531 RVA: 0x0001BE07 File Offset: 0x0001A007
		public MultiplayerLobbyArmoryCosmeticTierVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x0001BE18 File Offset: 0x0001A018
		private void UpdateVisual()
		{
			switch (this._rarity)
			{
			case 0:
			case 1:
				this.SetState("Common");
				return;
			case 2:
				this.SetState("Rare");
				return;
			case 3:
				this.SetState("Unique");
				return;
			default:
				return;
			}
		}

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0001BE66 File Offset: 0x0001A066
		// (set) Token: 0x060009E6 RID: 2534 RVA: 0x0001BE6E File Offset: 0x0001A06E
		[Editor(false)]
		public int Rarity
		{
			get
			{
				return this._rarity;
			}
			set
			{
				if (this._rarity != value)
				{
					this._rarity = value;
					base.OnPropertyChanged(value, "Rarity");
					this.UpdateVisual();
				}
			}
		}

		// Token: 0x04000476 RID: 1142
		private int _rarity = -1;
	}
}
