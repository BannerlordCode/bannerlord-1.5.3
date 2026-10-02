using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000023 RID: 35
	public class MultiplayerInfo
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00007F98 File Offset: 0x00006198
		public MultiplayerData MultiplayerDataValues
		{
			get
			{
				return this.multiplayerDataValues;
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00007FA0 File Offset: 0x000061A0
		public MultiplayerInfo()
		{
			this.multiplayerDataValues = new MultiplayerData();
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00007FB3 File Offset: 0x000061B3
		public bool IsMultiplayerTeamAvailable(int peerNo, int teamNo)
		{
			return true;
		}

		// Token: 0x04000068 RID: 104
		protected MultiplayerData multiplayerDataValues;
	}
}
