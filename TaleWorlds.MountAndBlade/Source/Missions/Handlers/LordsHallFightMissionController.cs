using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Objects;

namespace TaleWorlds.MountAndBlade.Source.Missions.Handlers
{
	// Token: 0x020003E4 RID: 996
	public class LordsHallFightMissionController : MissionLogic, IMissionAgentSpawnLogic, IMissionBehavior
	{
		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06003762 RID: 14178 RVA: 0x000E5DF5 File Offset: 0x000E3FF5
		// (set) Token: 0x06003763 RID: 14179 RVA: 0x000E5DFD File Offset: 0x000E3FFD
		public BattleSideEnum PlayerSide { get; private set; }

		// Token: 0x06003764 RID: 14180 RVA: 0x000E5E08 File Offset: 0x000E4008
		public LordsHallFightMissionController(IMissionTroopSupplier[] suppliers, float areaLostRatio, float attackerDefenderTroopCountRatio, int attackerSideTroopCountMax, int defenderSideTroopCountMax, BattleSideEnum playerSide)
		{
			this.PlayerSide = playerSide;
			this._areaLostRatio = areaLostRatio;
			this._attackerDefenderTroopCountRatio = attackerDefenderTroopCountRatio;
			this._attackerSideTroopCountMax = attackerSideTroopCountMax;
			this._defenderSideTroopCountMax = defenderSideTroopCountMax;
			this._missionSides = new LordsHallFightMissionController.MissionSide[2];
			this._playerSide = playerSide;
			for (int i = 0; i < 2; i++)
			{
				IMissionTroopSupplier missionTroopSupplier = suppliers[i];
				bool flag = i == (int)playerSide;
				this._missionSides[i] = new LordsHallFightMissionController.MissionSide((BattleSideEnum)i, missionTroopSupplier, flag);
			}
		}

