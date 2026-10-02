using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000310 RID: 784
	public class MultiplayerOptions
	{
		// Token: 0x17000862 RID: 2146
		// (get) Token: 0x06002D14 RID: 11540 RVA: 0x000AD71D File Offset: 0x000AB91D
		public static MultiplayerOptions Instance
		{
			get
			{
				MultiplayerOptions multiplayerOptions;
				if ((multiplayerOptions = MultiplayerOptions._instance) == null)
				{
					multiplayerOptions = (MultiplayerOptions._instance = new MultiplayerOptions());
				}
				return multiplayerOptions;
			}
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x000AD734 File Offset: 0x000AB934
		public MultiplayerOptions()
		{
			this._default = new MultiplayerOptions.MultiplayerOptionsContainer();
			this._current = new MultiplayerOptions.MultiplayerOptionsContainer();
			this._next = new MultiplayerOptions.MultiplayerOptionsContainer();
			for (MultiplayerOptions.OptionType optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
			{
				this._current.CreateOption(optionType);
				this._default.CreateOption(optionType);
			}
			MBReadOnlyList<MultiplayerGameTypeInfo> multiplayerGameTypes = Module.CurrentModule.GetMultiplayerGameTypes();
			if (multiplayerGameTypes.Count > 0)
			{
				MultiplayerGameTypeInfo multiplayerGameTypeInfo = multiplayerGameTypes[0];
				this._current.UpdateOptionValue(MultiplayerOptions.OptionType.GameType, multiplayerGameTypeInfo.GameType);
				this._current.UpdateOptionValue(MultiplayerOptions.OptionType.PremadeMatchGameMode, multiplayerGameTypes.First<MultiplayerGameTypeInfo>((MultiplayerGameTypeInfo info) => info.GameType == "Skirmish").GameType);
				this._current.UpdateOptionValue(MultiplayerOptions.OptionType.Map, multiplayerGameTypeInfo.Scenes.FirstOrDefault<string>());
			}
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.CultureTeam1, MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>()[0].StringId);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.CultureTeam2, MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>()[2].StringId);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.MaxNumberOfPlayers, 120);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.MaxSpectatorCount, 0);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.MinNumberOfPlayersForMatchStart, 1);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds, 300);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.MapTimeLimit, 30);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.RoundTimeLimit, 120);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.RoundPreparationTimeLimit, 10);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.RoundTotal, 1);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.RespawnPeriodTeam1, 3);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.RespawnPeriodTeam2, 3);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.MinScoreToWinMatch, 120000);
			this._current.UpdateOptionValue(MultiplayerOptions.OptionType.AutoTeamBalanceThreshold, 0);
			this._current.CopyAllValuesTo(this._next);
			this._current.CopyAllValuesTo(this._default);
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x000AD925 File Offset: 0x000ABB25
		public static void Release()
		{
			MultiplayerOptions._instance = null;
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x000AD92D File Offset: 0x000ABB2D
		public MultiplayerOptions.MultiplayerOption GetOptionFromOptionType(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode mode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
		{
			return this.GetContainer(mode).GetOptionFromOptionType(optionType);
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x000AD93C File Offset: 0x000ABB3C
		public void OnGameTypeChanged(MultiplayerOptions.MultiplayerOptionsAccessMode mode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
		{
			string text = "";
			if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.Default)
			{
				text = MultiplayerOptions.OptionType.GameType.GetStrValue(mode);
			}
			else if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.PremadeMatch)
			{
				text = MultiplayerOptions.OptionType.PremadeMatchGameMode.GetStrValue(mode);
			}
			MultiplayerOptions.OptionType.DisableInactivityKick.SetValue(false, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			if (!(text == "TeamDeathmatch"))
			{
				if (!(text == "Duel"))
				{
					if (!(text == "Siege"))
					{
						if (!(text == "Captain"))
						{
							if (!(text == "Skirmish"))
							{
								if (text == "Battle")
								{
									this.InitializeForBattle(mode);
								}
							}
							else
							{
								this.InitializeForSkirmish(mode);
							}
						}
						else
						{
							this.InitializeForCaptain(mode);
						}
					}
					else
					{
						this.InitializeForSiege(mode);
					}
				}
				else
				{
					this.InitializeForDuel(mode);
				}
			}
			else
			{
				this.InitializeForTeamDeathmatch(mode);
			}
			MBList<string> mapList = this.GetMapList();
			if (mapList.Count > 0)
			{
				MultiplayerOptions.OptionType.Map.SetValue(mapList[0], MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x000ADA20 File Offset: 0x000ABC20
		public void InitializeNextAndDefaultOptionContainers()
		{
			this._current.CopyAllValuesTo(this._next);
			this._current.CopyAllValuesTo(this._default);
		}

		// Token: 0x06002D1A RID: 11546 RVA: 0x000ADA44 File Offset: 0x000ABC44
		private void InitializeForTeamDeathmatch(MultiplayerOptions.MultiplayerOptionsAccessMode mode)
		{
			string text = "TeamDeathmatch";
			MultiplayerOptions.OptionType.MaxNumberOfPlayers.SetValue(this.GetNumberOfPlayersForGameMode(text), mode);
			MultiplayerOptions.OptionType.NumberOfBotsPerFormation.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.SpectatorCamera.SetValue(0, mode);
			MultiplayerOptions.OptionType.MapTimeLimit.SetValue(this.GetRoundTimeLimitInMinutesForGameMode(text), mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam1.SetValue(3, mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam2.SetValue(3, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.SetValue(0, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam2.SetValue(0, mode);
			MultiplayerOptions.OptionType.MinScoreToWinMatch.SetValue(120000, mode);
			MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.SetValue(2, mode);
			this.InitializeFormationMarkerDefaults(mode);
		}

		// Token: 0x06002D1B RID: 11547 RVA: 0x000ADAEC File Offset: 0x000ABCEC
		private void InitializeForDuel(MultiplayerOptions.MultiplayerOptionsAccessMode mode)
		{
			string text = "Duel";
			MultiplayerOptions.OptionType.MaxNumberOfPlayers.SetValue(this.GetNumberOfPlayersForGameMode(text), mode);
			MultiplayerOptions.OptionType.NumberOfBotsPerFormation.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.SpectatorCamera.SetValue(0, mode);
			MultiplayerOptions.OptionType.MapTimeLimit.SetValue(MultiplayerOptions.OptionType.MapTimeLimit.GetMaximumValue(), mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam1.SetValue(3, mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam2.SetValue(3, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.SetValue(0, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam2.SetValue(0, mode);
			MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.SetValue(0, mode);
			MultiplayerOptions.OptionType.MinScoreToWinDuel.SetValue(3, mode);
			this.InitializeFormationMarkerDefaults(mode);
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x000ADB90 File Offset: 0x000ABD90
		private void InitializeForSiege(MultiplayerOptions.MultiplayerOptionsAccessMode mode)
		{
			string text = "Siege";
			MultiplayerOptions.OptionType.MaxNumberOfPlayers.SetValue(this.GetNumberOfPlayersForGameMode(text), mode);
			MultiplayerOptions.OptionType.NumberOfBotsPerFormation.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.SpectatorCamera.SetValue(0, mode);
			MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds.SetValue(180, mode);
			MultiplayerOptions.OptionType.MapTimeLimit.SetValue(this.GetRoundTimeLimitInMinutesForGameMode(text), mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam1.SetValue(3, mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam2.SetValue(12, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.SetValue(30, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam2.SetValue(0, mode);
			MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.SetValue(2, mode);
			this.InitializeFormationMarkerDefaults(mode);
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x000ADC3C File Offset: 0x000ABE3C
		private void InitializeForCaptain(MultiplayerOptions.MultiplayerOptionsAccessMode mode)
		{
			string text = "Captain";
			MultiplayerOptions.OptionType.MaxNumberOfPlayers.SetValue(this.GetNumberOfPlayersForGameMode(text), mode);
			MultiplayerOptions.OptionType.NumberOfBotsPerFormation.SetValue(25, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.SpectatorCamera.SetValue(6, mode);
			MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds.SetValue(300, mode);
			MultiplayerOptions.OptionType.MapTimeLimit.SetValue(5, mode);
			MultiplayerOptions.OptionType.RoundTimeLimit.SetValue(this.GetRoundTimeLimitInMinutesForGameMode(text) * 60, mode);
			MultiplayerOptions.OptionType.RoundPreparationTimeLimit.SetValue(20, mode);
			MultiplayerOptions.OptionType.RoundTotal.SetValue(this.GetRoundCountForGameMode(text), mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam1.SetValue(3, mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam2.SetValue(3, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.SetValue(0, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam2.SetValue(0, mode);
			MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.SetValue(2, mode);
			MultiplayerOptions.OptionType.AllowPollsToKickPlayers.SetValue(true, mode);
			MultiplayerOptions.OptionType.SingleSpawn.SetValue(true, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			MultiplayerOptions.OptionType.FormationTargetingVisibilityMode.SetValue(1, mode);
			MultiplayerOptions.OptionType.FormationTargetingVisibilityThreshold.SetValue(20, mode);
			MultiplayerOptions.OptionType.FormationTargetingVisibilityAppliesAtCloseRange.SetValue(false, mode);
			this.InitializeFormationMarkerDefaults(mode);
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x000ADD39 File Offset: 0x000ABF39
		private void InitializeFormationMarkerDefaults(MultiplayerOptions.MultiplayerOptionsAccessMode mode)
		{
			MultiplayerOptions.OptionType.FormationMarkerFarDistanceCutoff.SetValue(-1, mode);
			MultiplayerOptions.OptionType.FormationMarkerFarAlphaTarget.SetValue(-1, mode);
			MultiplayerOptions.OptionType.FormationMarkerAlwaysOnDistance.SetValue(-1, mode);
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x000ADD58 File Offset: 0x000ABF58
		private void InitializeForSkirmish(MultiplayerOptions.MultiplayerOptionsAccessMode mode)
		{
			string text = "Skirmish";
			MultiplayerOptions.OptionType.MaxNumberOfPlayers.SetValue(this.GetNumberOfPlayersForGameMode(text), mode);
			MultiplayerOptions.OptionType.NumberOfBotsPerFormation.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.SpectatorCamera.SetValue(6, mode);
			MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds.SetValue(300, mode);
			MultiplayerOptions.OptionType.MapTimeLimit.SetValue(5, mode);
			MultiplayerOptions.OptionType.RoundTimeLimit.SetValue(this.GetRoundTimeLimitInMinutesForGameMode(text) * 60, mode);
			MultiplayerOptions.OptionType.RoundPreparationTimeLimit.SetValue(20, mode);
			MultiplayerOptions.OptionType.RoundTotal.SetValue(this.GetRoundCountForGameMode(text), mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam1.SetValue(3, mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam2.SetValue(3, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.SetValue(0, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam2.SetValue(0, mode);
			MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.SetValue(2, mode);
			MultiplayerOptions.OptionType.AllowPollsToKickPlayers.SetValue(true, mode);
			this.InitializeFormationMarkerDefaults(mode);
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x000ADE30 File Offset: 0x000AC030
		private void InitializeForBattle(MultiplayerOptions.MultiplayerOptionsAccessMode mode)
		{
			string text = "Battle";
			MultiplayerOptions.OptionType.MaxNumberOfPlayers.SetValue(this.GetNumberOfPlayersForGameMode(text), mode);
			MultiplayerOptions.OptionType.NumberOfBotsPerFormation.SetValue(0, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeSelfPercent.SetValue(25, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageMeleeFriendPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedSelfPercent.SetValue(25, mode);
			MultiplayerOptions.OptionType.FriendlyFireDamageRangedFriendPercent.SetValue(50, mode);
			MultiplayerOptions.OptionType.SpectatorCamera.SetValue(6, mode);
			MultiplayerOptions.OptionType.WarmupTimeLimitInSeconds.SetValue(300, mode);
			MultiplayerOptions.OptionType.MapTimeLimit.SetValue(90, mode);
			MultiplayerOptions.OptionType.RoundTimeLimit.SetValue(this.GetRoundTimeLimitInMinutesForGameMode(text) * 60, mode);
			MultiplayerOptions.OptionType.RoundPreparationTimeLimit.SetValue(20, mode);
			MultiplayerOptions.OptionType.RoundTotal.SetValue(this.GetRoundCountForGameMode(text), mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam1.SetValue(3, mode);
			MultiplayerOptions.OptionType.RespawnPeriodTeam2.SetValue(3, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam1.SetValue(0, mode);
			MultiplayerOptions.OptionType.GoldGainChangePercentageTeam2.SetValue(0, mode);
			MultiplayerOptions.OptionType.AutoTeamBalanceThreshold.SetValue(2, mode);
			MultiplayerOptions.OptionType.SingleSpawn.SetValue(true, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			this.InitializeFormationMarkerDefaults(mode);
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x000ADF0C File Offset: 0x000AC10C
		public int GetNumberOfPlayersForGameMode(string gameModeID)
		{
			if (gameModeID == "TeamDeathmatch" || gameModeID == "Siege" || gameModeID == "Battle")
			{
				return 120;
			}
			if (gameModeID == "Captain" || gameModeID == "Skirmish")
			{
				return 12;
			}
			if (!(gameModeID == "Duel"))
			{
				return 0;
			}
			return 32;
		}

		// Token: 0x06002D22 RID: 11554 RVA: 0x000ADF74 File Offset: 0x000AC174
		public int GetRoundCountForGameMode(string gameModeID)
		{
			if (gameModeID == "TeamDeathmatch" || gameModeID == "Siege" || gameModeID == "Duel")
			{
				return 1;
			}
			if (gameModeID == "Battle")
			{
				return 9;
			}
			if (!(gameModeID == "Captain") && !(gameModeID == "Skirmish"))
			{
				return 0;
			}
			return 5;
		}

		// Token: 0x06002D23 RID: 11555 RVA: 0x000ADFDC File Offset: 0x000AC1DC
		public int GetRoundTimeLimitInMinutesForGameMode(string gameModeID)
		{
			if (gameModeID == "TeamDeathmatch" || gameModeID == "Siege" || gameModeID == "Duel")
			{
				return 30;
			}
			if (gameModeID == "Battle")
			{
				return 20;
			}
			if (gameModeID == "Captain")
			{
				return 10;
			}
			if (!(gameModeID == "Skirmish"))
			{
				return 0;
			}
			return 7;
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x000AE048 File Offset: 0x000AC248
		public void InitializeFromCommandList(List<string> arguments)
		{
			foreach (string text in arguments)
			{
				GameNetwork.HandleConsoleCommand(text);
			}
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x000AE094 File Offset: 0x000AC294
		public void ResetDefaultsToCurrent()
		{
			this._current.CopyAllValuesTo(this._default);
		}

		// Token: 0x06002D26 RID: 11558 RVA: 0x000AE0A8 File Offset: 0x000AC2A8
		public List<string> GetMultiplayerOptionsTextList(MultiplayerOptions.OptionType optionType)
		{
			List<string> list = new List<string>();
			string text = new TextObject("{=vBkrw5VV}Random", null).ToString();
			string text2 = "-- " + text + " --";
			switch (optionType)
			{
			case MultiplayerOptions.OptionType.PremadeMatchGameMode:
				return (from q in Module.CurrentModule.GetMultiplayerGameTypes()
					where q.GameType == "Skirmish" || q.GameType == "Captain"
					select GameTexts.FindText("str_multiplayer_official_game_type_name", q.GameType).ToString()).ToList<string>();
			case MultiplayerOptions.OptionType.GameType:
				return (from q in Module.CurrentModule.GetMultiplayerGameTypes()
					select GameTexts.FindText("str_multiplayer_official_game_type_name", q.GameType).ToString()).ToList<string>();
			case MultiplayerOptions.OptionType.PremadeGameType:
				break;
			case MultiplayerOptions.OptionType.Map:
			{
				List<string> list2 = new List<string>();
				if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.Default)
				{
					list2 = MultiplayerGameTypes.GetGameTypeInfo(MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)).Scenes.ToList<string>();
				}
				else if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.PremadeMatch)
				{
					list2 = this.GetAvailableClanMatchScenes();
					list.Insert(0, text2);
				}
				using (List<string>.Enumerator enumerator = list2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text3 = enumerator.Current;
						TextObject textObject;
						string text4;
						if (GameTexts.TryGetText("str_multiplayer_scene_name", out textObject, text3))
						{
							text4 = textObject.ToString();
						}
						else
						{
							text4 = text3;
						}
						list.Add(text4);
					}
					return list;
				}
				break;
			}
			case MultiplayerOptions.OptionType.CultureTeam1:
			case MultiplayerOptions.OptionType.CultureTeam2:
				list = (from c in MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>()
					where c.IsMainCulture
					select c into x
					select MultiplayerOptions.GetLocalizedCultureNameFromStringID(x.StringId)).ToList<string>();
				if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.PremadeMatch)
				{
					list.Insert(0, text2);
					return list;
				}
				return list;
			default:
				if (optionType != MultiplayerOptions.OptionType.SpectatorCamera)
				{
					return this.GetMultiplayerOptionsList(optionType);
				}
				return new List<string>
				{
					GameTexts.FindText("str_multiplayer_spectator_camera_type", SpectatorCameraTypes.LockToAnyAgent.ToString()).ToString(),
					GameTexts.FindText("str_multiplayer_spectator_camera_type", SpectatorCameraTypes.LockToAnyPlayer.ToString()).ToString(),
					GameTexts.FindText("str_multiplayer_spectator_camera_type", SpectatorCameraTypes.LockToTeamMembers.ToString()).ToString(),
					GameTexts.FindText("str_multiplayer_spectator_camera_type", SpectatorCameraTypes.LockToTeamMembersView.ToString()).ToString()
				};
			}
			list = new List<string>
			{
				new TextObject("{=H5tiRTya}Practice", null).ToString(),
				new TextObject("{=YNkPy4ta}Clan Match", null).ToString()
			};
			return list;
		}

		// Token: 0x06002D27 RID: 11559 RVA: 0x000AE390 File Offset: 0x000AC590
		public List<string> GetMultiplayerOptionsList(MultiplayerOptions.OptionType optionType)
		{
			List<string> list = new List<string>();
			switch (optionType)
			{
			case MultiplayerOptions.OptionType.PremadeMatchGameMode:
				list = (from q in Module.CurrentModule.GetMultiplayerGameTypes()
					select q.GameType).ToList<string>();
				list.Remove("TeamDeathmatch");
				list.Remove("Duel");
				list.Remove("Siege");
				break;
			case MultiplayerOptions.OptionType.GameType:
				list = (from q in Module.CurrentModule.GetMultiplayerGameTypes()
					select q.GameType).ToList<string>();
				break;
			case MultiplayerOptions.OptionType.PremadeGameType:
				list = new List<string>
				{
					PremadeGameType.Practice.ToString(),
					PremadeGameType.Clan.ToString()
				};
				break;
			case MultiplayerOptions.OptionType.Map:
				if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.Default)
				{
					list = MultiplayerGameTypes.GetGameTypeInfo(MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)).Scenes.ToList<string>();
				}
				else if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.PremadeMatch)
				{
					MultiplayerOptions.OptionType.PremadeMatchGameMode.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
					list = this.GetAvailableClanMatchScenes();
					list.Insert(0, "RandomSelection");
				}
				break;
			case MultiplayerOptions.OptionType.CultureTeam1:
			case MultiplayerOptions.OptionType.CultureTeam2:
				list = (from c in MBObjectManager.Instance.GetObjectTypeList<BasicCultureObject>()
					where c.IsMainCulture
					select c into x
					select x.StringId).ToList<string>();
				if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.PremadeMatch)
				{
					list.Insert(0, Parameters.RandomSelectionString);
				}
				break;
			default:
				if (optionType == MultiplayerOptions.OptionType.SpectatorCamera)
				{
					list = new List<string>
					{
						SpectatorCameraTypes.LockToAnyAgent.ToString(),
						SpectatorCameraTypes.LockToAnyPlayer.ToString(),
						SpectatorCameraTypes.LockToTeamMembers.ToString(),
						SpectatorCameraTypes.LockToTeamMembersView.ToString()
					};
				}
				break;
			}
			return list;
		}

		// Token: 0x06002D28 RID: 11560 RVA: 0x000AE5B8 File Offset: 0x000AC7B8
		private List<string> GetAvailableClanMatchScenes()
		{
			string[] array = new string[0];
			string[] array2;
			if (NetworkMain.GameClient.AvailableScenes.ScenesByGameTypes.TryGetValue(MultiplayerOptions.OptionType.PremadeMatchGameMode.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions), out array2))
			{
				array = array2;
			}
			return array.ToList<string>();
		}

		// Token: 0x06002D29 RID: 11561 RVA: 0x000AE5F4 File Offset: 0x000AC7F4
		private MultiplayerOptions.MultiplayerOptionsContainer GetContainer(MultiplayerOptions.MultiplayerOptionsAccessMode mode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
		{
			switch (mode)
			{
			case MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions:
				return this._default;
			case MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions:
				return this._current;
			case MultiplayerOptions.MultiplayerOptionsAccessMode.NextMapOptions:
				return this._next;
			default:
				return null;
			}
		}

		// Token: 0x06002D2A RID: 11562 RVA: 0x000AE620 File Offset: 0x000AC820
		public void InitializeAllOptionsFromNext()
		{
			this._next.CopyAllValuesTo(this._current);
			this.UpdateMbMultiplayerData(this._current);
		}

		// Token: 0x06002D2B RID: 11563 RVA: 0x000AE640 File Offset: 0x000AC840
		private void UpdateMbMultiplayerData(MultiplayerOptions.MultiplayerOptionsContainer container)
		{
			container.GetOptionFromOptionType(MultiplayerOptions.OptionType.ServerName).GetValue(out MBMultiplayerData.ServerName);
			if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.Default)
			{
				container.GetOptionFromOptionType(MultiplayerOptions.OptionType.GameType).GetValue(out MBMultiplayerData.GameType);
			}
			else if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.PremadeMatch)
			{
				container.GetOptionFromOptionType(MultiplayerOptions.OptionType.PremadeMatchGameMode).GetValue(out MBMultiplayerData.GameType);
			}
			container.GetOptionFromOptionType(MultiplayerOptions.OptionType.Map).GetValue(out MBMultiplayerData.Map);
			container.GetOptionFromOptionType(MultiplayerOptions.OptionType.MaxNumberOfPlayers).GetValue(out MBMultiplayerData.PlayerCountLimit);
		}

		// Token: 0x06002D2C RID: 11564 RVA: 0x000AE6BC File Offset: 0x000AC8BC
		public MBList<string> GetMapList()
		{
			MultiplayerGameTypeInfo multiplayerGameTypeInfo = null;
			if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.Default)
			{
				multiplayerGameTypeInfo = MultiplayerGameTypes.GetGameTypeInfo(MultiplayerOptions.OptionType.GameType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			}
			else if (this.CurrentOptionsCategory == MultiplayerOptions.OptionsCategory.PremadeMatch)
			{
				multiplayerGameTypeInfo = MultiplayerGameTypes.GetGameTypeInfo(MultiplayerOptions.OptionType.PremadeMatchGameMode.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			}
			MBList<string> mblist = new MBList<string>();
			if (multiplayerGameTypeInfo.Scenes.Count > 0)
			{
				mblist.Add(multiplayerGameTypeInfo.Scenes[0]);
				MultiplayerOptions.OptionType.Map.SetValue(mblist[0], MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
			return mblist;
		}

		// Token: 0x06002D2D RID: 11565 RVA: 0x000AE730 File Offset: 0x000AC930
		public string GetValueTextForOptionWithMultipleSelection(MultiplayerOptions.OptionType optionType)
		{
			MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
			MultiplayerOptions.OptionValueType optionValueType = optionProperty.OptionValueType;
			if (optionValueType == MultiplayerOptions.OptionValueType.Enum)
			{
				return Enum.ToObject(optionProperty.EnumType, optionType.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)).ToString();
			}
			if (optionValueType != MultiplayerOptions.OptionValueType.String)
			{
				return null;
			}
			return optionType.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x000AE778 File Offset: 0x000AC978
		public void SetValueForOptionWithMultipleSelectionFromText(MultiplayerOptions.OptionType optionType, string value)
		{
			MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
			MultiplayerOptions.OptionValueType optionValueType = optionProperty.OptionValueType;
			if (optionValueType != MultiplayerOptions.OptionValueType.Enum)
			{
				if (optionValueType == MultiplayerOptions.OptionValueType.String)
				{
					optionType.SetValue(value, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				}
			}
			else
			{
				optionType.SetValue((int)Enum.Parse(optionProperty.EnumType, value), MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
			if (optionType == MultiplayerOptions.OptionType.GameType || optionType == MultiplayerOptions.OptionType.PremadeMatchGameMode)
			{
				this.OnGameTypeChanged(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			}
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x000AE7D0 File Offset: 0x000AC9D0
		private static string GetLocalizedCultureNameFromStringID(string cultureID)
		{
			if (cultureID == "sturgia")
			{
				return new TextObject("{=PjO7oY16}Sturgia", null).ToString();
			}
			if (cultureID == "vlandia")
			{
				return new TextObject("{=FjwRsf1C}Vlandia", null).ToString();
			}
			if (cultureID == "battania")
			{
				return new TextObject("{=0B27RrYJ}Battania", null).ToString();
			}
			if (cultureID == "empire")
			{
				return new TextObject("{=empirefaction}Empire", null).ToString();
			}
			if (cultureID == "khuzait")
			{
				return new TextObject("{=sZLd6VHi}Khuzait", null).ToString();
			}
			if (!(cultureID == "aserai"))
			{
				Debug.FailedAssert("Unidentified culture id: " + cultureID, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\Gameplay\\MultiplayerOptions.cs", "GetLocalizedCultureNameFromStringID", 1042);
				return "";
			}
			return new TextObject("{=aseraifaction}Aserai", null).ToString();
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x000AE8B8 File Offset: 0x000ACAB8
		public static bool IsSpectatorCameraFreedomAllowed()
		{
			SpectatorCameraTypes intValue = (SpectatorCameraTypes)MultiplayerOptions.OptionType.SpectatorCamera.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			return intValue != SpectatorCameraTypes.LockToMainPlayer && intValue - SpectatorCameraTypes.LockToPlayerFormation > 2;
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x000AE8DC File Offset: 0x000ACADC
		public static bool TryGetOptionTypeFromString(string optionTypeString, out MultiplayerOptions.OptionType optionType, out MultiplayerOptionsProperty optionAttribute)
		{
			optionAttribute = null;
			for (optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
			{
				MultiplayerOptionsProperty optionProperty = optionType.GetOptionProperty();
				if (optionProperty != null && optionType.ToString().Equals(optionTypeString))
				{
					optionAttribute = optionProperty;
					return true;
				}
			}
			return false;
		}

		// Token: 0x040011D2 RID: 4562
		private const int PlayerCountLimitMin = 1;

		// Token: 0x040011D3 RID: 4563
		private const int PlayerCountLimitMax = 1023;

		// Token: 0x040011D4 RID: 4564
		private const int SpectatorCountLimitMin = 0;

		// Token: 0x040011D5 RID: 4565
		private const int SpectatorCountLimitMax = 1023;

		// Token: 0x040011D6 RID: 4566
		private const int PlayerCountLimitForMatchStartMin = 0;

		// Token: 0x040011D7 RID: 4567
		private const int PlayerCountLimitForMatchStartMax = 20;

		// Token: 0x040011D8 RID: 4568
		private const int MapTimeLimitMin = 1;

		// Token: 0x040011D9 RID: 4569
		private const int MapTimeLimitMax = 60;

		// Token: 0x040011DA RID: 4570
		private const int WarmupTimeLimitMin = 60;

		// Token: 0x040011DB RID: 4571
		private const int WarmupTimeLimitMax = 3600;

		// Token: 0x040011DC RID: 4572
		private const int RoundLimitMin = 1;

		// Token: 0x040011DD RID: 4573
		private const int RoundLimitMax = 99;

		// Token: 0x040011DE RID: 4574
		private const int RoundTimeLimitMin = 60;

		// Token: 0x040011DF RID: 4575
		private const int RoundTimeLimitMax = 3600;

		// Token: 0x040011E0 RID: 4576
		private const int RoundPreparationTimeLimitMin = 2;

		// Token: 0x040011E1 RID: 4577
		private const int RoundPreparationTimeLimitMax = 60;

		// Token: 0x040011E2 RID: 4578
		private const int RespawnPeriodMin = 1;

		// Token: 0x040011E3 RID: 4579
		private const int RespawnPeriodMax = 60;

		// Token: 0x040011E4 RID: 4580
		private const int GoldGainChangePercentageMin = -100;

		// Token: 0x040011E5 RID: 4581
		private const int GoldGainChangePercentageMax = 100;

		// Token: 0x040011E6 RID: 4582
		private const int PollAcceptThresholdMin = 0;

		// Token: 0x040011E7 RID: 4583
		private const int PollAcceptThresholdMax = 10;

		// Token: 0x040011E8 RID: 4584
		private const int BotsPerTeamLimitMin = 0;

		// Token: 0x040011E9 RID: 4585
		private const int BotsPerTeamLimitMax = 510;

		// Token: 0x040011EA RID: 4586
		private const int BotsPerFormationLimitMin = 0;

		// Token: 0x040011EB RID: 4587
		private const int BotsPerFormationLimitMax = 100;

		// Token: 0x040011EC RID: 4588
		private const int FormationTargetingVisibilityThresholdMin = 0;

		// Token: 0x040011ED RID: 4589
		private const int FormationTargetingVisibilityThresholdMax = 100;

		// Token: 0x040011EE RID: 4590
		public const int FormationMarkerUseDefault = -1;

		// Token: 0x040011EF RID: 4591
		private const int FormationMarkerFarDistanceCutoffMin = -1;

		// Token: 0x040011F0 RID: 4592
		private const int FormationMarkerFarDistanceCutoffMax = 1000;

		// Token: 0x040011F1 RID: 4593
		private const int FormationMarkerFarAlphaTargetMin = -1;

		// Token: 0x040011F2 RID: 4594
		private const int FormationMarkerFarAlphaTargetMax = 100;

		// Token: 0x040011F3 RID: 4595
		private const int FormationMarkerAlwaysOnDistanceMin = -1;

		// Token: 0x040011F4 RID: 4596
		private const int FormationMarkerAlwaysOnDistanceMax = 1000;

		// Token: 0x040011F5 RID: 4597
		private const int FriendlyFireDamagePercentMin = 0;

		// Token: 0x040011F6 RID: 4598
		private const int FriendlyFireDamagePercentMax = 2000;

		// Token: 0x040011F7 RID: 4599
		private const int GameDefinitionIdMin = -2147483648;

		// Token: 0x040011F8 RID: 4600
		private const int GameDefinitionIdMax = 2147483647;

		// Token: 0x040011F9 RID: 4601
		private const int MaxScoreToEndDuel = 7;

		// Token: 0x040011FA RID: 4602
		private static MultiplayerOptions _instance;

		// Token: 0x040011FB RID: 4603
		private readonly MultiplayerOptions.MultiplayerOptionsContainer _default;

		// Token: 0x040011FC RID: 4604
		private readonly MultiplayerOptions.MultiplayerOptionsContainer _current;

		// Token: 0x040011FD RID: 4605
		private readonly MultiplayerOptions.MultiplayerOptionsContainer _next;

		// Token: 0x040011FE RID: 4606
		public MultiplayerOptions.OptionsCategory CurrentOptionsCategory;

		// Token: 0x020005FA RID: 1530
		public enum FormationTargetingVisibilityModes
		{
			// Token: 0x04002047 RID: 8263
			Disabled,
			// Token: 0x04002048 RID: 8264
			Percentage,
			// Token: 0x04002049 RID: 8265
			AbsoluteCount
		}

		// Token: 0x020005FB RID: 1531
		public enum MultiplayerOptionsAccessMode
		{
			// Token: 0x0400204B RID: 8267
			DefaultMapOptions,
			// Token: 0x0400204C RID: 8268
			CurrentMapOptions,
			// Token: 0x0400204D RID: 8269
			NextMapOptions,
			// Token: 0x0400204E RID: 8270
			NumAccessModes
		}

		// Token: 0x020005FC RID: 1532
		public enum OptionValueType
		{
			// Token: 0x04002050 RID: 8272
			Bool,
			// Token: 0x04002051 RID: 8273
			Integer,
			// Token: 0x04002052 RID: 8274
			Enum,
			// Token: 0x04002053 RID: 8275
			String
		}

		// Token: 0x020005FD RID: 1533
		public enum OptionType
		{
			// Token: 0x04002055 RID: 8277
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Changes the name of the server in the server list", 0, 0, null, false, null)]
			ServerName,
			// Token: 0x04002056 RID: 8278
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Welcome messages which is shown to all players when they enter the server.", 0, 0, null, false, null)]
			WelcomeMessage,
			// Token: 0x04002057 RID: 8279
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.Never, "Sets a password that clients have to enter before connecting to the server.", 0, 0, null, false, null)]
			GamePassword,
			// Token: 0x04002058 RID: 8280
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.Never, "Sets a password that allows players access to admin tools during the game.", 0, 0, null, false, null)]
			AdminPassword,
			// Token: 0x04002059 RID: 8281
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.Never, "Sets a password that allows players to join as spectators for detailed spectation of the game.", 0, 0, null, false, null)]
			SpectatorPassword,
			// Token: 0x0400205A RID: 8282
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Never, "Sets ID of the private game definition.", -2147483648, 2147483647, null, false, null)]
			GameDefinitionId,
			// Token: 0x0400205B RID: 8283
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Allow players to start polls to kick other players.", 0, 0, null, false, null)]
			AllowPollsToKickPlayers,
			// Token: 0x0400205C RID: 8284
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Allow players to start polls to ban other players.", 0, 0, null, false, null)]
			AllowPollsToBanPlayers,
			// Token: 0x0400205D RID: 8285
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Allow players to start polls to change the current map.", 0, 0, null, false, null)]
			AllowPollsToChangeMaps,
			// Token: 0x0400205E RID: 8286
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Allow players to use their custom banner.", 0, 0, null, false, null)]
			AllowIndividualBanners,
			// Token: 0x0400205F RID: 8287
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Use animation progress dependent blocking.", 0, 0, null, false, null)]
			UseRealisticBlocking,
			// Token: 0x04002060 RID: 8288
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Changes the game type.", 0, 0, null, true, null)]
			PremadeMatchGameMode,
			// Token: 0x04002061 RID: 8289
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Changes the game type.", 0, 0, null, true, null)]
			GameType,
			// Token: 0x04002062 RID: 8290
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Enum, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Type of the premade game.", 0, 1, null, true, typeof(PremadeGameType))]
			PremadeGameType,
			// Token: 0x04002063 RID: 8291
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Map of the game.", 0, 0, null, true, null)]
			Map,
			// Token: 0x04002064 RID: 8292
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Sets culture for team 1", 0, 0, null, true, null)]
			CultureTeam1,
			// Token: 0x04002065 RID: 8293
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.String, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Sets culture for team 2", 0, 0, null, true, null)]
			CultureTeam2,
			// Token: 0x04002066 RID: 8294
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Set the maximum amount of player allowed on the server.", 1, 1023, null, false, null)]
			MaxNumberOfPlayers,
			// Token: 0x04002067 RID: 8295
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Set the amount of players that are needed to start the first round. If not met, players will just wait.", 0, 20, null, false, null)]
			MinNumberOfPlayersForMatchStart,
			// Token: 0x04002068 RID: 8296
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Amount of bots on team 1", 0, 510, null, false, null)]
			NumberOfBotsTeam1,
			// Token: 0x04002069 RID: 8297
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Amount of bots on team 2", 0, 510, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege", "TeamDeathmatch" }, false, null)]
			NumberOfBotsTeam2,
			// Token: 0x0400206A RID: 8298
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Amount of bots per formation", 0, 100, new string[] { "Captain" }, false, null)]
			NumberOfBotsPerFormation,
			// Token: 0x0400206B RID: 8299
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "A percentage of how much melee damage inflicted upon a friend is dealt back to the inflictor.", 0, 2000, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege", "TeamDeathmatch" }, false, null)]
			FriendlyFireDamageMeleeSelfPercent,
			// Token: 0x0400206C RID: 8300
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "A percentage of how much melee damage inflicted upon a friend is actually dealt.", 0, 2000, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege", "TeamDeathmatch" }, false, null)]
			FriendlyFireDamageMeleeFriendPercent,
			// Token: 0x0400206D RID: 8301
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "A percentage of how much ranged damage inflicted upon a friend is dealt back to the inflictor.", 0, 2000, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege", "TeamDeathmatch" }, false, null)]
			FriendlyFireDamageRangedSelfPercent,
			// Token: 0x0400206E RID: 8302
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "A percentage of how much ranged damage inflicted upon a friend is actually dealt.", 0, 2000, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege", "TeamDeathmatch" }, false, null)]
			FriendlyFireDamageRangedFriendPercent,
			// Token: 0x0400206F RID: 8303
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Enum, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Who can spectators look at, and how.", 0, 8, null, true, typeof(SpectatorCameraTypes))]
			SpectatorCamera,
			// Token: 0x04002070 RID: 8304
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Maximum duration for the warmup. In seconds.", 60, 3600, null, false, null)]
			WarmupTimeLimitInSeconds,
			// Token: 0x04002071 RID: 8305
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Maximum duration for the map. In minutes.", 1, 60, null, false, null)]
			MapTimeLimit,
			// Token: 0x04002072 RID: 8306
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Maximum duration for each round. In seconds.", 60, 3600, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege" }, false, null)]
			RoundTimeLimit,
			// Token: 0x04002073 RID: 8307
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Time available to select class/equipment. In seconds.", 2, 60, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege" }, false, null)]
			RoundPreparationTimeLimit,
			// Token: 0x04002074 RID: 8308
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Maximum amount of rounds before the game ends.", 1, 99, new string[] { "Battle", "NewBattle", "ClassicBattle", "Captain", "Skirmish", "Siege" }, false, null)]
			RoundTotal,
			// Token: 0x04002075 RID: 8309
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Wait time after death, before respawning again. In seconds.", 1, 60, new string[] { "Siege" }, false, null)]
			RespawnPeriodTeam1,
			// Token: 0x04002076 RID: 8310
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Wait time after death, before respawning again. In seconds.", 1, 60, new string[] { "Siege" }, false, null)]
			RespawnPeriodTeam2,
			// Token: 0x04002077 RID: 8311
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Unlimited gold option.", 0, 0, new string[] { "Battle", "Skirmish", "Siege", "TeamDeathmatch" }, false, null)]
			UnlimitedGold,
			// Token: 0x04002078 RID: 8312
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Gold gain multiplier from agent deaths.", -100, 100, new string[] { "Siege", "TeamDeathmatch" }, false, null)]
			GoldGainChangePercentageTeam1,
			// Token: 0x04002079 RID: 8313
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Gold gain multiplier from agent deaths.", -100, 100, new string[] { "Siege", "TeamDeathmatch" }, false, null)]
			GoldGainChangePercentageTeam2,
			// Token: 0x0400207A RID: 8314
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Min score to win match.", 0, 1023000, new string[] { "TeamDeathmatch" }, false, null)]
			MinScoreToWinMatch,
			// Token: 0x0400207B RID: 8315
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Min score to win duel.", 0, 7, new string[] { "Duel" }, false, null)]
			MinScoreToWinDuel,
			// Token: 0x0400207C RID: 8316
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Minimum needed difference in poll results before it is accepted.", 0, 10, null, false, null)]
			PollAcceptThreshold,
			// Token: 0x0400207D RID: 8317
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Maximum player imbalance between team 1 and team 2. Selecting 0 will disable auto team balancing.", 0, 30, null, false, null)]
			AutoTeamBalanceThreshold,
			// Token: 0x0400207E RID: 8318
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Enables mission recording.", 0, 0, null, false, null)]
			EnableMissionRecording,
			// Token: 0x0400207F RID: 8319
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Sets if the game mode uses single spawning.", 0, 0, null, false, null)]
			SingleSpawn,
			// Token: 0x04002080 RID: 8320
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Disables the inactivity kick timer.", 0, 0, null, false, null)]
			DisableInactivityKick,
			// Token: 0x04002081 RID: 8321
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Enables the dedicated streamer overlay (all-players overlay + through-wall silhouettes) for spectators.", 0, 0, null, false, null)]
			StreamerModeEnabled,
			// Token: 0x04002082 RID: 8322
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Allows players to join this server as spectators. Disabled by default.", 0, 0, null, false, null)]
			EnableSpectators,
			// Token: 0x04002083 RID: 8323
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.AtMapLoad, "Set the maximum amount of spectators allowed on the server. 0 means unlimited.", 0, 1023, null, false, null)]
			MaxSpectatorCount,
			// Token: 0x04002084 RID: 8324
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Visibility rule that must be satisfied before an enemy formation can be focused for targeting. 0 = Disabled, 1 = Percentage of visible troops, 2 = Absolute count of visible troops.", 0, 2, new string[] { "Captain" }, false, null)]
			FormationTargetingVisibilityMode,
			// Token: 0x04002085 RID: 8325
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Threshold for the formation targeting visibility rule. In Percentage mode: minimum percent of sampled troops that must be visible (0-100). In AbsoluteCount mode: minimum estimated visible troop count (0-100).", 0, 100, new string[] { "Captain" }, false, null)]
			FormationTargetingVisibilityThreshold,
			// Token: 0x04002086 RID: 8326
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Bool, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "If true, the formation targeting visibility rule also applies to formations within close range (under 10 meters). If false, close-range formations are always focusable.", 0, 0, new string[] { "Captain" }, false, null)]
			FormationTargetingVisibilityAppliesAtCloseRange,
			// Token: 0x04002087 RID: 8327
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Distance (in metres) beyond which the formation marker is drawn at FarAlphaTarget. The marker fades from full alpha at close range to FarAlphaTarget at this distance. -1 uses the default hardcoded value.", -1, 1000, new string[] { "Captain" }, false, null)]
			FormationMarkerFarDistanceCutoff,
			// Token: 0x04002088 RID: 8328
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Alpha of the formation marker at and beyond FarDistanceCutoff, as an integer percent (0-100, where 0 hides the marker completely). -1 uses the default hardcoded value.", -1, 100, new string[] { "Captain" }, false, null)]
			FormationMarkerFarAlphaTarget,
			// Token: 0x04002089 RID: 8329
			[MultiplayerOptionsProperty(MultiplayerOptions.OptionValueType.Integer, MultiplayerOptionsProperty.ReplicationOccurrence.Immediately, "Distance (in metres) within which a visible formation marker is always drawn at full alpha. Beyond it the marker fades toward FarAlphaTarget. -1 uses the default hardcoded value.", -1, 1000, new string[] { "Captain" }, false, null)]
			FormationMarkerAlwaysOnDistance,
			// Token: 0x0400208A RID: 8330
			NumOfSlots
		}

		// Token: 0x020005FE RID: 1534
		public enum OptionsCategory
		{
			// Token: 0x0400208C RID: 8332
			Default,
			// Token: 0x0400208D RID: 8333
			PremadeMatch
		}

		// Token: 0x020005FF RID: 1535
		public class MultiplayerOption
		{
			// Token: 0x06004006 RID: 16390 RVA: 0x000FABD2 File Offset: 0x000F8DD2
			public static MultiplayerOptions.MultiplayerOption CreateMultiplayerOption(MultiplayerOptions.OptionType optionType)
			{
				return new MultiplayerOptions.MultiplayerOption(optionType);
			}

			// Token: 0x06004007 RID: 16391 RVA: 0x000FABDA File Offset: 0x000F8DDA
			public static MultiplayerOptions.MultiplayerOption CopyMultiplayerOption(MultiplayerOptions.MultiplayerOption option)
			{
				return new MultiplayerOptions.MultiplayerOption(option.OptionType)
				{
					_intValue = option._intValue,
					_stringValue = option._stringValue
				};
			}

			// Token: 0x06004008 RID: 16392 RVA: 0x000FAC00 File Offset: 0x000F8E00
			private MultiplayerOption(MultiplayerOptions.OptionType optionType)
			{
				this.OptionType = optionType;
				if (optionType.GetOptionProperty().OptionValueType == MultiplayerOptions.OptionValueType.String)
				{
					this._intValue = MultiplayerOptions.MultiplayerOption.IntegerValue.Invalid;
					this._stringValue = MultiplayerOptions.MultiplayerOption.StringValue.Create();
					return;
				}
				this._intValue = MultiplayerOptions.MultiplayerOption.IntegerValue.Create();
				this._stringValue = MultiplayerOptions.MultiplayerOption.StringValue.Invalid;
			}

			// Token: 0x06004009 RID: 16393 RVA: 0x000FAC55 File Offset: 0x000F8E55
			public MultiplayerOptions.MultiplayerOption UpdateValue(bool value)
			{
				this.UpdateValue(value ? 1 : 0);
				return this;
			}

			// Token: 0x0600400A RID: 16394 RVA: 0x000FAC66 File Offset: 0x000F8E66
			public MultiplayerOptions.MultiplayerOption UpdateValue(int value)
			{
				this._intValue.UpdateValue(value);
				return this;
			}

			// Token: 0x0600400B RID: 16395 RVA: 0x000FAC75 File Offset: 0x000F8E75
			public MultiplayerOptions.MultiplayerOption UpdateValue(string value)
			{
				this._stringValue.UpdateValue(value);
				return this;
			}

			// Token: 0x0600400C RID: 16396 RVA: 0x000FAC84 File Offset: 0x000F8E84
			public void GetValue(out bool value)
			{
				value = this._intValue.Value == 1;
			}

			// Token: 0x0600400D RID: 16397 RVA: 0x000FAC96 File Offset: 0x000F8E96
			public void GetValue(out int value)
			{
				value = this._intValue.Value;
			}

			// Token: 0x0600400E RID: 16398 RVA: 0x000FACA5 File Offset: 0x000F8EA5
			public void GetValue(out string value)
			{
				value = this._stringValue.Value;
			}

			// Token: 0x0400208E RID: 8334
			public readonly MultiplayerOptions.OptionType OptionType;

			// Token: 0x0400208F RID: 8335
			private MultiplayerOptions.MultiplayerOption.IntegerValue _intValue;

			// Token: 0x04002090 RID: 8336
			private MultiplayerOptions.MultiplayerOption.StringValue _stringValue;

			// Token: 0x020006D2 RID: 1746
			private struct IntegerValue
			{
				// Token: 0x17000B30 RID: 2864
				// (get) Token: 0x06004318 RID: 17176 RVA: 0x001014EC File Offset: 0x000FF6EC
				public static MultiplayerOptions.MultiplayerOption.IntegerValue Invalid
				{
					get
					{
						return default(MultiplayerOptions.MultiplayerOption.IntegerValue);
					}
				}

				// Token: 0x17000B31 RID: 2865
				// (get) Token: 0x06004319 RID: 17177 RVA: 0x00101502 File Offset: 0x000FF702
				// (set) Token: 0x0600431A RID: 17178 RVA: 0x0010150A File Offset: 0x000FF70A
				public bool IsValid { get; private set; }

				// Token: 0x17000B32 RID: 2866
				// (get) Token: 0x0600431B RID: 17179 RVA: 0x00101513 File Offset: 0x000FF713
				// (set) Token: 0x0600431C RID: 17180 RVA: 0x0010151B File Offset: 0x000FF71B
				public int Value { get; private set; }

				// Token: 0x0600431D RID: 17181 RVA: 0x00101524 File Offset: 0x000FF724
				public static MultiplayerOptions.MultiplayerOption.IntegerValue Create()
				{
					return new MultiplayerOptions.MultiplayerOption.IntegerValue
					{
						IsValid = true
					};
				}

				// Token: 0x0600431E RID: 17182 RVA: 0x00101542 File Offset: 0x000FF742
				public void UpdateValue(int value)
				{
					this.Value = value;
				}
			}

			// Token: 0x020006D3 RID: 1747
			private struct StringValue
			{
				// Token: 0x17000B33 RID: 2867
				// (get) Token: 0x0600431F RID: 17183 RVA: 0x0010154C File Offset: 0x000FF74C
				public static MultiplayerOptions.MultiplayerOption.StringValue Invalid
				{
					get
					{
						return default(MultiplayerOptions.MultiplayerOption.StringValue);
					}
				}

				// Token: 0x17000B34 RID: 2868
				// (get) Token: 0x06004320 RID: 17184 RVA: 0x00101562 File Offset: 0x000FF762
				// (set) Token: 0x06004321 RID: 17185 RVA: 0x0010156A File Offset: 0x000FF76A
				public bool IsValid { get; private set; }

				// Token: 0x17000B35 RID: 2869
				// (get) Token: 0x06004322 RID: 17186 RVA: 0x00101573 File Offset: 0x000FF773
				// (set) Token: 0x06004323 RID: 17187 RVA: 0x0010157B File Offset: 0x000FF77B
				public string Value { get; private set; }

				// Token: 0x06004324 RID: 17188 RVA: 0x00101584 File Offset: 0x000FF784
				public static MultiplayerOptions.MultiplayerOption.StringValue Create()
				{
					return new MultiplayerOptions.MultiplayerOption.StringValue
					{
						IsValid = true
					};
				}

				// Token: 0x06004325 RID: 17189 RVA: 0x001015A2 File Offset: 0x000FF7A2
				public void UpdateValue(string value)
				{
					this.Value = value;
				}
			}
		}

		// Token: 0x02000600 RID: 1536
		private class MultiplayerOptionsContainer
		{
			// Token: 0x0600400F RID: 16399 RVA: 0x000FACB4 File Offset: 0x000F8EB4
			public MultiplayerOptionsContainer()
			{
				this._multiplayerOptions = new MultiplayerOptions.MultiplayerOption[53];
			}

			// Token: 0x06004010 RID: 16400 RVA: 0x000FACC9 File Offset: 0x000F8EC9
			public MultiplayerOptions.MultiplayerOption GetOptionFromOptionType(MultiplayerOptions.OptionType optionType)
			{
				return this._multiplayerOptions[(int)optionType];
			}

			// Token: 0x06004011 RID: 16401 RVA: 0x000FACD3 File Offset: 0x000F8ED3
			private void CopyOptionFromOther(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOption option)
			{
				this._multiplayerOptions[(int)optionType] = MultiplayerOptions.MultiplayerOption.CopyMultiplayerOption(option);
			}

			// Token: 0x06004012 RID: 16402 RVA: 0x000FACE3 File Offset: 0x000F8EE3
			public void CreateOption(MultiplayerOptions.OptionType optionType)
			{
				this._multiplayerOptions[(int)optionType] = MultiplayerOptions.MultiplayerOption.CreateMultiplayerOption(optionType);
			}

			// Token: 0x06004013 RID: 16403 RVA: 0x000FACF3 File Offset: 0x000F8EF3
			public void UpdateOptionValue(MultiplayerOptions.OptionType optionType, int value)
			{
				this._multiplayerOptions[(int)optionType].UpdateValue(value);
			}

			// Token: 0x06004014 RID: 16404 RVA: 0x000FAD04 File Offset: 0x000F8F04
			public void UpdateOptionValue(MultiplayerOptions.OptionType optionType, string value)
			{
				this._multiplayerOptions[(int)optionType].UpdateValue(value);
			}

			// Token: 0x06004015 RID: 16405 RVA: 0x000FAD15 File Offset: 0x000F8F15
			public void UpdateOptionValue(MultiplayerOptions.OptionType optionType, bool value)
			{
				this._multiplayerOptions[(int)optionType].UpdateValue(value ? 1 : 0);
			}

			// Token: 0x06004016 RID: 16406 RVA: 0x000FAD2C File Offset: 0x000F8F2C
			public void CopyAllValuesTo(MultiplayerOptions.MultiplayerOptionsContainer other)
			{
				for (MultiplayerOptions.OptionType optionType = MultiplayerOptions.OptionType.ServerName; optionType < MultiplayerOptions.OptionType.NumOfSlots; optionType++)
				{
					other.CopyOptionFromOther(optionType, this._multiplayerOptions[(int)optionType]);
				}
			}

			// Token: 0x04002091 RID: 8337
			private readonly MultiplayerOptions.MultiplayerOption[] _multiplayerOptions;
		}
	}
}
