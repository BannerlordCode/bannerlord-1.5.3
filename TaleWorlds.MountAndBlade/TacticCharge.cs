using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200016C RID: 364
	public class TacticCharge : TacticComponent
	{
		// Token: 0x060012FD RID: 4861 RVA: 0x0003D747 File Offset: 0x0003B947
		public TacticCharge(Team team)
			: base(team)
		{
		}

		// Token: 0x060012FE RID: 4862 RVA: 0x0003D750 File Offset: 0x0003B950
		public override void TickOccasionally()
		{
			foreach (Formation formation in base.FormationsIncludingEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					formation.AI.ResetBehaviorWeights();
					TacticComponent.SetDefaultBehaviorWeights(formation);
					formation.AI.SetBehaviorWeight<BehaviorCharge>(10000f);
				}
			}
			base.TickOccasionally();
		}

		// Token: 0x060012FF RID: 4863 RVA: 0x0003D7D0 File Offset: 0x0003B9D0
		protected internal override void OnApply()
		{
			base.OnApply();
			if (base.Team.IsPlayerTeam && !base.Team.IsPlayerGeneral && base.Team.IsPlayerSergeant)
			{
				using (List<Formation>.Enumerator enumerator = base.Team.FormationsIncludingSpecialAndEmpty.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.CountOfUnits > 0)
						{
							foreach (Team team in base.Team.Mission.Teams)
							{
								if (team.IsEnemyOf(base.Team))
								{
									using (List<Formation>.Enumerator enumerator3 = team.FormationsIncludingSpecialAndEmpty.GetEnumerator())
									{
										while (enumerator3.MoveNext())
										{
											if (enumerator3.Current.CountOfUnits > 0)
											{
												base.SoundTacticalHorn(TacticComponent.AttackHornSoundIndex);
												return;
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001300 RID: 4864 RVA: 0x0003D90C File Offset: 0x0003BB0C
		protected internal override float GetTacticWeight()
		{
			float num = base.Team.QuerySystem.RemainingPowerRatio / base.Team.QuerySystem.TotalPowerRatio;
			float num2 = MathF.Max(base.Team.QuerySystem.InfantryRatio, MathF.Max(base.Team.QuerySystem.RangedRatio, base.Team.QuerySystem.CavalryRatio)) + ((base.Team.Side == BattleSideEnum.Defender) ? 0.33f : 0f);
			float num3 = ((base.Team.Side == BattleSideEnum.Defender) ? 0.33f : 0.5f);
			float num4 = 0f;
			float num5 = 0f;
			CasualtyHandler missionBehavior = Mission.Current.GetMissionBehavior<CasualtyHandler>();
			foreach (Team team in base.Team.Mission.Teams)
			{
				if (team == base.Team || team.IsEnemyOf(base.Team))
				{
					for (int i = 0; i < Math.Min(team.FormationsIncludingSpecialAndEmpty.Count, 8); i++)
					{
						Formation formation = team.FormationsIncludingSpecialAndEmpty[i];
						if (formation.CountOfUnits > 0)
						{
							num4 += formation.QuerySystem.FormationPower;
							num5 += missionBehavior.GetCasualtyPowerLossOfFormation(formation);
						}
					}
				}
			}
			float num6 = num4 + num5;
			float num7 = num4 / num6;
			num7 = ((base.Team.Side == BattleSideEnum.Attacker && num < 0.5f) ? 0f : MBMath.LinearExtrapolation(0f, 1.6f * num2, (1f - num7) / (1f - num3)));
			float num8 = MBMath.LinearExtrapolation(0f, 1.6f * num2, base.Team.QuerySystem.RemainingPowerRatio * num3 * 0.5f);
			return MathF.Max(num7, num8);
		}
	}
}
