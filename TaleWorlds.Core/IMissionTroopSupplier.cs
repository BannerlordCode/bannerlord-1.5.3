using System;
using System.Collections.Generic;

namespace TaleWorlds.Core
{
	// Token: 0x020000BC RID: 188
	public interface IMissionTroopSupplier
	{
		// Token: 0x060009FB RID: 2555
		IEnumerable<IAgentOriginBase> SupplyTroops(int numberToAllocate);

		// Token: 0x060009FC RID: 2556
		IAgentOriginBase SupplyOneTroop();

		// Token: 0x060009FD RID: 2557
		IEnumerable<IAgentOriginBase> GetAllTroops();

		// Token: 0x060009FE RID: 2558
		BasicCharacterObject GetGeneralCharacter();

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060009FF RID: 2559
		int NumRemovedTroops { get; }

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000A00 RID: 2560
		int NumTroopsNotSupplied { get; }

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000A01 RID: 2561
		bool AnyTroopRemainsToBeSupplied { get; }

		// Token: 0x06000A02 RID: 2562
		int GetNumberOfPlayerControllableTroops();
	}
}
