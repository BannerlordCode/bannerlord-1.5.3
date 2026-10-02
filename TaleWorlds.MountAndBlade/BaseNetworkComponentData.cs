using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002FA RID: 762
	public class BaseNetworkComponentData : UdpNetworkComponent
	{
		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06002C00 RID: 11264 RVA: 0x000AA06E File Offset: 0x000A826E
		// (set) Token: 0x06002C01 RID: 11265 RVA: 0x000AA076 File Offset: 0x000A8276
		public int CurrentBattleIndex { get; private set; }

		// Token: 0x06002C02 RID: 11266 RVA: 0x000AA07F File Offset: 0x000A827F
		public void UpdateCurrentBattleIndex(int currentBattleIndex)
		{
			this.CurrentBattleIndex = currentBattleIndex;
		}

		// Token: 0x0400110F RID: 4367
		public const float MaxIntermissionStateTime = 240f;
	}
}
