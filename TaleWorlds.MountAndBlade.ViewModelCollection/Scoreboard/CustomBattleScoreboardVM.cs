using System;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Missions.BattleScore;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard
{
	// Token: 0x0200000D RID: 13
	public class CustomBattleScoreboardVM : ScoreboardBaseVM, IBattleObserver
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x0000410D File Offset: 0x0000230D
		public CustomBattleScoreboardVM(BattleScoreContext scoreboardContext)
			: base(scoreboardContext)
		{
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004124 File Offset: 0x00002324
		public override void Initialize(IMissionScreen missionScreen, Mission mission, Action releaseSimulationSources, Action<bool> onToggle)
		{
			base.Initialize(missionScreen, mission, releaseSimulationSources, onToggle);
			base.IsSimulation = false;
			base.SimulationResult = "NotSimulation";
			BattleObserverMissionLogic missionBehavior = Mission.Current.GetMissionBehavior<BattleObserverMissionLogic>();
			this._sallyOutEndLogic = Mission.Current.GetMissionBehavior<SallyOutEndLogic>();
			this._missionCombatantsLogic = this._mission.GetMissionBehavior<MissionCombatantsLogic>();
			if (this._missionCombatantsLogic != null)
			{
				this.PlayerSide = this._missionCombatantsLogic.PlayerSide;
				base.Attackers = new SPScoreboardSideVM(GameTexts.FindText("str_battle_result_side", "attacker"), this.ScoreboardContext.GetAttackerBanner(), false, this.PlayerSide == BattleSideEnum.Attacker);
				base.Defenders = new SPScoreboardSideVM(GameTexts.FindText("str_battle_result_side", "defender"), this.ScoreboardContext.GetDefenderBanner(), false, this.PlayerSide == BattleSideEnum.Defender);
			}
			this.PlayerSide = Mission.Current.PlayerTeam.Side;
			if (missionBehavior == null)
			{
				return;
			}
			missionBehavior.SetObserver(this);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004210 File Offset: 0x00002410
		public override void RefreshValues()
		{
			base.RefreshValues();
			SPScoreboardSideVM defenders = base.Defenders;
			if (defenders != null)
			{
				defenders.RefreshValues();
			}
			SPScoreboardSideVM attackers = base.Attackers;
			if (attackers == null)
			{
				return;
			}
			attackers.RefreshValues();
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000423C File Offset: 0x0000243C
		protected override void OnTick(float dt)
		{
			if (!base.IsOver)
			{
				if (!this._mission.IsMissionEnding)
				{
					BattleEndLogic battleEndLogic = this._battleEndLogic;
					if ((battleEndLogic == null || !battleEndLogic.IsEnemySideRetreating) && (this._missionCombatantsLogic == null || this._battleEndLogic == null || (!this._battleEndLogic.PlayerVictory && !this._battleEndLogic.EnemyVictory)))
					{
						SallyOutEndLogic sallyOutEndLogic = this._sallyOutEndLogic;
						if (sallyOutEndLogic == null || !sallyOutEndLogic.IsSallyOutOver)
						{
							goto IL_008D;
						}
					}
				}
				if (this._missionEndScoreboardDelayTimer < 1.5f)
				{
					this._missionEndScoreboardDelayTimer += dt;
				}
				else
				{
					this.OnBattleOver();
				}
			}
			IL_008D:
			if (!base.IsSimulation && !base.IsOver)
			{
				base.MissionTimeInSeconds = (int)this._mission.CurrentTime;
			}
			if (!base.IsSimulation)
			{
				this._moraleUpdateTimer += dt;
				if (this._moraleUpdateTimer >= 1f)
				{
					base.Attackers.Morale = base.GetBattleMoraleOfSide(BattleSideEnum.Attacker);
					base.Defenders.Morale = base.GetBattleMoraleOfSide(BattleSideEnum.Defender);
					this._moraleUpdateTimer = 0f;
				}
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000434A File Offset: 0x0000254A
		public override void ExecuteFastForwardAction()
		{
			if (base.IsMainCharacterDead)
			{
				Mission.Current.SetFastForwardingFromUI(base.IsFastForwarding);
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004364 File Offset: 0x00002564
		public override void ExecuteQuitAction()
		{
			this.OnExitBattle();
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000436C File Offset: 0x0000256C
		public void OnBattleOver()
		{
			Mission mission = Mission.Current;
			if (mission == null || !mission.MissionEnded)
			{
				BattleEndLogic battleEndLogic = this._battleEndLogic;
				if (battleEndLogic != null && battleEndLogic.IsEnemySideRetreating)
				{
					base.IsOver = true;
				}
				return;
			}
			base.IsOver = true;
			if (Mission.Current.GetMissionBehavior<SallyOutEndLogic>() == null || Mission.Current.MissionResult.BattleResolved)
			{
				MissionResult missionResult = Mission.Current.MissionResult;
				bool flag = missionResult != null && missionResult.PlayerVictory;
				base.BattleResultIndex = (flag ? 1 : 0);
				base.BattleResult = (flag ? GameTexts.FindText("str_victory", null).ToString() : GameTexts.FindText("str_defeat", null).ToString());
				return;
			}
			if (Mission.Current.MissionResult.BattleState == BattleState.DefenderPullBack)
			{
				base.BattleResultIndex = 2;
				base.BattleResult = GameTexts.FindText("str_battle_result_retreat", null).ToString();
				return;
			}
			if (Mission.Current.MissionResult.PlayerVictory)
			{
				base.BattleResultIndex = 1;
				base.BattleResult = GameTexts.FindText("str_finished", null).ToString();
				return;
			}
			base.BattleResultIndex = 0;
			base.BattleResult = GameTexts.FindText("str_defeat", null).ToString();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x000044A0 File Offset: 0x000026A0
		public void OnExitBattle()
		{
			BasicMissionHandler missionBehavior = this._mission.GetMissionBehavior<BasicMissionHandler>();
			BattleEndLogic.ExitResult exitResult = (this._mission.MissionEnded ? BattleEndLogic.ExitResult.True : BattleEndLogic.ExitResult.NeedsPlayerConfirmation);
			if (exitResult == BattleEndLogic.ExitResult.NeedsPlayerConfirmation)
			{
				this.OnToggle(false);
				missionBehavior.CreateWarningWidgetForResult(exitResult);
				return;
			}
			this._mission.EndMission();
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000044F0 File Offset: 0x000026F0
		public void TroopNumberChanged(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject character, int number = 0, int numberDead = 0, int numberWounded = 0, int numberRouted = 0, int numberKilled = 0, int numberReadyToUpgrade = 0)
		{
			base.GetSide(side).UpdateScores(battleCombatant, false, character, number, numberDead, numberWounded, numberRouted, numberKilled, numberReadyToUpgrade);
			base.PowerComparer.Update((double)base.Defenders.CurrentPower, (double)base.Attackers.CurrentPower, (double)base.Defenders.InitialPower, (double)base.Attackers.InitialPower);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004553 File Offset: 0x00002753
		public void HeroSkillIncreased(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject heroCharacter, SkillObject upgradedSkill)
		{
			base.GetSide(side).UpdateHeroSkills(battleCombatant, false, heroCharacter, upgradedSkill);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004566 File Offset: 0x00002766
		public void BattleResultsReady()
		{
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00004568 File Offset: 0x00002768
		public void TroopSideChanged(BattleSideEnum prevSide, BattleSideEnum newSide, IBattleCombatant battleCombatant, BasicCharacterObject character)
		{
			SPScoreboardStatsVM spscoreboardStatsVM = base.GetSide(prevSide).RemoveTroop(battleCombatant, character);
			base.GetSide(newSide).GetPartyAddIfNotExists(battleCombatant, false);
			base.GetSide(newSide).AddTroop(battleCombatant, character, spscoreboardStatsVM);
		}

		// Token: 0x04000052 RID: 82
		private SallyOutEndLogic _sallyOutEndLogic;

		// Token: 0x04000053 RID: 83
		private MissionCombatantsLogic _missionCombatantsLogic;

		// Token: 0x04000054 RID: 84
		private float _missionEndScoreboardDelayTimer;

		// Token: 0x04000055 RID: 85
		private float _moraleUpdateTimer = 1f;
	}
}
