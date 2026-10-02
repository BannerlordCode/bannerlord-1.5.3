using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.MissionRepresentatives;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000006 RID: 6
	public class MissionDuelMarkersVM : ViewModel
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002270 File Offset: 0x00000470
		public MissionDuelMarkersVM(Camera missionCamera, MissionMultiplayerGameModeDuelClient client)
		{
			this._missionCamera = missionCamera;
			this._client = client;
			List<GameEntity> list = new List<GameEntity>();
			list.AddRange(Mission.Current.Scene.FindEntitiesWithTag("duel_zone_landmark"));
			this.Landmarks = new MBBindingList<MissionDuelLandmarkMarkerVM>();
			foreach (GameEntity gameEntity in list)
			{
				this.Landmarks.Add(new MissionDuelLandmarkMarkerVM(gameEntity));
			}
			this.Targets = new MBBindingList<MissionDuelPeerMarkerVM>();
			this._targetPeersToMarkersDictionary = new Dictionary<MissionPeer, MissionDuelPeerMarkerVM>();
			this._targetPeersInDuelDictionary = new Dictionary<MissionPeer, bool>();
			this._distanceComparer = new MissionDuelMarkersVM.PeerMarkerDistanceComparer();
			this.UpdateScreenCenter();
			this.RefreshValues();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002340 File Offset: 0x00000540
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Targets.ApplyActionOnAllItems(delegate(MissionDuelPeerMarkerVM t)
			{
				t.RefreshValues();
			});
			this.Landmarks.ApplyActionOnAllItems(delegate(MissionDuelLandmarkMarkerVM l)
			{
				l.RefreshValues();
			});
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000023A7 File Offset: 0x000005A7
		public void UpdateScreenCenter()
		{
			this._screenCenter = new Vec2(Screen.RealScreenResolutionWidth / 2f, Screen.RealScreenResolutionHeight / 2f);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000023CA File Offset: 0x000005CA
		public void Tick(float dt)
		{
			if (this._hasEnteredLobby && GameNetwork.MyPeer != null)
			{
				this.OnRefreshPeerMarkers();
				this.UpdateTargets(dt);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000023E8 File Offset: 0x000005E8
		public void RegisterEvents()
		{
			DuelMissionRepresentative myRepresentative = this._client.MyRepresentative;
			myRepresentative.OnDuelRequestSentEvent = (Action<MissionPeer>)Delegate.Combine(myRepresentative.OnDuelRequestSentEvent, new Action<MissionPeer>(this.OnDuelRequestSent));
			DuelMissionRepresentative myRepresentative2 = this._client.MyRepresentative;
			myRepresentative2.OnDuelRequestedEvent = (Action<MissionPeer, TroopType>)Delegate.Combine(myRepresentative2.OnDuelRequestedEvent, new Action<MissionPeer, TroopType>(this.OnDuelRequested));
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionsChanged));
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002470 File Offset: 0x00000670
		public void UnregisterEvents()
		{
			DuelMissionRepresentative myRepresentative = this._client.MyRepresentative;
			myRepresentative.OnDuelRequestSentEvent = (Action<MissionPeer>)Delegate.Remove(myRepresentative.OnDuelRequestSentEvent, new Action<MissionPeer>(this.OnDuelRequestSent));
			DuelMissionRepresentative myRepresentative2 = this._client.MyRepresentative;
			myRepresentative2.OnDuelRequestedEvent = (Action<MissionPeer, TroopType>)Delegate.Remove(myRepresentative2.OnDuelRequestedEvent, new Action<MissionPeer, TroopType>(this.OnDuelRequested));
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionsChanged));
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000024F5 File Offset: 0x000006F5
		private void OnManagedOptionsChanged(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.EnableGenericNames)
			{
				this.Targets.ApplyActionOnAllItems(delegate(MissionDuelPeerMarkerVM t)
				{
					t.RefreshValues();
				});
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002528 File Offset: 0x00000728
		private void UpdateTargets(float dt)
		{
			if (this._currentFocusTarget != null)
			{
				this._previousFocusTarget = this._currentFocusTarget;
				this._currentFocusTarget = null;
				if (this._isPlayerFocused)
				{
					this._previousFocusTarget.IsFocused = false;
				}
			}
			if (this._currentLandmarkTarget != null)
			{
				this._previousLandmarkTarget = this._currentLandmarkTarget;
				this._currentLandmarkTarget = null;
				if (this._isPlayerFocused)
				{
					this._previousLandmarkTarget.IsFocused = false;
				}
			}
			DuelMissionRepresentative myRepresentative = this._client.MyRepresentative;
			if (((myRepresentative != null) ? myRepresentative.MissionPeer.ControlledAgent : null) != null)
			{
				float num = float.MaxValue;
				foreach (MissionDuelPeerMarkerVM missionDuelPeerMarkerVM in this.Targets)
				{
					missionDuelPeerMarkerVM.OnTick(dt);
					if (missionDuelPeerMarkerVM.IsEnabled)
					{
						if (!missionDuelPeerMarkerVM.HasSentDuelRequest && !missionDuelPeerMarkerVM.HasDuelRequestForPlayer && missionDuelPeerMarkerVM.TargetPeer.ControlledAgent != null)
						{
							missionDuelPeerMarkerVM.PreferredArenaType = this._playerPreferredArenaType;
						}
						missionDuelPeerMarkerVM.UpdateScreenPosition(this._missionCamera);
						missionDuelPeerMarkerVM.HasDuelRequestForPlayer = this._client.MyRepresentative.CheckHasRequestFromAndRemoveRequestIfNeeded(missionDuelPeerMarkerVM.TargetPeer);
						float num2 = missionDuelPeerMarkerVM.ScreenPosition.Distance(this._screenCenter);
						if (!this._isPlayerFocused && missionDuelPeerMarkerVM.WSign >= 0 && num2 < 350f && num2 < num)
						{
							num = num2;
							this._currentFocusTarget = missionDuelPeerMarkerVM;
						}
					}
				}
				this.Targets.Sort(this._distanceComparer);
				if (this._client.MyRepresentative != null)
				{
					if (this._currentFocusTarget != null && this._currentFocusTarget.TargetPeer.ControlledAgent != null)
					{
						this._client.MyRepresentative.OnObjectFocused(this._currentFocusTarget.TargetPeer.ControlledAgent);
						if (this._previousFocusTarget != null && this._currentFocusTarget.TargetPeer != this._previousFocusTarget.TargetPeer)
						{
							this._previousFocusTarget.IsFocused = false;
						}
						this._currentFocusTarget.IsFocused = true;
						if (this._previousLandmarkTarget != null)
						{
							this._previousLandmarkTarget.IsFocused = false;
							return;
						}
					}
					else
					{
						if (this._previousFocusTarget != null)
						{
							this._previousFocusTarget.IsFocused = false;
						}
						foreach (MissionDuelLandmarkMarkerVM missionDuelLandmarkMarkerVM in this.Landmarks)
						{
							if (Agent.Main != null)
							{
								missionDuelLandmarkMarkerVM.UpdateScreenPosition(this._missionCamera);
								if (!this._isPlayerFocused && missionDuelLandmarkMarkerVM.IsInScreenBoundaries && Agent.Main.GetWorldPosition().GetGroundVec3().DistanceSquared(missionDuelLandmarkMarkerVM.Entity.GlobalPosition) < 500f)
								{
									missionDuelLandmarkMarkerVM.IsFocused = true;
									this._currentLandmarkTarget = missionDuelLandmarkMarkerVM;
									if (this._previousLandmarkTarget != missionDuelLandmarkMarkerVM)
									{
										if (this._previousLandmarkTarget != null)
										{
											this._previousLandmarkTarget.IsFocused = false;
										}
										this._currentLandmarkTarget.IsFocused = true;
									}
									this._client.MyRepresentative.OnObjectFocused(missionDuelLandmarkMarkerVM.FocusableComponent);
									break;
								}
							}
						}
						if (this._currentLandmarkTarget == null && this._previousLandmarkTarget != null)
						{
							this._previousLandmarkTarget.IsFocused = false;
						}
						if (this._currentFocusTarget == null && this._currentLandmarkTarget == null)
						{
							this._client.MyRepresentative.OnObjectFocusLost();
						}
					}
				}
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002888 File Offset: 0x00000A88
		public void RefreshPeerEquipments()
		{
			foreach (MissionPeer missionPeer in VirtualPlayer.Peers<MissionPeer>())
			{
				this.OnPeerEquipmentRefreshed(missionPeer);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000028DC File Offset: 0x00000ADC
		private void OnRefreshPeerMarkers()
		{
			List<MissionDuelPeerMarkerVM> list = this.Targets.ToList<MissionDuelPeerMarkerVM>();
			foreach (MissionPeer missionPeer in VirtualPlayer.Peers<MissionPeer>())
			{
				if (((missionPeer != null) ? missionPeer.Team : null) != null && missionPeer.IsControlledAgentActive && !missionPeer.IsMine)
				{
					if (!this._targetPeersToMarkersDictionary.ContainsKey(missionPeer))
					{
						MissionDuelPeerMarkerVM missionDuelPeerMarkerVM = new MissionDuelPeerMarkerVM(missionPeer);
						this.Targets.Add(missionDuelPeerMarkerVM);
						this._targetPeersToMarkersDictionary.Add(missionPeer, missionDuelPeerMarkerVM);
						this.OnPeerEquipmentRefreshed(missionPeer);
						if (this._targetPeersInDuelDictionary.ContainsKey(missionPeer))
						{
							missionDuelPeerMarkerVM.UpdateCurentDuelStatus(this._targetPeersInDuelDictionary[missionPeer]);
						}
					}
					else
					{
						list.Remove(this._targetPeersToMarkersDictionary[missionPeer]);
					}
					if (!this._targetPeersInDuelDictionary.ContainsKey(missionPeer))
					{
						this._targetPeersInDuelDictionary.Add(missionPeer, false);
					}
				}
			}
			foreach (MissionDuelPeerMarkerVM missionDuelPeerMarkerVM2 in list)
			{
				this.Targets.Remove(missionDuelPeerMarkerVM2);
				this._targetPeersToMarkersDictionary.Remove(missionDuelPeerMarkerVM2.TargetPeer);
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002A40 File Offset: 0x00000C40
		private void UpdateTargetsEnabled(bool isEnabled)
		{
			foreach (MissionDuelPeerMarkerVM missionDuelPeerMarkerVM in this.Targets)
			{
				missionDuelPeerMarkerVM.IsEnabled = !missionDuelPeerMarkerVM.IsInDuel && isEnabled;
			}
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002A98 File Offset: 0x00000C98
		private void OnDuelRequestSent(MissionPeer targetPeer)
		{
			foreach (MissionDuelPeerMarkerVM missionDuelPeerMarkerVM in this.Targets)
			{
				if (missionDuelPeerMarkerVM.TargetPeer == targetPeer)
				{
					missionDuelPeerMarkerVM.HasSentDuelRequest = true;
				}
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002AF0 File Offset: 0x00000CF0
		private void OnDuelRequested(MissionPeer targetPeer, TroopType troopType)
		{
			MissionDuelPeerMarkerVM missionDuelPeerMarkerVM = this.Targets.FirstOrDefault<MissionDuelPeerMarkerVM>((MissionDuelPeerMarkerVM t) => t.TargetPeer == targetPeer);
			if (missionDuelPeerMarkerVM != null)
			{
				missionDuelPeerMarkerVM.HasDuelRequestForPlayer = true;
				missionDuelPeerMarkerVM.PreferredArenaType = (int)troopType;
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002B33 File Offset: 0x00000D33
		public void OnAgentSpawnedWithoutDuel()
		{
			this._hasEnteredLobby = true;
			this.IsEnabled = true;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002B43 File Offset: 0x00000D43
		public void OnAgentBuiltForTheFirstTime()
		{
			this._playerPreferredArenaType = (int)MultiplayerDuelVM.GetAgentDefaultPreferredArenaType(Agent.Main);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002B58 File Offset: 0x00000D58
		public void OnDuelStarted(MissionPeer firstPeer, MissionPeer secondPeer)
		{
			if (this._client.MyRepresentative.MissionPeer == firstPeer || this._client.MyRepresentative.MissionPeer == secondPeer)
			{
				this.IsEnabled = false;
			}
			foreach (MissionDuelPeerMarkerVM missionDuelPeerMarkerVM in this.Targets)
			{
				if (missionDuelPeerMarkerVM.TargetPeer == firstPeer || missionDuelPeerMarkerVM.TargetPeer == secondPeer)
				{
					missionDuelPeerMarkerVM.OnDuelStarted();
				}
			}
			this._targetPeersInDuelDictionary[firstPeer] = true;
			this._targetPeersInDuelDictionary[secondPeer] = true;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002C00 File Offset: 0x00000E00
		public void SetMarkerOfPeerEnabled(MissionPeer peer, bool isEnabled)
		{
			if (peer != null)
			{
				if (this._targetPeersToMarkersDictionary.ContainsKey(peer))
				{
					this._targetPeersToMarkersDictionary[peer].UpdateCurentDuelStatus(!isEnabled);
					this._targetPeersToMarkersDictionary[peer].UpdateBounty();
				}
				if (this._targetPeersInDuelDictionary.ContainsKey(peer))
				{
					this._targetPeersInDuelDictionary[peer] = !isEnabled;
				}
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002C62 File Offset: 0x00000E62
		public void OnPlayerPreferredZoneChanged(int playerPrefferedArenaType)
		{
			this._playerPreferredArenaType = playerPrefferedArenaType;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002C6B File Offset: 0x00000E6B
		public void OnFocusGained()
		{
			this._isPlayerFocused = true;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002C74 File Offset: 0x00000E74
		public void OnFocusLost()
		{
			this._isPlayerFocused = false;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002C80 File Offset: 0x00000E80
		public void OnPeerEquipmentRefreshed(MissionPeer peer)
		{
			MissionDuelPeerMarkerVM missionDuelPeerMarkerVM;
			if (this._targetPeersToMarkersDictionary.TryGetValue(peer, out missionDuelPeerMarkerVM))
			{
				missionDuelPeerMarkerVM.RefreshPerkSelection();
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002CA3 File Offset: 0x00000EA3
		// (set) Token: 0x06000025 RID: 37 RVA: 0x00002CAB File Offset: 0x00000EAB
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
					this.UpdateTargetsEnabled(value);
				}
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002CD0 File Offset: 0x00000ED0
		// (set) Token: 0x06000027 RID: 39 RVA: 0x00002CD8 File Offset: 0x00000ED8
		[DataSourceProperty]
		public MBBindingList<MissionDuelPeerMarkerVM> Targets
		{
			get
			{
				return this._targets;
			}
			set
			{
				if (value != this._targets)
				{
					this._targets = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionDuelPeerMarkerVM>>(value, "Targets");
				}
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00002CF6 File Offset: 0x00000EF6
		// (set) Token: 0x06000029 RID: 41 RVA: 0x00002CFE File Offset: 0x00000EFE
		[DataSourceProperty]
		public MBBindingList<MissionDuelLandmarkMarkerVM> Landmarks
		{
			get
			{
				return this._landmarks;
			}
			set
			{
				if (value != this._landmarks)
				{
					this._landmarks = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionDuelLandmarkMarkerVM>>(value, "Landmarks");
				}
			}
		}

		// Token: 0x0400000A RID: 10
		private const string ZoneLandmarkTag = "duel_zone_landmark";

		// Token: 0x0400000B RID: 11
		private const float FocusScreenDistanceThreshold = 350f;

		// Token: 0x0400000C RID: 12
		private const float LandmarkFocusDistanceThrehsold = 500f;

		// Token: 0x0400000D RID: 13
		private bool _hasEnteredLobby;

		// Token: 0x0400000E RID: 14
		private Camera _missionCamera;

		// Token: 0x0400000F RID: 15
		private MissionDuelPeerMarkerVM _previousFocusTarget;

		// Token: 0x04000010 RID: 16
		private MissionDuelPeerMarkerVM _currentFocusTarget;

		// Token: 0x04000011 RID: 17
		private MissionDuelLandmarkMarkerVM _previousLandmarkTarget;

		// Token: 0x04000012 RID: 18
		private MissionDuelLandmarkMarkerVM _currentLandmarkTarget;

		// Token: 0x04000013 RID: 19
		private MissionDuelMarkersVM.PeerMarkerDistanceComparer _distanceComparer;

		// Token: 0x04000014 RID: 20
		private readonly Dictionary<MissionPeer, MissionDuelPeerMarkerVM> _targetPeersToMarkersDictionary;

		// Token: 0x04000015 RID: 21
		private readonly MissionMultiplayerGameModeDuelClient _client;

		// Token: 0x04000016 RID: 22
		private Vec2 _screenCenter;

		// Token: 0x04000017 RID: 23
		private Dictionary<MissionPeer, bool> _targetPeersInDuelDictionary;

		// Token: 0x04000018 RID: 24
		private int _playerPreferredArenaType;

		// Token: 0x04000019 RID: 25
		private bool _isPlayerFocused;

		// Token: 0x0400001A RID: 26
		private bool _isEnabled;

		// Token: 0x0400001B RID: 27
		private MBBindingList<MissionDuelPeerMarkerVM> _targets;

		// Token: 0x0400001C RID: 28
		private MBBindingList<MissionDuelLandmarkMarkerVM> _landmarks;

		// Token: 0x020000B9 RID: 185
		private class PeerMarkerDistanceComparer : IComparer<MissionDuelPeerMarkerVM>
		{
			// Token: 0x06001162 RID: 4450 RVA: 0x0003692C File Offset: 0x00034B2C
			public int Compare(MissionDuelPeerMarkerVM x, MissionDuelPeerMarkerVM y)
			{
				return y.Distance.CompareTo(x.Distance);
			}
		}
	}
}
