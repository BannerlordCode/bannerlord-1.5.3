using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.ClassLoadout
{
	// Token: 0x020000AD RID: 173
	public class MultiplayerClassLoadoutVM : ViewModel
	{
		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x000336AE File Offset: 0x000318AE
		private MissionRepresentativeBase missionRep
		{
			get
			{
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				if (myPeer == null)
				{
					return null;
				}
				VirtualPlayer virtualPlayer = myPeer.VirtualPlayer;
				if (virtualPlayer == null)
				{
					return null;
				}
				return virtualPlayer.GetComponent<MissionRepresentativeBase>();
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x0600108D RID: 4237 RVA: 0x000336CC File Offset: 0x000318CC
		private Team _playerTeam
		{
			get
			{
				if (!GameNetwork.IsMyPeerReady)
				{
					return null;
				}
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				if (component.Team == null || component.Team.Side == BattleSideEnum.None)
				{
					return null;
				}
				return component.Team;
			}
		}

		// Token: 0x0600108E RID: 4238 RVA: 0x0003370C File Offset: 0x0003190C
		public MultiplayerClassLoadoutVM(MissionMultiplayerGameModeBaseClient gameMode, Action<MultiplayerClassDivisions.MPHeroClass> onRefreshSelection, MultiplayerClassDivisions.MPHeroClass initialHeroSelection)
		{
			MBTextManager.SetTextVariable("newline", "\n", false);
			this._isInitializing = true;
			this._onRefreshSelection = onRefreshSelection;
			this._missionMultiplayerGameMode = gameMode;
			this._mission = gameMode.Mission;
			Team team = GameNetwork.MyPeer.GetComponent<MissionPeer>().Team;
			this.Classes = new MBBindingList<HeroClassGroupVM>();
			this.HeroInformation = new HeroInformationVM();
			this._enemyDictionary = new Dictionary<MissionPeer, MPPlayerVM>();
			this._missionLobbyEquipmentNetworkComponent = Mission.Current.GetMissionBehavior<MissionLobbyEquipmentNetworkComponent>();
			this.IsGoldEnabled = this._missionMultiplayerGameMode.IsGameModeUsingGold;
			if (this.IsGoldEnabled)
			{
				this.Gold = this._missionMultiplayerGameMode.GetGoldAmount();
			}
			MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors.MultiplayerCultureColorInfo peerColors = MultiplayerBattleColors.CreateWith(@object, object2).GetPeerColors(component);
			HeroClassVM heroClassVM = null;
			foreach (MultiplayerClassDivisions.MPHeroClassGroup mpheroClassGroup in MultiplayerClassDivisions.MultiplayerHeroClassGroups)
			{
				HeroClassGroupVM heroClassGroupVM = new HeroClassGroupVM(new Action<HeroClassVM>(this.RefreshCharacter), new Action<HeroPerkVM, MPPerkVM>(this.OnSelectPerk), mpheroClassGroup, peerColors);
				if (heroClassGroupVM.IsValid)
				{
					this.Classes.Add(heroClassGroupVM);
				}
			}
			int num = ((initialHeroSelection != null) ? ((!gameMode.IsGameModeUsingCasualGold) ? ((gameMode.GameType == MultiplayerGameType.Battle) ? initialHeroSelection.TroopBattleCost : initialHeroSelection.TroopCost) : initialHeroSelection.TroopCasualCost) : 0);
			if (initialHeroSelection == null || (this.IsGoldEnabled && num > this.Gold))
			{
				HeroClassGroupVM heroClassGroupVM2 = this.Classes.FirstOrDefault<HeroClassGroupVM>();
				heroClassVM = ((heroClassGroupVM2 != null) ? heroClassGroupVM2.SubClasses.FirstOrDefault<HeroClassVM>() : null);
			}
			else
			{
				foreach (HeroClassGroupVM heroClassGroupVM3 in this.Classes)
				{
					foreach (HeroClassVM heroClassVM2 in heroClassGroupVM3.SubClasses)
					{
						if (heroClassVM2.HeroClass == initialHeroSelection)
						{
							heroClassVM = heroClassVM2;
							break;
						}
					}
					if (heroClassVM != null)
					{
						break;
					}
				}
				if (heroClassVM == null)
				{
					HeroClassGroupVM heroClassGroupVM4 = this.Classes.FirstOrDefault<HeroClassGroupVM>();
					heroClassVM = ((heroClassGroupVM4 != null) ? heroClassGroupVM4.SubClasses.FirstOrDefault<HeroClassVM>() : null);
				}
			}
			this._isInitializing = false;
			this.RefreshCharacter(heroClassVM);
			this._teammateDictionary = new Dictionary<MissionPeer, MPPlayerVM>();
			this.Teammates = new MBBindingList<MPPlayerVM>();
			this.Enemies = new MBBindingList<MPPlayerVM>();
			MissionPeer.OnEquipmentIndexRefreshed += this.RefreshPeerDivision;
			MissionPeer.OnPerkSelectionUpdated += this.RefreshPeerPerkSelection;
			NetworkCommunicator.OnPeerComponentAdded += this.OnPeerComponentAdded;
			BasicCultureObject culture = component.Culture;
			this.CultureId = culture.StringId;
			this.CultureColor1 = peerColors.Color1;
			this.CultureColor2 = peerColors.Color2;
			if (Mission.Current.HasMissionBehavior<MissionMultiplayerSiegeClient>())
			{
				this.ShowAttackerOrDefenderIcons = true;
				this.IsAttacker = team.Side == BattleSideEnum.Attacker;
			}
			this.RefreshValues();
			this._isTeammateAndEnemiesRelevant = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>().IsGameModeTactical && !Mission.Current.HasMissionBehavior<MissionMultiplayerSiegeClient>() && Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>().GameType != MultiplayerGameType.Battle;
			if (this._isTeammateAndEnemiesRelevant)
			{
				this.OnRefreshTeamMembers();
				this.OnRefreshEnemyMembers();
			}
		}

		// Token: 0x0600108F RID: 4239 RVA: 0x00033A94 File Offset: 0x00031C94
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.UpdateSpawnAndTimerLabels();
			string strValue = MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			TextObject textObject = new TextObject("{=XJTX8w8M}Warmup Phase - {GAME_MODE}{newline}Waiting for players to join", null);
			textObject.SetTextVariable("GAME_MODE", GameTexts.FindText("str_multiplayer_official_game_type_name", strValue));
			this.WarmupInfoText = textObject.ToString();
			BasicCultureObject culture = GameNetwork.MyPeer.GetComponent<MissionPeer>().Culture;
			this.Culture = culture.Name.ToString();
			this.Classes.ApplyActionOnAllItems(delegate(HeroClassGroupVM x)
			{
				x.RefreshValues();
			});
			this.CurrentSelectedClass.RefreshValues();
			this.HeroInformation.RefreshValues();
		}

		// Token: 0x06001090 RID: 4240 RVA: 0x00033B48 File Offset: 0x00031D48
		private void UpdateSpawnAndTimerLabels()
		{
			string keyHyperlinkText = HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f);
			GameTexts.SetVariable("USE_KEY", keyHyperlinkText);
			this.SpawnLabelText = GameTexts.FindText("str_skirmish_battle_press_action_to_spawn", null).ToString();
			if (this._missionMultiplayerGameMode.RoundComponent != null)
			{
				if (!this._missionMultiplayerGameMode.IsInWarmup && !this._missionMultiplayerGameMode.IsRoundInProgress)
				{
					this.IsSpawnTimerVisible = true;
					return;
				}
				this.IsSpawnTimerVisible = false;
				this.IsSpawnLabelVisible = true;
				if (this._missionMultiplayerGameMode.IsRoundInProgress && (this._missionMultiplayerGameMode is MissionMultiplayerGameModeFlagDominationClient && this._missionMultiplayerGameMode.GameType == MultiplayerGameType.Skirmish) && GameNetwork.MyPeer.GetComponent<MissionPeer>() != null)
				{
					this.IsSpawnForfeitLabelVisible = true;
					string keyHyperlinkText2 = HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", "ForfeitSpawn"), 1f);
					GameTexts.SetVariable("ALT_WEAP_KEY", keyHyperlinkText2);
					this.SpawnForfeitLabelText = GameTexts.FindText("str_skirmish_battle_press_alternative_to_forfeit_spawning", null).ToString();
					return;
				}
			}
			else
			{
				this.IsSpawnTimerVisible = false;
				this.IsSpawnLabelVisible = true;
			}
		}

		// Token: 0x06001091 RID: 4241 RVA: 0x00033C5D File Offset: 0x00031E5D
		public override void OnFinalize()
		{
			base.OnFinalize();
			MissionPeer.OnEquipmentIndexRefreshed -= this.RefreshPeerDivision;
			MissionPeer.OnPerkSelectionUpdated -= this.RefreshPeerPerkSelection;
			NetworkCommunicator.OnPeerComponentAdded -= this.OnPeerComponentAdded;
		}

		// Token: 0x06001092 RID: 4242 RVA: 0x00033C98 File Offset: 0x00031E98
		private void RefreshCharacter(HeroClassVM heroClass)
		{
			if (this._isInitializing)
			{
				return;
			}
			foreach (HeroClassGroupVM heroClassGroupVM in this.Classes)
			{
				foreach (HeroClassVM heroClassVM in heroClassGroupVM.SubClasses)
				{
					heroClassVM.IsSelected = false;
				}
			}
			heroClass.IsSelected = true;
			this.CurrentSelectedClass = heroClass;
			if (GameNetwork.IsMyPeerReady)
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				int num = MultiplayerClassDivisions.GetMPHeroClasses(heroClass.HeroClass.Culture).ToList<MultiplayerClassDivisions.MPHeroClass>().IndexOf(heroClass.HeroClass);
				component.NextSelectedTroopIndex = num;
			}
			this.HeroInformation.RefreshWith(heroClass.HeroClass, heroClass.SelectedPerks);
			this._missionLobbyEquipmentNetworkComponent.EquipmentUpdated();
			if (this._missionMultiplayerGameMode.IsGameModeUsingGold)
			{
				this.Gold = this._missionMultiplayerGameMode.GetGoldAmount();
			}
			List<IReadOnlyPerkObject> list = heroClass.Perks.Select<HeroPerkVM, IReadOnlyPerkObject>((HeroPerkVM x) => x.SelectedPerk).ToList<IReadOnlyPerkObject>();
			this.HeroInformation.RefreshWith(this.HeroInformation.HeroClass, list);
			List<Tuple<HeroPerkVM, MPPerkVM>> list2 = new List<Tuple<HeroPerkVM, MPPerkVM>>();
			foreach (HeroPerkVM heroPerkVM in heroClass.Perks)
			{
				list2.Add(new Tuple<HeroPerkVM, MPPerkVM>(heroPerkVM, heroPerkVM.SelectedPerkItem));
			}
			list2.ForEach(delegate(Tuple<HeroPerkVM, MPPerkVM> p)
			{
				this.OnSelectPerk(p.Item1, p.Item2);
			});
			Action<MultiplayerClassDivisions.MPHeroClass> onRefreshSelection = this._onRefreshSelection;
			if (onRefreshSelection == null)
			{
				return;
			}
			onRefreshSelection(heroClass.HeroClass);
		}

		// Token: 0x06001093 RID: 4243 RVA: 0x00033E6C File Offset: 0x0003206C
		private void OnSelectPerk(HeroPerkVM heroPerk, MPPerkVM candidate)
		{
			if (GameNetwork.IsMyPeerReady && this.HeroInformation.HeroClass != null && this.CurrentSelectedClass != null)
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				if (!GameNetwork.IsServer || component.SelectPerk(heroPerk.PerkIndex, candidate.PerkIndex, -1))
				{
					this._missionLobbyEquipmentNetworkComponent.PerkUpdated(heroPerk.PerkIndex, candidate.PerkIndex);
				}
				List<IReadOnlyPerkObject> list = this.CurrentSelectedClass.Perks.Select<HeroPerkVM, IReadOnlyPerkObject>((HeroPerkVM x) => x.SelectedPerk).ToList<IReadOnlyPerkObject>();
				if (list.Count > 0)
				{
					this.HeroInformation.RefreshWith(this.HeroInformation.HeroClass, list);
				}
			}
		}

		// Token: 0x06001094 RID: 4244 RVA: 0x00033F34 File Offset: 0x00032134
		public void RefreshPeerDivision(MissionPeer peer, int divisionType)
		{
			MPPlayerVM mpplayerVM = this.Teammates.FirstOrDefault<MPPlayerVM>((MPPlayerVM t) => t.Peer == peer);
			if (mpplayerVM != null)
			{
				mpplayerVM.RefreshDivision(false);
			}
		}

		// Token: 0x06001095 RID: 4245 RVA: 0x00033F70 File Offset: 0x00032170
		private void RefreshPeerPerkSelection(MissionPeer peer)
		{
			MPPlayerVM mpplayerVM = this.Teammates.FirstOrDefault<MPPlayerVM>((MPPlayerVM t) => t.Peer == peer);
			if (mpplayerVM != null)
			{
				mpplayerVM.RefreshActivePerks();
			}
		}

		// Token: 0x06001096 RID: 4246 RVA: 0x00033FAC File Offset: 0x000321AC
		public void Tick(float dt)
		{
			if (this._missionMultiplayerGameMode != null)
			{
				this.IsInWarmup = this._missionMultiplayerGameMode.IsInWarmup;
				this.IsGoldEnabled = !this.IsInWarmup && this._missionMultiplayerGameMode.IsGameModeUsingGold;
				if (this.IsGoldEnabled)
				{
					this.Gold = this._missionMultiplayerGameMode.GetGoldAmount();
				}
				foreach (HeroClassGroupVM heroClassGroupVM in this.Classes)
				{
					foreach (HeroClassVM heroClassVM in heroClassGroupVM.SubClasses)
					{
						heroClassVM.IsGoldEnabled = this.IsGoldEnabled;
					}
				}
			}
			this.RefreshRemainingTime();
			this._updateTimeElapsed += dt;
			if (this._updateTimeElapsed < 1f)
			{
				return;
			}
			this._updateTimeElapsed = 0f;
			if (this._isTeammateAndEnemiesRelevant)
			{
				this.OnRefreshTeamMembers();
				this.OnRefreshEnemyMembers();
			}
		}

		// Token: 0x06001097 RID: 4247 RVA: 0x000340C4 File Offset: 0x000322C4
		private void OnPeerComponentAdded(PeerComponent component)
		{
			if (component.IsMine && component is MissionRepresentativeBase)
			{
				this._isTeammateAndEnemiesRelevant = Mission.Current.GetMissionBehavior<MissionMultiplayerGameModeBaseClient>().IsGameModeTactical && !Mission.Current.HasMissionBehavior<MissionMultiplayerSiegeClient>();
				if (this._isTeammateAndEnemiesRelevant)
				{
					this.OnRefreshTeamMembers();
					this.OnRefreshEnemyMembers();
				}
			}
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0003411C File Offset: 0x0003231C
		private void OnRefreshTeamMembers()
		{
			List<MPPlayerVM> list = this.Teammates.ToList<MPPlayerVM>();
			foreach (MissionPeer missionPeer in VirtualPlayer.Peers<MissionPeer>())
			{
				if (missionPeer.GetNetworkPeer().GetComponent<MissionPeer>() != null && this._playerTeam != null && missionPeer.Team == this._playerTeam)
				{
					if (!this._teammateDictionary.ContainsKey(missionPeer))
					{
						MPPlayerVM mpplayerVM = new MPPlayerVM(missionPeer);
						this.Teammates.Add(mpplayerVM);
						this._teammateDictionary.Add(missionPeer, mpplayerVM);
					}
					else
					{
						list.Remove(this._teammateDictionary[missionPeer]);
					}
				}
			}
			foreach (MPPlayerVM mpplayerVM2 in list)
			{
				this.Teammates.Remove(mpplayerVM2);
				this._teammateDictionary.Remove(mpplayerVM2.Peer);
			}
			foreach (MPPlayerVM mpplayerVM3 in this.Teammates)
			{
				if (mpplayerVM3.CompassElement == null)
				{
					mpplayerVM3.RefreshDivision(false);
				}
			}
		}

		// Token: 0x06001099 RID: 4249 RVA: 0x00034280 File Offset: 0x00032480
		private void OnRefreshEnemyMembers()
		{
			List<MPPlayerVM> list = this.Enemies.ToList<MPPlayerVM>();
			foreach (MissionPeer missionPeer in VirtualPlayer.Peers<MissionPeer>())
			{
				if (missionPeer.GetNetworkPeer().GetComponent<MissionPeer>() != null && this._playerTeam != null && missionPeer.Team != null && missionPeer.Team != this._playerTeam && missionPeer.Team != Mission.Current.SpectatorTeam)
				{
					if (!this._enemyDictionary.ContainsKey(missionPeer))
					{
						MPPlayerVM mpplayerVM = new MPPlayerVM(missionPeer);
						this.Enemies.Add(mpplayerVM);
						this._enemyDictionary.Add(missionPeer, mpplayerVM);
					}
					else
					{
						list.Remove(this._enemyDictionary[missionPeer]);
					}
				}
			}
			foreach (MPPlayerVM mpplayerVM2 in list)
			{
				this.Enemies.Remove(mpplayerVM2);
				this._enemyDictionary.Remove(mpplayerVM2.Peer);
			}
			foreach (MPPlayerVM mpplayerVM3 in this.Enemies)
			{
				mpplayerVM3.RefreshDivision(false);
				mpplayerVM3.UpdateDisabled();
			}
		}

		// Token: 0x0600109A RID: 4250 RVA: 0x000343FC File Offset: 0x000325FC
		public void OnPeerEquipmentRefreshed(MissionPeer peer)
		{
			if (this._teammateDictionary.ContainsKey(peer))
			{
				this._teammateDictionary[peer].RefreshActivePerks();
				return;
			}
			if (this._enemyDictionary.ContainsKey(peer))
			{
				this._enemyDictionary[peer].RefreshActivePerks();
			}
		}

		// Token: 0x0600109B RID: 4251 RVA: 0x00034448 File Offset: 0x00032648
		public void OnGoldUpdated()
		{
			foreach (HeroClassGroupVM heroClassGroupVM in this.Classes)
			{
				heroClassGroupVM.SubClasses.ApplyActionOnAllItems(delegate(HeroClassVM sc)
				{
					sc.UpdateEnabled();
				});
			}
		}

		// Token: 0x0600109C RID: 4252 RVA: 0x000344B8 File Offset: 0x000326B8
		public void RefreshRemainingTime()
		{
			int num = MathF.Ceiling(this._missionMultiplayerGameMode.RemainingTime);
			this.RemainingTimeText = TimeSpan.FromSeconds((double)num).ToString("mm':'ss");
			this.WarnRemainingTime = (float)num < 5f;
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x0600109D RID: 4253 RVA: 0x000344FF File Offset: 0x000326FF
		// (set) Token: 0x0600109E RID: 4254 RVA: 0x00034507 File Offset: 0x00032707
		[DataSourceProperty]
		public string Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				if (value != this._culture)
				{
					this._culture = value;
					base.OnPropertyChangedWithValue<string>(value, "Culture");
				}
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x0600109F RID: 4255 RVA: 0x0003452A File Offset: 0x0003272A
		// (set) Token: 0x060010A0 RID: 4256 RVA: 0x00034532 File Offset: 0x00032732
		[DataSourceProperty]
		public Color CultureColor1
		{
			get
			{
				return this._cultureColor1;
			}
			set
			{
				if (value != this._cultureColor1)
				{
					this._cultureColor1 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor1");
				}
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x060010A1 RID: 4257 RVA: 0x00034555 File Offset: 0x00032755
		// (set) Token: 0x060010A2 RID: 4258 RVA: 0x0003455D File Offset: 0x0003275D
		[DataSourceProperty]
		public Color CultureColor2
		{
			get
			{
				return this._cultureColor2;
			}
			set
			{
				if (value != this._cultureColor2)
				{
					this._cultureColor2 = value;
					base.OnPropertyChangedWithValue(value, "CultureColor2");
				}
			}
		}

		// Token: 0x1700058A RID: 1418
		// (get) Token: 0x060010A3 RID: 4259 RVA: 0x00034580 File Offset: 0x00032780
		// (set) Token: 0x060010A4 RID: 4260 RVA: 0x00034588 File Offset: 0x00032788
		[DataSourceProperty]
		public string CultureId
		{
			get
			{
				return this._cultureId;
			}
			set
			{
				if (value != this._cultureId)
				{
					this._cultureId = value;
					base.OnPropertyChangedWithValue<string>(value, "CultureId");
				}
			}
		}

		// Token: 0x1700058B RID: 1419
		// (get) Token: 0x060010A5 RID: 4261 RVA: 0x000345AB File Offset: 0x000327AB
		// (set) Token: 0x060010A6 RID: 4262 RVA: 0x000345B3 File Offset: 0x000327B3
		[DataSourceProperty]
		public bool IsSpawnTimerVisible
		{
			get
			{
				return this._isSpawnTimerVisible;
			}
			set
			{
				if (value != this._isSpawnTimerVisible)
				{
					this._isSpawnTimerVisible = value;
					base.OnPropertyChangedWithValue(value, "IsSpawnTimerVisible");
				}
			}
		}

		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x060010A7 RID: 4263 RVA: 0x000345D1 File Offset: 0x000327D1
		// (set) Token: 0x060010A8 RID: 4264 RVA: 0x000345D9 File Offset: 0x000327D9
		[DataSourceProperty]
		public string SpawnLabelText
		{
			get
			{
				return this._spawnLabelText;
			}
			set
			{
				if (value != this._spawnLabelText)
				{
					this._spawnLabelText = value;
					base.OnPropertyChangedWithValue<string>(value, "SpawnLabelText");
				}
			}
		}

		// Token: 0x1700058D RID: 1421
		// (get) Token: 0x060010A9 RID: 4265 RVA: 0x000345FC File Offset: 0x000327FC
		// (set) Token: 0x060010AA RID: 4266 RVA: 0x00034604 File Offset: 0x00032804
		[DataSourceProperty]
		public bool IsSpawnLabelVisible
		{
			get
			{
				return this._isSpawnLabelVisible;
			}
			set
			{
				if (value != this._isSpawnLabelVisible)
				{
					this._isSpawnLabelVisible = value;
					base.OnPropertyChangedWithValue(value, "IsSpawnLabelVisible");
				}
			}
		}

		// Token: 0x1700058E RID: 1422
		// (get) Token: 0x060010AB RID: 4267 RVA: 0x00034622 File Offset: 0x00032822
		// (set) Token: 0x060010AC RID: 4268 RVA: 0x0003462A File Offset: 0x0003282A
		[DataSourceProperty]
		public bool ShowAttackerOrDefenderIcons
		{
			get
			{
				return this._showAttackerOrDefenderIcons;
			}
			set
			{
				if (value != this._showAttackerOrDefenderIcons)
				{
					this._showAttackerOrDefenderIcons = value;
					base.OnPropertyChangedWithValue(value, "ShowAttackerOrDefenderIcons");
				}
			}
		}

		// Token: 0x1700058F RID: 1423
		// (get) Token: 0x060010AD RID: 4269 RVA: 0x00034648 File Offset: 0x00032848
		// (set) Token: 0x060010AE RID: 4270 RVA: 0x00034650 File Offset: 0x00032850
		[DataSourceProperty]
		public bool IsAttacker
		{
			get
			{
				return this._isAttacker;
			}
			set
			{
				if (value != this._isAttacker)
				{
					this._isAttacker = value;
					base.OnPropertyChangedWithValue(value, "IsAttacker");
				}
			}
		}

		// Token: 0x17000590 RID: 1424
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x0003466E File Offset: 0x0003286E
		// (set) Token: 0x060010B0 RID: 4272 RVA: 0x00034676 File Offset: 0x00032876
		[DataSourceProperty]
		public string SpawnForfeitLabelText
		{
			get
			{
				return this._spawnForfeitLabelText;
			}
			set
			{
				if (value != this._spawnForfeitLabelText)
				{
					this._spawnForfeitLabelText = value;
					base.OnPropertyChangedWithValue<string>(value, "SpawnForfeitLabelText");
				}
			}
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x00034699 File Offset: 0x00032899
		// (set) Token: 0x060010B2 RID: 4274 RVA: 0x000346A1 File Offset: 0x000328A1
		[DataSourceProperty]
		public bool IsSpawnForfeitLabelVisible
		{
			get
			{
				return this._isSpawnForfeitLabelVisible;
			}
			set
			{
				if (value != this._isSpawnForfeitLabelVisible)
				{
					this._isSpawnForfeitLabelVisible = value;
					base.OnPropertyChangedWithValue(value, "IsSpawnForfeitLabelVisible");
				}
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x060010B3 RID: 4275 RVA: 0x000346BF File Offset: 0x000328BF
		// (set) Token: 0x060010B4 RID: 4276 RVA: 0x000346C7 File Offset: 0x000328C7
		[DataSourceProperty]
		public int Gold
		{
			get
			{
				return this._gold;
			}
			set
			{
				if (value != this._gold)
				{
					this._gold = value;
					base.OnPropertyChangedWithValue(value, "Gold");
				}
			}
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x060010B5 RID: 4277 RVA: 0x000346E5 File Offset: 0x000328E5
		// (set) Token: 0x060010B6 RID: 4278 RVA: 0x000346ED File Offset: 0x000328ED
		[DataSourceProperty]
		public MBBindingList<MPPlayerVM> Teammates
		{
			get
			{
				return this._teammates;
			}
			set
			{
				if (value != this._teammates)
				{
					this._teammates = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPlayerVM>>(value, "Teammates");
				}
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x060010B7 RID: 4279 RVA: 0x0003470B File Offset: 0x0003290B
		// (set) Token: 0x060010B8 RID: 4280 RVA: 0x00034713 File Offset: 0x00032913
		[DataSourceProperty]
		public MBBindingList<MPPlayerVM> Enemies
		{
			get
			{
				return this._enemies;
			}
			set
			{
				if (value != this._enemies)
				{
					this._enemies = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPPlayerVM>>(value, "Enemies");
				}
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x060010B9 RID: 4281 RVA: 0x00034731 File Offset: 0x00032931
		// (set) Token: 0x060010BA RID: 4282 RVA: 0x00034739 File Offset: 0x00032939
		[DataSourceProperty]
		public HeroInformationVM HeroInformation
		{
			get
			{
				return this._heroInformation;
			}
			set
			{
				if (value != this._heroInformation)
				{
					this._heroInformation = value;
					base.OnPropertyChangedWithValue<HeroInformationVM>(value, "HeroInformation");
				}
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x060010BB RID: 4283 RVA: 0x00034757 File Offset: 0x00032957
		// (set) Token: 0x060010BC RID: 4284 RVA: 0x0003475F File Offset: 0x0003295F
		[DataSourceProperty]
		public HeroClassVM CurrentSelectedClass
		{
			get
			{
				return this._currentSelectedClass;
			}
			set
			{
				if (value != this._currentSelectedClass)
				{
					this._currentSelectedClass = value;
					base.OnPropertyChangedWithValue<HeroClassVM>(value, "CurrentSelectedClass");
				}
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x060010BD RID: 4285 RVA: 0x0003477D File Offset: 0x0003297D
		// (set) Token: 0x060010BE RID: 4286 RVA: 0x00034785 File Offset: 0x00032985
		[DataSourceProperty]
		public string RemainingTimeText
		{
			get
			{
				return this._remainingTimeText;
			}
			set
			{
				if (value != this._remainingTimeText)
				{
					this._remainingTimeText = value;
					base.OnPropertyChangedWithValue<string>(value, "RemainingTimeText");
				}
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x060010BF RID: 4287 RVA: 0x000347A8 File Offset: 0x000329A8
		// (set) Token: 0x060010C0 RID: 4288 RVA: 0x000347B0 File Offset: 0x000329B0
		[DataSourceProperty]
		public bool WarnRemainingTime
		{
			get
			{
				return this._warnRemainingTime;
			}
			set
			{
				if (value != this._warnRemainingTime)
				{
					this._warnRemainingTime = value;
					base.OnPropertyChangedWithValue(value, "WarnRemainingTime");
				}
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x060010C1 RID: 4289 RVA: 0x000347CE File Offset: 0x000329CE
		// (set) Token: 0x060010C2 RID: 4290 RVA: 0x000347D6 File Offset: 0x000329D6
		[DataSourceProperty]
		public MBBindingList<HeroClassGroupVM> Classes
		{
			get
			{
				return this._classes;
			}
			set
			{
				if (value != this._classes)
				{
					this._classes = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroClassGroupVM>>(value, "Classes");
				}
			}
		}

		// Token: 0x1700059A RID: 1434
		// (get) Token: 0x060010C3 RID: 4291 RVA: 0x000347F4 File Offset: 0x000329F4
		// (set) Token: 0x060010C4 RID: 4292 RVA: 0x000347FC File Offset: 0x000329FC
		[DataSourceProperty]
		public bool IsGoldEnabled
		{
			get
			{
				return this._isGoldEnabled;
			}
			set
			{
				if (value != this._isGoldEnabled)
				{
					this._isGoldEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsGoldEnabled");
				}
			}
		}

		// Token: 0x1700059B RID: 1435
		// (get) Token: 0x060010C5 RID: 4293 RVA: 0x0003481A File Offset: 0x00032A1A
		// (set) Token: 0x060010C6 RID: 4294 RVA: 0x00034822 File Offset: 0x00032A22
		[DataSourceProperty]
		public bool IsInWarmup
		{
			get
			{
				return this._isInWarmup;
			}
			set
			{
				if (value != this._isInWarmup)
				{
					this._isInWarmup = value;
					base.OnPropertyChangedWithValue(value, "IsInWarmup");
				}
			}
		}

		// Token: 0x1700059C RID: 1436
		// (get) Token: 0x060010C7 RID: 4295 RVA: 0x00034840 File Offset: 0x00032A40
		// (set) Token: 0x060010C8 RID: 4296 RVA: 0x00034848 File Offset: 0x00032A48
		[DataSourceProperty]
		public string WarmupInfoText
		{
			get
			{
				return this._warmupInfoText;
			}
			set
			{
				if (value != this._warmupInfoText)
				{
					this._warmupInfoText = value;
					base.OnPropertyChangedWithValue<string>(value, "WarmupInfoText");
				}
			}
		}

		// Token: 0x040007C4 RID: 1988
		public const float UPDATE_INTERVAL = 1f;

		// Token: 0x040007C5 RID: 1989
		private float _updateTimeElapsed;

		// Token: 0x040007C6 RID: 1990
		private readonly Action<MultiplayerClassDivisions.MPHeroClass> _onRefreshSelection;

		// Token: 0x040007C7 RID: 1991
		private readonly MissionMultiplayerGameModeBaseClient _missionMultiplayerGameMode;

		// Token: 0x040007C8 RID: 1992
		private Dictionary<MissionPeer, MPPlayerVM> _enemyDictionary;

		// Token: 0x040007C9 RID: 1993
		private readonly Mission _mission;

		// Token: 0x040007CA RID: 1994
		private bool _isTeammateAndEnemiesRelevant;

		// Token: 0x040007CB RID: 1995
		private const float REMAINING_TIME_WARNING_THRESHOLD = 5f;

		// Token: 0x040007CC RID: 1996
		private MissionLobbyEquipmentNetworkComponent _missionLobbyEquipmentNetworkComponent;

		// Token: 0x040007CD RID: 1997
		private bool _isInitializing;

		// Token: 0x040007CE RID: 1998
		private Dictionary<MissionPeer, MPPlayerVM> _teammateDictionary;

		// Token: 0x040007CF RID: 1999
		private int _gold;

		// Token: 0x040007D0 RID: 2000
		private string _culture;

		// Token: 0x040007D1 RID: 2001
		private string _cultureId;

		// Token: 0x040007D2 RID: 2002
		private string _spawnLabelText;

		// Token: 0x040007D3 RID: 2003
		private string _spawnForfeitLabelText;

		// Token: 0x040007D4 RID: 2004
		private string _remainingTimeText;

		// Token: 0x040007D5 RID: 2005
		private bool _warnRemainingTime;

		// Token: 0x040007D6 RID: 2006
		private bool _isSpawnTimerVisible;

		// Token: 0x040007D7 RID: 2007
		private bool _isSpawnLabelVisible;

		// Token: 0x040007D8 RID: 2008
		private bool _isSpawnForfeitLabelVisible;

		// Token: 0x040007D9 RID: 2009
		private bool _isGoldEnabled;

		// Token: 0x040007DA RID: 2010
		private bool _isInWarmup;

		// Token: 0x040007DB RID: 2011
		private bool _showAttackerOrDefenderIcons;

		// Token: 0x040007DC RID: 2012
		private bool _isAttacker;

		// Token: 0x040007DD RID: 2013
		private string _warmupInfoText;

		// Token: 0x040007DE RID: 2014
		private Color _cultureColor1;

		// Token: 0x040007DF RID: 2015
		private Color _cultureColor2;

		// Token: 0x040007E0 RID: 2016
		private MBBindingList<HeroClassGroupVM> _classes;

		// Token: 0x040007E1 RID: 2017
		private HeroInformationVM _heroInformation;

		// Token: 0x040007E2 RID: 2018
		private HeroClassVM _currentSelectedClass;

		// Token: 0x040007E3 RID: 2019
		private MBBindingList<MPPlayerVM> _teammates;

		// Token: 0x040007E4 RID: 2020
		private MBBindingList<MPPlayerVM> _enemies;
	}
}
