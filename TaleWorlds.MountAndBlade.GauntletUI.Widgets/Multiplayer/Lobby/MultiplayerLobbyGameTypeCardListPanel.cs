using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A5 RID: 165
	public class MultiplayerLobbyGameTypeCardListPanel : ListPanel
	{
		// Token: 0x060008E6 RID: 2278 RVA: 0x00019B0F File Offset: 0x00017D0F
		public MultiplayerLobbyGameTypeCardListPanel(UIContext context)
			: base(context)
		{
			this._cardButtons = new List<MultiplayerLobbyGameTypeCardButtonWidget>();
		}

		// Token: 0x04000407 RID: 1031
		private List<MultiplayerLobbyGameTypeCardButtonWidget> _cardButtons;
	}
}
