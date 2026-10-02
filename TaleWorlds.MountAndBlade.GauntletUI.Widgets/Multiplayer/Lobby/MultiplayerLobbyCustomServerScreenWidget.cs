using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A3 RID: 163
	public class MultiplayerLobbyCustomServerScreenWidget : Widget
	{
		// Token: 0x060008D8 RID: 2264 RVA: 0x000198CC File Offset: 0x00017ACC
		public MultiplayerLobbyCustomServerScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060008D9 RID: 2265 RVA: 0x000198D5 File Offset: 0x00017AD5
		// (set) Token: 0x060008DA RID: 2266 RVA: 0x000198DD File Offset: 0x00017ADD
		[Editor(false)]
		public bool IsPartyLeader
		{
			get
			{
				return this._isPartyLeader;
			}
			set
			{
				if (this._isPartyLeader != value)
				{
					this._isPartyLeader = value;
					base.OnPropertyChanged(value, "IsPartyLeader");
				}
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060008DB RID: 2267 RVA: 0x000198FB File Offset: 0x00017AFB
		// (set) Token: 0x060008DC RID: 2268 RVA: 0x00019903 File Offset: 0x00017B03
		[Editor(false)]
		public bool IsInParty
		{
			get
			{
				return this._isInParty;
			}
			set
			{
				if (this._isInParty != value)
				{
					this._isInParty = value;
					base.OnPropertyChanged(value, "IsInParty");
				}
			}
		}

		// Token: 0x04000402 RID: 1026
		private bool _isPartyLeader;

		// Token: 0x04000403 RID: 1027
		private bool _isInParty;
	}
}
