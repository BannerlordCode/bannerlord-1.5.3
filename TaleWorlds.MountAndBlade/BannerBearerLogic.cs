using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200027A RID: 634
	public class BannerBearerLogic : MissionLogic
	{
		// Token: 0x14000036 RID: 54
		// (add) Token: 0x0600239E RID: 9118 RVA: 0x0007EBA4 File Offset: 0x0007CDA4
		// (remove) Token: 0x0600239F RID: 9119 RVA: 0x0007EBDC File Offset: 0x0007CDDC
		public event Action<Formation> OnBannerBearersUpdated;

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x060023A0 RID: 9120 RVA: 0x0007EC14 File Offset: 0x0007CE14
		// (remove) Token: 0x060023A1 RID: 9121 RVA: 0x0007EC4C File Offset: 0x0007CE4C
		public event Action<Agent, bool> OnBannerBearerAgentUpdated;

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x060023A3 RID: 9123 RVA: 0x0007ECB5 File Offset: 0x0007CEB5
		// (set) Token: 0x060023A4 RID: 9124 RVA: 0x0007ECBD File Offset: 0x0007CEBD
		public IMissionAgentSpawnLogic AgentSpawnLogic { get; private set; }

		// Token: 0x060023A5 RID: 9125 RVA: 0x0007ECC8 File Offset: 0x0007CEC8
		public bool IsFormationBanner(Formation formation, SpawnedItemEntity spawnedItem)
		{
			if (!BannerBearerLogic.IsBannerItem(spawnedItem.WeaponCopy.Item))
			{
				return false;
			}
			BannerBearerLogic.FormationBannerController formationControllerFromBannerEntity = this.GetFormationControllerFromBannerEntity(spawnedItem.GameEntity);
			return formationControllerFromBannerEntity != null && formationControllerFromBannerEntity.Formation == formation;
		}

		// Token: 0x060023A6 RID: 9126 RVA: 0x0007ED08 File Offset: 0x0007CF08
		public bool HasBannerOnGround(Formation formation)
		{
			BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(formation);
			return formationControllerFromFormation != null && formationControllerFromFormation.HasBannerOnGround();
		}

		// Token: 0x060023A7 RID: 9127 RVA: 0x0007ED28 File Offset: 0x0007CF28
		public BannerComponent GetActiveBanner(Formation formation)
		{
			BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(formation);
			if (formationControllerFromFormation == null)
			{
				return null;
			}
			if (!formationControllerFromFormation.HasActiveBannerBearers())
			{
				return null;
			}
			return formationControllerFromFormation.BannerItem.BannerComponent;
		}

		// Token: 0x060023A8 RID: 9128 RVA: 0x0007ED58 File Offset: 0x0007CF58
		public List<Agent> GetFormationBannerBearers(Formation formation)
		{
			BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(formation);
			if (formationControllerFromFormation != null)
			{
				return formationControllerFromFormation.BannerBearers;
			}
			return new List<Agent>();
		}

		// Token: 0x060023A9 RID: 9129 RVA: 0x0007ED7C File Offset: 0x0007CF7C
		public ItemObject GetFormationBanner(Formation formation)
		{
			ItemObject itemObject = null;
			BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(formation);
			if (formationControllerFromFormation != null)
			{
				itemObject = formationControllerFromFormation.BannerItem;
			}
			return itemObject;
		}

		// Token: 0x060023AA RID: 9130 RVA: 0x0007EDA0 File Offset: 0x0007CFA0
		public bool IsBannerSearchingAgent(Agent agent)
		{
			if (agent.Formation != null)
			{
				BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(agent.Formation);
				if (formationControllerFromFormation != null)
				{
					return formationControllerFromFormation.IsBannerSearchingAgent(agent);
				}
			}
			return false;
		}

		// Token: 0x060023AB RID: 9131 RVA: 0x0007EDD0 File Offset: 0x0007CFD0
		public int GetMissingBannerCount(Formation formation)
		{
			BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(formation);
			if (formationControllerFromFormation == null || formationControllerFromFormation.BannerItem == null)
			{
				return 0;
			}
			int num = MissionGameModels.Current.BattleBannerBearersModel.GetDesiredNumberOfBannerBearersForFormation(formation) - formationControllerFromFormation.NumberOfBanners;
			if (num <= 0)
			{
				return 0;
			}
			return num;
		}

		// Token: 0x060023AC RID: 9132 RVA: 0x0007EE14 File Offset: 0x0007D014
		public Formation GetFormationFromBanner(SpawnedItemEntity spawnedItem)
		{
			WeakGameEntity weakGameEntity = spawnedItem.GameEntity;
			weakGameEntity = ((!weakGameEntity.IsValid) ? spawnedItem.GameEntityWithWorldPosition.GameEntity : weakGameEntity);
			BannerBearerLogic.FormationBannerController formationControllerFromBannerEntity = this.GetFormationControllerFromBannerEntity(weakGameEntity);
			if (formationControllerFromBannerEntity == null)
			{
				return null;
			}
			return formationControllerFromBannerEntity.Formation;
		}

		// Token: 0x060023AD RID: 9133 RVA: 0x0007EE54 File Offset: 0x0007D054
		public void SetFormationBanner(Formation formation, ItemObject newBanner)
		{
			if (newBanner != null)
			{
				BannerBearerLogic.IsBannerItem(newBanner);
			}
			BannerBearerLogic.FormationBannerController formationBannerController = this.GetFormationControllerFromFormation(formation);
			if (formationBannerController != null)
			{
				if (formationBannerController.BannerItem != newBanner)
				{
					formationBannerController.SetBannerItem(newBanner);
				}
			}
			else
			{
				formationBannerController = new BannerBearerLogic.FormationBannerController(formation, newBanner, this, base.Mission);
				this._formationBannerData.Add(formation, formationBannerController);
			}
			formationBannerController.UpdateBannerBearersForDeployment();
		}

		// Token: 0x060023AE RID: 9134 RVA: 0x0007EEB0 File Offset: 0x0007D0B0
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			MissionGameModels.Current.BattleBannerBearersModel.InitializeModel(this);
			this.AgentSpawnLogic = base.Mission.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>();
			base.Mission.OnItemPickUp += this.OnItemPickup;
			base.Mission.OnItemDrop += this.OnItemDrop;
			this._bannerSearcherUpdateTimer = new BasicMissionTimer();
			this._initialSpawnEquipments.Clear();
		}

		// Token: 0x060023AF RID: 9135 RVA: 0x0007EF28 File Offset: 0x0007D128
		protected override void OnEndMission()
		{
			base.OnEndMission();
			MissionGameModels.Current.BattleBannerBearersModel.FinalizeModel();
			base.Mission.OnItemPickUp -= this.OnItemPickup;
			base.Mission.OnItemDrop -= this.OnItemDrop;
			this.AgentSpawnLogic = null;
			this._isMissionEnded = true;
		}

		// Token: 0x060023B0 RID: 9136 RVA: 0x0007EF86 File Offset: 0x0007D186
		public override void OnDeploymentFinished()
		{
			this._initialSpawnEquipments.Clear();
			this._isMissionEnded = false;
		}

		// Token: 0x060023B1 RID: 9137 RVA: 0x0007EF9C File Offset: 0x0007D19C
		public override void OnMissionTick(float dt)
		{
			if (this._bannerSearcherUpdateTimer.ElapsedTime >= 3f)
			{
				foreach (BannerBearerLogic.FormationBannerController formationBannerController in this._formationBannerData.Values)
				{
					formationBannerController.UpdateBannerSearchers();
				}
				this._bannerSearcherUpdateTimer.Reset();
			}
			if (base.Mission.Mode == MissionMode.Deployment && !this._playerFormationsRequiringUpdate.IsEmpty<BannerBearerLogic.FormationBannerController>())
			{
				foreach (BannerBearerLogic.FormationBannerController formationBannerController2 in this._playerFormationsRequiringUpdate)
				{
					formationBannerController2.UpdateBannerBearersForDeployment();
				}
				this._playerFormationsRequiringUpdate.Clear();
			}
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x0007F074 File Offset: 0x0007D274
		public void OnItemPickup(Agent agent, SpawnedItemEntity spawnedItem)
		{
			if (!BannerBearerLogic.IsBannerItem(spawnedItem.WeaponCopy.Item))
			{
				return;
			}
			WeakGameEntity gameEntity = spawnedItem.GameEntity;
			BannerBearerLogic.FormationBannerController formationControllerFromBannerEntity = this.GetFormationControllerFromBannerEntity(gameEntity);
			if (formationControllerFromBannerEntity != null)
			{
				formationControllerFromBannerEntity.OnBannerEntityPickedUp(GameEntity.CreateFromWeakEntity(gameEntity), agent);
				formationControllerFromBannerEntity.UpdateAgentStats(false);
			}
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x0007F0C0 File Offset: 0x0007D2C0
		public void OnItemDrop(Agent agent, SpawnedItemEntity spawnedItem)
		{
			if (!BannerBearerLogic.IsBannerItem(spawnedItem.WeaponCopy.Item))
			{
				return;
			}
			BannerBearerLogic.FormationBannerController formationControllerFromBannerEntity = this.GetFormationControllerFromBannerEntity(spawnedItem.GameEntity);
			if (formationControllerFromBannerEntity != null)
			{
				formationControllerFromBannerEntity.OnBannerEntityDropped(GameEntity.CreateFromWeakEntity(spawnedItem.GameEntity));
				formationControllerFromBannerEntity.UpdateAgentStats(false);
			}
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x0007F10B File Offset: 0x0007D30B
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (affectedAgent.Banner != null && agentState == AgentState.Routed)
			{
				this.RemoveBannerOfAgent(affectedAgent);
			}
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x0007F120 File Offset: 0x0007D320
		public override void OnAgentPanicked(Agent affectedAgent)
		{
			if (affectedAgent.Banner != null)
			{
				affectedAgent.Mission.AddTickAction(Mission.MissionTickAction.DropItem, affectedAgent, 4, 0);
			}
		}

		// Token: 0x060023B6 RID: 9142 RVA: 0x0007F13C File Offset: 0x0007D33C
		public void UpdateAgent(Agent agent, bool willBecomeBannerBearer)
		{
			if (willBecomeBannerBearer)
			{
				Formation formation = agent.Formation;
				BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(formation);
				ItemObject bannerItem = formationControllerFromFormation.BannerItem;
				if (agent.Banner != null)
				{
					this.RemoveBannerOfAgent(agent);
				}
				Equipment equipment = this.CreateBannerEquipmentForAgent(agent, bannerItem);
				agent.UpdateSpawnEquipmentAndRefreshVisuals(equipment);
				GameEntity gameEntity = GameEntity.CreateFromWeakEntity(agent.GetWeaponEntityFromEquipmentSlot(EquipmentIndex.ExtraWeaponSlot));
				this.AddBannerEntity(formationControllerFromFormation, gameEntity);
				formationControllerFromFormation.OnBannerEntityPickedUp(gameEntity, agent);
			}
			else if (agent.Banner != null)
			{
				this.RemoveBannerOfAgent(agent);
				agent.UpdateSpawnEquipmentAndRefreshVisuals(this._initialSpawnEquipments[agent]);
			}
			agent.ForceUpdateCachedAndFormationValues(false, false);
			agent.SetIsAIPaused(true);
			Action<Agent, bool> onBannerBearerAgentUpdated = this.OnBannerBearerAgentUpdated;
			if (onBannerBearerAgentUpdated == null)
			{
				return;
			}
			onBannerBearerAgentUpdated(agent, willBecomeBannerBearer);
		}

		// Token: 0x060023B7 RID: 9143 RVA: 0x0007F1E8 File Offset: 0x0007D3E8
		public Agent SpawnBannerBearer(IAgentOriginBase troopOrigin, bool isPlayerSide, Formation formation, bool spawnWithHorse, bool isReinforcement, int formationTroopCount, int formationTroopIndex, bool isAlarmed, bool wieldInitialWeapons, Vec3? initialPosition, Vec2? initialDirection, string specialActionSetSuffix = null, bool useTroopClassForSpawn = false)
		{
			BannerBearerLogic.FormationBannerController formationControllerFromFormation = this.GetFormationControllerFromFormation(formation);
			ItemObject bannerItem = formationControllerFromFormation.BannerItem;
			Agent agent = base.Mission.SpawnTroop(troopOrigin, isPlayerSide, true, spawnWithHorse, isReinforcement, formationTroopCount, formationTroopIndex, isAlarmed, wieldInitialWeapons, initialPosition, initialDirection, specialActionSetSuffix, bannerItem, formationControllerFromFormation.Formation.FormationIndex, useTroopClassForSpawn);
			agent.ForceUpdateCachedAndFormationValues(false, false);
			GameEntity gameEntity = GameEntity.CreateFromWeakEntity(agent.GetWeaponEntityFromEquipmentSlot(EquipmentIndex.ExtraWeaponSlot));
			this.AddBannerEntity(formationControllerFromFormation, gameEntity);
			formationControllerFromFormation.OnBannerEntityPickedUp(gameEntity, agent);
			return agent;
		}

		// Token: 0x060023B8 RID: 9144 RVA: 0x0007F259 File Offset: 0x0007D459
		public static bool IsBannerItem(ItemObject item)
		{
			return item != null && item.IsBannerItem && item.BannerComponent != null;
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x0007F271 File Offset: 0x0007D471
		private void AddBannerEntity(BannerBearerLogic.FormationBannerController formationBannerController, GameEntity bannerEntity)
		{
			this._bannerToFormationMap.Add(bannerEntity.Pointer, formationBannerController);
			formationBannerController.AddBannerEntity(bannerEntity);
		}

		// Token: 0x060023BA RID: 9146 RVA: 0x0007F28C File Offset: 0x0007D48C
		private void RemoveBannerEntity(BannerBearerLogic.FormationBannerController formationBannerController, WeakGameEntity bannerEntity)
		{
			this._bannerToFormationMap.Remove(bannerEntity.Pointer);
			formationBannerController.RemoveBannerEntity(bannerEntity);
		}

		// Token: 0x060023BB RID: 9147 RVA: 0x0007F2A8 File Offset: 0x0007D4A8
		private BannerBearerLogic.FormationBannerController GetFormationControllerFromFormation(Formation formation)
		{
			BannerBearerLogic.FormationBannerController formationBannerController;
			if (!this._formationBannerData.TryGetValue(formation, out formationBannerController))
			{
				return null;
			}
			return formationBannerController;
		}

		// Token: 0x060023BC RID: 9148 RVA: 0x0007F2C8 File Offset: 0x0007D4C8
		private BannerBearerLogic.FormationBannerController GetFormationControllerFromBannerEntity(WeakGameEntity bannerEntity)
		{
			BannerBearerLogic.FormationBannerController formationBannerController;
			if (this._bannerToFormationMap.TryGetValue(bannerEntity.Pointer, out formationBannerController))
			{
				return formationBannerController;
			}
			return null;
		}

		// Token: 0x060023BD RID: 9149 RVA: 0x0007F2F0 File Offset: 0x0007D4F0
		private Equipment CreateBannerEquipmentForAgent(Agent agent, ItemObject bannerItem)
		{
			Equipment spawnEquipment = agent.SpawnEquipment;
			if (!this._initialSpawnEquipments.ContainsKey(agent))
			{
				this._initialSpawnEquipments[agent] = spawnEquipment;
			}
			Equipment equipment = new Equipment(spawnEquipment);
			ItemObject bannerBearerReplacementWeapon = MissionGameModels.Current.BattleBannerBearersModel.GetBannerBearerReplacementWeapon(agent.Character);
			equipment[EquipmentIndex.WeaponItemBeginSlot] = new EquipmentElement(bannerBearerReplacementWeapon, null, null, false);
			for (int i = 1; i < 4; i++)
			{
				equipment[i] = default(EquipmentElement);
			}
			equipment[EquipmentIndex.ExtraWeaponSlot] = new EquipmentElement(bannerItem, null, null, false);
			return equipment;
		}

		// Token: 0x060023BE RID: 9150 RVA: 0x0007F37C File Offset: 0x0007D57C
		private void RemoveBannerOfAgent(Agent agent)
		{
			WeakGameEntity weaponEntityFromEquipmentSlot = agent.GetWeaponEntityFromEquipmentSlot(EquipmentIndex.ExtraWeaponSlot);
			BannerBearerLogic.FormationBannerController formationControllerFromBannerEntity = this.GetFormationControllerFromBannerEntity(weaponEntityFromEquipmentSlot);
			if (formationControllerFromBannerEntity != null)
			{
				this.RemoveBannerEntity(formationControllerFromBannerEntity, weaponEntityFromEquipmentSlot);
				formationControllerFromBannerEntity.UpdateAgentStats(false);
			}
		}

		// Token: 0x04000DAB RID: 3499
		public const float DefaultBannerBearerAgentDefensiveness = 1f;

		// Token: 0x04000DAC RID: 3500
		public const float BannerSearcherUpdatePeriod = 3f;

		// Token: 0x04000DB0 RID: 3504
		private readonly Dictionary<UIntPtr, BannerBearerLogic.FormationBannerController> _bannerToFormationMap = new Dictionary<UIntPtr, BannerBearerLogic.FormationBannerController>();

		// Token: 0x04000DB1 RID: 3505
		private readonly Dictionary<Formation, BannerBearerLogic.FormationBannerController> _formationBannerData = new Dictionary<Formation, BannerBearerLogic.FormationBannerController>();

		// Token: 0x04000DB2 RID: 3506
		private readonly Dictionary<Agent, Equipment> _initialSpawnEquipments = new Dictionary<Agent, Equipment>();

		// Token: 0x04000DB3 RID: 3507
		private BasicMissionTimer _bannerSearcherUpdateTimer;

		// Token: 0x04000DB4 RID: 3508
		private readonly List<BannerBearerLogic.FormationBannerController> _playerFormationsRequiringUpdate = new List<BannerBearerLogic.FormationBannerController>();

		// Token: 0x04000DB5 RID: 3509
		private bool _isMissionEnded;

		// Token: 0x02000559 RID: 1369
		private class FormationBannerController
		{
			// Token: 0x17000A7D RID: 2685
			// (get) Token: 0x06003D81 RID: 15745 RVA: 0x000F5CC3 File Offset: 0x000F3EC3
			// (set) Token: 0x06003D82 RID: 15746 RVA: 0x000F5CCB File Offset: 0x000F3ECB
			public Formation Formation { get; private set; }

			// Token: 0x17000A7E RID: 2686
			// (get) Token: 0x06003D83 RID: 15747 RVA: 0x000F5CD4 File Offset: 0x000F3ED4
			// (set) Token: 0x06003D84 RID: 15748 RVA: 0x000F5CDC File Offset: 0x000F3EDC
			public ItemObject BannerItem { get; private set; }

			// Token: 0x17000A7F RID: 2687
			// (get) Token: 0x06003D85 RID: 15749 RVA: 0x000F5CE5 File Offset: 0x000F3EE5
			public bool HasBanner
			{
				get
				{
					return this.BannerItem != null;
				}
			}

			// Token: 0x17000A80 RID: 2688
			// (get) Token: 0x06003D86 RID: 15750 RVA: 0x000F5CF0 File Offset: 0x000F3EF0
			public List<Agent> BannerBearers
			{
				get
				{
					return (from instance in this._bannerInstances.Values
						where instance.IsOnAgent
						select instance.BannerBearer).ToList<Agent>();
				}
			}

			// Token: 0x17000A81 RID: 2689
			// (get) Token: 0x06003D87 RID: 15751 RVA: 0x000F5D58 File Offset: 0x000F3F58
			public List<GameEntity> BannersOnGround
			{
				get
				{
					return (from instance in this._bannerInstances.Values
						where instance.IsOnGround
						select instance.Entity).ToList<GameEntity>();
				}
			}

			// Token: 0x17000A82 RID: 2690
			// (get) Token: 0x06003D88 RID: 15752 RVA: 0x000F5DBD File Offset: 0x000F3FBD
			public int NumberOfBannerBearers
			{
				get
				{
					return this._bannerInstances.Values.Count<BannerBearerLogic.FormationBannerController.BannerInstance>((BannerBearerLogic.FormationBannerController.BannerInstance instance) => instance.IsOnAgent);
				}
			}

			// Token: 0x17000A83 RID: 2691
			// (get) Token: 0x06003D89 RID: 15753 RVA: 0x000F5DEE File Offset: 0x000F3FEE
			public int NumberOfBanners
			{
				get
				{
					return this._bannerInstances.Count;
				}
			}

			// Token: 0x17000A84 RID: 2692
			// (get) Token: 0x06003D8A RID: 15754 RVA: 0x000F5DFB File Offset: 0x000F3FFB
			public static float BannerSearchDistance
			{
				get
				{
					return 9f;
				}
			}

			// Token: 0x06003D8B RID: 15755 RVA: 0x000F5E04 File Offset: 0x000F4004
			public FormationBannerController(Formation formation, ItemObject bannerItem, BannerBearerLogic bannerLogic, Mission mission)
			{
				this.Formation = formation;
				this.Formation.OnUnitAdded += this.OnAgentAdded;
				this.Formation.OnUnitRemoved += this.OnAgentRemoved;
				this.Formation.OnBeforeMovementOrderApplied += this.OnBeforeFormationMovementOrderApplied;
				this.Formation.OnAfterArrangementOrderApplied += this.OnAfterArrangementOrderApplied;
				this._bannerInstances = new Dictionary<UIntPtr, BannerBearerLogic.FormationBannerController.BannerInstance>();
				this._bannerSearchers = new Dictionary<Agent, ValueTuple<GameEntity, float>>();
				this._requiresAgentStatUpdate = false;
				this._lastActiveBannerBearerCount = 0;
				this._bannerLogic = bannerLogic;
				this._mission = mission;
				this.SetBannerItem(bannerItem);
			}

			// Token: 0x06003D8C RID: 15756 RVA: 0x000F5EBF File Offset: 0x000F40BF
			public void SetBannerItem(ItemObject bannerItem)
			{
				if (bannerItem != null)
				{
					BannerBearerLogic.IsBannerItem(bannerItem);
				}
				this.BannerItem = bannerItem;
			}

			// Token: 0x06003D8D RID: 15757 RVA: 0x000F5ED5 File Offset: 0x000F40D5
			public bool HasBannerEntity(GameEntity bannerEntity)
			{
				return bannerEntity != null && this._bannerInstances.Keys.Contains(bannerEntity.Pointer);
			}

			// Token: 0x06003D8E RID: 15758 RVA: 0x000F5EF8 File Offset: 0x000F40F8
			public bool HasBannerOnGround()
			{
				if (this.HasBanner)
				{
					return this._bannerInstances.Any<KeyValuePair<UIntPtr, BannerBearerLogic.FormationBannerController.BannerInstance>>((KeyValuePair<UIntPtr, BannerBearerLogic.FormationBannerController.BannerInstance> instance) => instance.Value.IsOnGround);
				}
				return false;
			}

			// Token: 0x06003D8F RID: 15759 RVA: 0x000F5F2E File Offset: 0x000F412E
			public bool HasActiveBannerBearers()
			{
				return this.GetNumberOfActiveBannerBearers() > 0;
			}

			// Token: 0x06003D90 RID: 15760 RVA: 0x000F5F39 File Offset: 0x000F4139
			public bool IsBannerSearchingAgent(Agent agent)
			{
				return this._bannerSearchers.Keys.Contains(agent);
			}

			// Token: 0x06003D91 RID: 15761 RVA: 0x000F5F4C File Offset: 0x000F414C
			public int GetNumberOfActiveBannerBearers()
			{
				int num = 0;
				if (this.HasBanner)
				{
					BattleBannerBearersModel bannerBearersModel = MissionGameModels.Current.BattleBannerBearersModel;
					num = this._bannerInstances.Values.Count<BannerBearerLogic.FormationBannerController.BannerInstance>((BannerBearerLogic.FormationBannerController.BannerInstance instance) => instance.IsOnAgent && bannerBearersModel.CanBannerBearerProvideEffectToFormation(instance.BannerBearer, this.Formation));
				}
				return num;
			}

			// Token: 0x06003D92 RID: 15762 RVA: 0x000F5F9E File Offset: 0x000F419E
			public void UpdateAgentStats(bool forceUpdate = false)
			{
				if (forceUpdate || this._requiresAgentStatUpdate)
				{
					this.Formation.ApplyActionOnEachUnit(delegate(Agent agent)
					{
						agent.UpdateAgentProperties();
						Agent mountAgent = agent.MountAgent;
						if (mountAgent != null)
						{
							mountAgent.UpdateAgentProperties();
						}
					}, null);
					this._requiresAgentStatUpdate = false;
				}
			}

			// Token: 0x06003D93 RID: 15763 RVA: 0x000F5FE0 File Offset: 0x000F41E0
			private unsafe void RepositionFormation()
			{
				this.Formation.SetMovementOrder(*this.Formation.GetReadonlyMovementOrderReference());
				this.Formation.ApplyActionOnEachUnit(delegate(Agent agent)
				{
					agent.ForceUpdateCachedAndFormationValues(true, false);
				}, null);
				this.Formation.SetHasPendingUnitPositions(false);
			}

			// Token: 0x06003D94 RID: 15764 RVA: 0x000F6040 File Offset: 0x000F4240
			public void UpdateBannerSearchers()
			{
				List<GameEntity> bannersOnGround = this.BannersOnGround;
				if (!this._bannerSearchers.IsEmpty<KeyValuePair<Agent, ValueTuple<GameEntity, float>>>())
				{
					List<Agent> list = new List<Agent>();
					using (Dictionary<Agent, ValueTuple<GameEntity, float>>.Enumerator enumerator = this._bannerSearchers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<Agent, ValueTuple<GameEntity, float>> searcherTuple = enumerator.Current;
							Agent key = searcherTuple.Key;
							if (key.IsActive())
							{
								if (!bannersOnGround.Any<GameEntity>((GameEntity bannerEntity) => bannerEntity.Pointer == searcherTuple.Value.Item1.Pointer))
								{
									list.Add(key);
								}
							}
							else
							{
								list.Add(key);
							}
						}
					}
					foreach (Agent agent in list)
					{
						this.RemoveBannerSearcher(agent);
					}
				}
				using (List<GameEntity>.Enumerator enumerator3 = bannersOnGround.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						GameEntity banner = enumerator3.Current;
						bool flag = false;
						if (this._bannerSearchers.IsEmpty<KeyValuePair<Agent, ValueTuple<GameEntity, float>>>())
						{
							flag = true;
						}
						else
						{
							KeyValuePair<Agent, ValueTuple<GameEntity, float>> keyValuePair = this._bannerSearchers.FirstOrDefault<KeyValuePair<Agent, ValueTuple<GameEntity, float>>>(([TupleElementNames(new string[] { "bannerEntity", "lastDistance" })] KeyValuePair<Agent, ValueTuple<GameEntity, float>> tuple) => tuple.Value.Item1.Pointer == banner.Pointer);
							if (keyValuePair.Key == null)
							{
								flag = true;
							}
							else
							{
								Agent key2 = keyValuePair.Key;
								if (key2.IsActive())
								{
									GameEntity item = keyValuePair.Value.Item1;
									float item2 = keyValuePair.Value.Item2;
									float num = key2.Position.AsVec2.Distance(item.GlobalPosition.AsVec2);
									if (num <= item2 && num < BannerBearerLogic.FormationBannerController.BannerSearchDistance)
									{
										this._bannerSearchers[key2] = new ValueTuple<GameEntity, float>(item, num);
									}
									else
									{
										this.RemoveBannerSearcher(key2);
										flag = true;
									}
								}
								else
								{
									this.RemoveBannerSearcher(key2);
									flag = true;
								}
							}
						}
						if (flag)
						{
							float num2;
							Agent agent2 = this.FindBestSearcherForBanner(banner, out num2);
							if (agent2 != null)
							{
								this.AddBannerSearcher(agent2, banner, num2);
							}
						}
					}
				}
			}

			// Token: 0x06003D95 RID: 15765 RVA: 0x000F62A4 File Offset: 0x000F44A4
			public void UpdateBannerBearersForDeployment()
			{
				List<Agent> bannerBearers = this.BannerBearers;
				List<ValueTuple<Agent, bool>> list = new List<ValueTuple<Agent, bool>>();
				int num = 0;
				BattleBannerBearersModel battleBannerBearersModel = MissionGameModels.Current.BattleBannerBearersModel;
				if (battleBannerBearersModel.CanFormationDeployBannerBearers(this.Formation))
				{
					num = battleBannerBearersModel.GetDesiredNumberOfBannerBearersForFormation(this.Formation);
					using (List<Agent>.Enumerator enumerator = bannerBearers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Agent agent = enumerator.Current;
							if (num > 0 && agent.Formation == this.Formation)
							{
								num--;
							}
							else
							{
								list.Add(new ValueTuple<Agent, bool>(agent, false));
							}
						}
						goto IL_00C2;
					}
				}
				foreach (Agent agent2 in bannerBearers)
				{
					list.Add(new ValueTuple<Agent, bool>(agent2, false));
				}
				IL_00C2:
				if (num > 0)
				{
					List<Agent> list2 = this.FindBannerBearableAgents(num);
					int num2 = 0;
					while (num2 < list2.Count && num > 0)
					{
						Agent agent3 = list2[num2];
						list.Add(new ValueTuple<Agent, bool>(agent3, true));
						num--;
						num2++;
					}
				}
				if (!list.IsEmpty<ValueTuple<Agent, bool>>())
				{
					BattleSideEnum side = this.Formation.Team.Side;
					this._bannerLogic.AgentSpawnLogic.GetSpawnHorses(side);
					BattleSideEnum side2 = this._mission.PlayerTeam.Side;
					foreach (ValueTuple<Agent, bool> valueTuple in list)
					{
						this._bannerLogic.UpdateAgent(valueTuple.Item1, valueTuple.Item2);
					}
				}
				this.UpdateAgentStats(false);
				this.RepositionFormation();
				Action<Formation> onBannerBearersUpdated = this._bannerLogic.OnBannerBearersUpdated;
				if (onBannerBearersUpdated == null)
				{
					return;
				}
				onBannerBearersUpdated(this.Formation);
			}

			// Token: 0x06003D96 RID: 15766 RVA: 0x000F648C File Offset: 0x000F468C
			public void AddBannerEntity(GameEntity entity)
			{
				if (!this._bannerInstances.ContainsKey(entity.Pointer))
				{
					this._bannerInstances.Add(entity.Pointer, new BannerBearerLogic.FormationBannerController.BannerInstance(null, entity, BannerBearerLogic.FormationBannerController.BannerState.Initialized));
				}
			}

			// Token: 0x06003D97 RID: 15767 RVA: 0x000F64BA File Offset: 0x000F46BA
			public void RemoveBannerEntity(WeakGameEntity entity)
			{
				this._bannerInstances.Remove(entity.Pointer);
				this.UpdateBannerSearchers();
				this.CheckRequiresAgentStatUpdate();
			}

			// Token: 0x06003D98 RID: 15768 RVA: 0x000F64DB File Offset: 0x000F46DB
			public void OnBannerEntityPickedUp(GameEntity entity, Agent agent)
			{
				this._bannerInstances[entity.Pointer] = new BannerBearerLogic.FormationBannerController.BannerInstance(agent, entity, BannerBearerLogic.FormationBannerController.BannerState.OnAgent);
				if (agent.IsAIControlled)
				{
					agent.ResetEnemyCaches();
					agent.Defensiveness = 1f;
				}
				this.UpdateBannerSearchers();
				this.CheckRequiresAgentStatUpdate();
			}

			// Token: 0x06003D99 RID: 15769 RVA: 0x000F651B File Offset: 0x000F471B
			public void OnBannerEntityDropped(GameEntity entity)
			{
				this._bannerInstances[entity.Pointer] = new BannerBearerLogic.FormationBannerController.BannerInstance(null, entity, BannerBearerLogic.FormationBannerController.BannerState.OnGround);
				this.UpdateBannerSearchers();
				this.CheckRequiresAgentStatUpdate();
			}

			// Token: 0x06003D9A RID: 15770 RVA: 0x000F6542 File Offset: 0x000F4742
			public void OnBeforeFormationMovementOrderApplied(Formation formation, MovementOrder.MovementOrderEnum orderType)
			{
				if (formation == this.Formation)
				{
					this.UpdateBannerBearerArrangementPositions();
				}
			}

			// Token: 0x06003D9B RID: 15771 RVA: 0x000F6553 File Offset: 0x000F4753
			public void OnAfterArrangementOrderApplied(Formation formation, ArrangementOrder.ArrangementOrderEnum orderEnum)
			{
				if (formation == this.Formation)
				{
					this.UpdateBannerBearerArrangementPositions();
				}
			}

			// Token: 0x06003D9C RID: 15772 RVA: 0x000F6564 File Offset: 0x000F4764
			private Agent FindBestSearcherForBanner(GameEntity banner, out float distance)
			{
				distance = float.MaxValue;
				Agent agent = null;
				Vec2 asVec = banner.GlobalPosition.AsVec2;
				this._mission.GetNearbyAllyAgents(asVec, BannerBearerLogic.FormationBannerController.BannerSearchDistance, this.Formation.Team, this._nearbyAllyAgentsListCache);
				BattleBannerBearersModel battleBannerBearersModel = MissionGameModels.Current.BattleBannerBearersModel;
				foreach (Agent agent2 in this._nearbyAllyAgentsListCache)
				{
					if (agent2.Formation == this.Formation && battleBannerBearersModel.CanAgentPickUpAnyBanner(agent2))
					{
						float num = agent2.Position.AsVec2.Distance(asVec);
						if (num < distance && !this._bannerSearchers.ContainsKey(agent2))
						{
							agent = agent2;
							distance = num;
						}
					}
				}
				return agent;
			}

			// Token: 0x06003D9D RID: 15773 RVA: 0x000F664C File Offset: 0x000F484C
			private List<Agent> FindBannerBearableAgents(int count)
			{
				List<Agent> list = new List<Agent>();
				if (count > 0)
				{
					BattleBannerBearersModel bannerBearerModel = MissionGameModels.Current.BattleBannerBearersModel;
					using (List<IFormationUnit>.Enumerator enumerator = this.Formation.UnitsWithoutLooseDetachedOnes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Agent agent2;
							if ((agent2 = enumerator.Current as Agent) != null && (agent2.Banner == null || agent2.Banner != this.BannerItem) && bannerBearerModel.CanAgentBecomeBannerBearer(agent2))
							{
								list.Add(agent2);
							}
						}
					}
					list = list.OrderByDescending<Agent, int>((Agent agent) => bannerBearerModel.GetAgentBannerBearingPriority(agent)).ToList<Agent>();
				}
				return list;
			}

			// Token: 0x06003D9E RID: 15774 RVA: 0x000F670C File Offset: 0x000F490C
			private void UpdateBannerBearerArrangementPositions()
			{
				List<Agent> list = (from instance in this._bannerInstances.Values
					where instance.IsOnAgent && instance.BannerBearer.Formation == this.Formation
					select instance.BannerBearer).ToList<Agent>();
				List<FormationArrangementModel.ArrangementPosition> bannerBearerPositions = MissionGameModels.Current.FormationArrangementsModel.GetBannerBearerPositions(this.Formation, list.Count);
				if (bannerBearerPositions == null || bannerBearerPositions.IsEmpty<FormationArrangementModel.ArrangementPosition>())
				{
					return;
				}
				int i = 0;
				foreach (Agent agent in list)
				{
					if (agent != null && agent.IsAIControlled && agent.Formation == this.Formation)
					{
						int num;
						int num2;
						agent.GetFormationFileAndRankInfo(out num, out num2);
						while (i < bannerBearerPositions.Count)
						{
							FormationArrangementModel.ArrangementPosition arrangementPosition = bannerBearerPositions[i];
							int fileIndex = arrangementPosition.FileIndex;
							int rankIndex = arrangementPosition.RankIndex;
							bool flag = num == fileIndex && num2 == rankIndex;
							if (!flag)
							{
								IFormationUnit unit = this.Formation.Arrangement.GetUnit(fileIndex, rankIndex);
								Agent agent2;
								if (unit != null && (agent2 = unit as Agent) != null)
								{
									if (agent2 == agent)
									{
										flag = true;
									}
									else if (agent2 != this.Formation.Captain)
									{
										this.Formation.SwitchUnitLocations(agent, agent2);
										flag = true;
									}
								}
							}
							if (flag)
							{
								i++;
								break;
							}
							i++;
						}
					}
				}
			}

			// Token: 0x06003D9F RID: 15775 RVA: 0x000F689C File Offset: 0x000F4A9C
			private void OnAgentAdded(Formation formation, Agent agent)
			{
				if (this.Formation == formation)
				{
					if (!this._bannerLogic._isMissionEnded && this._mission.Mode == MissionMode.Deployment && formation.Team.IsPlayerTeam && MissionGameModels.Current.BattleInitializationModel.CanPlayerSideDeployWithOrderOfBattle())
					{
						int minimumFormationTroopCountToBearBanners = MissionGameModels.Current.BattleBannerBearersModel.GetMinimumFormationTroopCountToBearBanners();
						if (formation.CountOfUnits == minimumFormationTroopCountToBearBanners && !this._bannerLogic._playerFormationsRequiringUpdate.Contains(this))
						{
							this._bannerLogic._playerFormationsRequiringUpdate.Add(this);
							return;
						}
					}
					else
					{
						this.UpdateBannerSearchers();
					}
				}
			}

			// Token: 0x06003DA0 RID: 15776 RVA: 0x000F6930 File Offset: 0x000F4B30
			private void OnAgentRemoved(Formation formation, Agent agent)
			{
				if (this.Formation == formation)
				{
					if (!this._bannerLogic._isMissionEnded && this._mission.Mode == MissionMode.Deployment && formation.Team.IsPlayerTeam && MissionGameModels.Current.BattleInitializationModel.CanPlayerSideDeployWithOrderOfBattle())
					{
						int minimumFormationTroopCountToBearBanners = MissionGameModels.Current.BattleBannerBearersModel.GetMinimumFormationTroopCountToBearBanners();
						if (formation.CountOfUnits == minimumFormationTroopCountToBearBanners - 1 && !this._bannerLogic._playerFormationsRequiringUpdate.Contains(this))
						{
							this._bannerLogic._playerFormationsRequiringUpdate.Add(this);
							return;
						}
					}
					else
					{
						this.UpdateBannerSearchers();
					}
				}
			}

			// Token: 0x06003DA1 RID: 15777 RVA: 0x000F69C8 File Offset: 0x000F4BC8
			private void CheckRequiresAgentStatUpdate()
			{
				if (!this._requiresAgentStatUpdate)
				{
					int numberOfActiveBannerBearers = this.GetNumberOfActiveBannerBearers();
					if ((numberOfActiveBannerBearers > 0 && this._lastActiveBannerBearerCount == 0) || (numberOfActiveBannerBearers == 0 && this._lastActiveBannerBearerCount > 0))
					{
						this._requiresAgentStatUpdate = true;
						this._lastActiveBannerBearerCount = numberOfActiveBannerBearers;
					}
				}
			}

			// Token: 0x06003DA2 RID: 15778 RVA: 0x000F6A0A File Offset: 0x000F4C0A
			private void AddBannerSearcher(Agent searcher, GameEntity banner, float distance)
			{
				this._bannerSearchers.Add(searcher, new ValueTuple<GameEntity, float>(banner, distance));
				HumanAIComponent humanAIComponent = searcher.HumanAIComponent;
				if (humanAIComponent == null)
				{
					return;
				}
				humanAIComponent.DisablePickUpForAgentIfNeeded();
			}

			// Token: 0x06003DA3 RID: 15779 RVA: 0x000F6A2F File Offset: 0x000F4C2F
			private void RemoveBannerSearcher(Agent searcher)
			{
				this._bannerSearchers.Remove(searcher);
				if (searcher.IsActive())
				{
					HumanAIComponent humanAIComponent = searcher.HumanAIComponent;
					if (humanAIComponent == null)
					{
						return;
					}
					humanAIComponent.DisablePickUpForAgentIfNeeded();
				}
			}

			// Token: 0x04001E51 RID: 7761
			private int _lastActiveBannerBearerCount;

			// Token: 0x04001E52 RID: 7762
			private bool _requiresAgentStatUpdate;

			// Token: 0x04001E53 RID: 7763
			private BannerBearerLogic _bannerLogic;

			// Token: 0x04001E54 RID: 7764
			private Mission _mission;

			// Token: 0x04001E55 RID: 7765
			[TupleElementNames(new string[] { "bannerEntity", "lastDistance" })]
			private Dictionary<Agent, ValueTuple<GameEntity, float>> _bannerSearchers;

			// Token: 0x04001E56 RID: 7766
			private readonly Dictionary<UIntPtr, BannerBearerLogic.FormationBannerController.BannerInstance> _bannerInstances;

			// Token: 0x04001E57 RID: 7767
			private MBList<Agent> _nearbyAllyAgentsListCache = new MBList<Agent>();

			// Token: 0x020006C4 RID: 1732
			public enum BannerState
			{
				// Token: 0x040023AD RID: 9133
				Initialized,
				// Token: 0x040023AE RID: 9134
				OnAgent,
				// Token: 0x040023AF RID: 9135
				OnGround
			}

			// Token: 0x020006C5 RID: 1733
			public struct BannerInstance
			{
				// Token: 0x17000B2A RID: 2858
				// (get) Token: 0x060042E9 RID: 17129 RVA: 0x001011CD File Offset: 0x000FF3CD
				public bool IsOnGround
				{
					get
					{
						return this.State == BannerBearerLogic.FormationBannerController.BannerState.OnGround;
					}
				}

				// Token: 0x17000B2B RID: 2859
				// (get) Token: 0x060042EA RID: 17130 RVA: 0x001011D8 File Offset: 0x000FF3D8
				public bool IsOnAgent
				{
					get
					{
						return this.State == BannerBearerLogic.FormationBannerController.BannerState.OnAgent;
					}
				}

				// Token: 0x060042EB RID: 17131 RVA: 0x001011E3 File Offset: 0x000FF3E3
				public BannerInstance(Agent bannerBearer, GameEntity entity, BannerBearerLogic.FormationBannerController.BannerState state)
				{
					this.BannerBearer = bannerBearer;
					this.Entity = entity;
					this.State = state;
				}

				// Token: 0x040023B0 RID: 9136
				public readonly Agent BannerBearer;

				// Token: 0x040023B1 RID: 9137
				public readonly GameEntity Entity;

				// Token: 0x040023B2 RID: 9138
				private readonly BannerBearerLogic.FormationBannerController.BannerState State;
			}
		}
	}
}