		// Token: 0x06003765 RID: 14181 RVA: 0x000E5E7B File Offset: 0x000E407B
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			base.Mission.GetAgentTroopClass_Override += this.GetLordsHallFightTroopClass;
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x000E5E9A File Offset: 0x000E409A
		public override void OnMissionStateFinalized()
		{
			base.OnMissionStateFinalized();
			base.Mission.GetAgentTroopClass_Override -= this.GetLordsHallFightTroopClass;
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x000E5EB9 File Offset: 0x000E40B9
		public override void OnCreated()
		{
			base.OnCreated();
			base.Mission.DoesMissionRequireCivilianEquipment = false;
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x000E5ED0 File Offset: 0x000E40D0
		public override void OnMissionTick(float dt)
		{
			if (!this._isMissionInitialized)
			{
				this.InitializeMission();
				this._isMissionInitialized = true;
				return;
			}
			if (!this._troopsInitialized)
			{
				this._troopsInitialized = true;
			}
			if (this._setChargeOrderNextFrame)
			{
				if (base.Mission.PlayerTeam.ActiveAgents.Count > 0)
				{
					base.Mission.PlayerTeam.PlayerOrderController.SelectAllFormations(false);
					base.Mission.PlayerTeam.PlayerOrderController.SetOrder(OrderType.Charge);
				}
				this._setChargeOrderNextFrame = false;
			}
			this.CheckForReinforcement();
			this.CheckIfAnyAreaIsLostByDefender();
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x000E5F64 File Offset: 0x000E4164
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (!affectedAgent.Team.IsDefender)
			{
				this._setChargeOrderNextFrame = affectedAgent.IsMainAgent;
				this._removedAllyCounter++;
				if (this._removedAllyCounter == 5)
				{
					this._spawnReinforcements = true;
					this._removedAllyCounter = 0;
				}
				return;
			}
			Tuple<int, LordsHallFightMissionController.AreaEntityData> tuple = this.FindAgentMachine(affectedAgent);
			if (tuple == null)
			{
				return;
			}
			tuple.Item2.StopUse();
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x000E5FC8 File Offset: 0x000E41C8
		private Tuple<int, LordsHallFightMissionController.AreaEntityData> FindAgentMachine(Agent agent)
		{
			Tuple<int, LordsHallFightMissionController.AreaEntityData> tuple = null;
			foreach (KeyValuePair<int, Dictionary<int, LordsHallFightMissionController.AreaData>> keyValuePair in this._dividedAreaDictionary)
			{
				if (tuple != null)
				{
					break;
				}
				foreach (KeyValuePair<int, LordsHallFightMissionController.AreaData> keyValuePair2 in keyValuePair.Value)
				{
					LordsHallFightMissionController.AreaEntityData areaEntityData = keyValuePair2.Value.FindAgentMachine(agent);
					if (areaEntityData != null)
					{
						tuple = new Tuple<int, LordsHallFightMissionController.AreaEntityData>(keyValuePair.Key, areaEntityData);
						break;
					}
				}
			}
			return tuple;
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x000E6080 File Offset: 0x000E4280
		private void InitializeMission()
		{
			this._areaIndexList = new List<int>();
			this._dividedAreaDictionary = new Dictionary<int, Dictionary<int, LordsHallFightMissionController.AreaData>>();
			IEnumerable<FightAreaMarker> enumerable = from area in base.Mission.ActiveMissionObjects.FindAllWithType<FightAreaMarker>()
				orderby area.AreaIndex
				select area;
			base.Mission.DeploymentPlan.MakeDefaultDeploymentPlans();
			foreach (FightAreaMarker fightAreaMarker in enumerable)
			{
				if (!this._dividedAreaDictionary.ContainsKey(fightAreaMarker.AreaIndex))
				{
					this._dividedAreaDictionary.Add(fightAreaMarker.AreaIndex, new Dictionary<int, LordsHallFightMissionController.AreaData>());
				}
				if (!this._dividedAreaDictionary[fightAreaMarker.AreaIndex].ContainsKey(fightAreaMarker.SubAreaIndex))
				{
					this._dividedAreaDictionary[fightAreaMarker.AreaIndex].Add(fightAreaMarker.SubAreaIndex, new LordsHallFightMissionController.AreaData(new List<FightAreaMarker> { fightAreaMarker }));
				}
				else
				{
					this._dividedAreaDictionary[fightAreaMarker.AreaIndex][fightAreaMarker.SubAreaIndex].AddAreaMarker(fightAreaMarker);
				}
			}
			this._areaIndexList = this._dividedAreaDictionary.Keys.ToList<int>();
			this._missionSides[0].SpawnTroops(this._dividedAreaDictionary, this._defenderSideTroopCountMax);
			int numberOfActiveTroops = this._missionSides[0].NumberOfActiveTroops;
			this._defenderTeams = new Team[2];
			this._defenderTeams[0] = Mission.Current.DefenderTeam;
			this._defenderTeams[1] = Mission.Current.DefenderAllyTeam;
			int num = MathF.Max(1, MathF.Min(this._attackerSideTroopCountMax, MathF.Round((float)numberOfActiveTroops * this._attackerDefenderTroopCountRatio)));
			this._missionSides[1].SpawnTroops(num, false);
			base.Mission.OnInitialSpawnCompleted(BattleSideEnum.None);
			bool flag = Mission.Current.AttackerTeam == Mission.Current.PlayerTeam || (Mission.Current.AttackerAllyTeam != null && Mission.Current.AttackerAllyTeam == Mission.Current.PlayerTeam);
			this._attackerTeams = new Team[2];
			this._attackerTeams[0] = Mission.Current.AttackerTeam;
			this._attackerTeams[1] = Mission.Current.AttackerAllyTeam;
			foreach (Team team in this._attackerTeams)
			{
				if (team != null)
				{
					foreach (Formation formation in team.FormationsIncludingEmpty)
					{
						if (formation.CountOfUnits > 0)
						{
							formation.SetArrangementOrder(ArrangementOrder.ArrangementOrderSquare);
							formation.SetFormOrder(FormOrder.FormOrderDeep, true);
						}
						formation.SetMovementOrder(MovementOrder.MovementOrderCharge);
						formation.SetFiringOrder(FiringOrder.FiringOrderHoldYourFire);
						if (flag)
						{
							formation.PlayerOwner = Mission.Current.MainAgent;
						}
					}
				}
			}
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x000E6398 File Offset: 0x000E4598
		private void CheckForReinforcement()
		{
			if (this._spawnReinforcements)
			{
				this._missionSides[1].SpawnTroops(5, true);
				this._spawnReinforcements = false;
			}
		}

		// Token: 0x0600376D RID: 14189 RVA: 0x000E63B8 File Offset: 0x000E45B8
		public void StartSpawner(BattleSideEnum side)
		{
			this._missionSides[(int)side].SetSpawnTroops(true);
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x000E63C8 File Offset: 0x000E45C8
		public void StopSpawner(BattleSideEnum side)
		{
			this._missionSides[(int)side].SetSpawnTroops(false);
		}

		// Token: 0x0600376F RID: 14191 RVA: 0x000E63D8 File Offset: 0x000E45D8
		public bool IsSideSpawnEnabled(BattleSideEnum side)
		{
			return this._missionSides[(int)side].TroopSpawningActive;
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x000E63E7 File Offset: 0x000E45E7
		public float GetReinforcementInterval(BattleSideEnum side = BattleSideEnum.None)
		{
			return 0f;
		}

		// Token: 0x06003771 RID: 14193 RVA: 0x000E63EE File Offset: 0x000E45EE
		public bool IsSideDepleted(BattleSideEnum side)
		{
			return this._missionSides[(int)side].NumberOfActiveTroops == 0;
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x000E6400 File Offset: 0x000E4600
		public int GetNumberOfPlayerControllableTroops()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06003773 RID: 14195 RVA: 0x000E6407 File Offset: 0x000E4607
		public IEnumerable<IAgentOriginBase> GetAllTroopsForSide(BattleSideEnum side)
		{
			return this._missionSides[(int)side].GetAllTroops();
		}

		// Token: 0x06003774 RID: 14196 RVA: 0x000E6416 File Offset: 0x000E4616
		public bool GetSpawnHorses(BattleSideEnum side)
		{
			return false;
		}

		// Token: 0x06003775 RID: 14197 RVA: 0x000E641C File Offset: 0x000E461C
		private void CheckIfAnyAreaIsLostByDefender()
		{
			int num = -1;
			for (int i = 0; i < this._areaIndexList.Count; i++)
			{
				int num2 = this._areaIndexList[i];
				if (num2 > this._lastAreaLostByDefender && num < 0)
				{
					foreach (KeyValuePair<int, LordsHallFightMissionController.AreaData> keyValuePair in this._dividedAreaDictionary[num2])
					{
						if (this.IsAreaLostByDefender(keyValuePair.Value))
						{
							num = num2;
							break;
						}
					}
				}
			}
			if (num > 0)
			{
				this.OnAreaLost(num);
			}
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x000E64C0 File Offset: 0x000E46C0
		private void OnAreaLost(int areaIndex)
		{
			int num = MathF.Min(this._areaIndexList.IndexOf(areaIndex) + 1, this._areaIndexList.Count - 1);
			for (int i = MathF.Max(0, this._areaIndexList.IndexOf(this._lastAreaLostByDefender)); i < num; i++)
			{
				int num2 = this._areaIndexList[i];
				foreach (KeyValuePair<int, LordsHallFightMissionController.AreaData> keyValuePair in this._dividedAreaDictionary[num2])
				{
					this.StartAreaPullBack(keyValuePair.Value, this._areaIndexList[num]);
				}
			}
			this._lastAreaLostByDefender = areaIndex;
		}

		// Token: 0x06003777 RID: 14199 RVA: 0x000E6584 File Offset: 0x000E4784
		private void StartAreaPullBack(LordsHallFightMissionController.AreaData areaData, int nextAreaIndex)
		{
			foreach (LordsHallFightMissionController.AreaEntityData areaEntityData in areaData.ArcherUsablePoints)
			{
				if (areaEntityData.InUse)
				{
					Agent userAgent = areaEntityData.UserAgent;
					areaEntityData.StopUse();
					LordsHallFightMissionController.AreaEntityData areaEntityData2 = this.FindPosition(nextAreaIndex, true);
					if (areaEntityData2 != null)
					{
						areaEntityData2.AssignAgent(userAgent);
					}
				}
			}
			foreach (LordsHallFightMissionController.AreaEntityData areaEntityData3 in areaData.InfantryUsablePoints)
			{
				if (areaEntityData3.InUse)
				{
					Agent userAgent2 = areaEntityData3.UserAgent;
					areaEntityData3.StopUse();
					LordsHallFightMissionController.AreaEntityData areaEntityData4 = this.FindPosition(nextAreaIndex, false);
					if (areaEntityData4 != null)
					{
						areaEntityData4.AssignAgent(userAgent2);
					}
				}
			}
		}

		// Token: 0x06003778 RID: 14200 RVA: 0x000E6654 File Offset: 0x000E4854
		private LordsHallFightMissionController.AreaEntityData FindPosition(int nextAreaIndex, bool isArcher)
		{
			int num = this.SelectBestSubArea(nextAreaIndex, isArcher);
			if (num < 0)
			{
				isArcher = !isArcher;
				num = this.SelectBestSubArea(nextAreaIndex, isArcher);
			}
			return this._dividedAreaDictionary[nextAreaIndex][num].GetAvailableMachines(isArcher).GetRandomElementInefficiently<LordsHallFightMissionController.AreaEntityData>();
		}

		// Token: 0x06003779 RID: 14201 RVA: 0x000E669C File Offset: 0x000E489C
		private int SelectBestSubArea(int areaIndex, bool isArcher)
		{
			int num = -1;
			float num2 = 0f;
			foreach (KeyValuePair<int, LordsHallFightMissionController.AreaData> keyValuePair in this._dividedAreaDictionary[areaIndex])
			{
				float areaAvailabilityRatio = this.GetAreaAvailabilityRatio(keyValuePair.Value, isArcher);
				if (areaAvailabilityRatio > num2)
				{
					num2 = areaAvailabilityRatio;
					num = keyValuePair.Key;
				}
			}
			return num;
		}

		// Token: 0x0600377A RID: 14202 RVA: 0x000E6718 File Offset: 0x000E4918
		private float GetAreaAvailabilityRatio(LordsHallFightMissionController.AreaData areaData, bool isArcher)
		{
			int num = (isArcher ? areaData.ArcherUsablePoints.Count<LordsHallFightMissionController.AreaEntityData>() : areaData.InfantryUsablePoints.Count<LordsHallFightMissionController.AreaEntityData>());
			int num2;
			if (!isArcher)
			{
				num2 = areaData.InfantryUsablePoints.Count<LordsHallFightMissionController.AreaEntityData>((LordsHallFightMissionController.AreaEntityData x) => !x.InUse);
			}
			else
			{
				num2 = areaData.ArcherUsablePoints.Count<LordsHallFightMissionController.AreaEntityData>((LordsHallFightMissionController.AreaEntityData x) => !x.InUse);
			}
			int num3 = num2;
			if (num != 0)
			{
				return (float)num3 / (float)num;
			}
			return 0f;
		}

		// Token: 0x0600377B RID: 14203 RVA: 0x000E67AC File Offset: 0x000E49AC
		private bool IsAreaLostByDefender(LordsHallFightMissionController.AreaData areaData)
		{
			int num = 0;
			foreach (Team team in this._defenderTeams)
			{
				if (team != null)
				{
					foreach (Agent agent in team.ActiveAgents)
					{
						if (this.IsAgentInArea(agent, areaData))
						{
							num++;
						}
					}
				}
			}
			int num2 = MathF.Round((float)num * this._areaLostRatio);
			bool flag = num2 == 0;
			if (!flag)
			{
				foreach (Team team2 in this._attackerTeams)
				{
					if (team2 != null)
					{
						foreach (Agent agent2 in team2.ActiveAgents)
						{
							if (this.IsAgentInArea(agent2, areaData))
							{
								num2--;
								if (num2 == 0)
								{
									flag = true;
									break;
								}
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x000E68C4 File Offset: 0x000E4AC4
		private bool IsAgentInArea(Agent agent, LordsHallFightMissionController.AreaData areaData)
		{
			bool flag = false;
			Vec3 position = agent.Position;
			using (IEnumerator<FightAreaMarker> enumerator = areaData.AreaList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsPositionInRange(position))
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x0600377D RID: 14205 RVA: 0x000E6920 File Offset: 0x000E4B20
		private FormationClass GetLordsHallFightTroopClass(BattleSideEnum side, BasicCharacterObject agentCharacter)
		{
			return agentCharacter.GetFormationClass().DismountedClass();
		}

		// Token: 0x040017E7 RID: 6119
		private const int ReinforcementWaveAgentCount = 5;

		// Token: 0x040017E9 RID: 6121
		private readonly float _areaLostRatio;

		// Token: 0x040017EA RID: 6122
		private readonly float _attackerDefenderTroopCountRatio;

		// Token: 0x040017EB RID: 6123
		private readonly int _attackerSideTroopCountMax;

		// Token: 0x040017EC RID: 6124
		private readonly int _defenderSideTroopCountMax;

		// Token: 0x040017ED RID: 6125
		private readonly LordsHallFightMissionController.MissionSide[] _missionSides;

		// Token: 0x040017EE RID: 6126
		private Team[] _attackerTeams;

		// Token: 0x040017EF RID: 6127
		private Team[] _defenderTeams;

		// Token: 0x040017F0 RID: 6128
		private Dictionary<int, Dictionary<int, LordsHallFightMissionController.AreaData>> _dividedAreaDictionary;

		// Token: 0x040017F1 RID: 6129
		private List<int> _areaIndexList;

		// Token: 0x040017F2 RID: 6130
		private int _lastAreaLostByDefender;

		// Token: 0x040017F3 RID: 6131
		private bool _troopsInitialized;

		// Token: 0x040017F4 RID: 6132
		private bool _isMissionInitialized;

		// Token: 0x040017F5 RID: 6133
		private bool _spawnReinforcements;

		// Token: 0x040017F6 RID: 6134
		private bool _setChargeOrderNextFrame;

		// Token: 0x040017F7 RID: 6135
		private BattleSideEnum _playerSide;

		// Token: 0x040017F8 RID: 6136
		private int _removedAllyCounter;

		// Token: 0x0200069D RID: 1693
		private class MissionSide
		{
			// Token: 0x17000B1D RID: 2845
			// (get) Token: 0x06004261 RID: 16993 RVA: 0x001000E0 File Offset: 0x000FE2E0
			public bool TroopSpawningActive
			{
				get
				{
					return this._troopSpawningActive;
				}
			}

			// Token: 0x17000B1E RID: 2846
			// (get) Token: 0x06004262 RID: 16994 RVA: 0x001000E8 File Offset: 0x000FE2E8
			public int NumberOfActiveTroops
			{
				get
				{
					return this._numberOfSpawnedTroops - this._troopSupplier.NumRemovedTroops;
				}
			}

			// Token: 0x06004263 RID: 16995 RVA: 0x001000FC File Offset: 0x000FE2FC
			public MissionSide(BattleSideEnum side, IMissionTroopSupplier troopSupplier, bool isPlayerSide)
			{
				this._side = side;
				this._isPlayerSide = isPlayerSide;
				this._troopSupplier = troopSupplier;
			}

			// Token: 0x06004264 RID: 16996 RVA: 0x00100120 File Offset: 0x000FE320
			public void SpawnTroops(Dictionary<int, Dictionary<int, LordsHallFightMissionController.AreaData>> areaMarkerDictionary, int spawnCount)
			{
				List<IAgentOriginBase> list = this._troopSupplier.SupplyTroops(spawnCount).OrderByDescending<IAgentOriginBase, int>(delegate(IAgentOriginBase x)
				{
					FormationClass agentTroopClass = Mission.Current.GetAgentTroopClass(this._side, x.Troop);
					if (agentTroopClass != FormationClass.Ranged && agentTroopClass != FormationClass.HorseArcher)
					{
						return 0;
					}
					return 1;
				}).ToList<IAgentOriginBase>();
				for (int i = 0; i < list.Count; i++)
				{
					IAgentOriginBase agentOriginBase = list[i];
					bool flag = Mission.Current.GetAgentTroopClass(this._side, agentOriginBase.Troop).IsRanged();
					List<KeyValuePair<int, LordsHallFightMissionController.AreaData>> list2 = areaMarkerDictionary.ElementAt<KeyValuePair<int, Dictionary<int, LordsHallFightMissionController.AreaData>>>(i % areaMarkerDictionary.Count).Value.ToList<KeyValuePair<int, LordsHallFightMissionController.AreaData>>();
					List<ValueTuple<KeyValuePair<int, LordsHallFightMissionController.AreaData>, float>> list3 = new List<ValueTuple<KeyValuePair<int, LordsHallFightMissionController.AreaData>, float>>();
					foreach (KeyValuePair<int, LordsHallFightMissionController.AreaData> keyValuePair in list2)
					{
						int num = 1000 * keyValuePair.Value.GetAvailableMachines(flag).Count<LordsHallFightMissionController.AreaEntityData>() + keyValuePair.Value.GetAvailableMachines(!flag).Count<LordsHallFightMissionController.AreaEntityData>();
						list3.Add(new ValueTuple<KeyValuePair<int, LordsHallFightMissionController.AreaData>, float>(new KeyValuePair<int, LordsHallFightMissionController.AreaData>(keyValuePair.Key, keyValuePair.Value), (float)num));
					}
					KeyValuePair<int, LordsHallFightMissionController.AreaData> keyValuePair2 = MBRandom.ChooseWeighted<KeyValuePair<int, LordsHallFightMissionController.AreaData>>(list3);
					LordsHallFightMissionController.AreaEntityData areaEntityData = keyValuePair2.Value.GetAvailableMachines(flag).GetRandomElementInefficiently<LordsHallFightMissionController.AreaEntityData>() ?? keyValuePair2.Value.GetAvailableMachines(!flag).GetRandomElementInefficiently<LordsHallFightMissionController.AreaEntityData>();
					MatrixFrame globalFrame = areaEntityData.Entity.GetGlobalFrame();
					Agent agent = Mission.Current.SpawnTroop(agentOriginBase, false, false, false, false, 0, 0, false, false, new Vec3?(globalFrame.origin), new Vec2?(globalFrame.rotation.f.AsVec2.Normalized()), null, null, FormationClass.NumberOfAllFormations, false);
					this._numberOfSpawnedTroops++;
					AgentFlag agentFlags = agent.GetAgentFlags();
					agent.SetAgentFlags(agentFlags & ~AgentFlag.CanRetreat);
					agent.WieldInitialWeapons(Agent.WeaponWieldActionType.Instant, Equipment.InitialWeaponEquipPreference.Any);
					agent.SetWatchState(Agent.WatchState.Alarmed);
					agent.SetBehaviorValueSet(HumanAIComponent.BehaviorValueSet.DefensiveArrangementMove);
					areaEntityData.AssignAgent(agent);
				}
			}

			// Token: 0x06004265 RID: 16997 RVA: 0x00100308 File Offset: 0x000FE508
			public void SpawnTroops(int spawnCount, bool isReinforcement)
			{
				if (this._troopSpawningActive)
				{
					List<IAgentOriginBase> list = this._troopSupplier.SupplyTroops(spawnCount).ToList<IAgentOriginBase>();
					for (int i = 0; i < list.Count; i++)
					{
						if (BattleSideEnum.Attacker == this._side)
						{
							Mission.Current.SpawnTroop(list[i], this._isPlayerSide, true, false, isReinforcement, spawnCount, i, true, true, null, null, null, null, FormationClass.NumberOfAllFormations, false);
							this._numberOfSpawnedTroops++;
						}
					}
				}
			}

			// Token: 0x06004266 RID: 16998 RVA: 0x0010038D File Offset: 0x000FE58D
			public void SetSpawnTroops(bool spawnTroops)
			{
				this._troopSpawningActive = spawnTroops;
			}

			// Token: 0x06004267 RID: 16999 RVA: 0x00100396 File Offset: 0x000FE596
			public IEnumerable<IAgentOriginBase> GetAllTroops()
			{
				return this._troopSupplier.GetAllTroops();
			}

			// Token: 0x0400233F RID: 9023
			private readonly BattleSideEnum _side;

			// Token: 0x04002340 RID: 9024
			private readonly IMissionTroopSupplier _troopSupplier;

			// Token: 0x04002341 RID: 9025
			private readonly bool _isPlayerSide;

			// Token: 0x04002342 RID: 9026
			private bool _troopSpawningActive = true;

			// Token: 0x04002343 RID: 9027
			private int _numberOfSpawnedTroops;
		}

		// Token: 0x0200069E RID: 1694
		private class AreaData
		{
			// Token: 0x17000B1F RID: 2847
			// (get) Token: 0x06004269 RID: 17001 RVA: 0x001003D3 File Offset: 0x000FE5D3
			public IEnumerable<FightAreaMarker> AreaList
			{
				get
				{
					return this._areaList;
				}
			}

			// Token: 0x17000B20 RID: 2848
			// (get) Token: 0x0600426A RID: 17002 RVA: 0x001003DB File Offset: 0x000FE5DB
			public IEnumerable<LordsHallFightMissionController.AreaEntityData> ArcherUsablePoints
			{
				get
				{
					return this._archerUsablePoints;
				}
			}

			// Token: 0x17000B21 RID: 2849
			// (get) Token: 0x0600426B RID: 17003 RVA: 0x001003E3 File Offset: 0x000FE5E3
			public IEnumerable<LordsHallFightMissionController.AreaEntityData> InfantryUsablePoints
			{
				get
				{
					return this._infantryUsablePoints;
				}
			}

			// Token: 0x0600426C RID: 17004 RVA: 0x001003EC File Offset: 0x000FE5EC
			public AreaData(List<FightAreaMarker> areaList)
			{
				this._areaList = new List<FightAreaMarker>();
				this._archerUsablePoints = new List<LordsHallFightMissionController.AreaEntityData>();
				this._infantryUsablePoints = new List<LordsHallFightMissionController.AreaEntityData>();
				foreach (FightAreaMarker fightAreaMarker in areaList)
				{
					this.AddAreaMarker(fightAreaMarker);
				}
			}

			// Token: 0x0600426D RID: 17005 RVA: 0x00100464 File Offset: 0x000FE664
			public IEnumerable<LordsHallFightMissionController.AreaEntityData> GetAvailableMachines(bool isArcher)
			{
				List<LordsHallFightMissionController.AreaEntityData> list = (isArcher ? this._archerUsablePoints : this._infantryUsablePoints);
				foreach (LordsHallFightMissionController.AreaEntityData areaEntityData in list)
				{
					if (!areaEntityData.InUse)
					{
						yield return areaEntityData;
					}
				}
				List<LordsHallFightMissionController.AreaEntityData>.Enumerator enumerator = default(List<LordsHallFightMissionController.AreaEntityData>.Enumerator);
				yield break;
				yield break;
			}

			// Token: 0x0600426E RID: 17006 RVA: 0x0010047C File Offset: 0x000FE67C
			public void AddAreaMarker(FightAreaMarker marker)
			{
				this._areaList.Add(marker);
				using (List<GameEntity>.Enumerator enumerator = marker.GetGameEntitiesWithTagInRange("defender_archer").GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameEntity entity2 = enumerator.Current;
						PathFaceRecord nullFaceRecord = PathFaceRecord.NullFaceRecord;
						Mission.Current.Scene.GetNavMeshFaceIndex(ref nullFaceRecord, entity2.GetGlobalFrame().origin, true);
						if (nullFaceRecord.FaceIndex != -1 && this._archerUsablePoints.All<LordsHallFightMissionController.AreaEntityData>((LordsHallFightMissionController.AreaEntityData x) => x.Entity != entity2))
						{
							this._archerUsablePoints.Add(new LordsHallFightMissionController.AreaEntityData(entity2));
						}
					}
				}
				using (List<GameEntity>.Enumerator enumerator = marker.GetGameEntitiesWithTagInRange("defender_infantry").GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameEntity entity = enumerator.Current;
						if (this._infantryUsablePoints.All<LordsHallFightMissionController.AreaEntityData>((LordsHallFightMissionController.AreaEntityData x) => x.Entity != entity))
						{
							this._infantryUsablePoints.Add(new LordsHallFightMissionController.AreaEntityData(entity));
						}
					}
				}
			}

			// Token: 0x0600426F RID: 17007 RVA: 0x001005C8 File Offset: 0x000FE7C8
			public LordsHallFightMissionController.AreaEntityData FindAgentMachine(Agent agent)
			{
				return this._infantryUsablePoints.FirstOrDefault<LordsHallFightMissionController.AreaEntityData>((LordsHallFightMissionController.AreaEntityData x) => x.UserAgent == agent) ?? this._archerUsablePoints.FirstOrDefault<LordsHallFightMissionController.AreaEntityData>((LordsHallFightMissionController.AreaEntityData x) => x.UserAgent == agent);
			}

			// Token: 0x04002344 RID: 9028
			private const string ArcherSpawnPointTag = "defender_archer";

			// Token: 0x04002345 RID: 9029
			private const string InfantrySpawnPointTag = "defender_infantry";

			// Token: 0x04002346 RID: 9030
			private readonly List<FightAreaMarker> _areaList;

			// Token: 0x04002347 RID: 9031
			private readonly List<LordsHallFightMissionController.AreaEntityData> _archerUsablePoints;

			// Token: 0x04002348 RID: 9032
			private readonly List<LordsHallFightMissionController.AreaEntityData> _infantryUsablePoints;
		}

		// Token: 0x0200069F RID: 1695
		private class AreaEntityData
		{
			// Token: 0x17000B22 RID: 2850
			// (get) Token: 0x06004270 RID: 17008 RVA: 0x00100614 File Offset: 0x000FE814
			// (set) Token: 0x06004271 RID: 17009 RVA: 0x0010061C File Offset: 0x000FE81C
			public Agent UserAgent { get; private set; }

			// Token: 0x17000B23 RID: 2851
			// (get) Token: 0x06004272 RID: 17010 RVA: 0x00100625 File Offset: 0x000FE825
			public bool InUse
			{
				get
				{
					return this.UserAgent != null;
				}
			}

			// Token: 0x06004273 RID: 17011 RVA: 0x00100630 File Offset: 0x000FE830
			public AreaEntityData(GameEntity entity)
			{
				this.Entity = entity;
			}

			// Token: 0x06004274 RID: 17012 RVA: 0x00100640 File Offset: 0x000FE840
			public void AssignAgent(Agent agent)
			{
				this.UserAgent = agent;
				MatrixFrame globalFrame = this.Entity.GetGlobalFrame();
				agent.SetBehaviorValueSet(HumanAIComponent.BehaviorValueSet.DefaultMove);
				this.UserAgent.SetFormationFrameEnabled(new WorldPosition(agent.Mission.Scene, globalFrame.origin), globalFrame.rotation.f.AsVec2.Normalized(), Vec2.Zero, 0f);
			}

			// Token: 0x06004275 RID: 17013 RVA: 0x001006AB File Offset: 0x000FE8AB
			public void StopUse()
			{
				if (this.UserAgent.IsActive())
				{
					this.UserAgent.SetFormationFrameDisabled();
				}
				this.UserAgent = null;
			}

			// Token: 0x04002349 RID: 9033
			public readonly GameEntity Entity;
		}
	}
}
