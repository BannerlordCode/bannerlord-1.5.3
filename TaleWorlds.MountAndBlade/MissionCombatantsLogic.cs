using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000294 RID: 660
	public class MissionCombatantsLogic : MissionLogic
	{
		// Token: 0x1700073F RID: 1855
		// (get) Token: 0x060024D9 RID: 9433 RVA: 0x00086517 File Offset: 0x00084717
		public BattleSideEnum PlayerSide
		{
			get
			{
				if (this.PlayerBattleCombatant == null)
				{
					return BattleSideEnum.None;
				}
				if (this.PlayerBattleCombatant != this.DefenderLeaderBattleCombatant)
				{
					return BattleSideEnum.Attacker;
				}
				return BattleSideEnum.Defender;
			}
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x00086534 File Offset: 0x00084734
		public MissionCombatantsLogic(IEnumerable<IBattleCombatant> battleCombatants, IBattleCombatant playerBattleCombatant, IBattleCombatant defenderLeaderBattleCombatant, IBattleCombatant attackerLeaderBattleCombatant, Mission.MissionTeamAITypeEnum teamAIType, bool isPlayerSergeant)
		{
			if (battleCombatants == null)
			{
				battleCombatants = new IBattleCombatant[] { defenderLeaderBattleCombatant, attackerLeaderBattleCombatant };
			}
			this.BattleCombatants = battleCombatants;
			this.PlayerBattleCombatant = playerBattleCombatant;
			this.DefenderLeaderBattleCombatant = defenderLeaderBattleCombatant;
			this.AttackerLeaderBattleCombatant = attackerLeaderBattleCombatant;
			this.TeamAIType = teamAIType;
			this.IsPlayerSergeant = isPlayerSergeant;
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x00086588 File Offset: 0x00084788
		public bool SupportsAllyTeamOnPlayerSide(out IBattleCombatant allyCombatant)
		{
			allyCombatant = null;
			BattleSideEnum playerSide = this.PlayerBattleCombatant.Side;
			bool flag = this.TeamAIType == Mission.MissionTeamAITypeEnum.NavalRaid;
			return MissionCombatantsLogic.SupportsAllyTeamOnPlayerSide(this.BattleCombatants.Where<IBattleCombatant>((IBattleCombatant cmbt) => cmbt.Side == playerSide), this.PlayerBattleCombatant, this.IsPlayerSergeant, flag, out allyCombatant);
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x000865E8 File Offset: 0x000847E8
		public Banner GetBannerForSide(BattleSideEnum side)
		{
			if (side != BattleSideEnum.Defender)
			{
				return this.AttackerLeaderBattleCombatant.Banner;
			}
			return this.DefenderLeaderBattleCombatant.Banner;
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x00086604 File Offset: 0x00084804
		public BasicCultureObject GetCultureForPlayerSide()
		{
			return this.PlayerBattleCombatant.BasicCulture;
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x00086614 File Offset: 0x00084814
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			if (!base.Mission.Teams.IsEmpty<Team>())
			{
				throw new MBIllegalValueException("Number of teams is not 0.");
			}
			BattleSideEnum playerSide = this.PlayerBattleCombatant.Side;
			BattleSideEnum oppositeSide = playerSide.GetOppositeSide();
			if (playerSide == BattleSideEnum.Defender)
			{
				this.AddPlayerTeam(playerSide);
			}
			else
			{
				this.AddEnemyTeam(oppositeSide);
			}
			if (playerSide == BattleSideEnum.Attacker)
			{
				this.AddPlayerTeam(playerSide);
			}
			else
			{
				this.AddEnemyTeam(oppositeSide);
			}
			bool flag = this.TeamAIType == Mission.MissionTeamAITypeEnum.NavalRaid;
			IBattleCombatant battleCombatant;
			if (MissionCombatantsLogic.SupportsAllyTeamOnPlayerSide(this.BattleCombatants.Where<IBattleCombatant>((IBattleCombatant cmbt) => cmbt.Side == playerSide), this.PlayerBattleCombatant, this.IsPlayerSergeant, flag, out battleCombatant))
			{
				this.AddPlayerAllyTeam(playerSide, battleCombatant);
			}
		}

		// Token: 0x060024DF RID: 9439 RVA: 0x000866E8 File Offset: 0x000848E8
		public override void EarlyStart()
		{
			Mission.Current.MissionTeamAIType = this.TeamAIType;
			switch (this.TeamAIType)
			{
			case Mission.MissionTeamAITypeEnum.FieldBattle:
			{
				using (List<Team>.Enumerator enumerator = Mission.Current.Teams.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Team team8 = enumerator.Current;
						team8.AddTeamAI(new TeamAIGeneral(base.Mission, team8, 10f, 1f), false);
					}
					goto IL_0182;
				}
				break;
			}
			case Mission.MissionTeamAITypeEnum.Siege:
				break;
			case Mission.MissionTeamAITypeEnum.SallyOut:
				goto IL_0104;
			default:
				goto IL_0182;
			}
			using (List<Team>.Enumerator enumerator = Mission.Current.Teams.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Team team2 = enumerator.Current;
					if (team2.Side == BattleSideEnum.Attacker)
					{
						team2.AddTeamAI(new TeamAISiegeAttacker(base.Mission, team2, 5f, 1f), false);
					}
					if (team2.Side == BattleSideEnum.Defender)
					{
						team2.AddTeamAI(new TeamAISiegeDefender(base.Mission, team2, 5f, 1f), false);
					}
				}
				goto IL_0182;
			}
			IL_0104:
			foreach (Team team3 in Mission.Current.Teams)
			{
				if (team3.Side == BattleSideEnum.Attacker)
				{
					team3.AddTeamAI(new TeamAISallyOutDefender(base.Mission, team3, 5f, 1f), false);
				}
				else
				{
					team3.AddTeamAI(new TeamAISallyOutAttacker(base.Mission, team3, 5f, 1f), false);
				}
			}
			IL_0182:
			if (Mission.Current.Teams.Count > 0)
			{
				switch (Mission.Current.MissionTeamAIType)
				{
				case Mission.MissionTeamAITypeEnum.NoTeamAI:
				{
					using (List<Team>.Enumerator enumerator = Mission.Current.Teams.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Team team4 = enumerator.Current;
							if (team4.HasTeamAi)
							{
								team4.AddTacticOption(new TacticCharge(team4));
							}
						}
						goto IL_04AD;
					}
					break;
				}
				case Mission.MissionTeamAITypeEnum.FieldBattle:
					break;
				case Mission.MissionTeamAITypeEnum.Siege:
					goto IL_03C4;
				case Mission.MissionTeamAITypeEnum.SallyOut:
					goto IL_0433;
				default:
					goto IL_04AD;
				}
				using (List<Team>.Enumerator enumerator = Mission.Current.Teams.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Team team = enumerator.Current;
						if (team.HasTeamAi)
						{
							int num = this.BattleCombatants.Where<IBattleCombatant>((IBattleCombatant bc) => bc.Side == team.Side).Max<IBattleCombatant>((IBattleCombatant bcs) => bcs.GetTacticsSkillAmount());
							team.AddTacticOption(new TacticCharge(team));
							if ((float)num >= 20f)
							{
								team.AddTacticOption(new TacticFullScaleAttack(team));
								if (team.Side == BattleSideEnum.Defender)
								{
									team.AddTacticOption(new TacticDefensiveEngagement(team));
									team.AddTacticOption(new TacticDefensiveLine(team));
								}
								if (team.Side == BattleSideEnum.Attacker)
								{
									team.AddTacticOption(new TacticRangedHarrassmentOffensive(team));
								}
							}
							if ((float)num >= 50f)
							{
								team.AddTacticOption(new TacticFrontalCavalryCharge(team));
								if (team.Side == BattleSideEnum.Defender)
								{
									team.AddTacticOption(new TacticDefensiveRing(team));
									team.AddTacticOption(new TacticHoldChokePoint(team));
								}
								if (team.Side == BattleSideEnum.Attacker)
								{
									team.AddTacticOption(new TacticCoordinatedRetreat(team));
								}
							}
						}
					}
					goto IL_04AD;
				}
				IL_03C4:
				using (List<Team>.Enumerator enumerator = Mission.Current.Teams.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Team team5 = enumerator.Current;
						if (team5.HasTeamAi)
						{
							if (team5.Side == BattleSideEnum.Attacker)
							{
								team5.AddTacticOption(new TacticBreachWalls(team5));
							}
							if (team5.Side == BattleSideEnum.Defender)
							{
								team5.AddTacticOption(new TacticDefendCastle(team5));
							}
						}
					}
					goto IL_04AD;
				}
				IL_0433:
				foreach (Team team6 in Mission.Current.Teams)
				{
					if (team6.HasTeamAi)
					{
						if (team6.Side == BattleSideEnum.Defender)
						{
							team6.AddTacticOption(new TacticSallyOutHitAndRun(team6));
						}
						if (team6.Side == BattleSideEnum.Attacker)
						{
							team6.AddTacticOption(new TacticSallyOutDefense(team6));
						}
						team6.AddTacticOption(new TacticCharge(team6));
					}
				}
				IL_04AD:
				foreach (Team team7 in base.Mission.Teams)
				{
					team7.QuerySystem.Expire();
					team7.ResetTactic();
				}
			}
		}

		// Token: 0x060024E0 RID: 9440 RVA: 0x00086CAC File Offset: 0x00084EAC
		public override void AfterStart()
		{
			base.Mission.SetMissionMode(MissionMode.Battle, true);
		}

		// Token: 0x060024E1 RID: 9441 RVA: 0x00086CBB File Offset: 0x00084EBB
		public IEnumerable<IBattleCombatant> GetAllCombatants()
		{
			foreach (IBattleCombatant battleCombatant in this.BattleCombatants)
			{
				yield return battleCombatant;
			}
			IEnumerator<IBattleCombatant> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x00086CCC File Offset: 0x00084ECC
		protected void AddPlayerTeam(BattleSideEnum playerSide)
		{
			base.Mission.Teams.Add(playerSide, this.PlayerBattleCombatant.PrimaryColorPair.Item1, this.PlayerBattleCombatant.PrimaryColorPair.Item2, this.PlayerBattleCombatant.Banner, true, false, true);
			base.Mission.PlayerTeam = ((playerSide == BattleSideEnum.Attacker) ? base.Mission.AttackerTeam : base.Mission.DefenderTeam);
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x00086D40 File Offset: 0x00084F40
		protected void AddEnemyTeam(BattleSideEnum enemySide)
		{
			IBattleCombatant battleCombatant = ((enemySide == BattleSideEnum.Attacker) ? this.AttackerLeaderBattleCombatant : this.DefenderLeaderBattleCombatant);
			base.Mission.Teams.Add(enemySide, battleCombatant.PrimaryColorPair.Item1, battleCombatant.PrimaryColorPair.Item2, battleCombatant.Banner, true, false, true);
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x00086D94 File Offset: 0x00084F94
		protected void AddPlayerAllyTeam(BattleSideEnum playerSide, IBattleCombatant allyCombatant)
		{
			base.Mission.Teams.Add(playerSide, allyCombatant.PrimaryColorPair.Item1, allyCombatant.PrimaryColorPair.Item2, allyCombatant.Banner, true, false, true);
			if (playerSide != BattleSideEnum.Attacker)
			{
				Team defenderAllyTeam = base.Mission.DefenderAllyTeam;
				return;
			}
			Team attackerAllyTeam = base.Mission.AttackerAllyTeam;
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x00086DF0 File Offset: 0x00084FF0
		public static bool SupportsAllyTeamOnPlayerSide(IEnumerable<IBattleCombatant> playerSideBattleCombatants, IBattleCombatant playerBattleCombatant, bool isPlayerSergeant, bool isNavalLandHybridMission, out IBattleCombatant allyCombatant)
		{
			allyCombatant = null;
			if (isNavalLandHybridMission)
			{
				return false;
			}
			BasicCharacterObject general = playerBattleCombatant.General;
			foreach (IBattleCombatant battleCombatant in playerSideBattleCombatants)
			{
				if (battleCombatant != playerBattleCombatant && (!isPlayerSergeant || battleCombatant.General != general) && !battleCombatant.IsUnderPlayersCommand(playerBattleCombatant.Side) && battleCombatant.GetNumberOfMissionReadyTroops() > 0)
				{
					allyCombatant = battleCombatant;
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000E3D RID: 3645
		protected readonly IEnumerable<IBattleCombatant> BattleCombatants;

		// Token: 0x04000E3E RID: 3646
		protected readonly IBattleCombatant PlayerBattleCombatant;

		// Token: 0x04000E3F RID: 3647
		protected readonly IBattleCombatant DefenderLeaderBattleCombatant;

		// Token: 0x04000E40 RID: 3648
		protected readonly IBattleCombatant AttackerLeaderBattleCombatant;

		// Token: 0x04000E41 RID: 3649
		protected readonly Mission.MissionTeamAITypeEnum TeamAIType;

		// Token: 0x04000E42 RID: 3650
		protected readonly bool IsPlayerSergeant;
	}
}
