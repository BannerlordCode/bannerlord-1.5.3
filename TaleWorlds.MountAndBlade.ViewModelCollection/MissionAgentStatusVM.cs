using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD.DamageFeed;
using TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction;

namespace TaleWorlds.MountAndBlade.ViewModelCollection
{
	// Token: 0x02000007 RID: 7
	public class MissionAgentStatusVM : ViewModel
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002556 File Offset: 0x00000756
		// (set) Token: 0x06000025 RID: 37 RVA: 0x0000255E File Offset: 0x0000075E
		public bool IsInDeployement { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002567 File Offset: 0x00000767
		private MissionPeer _myMissionPeer
		{
			get
			{
				if (this._missionPeer != null)
				{
					return this._missionPeer;
				}
				if (GameNetwork.MyPeer != null)
				{
					this._missionPeer = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				}
				return this._missionPeer;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002598 File Offset: 0x00000798
		public MissionAgentStatusVM(Mission mission, Camera missionCamera, Func<float> getCameraToggleProgress)
		{
			this.InteractionInterface = new AgentInteractionInterfaceVM(mission);
			this._mission = mission;
			this._missionCamera = missionCamera;
			this._getCameraToggleProgress = getCameraToggleProgress;
			this.PrimaryWeapon = new ItemImageIdentifierVM(null, "");
			this.OffhandWeapon = new ItemImageIdentifierVM(null, "");
			this.TakenDamageFeed = new MissionAgentDamageFeedVM();
			this.TakenDamageController = new MissionAgentTakenDamageVM(this._missionCamera);
			this.IsInteractionAvailable = true;
			this.IsAgentStatusPrioritized = true;
			this.RefreshValues();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002633 File Offset: 0x00000833
		public void InitializeMainAgentPropterties()
		{
			Mission.Current.OnMainAgentChanged += this.OnMainAgentChanged;
			this.OnMainAgentChanged(null);
			this.OnMainAgentWeaponChange();
			this._mpGameMode = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002668 File Offset: 0x00000868
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CameraToggleText = GameTexts.FindText("str_toggle_camera", null).ToString();
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002688 File Offset: 0x00000888
		private void OnMainAgentChanged(Agent oldAgent)
		{
			if (oldAgent != null)
			{
				oldAgent.OnMainAgentWieldedItemChange = (Agent.OnMainAgentWieldedItemChangeDelegate)Delegate.Remove(oldAgent.OnMainAgentWieldedItemChange, new Agent.OnMainAgentWieldedItemChangeDelegate(this.OnMainAgentWeaponChange));
			}
			if (Agent.Main != null)
			{
				Agent main = Agent.Main;
				main.OnMainAgentWieldedItemChange = (Agent.OnMainAgentWieldedItemChangeDelegate)Delegate.Combine(main.OnMainAgentWieldedItemChange, new Agent.OnMainAgentWieldedItemChangeDelegate(this.OnMainAgentWeaponChange));
				this.OnMainAgentWeaponChange();
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000026F0 File Offset: 0x000008F0
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (Agent.Main != null)
			{
				Agent main = Agent.Main;
				main.OnMainAgentWieldedItemChange = (Agent.OnMainAgentWieldedItemChangeDelegate)Delegate.Remove(main.OnMainAgentWieldedItemChange, new Agent.OnMainAgentWieldedItemChangeDelegate(this.OnMainAgentWeaponChange));
			}
			Mission.Current.OnMainAgentChanged -= this.OnMainAgentChanged;
			this.TakenDamageFeed.OnFinalize();
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002754 File Offset: 0x00000954
		public void Tick(float dt)
		{
			if (this._mission == null)
			{
				return;
			}
			this.CouchLanceState = this.GetCouchLanceState();
			this.SpearBraceState = this.GetSpearBraceState();
			Func<float> getCameraToggleProgress = this._getCameraToggleProgress;
			this.CameraToggleProgress = ((getCameraToggleProgress != null) ? getCameraToggleProgress() : 0f);
			if (this._mission.MainAgent != null && !this.IsInDeployement)
			{
				this.ShowAgentHealthBar = true;
				this.InteractionInterface.Tick(dt);
				if (this._mission.Mode == MissionMode.Battle && !this._mission.IsFriendlyMission && this._myMissionPeer != null)
				{
					MissionPeer myMissionPeer = this._myMissionPeer;
					this.IsTroopsActive = ((myMissionPeer != null) ? myMissionPeer.ControlledFormation : null) != null;
					if (this.IsTroopsActive)
					{
						this.TroopCount = this._myMissionPeer.ControlledFormation.CountOfUnits;
						if (this._myMissionPeer.ControlledFormation.QuerySystem.RangedUnitRatio > 0f || this._myMissionPeer.ControlledFormation.QuerySystem.RangedCavalryUnitRatio > 0f)
						{
							int totalCurrentAmmo = 0;
							int totalMaxAmmo = 0;
							this._myMissionPeer.ControlledFormation.ApplyActionOnEachUnit(delegate(Agent agent)
							{
								if (!agent.IsMainAgent)
								{
									int num;
									int num2;
									this.GetMaxAndCurrentAmmoOfAgent(agent, out num, out num2);
									totalCurrentAmmo += num;
									totalMaxAmmo += num2;
								}
							}, null);
							if (totalMaxAmmo > 0)
							{
								this.TroopsAmmoAvailable = true;
								this.TroopsAmmoPercentage = (float)totalCurrentAmmo / (float)totalMaxAmmo;
							}
							else
							{
								this.TroopsAmmoAvailable = false;
							}
						}
						else
						{
							this.TroopsAmmoAvailable = false;
						}
					}
				}
				this.UpdateWeaponStatuses();
				this.UpdateAgentAndMountStatuses();
				this.IsPlayerActive = true;
				this.IsCombatUIActive = true;
			}
			else
			{
				this.AgentHealth = 0;
				this.ShowMountHealthBar = false;
				this.ShowShieldHealthBar = false;
				if (this.IsCombatUIActive)
				{
					this._combatUIRemainTimer += dt;
					if (this._combatUIRemainTimer >= 2f)
					{
						this.IsCombatUIActive = false;
					}
				}
			}
			MissionMultiplayerGameModeBaseClient mpGameMode = this._mpGameMode;
			this.IsGoldActive = mpGameMode != null && mpGameMode.IsGameModeUsingGold;
			if (this.IsGoldActive && this._myMissionPeer != null && this._myMissionPeer.GetNetworkPeer().IsSynchronized)
			{
				MissionMultiplayerGameModeBaseClient mpGameMode2 = this._mpGameMode;
				this.GoldAmount = ((mpGameMode2 != null) ? mpGameMode2.GetGoldAmount() : 0);
			}
			MissionAgentTakenDamageVM takenDamageController = this.TakenDamageController;
			if (takenDamageController == null)
			{
				return;
			}
			takenDamageController.Tick(dt);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000029A0 File Offset: 0x00000BA0
		private void UpdateWeaponStatuses()
		{
			bool flag = false;
			if (this._mission.MainAgent != null)
			{
				int num = -1;
				EquipmentIndex primaryWieldedItemIndex = this._mission.MainAgent.GetPrimaryWieldedItemIndex();
				EquipmentIndex offhandWieldedItemIndex = this._mission.MainAgent.GetOffhandWieldedItemIndex();
				if (primaryWieldedItemIndex != EquipmentIndex.None && this._mission.MainAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem != null)
				{
					if (this._mission.MainAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.IsRangedWeapon && this._mission.MainAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.IsConsumable)
					{
						int num2;
						if (!this._mission.MainAgent.Equipment[primaryWieldedItemIndex].Item.PrimaryWeapon.IsConsumable && this._mission.MainAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.IsConsumable)
						{
							num2 = 1;
						}
						else
						{
							num2 = this._mission.MainAgent.Equipment.GetAmmoAmount(primaryWieldedItemIndex);
						}
						if (this._mission.MainAgent.Equipment[primaryWieldedItemIndex].ModifiedMaxAmount == 1 || num2 > 0)
						{
							num = num2;
						}
					}
					else if (this._mission.MainAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.IsRangedWeapon)
					{
						bool flag2 = this._mission.MainAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.WeaponClass == WeaponClass.Crossbow;
						num = this._mission.MainAgent.Equipment.GetAmmoAmount(primaryWieldedItemIndex) + (int)(flag2 ? this._mission.MainAgent.Equipment[primaryWieldedItemIndex].Ammo : 0);
					}
					if (!this._mission.MainAgent.Equipment[primaryWieldedItemIndex].IsEmpty)
					{
						int num3;
						if (!this._mission.MainAgent.Equipment[primaryWieldedItemIndex].Item.PrimaryWeapon.IsConsumable && this._mission.MainAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.IsConsumable)
						{
							num3 = 1;
						}
						else
						{
							num3 = this._mission.MainAgent.Equipment.GetMaxAmmo(primaryWieldedItemIndex);
						}
						float num4 = (float)num3 * 0.2f;
						flag = num3 != this.AmmoCount && this.AmmoCount <= MathF.Ceiling(num4);
					}
				}
				if (offhandWieldedItemIndex != EquipmentIndex.None && this._mission.MainAgent.Equipment[offhandWieldedItemIndex].CurrentUsageItem != null)
				{
					MissionWeapon missionWeapon = this._mission.MainAgent.Equipment[offhandWieldedItemIndex];
					this.ShowShieldHealthBar = missionWeapon.CurrentUsageItem.IsShield;
					if (this.ShowShieldHealthBar)
					{
						this.ShieldHealthMax = (int)missionWeapon.ModifiedMaxHitPoints;
						this.ShieldHealth = (int)missionWeapon.HitPoints;
					}
				}
				this.AmmoCount = num;
			}
			else
			{
				this.ShieldHealth = 0;
				this.AmmoCount = 0;
				this.ShowShieldHealthBar = false;
			}
			this.IsAmmoCountAlertEnabled = flag;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002CE1 File Offset: 0x00000EE1
		public void OnEquipmentInteractionViewToggled(bool isActive)
		{
			this.IsInteractionAvailable = !isActive;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002CF0 File Offset: 0x00000EF0
		private void UpdateAgentAndMountStatuses()
		{
			if (this._mission.MainAgent == null)
			{
				this.AgentHealthMax = 1;
				this.AgentHealth = (int)this._mission.MainAgent.Health;
				this.HorseHealthMax = 1;
				this.HorseHealth = 0;
				this.ShowMountHealthBar = false;
				return;
			}
			this.AgentHealthMax = (int)this._mission.MainAgent.HealthLimit;
			this.AgentHealth = (int)this._mission.MainAgent.Health;
			if (this._mission.MainAgent.MountAgent != null)
			{
				this.HorseHealthMax = (int)this._mission.MainAgent.MountAgent.HealthLimit;
				this.HorseHealth = (int)this._mission.MainAgent.MountAgent.Health;
				this.ShowMountHealthBar = true;
				return;
			}
			this.ShowMountHealthBar = false;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002DC8 File Offset: 0x00000FC8
		public void OnMainAgentWeaponChange()
		{
			if (this._mission.MainAgent == null)
			{
				return;
			}
			MissionWeapon missionWeapon = MissionWeapon.Invalid;
			MissionWeapon missionWeapon2 = MissionWeapon.Invalid;
			EquipmentIndex equipmentIndex = this._mission.MainAgent.GetOffhandWieldedItemIndex();
			if (equipmentIndex > EquipmentIndex.None && equipmentIndex < EquipmentIndex.NumAllWeaponSlots)
			{
				missionWeapon = this._mission.MainAgent.Equipment[equipmentIndex];
			}
			equipmentIndex = this._mission.MainAgent.GetPrimaryWieldedItemIndex();
			if (equipmentIndex > EquipmentIndex.None && equipmentIndex < EquipmentIndex.NumAllWeaponSlots)
			{
				missionWeapon2 = this._mission.MainAgent.Equipment[equipmentIndex];
			}
			WeaponComponentData currentUsageItem = missionWeapon.CurrentUsageItem;
			this.ShowShieldHealthBar = currentUsageItem != null && currentUsageItem.IsShield;
			this.PrimaryWeapon = (missionWeapon2.IsEmpty ? new ItemImageIdentifierVM(null, "") : new ItemImageIdentifierVM(missionWeapon2.Item, ""));
			this.OffhandWeapon = (missionWeapon.IsEmpty ? new ItemImageIdentifierVM(null, "") : new ItemImageIdentifierVM(missionWeapon.Item, ""));
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002EC2 File Offset: 0x000010C2
		public void OnAgentRemoved(Agent agent)
		{
			this.InteractionInterface.CheckAndClearFocusedAgent(agent);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002ED0 File Offset: 0x000010D0
		public void OnAgentDeleted(Agent agent)
		{
			this.InteractionInterface.CheckAndClearFocusedAgent(agent);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002EDE File Offset: 0x000010DE
		public void OnMainAgentHit(int damage, float distance)
		{
			this.TakenDamageController.OnMainAgentHit(damage, distance);
			this.TakenDamageFeed.OnMainAgentHit((float)damage);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002EFA File Offset: 0x000010FA
		public void OnFocusGained(Agent mainAgent, IFocusable focusableObject, bool isInteractable)
		{
			this.InteractionInterface.OnFocusGained(mainAgent, focusableObject, isInteractable);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002F0A File Offset: 0x0000110A
		public void OnFocusLost(Agent agent, IFocusable focusableObject)
		{
			this.InteractionInterface.OnFocusLost(agent, focusableObject);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002F19 File Offset: 0x00001119
		public void OnSecondaryFocusGained(Agent agent, IFocusable focusableObject, bool isInteractable)
		{
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002F1B File Offset: 0x0000111B
		public void OnSecondaryFocusLost(Agent agent, IFocusable focusableObject)
		{
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002F1D File Offset: 0x0000111D
		public void OnAgentInteraction(Agent userAgent, Agent agent, sbyte agentBoneIndex)
		{
			this.InteractionInterface.OnAgentInteraction(userAgent, agent, agentBoneIndex);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002F30 File Offset: 0x00001130
		private void GetMaxAndCurrentAmmoOfAgent(Agent agent, out int currentAmmo, out int maxAmmo)
		{
			currentAmmo = 0;
			maxAmmo = 0;
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.ExtraWeaponSlot; equipmentIndex++)
			{
				if (!agent.Equipment[equipmentIndex].IsEmpty && agent.Equipment[equipmentIndex].CurrentUsageItem.IsRangedWeapon)
				{
					currentAmmo = agent.Equipment.GetAmmoAmount(equipmentIndex);
					maxAmmo = agent.Equipment.GetMaxAmmo(equipmentIndex);
					return;
				}
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002FA0 File Offset: 0x000011A0
		private int GetCouchLanceState()
		{
			int num = 0;
			if (Agent.Main != null)
			{
				MissionWeapon wieldedWeapon = Agent.Main.WieldedWeapon;
				if (Agent.Main.HasMount && this.IsWeaponCouchable(wieldedWeapon))
				{
					if (this.IsPassiveUsageActiveWithCurrentWeapon(wieldedWeapon))
					{
						num = 3;
					}
					else if (this.IsConditionsMetForCouching())
					{
						num = 2;
					}
				}
			}
			return num;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002FF0 File Offset: 0x000011F0
		private bool IsWeaponCouchable(MissionWeapon weapon)
		{
			if (weapon.IsEmpty)
			{
				return false;
			}
			foreach (WeaponComponentData weaponComponentData in weapon.Item.Weapons)
			{
				string weaponDescriptionId = weaponComponentData.WeaponDescriptionId;
				if (weaponDescriptionId != null && weaponDescriptionId.IndexOf("couch", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003074 File Offset: 0x00001274
		private bool IsConditionsMetForCouching()
		{
			return Agent.Main.HasMount && Agent.Main.IsPassiveUsageConditionsAreMet;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003090 File Offset: 0x00001290
		private int GetSpearBraceState()
		{
			int num = 0;
			if (Agent.Main != null)
			{
				MissionWeapon wieldedWeapon = Agent.Main.WieldedWeapon;
				if (!Agent.Main.HasMount && Agent.Main.GetOffhandWieldedItemIndex() == EquipmentIndex.None && this.IsWeaponBracable(wieldedWeapon))
				{
					if (this.IsPassiveUsageActiveWithCurrentWeapon(wieldedWeapon))
					{
						num = 3;
					}
					else if (this.IsConditionsMetForBracing())
					{
						num = 2;
					}
				}
			}
			return num;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000030EC File Offset: 0x000012EC
		private bool IsWeaponBracable(MissionWeapon weapon)
		{
			if (weapon.IsEmpty)
			{
				return false;
			}
			foreach (WeaponComponentData weaponComponentData in weapon.Item.Weapons)
			{
				string weaponDescriptionId = weaponComponentData.WeaponDescriptionId;
				if (weaponDescriptionId != null && weaponDescriptionId.IndexOf("bracing", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003170 File Offset: 0x00001370
		private bool IsConditionsMetForBracing()
		{
			return !Agent.Main.HasMount && !Agent.Main.WalkMode && Agent.Main.IsPassiveUsageConditionsAreMet;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003196 File Offset: 0x00001396
		private bool IsPassiveUsageActiveWithCurrentWeapon(MissionWeapon weapon)
		{
			return !weapon.IsEmpty && MBItem.GetItemIsPassiveUsage(weapon.CurrentUsageItem.ItemUsage);
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000041 RID: 65 RVA: 0x000031B4 File Offset: 0x000013B4
		// (set) Token: 0x06000042 RID: 66 RVA: 0x000031BC File Offset: 0x000013BC
		[DataSourceProperty]
		public MissionAgentTakenDamageVM TakenDamageController
		{
			get
			{
				return this._takenDamageController;
			}
			set
			{
				if (value != this._takenDamageController)
				{
					this._takenDamageController = value;
					base.OnPropertyChangedWithValue<MissionAgentTakenDamageVM>(value, "TakenDamageController");
				}
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000043 RID: 67 RVA: 0x000031DA File Offset: 0x000013DA
		// (set) Token: 0x06000044 RID: 68 RVA: 0x000031E2 File Offset: 0x000013E2
		[DataSourceProperty]
		public AgentInteractionInterfaceVM InteractionInterface
		{
			get
			{
				return this._interactionInterface;
			}
			set
			{
				if (value != this._interactionInterface)
				{
					this._interactionInterface = value;
					base.OnPropertyChangedWithValue<AgentInteractionInterfaceVM>(value, "InteractionInterface");
				}
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00003200 File Offset: 0x00001400
		// (set) Token: 0x06000046 RID: 70 RVA: 0x00003208 File Offset: 0x00001408
		[DataSourceProperty]
		public int AgentHealth
		{
			get
			{
				return this._agentHealth;
			}
			set
			{
				if (value != this._agentHealth)
				{
					if (value <= 0)
					{
						this._agentHealth = 0;
						this.OffhandWeapon = new ItemImageIdentifierVM(null, "");
						this.PrimaryWeapon = new ItemImageIdentifierVM(null, "");
						this.AmmoCount = -1;
						this.ShieldHealth = 100;
						this.IsPlayerActive = false;
					}
					else
					{
						this._agentHealth = value;
					}
					base.OnPropertyChangedWithValue(value, "AgentHealth");
				}
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000047 RID: 71 RVA: 0x00003276 File Offset: 0x00001476
		// (set) Token: 0x06000048 RID: 72 RVA: 0x0000327E File Offset: 0x0000147E
		[DataSourceProperty]
		public int AgentHealthMax
		{
			get
			{
				return this._agentHealthMax;
			}
			set
			{
				if (value != this._agentHealthMax)
				{
					this._agentHealthMax = value;
					base.OnPropertyChangedWithValue(value, "AgentHealthMax");
				}
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000049 RID: 73 RVA: 0x0000329C File Offset: 0x0000149C
		// (set) Token: 0x0600004A RID: 74 RVA: 0x000032A4 File Offset: 0x000014A4
		[DataSourceProperty]
		public int HorseHealth
		{
			get
			{
				return this._horseHealth;
			}
			set
			{
				if (value != this._horseHealth)
				{
					this._horseHealth = value;
					base.OnPropertyChangedWithValue(value, "HorseHealth");
				}
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600004B RID: 75 RVA: 0x000032C2 File Offset: 0x000014C2
		// (set) Token: 0x0600004C RID: 76 RVA: 0x000032CA File Offset: 0x000014CA
		[DataSourceProperty]
		public int HorseHealthMax
		{
			get
			{
				return this._horseHealthMax;
			}
			set
			{
				if (value != this._horseHealthMax)
				{
					this._horseHealthMax = value;
					base.OnPropertyChangedWithValue(value, "HorseHealthMax");
				}
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600004D RID: 77 RVA: 0x000032E8 File Offset: 0x000014E8
		// (set) Token: 0x0600004E RID: 78 RVA: 0x000032F0 File Offset: 0x000014F0
		[DataSourceProperty]
		public int ShieldHealth
		{
			get
			{
				return this._shieldHealth;
			}
			set
			{
				if (value != this._shieldHealth)
				{
					this._shieldHealth = value;
					base.OnPropertyChangedWithValue(value, "ShieldHealth");
				}
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600004F RID: 79 RVA: 0x0000330E File Offset: 0x0000150E
		// (set) Token: 0x06000050 RID: 80 RVA: 0x00003316 File Offset: 0x00001516
		[DataSourceProperty]
		public int ShieldHealthMax
		{
			get
			{
				return this._shieldHealthMax;
			}
			set
			{
				if (value != this._shieldHealthMax)
				{
					this._shieldHealthMax = value;
					base.OnPropertyChangedWithValue(value, "ShieldHealthMax");
				}
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000051 RID: 81 RVA: 0x00003334 File Offset: 0x00001534
		// (set) Token: 0x06000052 RID: 82 RVA: 0x0000333C File Offset: 0x0000153C
		[DataSourceProperty]
		public bool IsPlayerActive
		{
			get
			{
				return this._isPlayerActive;
			}
			set
			{
				if (value != this._isPlayerActive)
				{
					this._isPlayerActive = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerActive");
				}
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000053 RID: 83 RVA: 0x0000335A File Offset: 0x0000155A
		// (set) Token: 0x06000054 RID: 84 RVA: 0x00003362 File Offset: 0x00001562
		public bool IsCombatUIActive
		{
			get
			{
				return this._isCombatUIActive;
			}
			set
			{
				if (value != this._isCombatUIActive)
				{
					this._isCombatUIActive = value;
					base.OnPropertyChangedWithValue(value, "IsCombatUIActive");
					this._combatUIRemainTimer = 0f;
				}
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000055 RID: 85 RVA: 0x0000338B File Offset: 0x0000158B
		// (set) Token: 0x06000056 RID: 86 RVA: 0x00003393 File Offset: 0x00001593
		[DataSourceProperty]
		public bool ShowAgentHealthBar
		{
			get
			{
				return this._showAgentHealthBar;
			}
			set
			{
				if (value != this._showAgentHealthBar)
				{
					this._showAgentHealthBar = value;
					base.OnPropertyChangedWithValue(value, "ShowAgentHealthBar");
				}
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000057 RID: 87 RVA: 0x000033B1 File Offset: 0x000015B1
		// (set) Token: 0x06000058 RID: 88 RVA: 0x000033B9 File Offset: 0x000015B9
		[DataSourceProperty]
		public bool ShowMountHealthBar
		{
			get
			{
				return this._showMountHealthBar;
			}
			set
			{
				if (value != this._showMountHealthBar)
				{
					this._showMountHealthBar = value;
					base.OnPropertyChangedWithValue(value, "ShowMountHealthBar");
				}
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000059 RID: 89 RVA: 0x000033D7 File Offset: 0x000015D7
		// (set) Token: 0x0600005A RID: 90 RVA: 0x000033DF File Offset: 0x000015DF
		[DataSourceProperty]
		public bool ShowShieldHealthBar
		{
			get
			{
				return this._showShieldHealthBar;
			}
			set
			{
				if (value != this._showShieldHealthBar)
				{
					this._showShieldHealthBar = value;
					base.OnPropertyChangedWithValue(value, "ShowShieldHealthBar");
				}
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600005B RID: 91 RVA: 0x000033FD File Offset: 0x000015FD
		// (set) Token: 0x0600005C RID: 92 RVA: 0x00003405 File Offset: 0x00001605
		[DataSourceProperty]
		public bool IsInteractionAvailable
		{
			get
			{
				return this._isInteractionAvailable;
			}
			set
			{
				if (value != this._isInteractionAvailable)
				{
					this._isInteractionAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsInteractionAvailable");
				}
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00003423 File Offset: 0x00001623
		// (set) Token: 0x0600005E RID: 94 RVA: 0x0000342B File Offset: 0x0000162B
		[DataSourceProperty]
		public bool IsAgentStatusPrioritized
		{
			get
			{
				return this._isAgentStatusPrioritized;
			}
			set
			{
				if (value != this._isAgentStatusPrioritized)
				{
					this._isAgentStatusPrioritized = value;
					base.OnPropertyChangedWithValue(value, "IsAgentStatusPrioritized");
				}
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00003449 File Offset: 0x00001649
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00003451 File Offset: 0x00001651
		[DataSourceProperty]
		public bool IsAgentStatusAvailable
		{
			get
			{
				return this._isAgentStatusAvailable;
			}
			set
			{
				if (value != this._isAgentStatusAvailable)
				{
					this._isAgentStatusAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsAgentStatusAvailable");
				}
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000061 RID: 97 RVA: 0x0000346F File Offset: 0x0000166F
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00003477 File Offset: 0x00001677
		[DataSourceProperty]
		public int CouchLanceState
		{
			get
			{
				return this._couchLanceState;
			}
			set
			{
				if (value != this._couchLanceState)
				{
					this._couchLanceState = value;
					base.OnPropertyChangedWithValue(value, "CouchLanceState");
				}
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00003495 File Offset: 0x00001695
		// (set) Token: 0x06000064 RID: 100 RVA: 0x0000349D File Offset: 0x0000169D
		[DataSourceProperty]
		public int SpearBraceState
		{
			get
			{
				return this._spearBraceState;
			}
			set
			{
				if (value != this._spearBraceState)
				{
					this._spearBraceState = value;
					base.OnPropertyChangedWithValue(value, "SpearBraceState");
				}
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000065 RID: 101 RVA: 0x000034BB File Offset: 0x000016BB
		// (set) Token: 0x06000066 RID: 102 RVA: 0x000034C3 File Offset: 0x000016C3
		[DataSourceProperty]
		public int TroopCount
		{
			get
			{
				return this._troopCount;
			}
			set
			{
				if (value != this._troopCount)
				{
					this._troopCount = value;
					base.OnPropertyChangedWithValue(value, "TroopCount");
				}
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000034E1 File Offset: 0x000016E1
		// (set) Token: 0x06000068 RID: 104 RVA: 0x000034E9 File Offset: 0x000016E9
		[DataSourceProperty]
		public bool IsTroopsActive
		{
			get
			{
				return this._isTroopsActive;
			}
			set
			{
				if (value != this._isTroopsActive)
				{
					this._isTroopsActive = value;
					base.OnPropertyChangedWithValue(value, "IsTroopsActive");
				}
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000069 RID: 105 RVA: 0x00003507 File Offset: 0x00001707
		// (set) Token: 0x0600006A RID: 106 RVA: 0x0000350F File Offset: 0x0000170F
		[DataSourceProperty]
		public bool IsGoldActive
		{
			get
			{
				return this._isGoldActive;
			}
			set
			{
				if (value != this._isGoldActive)
				{
					this._isGoldActive = value;
					base.OnPropertyChangedWithValue(value, "IsGoldActive");
				}
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600006B RID: 107 RVA: 0x0000352D File Offset: 0x0000172D
		// (set) Token: 0x0600006C RID: 108 RVA: 0x00003535 File Offset: 0x00001735
		[DataSourceProperty]
		public int GoldAmount
		{
			get
			{
				return this._goldAmount;
			}
			set
			{
				if (value != this._goldAmount)
				{
					this._goldAmount = value;
					base.OnPropertyChangedWithValue(value, "GoldAmount");
				}
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003553 File Offset: 0x00001753
		// (set) Token: 0x0600006E RID: 110 RVA: 0x0000355B File Offset: 0x0000175B
		[DataSourceProperty]
		public bool ShowAmmoCount
		{
			get
			{
				return this._showAmmoCount;
			}
			set
			{
				if (value != this._showAmmoCount)
				{
					this._showAmmoCount = value;
					base.OnPropertyChangedWithValue(value, "ShowAmmoCount");
				}
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003579 File Offset: 0x00001779
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00003581 File Offset: 0x00001781
		[DataSourceProperty]
		public int AmmoCount
		{
			get
			{
				return this._ammoCount;
			}
			set
			{
				if (value != this._ammoCount)
				{
					this._ammoCount = value;
					base.OnPropertyChangedWithValue(value, "AmmoCount");
					this.ShowAmmoCount = value >= 0;
				}
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000071 RID: 113 RVA: 0x000035AC File Offset: 0x000017AC
		// (set) Token: 0x06000072 RID: 114 RVA: 0x000035B4 File Offset: 0x000017B4
		[DataSourceProperty]
		public float TroopsAmmoPercentage
		{
			get
			{
				return this._troopsAmmoPercentage;
			}
			set
			{
				if (value != this._troopsAmmoPercentage)
				{
					this._troopsAmmoPercentage = value;
					base.OnPropertyChangedWithValue(value, "TroopsAmmoPercentage");
				}
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000073 RID: 115 RVA: 0x000035D2 File Offset: 0x000017D2
		// (set) Token: 0x06000074 RID: 116 RVA: 0x000035DA File Offset: 0x000017DA
		[DataSourceProperty]
		public bool TroopsAmmoAvailable
		{
			get
			{
				return this._troopsAmmoAvailable;
			}
			set
			{
				if (value != this._troopsAmmoAvailable)
				{
					this._troopsAmmoAvailable = value;
					base.OnPropertyChangedWithValue(value, "TroopsAmmoAvailable");
				}
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000075 RID: 117 RVA: 0x000035F8 File Offset: 0x000017F8
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00003600 File Offset: 0x00001800
		[DataSourceProperty]
		public bool IsAmmoCountAlertEnabled
		{
			get
			{
				return this._isAmmoCountAlertEnabled;
			}
			set
			{
				if (value != this._isAmmoCountAlertEnabled)
				{
					this._isAmmoCountAlertEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsAmmoCountAlertEnabled");
				}
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000077 RID: 119 RVA: 0x0000361E File Offset: 0x0000181E
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00003626 File Offset: 0x00001826
		[DataSourceProperty]
		public float CameraToggleProgress
		{
			get
			{
				return this._cameraToggleProgress;
			}
			set
			{
				if (value != this._cameraToggleProgress)
				{
					this._cameraToggleProgress = value;
					base.OnPropertyChangedWithValue(value, "CameraToggleProgress");
				}
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003644 File Offset: 0x00001844
		// (set) Token: 0x0600007A RID: 122 RVA: 0x0000364C File Offset: 0x0000184C
		[DataSourceProperty]
		public string CameraToggleText
		{
			get
			{
				return this._cameraToggleText;
			}
			set
			{
				if (value != this._cameraToggleText)
				{
					this._cameraToggleText = value;
					base.OnPropertyChangedWithValue<string>(value, "CameraToggleText");
				}
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600007B RID: 123 RVA: 0x0000366F File Offset: 0x0000186F
		// (set) Token: 0x0600007C RID: 124 RVA: 0x00003677 File Offset: 0x00001877
		[DataSourceProperty]
		public ItemImageIdentifierVM OffhandWeapon
		{
			get
			{
				return this._offhandWeapon;
			}
			set
			{
				if (value != this._offhandWeapon)
				{
					this._offhandWeapon = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "OffhandWeapon");
				}
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003695 File Offset: 0x00001895
		// (set) Token: 0x0600007E RID: 126 RVA: 0x0000369D File Offset: 0x0000189D
		[DataSourceProperty]
		public ItemImageIdentifierVM PrimaryWeapon
		{
			get
			{
				return this._primaryWeapon;
			}
			set
			{
				if (value != this._primaryWeapon)
				{
					this._primaryWeapon = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "PrimaryWeapon");
				}
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600007F RID: 127 RVA: 0x000036BB File Offset: 0x000018BB
		// (set) Token: 0x06000080 RID: 128 RVA: 0x000036C3 File Offset: 0x000018C3
		[DataSourceProperty]
		public MissionAgentDamageFeedVM TakenDamageFeed
		{
			get
			{
				return this._takenDamageFeed;
			}
			set
			{
				if (value != this._takenDamageFeed)
				{
					this._takenDamageFeed = value;
					base.OnPropertyChangedWithValue<MissionAgentDamageFeedVM>(value, "TakenDamageFeed");
				}
			}
		}

		// Token: 0x04000012 RID: 18
		private readonly Mission _mission;

		// Token: 0x04000013 RID: 19
		private readonly Camera _missionCamera;

		// Token: 0x04000014 RID: 20
		private float _combatUIRemainTimer;

		// Token: 0x04000015 RID: 21
		private const float _combatUIRemainDuration = 2f;

		// Token: 0x04000016 RID: 22
		private MissionPeer _missionPeer;

		// Token: 0x04000017 RID: 23
		private MissionMultiplayerGameModeBaseClient _mpGameMode;

		// Token: 0x04000018 RID: 24
		private readonly Func<float> _getCameraToggleProgress;

		// Token: 0x04000019 RID: 25
		private int _agentHealth;

		// Token: 0x0400001A RID: 26
		private int _agentHealthMax;

		// Token: 0x0400001B RID: 27
		private int _horseHealth;

		// Token: 0x0400001C RID: 28
		private int _horseHealthMax;

		// Token: 0x0400001D RID: 29
		private int _shieldHealth;

		// Token: 0x0400001E RID: 30
		private int _shieldHealthMax;

		// Token: 0x0400001F RID: 31
		private bool _isPlayerActive = true;

		// Token: 0x04000020 RID: 32
		private bool _isCombatUIActive;

		// Token: 0x04000021 RID: 33
		private bool _showAgentHealthBar;

		// Token: 0x04000022 RID: 34
		private bool _showMountHealthBar;

		// Token: 0x04000023 RID: 35
		private bool _showShieldHealthBar;

		// Token: 0x04000024 RID: 36
		private bool _troopsAmmoAvailable;

		// Token: 0x04000025 RID: 37
		private bool _isAgentStatusAvailable;

		// Token: 0x04000026 RID: 38
		private bool _isInteractionAvailable;

		// Token: 0x04000027 RID: 39
		private bool _isAgentStatusPrioritized;

		// Token: 0x04000028 RID: 40
		private float _troopsAmmoPercentage;

		// Token: 0x04000029 RID: 41
		private int _troopCount;

		// Token: 0x0400002A RID: 42
		private int _goldAmount;

		// Token: 0x0400002B RID: 43
		private bool _isTroopsActive;

		// Token: 0x0400002C RID: 44
		private bool _isGoldActive;

		// Token: 0x0400002D RID: 45
		private AgentInteractionInterfaceVM _interactionInterface;

		// Token: 0x0400002E RID: 46
		private ItemImageIdentifierVM _offhandWeapon;

		// Token: 0x0400002F RID: 47
		private ItemImageIdentifierVM _primaryWeapon;

		// Token: 0x04000030 RID: 48
		private MissionAgentTakenDamageVM _takenDamageController;

		// Token: 0x04000031 RID: 49
		private MissionAgentDamageFeedVM _takenDamageFeed;

		// Token: 0x04000032 RID: 50
		private int _ammoCount;

		// Token: 0x04000033 RID: 51
		private int _couchLanceState = -1;

		// Token: 0x04000034 RID: 52
		private int _spearBraceState = -1;

		// Token: 0x04000035 RID: 53
		private bool _showAmmoCount;

		// Token: 0x04000036 RID: 54
		private bool _isAmmoCountAlertEnabled;

		// Token: 0x04000037 RID: 55
		private float _cameraToggleProgress;

		// Token: 0x04000038 RID: 56
		private string _cameraToggleText;

		// Token: 0x0200008B RID: 139
		private enum PassiveUsageStates
		{
			// Token: 0x04000558 RID: 1368
			NotPossible,
			// Token: 0x04000559 RID: 1369
			ConditionsNotMet,
			// Token: 0x0400055A RID: 1370
			Possible,
			// Token: 0x0400055B RID: 1371
			Active
		}
	}
}
