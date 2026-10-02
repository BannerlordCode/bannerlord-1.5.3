using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData
{
	// Token: 0x02000175 RID: 373
	public class FavoriteServerDataContainer : MultiplayerLocalDataContainer<FavoriteServerData>
	{
		// Token: 0x06000A79 RID: 2681 RVA: 0x00010EC8 File Offset: 0x0000F0C8
		protected override string GetSaveDirectoryName()
		{
			return "Data";
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00010ECF File Offset: 0x0000F0CF
		protected override string GetSaveFileName()
		{
			return "FavoriteServers.json";
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00010ED8 File Offset: 0x0000F0D8
		public bool TryGetServerData(GameServerEntry serverEntry, out FavoriteServerData favoriteServerData)
		{
			favoriteServerData = null;
			MBReadOnlyList<FavoriteServerData> entries = base.GetEntries();
			for (int i = 0; i < entries.Count; i++)
			{
				FavoriteServerData favoriteServerData2 = entries[i];
				if (favoriteServerData2.HasSameContentWith(serverEntry))
				{
					favoriteServerData = favoriteServerData2;
					return true;
				}
			}
			return false;
		}
	}
}
