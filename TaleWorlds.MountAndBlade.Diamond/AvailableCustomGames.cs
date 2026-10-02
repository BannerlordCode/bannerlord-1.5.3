using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000EC RID: 236
	[Serializable]
	public class AvailableCustomGames
	{
		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600048F RID: 1167 RVA: 0x00005298 File Offset: 0x00003498
		// (set) Token: 0x06000490 RID: 1168 RVA: 0x000052A0 File Offset: 0x000034A0
		[JsonProperty]
		public List<GameServerEntry> CustomGameServerInfos { get; private set; }

		// Token: 0x06000491 RID: 1169 RVA: 0x000052A9 File Offset: 0x000034A9
		public AvailableCustomGames()
		{
			this.CustomGameServerInfos = new List<GameServerEntry>();
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x000052BC File Offset: 0x000034BC
		public AvailableCustomGames GetCustomGamesByPermission(int playerPermission)
		{
			AvailableCustomGames availableCustomGames = new AvailableCustomGames();
			foreach (GameServerEntry gameServerEntry in this.CustomGameServerInfos)
			{
				if (gameServerEntry.Permission <= playerPermission)
				{
					availableCustomGames.CustomGameServerInfos.Add(gameServerEntry);
				}
			}
			return availableCustomGames;
		}
	}
}
