using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions
{
	// Token: 0x02000096 RID: 150
	public class MissionMultiplayerSpectatorHUDVM : ViewModel
	{
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000ED9 RID: 3801 RVA: 0x0002DF6C File Offset: 0x0002C16C
		// (remove) Token: 0x06000EDA RID: 3802 RVA: 0x0002DFA4 File Offset: 0x0002C1A4
		public event Action<int> OnCycleTargetRequested;

		// Token: 0x06000EDB RID: 3803 RVA: 0x0002DFDC File Offset: 0x0002C1DC
		private void RefreshCycleTargetKeys()
		{
			GameKeyContext category = HotKeyManager.GetCategory("MultiplayerHotkeyCategory");
			HotKey hotKey = ((category != null) ? category.GetHotKey("CycleSpectatorTargetPrevious") : null);
			if (hotKey != null)
			{
				this.CyclePreviousKey = InputKeyItemVM.CreateFromHotKey(hotKey, false);
			}
			HotKey hotKey2 = ((category != null) ? category.GetHotKey("CycleSpectatorTargetNext") : null);
			if (hotKey2 != null)
			{
				this.CycleNextKey = InputKeyItemVM.CreateFromHotKey(hotKey2, false);
			}
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x0002E038 File Offset: 0x0002C238
		public void ExecuteCyclePreviousTarget()
		{
			Action<int> onCycleTargetRequested = this.OnCycleTargetRequested;
			if (onCycleTargetRequested == null)
			{
				return;
			}
			onCycleTargetRequested(-1);
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x0002E04B File Offset: 0x0002C24B
		public void ExecuteCycleNextTarget()
		{
			Action<int> onCycleTargetRequested = this.OnCycleTargetRequested;
			if (onCycleTargetRequested == null)
			{
				return;
			}
			onCycleTargetRequested(1);
		}

		// Token: 0x06000EDE RID: 3806 RVA: 0x0002E060 File Offset: 0x0002C260
		public MissionMultiplayerSpectatorHUDVM(Mission mission)
		{
			this._mission = mission;
			MissionLobbyComponent missionBehavior = mission.GetMissionBehavior<MissionLobbyComponent>();
			this._isTeamsEnabled = missionBehavior == null || missionBehavior.MissionType != MultiplayerGameType.Duel;
			this._isFlagDominationMode = Mission.Current.HasMissionBehavior<MissionMultiplayerGameModeFlagDominationClient>();
			this._gameModeClient = mission.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>();
			this._isGameModeUsingGold = this._gameModeClient != null && this._gameModeClient.IsGameModeUsingGold;
			this.SpectatedPlayerWeapons = new MBBindingList<SpectatorWeaponSlotVM>();
			this.SpectatedPlayerPerks = new MBBindingList<MPPerkVM>();
			this.SpectatorStats = new MBBindingList<MPOverlayStatVM>();
			this.BuildStats();
			this.RefreshValues();
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x0002E140 File Offset: 0x0002C340
		public override void RefreshValues()
		{
			base.RefreshValues();
			string keyHyperlinkText = HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f);
			GameTexts.SetVariable("USE_KEY", keyHyperlinkText);
			this.TakeControlText = GameTexts.FindText("str_sergeant_battle_press_action_to_control_bot_2", null).ToString();
			this.RefreshCycleTargetKeys();
			this.RefreshStatNames();
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x0002E198 File Offset: 0x0002C398
		private void BuildStats()
		{
			this._killCountStat = this.AddStat("kill");
			this._deathCountStat = this.AddStat("death");
			this._assistCountStat = this.AddStat("assist");
			if (this._isGameModeUsingGold)
			{
				this._goldStat = this.AddStat("gold");
			}
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0002E1F4 File Offset: 0x0002C3F4
		private MPOverlayStatVM AddStat(string statId)
		{
			MPOverlayStatVM mpoverlayStatVM = new MPOverlayStatVM(statId, MissionMultiplayerSpectatorHUDVM.GetStatName(statId), string.Empty);
			this.SpectatorStats.Add(mpoverlayStatVM);
			return mpoverlayStatVM;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x0002E220 File Offset: 0x0002C420
		private void RefreshEquippedWeaponSlot()
		{
			if (this.SpectatedPlayerWeapons.Count == 0)
			{
				return;
			}
			EquipmentIndex equipmentIndex = EquipmentIndex.None;
			EquipmentIndex equipmentIndex2 = EquipmentIndex.None;
			if (this._spectatedAgent != null)
			{
				equipmentIndex = this._spectatedAgent.GetPrimaryWieldedItemIndex();
				equipmentIndex2 = this._spectatedAgent.GetOffhandWieldedItemIndex();
			}
			for (int i = 0; i < this.SpectatedPlayerWeapons.Count; i++)
			{
				SpectatorWeaponSlotVM spectatorWeaponSlotVM = this.SpectatedPlayerWeapons[i];
				bool flag = spectatorWeaponSlotVM.SlotIndex != EquipmentIndex.None && (spectatorWeaponSlotVM.SlotIndex == equipmentIndex || spectatorWeaponSlotVM.SlotIndex == equipmentIndex2);
				spectatorWeaponSlotVM.SetEquipped(flag);
			}
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x0002E2B0 File Offset: 0x0002C4B0
		private void RefreshStatNames()
		{
			for (int i = 0; i < this.SpectatorStats.Count; i++)
			{
				MPOverlayStatVM mpoverlayStatVM = this.SpectatorStats[i];
				mpoverlayStatVM.Header = MissionMultiplayerSpectatorHUDVM.GetStatName(mpoverlayStatVM.Id);
			}
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x0002E2EF File Offset: 0x0002C4EF
		private static string GetStatName(string statId)
		{
			return GameTexts.FindText("str_scoreboard_header", statId).ToString();
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x0002E304 File Offset: 0x0002C504
		public void Tick(float dt)
		{
			if (this._mission.MainAgent != null)
			{
				this.SpectatedPlayerNeutrality = -1;
			}
			this.IsSpectating = MultiplayerSpectatorHelper.IsLocalPeerSpectator();
			this.ShowBothTeamsData = MultiplayerSpectatorHelper.ShouldShowBothTeamsData();
			this.UpdateDynamicProperties();
			this.UpdatePeerStats();
			this.RefreshWeaponsIfChanged();
			this.RefreshEquippedWeaponSlot();
			this.RefreshPerksIfChanged();
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x0002E35C File Offset: 0x0002C55C
		private void UpdateDynamicProperties()
		{
			this.AgentHasShield = false;
			this.AgentHasMount = false;
			this.ShowAgentHealth = false;
			this.AgentHasRangedWeapon = false;
			if ((this.SpectatedPlayerNeutrality > 0 || this.IsSpectating) && this._spectatedAgent != null)
			{
				this.ShowAgentHealth = true;
				this.SpectatedPlayerHealthLimit = this._spectatedAgent.HealthLimit;
				this.SpectatedPlayerCurrentHealth = this._spectatedAgent.Health;
				this.AgentHasMount = this._spectatedAgent.MountAgent != null;
				if (this.AgentHasMount)
				{
					this.SpectatedPlayerMountCurrentHealth = this._spectatedAgent.MountAgent.Health;
					this.SpectatedPlayerMountHealthLimit = this._spectatedAgent.MountAgent.HealthLimit;
				}
				EquipmentIndex primaryWieldedItemIndex = this._spectatedAgent.GetPrimaryWieldedItemIndex();
				EquipmentIndex offhandWieldedItemIndex = this._spectatedAgent.GetOffhandWieldedItemIndex();
				int num = -1;
				if (primaryWieldedItemIndex != EquipmentIndex.None && this._spectatedAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem != null)
				{
					if (this._spectatedAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.IsRangedWeapon && this._spectatedAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.IsConsumable)
					{
						int ammoAmount = this._spectatedAgent.Equipment.GetAmmoAmount(primaryWieldedItemIndex);
						if (this._spectatedAgent.Equipment[primaryWieldedItemIndex].ModifiedMaxAmount == 1 || ammoAmount > 0)
						{
							num = ((this._spectatedAgent.Equipment[primaryWieldedItemIndex].ModifiedMaxAmount == 1) ? (-1) : ammoAmount);
						}
					}
					else if (this._spectatedAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.IsRangedWeapon)
					{
						bool flag = this._spectatedAgent.Equipment[primaryWieldedItemIndex].CurrentUsageItem.WeaponClass == WeaponClass.Crossbow;
						num = this._spectatedAgent.Equipment.GetAmmoAmount(primaryWieldedItemIndex) + (int)(flag ? this._spectatedAgent.Equipment[primaryWieldedItemIndex].Ammo : 0);
					}
				}
				if (offhandWieldedItemIndex != EquipmentIndex.None && this._spectatedAgent.Equipment[offhandWieldedItemIndex].CurrentUsageItem != null)
				{
					MissionWeapon missionWeapon = this._spectatedAgent.Equipment[offhandWieldedItemIndex];
					this.AgentHasShield = missionWeapon.CurrentUsageItem.IsShield;
					if (this.AgentHasShield)
					{
						this.SpectatedPlayerShieldHealthLimit = (float)missionWeapon.ModifiedMaxHitPoints;
						this.SpectatedPlayerShieldCurrentHealth = (float)missionWeapon.HitPoints;
					}
				}
				this.AgentHasRangedWeapon = num >= 0;
				this.SpectatedPlayerAmmoAmount = num;
			}
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x0002E5E8 File Offset: 0x0002C7E8
		private void ClearWeaponSlots()
		{
			for (int i = 0; i < this.SpectatedPlayerWeapons.Count; i++)
			{
				this.SpectatedPlayerWeapons[i].OnFinalize();
			}
			this.SpectatedPlayerWeapons.Clear();
			this.ShowAgentWeapons = false;
		}

		// Token: 0x06000EE8 RID: 3816 RVA: 0x0002E630 File Offset: 0x0002C830
		private void UpdatePeerStats()
		{
			bool flag = this._spectatedPeer != null;
			this.ShowAgentStats = flag;
			if (!flag)
			{
				this.ResetPeerStatCaches();
				this.SpectatorClanText = string.Empty;
				this.SpectatorLastKillText = string.Empty;
				this.SpectatorMostUsedWeaponText = string.Empty;
				return;
			}
			if (this._cachedKillCount != this._spectatedPeer.KillCount)
			{
				this._cachedKillCount = this._spectatedPeer.KillCount;
				this._killCountStat.Refresh(this._cachedKillCount.ToString());
			}
			if (this._cachedDeathCount != this._spectatedPeer.DeathCount)
			{
				this._cachedDeathCount = this._spectatedPeer.DeathCount;
				this._deathCountStat.Refresh(this._cachedDeathCount.ToString());
			}
			if (this._cachedAssistCount != this._spectatedPeer.AssistCount)
			{
				this._cachedAssistCount = this._spectatedPeer.AssistCount;
				this._assistCountStat.Refresh(this._cachedAssistCount.ToString());
			}
			string text = this._spectatedPeer.ClanName ?? string.Empty;
			if (this._cachedClanName != text)
			{
				this._cachedClanName = text;
				this.SpectatorClanText = text;
			}
			string text2 = this._spectatedPeer.LastKillVictimName ?? string.Empty;
			if (this._cachedLastKillVictimName != text2)
			{
				this._cachedLastKillVictimName = text2;
				if (!string.IsNullOrEmpty(text2))
				{
					TextObject textObject = new TextObject("{=jF9sZMB5}Last Kill: {VALUE}", null);
					textObject.SetTextVariable("VALUE", text2);
					this.SpectatorLastKillText = textObject.ToString();
				}
				else
				{
					this.SpectatorLastKillText = string.Empty;
				}
			}
			string text3 = this._spectatedPeer.MostUsedWeaponName ?? string.Empty;
			if (this._cachedMostUsedWeaponName != text3)
			{
				this._cachedMostUsedWeaponName = text3;
				if (!string.IsNullOrEmpty(text3))
				{
					TextObject textObject2 = new TextObject("{=YviubqFI}Top Weapon: {VALUE}", null);
					textObject2.SetTextVariable("VALUE", text3);
					this.SpectatorMostUsedWeaponText = textObject2.ToString();
				}
				else
				{
					this.SpectatorMostUsedWeaponText = string.Empty;
				}
			}
			this.UpdateGold();
		}

		// Token: 0x06000EE9 RID: 3817 RVA: 0x0002E82C File Offset: 0x0002CA2C
		private void UpdateGold()
		{
			if (this._goldStat == null || this._spectatedPeer == null)
			{
				this._cachedGold = -1;
				return;
			}
			MissionRepresentativeBase component = this._spectatedPeer.GetComponent<MissionRepresentativeBase>();
			if (component == null)
			{
				this._cachedGold = -1;
				this._goldStat.Refresh(string.Empty);
				return;
			}
			if (this._cachedGold != component.Gold)
			{
				this._cachedGold = component.Gold;
				this._goldStat.Refresh(this._cachedGold.ToString());
			}
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x0002E8A8 File Offset: 0x0002CAA8
		internal void OnSpectatedAgentFocusIn(Agent followedAgent)
		{
			this._spectatedAgent = followedAgent;
			int num = 0;
			MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			if (component != null && component.Team != this._mission.SpectatorTeam && component.Team == followedAgent.Team && this._isTeamsEnabled)
			{
				num = 1;
			}
			this.IsSpectating = MultiplayerSpectatorHelper.IsLocalPeerSpectator();
			this.ShowBothTeamsData = MultiplayerSpectatorHelper.ShouldShowBothTeamsData();
			this.SpectatedPlayerNeutrality = num;
			MissionPeer missionPeer = followedAgent.MissionPeer;
			this.SpectatedPlayerName = ((missionPeer != null) ? missionPeer.DisplayedName : null) ?? followedAgent.Name.ToString();
			this.CanTakeControlOfSpectatedAgent = this._isFlagDominationMode && ((component != null) ? component.ControlledFormation : null) != null && component.ControlledFormation == followedAgent.Formation;
			this.CompassElement = null;
			this.AgentHasCompassElement = false;
			this.SpectatedPlayerSigil = null;
			this.AgentHasSigil = false;
			MissionPeer missionPeer2;
			if ((missionPeer2 = followedAgent.MissionPeer) == null)
			{
				Formation formation = followedAgent.Formation;
				if (formation == null)
				{
					missionPeer2 = null;
				}
				else
				{
					Agent playerOwner = formation.PlayerOwner;
					missionPeer2 = ((playerOwner != null) ? playerOwner.MissionPeer : null);
				}
			}
			MissionPeer missionPeer3 = missionPeer2;
			this._spectatedPeer = missionPeer3;
			Team team = ((missionPeer3 != null) ? missionPeer3.Team : null);
			if (((missionPeer3 != null) ? missionPeer3.Peer : null) != null && team != null)
			{
				MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(missionPeer3, false);
				TargetIconType targetIconType = ((mpheroClassForPeer != null) ? mpheroClassForPeer.IconType : TargetIconType.None);
				Banner banner = new Banner(missionPeer3.Peer.BannerCode, team.Color, team.Color2);
				this.CompassElement = new MPTeammateCompassTargetVM(targetIconType, team.Color, team.Color2, banner, team.IsPlayerAlly);
				this.AgentHasCompassElement = true;
				this.SpectatedPlayerSigil = new BannerImageIdentifierVM(banner, true);
				this.AgentHasSigil = true;
				this.TeamColor = Color.FromUint(team.Color);
			}
			else
			{
				this.TeamColor = Color.White;
			}
			this.ResetPeerStatCaches();
			this.RefreshWeaponsIfChanged();
			this.RefreshPerksIfChanged();
			this.UpdatePeerStats();
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x0002EA84 File Offset: 0x0002CC84
		private void RefreshWeapons()
		{
			this.ClearWeaponSlots();
			if (!this.IsSpectating || this._spectatedAgent == null)
			{
				return;
			}
			MissionEquipment equipment = this._spectatedAgent.Equipment;
			if (equipment == null)
			{
				return;
			}
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex <= EquipmentIndex.Weapon3; equipmentIndex++)
			{
				ItemObject item = equipment[equipmentIndex].Item;
				if (item != null && item.PrimaryWeapon != null && !item.PrimaryWeapon.IsAmmo)
				{
					this.SpectatedPlayerWeapons.Add(new SpectatorWeaponSlotVM(item, equipmentIndex, equipment));
				}
			}
			this.ShowAgentWeapons = this.SpectatedPlayerWeapons.Count > 0;
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x0002EB14 File Offset: 0x0002CD14
		private void RefreshWeaponsIfChanged()
		{
			if (!this.IsSpectating || this._spectatedAgent == null)
			{
				if (this.SpectatedPlayerWeapons.Count > 0)
				{
					this.ClearWeaponSlots();
				}
				for (int i = 0; i < this._cachedWeaponItems.Length; i++)
				{
					this._cachedWeaponItems[i] = null;
				}
				return;
			}
			MissionEquipment equipment = this._spectatedAgent.Equipment;
			bool flag = false;
			int num = 0;
			EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot;
			while (equipmentIndex <= EquipmentIndex.Weapon3)
			{
				ItemObject itemObject = ((equipment == null) ? null : equipment[equipmentIndex].Item);
				if (this._cachedWeaponItems[num] != itemObject)
				{
					this._cachedWeaponItems[num] = itemObject;
					flag = true;
				}
				equipmentIndex++;
				num++;
			}
			if (flag)
			{
				this.RefreshWeapons();
			}
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x0002EBC0 File Offset: 0x0002CDC0
		private void RefreshPerksIfChanged()
		{
			if (this._spectatedPeer == null)
			{
				if (this.SpectatedPlayerPerks.Count > 0)
				{
					this.SpectatedPlayerPerks.Clear();
				}
				this.ShowAgentPerks = false;
				this._cachedPerkPeer = null;
				this._cachedPerkSelectedTroopIndex = -1;
				return;
			}
			if (this._cachedPerkPeer == this._spectatedPeer && this._cachedPerkSelectedTroopIndex == this._spectatedPeer.SelectedTroopIndex)
			{
				return;
			}
			this._cachedPerkPeer = this._spectatedPeer;
			this._cachedPerkSelectedTroopIndex = this._spectatedPeer.SelectedTroopIndex;
			this.SpectatedPlayerPerks.Clear();
			MultiplayerClassDivisions.MPHeroClass mpheroClassForPeer = MultiplayerClassDivisions.GetMPHeroClassForPeer(this._spectatedPeer, false);
			if (this._spectatedPeer.Culture != null && mpheroClassForPeer != null)
			{
				foreach (MPPerkObject mpperkObject in this._spectatedPeer.SelectedPerks)
				{
					this.SpectatedPlayerPerks.Add(new MPPerkVM(null, mpperkObject, false, 0));
				}
			}
			this.ShowAgentPerks = this.SpectatedPlayerPerks.Count > 0;
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x0002ECD8 File Offset: 0x0002CED8
		public override void OnFinalize()
		{
			base.OnFinalize();
			for (int i = 0; i < this.SpectatorStats.Count; i++)
			{
				this.SpectatorStats[i].OnFinalize();
			}
			this.ClearWeaponSlots();
			for (int j = 0; j < this.SpectatedPlayerPerks.Count; j++)
			{
				this.SpectatedPlayerPerks[j].OnFinalize();
			}
			InputKeyItemVM cyclePreviousKey = this.CyclePreviousKey;
			if (cyclePreviousKey != null)
			{
				cyclePreviousKey.OnFinalize();
			}
			InputKeyItemVM cycleNextKey = this.CycleNextKey;
			if (cycleNextKey == null)
			{
				return;
			}
			cycleNextKey.OnFinalize();
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0002ED60 File Offset: 0x0002CF60
		internal void OnSpectatedAgentFocusOut(Agent followedPeer)
		{
			this._spectatedAgent = null;
			this._spectatedPeer = null;
			this.ShowAgentStats = false;
			this.ClearWeaponSlots();
			this.ShowAgentPerks = false;
			this.SpectatedPlayerPerks.Clear();
			this._cachedPerkPeer = null;
			this._cachedPerkSelectedTroopIndex = -1;
			this.ResetPeerStatCaches();
			this.SpectatorClanText = string.Empty;
			this.SpectatorLastKillText = string.Empty;
			this.SpectatorMostUsedWeaponText = string.Empty;
			this.TeamColor = Color.White;
			this.SpectatedPlayerNeutrality = -1;
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x0002EDE4 File Offset: 0x0002CFE4
		private void ResetPeerStatCaches()
		{
			this._cachedKillCount = -1;
			this._cachedDeathCount = -1;
			this._cachedAssistCount = -1;
			this._cachedGold = -1;
			this._cachedClanName = null;
			this._cachedLastKillVictimName = null;
			this._cachedMostUsedWeaponName = null;
			for (int i = 0; i < this.SpectatorStats.Count; i++)
			{
				this.SpectatorStats[i].Refresh(string.Empty);
			}
		}

		// Token: 0x170004E2 RID: 1250
		// (get) Token: 0x06000EF1 RID: 3825 RVA: 0x0002EE4E File Offset: 0x0002D04E
		// (set) Token: 0x06000EF2 RID: 3826 RVA: 0x0002EE56 File Offset: 0x0002D056
		[DataSourceProperty]
		public InputKeyItemVM CyclePreviousKey
		{
			get
			{
				return this._cyclePreviousKey;
			}
			set
			{
				if (value != this._cyclePreviousKey)
				{
					this._cyclePreviousKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CyclePreviousKey");
				}
			}
		}

		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06000EF3 RID: 3827 RVA: 0x0002EE74 File Offset: 0x0002D074
		// (set) Token: 0x06000EF4 RID: 3828 RVA: 0x0002EE7C File Offset: 0x0002D07C
		[DataSourceProperty]
		public InputKeyItemVM CycleNextKey
		{
			get
			{
				return this._cycleNextKey;
			}
			set
			{
				if (value != this._cycleNextKey)
				{
					this._cycleNextKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CycleNextKey");
				}
			}
		}

		// Token: 0x170004E4 RID: 1252
		// (get) Token: 0x06000EF5 RID: 3829 RVA: 0x0002EE9A File Offset: 0x0002D09A
		// (set) Token: 0x06000EF6 RID: 3830 RVA: 0x0002EEA2 File Offset: 0x0002D0A2
		[DataSourceProperty]
		public Color TeamColor
		{
			get
			{
				return this._teamColor;
			}
			set
			{
				if (value != this._teamColor)
				{
					this._teamColor = value;
					base.OnPropertyChangedWithValue(value, "TeamColor");
				}
			}
		}

		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x06000EF7 RID: 3831 RVA: 0x0002EEC5 File Offset: 0x0002D0C5
		// (set) Token: 0x06000EF8 RID: 3832 RVA: 0x0002EECD File Offset: 0x0002D0CD
		[DataSourceProperty]
		public int SpectatedPlayerNeutrality
		{
			get
			{
				return this._spectatedPlayerNeutrality;
			}
			set
			{
				if (value != this._spectatedPlayerNeutrality)
				{
					this._spectatedPlayerNeutrality = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerNeutrality");
					this.IsSpectatingAgent = value >= 0;
				}
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x06000EF9 RID: 3833 RVA: 0x0002EEF8 File Offset: 0x0002D0F8
		// (set) Token: 0x06000EFA RID: 3834 RVA: 0x0002EF00 File Offset: 0x0002D100
		[DataSourceProperty]
		public MPTeammateCompassTargetVM CompassElement
		{
			get
			{
				return this._compassElement;
			}
			set
			{
				if (value != this._compassElement)
				{
					this._compassElement = value;
					base.OnPropertyChangedWithValue<MPTeammateCompassTargetVM>(value, "CompassElement");
				}
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x06000EFB RID: 3835 RVA: 0x0002EF1E File Offset: 0x0002D11E
		// (set) Token: 0x06000EFC RID: 3836 RVA: 0x0002EF26 File Offset: 0x0002D126
		[DataSourceProperty]
		public BannerImageIdentifierVM SpectatedPlayerSigil
		{
			get
			{
				return this._spectatedPlayerSigil;
			}
			set
			{
				if (value != this._spectatedPlayerSigil)
				{
					this._spectatedPlayerSigil = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "SpectatedPlayerSigil");
				}
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x06000EFD RID: 3837 RVA: 0x0002EF44 File Offset: 0x0002D144
		// (set) Token: 0x06000EFE RID: 3838 RVA: 0x0002EF4C File Offset: 0x0002D14C
		[DataSourceProperty]
		public bool AgentHasSigil
		{
			get
			{
				return this._agentHasSigil;
			}
			set
			{
				if (value != this._agentHasSigil)
				{
					this._agentHasSigil = value;
					base.OnPropertyChangedWithValue(value, "AgentHasSigil");
				}
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x0002EF6A File Offset: 0x0002D16A
		// (set) Token: 0x06000F00 RID: 3840 RVA: 0x0002EF72 File Offset: 0x0002D172
		[DataSourceProperty]
		public bool IsSpectatingAgent
		{
			get
			{
				return this._isSpectatingPlayer;
			}
			set
			{
				if (value != this._isSpectatingPlayer)
				{
					this._isSpectatingPlayer = value;
					base.OnPropertyChangedWithValue(value, "IsSpectatingAgent");
				}
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06000F01 RID: 3841 RVA: 0x0002EF90 File Offset: 0x0002D190
		// (set) Token: 0x06000F02 RID: 3842 RVA: 0x0002EF98 File Offset: 0x0002D198
		[DataSourceProperty]
		public bool AgentHasCompassElement
		{
			get
			{
				return this._agentHasCompassElement;
			}
			set
			{
				if (value != this._agentHasCompassElement)
				{
					this._agentHasCompassElement = value;
					base.OnPropertyChangedWithValue(value, "AgentHasCompassElement");
				}
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06000F03 RID: 3843 RVA: 0x0002EFB6 File Offset: 0x0002D1B6
		// (set) Token: 0x06000F04 RID: 3844 RVA: 0x0002EFBE File Offset: 0x0002D1BE
		[DataSourceProperty]
		public bool AgentHasMount
		{
			get
			{
				return this._agentHasMount;
			}
			set
			{
				if (value != this._agentHasMount)
				{
					this._agentHasMount = value;
					base.OnPropertyChangedWithValue(value, "AgentHasMount");
				}
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06000F05 RID: 3845 RVA: 0x0002EFDC File Offset: 0x0002D1DC
		// (set) Token: 0x06000F06 RID: 3846 RVA: 0x0002EFE4 File Offset: 0x0002D1E4
		[DataSourceProperty]
		public bool ShowAgentHealth
		{
			get
			{
				return this._showAgentHealth;
			}
			set
			{
				if (value != this._showAgentHealth)
				{
					this._showAgentHealth = value;
					base.OnPropertyChangedWithValue(value, "ShowAgentHealth");
				}
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06000F07 RID: 3847 RVA: 0x0002F002 File Offset: 0x0002D202
		// (set) Token: 0x06000F08 RID: 3848 RVA: 0x0002F00A File Offset: 0x0002D20A
		[DataSourceProperty]
		public bool AgentHasRangedWeapon
		{
			get
			{
				return this._agentHasRangedWeapon;
			}
			set
			{
				if (value != this._agentHasRangedWeapon)
				{
					this._agentHasRangedWeapon = value;
					base.OnPropertyChangedWithValue(value, "AgentHasRangedWeapon");
				}
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06000F09 RID: 3849 RVA: 0x0002F028 File Offset: 0x0002D228
		// (set) Token: 0x06000F0A RID: 3850 RVA: 0x0002F030 File Offset: 0x0002D230
		[DataSourceProperty]
		public bool AgentHasShield
		{
			get
			{
				return this._agentHasShield;
			}
			set
			{
				if (value != this._agentHasShield)
				{
					this._agentHasShield = value;
					base.OnPropertyChangedWithValue(value, "AgentHasShield");
				}
			}
		}

		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x0002F04E File Offset: 0x0002D24E
		// (set) Token: 0x06000F0C RID: 3852 RVA: 0x0002F056 File Offset: 0x0002D256
		[DataSourceProperty]
		public bool CanTakeControlOfSpectatedAgent
		{
			get
			{
				return this._canTakeControlOfSpectatedAgent;
			}
			set
			{
				if (value != this._canTakeControlOfSpectatedAgent)
				{
					this._canTakeControlOfSpectatedAgent = value;
					base.OnPropertyChangedWithValue(value, "CanTakeControlOfSpectatedAgent");
				}
			}
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x0002F074 File Offset: 0x0002D274
		// (set) Token: 0x06000F0E RID: 3854 RVA: 0x0002F07C File Offset: 0x0002D27C
		[DataSourceProperty]
		public string SpectatedPlayerName
		{
			get
			{
				return this._spectatedPlayerName;
			}
			set
			{
				if (value != this._spectatedPlayerName)
				{
					this._spectatedPlayerName = value;
					base.OnPropertyChangedWithValue<string>(value, "SpectatedPlayerName");
				}
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06000F0F RID: 3855 RVA: 0x0002F09F File Offset: 0x0002D29F
		// (set) Token: 0x06000F10 RID: 3856 RVA: 0x0002F0A7 File Offset: 0x0002D2A7
		[DataSourceProperty]
		public string TakeControlText
		{
			get
			{
				return this._takeControlText;
			}
			set
			{
				if (value != this._takeControlText)
				{
					this._takeControlText = value;
					base.OnPropertyChangedWithValue<string>(value, "TakeControlText");
				}
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x0002F0CA File Offset: 0x0002D2CA
		// (set) Token: 0x06000F12 RID: 3858 RVA: 0x0002F0D2 File Offset: 0x0002D2D2
		[DataSourceProperty]
		public float SpectatedPlayerHealthLimit
		{
			get
			{
				return this._spectatedPlayerHealthLimit;
			}
			set
			{
				if (value != this._spectatedPlayerHealthLimit)
				{
					this._spectatedPlayerHealthLimit = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerHealthLimit");
				}
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06000F13 RID: 3859 RVA: 0x0002F0F0 File Offset: 0x0002D2F0
		// (set) Token: 0x06000F14 RID: 3860 RVA: 0x0002F0F8 File Offset: 0x0002D2F8
		[DataSourceProperty]
		public float SpectatedPlayerCurrentHealth
		{
			get
			{
				return this._spectatedPlayerCurrentHealth;
			}
			set
			{
				if (value != this._spectatedPlayerCurrentHealth)
				{
					this._spectatedPlayerCurrentHealth = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerCurrentHealth");
				}
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x0002F116 File Offset: 0x0002D316
		// (set) Token: 0x06000F16 RID: 3862 RVA: 0x0002F11E File Offset: 0x0002D31E
		[DataSourceProperty]
		public float SpectatedPlayerMountCurrentHealth
		{
			get
			{
				return this._spectatedPlayerMountCurrentHealth;
			}
			set
			{
				if (value != this._spectatedPlayerMountCurrentHealth)
				{
					this._spectatedPlayerMountCurrentHealth = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerMountCurrentHealth");
				}
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x0002F13C File Offset: 0x0002D33C
		// (set) Token: 0x06000F18 RID: 3864 RVA: 0x0002F144 File Offset: 0x0002D344
		[DataSourceProperty]
		public float SpectatedPlayerMountHealthLimit
		{
			get
			{
				return this._spectatedPlayerMountHealthLimit;
			}
			set
			{
				if (value != this._spectatedPlayerMountHealthLimit)
				{
					this._spectatedPlayerMountHealthLimit = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerMountHealthLimit");
				}
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x0002F162 File Offset: 0x0002D362
		// (set) Token: 0x06000F1A RID: 3866 RVA: 0x0002F16A File Offset: 0x0002D36A
		[DataSourceProperty]
		public float SpectatedPlayerShieldCurrentHealth
		{
			get
			{
				return this._spectatedPlayerShieldCurrentHealth;
			}
			set
			{
				if (value != this._spectatedPlayerShieldCurrentHealth)
				{
					this._spectatedPlayerShieldCurrentHealth = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerShieldCurrentHealth");
				}
			}
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06000F1B RID: 3867 RVA: 0x0002F188 File Offset: 0x0002D388
		// (set) Token: 0x06000F1C RID: 3868 RVA: 0x0002F190 File Offset: 0x0002D390
		[DataSourceProperty]
		public float SpectatedPlayerShieldHealthLimit
		{
			get
			{
				return this._spectatedPlayerShieldHealthLimit;
			}
			set
			{
				if (value != this._spectatedPlayerShieldHealthLimit)
				{
					this._spectatedPlayerShieldHealthLimit = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerShieldHealthLimit");
				}
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06000F1D RID: 3869 RVA: 0x0002F1AE File Offset: 0x0002D3AE
		// (set) Token: 0x06000F1E RID: 3870 RVA: 0x0002F1B6 File Offset: 0x0002D3B6
		[DataSourceProperty]
		public int SpectatedPlayerAmmoAmount
		{
			get
			{
				return this._spectatedPlayerAmmoAmount;
			}
			set
			{
				if (value != this._spectatedPlayerAmmoAmount)
				{
					this._spectatedPlayerAmmoAmount = value;
					base.OnPropertyChangedWithValue(value, "SpectatedPlayerAmmoAmount");
				}
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06000F1F RID: 3871 RVA: 0x0002F1D4 File Offset: 0x0002D3D4
		// (set) Token: 0x06000F20 RID: 3872 RVA: 0x0002F1DC File Offset: 0x0002D3DC
		public bool ShowBothTeamsData
		{
			get
			{
				return this._showBothTeamsData;
			}
			set
			{
				this._showBothTeamsData = value;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06000F21 RID: 3873 RVA: 0x0002F1E5 File Offset: 0x0002D3E5
		// (set) Token: 0x06000F22 RID: 3874 RVA: 0x0002F1ED File Offset: 0x0002D3ED
		public bool IsSpectating
		{
			get
			{
				return this._isSpectating;
			}
			set
			{
				this._isSpectating = value;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06000F23 RID: 3875 RVA: 0x0002F1F6 File Offset: 0x0002D3F6
		// (set) Token: 0x06000F24 RID: 3876 RVA: 0x0002F1FE File Offset: 0x0002D3FE
		[DataSourceProperty]
		public bool ShowAgentStats
		{
			get
			{
				return this._showAgentStats;
			}
			set
			{
				if (value != this._showAgentStats)
				{
					this._showAgentStats = value;
					base.OnPropertyChangedWithValue(value, "ShowAgentStats");
				}
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06000F25 RID: 3877 RVA: 0x0002F21C File Offset: 0x0002D41C
		// (set) Token: 0x06000F26 RID: 3878 RVA: 0x0002F224 File Offset: 0x0002D424
		[DataSourceProperty]
		public MBBindingList<MPOverlayStatVM> SpectatorStats
		{
			get
			{
				return this._spectatorStats;
			}
			set
			{
				if (value != this._spectatorStats)
				{
					this._spectatorStats = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPOverlayStatVM>>(value, "SpectatorStats");
				}
			}
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06000F27 RID: 3879 RVA: 0x0002F242 File Offset: 0x0002D442
		// (set) Token: 0x06000F28 RID: 3880 RVA: 0x0002F24A File Offset: 0x0002D44A
		[DataSourceProperty]
		public string SpectatorClanText
		{
			get
			{
				return this._spectatorClanText;
			}
			set
			{
				if (value != this._spectatorClanText)
				{
					this._spectatorClanText = value;
					base.OnPropertyChangedWithValue<string>(value, "SpectatorClanText");
				}
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06000F29 RID: 3881 RVA: 0x0002F26D File Offset: 0x0002D46D
		// (set) Token: 0x06000F2A RID: 3882 RVA: 0x0002F275 File Offset: 0x0002D475
		[DataSourceProperty]
		public string SpectatorLastKillText
		{
			get
			{
				return this._spectatorLastKillText;
			}
			set
			{
				if (value != this._spectatorLastKillText)
				{
					this._spectatorLastKillText = value;
					base.OnPropertyChangedWithValue<string>(value, "SpectatorLastKillText");
				}
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06000F2B RID: 3883 RVA: 0x0002F298 File Offset: 0x0002D498
		// (set) Token: 0x06000F2C RID: 3884 RVA: 0x0002F2A0 File Offset: 0x0002D4A0
		[DataSourceProperty]
		public string SpectatorMostUsedWeaponText
		{
			get
			{
				return this._spectatorMostUsedWeaponText;
			}
			set
			{
				if (value != this._spectatorMostUsedWeaponText)
				{
					this._spectatorMostUsedWeaponText = value;
					base.OnPropertyChangedWithValue<string>(value, "SpectatorMostUsedWeaponText");
				}
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06000F2D RID: 3885 RVA: 0x0002F2C3 File Offset: 0x0002D4C3
		// (set) Token: 0x06000F2E RID: 3886 RVA: 0x0002F2CB File Offset: 0x0002D4CB
		[DataSourceProperty]
		public bool ShowAgentWeapons
		{
			get
			{
				return this._showAgentWeapons;
			}
			set
			{
				if (value != this._showAgentWeapons)
				{
					this._showAgentWeapons = value;
					base.OnPropertyChangedWithValue(value, "ShowAgentWeapons");
				}
			}
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06000F2F RID: 3887 RVA: 0x0002F2E9 File Offset: 0x0002D4E9
		// (set) Token: 0x06000F30 RID: 3888 RVA: 0x0002F2F1 File Offset: 0x0002D4F1
		[DataSourceProperty]
		public MBBindingList<SpectatorWeaponSlotVM> SpectatedPlayerWeapons
		{
			get
			{
				return this._spectatedPlayerWeapons;
			}
			set
			{
				if (value != this._spectatedPlayerWeapons)
				{
					this._spectatedPlayerWeapons = value;
					base.OnPropertyChangedWithValue<MBBindingList<SpectatorWeaponSlotVM>>(value, "SpectatedPlayerWeapons");
				}
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06000F31 RID: 3889 RVA: 0x0002F30F File Offset: 0x0002D50F
		// (set) Token: 0x06000F32 RID: 3890 RVA: 0x0002F317 File Offset: 0x0002D517
		[DataSourceProperty]
		public bool ShowAgentPerks
		{
			get
			{
				return this._showAgentPerks;
			}
			set
			{
				if (value != this._showAgentPerks)
				{
					this._showAgentPerks = value;
					base.OnPropertyChangedWithValue(value, "ShowAgentPerks");
				}
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x06000F33 RID: 3891 RVA: 0x0002F335 File Offset: 0x0002D535
		// (set) Token: 0x06000F34 RID: 3892 RVA: 0x0002F33D File Offset: 0x0002D53D
		[DataSourceProperty]
		public MBBindingList<MPPerkVM> SpectatedPlayerPerks
		{
			get
			{
				return this._spectatedPlayerPerks;
			}
			set
			{
				if (value != this._spectatedPlayerPerks)
				{
					this._spectatedPlayerPerks = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPerkVM>>(value, "SpectatedPlayerPerks");
				}
			}
		}

		// Token: 0x040006D5 RID: 1749
		private const string KillStatId = "kill";

		// Token: 0x040006D6 RID: 1750
		private const string DeathStatId = "death";

		// Token: 0x040006D7 RID: 1751
		private const string AssistStatId = "assist";

		// Token: 0x040006D8 RID: 1752
		private const string GoldStatId = "gold";

		// Token: 0x040006D9 RID: 1753
		private readonly Mission _mission;

		// Token: 0x040006DA RID: 1754
		private readonly bool _isTeamsEnabled;

		// Token: 0x040006DB RID: 1755
		private readonly bool _isFlagDominationMode;

		// Token: 0x040006DC RID: 1756
		private Agent _spectatedAgent;

		// Token: 0x040006DD RID: 1757
		private MissionPeer _spectatedPeer;

		// Token: 0x040006DE RID: 1758
		private int _cachedKillCount = -1;

		// Token: 0x040006DF RID: 1759
		private int _cachedDeathCount = -1;

		// Token: 0x040006E0 RID: 1760
		private int _cachedAssistCount = -1;

		// Token: 0x040006E1 RID: 1761
		private int _cachedGold = -1;

		// Token: 0x040006E2 RID: 1762
		private string _cachedClanName;

		// Token: 0x040006E3 RID: 1763
		private string _cachedLastKillVictimName;

		// Token: 0x040006E4 RID: 1764
		private string _cachedMostUsedWeaponName;

		// Token: 0x040006E5 RID: 1765
		private readonly ItemObject[] _cachedWeaponItems = new ItemObject[4];

		// Token: 0x040006E6 RID: 1766
		private int _cachedPerkSelectedTroopIndex = -1;

		// Token: 0x040006E7 RID: 1767
		private MissionPeer _cachedPerkPeer;

		// Token: 0x040006E8 RID: 1768
		private readonly MissionMultiplayerGameModeBaseClient _gameModeClient;

		// Token: 0x040006E9 RID: 1769
		private readonly bool _isGameModeUsingGold;

		// Token: 0x040006EA RID: 1770
		private MPOverlayStatVM _killCountStat;

		// Token: 0x040006EB RID: 1771
		private MPOverlayStatVM _deathCountStat;

		// Token: 0x040006EC RID: 1772
		private MPOverlayStatVM _assistCountStat;

		// Token: 0x040006ED RID: 1773
		private MPOverlayStatVM _goldStat;

		// Token: 0x040006EE RID: 1774
		private string _spectatedPlayerName;

		// Token: 0x040006EF RID: 1775
		private string _takeControlText;

		// Token: 0x040006F0 RID: 1776
		private int _spectatedPlayerNeutrality = -1;

		// Token: 0x040006F1 RID: 1777
		private bool _isSpectatingPlayer;

		// Token: 0x040006F2 RID: 1778
		private bool _canTakeControlOfSpectatedAgent;

		// Token: 0x040006F3 RID: 1779
		private bool _agentHasMount;

		// Token: 0x040006F4 RID: 1780
		private bool _agentHasShield;

		// Token: 0x040006F5 RID: 1781
		private bool _showAgentHealth;

		// Token: 0x040006F6 RID: 1782
		private Color _teamColor = Color.White;

		// Token: 0x040006F7 RID: 1783
		private InputKeyItemVM _cyclePreviousKey;

		// Token: 0x040006F8 RID: 1784
		private InputKeyItemVM _cycleNextKey;

		// Token: 0x040006F9 RID: 1785
		private bool _agentHasRangedWeapon;

		// Token: 0x040006FA RID: 1786
		private bool _agentHasCompassElement;

		// Token: 0x040006FB RID: 1787
		private BannerImageIdentifierVM _spectatedPlayerSigil;

		// Token: 0x040006FC RID: 1788
		private bool _agentHasSigil;

		// Token: 0x040006FD RID: 1789
		private bool _showBothTeamsData;

		// Token: 0x040006FE RID: 1790
		private bool _isSpectating;

		// Token: 0x040006FF RID: 1791
		private bool _showAgentStats;

		// Token: 0x04000700 RID: 1792
		private MBBindingList<MPOverlayStatVM> _spectatorStats;

		// Token: 0x04000701 RID: 1793
		private string _spectatorClanText;

		// Token: 0x04000702 RID: 1794
		private string _spectatorLastKillText;

		// Token: 0x04000703 RID: 1795
		private string _spectatorMostUsedWeaponText;

		// Token: 0x04000704 RID: 1796
		private bool _showAgentWeapons;

		// Token: 0x04000705 RID: 1797
		private MBBindingList<SpectatorWeaponSlotVM> _spectatedPlayerWeapons;

		// Token: 0x04000706 RID: 1798
		private MBBindingList<MPPerkVM> _spectatedPlayerPerks;

		// Token: 0x04000707 RID: 1799
		private bool _showAgentPerks;

		// Token: 0x04000708 RID: 1800
		private float _spectatedPlayerHealthLimit;

		// Token: 0x04000709 RID: 1801
		private float _spectatedPlayerCurrentHealth;

		// Token: 0x0400070A RID: 1802
		private float _spectatedPlayerMountCurrentHealth;

		// Token: 0x0400070B RID: 1803
		private float _spectatedPlayerMountHealthLimit;

		// Token: 0x0400070C RID: 1804
		private float _spectatedPlayerShieldCurrentHealth;

		// Token: 0x0400070D RID: 1805
		private float _spectatedPlayerShieldHealthLimit;

		// Token: 0x0400070E RID: 1806
		private int _spectatedPlayerAmmoAmount;

		// Token: 0x0400070F RID: 1807
		private MPTeammateCompassTargetVM _compassElement;
	}
}
