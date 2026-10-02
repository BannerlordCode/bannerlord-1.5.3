using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories
{
	// Token: 0x0200013E RID: 318
	public class ClanFiefsVM : ViewModel
	{
		// Token: 0x06001E3E RID: 7742 RVA: 0x0006D31C File Offset: 0x0006B51C
		public ClanFiefsVM(Action onRefresh, Action<ClanCardSelectionInfo> openCardSelectionPopup)
		{
			this._onRefresh = onRefresh;
			this._clan = Hero.MainHero.Clan;
			this._openCardSelectionPopup = openCardSelectionPopup;
			this._teleportationBehavior = Campaign.Current.GetCampaignBehavior<ITeleportationCampaignBehavior>();
			this.Settlements = new MBBindingList<ClanSettlementItemVM>();
			this.Castles = new MBBindingList<ClanSettlementItemVM>();
			List<MBBindingList<ClanSettlementItemVM>> list = new List<MBBindingList<ClanSettlementItemVM>> { this.Settlements, this.Castles };
			this.SortController = new ClanFiefsSortControllerVM(list);
			this.RefreshAllLists();
			this.RefreshValues();
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x0006D3BA File Offset: 0x0006B5BA
		protected virtual ClanSettlementItemVM CreateSettlementItem(Settlement settlement, Action<ClanSettlementItemVM> onSelection, Action onShowSendMembers, ITeleportationCampaignBehavior teleportationBehavior)
		{
			return new ClanSettlementItemVM(settlement, onSelection, onShowSendMembers, teleportationBehavior);
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x0006D3C8 File Offset: 0x0006B5C8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TaxText = GameTexts.FindText("str_tax", null).ToString();
			this.GovernorText = GameTexts.FindText("str_notable_governor", null).ToString();
			this.ProfitText = GameTexts.FindText("str_profit", null).ToString();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.NoFiefsText = GameTexts.FindText("str_clan_no_fiefs", null).ToString();
			this.NoGovernorText = this._noGovernorTextSource.ToString();
			this.Settlements.ApplyActionOnAllItems(delegate(ClanSettlementItemVM x)
			{
				x.RefreshValues();
			});
			this.Castles.ApplyActionOnAllItems(delegate(ClanSettlementItemVM x)
			{
				x.RefreshValues();
			});
			ClanSettlementItemVM currentSelectedFief = this.CurrentSelectedFief;
			if (currentSelectedFief != null)
			{
				currentSelectedFief.RefreshValues();
			}
			this.SortController.RefreshValues();
		}

		// Token: 0x06001E41 RID: 7745 RVA: 0x0006D4CA File Offset: 0x0006B6CA
		public override void OnFinalize()
		{
			base.OnFinalize();
		}

		// Token: 0x06001E42 RID: 7746 RVA: 0x0006D4D4 File Offset: 0x0006B6D4
		public void RefreshAllLists()
		{
			this.Settlements.Clear();
			this.Castles.Clear();
			this.SortController.ResetAllStates();
			foreach (Settlement settlement in this._clan.Settlements)
			{
				if (settlement.IsTown)
				{
					this.Settlements.Add(this.CreateSettlementItem(settlement, new Action<ClanSettlementItemVM>(this.OnFiefSelection), new Action(this.OnShowSendMembers), this._teleportationBehavior));
				}
				else if (settlement.IsCastle)
				{
					this.Castles.Add(this.CreateSettlementItem(settlement, new Action<ClanSettlementItemVM>(this.OnFiefSelection), new Action(this.OnShowSendMembers), this._teleportationBehavior));
				}
			}
			GameTexts.SetVariable("RANK", GameTexts.FindText("str_towns", null));
			GameTexts.SetVariable("NUMBER", this.Settlements.Count);
			this.TownsText = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null).ToString();
			GameTexts.SetVariable("RANK", GameTexts.FindText("str_castles", null));
			GameTexts.SetVariable("NUMBER", this.Castles.Count);
			this.CastlesText = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null).ToString();
			this.OnFiefSelection(this.GetDefaultMember());
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x0006D648 File Offset: 0x0006B848
		private ClanSettlementItemVM GetDefaultMember()
		{
			if (!this.Settlements.IsEmpty<ClanSettlementItemVM>())
			{
				return this.Settlements.FirstOrDefault<ClanSettlementItemVM>();
			}
			return this.Castles.FirstOrDefault<ClanSettlementItemVM>();
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x0006D670 File Offset: 0x0006B870
		public void SelectFief(Settlement settlement)
		{
			foreach (ClanSettlementItemVM clanSettlementItemVM in this.Settlements)
			{
				if (clanSettlementItemVM.Settlement == settlement)
				{
					this.OnFiefSelection(clanSettlementItemVM);
					break;
				}
			}
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x0006D6C8 File Offset: 0x0006B8C8
		private void OnFiefSelection(ClanSettlementItemVM fief)
		{
			if (this.CurrentSelectedFief != null)
			{
				this.CurrentSelectedFief.IsSelected = false;
			}
			this.CurrentSelectedFief = fief;
			TextObject textObject;
			this.CanChangeGovernorOfCurrentFief = this.GetCanChangeGovernor(out textObject);
			this.GovernorActionHint = new HintViewModel(textObject, null);
			if (fief != null)
			{
				fief.IsSelected = true;
				this.GovernorActionText = (fief.HasGovernor ? GameTexts.FindText("str_clan_change_governor", null).ToString() : GameTexts.FindText("str_clan_assign_governor", null).ToString());
			}
		}

		// Token: 0x06001E46 RID: 7750 RVA: 0x0006D748 File Offset: 0x0006B948
		private bool GetCanChangeGovernor(out TextObject disabledReason)
		{
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			ClanSettlementItemVM currentSelectedFief = this.CurrentSelectedFief;
			bool flag;
			if (currentSelectedFief == null)
			{
				flag = false;
			}
			else
			{
				HeroVM governor = currentSelectedFief.Governor;
				bool? flag2;
				if (governor == null)
				{
					flag2 = null;
				}
				else
				{
					Hero hero = governor.Hero;
					flag2 = ((hero != null) ? new bool?(hero.IsTraveling) : null);
				}
				bool? flag3 = flag2;
				bool flag4 = true;
				flag = (flag3.GetValueOrDefault() == flag4) & (flag3 != null);
			}
			if (flag)
			{
				disabledReason = new TextObject("{=qbqimqMb}{GOVERNOR.NAME} is on the way to be the new governor of {SETTLEMENT_NAME}", null);
				if (this.CurrentSelectedFief.Governor.Hero.CharacterObject != null)
				{
					StringHelpers.SetCharacterProperties("GOVERNOR", this.CurrentSelectedFief.Governor.Hero.CharacterObject, disabledReason, false);
				}
				TextObject textObject2 = disabledReason;
				string text = "SETTLEMENT_NAME";
				Settlement settlement = this.CurrentSelectedFief.Settlement;
				string text2;
				if (settlement == null)
				{
					text2 = null;
				}
				else
				{
					TextObject name = settlement.Name;
					text2 = ((name != null) ? name.ToString() : null);
				}
				textObject2.SetTextVariable(text, text2 ?? string.Empty);
				return false;
			}
			ClanSettlementItemVM currentSelectedFief2 = this.CurrentSelectedFief;
			if (((currentSelectedFief2 != null) ? currentSelectedFief2.Settlement.Town : null) == null)
			{
				disabledReason = TextObject.GetEmpty();
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06001E47 RID: 7751 RVA: 0x0006D868 File Offset: 0x0006BA68
		public void ExecuteAssignGovernor()
		{
			ClanSettlementItemVM currentSelectedFief = this.CurrentSelectedFief;
			bool flag;
			if (currentSelectedFief == null)
			{
				flag = null != null;
			}
			else
			{
				Settlement settlement = currentSelectedFief.Settlement;
				flag = ((settlement != null) ? settlement.Town : null) != null;
			}
			if (flag)
			{
				ClanCardSelectionInfo clanCardSelectionInfo = new ClanCardSelectionInfo(GameTexts.FindText("str_clan_assign_governor", null).CopyTextObject(), this.GetGovernorCandidates(), new Action<List<object>, Action>(this.OnGovernorSelectionOver), false, 1, 0);
				Action<ClanCardSelectionInfo> openCardSelectionPopup = this._openCardSelectionPopup;
				if (openCardSelectionPopup == null)
				{
					return;
				}
				openCardSelectionPopup(clanCardSelectionInfo);
			}
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x0006D8D2 File Offset: 0x0006BAD2
		private IEnumerable<ClanCardSelectionItemInfo> GetGovernorCandidates()
		{
			yield return new ClanCardSelectionItemInfo(this._noGovernorTextSource.CopyTextObject(), false, null, null, false);
			foreach (Hero hero in this._clan.Heroes.Where<Hero>((Hero h) => !h.IsDisabled).Union<Hero>(this._clan.Companions))
			{
				if ((hero.IsActive || hero.IsTraveling) && !hero.IsChild && hero != Hero.MainHero)
				{
					Hero hero2 = hero;
					HeroVM governor = this.CurrentSelectedFief.Governor;
					if (hero2 != ((governor != null) ? governor.Hero : null) && hero.CanBeGovernorOrHavePartyRole())
					{
						TextObject textObject;
						bool flag = FactionHelper.IsMainClanMemberAvailableForSendingSettlementAsGovernor(hero, this.GetSettlementOfGovernor(hero), out textObject);
						SkillObject charm = DefaultSkills.Charm;
						int skillValue = hero.GetSkillValue(charm);
						CharacterImageIdentifier characterImageIdentifier = new CharacterImageIdentifier(CampaignUIHelper.GetCharacterCode(hero.CharacterObject, false));
						yield return new ClanCardSelectionItemInfo(hero, hero.Name, characterImageIdentifier, CardSelectionItemSpriteType.Skill, charm.StringId.ToLower(), skillValue.ToString(), this.GetGovernorCandidateProperties(hero), !flag, textObject, null, false);
					}
				}
			}
			IEnumerator<Hero> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x0006D8E2 File Offset: 0x0006BAE2
		private IEnumerable<ClanCardSelectionItemPropertyInfo> GetGovernorCandidateProperties(Hero hero)
		{
			GameTexts.SetVariable("newline", "\n");
			TextObject teleportationDelayText = CampaignUIHelper.GetTeleportationDelayText(hero, this.CurrentSelectedFief.Settlement.Party);
			yield return new ClanCardSelectionItemPropertyInfo(teleportationDelayText);
			ValueTuple<TextObject, TextObject> governorEngineeringSkillEffectForHero = PerkHelper.GetGovernorEngineeringSkillEffectForHero(hero);
			yield return new ClanCardSelectionItemPropertyInfo(new TextObject("{=J8ddrAOf}Governor Effects", null), governorEngineeringSkillEffectForHero.Item2);
			List<PerkObject> governorPerksForHero = PerkHelper.GetGovernorPerksForHero(hero);
			TextObject textObject = new TextObject("{=oSfsqBwJ}No perks", null);
			int num = 0;
			foreach (PerkObject perkObject in governorPerksForHero)
			{
				bool flag = perkObject.PrimaryRole == PartyRole.Governor;
				bool flag2 = perkObject.SecondaryRole == PartyRole.Governor;
				if (flag)
				{
					TextObject textObject2 = ClanCardSelectionItemPropertyInfo.CreateLabeledValueText(perkObject.Name, perkObject.PrimaryDescription);
					this.SetPerksPropertyText(textObject2, ref textObject, ref num);
				}
				if (flag2)
				{
					TextObject textObject3 = ClanCardSelectionItemPropertyInfo.CreateLabeledValueText(perkObject.Name, perkObject.SecondaryDescription);
					this.SetPerksPropertyText(textObject3, ref textObject, ref num);
				}
			}
			yield return new ClanCardSelectionItemPropertyInfo(GameTexts.FindText("str_clan_governor_perks", null), textObject);
			yield break;
		}

		// Token: 0x06001E4A RID: 7754 RVA: 0x0006D8FC File Offset: 0x0006BAFC
		private void SetPerksPropertyText(TextObject perkText, ref TextObject perksPropertyText, ref int addedPerkCount)
		{
			if (addedPerkCount == 0)
			{
				perksPropertyText = perkText;
			}
			else
			{
				TextObject textObject = GameTexts.FindText("str_string_newline_newline_string", null);
				textObject.SetTextVariable("STR1", perksPropertyText);
				textObject.SetTextVariable("STR2", perkText);
				perksPropertyText = textObject;
			}
			addedPerkCount++;
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x0006D944 File Offset: 0x0006BB44
		private void OnGovernorSelectionOver(List<object> selectedItems, Action closePopup)
		{
			if (selectedItems.Count == 1)
			{
				ClanSettlementItemVM currentSelectedFief = this.CurrentSelectedFief;
				Hero hero;
				if (currentSelectedFief == null)
				{
					hero = null;
				}
				else
				{
					HeroVM governor = currentSelectedFief.Governor;
					hero = ((governor != null) ? governor.Hero : null);
				}
				Hero hero2 = hero;
				Hero newGovernor = selectedItems.FirstOrDefault<object>() as Hero;
				bool isRemoveGovernor = newGovernor == null;
				if (!isRemoveGovernor || hero2 != null)
				{
					ValueTuple<TextObject, TextObject> governorSelectionConfirmationPopupTexts = CampaignUIHelper.GetGovernorSelectionConfirmationPopupTexts(hero2, newGovernor, this.CurrentSelectedFief.Settlement);
					InformationManager.ShowInquiry(new InquiryData(governorSelectionConfirmationPopupTexts.Item1.ToString(), governorSelectionConfirmationPopupTexts.Item2.ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
					{
						Action closePopup4 = closePopup;
						if (closePopup4 != null)
						{
							closePopup4();
						}
						if (isRemoveGovernor)
						{
							ChangeGovernorAction.RemoveGovernorOfIfExists(this.CurrentSelectedFief.Settlement.Town);
						}
						else
						{
							ChangeGovernorAction.Apply(this.CurrentSelectedFief.Settlement.Town, newGovernor);
						}
						Action onRefresh = this._onRefresh;
						if (onRefresh == null)
						{
							return;
						}
						onRefresh();
					}, null, "", 0f, null, null, null), false, false);
					return;
				}
				Action closePopup2 = closePopup;
				if (closePopup2 == null)
				{
					return;
				}
				closePopup2();
				return;
			}
			else
			{
				Action closePopup3 = closePopup;
				if (closePopup3 == null)
				{
					return;
				}
				closePopup3();
				return;
			}
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x0006DA68 File Offset: 0x0006BC68
		private Settlement GetSettlementOfGovernor(Hero hero)
		{
			foreach (ClanSettlementItemVM clanSettlementItemVM in this.Settlements)
			{
				Hero hero2;
				if (clanSettlementItemVM == null)
				{
					hero2 = null;
				}
				else
				{
					HeroVM governor = clanSettlementItemVM.Governor;
					hero2 = ((governor != null) ? governor.Hero : null);
				}
				if (hero2 == hero)
				{
					return clanSettlementItemVM.Settlement;
				}
			}
			foreach (ClanSettlementItemVM clanSettlementItemVM2 in this.Castles)
			{
				Hero hero3;
				if (clanSettlementItemVM2 == null)
				{
					hero3 = null;
				}
				else
				{
					HeroVM governor2 = clanSettlementItemVM2.Governor;
					hero3 = ((governor2 != null) ? governor2.Hero : null);
				}
				if (hero3 == hero)
				{
					return clanSettlementItemVM2.Settlement;
				}
			}
			return null;
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x0006DB30 File Offset: 0x0006BD30
		private void OnShowSendMembers()
		{
			ClanSettlementItemVM currentSelectedFief = this.CurrentSelectedFief;
			Settlement settlement = ((currentSelectedFief != null) ? currentSelectedFief.Settlement : null);
			if (settlement != null)
			{
				TextObject textObject = GameTexts.FindText("str_send_members", null);
				textObject.SetTextVariable("SETTLEMENT_NAME", settlement.Name);
				ClanCardSelectionInfo clanCardSelectionInfo = new ClanCardSelectionInfo(textObject, this.GetSendMembersCandidates(), new Action<List<object>, Action>(this.OnSendMembersSelectionOver), true, 1, 0);
				Action<ClanCardSelectionInfo> openCardSelectionPopup = this._openCardSelectionPopup;
				if (openCardSelectionPopup == null)
				{
					return;
				}
				openCardSelectionPopup(clanCardSelectionInfo);
			}
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x0006DB9F File Offset: 0x0006BD9F
		private IEnumerable<ClanCardSelectionItemInfo> GetSendMembersCandidates()
		{
			foreach (Hero hero in this._clan.Heroes.Where<Hero>((Hero h) => !h.IsDisabled).Union<Hero>(this._clan.Companions))
			{
				if ((hero.IsActive || hero.IsTraveling) && (hero.CurrentSettlement != this.CurrentSelectedFief.Settlement || hero.PartyBelongedTo != null) && !hero.IsChild && hero != Hero.MainHero)
				{
					TextObject textObject;
					bool flag = FactionHelper.IsMainClanMemberAvailableForSendingSettlement(hero, this.CurrentSelectedFief.Settlement, out textObject);
					SkillObject charm = DefaultSkills.Charm;
					int skillValue = hero.GetSkillValue(charm);
					CharacterImageIdentifier characterImageIdentifier = new CharacterImageIdentifier(CampaignUIHelper.GetCharacterCode(hero.CharacterObject, false));
					yield return new ClanCardSelectionItemInfo(hero, hero.Name, characterImageIdentifier, CardSelectionItemSpriteType.Skill, charm.StringId.ToLower(), skillValue.ToString(), this.GetSendMembersCandidateProperties(hero), !flag, textObject, null, false);
				}
			}
			IEnumerator<Hero> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x0006DBAF File Offset: 0x0006BDAF
		private IEnumerable<ClanCardSelectionItemPropertyInfo> GetSendMembersCandidateProperties(Hero hero)
		{
			TextObject teleportationDelayText = CampaignUIHelper.GetTeleportationDelayText(hero, this.CurrentSelectedFief.Settlement.Party);
			yield return new ClanCardSelectionItemPropertyInfo(teleportationDelayText);
			TextObject textObject = new TextObject("{=otaUtXMX}+{AMOUNT} relation chance with notables per day.", null);
			int emissaryRelationBonusForMainClan = Campaign.Current.Models.EmissaryModel.EmissaryRelationBonusForMainClan;
			textObject.SetTextVariable("AMOUNT", emissaryRelationBonusForMainClan);
			yield return new ClanCardSelectionItemPropertyInfo(textObject);
			yield break;
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x0006DBC8 File Offset: 0x0006BDC8
		private void OnSendMembersSelectionOver(List<object> selectedItems, Action closePopup)
		{
			if (selectedItems.Count > 0)
			{
				string text = "SETTLEMENT_NAME";
				ClanSettlementItemVM currentSelectedFief = this.CurrentSelectedFief;
				string text2;
				if (currentSelectedFief == null)
				{
					text2 = null;
				}
				else
				{
					Settlement settlement = currentSelectedFief.Settlement;
					if (settlement == null)
					{
						text2 = null;
					}
					else
					{
						TextObject name = settlement.Name;
						text2 = ((name != null) ? name.ToString() : null);
					}
				}
				MBTextManager.SetTextVariable(text, text2 ?? string.Empty, false);
				InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_send_members", null).ToString(), GameTexts.FindText("str_send_members_inquiry", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
				{
					Action closePopup3 = closePopup;
					if (closePopup3 != null)
					{
						closePopup3();
					}
					using (List<object>.Enumerator enumerator = selectedItems.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Hero hero;
							if ((hero = enumerator.Current as Hero) != null)
							{
								TeleportHeroAction.ApplyDelayedTeleportToSettlement(hero, this.CurrentSelectedFief.Settlement);
							}
						}
					}
					Action onRefresh = this._onRefresh;
					if (onRefresh == null)
					{
						return;
					}
					onRefresh();
				}, null, "", 0f, null, null, null), false, false);
				return;
			}
			Action closePopup2 = closePopup;
			if (closePopup2 == null)
			{
				return;
			}
			closePopup2();
		}

		// Token: 0x17000A64 RID: 2660
		// (get) Token: 0x06001E51 RID: 7761 RVA: 0x0006DCB8 File Offset: 0x0006BEB8
		// (set) Token: 0x06001E52 RID: 7762 RVA: 0x0006DCC0 File Offset: 0x0006BEC0
		[DataSourceProperty]
		public string GovernorActionText
		{
			get
			{
				return this._governorActionText;
			}
			set
			{
				if (value != this._governorActionText)
				{
					this._governorActionText = value;
					base.OnPropertyChangedWithValue<string>(value, "GovernorActionText");
				}
			}
		}

		// Token: 0x17000A65 RID: 2661
		// (get) Token: 0x06001E53 RID: 7763 RVA: 0x0006DCE3 File Offset: 0x0006BEE3
		// (set) Token: 0x06001E54 RID: 7764 RVA: 0x0006DCEB File Offset: 0x0006BEEB
		[DataSourceProperty]
		public bool CanChangeGovernorOfCurrentFief
		{
			get
			{
				return this._canChangeGovernorOfCurrentFief;
			}
			set
			{
				if (value != this._canChangeGovernorOfCurrentFief)
				{
					this._canChangeGovernorOfCurrentFief = value;
					base.OnPropertyChangedWithValue(value, "CanChangeGovernorOfCurrentFief");
				}
			}
		}

		// Token: 0x17000A66 RID: 2662
		// (get) Token: 0x06001E55 RID: 7765 RVA: 0x0006DD09 File Offset: 0x0006BF09
		// (set) Token: 0x06001E56 RID: 7766 RVA: 0x0006DD11 File Offset: 0x0006BF11
		[DataSourceProperty]
		public HintViewModel GovernorActionHint
		{
			get
			{
				return this._governorActionHint;
			}
			set
			{
				if (value != this._governorActionHint)
				{
					this._governorActionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "GovernorActionHint");
				}
			}
		}

		// Token: 0x17000A67 RID: 2663
		// (get) Token: 0x06001E57 RID: 7767 RVA: 0x0006DD2F File Offset: 0x0006BF2F
		// (set) Token: 0x06001E58 RID: 7768 RVA: 0x0006DD37 File Offset: 0x0006BF37
		[DataSourceProperty]
		public bool IsAnyValidFiefSelected
		{
			get
			{
				return this._isAnyValidFiefSelected;
			}
			set
			{
				if (value != this._isAnyValidFiefSelected)
				{
					this._isAnyValidFiefSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyValidFiefSelected");
				}
			}
		}

		// Token: 0x17000A68 RID: 2664
		// (get) Token: 0x06001E59 RID: 7769 RVA: 0x0006DD55 File Offset: 0x0006BF55
		// (set) Token: 0x06001E5A RID: 7770 RVA: 0x0006DD5D File Offset: 0x0006BF5D
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x17000A69 RID: 2665
		// (get) Token: 0x06001E5B RID: 7771 RVA: 0x0006DD80 File Offset: 0x0006BF80
		// (set) Token: 0x06001E5C RID: 7772 RVA: 0x0006DD88 File Offset: 0x0006BF88
		[DataSourceProperty]
		public string TaxText
		{
			get
			{
				return this._taxText;
			}
			set
			{
				if (value != this._taxText)
				{
					this._taxText = value;
					base.OnPropertyChangedWithValue<string>(value, "TaxText");
				}
			}
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06001E5D RID: 7773 RVA: 0x0006DDAB File Offset: 0x0006BFAB
		// (set) Token: 0x06001E5E RID: 7774 RVA: 0x0006DDB3 File Offset: 0x0006BFB3
		[DataSourceProperty]
		public string GovernorText
		{
			get
			{
				return this._governorText;
			}
			set
			{
				if (value != this._governorText)
				{
					this._governorText = value;
					base.OnPropertyChangedWithValue<string>(value, "GovernorText");
				}
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06001E5F RID: 7775 RVA: 0x0006DDD6 File Offset: 0x0006BFD6
		// (set) Token: 0x06001E60 RID: 7776 RVA: 0x0006DDDE File Offset: 0x0006BFDE
		[DataSourceProperty]
		public string ProfitText
		{
			get
			{
				return this._profitText;
			}
			set
			{
				if (value != this._profitText)
				{
					this._profitText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProfitText");
				}
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06001E61 RID: 7777 RVA: 0x0006DE01 File Offset: 0x0006C001
		// (set) Token: 0x06001E62 RID: 7778 RVA: 0x0006DE09 File Offset: 0x0006C009
		[DataSourceProperty]
		public string TownsText
		{
			get
			{
				return this._townsText;
			}
			set
			{
				if (value != this._townsText)
				{
					this._townsText = value;
					base.OnPropertyChangedWithValue<string>(value, "TownsText");
				}
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06001E63 RID: 7779 RVA: 0x0006DE2C File Offset: 0x0006C02C
		// (set) Token: 0x06001E64 RID: 7780 RVA: 0x0006DE34 File Offset: 0x0006C034
		[DataSourceProperty]
		public string CastlesText
		{
			get
			{
				return this._castlesText;
			}
			set
			{
				if (value != this._castlesText)
				{
					this._castlesText = value;
					base.OnPropertyChangedWithValue<string>(value, "CastlesText");
				}
			}
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x06001E65 RID: 7781 RVA: 0x0006DE57 File Offset: 0x0006C057
		// (set) Token: 0x06001E66 RID: 7782 RVA: 0x0006DE5F File Offset: 0x0006C05F
		[DataSourceProperty]
		public string NoFiefsText
		{
			get
			{
				return this._noFiefsText;
			}
			set
			{
				if (value != this._noFiefsText)
				{
					this._noFiefsText = value;
					base.OnPropertyChangedWithValue<string>(value, "NoFiefsText");
				}
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x0006DE82 File Offset: 0x0006C082
		// (set) Token: 0x06001E68 RID: 7784 RVA: 0x0006DE8A File Offset: 0x0006C08A
		[DataSourceProperty]
		public string NoGovernorText
		{
			get
			{
				return this._noGovernorText;
			}
			set
			{
				if (value != this._noGovernorText)
				{
					this._noGovernorText = value;
					base.OnPropertyChangedWithValue<string>(value, "NoGovernorText");
				}
			}
		}

		// Token: 0x17000A70 RID: 2672
		// (get) Token: 0x06001E69 RID: 7785 RVA: 0x0006DEAD File Offset: 0x0006C0AD
		// (set) Token: 0x06001E6A RID: 7786 RVA: 0x0006DEB5 File Offset: 0x0006C0B5
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000A71 RID: 2673
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x0006DED3 File Offset: 0x0006C0D3
		// (set) Token: 0x06001E6C RID: 7788 RVA: 0x0006DEDB File Offset: 0x0006C0DB
		[DataSourceProperty]
		public MBBindingList<ClanSettlementItemVM> Settlements
		{
			get
			{
				return this._settlements;
			}
			set
			{
				if (value != this._settlements)
				{
					this._settlements = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanSettlementItemVM>>(value, "Settlements");
				}
			}
		}

		// Token: 0x17000A72 RID: 2674
		// (get) Token: 0x06001E6D RID: 7789 RVA: 0x0006DEF9 File Offset: 0x0006C0F9
		// (set) Token: 0x06001E6E RID: 7790 RVA: 0x0006DF01 File Offset: 0x0006C101
		[DataSourceProperty]
		public MBBindingList<ClanSettlementItemVM> Castles
		{
			get
			{
				return this._castles;
			}
			set
			{
				if (value != this._castles)
				{
					this._castles = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanSettlementItemVM>>(value, "Castles");
				}
			}
		}

		// Token: 0x17000A73 RID: 2675
		// (get) Token: 0x06001E6F RID: 7791 RVA: 0x0006DF1F File Offset: 0x0006C11F
		// (set) Token: 0x06001E70 RID: 7792 RVA: 0x0006DF27 File Offset: 0x0006C127
		[DataSourceProperty]
		public ClanSettlementItemVM CurrentSelectedFief
		{
			get
			{
				return this._currentSelectedFief;
			}
			set
			{
				if (value != this._currentSelectedFief)
				{
					this._currentSelectedFief = value;
					base.OnPropertyChangedWithValue<ClanSettlementItemVM>(value, "CurrentSelectedFief");
					this.IsAnyValidFiefSelected = value != null;
				}
			}
		}

		// Token: 0x17000A74 RID: 2676
		// (get) Token: 0x06001E71 RID: 7793 RVA: 0x0006DF4F File Offset: 0x0006C14F
		// (set) Token: 0x06001E72 RID: 7794 RVA: 0x0006DF57 File Offset: 0x0006C157
		[DataSourceProperty]
		public ClanFiefsSortControllerVM SortController
		{
			get
			{
				return this._sortController;
			}
			set
			{
				if (value != this._sortController)
				{
					this._sortController = value;
					base.OnPropertyChangedWithValue<ClanFiefsSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x04000DD7 RID: 3543
		private readonly Clan _clan;

		// Token: 0x04000DD8 RID: 3544
		private readonly Action _onRefresh;

		// Token: 0x04000DD9 RID: 3545
		private readonly Action<ClanCardSelectionInfo> _openCardSelectionPopup;

		// Token: 0x04000DDA RID: 3546
		private readonly ITeleportationCampaignBehavior _teleportationBehavior;

		// Token: 0x04000DDB RID: 3547
		private readonly TextObject _noGovernorTextSource = new TextObject("{=zLFsnaqR}No Governor", null);

		// Token: 0x04000DDC RID: 3548
		private MBBindingList<ClanSettlementItemVM> _settlements;

		// Token: 0x04000DDD RID: 3549
		private MBBindingList<ClanSettlementItemVM> _castles;

		// Token: 0x04000DDE RID: 3550
		private ClanSettlementItemVM _currentSelectedFief;

		// Token: 0x04000DDF RID: 3551
		private bool _isSelected;

		// Token: 0x04000DE0 RID: 3552
		private string _nameText;

		// Token: 0x04000DE1 RID: 3553
		private string _taxText;

		// Token: 0x04000DE2 RID: 3554
		private string _governorText;

		// Token: 0x04000DE3 RID: 3555
		private string _profitText;

		// Token: 0x04000DE4 RID: 3556
		private string _townsText;

		// Token: 0x04000DE5 RID: 3557
		private string _castlesText;

		// Token: 0x04000DE6 RID: 3558
		private string _noFiefsText;

		// Token: 0x04000DE7 RID: 3559
		private string _noGovernorText;

		// Token: 0x04000DE8 RID: 3560
		private bool _isAnyValidFiefSelected;

		// Token: 0x04000DE9 RID: 3561
		private bool _canChangeGovernorOfCurrentFief;

		// Token: 0x04000DEA RID: 3562
		private HintViewModel _governorActionHint;

		// Token: 0x04000DEB RID: 3563
		private string _governorActionText;

		// Token: 0x04000DEC RID: 3564
		private ClanFiefsSortControllerVM _sortController;
	}
}
