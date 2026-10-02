using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000287 RID: 647
	public class DefaultBattleMissionAgentSpawnLogic : MissionLogic, IBattleMissionAgentSpawnLogic, IMissionAgentSpawnLogic, IMissionBehavior
	{
		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x06002406 RID: 9222 RVA: 0x00080C12 File Offset: 0x0007EE12
		public static int MaxNumberOfAgentsForMission
		{
			get
			{
				if (DefaultBattleMissionAgentSpawnLogic._maxNumberOfAgentsForMissionCache == 0)
				{
					DefaultBattleMissionAgentSpawnLogic._maxNumberOfAgentsForMissionCache = MBAPI.IMBAgent.GetMaximumNumberOfAgents();
				}
				return DefaultBattleMissionAgentSpawnLogic._maxNumberOfAgentsForMissionCache;
			}
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06002407 RID: 9223 RVA: 0x00080C2F File Offset: 0x0007EE2F
		public static int MaxNumberOfTroopsForMission
		{
			get
			{
				return DefaultBattleMissionAgentSpawnLogic.MaxNumberOfAgentsForMission / 2;
			}
		}

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06002408 RID: 9224 RVA: 0x00080C38 File Offset: 0x0007EE38
		// (remove) Token: 0x06002409 RID: 9225 RVA: 0x00080C70 File Offset: 0x0007EE70
		public event Action<BattleSideEnum, int> OnReinforcementsSpawned;

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x0600240A RID: 9226 RVA: 0x00080CA8 File Offset: 0x0007EEA8
		// (remove) Token: 0x0600240B RID: 9227 RVA: 0x00080CE0 File Offset: 0x0007EEE0
		public event Action<BattleSideEnum, int> OnInitialTroopsSpawned;

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x0600240C RID: 9228 RVA: 0x00080D15 File Offset: 0x0007EF15
		public int NumberOfRemainingTroops
		{
			get
			{
				MissionSpawnPhase defenderActivePhase = this.DefenderActivePhase;
				int num = ((defenderActivePhase != null) ? defenderActivePhase.RemainingSpawnNumber : 0);
				MissionSpawnPhase attackerActivePhase = this.AttackerActivePhase;
				return num + ((attackerActivePhase != null) ? attackerActivePhase.RemainingSpawnNumber : 0);
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x0600240D RID: 9229 RVA: 0x00080D3C File Offset: 0x0007EF3C
		public int NumberOfActiveDefenderTroops
		{
			get
			{
				MissionSpawnPhase defenderActivePhase = this.DefenderActivePhase;
				if (defenderActivePhase == null)
				{
					return 0;
				}
				return defenderActivePhase.NumberActiveTroops;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x0600240E RID: 9230 RVA: 0x00080D4F File Offset: 0x0007EF4F
		public int NumberOfActiveAttackerTroops
		{
			get
			{
				MissionSpawnPhase attackerActivePhase = this.AttackerActivePhase;
				if (attackerActivePhase == null)
				{
					return 0;
				}
				return attackerActivePhase.NumberActiveTroops;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x0600240F RID: 9231 RVA: 0x00080D62 File Offset: 0x0007EF62
		public int NumberOfRemainingDefenderTroops
		{
			get
			{
				MissionSpawnPhase defenderActivePhase = this.DefenderActivePhase;
				if (defenderActivePhase == null)
				{
					return 0;
				}
				return defenderActivePhase.RemainingSpawnNumber;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06002410 RID: 9232 RVA: 0x00080D75 File Offset: 0x0007EF75
		public int NumberOfRemainingAttackerTroops
		{
			get
			{
				MissionSpawnPhase attackerActivePhase = this.AttackerActivePhase;
				if (attackerActivePhase == null)
				{
					return 0;
				}
				return attackerActivePhase.RemainingSpawnNumber;
			}
		}

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x06002411 RID: 9233 RVA: 0x00080D88 File Offset: 0x0007EF88
		// (set) Token: 0x06002412 RID: 9234 RVA: 0x00080D90 File Offset: 0x0007EF90
		public BattleSideEnum PlayerSide { get; private set; }

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x06002413 RID: 9235 RVA: 0x00080D99 File Offset: 0x0007EF99
		public int BattleSize
		{
			get
			{
				return this._battleSize;
			}
		}

		// Token: 0x17000722 RID: 1826
		// (get) Token: 0x06002414 RID: 9236 RVA: 0x00080DA1 File Offset: 0x0007EFA1
		public int TotalSpawnNumber
		{
			get
			{
				MissionSpawnPhase defenderActivePhase = this.DefenderActivePhase;
				int num = ((defenderActivePhase != null) ? defenderActivePhase.TotalSpawnNumber : 0);
				MissionSpawnPhase attackerActivePhase = this.AttackerActivePhase;
				return num + ((attackerActivePhase != null) ? attackerActivePhase.TotalSpawnNumber : 0);
			}
		}

		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06002415 RID: 9237 RVA: 0x00080DC8 File Offset: 0x0007EFC8
		public int NumberOfAgents
		{
			get
			{
				return base.Mission.AllAgents.Count;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06002416 RID: 9238 RVA: 0x00080DDA File Offset: 0x0007EFDA
		public readonly ref MissionSpawnSettings SpawnSettings
		{
			get
			{
				return ref this._spawnSettings;
			}
		}

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002417 RID: 9239 RVA: 0x00080DE2 File Offset: 0x0007EFE2
		public IMissionDeploymentPlan DeploymentPlan
		{
			get
			{
				return this._deploymentPlan;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002418 RID: 9240 RVA: 0x00080DEA File Offset: 0x0007EFEA
		public MissionSpawnPhase DefenderActivePhase
		{
			get
			{
				return this._phases[0].FirstOrDefault<MissionSpawnPhase>();
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06002419 RID: 9241 RVA: 0x00080DF9 File Offset: 0x0007EFF9
		public MissionSpawnPhase AttackerActivePhase
		{
			get
			{
				return this._phases[1].FirstOrDefault<MissionSpawnPhase>();
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x0600241A RID: 9242 RVA: 0x00080E08 File Offset: 0x0007F008
		public bool IsInitialSpawnOver
		{
			get
			{
				return this.DefenderActivePhase.InitialSpawnNumber + this.AttackerActivePhase.InitialSpawnNumber == 0;
			}
		}

		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x0600241B RID: 9243 RVA: 0x00080E24 File Offset: 0x0007F024
		public bool IsDeploymentOver
		{
			get
			{
				return base.Mission.Mode != MissionMode.Deployment && this.IsInitialSpawnOver;
			}
		}

		// Token: 0x0600241C RID: 9244 RVA: 0x00080E3C File Offset: 0x0007F03C
		public DefaultBattleMissionAgentSpawnLogic(IMissionTroopSupplier[] suppliers, BattleSideEnum playerSide, Mission.BattleSizeType battleSizeType)
		{
			this.PlayerSide = playerSide;
			switch (battleSizeType)
			{
			case Mission.BattleSizeType.Battle:
				this._battleSize = BannerlordConfig.GetRealBattleSize();
				break;
			case Mission.BattleSizeType.Siege:
				this._battleSize = BannerlordConfig.GetRealBattleSizeForSiege();
				break;
			case Mission.BattleSizeType.SallyOut:
				this._battleSize = BannerlordConfig.GetRealBattleSizeForSallyOut();
				break;
			}
			this._battleSize = MathF.Min(this._battleSize, DefaultBattleMissionAgentSpawnLogic.MaxNumberOfTroopsForMission);
			this._spawnSettings = MissionSpawnSettings.CreateDefaultSpawnSettings();
			this._globalReinforcementInterval = this._spawnSettings.GlobalReinforcementInterval;
			this._battleSideSpawnContexts = new MissionBattleSideSpawnContext[2];
			for (int i = 0; i < 2; i++)
			{
				IMissionTroopSupplier missionTroopSupplier = suppliers[i];
				bool flag = i == (int)playerSide;
				MissionBattleSideSpawnContext missionBattleSideSpawnContext = new MissionBattleSideSpawnContext(this, (BattleSideEnum)i, missionTroopSupplier, flag, true);
				if (flag)
				{
					this._playerBattleSideSpawnContext = missionBattleSideSpawnContext;
				}
				this._battleSideSpawnContexts[i] = missionBattleSideSpawnContext;
			}
			this._numberOfTroopsInTotal = new int[2];
			this._phases = new List<MissionSpawnPhase>[2];
			for (int j = 0; j < 2; j++)
			{
				this._phases[j] = new List<MissionSpawnPhase>();
			}
			this._reinforcementSpawnEnabled = false;
		}

		// Token: 0x0600241D RID: 9245 RVA: 0x00080F5B File Offset: 0x0007F15B
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._globalReinforcementSpawnTimer = new BasicMissionTimer();
			MissionGameModels.Current.BattleInitializationModel.InitializeModel();
		}

		// Token: 0x0600241E RID: 9246 RVA: 0x00080F80 File Offset: 0x0007F180
		public override void AfterStart()
		{
			this._bannerBearerLogic = base.Mission.GetMissionBehavior<BannerBearerLogic>();
			if (this._bannerBearerLogic != null)
			{
				for (int i = 0; i < 2; i++)
				{
					this._battleSideSpawnContexts[i].SetBannerBearerLogic(this._bannerBearerLogic);
				}
			}
			MissionGameModels.Current.BattleSpawnModel.OnMissionStart();
		}

		// Token: 0x0600241F RID: 9247 RVA: 0x00080FD4 File Offset: 0x0007F1D4
		public override void OnMissionTick(float dt)
		{
			if (!GameNetwork.IsClient && !this.CheckDeployment())
			{
				return;
			}
			this.PhaseTick();
			if (this._reinforcementSpawnEnabled)
			{
				if (this._spawnSettings.ReinforcementTroopsTimingMethod == MissionSpawnSettings.ReinforcementTimingMethod.GlobalTimer)
				{
					this.CheckGlobalReinforcementBatch();
				}
				else if (this._spawnSettings.ReinforcementTroopsTimingMethod == MissionSpawnSettings.ReinforcementTimingMethod.CustomTimer)
				{
					this.CheckCustomReinforcementBatch();
				}
			}
			if (this._spawningReinforcements)
			{
				this.CheckReinforcementSpawn();
			}
		}

		// Token: 0x06002420 RID: 9248 RVA: 0x00081036 File Offset: 0x0007F236
		protected override void OnEndMission()
		{
			MissionGameModels.Current.BattleSpawnModel.OnMissionEnd();
			MissionGameModels.Current.BattleInitializationModel.FinalizeModel();
		}

		// Token: 0x06002421 RID: 9249 RVA: 0x00081056 File Offset: 0x0007F256
		public void InitWithSinglePhase(int defenderTotalSpawn, int attackerTotalSpawn, int defenderInitialSpawn, int attackerInitialSpawn, bool spawnDefenders, bool spawnAttackers, in MissionSpawnSettings spawnSettings)
		{
			this.AddPhase(BattleSideEnum.Defender, defenderTotalSpawn, defenderInitialSpawn);
			this.AddPhase(BattleSideEnum.Attacker, attackerTotalSpawn, attackerInitialSpawn);
			this.Init(spawnDefenders, spawnAttackers, in spawnSettings);
		}

		// Token: 0x06002422 RID: 9250 RVA: 0x00081078 File Offset: 0x0007F278
		public IEnumerable<IAgentOriginBase> GetAllTroopsForSide(BattleSideEnum side)
		{
			return this._battleSideSpawnContexts[(int)side].GetAllTroops();
		}

		// Token: 0x06002423 RID: 9251 RVA: 0x00081094 File Offset: 0x0007F294
		public void SetCustomReinforcementSpawnTimer(ICustomReinforcementSpawnTimer timer)
		{
			this._customReinforcementSpawnTimer = timer;
		}

		// Token: 0x06002424 RID: 9252 RVA: 0x0008109D File Offset: 0x0007F29D
		public void SetSpawnTroops(BattleSideEnum side, bool spawnTroops, bool enforceSpawning = false)
		{
			this._battleSideSpawnContexts[(int)side].SetSpawnTroops(spawnTroops);
			if (spawnTroops && enforceSpawning)
			{
				this.CheckDeployment();
			}
		}

		// Token: 0x06002425 RID: 9253 RVA: 0x000810B9 File Offset: 0x0007F2B9
		public void SetSpawnHorses(BattleSideEnum side, bool spawnHorses)
		{
			this._battleSideSpawnContexts[(int)side].SetSpawnWithHorses(spawnHorses);
		}

		// Token: 0x06002426 RID: 9254 RVA: 0x000810C9 File Offset: 0x0007F2C9
		public void StartSpawner(BattleSideEnum side)
		{
			this._battleSideSpawnContexts[(int)side].SetSpawnTroops(true);
		}

		// Token: 0x06002427 RID: 9255 RVA: 0x000810D9 File Offset: 0x0007F2D9
		public void StopSpawner(BattleSideEnum side)
		{
			this._battleSideSpawnContexts[(int)side].SetSpawnTroops(false);
		}

		// Token: 0x06002428 RID: 9256 RVA: 0x000810E9 File Offset: 0x0007F2E9
		public bool IsSideSpawnEnabled(BattleSideEnum side)
		{
			return this._battleSideSpawnContexts[(int)side].TroopSpawnActive;
		}

		// Token: 0x06002429 RID: 9257 RVA: 0x000810F8 File Offset: 0x0007F2F8
		public void OnSideDeploymentOver(BattleSideEnum battleSide)
		{
			base.Mission.OnInitialSpawnCompleted(battleSide);
			foreach (Team team in base.Mission.Teams)
			{
				if (team.Side == battleSide)
				{
					foreach (Formation formation in team.FormationsIncludingEmpty)
					{
						if (formation.CountOfUnits > 0)
						{
							formation.QuerySystem.EvaluateAllPreliminaryQueryData();
						}
					}
				}
			}
		}

		// Token: 0x0600242A RID: 9258 RVA: 0x000811B0 File Offset: 0x0007F3B0
		public int GetNumberOfPlayerControllableTroops()
		{
			MissionBattleSideSpawnContext playerBattleSideSpawnContext = this._playerBattleSideSpawnContext;
			if (playerBattleSideSpawnContext == null)
			{
				return 0;
			}
			return playerBattleSideSpawnContext.GetNumberOfPlayerControllableTroops();
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x000811C3 File Offset: 0x0007F3C3
		public float GetReinforcementInterval(BattleSideEnum side = BattleSideEnum.None)
		{
			return this._globalReinforcementInterval;
		}

		// Token: 0x0600242C RID: 9260 RVA: 0x000811CC File Offset: 0x0007F3CC
		public void SetReinforcementsSpawnEnabled(bool value, bool resetTimers = true)
		{
			if (this._reinforcementSpawnEnabled != value)
			{
				this._reinforcementSpawnEnabled = value;
				if (resetTimers)
				{
					if (this._spawnSettings.ReinforcementTroopsTimingMethod == MissionSpawnSettings.ReinforcementTimingMethod.GlobalTimer)
					{
						this._globalReinforcementSpawnTimer.Reset();
						return;
					}
					if (this._spawnSettings.ReinforcementTroopsTimingMethod == MissionSpawnSettings.ReinforcementTimingMethod.CustomTimer)
					{
						for (int i = 0; i < 2; i++)
						{
							this._customReinforcementSpawnTimer.ResetTimer((BattleSideEnum)i);
						}
					}
				}
			}
		}

		// Token: 0x0600242D RID: 9261 RVA: 0x0008122B File Offset: 0x0007F42B
		public int GetTotalNumberOfTroopsForSide(BattleSideEnum side)
		{
			return this._numberOfTroopsInTotal[(int)side];
		}

		// Token: 0x0600242E RID: 9262 RVA: 0x00081238 File Offset: 0x0007F438
		public BasicCharacterObject GetGeneralCharacterOfSide(BattleSideEnum side)
		{
			if (side >= BattleSideEnum.Defender && side < BattleSideEnum.NumSides)
			{
				this._battleSideSpawnContexts[(int)side].GetGeneralCharacter();
			}
			return null;
		}

		// Token: 0x0600242F RID: 9263 RVA: 0x0008125E File Offset: 0x0007F45E
		public bool GetSpawnHorses(BattleSideEnum side)
		{
			return this._battleSideSpawnContexts[(int)side].SpawnWithHorses;
		}

		// Token: 0x06002430 RID: 9264 RVA: 0x00081270 File Offset: 0x0007F470
		private bool CheckMinimumBatchQuotaRequirement()
		{
			int num = DefaultBattleMissionAgentSpawnLogic.MaxNumberOfAgentsForMission - this.NumberOfAgents;
			int num2 = 0;
			for (int i = 0; i < 2; i++)
			{
				num2 += this._battleSideSpawnContexts[i].ReinforcementQuotaRequirement;
			}
			return num >= num2;
		}

		// Token: 0x06002431 RID: 9265 RVA: 0x000812AF File Offset: 0x0007F4AF
		public bool IsSideDepleted(BattleSideEnum side)
		{
			return this._phases[(int)side].Count == 1 && this._battleSideSpawnContexts[(int)side].NumberOfActiveTroops == 0 && this.GetActivePhaseForSide(side).RemainingSpawnNumber == 0;
		}

		// Token: 0x06002432 RID: 9266 RVA: 0x000812E1 File Offset: 0x0007F4E1
		public void AddPhaseChangeAction(BattleSideEnum side, DefaultBattleMissionAgentSpawnLogic.OnPhaseChangedDelegate onPhaseChanged)
		{
			DefaultBattleMissionAgentSpawnLogic.OnPhaseChangedDelegate[] onPhaseChanged2 = this._onPhaseChanged;
			onPhaseChanged2[(int)side] = (DefaultBattleMissionAgentSpawnLogic.OnPhaseChangedDelegate)Delegate.Combine(onPhaseChanged2[(int)side], onPhaseChanged);
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x00081300 File Offset: 0x0007F500
		private void CheckGlobalReinforcementBatch()
		{
			if (this._globalReinforcementSpawnTimer.ElapsedTime >= this._globalReinforcementInterval)
			{
				bool flag = false;
				for (int i = 0; i < 2; i++)
				{
					BattleSideEnum battleSideEnum = (BattleSideEnum)i;
					this.NotifyReinforcementTroopsSpawned(battleSideEnum, false);
					bool flag2 = this._battleSideSpawnContexts[i].CheckReinforcementBatch();
					flag = flag || flag2;
				}
				this._spawningReinforcements = flag && this.CheckMinimumBatchQuotaRequirement();
				this._globalReinforcementSpawnTimer.Reset();
			}
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x00081368 File Offset: 0x0007F568
		private void CheckCustomReinforcementBatch()
		{
			bool flag = false;
			for (int i = 0; i < 2; i++)
			{
				BattleSideEnum battleSideEnum = (BattleSideEnum)i;
				if (this._customReinforcementSpawnTimer.Check(battleSideEnum))
				{
					flag = true;
					this.NotifyReinforcementTroopsSpawned(battleSideEnum, false);
					this._battleSideSpawnContexts[i].CheckReinforcementBatch();
				}
			}
			if (flag)
			{
				bool flag2 = false;
				for (int j = 0; j < 2; j++)
				{
					flag2 = flag2 || this._battleSideSpawnContexts[j].ReinforcementSpawnActive;
				}
				this._spawningReinforcements = flag2 && this.CheckMinimumBatchQuotaRequirement();
			}
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x000813E8 File Offset: 0x0007F5E8
		private void Init(bool spawnDefenders, bool spawnAttackers, in MissionSpawnSettings reinforcementSpawnSettings)
		{
			base.Mission.GetDeploymentPlan<DefaultMissionDeploymentPlan>(out this._deploymentPlan);
			List<MissionSpawnPhase>[] phases = this._phases;
			for (int i = 0; i < phases.Length; i++)
			{
				if (phases[i].Count <= 0)
				{
					return;
				}
			}
			foreach (Team team in base.Mission.Teams)
			{
				BattleSideEnum side = team.Side;
				bool spawnWithHorses = this._battleSideSpawnContexts[(int)side].SpawnWithHorses;
				this._deploymentPlan.SetSpawnWithHorses(team, spawnWithHorses);
			}
			this._spawnSettings = reinforcementSpawnSettings;
			int num = 0;
			int num2 = 1;
			this._globalReinforcementInterval = this._spawnSettings.GlobalReinforcementInterval;
			int[] array = new int[2];
			array[0] = this._phases[num].Sum<MissionSpawnPhase>((MissionSpawnPhase p) => p.TotalSpawnNumber);
			array[1] = this._phases[num2].Sum<MissionSpawnPhase>((MissionSpawnPhase p) => p.TotalSpawnNumber);
			int[] array2 = array;
			int num3 = array2.Sum();
			if (this._spawnSettings.InitialTroopsSpawnMethod == MissionSpawnSettings.InitialSpawnMethod.BattleSizeAllocating)
			{
				float[] array3 = new float[]
				{
					(float)array2[num] / (float)num3,
					(float)array2[num2] / (float)num3
				};
				array3[num] = MathF.Min(this._spawnSettings.MaximumBattleSideRatio, array3[num] * this._spawnSettings.DefenderAdvantageFactor);
				array3[num2] = 1f - array3[num];
				int num4 = ((array3[num] < array3[num2]) ? 0 : 1);
				int oppositeSide = (int)((BattleSideEnum)num4).GetOppositeSide();
				int num5 = num4;
				if (array3[oppositeSide] > this._spawnSettings.MaximumBattleSideRatio)
				{
					array3[oppositeSide] = this._spawnSettings.MaximumBattleSideRatio;
					array3[num5] = 1f - this._spawnSettings.MaximumBattleSideRatio;
				}
				int[] array4 = new int[2];
				int num6 = MathF.Ceiling(array3[num5] * (float)this._battleSize);
				array4[num5] = Math.Min(num6, array2[num5]);
				array4[oppositeSide] = this._battleSize - array4[num5];
				for (int j = 0; j < 2; j++)
				{
					foreach (MissionSpawnPhase missionSpawnPhase in this._phases[j])
					{
						if (missionSpawnPhase.InitialSpawnNumber > array4[j])
						{
							int num7 = array4[j];
							int num8 = missionSpawnPhase.InitialSpawnNumber - num7;
							missionSpawnPhase.InitialSpawnNumber = num7;
							missionSpawnPhase.RemainingSpawnNumber += num8;
						}
					}
				}
			}
			else if (this._spawnSettings.InitialTroopsSpawnMethod == MissionSpawnSettings.InitialSpawnMethod.FreeAllocation)
			{
				this._phases[num].Max<MissionSpawnPhase>((MissionSpawnPhase p) => p.InitialSpawnNumber);
				this._phases[num2].Max<MissionSpawnPhase>((MissionSpawnPhase p) => p.InitialSpawnNumber);
			}
			if (this._spawnSettings.ReinforcementTroopsSpawnMethod == MissionSpawnSettings.ReinforcementSpawnMethod.Wave)
			{
				for (int k = 0; k < 2; k++)
				{
					foreach (MissionSpawnPhase missionSpawnPhase2 in this._phases[k])
					{
						int num9 = (int)Math.Max(1f, (float)missionSpawnPhase2.InitialSpawnNumber * this._spawnSettings.ReinforcementWavePercentage);
						if (this._spawnSettings.MaximumReinforcementWaveCount > 0)
						{
							int num10 = Math.Min(missionSpawnPhase2.RemainingSpawnNumber, num9 * this._spawnSettings.MaximumReinforcementWaveCount);
							int num11 = Math.Max(0, missionSpawnPhase2.RemainingSpawnNumber - num10);
							this._numberOfTroopsInTotal[k] -= num11;
							array2[k] -= num11;
							missionSpawnPhase2.RemainingSpawnNumber = num10;
							missionSpawnPhase2.TotalSpawnNumber = missionSpawnPhase2.RemainingSpawnNumber + missionSpawnPhase2.InitialSpawnNumber;
						}
					}
				}
			}
			base.Mission.SetBattleAgentCount(MathF.Min(this.DefenderActivePhase.InitialSpawnNumber, this.AttackerActivePhase.InitialSpawnNumber));
			base.Mission.SetInitialAgentCountForSide(BattleSideEnum.Defender, array2[num]);
			base.Mission.SetInitialAgentCountForSide(BattleSideEnum.Attacker, array2[num2]);
			this._battleSideSpawnContexts[num].SetSpawnTroops(spawnDefenders);
			this._battleSideSpawnContexts[num2].SetSpawnTroops(spawnAttackers);
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x00081878 File Offset: 0x0007FA78
		private void AddPhase(BattleSideEnum side, int totalSpawn, int initialSpawn)
		{
			MissionSpawnPhase missionSpawnPhase = new MissionSpawnPhase
			{
				TotalSpawnNumber = totalSpawn,
				InitialSpawnNumber = initialSpawn,
				RemainingSpawnNumber = totalSpawn - initialSpawn
			};
			this._phases[(int)side].Add(missionSpawnPhase);
			this._numberOfTroopsInTotal[(int)side] += totalSpawn;
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x000818C4 File Offset: 0x0007FAC4
		private void PhaseTick()
		{
			for (int i = 0; i < 2; i++)
			{
				MissionSpawnPhase activePhaseForSide = this.GetActivePhaseForSide((BattleSideEnum)i);
				activePhaseForSide.NumberActiveTroops = this._battleSideSpawnContexts[i].NumberOfActiveTroops;
				if (activePhaseForSide.NumberActiveTroops == 0 && activePhaseForSide.RemainingSpawnNumber == 0 && this._phases[i].Count > 1)
				{
					this._phases[i].Remove(activePhaseForSide);
					BattleSideEnum battleSideEnum = (BattleSideEnum)i;
					if (this.GetActivePhaseForSide(battleSideEnum) != null)
					{
						if (this._onPhaseChanged[i] != null)
						{
							this._onPhaseChanged[i]();
						}
						foreach (Team team in base.Mission.Teams)
						{
							if (team.Side == battleSideEnum)
							{
								if (this._deploymentPlan.IsPlanMade(team))
								{
									this._deploymentPlan.ClearAddedTroops(team, false);
									this._deploymentPlan.ClearDeploymentPlan(team);
								}
								if (this._deploymentPlan.IsReinforcementPlanMade(team))
								{
									this._deploymentPlan.ClearAddedTroops(team, true);
									this._deploymentPlan.ClearReinforcementPlan(team);
								}
							}
						}
						Debug.Print("New spawn phase!", 0, Debug.DebugColor.Green, 64UL);
					}
				}
			}
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x00081A10 File Offset: 0x0007FC10
		private bool CheckDeployment()
		{
			bool flag = this.IsDeploymentOver;
			if (!flag)
			{
				int num = this.DefenderActivePhase.InitialSpawnNumber + this.AttackerActivePhase.InitialSpawnNumber;
				for (int i = 0; i < 2; i++)
				{
					BattleSideEnum battleSideEnum = (BattleSideEnum)i;
					MissionSpawnPhase activePhaseForSide = this.GetActivePhaseForSide(battleSideEnum);
					if (activePhaseForSide.InitialSpawnNumber > 0)
					{
						if (activePhaseForSide.InitialSpawnNumber > this._battleSideSpawnContexts[i].ReservedTroopsCount)
						{
							int num2 = activePhaseForSide.InitialSpawnNumber - this._battleSideSpawnContexts[i].ReservedTroopsCount;
							this._battleSideSpawnContexts[i].ReserveTroops(num2);
						}
						if (this._battleSideSpawnContexts[i].ReservedTroopsCount >= activePhaseForSide.InitialSpawnNumber)
						{
							bool flag2 = true;
							int num3 = 0;
							foreach (Team team in base.Mission.Teams)
							{
								if (team.Side == battleSideEnum)
								{
									flag2 = flag2 && this._deploymentPlan.IsPlanMade(team) && this._deploymentPlan.IsReinforcementPlanMade(team);
									num3++;
								}
							}
							if (num3 > 0 && !flag2)
							{
								MBList<ValueTuple<Team, MissionFormationSpawnData[]>> mblist;
								this._battleSideSpawnContexts[i].GetTeamFormationsSpawnData(out mblist);
								if (base.Mission.HasSpawnPath)
								{
									MBReadOnlyList<ValueTuple<Team, int, MissionFormationSpawnData[]>> mbreadOnlyList = this.CollectSortedBattleSideTeamsData(mblist);
									SpawnPathData initialSpawnPathData = Mission.Current.GetInitialSpawnPathData(battleSideEnum);
									Path path = initialSpawnPathData.Path;
									float[] array = new float[mbreadOnlyList.Count];
									for (int j = 0; j < mbreadOnlyList.Count; j++)
									{
										array[j] = this.GetTeamSpawnPathOffsetRange(initialSpawnPathData, battleSideEnum, mbreadOnlyList[j].Item3);
									}
									Path path2 = initialSpawnPathData.Path;
									float num4 = Mission.ComputeSpawnPathDeploymentOffset(num, path2);
									float num5;
									float num6;
									DefaultBattleMissionAgentSpawnLogic.ComputeDeploymentBaseOffsets(initialSpawnPathData, num4, out num5, out num6);
									float[] array2;
									DefaultBattleMissionAgentSpawnLogic.ComputeTeamDeploymentOffsets(initialSpawnPathData, num5, 20f, array, out array2);
									for (int k = 0; k < mbreadOnlyList.Count; k++)
									{
										ValueTuple<Team, int, MissionFormationSpawnData[]> valueTuple = mbreadOnlyList[k];
										this.MakeTeamPlans(valueTuple.Item1, valueTuple.Item3, array2[k], num6);
									}
								}
								else
								{
									foreach (ValueTuple<Team, MissionFormationSpawnData[]> valueTuple2 in mblist)
									{
										this.MakeTeamPlans(valueTuple2.Item1, valueTuple2.Item2, 0f, 0f);
									}
								}
							}
						}
					}
				}
				for (int l = 0; l < 2; l++)
				{
					BattleSideEnum battleSideEnum2 = (BattleSideEnum)l;
					int initialSpawnNumber = this.GetActivePhaseForSide(battleSideEnum2).InitialSpawnNumber;
					if (this._battleSideSpawnContexts[l].TroopSpawnActive)
					{
						int reservedTroopsCount = this._battleSideSpawnContexts[l].ReservedTroopsCount;
						if (reservedTroopsCount > 0 && reservedTroopsCount >= initialSpawnNumber)
						{
							bool flag3 = true;
							int num7 = 0;
							foreach (Team team2 in base.Mission.Teams)
							{
								if (team2.Side == battleSideEnum2)
								{
									flag3 = flag3 && this._deploymentPlan.IsPlanMade(team2);
									num7++;
								}
							}
							if (num7 > 0 && flag3)
							{
								this._battleSideSpawnContexts[l].SpawnTroops(initialSpawnNumber, false);
								this.GetActivePhaseForSide(battleSideEnum2).OnInitialTroopsSpawned();
								this._battleSideSpawnContexts[l].OnInitialSpawnOver();
								if (!this._sidesWhereSpawnOccured.Contains(battleSideEnum2))
								{
									this._sidesWhereSpawnOccured.Add(battleSideEnum2);
								}
								Action<BattleSideEnum, int> onInitialTroopsSpawned = this.OnInitialTroopsSpawned;
								if (onInitialTroopsSpawned != null)
								{
									onInitialTroopsSpawned(battleSideEnum2, initialSpawnNumber);
								}
							}
						}
					}
				}
				flag = this.IsDeploymentOver;
				if (flag)
				{
					foreach (BattleSideEnum battleSideEnum3 in this._sidesWhereSpawnOccured)
					{
						this.OnSideDeploymentOver(battleSideEnum3);
					}
				}
			}
			return flag;
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x00081E1C File Offset: 0x0008001C
		private float GetTeamSpawnPathOffsetRange(SpawnPathData initialSpawnPath, BattleSideEnum battleSide, MissionFormationSpawnData[] teamFormationsSpawnData)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 3f;
			bool flag = teamFormationsSpawnData.Sum<MissionFormationSpawnData>((MissionFormationSpawnData data) => data.FootTroopCount) > 0;
			for (int i = 0; i < teamFormationsSpawnData.Count<MissionFormationSpawnData>(); i++)
			{
				MissionFormationSpawnData missionFormationSpawnData = teamFormationsSpawnData[i];
				FormationClass formationClass = (FormationClass)i;
				if (missionFormationSpawnData.NumTroops > 0)
				{
					bool flag2 = DefaultMissionDeploymentPlan.HasSignificantMountedTroops(missionFormationSpawnData.FootTroopCount, missionFormationSpawnData.MountedTroopCount);
					float item = DefaultDeploymentPlan.GetFormationSpawnWidthAndDepth(formationClass, missionFormationSpawnData.NumTroops, flag2, !this.GetSpawnHorses(battleSide)).Item2;
					ref float flankDepthRef = ref DefaultBattleMissionAgentSpawnLogic.GetFlankDepthRef(DefaultFormationDeploymentPlan.GetFormationDefaultFlankAux(formationClass, missionFormationSpawnData.NumTroops, flag, flag2, this.GetSpawnHorses(battleSide)), ref num, ref num2, ref num3);
					if (flankDepthRef > 0.0001f)
					{
						flankDepthRef += num4;
					}
					flankDepthRef += item;
				}
				else if ((formationClass == FormationClass.NumberOfRegularFormations || formationClass == FormationClass.Bodyguard) && num > 0.0001f)
				{
					num += num4;
				}
			}
			return MathF.Max(num, MathF.Max(num2, num3));
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x00081F3C File Offset: 0x0008013C
		private void MakeTeamPlans(Team team, MissionFormationSpawnData[] formationsSpawnData, float spawnPathOffset = 0f, float targetOffset = 0f)
		{
			bool spawnHorses = this.GetSpawnHorses(team.Side);
			if (!this._deploymentPlan.IsPlanMade(team))
			{
				for (int i = 0; i < formationsSpawnData.Length; i++)
				{
					if (formationsSpawnData[i].NumTroops > 0)
					{
						this._deploymentPlan.AddTroops(team, (FormationClass)i, formationsSpawnData[i].FootTroopCount, formationsSpawnData[i].MountedTroopCount, false);
					}
				}
				this._deploymentPlan.MakeDeploymentPlan(team, spawnPathOffset, targetOffset);
			}
			if (!this._deploymentPlan.IsReinforcementPlanMade(team))
			{
				int num = 4;
				int num2 = Math.Max(this._battleSize / (2 * num), 1);
				for (int j = 0; j < num; j++)
				{
					if (((FormationClass)j).IsMounted() && spawnHorses)
					{
						this._deploymentPlan.AddTroops(team, (FormationClass)j, 0, num2, true);
					}
					else
					{
						this._deploymentPlan.AddTroops(team, (FormationClass)j, num2, 0, true);
					}
				}
				this._deploymentPlan.MakeReinforcementDeploymentPlan(team);
			}
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x00082024 File Offset: 0x00080224
		private void CheckReinforcementSpawn()
		{
			int num = 0;
			int num2 = 1;
			MissionBattleSideSpawnContext missionBattleSideSpawnContext = this._battleSideSpawnContexts[num];
			MissionBattleSideSpawnContext missionBattleSideSpawnContext2 = this._battleSideSpawnContexts[num2];
			bool flag = missionBattleSideSpawnContext.HasSpawnableReinforcements && ((float)missionBattleSideSpawnContext.ReinforcementsSpawnedInLastBatch < missionBattleSideSpawnContext.ReinforcementBatchSize || missionBattleSideSpawnContext.ReinforcementBatchPriority >= missionBattleSideSpawnContext2.ReinforcementBatchPriority);
			bool flag2 = missionBattleSideSpawnContext2.HasSpawnableReinforcements && ((float)missionBattleSideSpawnContext2.ReinforcementsSpawnedInLastBatch < missionBattleSideSpawnContext2.ReinforcementBatchSize || missionBattleSideSpawnContext2.ReinforcementBatchPriority >= missionBattleSideSpawnContext.ReinforcementBatchPriority);
			int num3 = 0;
			if (flag && flag2)
			{
				if (missionBattleSideSpawnContext.ReinforcementBatchPriority >= missionBattleSideSpawnContext2.ReinforcementBatchPriority)
				{
					int num4 = missionBattleSideSpawnContext.TryReinforcementSpawn();
					this.DefenderActivePhase.RemainingSpawnNumber -= num4;
					num3 += num4;
					num4 = missionBattleSideSpawnContext2.TryReinforcementSpawn();
					this.AttackerActivePhase.RemainingSpawnNumber -= num4;
					num3 += num4;
				}
				else
				{
					int num4 = missionBattleSideSpawnContext2.TryReinforcementSpawn();
					this.AttackerActivePhase.RemainingSpawnNumber -= num4;
					num3 += num4;
					num4 = missionBattleSideSpawnContext.TryReinforcementSpawn();
					this.DefenderActivePhase.RemainingSpawnNumber -= num4;
					num3 += num4;
				}
			}
			else if (flag)
			{
				int num4 = missionBattleSideSpawnContext.TryReinforcementSpawn();
				this.DefenderActivePhase.RemainingSpawnNumber -= num4;
				num3 += num4;
			}
			else if (flag2)
			{
				int num4 = missionBattleSideSpawnContext2.TryReinforcementSpawn();
				this.AttackerActivePhase.RemainingSpawnNumber -= num4;
				num3 += num4;
			}
			if (num3 > 0)
			{
				for (int i = 0; i < 2; i++)
				{
					this.NotifyReinforcementTroopsSpawned((BattleSideEnum)i, true);
				}
			}
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x000821C8 File Offset: 0x000803C8
		private void NotifyReinforcementTroopsSpawned(BattleSideEnum battleSide, bool checkEmptyReserves = false)
		{
			MissionBattleSideSpawnContext missionBattleSideSpawnContext = this._battleSideSpawnContexts[(int)battleSide];
			int reinforcementsSpawnedInLastBatch = missionBattleSideSpawnContext.ReinforcementsSpawnedInLastBatch;
			if (!missionBattleSideSpawnContext.ReinforcementsNotifiedOnLastBatch && reinforcementsSpawnedInLastBatch > 0 && (!checkEmptyReserves || (checkEmptyReserves && !missionBattleSideSpawnContext.HasReservedTroops)))
			{
				Action<BattleSideEnum, int> onReinforcementsSpawned = this.OnReinforcementsSpawned;
				if (onReinforcementsSpawned != null)
				{
					onReinforcementsSpawned(battleSide, reinforcementsSpawnedInLastBatch);
				}
				missionBattleSideSpawnContext.SetReinforcementsNotifiedOnLastBatch(true);
			}
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x00082219 File Offset: 0x00080419
		private MissionSpawnPhase GetActivePhaseForSide(BattleSideEnum side)
		{
			if (side == BattleSideEnum.Defender)
			{
				return this.DefenderActivePhase;
			}
			if (side != BattleSideEnum.Attacker)
			{
				Debug.FailedAssert("Wrong Side", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\MissionLogics\\DefaultBattleMissionAgentSpawnLogic.cs", "GetActivePhaseForSide", 951);
				return null;
			}
			return this.AttackerActivePhase;
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x0008224C File Offset: 0x0008044C
		[return: TupleElementNames(new string[] { "team", "deployingTroopCount", "formationSpawnData" })]
		private MBReadOnlyList<ValueTuple<Team, int, MissionFormationSpawnData[]>> CollectSortedBattleSideTeamsData([TupleElementNames(new string[] { "team", "formationSpawnData" })] MBList<ValueTuple<Team, MissionFormationSpawnData[]>> teamFormationsSpawnData)
		{
			MBList<ValueTuple<Team, int, MissionFormationSpawnData[]>> mblist = new MBList<ValueTuple<Team, int, MissionFormationSpawnData[]>>();
			foreach (ValueTuple<Team, MissionFormationSpawnData[]> valueTuple in teamFormationsSpawnData)
			{
				int num = 0;
				foreach (MissionFormationSpawnData missionFormationSpawnData in valueTuple.Item2)
				{
					num += missionFormationSpawnData.NumTroops;
				}
				if (num > 0)
				{
					mblist.Add(new ValueTuple<Team, int, MissionFormationSpawnData[]>(valueTuple.Item1, num, valueTuple.Item2));
				}
			}
			mblist.Sort(delegate([TupleElementNames(new string[] { "team", "deployingTroopCount", "formationSpawnData" })] ValueTuple<Team, int, MissionFormationSpawnData[]> t1, [TupleElementNames(new string[] { "team", "deployingTroopCount", "formationSpawnData" })] ValueTuple<Team, int, MissionFormationSpawnData[]> t2)
			{
				bool flag = t1.Item1 == base.Mission.PlayerTeam || t1.Item1 == base.Mission.PlayerEnemyTeam;
				bool flag2 = t2.Item1 == base.Mission.PlayerTeam || t2.Item1 == base.Mission.PlayerEnemyTeam;
				if (flag && flag2)
				{
					Debug.FailedAssert("There can be only one main team in a battle side", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\MissionLogics\\DefaultBattleMissionAgentSpawnLogic.cs", "CollectSortedBattleSideTeamsData", 982);
					return 0;
				}
				if (!flag && !flag2)
				{
					if (t1.Item2 > t2.Item2)
					{
						return -1;
					}
					if (t1.Item2 < t2.Item2)
					{
						return 1;
					}
					return 0;
				}
				else
				{
					if (flag)
					{
						return 1;
					}
					return -1;
				}
			});
			return mblist;
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x000822FC File Offset: 0x000804FC
		public static void ComputeDeploymentBaseOffsets(SpawnPathData sideSpawnPathData, float baseDeploymentOffset, out float deployingSideBaseOffset, out float opposingSideBaseOffset)
		{
			float num = -sideSpawnPathData.PivotOffset;
			float num2 = sideSpawnPathData.PathLength - sideSpawnPathData.PivotOffset;
			if (2f * MathF.Abs(baseDeploymentOffset) <= sideSpawnPathData.PathLength)
			{
				float num3 = -baseDeploymentOffset;
				float num4 = num - MathF.Min(baseDeploymentOffset, num3);
				float num5 = num2 - MathF.Max(baseDeploymentOffset, num3);
				float num6 = 0f;
				if (num6 < num4)
				{
					num6 = num4;
				}
				else if (num6 > num5)
				{
					num6 = num5;
				}
				deployingSideBaseOffset = baseDeploymentOffset + num6;
				opposingSideBaseOffset = num3 + num6;
			}
			else if (baseDeploymentOffset <= 0f)
			{
				deployingSideBaseOffset = num;
				opposingSideBaseOffset = num2;
			}
			else
			{
				deployingSideBaseOffset = num2;
				opposingSideBaseOffset = num;
			}
			sideSpawnPathData.ClampPathOffset(ref deployingSideBaseOffset);
			sideSpawnPathData.ClampPathOffset(ref opposingSideBaseOffset);
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x000823A0 File Offset: 0x000805A0
		public static void ComputeTeamDeploymentOffsets(SpawnPathData spawnPathData, float deploymentBaseOffset, float interTeamGapOffset, float[] teamOffsetRanges, out float[] teamDeployOffsets)
		{
			int num = teamOffsetRanges.Length;
			teamDeployOffsets = new float[num];
			if (!DefaultBattleMissionAgentSpawnLogic.TryPlaceTeamsOneSide(spawnPathData, deploymentBaseOffset, teamOffsetRanges, interTeamGapOffset, true, ref teamDeployOffsets))
			{
				float num2 = -spawnPathData.PathLength;
				spawnPathData.ClampPathOffset(ref num2);
				DefaultBattleMissionAgentSpawnLogic.TryPlaceTeamsOneSide(spawnPathData, num2, teamOffsetRanges, interTeamGapOffset, false, ref teamDeployOffsets);
			}
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x000823E8 File Offset: 0x000805E8
		private static bool TryPlaceTeamsOneSide(SpawnPathData spawnPathData, float deploymentBaseOffset, float[] teamOffsetRanges, float interTeamGapOffset, bool searchBackwardFromBase, ref float[] teamDeployOffsets)
		{
			int num = teamOffsetRanges.Length;
			for (int i = 0; i < num; i++)
			{
			}
			if (teamDeployOffsets == null)
			{
				teamDeployOffsets = new float[num];
			}
			else
			{
				for (int j = 0; j < teamDeployOffsets.Length; j++)
				{
					teamDeployOffsets[j] = 0f;
				}
			}
			int freeSegmentCount = spawnPathData.FreeSegmentCount;
			if (searchBackwardFromBase)
			{
				float num2 = deploymentBaseOffset;
				for (int k = 0; k < num; k++)
				{
					float num3 = teamOffsetRanges[k];
					if (num3 == 0f)
					{
						teamDeployOffsets[k] = num2;
					}
					else
					{
						bool flag = false;
						for (int l = freeSegmentCount - 1; l >= 0; l--)
						{
							ValueTuple<float, float> freeSegment = spawnPathData.GetFreeSegment(l);
							float item = freeSegment.Item1;
							float item2 = freeSegment.Item2;
							if (item < num2)
							{
								float num4 = item;
								float num5 = item2;
								if (num5 > num2)
								{
									num5 = num2;
								}
								if (num5 - num4 >= num3)
								{
									float num6 = num5;
									float num7 = num6 - num3;
									teamDeployOffsets[k] = num6;
									num2 = num7 - interTeamGapOffset;
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							return false;
						}
					}
				}
				return true;
			}
			float num8 = deploymentBaseOffset;
			for (int m = num - 1; m >= 0; m--)
			{
				float num9 = teamOffsetRanges[m];
				if (num9 == 0f)
				{
					teamDeployOffsets[m] = num8;
				}
				else
				{
					bool flag2 = false;
					for (int n = 0; n < freeSegmentCount; n++)
					{
						ValueTuple<float, float> freeSegment2 = spawnPathData.GetFreeSegment(n);
						float item3 = freeSegment2.Item1;
						float item4 = freeSegment2.Item2;
						if (item4 > num8)
						{
							float num10 = item3;
							if (num10 < num8)
							{
								num10 = num8;
							}
							if (item4 - num10 >= num9)
							{
								float num11 = num10 + num9;
								teamDeployOffsets[m] = num11;
								num8 = num11 + interTeamGapOffset;
								flag2 = true;
								break;
							}
						}
					}
					if (!flag2)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x0008257A File Offset: 0x0008077A
		private static ref float GetFlankDepthRef(FormationDeploymentFlank flank, ref float centerFlanksDepth, ref float leftFlanksDepth, ref float rightFlanksDepth)
		{
			if (flank == FormationDeploymentFlank.Left)
			{
				return ref leftFlanksDepth;
			}
			if (flank == FormationDeploymentFlank.Right)
			{
				return ref rightFlanksDepth;
			}
			return ref centerFlanksDepth;
		}

		// Token: 0x04000DDA RID: 3546
		private const float InterTeamDeploymentGap = 20f;

		// Token: 0x04000DDB RID: 3547
		private static int _maxNumberOfAgentsForMissionCache;

		// Token: 0x04000DDE RID: 3550
		private readonly DefaultBattleMissionAgentSpawnLogic.OnPhaseChangedDelegate[] _onPhaseChanged = new DefaultBattleMissionAgentSpawnLogic.OnPhaseChangedDelegate[2];

		// Token: 0x04000DDF RID: 3551
		private readonly List<MissionSpawnPhase>[] _phases;

		// Token: 0x04000DE0 RID: 3552
		private readonly int[] _numberOfTroopsInTotal;

		// Token: 0x04000DE1 RID: 3553
		private readonly int _battleSize;

		// Token: 0x04000DE2 RID: 3554
		private readonly MissionBattleSideSpawnContext _playerBattleSideSpawnContext;

		// Token: 0x04000DE3 RID: 3555
		private BasicMissionTimer _globalReinforcementSpawnTimer;

		// Token: 0x04000DE4 RID: 3556
		private bool _reinforcementSpawnEnabled = true;

		// Token: 0x04000DE5 RID: 3557
		private bool _spawningReinforcements;

		// Token: 0x04000DE6 RID: 3558
		private ICustomReinforcementSpawnTimer _customReinforcementSpawnTimer;

		// Token: 0x04000DE7 RID: 3559
		private float _globalReinforcementInterval;

		// Token: 0x04000DE8 RID: 3560
		private MissionSpawnSettings _spawnSettings;

		// Token: 0x04000DE9 RID: 3561
		private readonly MissionBattleSideSpawnContext[] _battleSideSpawnContexts;

		// Token: 0x04000DEA RID: 3562
		private BannerBearerLogic _bannerBearerLogic;

		// Token: 0x04000DEB RID: 3563
		private DefaultMissionDeploymentPlan _deploymentPlan;

		// Token: 0x04000DEC RID: 3564
		private List<BattleSideEnum> _sidesWhereSpawnOccured = new List<BattleSideEnum>();

		// Token: 0x0200055C RID: 1372
		// (Invoke) Token: 0x06003DAA RID: 15786
		public delegate void OnPhaseChangedDelegate();
	}
}
