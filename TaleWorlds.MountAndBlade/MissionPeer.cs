using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200030B RID: 779
	public class MissionPeer : PeerComponent
	{
		// Token: 0x1400008D RID: 141
		// (add) Token: 0x06002C73 RID: 11379 RVA: 0x000AB564 File Offset: 0x000A9764
		// (remove) Token: 0x06002C74 RID: 11380 RVA: 0x000AB598 File Offset: 0x000A9798
		public static event MissionPeer.OnUpdateEquipmentSetIndexEventDelegate OnEquipmentIndexRefreshed;

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x06002C75 RID: 11381 RVA: 0x000AB5CC File Offset: 0x000A97CC
		// (remove) Token: 0x06002C76 RID: 11382 RVA: 0x000AB600 File Offset: 0x000A9800
		public static event MissionPeer.OnPerkUpdateEventDelegate OnPerkSelectionUpdated;

		// Token: 0x1400008F RID: 143
		// (add) Token: 0x06002C77 RID: 11383 RVA: 0x000AB634 File Offset: 0x000A9834
		// (remove) Token: 0x06002C78 RID: 11384 RVA: 0x000AB668 File Offset: 0x000A9868
		public static event MissionPeer.OnTeamChangedDelegate OnPreTeamChanged;

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x06002C79 RID: 11385 RVA: 0x000AB69C File Offset: 0x000A989C
		// (remove) Token: 0x06002C7A RID: 11386 RVA: 0x000AB6D0 File Offset: 0x000A98D0
		public static event MissionPeer.OnTeamChangedDelegate OnTeamChanged;

		// Token: 0x14000091 RID: 145
		// (add) Token: 0x06002C7B RID: 11387 RVA: 0x000AB704 File Offset: 0x000A9904
		// (remove) Token: 0x06002C7C RID: 11388 RVA: 0x000AB73C File Offset: 0x000A993C
		private event MissionPeer.OnCultureChangedDelegate OnCultureChanged;

		// Token: 0x14000092 RID: 146
		// (add) Token: 0x06002C7D RID: 11389 RVA: 0x000AB774 File Offset: 0x000A9974
		// (remove) Token: 0x06002C7E RID: 11390 RVA: 0x000AB7A8 File Offset: 0x000A99A8
		public static event MissionPeer.OnPlayerKilledDelegate OnPlayerKilled;

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06002C7F RID: 11391 RVA: 0x000AB7DB File Offset: 0x000A99DB
		// (set) Token: 0x06002C80 RID: 11392 RVA: 0x000AB7E3 File Offset: 0x000A99E3
		public DateTime JoinTime { get; internal set; }

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06002C81 RID: 11393 RVA: 0x000AB7EC File Offset: 0x000A99EC
		// (set) Token: 0x06002C82 RID: 11394 RVA: 0x000AB7F4 File Offset: 0x000A99F4
		public bool EquipmentUpdatingExpired { get; set; }

		// Token: 0x17000839 RID: 2105
		// (get) Token: 0x06002C83 RID: 11395 RVA: 0x000AB7FD File Offset: 0x000A99FD
		// (set) Token: 0x06002C84 RID: 11396 RVA: 0x000AB805 File Offset: 0x000A9A05
		public bool TeamInitialPerkInfoReady { get; private set; }

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06002C85 RID: 11397 RVA: 0x000AB80E File Offset: 0x000A9A0E
		// (set) Token: 0x06002C86 RID: 11398 RVA: 0x000AB816 File Offset: 0x000A9A16
		public bool HasSpawnedAgentVisuals { get; set; }

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06002C87 RID: 11399 RVA: 0x000AB81F File Offset: 0x000A9A1F
		// (set) Token: 0x06002C88 RID: 11400 RVA: 0x000AB827 File Offset: 0x000A9A27
		public int SelectedTroopIndex
		{
			get
			{
				return this._selectedTroopIndex;
			}
			set
			{
				if (this._selectedTroopIndex != value)
				{
					this._selectedTroopIndex = value;
					this.ResetSelectedPerks();
					MissionPeer.OnUpdateEquipmentSetIndexEventDelegate onEquipmentIndexRefreshed = MissionPeer.OnEquipmentIndexRefreshed;
					if (onEquipmentIndexRefreshed == null)
					{
						return;
					}
					onEquipmentIndexRefreshed(this, value);
				}
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06002C89 RID: 11401 RVA: 0x000AB850 File Offset: 0x000A9A50
		// (set) Token: 0x06002C8A RID: 11402 RVA: 0x000AB858 File Offset: 0x000A9A58
		public int NextSelectedTroopIndex { get; set; }

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002C8B RID: 11403 RVA: 0x000AB861 File Offset: 0x000A9A61
		public MissionRepresentativeBase Representative
		{
			get
			{
				if (this._representative == null)
				{
					this._representative = base.Peer.GetComponent<MissionRepresentativeBase>();
				}
				return this._representative;
			}
		}

		// Token: 0x1700083E RID: 2110
		// (get) Token: 0x06002C8C RID: 11404 RVA: 0x000AB882 File Offset: 0x000A9A82
		public MBReadOnlyList<int[]> Perks
		{
			get
			{
				return this._perks;
			}
		}

		// Token: 0x1700083F RID: 2111
		// (get) Token: 0x06002C8D RID: 11405 RVA: 0x000AB88C File Offset: 0x000A9A8C
		public string DisplayedName
		{
			get
			{
				if (GameNetwork.IsDedicatedServer)
				{
					return base.Name;
				}
				if (NetworkMain.CommunityClient.IsInGame)
				{
					return base.Name;
				}
				if (NetworkMain.GameClient.HasUserGeneratedContentPrivilege && (NetworkMain.GameClient.IsKnownPlayer(base.Peer.Id) || !BannerlordConfig.EnableGenericNames))
				{
					VirtualPlayer peer = base.Peer;
					return ((peer != null) ? peer.UserName : null) ?? "";
				}
				if (this.Culture == null || MultiplayerClassDivisions.GetMPHeroClassForPeer(this, false) == null)
				{
					return new TextObject("{=RN6zHak0}Player", null).ToString();
				}
				return MultiplayerClassDivisions.GetMPHeroClassForPeer(this, false).TroopName.ToString();
			}
		}

		// Token: 0x17000840 RID: 2112
		// (get) Token: 0x06002C8E RID: 11406 RVA: 0x000AB933 File Offset: 0x000A9B33
		// (set) Token: 0x06002C8F RID: 11407 RVA: 0x000AB93B File Offset: 0x000A9B3B
		public string ClanName { get; internal set; } = string.Empty;

		// Token: 0x17000841 RID: 2113
		// (get) Token: 0x06002C90 RID: 11408 RVA: 0x000AB944 File Offset: 0x000A9B44
		// (set) Token: 0x06002C91 RID: 11409 RVA: 0x000AB94C File Offset: 0x000A9B4C
		public bool HasSentClanInfo { get; internal set; }

		// Token: 0x17000842 RID: 2114
		// (get) Token: 0x06002C92 RID: 11410 RVA: 0x000AB955 File Offset: 0x000A9B55
		// (set) Token: 0x06002C93 RID: 11411 RVA: 0x000AB95D File Offset: 0x000A9B5D
		public string LastKillVictimName { get; internal set; } = string.Empty;

		// Token: 0x17000843 RID: 2115
		// (get) Token: 0x06002C94 RID: 11412 RVA: 0x000AB966 File Offset: 0x000A9B66
		// (set) Token: 0x06002C95 RID: 11413 RVA: 0x000AB96E File Offset: 0x000A9B6E
		public string MostUsedWeaponName { get; internal set; } = string.Empty;

		// Token: 0x06002C96 RID: 11414 RVA: 0x000AB978 File Offset: 0x000A9B78
		public bool RegisterWeaponUsage(WeaponClass weaponClass, int weight)
		{
			if (weaponClass == WeaponClass.Undefined || weight <= 0)
			{
				return false;
			}
			if (!this._weaponUsageByClass.ContainsKey(weaponClass))
			{
				this._weaponUsageByClass[weaponClass] = 0;
			}
			Dictionary<WeaponClass, int> weaponUsageByClass = this._weaponUsageByClass;
			weaponUsageByClass[weaponClass] += weight;
			WeaponClass weaponClass2 = this._mostUsedWeaponClass;
			int num = ((weaponClass2 != WeaponClass.Undefined && this._weaponUsageByClass.ContainsKey(weaponClass2)) ? this._weaponUsageByClass[weaponClass2] : (-1));
			foreach (KeyValuePair<WeaponClass, int> keyValuePair in this._weaponUsageByClass)
			{
				if (keyValuePair.Value > num)
				{
					num = keyValuePair.Value;
					weaponClass2 = keyValuePair.Key;
				}
			}
			if (weaponClass2 != this._mostUsedWeaponClass)
			{
				this._mostUsedWeaponClass = weaponClass2;
				return true;
			}
			return false;
		}

		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06002C97 RID: 11415 RVA: 0x000ABA58 File Offset: 0x000A9C58
		public WeaponClass MostUsedWeaponClass
		{
			get
			{
				return this._mostUsedWeaponClass;
			}
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x000ABA60 File Offset: 0x000A9C60
		public void ResetSpectatorStats()
		{
			this._weaponUsageByClass.Clear();
			this._mostUsedWeaponClass = WeaponClass.Undefined;
			this.MostUsedWeaponName = string.Empty;
			this.LastKillVictimName = string.Empty;
		}

		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06002C99 RID: 11417 RVA: 0x000ABA8C File Offset: 0x000A9C8C
		public MBReadOnlyList<MPPerkObject> SelectedPerks
		{
			get
			{
				if (this.SelectedTroopIndex < 0 || this.Team == null || this.Team.Side == BattleSideEnum.None)
				{
					return new MBList<MPPerkObject>();
				}
				if ((this._selectedPerks.Item2 == null || this.SelectedTroopIndex != this._selectedPerks.Item1 || this._selectedPerks.Item2.Count < 3) && !this.RefreshSelectedPerks())
				{
					return new MBReadOnlyList<MPPerkObject>();
				}
				return this._selectedPerks.Item2;
			}
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x000ABB0C File Offset: 0x000A9D0C
		public MissionPeer()
		{
			this.SpawnTimer = new Timer(Mission.Current.CurrentTime, 3f, false);
			this._selectedPerks = new ValueTuple<int, MBList<MPPerkObject>>(0, null);
			this._perks = new MBList<int[]>();
			for (int i = 0; i < 16; i++)
			{
				int[] array = new int[3];
				this._perks.Add(array);
			}
		}

		// Token: 0x17000846 RID: 2118
		// (get) Token: 0x06002C9B RID: 11419 RVA: 0x000ABBC7 File Offset: 0x000A9DC7
		// (set) Token: 0x06002C9C RID: 11420 RVA: 0x000ABBCF File Offset: 0x000A9DCF
		public Timer SpawnTimer { get; internal set; }

		// Token: 0x17000847 RID: 2119
		// (get) Token: 0x06002C9D RID: 11421 RVA: 0x000ABBD8 File Offset: 0x000A9DD8
		// (set) Token: 0x06002C9E RID: 11422 RVA: 0x000ABBE0 File Offset: 0x000A9DE0
		public bool HasSpawnTimerExpired { get; set; }

		// Token: 0x17000848 RID: 2120
		// (get) Token: 0x06002C9F RID: 11423 RVA: 0x000ABBE9 File Offset: 0x000A9DE9
		// (set) Token: 0x06002CA0 RID: 11424 RVA: 0x000ABBF1 File Offset: 0x000A9DF1
		public BasicCultureObject VotedForBan { get; private set; }

		// Token: 0x17000849 RID: 2121
		// (get) Token: 0x06002CA1 RID: 11425 RVA: 0x000ABBFA File Offset: 0x000A9DFA
		// (set) Token: 0x06002CA2 RID: 11426 RVA: 0x000ABC02 File Offset: 0x000A9E02
		public BasicCultureObject VotedForSelection { get; private set; }

		// Token: 0x1700084A RID: 2122
		// (get) Token: 0x06002CA3 RID: 11427 RVA: 0x000ABC0B File Offset: 0x000A9E0B
		// (set) Token: 0x06002CA4 RID: 11428 RVA: 0x000ABC13 File Offset: 0x000A9E13
		public bool WantsToSpawnAsBot { get; set; }

		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x06002CA5 RID: 11429 RVA: 0x000ABC1C File Offset: 0x000A9E1C
		// (set) Token: 0x06002CA6 RID: 11430 RVA: 0x000ABC24 File Offset: 0x000A9E24
		public int SpawnCountThisRound { get; set; }

		// Token: 0x1700084C RID: 2124
		// (get) Token: 0x06002CA7 RID: 11431 RVA: 0x000ABC2D File Offset: 0x000A9E2D
		// (set) Token: 0x06002CA8 RID: 11432 RVA: 0x000ABC35 File Offset: 0x000A9E35
		public int RequestedKickPollCount { get; private set; }

		// Token: 0x1700084D RID: 2125
		// (get) Token: 0x06002CA9 RID: 11433 RVA: 0x000ABC3E File Offset: 0x000A9E3E
		// (set) Token: 0x06002CAA RID: 11434 RVA: 0x000ABC46 File Offset: 0x000A9E46
		public int KillCount
		{
			get
			{
				return this._killCount;
			}
			internal set
			{
				this._killCount = MBMath.ClampInt(value, -1000, 100000);
			}
		}

		// Token: 0x1700084E RID: 2126
		// (get) Token: 0x06002CAB RID: 11435 RVA: 0x000ABC5E File Offset: 0x000A9E5E
		// (set) Token: 0x06002CAC RID: 11436 RVA: 0x000ABC66 File Offset: 0x000A9E66
		public int AssistCount
		{
			get
			{
				return this._assistCount;
			}
			internal set
			{
				this._assistCount = MBMath.ClampInt(value, -1000, 100000);
			}
		}

		// Token: 0x1700084F RID: 2127
		// (get) Token: 0x06002CAD RID: 11437 RVA: 0x000ABC7E File Offset: 0x000A9E7E
		// (set) Token: 0x06002CAE RID: 11438 RVA: 0x000ABC86 File Offset: 0x000A9E86
		public int DeathCount
		{
			get
			{
				return this._deathCount;
			}
			internal set
			{
				this._deathCount = MBMath.ClampInt(value, -1000, 100000);
			}
		}

		// Token: 0x17000850 RID: 2128
		// (get) Token: 0x06002CAF RID: 11439 RVA: 0x000ABC9E File Offset: 0x000A9E9E
		// (set) Token: 0x06002CB0 RID: 11440 RVA: 0x000ABCA6 File Offset: 0x000A9EA6
		public int Score
		{
			get
			{
				return this._score;
			}
			internal set
			{
				this._score = MBMath.ClampInt(value, -1000000, 1000000);
			}
		}

		// Token: 0x17000851 RID: 2129
		// (get) Token: 0x06002CB1 RID: 11441 RVA: 0x000ABCBE File Offset: 0x000A9EBE
		// (set) Token: 0x06002CB2 RID: 11442 RVA: 0x000ABCC6 File Offset: 0x000A9EC6
		public int BotsUnderControlAlive
		{
			get
			{
				return this._botsUnderControlAlive;
			}
			set
			{
				if (this._botsUnderControlAlive != value)
				{
					this._botsUnderControlAlive = value;
					MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(this);
					if (perkHandler == null)
					{
						return;
					}
					perkHandler.OnEvent(MPPerkCondition.PerkEventFlags.AliveBotCountChange);
				}
			}
		}

		// Token: 0x17000852 RID: 2130
		// (get) Token: 0x06002CB3 RID: 11443 RVA: 0x000ABCEA File Offset: 0x000A9EEA
		// (set) Token: 0x06002CB4 RID: 11444 RVA: 0x000ABCF2 File Offset: 0x000A9EF2
		public int BotsUnderControlTotal { get; internal set; }

		// Token: 0x17000853 RID: 2131
		// (get) Token: 0x06002CB5 RID: 11445 RVA: 0x000ABCFB File Offset: 0x000A9EFB
		public bool IsControlledAgentActive
		{
			get
			{
				return this.ControlledAgent != null && this.ControlledAgent.IsActive();
			}
		}

		// Token: 0x17000854 RID: 2132
		// (get) Token: 0x06002CB6 RID: 11446 RVA: 0x000ABD12 File Offset: 0x000A9F12
		// (set) Token: 0x06002CB7 RID: 11447 RVA: 0x000ABD20 File Offset: 0x000A9F20
		public Agent ControlledAgent
		{
			get
			{
				return this.GetNetworkPeer().ControlledAgent;
			}
			set
			{
				NetworkCommunicator networkPeer = this.GetNetworkPeer();
				if (networkPeer.ControlledAgent != value)
				{
					this.ResetSelectedPerks();
					Agent controlledAgent = networkPeer.ControlledAgent;
					networkPeer.ControlledAgent = value;
					if (controlledAgent != null && controlledAgent.MissionPeer == this && controlledAgent.IsActive())
					{
						controlledAgent.MissionPeer = null;
					}
					if (networkPeer.ControlledAgent != null && networkPeer.ControlledAgent.MissionPeer != this)
					{
						networkPeer.ControlledAgent.MissionPeer = this;
					}
					MissionRepresentativeBase component = networkPeer.VirtualPlayer.GetComponent<MissionRepresentativeBase>();
					if (component != null)
					{
						component.SetAgent(value);
					}
					if (value != null)
					{
						MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(this);
						if (perkHandler == null)
						{
							return;
						}
						perkHandler.OnEvent(value, MPPerkCondition.PerkEventFlags.PeerControlledAgentChange);
					}
				}
			}
		}

		// Token: 0x17000855 RID: 2133
		// (get) Token: 0x06002CB8 RID: 11448 RVA: 0x000ABDBA File Offset: 0x000A9FBA
		// (set) Token: 0x06002CB9 RID: 11449 RVA: 0x000ABDC2 File Offset: 0x000A9FC2
		public Agent FollowedAgent
		{
			get
			{
				return this._followedAgent;
			}
			set
			{
				if (this._followedAgent != value)
				{
					this._followedAgent = value;
					if (GameNetwork.IsClient)
					{
						GameNetwork.BeginModuleEventAsClient();
						Agent followedAgent = this._followedAgent;
						GameNetwork.WriteMessage(new SetFollowedAgent((followedAgent != null) ? followedAgent.Index : (-1)));
						GameNetwork.EndModuleEventAsClient();
					}
				}
			}
		}

		// Token: 0x17000856 RID: 2134
		// (get) Token: 0x06002CBA RID: 11450 RVA: 0x000ABE01 File Offset: 0x000AA001
		// (set) Token: 0x06002CBB RID: 11451 RVA: 0x000ABE0C File Offset: 0x000AA00C
		public Team Team
		{
			get
			{
				return this._team;
			}
			set
			{
				if (this._team != value)
				{
					if (MissionPeer.OnPreTeamChanged != null)
					{
						MissionPeer.OnPreTeamChanged(this.GetNetworkPeer(), this._team, value);
					}
					Team team = this._team;
					this._team = value;
					string text = "Set the team to: ";
					Team team2 = this._team;
					Debug.Print(text + (((team2 != null) ? team2.Side.ToString() : null) ?? "null") + ", for peer: " + base.Name, 0, Debug.DebugColor.White, 17592186044416UL);
					this._controlledFormation = null;
					if (this._team != null)
					{
						if (GameNetwork.IsServer)
						{
							MBAPI.IMBPeer.SetTeam(base.Peer.Index, this._team.MBTeam.Index);
							GameNetwork.BeginBroadcastModuleEvent();
							GameNetwork.WriteMessage(new SetPeerTeam(this.GetNetworkPeer(), this._team.TeamIndex));
							GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
						}
						if (MissionPeer.OnTeamChanged != null)
						{
							MissionPeer.OnTeamChanged(this.GetNetworkPeer(), team, this._team);
							return;
						}
					}
					else if (GameNetwork.IsServer)
					{
						MBAPI.IMBPeer.SetTeam(base.Peer.Index, -1);
					}
				}
			}
		}

		// Token: 0x17000857 RID: 2135
		// (get) Token: 0x06002CBC RID: 11452 RVA: 0x000ABF3F File Offset: 0x000AA13F
		// (set) Token: 0x06002CBD RID: 11453 RVA: 0x000ABF48 File Offset: 0x000AA148
		public BasicCultureObject Culture
		{
			get
			{
				return this._culture;
			}
			set
			{
				BasicCultureObject culture = this._culture;
				this._culture = value;
				if (GameNetwork.IsServerOrRecorder)
				{
					this.TeamInitialPerkInfoReady = base.Peer.IsMine;
					GameNetwork.BeginBroadcastModuleEvent();
					GameNetwork.WriteMessage(new ChangeCulture(this, this._culture));
					GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord, null);
				}
				if (this.OnCultureChanged != null)
				{
					this.OnCultureChanged(this._culture);
				}
			}
		}

		// Token: 0x17000858 RID: 2136
		// (get) Token: 0x06002CBE RID: 11454 RVA: 0x000ABFB2 File Offset: 0x000AA1B2
		// (set) Token: 0x06002CBF RID: 11455 RVA: 0x000ABFBA File Offset: 0x000AA1BA
		public Formation ControlledFormation
		{
			get
			{
				return this._controlledFormation;
			}
			set
			{
				if (this._controlledFormation != value)
				{
					this._controlledFormation = value;
				}
			}
		}

		// Token: 0x17000859 RID: 2137
		// (get) Token: 0x06002CC0 RID: 11456 RVA: 0x000ABFCC File Offset: 0x000AA1CC
		public bool IsAgentAliveForChatting
		{
			get
			{
				MissionPeer component = base.GetComponent<MissionPeer>();
				return component != null && (this.IsControlledAgentActive || component.HasSpawnedAgentVisuals);
			}
		}

		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x06002CC1 RID: 11457 RVA: 0x000ABFF5 File Offset: 0x000AA1F5
		// (set) Token: 0x06002CC2 RID: 11458 RVA: 0x000ABFFD File Offset: 0x000AA1FD
		public bool IsMutedFromPlatform { get; private set; }

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x06002CC3 RID: 11459 RVA: 0x000AC006 File Offset: 0x000AA206
		// (set) Token: 0x06002CC4 RID: 11460 RVA: 0x000AC00E File Offset: 0x000AA20E
		public bool IsMuted { get; private set; }

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x06002CC5 RID: 11461 RVA: 0x000AC017 File Offset: 0x000AA217
		public bool IsMutedFromGameOrPlatform
		{
			get
			{
				return this.IsMutedFromPlatform || this.IsMuted;
			}
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x000AC029 File Offset: 0x000AA229
		public void SetMutedFromPlatform(bool isMuted)
		{
			this.IsMutedFromPlatform = isMuted;
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x000AC032 File Offset: 0x000AA232
		public void SetMuted(bool isMuted)
		{
			this.IsMuted = isMuted;
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x000AC03B File Offset: 0x000AA23B
		public void ResetRequestedKickPollCount()
		{
			this.RequestedKickPollCount = 0;
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x000AC044 File Offset: 0x000AA244
		public void IncrementRequestedKickPollCount()
		{
			int requestedKickPollCount = this.RequestedKickPollCount;
			this.RequestedKickPollCount = requestedKickPollCount + 1;
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x000AC061 File Offset: 0x000AA261
		public int GetSelectedPerkIndexWithPerkListIndex(int troopIndex, int perkListIndex)
		{
			return this._perks[troopIndex][perkListIndex];
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x000AC074 File Offset: 0x000AA274
		public bool SelectPerk(int perkListIndex, int perkIndex, int enforcedSelectedTroopIndex = -1)
		{
			if (this.SelectedTroopIndex >= 0 && enforcedSelectedTroopIndex >= 0 && this.SelectedTroopIndex != enforcedSelectedTroopIndex)
			{
				Debug.Print("SelectedTroopIndex < 0 || enforcedSelectedTroopIndex < 0 || SelectedTroopIndex == enforcedSelectedTroopIndex", 0, Debug.DebugColor.White, 17179869184UL);
				Debug.Print(string.Format("SelectedTroopIndex: {0} enforcedSelectedTroopIndex: {1}", this.SelectedTroopIndex, enforcedSelectedTroopIndex), 0, Debug.DebugColor.White, 17179869184UL);
			}
			int num = ((enforcedSelectedTroopIndex >= 0) ? enforcedSelectedTroopIndex : this.SelectedTroopIndex);
			if (perkIndex != this._perks[num][perkListIndex])
			{
				this._perks[num][perkListIndex] = perkIndex;
				if (this.GetNetworkPeer().IsMine)
				{
					List<MultiplayerClassDivisions.MPHeroClass> list = MultiplayerClassDivisions.GetMPHeroClasses(this.Culture).ToList<MultiplayerClassDivisions.MPHeroClass>();
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						if (num == i)
						{
							MultiplayerClassDivisions.MPHeroClass mpheroClass = list[i];
							List<MPPerkSelectionManager.MPPerkSelection> list2 = new List<MPPerkSelectionManager.MPPerkSelection>();
							for (int j = 0; j < 3; j++)
							{
								list2.Add(new MPPerkSelectionManager.MPPerkSelection(this._perks[i][j], j));
							}
							MPPerkSelectionManager.Instance.SetSelectionsForHeroClassTemporarily(mpheroClass, list2);
							break;
						}
					}
				}
				if (num == this.SelectedTroopIndex)
				{
					this.ResetSelectedPerks();
				}
				MissionPeer.OnPerkUpdateEventDelegate onPerkSelectionUpdated = MissionPeer.OnPerkSelectionUpdated;
				if (onPerkSelectionUpdated != null)
				{
					onPerkSelectionUpdated(this);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x000AC1B0 File Offset: 0x000AA3B0
		public void HandleVoteChange(CultureVoteTypes voteType, BasicCultureObject culture)
		{
			if (voteType != CultureVoteTypes.Ban)
			{
				if (voteType == CultureVoteTypes.Select)
				{
					this.VotedForSelection = culture;
				}
			}
			else
			{
				this.VotedForBan = culture;
			}
			if (GameNetwork.IsServer)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new CultureVoteServer(this.GetNetworkPeer(), voteType, (voteType == CultureVoteTypes.Ban) ? this.VotedForBan : this.VotedForSelection));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x000AC20C File Offset: 0x000AA40C
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (base.IsMine)
			{
				MPPerkSelectionManager.Instance.TryToApplyAndSavePendingChanges();
			}
			this.ResetKillRegistry();
			if (this.HasSpawnedAgentVisuals && Mission.Current != null)
			{
				MultiplayerMissionAgentVisualSpawnComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerMissionAgentVisualSpawnComponent>();
				if (missionBehavior != null)
				{
					missionBehavior.RemoveAgentVisuals(this, false);
				}
				this.HasSpawnedAgentVisuals = false;
				this.OnCultureChanged -= this.CultureChanged;
			}
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x000AC276 File Offset: 0x000AA476
		public override void OnInitialize()
		{
			base.OnInitialize();
			this.OnCultureChanged += this.CultureChanged;
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x000AC290 File Offset: 0x000AA490
		public int GetAmountOfAgentVisualsForPeer()
		{
			return this._visuals.Count<PeerVisualsHolder>((PeerVisualsHolder v) => v != null);
		}

		// Token: 0x06002CD0 RID: 11472 RVA: 0x000AC2BC File Offset: 0x000AA4BC
		public PeerVisualsHolder GetVisuals(int visualIndex)
		{
			if (this._visuals.Count <= 0)
			{
				return null;
			}
			return this._visuals[visualIndex];
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x000AC2DC File Offset: 0x000AA4DC
		public void ClearVisuals(int visualIndex)
		{
			if (visualIndex < this._visuals.Count && this._visuals[visualIndex] != null)
			{
				if (!GameNetwork.IsDedicatedServer)
				{
					MBAgentVisuals visuals = this._visuals[visualIndex].AgentVisuals.GetVisuals();
					visuals.ClearVisualComponents(true, true);
					visuals.ClearAllWeaponMeshes();
					visuals.Reset();
					if (this._visuals[visualIndex].MountAgentVisuals != null)
					{
						MBAgentVisuals visuals2 = this._visuals[visualIndex].MountAgentVisuals.GetVisuals();
						visuals2.ClearVisualComponents(true, true);
						visuals2.ClearAllWeaponMeshes();
						visuals2.Reset();
					}
				}
				this._visuals[visualIndex] = null;
			}
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x000AC384 File Offset: 0x000AA584
		public void ClearAllVisuals(bool freeResources = false)
		{
			if (this._visuals != null)
			{
				for (int i = this._visuals.Count - 1; i >= 0; i--)
				{
					if (this._visuals[i] != null)
					{
						this.ClearVisuals(i);
					}
				}
				if (freeResources)
				{
					this._visuals = null;
				}
			}
		}

		// Token: 0x06002CD3 RID: 11475 RVA: 0x000AC3D0 File Offset: 0x000AA5D0
		public void OnVisualsSpawned(PeerVisualsHolder visualsHolder, int visualIndex)
		{
			if (visualIndex >= this._visuals.Count)
			{
				int num = visualIndex - this._visuals.Count;
				for (int i = 0; i < num + 1; i++)
				{
					this._visuals.Add(null);
				}
			}
			this._visuals[visualIndex] = visualsHolder;
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x000AC420 File Offset: 0x000AA620
		public IEnumerable<IAgentVisual> GetAllAgentVisualsForPeer()
		{
			int count = this.GetAmountOfAgentVisualsForPeer();
			int num;
			for (int i = 0; i < count; i = num + 1)
			{
				yield return this.GetVisuals(i).AgentVisuals;
				num = i;
			}
			yield break;
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x000AC430 File Offset: 0x000AA630
		public IAgentVisual GetAgentVisualForPeer(int visualsIndex)
		{
			IAgentVisual agentVisual;
			return this.GetAgentVisualForPeer(visualsIndex, out agentVisual);
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x000AC448 File Offset: 0x000AA648
		public IAgentVisual GetAgentVisualForPeer(int visualsIndex, out IAgentVisual mountAgentVisuals)
		{
			PeerVisualsHolder visuals = this.GetVisuals(visualsIndex);
			mountAgentVisuals = ((visuals != null) ? visuals.MountAgentVisuals : null);
			if (visuals == null)
			{
				return null;
			}
			return visuals.AgentVisuals;
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x000AC478 File Offset: 0x000AA678
		public void TickInactivityStatus()
		{
			NetworkCommunicator networkPeer = this.GetNetworkPeer();
			if (!networkPeer.IsMine)
			{
				if (this.ControlledAgent != null && this.ControlledAgent.IsActive())
				{
					if (this._lastActiveTime == MissionTime.Zero)
					{
						this._lastActiveTime = MissionTime.Now;
						this._previousActivityStatus = ValueTuple.Create<Agent.MovementControlFlag, Vec2, Vec3>(this.ControlledAgent.MovementFlags, this.ControlledAgent.MovementInputVector, this.ControlledAgent.LookDirection);
						this._inactiveWarningGiven = false;
						return;
					}
					ValueTuple<Agent.MovementControlFlag, Vec2, Vec3> valueTuple = ValueTuple.Create<Agent.MovementControlFlag, Vec2, Vec3>(this.ControlledAgent.MovementFlags, this.ControlledAgent.MovementInputVector, this.ControlledAgent.LookDirection);
					if (this._previousActivityStatus.Item1 != valueTuple.Item1 || this._previousActivityStatus.Item2.DistanceSquared(valueTuple.Item2) > 1E-05f || this._previousActivityStatus.Item3.DistanceSquared(valueTuple.Item3) > 1E-05f)
					{
						this._lastActiveTime = MissionTime.Now;
						this._previousActivityStatus = valueTuple;
						this._inactiveWarningGiven = false;
					}
					if (this._lastActiveTime.ElapsedSeconds > 180f)
					{
						DisconnectInfo disconnectInfo = networkPeer.PlayerConnectionInfo.GetParameter<DisconnectInfo>("DisconnectInfo") ?? new DisconnectInfo();
						disconnectInfo.Type = DisconnectType.Inactivity;
						networkPeer.PlayerConnectionInfo.AddParameter("DisconnectInfo", disconnectInfo);
						GameNetwork.AddNetworkPeerToDisconnectAsServer(networkPeer);
						return;
					}
					if (this._lastActiveTime.ElapsedSeconds > 120f && !this._inactiveWarningGiven)
					{
						MultiplayerGameNotificationsComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerGameNotificationsComponent>();
						if (missionBehavior != null)
						{
							missionBehavior.PlayerIsInactive(this.GetNetworkPeer());
						}
						this._inactiveWarningGiven = true;
						return;
					}
				}
				else
				{
					this._lastActiveTime = MissionTime.Now;
					this._inactiveWarningGiven = false;
				}
			}
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x000AC62C File Offset: 0x000AA82C
		public void OnKillAnotherPeer(MissionPeer victimPeer)
		{
			if (victimPeer != null)
			{
				if (!this._numberOfTimesPeerKilledPerPeer.ContainsKey(victimPeer))
				{
					this._numberOfTimesPeerKilledPerPeer.Add(victimPeer, 1);
				}
				else
				{
					Dictionary<MissionPeer, int> numberOfTimesPeerKilledPerPeer = this._numberOfTimesPeerKilledPerPeer;
					int num = numberOfTimesPeerKilledPerPeer[victimPeer];
					numberOfTimesPeerKilledPerPeer[victimPeer] = num + 1;
				}
				this.LastKillVictimName = victimPeer.DisplayedName ?? string.Empty;
				MissionPeer.OnPlayerKilledDelegate onPlayerKilled = MissionPeer.OnPlayerKilled;
				if (onPlayerKilled == null)
				{
					return;
				}
				onPlayerKilled(this, victimPeer);
			}
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x000AC698 File Offset: 0x000AA898
		public void OnKillBot(string botName)
		{
			if (!string.IsNullOrEmpty(botName))
			{
				this.LastKillVictimName = botName;
			}
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x000AC6AC File Offset: 0x000AA8AC
		public void OverrideCultureWithTeamCulture()
		{
			MultiplayerOptions.OptionType optionType = ((this.Team.Side == BattleSideEnum.Attacker) ? MultiplayerOptions.OptionType.CultureTeam1 : MultiplayerOptions.OptionType.CultureTeam2);
			this.Culture = MBObjectManager.Instance.GetObject<BasicCultureObject>(optionType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
		}

		// Token: 0x06002CDB RID: 11483 RVA: 0x000AC6E5 File Offset: 0x000AA8E5
		public int GetNumberOfTimesPeerKilledPeer(MissionPeer killedPeer)
		{
			if (this._numberOfTimesPeerKilledPerPeer.ContainsKey(killedPeer))
			{
				return this._numberOfTimesPeerKilledPerPeer[killedPeer];
			}
			return 0;
		}

		// Token: 0x06002CDC RID: 11484 RVA: 0x000AC703 File Offset: 0x000AA903
		public void ResetKillRegistry()
		{
			this._numberOfTimesPeerKilledPerPeer.Clear();
		}

		// Token: 0x06002CDD RID: 11485 RVA: 0x000AC710 File Offset: 0x000AA910
		public bool RefreshSelectedPerks()
		{
			MBList<MPPerkObject> mblist = new MBList<MPPerkObject>();
			List<List<IReadOnlyPerkObject>> availablePerksForPeer = MultiplayerClassDivisions.GetAvailablePerksForPeer(this);
			if (availablePerksForPeer.Count == 3)
			{
				for (int i = 0; i < 3; i++)
				{
					int num = this._perks[this.SelectedTroopIndex][i];
					if (availablePerksForPeer[i].Count > 0)
					{
						mblist.Add(availablePerksForPeer[i][(num >= 0 && num < availablePerksForPeer[i].Count) ? num : 0].Clone(this));
					}
				}
				this._selectedPerks = new ValueTuple<int, MBList<MPPerkObject>>(this.SelectedTroopIndex, mblist);
				return true;
			}
			return false;
		}

		// Token: 0x06002CDE RID: 11486 RVA: 0x000AC7A8 File Offset: 0x000AA9A8
		private void ResetSelectedPerks()
		{
			if (this._selectedPerks.Item2 != null)
			{
				foreach (MPPerkObject mpperkObject in this._selectedPerks.Item2)
				{
					mpperkObject.Reset();
				}
			}
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000AC80C File Offset: 0x000AAA0C
		private void CultureChanged(BasicCultureObject newCulture)
		{
			List<MultiplayerClassDivisions.MPHeroClass> list = MultiplayerClassDivisions.GetMPHeroClasses(newCulture).ToList<MultiplayerClassDivisions.MPHeroClass>();
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				MultiplayerClassDivisions.MPHeroClass mpheroClass = list[i];
				List<MPPerkSelectionManager.MPPerkSelection> selectionsForHeroClass = MPPerkSelectionManager.Instance.GetSelectionsForHeroClass(mpheroClass);
				if (selectionsForHeroClass != null)
				{
					int count2 = selectionsForHeroClass.Count;
					for (int j = 0; j < count2; j++)
					{
						MPPerkSelectionManager.MPPerkSelection mpperkSelection = selectionsForHeroClass[j];
						this._perks[i][mpperkSelection.ListIndex] = mpperkSelection.Index;
					}
				}
				else
				{
					for (int k = 0; k < 3; k++)
					{
						this._perks[i][k] = 0;
					}
				}
			}
			if (base.IsMine && GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new TeamInitialPerkInfoMessage(this._perks[this.SelectedTroopIndex]));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x000AC8EC File Offset: 0x000AAAEC
		public void OnTeamInitialPerkInfoReceived(int[] perks)
		{
			for (int i = 0; i < 3; i++)
			{
				this.SelectPerk(i, perks[i], -1);
			}
			this.TeamInitialPerkInfoReady = true;
		}

		// Token: 0x04001185 RID: 4485
		public const int NumberOfPerkLists = 3;

		// Token: 0x04001186 RID: 4486
		public const int MaxNumberOfTroopTypesPerCulture = 16;

		// Token: 0x04001187 RID: 4487
		private const float InactivityKickInSeconds = 180f;

		// Token: 0x04001188 RID: 4488
		private const float InactivityWarnInSeconds = 120f;

		// Token: 0x04001189 RID: 4489
		public const int MinKDACount = -1000;

		// Token: 0x0400118A RID: 4490
		public const int MaxKDACount = 100000;

		// Token: 0x0400118B RID: 4491
		public const int MinScore = -1000000;

		// Token: 0x0400118C RID: 4492
		public const int MaxScore = 1000000;

		// Token: 0x0400118D RID: 4493
		public const int MinSpawnTimer = 3;

		// Token: 0x0400118E RID: 4494
		public int CaptainBeingDetachedThreshold = 125;

		// Token: 0x04001195 RID: 4501
		private List<PeerVisualsHolder> _visuals = new List<PeerVisualsHolder>();

		// Token: 0x04001196 RID: 4502
		private Dictionary<MissionPeer, int> _numberOfTimesPeerKilledPerPeer = new Dictionary<MissionPeer, int>();

		// Token: 0x04001197 RID: 4503
		private MissionTime _lastActiveTime = MissionTime.Zero;

		// Token: 0x04001198 RID: 4504
		private ValueTuple<Agent.MovementControlFlag, Vec2, Vec3> _previousActivityStatus;

		// Token: 0x04001199 RID: 4505
		private bool _inactiveWarningGiven;

		// Token: 0x0400119E RID: 4510
		private int _selectedTroopIndex;

		// Token: 0x040011A0 RID: 4512
		private Agent _followedAgent;

		// Token: 0x040011A1 RID: 4513
		private Team _team;

		// Token: 0x040011A2 RID: 4514
		private BasicCultureObject _culture;

		// Token: 0x040011A3 RID: 4515
		private Formation _controlledFormation;

		// Token: 0x040011A4 RID: 4516
		private MissionRepresentativeBase _representative;

		// Token: 0x040011A5 RID: 4517
		private readonly MBList<int[]> _perks;

		// Token: 0x040011A6 RID: 4518
		private int _killCount;

		// Token: 0x040011A7 RID: 4519
		private int _assistCount;

		// Token: 0x040011A8 RID: 4520
		private int _deathCount;

		// Token: 0x040011A9 RID: 4521
		private int _score;

		// Token: 0x040011AE RID: 4526
		private readonly Dictionary<WeaponClass, int> _weaponUsageByClass = new Dictionary<WeaponClass, int>();

		// Token: 0x040011AF RID: 4527
		private WeaponClass _mostUsedWeaponClass;

		// Token: 0x040011B0 RID: 4528
		private ValueTuple<int, MBList<MPPerkObject>> _selectedPerks;

		// Token: 0x040011B8 RID: 4536
		private int _botsUnderControlAlive;

		// Token: 0x020005E8 RID: 1512
		// (Invoke) Token: 0x06003F89 RID: 16265
		public delegate void OnUpdateEquipmentSetIndexEventDelegate(MissionPeer lobbyPeer, int equipmentSetIndex);

		// Token: 0x020005E9 RID: 1513
		// (Invoke) Token: 0x06003F8D RID: 16269
		public delegate void OnPerkUpdateEventDelegate(MissionPeer peer);

		// Token: 0x020005EA RID: 1514
		// (Invoke) Token: 0x06003F91 RID: 16273
		public delegate void OnTeamChangedDelegate(NetworkCommunicator peer, Team previousTeam, Team newTeam);

		// Token: 0x020005EB RID: 1515
		// (Invoke) Token: 0x06003F95 RID: 16277
		public delegate void OnCultureChangedDelegate(BasicCultureObject newCulture);

		// Token: 0x020005EC RID: 1516
		// (Invoke) Token: 0x06003F99 RID: 16281
		public delegate void OnPlayerKilledDelegate(MissionPeer killerPeer, MissionPeer killedPeer);
	}
}
