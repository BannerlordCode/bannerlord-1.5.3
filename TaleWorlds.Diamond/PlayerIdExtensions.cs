using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000024 RID: 36
	public static class PlayerIdExtensions
	{
		// Token: 0x060000CB RID: 203 RVA: 0x0000336B File Offset: 0x0000156B
		public static PeerId ConvertToPeerId(this PlayerId playerId)
		{
			return new PeerId(playerId.ToByteArray());
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00003379 File Offset: 0x00001579
		public static PlayerId ConvertToPlayerId(this PeerId peerId)
		{
			return new PlayerId(peerId.ToByteArray());
		}
	}
}
