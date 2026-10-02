using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Multiplayer
{
	// Token: 0x0200003B RID: 59
	public class MPChatVM : ViewModel, IChatHandler
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x0001319A File Offset: 0x0001139A
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x000131A4 File Offset: 0x000113A4
		public ChatChannelType ActiveChannelType
		{
			get
			{
				return this._activeChannelType;
			}
			set
			{
				if ((value == ChatChannelType.All || value == ChatChannelType.Team) && !GameNetwork.IsClient && NetworkMain.GameClient == null && NetworkMain.CommunityClient == null)
				{
					this._activeChannelType = ChatChannelType.None;
				}
				else
				{
					if (value == ChatChannelType.All && MPChatVM.IsLocalPeerSpectator())
					{
						value = ChatChannelType.Team;
					}
					if (value != this._activeChannelType)
					{
						this._activeChannelType = value;
						this.RefreshActiveChannelNameData();
					}
				}
				this.IsChatDisabled = value == ChatChannelType.None;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00013208 File Offset: 0x00011408
		private string _playerName
		{
			get
			{
				string text = ((NetworkMain.GameClient.PlayerData != null) ? NetworkMain.GameClient.Name : new TextObject("{=!}ERROR: MISSING PLAYERDATA", null).ToString());
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
				if (missionPeer != null && !missionPeer.IsAgentAliveForChatting)
				{
					GameTexts.SetVariable("PLAYER_NAME", "{=!}" + text);
					text = GameTexts.FindText("str_chat_message_dead_player", null).ToString();
				}
				return text;
			}
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00013288 File Offset: 0x00011488
		public MPChatVM()
		{
			this._allMessages = new List<MPChatLineVM>();
			this._requestedMessages = new Queue<MPChatLineVM>();
			this.MessageHistory = new MBBindingList<MPChatLineVM>();
			this.CombatLogHint = new HintViewModel();
			this.IncludeCombatLog = BannerlordConfig.ReportDamage;
			this.IncludeBark = BannerlordConfig.ReportBark;
			InformationManager.DisplayMessageInternal += this.OnDisplayMessageReceived;
			InformationManager.ClearAllMessagesInternal += this.ClearAllMessages;
			InformationManager.HideAllMessagesInternal += this.HideAllMessages;
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnOptionChange));
			this.MaxMessageLength = 100;
			this._recentlySentMessagesTimes = new List<float>();
			this.RefreshValues();
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x000133A8 File Offset: 0x000115A8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.CombatLogHint.HintText = new TextObject("{=FRSGOfUJ}Toggle include Combat Log", null);
			this.ToggleCombatLogText = new TextObject("{=rx18kyZb}Combat Log", null).ToString();
			this.ToggleBarkText = new TextObject("{=NuMQvQxg}Shouts", null).ToString();
			this.UpdateHideShowText(this._isInspectingMessages);
			this.UpdateShortcutTexts();
			this.RefreshActiveChannelNameData();
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00013415 File Offset: 0x00011615
		private static bool IsLocalPeerSpectator()
		{
			return SpectatorHelper.IsLocalPeerSpectator();
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001341C File Offset: 0x0001161C
		private void RefreshActiveChannelNameData()
		{
			if (this.ActiveChannelType == ChatChannelType.None)
			{
				this.ActiveChannelNameText = string.Empty;
				this.ActiveChannelColor = Color.White;
				return;
			}
			string text = ((this.ActiveChannelType == ChatChannelType.Team && MPChatVM.IsLocalPeerSpectator()) ? "Spectator" : this.ActiveChannelType.ToString());
			string text2 = GameTexts.FindText("str_multiplayer_chat_channel", text).ToString();
			GameTexts.SetVariable("STR", text2);
			this.ActiveChannelNameText = GameTexts.FindText("str_STR_in_parentheses", null).ToString();
			this.ActiveChannelColor = this.GetChannelColor(this.ActiveChannelType);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x000134B9 File Offset: 0x000116B9
		private void OnOptionChange(ManagedOptions.ManagedOptionsType changedManagedOptionsType)
		{
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.ReportDamage)
			{
				this.IncludeCombatLog = BannerlordConfig.ReportDamage;
				return;
			}
			if (changedManagedOptionsType == ManagedOptions.ManagedOptionsType.ReportBark)
			{
				this.IncludeBark = BannerlordConfig.ReportBark;
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x000134DC File Offset: 0x000116DC
		public void ToggleIncludeCombatLog()
		{
			this.IncludeCombatLog = !this.IncludeCombatLog;
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x000134ED File Offset: 0x000116ED
		public void ExecuteToggleIncludeShouts()
		{
			this.IncludeBark = !this.IncludeBark;
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00013500 File Offset: 0x00011700
		private void UpdateHideShowText(bool isInspecting)
		{
			TextObject textObject;
			if (this._game != null && isInspecting)
			{
				textObject = this._hideText;
				textObject.SetTextVariable("KEY", this._getToggleChatKeyText() ?? TextObject.GetEmpty());
			}
			else
			{
				textObject = TextObject.GetEmpty();
			}
			this.HideShowText = textObject.ToString();
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x00013558 File Offset: 0x00011758
		private void UpdateShortcutTexts()
		{
			TextObject cycleChannelsText = this._cycleChannelsText;
			string text = "KEY";
			Func<TextObject> getCycleChannelsKeyText = this._getCycleChannelsKeyText;
			cycleChannelsText.SetTextVariable(text, ((getCycleChannelsKeyText != null) ? getCycleChannelsKeyText() : null) ?? TextObject.GetEmpty());
			this.CycleThroughChannelsText = this._cycleChannelsText.ToString();
			if (Input.IsGamepadActive)
			{
				TextObject sendMessageTextObject = this._sendMessageTextObject;
				string text2 = "KEY";
				Func<TextObject> getSendMessageKeyText = this._getSendMessageKeyText;
				sendMessageTextObject.SetTextVariable(text2, ((getSendMessageKeyText != null) ? getSendMessageKeyText() : null) ?? TextObject.GetEmpty());
				this.SendMessageText = this._sendMessageTextObject.ToString();
				TextObject cancelSendingTextObject = this._cancelSendingTextObject;
				string text3 = "KEY";
				Func<TextObject> getCancelSendingKeyText = this._getCancelSendingKeyText;
				cancelSendingTextObject.SetTextVariable(text3, ((getCancelSendingKeyText != null) ? getCancelSendingKeyText() : null) ?? TextObject.GetEmpty());
				this.CancelSendingText = this._cancelSendingTextObject.ToString();
				return;
			}
			this.SendMessageText = string.Empty;
			this.CancelSendingText = string.Empty;
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001363C File Offset: 0x0001183C
		public void Tick(float dt)
		{
			while (this._requestedMessages.Count > 0)
			{
				this.AddChatLine(this._requestedMessages.Dequeue());
			}
			float applicationTime = Time.ApplicationTime;
			for (int i = 0; i < this._recentlySentMessagesTimes.Count; i++)
			{
				if (applicationTime - this._recentlySentMessagesTimes[i] >= 15f)
				{
					this._recentlySentMessagesTimes.RemoveAt(i);
				}
			}
			this.CheckChatFading(dt);
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000136B0 File Offset: 0x000118B0
		public void Hide()
		{
			this._allMessages.ForEach(delegate(MPChatLineVM l)
			{
				l.ForceInvisible();
			});
			this.MessageHistory.ToList<MPChatLineVM>().ForEach(delegate(MPChatLineVM l)
			{
				l.ForceInvisible();
			});
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x00013718 File Offset: 0x00011918
		public void Clear()
		{
			this._allMessages.ForEach(delegate(MPChatLineVM l)
			{
				l.ForceInvisible();
			});
			this.MessageHistory.ToList<MPChatLineVM>().ForEach(delegate(MPChatLineVM l)
			{
				l.ForceInvisible();
			});
			this._allMessages.Clear();
			this.MessageHistory.Clear();
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00013794 File Offset: 0x00011994
		private void OnDisplayMessageReceived(InformationMessage informationMessage)
		{
			if (this.IsChatAllowedByOptions())
			{
				this.HandleAddChatLineRequest(informationMessage);
			}
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000137A5 File Offset: 0x000119A5
		private void ClearAllMessages()
		{
			this.Clear();
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x000137AD File Offset: 0x000119AD
		private void HideAllMessages()
		{
			this.Hide();
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x000137B8 File Offset: 0x000119B8
		public void UpdateObjects(Game game, Mission mission)
		{
			if (this._game != game)
			{
				if (this._game != null)
				{
					this.ClearGame();
				}
				this._game = game;
				if (this._game != null)
				{
					this.SetGame();
				}
			}
			if (this._mission != mission)
			{
				if (this._mission != null)
				{
					this.ClearMission();
				}
				this._mission = mission;
				if (this._mission != null)
				{
					this.SetMission();
				}
			}
			if (this._game != null)
			{
				ChatBox gameHandler = this._game.GetGameHandler<ChatBox>();
				if (this._chatBox != gameHandler)
				{
					if (this._chatBox != null)
					{
						this.ClearChatBox();
					}
					this._chatBox = gameHandler;
					if (this._chatBox != null)
					{
						this.SetChatBox();
					}
				}
			}
			this.IsOptionsAvailable = this.IsInspectingMessages && this.IsTypingText;
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x00013874 File Offset: 0x00011A74
		private void ClearGame()
		{
			this._game = null;
			this.ActiveChannelType = ChatChannelType.None;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00013884 File Offset: 0x00011A84
		private void ClearChatBox()
		{
			if (this._chatBox != null)
			{
				this._chatBox.PlayerMessageReceived -= this.OnPlayerMessageReceived;
				this._chatBox.WhisperMessageSent -= this.OnWhisperMessageSent;
				this._chatBox.WhisperMessageReceived -= this.OnWhisperMessageReceived;
				this._chatBox.ErrorWhisperMessageReceived -= this.OnErrorWhisperMessageReceived;
				this._chatBox.ServerMessage -= this.OnServerMessage;
				this._chatBox.ServerAdminMessage -= this.OnServerAdminMessage;
				this._chatBox = null;
				this.ActiveChannelType = ChatChannelType.None;
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00013934 File Offset: 0x00011B34
		private void SetGame()
		{
			this.UpdateHideShowText(this.IsInspectingMessages);
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00013944 File Offset: 0x00011B44
		private void SetChatBox()
		{
			this._chatBox.PlayerMessageReceived += this.OnPlayerMessageReceived;
			this._chatBox.WhisperMessageSent += this.OnWhisperMessageSent;
			this._chatBox.WhisperMessageReceived += this.OnWhisperMessageReceived;
			this._chatBox.ErrorWhisperMessageReceived += this.OnErrorWhisperMessageReceived;
			this._chatBox.ServerMessage += this.OnServerMessage;
			this._chatBox.ServerAdminMessage += this.OnServerAdminMessage;
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x000139DC File Offset: 0x00011BDC
		private void SetMission()
		{
			this.ActiveChannelType = ChatChannelType.All;
			Game game = Game.Current;
			bool flag;
			if (game == null)
			{
				flag = false;
			}
			else
			{
				ChatBox gameHandler = game.GetGameHandler<ChatBox>();
				bool? flag2 = ((gameHandler != null) ? new bool?(gameHandler.IsContentRestricted) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			this.IsChatDisabled = flag;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00013A35 File Offset: 0x00011C35
		private void ClearMission()
		{
			this._mission = null;
			this.ActiveChannelType = ChatChannelType.None;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00013A45 File Offset: 0x00011C45
		public override void OnFinalize()
		{
			base.OnFinalize();
			if (this._game != null)
			{
				this.ClearGame();
			}
			if (this._mission != null)
			{
				this.ClearMission();
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00013A6C File Offset: 0x00011C6C
		private void ExecuteSendMessage()
		{
			string text = this.WrittenText;
			if (string.IsNullOrEmpty(text))
			{
				this.WrittenText = string.Empty;
				return;
			}
			if (text.Length > this.MaxMessageLength)
			{
				text = this.WrittenText.Substring(0, this.MaxMessageLength);
			}
			text = Regex.Replace(text.Trim(), "\\s+", " ");
			if (text.StartsWith("/"))
			{
				string[] array = text.Split(new char[] { ' ' });
				ChatChannelType chatChannelType = ChatChannelType.None;
				LobbyClient gameClient = NetworkMain.GameClient;
				if (gameClient != null && gameClient.Connected)
				{
					string text2 = array[0].ToLower();
					if (!(text2 == "/all") && !(text2 == "/a"))
					{
						if (!(text2 == "/team") && !(text2 == "/t"))
						{
							if (!(text2 == "/ab"))
							{
								if (text2 == "/ac")
								{
									if (Mission.Current != null)
									{
										MissionLobbyComponent missionBehavior = Mission.Current.GetMissionBehavior<MissionLobbyComponent>();
										if (missionBehavior != null)
										{
											missionBehavior.RequestAdminMessage(string.Join(" ", array.Skip<string>(1)), false);
										}
									}
								}
							}
							else if (Mission.Current != null)
							{
								MissionLobbyComponent missionBehavior2 = Mission.Current.GetMissionBehavior<MissionLobbyComponent>();
								if (missionBehavior2 != null)
								{
									missionBehavior2.RequestAdminMessage(string.Join(" ", array.Skip<string>(1)), true);
								}
							}
						}
						else
						{
							chatChannelType = ChatChannelType.Team;
						}
					}
					else
					{
						chatChannelType = ChatChannelType.All;
					}
				}
				this.ActiveChannelType = chatChannelType;
			}
			else
			{
				ChatChannelType activeChannelType = this.ActiveChannelType;
				if (activeChannelType != ChatChannelType.Private)
				{
					if (activeChannelType - ChatChannelType.All <= 2)
					{
						this.CheckSpamAndSendMessage(this.ActiveChannelType, text);
					}
					else
					{
						Debug.FailedAssert("Player in invalid channel", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Multiplayer\\MPChatVM.cs", "ExecuteSendMessage", 514);
					}
				}
			}
			this.WrittenText = "";
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00013C14 File Offset: 0x00011E14
		private void CheckSpamAndSendMessage(ChatChannelType channelType, string textToSend)
		{
			if (this._recentlySentMessagesTimes.Count >= 5)
			{
				GameTexts.SetVariable("SECONDS", (15f - (Time.ApplicationTime - this._recentlySentMessagesTimes[0])).ToString("0.0"));
				this.AddChatLine(new MPChatLineVM(new TextObject("{=76VR5o8h}You must wait {SECONDS} seconds before sending another message.", null).ToString(), this.GetChannelColor(ChatChannelType.System), "Default"));
				return;
			}
			this._recentlySentMessagesTimes.Add(Time.ApplicationTime);
			this.SendMessageToChannel(this.ActiveChannelType, textToSend);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00013CA4 File Offset: 0x00011EA4
		private void HandleAddChatLineRequest(InformationMessage informationMessage)
		{
			string information = informationMessage.Information;
			string text = (string.IsNullOrEmpty(informationMessage.Category) ? "Default" : informationMessage.Category);
			Color color = informationMessage.Color;
			MPChatLineVM mpchatLineVM = new MPChatLineVM(information, color, text);
			this._requestedMessages.Enqueue(mpchatLineVM);
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00013CF0 File Offset: 0x00011EF0
		public void SendMessageToChannel(ChatChannelType channel, string message)
		{
			LobbyClient gameClient = NetworkMain.GameClient;
			if (gameClient != null && gameClient.Connected)
			{
				if (channel == ChatChannelType.All && MPChatVM.IsLocalPeerSpectator())
				{
					channel = ChatChannelType.Team;
				}
				switch (channel)
				{
				case ChatChannelType.All:
					this._chatBox.SendMessageToAll(message);
					return;
				case ChatChannelType.Team:
					this._chatBox.SendMessageToTeam(message);
					return;
				}
				throw new NotImplementedException();
			}
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00013D54 File Offset: 0x00011F54
		void IChatHandler.ReceiveChatMessage(ChatChannelType channel, string sender, string message)
		{
			TextObject textObject;
			if (channel == ChatChannelType.Private)
			{
				textObject = new TextObject("{=6syoutpV}From {WHISPER_TARGET}", null);
				textObject.SetTextVariable("WHISPER_TARGET", sender);
			}
			else
			{
				textObject = TextObject.GetEmpty();
			}
			this.AddMessage(message, sender, channel, textObject);
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00013D90 File Offset: 0x00011F90
		private void AddMessage(string msg, string author, ChatChannelType type, TextObject customChannelName = null)
		{
			Color channelColor = this.GetChannelColor(type);
			string text = ((!TextObject.IsNullOrEmpty(customChannelName)) ? customChannelName.ToString() : type.ToString());
			MPChatLineVM mpchatLineVM = new MPChatLineVM(string.Concat(new string[] { "(", text, ") ", author, ": ", msg }), channelColor, "Social");
			this.AddChatLine(mpchatLineVM);
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x00013E08 File Offset: 0x00012008
		private void AddChatLine(MPChatLineVM chatLine)
		{
			if (NativeConfig.DisableGuiMessages || chatLine == null)
			{
				return;
			}
			this._allMessages.Add(chatLine);
			int num = this._maxHistoryCount * 5;
			if (this._allMessages.Count > num)
			{
				this._allMessages.RemoveAt(0);
			}
			if (this.IsMessageIncluded(chatLine))
			{
				this.MessageHistory.Add(chatLine);
				if (this.MessageHistory.Count > this._maxHistoryCount)
				{
					this.MessageHistory.RemoveAt(0);
				}
			}
			this.RefreshVisibility();
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00013E8C File Offset: 0x0001208C
		public void CheckChatFading(float dt)
		{
			foreach (MPChatLineVM mpchatLineVM in this._allMessages)
			{
				mpchatLineVM.HandleFading(dt);
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00013EE0 File Offset: 0x000120E0
		private void ChatHistoryFilterToggled()
		{
			this.MessageHistory.Clear();
			int num = 0;
			while (num < this._allMessages.Count && this.MessageHistory.Count < this._maxHistoryCount)
			{
				MPChatLineVM mpchatLineVM = this._allMessages[num];
				if (this.IsMessageIncluded(mpchatLineVM))
				{
					this.MessageHistory.Add(mpchatLineVM);
				}
				num++;
			}
			this.RefreshVisibility();
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00013F49 File Offset: 0x00012149
		private bool IsMessageIncluded(MPChatLineVM chatLine)
		{
			if (chatLine.Category == "Combat")
			{
				return this.IncludeCombatLog;
			}
			return !(chatLine.Category == "Bark") || this.IncludeBark;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00013F7E File Offset: 0x0001217E
		public void SetChatDisabledStateChangedCallback(Action<bool> onChatDisabledStateChanged)
		{
			this._onChatDisabledStateChanged = onChatDisabledStateChanged;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00013F87 File Offset: 0x00012187
		public void SetGetKeyTextFromKeyIDFunc(Func<TextObject> getToggleChatKeyText)
		{
			this._getToggleChatKeyText = getToggleChatKeyText;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00013F90 File Offset: 0x00012190
		public void SetGetCycleChannelKeyTextFunc(Func<TextObject> getCycleChannelsKeyText)
		{
			this._getCycleChannelsKeyText = getCycleChannelsKeyText;
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x00013F99 File Offset: 0x00012199
		public void SetGetSendMessageKeyTextFunc(Func<TextObject> getSendMessageKeyText)
		{
			this._getSendMessageKeyText = getSendMessageKeyText;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00013FA2 File Offset: 0x000121A2
		public void SetGetCancelSendingKeyTextFunc(Func<TextObject> getCancelSendingKeyText)
		{
			this._getCancelSendingKeyText = getCancelSendingKeyText;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00013FAC File Offset: 0x000121AC
		private void OnPlayerMessageReceived(NetworkCommunicator player, string message, bool toTeamOnly)
		{
			MissionPeer component = player.GetComponent<MissionPeer>();
			string text = ((component != null) ? component.DisplayedName : null) ?? player.UserName;
			if (component != null && !component.IsAgentAliveForChatting)
			{
				GameTexts.SetVariable("PLAYER_NAME", text);
				text = GameTexts.FindText("str_chat_message_dead_player", null).ToString();
			}
			this.AddMessage(message, text, toTeamOnly ? ChatChannelType.Team : ChatChannelType.All, null);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x00014014 File Offset: 0x00012214
		private void OnWhisperMessageReceived(string fromUserName, string message)
		{
			this.AddMessage(message, fromUserName, ChatChannelType.Private, null);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x00014020 File Offset: 0x00012220
		private void OnErrorWhisperMessageReceived(string toUserName)
		{
			TextObject textObject = new TextObject("{=61isYVW0}Player {USER_NAME} is not found", null);
			textObject.SetTextVariable("USER_NAME", toUserName);
			MPChatLineVM mpchatLineVM = new MPChatLineVM(textObject.ToString(), Color.White, "Social");
			this.AddChatLine(mpchatLineVM);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x00014061 File Offset: 0x00012261
		private void OnWhisperMessageSent(string message, string whisperTarget)
		{
			this.AddMessage(message, whisperTarget, ChatChannelType.Private, null);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00014070 File Offset: 0x00012270
		private void OnServerMessage(string message)
		{
			MPChatLineVM mpchatLineVM = new MPChatLineVM(message, Color.White, "Social");
			this.AddChatLine(mpchatLineVM);
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00014098 File Offset: 0x00012298
		private void OnServerAdminMessage(string message)
		{
			MPChatLineVM mpchatLineVM = new MPChatLineVM("[Admin]: " + message, Color.ConvertStringToColor("#CC0099FF"), "Social");
			this.AddChatLine(mpchatLineVM);
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x000140CC File Offset: 0x000122CC
		private Color GetChannelColor(ChatChannelType type)
		{
			string text;
			switch (type)
			{
			case ChatChannelType.Private:
				text = "#8C1ABDFF";
				break;
			case ChatChannelType.All:
				text = "#EC943EFF";
				break;
			case ChatChannelType.Team:
				text = "#05C5F7FF";
				break;
			case ChatChannelType.Party:
				text = "#05C587FF";
				break;
			case ChatChannelType.System:
				text = "#FF0000FF";
				break;
			case ChatChannelType.Custom:
				text = "#FF0000FF";
				break;
			default:
				text = "#FFFFFFFF";
				break;
			}
			return Color.ConvertStringToColor(text);
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x00014135 File Offset: 0x00012335
		public bool IsChatAllowedByOptions()
		{
			if (GameNetwork.IsMultiplayer)
			{
				return BannerlordConfig.EnableMultiplayerChatBox;
			}
			return BannerlordConfig.EnableSingleplayerChatBox && (Mission.Current == null || !BannerlordConfig.HideBattleUI);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001415E File Offset: 0x0001235E
		public void TypeToChannelAll(bool startTyping = false)
		{
			this.ActiveChannelType = ChatChannelType.All;
			if (startTyping)
			{
				this.StartTyping();
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00014170 File Offset: 0x00012370
		public void TypeToChannelTeam(bool startTyping = false)
		{
			this.ActiveChannelType = ChatChannelType.Team;
			if (startTyping)
			{
				this.StartTyping();
			}
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00014182 File Offset: 0x00012382
		public void StartInspectingMessages()
		{
			this.IsInspectingMessages = true;
			this.IsTypingText = false;
			this.WrittenText = "";
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001419D File Offset: 0x0001239D
		public void StopInspectingMessages()
		{
			this.IsInspectingMessages = false;
			this.IsTypingText = false;
			this.WrittenText = "";
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000141B8 File Offset: 0x000123B8
		public void StartTyping()
		{
			this.IsTypingText = true;
			this.IsInspectingMessages = true;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x000141C8 File Offset: 0x000123C8
		public void StopTyping(bool resetWrittenText = false)
		{
			this.IsTypingText = false;
			this.IsInspectingMessages = false;
			if (resetWrittenText)
			{
				this.WrittenText = "";
			}
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x000141E6 File Offset: 0x000123E6
		public void SendCurrentlyTypedMessage()
		{
			this.ExecuteSendMessage();
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x000141F0 File Offset: 0x000123F0
		private void RefreshVisibility()
		{
			foreach (MPChatLineVM mpchatLineVM in this._allMessages)
			{
				mpchatLineVM.ToggleForceVisible(this.IsTypingText || this.IsInspectingMessages);
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00014254 File Offset: 0x00012454
		public void ExecuteSaveSizes()
		{
			BannerlordConfig.ChatBoxSizeX = this.ChatBoxSizeX;
			BannerlordConfig.ChatBoxSizeY = this.ChatBoxSizeY;
			BannerlordConfig.Save();
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00014272 File Offset: 0x00012472
		public void SetMessageHistoryCapacity(int capacity)
		{
			this._maxHistoryCount = capacity;
			MBBindingList<MPChatLineVM> messageHistory = this.MessageHistory;
			if (messageHistory == null)
			{
				return;
			}
			messageHistory.Clear();
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0001428B File Offset: 0x0001248B
		// (set) Token: 0x06000515 RID: 1301 RVA: 0x00014293 File Offset: 0x00012493
		[DataSourceProperty]
		public float ChatBoxSizeX
		{
			get
			{
				return this._chatBoxSizeX;
			}
			set
			{
				if (value != this._chatBoxSizeX)
				{
					this._chatBoxSizeX = value;
					base.OnPropertyChangedWithValue(value, "ChatBoxSizeX");
				}
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x000142B1 File Offset: 0x000124B1
		// (set) Token: 0x06000517 RID: 1303 RVA: 0x000142B9 File Offset: 0x000124B9
		[DataSourceProperty]
		public float ChatBoxSizeY
		{
			get
			{
				return this._chatBoxSizeY;
			}
			set
			{
				if (value != this._chatBoxSizeY)
				{
					this._chatBoxSizeY = value;
					base.OnPropertyChangedWithValue(value, "ChatBoxSizeY");
				}
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x000142D7 File Offset: 0x000124D7
		// (set) Token: 0x06000519 RID: 1305 RVA: 0x000142DF File Offset: 0x000124DF
		[DataSourceProperty]
		public int MaxMessageLength
		{
			get
			{
				return this._maxMessageLength;
			}
			set
			{
				if (value != this._maxMessageLength)
				{
					this._maxMessageLength = value;
					base.OnPropertyChangedWithValue(value, "MaxMessageLength");
				}
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x000142FD File Offset: 0x000124FD
		// (set) Token: 0x0600051B RID: 1307 RVA: 0x00014305 File Offset: 0x00012505
		[DataSourceProperty]
		public bool IsTypingText
		{
			get
			{
				return this._isTypingText;
			}
			set
			{
				if (value != this._isTypingText)
				{
					this._isTypingText = value;
					base.OnPropertyChangedWithValue(value, "IsTypingText");
					this.RefreshVisibility();
				}
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x00014329 File Offset: 0x00012529
		// (set) Token: 0x0600051D RID: 1309 RVA: 0x00014331 File Offset: 0x00012531
		[DataSourceProperty]
		public bool IsInspectingMessages
		{
			get
			{
				return this._isInspectingMessages;
			}
			set
			{
				if (value != this._isInspectingMessages)
				{
					this._isInspectingMessages = value;
					this.UpdateHideShowText(this._isInspectingMessages);
					this.UpdateShortcutTexts();
					base.OnPropertyChangedWithValue(value, "IsInspectingMessages");
					this.RefreshVisibility();
				}
			}
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00014367 File Offset: 0x00012567
		// (set) Token: 0x0600051F RID: 1311 RVA: 0x0001436F File Offset: 0x0001256F
		[DataSourceProperty]
		public bool IsChatDisabled
		{
			get
			{
				return this._isChatDisabled;
			}
			set
			{
				if (value != this._isChatDisabled)
				{
					this._isChatDisabled = value;
					if (value)
					{
						this.StopTyping(true);
					}
					Action<bool> onChatDisabledStateChanged = this._onChatDisabledStateChanged;
					if (onChatDisabledStateChanged != null)
					{
						onChatDisabledStateChanged(value);
					}
					base.OnPropertyChangedWithValue(value, "IsChatDisabled");
				}
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x000143A9 File Offset: 0x000125A9
		// (set) Token: 0x06000521 RID: 1313 RVA: 0x000143B1 File Offset: 0x000125B1
		[DataSourceProperty]
		public bool ShowHideShowHint
		{
			get
			{
				return this._showHideShowHint;
			}
			set
			{
				if (value != this._showHideShowHint)
				{
					this._showHideShowHint = value;
					base.OnPropertyChangedWithValue(value, "ShowHideShowHint");
				}
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x06000522 RID: 1314 RVA: 0x000143CF File Offset: 0x000125CF
		// (set) Token: 0x06000523 RID: 1315 RVA: 0x000143D7 File Offset: 0x000125D7
		[DataSourceProperty]
		public bool IsOptionsAvailable
		{
			get
			{
				return this._isOptionsAvailable;
			}
			set
			{
				if (value != this._isOptionsAvailable)
				{
					this._isOptionsAvailable = value;
					base.OnPropertyChangedWithValue(value, "IsOptionsAvailable");
				}
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x000143F5 File Offset: 0x000125F5
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x000143FD File Offset: 0x000125FD
		[DataSourceProperty]
		public bool ShouldHaveOffset
		{
			get
			{
				return this._shouldHaveOffset;
			}
			set
			{
				if (value != this._shouldHaveOffset)
				{
					this._shouldHaveOffset = value;
					base.OnPropertyChangedWithValue(value, "ShouldHaveOffset");
				}
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x0001441B File Offset: 0x0001261B
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x00014423 File Offset: 0x00012623
		[DataSourceProperty]
		public string WrittenText
		{
			get
			{
				return this._writtenText;
			}
			set
			{
				if (value != this._writtenText)
				{
					this._writtenText = value;
					base.OnPropertyChangedWithValue<string>(value, "WrittenText");
				}
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x00014446 File Offset: 0x00012646
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x0001444E File Offset: 0x0001264E
		[DataSourceProperty]
		public Color ActiveChannelColor
		{
			get
			{
				return this._activeChannelColor;
			}
			set
			{
				if (value != this._activeChannelColor)
				{
					this._activeChannelColor = value;
					base.OnPropertyChangedWithValue(value, "ActiveChannelColor");
				}
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x00014471 File Offset: 0x00012671
		// (set) Token: 0x0600052B RID: 1323 RVA: 0x00014479 File Offset: 0x00012679
		[DataSourceProperty]
		public string ActiveChannelNameText
		{
			get
			{
				return this._activeChannelNameText;
			}
			set
			{
				if (value != this._activeChannelNameText)
				{
					this._activeChannelNameText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActiveChannelNameText");
				}
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x0001449C File Offset: 0x0001269C
		// (set) Token: 0x0600052D RID: 1325 RVA: 0x000144A4 File Offset: 0x000126A4
		[DataSourceProperty]
		public string HideShowText
		{
			get
			{
				return this._hideShowText;
			}
			set
			{
				if (value != this._hideShowText)
				{
					this._hideShowText = value;
					base.OnPropertyChangedWithValue<string>(value, "HideShowText");
				}
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x000144C7 File Offset: 0x000126C7
		// (set) Token: 0x0600052F RID: 1327 RVA: 0x000144CF File Offset: 0x000126CF
		[DataSourceProperty]
		public string ToggleCombatLogText
		{
			get
			{
				return this._toggleCombatLogText;
			}
			set
			{
				if (value != this._toggleCombatLogText)
				{
					this._toggleCombatLogText = value;
					base.OnPropertyChangedWithValue<string>(value, "ToggleCombatLogText");
				}
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x000144F2 File Offset: 0x000126F2
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x000144FA File Offset: 0x000126FA
		[DataSourceProperty]
		public string ToggleBarkText
		{
			get
			{
				return this._toggleBarkText;
			}
			set
			{
				if (value != this._toggleBarkText)
				{
					this._toggleBarkText = value;
					base.OnPropertyChangedWithValue<string>(value, "ToggleBarkText");
				}
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x0001451D File Offset: 0x0001271D
		// (set) Token: 0x06000533 RID: 1331 RVA: 0x00014525 File Offset: 0x00012725
		[DataSourceProperty]
		public string CycleThroughChannelsText
		{
			get
			{
				return this._cycleThroughChannelsText;
			}
			set
			{
				if (value != this._cycleThroughChannelsText)
				{
					this._cycleThroughChannelsText = value;
					base.OnPropertyChangedWithValue<string>(value, "CycleThroughChannelsText");
				}
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00014548 File Offset: 0x00012748
		// (set) Token: 0x06000535 RID: 1333 RVA: 0x00014550 File Offset: 0x00012750
		[DataSourceProperty]
		public string SendMessageText
		{
			get
			{
				return this._sendMessageText;
			}
			set
			{
				if (value != this._sendMessageText)
				{
					this._sendMessageText = value;
					base.OnPropertyChangedWithValue<string>(value, "SendMessageText");
				}
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x00014573 File Offset: 0x00012773
		// (set) Token: 0x06000537 RID: 1335 RVA: 0x0001457B File Offset: 0x0001277B
		[DataSourceProperty]
		public string CancelSendingText
		{
			get
			{
				return this._cancelSendingText;
			}
			set
			{
				if (value != this._cancelSendingText)
				{
					this._cancelSendingText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelSendingText");
				}
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0001459E File Offset: 0x0001279E
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x000145A6 File Offset: 0x000127A6
		[DataSourceProperty]
		public MBBindingList<MPChatLineVM> MessageHistory
		{
			get
			{
				return this._messageHistory;
			}
			set
			{
				if (value != this._messageHistory)
				{
					this._messageHistory = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPChatLineVM>>(value, "MessageHistory");
				}
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x000145C4 File Offset: 0x000127C4
		// (set) Token: 0x0600053B RID: 1339 RVA: 0x000145CC File Offset: 0x000127CC
		[DataSourceProperty]
		public HintViewModel CombatLogHint
		{
			get
			{
				return this._combatLogHint;
			}
			set
			{
				if (value != this._combatLogHint)
				{
					this._combatLogHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CombatLogHint");
				}
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x000145EA File Offset: 0x000127EA
		// (set) Token: 0x0600053D RID: 1341 RVA: 0x000145F2 File Offset: 0x000127F2
		[DataSourceProperty]
		public bool IncludeCombatLog
		{
			get
			{
				return this._includeCombatLog;
			}
			set
			{
				if (value != this._includeCombatLog)
				{
					this._includeCombatLog = value;
					base.OnPropertyChangedWithValue(value, "IncludeCombatLog");
					this.ChatHistoryFilterToggled();
					BannerlordConfig.ReportDamage = value;
				}
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x0001461C File Offset: 0x0001281C
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x00014624 File Offset: 0x00012824
		[DataSourceProperty]
		public bool IncludeBark
		{
			get
			{
				return this._includeBark;
			}
			set
			{
				if (value != this._includeBark)
				{
					this._includeBark = value;
					base.OnPropertyChangedWithValue(value, "IncludeBark");
					this.ChatHistoryFilterToggled();
					BannerlordConfig.ReportBark = value;
				}
			}
		}

		// Token: 0x0400023A RID: 570
		private readonly TextObject _hideText = new TextObject("{=ou5KJERr}Press '{KEY}' to hide", null);

		// Token: 0x0400023B RID: 571
		private readonly TextObject _cycleChannelsText = new TextObject("{=Dhb2N5JD}Press '{KEY}' to cycle through channels", null);

		// Token: 0x0400023C RID: 572
		private readonly TextObject _sendMessageTextObject = new TextObject("{=f64QfbTO}'{KEY}' to send", null);

		// Token: 0x0400023D RID: 573
		private readonly TextObject _cancelSendingTextObject = new TextObject("{=U1rHNqOk}'{KEY}' to cancel", null);

		// Token: 0x0400023E RID: 574
		public const string DefaultCategory = "Default";

		// Token: 0x0400023F RID: 575
		public const string CombatCategory = "Combat";

		// Token: 0x04000240 RID: 576
		public const string SocialCategory = "Social";

		// Token: 0x04000241 RID: 577
		public const string BarkCategory = "Bark";

		// Token: 0x04000242 RID: 578
		private int _maxHistoryCount = 100;

		// Token: 0x04000243 RID: 579
		private const int _spamDetectionInterval = 15;

		// Token: 0x04000244 RID: 580
		private const int _maxMessagesAllowedPerInterval = 5;

		// Token: 0x04000245 RID: 581
		private List<float> _recentlySentMessagesTimes;

		// Token: 0x04000246 RID: 582
		private readonly List<MPChatLineVM> _allMessages;

		// Token: 0x04000247 RID: 583
		private readonly Queue<MPChatLineVM> _requestedMessages;

		// Token: 0x04000248 RID: 584
		private Action<bool> _onChatDisabledStateChanged;

		// Token: 0x04000249 RID: 585
		private Func<TextObject> _getToggleChatKeyText;

		// Token: 0x0400024A RID: 586
		private Func<TextObject> _getCycleChannelsKeyText;

		// Token: 0x0400024B RID: 587
		private Func<TextObject> _getSendMessageKeyText;

		// Token: 0x0400024C RID: 588
		private Func<TextObject> _getCancelSendingKeyText;

		// Token: 0x0400024D RID: 589
		private ChatBox _chatBox;

		// Token: 0x0400024E RID: 590
		private Game _game;

		// Token: 0x0400024F RID: 591
		private Mission _mission;

		// Token: 0x04000250 RID: 592
		private ChatChannelType _activeChannelType = ChatChannelType.None;

		// Token: 0x04000251 RID: 593
		private float _chatBoxSizeX;

		// Token: 0x04000252 RID: 594
		private float _chatBoxSizeY;

		// Token: 0x04000253 RID: 595
		private int _maxMessageLength;

		// Token: 0x04000254 RID: 596
		private string _writtenText = "";

		// Token: 0x04000255 RID: 597
		private string _activeChannelNameText;

		// Token: 0x04000256 RID: 598
		private string _hideShowText;

		// Token: 0x04000257 RID: 599
		private string _toggleCombatLogText;

		// Token: 0x04000258 RID: 600
		private string _toggleBarkText;

		// Token: 0x04000259 RID: 601
		private string _cycleThroughChannelsText;

		// Token: 0x0400025A RID: 602
		private string _sendMessageText;

		// Token: 0x0400025B RID: 603
		private string _cancelSendingText;

		// Token: 0x0400025C RID: 604
		private MBBindingList<MPChatLineVM> _messageHistory;

		// Token: 0x0400025D RID: 605
		private bool _includeCombatLog;

		// Token: 0x0400025E RID: 606
		private bool _includeBark;

		// Token: 0x0400025F RID: 607
		private bool _isTypingText;

		// Token: 0x04000260 RID: 608
		private bool _isInspectingMessages;

		// Token: 0x04000261 RID: 609
		private bool _isChatDisabled;

		// Token: 0x04000262 RID: 610
		private bool _showHideShowHint;

		// Token: 0x04000263 RID: 611
		private bool _isOptionsAvailable;

		// Token: 0x04000264 RID: 612
		private bool _shouldHaveOffset;

		// Token: 0x04000265 RID: 613
		private HintViewModel _combatLogHint;

		// Token: 0x04000266 RID: 614
		private Color _activeChannelColor;
	}
}
