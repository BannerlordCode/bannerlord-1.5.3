using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000291 RID: 657
	public class MissionBattleSideSpawnContext
	{
		// Token: 0x17000731 RID: 1841
		// (get) Token: 0x0600249B RID: 9371 RVA: 0x00084BD1 File Offset: 0x00082DD1
		// (set) Token: 0x0600249C RID: 9372 RVA: 0x00084BD9 File Offset: 0x00082DD9
		public bool TroopSpawnActive { get; private set; }

		// Token: 0x17000732 RID: 1842
		// (get) Token: 0x0600249D RID: 9373 RVA: 0x00084BE2 File Offset: 0x00082DE2
		public bool IsPlayerSide { get; }

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x0600249E RID: 9374 RVA: 0x00084BEA File Offset: 0x00082DEA
		// (set) Token: 0x0600249F RID: 9375 RVA: 0x00084BF2 File Offset: 0x00082DF2
		public bool ReinforcementSpawnActive { get; private set; }

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x060024A0 RID: 9376 RVA: 0x00084BFB File Offset: 0x00082DFB
		public bool SpawnWithHorses
		{
			get
			{
				return this._spawnWithHorses;
			}
		}

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x060024A1 RID: 9377 RVA: 0x00084C03 File Offset: 0x00082E03
		// (set) Token: 0x060024A2 RID: 9378 RVA: 0x00084C0B File Offset: 0x00082E0B
		public bool ReinforcementsNotifiedOnLastBatch { get; private set; }

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x060024A3 RID: 9379 RVA: 0x00084C14 File Offset: 0x00082E14
		public int NumberOfActiveTroops
		{
			get
			{
				return this._numSpawnedTroops - this._troopSupplier.NumRemovedTroops;
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x060024A4 RID: 9380 RVA: 0x00084C28 File Offset: 0x00082E28
		public int ReinforcementQuotaRequirement
		{
			get
			{
				return this._reinforcementQuotaRequirement;
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x060024A5 RID: 9381 RVA: 0x00084C30 File Offset: 0x00082E30
		public int ReinforcementsSpawnedInLastBatch
		{
			get
			{
				return this._reinforcementsSpawnedInLastBatch;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x060024A6 RID: 9382 RVA: 0x00084C38 File Offset: 0x00082E38
		public float ReinforcementBatchSize
		{
			get
			{
				return (float)this._reinforcementBatchSize;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x060024A7 RID: 9383 RVA: 0x00084C41 File Offset: 0x00082E41
		public bool HasReservedTroops
		{
			get
			{
				return this._reservedTroops.Count > 0;
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x060024A8 RID: 9384 RVA: 0x00084C51 File Offset: 0x00082E51
		public bool HasSpawnableReinforcements
		{
			get
			{
				return this.ReinforcementSpawnActive && this.HasReservedTroops && this.ReinforcementBatchSize > 0f;
			}
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x060024A9 RID: 9385 RVA: 0x00084C72 File Offset: 0x00082E72
		// (set) Token: 0x060024AA RID: 9386 RVA: 0x00084C7A File Offset: 0x00082E7A
		public bool ForceSpawnPlayerMounted { get; private set; }

		// Token: 0x1700073D RID: 1853
		// (get) Token: 0x060024AB RID: 9387 RVA: 0x00084C83 File Offset: 0x00082E83
		public float ReinforcementBatchPriority
		{
			get
			{
				return this._reinforcementBatchPriority;
			}
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x00084C8B File Offset: 0x00082E8B
		public int GetNumberOfPlayerControllableTroops()
		{
			return this._troopSupplier.GetNumberOfPlayerControllableTroops();
		}

		// Token: 0x1700073E RID: 1854
		// (get) Token: 0x060024AD RID: 9389 RVA: 0x00084C98 File Offset: 0x00082E98
		public int ReservedTroopsCount
		{
			get
			{
				return this._reservedTroops.Count;
			}
		}

		// Token: 0x060024AE RID: 9390 RVA: 0x00084CA8 File Offset: 0x00082EA8
		public MissionBattleSideSpawnContext(IBattleMissionAgentSpawnLogic spawnLogic, BattleSideEnum side, IMissionTroopSupplier troopSupplier, bool isPlayerSide, bool forceSpawnPlayerMounted = true)
		{
			this._spawnLogic = spawnLogic;
			this._side = side;
			this._spawnWithHorses = true;
			this._spawnedFormations = new MBArrayList<Formation>();
			this._troopSupplier = troopSupplier;
			this._reinforcementQuotaRequirement = 0;
			this._reinforcementBatchSize = 0;
			this._reinforcementSpawnedUnitCountPerFormation = new ValueTuple<int, int>[8];
			this._reinforcementTroopFormationAssignments = new Dictionary<IAgentOriginBase, int>();
			this.IsPlayerSide = isPlayerSide;
			this.ReinforcementsNotifiedOnLastBatch = false;
			this.ForceSpawnPlayerMounted = forceSpawnPlayerMounted;
		}

		// Token: 0x060024AF RID: 9391 RVA: 0x00084D2C File Offset: 0x00082F2C
		public int TryReinforcementSpawn()
		{
			int num = 0;
			if (this.ReinforcementSpawnActive && this.TroopSpawnActive && this._reservedTroops.Count > 0)
			{
				int num2 = DefaultBattleMissionAgentSpawnLogic.MaxNumberOfAgentsForMission - this._spawnLogic.NumberOfAgents;
				int reservedTroopQuota = this.GetReservedTroopQuota(0);
				if (num2 >= reservedTroopQuota)
				{
					num = this.SpawnTroops(1, true);
					if (num > 0)
					{
						this._reinforcementQuotaRequirement -= reservedTroopQuota;
						if (this._reservedTroops.Count >= this._reinforcementBatchSize)
						{
							this._reinforcementQuotaRequirement += this.GetReservedTroopQuota(this._reinforcementBatchSize - 1);
						}
						this._reinforcementBatchPriority /= 2f;
					}
				}
			}
			this._reinforcementsSpawnedInLastBatch += num;
			return num;
		}

		// Token: 0x060024B0 RID: 9392 RVA: 0x00084DE8 File Offset: 0x00082FE8
		public void GetTeamFormationsSpawnData([TupleElementNames(new string[] { "team", "formationSpawnData" })] out MBList<ValueTuple<Team, MissionFormationSpawnData[]>> teamFormationsSpawnData)
		{
			Mission mission = Mission.Current;
			teamFormationsSpawnData = new MBList<ValueTuple<Team, MissionFormationSpawnData[]>>();
			foreach (Team team in mission.Teams.Where<Team>((Team t) => t.Side == this._side && t == mission.PlayerTeam).Concat<Team>(mission.Teams.Where<Team>((Team t) => t.Side == this._side && t != mission.PlayerTeam)))
			{
				if (team.Side == this._side)
				{
					MissionFormationSpawnData[] array = new MissionFormationSpawnData[11];
					for (int i = 0; i < array.Length; i++)
					{
						array[i].FootTroopCount = 0;
						array[i].MountedTroopCount = 0;
					}
					teamFormationsSpawnData.Add(new ValueTuple<Team, MissionFormationSpawnData[]>(team, array));
				}
			}
			bool flag = this._side == mission.PlayerTeam.Side;
			Dictionary<Team, List<IAgentOriginBase>> dictionary = new Dictionary<Team, List<IAgentOriginBase>>();
			foreach (IAgentOriginBase agentOriginBase in this._reservedTroops)
			{
				Team agentTeam = Mission.GetAgentTeam(agentOriginBase, flag);
				List<IAgentOriginBase> list;
				if (!dictionary.TryGetValue(agentTeam, out list))
				{
					list = new List<IAgentOriginBase>();
					dictionary[agentTeam] = list;
				}
				list.Add(agentOriginBase);
			}
			foreach (KeyValuePair<Team, List<IAgentOriginBase>> keyValuePair in dictionary)
			{
				Team troopTeam = keyValuePair.Key;
				List<IAgentOriginBase> value = keyValuePair.Value;
				MissionFormationSpawnData[] item = teamFormationsSpawnData.FirstOrDefault<ValueTuple<Team, MissionFormationSpawnData[]>>(([TupleElementNames(new string[] { "team", "formationSpawnData" })] ValueTuple<Team, MissionFormationSpawnData[]> tf) => tf.Item1 == troopTeam).Item2;
				foreach (ValueTuple<IAgentOriginBase, int> valueTuple in MissionGameModels.Current.BattleSpawnModel.GetInitialSpawnAssignments(this._side, value))
				{
					IAgentOriginBase item2 = valueTuple.Item1;
					int item3 = valueTuple.Item2;
					if (item2.Troop.HasMount() && this.SpawnWithHorses)
					{
						MissionFormationSpawnData[] array2 = item;
						int num = item3;
						array2[num].MountedTroopCount = array2[num].MountedTroopCount + 1;
					}
					else
					{
						MissionFormationSpawnData[] array3 = item;
						int num2 = item3;
						array3[num2].FootTroopCount = array3[num2].FootTroopCount + 1;
					}
				}
			}
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x0008507C File Offset: 0x0008327C
		public void ReserveTroops(int number)
		{
			if (number > 0 && this._troopSupplier.AnyTroopRemainsToBeSupplied)
			{
				this._reservedTroops.AddRange(this._troopSupplier.SupplyTroops(number));
			}
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x000850A6 File Offset: 0x000832A6
		public BasicCharacterObject GetGeneralCharacter()
		{
			return this._troopSupplier.GetGeneralCharacter();
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x000850B4 File Offset: 0x000832B4
		public unsafe bool CheckReinforcementBatch()
		{
			MissionSpawnPhase missionSpawnPhase;
			if (this._side == BattleSideEnum.Defender)
			{
				missionSpawnPhase = this._spawnLogic.DefenderActivePhase;
			}
			else
			{
				missionSpawnPhase = this._spawnLogic.AttackerActivePhase;
			}
			this._reinforcementsSpawnedInLastBatch = 0;
			this.ReinforcementsNotifiedOnLastBatch = false;
			int num = 0;
			MissionSpawnSettings missionSpawnSettings = *this._spawnLogic.SpawnSettings;
			switch (missionSpawnSettings.ReinforcementTroopsSpawnMethod)
			{
			case MissionSpawnSettings.ReinforcementSpawnMethod.Balanced:
				num = this.ComputeBalancedBatch(missionSpawnPhase);
				break;
			case MissionSpawnSettings.ReinforcementSpawnMethod.Wave:
				num = this.ComputeWaveBatch(missionSpawnPhase);
				break;
			case MissionSpawnSettings.ReinforcementSpawnMethod.Fixed:
				num = this.ComputeFixedBatch(missionSpawnPhase);
				break;
			}
			num = Math.Min(num, missionSpawnPhase.RemainingSpawnNumber);
			num -= this._reservedTroops.Count;
			if (num > 0)
			{
				int count = this._reservedTroops.Count;
				this.ReserveTroops(num);
				if (count < this._reinforcementBatchSize)
				{
					int num2 = Math.Min(this._reservedTroops.Count, this._reinforcementBatchSize);
					for (int i = count; i < num2; i++)
					{
						this._reinforcementQuotaRequirement += this.GetReservedTroopQuota(i);
					}
				}
			}
			this._reinforcementBatchPriority = (float)this._reservedTroops.Count;
			bool flag;
			if (missionSpawnSettings.ReinforcementTroopsSpawnMethod == MissionSpawnSettings.ReinforcementSpawnMethod.Wave)
			{
				flag = this._reservedTroops.Count > 0;
			}
			else
			{
				flag = this._reservedTroops.Count > 0 && (this._reservedTroops.Count >= this._reinforcementBatchSize || missionSpawnPhase.RemainingSpawnNumber <= this._reinforcementBatchSize);
			}
			this.ReinforcementSpawnActive = flag;
			if (this.ReinforcementSpawnActive)
			{
				this.ResetReinforcementSpawnedUnitCountsPerFormation();
				foreach (Team team in Mission.Current.Teams)
				{
					if (team.Side == this._side)
					{
						this._spawnLogic.DeploymentPlan.UpdateReinforcementPlan(team);
					}
				}
			}
			return this.ReinforcementSpawnActive;
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x000852A8 File Offset: 0x000834A8
		public IEnumerable<IAgentOriginBase> GetAllTroops()
		{
			return this._troopSupplier.GetAllTroops();
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x000852B8 File Offset: 0x000834B8
		public int SpawnTroops(int number, bool isReinforcement)
		{
			if (number <= 0)
			{
				return 0;
			}
			List<IAgentOriginBase> list = new List<IAgentOriginBase>();
			int num = MathF.Min(this._reservedTroops.Count, number);
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					IAgentOriginBase agentOriginBase = this._reservedTroops[i];
					list.Add(agentOriginBase);
				}
				this._reservedTroops.RemoveRange(0, num);
			}
			int num2 = number - num;
			list.AddRange(this._troopSupplier.SupplyTroops(num2));
			Mission mission = Mission.Current;
			if (this._troopOriginsToSpawnPerTeam == null)
			{
				this._troopOriginsToSpawnPerTeam = new List<ValueTuple<Team, List<IAgentOriginBase>>>();
				using (List<Team>.Enumerator enumerator = mission.Teams.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Team team = enumerator.Current;
						bool flag = team.Side == mission.PlayerTeam.Side;
						if ((this.IsPlayerSide && flag) || (!this.IsPlayerSide && !flag))
						{
							this._troopOriginsToSpawnPerTeam.Add(new ValueTuple<Team, List<IAgentOriginBase>>(team, new List<IAgentOriginBase>()));
						}
					}
					goto IL_0136;
				}
			}
			foreach (ValueTuple<Team, List<IAgentOriginBase>> valueTuple in this._troopOriginsToSpawnPerTeam)
			{
				valueTuple.Item2.Clear();
			}
			IL_0136:
			int num3 = 0;
			foreach (IAgentOriginBase agentOriginBase2 in list)
			{
				Team agentTeam = Mission.GetAgentTeam(agentOriginBase2, this.IsPlayerSide);
				foreach (ValueTuple<Team, List<IAgentOriginBase>> valueTuple2 in this._troopOriginsToSpawnPerTeam)
				{
					if (agentTeam == valueTuple2.Item1)
					{
						num3++;
						valueTuple2.Item2.Add(agentOriginBase2);
					}
				}
			}
			int num4 = 0;
			List<IAgentOriginBase> list2 = new List<IAgentOriginBase>();
			foreach (ValueTuple<Team, List<IAgentOriginBase>> valueTuple3 in this._troopOriginsToSpawnPerTeam)
			{
				if (!valueTuple3.Item2.IsEmpty<IAgentOriginBase>())
				{
					int num5 = 0;
					List<ValueTuple<IAgentOriginBase, int>> list3 = null;
					if (isReinforcement)
					{
						list3 = new List<ValueTuple<IAgentOriginBase, int>>();
						using (List<IAgentOriginBase>.Enumerator enumerator3 = valueTuple3.Item2.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								IAgentOriginBase agentOriginBase3 = enumerator3.Current;
								int num6;
								this._reinforcementTroopFormationAssignments.TryGetValue(agentOriginBase3, out num6);
								list3.Add(new ValueTuple<IAgentOriginBase, int>(agentOriginBase3, num6));
							}
							goto IL_027A;
						}
						goto IL_025C;
					}
					goto IL_025C;
					IL_027A:
					for (int j = 7; j >= 0; j--)
					{
						int num7 = 0;
						int num8 = 0;
						list2.Clear();
						IAgentOriginBase agentOriginBase4 = null;
						foreach (ValueTuple<IAgentOriginBase, int> valueTuple4 in list3)
						{
							IAgentOriginBase item = valueTuple4.Item1;
							int item2 = valueTuple4.Item2;
							if (j == item2)
							{
								if (item.Troop == Game.Current.PlayerTroop)
								{
									agentOriginBase4 = item;
								}
								else
								{
									if (item.Troop.HasMount())
									{
										num7++;
									}
									else
									{
										num8++;
									}
									list2.Add(item);
								}
							}
						}
						if (agentOriginBase4 != null)
						{
							if (agentOriginBase4.Troop.HasMount())
							{
								num7++;
							}
							else
							{
								num8++;
							}
							list2.Add(agentOriginBase4);
						}
						int count = list2.Count;
						if (count > 0)
						{
							bool flag2 = this._spawnWithHorses && DefaultMissionDeploymentPlan.HasSignificantMountedTroops(num8, num7);
							int num9 = 0;
							int num10 = count;
							if (this.ReinforcementSpawnActive)
							{
								num9 = this._reinforcementSpawnedUnitCountPerFormation[j].Item1;
								num10 = this._reinforcementSpawnedUnitCountPerFormation[j].Item2;
							}
							Formation formation = valueTuple3.Item1.GetFormation((FormationClass)j);
							if (!formation.HasBeenPositioned)
							{
								formation.BeginSpawn(num10, flag2);
								mission.SetFormationPositioningFromDeploymentPlan(formation, isReinforcement);
								this._spawnedFormations.Add(formation);
							}
							foreach (IAgentOriginBase agentOriginBase5 in list2)
							{
								if (!agentOriginBase5.Troop.IsHero && this._bannerBearerLogic != null && mission.Mode != MissionMode.Deployment && this._bannerBearerLogic.GetMissingBannerCount(formation) > 0)
								{
									this._bannerBearerLogic.SpawnBannerBearer(agentOriginBase5, this.IsPlayerSide, formation, this._spawnWithHorses, isReinforcement, num10, num9, true, true, null, null, null, mission.IsSallyOutBattle);
								}
								else
								{
									bool flag3 = (agentOriginBase5.Troop.IsPlayerCharacter && this.ForceSpawnPlayerMounted) || this._spawnWithHorses;
									mission.SpawnTroop(agentOriginBase5, this.IsPlayerSide, true, flag3, isReinforcement, num10, num9, true, true, null, null, null, null, formation.FormationIndex, mission.IsSallyOutBattle);
								}
								this._numSpawnedTroops++;
								num9++;
								num5++;
							}
							if (this.ReinforcementSpawnActive)
							{
								this._reinforcementSpawnedUnitCountPerFormation[j].Item1 = num9;
							}
						}
					}
					if (num5 > 0)
					{
						valueTuple3.Item1.QuerySystem.Expire();
					}
					num4 += num5;
					foreach (Formation formation2 in valueTuple3.Item1.FormationsIncludingEmpty)
					{
						if (formation2.CountOfUnits > 0 && formation2.IsSpawning)
						{
							formation2.EndSpawn();
						}
					}
					continue;
					IL_025C:
					list3 = MissionGameModels.Current.BattleSpawnModel.GetInitialSpawnAssignments(this._side, valueTuple3.Item2);
					goto IL_027A;
				}
			}
			return num4;
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x00085944 File Offset: 0x00083B44
		public void SetSpawnWithHorses(bool spawnWithHorses)
		{
			this._spawnWithHorses = spawnWithHorses;
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x00085950 File Offset: 0x00083B50
		private unsafe int ComputeBalancedBatch(MissionSpawnPhase activePhase)
		{
			int num = 0;
			if (activePhase != null && activePhase.RemainingSpawnNumber > 0)
			{
				MissionSpawnSettings missionSpawnSettings = *this._spawnLogic.SpawnSettings;
				int reinforcementBatchSize = this._reinforcementBatchSize;
				this._reinforcementBatchSize = (int)((float)this._spawnLogic.BattleSize * missionSpawnSettings.ReinforcementBatchPercentage);
				if (reinforcementBatchSize != this._reinforcementBatchSize)
				{
					this.UpdateReinforcementQuotaRequirement(reinforcementBatchSize);
				}
				int num2 = activePhase.TotalSpawnNumber - activePhase.InitialSpawnedNumber;
				num = MathF.Max(1, this._reservedTroops.Count + (int)((float)num2 * missionSpawnSettings.DesiredReinforcementPercentage));
				num = MathF.Min(num, activePhase.InitialSpawnedNumber - this.NumberOfActiveTroops);
			}
			return num;
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x000859F8 File Offset: 0x00083BF8
		private unsafe int ComputeFixedBatch(MissionSpawnPhase activePhase)
		{
			int num = 0;
			if (activePhase != null && activePhase.RemainingSpawnNumber > 0)
			{
				MissionSpawnSettings missionSpawnSettings = *this._spawnLogic.SpawnSettings;
				float num2 = ((this._side == BattleSideEnum.Defender) ? missionSpawnSettings.DefenderReinforcementBatchPercentage : missionSpawnSettings.AttackerReinforcementBatchPercentage);
				int reinforcementBatchSize = this._reinforcementBatchSize;
				this._reinforcementBatchSize = (int)((float)this._spawnLogic.TotalSpawnNumber * num2);
				if (reinforcementBatchSize != this._reinforcementBatchSize)
				{
					this.UpdateReinforcementQuotaRequirement(reinforcementBatchSize);
				}
				num = MathF.Max(1, this._reinforcementBatchSize);
			}
			return num;
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x00085A78 File Offset: 0x00083C78
		private unsafe int ComputeWaveBatch(MissionSpawnPhase activePhase)
		{
			int num = 0;
			if (activePhase != null && activePhase.RemainingSpawnNumber > 0 && this._reservedTroops.IsEmpty<IAgentOriginBase>())
			{
				MissionSpawnSettings missionSpawnSettings = *this._spawnLogic.SpawnSettings;
				int reinforcementBatchSize = this._reinforcementBatchSize;
				int num2 = (int)Math.Max(1f, (float)activePhase.InitialSpawnedNumber * missionSpawnSettings.ReinforcementWavePercentage);
				this._reinforcementBatchSize = num2;
				if (reinforcementBatchSize != this._reinforcementBatchSize)
				{
					this.UpdateReinforcementQuotaRequirement(reinforcementBatchSize);
				}
				if (activePhase.InitialSpawnedNumber - activePhase.NumberActiveTroops >= num2)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x00085AFD File Offset: 0x00083CFD
		public void SetBannerBearerLogic(BannerBearerLogic bannerBearerLogic)
		{
			this._bannerBearerLogic = bannerBearerLogic;
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x00085B08 File Offset: 0x00083D08
		private void UpdateReinforcementQuotaRequirement(int previousBatchSize)
		{
			if (this._reinforcementBatchSize < previousBatchSize)
			{
				for (int i = MathF.Min(this._reservedTroops.Count - 1, previousBatchSize - 1); i >= this._reinforcementBatchSize; i--)
				{
					this._reinforcementQuotaRequirement -= this.GetReservedTroopQuota(i);
				}
				return;
			}
			if (this._reinforcementBatchSize > previousBatchSize)
			{
				int num = MathF.Min(this._reservedTroops.Count - 1, this._reinforcementBatchSize - 1);
				for (int j = previousBatchSize; j <= num; j++)
				{
					this._reinforcementQuotaRequirement += this.GetReservedTroopQuota(j);
				}
			}
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x00085B9C File Offset: 0x00083D9C
		public void SetReinforcementsNotifiedOnLastBatch(bool value)
		{
			this.ReinforcementsNotifiedOnLastBatch = value;
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x00085BA8 File Offset: 0x00083DA8
		private void ResetReinforcementSpawnedUnitCountsPerFormation()
		{
			for (int i = 0; i < 8; i++)
			{
				this._reinforcementSpawnedUnitCountPerFormation[i].Item1 = 0;
				this._reinforcementSpawnedUnitCountPerFormation[i].Item2 = 0;
			}
			this._reinforcementTroopFormationAssignments.Clear();
			foreach (ValueTuple<IAgentOriginBase, int> valueTuple in MissionGameModels.Current.BattleSpawnModel.GetReinforcementAssignments(this._side, this._reservedTroops))
			{
				int item = valueTuple.Item2;
				this._reinforcementTroopFormationAssignments.Add(valueTuple.Item1, valueTuple.Item2);
				ValueTuple<int, int>[] reinforcementSpawnedUnitCountPerFormation = this._reinforcementSpawnedUnitCountPerFormation;
				int num = item;
				reinforcementSpawnedUnitCountPerFormation[num].Item2 = reinforcementSpawnedUnitCountPerFormation[num].Item2 + 1;
			}
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x00085C78 File Offset: 0x00083E78
		public void SetSpawnTroops(bool spawnTroops)
		{
			this.TroopSpawnActive = spawnTroops;
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x00085C81 File Offset: 0x00083E81
		private int GetReservedTroopQuota(int index)
		{
			if (!this._spawnWithHorses || !this._reservedTroops[index].Troop.IsMounted)
			{
				return 1;
			}
			return 2;
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x00085CA8 File Offset: 0x00083EA8
		public void OnInitialSpawnOver()
		{
			foreach (Formation formation in this._spawnedFormations)
			{
				formation.EndSpawn();
			}
		}

		// Token: 0x04000E21 RID: 3617
		private readonly IBattleMissionAgentSpawnLogic _spawnLogic;

		// Token: 0x04000E22 RID: 3618
		private readonly BattleSideEnum _side;

		// Token: 0x04000E23 RID: 3619
		private readonly IMissionTroopSupplier _troopSupplier;

		// Token: 0x04000E24 RID: 3620
		private BannerBearerLogic _bannerBearerLogic;

		// Token: 0x04000E25 RID: 3621
		private readonly MBArrayList<Formation> _spawnedFormations;

		// Token: 0x04000E26 RID: 3622
		private bool _spawnWithHorses;

		// Token: 0x04000E27 RID: 3623
		private float _reinforcementBatchPriority;

		// Token: 0x04000E28 RID: 3624
		private int _reinforcementQuotaRequirement;

		// Token: 0x04000E29 RID: 3625
		private int _reinforcementBatchSize;

		// Token: 0x04000E2A RID: 3626
		private int _reinforcementsSpawnedInLastBatch;

		// Token: 0x04000E2B RID: 3627
		private int _numSpawnedTroops;

		// Token: 0x04000E2C RID: 3628
		private readonly List<IAgentOriginBase> _reservedTroops = new List<IAgentOriginBase>();

		// Token: 0x04000E2D RID: 3629
		[TupleElementNames(new string[] { "team", "origins" })]
		private List<ValueTuple<Team, List<IAgentOriginBase>>> _troopOriginsToSpawnPerTeam;

		// Token: 0x04000E33 RID: 3635
		[TupleElementNames(new string[] { "currentTroopIndex", "troopCount" })]
		private readonly ValueTuple<int, int>[] _reinforcementSpawnedUnitCountPerFormation;

		// Token: 0x04000E34 RID: 3636
		private readonly Dictionary<IAgentOriginBase, int> _reinforcementTroopFormationAssignments;
	}
}
