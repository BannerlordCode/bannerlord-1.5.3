using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200002F RID: 47
	public class BattleSimulation : IBattleObserver
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060001EF RID: 495 RVA: 0x00014827 File Offset: 0x00012A27
		// (set) Token: 0x060001F0 RID: 496 RVA: 0x0001482F File Offset: 0x00012A2F
		public bool IsSimulationFinished { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00014838 File Offset: 0x00012A38
		private bool IsPlayerJoinedBattle
		{
			get
			{
				return PlayerEncounter.Current.IsJoinedBattle;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00014844 File Offset: 0x00012A44
		public MapEvent MapEvent
		{
			get
			{
				return this._mapEvent;
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0001484C File Offset: 0x00012A4C
		public bool IsPlayerRetreated
		{
			get
			{
				return this._isPlayerRetreated;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x00014854 File Offset: 0x00012A54
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x0001485C File Offset: 0x00012A5C
		public IBattleObserver BattleObserver
		{
			get
			{
				return this._battleObserver;
			}
			set
			{
				this._battleObserver = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00014865 File Offset: 0x00012A65
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0001486D File Offset: 0x00012A6D
		public List<List<BattleResultPartyData>> Teams { get; private set; }

		// Token: 0x060001F8 RID: 504 RVA: 0x00014878 File Offset: 0x00012A78
		public BattleSimulation(FlattenedTroopRoster selectedTroopsForPlayerSide, FlattenedTroopRoster selectedTroopsForOtherSide)
		{
			this._mapEvent = PlayerEncounter.Battle ?? PlayerEncounter.StartBattle();
			this._mapEvent.IsPlayerSimulation = true;
			this._mapEvent.BattleObserver = this;
			this._isPlayerRetreated = false;
			this.SelectedTroops[(int)this._mapEvent.PlayerSide] = selectedTroopsForPlayerSide;
			this.SelectedTroops[(int)this._mapEvent.GetOtherSide(this._mapEvent.PlayerSide)] = selectedTroopsForOtherSide;
			this._mapEvent.GetNumberOfInvolvedMen();
			if (this._mapEvent.IsSiegeAssault)
			{
				PlayerSiege.StartPlayerSiege(MobileParty.MainParty.Party.Side, true, this._mapEvent.MapEventSettlement);
			}
			List<List<BattleResultPartyData>> list = new List<List<BattleResultPartyData>>
			{
				new List<BattleResultPartyData>(),
				new List<BattleResultPartyData>()
			};
			foreach (PartyBase partyBase in this._mapEvent.InvolvedParties)
			{
				BattleResultPartyData battleResultPartyData = default(BattleResultPartyData);
				bool flag = false;
				foreach (BattleResultPartyData battleResultPartyData2 in list[(int)partyBase.Side])
				{
					if (battleResultPartyData2.Party == partyBase)
					{
						flag = true;
						battleResultPartyData = battleResultPartyData2;
						break;
					}
				}
				if (!flag)
				{
					battleResultPartyData = new BattleResultPartyData(partyBase);
					list[(int)partyBase.Side].Add(battleResultPartyData);
				}
				for (int i = 0; i < partyBase.MemberRoster.Count; i++)
				{
					TroopRosterElement elementCopyAtIndex = partyBase.MemberRoster.GetElementCopyAtIndex(i);
					if (!battleResultPartyData.Characters.Contains(elementCopyAtIndex.Character))
					{
						battleResultPartyData.Characters.Add(elementCopyAtIndex.Character);
					}
				}
			}
			this.Teams = list;
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00014A68 File Offset: 0x00012C68
		public void Play()
		{
			this._simulationState = BattleSimulation.SimulationState.Play;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00014A71 File Offset: 0x00012C71
		public void FastForward()
		{
			this._simulationState = BattleSimulation.SimulationState.FastForward;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00014A7A File Offset: 0x00012C7A
		public void Skip()
		{
			this._simulationState = BattleSimulation.SimulationState.Skip;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00014A83 File Offset: 0x00012C83
		public void Pause()
		{
			this._simulationState = BattleSimulation.SimulationState.Pause;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00014A8C File Offset: 0x00012C8C
		public void OnFinished()
		{
			foreach (PartyBase partyBase in this._mapEvent.InvolvedParties)
			{
				partyBase.MemberRoster.RemoveZeroCounts();
			}
			GameMenu.ActivateGameMenu("encounter");
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00014AEC File Offset: 0x00012CEC
		public void OnPlayerRetreat()
		{
			this._isPlayerRetreated = true;
			this._mapEvent.CommitXpGains();
			this.OnFinished();
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00014B08 File Offset: 0x00012D08
		public void Tick(float dt)
		{
			if (this.IsSimulationFinished)
			{
				return;
			}
			if (PlayerEncounter.Current == null)
			{
				Debug.FailedAssert("PlayerEncounter.Current == null", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\BattleSimulation.cs", "Tick", 159);
				this.IsSimulationFinished = true;
				return;
			}
			if (BattleSimulation.ShouldFinishSimulation())
			{
				this.IsSimulationFinished = true;
				return;
			}
			if (this._simulationState == BattleSimulation.SimulationState.Skip)
			{
				while (!BattleSimulation.ShouldFinishSimulation())
				{
					this.SimulateBattle();
				}
				return;
			}
			if (this._simulationState == BattleSimulation.SimulationState.FastForward)
			{
				dt *= 6f;
			}
			else if (this._simulationState == BattleSimulation.SimulationState.Pause)
			{
				dt = 0f;
			}
			this._numTicks += dt;
			while (this._numTicks >= 1f && !BattleSimulation.ShouldFinishSimulation())
			{
				this.SimulateBattle();
				this._numTicks -= 1f;
			}
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00014BCE File Offset: 0x00012DCE
		public void ResetSimulation()
		{
			this.MapEvent.SimulateBattleSetup(PlayerEncounter.CurrentBattleSimulation.SelectedTroops);
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00014BE8 File Offset: 0x00012DE8
		public void TroopNumberChanged(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject character, int number = 0, int numberKilled = 0, int numberWounded = 0, int numberRouted = 0, int killCount = 0, int numberReadyToUpgrade = 0)
		{
			IBattleObserver battleObserver = this.BattleObserver;
			if (battleObserver == null)
			{
				return;
			}
			battleObserver.TroopNumberChanged(side, battleCombatant, character, number, numberKilled, numberWounded, numberRouted, killCount, numberReadyToUpgrade);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00014C14 File Offset: 0x00012E14
		public void HeroSkillIncreased(BattleSideEnum side, IBattleCombatant battleCombatant, BasicCharacterObject heroCharacter, SkillObject skill)
		{
			IBattleObserver battleObserver = this.BattleObserver;
			if (battleObserver == null)
			{
				return;
			}
			battleObserver.HeroSkillIncreased(side, battleCombatant, heroCharacter, skill);
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00014C2B File Offset: 0x00012E2B
		public void BattleResultsReady()
		{
			IBattleObserver battleObserver = this.BattleObserver;
			if (battleObserver == null)
			{
				return;
			}
			battleObserver.BattleResultsReady();
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00014C3D File Offset: 0x00012E3D
		public void TroopSideChanged(BattleSideEnum prevSide, BattleSideEnum newSide, IBattleCombatant battleCombatant, BasicCharacterObject character)
		{
			IBattleObserver battleObserver = this.BattleObserver;
			if (battleObserver == null)
			{
				return;
			}
			battleObserver.TroopSideChanged(prevSide, newSide, battleCombatant, character);
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00014C54 File Offset: 0x00012E54
		private void SimulateBattle()
		{
			this._mapEvent.SimulatePlayerEncounterBattle();
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00014C61 File Offset: 0x00012E61
		private static bool ShouldFinishSimulation()
		{
			return PlayerEncounter.Battle.HasWinner;
		}

		// Token: 0x04000022 RID: 34
		private readonly MapEvent _mapEvent;

		// Token: 0x04000023 RID: 35
		private bool _isPlayerRetreated;

		// Token: 0x04000024 RID: 36
		private float _numTicks;

		// Token: 0x04000025 RID: 37
		private IBattleObserver _battleObserver;

		// Token: 0x04000027 RID: 39
		public readonly FlattenedTroopRoster[] SelectedTroops = new FlattenedTroopRoster[2];

		// Token: 0x04000028 RID: 40
		private BattleSimulation.SimulationState _simulationState;

		// Token: 0x02000522 RID: 1314
		private enum SimulationState
		{
			// Token: 0x0400168F RID: 5775
			Play,
			// Token: 0x04001690 RID: 5776
			FastForward,
			// Token: 0x04001691 RID: 5777
			Skip,
			// Token: 0x04001692 RID: 5778
			Pause
		}
	}
}
