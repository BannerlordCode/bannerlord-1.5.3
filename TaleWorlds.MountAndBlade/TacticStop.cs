using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200017B RID: 379
	public class TacticStop : TacticComponent
	{
		// Token: 0x060013BA RID: 5050 RVA: 0x00048F88 File Offset: 0x00047188
		public TacticStop(Team team)
			: base(team)
		{
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00048F94 File Offset: 0x00047194
		public override void TickOccasionally()
		{
			foreach (Formation formation in base.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					formation.AI.ResetBehaviorWeights();
					formation.AI.SetBehaviorWeight<BehaviorStop>(1f);
				}
			}
			base.TickOccasionally();
		}
	}
}
