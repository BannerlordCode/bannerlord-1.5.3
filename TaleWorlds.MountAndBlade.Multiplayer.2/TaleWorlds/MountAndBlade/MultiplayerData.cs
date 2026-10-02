using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000020 RID: 32
	public class MultiplayerData : MBMultiplayerData
	{
		// Token: 0x060001A7 RID: 423 RVA: 0x00007A4E File Offset: 0x00005C4E
		public MultiplayerData()
		{
			new List<NetworkCommunicator>();
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00007A64 File Offset: 0x00005C64
		public bool IsMultiplayerTeamAvailable(int peerNo, int teamNo)
		{
			return false;
		}

		// Token: 0x04000067 RID: 103
		public readonly int AutoTeamBalanceLimit = 50;
	}
}
