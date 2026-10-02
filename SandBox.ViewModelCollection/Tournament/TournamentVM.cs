using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Tournaments.MissionLogics;
using SandBox.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.TournamentGames;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Tournament
{
	// Token: 0x02000011 RID: 17
	public class TournamentVM : ViewModel
	{
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00006A34 File Offset: 0x00004C34
		public Action DisableUI { get; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00006A3C File Offset: 0x00004C3C
		public TournamentBehavior Tournament { get; }

		// Token: 0x06000125 RID: 293 RVA: 0x00006A44 File Offset: 0x00004C44
		public TournamentVM(Action disableUI, TournamentBehavior tournamentBehavior)
		{
			this.DisableUI = disableUI;
			this.CurrentMatch = new TournamentMatchVM();
			this.Round1 = new TournamentRoundVM();
			this.Round2 = new TournamentRoundVM();
			this.Round3 = new TournamentRoundVM();
			this.Round4 = new TournamentRoundVM();
			this._rounds = new List<TournamentRoundVM> { this.Round1, this.Round2, this.Round3, this.Round4 };
			this._tournamentWinner = new TournamentParticipantVM();
			this.Tournament = tournamentBehavior;
			this.WinnerIntro = GameTexts.FindText("str_tournament_winner_intro", null).ToString();
			this.BattleRewards = new MBBindingList<TournamentRewardVM>();
			for (int i = 0; i < this._rounds.Count; i++)
			{
				this._rounds[i].Initialize(this.Tournament.Rounds[i], GameTexts.FindText("str_tournament_round", i.ToString()));
			}
			this.Refresh();
			this.Tournament.TournamentEnd += this.OnTournamentEnd;
			this.PrizeVisual = (this.HasPrizeItem ? new ItemImageIdentifierVM(this.Tournament.TournamentGame.Prize, "") : new ItemImageIdentifierVM(null, ""));
			this.SkipAllRoundsHint = new HintViewModel();
			this.RefreshValues();
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00006BC0 File Offset: 0x00004DC0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.LeaveText = GameTexts.FindText("str_tournament_leave", null).ToString();
			this.SkipRoundText = GameTexts.FindText("str_tournament_skip_round", null).ToString();
			this.WatchRoundText = GameTexts.FindText("str_tournament_watch_round", null).ToString();
			this.JoinTournamentText = GameTexts.FindText("str_tournament_join_tournament", null).ToString();
			this.BetText = GameTexts.FindText("str_bet", null).ToString();
			this.AcceptText = GameTexts.FindText("str_accept", null).ToString();
			this.CancelText = GameTexts.FindText("str_cancel", null).ToString();
			this.TournamentWinnerTitle = GameTexts.FindText("str_tournament_winner_title", null).ToString();
			this.BetTitleText = GameTexts.FindText("str_wager", null).ToString();
			GameTexts.SetVariable("MAX_AMOUNT", this.Tournament.GetMaximumBet());
			GameTexts.SetVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			this.BetDescriptionText = GameTexts.FindText("str_tournament_bet_description", null).ToString();
			this.TournamentPrizeText = GameTexts.FindText("str_tournament_prize", null).ToString();
			this.PrizeItemName = this.Tournament.TournamentGame.Prize.Name.ToString();
			MBTextManager.SetTextVariable("SETTLEMENT_NAME", this.Tournament.Settlement.Name, false);
			this.TournamentTitle = GameTexts.FindText("str_tournament", null).ToString();
			this.CurrentWagerText = GameTexts.FindText("str_tournament_current_wager", null).ToString();
			this.SkipAllRoundsHint.HintText = new TextObject("{=GaOE4bdd}Skip All Rounds", null);
			TournamentRoundVM round = this._round1;
			if (round != null)
			{
				round.RefreshValues();
			}
			TournamentRoundVM round2 = this._round2;
			if (round2 != null)
			{
				round2.RefreshValues();
			}
			TournamentRoundVM round3 = this._round3;
			if (round3 != null)
			{
				round3.RefreshValues();
			}
			TournamentRoundVM round4 = this._round4;
			if (round4 != null)
			{
				round4.RefreshValues();
			}
			TournamentMatchVM currentMatch = this._currentMatch;
			if (currentMatch != null)
			{
				currentMatch.RefreshValues();
			}
			TournamentParticipantVM tournamentWinner = this._tournamentWinner;
			if (tournamentWinner == null)
			{
				return;
			}
			tournamentWinner.RefreshValues();
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006DCB File Offset: 0x00004FCB
		public void ExecuteBet()
		{
			this._thisRoundBettedAmount += this.WageredDenars;
			this.Tournament.PlaceABet(this.WageredDenars);
			this.RefreshBetProperties();
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006DF8 File Offset: 0x00004FF8
		public void ExecuteJoinTournament()
		{
			if (this.PlayerCanJoinMatch())
			{
				this.Tournament.StartMatch();
				this.IsCurrentMatchActive = true;
				this.CurrentMatch.Refresh(true);
				this.CurrentMatch.State = 3;
				this.DisableUI();
				this.IsCurrentMatchActive = true;
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006E49 File Offset: 0x00005049
		public void ExecuteSkipRound()
		{
			if (this.IsTournamentIncomplete)
			{
				this.Tournament.SkipMatch(false);
			}
			this.Refresh();
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006E68 File Offset: 0x00005068
		public void ExecuteSkipAllRounds()
		{
			int num = 0;
			int num2 = this.Tournament.Rounds.Sum<TournamentRound>((TournamentRound r) => r.Matches.Length);
			while (!this.CanPlayerJoin)
			{
				TournamentRound currentRound = this.Tournament.CurrentRound;
				if (((currentRound != null) ? currentRound.CurrentMatch : null) == null || num >= num2)
				{
					break;
				}
				this.ExecuteSkipRound();
				num++;
			}
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006ED8 File Offset: 0x000050D8
		public void ExecuteWatchRound()
		{
			if (!this.PlayerCanJoinMatch())
			{
				this.Tournament.StartMatch();
				this.IsCurrentMatchActive = true;
				this.CurrentMatch.Refresh(true);
				this.CurrentMatch.State = 3;
				this.DisableUI();
				this.IsCurrentMatchActive = true;
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00006F2C File Offset: 0x0000512C
		public void ExecuteLeave()
		{
			if (this.CurrentMatch != null)
			{
				List<TournamentMatch> list = new List<TournamentMatch>();
				for (int i = this.Tournament.CurrentRoundIndex; i < this.Tournament.Rounds.Length; i++)
				{
					list.AddRange(this.Tournament.Rounds[i].Matches.Where<TournamentMatch>((TournamentMatch x) => x.State != TournamentMatch.MatchState.Finished));
				}
				if (list.Any<TournamentMatch>((TournamentMatch x) => x.Participants.Any<TournamentParticipant>((TournamentParticipant y) => y.Character == CharacterObject.PlayerCharacter)))
				{
					InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_forfeit", null).ToString(), GameTexts.FindText("str_tournament_forfeit_game", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.EndTournamentMission), null, "", 0f, null, null, null), true, false);
					return;
				}
			}
			this.EndTournamentMission();
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000703E File Offset: 0x0000523E
		private void EndTournamentMission()
		{
			this.Tournament.EndTournamentViaLeave();
			Mission.Current.EndMission();
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00007058 File Offset: 0x00005258
		private void RefreshBetProperties()
		{
			TextObject textObject = new TextObject("{=L9GnQvsq}Stake: {BETTED_DENARS}", null);
			textObject.SetTextVariable("BETTED_DENARS", this.Tournament.BettedDenars);
			this.BettedDenarsText = textObject.ToString();
			TextObject textObject2 = new TextObject("{=xzzSaN4b}Expected: {OVERALL_EXPECTED_DENARS}", null);
			textObject2.SetTextVariable("OVERALL_EXPECTED_DENARS", this.Tournament.OverallExpectedDenars);
			this.OverallExpectedDenarsText = textObject2.ToString();
			TextObject textObject3 = new TextObject("{=yF5fpwNE}Total: {TOTAL}", null);
			textObject3.SetTextVariable("TOTAL", this.Tournament.PlayerDenars);
			this.TotalDenarsText = textObject3.ToString();
			base.OnPropertyChanged("IsBetButtonEnabled");
			this.MaximumBetValue = MathF.Min(this.Tournament.GetMaximumBet() - this._thisRoundBettedAmount, Hero.MainHero.Gold);
			GameTexts.SetVariable("NORMALIZED_EXPECTED_GOLD", (int)(this.Tournament.BetOdd * 100f));
			GameTexts.SetVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
			this.BetOddsText = GameTexts.FindText("str_tournament_bet_odd", null).ToString();
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00007165 File Offset: 0x00005365
		private void OnNewRoundStarted(int prevRoundIndex, int currentRoundIndex)
		{
			this._isPlayerParticipating = this.Tournament.IsPlayerParticipating;
			this._thisRoundBettedAmount = 0;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00007180 File Offset: 0x00005380
		public void Refresh()
		{
			this.IsCurrentMatchActive = false;
			this.CurrentMatch = this._rounds[this.Tournament.CurrentRoundIndex].Matches.Find((TournamentMatchVM m) => m.IsValid && m.Match == this.Tournament.CurrentMatch);
			this.ActiveRoundIndex = this.Tournament.CurrentRoundIndex;
			this.CanPlayerJoin = this.PlayerCanJoinMatch();
			base.OnPropertyChanged("IsTournamentIncomplete");
			base.OnPropertyChanged("InitializationOver");
			base.OnPropertyChanged("IsBetButtonEnabled");
			this.HasPrizeItem = this.Tournament.TournamentGame.Prize != null && !this.IsOver;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00007228 File Offset: 0x00005428
		private void OnTournamentEnd()
		{
			TournamentParticipantVM[] array = this.Round4.Matches.Last<TournamentMatchVM>((TournamentMatchVM m) => m.IsValid).GetParticipants().ToArray<TournamentParticipantVM>();
			TournamentParticipantVM tournamentParticipantVM = array[0];
			TournamentParticipantVM tournamentParticipantVM2 = array[1];
			this.TournamentWinner = this.Round4.Matches.Last<TournamentMatchVM>((TournamentMatchVM m) => m.IsValid).GetParticipants().First<TournamentParticipantVM>((TournamentParticipantVM p) => p.Participant == this.Tournament.Winner);
			this.TournamentWinner.Refresh();
			if (this.TournamentWinner.Participant.Character.IsHero)
			{
				Hero heroObject = this.TournamentWinner.Participant.Character.HeroObject;
				this.TournamentWinner.Character.ArmorColor1 = heroObject.MapFaction.Color;
				this.TournamentWinner.Character.ArmorColor2 = heroObject.MapFaction.Color2;
			}
			else
			{
				CultureObject culture = this.TournamentWinner.Participant.Character.Culture;
				this.TournamentWinner.Character.ArmorColor1 = culture.Color;
				this.TournamentWinner.Character.ArmorColor2 = culture.Color2;
			}
			this.IsWinnerHero = this.Tournament.Winner.Character.IsHero;
			if (this.IsWinnerHero)
			{
				this.WinnerBanner = new BannerImageIdentifierVM(this.Tournament.Winner.Character.HeroObject.ClanBanner, true);
			}
			if (this.TournamentWinner.Participant.Character.IsPlayerCharacter)
			{
				TournamentParticipantVM tournamentParticipantVM3 = ((tournamentParticipantVM == this.TournamentWinner) ? tournamentParticipantVM2 : tournamentParticipantVM);
				GameTexts.SetVariable("TOURNAMENT_FINAL_OPPONENT", tournamentParticipantVM3.Name);
				this.WinnerIntro = GameTexts.FindText("str_tournament_result_won", null).ToString();
				if (this.Tournament.TournamentGame.TournamentWinRenown > 0f)
				{
					GameTexts.SetVariable("RENOWN", this.Tournament.TournamentGame.TournamentWinRenown.ToString("F1"));
					this.BattleRewards.Add(new TournamentRewardVM(GameTexts.FindText("str_tournament_renown", null).ToString()));
				}
				if (this.Tournament.TournamentGame.TournamentWinInfluence > 0f)
				{
					float tournamentWinInfluence = this.Tournament.TournamentGame.TournamentWinInfluence;
					TextObject textObject = GameTexts.FindText("str_tournament_influence", null);
					textObject.SetTextVariable("INFLUENCE", tournamentWinInfluence.ToString("F1"));
					textObject.SetTextVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
					this.BattleRewards.Add(new TournamentRewardVM(textObject.ToString()));
				}
				if (this.Tournament.TournamentGame.Prize != null)
				{
					string text = this.Tournament.TournamentGame.Prize.Name.ToString();
					GameTexts.SetVariable("REWARD", text);
					this.BattleRewards.Add(new TournamentRewardVM(GameTexts.FindText("str_tournament_reward", null).ToString(), new ItemImageIdentifierVM(this.Tournament.TournamentGame.Prize, "")));
				}
				if (this.Tournament.OverallExpectedDenars > 0)
				{
					int overallExpectedDenars = this.Tournament.OverallExpectedDenars;
					TextObject textObject2 = GameTexts.FindText("str_tournament_bet", null);
					textObject2.SetTextVariable("BET", overallExpectedDenars);
					textObject2.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">");
					this.BattleRewards.Add(new TournamentRewardVM(textObject2.ToString()));
				}
			}
			else if (tournamentParticipantVM.Participant.Character.IsPlayerCharacter || tournamentParticipantVM2.Participant.Character.IsPlayerCharacter)
			{
				TournamentParticipantVM tournamentParticipantVM4 = ((tournamentParticipantVM == this.TournamentWinner) ? tournamentParticipantVM : tournamentParticipantVM2);
				GameTexts.SetVariable("TOURNAMENT_FINAL_OPPONENT", tournamentParticipantVM4.Name);
				this.WinnerIntro = GameTexts.FindText("str_tournament_result_eliminated_at_final", null).ToString();
			}
			else
			{
				int num = 3;
				bool flag = this.Round3.GetParticipants().Any<TournamentParticipantVM>((TournamentParticipantVM p) => p.Participant.Character.IsPlayerCharacter);
				bool flag2 = this.Round2.GetParticipants().Any<TournamentParticipantVM>((TournamentParticipantVM p) => p.Participant.Character.IsPlayerCharacter);
				bool flag3 = this.Round1.GetParticipants().Any<TournamentParticipantVM>((TournamentParticipantVM p) => p.Participant.Character.IsPlayerCharacter);
				if (flag)
				{
					num = 3;
				}
				else if (flag2)
				{
					num = 2;
				}
				else if (flag3)
				{
					num = 1;
				}
				bool flag4 = tournamentParticipantVM == this.TournamentWinner;
				GameTexts.SetVariable("TOURNAMENT_FINAL_PARTICIPANT_A", flag4 ? tournamentParticipantVM.Name : tournamentParticipantVM2.Name);
				GameTexts.SetVariable("TOURNAMENT_FINAL_PARTICIPANT_B", flag4 ? tournamentParticipantVM2.Name : tournamentParticipantVM.Name);
				if (this._isPlayerParticipating)
				{
					GameTexts.SetVariable("TOURNAMENT_ELIMINATED_ROUND", num.ToString());
					this.WinnerIntro = GameTexts.FindText("str_tournament_result_eliminated", null).ToString();
				}
				else
				{
					this.WinnerIntro = GameTexts.FindText("str_tournament_result_spectator", null).ToString();
				}
			}
			this.IsOver = true;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00007759 File Offset: 0x00005959
		private bool PlayerCanJoinMatch()
		{
			if (this.IsTournamentIncomplete)
			{
				return this.Tournament.CurrentMatch.Participants.Any<TournamentParticipant>((TournamentParticipant x) => x.Character == CharacterObject.PlayerCharacter);
			}
			return false;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000779C File Offset: 0x0000599C
		public void OnAgentRemoved(Agent agent)
		{
			if (this.IsCurrentMatchActive && agent.IsHuman)
			{
				TournamentParticipant participant = this.CurrentMatch.Match.GetParticipant(agent.Origin.UniqueSeed);
				if (participant != null)
				{
					this.CurrentMatch.GetParticipants().First<TournamentParticipantVM>((TournamentParticipantVM p) => p.Participant == participant).IsDead = true;
				}
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000780A File Offset: 0x00005A0A
		public void ExecuteShowPrizeItemTooltip()
		{
			if (this.HasPrizeItem)
			{
				InformationManager.ShowTooltip(typeof(ItemObject), new object[]
				{
					new EquipmentElement(this.Tournament.TournamentGame.Prize, null, null, false)
				});
			}
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00007849 File Offset: 0x00005A49
		public void ExecuteHidePrizeItemTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00007850 File Offset: 0x00005A50
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM doneInputKey = this.DoneInputKey;
			if (doneInputKey != null)
			{
				doneInputKey.OnFinalize();
			}
			InputKeyItemVM cancelInputKey = this.CancelInputKey;
			if (cancelInputKey == null)
			{
				return;
			}
			cancelInputKey.OnFinalize();
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00007879 File Offset: 0x00005A79
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00007888 File Offset: 0x00005A88
		public void SetCancelInputKey(HotKey hotKey)
		{
			this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00007897 File Offset: 0x00005A97
		// (set) Token: 0x0600013A RID: 314 RVA: 0x0000789F File Offset: 0x00005A9F
		[DataSourceProperty]
		public InputKeyItemVM DoneInputKey
		{
			get
			{
				return this._doneInputKey;
			}
			set
			{
				if (value != this._doneInputKey)
				{
					this._doneInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "DoneInputKey");
				}
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600013B RID: 315 RVA: 0x000078BD File Offset: 0x00005ABD
		// (set) Token: 0x0600013C RID: 316 RVA: 0x000078C5 File Offset: 0x00005AC5
		[DataSourceProperty]
		public InputKeyItemVM CancelInputKey
		{
			get
			{
				return this._cancelInputKey;
			}
			set
			{
				if (value != this._cancelInputKey)
				{
					this._cancelInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
				}
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600013D RID: 317 RVA: 0x000078E3 File Offset: 0x00005AE3
		// (set) Token: 0x0600013E RID: 318 RVA: 0x000078EB File Offset: 0x00005AEB
		[DataSourceProperty]
		public string TournamentWinnerTitle
		{
			get
			{
				return this._tournamentWinnerTitle;
			}
			set
			{
				if (value != this._tournamentWinnerTitle)
				{
					this._tournamentWinnerTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "TournamentWinnerTitle");
				}
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600013F RID: 319 RVA: 0x0000790E File Offset: 0x00005B0E
		// (set) Token: 0x06000140 RID: 320 RVA: 0x00007916 File Offset: 0x00005B16
		[DataSourceProperty]
		public TournamentParticipantVM TournamentWinner
		{
			get
			{
				return this._tournamentWinner;
			}
			set
			{
				if (value != this._tournamentWinner)
				{
					this._tournamentWinner = value;
					base.OnPropertyChangedWithValue<TournamentParticipantVM>(value, "TournamentWinner");
				}
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00007934 File Offset: 0x00005B34
		// (set) Token: 0x06000142 RID: 322 RVA: 0x0000793C File Offset: 0x00005B3C
		[DataSourceProperty]
		public int MaximumBetValue
		{
			get
			{
				return this._maximumBetValue;
			}
			set
			{
				if (value != this._maximumBetValue)
				{
					this._maximumBetValue = value;
					base.OnPropertyChangedWithValue(value, "MaximumBetValue");
					this._wageredDenars = -1;
					this.WageredDenars = 0;
				}
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00007968 File Offset: 0x00005B68
		[DataSourceProperty]
		public bool IsBetButtonEnabled
		{
			get
			{
				return this.PlayerCanJoinMatch() && this.Tournament.GetMaximumBet() > this._thisRoundBettedAmount && Hero.MainHero.Gold > 0;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00007994 File Offset: 0x00005B94
		// (set) Token: 0x06000145 RID: 325 RVA: 0x0000799C File Offset: 0x00005B9C
		[DataSourceProperty]
		public string BetText
		{
			get
			{
				return this._betText;
			}
			set
			{
				if (value != this._betText)
				{
					this._betText = value;
					base.OnPropertyChangedWithValue<string>(value, "BetText");
				}
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000146 RID: 326 RVA: 0x000079BF File Offset: 0x00005BBF
		// (set) Token: 0x06000147 RID: 327 RVA: 0x000079C7 File Offset: 0x00005BC7
		[DataSourceProperty]
		public string BetTitleText
		{
			get
			{
				return this._betTitleText;
			}
			set
			{
				if (value != this._betTitleText)
				{
					this._betTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "BetTitleText");
				}
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000148 RID: 328 RVA: 0x000079EA File Offset: 0x00005BEA
		// (set) Token: 0x06000149 RID: 329 RVA: 0x000079F2 File Offset: 0x00005BF2
		[DataSourceProperty]
		public string CurrentWagerText
		{
			get
			{
				return this._currentWagerText;
			}
			set
			{
				if (value != this._currentWagerText)
				{
					this._currentWagerText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentWagerText");
				}
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00007A15 File Offset: 0x00005C15
		// (set) Token: 0x0600014B RID: 331 RVA: 0x00007A1D File Offset: 0x00005C1D
		[DataSourceProperty]
		public string BetDescriptionText
		{
			get
			{
				return this._betDescriptionText;
			}
			set
			{
				if (value != this._betDescriptionText)
				{
					this._betDescriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "BetDescriptionText");
				}
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00007A40 File Offset: 0x00005C40
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00007A48 File Offset: 0x00005C48
		[DataSourceProperty]
		public ItemImageIdentifierVM PrizeVisual
		{
			get
			{
				return this._prizeVisual;
			}
			set
			{
				if (value != this._prizeVisual)
				{
					this._prizeVisual = value;
					base.OnPropertyChangedWithValue<ItemImageIdentifierVM>(value, "PrizeVisual");
				}
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00007A66 File Offset: 0x00005C66
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00007A6E File Offset: 0x00005C6E
		[DataSourceProperty]
		public string PrizeItemName
		{
			get
			{
				return this._prizeItemName;
			}
			set
			{
				if (value != this._prizeItemName)
				{
					this._prizeItemName = value;
					base.OnPropertyChangedWithValue<string>(value, "PrizeItemName");
				}
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00007A91 File Offset: 0x00005C91
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00007A99 File Offset: 0x00005C99
		[DataSourceProperty]
		public string TournamentPrizeText
		{
			get
			{
				return this._tournamentPrizeText;
			}
			set
			{
				if (value != this._tournamentPrizeText)
				{
					this._tournamentPrizeText = value;
					base.OnPropertyChangedWithValue<string>(value, "TournamentPrizeText");
				}
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00007ABC File Offset: 0x00005CBC
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00007AC4 File Offset: 0x00005CC4
		[DataSourceProperty]
		public int WageredDenars
		{
			get
			{
				return this._wageredDenars;
			}
			set
			{
				if (value != this._wageredDenars)
				{
					this._wageredDenars = value;
					base.OnPropertyChangedWithValue(value, "WageredDenars");
					this.ExpectedBetDenars = ((this._wageredDenars == 0) ? 0 : this.Tournament.GetExpectedDenarsForBet(this._wageredDenars));
				}
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00007B04 File Offset: 0x00005D04
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00007B0C File Offset: 0x00005D0C
		[DataSourceProperty]
		public int ExpectedBetDenars
		{
			get
			{
				return this._expectedBetDenars;
			}
			set
			{
				if (value != this._expectedBetDenars)
				{
					this._expectedBetDenars = value;
					base.OnPropertyChangedWithValue(value, "ExpectedBetDenars");
				}
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000156 RID: 342 RVA: 0x00007B2A File Offset: 0x00005D2A
		// (set) Token: 0x06000157 RID: 343 RVA: 0x00007B32 File Offset: 0x00005D32
		[DataSourceProperty]
		public string BetOddsText
		{
			get
			{
				return this._betOddsText;
			}
			set
			{
				if (value != this._betOddsText)
				{
					this._betOddsText = value;
					base.OnPropertyChangedWithValue<string>(value, "BetOddsText");
				}
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000158 RID: 344 RVA: 0x00007B55 File Offset: 0x00005D55
		// (set) Token: 0x06000159 RID: 345 RVA: 0x00007B5D File Offset: 0x00005D5D
		[DataSourceProperty]
		public string BettedDenarsText
		{
			get
			{
				return this._bettedDenarsText;
			}
			set
			{
				if (value != this._bettedDenarsText)
				{
					this._bettedDenarsText = value;
					base.OnPropertyChangedWithValue<string>(value, "BettedDenarsText");
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600015A RID: 346 RVA: 0x00007B80 File Offset: 0x00005D80
		// (set) Token: 0x0600015B RID: 347 RVA: 0x00007B88 File Offset: 0x00005D88
		[DataSourceProperty]
		public string OverallExpectedDenarsText
		{
			get
			{
				return this._overallExpectedDenarsText;
			}
			set
			{
				if (value != this._overallExpectedDenarsText)
				{
					this._overallExpectedDenarsText = value;
					base.OnPropertyChangedWithValue<string>(value, "OverallExpectedDenarsText");
				}
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00007BAB File Offset: 0x00005DAB
		// (set) Token: 0x0600015D RID: 349 RVA: 0x00007BB3 File Offset: 0x00005DB3
		[DataSourceProperty]
		public string CurrentExpectedDenarsText
		{
			get
			{
				return this._currentExpectedDenarsText;
			}
			set
			{
				if (value != this._currentExpectedDenarsText)
				{
					this._currentExpectedDenarsText = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentExpectedDenarsText");
				}
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00007BD6 File Offset: 0x00005DD6
		// (set) Token: 0x0600015F RID: 351 RVA: 0x00007BDE File Offset: 0x00005DDE
		[DataSourceProperty]
		public string TotalDenarsText
		{
			get
			{
				return this._totalDenarsText;
			}
			set
			{
				if (value != this._totalDenarsText)
				{
					this._totalDenarsText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalDenarsText");
				}
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00007C01 File Offset: 0x00005E01
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00007C09 File Offset: 0x00005E09
		[DataSourceProperty]
		public string AcceptText
		{
			get
			{
				return this._acceptText;
			}
			set
			{
				if (value != this._acceptText)
				{
					this._acceptText = value;
					base.OnPropertyChangedWithValue<string>(value, "AcceptText");
				}
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000162 RID: 354 RVA: 0x00007C2C File Offset: 0x00005E2C
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00007C34 File Offset: 0x00005E34
		[DataSourceProperty]
		public string CancelText
		{
			get
			{
				return this._cancelText;
			}
			set
			{
				if (value != this._cancelText)
				{
					this._cancelText = value;
					base.OnPropertyChangedWithValue<string>(value, "CancelText");
				}
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000164 RID: 356 RVA: 0x00007C57 File Offset: 0x00005E57
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00007C5F File Offset: 0x00005E5F
		[DataSourceProperty]
		public bool IsCurrentMatchActive
		{
			get
			{
				return this._isCurrentMatchActive;
			}
			set
			{
				this._isCurrentMatchActive = value;
				base.OnPropertyChangedWithValue(value, "IsCurrentMatchActive");
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00007C74 File Offset: 0x00005E74
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00007C7C File Offset: 0x00005E7C
		[DataSourceProperty]
		public TournamentMatchVM CurrentMatch
		{
			get
			{
				return this._currentMatch;
			}
			set
			{
				if (value != this._currentMatch)
				{
					TournamentMatchVM currentMatch = this._currentMatch;
					if (currentMatch != null && currentMatch.IsValid)
					{
						this._currentMatch.State = 2;
						this._currentMatch.Refresh(false);
						int num = this._rounds.FindIndex((TournamentRoundVM r) => r.Matches.Any<TournamentMatchVM>((TournamentMatchVM m) => m.Match == this.Tournament.LastMatch));
						if (num < this.Tournament.Rounds.Length - 1)
						{
							this._rounds[num + 1].Initialize();
						}
					}
					this._currentMatch = value;
					base.OnPropertyChangedWithValue<TournamentMatchVM>(value, "CurrentMatch");
					if (this._currentMatch != null)
					{
						this._currentMatch.State = 1;
					}
				}
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00007D25 File Offset: 0x00005F25
		// (set) Token: 0x06000169 RID: 361 RVA: 0x00007D3F File Offset: 0x00005F3F
		[DataSourceProperty]
		public bool IsTournamentIncomplete
		{
			get
			{
				return this.Tournament == null || this.Tournament.CurrentMatch != null;
			}
			set
			{
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00007D41 File Offset: 0x00005F41
		// (set) Token: 0x0600016B RID: 363 RVA: 0x00007D49 File Offset: 0x00005F49
		[DataSourceProperty]
		public int ActiveRoundIndex
		{
			get
			{
				return this._activeRoundIndex;
			}
			set
			{
				if (value != this._activeRoundIndex)
				{
					this.OnNewRoundStarted(this._activeRoundIndex, value);
					this._activeRoundIndex = value;
					base.OnPropertyChangedWithValue(value, "ActiveRoundIndex");
					this.RefreshBetProperties();
				}
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00007D7A File Offset: 0x00005F7A
		// (set) Token: 0x0600016D RID: 365 RVA: 0x00007D82 File Offset: 0x00005F82
		[DataSourceProperty]
		public bool CanPlayerJoin
		{
			get
			{
				return this._canPlayerJoin;
			}
			set
			{
				if (value != this._canPlayerJoin)
				{
					this._canPlayerJoin = value;
					base.OnPropertyChangedWithValue(value, "CanPlayerJoin");
				}
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00007DA0 File Offset: 0x00005FA0
		// (set) Token: 0x0600016F RID: 367 RVA: 0x00007DA8 File Offset: 0x00005FA8
		[DataSourceProperty]
		public bool HasPrizeItem
		{
			get
			{
				return this._hasPrizeItem;
			}
			set
			{
				if (value != this._hasPrizeItem)
				{
					this._hasPrizeItem = value;
					base.OnPropertyChangedWithValue(value, "HasPrizeItem");
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00007DC6 File Offset: 0x00005FC6
		// (set) Token: 0x06000171 RID: 369 RVA: 0x00007DCE File Offset: 0x00005FCE
		[DataSourceProperty]
		public string JoinTournamentText
		{
			get
			{
				return this._joinTournamentText;
			}
			set
			{
				if (value != this._joinTournamentText)
				{
					this._joinTournamentText = value;
					base.OnPropertyChangedWithValue<string>(value, "JoinTournamentText");
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00007DF1 File Offset: 0x00005FF1
		// (set) Token: 0x06000173 RID: 371 RVA: 0x00007DF9 File Offset: 0x00005FF9
		[DataSourceProperty]
		public string SkipRoundText
		{
			get
			{
				return this._skipRoundText;
			}
			set
			{
				if (value != this._skipRoundText)
				{
					this._skipRoundText = value;
					base.OnPropertyChangedWithValue<string>(value, "SkipRoundText");
				}
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00007E1C File Offset: 0x0000601C
		// (set) Token: 0x06000175 RID: 373 RVA: 0x00007E24 File Offset: 0x00006024
		[DataSourceProperty]
		public string WatchRoundText
		{
			get
			{
				return this._watchRoundText;
			}
			set
			{
				if (value != this._watchRoundText)
				{
					this._watchRoundText = value;
					base.OnPropertyChangedWithValue<string>(value, "WatchRoundText");
				}
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00007E47 File Offset: 0x00006047
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00007E4F File Offset: 0x0000604F
		[DataSourceProperty]
		public string LeaveText
		{
			get
			{
				return this._leaveText;
			}
			set
			{
				if (value != this._leaveText)
				{
					this._leaveText = value;
					base.OnPropertyChangedWithValue<string>(value, "LeaveText");
				}
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00007E72 File Offset: 0x00006072
		// (set) Token: 0x06000179 RID: 377 RVA: 0x00007E7A File Offset: 0x0000607A
		[DataSourceProperty]
		public TournamentRoundVM Round1
		{
			get
			{
				return this._round1;
			}
			set
			{
				if (value != this._round1)
				{
					this._round1 = value;
					base.OnPropertyChangedWithValue<TournamentRoundVM>(value, "Round1");
				}
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00007E98 File Offset: 0x00006098
		// (set) Token: 0x0600017B RID: 379 RVA: 0x00007EA0 File Offset: 0x000060A0
		[DataSourceProperty]
		public TournamentRoundVM Round2
		{
			get
			{
				return this._round2;
			}
			set
			{
				if (value != this._round2)
				{
					this._round2 = value;
					base.OnPropertyChangedWithValue<TournamentRoundVM>(value, "Round2");
				}
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00007EBE File Offset: 0x000060BE
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00007EC6 File Offset: 0x000060C6
		[DataSourceProperty]
		public TournamentRoundVM Round3
		{
			get
			{
				return this._round3;
			}
			set
			{
				if (value != this._round3)
				{
					this._round3 = value;
					base.OnPropertyChangedWithValue<TournamentRoundVM>(value, "Round3");
				}
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00007EE4 File Offset: 0x000060E4
		// (set) Token: 0x0600017F RID: 383 RVA: 0x00007EEC File Offset: 0x000060EC
		[DataSourceProperty]
		public TournamentRoundVM Round4
		{
			get
			{
				return this._round4;
			}
			set
			{
				if (value != this._round4)
				{
					this._round4 = value;
					base.OnPropertyChangedWithValue<TournamentRoundVM>(value, "Round4");
				}
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00007F0A File Offset: 0x0000610A
		[DataSourceProperty]
		public bool InitializationOver
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00007F0D File Offset: 0x0000610D
		// (set) Token: 0x06000182 RID: 386 RVA: 0x00007F15 File Offset: 0x00006115
		[DataSourceProperty]
		public string TournamentTitle
		{
			get
			{
				return this._tournamentTitle;
			}
			set
			{
				if (value != this._tournamentTitle)
				{
					this._tournamentTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "TournamentTitle");
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000183 RID: 387 RVA: 0x00007F38 File Offset: 0x00006138
		// (set) Token: 0x06000184 RID: 388 RVA: 0x00007F40 File Offset: 0x00006140
		[DataSourceProperty]
		public bool IsOver
		{
			get
			{
				return this._isOver;
			}
			set
			{
				if (this._isOver != value)
				{
					this._isOver = value;
					base.OnPropertyChangedWithValue(value, "IsOver");
				}
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00007F5E File Offset: 0x0000615E
		// (set) Token: 0x06000186 RID: 390 RVA: 0x00007F66 File Offset: 0x00006166
		[DataSourceProperty]
		public string WinnerIntro
		{
			get
			{
				return this._winnerIntro;
			}
			set
			{
				if (value != this._winnerIntro)
				{
					this._winnerIntro = value;
					base.OnPropertyChangedWithValue<string>(value, "WinnerIntro");
				}
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00007F89 File Offset: 0x00006189
		// (set) Token: 0x06000188 RID: 392 RVA: 0x00007F91 File Offset: 0x00006191
		[DataSourceProperty]
		public MBBindingList<TournamentRewardVM> BattleRewards
		{
			get
			{
				return this._battleRewards;
			}
			set
			{
				if (value != this._battleRewards)
				{
					this._battleRewards = value;
					base.OnPropertyChangedWithValue<MBBindingList<TournamentRewardVM>>(value, "BattleRewards");
				}
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00007FAF File Offset: 0x000061AF
		// (set) Token: 0x0600018A RID: 394 RVA: 0x00007FB7 File Offset: 0x000061B7
		[DataSourceProperty]
		public bool IsWinnerHero
		{
			get
			{
				return this._isWinnerHero;
			}
			set
			{
				if (value != this._isWinnerHero)
				{
					this._isWinnerHero = value;
					base.OnPropertyChangedWithValue(value, "IsWinnerHero");
				}
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00007FD5 File Offset: 0x000061D5
		// (set) Token: 0x0600018C RID: 396 RVA: 0x00007FDD File Offset: 0x000061DD
		[DataSourceProperty]
		public bool IsBetWindowEnabled
		{
			get
			{
				return this._isBetWindowEnabled;
			}
			set
			{
				if (value != this._isBetWindowEnabled)
				{
					this._isBetWindowEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsBetWindowEnabled");
				}
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00007FFB File Offset: 0x000061FB
		// (set) Token: 0x0600018E RID: 398 RVA: 0x00008003 File Offset: 0x00006203
		[DataSourceProperty]
		public BannerImageIdentifierVM WinnerBanner
		{
			get
			{
				return this._winnerBanner;
			}
			set
			{
				if (value != this._winnerBanner)
				{
					this._winnerBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "WinnerBanner");
				}
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00008021 File Offset: 0x00006221
		// (set) Token: 0x06000190 RID: 400 RVA: 0x00008029 File Offset: 0x00006229
		[DataSourceProperty]
		public HintViewModel SkipAllRoundsHint
		{
			get
			{
				return this._skipAllRoundsHint;
			}
			set
			{
				if (value != this._skipAllRoundsHint)
				{
					this._skipAllRoundsHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "SkipAllRoundsHint");
				}
			}
		}

		// Token: 0x04000084 RID: 132
		private readonly List<TournamentRoundVM> _rounds;

		// Token: 0x04000085 RID: 133
		private int _thisRoundBettedAmount;

		// Token: 0x04000086 RID: 134
		private bool _isPlayerParticipating;

		// Token: 0x04000087 RID: 135
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000088 RID: 136
		private InputKeyItemVM _cancelInputKey;

		// Token: 0x04000089 RID: 137
		private TournamentRoundVM _round1;

		// Token: 0x0400008A RID: 138
		private TournamentRoundVM _round2;

		// Token: 0x0400008B RID: 139
		private TournamentRoundVM _round3;

		// Token: 0x0400008C RID: 140
		private TournamentRoundVM _round4;

		// Token: 0x0400008D RID: 141
		private int _activeRoundIndex = -1;

		// Token: 0x0400008E RID: 142
		private string _joinTournamentText;

		// Token: 0x0400008F RID: 143
		private string _skipRoundText;

		// Token: 0x04000090 RID: 144
		private string _watchRoundText;

		// Token: 0x04000091 RID: 145
		private string _leaveText;

		// Token: 0x04000092 RID: 146
		private bool _canPlayerJoin;

		// Token: 0x04000093 RID: 147
		private TournamentMatchVM _currentMatch;

		// Token: 0x04000094 RID: 148
		private bool _isCurrentMatchActive;

		// Token: 0x04000095 RID: 149
		private string _betTitleText;

		// Token: 0x04000096 RID: 150
		private string _betDescriptionText;

		// Token: 0x04000097 RID: 151
		private string _betOddsText;

		// Token: 0x04000098 RID: 152
		private string _bettedDenarsText;

		// Token: 0x04000099 RID: 153
		private string _overallExpectedDenarsText;

		// Token: 0x0400009A RID: 154
		private string _currentExpectedDenarsText;

		// Token: 0x0400009B RID: 155
		private string _totalDenarsText;

		// Token: 0x0400009C RID: 156
		private string _acceptText;

		// Token: 0x0400009D RID: 157
		private string _cancelText;

		// Token: 0x0400009E RID: 158
		private string _prizeItemName;

		// Token: 0x0400009F RID: 159
		private string _tournamentPrizeText;

		// Token: 0x040000A0 RID: 160
		private string _currentWagerText;

		// Token: 0x040000A1 RID: 161
		private int _wageredDenars = -1;

		// Token: 0x040000A2 RID: 162
		private int _expectedBetDenars = -1;

		// Token: 0x040000A3 RID: 163
		private string _betText;

		// Token: 0x040000A4 RID: 164
		private int _maximumBetValue;

		// Token: 0x040000A5 RID: 165
		private string _tournamentWinnerTitle;

		// Token: 0x040000A6 RID: 166
		private TournamentParticipantVM _tournamentWinner;

		// Token: 0x040000A7 RID: 167
		private string _tournamentTitle;

		// Token: 0x040000A8 RID: 168
		private bool _isOver;

		// Token: 0x040000A9 RID: 169
		private bool _hasPrizeItem;

		// Token: 0x040000AA RID: 170
		private bool _isWinnerHero;

		// Token: 0x040000AB RID: 171
		private bool _isBetWindowEnabled;

		// Token: 0x040000AC RID: 172
		private string _winnerIntro;

		// Token: 0x040000AD RID: 173
		private ItemImageIdentifierVM _prizeVisual;

		// Token: 0x040000AE RID: 174
		private BannerImageIdentifierVM _winnerBanner;

		// Token: 0x040000AF RID: 175
		private MBBindingList<TournamentRewardVM> _battleRewards;

		// Token: 0x040000B0 RID: 176
		private HintViewModel _skipAllRoundsHint;
	}
}
