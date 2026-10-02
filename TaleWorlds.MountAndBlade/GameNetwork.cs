using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002F8 RID: 760
	public static class GameNetwork
	{
		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06002B84 RID: 11140 RVA: 0x000A7FC5 File Offset: 0x000A61C5
		public static bool IsServer
		{
			get
			{
				return MBCommon.CurrentGameType == MBCommon.GameType.MultiServer || MBCommon.CurrentGameType == MBCommon.GameType.MultiClientServer;
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06002B85 RID: 11141 RVA: 0x000A7FD9 File Offset: 0x000A61D9
		public static bool IsServerOrRecorder
		{
			get
			{
				return GameNetwork.IsServer || MBCommon.CurrentGameType == MBCommon.GameType.SingleRecord;
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06002B86 RID: 11142 RVA: 0x000A7FEC File Offset: 0x000A61EC
		public static bool IsClient
		{
			get
			{
				return MBCommon.CurrentGameType == MBCommon.GameType.MultiClient;
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06002B87 RID: 11143 RVA: 0x000A7FF6 File Offset: 0x000A61F6
		public static bool IsReplay
		{
			get
			{
				return MBCommon.CurrentGameType == MBCommon.GameType.SingleReplay;
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06002B88 RID: 11144 RVA: 0x000A8000 File Offset: 0x000A6200
		public static bool IsClientOrReplay
		{
			get
			{
				return GameNetwork.IsClient || GameNetwork.IsReplay;
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06002B89 RID: 11145 RVA: 0x000A8010 File Offset: 0x000A6210
		public static bool IsDedicatedServer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06002B8A RID: 11146 RVA: 0x000A8013 File Offset: 0x000A6213
		public static bool MultiplayerDisabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06002B8B RID: 11147 RVA: 0x000A8016 File Offset: 0x000A6216
		public static bool IsMultiplayer
		{
			get
			{
				return GameNetwork.IsServer || GameNetwork.IsClient;
			}
		}

		// Token: 0x17000827 RID: 2087
		// (get) Token: 0x06002B8C RID: 11148 RVA: 0x000A8026 File Offset: 0x000A6226
		public static bool IsMultiplayerOrReplay
		{
			get
			{
				return GameNetwork.IsMultiplayer || GameNetwork.IsReplay;
			}
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06002B8D RID: 11149 RVA: 0x000A8036 File Offset: 0x000A6236
		public static bool IsSessionActive
		{
			get
			{
				return GameNetwork.IsServerOrRecorder || GameNetwork.IsClientOrReplay;
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06002B8E RID: 11150 RVA: 0x000A8046 File Offset: 0x000A6246
		public static IEnumerable<NetworkCommunicator> NetworkPeersIncludingDisconnectedPeers
		{
			get
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					yield return networkCommunicator;
				}
				List<NetworkCommunicator>.Enumerator enumerator = default(List<NetworkCommunicator>.Enumerator);
				int num;
				for (int i = 0; i < GameNetwork.DisconnectedNetworkPeers.Count; i = num + 1)
				{
					yield return GameNetwork.DisconnectedNetworkPeers[i];
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06002B8F RID: 11151 RVA: 0x000A804F File Offset: 0x000A624F
		// (set) Token: 0x06002B90 RID: 11152 RVA: 0x000A8056 File Offset: 0x000A6256
		public static VirtualPlayer[] VirtualPlayers { get; private set; }

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06002B91 RID: 11153 RVA: 0x000A805E File Offset: 0x000A625E
		// (set) Token: 0x06002B92 RID: 11154 RVA: 0x000A8065 File Offset: 0x000A6265
		public static List<NetworkCommunicator> NetworkPeers { get; private set; }

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06002B93 RID: 11155 RVA: 0x000A806D File Offset: 0x000A626D
		// (set) Token: 0x06002B94 RID: 11156 RVA: 0x000A8074 File Offset: 0x000A6274
		public static List<NetworkCommunicator> DisconnectedNetworkPeers { get; private set; }

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06002B95 RID: 11157 RVA: 0x000A807C File Offset: 0x000A627C
		public static int NetworkPeerCount
		{
			get
			{
				return GameNetwork.NetworkPeers.Count;
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06002B96 RID: 11158 RVA: 0x000A8088 File Offset: 0x000A6288
		public static bool NetworkPeersValid
		{
			get
			{
				return GameNetwork.NetworkPeers != null;
			}
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x000A8092 File Offset: 0x000A6292
		private static void AddNetworkPeer(NetworkCommunicator networkPeer)
		{
			GameNetwork.NetworkPeers.Add(networkPeer);
			Debug.Print("AddNetworkPeer: " + networkPeer.UserName, 0, Debug.DebugColor.White, 17179869184UL);
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x000A80C0 File Offset: 0x000A62C0
		private static void RemoveNetworkPeer(NetworkCommunicator networkPeer)
		{
			Debug.Print("RemoveNetworkPeer: " + networkPeer.UserName, 0, Debug.DebugColor.White, 17179869184UL);
			GameNetwork.NetworkPeers.Remove(networkPeer);
		}

		// Token: 0x06002B99 RID: 11161 RVA: 0x000A80EF File Offset: 0x000A62EF
		private static void AddToDisconnectedPeers(NetworkCommunicator networkPeer)
		{
			Debug.Print("AddToDisconnectedPeers: " + networkPeer.UserName, 0, Debug.DebugColor.White, 17179869184UL);
			GameNetwork.DisconnectedNetworkPeers.Add(networkPeer);
		}

		// Token: 0x06002B9A RID: 11162 RVA: 0x000A8120 File Offset: 0x000A6320
		public static void ClearAllPeers()
		{
			if (GameNetwork.VirtualPlayers != null)
			{
				for (int i = 0; i < GameNetwork.VirtualPlayers.Length; i++)
				{
					GameNetwork.VirtualPlayers[i] = null;
				}
				GameNetwork.NetworkPeers.Clear();
				GameNetwork.DisconnectedNetworkPeers.Clear();
			}
		}

		// Token: 0x06002B9B RID: 11163 RVA: 0x000A8164 File Offset: 0x000A6364
		public static NetworkCommunicator FindNetworkPeer(int index)
		{
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (networkCommunicator.Index == index)
				{
					return networkCommunicator;
				}
			}
			return null;
		}

		// Token: 0x06002B9C RID: 11164 RVA: 0x000A81C0 File Offset: 0x000A63C0
		public static void Initialize(IGameNetworkHandler handler)
		{
			GameNetwork._handler = handler;
			GameNetwork.VirtualPlayers = new VirtualPlayer[1023];
			GameNetwork.NetworkPeers = new List<NetworkCommunicator>();
			GameNetwork.DisconnectedNetworkPeers = new List<NetworkCommunicator>();
			MBNetwork.Initialize(new NetworkCommunication());
			GameNetwork.NetworkComponents = new List<UdpNetworkComponent>();
			GameNetwork.NetworkHandlers = new List<IUdpNetworkHandler>();
			GameNetwork._handler.OnInitialize();
		}

		// Token: 0x06002B9D RID: 11165 RVA: 0x000A8220 File Offset: 0x000A6420
		internal static void Tick(float dt)
		{
			int i = 0;
			try
			{
				for (i = 0; i < GameNetwork.NetworkHandlers.Count; i++)
				{
					GameNetwork.NetworkHandlers[i].OnUdpNetworkHandlerTick(dt);
				}
			}
			catch (Exception ex)
			{
				if (GameNetwork.NetworkHandlers.Count > 0 && i < GameNetwork.NetworkHandlers.Count && GameNetwork.NetworkHandlers[i] != null)
				{
					string text = GameNetwork.NetworkHandlers[i].ToString();
					Debug.Print("Exception On Network Component: " + text, 0, Debug.DebugColor.White, 17592186044416UL);
				}
				Debug.Print(ex.StackTrace, 0, Debug.DebugColor.White, 17592186044416UL);
				Debug.Print(ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
			}
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x000A82F0 File Offset: 0x000A64F0
		private static void StartMultiplayer()
		{
			VirtualPlayer.Reset();
			GameNetwork._handler.OnStartMultiplayer();
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x000A8304 File Offset: 0x000A6504
		public static void EndMultiplayer()
		{
			GameNetwork._handler.OnEndMultiplayer();
			for (int i = GameNetwork.NetworkComponents.Count - 1; i >= 0; i--)
			{
				GameNetwork.DestroyComponent(GameNetwork.NetworkComponents[i]);
			}
			for (int j = GameNetwork.NetworkHandlers.Count - 1; j >= 0; j--)
			{
				GameNetwork.RemoveNetworkHandler(GameNetwork.NetworkHandlers[j]);
			}
			if (GameNetwork.IsServer)
			{
				GameNetwork.TerminateServerSide();
			}
			if (GameNetwork.IsClientOrReplay)
			{
				GameNetwork.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
			}
			if (GameNetwork.IsClient)
			{
				GameNetwork.TerminateClientSide();
			}
			Debug.Print("Clearing peers list with count " + GameNetwork.NetworkPeerCount, 0, Debug.DebugColor.White, 17592186044416UL);
			GameNetwork.ClearAllPeers();
			VirtualPlayer.Reset();
			GameNetwork.MyPeer = null;
			Debug.Print("NetworkManager::HandleMultiplayerEnd", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x06002BA0 RID: 11168 RVA: 0x000A83DC File Offset: 0x000A65DC
		[MBCallback(null, false)]
		internal static void HandleRemovePlayer(MBNetworkPeer peer, bool isTimedOut)
		{
			DisconnectInfo disconnectInfo;
			if ((disconnectInfo = peer.NetworkPeer.PlayerConnectionInfo.GetParameter<DisconnectInfo>("DisconnectInfo")) == null)
			{
				(disconnectInfo = new DisconnectInfo()).Type = DisconnectType.QuitFromGame;
			}
			DisconnectInfo disconnectInfo2 = disconnectInfo;
			disconnectInfo2.Type = (isTimedOut ? DisconnectType.TimedOut : disconnectInfo2.Type);
			peer.NetworkPeer.PlayerConnectionInfo.AddParameter("DisconnectInfo", disconnectInfo2);
			GameNetwork.HandleRemovePlayerInternal(peer.NetworkPeer, peer.NetworkPeer.IsSynchronized && MultiplayerIntermissionVotingManager.Instance.CurrentVoteState == MultiplayerIntermissionState.Idle);
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x000A8460 File Offset: 0x000A6660
		internal static void HandleRemovePlayerInternal(NetworkCommunicator networkPeer, bool isDisconnected)
		{
			if (GameNetwork.IsClient && networkPeer.IsMine)
			{
				GameNetwork.HandleDisconnect();
				return;
			}
			GameNetwork._handler.OnPlayerDisconnectedFromServer(networkPeer);
			if (GameNetwork.IsServer)
			{
				foreach (IUdpNetworkHandler udpNetworkHandler in GameNetwork.NetworkHandlers)
				{
					udpNetworkHandler.HandleEarlyPlayerDisconnect(networkPeer);
				}
				foreach (IUdpNetworkHandler udpNetworkHandler2 in GameNetwork.NetworkHandlers)
				{
					udpNetworkHandler2.HandlePlayerDisconnect(networkPeer);
				}
			}
			foreach (IUdpNetworkHandler udpNetworkHandler3 in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler3.OnPlayerDisconnectedFromServer(networkPeer);
			}
			GameNetwork.RemoveNetworkPeer(networkPeer);
			if (isDisconnected)
			{
				GameNetwork.AddToDisconnectedPeers(networkPeer);
			}
			GameNetwork.VirtualPlayers[networkPeer.VirtualPlayer.Index] = null;
			if (GameNetwork.IsServer)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					if (!networkCommunicator.IsServerPeer)
					{
						GameNetwork.BeginModuleEventAsServer(networkCommunicator);
						GameNetwork.WriteMessage(new DeletePlayer(networkPeer.Index, isDisconnected));
						GameNetwork.EndModuleEventAsServer();
					}
				}
			}
		}

		// Token: 0x06002BA2 RID: 11170 RVA: 0x000A85DC File Offset: 0x000A67DC
		[MBCallback(null, false)]
		internal static void HandleDisconnect()
		{
			GameNetwork._handler.OnDisconnectedFromServer();
			foreach (IUdpNetworkHandler udpNetworkHandler in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler.OnDisconnectedFromServer();
			}
			GameNetwork.MyPeer = null;
		}

		// Token: 0x06002BA3 RID: 11171 RVA: 0x000A863C File Offset: 0x000A683C
		public static void StartReplay()
		{
			GameNetwork._handler.OnStartReplay();
		}

		// Token: 0x06002BA4 RID: 11172 RVA: 0x000A8648 File Offset: 0x000A6848
		public static void EndReplay()
		{
			GameNetwork._handler.OnEndReplay();
		}

		// Token: 0x06002BA5 RID: 11173 RVA: 0x000A8654 File Offset: 0x000A6854
		public static void PreStartMultiplayerOnServer()
		{
			MBCommon.CurrentGameType = (GameNetwork.IsDedicatedServer ? MBCommon.GameType.MultiServer : MBCommon.GameType.MultiClientServer);
			GameNetwork.ClientPeerIndex = -1;
		}

		// Token: 0x06002BA6 RID: 11174 RVA: 0x000A866C File Offset: 0x000A686C
		public static void StartMultiplayerOnServer(int port)
		{
			Debug.Print("StartMultiplayerOnServer", 0, Debug.DebugColor.White, 17592186044416UL);
			GameNetwork.PreStartMultiplayerOnServer();
			GameNetwork.InitializeServerSide(port);
			GameNetwork.StartMultiplayer();
		}

		// Token: 0x06002BA7 RID: 11175 RVA: 0x000A8694 File Offset: 0x000A6894
		[MBCallback(null, false)]
		internal static bool HandleNetworkPacketAsServer(MBNetworkPeer networkPeer)
		{
			return GameNetwork.HandleNetworkPacketAsServer(networkPeer.NetworkPeer);
		}

		// Token: 0x06002BA8 RID: 11176 RVA: 0x000A86A4 File Offset: 0x000A68A4
		internal static bool HandleNetworkPacketAsServer(NetworkCommunicator networkPeer)
		{
			if (networkPeer == null)
			{
				Debug.Print("networkPeer == null", 0, Debug.DebugColor.White, 17592186044416UL);
				return false;
			}
			bool flag = true;
			try
			{
				int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.NetworkComponentEventTypeFromClientCompressionInfo, ref flag);
				if (flag)
				{
					if (num >= 0 && num < GameNetwork._gameNetworkMessageIdsFromClient.Count)
					{
						GameNetworkMessage gameNetworkMessage = Activator.CreateInstance(GameNetwork._gameNetworkMessageIdsFromClient[num]) as GameNetworkMessage;
						gameNetworkMessage.MessageId = num;
						flag = gameNetworkMessage.Read();
						if (flag)
						{
							bool flag2 = false;
							bool flag3 = true;
							List<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>> list;
							if (GameNetwork._fromClientBaseMessageHandlers.TryGetValue(num, out list))
							{
								foreach (GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage> clientMessageHandlerDelegate in list)
								{
									flag = flag && clientMessageHandlerDelegate(networkPeer, gameNetworkMessage);
									if (!flag)
									{
										break;
									}
								}
								flag3 = false;
								flag2 = list.Count != 0;
							}
							List<object> list2;
							if (GameNetwork._fromClientMessageHandlers.TryGetValue(num, out list2))
							{
								foreach (object obj in list2)
								{
									Delegate @delegate = obj as Delegate;
									flag = flag && (bool)@delegate.DynamicInvokeWithLog(new object[] { networkPeer, gameNetworkMessage });
									if (!flag)
									{
										break;
									}
								}
								flag3 = false;
								flag2 = flag2 || list2.Count != 0;
							}
							if (flag3)
							{
								Debug.FailedAssert("Unknown network messageId " + gameNetworkMessage, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\GameNetwork.cs", "HandleNetworkPacketAsServer", 747);
								flag = false;
							}
							else if (!flag2)
							{
								Debug.Print("Handler not found for network message " + gameNetworkMessage, 0, Debug.DebugColor.White, 17179869184UL);
							}
						}
					}
					else
					{
						Debug.Print("Handler not found for network message " + num.ToString(), 0, Debug.DebugColor.White, 17179869184UL);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Print("error " + ex.Message, 0, Debug.DebugColor.White, 17592186044416UL);
				return false;
			}
			return flag;
		}

		// Token: 0x06002BA9 RID: 11177 RVA: 0x000A88E4 File Offset: 0x000A6AE4
		[MBCallback(null, false)]
		public static void HandleConsoleCommand(string command)
		{
			if (GameNetwork._handler != null)
			{
				GameNetwork._handler.OnHandleConsoleCommand(command);
			}
		}

		// Token: 0x06002BAA RID: 11178 RVA: 0x000A88F8 File Offset: 0x000A6AF8
		private static void InitializeServerSide(int port)
		{
			MBAPI.IMBNetwork.InitializeServerSide(port);
		}

		// Token: 0x06002BAB RID: 11179 RVA: 0x000A8905 File Offset: 0x000A6B05
		private static void TerminateServerSide()
		{
			MBAPI.IMBNetwork.TerminateServerSide();
			if (!GameNetwork.IsDedicatedServer)
			{
				MBCommon.CurrentGameType = MBCommon.GameType.Single;
			}
		}

		// Token: 0x06002BAC RID: 11180 RVA: 0x000A891E File Offset: 0x000A6B1E
		private static void PrepareNewUdpSession(int peerIndex, int sessionKey)
		{
			MBAPI.IMBNetwork.PrepareNewUdpSession(peerIndex, sessionKey);
		}

		// Token: 0x06002BAD RID: 11181 RVA: 0x000A892C File Offset: 0x000A6B2C
		public static string GetActiveUdpSessionsIpAddress()
		{
			return MBAPI.IMBNetwork.GetActiveUdpSessionsIpAddress();
		}

		// Token: 0x06002BAE RID: 11182 RVA: 0x000A8938 File Offset: 0x000A6B38
		public static ICommunicator AddNewPlayerOnServer(PlayerConnectionInfo playerConnectionInfo, bool serverPeer, bool isAdmin, bool isSpectator)
		{
			bool flag = playerConnectionInfo == null;
			int num = (flag ? MBAPI.IMBNetwork.AddNewBotOnServer() : MBAPI.IMBNetwork.AddNewPlayerOnServer(serverPeer));
			Debug.Print(string.Concat(new object[]
			{
				"AddNewPlayerOnServer: ",
				((playerConnectionInfo != null) ? playerConnectionInfo.Name : null) ?? "<bot>",
				" index: ",
				num
			}), 0, Debug.DebugColor.White, 17179869184UL);
			if (num >= 0)
			{
				int num2 = 0;
				if (!serverPeer)
				{
					num2 = GameNetwork.GetSessionKeyForPlayer();
				}
				int num3 = -1;
				ICommunicator communicator = null;
				if (flag)
				{
					communicator = DummyCommunicator.CreateAsServer(num, "");
				}
				else
				{
					for (int i = 0; i < GameNetwork.DisconnectedNetworkPeers.Count; i++)
					{
						PlayerData parameter = playerConnectionInfo.GetParameter<PlayerData>("PlayerData");
						if (parameter != null && GameNetwork.DisconnectedNetworkPeers[i].VirtualPlayer.Id == parameter.PlayerId)
						{
							num3 = i;
							communicator = GameNetwork.DisconnectedNetworkPeers[i];
							NetworkCommunicator networkCommunicator = communicator as NetworkCommunicator;
							networkCommunicator.UpdateIndexForReconnectingPlayer(num);
							networkCommunicator.UpdateConnectionInfoForReconnect(playerConnectionInfo, isAdmin, isSpectator);
							MBAPI.IMBPeer.SetUserData(num, new MBNetworkPeer(networkCommunicator));
							Debug.Print("RemoveFromDisconnectedPeers: " + networkCommunicator.UserName, 0, Debug.DebugColor.White, 17179869184UL);
							GameNetwork.DisconnectedNetworkPeers.RemoveAt(i);
							break;
						}
					}
					if (communicator == null)
					{
						communicator = NetworkCommunicator.CreateAsServer(playerConnectionInfo, num, isAdmin, isSpectator);
					}
				}
				GameNetwork.VirtualPlayers[communicator.VirtualPlayer.Index] = communicator.VirtualPlayer;
				if (!flag)
				{
					NetworkCommunicator networkCommunicator2 = communicator as NetworkCommunicator;
					if (serverPeer && GameNetwork.IsServer)
					{
						GameNetwork.ClientPeerIndex = num;
						GameNetwork.MyPeer = networkCommunicator2;
					}
					networkCommunicator2.SessionKey = num2;
					networkCommunicator2.SetServerPeer(serverPeer);
					GameNetwork.AddNetworkPeer(networkCommunicator2);
					playerConnectionInfo.NetworkPeer = networkCommunicator2;
					if (!serverPeer)
					{
						GameNetwork.PrepareNewUdpSession(num, num2);
					}
					if (num3 < 0)
					{
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new CreatePlayer(networkCommunicator2.Index, playerConnectionInfo.Name, num3, false, false, networkCommunicator2.IsSpectator, networkCommunicator2.IsAdmin));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.AddToMissionRecord | GameNetwork.EventBroadcastFlags.DontSendToPeers, null);
					}
					foreach (NetworkCommunicator networkCommunicator3 in GameNetwork.NetworkPeers)
					{
						if (networkCommunicator3 != networkCommunicator2 && networkCommunicator3 != GameNetwork.MyPeer)
						{
							GameNetwork.BeginModuleEventAsServer(networkCommunicator3);
							GameNetwork.WriteMessage(new CreatePlayer(networkCommunicator2.Index, playerConnectionInfo.Name, num3, false, false, networkCommunicator2.IsSpectator, networkCommunicator2.IsAdmin));
							GameNetwork.EndModuleEventAsServer();
						}
						if (!serverPeer)
						{
							bool flag2 = networkCommunicator3 == networkCommunicator2;
							GameNetwork.BeginModuleEventAsServer(networkCommunicator2);
							GameNetwork.WriteMessage(new CreatePlayer(networkCommunicator3.Index, networkCommunicator3.UserName, -1, false, flag2, networkCommunicator3.IsSpectator, networkCommunicator3.IsAdmin));
							GameNetwork.EndModuleEventAsServer();
						}
					}
					for (int j = 0; j < GameNetwork.DisconnectedNetworkPeers.Count; j++)
					{
						NetworkCommunicator networkCommunicator4 = GameNetwork.DisconnectedNetworkPeers[j];
						GameNetwork.BeginModuleEventAsServer(networkCommunicator2);
						GameNetwork.WriteMessage(new CreatePlayer(networkCommunicator4.Index, networkCommunicator4.UserName, j, true, false, networkCommunicator4.IsSpectator, networkCommunicator4.IsAdmin));
						GameNetwork.EndModuleEventAsServer();
					}
					foreach (IUdpNetworkHandler udpNetworkHandler in GameNetwork.NetworkHandlers)
					{
						udpNetworkHandler.HandleNewClientConnect(playerConnectionInfo);
					}
					GameNetwork._handler.OnPlayerConnectedToServer(networkCommunicator2);
				}
				return communicator;
			}
			return null;
		}

		// Token: 0x06002BAF RID: 11183 RVA: 0x000A8CD0 File Offset: 0x000A6ED0
		public static GameNetwork.AddPlayersResult AddNewPlayersOnServer(PlayerConnectionInfo[] playerConnectionInfos, bool serverPeer)
		{
			bool flag = MBAPI.IMBNetwork.CanAddNewPlayersOnServer(playerConnectionInfos.Length);
			NetworkCommunicator[] array = new NetworkCommunicator[playerConnectionInfos.Length];
			if (flag)
			{
				for (int i = 0; i < array.Length; i++)
				{
					CustomGameJoinType customGameJoinType = CustomGameJoinType.Player;
					string parameter = playerConnectionInfos[i].GetParameter<string>("JoinType");
					if (!string.IsNullOrEmpty(parameter) && !Enum.TryParse<CustomGameJoinType>(parameter, out customGameJoinType))
					{
						Debug.FailedAssert("Unrecognized JoinType '" + parameter + "'; treating the peer as a player.", "GameNetwork.cs", "AddNewPlayersOnServer", 992);
						customGameJoinType = CustomGameJoinType.Player;
					}
					ICommunicator communicator = GameNetwork.AddNewPlayerOnServer(playerConnectionInfos[i], serverPeer, customGameJoinType == CustomGameJoinType.Admin, customGameJoinType == CustomGameJoinType.Spectator);
					array[i] = communicator as NetworkCommunicator;
				}
			}
			return new GameNetwork.AddPlayersResult
			{
				NetworkPeers = array,
				Success = flag
			};
		}

		// Token: 0x06002BB0 RID: 11184 RVA: 0x000A8D88 File Offset: 0x000A6F88
		public static void ClientFinishedLoading(NetworkCommunicator networkPeer)
		{
			foreach (IUdpNetworkHandler udpNetworkHandler in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler.HandleEarlyNewClientAfterLoadingFinished(networkPeer);
			}
			foreach (IUdpNetworkHandler udpNetworkHandler2 in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler2.HandleNewClientAfterLoadingFinished(networkPeer);
			}
			foreach (IUdpNetworkHandler udpNetworkHandler3 in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler3.HandleLateNewClientAfterLoadingFinished(networkPeer);
			}
			networkPeer.IsSynchronized = true;
			foreach (IUdpNetworkHandler udpNetworkHandler4 in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler4.HandleNewClientAfterSynchronized(networkPeer);
			}
			foreach (IUdpNetworkHandler udpNetworkHandler5 in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler5.HandleLateNewClientAfterSynchronized(networkPeer);
			}
		}

		// Token: 0x06002BB1 RID: 11185 RVA: 0x000A8EDC File Offset: 0x000A70DC
		public static void BeginModuleEventAsClient()
		{
			MBAPI.IMBNetwork.BeginModuleEventAsClient(true);
		}

		// Token: 0x06002BB2 RID: 11186 RVA: 0x000A8EE9 File Offset: 0x000A70E9
		public static void EndModuleEventAsClient()
		{
			MBAPI.IMBNetwork.EndModuleEventAsClient(true);
		}

		// Token: 0x06002BB3 RID: 11187 RVA: 0x000A8EF6 File Offset: 0x000A70F6
		public static void BeginModuleEventAsClientUnreliable()
		{
			MBAPI.IMBNetwork.BeginModuleEventAsClient(false);
		}

		// Token: 0x06002BB4 RID: 11188 RVA: 0x000A8F03 File Offset: 0x000A7103
		public static void EndModuleEventAsClientUnreliable()
		{
			MBAPI.IMBNetwork.EndModuleEventAsClient(false);
		}

		// Token: 0x06002BB5 RID: 11189 RVA: 0x000A8F10 File Offset: 0x000A7110
		public static void BeginModuleEventAsServer(NetworkCommunicator communicator)
		{
			GameNetwork.BeginModuleEventAsServer(communicator.VirtualPlayer);
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x000A8F1D File Offset: 0x000A711D
		public static void BeginModuleEventAsServerUnreliable(NetworkCommunicator communicator)
		{
			GameNetwork.BeginModuleEventAsServerUnreliable(communicator.VirtualPlayer);
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x000A8F2A File Offset: 0x000A712A
		public static void BeginModuleEventAsServer(VirtualPlayer peer)
		{
			MBAPI.IMBPeer.BeginModuleEvent(peer.Index, true);
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x000A8F3D File Offset: 0x000A713D
		public static void EndModuleEventAsServer()
		{
			MBAPI.IMBPeer.EndModuleEvent(true);
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x000A8F4A File Offset: 0x000A714A
		public static void BeginModuleEventAsServerUnreliable(VirtualPlayer peer)
		{
			MBAPI.IMBPeer.BeginModuleEvent(peer.Index, false);
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x000A8F5D File Offset: 0x000A715D
		public static void EndModuleEventAsServerUnreliable()
		{
			MBAPI.IMBPeer.EndModuleEvent(false);
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x000A8F6A File Offset: 0x000A716A
		public static void BeginBroadcastModuleEvent()
		{
			MBAPI.IMBNetwork.BeginBroadcastModuleEvent();
		}

		// Token: 0x06002BBC RID: 11196 RVA: 0x000A8F78 File Offset: 0x000A7178
		public static void EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags broadcastFlags, NetworkCommunicator targetPlayer = null)
		{
			int num = ((targetPlayer != null) ? targetPlayer.Index : (-1));
			MBAPI.IMBNetwork.EndBroadcastModuleEvent((int)broadcastFlags, num, true);
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x000A8F9F File Offset: 0x000A719F
		public static double ElapsedTimeSinceLastUdpPacketArrived()
		{
			return MBAPI.IMBNetwork.ElapsedTimeSinceLastUdpPacketArrived();
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x000A8FAC File Offset: 0x000A71AC
		public static void EndBroadcastModuleEventUnreliable(GameNetwork.EventBroadcastFlags broadcastFlags, NetworkCommunicator targetPlayer = null)
		{
			int num = ((targetPlayer != null) ? targetPlayer.Index : (-1));
			MBAPI.IMBNetwork.EndBroadcastModuleEvent((int)broadcastFlags, num, false);
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x000A8FD4 File Offset: 0x000A71D4
		public static void UnSynchronizeEveryone()
		{
			Debug.Print("UnSynchronizeEveryone is called!", 0, Debug.DebugColor.White, 17179869184UL);
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				networkCommunicator.IsSynchronized = false;
			}
			foreach (IUdpNetworkHandler udpNetworkHandler in GameNetwork.NetworkHandlers)
			{
				udpNetworkHandler.OnEveryoneUnSynchronized();
			}
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x000A9078 File Offset: 0x000A7278
		public static void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
		{
			GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);
			networkMessageHandlerRegisterer.Register<CreatePlayer>(new GameNetworkMessage.ServerMessageHandlerDelegate<CreatePlayer>(GameNetwork.HandleServerEventCreatePlayer));
			networkMessageHandlerRegisterer.Register<DeletePlayer>(new GameNetworkMessage.ServerMessageHandlerDelegate<DeletePlayer>(GameNetwork.HandleServerEventDeletePlayer));
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x000A90A3 File Offset: 0x000A72A3
		public static void StartMultiplayerOnClient(string serverAddress, int port, int sessionKey, int playerIndex)
		{
			Debug.Print("StartMultiplayerOnClient", 0, Debug.DebugColor.White, 17592186044416UL);
			MBCommon.CurrentGameType = MBCommon.GameType.MultiClient;
			GameNetwork.ClientPeerIndex = playerIndex;
			GameNetwork.InitializeClientSide(serverAddress, port, sessionKey, playerIndex);
			GameNetwork.StartMultiplayer();
			GameNetwork.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x000A90DC File Offset: 0x000A72DC
		[MBCallback(null, false)]
		internal static bool HandleNetworkPacketAsClient()
		{
			if (!TWParallel.IsMainThread())
			{
				Debug.FailedAssert("Network messages should be handled from main thread", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\GameNetwork.cs", "HandleNetworkPacketAsClient", 1194);
			}
			bool flag = true;
			int num = GameNetworkMessage.ReadIntFromPacket(CompressionBasic.NetworkComponentEventTypeFromServerCompressionInfo, ref flag);
			if (flag && num >= 0 && num < GameNetwork._gameNetworkMessageIdsFromServer.Count)
			{
				GameNetworkMessage gameNetworkMessage = Activator.CreateInstance(GameNetwork._gameNetworkMessageIdsFromServer[num]) as GameNetworkMessage;
				gameNetworkMessage.MessageId = num;
				Debug.Print("Reading message: " + gameNetworkMessage.GetType().Name, 0, Debug.DebugColor.White, 17179869184UL);
				flag = gameNetworkMessage.Read();
				if (flag)
				{
					if (!NetworkMain.GameClient.IsInGame && !GameNetwork.IsReplay && !NetworkMain.CommunityClient.IsInGame)
					{
						Debug.Print("ignoring post mission message: " + gameNetworkMessage.GetType().Name, 0, Debug.DebugColor.White, 17179869184UL);
					}
					else
					{
						bool flag2 = false;
						bool flag3 = true;
						if ((gameNetworkMessage.GetLogFilter() & GameNetwork.MultiplayerLogging) != MultiplayerMessageFilter.None)
						{
							if (GameNetworkMessage.IsClientMissionOver)
							{
								Debug.Print("WARNING: Entering message processing while client mission is over", 0, Debug.DebugColor.White, 17592186044416UL);
							}
							Debug.Print("Processing message: " + gameNetworkMessage.GetType().Name + ": " + gameNetworkMessage.GetLogFormat(), 0, Debug.DebugColor.White, 17179869184UL);
						}
						List<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>> list;
						if (GameNetwork._fromServerBaseMessageHandlers.TryGetValue(num, out list))
						{
							foreach (GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage> serverMessageHandlerDelegate in list)
							{
								try
								{
									serverMessageHandlerDelegate(gameNetworkMessage);
								}
								catch
								{
									Debug.Print("Exception in handler of " + num.ToString(), 0, Debug.DebugColor.White, 17179869184UL);
									Debug.Print("Exception in handler of " + gameNetworkMessage.GetType().Name, 0, Debug.DebugColor.Red, 17179869184UL);
								}
							}
							flag3 = false;
							flag2 = list.Count != 0;
						}
						List<object> list2;
						if (GameNetwork._fromServerMessageHandlers.TryGetValue(num, out list2))
						{
							foreach (object obj in list2)
							{
								(obj as Delegate).DynamicInvokeWithLog(new object[] { gameNetworkMessage });
							}
							flag3 = false;
							flag2 = flag2 || list2.Count != 0;
						}
						if (flag3)
						{
							Debug.Print("Invalid messageId " + num.ToString(), 0, Debug.DebugColor.White, 17179869184UL);
							Debug.Print("Invalid messageId " + gameNetworkMessage.GetType().Name, 0, Debug.DebugColor.White, 17179869184UL);
						}
						else if (!flag2)
						{
							Debug.Print("No message handler found for " + gameNetworkMessage.GetType().Name, 0, Debug.DebugColor.Red, 17179869184UL);
						}
					}
				}
				else
				{
					Debug.Print("Invalid message read for: " + gameNetworkMessage.GetType().Name, 0, Debug.DebugColor.White, 17179869184UL);
				}
			}
			else
			{
				Debug.Print("Invalid message id read: " + num, 0, Debug.DebugColor.White, 17179869184UL);
			}
			return flag;
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x000A9420 File Offset: 0x000A7620
		private static int GetSessionKeyForPlayer()
		{
			return new Random(DateTime.Now.Millisecond).Next(1, 4001);
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x000A944C File Offset: 0x000A764C
		public static NetworkCommunicator HandleNewClientConnect(PlayerConnectionInfo playerConnectionInfo, bool isAdmin, bool isSpectator)
		{
			NetworkCommunicator networkCommunicator = GameNetwork.AddNewPlayerOnServer(playerConnectionInfo, false, isAdmin, isSpectator) as NetworkCommunicator;
			GameNetwork._handler.OnNewPlayerConnect(playerConnectionInfo, networkCommunicator);
			return networkCommunicator;
		}

		// Token: 0x06002BC5 RID: 11205 RVA: 0x000A9478 File Offset: 0x000A7678
		public static GameNetwork.AddPlayersResult HandleNewClientsConnect(PlayerConnectionInfo[] playerConnectionInfos, bool isAdmin)
		{
			GameNetwork.AddPlayersResult addPlayersResult = GameNetwork.AddNewPlayersOnServer(playerConnectionInfos, isAdmin);
			if (addPlayersResult.Success)
			{
				for (int i = 0; i < playerConnectionInfos.Length; i++)
				{
					GameNetwork._handler.OnNewPlayerConnect(playerConnectionInfos[i], addPlayersResult.NetworkPeers[i]);
				}
			}
			return addPlayersResult;
		}

		// Token: 0x06002BC6 RID: 11206 RVA: 0x000A94BC File Offset: 0x000A76BC
		public static void AddNetworkPeerToDisconnectAsServer(NetworkCommunicator networkPeer)
		{
			Debug.Print("adding peer to disconnect index:" + networkPeer.Index, 0, Debug.DebugColor.White, 17179869184UL);
			GameNetwork.AddPeerToDisconnect(networkPeer);
			GameNetwork.BeginModuleEventAsServer(networkPeer);
			GameNetwork.WriteMessage(new DeletePlayer(networkPeer.Index, false));
			GameNetwork.EndModuleEventAsServer();
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x000A9514 File Offset: 0x000A7714
		private static void HandleServerEventCreatePlayer(CreatePlayer message)
		{
			int playerIndex = message.PlayerIndex;
			string playerName = message.PlayerName;
			bool isReceiverPeer = message.IsReceiverPeer;
			NetworkCommunicator networkCommunicator;
			if (isReceiverPeer || message.IsNonExistingDisconnectedPeer || message.DisconnectedPeerIndex < 0)
			{
				networkCommunicator = NetworkCommunicator.CreateAsClient(playerName, playerIndex);
			}
			else
			{
				networkCommunicator = GameNetwork.DisconnectedNetworkPeers[message.DisconnectedPeerIndex];
				networkCommunicator.UpdateIndexForReconnectingPlayer(message.PlayerIndex);
				Debug.Print("RemoveFromDisconnectedPeers: " + networkCommunicator.UserName, 0, Debug.DebugColor.White, 17179869184UL);
				GameNetwork.DisconnectedNetworkPeers.RemoveAt(message.DisconnectedPeerIndex);
			}
			networkCommunicator.SetJoinFlagsAsClient(message.IsSpectator, message.IsAdmin);
			if (isReceiverPeer)
			{
				GameNetwork.MyPeer = networkCommunicator;
			}
			if (message.IsNonExistingDisconnectedPeer)
			{
				GameNetwork.AddToDisconnectedPeers(networkCommunicator);
			}
			else
			{
				GameNetwork.VirtualPlayers[networkCommunicator.VirtualPlayer.Index] = networkCommunicator.VirtualPlayer;
				GameNetwork.AddNetworkPeer(networkCommunicator);
			}
			GameNetwork._handler.OnPlayerConnectedToServer(networkCommunicator);
		}

		// Token: 0x06002BC8 RID: 11208 RVA: 0x000A95F4 File Offset: 0x000A77F4
		private static void HandleServerEventDeletePlayer(DeletePlayer message)
		{
			NetworkCommunicator networkCommunicator = GameNetwork.NetworkPeers.FirstOrDefault<NetworkCommunicator>((NetworkCommunicator networkPeer) => networkPeer.Index == message.PlayerIndex);
			if (networkCommunicator != null)
			{
				GameNetwork.HandleRemovePlayerInternal(networkCommunicator, message.AddToDisconnectList);
			}
		}

		// Token: 0x06002BC9 RID: 11209 RVA: 0x000A9639 File Offset: 0x000A7839
		public static void InitializeClientSide(string serverAddress, int port, int sessionKey, int playerIndex)
		{
			MBAPI.IMBNetwork.InitializeClientSide(serverAddress, port, sessionKey, playerIndex);
		}

		// Token: 0x06002BCA RID: 11210 RVA: 0x000A9649 File Offset: 0x000A7849
		public static void TerminateClientSide()
		{
			MBAPI.IMBNetwork.TerminateClientSide();
			MBCommon.CurrentGameType = MBCommon.GameType.Single;
		}

		// Token: 0x06002BCB RID: 11211 RVA: 0x000A965B File Offset: 0x000A785B
		public static Type GetSynchedMissionObjectReadableRecordTypeFromIndex(int typeIndex)
		{
			return GameNetwork._synchedMissionObjectClassTypes[typeIndex];
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x000A9668 File Offset: 0x000A7868
		public static int GetSynchedMissionObjectReadableRecordIndexFromType(Type type)
		{
			for (int i = 0; i < GameNetwork._synchedMissionObjectClassTypes.Count; i++)
			{
				Type type2 = GameNetwork._synchedMissionObjectClassTypes[i];
				DefineSynchedMissionObjectType customAttribute = type2.GetCustomAttribute<DefineSynchedMissionObjectType>();
				DefineSynchedMissionObjectTypeForMod customAttribute2 = type2.GetCustomAttribute<DefineSynchedMissionObjectTypeForMod>();
				Type type3 = ((customAttribute != null) ? customAttribute.Type : null) ?? ((customAttribute2 != null) ? customAttribute2.Type : null);
				Type type4 = type;
				while (type4 != null)
				{
					if (type4 == type3)
					{
						return i;
					}
					type4 = type4.BaseType;
				}
			}
			return -1;
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x000A96E4 File Offset: 0x000A78E4
		public static void DestroyComponent(UdpNetworkComponent udpNetworkComponent)
		{
			GameNetwork.RemoveNetworkHandler(udpNetworkComponent);
			GameNetwork.NetworkComponents.Remove(udpNetworkComponent);
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x000A96F8 File Offset: 0x000A78F8
		public static T AddNetworkComponent<T>() where T : UdpNetworkComponent
		{
			T t = (T)((object)Activator.CreateInstance(typeof(T), new object[0]));
			GameNetwork.NetworkComponents.Add(t);
			GameNetwork.NetworkHandlers.Add(t);
			return t;
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x000A9741 File Offset: 0x000A7941
		public static void AddNetworkHandler(IUdpNetworkHandler handler)
		{
			GameNetwork.NetworkHandlers.Add(handler);
		}

		// Token: 0x06002BD0 RID: 11216 RVA: 0x000A974E File Offset: 0x000A794E
		public static void RemoveNetworkHandler(IUdpNetworkHandler handler)
		{
			handler.OnUdpNetworkHandlerClose();
			GameNetwork.NetworkHandlers.Remove(handler);
		}

		// Token: 0x06002BD1 RID: 11217 RVA: 0x000A9764 File Offset: 0x000A7964
		public static T GetNetworkComponent<T>() where T : UdpNetworkComponent
		{
			using (List<UdpNetworkComponent>.Enumerator enumerator = GameNetwork.NetworkComponents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t;
					if ((t = enumerator.Current as T) != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06002BD2 RID: 11218 RVA: 0x000A97D0 File Offset: 0x000A79D0
		// (set) Token: 0x06002BD3 RID: 11219 RVA: 0x000A97D7 File Offset: 0x000A79D7
		public static List<UdpNetworkComponent> NetworkComponents { get; private set; }

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x000A97DF File Offset: 0x000A79DF
		// (set) Token: 0x06002BD5 RID: 11221 RVA: 0x000A97E6 File Offset: 0x000A79E6
		public static List<IUdpNetworkHandler> NetworkHandlers { get; private set; }

		// Token: 0x06002BD6 RID: 11222 RVA: 0x000A97F0 File Offset: 0x000A79F0
		public static void WriteMessage(GameNetworkMessage message)
		{
			if ((message.GetLogFilter() & GameNetwork.MultiplayerLogging) != MultiplayerMessageFilter.None)
			{
				Debug.Print("Writing message: " + message.GetLogFormat(), 0, Debug.DebugColor.White, 17179869184UL);
			}
			Type type = message.GetType();
			message.MessageId = GameNetwork._gameNetworkMessageTypesAll[type];
			message.Write();
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x000A984C File Offset: 0x000A7A4C
		private static void AddServerMessageHandler<T>(GameNetworkMessage.ServerMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
		{
			int num = GameNetwork._gameNetworkMessageTypesFromServer[typeof(T)];
			GameNetwork._fromServerMessageHandlers[num].Add(handler);
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x000A9880 File Offset: 0x000A7A80
		private static void AddServerBaseMessageHandler(GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage> handler, Type messageType)
		{
			int num = GameNetwork._gameNetworkMessageTypesFromServer[messageType];
			GameNetwork._fromServerBaseMessageHandlers[num].Add(handler);
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x000A98AC File Offset: 0x000A7AAC
		private static void AddClientMessageHandler<T>(GameNetworkMessage.ClientMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
		{
			int num = GameNetwork._gameNetworkMessageTypesFromClient[typeof(T)];
			GameNetwork._fromClientMessageHandlers[num].Add(handler);
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x000A98E0 File Offset: 0x000A7AE0
		private static void AddClientBaseMessageHandler(GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage> handler, Type messageType)
		{
			int num = GameNetwork._gameNetworkMessageTypesFromClient[messageType];
			GameNetwork._fromClientBaseMessageHandlers[num].Add(handler);
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x000A990C File Offset: 0x000A7B0C
		private static void RemoveServerMessageHandler<T>(GameNetworkMessage.ServerMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
		{
			int num = GameNetwork._gameNetworkMessageTypesFromServer[typeof(T)];
			GameNetwork._fromServerMessageHandlers[num].Remove(handler);
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x000A9940 File Offset: 0x000A7B40
		private static void RemoveServerBaseMessageHandler(GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage> handler, Type messageType)
		{
			int num = GameNetwork._gameNetworkMessageTypesFromServer[messageType];
			GameNetwork._fromServerBaseMessageHandlers[num].Remove(handler);
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x000A996C File Offset: 0x000A7B6C
		private static void RemoveClientMessageHandler<T>(GameNetworkMessage.ClientMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
		{
			int num = GameNetwork._gameNetworkMessageTypesFromClient[typeof(T)];
			GameNetwork._fromClientMessageHandlers[num].Remove(handler);
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x000A99A0 File Offset: 0x000A7BA0
		internal static void FindGameNetworkMessages()
		{
			Debug.Print("Searching Game NetworkMessages Methods", 0, Debug.DebugColor.White, 17179869184UL);
			GameNetwork._fromClientMessageHandlers = new Dictionary<int, List<object>>();
			GameNetwork._fromServerMessageHandlers = new Dictionary<int, List<object>>();
			GameNetwork._fromClientBaseMessageHandlers = new Dictionary<int, List<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>>>();
			GameNetwork._fromServerBaseMessageHandlers = new Dictionary<int, List<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>>>();
			GameNetwork._gameNetworkMessageTypesAll = new Dictionary<Type, int>();
			GameNetwork._gameNetworkMessageTypesFromClient = new Dictionary<Type, int>();
			GameNetwork._gameNetworkMessageTypesFromServer = new Dictionary<Type, int>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			List<Type> list = new List<Type>();
			List<Type> list2 = new List<Type>();
			foreach (Assembly assembly in assemblies)
			{
				if (GameNetwork.CheckAssemblyForNetworkMessage(assembly))
				{
					GameNetwork.CollectGameNetworkMessagesFromAssembly(assembly, list, list2);
				}
			}
			list.Sort((Type s1, Type s2) => s1.FullName.CompareTo(s2.FullName));
			list2.Sort((Type s1, Type s2) => s1.FullName.CompareTo(s2.FullName));
			GameNetwork._gameNetworkMessageIdsFromClient = new List<Type>(list.Count);
			for (int j = 0; j < list.Count; j++)
			{
				Type type = list[j];
				GameNetwork._gameNetworkMessageIdsFromClient.Add(type);
				GameNetwork._gameNetworkMessageTypesFromClient.Add(type, j);
				GameNetwork._gameNetworkMessageTypesAll.Add(type, j);
				GameNetwork._fromClientMessageHandlers.Add(j, new List<object>());
				GameNetwork._fromClientBaseMessageHandlers.Add(j, new List<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>>());
			}
			GameNetwork._gameNetworkMessageIdsFromServer = new List<Type>(list2.Count);
			for (int k = 0; k < list2.Count; k++)
			{
				Type type2 = list2[k];
				GameNetwork._gameNetworkMessageIdsFromServer.Add(type2);
				GameNetwork._gameNetworkMessageTypesFromServer.Add(type2, k);
				GameNetwork._gameNetworkMessageTypesAll.Add(type2, k);
				GameNetwork._fromServerMessageHandlers.Add(k, new List<object>());
				GameNetwork._fromServerBaseMessageHandlers.Add(k, new List<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>>());
			}
			CompressionBasic.NetworkComponentEventTypeFromClientCompressionInfo = new CompressionInfo.Integer(0, list.Count - 1, true);
			CompressionBasic.NetworkComponentEventTypeFromServerCompressionInfo = new CompressionInfo.Integer(0, list2.Count - 1, true);
			Debug.Print("Found " + list.Count + " Client Game Network Messages", 0, Debug.DebugColor.White, 17179869184UL);
			Debug.Print("Found " + list2.Count + " Server Game Network Messages", 0, Debug.DebugColor.White, 17179869184UL);
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x000A9C00 File Offset: 0x000A7E00
		internal static void FindSynchedMissionObjectTypes()
		{
			Debug.Print("Searching Game SynchedMissionObjects", 0, Debug.DebugColor.White, 17179869184UL);
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			GameNetwork._synchedMissionObjectClassTypes = new List<Type>();
			foreach (Assembly assembly in assemblies)
			{
				if (GameNetwork.CheckAssemblyForNetworkMessage(assembly))
				{
					GameNetwork.CollectSynchedMissionObjectTypesFromAssembly(assembly, GameNetwork._synchedMissionObjectClassTypes);
				}
			}
			GameNetwork._synchedMissionObjectClassTypes.Sort((Type s1, Type s2) => s1.FullName.CompareTo(s2.FullName));
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x000A9C88 File Offset: 0x000A7E88
		private static void RemoveClientBaseMessageHandler(GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage> handler, Type messageType)
		{
			int num = GameNetwork._gameNetworkMessageTypesFromClient[messageType];
			GameNetwork._fromClientBaseMessageHandlers[num].Remove(handler);
		}

		// Token: 0x06002BE1 RID: 11233 RVA: 0x000A9CB4 File Offset: 0x000A7EB4
		private static bool CheckAssemblyForNetworkMessage(Assembly assembly)
		{
			Assembly assembly2 = Assembly.GetAssembly(typeof(GameNetworkMessage));
			if (assembly == assembly2)
			{
				return true;
			}
			AssemblyName[] referencedAssembliesSafe = assembly.GetReferencedAssembliesSafe();
			for (int i = 0; i < referencedAssembliesSafe.Length; i++)
			{
				if (referencedAssembliesSafe[i].FullName == assembly2.FullName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x000A9D09 File Offset: 0x000A7F09
		public static void SetServerBandwidthLimitInMbps(double value)
		{
			MBAPI.IMBNetwork.SetServerBandwidthLimitInMbps(value);
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x000A9D16 File Offset: 0x000A7F16
		public static void SetServerTickRate(double value)
		{
			MBAPI.IMBNetwork.SetServerTickRate(value);
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x000A9D23 File Offset: 0x000A7F23
		public static void SetServerFrameRate(double value)
		{
			MBAPI.IMBNetwork.SetServerFrameRate(value);
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x000A9D30 File Offset: 0x000A7F30
		public static void ResetDebugVariables()
		{
			MBAPI.IMBNetwork.ResetDebugVariables();
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x000A9D3C File Offset: 0x000A7F3C
		public static void PrintDebugStats()
		{
			MBAPI.IMBNetwork.PrintDebugStats();
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x000A9D48 File Offset: 0x000A7F48
		public static float GetAveragePacketLossRatio()
		{
			return MBAPI.IMBNetwork.GetAveragePacketLossRatio();
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x000A9D54 File Offset: 0x000A7F54
		public static void GetDebugUploadsInBits(ref GameNetwork.DebugNetworkPacketStatisticsStruct networkStatisticsStruct, ref GameNetwork.DebugNetworkPositionCompressionStatisticsStruct posStatisticsStruct)
		{
			MBAPI.IMBNetwork.GetDebugUploadsInBits(ref networkStatisticsStruct, ref posStatisticsStruct);
		}

		// Token: 0x06002BE9 RID: 11241 RVA: 0x000A9D62 File Offset: 0x000A7F62
		public static void PrintReplicationTableStatistics()
		{
			MBAPI.IMBNetwork.PrintReplicationTableStatistics();
		}

		// Token: 0x06002BEA RID: 11242 RVA: 0x000A9D6E File Offset: 0x000A7F6E
		public static void ClearReplicationTableStatistics()
		{
			MBAPI.IMBNetwork.ClearReplicationTableStatistics();
		}

		// Token: 0x06002BEB RID: 11243 RVA: 0x000A9D7A File Offset: 0x000A7F7A
		public static void ResetDebugUploads()
		{
			MBAPI.IMBNetwork.ResetDebugUploads();
		}

		// Token: 0x06002BEC RID: 11244 RVA: 0x000A9D86 File Offset: 0x000A7F86
		public static void ResetMissionData()
		{
			MBAPI.IMBNetwork.ResetMissionData();
		}

		// Token: 0x06002BED RID: 11245 RVA: 0x000A9D92 File Offset: 0x000A7F92
		private static void AddPeerToDisconnect(NetworkCommunicator networkPeer)
		{
			MBAPI.IMBNetwork.AddPeerToDisconnect(networkPeer.Index);
		}

		// Token: 0x06002BEE RID: 11246 RVA: 0x000A9DA4 File Offset: 0x000A7FA4
		public static void InitializeCompressionInfos()
		{
			CompressionBasic.ActionCodeCompressionInfo = new CompressionInfo.Integer(ActionIndexCache.act_none.Index, MBAnimation.GetNumActionCodes() - 1, true);
			CompressionBasic.AnimationIndexCompressionInfo = new CompressionInfo.Integer(0, MBAnimation.GetNumAnimations() - 1, true);
			CompressionBasic.CultureIndexCompressionInfo = new CompressionInfo.Integer(-1, MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>().Count - 1, true);
			CompressionBasic.SoundEventsCompressionInfo = new CompressionInfo.Integer(0, SoundEvent.GetTotalEventCount() - 1, true);
			CompressionMission.ActionSetCompressionInfo = new CompressionInfo.Integer(0, MBActionSet.GetNumberOfActionSets() - 1, true);
			CompressionMission.MonsterUsageSetCompressionInfo = new CompressionInfo.Integer(0, MBActionSet.GetNumberOfMonsterUsageSets() - 1, true);
		}

		// Token: 0x06002BEF RID: 11247 RVA: 0x000A9E36 File Offset: 0x000A8036
		[MBCallback(null, false)]
		internal static void SyncRelevantGameOptionsToServer()
		{
			SyncRelevantGameOptionsToServer syncRelevantGameOptionsToServer = new SyncRelevantGameOptionsToServer();
			syncRelevantGameOptionsToServer.InitializeOptions();
			GameNetwork.BeginModuleEventAsClient();
			GameNetwork.WriteMessage(syncRelevantGameOptionsToServer);
			GameNetwork.EndModuleEventAsClient();
		}

		// Token: 0x06002BF0 RID: 11248 RVA: 0x000A9E54 File Offset: 0x000A8054
		private static void CollectGameNetworkMessagesFromAssembly(Assembly assembly, List<Type> gameNetworkMessagesFromClient, List<Type> gameNetworkMessagesFromServer)
		{
			Type typeFromHandle = typeof(GameNetworkMessage);
			bool? flag = null;
			List<Type> typesSafe = assembly.GetTypesSafe(null);
			for (int i = 0; i < typesSafe.Count; i++)
			{
				Type type = typesSafe[i];
				if (typeFromHandle.IsAssignableFrom(type) && type != typeFromHandle && type.IsSealed && !(type.GetConstructor(Type.EmptyTypes) == null))
				{
					DefineGameNetworkMessageType customAttribute = type.GetCustomAttribute<DefineGameNetworkMessageType>();
					if (customAttribute != null)
					{
						if (flag == null || !flag.Value)
						{
							flag = new bool?(false);
							GameNetworkMessageSendType sendType = customAttribute.SendType;
							if (sendType != GameNetworkMessageSendType.FromClient)
							{
								if (sendType - GameNetworkMessageSendType.FromServer <= 1)
								{
									gameNetworkMessagesFromServer.Add(type);
								}
							}
							else
							{
								gameNetworkMessagesFromClient.Add(type);
							}
						}
					}
					else
					{
						DefineGameNetworkMessageTypeForMod customAttribute2 = type.GetCustomAttribute<DefineGameNetworkMessageTypeForMod>();
						if (customAttribute2 != null && (flag == null || flag.Value))
						{
							flag = new bool?(true);
							GameNetworkMessageSendType sendType2 = customAttribute2.SendType;
							if (sendType2 != GameNetworkMessageSendType.FromClient)
							{
								if (sendType2 - GameNetworkMessageSendType.FromServer <= 1)
								{
									gameNetworkMessagesFromServer.Add(type);
								}
							}
							else
							{
								gameNetworkMessagesFromClient.Add(type);
							}
						}
					}
				}
			}
		}

		// Token: 0x06002BF1 RID: 11249 RVA: 0x000A9F84 File Offset: 0x000A8184
		private static void CollectSynchedMissionObjectTypesFromAssembly(Assembly assembly, List<Type> synchedMissionObjectClassTypes)
		{
			Type typeFromHandle = typeof(ISynchedMissionObjectReadableRecord);
			bool? flag = null;
			List<Type> typesSafe = assembly.GetTypesSafe(null);
			for (int i = 0; i < typesSafe.Count; i++)
			{
				Type type = typesSafe[i];
				if (typeFromHandle.IsAssignableFrom(type) && type != typeFromHandle)
				{
					if (type.GetCustomAttribute<DefineSynchedMissionObjectType>() != null)
					{
						if (flag == null || !flag.Value)
						{
							flag = new bool?(false);
							synchedMissionObjectClassTypes.Add(type);
						}
					}
					else if (type.GetCustomAttribute<DefineSynchedMissionObjectTypeForMod>() != null && (flag == null || flag.Value))
					{
						flag = new bool?(true);
						synchedMissionObjectClassTypes.Add(type);
					}
				}
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06002BF2 RID: 11250 RVA: 0x000AA041 File Offset: 0x000A8241
		// (set) Token: 0x06002BF3 RID: 11251 RVA: 0x000AA048 File Offset: 0x000A8248
		public static NetworkCommunicator MyPeer { get; private set; }

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06002BF4 RID: 11252 RVA: 0x000AA050 File Offset: 0x000A8250
		public static bool IsMyPeerReady
		{
			get
			{
				return GameNetwork.MyPeer != null && GameNetwork.MyPeer.IsSynchronized;
			}
		}

		// Token: 0x040010F9 RID: 4345
		public const int MaxAutomatedBattleIndex = 10;

		// Token: 0x040010FA RID: 4346
		public const int MaxPlayerCount = 1023;

		// Token: 0x040010FB RID: 4347
		private static IGameNetworkHandler _handler;

		// Token: 0x040010FF RID: 4351
		public static int ClientPeerIndex;

		// Token: 0x04001100 RID: 4352
		private static MultiplayerMessageFilter MultiplayerLogging = (MultiplayerMessageFilter)(-1);

		// Token: 0x04001103 RID: 4355
		private static Dictionary<Type, int> _gameNetworkMessageTypesAll;

		// Token: 0x04001104 RID: 4356
		private static Dictionary<Type, int> _gameNetworkMessageTypesFromClient;

		// Token: 0x04001105 RID: 4357
		private static List<Type> _gameNetworkMessageIdsFromClient;

		// Token: 0x04001106 RID: 4358
		private static Dictionary<Type, int> _gameNetworkMessageTypesFromServer;

		// Token: 0x04001107 RID: 4359
		private static List<Type> _gameNetworkMessageIdsFromServer;

		// Token: 0x04001108 RID: 4360
		private static Dictionary<int, List<object>> _fromClientMessageHandlers;

		// Token: 0x04001109 RID: 4361
		private static Dictionary<int, List<object>> _fromServerMessageHandlers;

		// Token: 0x0400110A RID: 4362
		private static Dictionary<int, List<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>>> _fromClientBaseMessageHandlers;

		// Token: 0x0400110B RID: 4363
		private static Dictionary<int, List<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>>> _fromServerBaseMessageHandlers;

		// Token: 0x0400110C RID: 4364
		private static List<Type> _synchedMissionObjectClassTypes;

		// Token: 0x020005D7 RID: 1495
		public class NetworkMessageHandlerRegisterer
		{
			// Token: 0x06003F5B RID: 16219 RVA: 0x000F998E File Offset: 0x000F7B8E
			public NetworkMessageHandlerRegisterer(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode definitionMode)
			{
				this._registerMode = definitionMode;
			}

			// Token: 0x06003F5C RID: 16220 RVA: 0x000F999D File Offset: 0x000F7B9D
			public void Register<T>(GameNetworkMessage.ServerMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
			{
				if (this._registerMode == GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add)
				{
					GameNetwork.AddServerMessageHandler<T>(handler);
					return;
				}
				GameNetwork.RemoveServerMessageHandler<T>(handler);
			}

			// Token: 0x06003F5D RID: 16221 RVA: 0x000F99B4 File Offset: 0x000F7BB4
			public void RegisterBaseHandler<T>(GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage> handler) where T : GameNetworkMessage
			{
				if (this._registerMode == GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add)
				{
					GameNetwork.AddServerBaseMessageHandler(handler, typeof(T));
					return;
				}
				GameNetwork.RemoveServerBaseMessageHandler(handler, typeof(T));
			}

			// Token: 0x06003F5E RID: 16222 RVA: 0x000F99DF File Offset: 0x000F7BDF
			public void Register<T>(GameNetworkMessage.ClientMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
			{
				if (this._registerMode == GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add)
				{
					GameNetwork.AddClientMessageHandler<T>(handler);
					return;
				}
				GameNetwork.RemoveClientMessageHandler<T>(handler);
			}

			// Token: 0x06003F5F RID: 16223 RVA: 0x000F99F6 File Offset: 0x000F7BF6
			public void RegisterBaseHandler<T>(GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage> handler) where T : GameNetworkMessage
			{
				if (this._registerMode == GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add)
				{
					GameNetwork.AddClientBaseMessageHandler(handler, typeof(T));
					return;
				}
				GameNetwork.RemoveClientBaseMessageHandler(handler, typeof(T));
			}

			// Token: 0x04001FC9 RID: 8137
			private readonly GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode _registerMode;

			// Token: 0x020006D1 RID: 1745
			public enum RegisterMode
			{
				// Token: 0x040023DB RID: 9179
				Add,
				// Token: 0x040023DC RID: 9180
				Remove
			}
		}

		// Token: 0x020005D8 RID: 1496
		public class NetworkMessageHandlerRegistererContainer
		{
			// Token: 0x06003F60 RID: 16224 RVA: 0x000F9A21 File Offset: 0x000F7C21
			public NetworkMessageHandlerRegistererContainer()
			{
				this._fromClientHandlers = new List<Delegate>();
				this._fromServerHandlers = new List<Delegate>();
				this._fromServerBaseHandlers = new List<Tuple<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>, Type>>();
				this._fromClientBaseHandlers = new List<Tuple<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>, Type>>();
			}

			// Token: 0x06003F61 RID: 16225 RVA: 0x000F9A55 File Offset: 0x000F7C55
			public void RegisterBaseHandler<T>(GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage> handler) where T : GameNetworkMessage
			{
				this._fromServerBaseHandlers.Add(new Tuple<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>, Type>(handler, typeof(T)));
			}

			// Token: 0x06003F62 RID: 16226 RVA: 0x000F9A72 File Offset: 0x000F7C72
			public void Register<T>(GameNetworkMessage.ServerMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
			{
				this._fromServerHandlers.Add(handler);
			}

			// Token: 0x06003F63 RID: 16227 RVA: 0x000F9A80 File Offset: 0x000F7C80
			public void RegisterBaseHandler<T>(GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage> handler)
			{
				this._fromClientBaseHandlers.Add(new Tuple<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>, Type>(handler, typeof(T)));
			}

			// Token: 0x06003F64 RID: 16228 RVA: 0x000F9A9D File Offset: 0x000F7C9D
			public void Register<T>(GameNetworkMessage.ClientMessageHandlerDelegate<T> handler) where T : GameNetworkMessage
			{
				this._fromClientHandlers.Add(handler);
			}

			// Token: 0x06003F65 RID: 16229 RVA: 0x000F9AAC File Offset: 0x000F7CAC
			public void RegisterMessages()
			{
				if (this._fromServerHandlers.Count > 0 || this._fromServerBaseHandlers.Count > 0)
				{
					foreach (Delegate @delegate in this._fromServerHandlers)
					{
						Type type = @delegate.GetType().GenericTypeArguments[0];
						int num = GameNetwork._gameNetworkMessageTypesFromServer[type];
						GameNetwork._fromServerMessageHandlers[num].Add(@delegate);
					}
					using (List<Tuple<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>, Type>>.Enumerator enumerator2 = this._fromServerBaseHandlers.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Tuple<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>, Type> tuple = enumerator2.Current;
							int num2 = GameNetwork._gameNetworkMessageTypesFromServer[tuple.Item2];
							GameNetwork._fromServerBaseMessageHandlers[num2].Add(tuple.Item1);
						}
						return;
					}
				}
				foreach (Delegate delegate2 in this._fromClientHandlers)
				{
					Type type2 = delegate2.GetType().GenericTypeArguments[0];
					int num3 = GameNetwork._gameNetworkMessageTypesFromClient[type2];
					GameNetwork._fromClientMessageHandlers[num3].Add(delegate2);
				}
				foreach (Tuple<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>, Type> tuple2 in this._fromClientBaseHandlers)
				{
					int num4 = GameNetwork._gameNetworkMessageTypesFromClient[tuple2.Item2];
					GameNetwork._fromClientBaseMessageHandlers[num4].Add(tuple2.Item1);
				}
			}

			// Token: 0x06003F66 RID: 16230 RVA: 0x000F9C84 File Offset: 0x000F7E84
			public void UnregisterMessages()
			{
				if (this._fromServerHandlers.Count > 0 || this._fromServerBaseHandlers.Count > 0)
				{
					foreach (Delegate @delegate in this._fromServerHandlers)
					{
						Type type = @delegate.GetType().GenericTypeArguments[0];
						int num = GameNetwork._gameNetworkMessageTypesFromServer[type];
						GameNetwork._fromServerMessageHandlers[num].Remove(@delegate);
					}
					using (List<Tuple<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>, Type>>.Enumerator enumerator2 = this._fromServerBaseHandlers.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Tuple<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>, Type> tuple = enumerator2.Current;
							int num2 = GameNetwork._gameNetworkMessageTypesFromServer[tuple.Item2];
							GameNetwork._fromServerBaseMessageHandlers[num2].Remove(tuple.Item1);
						}
						return;
					}
				}
				foreach (Delegate delegate2 in this._fromClientHandlers)
				{
					Type type2 = delegate2.GetType().GenericTypeArguments[0];
					int num3 = GameNetwork._gameNetworkMessageTypesFromClient[type2];
					GameNetwork._fromClientMessageHandlers[num3].Remove(delegate2);
				}
				foreach (Tuple<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>, Type> tuple2 in this._fromClientBaseHandlers)
				{
					int num4 = GameNetwork._gameNetworkMessageTypesFromClient[tuple2.Item2];
					GameNetwork._fromClientBaseMessageHandlers[num4].Remove(tuple2.Item1);
				}
			}

			// Token: 0x04001FCA RID: 8138
			private List<Delegate> _fromClientHandlers;

			// Token: 0x04001FCB RID: 8139
			private List<Delegate> _fromServerHandlers;

			// Token: 0x04001FCC RID: 8140
			private List<Tuple<GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>, Type>> _fromServerBaseHandlers;

			// Token: 0x04001FCD RID: 8141
			private List<Tuple<GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>, Type>> _fromClientBaseHandlers;
		}

		// Token: 0x020005D9 RID: 1497
		[Flags]
		public enum EventBroadcastFlags
		{
			// Token: 0x04001FCF RID: 8143
			None = 0,
			// Token: 0x04001FD0 RID: 8144
			ExcludeTargetPlayer = 1,
			// Token: 0x04001FD1 RID: 8145
			ExcludeNoBloodStainsOption = 2,
			// Token: 0x04001FD2 RID: 8146
			ExcludeNoParticlesOption = 4,
			// Token: 0x04001FD3 RID: 8147
			ExcludeNoSoundOption = 8,
			// Token: 0x04001FD4 RID: 8148
			AddToMissionRecord = 16,
			// Token: 0x04001FD5 RID: 8149
			IncludeUnsynchronizedClients = 32,
			// Token: 0x04001FD6 RID: 8150
			ExcludeOtherTeamPlayers = 64,
			// Token: 0x04001FD7 RID: 8151
			ExcludePeerTeamPlayers = 128,
			// Token: 0x04001FD8 RID: 8152
			DontSendToPeers = 256
		}

		// Token: 0x020005DA RID: 1498
		[EngineStruct("Debug_network_position_compression_statistics_struct", false, null)]
		public struct DebugNetworkPositionCompressionStatisticsStruct
		{
			// Token: 0x04001FD9 RID: 8153
			public int totalPositionUpload;

			// Token: 0x04001FDA RID: 8154
			public int totalPositionPrecisionBitCount;

			// Token: 0x04001FDB RID: 8155
			public int totalPositionCoarseBitCountX;

			// Token: 0x04001FDC RID: 8156
			public int totalPositionCoarseBitCountY;

			// Token: 0x04001FDD RID: 8157
			public int totalPositionCoarseBitCountZ;
		}

		// Token: 0x020005DB RID: 1499
		[EngineStruct("Debug_network_packet_statistics_struct", false, null)]
		public struct DebugNetworkPacketStatisticsStruct
		{
			// Token: 0x04001FDE RID: 8158
			public int TotalPackets;

			// Token: 0x04001FDF RID: 8159
			public int TotalUpload;

			// Token: 0x04001FE0 RID: 8160
			public int TotalConstantsUpload;

			// Token: 0x04001FE1 RID: 8161
			public int TotalReliableEventUpload;

			// Token: 0x04001FE2 RID: 8162
			public int TotalReplicationUpload;

			// Token: 0x04001FE3 RID: 8163
			public int TotalUnreliableEventUpload;

			// Token: 0x04001FE4 RID: 8164
			public int TotalReplicationTableAdderCount;

			// Token: 0x04001FE5 RID: 8165
			public int TotalReplicationTableAdderBitCount;

			// Token: 0x04001FE6 RID: 8166
			public int TotalReplicationTableAdder;

			// Token: 0x04001FE7 RID: 8167
			public double TotalCellPriority;

			// Token: 0x04001FE8 RID: 8168
			public double TotalCellAgentPriority;

			// Token: 0x04001FE9 RID: 8169
			public double TotalCellCellPriority;

			// Token: 0x04001FEA RID: 8170
			public int TotalCellPriorityChecks;

			// Token: 0x04001FEB RID: 8171
			public int TotalSentCellCount;

			// Token: 0x04001FEC RID: 8172
			public int TotalNotSentCellCount;

			// Token: 0x04001FED RID: 8173
			public int TotalReplicationWriteCount;

			// Token: 0x04001FEE RID: 8174
			public int CurMaxPacketSizeInBytes;

			// Token: 0x04001FEF RID: 8175
			public double AveragePingTime;

			// Token: 0x04001FF0 RID: 8176
			public double AverageDtToSendPacket;

			// Token: 0x04001FF1 RID: 8177
			public double TimeOutPeriod;

			// Token: 0x04001FF2 RID: 8178
			public double PacingRate;

			// Token: 0x04001FF3 RID: 8179
			public double DeliveryRate;

			// Token: 0x04001FF4 RID: 8180
			public double RoundTripTime;

			// Token: 0x04001FF5 RID: 8181
			public int InflightBitCount;

			// Token: 0x04001FF6 RID: 8182
			public int IsCongested;

			// Token: 0x04001FF7 RID: 8183
			public int ProbeBwPhaseIndex;

			// Token: 0x04001FF8 RID: 8184
			public double LostPercent;

			// Token: 0x04001FF9 RID: 8185
			public int LostCount;

			// Token: 0x04001FFA RID: 8186
			public int TotalCountOnLostCheck;
		}

		// Token: 0x020005DC RID: 1500
		public struct AddPlayersResult
		{
			// Token: 0x04001FFB RID: 8187
			public bool Success;

			// Token: 0x04001FFC RID: 8188
			public NetworkCommunicator[] NetworkPeers;
		}
	}
}
