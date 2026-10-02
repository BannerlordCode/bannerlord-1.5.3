using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200011F RID: 287
	public interface IBadgeComponent
	{
		// Token: 0x1700022E RID: 558
		// (get) Token: 0x0600067D RID: 1661
		Dictionary<ValueTuple<PlayerId, string, string>, int> DataDictionary { get; }

		// Token: 0x0600067E RID: 1662
		void OnPlayerJoin(PlayerData playerData);

		// Token: 0x0600067F RID: 1663
		void OnStartingNextBattle();
	}
}
