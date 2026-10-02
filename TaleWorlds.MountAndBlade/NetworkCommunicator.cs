using System;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000325 RID: 805
	public sealed class NetworkCommunicator : ICommunicator
	{
		// Token: 0x14000097 RID: 151
		// (add) Token: 0x06002DF2 RID: 11762 RVA: 0x000B2AF8 File Offset: 0x000B0CF8
		// (remove) Token: 0x06002DF3 RID: 11763 RVA: 0x000B2B2C File Offset: 0x000B0D2C
		public static event Action<PeerComponent> OnPeerComponentAdded;

		// Token: 0x14000098 RID: 152
		// (add) Token: 0x06002DF4 RID: 11764 RVA: 0x000B2B60 File Offset: 0x000B0D60
		// (remove) Token: 0x06002DF5 RID: 11765 RVA: 0x000B2B94 File Offset: 0x000B0D94
		public static event Action<NetworkCommunicator> OnPeerSynchronized;

		// Token: 0x14000099 RID: 153
		// (add) Token: 0x06002DF6 RID: 11766 RVA: 0x000B2BC8 File Offset: 0x000B0DC8
		// (remove) Token: 0x06002DF7 RID: 11767 RVA: 0x000B2BFC File Offset: 0x000B0DFC
		public static event Action<NetworkCommunicator> OnPeerAveragePingUpdated;

		// Token: 0x17000886 RID: 2182
		// (get) Token: 0x06002DF8 RID: 11768 RVA: 0x000B2C2F File Offset: 0x000B0E2F
		public VirtualPlayer VirtualPlayer { get; }

		// Token: 0x17000887 RID: 2183
		// (get) Token: 0x06002DF9 RID: 11769 RVA: 0x000B2C37 File Offset: 0x000B0E37
		// (set) Token: 0x06002DFA RID: 11770 RVA: 0x000B2C3F File Offset: 0x000B0E3F
		public PlayerConnectionInfo PlayerConnectionInfo { get; private set; }

		// Token: 0x17000888 RID: 2184
		// (get) Token: 0x06002DFB RID: 11771 RVA: 0x000B2C48 File Offset: 0x000B0E48
		// (set) Token: 0x06002DFC RID: 11772 RVA: 0x000B2C50 File Offset: 0x000B0E50
		public bool QuitFromMission { get; set; }

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x06002DFD RID: 11773 RVA: 0x000B2C59 File Offset: 0x000B0E59
		// (set) Token: 0x06002DFE RID: 11774 RVA: 0x000B2C61 File Offset: 0x000B0E61
		public int SessionKey { get; internal set; }

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06002DFF RID: 11775 RVA: 0x000B2C6A File Offset: 0x000B0E6A
		// (set) Token: 0x06002E00 RID: 11776 RVA: 0x000B2C72 File Offset: 0x000B0E72
		public bool JustReconnecting { get; private set; }

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06002E01 RID: 11777 RVA: 0x000B2C7B File Offset: 0x000B0E7B
		// (set) Token: 0x06002E02 RID: 11778 RVA: 0x000B2C83 File Offset: 0x000B0E83
		public double AveragePingInMilliseconds
		{
			get
			{
				return this._averagePingInMilliseconds;
			}
			private set
			{
				if (value != this._averagePingInMilliseconds)
				{
					this._averagePingInMilliseconds = value;
					Action<NetworkCommunicator> onPeerAveragePingUpdated = NetworkCommunicator.OnPeerAveragePingUpdated;
					if (onPeerAveragePingUpdated == null)
					{
						return;
					}
					onPeerAveragePingUpdated(this);
				}
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002E03 RID: 11779 RVA: 0x000B2CA5 File Offset: 0x000B0EA5
		// (set) Token: 0x06002E04 RID: 11780 RVA: 0x000B2CAD File Offset: 0x000B0EAD
		public double AverageLossPercent
		{
			get
			{
				return this._averageLossPercent;
			}
			private set
			{
				if (value != this._averageLossPercent)
				{
					this._averageLossPercent = value;
				}
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002E05 RID: 11781 RVA: 0x000B2CBF File Offset: 0x000B0EBF
		public bool IsMine
		{
			get
			{
				return GameNetwork.MyPeer == this;
			}
		}

		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06002E06 RID: 11782 RVA: 0x000B2CC9 File Offset: 0x000B0EC9
		// (set) Token: 0x06002E07 RID: 11783 RVA: 0x000B2CD1 File Offset: 0x000B0ED1
		public bool IsAdmin { get; private set; }

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06002E08 RID: 11784 RVA: 0x000B2CDA File Offset: 0x000B0EDA
		// (set) Token: 0x06002E09 RID: 11785 RVA: 0x000B2CE2 File Offset: 0x000B0EE2
		public bool IsSpectator { get; private set; }

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06002E0A RID: 11786 RVA: 0x000B2CEB File Offset: 0x000B0EEB
		public int Index
		{
			get
			{
				return this.VirtualPlayer.Index;
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06002E0B RID: 11787 RVA: 0x000B2CF8 File Offset: 0x000B0EF8
		public string UserName
		{
			get
			{
				return this.VirtualPlayer.UserName;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06002E0C RID: 11788 RVA: 0x000B2D05 File Offset: 0x000B0F05
		// (set) Token: 0x06002E0D RID: 11789 RVA: 0x000B2D10 File Offset: 0x000B0F10
		public Agent ControlledAgent
		{
			get
			{
				return this._controlledAgent;
			}
			set
			{
				this._controlledAgent = value;
				if (GameNetwork.IsServer)
				{
					Mission mission = ((value != null) ? value.Mission : null);
					UIntPtr uintPtr = ((mission != null) ? mission.Pointer : UIntPtr.Zero);
					int num = ((value == null) ? (-1) : value.Index);
					MBAPI.IMBPeer.SetControlledAgent(this.Index, uintPtr, num);
				}
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06002E0E RID: 11790 RVA: 0x000B2D68 File Offset: 0x000B0F68
		// (set) Token: 0x06002E0F RID: 11791 RVA: 0x000B2D70 File Offset: 0x000B0F70
		public bool IsMuted { get; set; }

		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06002E10 RID: 11792 RVA: 0x000B2D79 File Offset: 0x000B0F79
		// (set) Token: 0x06002E11 RID: 11793 RVA: 0x000B2D81 File Offset: 0x000B0F81
		public int ForcedAvatarIndex { get; set; } = -1;

		// Token: 0x17000895 RID: 2197
		// (get) Token: 0x06002E12 RID: 11794 RVA: 0x000B2D8A File Offset: 0x000B0F8A
		public bool IsNetworkActive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x06002E13 RID: 11795 RVA: 0x000B2D8D File Offset: 0x000B0F8D
		public bool IsConnectionActive
		{
			get
			{
				return GameNetwork.VirtualPlayers[this.Index] == this.VirtualPlayer && MBAPI.IMBPeer.IsActive(this.Index);
			}
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06002E14 RID: 11796 RVA: 0x000B2DB5 File Offset: 0x000B0FB5
		// (set) Token: 0x06002E15 RID: 11797 RVA: 0x000B2DEC File Offset: 0x000B0FEC
		public bool IsSynchronized
		{
			get
			{
				if (GameNetwork.IsServer)
				{
					return GameNetwork.VirtualPlayers[this.Index] == this.VirtualPlayer && MBAPI.IMBPeer.GetIsSynchronized(this.Index);
				}
				return this._isSynchronized;
			}
			set
			{
				if (value != this._isSynchronized || this.JustReconnecting)
				{
					if (GameNetwork.IsServer)
					{
						MBAPI.IMBPeer.SetIsSynchronized(this.Index, value);
					}
					this._isSynchronized = value;
					if (this._isSynchronized)
					{
						this.JustReconnecting = false;
						Action<NetworkCommunicator> onPeerSynchronized = NetworkCommunicator.OnPeerSynchronized;
						if (onPeerSynchronized != null)
						{
							onPeerSynchronized(this);
						}
					}
					if (GameNetwork.IsServer && !this.IsServerPeer)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SynchronizingDone(this, value));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeTargetPlayer, this);
						GameNetwork.BeginModuleEventAsServer(this);
						GameNetwork.WriteMessage(new SynchronizingDone(this, value));
						GameNetwork.EndModuleEventAsServer();
						if (value)
						{
							MBDebug.Print("Server: " + this.UserName + " is now synchronized.", 0, Debug.DebugColor.White, 17179869184UL);
							return;
						}
						MBDebug.Print("Server: " + this.UserName + " is not synchronized.", 0, Debug.DebugColor.White, 17179869184UL);
					}
				}
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06002E16 RID: 11798 RVA: 0x000B2EDD File Offset: 0x000B10DD
		public bool IsServerPeer
		{
			get
			{
				return this._isServerPeer;
			}
		}

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06002E17 RID: 11799 RVA: 0x000B2EE5 File Offset: 0x000B10E5
		// (set) Token: 0x06002E18 RID: 11800 RVA: 0x000B2EED File Offset: 0x000B10ED
		public ServerPerformanceState ServerPerformanceProblemState
		{
			get
			{
				return this._serverPerformanceProblemState;
			}
			private set
			{
				if (value != this._serverPerformanceProblemState)
				{
					this._serverPerformanceProblemState = value;
				}
			}
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x000B2EFF File Offset: 0x000B10FF
		private NetworkCommunicator(int index, string name, PlayerId playerID)
		{
			this.VirtualPlayer = new VirtualPlayer(index, name, playerID, this);
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x000B2F20 File Offset: 0x000B1120
		internal static NetworkCommunicator CreateAsServer(PlayerConnectionInfo playerConnectionInfo, int index, bool isAdmin, bool isSpectator)
		{
			NetworkCommunicator networkCommunicator = new NetworkCommunicator(index, playerConnectionInfo.Name, playerConnectionInfo.PlayerID);
			networkCommunicator.PlayerConnectionInfo = playerConnectionInfo;
			networkCommunicator.IsAdmin = isAdmin;
			networkCommunicator.IsSpectator = isSpectator;
			MBNetworkPeer mbnetworkPeer = new MBNetworkPeer(networkCommunicator);
			MBAPI.IMBPeer.SetUserData(index, mbnetworkPeer);
			return networkCommunicator;
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x000B2F67 File Offset: 0x000B1167
		internal static NetworkCommunicator CreateAsClient(string name, int index)
		{
			return new NetworkCommunicator(index, name, PlayerId.Empty);
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x000B2F78 File Offset: 0x000B1178
		void ICommunicator.OnAddComponent(PeerComponent component)
		{
			if (GameNetwork.IsServer)
			{
				if (!this.IsServerPeer)
				{
					GameNetwork.BeginModuleEventAsServer(this);
					GameNetwork.WriteMessage(new AddPeerComponent(this, component.TypeId));
					GameNetwork.EndModuleEventAsServer();
				}
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new AddPeerComponent(this, component.TypeId));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeTargetPlayer | GameNetwork.EventBroadcastFlags.AddToMissionRecord, this);
			}
			Action<PeerComponent> onPeerComponentAdded = NetworkCommunicator.OnPeerComponentAdded;
			if (onPeerComponentAdded == null)
			{
				return;
			}
			onPeerComponentAdded(component);
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x000B2FE0 File Offset: 0x000B11E0
		void ICommunicator.OnRemoveComponent(PeerComponent component)
		{
			if (GameNetwork.IsServer)
			{
				if (!this.IsServerPeer && (this.IsSynchronized || !this.JustReconnecting))
				{
					GameNetwork.BeginModuleEventAsServer(this);
					GameNetwork.WriteMessage(new RemovePeerComponent(this, component.TypeId));
					GameNetwork.EndModuleEventAsServer();
				}
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new RemovePeerComponent(this, component.TypeId));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.ExcludeTargetPlayer | GameNetwork.EventBroadcastFlags.AddToMissionRecord, this);
			}
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x000B3046 File Offset: 0x000B1246
		void ICommunicator.OnSynchronizeComponentTo(VirtualPlayer peer, PeerComponent component)
		{
			GameNetwork.BeginModuleEventAsServer(peer);
			GameNetwork.WriteMessage(new AddPeerComponent(this, component.TypeId));
			GameNetwork.EndModuleEventAsServer();
		}

		// Token: 0x06002E1F RID: 11807 RVA: 0x000B3064 File Offset: 0x000B1264
		internal void SetServerPeer(bool serverPeer)
		{
			this._isServerPeer = serverPeer;
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x000B306D File Offset: 0x000B126D
		internal double RefreshAndGetAveragePingInMilliseconds()
		{
			this.AveragePingInMilliseconds = MBAPI.IMBPeer.GetAveragePingInMilliseconds(this.Index);
			return this.AveragePingInMilliseconds;
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x000B308B File Offset: 0x000B128B
		internal void SetAveragePingInMillisecondsAsClient(double pingValue)
		{
			this.AveragePingInMilliseconds = pingValue;
			Agent controlledAgent = this.ControlledAgent;
			if (controlledAgent == null)
			{
				return;
			}
			controlledAgent.SetAveragePingInMilliseconds(this.AveragePingInMilliseconds);
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x000B30AA File Offset: 0x000B12AA
		internal double RefreshAndGetAverageLossPercent()
		{
			this.AverageLossPercent = MBAPI.IMBPeer.GetAverageLossPercent(this.Index);
			return this.AverageLossPercent;
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x000B30C8 File Offset: 0x000B12C8
		internal void SetAverageLossPercentAsClient(double lossValue)
		{
			this.AverageLossPercent = lossValue;
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x000B30D1 File Offset: 0x000B12D1
		internal void SetServerPerformanceProblemStateAsClient(ServerPerformanceState serverPerformanceProblemState)
		{
			this.ServerPerformanceProblemState = serverPerformanceProblemState;
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x000B30DA File Offset: 0x000B12DA
		public void SetRelevantGameOptions(bool sendMeBloodEvents, bool sendMeSoundEvents)
		{
			MBAPI.IMBPeer.SetRelevantGameOptions(this.Index, sendMeBloodEvents, sendMeSoundEvents);
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x000B30EE File Offset: 0x000B12EE
		public uint GetHost()
		{
			return MBAPI.IMBPeer.GetHost(this.Index);
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x000B3100 File Offset: 0x000B1300
		public uint GetReversedHost()
		{
			return MBAPI.IMBPeer.GetReversedHost(this.Index);
		}

		// Token: 0x06002E28 RID: 11816 RVA: 0x000B3112 File Offset: 0x000B1312
		public ushort GetPort()
		{
			return MBAPI.IMBPeer.GetPort(this.Index);
		}

		// Token: 0x06002E29 RID: 11817 RVA: 0x000B3124 File Offset: 0x000B1324
		public void UpdateConnectionInfoForReconnect(PlayerConnectionInfo playerConnectionInfo, bool isAdmin, bool isSpectator)
		{
			this.PlayerConnectionInfo = playerConnectionInfo;
			this.IsAdmin = isAdmin;
			this.IsSpectator = isSpectator;
		}

		// Token: 0x06002E2A RID: 11818 RVA: 0x000B313B File Offset: 0x000B133B
		public void UpdateIndexForReconnectingPlayer(int newIndex)
		{
			this.JustReconnecting = true;
			this.VirtualPlayer.UpdateIndexForReconnectingPlayer(newIndex);
		}

		// Token: 0x06002E2B RID: 11819 RVA: 0x000B3150 File Offset: 0x000B1350
		internal void SetJoinFlagsAsClient(bool isSpectator, bool isAdmin)
		{
			this.IsSpectator = isSpectator;
			this.IsAdmin = isAdmin;
		}

		// Token: 0x04001229 RID: 4649
		private double _averagePingInMilliseconds;

		// Token: 0x0400122A RID: 4650
		private double _averageLossPercent;

		// Token: 0x0400122D RID: 4653
		private Agent _controlledAgent;

		// Token: 0x0400122E RID: 4654
		private bool _isServerPeer;

		// Token: 0x0400122F RID: 4655
		private bool _isSynchronized;

		// Token: 0x04001232 RID: 4658
		private ServerPerformanceState _serverPerformanceProblemState;
	}
}
