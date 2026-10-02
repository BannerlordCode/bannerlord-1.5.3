using System;

namespace TaleWorlds.Core
{
	// Token: 0x02000072 RID: 114
	public abstract class RidingModel : MBGameModel<RidingModel>
	{
		// Token: 0x060007EC RID: 2028
		public abstract float CalculateAcceleration(in EquipmentElement mountElement, in EquipmentElement harnessElement, int ridingSkill);
	}
}
