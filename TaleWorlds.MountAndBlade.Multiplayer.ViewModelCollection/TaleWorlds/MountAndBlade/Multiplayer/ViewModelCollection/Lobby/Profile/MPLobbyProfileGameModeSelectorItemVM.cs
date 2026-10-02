using System;
using TaleWorlds.Core.ViewModelCollection.Selector;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Profile
{
	// Token: 0x0200003A RID: 58
	public class MPLobbyProfileGameModeSelectorItemVM : SelectorItemVM
	{
		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600054F RID: 1359 RVA: 0x000123DA File Offset: 0x000105DA
		// (set) Token: 0x06000550 RID: 1360 RVA: 0x000123E2 File Offset: 0x000105E2
		public string GameModeCode { get; private set; }

		// Token: 0x06000551 RID: 1361 RVA: 0x000123EB File Offset: 0x000105EB
		public MPLobbyProfileGameModeSelectorItemVM(string gameModeCode, TextObject gameModeName)
			: base(gameModeName)
		{
			this.GameModeCode = gameModeCode;
		}
	}
}
