using System;
using Galaxy.Api;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.PlatformService.GOG
{
	// Token: 0x0200000F RID: 15
	public static class SteamPlayerIdExtensions
	{
		// Token: 0x0600008B RID: 139 RVA: 0x000033EF File Offset: 0x000015EF
		public static PlayerId ToPlayerId(this GalaxyID galaxyID)
		{
			return new PlayerId(5, 0UL, galaxyID.ToUint64());
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000033FF File Offset: 0x000015FF
		public static GalaxyID ToGOGID(this PlayerId playerId)
		{
			if (playerId.IsValidGOGId())
			{
				return new GalaxyID(playerId.Part4);
			}
			return new GalaxyID(0UL);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x0000341D File Offset: 0x0000161D
		public static bool IsValidGOGId(this PlayerId playerId)
		{
			return playerId.IsValid && playerId.ProvidedType == PlayerIdProvidedTypes.GOG;
		}
	}
}
