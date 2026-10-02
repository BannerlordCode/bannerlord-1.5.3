using System;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000017 RID: 23
	public static class MultiplayerSpectatorHelper
	{
		// Token: 0x0600014A RID: 330 RVA: 0x000060A4 File Offset: 0x000042A4
		public static bool IsLocalPeerSpectator()
		{
			return SpectatorHelper.IsLocalPeerSpectator();
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000060AB File Offset: 0x000042AB
		public static bool ShouldShowBothTeamsData()
		{
			return MultiplayerSpectatorHelper.IsStreamerModeActive();
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000060B2 File Offset: 0x000042B2
		public static bool IsStreamerModeActive()
		{
			return MultiplayerSpectatorHelper.IsLocalPeerSpectator() && MultiplayerOptions.OptionType.StreamerModeEnabled.GetBoolValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
		}
	}
}
