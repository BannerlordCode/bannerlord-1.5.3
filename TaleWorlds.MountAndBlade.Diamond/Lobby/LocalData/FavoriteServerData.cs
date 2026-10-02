using System;

namespace TaleWorlds.MountAndBlade.Diamond.Lobby.LocalData
{
	// Token: 0x02000174 RID: 372
	public class FavoriteServerData : MultiplayerLocalData
	{
		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000A6D RID: 2669 RVA: 0x00010D8B File Offset: 0x0000EF8B
		// (set) Token: 0x06000A6E RID: 2670 RVA: 0x00010D93 File Offset: 0x0000EF93
		public string Address { get; set; }

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x00010D9C File Offset: 0x0000EF9C
		// (set) Token: 0x06000A70 RID: 2672 RVA: 0x00010DA4 File Offset: 0x0000EFA4
		public int Port { get; set; }

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000A71 RID: 2673 RVA: 0x00010DAD File Offset: 0x0000EFAD
		// (set) Token: 0x06000A72 RID: 2674 RVA: 0x00010DB5 File Offset: 0x0000EFB5
		public string GameType { get; set; }

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x00010DBE File Offset: 0x0000EFBE
		// (set) Token: 0x06000A74 RID: 2676 RVA: 0x00010DC6 File Offset: 0x0000EFC6
		public string Name { get; set; }

		// Token: 0x06000A75 RID: 2677 RVA: 0x00010DCF File Offset: 0x0000EFCF
		private FavoriteServerData()
		{
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00010DD7 File Offset: 0x0000EFD7
		public static FavoriteServerData CreateFrom(GameServerEntry serverEntry)
		{
			if (serverEntry == null)
			{
				return null;
			}
			return new FavoriteServerData
			{
				Address = serverEntry.Address,
				Port = serverEntry.Port,
				GameType = serverEntry.GameType,
				Name = serverEntry.ServerName
			};
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00010E14 File Offset: 0x0000F014
		public override bool HasSameContentWith(MultiplayerLocalData other)
		{
			FavoriteServerData favoriteServerData;
			return (favoriteServerData = other as FavoriteServerData) != null && (this.Address == favoriteServerData.Address && this.Port == favoriteServerData.Port && this.GameType == favoriteServerData.GameType) && this.Name == favoriteServerData.Name;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00010E74 File Offset: 0x0000F074
		public bool HasSameContentWith(GameServerEntry serverEntry)
		{
			return this.Address == serverEntry.Address && this.Port == serverEntry.Port && this.GameType == serverEntry.GameType && this.Name == serverEntry.ServerName;
		}
	}
}
