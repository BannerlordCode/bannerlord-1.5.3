using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Scoreboard
{
	// Token: 0x02000022 RID: 34
	public class MissionScoreboardVM : ViewModel
	{
		// Token: 0x06000223 RID: 547 RVA: 0x000085AC File Offset: 0x000067AC
		public MissionScoreboardVM(bool isSingleTeam, Mission mission)
		{
			this._chatBox = Game.Current.GetGameHandler<ChatBox>();
			this._chatBox.OnPlayerMuteChanged += this.OnPlayerMuteChanged;
			this._mission = mission;
			MissionLobbyComponent missionBehavior = mission.GetMissionBehavior<MissionLobbyComponent>();
			this._missionScoreboardComponent = mission.GetMissionBehavior<MissionScoreboardComponent>();
			this._voiceChatHandler = this._mission.GetMissionBehavior<VoiceChatHandler>();
			this._permissionHandler = GameNetwork.GetNetworkComponent<MultiplayerPermissionHandler>();
			this._canStartKickPolls = MultiplayerOptions.OptionType.AllowPollsToKickPlayers.GetBoolValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			if (this._canStartKickPolls)
			{
				this._missionPollComponent = mission.GetMissionBehavior<MultiplayerPollComponent>();
			}
			this.EndOfBattle = new MPEndOfBattleVM(mission, this._missionScoreboardComponent, isSingleTeam);
			this.PlayerActionList = new MBBindingList<StringPairItemWithActionVM>();
			this.Sides = new MBBindingList<MissionScoreboardSideVM>();
			this._missionSides = new Dictionary<BattleSideEnum, MissionScoreboardSideVM>();
			this.IsSingleSide = isSingleTeam;
			this.InitSides();
			GameKey gameKey = HotKeyManager.GetCategory("ScoreboardHotKeyCategory").GetGameKey(35);
			this.ShowMouseKey = InputKeyItemVM.CreateFromGameKey(gameKey, false);
			this.MissionName = "";
			this.IsBotsEnabled = missionBehavior.MissionType == MultiplayerGameType.Captain || missionBehavior.MissionType == MultiplayerGameType.Battle;
			this.RegisterEvents();
			this.RefreshValues();
		}

		// Token: 0x06000224 RID: 548 RVA: 0x000086F4 File Offset: 0x000068F4
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.UnregisterEvents();
			foreach (MissionScoreboardSideVM missionScoreboardSideVM in this.Sides)
			{
				missionScoreboardSideVM.OnFinalize();
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000874C File Offset: 0x0000694C
		private void RegisterEvents()
		{
			if (this._voiceChatHandler != null)
			{
				this._voiceChatHandler.OnPeerMuteStatusUpdated += this.OnPeerMuteStatusUpdated;
			}
			if (this._permissionHandler != null)
			{
				this._permissionHandler.OnPlayerPlatformMuteChanged += this.OnPlayerPlatformMuteChanged;
			}
			this._missionScoreboardComponent.OnPlayerSideChanged += this.OnPlayerSideChanged;
			this._missionScoreboardComponent.OnPlayerPropertiesChanged += this.OnPlayerPropertiesChanged;
			this._missionScoreboardComponent.OnBotPropertiesChanged += this.OnBotPropertiesChanged;
			this._missionScoreboardComponent.OnRoundPropertiesChanged += this.OnRoundPropertiesChanged;
			this._missionScoreboardComponent.OnScoreboardInitialized += this.OnScoreboardInitialized;
			this._missionScoreboardComponent.OnMVPSelected += this.OnMVPSelected;
			MissionPeer.OnTeamChanged += this.OnPeerTeamChanged;
		}

		// Token: 0x06000226 RID: 550 RVA: 0x00008834 File Offset: 0x00006A34
		private void UnregisterEvents()
		{
			this._missionScoreboardComponent.OnPlayerSideChanged -= this.OnPlayerSideChanged;
			this._missionScoreboardComponent.OnPlayerPropertiesChanged -= this.OnPlayerPropertiesChanged;
			this._missionScoreboardComponent.OnBotPropertiesChanged -= this.OnBotPropertiesChanged;
			this._missionScoreboardComponent.OnRoundPropertiesChanged -= this.OnRoundPropertiesChanged;
			this._missionScoreboardComponent.OnScoreboardInitialized -= this.OnScoreboardInitialized;
			this._missionScoreboardComponent.OnMVPSelected -= this.OnMVPSelected;
			this._chatBox.OnPlayerMuteChanged -= this.OnPlayerMuteChanged;
			MissionPeer.OnTeamChanged -= this.OnPeerTeamChanged;
			if (this._voiceChatHandler != null)
			{
				this._voiceChatHandler.OnPeerMuteStatusUpdated -= this.OnPeerMuteStatusUpdated;
			}
			if (this._permissionHandler != null)
			{
				this._permissionHandler.OnPlayerPlatformMuteChanged -= this.OnPlayerPlatformMuteChanged;
			}
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00008934 File Offset: 0x00006B34
		private void OnPlayerPlatformMuteChanged(PlayerId playerId, bool isPlayerMuted)
		{
			foreach (MissionScoreboardSideVM missionScoreboardSideVM in this.Sides)
			{
				foreach (MissionScoreboardPlayerVM missionScoreboardPlayerVM in missionScoreboardSideVM.Players)
				{
					if (!missionScoreboardPlayerVM.IsBot && missionScoreboardPlayerVM.Peer.Peer.Id.Equals(playerId))
					{
						missionScoreboardPlayerVM.UpdateIsMuted();
						return;
					}
				}
			}
		}

		// Token: 0x06000228 RID: 552 RVA: 0x000089D8 File Offset: 0x00006BD8
		private void OnPlayerMuteChanged(PlayerId playerId, bool isMuted)
		{
			foreach (MissionScoreboardSideVM missionScoreboardSideVM in this.Sides)
			{
				foreach (MissionScoreboardPlayerVM missionScoreboardPlayerVM in missionScoreboardSideVM.Players)
				{
					if (!missionScoreboardPlayerVM.IsBot && missionScoreboardPlayerVM.Peer.Peer.Id.Equals(playerId))
					{
						missionScoreboardPlayerVM.UpdateIsMuted();
						return;
					}
				}
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00008A7C File Offset: 0x00006C7C
		public override void RefreshValues()
		{
			base.RefreshValues();
			MissionLobbyComponent missionBehavior = this._mission.GetMissionBehavior<MissionLobbyComponent>();
			this.UpdateToggleMuteText();
			this.GameModeText = GameTexts.FindText("str_multiplayer_game_type", missionBehavior.MissionType.ToString()).ToString().ToLower();
			this.EndOfBattle.RefreshValues();
			this.Sides.ApplyActionOnAllItems(delegate(MissionScoreboardSideVM x)
			{
				x.RefreshValues();
			});
			TextObject textObject;
			if (GameTexts.TryGetText("str_multiplayer_scene_name", out textObject, missionBehavior.Mission.SceneName))
			{
				this.MapName = textObject.ToString();
			}
			else
			{
				this.MapName = missionBehavior.Mission.SceneName;
			}
			this.ServerName = MultiplayerOptions.OptionType.ServerName.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			this.RefreshSpectatorCount();
			InputKeyItemVM showMouseKey = this.ShowMouseKey;
			if (showMouseKey == null)
			{
				return;
			}
			showMouseKey.RefreshValues();
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00008B60 File Offset: 0x00006D60
		private void ExecutePopulateActionList(MissionScoreboardPlayerVM player)
		{
			this.PlayerActionList.Clear();
			if (player.Peer != null && !player.IsMine && !player.IsBot)
			{
				PlayerId id = player.Peer.Peer.Id;
				bool flag = this._chatBox.IsPlayerMutedFromGame(id);
				bool flag2 = PermaMuteList.IsPlayerMuted(id);
				bool flag3 = this._chatBox.IsPlayerMutedFromPlatform(id);
				bool isMutedFromPlatform = player.Peer.IsMutedFromPlatform;
				if (!flag3)
				{
					if (!flag2)
					{
						if (PlatformServices.Instance.IsPermanentMuteAvailable)
						{
							this.PlayerActionList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecutePermanentlyMute), new TextObject("{=77jmd4QF}Mute Permanently", null).ToString(), "PermanentMute", player));
						}
						string text = (flag ? GameTexts.FindText("str_mp_scoreboard_context_unmute_text", null).ToString() : GameTexts.FindText("str_mp_scoreboard_context_mute_text", null).ToString());
						this.PlayerActionList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteMute), text, flag ? "UnmuteText" : "MuteText", player));
					}
					else
					{
						this.PlayerActionList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteLiftPermanentMute), new TextObject("{=CIVPNf2d}Remove Permanent Mute", null).ToString(), "UnmuteText", player));
					}
				}
				if (player.IsTeammate)
				{
					if (!isMutedFromPlatform && this._voiceChatHandler != null && !flag2)
					{
						string text2 = (player.Peer.IsMuted ? GameTexts.FindText("str_mp_scoreboard_context_unmute_voice", null).ToString() : GameTexts.FindText("str_mp_scoreboard_context_mute_voice", null).ToString());
						this.PlayerActionList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteMuteVoice), text2, player.Peer.IsMuted ? "UnmuteVoice" : "MuteVoice", player));
					}
					if (this._canStartKickPolls)
					{
						this.PlayerActionList.Add(new StringPairItemWithActionVM(new Action<object>(this.ExecuteKick), GameTexts.FindText("str_mp_scoreboard_context_kick", null).ToString(), "StartKickPoll", player));
					}
				}
				StringPairItemWithActionVM stringPairItemWithActionVM = new StringPairItemWithActionVM(new Action<object>(this.ExecuteReport), GameTexts.FindText("str_mp_scoreboard_context_report", null).ToString(), "Report", player);
				if (MultiplayerReportPlayerManager.IsPlayerReportedOverLimit(id))
				{
					stringPairItemWithActionVM.IsEnabled = false;
					stringPairItemWithActionVM.Hint.HintText = new TextObject("{=klkYFik9}You've already reported this player.", null);
				}
				this.PlayerActionList.Add(stringPairItemWithActionVM);
				MultiplayerPlayerContextMenuHelper.AddMissionViewProfileOptions(player, this.PlayerActionList);
			}
			if (this.PlayerActionList.Count > 0)
			{
				this.IsPlayerActionsActive = false;
				this.IsPlayerActionsActive = true;
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x00008DE9 File Offset: 0x00006FE9
		public void SetMouseState(bool isMouseVisible)
		{
			this.IsMouseEnabled = isMouseVisible;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00008DF4 File Offset: 0x00006FF4
		private void ExecuteReport(object playerObj)
		{
			MissionScoreboardPlayerVM missionScoreboardPlayerVM = playerObj as MissionScoreboardPlayerVM;
			MultiplayerReportPlayerManager.RequestReportPlayer(NetworkMain.GameClient.CurrentMatchId, missionScoreboardPlayerVM.Peer.Peer.Id, missionScoreboardPlayerVM.Peer.DisplayedName, true);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00008E34 File Offset: 0x00007034
		private void ExecuteMute(object playerObj)
		{
			MissionScoreboardPlayerVM missionScoreboardPlayerVM = playerObj as MissionScoreboardPlayerVM;
			bool flag = this._chatBox.IsPlayerMutedFromGame(missionScoreboardPlayerVM.Peer.Peer.Id);
			this._chatBox.SetPlayerMuted(missionScoreboardPlayerVM.Peer.Peer.Id, !flag);
			GameTexts.SetVariable("PLAYER_NAME", missionScoreboardPlayerVM.Peer.DisplayedName);
			InformationManager.DisplayMessage(new InformationMessage((!flag) ? GameTexts.FindText("str_mute_notification", null).ToString() : GameTexts.FindText("str_unmute_notification", null).ToString()));
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00008EC8 File Offset: 0x000070C8
		private void ExecuteMuteVoice(object playerObj)
		{
			MissionScoreboardPlayerVM missionScoreboardPlayerVM = playerObj as MissionScoreboardPlayerVM;
			missionScoreboardPlayerVM.Peer.SetMuted(!missionScoreboardPlayerVM.Peer.IsMuted);
			missionScoreboardPlayerVM.UpdateIsMuted();
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00008EFC File Offset: 0x000070FC
		private void ExecutePermanentlyMute(object playerObj)
		{
			MissionScoreboardPlayerVM missionScoreboardPlayerVM = playerObj as MissionScoreboardPlayerVM;
			PermaMuteList.MutePlayer(missionScoreboardPlayerVM.Peer.Peer.Id, missionScoreboardPlayerVM.Peer.Name);
			missionScoreboardPlayerVM.Peer.SetMuted(true);
			missionScoreboardPlayerVM.UpdateIsMuted();
			GameTexts.SetVariable("PLAYER_NAME", missionScoreboardPlayerVM.Peer.DisplayedName);
			InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_permanent_mute_notification", null).ToString()));
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00008F74 File Offset: 0x00007174
		private void ExecuteLiftPermanentMute(object playerObj)
		{
			MissionScoreboardPlayerVM missionScoreboardPlayerVM = playerObj as MissionScoreboardPlayerVM;
			PermaMuteList.RemoveMutedPlayer(missionScoreboardPlayerVM.Peer.Peer.Id);
			missionScoreboardPlayerVM.Peer.SetMuted(false);
			missionScoreboardPlayerVM.UpdateIsMuted();
			GameTexts.SetVariable("PLAYER_NAME", missionScoreboardPlayerVM.Peer.DisplayedName);
			InformationManager.DisplayMessage(new InformationMessage(GameTexts.FindText("str_unmute_notification", null).ToString()));
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00008FE0 File Offset: 0x000071E0
		private void ExecuteKick(object playerObj)
		{
			MissionScoreboardPlayerVM missionScoreboardPlayerVM = playerObj as MissionScoreboardPlayerVM;
			this._missionPollComponent.RequestKickPlayerPoll(missionScoreboardPlayerVM.Peer.GetNetworkPeer(), false);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000900C File Offset: 0x0000720C
		public void Tick(float dt)
		{
			if (this.IsActive)
			{
				MPEndOfBattleVM endOfBattle = this.EndOfBattle;
				if (endOfBattle != null)
				{
					endOfBattle.Tick(dt);
				}
				this.CheckAttributeRefresh(dt);
				foreach (MissionScoreboardSideVM missionScoreboardSideVM in this.Sides)
				{
					missionScoreboardSideVM.Tick(dt);
				}
				foreach (MissionScoreboardSideVM missionScoreboardSideVM2 in this.Sides)
				{
					foreach (MissionScoreboardPlayerVM missionScoreboardPlayerVM in missionScoreboardSideVM2.Players)
					{
						missionScoreboardPlayerVM.RefreshDivision(this.IsSingleSide);
					}
				}
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x000090F0 File Offset: 0x000072F0
		private void CheckAttributeRefresh(float dt)
		{
			this._attributeRefreshTimeElapsed += dt;
			if (this._attributeRefreshTimeElapsed >= 1f)
			{
				this.UpdateSideAllPlayersAttributes(BattleSideEnum.Attacker);
				this.UpdateSideAllPlayersAttributes(BattleSideEnum.Defender);
				this.RefreshSpectatorCount();
				this._attributeRefreshTimeElapsed = 0f;
			}
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000912C File Offset: 0x0000732C
		private void UpdateSideAllPlayersAttributes(BattleSideEnum battleSide)
		{
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide = this._missionScoreboardComponent.Sides.FirstOrDefault<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide s) => s != null && s.Side == battleSide);
			if (missionScoreboardSide != null)
			{
				foreach (MissionPeer missionPeer in missionScoreboardSide.Players)
				{
					this.OnPlayerPropertiesChanged(battleSide, missionPeer);
				}
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000091AC File Offset: 0x000073AC
		public void OnPlayerSideChanged(Team curTeam, Team nextTeam, MissionPeer client)
		{
			if (client.IsMine && nextTeam != null && this.IsSideValid(nextTeam.Side))
			{
				this.InitSides();
				return;
			}
			if (curTeam != null && this.IsSideValid(curTeam.Side))
			{
				this._missionSides[this._missionScoreboardComponent.GetSideSafe(curTeam.Side).Side].RemovePlayer(client);
			}
			if (nextTeam != null && this.IsSideValid(nextTeam.Side))
			{
				this._missionSides[this._missionScoreboardComponent.GetSideSafe(nextTeam.Side).Side].AddPlayer(client);
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000924C File Offset: 0x0000744C
		private void OnRoundPropertiesChanged()
		{
			foreach (MissionScoreboardSideVM missionScoreboardSideVM in this._missionSides.Values)
			{
				missionScoreboardSideVM.UpdateRoundAttributes();
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x000092A4 File Offset: 0x000074A4
		private void OnPlayerPropertiesChanged(BattleSideEnum side, MissionPeer client)
		{
			if (this.IsSideValid(side))
			{
				this._missionSides[this._missionScoreboardComponent.GetSideSafe(side).Side].UpdatePlayerAttributes(client);
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x000092D4 File Offset: 0x000074D4
		private void OnBotPropertiesChanged(BattleSideEnum side)
		{
			BattleSideEnum side2 = this._missionScoreboardComponent.GetSideSafe(side).Side;
			if (this.IsSideValid(side2))
			{
				this._missionSides[side2].UpdateBotAttributes();
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000930D File Offset: 0x0000750D
		private void OnScoreboardInitialized()
		{
			this.InitSides();
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00009318 File Offset: 0x00007518
		private void OnMVPSelected(MissionPeer mvpPeer, int mvpCount)
		{
			foreach (MissionScoreboardSideVM missionScoreboardSideVM in this.Sides)
			{
				foreach (MissionScoreboardPlayerVM missionScoreboardPlayerVM in missionScoreboardSideVM.Players)
				{
					if (missionScoreboardPlayerVM.Peer == mvpPeer)
					{
						missionScoreboardPlayerVM.SetMVPBadgeCount(mvpCount);
						break;
					}
				}
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x000093A4 File Offset: 0x000075A4
		private bool IsSideValid(BattleSideEnum side)
		{
			if (this.IsSingleSide)
			{
				return this._missionScoreboardComponent != null && side != BattleSideEnum.None && side != BattleSideEnum.NumSides;
			}
			return this._missionScoreboardComponent != null && side != BattleSideEnum.None && side != BattleSideEnum.NumSides && this._missionScoreboardComponent.Sides.Any<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide s) => s != null && s.Side == side);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00009420 File Offset: 0x00007620
		private void InitSides()
		{
			this.Sides.Clear();
			this._missionSides.Clear();
			if (this.IsSingleSide)
			{
				MissionScoreboardComponent.MissionScoreboardSide sideSafe = this._missionScoreboardComponent.GetSideSafe(BattleSideEnum.Defender);
				MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(sideSafe.GetCulture(), null);
				MissionScoreboardSideVM missionScoreboardSideVM = new MissionScoreboardSideVM(sideSafe, new Action<MissionScoreboardPlayerVM>(this.ExecutePopulateActionList), this.IsSingleSide, false, multiplayerBattleColors.AttackerColors);
				this.Sides.Add(missionScoreboardSideVM);
				this._missionSides.Add(sideSafe.Side, missionScoreboardSideVM);
				return;
			}
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide = this._missionScoreboardComponent.Sides.FirstOrDefault<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide s) => s != null && s.Side == BattleSideEnum.Attacker);
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide2 = this._missionScoreboardComponent.Sides.FirstOrDefault<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide s) => s != null && s.Side == BattleSideEnum.Defender);
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide3;
			if (missionPeer != null)
			{
				Team team = missionPeer.Team;
				BattleSideEnum? battleSideEnum = ((team != null) ? new BattleSideEnum?(team.Side) : null);
				BattleSideEnum battleSideEnum2 = BattleSideEnum.Attacker;
				if ((battleSideEnum.GetValueOrDefault() == battleSideEnum2) & (battleSideEnum != null))
				{
					missionScoreboardSide3 = missionScoreboardSide;
					goto IL_012E;
				}
			}
			missionScoreboardSide3 = missionScoreboardSide2;
			IL_012E:
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide4 = missionScoreboardSide3;
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide5;
			if (missionPeer != null)
			{
				Team team2 = missionPeer.Team;
				BattleSideEnum? battleSideEnum = ((team2 != null) ? new BattleSideEnum?(team2.Side) : null);
				BattleSideEnum battleSideEnum2 = BattleSideEnum.Attacker;
				if ((battleSideEnum.GetValueOrDefault() == battleSideEnum2) & (battleSideEnum != null))
				{
					missionScoreboardSide5 = missionScoreboardSide2;
					goto IL_0173;
				}
			}
			missionScoreboardSide5 = missionScoreboardSide;
			IL_0173:
			MissionScoreboardComponent.MissionScoreboardSide missionScoreboardSide6 = missionScoreboardSide5;
			BasicCultureObject basicCultureObject = ((missionScoreboardSide4 != null) ? missionScoreboardSide4.GetCulture() : null);
			BasicCultureObject basicCultureObject2 = ((missionScoreboardSide6 != null) ? missionScoreboardSide6.GetCulture() : null);
			MultiplayerBattleColors multiplayerBattleColors2 = MultiplayerBattleColors.CreateWith(basicCultureObject, basicCultureObject2);
			if (missionScoreboardSide4 != null)
			{
				MultiplayerBattleColors.MultiplayerCultureColorInfo multiplayerCultureColorInfo2;
				if (basicCultureObject.StringId == basicCultureObject2.StringId)
				{
					MultiplayerBattleColors.MultiplayerCultureColorInfo multiplayerCultureColorInfo;
					if (missionPeer != null)
					{
						Team team3 = missionPeer.Team;
						BattleSideEnum? battleSideEnum = ((team3 != null) ? new BattleSideEnum?(team3.Side) : null);
						BattleSideEnum battleSideEnum2 = BattleSideEnum.Attacker;
						if ((battleSideEnum.GetValueOrDefault() == battleSideEnum2) & (battleSideEnum != null))
						{
							multiplayerCultureColorInfo = multiplayerBattleColors2.AttackerColors;
							goto IL_0209;
						}
					}
					multiplayerCultureColorInfo = multiplayerBattleColors2.DefenderColors;
					IL_0209:
					multiplayerCultureColorInfo2 = multiplayerCultureColorInfo;
				}
				else
				{
					multiplayerCultureColorInfo2 = multiplayerBattleColors2.AttackerColors;
				}
				MissionScoreboardSideVM missionScoreboardSideVM2 = new MissionScoreboardSideVM(missionScoreboardSide4, new Action<MissionScoreboardPlayerVM>(this.ExecutePopulateActionList), this.IsSingleSide, false, multiplayerCultureColorInfo2);
				this.Sides.Add(missionScoreboardSideVM2);
				this._missionSides.Add(missionScoreboardSide4.Side, missionScoreboardSideVM2);
			}
			if (missionScoreboardSide6 != null)
			{
				MultiplayerBattleColors.MultiplayerCultureColorInfo multiplayerCultureColorInfo4;
				if (basicCultureObject.StringId == basicCultureObject2.StringId)
				{
					MultiplayerBattleColors.MultiplayerCultureColorInfo multiplayerCultureColorInfo3;
					if (missionPeer != null)
					{
						Team team4 = missionPeer.Team;
						BattleSideEnum? battleSideEnum = ((team4 != null) ? new BattleSideEnum?(team4.Side) : null);
						BattleSideEnum battleSideEnum2 = BattleSideEnum.Attacker;
						if ((battleSideEnum.GetValueOrDefault() == battleSideEnum2) & (battleSideEnum != null))
						{
							multiplayerCultureColorInfo3 = multiplayerBattleColors2.DefenderColors;
							goto IL_02BE;
						}
					}
					multiplayerCultureColorInfo3 = multiplayerBattleColors2.AttackerColors;
					IL_02BE:
					multiplayerCultureColorInfo4 = multiplayerCultureColorInfo3;
				}
				else
				{
					multiplayerCultureColorInfo4 = multiplayerBattleColors2.DefenderColors;
				}
				MissionScoreboardSideVM missionScoreboardSideVM3 = new MissionScoreboardSideVM(missionScoreboardSide6, new Action<MissionScoreboardPlayerVM>(this.ExecutePopulateActionList), this.IsSingleSide, true, multiplayerCultureColorInfo4);
				this.Sides.Add(missionScoreboardSideVM3);
				this._missionSides.Add(missionScoreboardSide6.Side, missionScoreboardSideVM3);
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600023D RID: 573 RVA: 0x00009738 File Offset: 0x00007938
		private BattleSideEnum AllySide
		{
			get
			{
				BattleSideEnum battleSideEnum = BattleSideEnum.None;
				if (GameNetwork.IsMyPeerReady)
				{
					MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
					if (component != null && component.Team != null)
					{
						battleSideEnum = component.Team.Side;
					}
				}
				return battleSideEnum;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600023E RID: 574 RVA: 0x00009774 File Offset: 0x00007974
		private BattleSideEnum EnemySide
		{
			get
			{
				BattleSideEnum allySide = this.AllySide;
				if (allySide == BattleSideEnum.Defender)
				{
					return BattleSideEnum.Attacker;
				}
				if (allySide == BattleSideEnum.Attacker)
				{
					return BattleSideEnum.Defender;
				}
				Debug.FailedAssert("Ally side must be either Attacker or Defender", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\Scoreboard\\MissionScoreboardVM.cs", "EnemySide", 556);
				return BattleSideEnum.None;
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x000097B0 File Offset: 0x000079B0
		private void OnPeerTeamChanged(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			Team spectatorTeam = this._mission.SpectatorTeam;
			if (previousTeam == spectatorTeam || newTeam == spectatorTeam)
			{
				this.RefreshSpectatorCount();
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x000097D8 File Offset: 0x000079D8
		public void RefreshSpectatorCount()
		{
			Team spectatorTeam = this._mission.SpectatorTeam;
			if (spectatorTeam == null)
			{
				this.ShowSpectatorCount = false;
				return;
			}
			int num = 0;
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				if (networkCommunicator != null && !networkCommunicator.IsServerPeer)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (((component != null) ? component.Team : null) == spectatorTeam)
					{
						num++;
					}
				}
			}
			this.ShowSpectatorCount = num > 0;
			if (this.ShowSpectatorCount)
			{
				TextObject textObject = new TextObject("{=GLkNGEPu}{?COUNT_PLURAL}{COUNT} spectators are{?}{COUNT} spectator is{\\?} watching this game.", null);
				textObject.SetTextVariable("COUNT", num);
				textObject.SetTextVariable("COUNT_PLURAL", (num == 1) ? 0 : 1);
				this.Spectators = textObject.ToString();
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x000098B0 File Offset: 0x00007AB0
		public void ExecuteToggleMute()
		{
			foreach (MissionScoreboardSideVM missionScoreboardSideVM in this.Sides)
			{
				foreach (MissionScoreboardPlayerVM missionScoreboardPlayerVM in missionScoreboardSideVM.Players)
				{
					if (!missionScoreboardPlayerVM.IsMine && missionScoreboardPlayerVM.Peer != null)
					{
						this._chatBox.SetPlayerMuted(missionScoreboardPlayerVM.Peer.Peer.Id, !this._hasMutedAll);
						missionScoreboardPlayerVM.Peer.SetMuted(!this._hasMutedAll);
						missionScoreboardPlayerVM.UpdateIsMuted();
					}
				}
			}
			this._hasMutedAll = !this._hasMutedAll;
			this.UpdateToggleMuteText();
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00009990 File Offset: 0x00007B90
		private void UpdateToggleMuteText()
		{
			if (this._hasMutedAll)
			{
				this.ToggleMuteText = this._unmuteAllText.ToString();
				return;
			}
			this.ToggleMuteText = this._muteAllText.ToString();
		}

		// Token: 0x06000243 RID: 579 RVA: 0x000099C0 File Offset: 0x00007BC0
		private void OnPeerMuteStatusUpdated(MissionPeer peer)
		{
			foreach (MissionScoreboardSideVM missionScoreboardSideVM in this.Sides)
			{
				foreach (MissionScoreboardPlayerVM missionScoreboardPlayerVM in missionScoreboardSideVM.Players)
				{
					if (missionScoreboardPlayerVM.Peer == peer)
					{
						missionScoreboardPlayerVM.UpdateIsMuted();
						break;
					}
				}
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000244 RID: 580 RVA: 0x00009A4C File Offset: 0x00007C4C
		// (set) Token: 0x06000245 RID: 581 RVA: 0x00009A54 File Offset: 0x00007C54
		[DataSourceProperty]
		public MPEndOfBattleVM EndOfBattle
		{
			get
			{
				return this._endOfBattle;
			}
			set
			{
				if (value != this._endOfBattle)
				{
					this._endOfBattle = value;
					base.OnPropertyChangedWithValue<MPEndOfBattleVM>(value, "EndOfBattle");
				}
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00009A72 File Offset: 0x00007C72
		// (set) Token: 0x06000247 RID: 583 RVA: 0x00009A7A File Offset: 0x00007C7A
		[DataSourceProperty]
		public MBBindingList<StringPairItemWithActionVM> PlayerActionList
		{
			get
			{
				return this._playerActionList;
			}
			set
			{
				if (value != this._playerActionList)
				{
					this._playerActionList = value;
					base.OnPropertyChangedWithValue<MBBindingList<StringPairItemWithActionVM>>(value, "PlayerActionList");
				}
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000248 RID: 584 RVA: 0x00009A98 File Offset: 0x00007C98
		// (set) Token: 0x06000249 RID: 585 RVA: 0x00009AA0 File Offset: 0x00007CA0
		[DataSourceProperty]
		public MBBindingList<MissionScoreboardSideVM> Sides
		{
			get
			{
				return this._sides;
			}
			set
			{
				if (value != this._sides)
				{
					this._sides = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionScoreboardSideVM>>(value, "Sides");
				}
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00009ABE File Offset: 0x00007CBE
		// (set) Token: 0x0600024B RID: 587 RVA: 0x00009AC6 File Offset: 0x00007CC6
		[DataSourceProperty]
		public bool IsUpdateOver
		{
			get
			{
				return this._isUpdateOver;
			}
			set
			{
				this._isUpdateOver = value;
				base.OnPropertyChangedWithValue(value, "IsUpdateOver");
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00009ADB File Offset: 0x00007CDB
		// (set) Token: 0x0600024D RID: 589 RVA: 0x00009AE3 File Offset: 0x00007CE3
		[DataSourceProperty]
		public bool IsInitalizationOver
		{
			get
			{
				return this._isInitalizationOver;
			}
			set
			{
				if (value != this._isInitalizationOver)
				{
					this._isInitalizationOver = value;
					base.OnPropertyChangedWithValue(value, "IsInitalizationOver");
				}
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600024E RID: 590 RVA: 0x00009B01 File Offset: 0x00007D01
		// (set) Token: 0x0600024F RID: 591 RVA: 0x00009B09 File Offset: 0x00007D09
		[DataSourceProperty]
		public bool IsMouseEnabled
		{
			get
			{
				return this._isMouseEnabled;
			}
			set
			{
				if (value != this._isMouseEnabled)
				{
					this._isMouseEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsMouseEnabled");
				}
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00009B27 File Offset: 0x00007D27
		// (set) Token: 0x06000251 RID: 593 RVA: 0x00009B2F File Offset: 0x00007D2F
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
				}
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000252 RID: 594 RVA: 0x00009B4D File Offset: 0x00007D4D
		// (set) Token: 0x06000253 RID: 595 RVA: 0x00009B55 File Offset: 0x00007D55
		[DataSourceProperty]
		public bool IsPlayerActionsActive
		{
			get
			{
				return this._isPlayerActionsActive;
			}
			set
			{
				if (value != this._isPlayerActionsActive)
				{
					this._isPlayerActionsActive = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerActionsActive");
				}
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000254 RID: 596 RVA: 0x00009B73 File Offset: 0x00007D73
		// (set) Token: 0x06000255 RID: 597 RVA: 0x00009B7B File Offset: 0x00007D7B
		[DataSourceProperty]
		public string Spectators
		{
			get
			{
				return this._spectators;
			}
			set
			{
				if (value != this._spectators)
				{
					this._spectators = value;
					base.OnPropertyChangedWithValue<string>(value, "Spectators");
				}
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000256 RID: 598 RVA: 0x00009B9E File Offset: 0x00007D9E
		// (set) Token: 0x06000257 RID: 599 RVA: 0x00009BA6 File Offset: 0x00007DA6
		[DataSourceProperty]
		public bool ShowSpectatorCount
		{
			get
			{
				return this._showSpectatorCount;
			}
			set
			{
				if (value != this._showSpectatorCount)
				{
					this._showSpectatorCount = value;
					base.OnPropertyChangedWithValue(value, "ShowSpectatorCount");
				}
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000258 RID: 600 RVA: 0x00009BC4 File Offset: 0x00007DC4
		// (set) Token: 0x06000259 RID: 601 RVA: 0x00009BCC File Offset: 0x00007DCC
		[DataSourceProperty]
		public InputKeyItemVM ShowMouseKey
		{
			get
			{
				return this._showMouseKey;
			}
			set
			{
				if (value != this._showMouseKey)
				{
					this._showMouseKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ShowMouseKey");
				}
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600025A RID: 602 RVA: 0x00009BEA File Offset: 0x00007DEA
		// (set) Token: 0x0600025B RID: 603 RVA: 0x00009BF2 File Offset: 0x00007DF2
		[DataSourceProperty]
		public string MissionName
		{
			get
			{
				return this._missionName;
			}
			set
			{
				if (value != this._missionName)
				{
					this._missionName = value;
					base.OnPropertyChangedWithValue<string>(value, "MissionName");
				}
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600025C RID: 604 RVA: 0x00009C15 File Offset: 0x00007E15
		// (set) Token: 0x0600025D RID: 605 RVA: 0x00009C1D File Offset: 0x00007E1D
		[DataSourceProperty]
		public string GameModeText
		{
			get
			{
				return this._gameModeText;
			}
			set
			{
				if (value != this._gameModeText)
				{
					this._gameModeText = value;
					base.OnPropertyChangedWithValue<string>(value, "GameModeText");
				}
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600025E RID: 606 RVA: 0x00009C40 File Offset: 0x00007E40
		// (set) Token: 0x0600025F RID: 607 RVA: 0x00009C48 File Offset: 0x00007E48
		[DataSourceProperty]
		public string MapName
		{
			get
			{
				return this._mapName;
			}
			set
			{
				if (value != this._mapName)
				{
					this._mapName = value;
					base.OnPropertyChangedWithValue<string>(value, "MapName");
				}
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000260 RID: 608 RVA: 0x00009C6B File Offset: 0x00007E6B
		// (set) Token: 0x06000261 RID: 609 RVA: 0x00009C73 File Offset: 0x00007E73
		[DataSourceProperty]
		public string ServerName
		{
			get
			{
				return this._serverName;
			}
			set
			{
				if (value != this._serverName)
				{
					this._serverName = value;
					base.OnPropertyChangedWithValue<string>(value, "ServerName");
				}
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000262 RID: 610 RVA: 0x00009C96 File Offset: 0x00007E96
		// (set) Token: 0x06000263 RID: 611 RVA: 0x00009C9E File Offset: 0x00007E9E
		[DataSourceProperty]
		public bool IsBotsEnabled
		{
			get
			{
				return this._isBotsEnabled;
			}
			set
			{
				if (value != this._isBotsEnabled)
				{
					this._isBotsEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsBotsEnabled");
				}
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000264 RID: 612 RVA: 0x00009CBC File Offset: 0x00007EBC
		// (set) Token: 0x06000265 RID: 613 RVA: 0x00009CC4 File Offset: 0x00007EC4
		[DataSourceProperty]
		public bool IsSingleSide
		{
			get
			{
				return this._isSingleSide;
			}
			set
			{
				if (value != this._isSingleSide)
				{
					this._isSingleSide = value;
					base.OnPropertyChangedWithValue(value, "IsSingleSide");
				}
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000266 RID: 614 RVA: 0x00009CE2 File Offset: 0x00007EE2
		// (set) Token: 0x06000267 RID: 615 RVA: 0x00009CEA File Offset: 0x00007EEA
		[DataSourceProperty]
		public string ToggleMuteText
		{
			get
			{
				return this._toggleMuteText;
			}
			set
			{
				if (value != this._toggleMuteText)
				{
					this._toggleMuteText = value;
					base.OnPropertyChangedWithValue<string>(value, "ToggleMuteText");
				}
			}
		}

		// Token: 0x04000123 RID: 291
		private const float AttributeRefreshDuration = 1f;

		// Token: 0x04000124 RID: 292
		private ChatBox _chatBox;

		// Token: 0x04000125 RID: 293
		private const float PermissionCheckDuration = 45f;

		// Token: 0x04000126 RID: 294
		private readonly Dictionary<BattleSideEnum, MissionScoreboardSideVM> _missionSides;

		// Token: 0x04000127 RID: 295
		private readonly MissionScoreboardComponent _missionScoreboardComponent;

		// Token: 0x04000128 RID: 296
		private readonly MultiplayerPollComponent _missionPollComponent;

		// Token: 0x04000129 RID: 297
		private VoiceChatHandler _voiceChatHandler;

		// Token: 0x0400012A RID: 298
		private MultiplayerPermissionHandler _permissionHandler;

		// Token: 0x0400012B RID: 299
		private readonly Mission _mission;

		// Token: 0x0400012C RID: 300
		private float _attributeRefreshTimeElapsed;

		// Token: 0x0400012D RID: 301
		private bool _hasMutedAll;

		// Token: 0x0400012E RID: 302
		private bool _canStartKickPolls;

		// Token: 0x0400012F RID: 303
		private TextObject _muteAllText = new TextObject("{=AZSbwcG5}Mute All", null);

		// Token: 0x04000130 RID: 304
		private TextObject _unmuteAllText = new TextObject("{=SzRVIPeZ}Unmute All", null);

		// Token: 0x04000131 RID: 305
		private bool _isActive;

		// Token: 0x04000132 RID: 306
		private InputKeyItemVM _showMouseKey;

		// Token: 0x04000133 RID: 307
		private MPEndOfBattleVM _endOfBattle;

		// Token: 0x04000134 RID: 308
		private MBBindingList<MissionScoreboardSideVM> _sides;

		// Token: 0x04000135 RID: 309
		private MBBindingList<StringPairItemWithActionVM> _playerActionList;

		// Token: 0x04000136 RID: 310
		private string _spectators;

		// Token: 0x04000137 RID: 311
		private bool _showSpectatorCount;

		// Token: 0x04000138 RID: 312
		private string _missionName;

		// Token: 0x04000139 RID: 313
		private string _gameModeText;

		// Token: 0x0400013A RID: 314
		private string _mapName;

		// Token: 0x0400013B RID: 315
		private string _serverName;

		// Token: 0x0400013C RID: 316
		private bool _isBotsEnabled;

		// Token: 0x0400013D RID: 317
		private bool _isSingleSide;

		// Token: 0x0400013E RID: 318
		private bool _isInitalizationOver;

		// Token: 0x0400013F RID: 319
		private bool _isUpdateOver;

		// Token: 0x04000140 RID: 320
		private bool _isMouseEnabled;

		// Token: 0x04000141 RID: 321
		private bool _isPlayerActionsActive;

		// Token: 0x04000142 RID: 322
		private string _toggleMuteText;
	}
}
