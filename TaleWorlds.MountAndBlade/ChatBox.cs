using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002FD RID: 765
	public class ChatBox : GameHandler
	{
		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06002C04 RID: 11268 RVA: 0x000AA090 File Offset: 0x000A8290
		// (set) Token: 0x06002C05 RID: 11269 RVA: 0x000AA098 File Offset: 0x000A8298
		public bool IsContentRestricted { get; private set; }

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06002C06 RID: 11270 RVA: 0x000AA0A1 File Offset: 0x000A82A1
		public bool NetworkReady
		{
			get
			{
				return GameNetwork.IsClient || GameNetwork.IsServer || (NetworkMain.GameClient != null && NetworkMain.GameClient.Connected);
			}
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x000AA0C6 File Offset: 0x000A82C6
		protected override void OnGameStart()
		{
			ChatBox._chatBox = this;
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x000AA0CE File Offset: 0x000A82CE
		public override void OnBeforeSave()
		{
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x000AA0D0 File Offset: 0x000A82D0
		public override void OnAfterSave()
		{
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x000AA0D2 File Offset: 0x000A82D2
		protected override void OnGameEnd()
		{
			ChatBox._chatBox = null;
		}

		// Token: 0x06002C0B RID: 11275 RVA: 0x000AA0DA File Offset: 0x000A82DA
		public void SendMessageToAll(string message)
		{
			this.SendMessageToAll(message, null);
		}

		// Token: 0x06002C0C RID: 11276 RVA: 0x000AA0E4 File Offset: 0x000A82E4
		public void SendMessageToAll(string message, List<VirtualPlayer> receiverList)
		{
			if (GameNetwork.IsClient && !this.IsContentRestricted)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new NetworkMessages.FromClient.PlayerMessageAll(message));
				GameNetwork.EndModuleEventAsClient();
				return;
			}
			if (GameNetwork.IsServer)
			{
				this.ServerPrepareAndSendMessage(GameNetwork.MyPeer, false, message, receiverList);
			}
		}

		// Token: 0x06002C0D RID: 11277 RVA: 0x000AA121 File Offset: 0x000A8321
		public void SendMessageToTeam(string message)
		{
			this.SendMessageToTeam(message, null);
		}

		// Token: 0x06002C0E RID: 11278 RVA: 0x000AA12B File Offset: 0x000A832B
		public void SendMessageToTeam(string message, List<VirtualPlayer> receiverList)
		{
			if (GameNetwork.IsClient && !this.IsContentRestricted)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new NetworkMessages.FromClient.PlayerMessageTeam(message));
				GameNetwork.EndModuleEventAsClient();
				return;
			}
			if (GameNetwork.IsServer)
			{
				this.ServerPrepareAndSendMessage(GameNetwork.MyPeer, true, message, receiverList);
			}
		}

		// Token: 0x06002C0F RID: 11279 RVA: 0x000AA168 File Offset: 0x000A8368
		public void SendMessageToWhisperTarget(string message, string platformName, string whisperTarget)
		{
			if (NetworkMain.GameClient != null && NetworkMain.GameClient.Connected)
			{
				NetworkMain.GameClient.SendWhisper(whisperTarget, message);
				if (this.WhisperMessageSent != null)
				{
					this.WhisperMessageSent(message, whisperTarget);
				}
			}
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x000AA19E File Offset: 0x000A839E
		private void OnServerMessage(string message)
		{
			if (this.ServerMessage != null)
			{
				this.ServerMessage(message);
			}
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x000AA1B4 File Offset: 0x000A83B4
		protected override void OnGameNetworkBegin()
		{
			ChatBox._queuedTeamMessages = new List<ChatBox.QueuedMessageInfo>();
			ChatBox._queuedEveryoneMessages = new List<ChatBox.QueuedMessageInfo>();
			this._isNetworkInitialized = true;
			this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
		}

		// Token: 0x06002C12 RID: 11282 RVA: 0x000AA1D8 File Offset: 0x000A83D8
		private void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode mode)
		{
			GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(mode);
			if (GameNetwork.IsClient)
			{
				networkMessageHandlerRegisterer.Register<NetworkMessages.FromServer.PlayerMessageTeam>(new GameNetworkMessage.ServerMessageHandlerDelegate<NetworkMessages.FromServer.PlayerMessageTeam>(this.HandleServerEventPlayerMessageTeam));
				networkMessageHandlerRegisterer.Register<NetworkMessages.FromServer.PlayerMessageAll>(new GameNetworkMessage.ServerMessageHandlerDelegate<NetworkMessages.FromServer.PlayerMessageAll>(this.HandleServerEventPlayerMessageAll));
				networkMessageHandlerRegisterer.Register<ServerMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<ServerMessage>(this.HandleServerEventServerMessage));
				networkMessageHandlerRegisterer.Register<ServerAdminMessage>(new GameNetworkMessage.ServerMessageHandlerDelegate<ServerAdminMessage>(this.HandleServerEventServerAdminMessage));
				return;
			}
			if (GameNetwork.IsServer)
			{
				networkMessageHandlerRegisterer.Register<NetworkMessages.FromClient.PlayerMessageAll>(new GameNetworkMessage.ClientMessageHandlerDelegate<NetworkMessages.FromClient.PlayerMessageAll>(this.HandleClientEventPlayerMessageAll));
				networkMessageHandlerRegisterer.Register<NetworkMessages.FromClient.PlayerMessageTeam>(new GameNetworkMessage.ClientMessageHandlerDelegate<NetworkMessages.FromClient.PlayerMessageTeam>(this.HandleClientEventPlayerMessageTeam));
			}
		}

		// Token: 0x06002C13 RID: 11283 RVA: 0x000AA267 File Offset: 0x000A8467
		protected override void OnGameNetworkEnd()
		{
			base.OnGameNetworkEnd();
			this.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x000AA278 File Offset: 0x000A8478
		private void HandleServerEventPlayerMessageAll(NetworkMessages.FromServer.PlayerMessageAll message)
		{
			if (!this.IsContentRestricted)
			{
				this.ShouldShowPlayersMessage(message.Player.VirtualPlayer.Id, delegate(bool result)
				{
					if (result)
					{
						this.OnPlayerMessageReceived(message.Player, message.Message, false);
					}
				});
			}
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x000AA2C8 File Offset: 0x000A84C8
		private void HandleServerEventPlayerMessageTeam(NetworkMessages.FromServer.PlayerMessageTeam message)
		{
			if (!this.IsContentRestricted)
			{
				this.ShouldShowPlayersMessage(message.Player.VirtualPlayer.Id, delegate(bool result)
				{
					if (result)
					{
						this.OnPlayerMessageReceived(message.Player, message.Message, true);
					}
				});
			}
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x000AA318 File Offset: 0x000A8518
		private void HandleServerEventServerMessage(ServerMessage message)
		{
			this.OnServerMessage(message.IsMessageTextId ? GameTexts.FindText(message.Message, null).ToString() : message.Message);
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x000AA344 File Offset: 0x000A8544
		private void HandleServerEventServerAdminMessage(ServerAdminMessage message)
		{
			if (message.IsAdminBroadcast)
			{
				TextObject textObject = new TextObject("{=!}{ADMIN_TEXT}", null);
				textObject.SetTextVariable("ADMIN_TEXT", message.Message);
				MBInformationManager.AddQuickInformation(textObject, 5000, null, null, "");
				SoundEvent.PlaySound2D("event:/ui/notification/alert");
			}
			ServerAdminMessageDelegate serverAdminMessage = this.ServerAdminMessage;
			if (serverAdminMessage == null)
			{
				return;
			}
			serverAdminMessage(message.Message);
		}

		// Token: 0x06002C18 RID: 11288 RVA: 0x000AA3A8 File Offset: 0x000A85A8
		private bool HandleClientEventPlayerMessageAll(NetworkCommunicator networkPeer, NetworkMessages.FromClient.PlayerMessageAll message)
		{
			return this.ServerPrepareAndSendMessage(networkPeer, false, message.Message, message.ReceiverList);
		}

		// Token: 0x06002C19 RID: 11289 RVA: 0x000AA3BE File Offset: 0x000A85BE
		private bool HandleClientEventPlayerMessageTeam(NetworkCommunicator networkPeer, NetworkMessages.FromClient.PlayerMessageTeam message)
		{
			return this.ServerPrepareAndSendMessage(networkPeer, true, message.Message, message.ReceiverList);
		}

		// Token: 0x06002C1A RID: 11290 RVA: 0x000AA3D4 File Offset: 0x000A85D4
		private static bool IsPeerSpectator(NetworkCommunicator networkPeer)
		{
			return SpectatorHelper.IsPeerSpectator(networkPeer);
		}

		// Token: 0x06002C1B RID: 11291 RVA: 0x000AA3DC File Offset: 0x000A85DC
		public static void ServerSendServerMessageToEveryone(string message)
		{
			ChatBox._chatBox.OnServerMessage(message);
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new ServerMessage(message, false, false));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
		}

		// Token: 0x06002C1C RID: 11292 RVA: 0x000AA404 File Offset: 0x000A8604
		private bool ServerPrepareAndSendMessage(NetworkCommunicator fromPeer, bool toTeamOnly, string message, List<VirtualPlayer> receiverList)
		{
			if (GameNetwork.IsDedicatedServer)
			{
				Action<NetworkCommunicator, string> onMessageReceivedAtDedicatedServer = this.OnMessageReceivedAtDedicatedServer;
				if (onMessageReceivedAtDedicatedServer != null)
				{
					onMessageReceivedAtDedicatedServer(fromPeer, message);
				}
			}
			if (!toTeamOnly && ChatBox.IsPeerSpectator(fromPeer))
			{
				toTeamOnly = true;
			}
			if (fromPeer.IsMuted || MultiplayerGlobalMutedPlayersManager.IsUserMuted(fromPeer.VirtualPlayer.Id))
			{
				GameNetwork.BeginModuleEventAsServer(fromPeer);
				GameNetwork.WriteMessage(new ServerMessage("str_multiplayer_muted_message", true, false));
				GameNetwork.EndModuleEventAsServer();
				return true;
			}
			if (this._profanityChecker != null)
			{
				message = this._profanityChecker.CensorText(message);
			}
			if (!GameNetwork.IsDedicatedServer && fromPeer != GameNetwork.MyPeer && !this._mutedPlayers.Contains(fromPeer.VirtualPlayer.Id) && !PermaMuteList.IsPlayerMuted(fromPeer.VirtualPlayer.Id))
			{
				MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
				if (component == null)
				{
					return false;
				}
				bool flag;
				if (toTeamOnly)
				{
					if (component == null)
					{
						return false;
					}
					MissionPeer component2 = fromPeer.GetComponent<MissionPeer>();
					if (component2 == null)
					{
						return false;
					}
					flag = component.Team == component2.Team;
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					this.OnPlayerMessageReceived(fromPeer, message, toTeamOnly);
				}
			}
			if (toTeamOnly)
			{
				ChatBox.ServerSendMessageToTeam(fromPeer, message, receiverList);
			}
			else
			{
				ChatBox.ServerSendMessageToEveryone(fromPeer, message, receiverList);
			}
			return true;
		}

		// Token: 0x06002C1D RID: 11293 RVA: 0x000AA520 File Offset: 0x000A8720
		private static void ServerSendMessageToTeam(NetworkCommunicator networkPeer, string message, List<VirtualPlayer> receiverList)
		{
			if (!networkPeer.IsSynchronized)
			{
				ChatBox._queuedTeamMessages.Add(new ChatBox.QueuedMessageInfo(networkPeer, message, receiverList));
				return;
			}
			MissionPeer missionPeer = networkPeer.GetComponent<MissionPeer>();
			MissionPeer missionPeer2 = missionPeer;
			if (((missionPeer2 != null) ? missionPeer2.Team : null) != null)
			{
				using (IEnumerator<NetworkCommunicator> enumerator = GameNetwork.NetworkPeers.Where<NetworkCommunicator>(delegate(NetworkCommunicator x)
				{
					if (!x.IsServerPeer && x.IsSynchronized)
					{
						MissionPeer component = x.GetComponent<MissionPeer>();
						return ((component != null) ? component.Team : null) == missionPeer.Team;
					}
					return false;
				}).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NetworkCommunicator networkCommunicator = enumerator.Current;
						if (receiverList == null || receiverList.Contains(networkCommunicator.VirtualPlayer))
						{
							GameNetwork.BeginModuleEventAsServer(networkCommunicator);
							GameNetwork.WriteMessage(new NetworkMessages.FromServer.PlayerMessageTeam(networkPeer, message));
							GameNetwork.EndModuleEventAsServer();
						}
					}
					return;
				}
			}
			if (ChatBox.IsPeerSpectator(networkPeer))
			{
				return;
			}
			ChatBox.ServerSendMessageToEveryone(networkPeer, message, receiverList);
		}

		// Token: 0x06002C1E RID: 11294 RVA: 0x000AA5F0 File Offset: 0x000A87F0
		private static void ServerSendMessageToEveryone(NetworkCommunicator networkPeer, string message, List<VirtualPlayer> receiverList)
		{
			if (!networkPeer.IsSynchronized)
			{
				ChatBox._queuedEveryoneMessages.Add(new ChatBox.QueuedMessageInfo(networkPeer, message, receiverList));
				return;
			}
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers.Where<NetworkCommunicator>((NetworkCommunicator x) => !x.IsServerPeer && x.IsSynchronized))
			{
				if (receiverList == null || receiverList.Contains(networkCommunicator.VirtualPlayer))
				{
					GameNetwork.BeginModuleEventAsServer(networkCommunicator);
					GameNetwork.WriteMessage(new NetworkMessages.FromServer.PlayerMessageAll(networkPeer, message));
					GameNetwork.EndModuleEventAsServer();
				}
			}
		}

		// Token: 0x06002C1F RID: 11295 RVA: 0x000AA69C File Offset: 0x000A889C
		public void ResetMuteList()
		{
			this._mutedPlayers.Clear();
		}

		// Token: 0x06002C20 RID: 11296 RVA: 0x000AA6A9 File Offset: 0x000A88A9
		public static void AddWhisperMessage(string fromUserName, string messageBody)
		{
			ChatBox._chatBox.OnWhisperMessageReceived(fromUserName, messageBody);
		}

		// Token: 0x06002C21 RID: 11297 RVA: 0x000AA6B7 File Offset: 0x000A88B7
		public static void AddErrorWhisperMessage(string toUserName)
		{
			ChatBox._chatBox.OnErrorWhisperMessageReceived(toUserName);
		}

		// Token: 0x06002C22 RID: 11298 RVA: 0x000AA6C4 File Offset: 0x000A88C4
		private void OnWhisperMessageReceived(string fromUserName, string messageBody)
		{
			if (this.WhisperMessageReceived != null)
			{
				this.WhisperMessageReceived(fromUserName, messageBody);
			}
		}

		// Token: 0x06002C23 RID: 11299 RVA: 0x000AA6DB File Offset: 0x000A88DB
		private void OnErrorWhisperMessageReceived(string toUserName)
		{
			if (this.ErrorWhisperMessageReceived != null)
			{
				this.ErrorWhisperMessageReceived(toUserName);
			}
		}

		// Token: 0x06002C24 RID: 11300 RVA: 0x000AA6F1 File Offset: 0x000A88F1
		private void OnPlayerMessageReceived(NetworkCommunicator networkPeer, string message, bool toTeamOnly)
		{
			if (this.PlayerMessageReceived != null)
			{
				this.PlayerMessageReceived(networkPeer, message, toTeamOnly);
			}
		}

		// Token: 0x06002C25 RID: 11301 RVA: 0x000AA709 File Offset: 0x000A8909
		public void SetPlayerMuted(PlayerId playerID, bool isMuted)
		{
			if (isMuted)
			{
				this.OnPlayerMuted(playerID);
				return;
			}
			this.OnPlayerUnmuted(playerID);
		}

		// Token: 0x06002C26 RID: 11302 RVA: 0x000AA71D File Offset: 0x000A891D
		public void SetPlayerMutedFromPlatform(PlayerId playerID, bool isMuted)
		{
			if (isMuted && !this._platformMutedPlayers.Contains(playerID))
			{
				this._platformMutedPlayers.Add(playerID);
				return;
			}
			if (!isMuted && this._platformMutedPlayers.Contains(playerID))
			{
				this._platformMutedPlayers.Remove(playerID);
			}
		}

		// Token: 0x06002C27 RID: 11303 RVA: 0x000AA75B File Offset: 0x000A895B
		private void OnPlayerMuted(PlayerId mutedPlayer)
		{
			if (!this._mutedPlayers.Contains(mutedPlayer))
			{
				this._mutedPlayers.Add(mutedPlayer);
				PlayerMutedDelegate onPlayerMuteChanged = this.OnPlayerMuteChanged;
				if (onPlayerMuteChanged == null)
				{
					return;
				}
				onPlayerMuteChanged(mutedPlayer, true);
			}
		}

		// Token: 0x06002C28 RID: 11304 RVA: 0x000AA789 File Offset: 0x000A8989
		private void OnPlayerUnmuted(PlayerId unmutedPlayer)
		{
			if (this._mutedPlayers.Contains(unmutedPlayer))
			{
				this._mutedPlayers.Remove(unmutedPlayer);
				PlayerMutedDelegate onPlayerMuteChanged = this.OnPlayerMuteChanged;
				if (onPlayerMuteChanged == null)
				{
					return;
				}
				onPlayerMuteChanged(unmutedPlayer, false);
			}
		}

		// Token: 0x06002C29 RID: 11305 RVA: 0x000AA7B8 File Offset: 0x000A89B8
		public bool IsPlayerMuted(PlayerId player)
		{
			return this.IsPlayerMutedFromGame(player) || this.IsPlayerMutedFromPlatform(player);
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x000AA7CC File Offset: 0x000A89CC
		public bool IsPlayerMutedFromPlatform(PlayerId player)
		{
			return this._platformMutedPlayers.Contains(player);
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x000AA7DC File Offset: 0x000A89DC
		public bool IsPlayerMutedFromGame(PlayerId player)
		{
			if (GameNetwork.IsDedicatedServer)
			{
				return this._mutedPlayers.Contains(player);
			}
			PlatformServices.Instance.CheckPrivilege(Privilege.Chat, false, delegate(bool result)
			{
				this.IsContentRestricted = !result;
			});
			return this._mutedPlayers.Contains(player) || PermaMuteList.IsPlayerMuted(player);
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x000AA82C File Offset: 0x000A8A2C
		private void ShouldShowPlayersMessage(PlayerId player, Action<bool> result)
		{
			if (this.IsPlayerMuted(player) || !NetworkMain.GameClient.SupportedFeatures.SupportsFeatures(Features.TextChat))
			{
				result(false);
				return;
			}
			PlayerIdProvidedTypes providedType = player.ProvidedType;
			LobbyClient gameClient = NetworkMain.GameClient;
			PlayerIdProvidedTypes? playerIdProvidedTypes = ((gameClient != null) ? new PlayerIdProvidedTypes?(gameClient.PlayerID.ProvidedType) : null);
			if (!((providedType == playerIdProvidedTypes.GetValueOrDefault()) & (playerIdProvidedTypes != null)))
			{
				result(true);
				return;
			}
			PlatformServices.Instance.CheckPermissionWithUser(Permission.CommunicateUsingText, player, delegate(bool res)
			{
				result(res);
			});
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x000AA8D5 File Offset: 0x000A8AD5
		public void SetChatFilterLists(string[] profanityList, string[] allowList)
		{
			this._profanityChecker = new ProfanityChecker(profanityList, allowList);
		}

		// Token: 0x06002C2E RID: 11310 RVA: 0x000AA8E4 File Offset: 0x000A8AE4
		public string CensorClientText(string text)
		{
			if (string.IsNullOrEmpty(text) || this._profanityChecker == null)
			{
				return text;
			}
			return this._profanityChecker.CensorText(text);
		}

		// Token: 0x06002C2F RID: 11311 RVA: 0x000AA904 File Offset: 0x000A8B04
		public void InitializeForMultiplayer()
		{
			PlatformServices.Instance.CheckPrivilege(Privilege.Chat, true, delegate(bool result)
			{
				this.IsContentRestricted = !result;
			});
		}

		// Token: 0x06002C30 RID: 11312 RVA: 0x000AA91E File Offset: 0x000A8B1E
		public void InitializeForSinglePlayer()
		{
			this.IsContentRestricted = false;
		}

		// Token: 0x06002C31 RID: 11313 RVA: 0x000AA927 File Offset: 0x000A8B27
		public void OnLogin()
		{
			PlatformServices.Instance.CheckPrivilege(Privilege.Chat, false, delegate(bool chatPrivilegeResult)
			{
				this.IsContentRestricted = !chatPrivilegeResult;
			});
		}

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x06002C32 RID: 11314 RVA: 0x000AA944 File Offset: 0x000A8B44
		// (remove) Token: 0x06002C33 RID: 11315 RVA: 0x000AA97C File Offset: 0x000A8B7C
		public event PlayerMessageReceivedDelegate PlayerMessageReceived;

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x06002C34 RID: 11316 RVA: 0x000AA9B4 File Offset: 0x000A8BB4
		// (remove) Token: 0x06002C35 RID: 11317 RVA: 0x000AA9EC File Offset: 0x000A8BEC
		public event WhisperMessageSentDelegate WhisperMessageSent;

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x06002C36 RID: 11318 RVA: 0x000AAA24 File Offset: 0x000A8C24
		// (remove) Token: 0x06002C37 RID: 11319 RVA: 0x000AAA5C File Offset: 0x000A8C5C
		public event WhisperMessageReceivedDelegate WhisperMessageReceived;

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x06002C38 RID: 11320 RVA: 0x000AAA94 File Offset: 0x000A8C94
		// (remove) Token: 0x06002C39 RID: 11321 RVA: 0x000AAACC File Offset: 0x000A8CCC
		public event ErrorWhisperMessageReceivedDelegate ErrorWhisperMessageReceived;

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06002C3A RID: 11322 RVA: 0x000AAB04 File Offset: 0x000A8D04
		// (remove) Token: 0x06002C3B RID: 11323 RVA: 0x000AAB3C File Offset: 0x000A8D3C
		public event ServerMessageDelegate ServerMessage;

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x06002C3C RID: 11324 RVA: 0x000AAB74 File Offset: 0x000A8D74
		// (remove) Token: 0x06002C3D RID: 11325 RVA: 0x000AABAC File Offset: 0x000A8DAC
		public event ServerAdminMessageDelegate ServerAdminMessage;

		// Token: 0x1400008C RID: 140
		// (add) Token: 0x06002C3E RID: 11326 RVA: 0x000AABE4 File Offset: 0x000A8DE4
		// (remove) Token: 0x06002C3F RID: 11327 RVA: 0x000AAC1C File Offset: 0x000A8E1C
		public event PlayerMutedDelegate OnPlayerMuteChanged;

		// Token: 0x06002C40 RID: 11328 RVA: 0x000AAC54 File Offset: 0x000A8E54
		protected override void OnTick(float dt)
		{
			if (GameNetwork.IsServer && this._isNetworkInitialized)
			{
				for (int i = ChatBox._queuedTeamMessages.Count - 1; i >= 0; i--)
				{
					ChatBox.QueuedMessageInfo queuedMessageInfo = ChatBox._queuedTeamMessages[i];
					if (queuedMessageInfo.SourcePeer.IsSynchronized)
					{
						ChatBox.ServerSendMessageToTeam(queuedMessageInfo.SourcePeer, queuedMessageInfo.Message, queuedMessageInfo.ReceiverList);
						ChatBox._queuedTeamMessages.RemoveAt(i);
					}
					else if (queuedMessageInfo.IsExpired)
					{
						ChatBox._queuedTeamMessages.RemoveAt(i);
					}
				}
				for (int j = ChatBox._queuedEveryoneMessages.Count - 1; j >= 0; j--)
				{
					ChatBox.QueuedMessageInfo queuedMessageInfo2 = ChatBox._queuedEveryoneMessages[j];
					if (queuedMessageInfo2.SourcePeer.IsSynchronized)
					{
						ChatBox.ServerSendMessageToEveryone(queuedMessageInfo2.SourcePeer, queuedMessageInfo2.Message, queuedMessageInfo2.ReceiverList);
						ChatBox._queuedEveryoneMessages.RemoveAt(j);
					}
					else if (queuedMessageInfo2.IsExpired)
					{
						ChatBox._queuedEveryoneMessages.RemoveAt(j);
					}
				}
			}
		}

		// Token: 0x0400111C RID: 4380
		private static ChatBox _chatBox;

		// Token: 0x0400111E RID: 4382
		private bool _isNetworkInitialized;

		// Token: 0x0400111F RID: 4383
		public const string AdminMessageSoundEvent = "event:/ui/notification/alert";

		// Token: 0x04001120 RID: 4384
		private List<PlayerId> _mutedPlayers = new List<PlayerId>();

		// Token: 0x04001121 RID: 4385
		private List<PlayerId> _platformMutedPlayers = new List<PlayerId>();

		// Token: 0x04001122 RID: 4386
		private ProfanityChecker _profanityChecker;

		// Token: 0x04001123 RID: 4387
		private static List<ChatBox.QueuedMessageInfo> _queuedTeamMessages;

		// Token: 0x04001124 RID: 4388
		private static List<ChatBox.QueuedMessageInfo> _queuedEveryoneMessages;

		// Token: 0x04001125 RID: 4389
		public Action<NetworkCommunicator, string> OnMessageReceivedAtDedicatedServer;

		// Token: 0x020005E0 RID: 1504
		private class QueuedMessageInfo
		{
			// Token: 0x17000AAC RID: 2732
			// (get) Token: 0x06003F77 RID: 16247 RVA: 0x000FA09C File Offset: 0x000F829C
			public bool IsExpired
			{
				get
				{
					return (DateTime.Now - this._creationTime).TotalSeconds >= 3.0;
				}
			}

			// Token: 0x06003F78 RID: 16248 RVA: 0x000FA0CF File Offset: 0x000F82CF
			public QueuedMessageInfo(NetworkCommunicator sourcePeer, string message, List<VirtualPlayer> receiverList)
			{
				this.SourcePeer = sourcePeer;
				this.Message = message;
				this._creationTime = DateTime.Now;
				this.ReceiverList = receiverList;
			}

			// Token: 0x04002007 RID: 8199
			public readonly NetworkCommunicator SourcePeer;

			// Token: 0x04002008 RID: 8200
			public readonly string Message;

			// Token: 0x04002009 RID: 8201
			public readonly List<VirtualPlayer> ReceiverList;

			// Token: 0x0400200A RID: 8202
			private const float _timeOutDuration = 3f;

			// Token: 0x0400200B RID: 8203
			private DateTime _creationTime;
		}
	}
}
