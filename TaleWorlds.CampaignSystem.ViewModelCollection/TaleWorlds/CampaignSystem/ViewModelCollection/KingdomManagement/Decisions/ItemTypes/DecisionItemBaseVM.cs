using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.Library;
using TaleWorlds.Library.EventSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes
{
	// Token: 0x02000080 RID: 128
	public class DecisionItemBaseVM : ViewModel
	{
		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x0002C24F File Offset: 0x0002A44F
		// (set) Token: 0x06000A23 RID: 2595 RVA: 0x0002C257 File Offset: 0x0002A457
		public KingdomElection KingdomDecisionMaker { get; private set; }

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x0002C260 File Offset: 0x0002A460
		private float _currentInfluenceCost
		{
			get
			{
				if (this._currentSelectedOption != null && !this._currentSelectedOption.IsOptionForAbstain)
				{
					if (!this.IsPlayerSupporter)
					{
						return (float)Campaign.Current.Models.ClanPoliticsModel.GetInfluenceRequiredToOverrideKingdomDecision(this.KingdomDecisionMaker.PossibleOutcomes.MaxBy<DecisionOutcome, float>((DecisionOutcome o) => o.WinChance), this._currentSelectedOption.Option, this._decision);
					}
					if (this._currentSelectedOption.CurrentSupportWeight != Supporter.SupportWeights.Choose)
					{
						return (float)this.KingdomDecisionMaker.GetInfluenceCostOfOutcome(this._currentSelectedOption.Option, Clan.PlayerClan, this._currentSelectedOption.CurrentSupportWeight);
					}
				}
				return 0f;
			}
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0002C320 File Offset: 0x0002A520
		public DecisionItemBaseVM(KingdomDecision decision, Action onDecisionOver)
		{
			this._decision = decision;
			this._onDecisionOver = onDecisionOver;
			this.DecisionType = 0;
			this.DecisionOptionsList = new MBBindingList<DecisionOptionVM>();
			this.EndDecisionHint = new HintViewModel();
			CampaignEvents.KingdomDecisionConcluded.AddNonSerializedListener(this, new Action<KingdomDecision, DecisionOutcome, bool>(this.OnKingdomDecisionConcluded));
			this.RefreshValues();
			this.InitValues();
			Game game = Game.Current;
			if (game == null)
			{
				return;
			}
			EventManager eventManager = game.EventManager;
			if (eventManager == null)
			{
				return;
			}
			eventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0002C3B8 File Offset: 0x0002A5B8
		private void OnKingdomDecisionConcluded(KingdomDecision decision, DecisionOutcome outcome, bool isPlayerInvolved)
		{
			if (decision == this._decision)
			{
				this.IsKingsDecisionOver = true;
				this.CurrentStageIndex = 1;
				foreach (DecisionOptionVM decisionOptionVM in this.DecisionOptionsList)
				{
					if (decisionOptionVM.Option == outcome)
					{
						decisionOptionVM.IsKingsOutcome = true;
					}
					decisionOptionVM.AfterKingChooseOutcome();
				}
			}
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0002C42C File Offset: 0x0002A62C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			GameTexts.SetVariable("TOTAL_INFLUENCE", MathF.Round(Hero.MainHero.Clan.Influence));
			this.TotalInfluenceText = GameTexts.FindText("str_total_influence", null).ToString();
			this.RefreshInfluenceCost();
			MBBindingList<DecisionOptionVM> decisionOptionsList = this.DecisionOptionsList;
			if (decisionOptionsList == null)
			{
				return;
			}
			decisionOptionsList.ApplyActionOnAllItems(delegate(DecisionOptionVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0002C4C0 File Offset: 0x0002A6C0
		protected virtual void InitValues()
		{
			this.DecisionOptionsList.Clear();
			this.KingdomDecisionMaker = new KingdomElection(this._decision);
			this.KingdomDecisionMaker.StartElection();
			this.CurrentStageIndex = ((!this.KingdomDecisionMaker.IsPlayerChooser) ? 0 : 1);
			this.IsPlayerSupporter = !this.KingdomDecisionMaker.IsPlayerChooser;
			this.KingdomDecisionMaker.DetermineOfficialSupport();
			foreach (DecisionOutcome decisionOutcome in this.KingdomDecisionMaker.PossibleOutcomes)
			{
				DecisionOptionVM decisionOptionVM = new DecisionOptionVM(decisionOutcome, this._decision, this.KingdomDecisionMaker, new Action<DecisionOptionVM>(this.OnChangeVote), new Action<DecisionOptionVM>(this.OnSupportStrengthChange))
				{
					WinPercentage = MathF.Round(decisionOutcome.WinChance * 100f),
					InitialPercentage = MathF.Round(decisionOutcome.WinChance * 100f)
				};
				this.DecisionOptionsList.Add(decisionOptionVM);
			}
			DecisionOptionVM decisionOptionVM2 = new DecisionOptionVM(null, null, this.KingdomDecisionMaker, new Action<DecisionOptionVM>(this.OnChangeVote), new Action<DecisionOptionVM>(this.OnSupportStrengthChange));
			this.DecisionOptionsList.Add(decisionOptionVM2);
			this.TitleText = this.KingdomDecisionMaker.GetTitle().ToString();
			this.DescriptionText = this.KingdomDecisionMaker.GetDescription().ToString();
			this.RefreshInfluenceCost();
			this.RefreshCanEndDecision();
			this.RefreshRelationChangeText();
			this.IsActive = true;
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0002C64C File Offset: 0x0002A84C
		private void OnChangeVote(DecisionOptionVM target)
		{
			if (this._currentSelectedOption != target)
			{
				if (this._currentSelectedOption != null)
				{
					this._currentSelectedOption.IsSelected = false;
				}
				this._currentSelectedOption = target;
				this._currentSelectedOption.IsSelected = true;
				if (this._currentSelectedOption.IsOptionForAbstain && !this.IsPlayerSupporter)
				{
					this.KingdomDecisionMaker.OnPlayerAbstainedAsRuler();
				}
				else
				{
					this.KingdomDecisionMaker.OnPlayerSupport(this._currentSelectedOption.Option, this._currentSelectedOption.CurrentSupportWeight);
				}
				this.RefreshWinPercentages();
				this.RefreshInfluenceCost();
				this.RefreshCanEndDecision();
				this.RefreshRelationChangeText();
			}
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0002C6E7 File Offset: 0x0002A8E7
		private void OnSupportStrengthChange(DecisionOptionVM option)
		{
			this.RefreshWinPercentages();
			this.RefreshCanEndDecision();
			this.RefreshRelationChangeText();
			this.RefreshInfluenceCost();
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0002C704 File Offset: 0x0002A904
		private void RefreshWinPercentages()
		{
			this.KingdomDecisionMaker.DetermineOfficialSupport();
			using (List<DecisionOutcome>.Enumerator enumerator = this.KingdomDecisionMaker.PossibleOutcomes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DecisionOutcome option = enumerator.Current;
					DecisionOptionVM decisionOptionVM = this.DecisionOptionsList.FirstOrDefault<DecisionOptionVM>((DecisionOptionVM c) => c.Option == option);
					if (decisionOptionVM == null)
					{
						Debug.FailedAssert("Couldn't find option to update win chance for!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\KingdomManagement\\Decisions\\ItemTypes\\DecisionItemBaseVM.cs", "RefreshWinPercentages", 190);
					}
					else
					{
						decisionOptionVM.WinPercentage = (int)MathF.Round(option.WinChance * 100f, 2);
					}
				}
			}
			int num = this.DecisionOptionsList.Where<DecisionOptionVM>((DecisionOptionVM d) => !d.IsOptionForAbstain).Sum<DecisionOptionVM>((DecisionOptionVM d) => d.WinPercentage);
			if (num != 100)
			{
				int num2 = 100 - num;
				List<DecisionOptionVM> list = this.DecisionOptionsList.Where<DecisionOptionVM>((DecisionOptionVM opt) => opt.Sponsor != null).ToList<DecisionOptionVM>();
				int num3 = list.Select<DecisionOptionVM, int>((DecisionOptionVM opt) => opt.WinPercentage).Sum();
				if (num3 == 0)
				{
					int num4 = num2 / list.Count;
					foreach (DecisionOptionVM decisionOptionVM2 in list)
					{
						decisionOptionVM2.WinPercentage += num4;
					}
					list[0].WinPercentage += num2 - num4 * list.Count;
					return;
				}
				int num5 = 0;
				foreach (DecisionOptionVM decisionOptionVM3 in list.Where<DecisionOptionVM>((DecisionOptionVM opt) => opt.WinPercentage > 0).ToList<DecisionOptionVM>())
				{
					int num6 = MathF.Floor((float)num2 * ((float)decisionOptionVM3.WinPercentage / (float)num3));
					decisionOptionVM3.WinPercentage += num6;
					num5 += num6;
				}
				list[0].WinPercentage += num2 - num5;
			}
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0002C9A0 File Offset: 0x0002ABA0
		private void RefreshInfluenceCost()
		{
			if (this._currentInfluenceCost > 0f)
			{
				GameTexts.SetVariable("AMOUNT", this._currentInfluenceCost);
				GameTexts.SetVariable("INFLUENCE_ICON", "{=!}<img src=\"General\\Icons\\Influence@2x\" extend=\"5\">");
				this.InfluenceCostText = GameTexts.FindText(this.IsPlayerSupporter ? "str_decision_influence_cost" : "str_decision_ruler_influence_cost", null).ToString();
				return;
			}
			this.InfluenceCostText = "";
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0002CA0C File Offset: 0x0002AC0C
		private void RefreshRelationChangeText()
		{
			this.RelationChangeText = "";
			DecisionOptionVM currentSelectedOption = this._currentSelectedOption;
			if (currentSelectedOption != null && !currentSelectedOption.IsOptionForAbstain)
			{
				DecisionOptionVM currentSelectedOption2 = this._currentSelectedOption;
				if (currentSelectedOption2 == null || currentSelectedOption2.CurrentSupportWeight > Supporter.SupportWeights.Choose)
				{
					foreach (DecisionOptionVM decisionOptionVM in this.DecisionOptionsList)
					{
						DecisionOutcome option = decisionOptionVM.Option;
						if (((option != null) ? option.SponsorClan : null) != null && decisionOptionVM.Option.SponsorClan != Clan.PlayerClan)
						{
							bool flag = this._currentSelectedOption == decisionOptionVM;
							GameTexts.SetVariable("HERO_NAME", decisionOptionVM.Option.SponsorClan.Leader.EncyclopediaLinkWithName);
							string text = (flag ? GameTexts.FindText("str_decision_relation_increase", null).ToString() : GameTexts.FindText("str_decision_relation_decrease", null).ToString());
							if (string.IsNullOrEmpty(this.RelationChangeText))
							{
								this.RelationChangeText = text;
							}
							else
							{
								GameTexts.SetVariable("newline", "\n");
								GameTexts.SetVariable("STR1", this.RelationChangeText);
								GameTexts.SetVariable("STR2", text);
								this.RelationChangeText = GameTexts.FindText("str_string_newline_string", null).ToString();
							}
						}
					}
				}
			}
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0002CB68 File Offset: 0x0002AD68
		private void RefreshCanEndDecision()
		{
			bool flag = this._currentSelectedOption != null && (!this.IsPlayerSupporter || this._currentSelectedOption.CurrentSupportWeight > Supporter.SupportWeights.Choose);
			bool flag2 = this._currentInfluenceCost <= Clan.PlayerClan.Influence || this._currentInfluenceCost == 0f;
			DecisionOptionVM currentSelectedOption = this._currentSelectedOption;
			bool flag3 = currentSelectedOption != null && currentSelectedOption.IsOptionForAbstain;
			this.CanEndDecision = !this._finalSelectionDone && (flag3 || (flag && flag2));
			if (this.CanEndDecision)
			{
				this.EndDecisionHint.HintText = TextObject.GetEmpty();
				return;
			}
			if (flag)
			{
				if (!flag2)
				{
					this.EndDecisionHint.HintText = GameTexts.FindText("str_decision_not_enough_influence", null);
				}
				return;
			}
			if (this.IsPlayerSupporter)
			{
				this.EndDecisionHint.HintText = GameTexts.FindText("str_decision_need_to_select_an_option_and_support", null);
				return;
			}
			this.EndDecisionHint.HintText = GameTexts.FindText("str_decision_need_to_select_an_outcome", null);
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0002CC55 File Offset: 0x0002AE55
		protected void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0002CC68 File Offset: 0x0002AE68
		protected void ExecuteShowStageTooltip()
		{
			if (!this.IsPlayerSupporter)
			{
				MBInformationManager.ShowHint(GameTexts.FindText("str_decision_second_stage_player_decider", null).ToString());
				return;
			}
			if (this.CurrentStageIndex == 0)
			{
				MBInformationManager.ShowHint(GameTexts.FindText("str_decision_first_stage_player_supporter", null).ToString());
				return;
			}
			MBInformationManager.ShowHint(GameTexts.FindText("str_decision_second_stage_player_supporter", null).ToString());
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0002CCC6 File Offset: 0x0002AEC6
		protected void ExecuteHideStageTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0002CCCD File Offset: 0x0002AECD
		public void ExecuteFinalSelection()
		{
			if (this.CanEndDecision)
			{
				this.KingdomDecisionMaker.ApplySelection();
				this._finalSelectionDone = true;
				this.RefreshCanEndDecision();
			}
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0002CCF0 File Offset: 0x0002AEF0
		protected void ExecuteDone()
		{
			TextObject chosenOutcomeText = this.KingdomDecisionMaker.GetChosenOutcomeText();
			this.IsActive = false;
			InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_decision_outcome", null).ToString(), chosenOutcomeText.ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", delegate
			{
				this._onDecisionOver();
			}, null, "", 0f, null, null, null), false, false);
			CampaignEvents.KingdomDecisionConcluded.ClearListeners(this);
			this._currentSelectedOption = null;
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0002CD75 File Offset: 0x0002AF75
		public override void OnFinalize()
		{
			base.OnFinalize();
			Game game = Game.Current;
			if (game == null)
			{
				return;
			}
			EventManager eventManager = game.EventManager;
			if (eventManager == null)
			{
				return;
			}
			eventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(this.OnTutorialNotificationElementIDChange));
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0002CDA4 File Offset: 0x0002AFA4
		private void OnTutorialNotificationElementIDChange(TutorialNotificationElementChangeEvent obj)
		{
			if (this._latestTutorialElementID != obj.NewNotificationElementID)
			{
				this._latestTutorialElementID = obj.NewNotificationElementID;
				if (this._isDecisionOptionsHighlightEnabled && this._latestTutorialElementID != this._decisionOptionsHighlightID)
				{
					this.SetOptionsHighlight(false);
					this._isDecisionOptionsHighlightEnabled = false;
					return;
				}
				if (!this._isDecisionOptionsHighlightEnabled && this._latestTutorialElementID == this._decisionOptionsHighlightID)
				{
					this.SetOptionsHighlight(true);
					this._isDecisionOptionsHighlightEnabled = true;
				}
			}
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0002CE24 File Offset: 0x0002B024
		private void SetOptionsHighlight(bool state)
		{
			for (int i = 0; i < this.DecisionOptionsList.Count; i++)
			{
				DecisionOptionVM decisionOptionVM = this.DecisionOptionsList[i];
				if (decisionOptionVM.CanBeChosen)
				{
					decisionOptionVM.IsHighlightEnabled = state;
				}
			}
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0002CE63 File Offset: 0x0002B063
		public void SetDoneInputKey(InputKeyItemVM inputKeyItemVM)
		{
			this.DoneInputKey = inputKeyItemVM;
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x0002CE6C File Offset: 0x0002B06C
		// (set) Token: 0x06000A39 RID: 2617 RVA: 0x0002CE74 File Offset: 0x0002B074
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

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x0002CE92 File Offset: 0x0002B092
		// (set) Token: 0x06000A3B RID: 2619 RVA: 0x0002CE9A File Offset: 0x0002B09A
		[DataSourceProperty]
		public HintViewModel EndDecisionHint
		{
			get
			{
				return this._endDecisionHint;
			}
			set
			{
				if (value != this._endDecisionHint)
				{
					this._endDecisionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "EndDecisionHint");
				}
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x0002CEB8 File Offset: 0x0002B0B8
		// (set) Token: 0x06000A3D RID: 2621 RVA: 0x0002CEC0 File Offset: 0x0002B0C0
		[DataSourceProperty]
		public int DecisionType
		{
			get
			{
				return this._decisionType;
			}
			set
			{
				if (value != this._decisionType)
				{
					this._decisionType = value;
					base.OnPropertyChangedWithValue(value, "DecisionType");
				}
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0002CEDE File Offset: 0x0002B0DE
		// (set) Token: 0x06000A3F RID: 2623 RVA: 0x0002CEE6 File Offset: 0x0002B0E6
		[DataSourceProperty]
		public string TotalInfluenceText
		{
			get
			{
				return this._totalInfluenceText;
			}
			set
			{
				if (value != this._totalInfluenceText)
				{
					this._totalInfluenceText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalInfluenceText");
				}
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x0002CF09 File Offset: 0x0002B109
		// (set) Token: 0x06000A41 RID: 2625 RVA: 0x0002CF11 File Offset: 0x0002B111
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

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x0002CF2F File Offset: 0x0002B12F
		// (set) Token: 0x06000A43 RID: 2627 RVA: 0x0002CF37 File Offset: 0x0002B137
		[DataSourceProperty]
		public int CurrentStageIndex
		{
			get
			{
				return this._currentStageIndex;
			}
			set
			{
				if (value != this._currentStageIndex)
				{
					this._currentStageIndex = value;
					base.OnPropertyChangedWithValue(value, "CurrentStageIndex");
				}
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x0002CF55 File Offset: 0x0002B155
		// (set) Token: 0x06000A45 RID: 2629 RVA: 0x0002CF5D File Offset: 0x0002B15D
		[DataSourceProperty]
		public bool IsPlayerSupporter
		{
			get
			{
				return this._isPlayerSupporter;
			}
			set
			{
				if (value != this._isPlayerSupporter)
				{
					this._isPlayerSupporter = value;
					base.OnPropertyChangedWithValue(value, "IsPlayerSupporter");
				}
			}
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x0002CF7B File Offset: 0x0002B17B
		// (set) Token: 0x06000A47 RID: 2631 RVA: 0x0002CF83 File Offset: 0x0002B183
		[DataSourceProperty]
		public bool CanEndDecision
		{
			get
			{
				return this._canEndDecision;
			}
			set
			{
				if (value != this._canEndDecision)
				{
					this._canEndDecision = value;
					base.OnPropertyChangedWithValue(value, "CanEndDecision");
				}
			}
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x0002CFA1 File Offset: 0x0002B1A1
		// (set) Token: 0x06000A49 RID: 2633 RVA: 0x0002CFA9 File Offset: 0x0002B1A9
		[DataSourceProperty]
		public bool IsKingsDecisionOver
		{
			get
			{
				return this._isKingsDecisionOver;
			}
			set
			{
				if (value != this._isKingsDecisionOver)
				{
					this._isKingsDecisionOver = value;
					base.OnPropertyChangedWithValue(value, "IsKingsDecisionOver");
				}
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0002CFC7 File Offset: 0x0002B1C7
		// (set) Token: 0x06000A4B RID: 2635 RVA: 0x0002CFCF File Offset: 0x0002B1CF
		[DataSourceProperty]
		public string RelationChangeText
		{
			get
			{
				return this._increaseRelationText;
			}
			set
			{
				if (value != this._increaseRelationText)
				{
					this._increaseRelationText = value;
					base.OnPropertyChangedWithValue<string>(value, "RelationChangeText");
				}
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x0002CFF2 File Offset: 0x0002B1F2
		// (set) Token: 0x06000A4D RID: 2637 RVA: 0x0002CFFA File Offset: 0x0002B1FA
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0002D01D File Offset: 0x0002B21D
		// (set) Token: 0x06000A4F RID: 2639 RVA: 0x0002D025 File Offset: 0x0002B225
		[DataSourceProperty]
		public string TitleText
		{
			get
			{
				return this._titleText;
			}
			set
			{
				if (value != this._titleText)
				{
					this._titleText = value;
					base.OnPropertyChangedWithValue<string>(value, "TitleText");
				}
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0002D048 File Offset: 0x0002B248
		// (set) Token: 0x06000A51 RID: 2641 RVA: 0x0002D050 File Offset: 0x0002B250
		[DataSourceProperty]
		public string DoneText
		{
			get
			{
				return this._doneText;
			}
			set
			{
				if (value != this._doneText)
				{
					this._doneText = value;
					base.OnPropertyChangedWithValue<string>(value, "DoneText");
				}
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x0002D073 File Offset: 0x0002B273
		// (set) Token: 0x06000A53 RID: 2643 RVA: 0x0002D07B File Offset: 0x0002B27B
		[DataSourceProperty]
		public string InfluenceCostText
		{
			get
			{
				return this._influenceCostText;
			}
			set
			{
				if (value != this._influenceCostText)
				{
					this._influenceCostText = value;
					base.OnPropertyChangedWithValue<string>(value, "InfluenceCostText");
				}
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x0002D09E File Offset: 0x0002B29E
		// (set) Token: 0x06000A55 RID: 2645 RVA: 0x0002D0A6 File Offset: 0x0002B2A6
		[DataSourceProperty]
		public MBBindingList<DecisionOptionVM> DecisionOptionsList
		{
			get
			{
				return this._decisionOptionsList;
			}
			set
			{
				if (value != this._decisionOptionsList)
				{
					this._decisionOptionsList = value;
					base.OnPropertyChangedWithValue<MBBindingList<DecisionOptionVM>>(value, "DecisionOptionsList");
				}
			}
		}

		// Token: 0x04000477 RID: 1143
		protected readonly KingdomDecision _decision;

		// Token: 0x04000478 RID: 1144
		private readonly Action _onDecisionOver;

		// Token: 0x04000479 RID: 1145
		private DecisionOptionVM _currentSelectedOption;

		// Token: 0x0400047A RID: 1146
		private bool _finalSelectionDone;

		// Token: 0x0400047B RID: 1147
		private bool _isDecisionOptionsHighlightEnabled;

		// Token: 0x0400047C RID: 1148
		private string _decisionOptionsHighlightID = "DecisionOptions";

		// Token: 0x0400047D RID: 1149
		private string _latestTutorialElementID;

		// Token: 0x0400047E RID: 1150
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400047F RID: 1151
		private int _decisionType;

		// Token: 0x04000480 RID: 1152
		private bool _isActive;

		// Token: 0x04000481 RID: 1153
		private bool _isPlayerSupporter;

		// Token: 0x04000482 RID: 1154
		private bool _canEndDecision;

		// Token: 0x04000483 RID: 1155
		private bool _isKingsDecisionOver;

		// Token: 0x04000484 RID: 1156
		private int _currentStageIndex = -1;

		// Token: 0x04000485 RID: 1157
		private string _titleText;

		// Token: 0x04000486 RID: 1158
		private string _doneText;

		// Token: 0x04000487 RID: 1159
		private string _descriptionText;

		// Token: 0x04000488 RID: 1160
		private string _influenceCostText;

		// Token: 0x04000489 RID: 1161
		private string _totalInfluenceText;

		// Token: 0x0400048A RID: 1162
		private string _increaseRelationText;

		// Token: 0x0400048B RID: 1163
		private HintViewModel _endDecisionHint;

		// Token: 0x0400048C RID: 1164
		private MBBindingList<DecisionOptionVM> _decisionOptionsList;

		// Token: 0x020001E3 RID: 483
		protected enum DecisionTypes
		{
			// Token: 0x0400116E RID: 4462
			Default,
			// Token: 0x0400116F RID: 4463
			Settlement,
			// Token: 0x04001170 RID: 4464
			ExpelClan,
			// Token: 0x04001171 RID: 4465
			Policy,
			// Token: 0x04001172 RID: 4466
			DeclareWar,
			// Token: 0x04001173 RID: 4467
			MakePeace,
			// Token: 0x04001174 RID: 4468
			KingSelection,
			// Token: 0x04001175 RID: 4469
			StartAlliance,
			// Token: 0x04001176 RID: 4470
			AcceptCallToWarAgreement,
			// Token: 0x04001177 RID: 4471
			ProposeCallToWarAgreement,
			// Token: 0x04001178 RID: 4472
			Trade
		}
	}
}
