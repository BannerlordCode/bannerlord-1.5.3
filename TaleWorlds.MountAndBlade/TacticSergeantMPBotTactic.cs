using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200017A RID: 378
	public class TacticSergeantMPBotTactic : TacticComponent
	{
		// Token: 0x060013B8 RID: 5048 RVA: 0x00048E9F File Offset: 0x0004709F
		public TacticSergeantMPBotTactic(Team team)
			: base(team)
		{
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x00048EA8 File Offset: 0x000470A8
		public override void TickOccasionally()
		{
			foreach (Formation formation in base.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					formation.AI.ResetBehaviorWeights();
					formation.AI.SetBehaviorWeight<BehaviorCharge>(1f);
					formation.AI.SetBehaviorWeight<BehaviorTacticalCharge>(1f);
					formation.AI.SetBehaviorWeight<BehaviorSergeantMPInfantry>(1f);
					formation.AI.SetBehaviorWeight<BehaviorSergeantMPRanged>(1f);
					formation.AI.SetBehaviorWeight<BehaviorSergeantMPMounted>(1f);
					formation.AI.SetBehaviorWeight<BehaviorSergeantMPMountedRanged>(1f);
					formation.AI.SetBehaviorWeight<BehaviorSergeantMPLastFlagLastStand>(1f);
				}
			}
		}
	}
}
