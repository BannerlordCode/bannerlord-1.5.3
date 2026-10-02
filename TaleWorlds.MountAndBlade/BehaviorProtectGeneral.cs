using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000123 RID: 291
	public class BehaviorProtectGeneral : BehaviorComponent
	{
		// Token: 0x06000E7E RID: 3710 RVA: 0x00021750 File Offset: 0x0001F950
		public BehaviorProtectGeneral(Formation formation)
			: base(formation)
		{
			base.CurrentOrder = MovementOrder.MovementOrderFollow((formation.Team.GeneralsFormation != null && formation.Team.GeneralsFormation.CountOfUnits > 0) ? formation.Team.GeneralsFormation.GetFirstUnit() : Mission.Current.MainAgent);
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x000217AB File Offset: 0x0001F9AB
		public override void TickOccasionally()
		{
			base.Formation.SetMovementOrder(base.CurrentOrder);
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000E80 RID: 3712 RVA: 0x000217BE File Offset: 0x0001F9BE
		public override float NavmeshlessTargetPositionPenalty
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x06000E81 RID: 3713 RVA: 0x000217C8 File Offset: 0x0001F9C8
		protected override float GetAiWeight()
		{
			if ((base.Formation.Team.GeneralsFormation != null && base.Formation.Team.GeneralsFormation.CountOfUnits > 0) || (base.Formation.Team.IsPlayerTeam && base.Formation.Team.IsPlayerGeneral && Mission.Current.MainAgent != null))
			{
				return 1f;
			}
			return 0f;
		}

		// Token: 0x06000E82 RID: 3714 RVA: 0x0002183C File Offset: 0x0001FA3C
		public override void OnAgentRemoved(Agent agent)
		{
			if (base.CurrentOrder._targetAgent == agent)
			{
				base.CurrentOrder = MovementOrder.MovementOrderNull;
			}
		}
	}
}
