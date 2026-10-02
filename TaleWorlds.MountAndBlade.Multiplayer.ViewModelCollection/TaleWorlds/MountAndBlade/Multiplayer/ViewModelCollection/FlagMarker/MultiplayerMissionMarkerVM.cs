using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker.Targets;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FlagMarker
{
	// Token: 0x0200009B RID: 155
	public class MultiplayerMissionMarkerVM : ViewModel
	{
		// Token: 0x06000F6B RID: 3947 RVA: 0x00030028 File Offset: 0x0002E228
		public MultiplayerMissionMarkerVM(Camera missionCamera)
		{
			this._missionCamera = missionCamera;
			this.FlagTargets = new MBBindingList<MissionFlagMarkerTargetVM>();
			this.PeerTargets = new MBBindingList<MissionPeerMarkerTargetVM>();
			this.SiegeEngineTargets = new MBBindingList<MissionSiegeEngineMarkerTargetVM>();
			this.AlwaysVisibleTargets = new MBBindingList<MissionAlwaysVisibleMarkerTargetVM>();
			this._teammateDictionary = new Dictionary<MissionPeer, MissionPeerMarkerTargetVM>();
			this._distanceComparer = new MultiplayerMissionMarkerVM.MarkerDistanceComparer();
			this._commanderInfo = Mission.Current.GetMissionBehavior<ICommanderInfo>();
			if (this._commanderInfo != null)
			{
				this._commanderInfo.OnFlagNumberChangedEvent += this.OnFlagNumberChangedEvent;
				this._commanderInfo.OnCapturePointOwnerChangedEvent += this.OnCapturePointOwnerChangedEvent;
				this.OnFlagNumberChangedEvent();
				this._siegeClient = Mission.Current.GetMissionBehavior<MissionMultiplayerSiegeClient>();
				if (this._siegeClient != null)
				{
					this._siegeClient.OnCapturePointRemainingMoraleGainsChangedEvent += this.OnCapturePointRemainingMoraleGainsChanged;
				}
			}
			MissionPeer.OnTeamChanged += this.OnTeamChanged;
			this._friendIDs = new List<PlayerId>();
			foreach (IFriendListService friendListService in PlatformServices.Instance.GetFriendListServices())
			{
				this._friendIDs.AddRange(friendListService.GetAllFriends());
			}
		}

		// Token: 0x06000F6C RID: 3948 RVA: 0x0003014C File Offset: 0x0002E34C
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this._commanderInfo != null)
			{
				this._commanderInfo.OnFlagNumberChangedEvent -= this.OnFlagNumberChangedEvent;
				this._commanderInfo.OnCapturePointOwnerChangedEvent -= this.OnCapturePointOwnerChangedEvent;
				if (this._siegeClient != null)
				{
					this._siegeClient.OnCapturePointRemainingMoraleGainsChangedEvent -= this.OnCapturePointRemainingMoraleGainsChanged;
				}
			}
			MissionPeer.OnTeamChanged -= this.OnTeamChanged;
		}

		// Token: 0x06000F6D RID: 3949 RVA: 0x000301C8 File Offset: 0x0002E3C8
		public void Tick(float dt)
		{
			this.OnRefreshPeerMarkers();
			this.UpdateAlwaysVisibleTargetScreenPosition();
			if (this.IsEnabled)
			{
				this.UpdateTargetScreenPositions();
				this._fadeOutTimerStarted = false;
				this._fadeOutTimer = 0f;
				this._prevEnabledState = this.IsEnabled;
			}
			else
			{
				if (this._prevEnabledState)
				{
					this._fadeOutTimerStarted = true;
				}
				if (this._fadeOutTimerStarted)
				{
					this._fadeOutTimer += dt;
				}
				if (this._fadeOutTimer < 2f)
				{
					this.UpdateTargetScreenPositions();
				}
				else
				{
					this._fadeOutTimerStarted = false;
				}
			}
			this._prevEnabledState = this.IsEnabled;
		}

		// Token: 0x06000F6E RID: 3950 RVA: 0x0003025C File Offset: 0x0002E45C
		private void OnCapturePointRemainingMoraleGainsChanged(int[] remainingMoraleGainsArr)
		{
			foreach (MissionFlagMarkerTargetVM missionFlagMarkerTargetVM in this.FlagTargets)
			{
				int flagIndex = missionFlagMarkerTargetVM.TargetFlag.FlagIndex;
				if (flagIndex >= 0 && flagIndex < remainingMoraleGainsArr.Length)
				{
					missionFlagMarkerTargetVM.OnRemainingMoraleChanged(remainingMoraleGainsArr[flagIndex]);
				}
			}
		}

		// Token: 0x06000F6F RID: 3951 RVA: 0x000302C4 File Offset: 0x0002E4C4
		private void OnTeamChanged(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			if (this._commanderInfo != null)
			{
				this.OnFlagNumberChangedEvent();
			}
			if (peer.IsMine)
			{
				this.SiegeEngineTargets.Clear();
				foreach (WeakGameEntity weakGameEntity in Mission.Current.GetActiveEntitiesWithScriptComponentOfType<SiegeWeapon>())
				{
					SiegeWeapon firstScriptOfType = weakGameEntity.GetFirstScriptOfType<SiegeWeapon>();
					if (newTeam.Side == firstScriptOfType.Side)
					{
						this.SiegeEngineTargets.Add(new MissionSiegeEngineMarkerTargetVM(firstScriptOfType));
					}
				}
			}
		}

		// Token: 0x06000F70 RID: 3952 RVA: 0x00030358 File Offset: 0x0002E558
		private void UpdateTargetScreenPositions()
		{
			this.PeerTargets.ApplyActionOnAllItems(delegate(MissionPeerMarkerTargetVM pt)
			{
				pt.UpdateScreenPosition(this._missionCamera);
			});
			this.FlagTargets.ApplyActionOnAllItems(delegate(MissionFlagMarkerTargetVM ft)
			{
				ft.UpdateScreenPosition(this._missionCamera);
			});
			this.SiegeEngineTargets.ApplyActionOnAllItems(delegate(MissionSiegeEngineMarkerTargetVM st)
			{
				st.UpdateScreenPosition(this._missionCamera);
			});
			this.PeerTargets.Sort(this._distanceComparer);
			this.FlagTargets.Sort(this._distanceComparer);
			this.SiegeEngineTargets.Sort(this._distanceComparer);
		}

		// Token: 0x06000F71 RID: 3953 RVA: 0x000303E0 File Offset: 0x0002E5E0
		private void UpdateAlwaysVisibleTargetScreenPosition()
		{
			foreach (MissionAlwaysVisibleMarkerTargetVM missionAlwaysVisibleMarkerTargetVM in this.AlwaysVisibleTargets)
			{
				missionAlwaysVisibleMarkerTargetVM.UpdateScreenPosition(this._missionCamera);
			}
		}

		// Token: 0x06000F72 RID: 3954 RVA: 0x00030430 File Offset: 0x0002E630
		private void OnFlagNumberChangedEvent()
		{
			this.ResetCapturePointLists();
			this.InitCapturePoints();
		}

		// Token: 0x06000F73 RID: 3955 RVA: 0x00030440 File Offset: 0x0002E640
		private void InitCapturePoints()
		{
			if (this._commanderInfo != null)
			{
				foreach (FlagCapturePoint flagCapturePoint in this._commanderInfo.AllCapturePoints.Where<FlagCapturePoint>((FlagCapturePoint c) => !c.IsDeactivated).ToArray<FlagCapturePoint>())
				{
					MissionFlagMarkerTargetVM missionFlagMarkerTargetVM = new MissionFlagMarkerTargetVM(flagCapturePoint);
					this.FlagTargets.Add(missionFlagMarkerTargetVM);
					missionFlagMarkerTargetVM.OnOwnerChanged(this._commanderInfo.GetFlagOwner(flagCapturePoint));
					missionFlagMarkerTargetVM.IsEnabled = this.IsEnabled;
				}
			}
		}

		// Token: 0x06000F74 RID: 3956 RVA: 0x000304CD File Offset: 0x0002E6CD
		private void ResetCapturePointLists()
		{
			this.FlagTargets.Clear();
		}

		// Token: 0x06000F75 RID: 3957 RVA: 0x000304DC File Offset: 0x0002E6DC
		private void OnCapturePointOwnerChangedEvent(FlagCapturePoint flag, Team team)
		{
			foreach (MissionFlagMarkerTargetVM missionFlagMarkerTargetVM in this.FlagTargets)
			{
				if (missionFlagMarkerTargetVM.TargetFlag == flag)
				{
					missionFlagMarkerTargetVM.OnOwnerChanged(team);
				}
			}
		}

		// Token: 0x06000F76 RID: 3958 RVA: 0x00030534 File Offset: 0x0002E734
		private void OnRefreshPeerMarkers()
		{
			if (GameNetwork.MyPeer == null)
			{
				return;
			}
			MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			BattleSideEnum? battleSideEnum;
			if (component == null)
			{
				battleSideEnum = null;
			}
			else
			{
				Team team = component.Team;
				battleSideEnum = ((team != null) ? new BattleSideEnum?(team.Side) : null);
			}
			BattleSideEnum battleSideEnum2 = battleSideEnum ?? BattleSideEnum.None;
			bool flag = MultiplayerSpectatorHelper.ShouldShowBothTeamsData();
			List<MissionPeerMarkerTargetVM> list = this.PeerTargets.ToList<MissionPeerMarkerTargetVM>();
			using (List<MissionPeer>.Enumerator enumerator = VirtualPlayer.Peers<MissionPeer>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MissionPeer missionPeer = enumerator.Current;
					MissionPeer missionPeer2 = missionPeer;
					if (((missionPeer2 != null) ? missionPeer2.Team : null) != null && !missionPeer.IsMine && (missionPeer.Team.Side == battleSideEnum2 || flag) && missionPeer.Team != Mission.Current.SpectatorTeam)
					{
						IEnumerable<MissionPeerMarkerTargetVM> enumerable = this.PeerTargets.Where<MissionPeerMarkerTargetVM>(delegate(MissionPeerMarkerTargetVM t)
						{
							MissionPeer targetPeer2 = t.TargetPeer;
							return targetPeer2 != null && targetPeer2.Peer.Id.Equals(missionPeer.Peer.Id);
						});
						if (enumerable.Any<MissionPeerMarkerTargetVM>())
						{
							MissionPeerMarkerTargetVM currentMarker = enumerable.First<MissionPeerMarkerTargetVM>();
							IEnumerable<MissionAlwaysVisibleMarkerTargetVM> enumerable2 = this.AlwaysVisibleTargets.Where<MissionAlwaysVisibleMarkerTargetVM>((MissionAlwaysVisibleMarkerTargetVM t) => t.TargetPeer.Peer.Id.Equals(currentMarker.TargetPeer.Peer.Id));
							if (BannerlordConfig.EnableDeathIcon && !missionPeer.IsControlledAgentActive)
							{
								if (enumerable2.Any<MissionAlwaysVisibleMarkerTargetVM>())
								{
									continue;
								}
								MissionPeer targetPeer = enumerable.First<MissionPeerMarkerTargetVM>().TargetPeer;
								if (((targetPeer != null) ? targetPeer.ControlledAgent : null) != null)
								{
									MissionAlwaysVisibleMarkerTargetVM missionAlwaysVisibleMarkerTargetVM = new MissionAlwaysVisibleMarkerTargetVM(currentMarker.TargetPeer, enumerable.First<MissionPeerMarkerTargetVM>().WorldPosition, new Action<MissionAlwaysVisibleMarkerTargetVM>(this.OnRemoveAlwaysVisibleMarker));
									missionAlwaysVisibleMarkerTargetVM.UpdateScreenPosition(this._missionCamera);
									this.AlwaysVisibleTargets.Add(missionAlwaysVisibleMarkerTargetVM);
									continue;
								}
								continue;
							}
						}
						if (!this._teammateDictionary.ContainsKey(missionPeer))
						{
							MissionPeerMarkerTargetVM missionPeerMarkerTargetVM = new MissionPeerMarkerTargetVM(missionPeer, this._friendIDs.Contains(missionPeer.Peer.Id));
							this.PeerTargets.Add(missionPeerMarkerTargetVM);
							this._teammateDictionary.Add(missionPeer, missionPeerMarkerTargetVM);
						}
						else
						{
							list.Remove(this._teammateDictionary[missionPeer]);
						}
					}
				}
			}
			using (List<MissionPeerMarkerTargetVM>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					MissionPeerMarkerTargetVM missionPeerMarkerTargetVM2;
					if ((missionPeerMarkerTargetVM2 = enumerator2.Current) != null)
					{
						this.PeerTargets.Remove(missionPeerMarkerTargetVM2);
						this._teammateDictionary.Remove(missionPeerMarkerTargetVM2.TargetPeer);
					}
				}
			}
		}

		// Token: 0x06000F77 RID: 3959 RVA: 0x00030828 File Offset: 0x0002EA28
		public void OnRemoveAlwaysVisibleMarker(MissionAlwaysVisibleMarkerTargetVM marker)
		{
			this.AlwaysVisibleTargets.Remove(marker);
		}

		// Token: 0x06000F78 RID: 3960 RVA: 0x00030838 File Offset: 0x0002EA38
		private void UpdateTargetStates(bool state)
		{
			this.PeerTargets.ApplyActionOnAllItems(delegate(MissionPeerMarkerTargetVM pt)
			{
				pt.IsEnabled = state;
			});
			this.FlagTargets.ApplyActionOnAllItems(delegate(MissionFlagMarkerTargetVM ft)
			{
				ft.IsEnabled = state;
			});
			this.SiegeEngineTargets.ApplyActionOnAllItems(delegate(MissionSiegeEngineMarkerTargetVM st)
			{
				st.IsEnabled = state;
			});
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06000F79 RID: 3961 RVA: 0x00030897 File Offset: 0x0002EA97
		// (set) Token: 0x06000F7A RID: 3962 RVA: 0x0003089F File Offset: 0x0002EA9F
		[DataSourceProperty]
		public MBBindingList<MissionFlagMarkerTargetVM> FlagTargets
		{
			get
			{
				return this._flagTargets;
			}
			set
			{
				if (value != this._flagTargets)
				{
					this._flagTargets = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionFlagMarkerTargetVM>>(value, "FlagTargets");
				}
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06000F7B RID: 3963 RVA: 0x000308BD File Offset: 0x0002EABD
		// (set) Token: 0x06000F7C RID: 3964 RVA: 0x000308C5 File Offset: 0x0002EAC5
		[DataSourceProperty]
		public MBBindingList<MissionPeerMarkerTargetVM> PeerTargets
		{
			get
			{
				return this._peerTargets;
			}
			set
			{
				if (value != this._peerTargets)
				{
					this._peerTargets = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionPeerMarkerTargetVM>>(value, "PeerTargets");
				}
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06000F7D RID: 3965 RVA: 0x000308E3 File Offset: 0x0002EAE3
		// (set) Token: 0x06000F7E RID: 3966 RVA: 0x000308EB File Offset: 0x0002EAEB
		[DataSourceProperty]
		public MBBindingList<MissionSiegeEngineMarkerTargetVM> SiegeEngineTargets
		{
			get
			{
				return this._siegeEngineTargets;
			}
			set
			{
				if (value != this._siegeEngineTargets)
				{
					this._siegeEngineTargets = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionSiegeEngineMarkerTargetVM>>(value, "SiegeEngineTargets");
				}
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06000F7F RID: 3967 RVA: 0x00030909 File Offset: 0x0002EB09
		// (set) Token: 0x06000F80 RID: 3968 RVA: 0x00030911 File Offset: 0x0002EB11
		[DataSourceProperty]
		public MBBindingList<MissionAlwaysVisibleMarkerTargetVM> AlwaysVisibleTargets
		{
			get
			{
				return this._alwaysVisibleTargets;
			}
			set
			{
				if (value != this._alwaysVisibleTargets)
				{
					this._alwaysVisibleTargets = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionAlwaysVisibleMarkerTargetVM>>(value, "AlwaysVisibleTargets");
				}
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06000F81 RID: 3969 RVA: 0x0003092F File Offset: 0x0002EB2F
		// (set) Token: 0x06000F82 RID: 3970 RVA: 0x00030937 File Offset: 0x0002EB37
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
					this.UpdateTargetStates(value);
				}
			}
		}

		// Token: 0x0400072D RID: 1837
		private readonly Camera _missionCamera;

		// Token: 0x0400072E RID: 1838
		private bool _prevEnabledState;

		// Token: 0x0400072F RID: 1839
		private bool _fadeOutTimerStarted;

		// Token: 0x04000730 RID: 1840
		private float _fadeOutTimer;

		// Token: 0x04000731 RID: 1841
		private MultiplayerMissionMarkerVM.MarkerDistanceComparer _distanceComparer;

		// Token: 0x04000732 RID: 1842
		private readonly ICommanderInfo _commanderInfo;

		// Token: 0x04000733 RID: 1843
		private readonly Dictionary<MissionPeer, MissionPeerMarkerTargetVM> _teammateDictionary;

		// Token: 0x04000734 RID: 1844
		private readonly MissionMultiplayerSiegeClient _siegeClient;

		// Token: 0x04000735 RID: 1845
		private readonly List<PlayerId> _friendIDs;

		// Token: 0x04000736 RID: 1846
		private MBBindingList<MissionFlagMarkerTargetVM> _flagTargets;

		// Token: 0x04000737 RID: 1847
		private MBBindingList<MissionPeerMarkerTargetVM> _peerTargets;

		// Token: 0x04000738 RID: 1848
		private MBBindingList<MissionSiegeEngineMarkerTargetVM> _siegeEngineTargets;

		// Token: 0x04000739 RID: 1849
		private MBBindingList<MissionAlwaysVisibleMarkerTargetVM> _alwaysVisibleTargets;

		// Token: 0x0400073A RID: 1850
		private bool _isEnabled;

		// Token: 0x0200018B RID: 395
		public class MarkerDistanceComparer : IComparer<MissionMarkerTargetVM>
		{
			// Token: 0x06001375 RID: 4981 RVA: 0x0003E4A8 File Offset: 0x0003C6A8
			public int Compare(MissionMarkerTargetVM x, MissionMarkerTargetVM y)
			{
				return y.Distance.CompareTo(x.Distance);
			}
		}
	}
}
