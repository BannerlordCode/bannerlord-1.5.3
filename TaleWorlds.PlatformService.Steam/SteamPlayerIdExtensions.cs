using System;
using Steamworks;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.PlatformService.Steam
{
	// Token: 0x02000007 RID: 7
	public static class SteamPlayerIdExtensions
	{
		// Token: 0x06000072 RID: 114 RVA: 0x00002E25 File Offset: 0x00001025
		public static PlayerId ToPlayerId(this CSteamID steamId)
		{
			return new PlayerId(2, 0UL, steamId.m_SteamID);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00002E35 File Offset: 0x00001035
		public static CSteamID ToSteamId(this PlayerId playerId)
		{
			if (playerId.IsValidSteamId())
			{
				return new CSteamID(playerId.Part4);
			}
			return new CSteamID(0UL);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002E53 File Offset: 0x00001053
		public static bool IsValidSteamId(this PlayerId playerId)
		{
			return playerId.IsValid && playerId.ProvidedType == PlayerIdProvidedTypes.Steam;
		}
	}
}
