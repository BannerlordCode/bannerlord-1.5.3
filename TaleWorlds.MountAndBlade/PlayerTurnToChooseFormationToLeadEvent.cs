using System;
using System.Collections.Generic;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000277 RID: 631
	// (Invoke) Token: 0x06002386 RID: 9094
	public delegate void PlayerTurnToChooseFormationToLeadEvent(Dictionary<int, Agent> lockedFormationIndicesAndSergeants, List<int> remainingFormationIndices);
}
