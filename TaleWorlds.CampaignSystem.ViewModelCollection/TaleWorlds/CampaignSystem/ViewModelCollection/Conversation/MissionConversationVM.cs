using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Conversation.Persuasion;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Conversation
{
	// Token: 0x0200011E RID: 286
	public class MissionConversationVM : ViewModel
	{
		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x060019C9 RID: 6601 RVA: 0x00061F6F File Offset: 0x0006016F
		// (set) Token: 0x060019CA RID: 6602 RVA: 0x00061F77 File Offset: 0x00060177
		public bool SelectedAnOptionOrLinkThisFrame { get; set; }

		// Token: 0x060019CB RID: 6603 RVA: 0x00061F80 File Offset: 0x00060180
		public MissionConversationVM(Func<string> getContinueInputText, bool isLinksDisabled = false)
		{
			this.AnswerList = new MBBindingList<ConversationItemVM>();
			this.AttackerParties = new MBBindingList<ConversationAggressivePartyItemVM>();
			this.DefenderParties = new MBBindingList<ConversationAggressivePartyItemVM>();
			this._conversationManager = Campaign.Current.ConversationManager;
			this._getContinueInputText = getContinueInputText;
			this._isLinksDisabled = isLinksDisabled;
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Combine(Input.OnGamepadActiveStateChanged, new Action(this.RefreshValues));
			CampaignEvents.PersuasionProgressCommittedEvent.AddNonSerializedListener(this, new Action<Tuple<PersuasionOptionArgs, PersuasionOptionResult>>(this.OnPersuasionProgress));
			this.Persuasion = new PersuasionVM(this._conversationManager);
			if (this._conversationManager.SpeakerAgent != null && (CharacterObject)this._conversationManager.SpeakerAgent.Character != null && ((CharacterObject)this._conversationManager.SpeakerAgent.Character).IsHero && this._conversationManager.SpeakerAgent.Character != CharacterObject.PlayerCharacter)
			{
				Hero heroObject = ((CharacterObject)this._conversationManager.SpeakerAgent.Character).HeroObject;
				this.Relation = (int)heroObject.GetRelationWithPlayer();
			}
			this.IsAggressive = Campaign.Current.CurrentConversationContext == ConversationContext.PartyEncounter && this._conversationManager.ConversationParty != null && FactionManager.IsAtWarAgainstFaction(this._conversationManager.ConversationParty.MapFaction, Hero.MainHero.MapFaction);
			this.OnAgressiveStateUpdated();
			this.ExecuteSetCurrentAnswer(null);
			this.RefreshValues();
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x000620F0 File Offset: 0x000602F0
		private void OnPersuasionProgress(Tuple<PersuasionOptionArgs, PersuasionOptionResult> result)
		{
			PersuasionVM persuasion = this.Persuasion;
			if (persuasion != null)
			{
				persuasion.OnPersuasionProgress(result);
			}
			this.AnswerList.ApplyActionOnAllItems(delegate(ConversationItemVM a)
			{
				a.OnPersuasionProgress(result);
			});
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x00062138 File Offset: 0x00060338
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ContinueText = this._getContinueInputText();
			this.MoreOptionText = GameTexts.FindText("str_more_brackets", null).ToString();
			this.PersuasionText = GameTexts.FindText("str_persuasion", null).ToString();
			this.RelationHint = new HintViewModel(GameTexts.FindText("str_tooltip_label_relation", null), null);
			this.GoldHint = new HintViewModel(new TextObject("{=o5G8A8ZH}Your Denars", null), null);
			this._answerList.ApplyActionOnAllItems(delegate(ConversationItemVM x)
			{
				x.RefreshValues();
			});
			this._defenderParties.ApplyActionOnAllItems(delegate(ConversationAggressivePartyItemVM x)
			{
				x.RefreshValues();
			});
			this._attackerParties.ApplyActionOnAllItems(delegate(ConversationAggressivePartyItemVM x)
			{
				x.RefreshValues();
			});
			this._defenderLeader.RefreshValues();
			this._attackerLeader.RefreshValues();
			this._currentSelectedAnswer.RefreshValues();
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x00062258 File Offset: 0x00060458
		public void Tick(float dt)
		{
			this.IsAggressive = Campaign.Current.CurrentConversationContext == ConversationContext.PartyEncounter && this._conversationManager.ConversationParty != null && FactionManager.IsAtWarAgainstFaction(this._conversationManager.ConversationParty.MapFaction, Hero.MainHero.MapFaction);
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x000622A8 File Offset: 0x000604A8
		private void OnAgressiveStateUpdated()
		{
			if (this.IsAggressive)
			{
				List<MobileParty> list = new List<MobileParty>();
				List<MobileParty> list2 = new List<MobileParty>();
				this.DefenderParties.Clear();
				this.AttackerParties.Clear();
				MobileParty conversationParty = this._conversationManager.ConversationParty;
				MobileParty mainParty = MobileParty.MainParty;
				if (PlayerEncounter.PlayerIsAttacker)
				{
					list2.Add(mainParty);
					list.Add(conversationParty);
					PlayerEncounter.Current.FindAllNpcPartiesWhoWillJoinEvent(list2, list);
				}
				else
				{
					list2.Add(conversationParty);
					list.Add(mainParty);
					PlayerEncounter.Current.FindAllNpcPartiesWhoWillJoinEvent(list, list2);
				}
				this.AttackerLeader = new ConversationAggressivePartyItemVM(PlayerEncounter.PlayerIsAttacker ? mainParty : conversationParty, null);
				this.DefenderLeader = new ConversationAggressivePartyItemVM(PlayerEncounter.PlayerIsAttacker ? conversationParty : mainParty, null);
				double num = 0.0;
				double num2 = 0.0;
				num += (double)this.DefenderLeader.Party.Party.CalculateCurrentStrength();
				num2 += (double)this.AttackerLeader.Party.Party.CalculateCurrentStrength();
				foreach (MobileParty mobileParty in list)
				{
					if (mobileParty != conversationParty && mobileParty != mainParty)
					{
						num += (double)mobileParty.Party.CalculateCurrentStrength();
						this.DefenderParties.Add(new ConversationAggressivePartyItemVM(mobileParty, null));
					}
				}
				foreach (MobileParty mobileParty2 in list2)
				{
					if (mobileParty2 != conversationParty && mobileParty2 != mainParty)
					{
						num2 += (double)mobileParty2.Party.CalculateCurrentStrength();
						this.AttackerParties.Add(new ConversationAggressivePartyItemVM(mobileParty2, null));
					}
				}
				string text;
				if (this.DefenderLeader.Party.MapFaction != null && this.DefenderLeader.Party.MapFaction is Kingdom)
				{
					text = Color.FromUint(((Kingdom)this.DefenderLeader.Party.MapFaction).PrimaryBannerColor).ToString();
				}
				else
				{
					text = Color.FromUint(this.DefenderLeader.Party.MapFaction.Banner.GetPrimaryColor()).ToString();
				}
				string text2;
				if (this.AttackerLeader.Party.MapFaction != null && this.AttackerLeader.Party.MapFaction is Kingdom)
				{
					text2 = Color.FromUint(((Kingdom)this.AttackerLeader.Party.MapFaction).PrimaryBannerColor).ToString();
				}
				else
				{
					text2 = Color.FromUint(this.AttackerLeader.Party.MapFaction.Banner.GetPrimaryColor()).ToString();
				}
				if (!list2.AnyQ<MobileParty>((MobileParty p) => p.IsInfoHidden))
				{
					if (!list.AnyQ<MobileParty>((MobileParty p) => p.IsInfoHidden))
					{
						goto IL_0345;
					}
				}
				if (PlayerEncounter.PlayerIsAttacker)
				{
					num2 = 0.0;
					num = 1.0;
				}
				else
				{
					num2 = 1.0;
					num = 0.0;
				}
				IL_0345:
				this.PowerComparer = new PowerLevelComparer(num, num2);
				this.PowerComparer.SetColors(text, text2);
				return;
			}
			this.DefenderLeader = new ConversationAggressivePartyItemVM(null, null);
			this.AttackerLeader = new ConversationAggressivePartyItemVM(null, null);
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x00062650 File Offset: 0x00060850
		public void OnConversationContinue()
		{
			if (ConversationManager.GetPersuasionIsActive() && (!ConversationManager.GetPersuasionIsActive() || this.IsPersuading))
			{
				List<ConversationSentenceOption> curOptions = this._conversationManager.CurOptions;
				if (((curOptions != null) ? curOptions.Count : 0) > 1)
				{
					return;
				}
			}
			this.Refresh();
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x00062688 File Offset: 0x00060888
		public void ExecuteLink(string link)
		{
			if (!this._isLinksDisabled)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(link);
			}
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x000626A4 File Offset: 0x000608A4
		public void ExecuteConversedHeroLink()
		{
			CharacterObject characterObject;
			if (!this._isLinksDisabled && (characterObject = this._currentDialogCharacter as CharacterObject) != null)
			{
				EncyclopediaManager encyclopediaManager = Campaign.Current.EncyclopediaManager;
				Hero heroObject = characterObject.HeroObject;
				encyclopediaManager.GoToLink(((heroObject != null) ? heroObject.EncyclopediaLink : null) ?? characterObject.EncyclopediaLink);
				this.SelectedAnOptionOrLinkThisFrame = true;
			}
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x000626FC File Offset: 0x000608FC
		public void Refresh()
		{
			this.ExecuteCloseTooltip();
			this._isProcessingOption = false;
			this.IsLoadingOver = false;
			IReadOnlyList<IAgent> conversationAgents = this._conversationManager.ConversationAgents;
			if (conversationAgents != null && conversationAgents.Count > 0)
			{
				this._currentDialogCharacter = this._conversationManager.SpeakerAgent.Character;
				this.CurrentCharacterNameLbl = this._currentDialogCharacter.Name.ToString();
				this.IsCurrentCharacterValidInEncyclopedia = false;
				if (((CharacterObject)this._currentDialogCharacter).IsHero && this._currentDialogCharacter != CharacterObject.PlayerCharacter)
				{
					this.MinRelation = Campaign.Current.Models.DiplomacyModel.MinRelationLimit;
					this.MaxRelation = Campaign.Current.Models.DiplomacyModel.MaxRelationLimit;
					Hero heroObject = ((CharacterObject)this._currentDialogCharacter).HeroObject;
					if (heroObject.IsLord && !heroObject.IsMinorFactionHero)
					{
						Clan clan = heroObject.Clan;
						if (((clan != null) ? clan.Leader : null) == heroObject)
						{
							Clan clan2 = heroObject.Clan;
							if (((clan2 != null) ? clan2.Kingdom : null) != null)
							{
								string stringId = heroObject.MapFaction.Culture.StringId;
								TextObject textObject;
								if (GameTexts.TryGetText("str_faction_noble_name_with_title", out textObject, stringId))
								{
									if (heroObject.Clan.Kingdom.Leader == heroObject)
									{
										textObject = GameTexts.FindText("str_faction_ruler_name_with_title", stringId);
									}
									StringHelpers.SetCharacterProperties("RULER", (CharacterObject)this._currentDialogCharacter, null, false);
									this.CurrentCharacterNameLbl = textObject.ToString();
								}
							}
						}
					}
					this.IsRelationEnabled = true;
					this.Relation = Hero.MainHero.GetRelation(heroObject);
					GameTexts.SetVariable("NUM", this.Relation.ToString());
					if (this.Relation > 0)
					{
						this.RelationText = "+" + this.Relation;
					}
					else if (this.Relation < 0)
					{
						this.RelationText = "-" + MathF.Abs(this.Relation);
					}
					else
					{
						this.RelationText = this.Relation.ToString();
					}
					if (heroObject.Clan == null)
					{
						this.ConversedHeroBanner = new BannerImageIdentifierVM(null, false);
						this.IsRelationEnabled = false;
						this.IsBannerEnabled = false;
					}
					else
					{
						this.ConversedHeroBanner = ((heroObject != null) ? new BannerImageIdentifierVM(heroObject.ClanBanner, false) : new BannerImageIdentifierVM(null, false));
						TextObject textObject2 = ((heroObject != null) ? heroObject.Clan.Name : TextObject.GetEmpty());
						this.FactionHint = new HintViewModel(textObject2, null);
						this.IsBannerEnabled = true;
					}
					this.IsCurrentCharacterValidInEncyclopedia = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Hero)).IsValidEncyclopediaItem(heroObject);
				}
				else
				{
					this.ConversedHeroBanner = new BannerImageIdentifierVM(null, false);
					this.IsRelationEnabled = false;
					this.IsBannerEnabled = false;
					this.IsCurrentCharacterValidInEncyclopedia = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(CharacterObject)).IsValidEncyclopediaItem((CharacterObject)this._conversationManager.SpeakerAgent.Character);
				}
			}
			this.DialogText = this._conversationManager.CurrentSentenceText;
			this.AnswerList.Clear();
			MissionConversationVM._isCurrentlyPlayerSpeaking = this._currentDialogCharacter == Hero.MainHero.CharacterObject;
			this._conversationManager.GetPlayerSentenceOptions();
			List<ConversationSentenceOption> curOptions = this._conversationManager.CurOptions;
			int num = ((curOptions != null) ? curOptions.Count : 0);
			if (num > 0 && !MissionConversationVM._isCurrentlyPlayerSpeaking)
			{
				for (int i = 0; i < num; i++)
				{
					this.AnswerList.Add(new ConversationItemVM(new Action<int>(this.OnSelectOption), new Action(this.OnReadyToContinue), new Action<ConversationItemVM>(this.ExecuteSetCurrentAnswer), i));
				}
			}
			this.GoldText = CampaignUIHelper.GetAbbreviatedValueTextFromValue(Hero.MainHero.Gold);
			this.IsPersuading = ConversationManager.GetPersuasionIsActive();
			if (this.IsPersuading)
			{
				this.CurrentSelectedAnswer = new ConversationItemVM();
			}
			this.IsLoadingOver = true;
			PersuasionVM persuasion = this.Persuasion;
			if (persuasion == null)
			{
				return;
			}
			persuasion.RefreshPersusasion();
		}

		// Token: 0x060019D4 RID: 6612 RVA: 0x00062AED File Offset: 0x00060CED
		private void OnReadyToContinue()
		{
			this.Refresh();
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x00062AF8 File Offset: 0x00060CF8
		private void ExecuteDefenderTooltip()
		{
			if (PlayerEncounter.PlayerIsDefender)
			{
				InformationManager.ShowTooltip(typeof(List<MobileParty>), new object[] { 0 });
				return;
			}
			InformationManager.ShowTooltip(typeof(List<MobileParty>), new object[] { 1 });
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x00062B49 File Offset: 0x00060D49
		public void ExecuteCloseTooltip()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x00062B50 File Offset: 0x00060D50
		public void ExecuteHeroTooltip()
		{
			CharacterObject characterObject = (CharacterObject)this._currentDialogCharacter;
			if (characterObject != null && characterObject.IsHero)
			{
				InformationManager.ShowTooltip(typeof(Hero), new object[] { characterObject.HeroObject, true });
			}
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x00062B9C File Offset: 0x00060D9C
		private void ExecuteAttackerTooltip()
		{
			if (PlayerEncounter.PlayerIsAttacker)
			{
				InformationManager.ShowTooltip(typeof(List<MobileParty>), new object[] { 0 });
				return;
			}
			InformationManager.ShowTooltip(typeof(List<MobileParty>), new object[] { 1 });
		}

		// Token: 0x060019D9 RID: 6617 RVA: 0x00062BF0 File Offset: 0x00060DF0
		private void ExecuteHeroInfo()
		{
			if (this._conversationManager.ListenerAgent.Character == Hero.MainHero.CharacterObject)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(Hero.MainHero.EncyclopediaLink);
				return;
			}
			if (CharacterObject.OneToOneConversationCharacter.IsHero)
			{
				Campaign.Current.EncyclopediaManager.GoToLink(CharacterObject.OneToOneConversationCharacter.HeroObject.EncyclopediaLink);
				return;
			}
			Campaign.Current.EncyclopediaManager.GoToLink(CharacterObject.OneToOneConversationCharacter.EncyclopediaLink);
		}

		// Token: 0x060019DA RID: 6618 RVA: 0x00062C77 File Offset: 0x00060E77
		private void OnSelectOption(int optionIndex)
		{
			if (!this._isProcessingOption)
			{
				this._isProcessingOption = true;
				this._conversationManager.DoOption(optionIndex);
				PersuasionVM persuasion = this.Persuasion;
				if (persuasion != null)
				{
					persuasion.RefreshPersusasion();
				}
				this.SelectedAnOptionOrLinkThisFrame = true;
			}
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x00062CAC File Offset: 0x00060EAC
		public void ExecuteFinalizeSelection()
		{
			this.Refresh();
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x00062CB4 File Offset: 0x00060EB4
		public void ExecuteContinue()
		{
			Debug.Print("ExecuteContinue", 0, Debug.DebugColor.White, 17592186044416UL);
			this._conversationManager.ContinueConversation();
			this._isProcessingOption = false;
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x00062CDE File Offset: 0x00060EDE
		private void ExecuteSetCurrentAnswer(ConversationItemVM _answer)
		{
			this.Persuasion.SetCurrentOption((_answer != null) ? _answer.PersuasionItem : null);
			if (_answer != null)
			{
				this.CurrentSelectedAnswer = _answer;
				return;
			}
			this.CurrentSelectedAnswer = new ConversationItemVM();
		}

		// Token: 0x060019DE RID: 6622 RVA: 0x00062D10 File Offset: 0x00060F10
		public override void OnFinalize()
		{
			base.OnFinalize();
			CampaignEvents.PersuasionProgressCommittedEvent.ClearListeners(this);
			Input.OnGamepadActiveStateChanged = (Action)Delegate.Remove(Input.OnGamepadActiveStateChanged, new Action(this.RefreshValues));
			PersuasionVM persuasion = this.Persuasion;
			if (persuasion == null)
			{
				return;
			}
			persuasion.OnFinalize();
		}

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x060019DF RID: 6623 RVA: 0x00062D5F File Offset: 0x00060F5F
		// (set) Token: 0x060019E0 RID: 6624 RVA: 0x00062D67 File Offset: 0x00060F67
		[DataSourceProperty]
		public PersuasionVM Persuasion
		{
			get
			{
				return this._persuasion;
			}
			set
			{
				if (value != this._persuasion)
				{
					this._persuasion = value;
					base.OnPropertyChangedWithValue<PersuasionVM>(value, "Persuasion");
				}
			}
		}

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x060019E1 RID: 6625 RVA: 0x00062D85 File Offset: 0x00060F85
		// (set) Token: 0x060019E2 RID: 6626 RVA: 0x00062D8D File Offset: 0x00060F8D
		[DataSourceProperty]
		public PowerLevelComparer PowerComparer
		{
			get
			{
				return this._powerComparer;
			}
			set
			{
				if (value != this._powerComparer)
				{
					this._powerComparer = value;
					base.OnPropertyChangedWithValue<PowerLevelComparer>(value, "PowerComparer");
				}
			}
		}

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x060019E3 RID: 6627 RVA: 0x00062DAB File Offset: 0x00060FAB
		// (set) Token: 0x060019E4 RID: 6628 RVA: 0x00062DB3 File Offset: 0x00060FB3
		[DataSourceProperty]
		public int Relation
		{
			get
			{
				return this._relation;
			}
			set
			{
				if (this._relation != value)
				{
					this._relation = value;
					base.OnPropertyChangedWithValue(value, "Relation");
				}
			}
		}

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x060019E5 RID: 6629 RVA: 0x00062DD1 File Offset: 0x00060FD1
		// (set) Token: 0x060019E6 RID: 6630 RVA: 0x00062DD9 File Offset: 0x00060FD9
		[DataSourceProperty]
		public int MinRelation
		{
			get
			{
				return this._minRelation;
			}
			set
			{
				if (this._minRelation != value)
				{
					this._minRelation = value;
					base.OnPropertyChangedWithValue(value, "MinRelation");
				}
			}
		}

		// Token: 0x1700089E RID: 2206
		// (get) Token: 0x060019E7 RID: 6631 RVA: 0x00062DF7 File Offset: 0x00060FF7
		// (set) Token: 0x060019E8 RID: 6632 RVA: 0x00062DFF File Offset: 0x00060FFF
		[DataSourceProperty]
		public int MaxRelation
		{
			get
			{
				return this._maxRelation;
			}
			set
			{
				if (this._maxRelation != value)
				{
					this._maxRelation = value;
					base.OnPropertyChangedWithValue(value, "MaxRelation");
				}
			}
		}

		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x060019E9 RID: 6633 RVA: 0x00062E1D File Offset: 0x0006101D
		// (set) Token: 0x060019EA RID: 6634 RVA: 0x00062E25 File Offset: 0x00061025
		[DataSourceProperty]
		public ConversationAggressivePartyItemVM DefenderLeader
		{
			get
			{
				return this._defenderLeader;
			}
			set
			{
				if (value != this._defenderLeader)
				{
					this._defenderLeader = value;
					base.OnPropertyChangedWithValue<ConversationAggressivePartyItemVM>(value, "DefenderLeader");
				}
			}
		}

		// Token: 0x170008A0 RID: 2208
		// (get) Token: 0x060019EB RID: 6635 RVA: 0x00062E43 File Offset: 0x00061043
		// (set) Token: 0x060019EC RID: 6636 RVA: 0x00062E4B File Offset: 0x0006104B
		[DataSourceProperty]
		public ConversationAggressivePartyItemVM AttackerLeader
		{
			get
			{
				return this._attackerLeader;
			}
			set
			{
				if (value != this._attackerLeader)
				{
					this._attackerLeader = value;
					base.OnPropertyChangedWithValue<ConversationAggressivePartyItemVM>(value, "AttackerLeader");
				}
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x060019ED RID: 6637 RVA: 0x00062E69 File Offset: 0x00061069
		// (set) Token: 0x060019EE RID: 6638 RVA: 0x00062E71 File Offset: 0x00061071
		[DataSourceProperty]
		public MBBindingList<ConversationAggressivePartyItemVM> AttackerParties
		{
			get
			{
				return this._attackerParties;
			}
			set
			{
				if (value != this._attackerParties)
				{
					this._attackerParties = value;
					base.OnPropertyChangedWithValue<MBBindingList<ConversationAggressivePartyItemVM>>(value, "AttackerParties");
				}
			}
		}

		// Token: 0x170008A2 RID: 2210
		// (get) Token: 0x060019EF RID: 6639 RVA: 0x00062E8F File Offset: 0x0006108F
		// (set) Token: 0x060019F0 RID: 6640 RVA: 0x00062E97 File Offset: 0x00061097
		[DataSourceProperty]
		public MBBindingList<ConversationAggressivePartyItemVM> DefenderParties
		{
			get
			{
				return this._defenderParties;
			}
			set
			{
				if (value != this._defenderParties)
				{
					this._defenderParties = value;
					base.OnPropertyChangedWithValue<MBBindingList<ConversationAggressivePartyItemVM>>(value, "DefenderParties");
				}
			}
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x060019F1 RID: 6641 RVA: 0x00062EB5 File Offset: 0x000610B5
		// (set) Token: 0x060019F2 RID: 6642 RVA: 0x00062EBD File Offset: 0x000610BD
		[DataSourceProperty]
		public string MoreOptionText
		{
			get
			{
				return this._moreOptionText;
			}
			set
			{
				if (this._moreOptionText != value)
				{
					this._moreOptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "MoreOptionText");
				}
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x060019F3 RID: 6643 RVA: 0x00062EE0 File Offset: 0x000610E0
		// (set) Token: 0x060019F4 RID: 6644 RVA: 0x00062EE8 File Offset: 0x000610E8
		[DataSourceProperty]
		public string GoldText
		{
			get
			{
				return this._goldText;
			}
			set
			{
				if (this._goldText != value)
				{
					this._goldText = value;
					base.OnPropertyChangedWithValue<string>(value, "GoldText");
				}
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x060019F5 RID: 6645 RVA: 0x00062F0B File Offset: 0x0006110B
		// (set) Token: 0x060019F6 RID: 6646 RVA: 0x00062F13 File Offset: 0x00061113
		[DataSourceProperty]
		public string PersuasionText
		{
			get
			{
				return this._persuasionText;
			}
			set
			{
				if (this._persuasionText != value)
				{
					this._persuasionText = value;
					base.OnPropertyChangedWithValue<string>(value, "PersuasionText");
				}
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x060019F7 RID: 6647 RVA: 0x00062F36 File Offset: 0x00061136
		// (set) Token: 0x060019F8 RID: 6648 RVA: 0x00062F3E File Offset: 0x0006113E
		[DataSourceProperty]
		public bool IsCurrentCharacterValidInEncyclopedia
		{
			get
			{
				return this._isCurrentCharacterValidInEncyclopedia;
			}
			set
			{
				if (this._isCurrentCharacterValidInEncyclopedia != value)
				{
					this._isCurrentCharacterValidInEncyclopedia = value;
					base.OnPropertyChangedWithValue(value, "IsCurrentCharacterValidInEncyclopedia");
				}
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x060019F9 RID: 6649 RVA: 0x00062F5C File Offset: 0x0006115C
		// (set) Token: 0x060019FA RID: 6650 RVA: 0x00062F64 File Offset: 0x00061164
		[DataSourceProperty]
		public bool IsLoadingOver
		{
			get
			{
				return this._isLoadingOver;
			}
			set
			{
				if (this._isLoadingOver != value)
				{
					this._isLoadingOver = value;
					base.OnPropertyChangedWithValue(value, "IsLoadingOver");
				}
			}
		}

		// Token: 0x170008A8 RID: 2216
		// (get) Token: 0x060019FB RID: 6651 RVA: 0x00062F82 File Offset: 0x00061182
		// (set) Token: 0x060019FC RID: 6652 RVA: 0x00062F8A File Offset: 0x0006118A
		[DataSourceProperty]
		public bool IsPersuading
		{
			get
			{
				return this._isPersuading;
			}
			set
			{
				if (this._isPersuading != value)
				{
					this._isPersuading = value;
					base.OnPropertyChangedWithValue(value, "IsPersuading");
				}
			}
		}

		// Token: 0x170008A9 RID: 2217
		// (get) Token: 0x060019FD RID: 6653 RVA: 0x00062FA8 File Offset: 0x000611A8
		// (set) Token: 0x060019FE RID: 6654 RVA: 0x00062FB0 File Offset: 0x000611B0
		[DataSourceProperty]
		public string ContinueText
		{
			get
			{
				return this._continueText;
			}
			set
			{
				if (this._continueText != value)
				{
					this._continueText = value;
					base.OnPropertyChangedWithValue<string>(value, "ContinueText");
				}
			}
		}

		// Token: 0x170008AA RID: 2218
		// (get) Token: 0x060019FF RID: 6655 RVA: 0x00062FD3 File Offset: 0x000611D3
		// (set) Token: 0x06001A00 RID: 6656 RVA: 0x00062FDB File Offset: 0x000611DB
		[DataSourceProperty]
		public string CurrentCharacterNameLbl
		{
			get
			{
				return this._currentCharacterNameLbl;
			}
			set
			{
				if (this._currentCharacterNameLbl != value)
				{
					this._currentCharacterNameLbl = value;
					base.OnPropertyChangedWithValue<string>(value, "CurrentCharacterNameLbl");
				}
			}
		}

		// Token: 0x170008AB RID: 2219
		// (get) Token: 0x06001A01 RID: 6657 RVA: 0x00062FFE File Offset: 0x000611FE
		// (set) Token: 0x06001A02 RID: 6658 RVA: 0x00063006 File Offset: 0x00061206
		[DataSourceProperty]
		public MBBindingList<ConversationItemVM> AnswerList
		{
			get
			{
				return this._answerList;
			}
			set
			{
				if (this._answerList != value)
				{
					this._answerList = value;
					base.OnPropertyChangedWithValue<MBBindingList<ConversationItemVM>>(value, "AnswerList");
				}
			}
		}

		// Token: 0x170008AC RID: 2220
		// (get) Token: 0x06001A03 RID: 6659 RVA: 0x00063024 File Offset: 0x00061224
		// (set) Token: 0x06001A04 RID: 6660 RVA: 0x0006302C File Offset: 0x0006122C
		[DataSourceProperty]
		public string DialogText
		{
			get
			{
				return this._dialogText;
			}
			set
			{
				if (this._dialogText != value)
				{
					this._dialogText = value;
					base.OnPropertyChangedWithValue<string>(value, "DialogText");
				}
			}
		}

		// Token: 0x170008AD RID: 2221
		// (get) Token: 0x06001A05 RID: 6661 RVA: 0x0006304F File Offset: 0x0006124F
		// (set) Token: 0x06001A06 RID: 6662 RVA: 0x00063057 File Offset: 0x00061257
		[DataSourceProperty]
		public bool IsAggressive
		{
			get
			{
				return this._isAggressive;
			}
			set
			{
				if (value != this._isAggressive)
				{
					this._isAggressive = value;
					base.OnPropertyChangedWithValue(value, "IsAggressive");
					this.OnAgressiveStateUpdated();
				}
			}
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06001A07 RID: 6663 RVA: 0x0006307B File Offset: 0x0006127B
		// (set) Token: 0x06001A08 RID: 6664 RVA: 0x00063083 File Offset: 0x00061283
		[DataSourceProperty]
		public int SelectedSide
		{
			get
			{
				return this._selectedSide;
			}
			set
			{
				if (value != this._selectedSide)
				{
					this._selectedSide = value;
					base.OnPropertyChangedWithValue(value, "SelectedSide");
				}
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x06001A09 RID: 6665 RVA: 0x000630A1 File Offset: 0x000612A1
		// (set) Token: 0x06001A0A RID: 6666 RVA: 0x000630A9 File Offset: 0x000612A9
		[DataSourceProperty]
		public string RelationText
		{
			get
			{
				return this._relationText;
			}
			set
			{
				if (this._relationText != value)
				{
					this._relationText = value;
					base.OnPropertyChangedWithValue<string>(value, "RelationText");
				}
			}
		}

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x06001A0B RID: 6667 RVA: 0x000630CC File Offset: 0x000612CC
		// (set) Token: 0x06001A0C RID: 6668 RVA: 0x000630D4 File Offset: 0x000612D4
		[DataSourceProperty]
		public bool IsRelationEnabled
		{
			get
			{
				return this._isRelationEnabled;
			}
			set
			{
				if (value != this._isRelationEnabled)
				{
					this._isRelationEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsRelationEnabled");
				}
			}
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x06001A0D RID: 6669 RVA: 0x000630F2 File Offset: 0x000612F2
		// (set) Token: 0x06001A0E RID: 6670 RVA: 0x000630FA File Offset: 0x000612FA
		[DataSourceProperty]
		public bool IsBannerEnabled
		{
			get
			{
				return this._isBannerEnabled;
			}
			set
			{
				if (value != this._isBannerEnabled)
				{
					this._isBannerEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsBannerEnabled");
				}
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x06001A0F RID: 6671 RVA: 0x00063118 File Offset: 0x00061318
		// (set) Token: 0x06001A10 RID: 6672 RVA: 0x00063120 File Offset: 0x00061320
		[DataSourceProperty]
		public ConversationItemVM CurrentSelectedAnswer
		{
			get
			{
				return this._currentSelectedAnswer;
			}
			set
			{
				if (this._currentSelectedAnswer != value)
				{
					this._currentSelectedAnswer = value;
					base.OnPropertyChangedWithValue<ConversationItemVM>(value, "CurrentSelectedAnswer");
				}
			}
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x06001A11 RID: 6673 RVA: 0x0006313E File Offset: 0x0006133E
		// (set) Token: 0x06001A12 RID: 6674 RVA: 0x00063146 File Offset: 0x00061346
		[DataSourceProperty]
		public BannerImageIdentifierVM ConversedHeroBanner
		{
			get
			{
				return this._conversedHeroBanner;
			}
			set
			{
				if (this._conversedHeroBanner != value)
				{
					this._conversedHeroBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "ConversedHeroBanner");
				}
			}
		}

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06001A13 RID: 6675 RVA: 0x00063164 File Offset: 0x00061364
		// (set) Token: 0x06001A14 RID: 6676 RVA: 0x0006316C File Offset: 0x0006136C
		[DataSourceProperty]
		public HintViewModel RelationHint
		{
			get
			{
				return this._relationHint;
			}
			set
			{
				if (this._relationHint != value)
				{
					this._relationHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "RelationHint");
				}
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06001A15 RID: 6677 RVA: 0x0006318A File Offset: 0x0006138A
		// (set) Token: 0x06001A16 RID: 6678 RVA: 0x00063192 File Offset: 0x00061392
		[DataSourceProperty]
		public HintViewModel FactionHint
		{
			get
			{
				return this._factionHint;
			}
			set
			{
				if (this._factionHint != value)
				{
					this._factionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "FactionHint");
				}
			}
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x06001A17 RID: 6679 RVA: 0x000631B0 File Offset: 0x000613B0
		// (set) Token: 0x06001A18 RID: 6680 RVA: 0x000631B8 File Offset: 0x000613B8
		[DataSourceProperty]
		public HintViewModel GoldHint
		{
			get
			{
				return this._goldHint;
			}
			set
			{
				if (this._goldHint != value)
				{
					this._goldHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "GoldHint");
				}
			}
		}

		// Token: 0x04000BCE RID: 3022
		private readonly ConversationManager _conversationManager;

		// Token: 0x04000BCF RID: 3023
		private readonly bool _isLinksDisabled;

		// Token: 0x04000BD0 RID: 3024
		private static bool _isCurrentlyPlayerSpeaking;

		// Token: 0x04000BD1 RID: 3025
		private bool _isProcessingOption;

		// Token: 0x04000BD2 RID: 3026
		private BasicCharacterObject _currentDialogCharacter;

		// Token: 0x04000BD3 RID: 3027
		private Func<string> _getContinueInputText;

		// Token: 0x04000BD4 RID: 3028
		private MBBindingList<ConversationItemVM> _answerList;

		// Token: 0x04000BD5 RID: 3029
		private string _dialogText;

		// Token: 0x04000BD6 RID: 3030
		private string _currentCharacterNameLbl;

		// Token: 0x04000BD7 RID: 3031
		private string _continueText;

		// Token: 0x04000BD8 RID: 3032
		private string _relationText;

		// Token: 0x04000BD9 RID: 3033
		private string _persuasionText;

		// Token: 0x04000BDA RID: 3034
		private bool _isLoadingOver;

		// Token: 0x04000BDB RID: 3035
		private string _moreOptionText;

		// Token: 0x04000BDC RID: 3036
		private string _goldText;

		// Token: 0x04000BDD RID: 3037
		private ConversationAggressivePartyItemVM _defenderLeader;

		// Token: 0x04000BDE RID: 3038
		private ConversationAggressivePartyItemVM _attackerLeader;

		// Token: 0x04000BDF RID: 3039
		private MBBindingList<ConversationAggressivePartyItemVM> _defenderParties;

		// Token: 0x04000BE0 RID: 3040
		private MBBindingList<ConversationAggressivePartyItemVM> _attackerParties;

		// Token: 0x04000BE1 RID: 3041
		private BannerImageIdentifierVM _conversedHeroBanner;

		// Token: 0x04000BE2 RID: 3042
		private bool _isAggressive;

		// Token: 0x04000BE3 RID: 3043
		private bool _isRelationEnabled;

		// Token: 0x04000BE4 RID: 3044
		private bool _isBannerEnabled;

		// Token: 0x04000BE5 RID: 3045
		private bool _isPersuading;

		// Token: 0x04000BE6 RID: 3046
		private bool _isCurrentCharacterValidInEncyclopedia;

		// Token: 0x04000BE7 RID: 3047
		private int _selectedSide;

		// Token: 0x04000BE8 RID: 3048
		private int _relation;

		// Token: 0x04000BE9 RID: 3049
		private int _minRelation;

		// Token: 0x04000BEA RID: 3050
		private int _maxRelation;

		// Token: 0x04000BEB RID: 3051
		private PowerLevelComparer _powerComparer;

		// Token: 0x04000BEC RID: 3052
		private ConversationItemVM _currentSelectedAnswer;

		// Token: 0x04000BED RID: 3053
		private PersuasionVM _persuasion;

		// Token: 0x04000BEE RID: 3054
		private HintViewModel _relationHint;

		// Token: 0x04000BEF RID: 3055
		private HintViewModel _factionHint;

		// Token: 0x04000BF0 RID: 3056
		private HintViewModel _goldHint;
	}
}
