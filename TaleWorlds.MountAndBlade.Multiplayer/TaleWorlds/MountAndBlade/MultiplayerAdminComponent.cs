using System;
using System.Collections.Generic;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000018 RID: 24
	public class MultiplayerAdminComponent : MissionNetwork
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000161 RID: 353 RVA: 0x00005C84 File Offset: 0x00003E84
		// (remove) Token: 0x06000162 RID: 354 RVA: 0x00005CBC File Offset: 0x00003EBC
		public event MultiplayerAdminComponent.OnSetAdminMenuActiveStateDelegate OnSetAdminMenuActiveState;

		// Token: 0x06000163 RID: 355 RVA: 0x00005CF1 File Offset: 0x00003EF1
		public MultiplayerAdminComponent()
		{
			if (string.IsNullOrEmpty(MultiplayerIntermissionVotingManager.Instance.InitialGameType))
			{
				MultiplayerIntermissionVotingManager.Instance.InitialGameType = MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00005D1C File Offset: 0x00003F1C
		public override void OnMissionStateActivated()
		{
			base.OnMissionStateActivated();
			MultiplayerIntermissionVotingManager.Instance.IsMapVoteEnabled = !MultiplayerIntermissionVotingManager.Instance.IsDisableMapVoteOverride;
			MultiplayerIntermissionVotingManager.Instance.IsCultureVoteEnabled = !MultiplayerIntermissionVotingManager.Instance.IsDisableCultureVoteOverride;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00005D52 File Offset: 0x00003F52
		public void ChangeAdminMenuActiveState(bool isActive)
		{
			MultiplayerAdminComponent.OnSetAdminMenuActiveStateDelegate onSetAdminMenuActiveState = this.OnSetAdminMenuActiveState;
			if (onSetAdminMenuActiveState == null)
			{
				return;
			}
			onSetAdminMenuActiveState(isActive);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00005D68 File Offset: 0x00003F68
		public void KickPlayer(NetworkCommunicator peerToKick, bool banPlayer)
		{
			if (GameNetwork.IsServer)
			{
				MissionPeer component = peerToKick.GetComponent<MissionPeer>();
				if (!peerToKick.IsMine && component != null && !peerToKick.IsAdmin)
				{
					DisconnectInfo disconnectInfo = peerToKick.PlayerConnectionInfo.GetParameter<DisconnectInfo>("DisconnectInfo") ?? new DisconnectInfo();
					disconnectInfo.Type = DisconnectType.KickedByHost;
					peerToKick.PlayerConnectionInfo.AddParameter("DisconnectInfo", disconnectInfo);
					GameNetwork.AddNetworkPeerToDisconnectAsServer(peerToKick);
					if (banPlayer)
					{
						CustomGameBannedPlayerManager.AddBannedPlayer(peerToKick.VirtualPlayer.Id, int.MaxValue);
						return;
					}
				}
			}
			else if (GameNetwork.IsClient && !peerToKick.IsMine)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new KickPlayer(peerToKick, banPlayer));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00005E10 File Offset: 0x00004010
		public void GlobalMuteUnmutePlayer(NetworkCommunicator peerToMute, bool unmute)
		{
			if (GameNetwork.IsServer)
			{
				MissionPeer component = peerToMute.GetComponent<MissionPeer>();
				if (!peerToMute.IsMine && component != null && !peerToMute.IsAdmin)
				{
					PlayerId id = peerToMute.VirtualPlayer.Id;
					if (MultiplayerGlobalMutedPlayersManager.IsUserMuted(id) == unmute)
					{
						if (unmute)
						{
							MultiplayerGlobalMutedPlayersManager.UnmutePlayer(peerToMute.VirtualPlayer.Id);
						}
						else
						{
							MultiplayerGlobalMutedPlayersManager.MutePlayer(peerToMute.VirtualPlayer.Id);
						}
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new SyncPlayerMuteState(id, !unmute));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
						return;
					}
				}
			}
			else if (GameNetwork.IsClient && !peerToMute.IsMine)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new AdminMuteUnmutePlayer(peerToMute, unmute));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00005EC0 File Offset: 0x000040C0
		public void EndWarmup()
		{
			if (GameNetwork.IsServer)
			{
				if (Mission.Current != null)
				{
					MultiplayerWarmupComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerWarmupComponent>();
					if (missionBehavior != null)
					{
						missionBehavior.EndWarmupProgress();
						return;
					}
				}
			}
			else
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new AdminRequestEndWarmup());
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00005F04 File Offset: 0x00004104
		public void ChangeWelcomeMessage(string newWelcomeMessage)
		{
			if (GameNetwork.IsServer)
			{
				MultiplayerOptions.OptionType.WelcomeMessage.SetValue(newWelcomeMessage, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				this.SyncImmediateOptions();
				return;
			}
			if (MultiplayerOptions.OptionType.WelcomeMessage.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) != newWelcomeMessage)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new ChangeWelcomeMessage(newWelcomeMessage));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00005F40 File Offset: 0x00004140
		public void AdminAnnouncement(string message, bool isBroadcast)
		{
			if (GameNetwork.IsServer)
			{
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new ServerAdminMessage(message, isBroadcast));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				return;
			}
			GameNetwork.BeginModuleEventAsClient();
			GameNetwork.WriteMessage(new AdminRequestAnnouncement(message, isBroadcast));
			GameNetwork.EndModuleEventAsClient();
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00005F78 File Offset: 0x00004178
		public void ChangeClassRestriction(FormationClass classToChangeRestriction, bool newValue)
		{
			if (GameNetwork.IsServer)
			{
				this._missionLobbyComponent.ChangeClassRestriction(classToChangeRestriction, newValue);
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new ChangeClassRestrictions(classToChangeRestriction, newValue));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				return;
			}
			if (!this._missionLobbyComponent.IsClassAvailable(classToChangeRestriction) != newValue)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new AdminRequestClassRestrictionChange(classToChangeRestriction, newValue));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00005FDA File Offset: 0x000041DA
		public void AdminEndMission()
		{
			if (GameNetwork.IsServer)
			{
				this._missionLobbyComponent.SetStateEndingAsServer();
				return;
			}
			GameNetwork.BeginModuleEventAsClient();
			GameNetwork.WriteMessage(new AdminRequestEndMission());
			GameNetwork.EndModuleEventAsClient();
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00006003 File Offset: 0x00004203
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionLobbyComponent = Mission.Current.GetMissionBehavior<MissionLobbyComponent>();
			this._missionLobbyComponent.OnAdminMessageRequested += this.AdminAnnouncement;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00006034 File Offset: 0x00004234
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsServer)
			{
				registerer.RegisterBaseHandler<KickPlayer>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventKickPlayer));
				registerer.RegisterBaseHandler<ChangeWelcomeMessage>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventChangeWelcomeMessage));
				registerer.RegisterBaseHandler<AdminRequestAnnouncement>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventAdminRequestAnnouncement));
				registerer.RegisterBaseHandler<AdminRequestClassRestrictionChange>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventAdminRequestClassRestrictionChange));
				registerer.RegisterBaseHandler<AdminRequestEndMission>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventAdminRequestEndMission));
				registerer.RegisterBaseHandler<AdminUpdateMultiplayerOptions>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleAdminUpdateMultiplayerOptions));
				registerer.RegisterBaseHandler<AdminMuteUnmutePlayer>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventMuteUnmutePlayer));
				registerer.RegisterBaseHandler<AdminRequestEndWarmup>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventAdminRequestEndWarmup));
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000060DC File Offset: 0x000042DC
		private bool HandleAdminUpdateMultiplayerOptions(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			AdminUpdateMultiplayerOptions adminUpdateMultiplayerOptions = (AdminUpdateMultiplayerOptions)baseMessage;
			if (peer.IsAdmin && adminUpdateMultiplayerOptions.Options != null)
			{
				bool flag = false;
				bool flag2 = false;
				string text = MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				string text2 = null;
				string text3 = null;
				bool flag3 = false;
				bool flag4 = false;
				for (int i = 0; i < adminUpdateMultiplayerOptions.Options.Count; i++)
				{
					AdminUpdateMultiplayerOptions.AdminMultiplayerOptionInfo adminMultiplayerOptionInfo = adminUpdateMultiplayerOptions.Options[i];
					bool flag5 = true;
					if (adminMultiplayerOptionInfo.OptionType == MultiplayerOptions.OptionType.GameType)
					{
						flag5 = !string.IsNullOrEmpty(adminMultiplayerOptionInfo.StringValue);
						if (adminMultiplayerOptionInfo.StringValue == MultiplayerIntermissionVotingManager.Instance.InitialGameType || (!flag5 && MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) == MultiplayerIntermissionVotingManager.Instance.InitialGameType))
						{
							flag = true;
						}
						if (flag5)
						{
							text = adminMultiplayerOptionInfo.StringValue;
						}
					}
					if (adminMultiplayerOptionInfo.OptionType == MultiplayerOptions.OptionType.Map)
					{
						flag5 = !string.IsNullOrEmpty(adminMultiplayerOptionInfo.StringValue);
						flag2 = !flag5;
					}
					if (adminMultiplayerOptionInfo.OptionType == MultiplayerOptions.OptionType.CultureTeam1)
					{
						flag3 = !string.IsNullOrEmpty(adminMultiplayerOptionInfo.StringValue);
						text2 = (flag3 ? adminMultiplayerOptionInfo.StringValue : null);
					}
					else if (adminMultiplayerOptionInfo.OptionType == MultiplayerOptions.OptionType.CultureTeam2)
					{
						flag4 = !string.IsNullOrEmpty(adminMultiplayerOptionInfo.StringValue);
						text3 = (flag4 ? adminMultiplayerOptionInfo.StringValue : null);
					}
					else if (flag5)
					{
						switch (adminMultiplayerOptionInfo.OptionType.GetOptionProperty().OptionValueType)
						{
						case MultiplayerOptions.OptionValueType.Bool:
							adminMultiplayerOptionInfo.OptionType.SetValue(adminMultiplayerOptionInfo.BoolValue, adminMultiplayerOptionInfo.AccessMode);
							break;
						case MultiplayerOptions.OptionValueType.Integer:
						case MultiplayerOptions.OptionValueType.Enum:
							adminMultiplayerOptionInfo.OptionType.SetValue(adminMultiplayerOptionInfo.IntValue, adminMultiplayerOptionInfo.AccessMode);
							break;
						case MultiplayerOptions.OptionValueType.String:
							adminMultiplayerOptionInfo.OptionType.SetValue(adminMultiplayerOptionInfo.StringValue, adminMultiplayerOptionInfo.AccessMode);
							break;
						}
					}
				}
				if (flag2)
				{
					MultiplayerIntermissionVotingManager.Instance.IsMapSelectedByAdmin = false;
					if (flag)
					{
						if (MultiplayerIntermissionVotingManager.Instance.IsDisableMapVoteOverride)
						{
							MultiplayerIntermissionVotingManager.Instance.IsMapVoteEnabled = false;
							string id = MultiplayerIntermissionVotingManager.Instance.MapVoteItems.GetRandomElement<IntermissionVoteItem>().Id;
							MultiplayerOptions.OptionType.Map.SetValue(id, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
							Debug.Print("[Admin] game type was default and map was undecided. Voting was disabled. Selected map randomly from automated map pool: " + id, 0, Debug.DebugColor.White, 17592186044416UL);
						}
						else
						{
							MultiplayerIntermissionVotingManager.Instance.IsMapVoteEnabled = true;
							Debug.Print("[Admin] game type was default and map was undecided. Maps will be voted from automated map pool", 0, Debug.DebugColor.White, 17592186044416UL);
						}
					}
					else
					{
						MultiplayerIntermissionVotingManager.Instance.IsMapVoteEnabled = false;
						string randomElement = MultiplayerIntermissionVotingManager.Instance.GetUsableMaps(text).GetRandomElement<string>();
						MultiplayerOptions.OptionType.Map.SetValue(randomElement, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
						Debug.Print("[Admin] game type wasn't default and map was undecided. Selected map randomly from usable maps: " + randomElement + ".", 0, Debug.DebugColor.White, 17592186044416UL);
					}
				}
				else
				{
					MultiplayerIntermissionVotingManager.Instance.IsMapVoteEnabled = false;
					MultiplayerIntermissionVotingManager.Instance.IsMapSelectedByAdmin = true;
					Debug.Print("[Admin] next game type: " + text + " next map: " + MultiplayerOptions.OptionType.Map.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				if (flag3 && flag4)
				{
					MultiplayerIntermissionVotingManager.Instance.IsCultureVoteEnabled = false;
					MultiplayerOptions.OptionType.CultureTeam1.SetValue(text2, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
					MultiplayerOptions.OptionType.CultureTeam2.SetValue(text3, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
					Debug.Print(string.Concat(new string[] { "[Admin] Both cultures were valid. Setting ", text2, " vs ", text3, " for next game." }), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				else if (MultiplayerIntermissionVotingManager.Instance.IsDisableCultureVoteOverride)
				{
					MultiplayerIntermissionVotingManager.Instance.IsCultureVoteEnabled = false;
					MultiplayerIntermissionVotingManager.Instance.SelectRandomCultures(MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
					string strValue = MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
					string strValue2 = MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
					Debug.Print(string.Concat(new string[] { "[Admin] Cultures weren't valid. Randomly setting ", strValue, " vs ", strValue2, " for next game." }), 0, Debug.DebugColor.White, 17592186044416UL);
				}
				else
				{
					MultiplayerIntermissionVotingManager.Instance.IsCultureVoteEnabled = true;
					Debug.Print("[Admin] Cultures weren't valid. Culture voting is enabled", 0, Debug.DebugColor.White, 17592186044416UL);
				}
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new MultiplayerOptionsImmediate());
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				GameNetwork.BeginModuleEventAsServer(peer);
				GameNetwork.WriteMessage(new UpdateIntermissionVotingManagerValues());
				GameNetwork.EndModuleEventAsServer();
			}
			return true;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x00006500 File Offset: 0x00004700
		private bool HandleClientEventKickPlayer(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			KickPlayer kickPlayer = (KickPlayer)baseMessage;
			if (peer.IsAdmin)
			{
				this.KickPlayer(kickPlayer.PlayerPeer, kickPlayer.BanPlayer);
			}
			return true;
		}

		// Token: 0x06000171 RID: 369 RVA: 0x00006530 File Offset: 0x00004730
		private bool HandleClientEventMuteUnmutePlayer(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			AdminMuteUnmutePlayer adminMuteUnmutePlayer = (AdminMuteUnmutePlayer)baseMessage;
			if (peer.IsAdmin)
			{
				this.GlobalMuteUnmutePlayer(adminMuteUnmutePlayer.PlayerPeer, adminMuteUnmutePlayer.Unmute);
			}
			return true;
		}

		// Token: 0x06000172 RID: 370 RVA: 0x00006560 File Offset: 0x00004760
		private bool HandleClientEventChangeWelcomeMessage(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			ChangeWelcomeMessage changeWelcomeMessage = (ChangeWelcomeMessage)baseMessage;
			if (peer.IsAdmin)
			{
				this.ChangeWelcomeMessage(changeWelcomeMessage.NewWelcomeMessage);
			}
			return true;
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000658C File Offset: 0x0000478C
		private bool HandleClientEventAdminRequestClassRestrictionChange(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			AdminRequestClassRestrictionChange adminRequestClassRestrictionChange = (AdminRequestClassRestrictionChange)baseMessage;
			if (peer.IsAdmin)
			{
				this.ChangeClassRestriction(adminRequestClassRestrictionChange.ClassToChangeRestriction, adminRequestClassRestrictionChange.NewValue);
			}
			return true;
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000065BC File Offset: 0x000047BC
		private bool HandleClientEventAdminRequestAnnouncement(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			AdminRequestAnnouncement adminRequestAnnouncement = (AdminRequestAnnouncement)baseMessage;
			if (peer.IsAdmin)
			{
				this.AdminAnnouncement(adminRequestAnnouncement.Message, adminRequestAnnouncement.IsAdminBroadcast);
			}
			return true;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x000065EB File Offset: 0x000047EB
		private bool HandleClientEventAdminRequestEndMission(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			AdminRequestEndMission adminRequestEndMission = (AdminRequestEndMission)baseMessage;
			if (peer.IsAdmin)
			{
				this.AdminEndMission();
			}
			return true;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00006604 File Offset: 0x00004804
		[CommandLineFunctionality.CommandLineArgumentFunction("announcement", "mp_admin")]
		public static string MPAdminAnnouncement(List<string> strings)
		{
			if (strings.Count == 0)
			{
				return "Wrong format! Usage: mp_admin.announcement {TEXT}";
			}
			if (Mission.Current == null)
			{
				return "Mission is not running!";
			}
			MultiplayerAdminComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerAdminComponent>();
			if (missionBehavior == null)
			{
				return "Admin component could not be found!";
			}
			missionBehavior.AdminAnnouncement(string.Join(" ", strings), true);
			return "Success";
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00006657 File Offset: 0x00004857
		private bool HandleClientEventAdminRequestEndWarmup(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			AdminRequestEndWarmup adminRequestEndWarmup = (AdminRequestEndWarmup)baseMessage;
			if (peer.IsAdmin)
			{
				this.EndWarmup();
			}
			return true;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000666F File Offset: 0x0000486F
		public override void OnRemoveBehavior()
		{
			MultiplayerAdminComponent._multiplayerAdminComponent = null;
			base.OnRemoveBehavior();
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000667D File Offset: 0x0000487D
		private void SyncImmediateOptions()
		{
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new MultiplayerOptionsImmediate());
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00006698 File Offset: 0x00004898
		[CommandLineFunctionality.CommandLineArgumentFunction("kick_player", "mp_admin")]
		public static string MPAdminKickPlayer(List<string> strings)
		{
			if (MultiplayerAdminComponent._multiplayerAdminComponent == null)
			{
				return "Failed: MultiplayerAdminComponent has not been created.";
			}
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			if (myPeer == null || !myPeer.IsAdmin)
			{
				return "Failed: Only admins can use mp_admin commands.";
			}
			if (strings.Count != 1)
			{
				return "Failed: Input is incorrect.";
			}
			string text = strings[0];
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (networkCommunicator.UserName == text)
				{
					MultiplayerAdminComponent._multiplayerAdminComponent.KickPlayer(networkCommunicator, false);
					return "Player " + text + " has been kicked from the server.";
				}
			}
			return text + " could not be found.";
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00006760 File Offset: 0x00004960
		[CommandLineFunctionality.CommandLineArgumentFunction("ban_player", "mp_admin")]
		public static string MPAdminBanPlayer(List<string> strings)
		{
			if (MultiplayerAdminComponent._multiplayerAdminComponent == null)
			{
				return "Failed: MultiplayerAdminComponent has not been created.";
			}
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			if (myPeer == null || !myPeer.IsAdmin)
			{
				return "Failed: Only admins can use mp_admin commands.";
			}
			if (strings.Count != 1)
			{
				return "Failed: Input is incorrect.";
			}
			string text = strings[0];
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (networkCommunicator.UserName == text)
				{
					MultiplayerAdminComponent._multiplayerAdminComponent.KickPlayer(networkCommunicator, true);
					return "Player " + text + " has been banned from the server.";
				}
			}
			return text + " could not be found.";
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00006828 File Offset: 0x00004A28
		[CommandLineFunctionality.CommandLineArgumentFunction("change_welcome_message", "mp_admin")]
		public static string MPAdminChangeWelcomeMessage(List<string> strings)
		{
			if (MultiplayerAdminComponent._multiplayerAdminComponent == null)
			{
				return "Failed: MultiplayerAdminComponent has not been created.";
			}
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			if (myPeer == null || !myPeer.IsAdmin)
			{
				return "Failed: Only admins can use mp_host commands.";
			}
			string text = "";
			foreach (string text2 in strings)
			{
				text = text + text2 + " ";
			}
			MultiplayerAdminComponent._multiplayerAdminComponent.ChangeWelcomeMessage(text);
			return "Changed welcome message to: " + text;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x000068C4 File Offset: 0x00004AC4
		[CommandLineFunctionality.CommandLineArgumentFunction("change_class_restriction", "mp_admin")]
		public static string MPAdminChangeClassRestriction(List<string> strings)
		{
			FormationClass formationClass;
			bool flag;
			if (strings.Count != 2 || !Enum.TryParse<FormationClass>(strings[0], out formationClass) || !bool.TryParse(strings[1], out flag))
			{
				return "Wrong format! Usage: mp_admin.change_class_restriction {FormationClass} {true/false}";
			}
			if (Mission.Current == null)
			{
				return "Mission is not running!";
			}
			MultiplayerAdminComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerAdminComponent>();
			if (missionBehavior == null)
			{
				return "Admin component could not be found!";
			}
			missionBehavior.ChangeClassRestriction(formationClass, flag);
			return "Success";
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00006930 File Offset: 0x00004B30
		[CommandLineFunctionality.CommandLineArgumentFunction("restart_game", "mp_admin")]
		public static string MPHostRestartGame(List<string> strings)
		{
			if (Mission.Current == null)
			{
				return "Mission is not running!";
			}
			MultiplayerAdminComponent missionBehavior = Mission.Current.GetMissionBehavior<MultiplayerAdminComponent>();
			if (missionBehavior == null)
			{
				return "Admin component could not be found!";
			}
			missionBehavior.AdminEndMission();
			return "Success";
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000696C File Offset: 0x00004B6C
		[CommandLineFunctionality.CommandLineArgumentFunction("change_server_slots", "mp_admin")]
		public static string MPAdminChangeServerSlots(List<string> strings)
		{
			if (Mission.Current == null)
			{
				return "Mission is not running!";
			}
			if (Mission.Current.GetMissionBehavior<MultiplayerAdminComponent>() == null)
			{
				return "Admin component could not be found!";
			}
			if (strings.Count != 1)
			{
				return "Wrong format! Usage: mp_admin.change_server_slots {NUMBER}";
			}
			int num;
			if (int.TryParse(strings[0], out num))
			{
				MultiplayerOptions.OptionType.MaxNumberOfPlayers.SetValue(num, MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions);
				return "Success";
			}
			return "Wrong format! Usage: mp_admin.change_server_slots {NUMBER}";
		}

		// Token: 0x04000040 RID: 64
		private MissionLobbyComponent _missionLobbyComponent;

		// Token: 0x04000041 RID: 65
		private static MultiplayerAdminComponent _multiplayerAdminComponent;

		// Token: 0x0200009C RID: 156
		// (Invoke) Token: 0x0600041C RID: 1052
		public delegate void OnSelectPlayerToKickDelegate(bool banPlayer);

		// Token: 0x0200009D RID: 157
		// (Invoke) Token: 0x06000420 RID: 1056
		public delegate void OnSetAdminMenuActiveStateDelegate(bool showMenu);
	}
}
