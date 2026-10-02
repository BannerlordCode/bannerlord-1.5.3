using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SandBox.Missions.BattleScore;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions.BattleScore;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard;

namespace SandBox.ViewModelCollection
{
	// Token: 0x02000009 RID: 9
	public class SPScoreboardVM : ScoreboardBaseVM, IBattleObserver
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000053 RID: 83 RVA: 0x00004115 File Offset: 0x00002315
		private bool _isPlayerDefendingSiege
		{
			get
			{
				Mission mission = Mission.Current;
				return mission != null && mission.IsSiegeBattle && Mission.Current.PlayerTeam.IsDefender;
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0000413B File Offset: 0x0000233B
		public static SPScoreboardVM CreateSimulation(BattleSimulation simulation)
		{
			return new SPScoreboardVM(new SandboxSimulationBattleScoreContext(simulation), simulation);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004149 File Offset: 0x00002349
		public static SPScoreboardVM CreateMission(Mission mission)
		{
			return new SPScoreboardVM(new SandboxMissionBattleScoreContext(mission), null);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004157 File Offset: 0x00002357
		public static SPScoreboardVM CreateCustom(BattleScoreContext battleScoreContext, BattleSimulation simulation = null)
		{
			return new SPScoreboardVM(battleScoreContext, simulation);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004160 File Offset: 0x00002360
		public SPScoreboardVM(BattleScoreContext scoreboardContext, BattleSimulation simulation)
			: base(scoreboardContext)
		{
			this._battleSimulation = simulation;
			this.BattleResults = new MBBindingList<BattleResultVM>();
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004188 File Offset: 0x00002388
		protected override void UpdateQuitText()
		{
			if (base.IsOver)
			{
				base.QuitText = GameTexts.FindText("str_done", null).ToString();
				return;
			}
			if (base.IsMainCharacterDead && !base.IsSimulation)
			{
				base.QuitText = GameTexts.FindText("str_end_battle", null).ToString();
				return;
			}
			if (this._isPlayerDefendingSiege)
			{
				base.QuitText = GameTexts.FindText("str_surrender", null).ToString();
				return;
			}
			base.QuitText = GameTexts.FindText("str_retreat", null).ToString();
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004210 File Offset: 0x00002410
		public override void Initialize(IMissionScreen missionScreen, Mission mission, Action releaseSimulationSources, Action<bool> onToggle)
		{
			base.Initialize(missionScreen, mission, releaseSimulationSources, onToggle);
			base.Attackers = new SPScoreboardSideVM(GameTexts.FindText("str_battle_result_side", "attacker"), this.ScoreboardContext.GetAttackerBanner(), true, PlayerEncounter.PlayerIsAttacker);
			base.Defenders = new SPScoreboardSideVM(GameTexts.FindText("str_battle_result_side", "defender"), this.ScoreboardContext.GetDefenderBanner(), true, !PlayerEncounter.PlayerIsAttacker);
			if (this._battleSimulation != null)
			{
				this.PlayerSide = (PlayerEncounter.PlayerIsAttacker ? BattleSideEnum.Attacker : BattleSideEnum.Defender);
				base.IsSimulation = true;
				base.IsMainCharacterDead = true;
				base.ShowScoreboard = true;
				foreach (UniqueTroopDescriptor uniqueTroopDescriptor in this._battleSimulation.MapEvent.AttackerSide.GetTroops())
				{
					PartyBase allocatedTroopParty = this._battleSimulation.MapEvent.AttackerSide.GetAllocatedTroopParty(uniqueTroopDescriptor);
					CharacterObject allocatedTroop = this._battleSimulation.MapEvent.AttackerSide.GetAllocatedTroop(uniqueTroopDescriptor);
					base.GetSide(allocatedTroopParty.Side).UpdateScores(allocatedTroopParty, ((allocatedTroopParty != null) ? allocatedTroopParty.Owner : null) == Hero.MainHero, allocatedTroop, 1, 0, 0, 0, 0, 0);
				}
				foreach (UniqueTroopDescriptor uniqueTroopDescriptor2 in this._battleSimulation.MapEvent.DefenderSide.GetTroops())
				{
					PartyBase allocatedTroopParty2 = this._battleSimulation.MapEvent.DefenderSide.GetAllocatedTroopParty(uniqueTroopDescriptor2);
					CharacterObject allocatedTroop2 = this._battleSimulation.MapEvent.DefenderSide.GetAllocatedTroop(uniqueTroopDescriptor2);
					base.GetSide(allocatedTroopParty2.Side).UpdateScores(allocatedTroopParty2, ((allocatedTroopParty2 != null) ? allocatedTroopParty2.Owner : null) == Hero.MainHero, allocatedTroop2, 1, 0, 0, 0, 0, 0);
				}
				this._battleSimulation.BattleObserver = this;
				base.PowerComparer.Update((double)base.Defenders.CurrentPower, (double)base.Attackers.CurrentPower, (double)base.Defenders.CurrentPower, (double)base.Attackers.CurrentPower);
			}
			else
			{
				base.IsSimulation = false;
				if (Campaign.Current != null)
				{
					if (PlayerEncounter.Battle != null)
					{
						this.PlayerSide = (PlayerEncounter.PlayerIsAttacker ? BattleSideEnum.Attacker : BattleSideEnum.Defender);
					}
					else
					{
						this.PlayerSide = BattleSideEnum.Defender;
					}
				}
				else
				{
					Debug.FailedAssert("SPScoreboard on CustomBattle", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\SPScoreboardVM.cs", "Initialize", 134);
				}
				BattleObserverMissionLogic missionBehavior = this._mission.GetMissionBehavior<BattleObserverMissionLogic>();
				if (missionBehavior != null)
				{
					missionBehavior.SetObserver(this);
				}
				else
				{
					Debug.FailedAssert("SPScoreboard on CustomBattle", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\SPScoreboardVM.cs", "Initialize", 159);
				}
			}
			string text;
			string text2;
			if (MobileParty.MainParty.MapEvent != null)
			{
				PartyBase leaderParty = MobileParty.MainParty.MapEvent.DefenderSide.LeaderParty;
				if (((leaderParty != null) ? leaderParty.MapFaction : null) is Kingdom)
				{
					text = Color.FromUint(((Kingdom)MobileParty.MainParty.MapEvent.DefenderSide.LeaderParty.MapFaction).PrimaryBannerColor).ToString();
				}
				else
				{
					IFaction mapFaction = MobileParty.MainParty.MapEvent.DefenderSide.LeaderParty.MapFaction;
					text = Color.FromUint((mapFaction != null) ? mapFaction.Banner.GetPrimaryColor() : 0U).ToString();
				}
				PartyBase leaderParty2 = MobileParty.MainParty.MapEvent.AttackerSide.LeaderParty;
				if (((leaderParty2 != null) ? leaderParty2.MapFaction : null) is Kingdom)
				{
					text2 = Color.FromUint(((Kingdom)MobileParty.MainParty.MapEvent.AttackerSide.LeaderParty.MapFaction).PrimaryBannerColor).ToString();
				}
				else
				{
					IFaction mapFaction2 = MobileParty.MainParty.MapEvent.AttackerSide.LeaderParty.MapFaction;
					text2 = Color.FromUint((mapFaction2 != null) ? mapFaction2.Banner.GetPrimaryColor() : 0U).ToString();
				}
			}
			else
			{
				text2 = Color.FromUint(Mission.Current.Teams.Attacker.Color).ToString();
				text = Color.FromUint(Mission.Current.Teams.Defender.Color).ToString();
			}
			base.PowerComparer.SetColors(text, text2);
			base.MissionTimeInSeconds = -1;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004684 File Offset: 0x00002884
		protected override void OnTick(float dt)
		{
			if (!base.IsSimulation)
			{
				if (this._sallyOutEndLogic == null)
				{
					Mission mission = this._mission;
					this._sallyOutEndLogic = ((mission != null) ? mission.GetMissionBehavior<SallyOutEndLogic>() : null);
				}
				if (!base.IsOver)
				{
					Mission mission2 = this._mission;
					if (mission2 == null || !mission2.IsMissionEnding)
					{
						BattleEndLogic battleEndLogic = this._battleEndLogic;
						if (battleEndLogic == null || !battleEndLogic.IsEnemySideRetreating)
						{
							SallyOutEndLogic sallyOutEndLogic = this._sallyOutEndLogic;
							if (sallyOutEndLogic == null || !sallyOutEndLogic.IsSallyOutOver)
							{
								goto IL_0092;
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
			}
			IL_0092:
			if (!base.IsSimulation && !base.IsOver)
			{
				base.MissionTimeInSeconds = (int)Mission.Current.CurrentTime;
			}
			this._moraleUpdateTimer += dt;
			if (this._moraleUpdateTimer > 1f)
			{
				if (base.IsSimulation)
				{
					base.Attackers.Morale = MobileParty.MainParty.MapEvent.AttackerSide.GetSideMorale();
					base.Defenders.Morale = MobileParty.MainParty.MapEvent.DefenderSide.GetSideMorale();
				}
				else
				{
					base.Attackers.Morale = base.GetBattleMoraleOfSide(BattleSideEnum.Attacker);
					base.Defenders.Morale = base.GetBattleMoraleOfSide(BattleSideEnum.Defender);
				}
				this._moraleUpdateTimer = 0f;
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x000047D6 File Offset: 0x000029D6
		public override void ExecutePlayAction()
		{
			if (base.IsSimulation)
			{
				this._battleSimulation.Play();
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000047EC File Offset: 0x000029EC
		public override void ExecuteFastForwardAction()
		{
			if (!base.IsSimulation)
			{
				Mission.Current.SetFastForwardingFromUI(base.IsFastForwarding);
				return;
			}
			base.IsPaused = false;
			if (!base.IsFastForwarding)
			{
				this._battleSimulation.Play();
				return;
			}
			this._battleSimulation.FastForward();
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004838 File Offset: 0x00002A38
		public override void ExecutePauseSimulationAction()
		{
			if (base.IsSimulation)
			{
				base.IsFastForwarding = false;
				if (!base.IsPaused)
				{
					this._battleSimulation.Play();
					return;
				}
				this._battleSimulation.Pause();
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004868 File Offset: 0x00002A68
		public override void ExecuteEndSimulationAction()
		{
			if (base.IsSimulation)
			{
				base.IsPaused = false;
				base.IsFastForwarding = false;
				this._battleSimulation.Skip();
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000488B File Offset: 0x00002A8B
		public override void ExecuteQuitAction()
		{
			this.OnExitBattle();
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004894 File Offset: 0x00002A94
		private void GetBattleRewards(bool playerVictory)
		{
			this.BattleResults.Clear();
			if (playerVictory && PlayerEncounter.Current != null)
			{
				ExplainedNumber renownExplained;
				ExplainedNumber influenceExplained;
				ExplainedNumber moraleExplained;
				float playerEarnedLootRate;
				Figurehead playerEarnedFigurehead;
				PlayerEncounter.Current.GetBattleRewards(out renownExplained, out influenceExplained, out moraleExplained, out playerEarnedLootRate, out playerEarnedFigurehead);
				float resultNumber = renownExplained.ResultNumber;
				float resultNumber2 = influenceExplained.ResultNumber;
				float resultNumber3 = moraleExplained.ResultNumber;
				if (resultNumber > 0.1f)
				{
					this.BattleResults.Add(new BattleResultVM(SPScoreboardVM._renownStr.Format(resultNumber), () => SandBoxUIHelper.GetExplainedNumberTooltip(ref renownExplained), null));
				}
				if (resultNumber2 > 0.1f)
				{
					this.BattleResults.Add(new BattleResultVM(SPScoreboardVM._influenceStr.Format(resultNumber2), () => SandBoxUIHelper.GetExplainedNumberTooltip(ref influenceExplained), null));
				}
				if (resultNumber3 > 0.1f || resultNumber3 < -0.1f)
				{
					this.BattleResults.Add(new BattleResultVM(SPScoreboardVM._moraleStr.Format(resultNumber3), () => SandBoxUIHelper.GetExplainedNumberTooltip(ref moraleExplained), null));
				}
				int num = ((this.PlayerSide == BattleSideEnum.Attacker) ? base.Attackers.Parties.Count : base.Defenders.Parties.Count);
				if (playerEarnedLootRate > 0.1f && num > 1)
				{
					playerEarnedLootRate *= 100f;
					this.BattleResults.Add(new BattleResultVM(SPScoreboardVM._lootStr.Format(playerEarnedLootRate), () => SandBoxUIHelper.GetBattleLootAwardTooltip(playerEarnedLootRate), null));
				}
				if (playerEarnedFigurehead != null)
				{
					Figurehead playerEarnedFigurehead2 = playerEarnedFigurehead;
					if (((playerEarnedFigurehead2 != null) ? playerEarnedFigurehead2.Name : null) != null)
					{
						Collection<BattleResultVM> battleResults = this.BattleResults;
						TextObject figureheadStr = SPScoreboardVM._figureheadStr;
						string text = "A0";
						TextObject name = playerEarnedFigurehead.Name;
						battleResults.Add(new BattleResultVM(figureheadStr.SetTextVariable(text, ((name != null) ? name.ToString() : null) ?? "").ToString(), () => SandBoxUIHelper.GetFigureheadTooltip(playerEarnedFigurehead), null));
					}
					else
					{
						Debug.FailedAssert("Battle rewards contain an invalid figurehead (null or name missing)", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\SPScoreboardVM.cs", "GetBattleRewards", 361);
					}
				}
			}
			foreach (SPScoreboardPartyVM spscoreboardPartyVM in base.Defenders.Parties)
			{
				foreach (SPScoreboardUnitVM spscoreboardUnitVM in spscoreboardPartyVM.Members.Where<SPScoreboardUnitVM>((SPScoreboardUnitVM member) => member.IsHero && member.Score.Dead > 0))
				{
					if (spscoreboardUnitVM.Character == null)
					{
						Debug.FailedAssert("Scoreboard has a member element without a character", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\SPScoreboardVM.cs", "GetBattleRewards", 378);
					}
					else
					{
						this.BattleResults.Add(new BattleResultVM(SPScoreboardVM._deadLordStr.SetTextVariable("A0", spscoreboardUnitVM.Character.Name).ToString(), () => new List<TooltipProperty>(), SandBoxUIHelper.GetCharacterCode(spscoreboardUnitVM.Character as CharacterObject, false)));
					}
				}
			}
			foreach (SPScoreboardPartyVM spscoreboardPartyVM2 in base.Attackers.Parties)
			{
				foreach (SPScoreboardUnitVM spscoreboardUnitVM2 in spscoreboardPartyVM2.Members.Where<SPScoreboardUnitVM>((SPScoreboardUnitVM member) => member.IsHero && member.Score.Dead > 0))
				{
					if (spscoreboardUnitVM2.Character == null)
					{
						Debug.FailedAssert("Scoreboard has a member element without a character", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\SPScoreboardVM.cs", "GetBattleRewards", 395);
					}
					else
					{
						this.BattleResults.Add(new BattleResultVM(SPScoreboardVM._deadLordStr.SetTextVariable("A0", spscoreboardUnitVM2.Character.Name).ToString(), () => new List<TooltipProperty>(), SandBoxUIHelper.GetCharacterCode(spscoreboardUnitVM2.Character as CharacterObject, false)));
					}
				}
			}
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00004D14 File Offset: 0x00002F14
		private void UpdateSimulationResult(bool playerVictory)
		{
			if (!base.IsSimulation)
			{
				base.SimulationResult = "NotSimulation";
				return;
			}
			if (!playerVictory)
			{
				base.SimulationResult = "SimulationDefeat";
				return;
			}
			if (PlayerEncounter.Battle.PartiesOnSide(this.PlayerSide).Sum<MapEventParty>((MapEventParty x) => x.Party.NumberOfHealthyMembers) < 70)
			{
				base.SimulationResult = "SimulationVictorySmall";
				return;
			}
			base.SimulationResult = "SimulationVictoryLarge";
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004D94 File Offset: 0x00002F94
		public void OnBattleOver()
		{
			ScoreboardBaseVM.BattleResultType battleResultType = ScoreboardBaseVM.BattleResultType.NotOver;
			if (PlayerEncounter.IsActive && PlayerEncounter.Battle != null)
			{
				base.IsOver = true;
				bool flag = false;
				if (PlayerEncounter.WinningSide == this.PlayerSide)
				{
					battleResultType = ScoreboardBaseVM.BattleResultType.Victory;
					flag = true;
				}
				else
				{
					CampaignBattleResult campaignBattleResult = PlayerEncounter.CampaignBattleResult;
					if (campaignBattleResult != null && campaignBattleResult.EnemyPulledBack)
					{
						battleResultType = ScoreboardBaseVM.BattleResultType.Retreat;
					}
					else
					{
						battleResultType = ScoreboardBaseVM.BattleResultType.Defeat;
					}
				}
				this.GetBattleRewards(flag);
			}
			else
			{
				Mission mission = Mission.Current;
				if (mission != null && mission.MissionEnded)
				{
					base.IsOver = true;
					if ((Mission.Current.HasMissionBehavior<SallyOutEndLogic>() && !Mission.Current.MissionResult.BattleResolved) || Mission.Current.MissionResult.PlayerVictory)
					{
						battleResultType = ScoreboardBaseVM.BattleResultType.Victory;
					}
					else if (Mission.Current.MissionResult.BattleState == BattleState.DefenderPullBack && Mission.Current.PlayerTeam.Side == BattleSideEnum.Attacker)
					{
						battleResultType = ScoreboardBaseVM.BattleResultType.Retreat;
					}
					else
					{
						battleResultType = ScoreboardBaseVM.BattleResultType.Defeat;
					}
				}
				else
				{
					BattleEndLogic battleEndLogic = this._battleEndLogic;
					if (battleEndLogic != null && battleEndLogic.IsEnemySideRetreating)
					{
						base.IsOver = true;
					}
				}
			}
			switch (battleResultType)
			{
			case ScoreboardBaseVM.BattleResultType.Defeat:
				base.BattleResult = GameTexts.FindText("str_defeat", null).ToString();
				base.BattleResultIndex = (int)battleResultType;
				break;
			case ScoreboardBaseVM.BattleResultType.Victory:
				if (PlayerEncounter.Battle != null && PlayerEncounter.Battle.EndedByRetreat)
				{
					base.BattleResult = ((PlayerEncounter.Battle.RetreatingSide == BattleSideEnum.Attacker) ? GameTexts.FindText("str_attackers_retreated", null).ToString() : GameTexts.FindText("str_defenders_retreated", null).ToString());
				}
				else
				{
					base.BattleResult = GameTexts.FindText("str_victory", null).ToString();
				}
				base.BattleResultIndex = (int)battleResultType;
				break;
			case ScoreboardBaseVM.BattleResultType.Retreat:
				base.BattleResult = GameTexts.FindText("str_battle_result_retreat", null).ToString();
				base.BattleResultIndex = (int)battleResultType;
				break;
			}
			if (battleResultType != ScoreboardBaseVM.BattleResultType.NotOver)
			{
				this.UpdateSimulationResult(battleResultType == ScoreboardBaseVM.BattleResultType.Victory || battleResultType == ScoreboardBaseVM.BattleResultType.Retreat);
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004F68 File Offset: 0x00003168
		public void OnExitBattle()
		{
			if (base.IsSimulation)
			{
				if (this._battleSimulation.IsSimulationFinished)
				{
					this._releaseSimulationSources();
					this._battleSimulation.OnFinished();
					return;
				}
				Game.Current.GameStateManager.RegisterActiveStateDisableRequest(this);
				InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_order_Retreat", null).ToString(), GameTexts.FindText("str_retreat_question", null).ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), delegate
				{
					Game.Current.GameStateManager.UnregisterActiveStateDisableRequest(this);
					this._releaseSimulationSources();
					this._battleSimulation.OnPlayerRetreat();
				}, delegate
				{
					Game.Current.GameStateManager.UnregisterActiveStateDisableRequest(this);
				}, "", 0f, null, null, null), false, false);
				return;
			}
			else
			{
				BattleEndLogic missionBehavior = this._mission.GetMissionBehavior<BattleEndLogic>();
				BasicMissionHandler missionBehavior2 = this._mission.GetMissionBehavior<BasicMissionHandler>();
				BattleEndLogic.ExitResult exitResult = ((missionBehavior != null) ? missionBehavior.TryExit() : (this._mission.MissionEnded ? BattleEndLogic.ExitResult.True : BattleEndLogic.ExitResult.NeedsPlayerConfirmation));
				if (exitResult == BattleEndLogic.ExitResult.NeedsPlayerConfirmation || exitResult == BattleEndLogic.ExitResult.SurrenderSiege)
				{
					this.OnToggle(false);
					missionBehavior2.CreateWarningWidgetForResult(exitResult);
					return;
				}
				if (exitResult == BattleEndLogic.ExitResult.False)
				{
					InformationManager.ShowInquiry(this._retreatInquiryData, false, false);
					return;
				}
				if (missionBehavior == null && exitResult == BattleEndLogic.ExitResult.True)
				{
					this._mission.EndMission();
				}
				return;
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000509C File Offset: 0x0000329C
		public void TroopNumberChanged(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject character, int number = 0, int numberDead = 0, int numberWounded = 0, int numberRouted = 0, int numberKilled = 0, int numberReadyToUpgrade = 0)
		{
			PartyBase partyBase = battleCombatant as PartyBase;
			bool flag = ((partyBase != null) ? partyBase.Owner : null) == Hero.MainHero;
			base.GetSide(side).UpdateScores(battleCombatant, flag, character, number, numberDead, numberWounded, numberRouted, numberKilled, numberReadyToUpgrade);
			base.PowerComparer.Update((double)base.Defenders.CurrentPower, (double)base.Attackers.CurrentPower, (double)base.Defenders.InitialPower, (double)base.Attackers.InitialPower);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000511C File Offset: 0x0000331C
		public void HeroSkillIncreased(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject heroCharacter, SkillObject upgradedSkill)
		{
			PartyBase partyBase = battleCombatant as PartyBase;
			bool flag = ((partyBase != null) ? partyBase.Owner : null) == Hero.MainHero;
			base.GetSide(side).UpdateHeroSkills(battleCombatant, flag, heroCharacter, upgradedSkill);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00005154 File Offset: 0x00003354
		public void BattleResultsReady()
		{
			if (!base.IsOver)
			{
				this.OnBattleOver();
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00005164 File Offset: 0x00003364
		public void TroopSideChanged(BattleSideEnum prevSide, BattleSideEnum newSide, IBattleCombatant battleCombatant, BasicCharacterObject character)
		{
			SPScoreboardStatsVM spscoreboardStatsVM = base.GetSide(prevSide).RemoveTroop(battleCombatant, character);
			SPScoreboardSideVM side = base.GetSide(newSide);
			PartyBase partyBase = battleCombatant as PartyBase;
			side.GetPartyAddIfNotExists(battleCombatant, ((partyBase != null) ? partyBase.Owner : null) == Hero.MainHero);
			base.GetSide(newSide).AddTroop(battleCombatant, character, spscoreboardStatsVM);
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000068 RID: 104 RVA: 0x000051B8 File Offset: 0x000033B8
		// (set) Token: 0x06000069 RID: 105 RVA: 0x000051C0 File Offset: 0x000033C0
		[DataSourceProperty]
		public override MBBindingList<BattleResultVM> BattleResults
		{
			get
			{
				return this._battleResults;
			}
			set
			{
				if (value != this._battleResults)
				{
					this._battleResults = value;
					base.OnPropertyChangedWithValue<MBBindingList<BattleResultVM>>(value, "BattleResults");
				}
			}
		}

		// Token: 0x04000026 RID: 38
		private readonly BattleSimulation _battleSimulation;

		// Token: 0x04000027 RID: 39
		private SallyOutEndLogic _sallyOutEndLogic;

		// Token: 0x04000028 RID: 40
		private static readonly TextObject _renownStr = new TextObject("{=eiWQoW9j}You gained {A0} renown.", null);

		// Token: 0x04000029 RID: 41
		private static readonly TextObject _influenceStr = new TextObject("{=5zeL8sa9}You gained {A0} influence.", null);

		// Token: 0x0400002A RID: 42
		private static readonly TextObject _moraleStr = new TextObject("{=WAKz9xX8}You gained {A0} morale.", null);

		// Token: 0x0400002B RID: 43
		private static readonly TextObject _lootStr = new TextObject("{=xu5NA6AW}You earned {A0}% of the loot.", null);

		// Token: 0x0400002C RID: 44
		private static readonly TextObject _deadLordStr = new TextObject("{=gDKhs4lD}{A0} has died on the battlefield.", null);

		// Token: 0x0400002D RID: 45
		private static readonly TextObject _figureheadStr = new TextObject("{=ANoYN1yZ}You unlocked the {A0} figurehead.", null);

		// Token: 0x0400002E RID: 46
		private float _moraleUpdateTimer = 1f;

		// Token: 0x0400002F RID: 47
		private float _missionEndScoreboardDelayTimer;

		// Token: 0x04000030 RID: 48
		private MBBindingList<BattleResultVM> _battleResults;
	}
}
