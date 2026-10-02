using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Armies;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Clans;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Policies;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement
{
	// Token: 0x0200006B RID: 107
	public class KingdomManagementVM : ViewModel
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x0002434F File Offset: 0x0002254F
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x00024357 File Offset: 0x00022557
		public Kingdom Kingdom { get; private set; }

		// Token: 0x060007B4 RID: 1972 RVA: 0x00024360 File Offset: 0x00022560
		public KingdomManagementVM(Action onClose, Action onManageArmy, Action<Army> onShowArmyOnMap)
		{
			this._onClose = onClose;
			this._onShowArmyOnMap = onShowArmyOnMap;
			this.Army = new KingdomArmyVM(onManageArmy, new Action(this.OnRefreshDecision), this._onShowArmyOnMap);
			this.Settlement = this.CreateSettlementVM(new Action<KingdomDecision>(this.ForceDecideDecision), new Action<Settlement>(this.OnGrantFief));
			this.Clan = new KingdomClanVM(new Action<KingdomDecision>(this.ForceDecideDecision));
			this.Policy = new KingdomPoliciesVM(new Action<KingdomDecision>(this.ForceDecideDecision));
			this.Diplomacy = new KingdomDiplomacyVM(new Action<KingdomDecision>(this.ForceDecideDecision));
			this.GiftFief = new KingdomGiftFiefPopupVM(new Action(this.OnSettlementGranted));
			this.Decision = new KingdomDecisionsVM(new Action(this.OnRefresh));
			this._categoryCount = 5;
			this._leaveKingdomPermissionEvent = new LeaveKingdomPermissionEvent(new Action<bool, TextObject>(this.OnLeaveKingdomRequest));
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			int num = this._viewDataTracker.GetLastOpenedKingdomTabIndex();
			if (this._categoryCount <= num)
			{
				Debug.FailedAssert("Tab index is out of bounds", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\KingdomManagement\\KingdomManagementVM.cs", ".ctor", 58);
				num = 0;
			}
			this.SetSelectedCategory(num);
			this.ChangeKingdomNameHint = new HintViewModel();
			this.RefreshValues();
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x000244A9 File Offset: 0x000226A9
		protected virtual KingdomSettlementVM CreateSettlementVM(Action<KingdomDecision> forceDecision, Action<Settlement> onGrantFief)
		{
			return new KingdomSettlementVM(forceDecision, onGrantFief);
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x000244B4 File Offset: 0x000226B4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.LeaderText = GameTexts.FindText("str_sort_by_leader_name_label", null).ToString();
			this.ClansText = GameTexts.FindText("str_encyclopedia_clans", null).ToString();
			this.FiefsText = GameTexts.FindText("str_fiefs", null).ToString();
			this.PoliciesText = GameTexts.FindText("str_policies", null).ToString();
			this.ArmiesText = GameTexts.FindText("str_armies", null).ToString();
			this.DiplomacyText = GameTexts.FindText("str_diplomatic_group", null).ToString();
			this.DoneText = GameTexts.FindText("str_done", null).ToString();
			this.RefreshDynamicKingdomProperties();
			this.Army.RefreshValues();
			this.Policy.RefreshValues();
			this.Clan.RefreshValues();
			this.Settlement.RefreshValues();
			this.Diplomacy.RefreshValues();
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x000245A0 File Offset: 0x000227A0
		private void RefreshDynamicKingdomProperties()
		{
			this.Name = ((Hero.MainHero.MapFaction == null) ? new TextObject("{=kQsXUvgO}You are not under a kingdom.", null).ToString() : Hero.MainHero.MapFaction.Name.ToString());
			this.PlayerHasKingdom = Hero.MainHero.MapFaction is Kingdom;
			if (this.PlayerHasKingdom)
			{
				this.Kingdom = Hero.MainHero.MapFaction as Kingdom;
				this.Leader = new HeroVM(this.Kingdom.Leader, false);
				this.KingdomBanner = new BannerImageIdentifierVM(this.Kingdom.Banner, true);
				this._isPlayerTheRuler = this.Kingdom.Leader == Hero.MainHero;
				this.KingdomActionText = (this._isPlayerTheRuler ? GameTexts.FindText("str_abdicate_leadership", null).ToString() : GameTexts.FindText("str_leave_kingdom", null).ToString());
			}
			else
			{
				this.Kingdom = null;
				this.Leader = null;
				this.KingdomBanner = null;
				this._isPlayerTheRuler = false;
				this.KingdomActionText = string.Empty;
			}
			TextObject textObject;
			this.PlayerCanChangeKingdomName = this.GetCanChangeKingdomNameWithReason(out textObject);
			this.ChangeKingdomNameHint.HintText = textObject;
			List<TextObject> kingdomActionDisabledReasons;
			this.IsKingdomActionEnabled = this.GetIsKingdomActionEnabledWithReason(this._isPlayerTheRuler, out kingdomActionDisabledReasons);
			this.KingdomActionHint = new BasicTooltipViewModel(() => CampaignUIHelper.MergeTextObjectsWithNewline(kingdomActionDisabledReasons));
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0002470C File Offset: 0x0002290C
		private bool GetCanChangeKingdomNameWithReason(out TextObject disabledReason)
		{
			if (!this.PlayerHasKingdom)
			{
				disabledReason = new TextObject("{=kQsXUvgO}You are not under a kingdom.", null);
				return false;
			}
			if (!this._isPlayerTheRuler)
			{
				disabledReason = new TextObject("{=HFZdseH9}Only the ruler of the kingdom can change its name.", null);
				return false;
			}
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x00024760 File Offset: 0x00022960
		private bool GetIsKingdomActionEnabledWithReason(bool isPlayerTheRuler, out List<TextObject> disabledReasons)
		{
			disabledReasons = new List<TextObject>();
			if (!this.PlayerHasKingdom)
			{
				disabledReasons.Add(new TextObject("{=kQsXUvgO}You are not under a kingdom.", null));
				return false;
			}
			List<TextObject> list;
			if (isPlayerTheRuler && !Campaign.Current.Models.KingdomCreationModel.IsPlayerKingdomAbdicationPossible(out list))
			{
				disabledReasons.AddRange(list);
				return false;
			}
			if (!isPlayerTheRuler && MobileParty.MainParty.Army != null)
			{
				disabledReasons.Add(new TextObject("{=4Y8u4JKO}You can't leave the kingdom while in an army", null));
				return false;
			}
			Game.Current.EventManager.TriggerEvent<LeaveKingdomPermissionEvent>(this._leaveKingdomPermissionEvent);
			if (this._mostRecentLeaveKingdomPermission != null && !this._mostRecentLeaveKingdomPermission.GetValueOrDefault().Item1)
			{
				disabledReasons.Add((this._mostRecentLeaveKingdomPermission != null) ? this._mostRecentLeaveKingdomPermission.GetValueOrDefault().Item2 : null);
				return false;
			}
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReasons.Add(textObject);
				return false;
			}
			return true;
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00024847 File Offset: 0x00022A47
		public void OnRefresh()
		{
			this.RefreshDynamicKingdomProperties();
			this.Army.RefreshArmyList();
			this.Policy.RefreshPolicyList();
			this.Clan.RefreshClan();
			this.Settlement.RefreshSettlementList();
			this.Diplomacy.RefreshDiplomacyList();
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x00024886 File Offset: 0x00022A86
		public void OnFrameTick()
		{
			KingdomDecisionsVM decision = this.Decision;
			if (decision == null)
			{
				return;
			}
			decision.OnFrameTick();
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x00024898 File Offset: 0x00022A98
		private void OnRefreshDecision()
		{
			this.Decision.HandleNextDecision();
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x000248A5 File Offset: 0x00022AA5
		private void ForceDecideDecision(KingdomDecision decision)
		{
			this.Decision.RefreshWith(decision);
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x000248B4 File Offset: 0x00022AB4
		private void OnGrantFief(Settlement settlement)
		{
			if (this.Kingdom.Leader == Hero.MainHero)
			{
				this.GiftFief.OpenWith(settlement);
				return;
			}
			string text = new TextObject("{=eIGFuGOx}Give Settlement", null).ToString();
			string text2 = new TextObject("{=rkubGa4K}Are you sure want to give this settlement back to your kingdom?", null).ToString();
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
			{
				Campaign.Current.KingdomManager.RelinquishSettlementOwnership(settlement);
				this.ForceDecideDecision(this.Kingdom.UnresolvedDecisions[this.Kingdom.UnresolvedDecisions.Count - 1]);
			}, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x00024963 File Offset: 0x00022B63
		private void OnSettlementGranted()
		{
			this.Settlement.RefreshSettlementList();
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x00024970 File Offset: 0x00022B70
		public void ExecuteClose()
		{
			this._onClose();
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x0002497D File Offset: 0x00022B7D
		private void ExecuteShowClan()
		{
			this.SetSelectedCategory(0);
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00024986 File Offset: 0x00022B86
		private void ExecuteShowFiefs()
		{
			this.SetSelectedCategory(1);
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x0002498F File Offset: 0x00022B8F
		private void ExecuteShowPolicies()
		{
			if (this.PlayerHasKingdom)
			{
				this.SetSelectedCategory(2);
			}
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x000249A0 File Offset: 0x00022BA0
		private void ExecuteShowDiplomacy()
		{
			if (this.PlayerHasKingdom)
			{
				this.SetSelectedCategory(4);
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x000249B1 File Offset: 0x00022BB1
		private void ExecuteShowArmy()
		{
			this.SetSelectedCategory(3);
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x000249BC File Offset: 0x00022BBC
		private void ExecuteKingdomAction()
		{
			if (this.IsKingdomActionEnabled)
			{
				if (this._isPlayerTheRuler)
				{
					GameTexts.SetVariable("WILL_DESTROY", (this.Kingdom.Clans.Count == 1) ? 1 : 0);
					InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_abdicate_leadership", null).ToString(), GameTexts.FindText("str_abdicate_leadership_question", null).ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.OnConfirmAbdicateLeadership), null, "", 0f, null, null, null), false, false);
					return;
				}
				if (this._mostRecentLeaveKingdomPermission != null && this._mostRecentLeaveKingdomPermission.GetValueOrDefault().Item1 && ((this._mostRecentLeaveKingdomPermission != null) ? this._mostRecentLeaveKingdomPermission.GetValueOrDefault().Item2 : null) != null)
				{
					InformationManager.ShowInquiry(new InquiryData(new TextObject("{=3sxtCWPe}Leaving Kingdom", null).ToString(), (this._mostRecentLeaveKingdomPermission != null) ? this._mostRecentLeaveKingdomPermission.GetValueOrDefault().Item2.ToString() : null, true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.OnConfirmLeaveKingdom), null, "", 0f, null, null, null), false, false);
					return;
				}
				if (TaleWorlds.CampaignSystem.Clan.PlayerClan.Settlements.Count == 0)
				{
					if (TaleWorlds.CampaignSystem.Clan.PlayerClan.IsUnderMercenaryService)
					{
						TextObject textObject = new TextObject("{=b7muQ9mt}Are you sure you want to end your mercenary contract with the {KINGDOM_INFORMALNAME}?", null);
						textObject.SetTextVariable("KINGDOM_INFORMALNAME", this.Kingdom.InformalName);
						InformationManager.ShowInquiry(new InquiryData(new TextObject("{=3sxtCWPe}Leaving Kingdom", null).ToString(), textObject.ToString(), true, true, GameTexts.FindText("str_confirm", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action(this.OnConfirmLeaveKingdom), null, "", 0f, null, null, null), false, false);
						return;
					}
					InformationManager.ShowInquiry(new InquiryData(new TextObject("{=3sxtCWPe}Leaving Kingdom", null).ToString(), new TextObject("{=BgqZWbga}The nobles of the realm will dislike you for abandoning your fealty. Are you sure you want to leave the Kingdom?", null).ToString(), true, true, GameTexts.FindText("str_confirm", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action(this.OnConfirmLeaveKingdom), null, "", 0f, null, null, null), false, false);
					return;
				}
				else
				{
					List<InquiryElement> list = new List<InquiryElement>
					{
						new InquiryElement("keep", new TextObject("{=z8h0BRAb}Keep all holdings", null).ToString(), null, true, new TextObject("{=lkJfq1ap}Owned settlements remain under your control but nobles will dislike this dishonorable act and the kingdom will declare war on you.", null).ToString()),
						new InquiryElement("dontkeep", new TextObject("{=JIr3Jc7b}Relinquish all holdings", null).ToString(), null, true, new TextObject("{=ZjaSde0X}Owned settlements are returned to the kingdom. This will avert a war and nobles will dislike you less for abandoning your fealty.", null).ToString())
					};
					MBInformationManager.ShowMultiSelectionInquiry(new MultiSelectionInquiryData(new TextObject("{=3sxtCWPe}Leaving Kingdom", null).ToString(), new TextObject("{=xtlIFKaa}Are you sure you want to leave the Kingdom?{newline}If so, choose how you want to leave the kingdom.", null).ToString(), list, true, 1, 1, GameTexts.FindText("str_confirm", null).ToString(), string.Empty, new Action<List<InquiryElement>>(this.OnConfirmLeaveKingdomWithOption), null, "", false), false, false);
				}
			}
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00024CF8 File Offset: 0x00022EF8
		private void OnLeaveKingdomRequest(bool isPossible, TextObject disabledReasonOrWarning)
		{
			this._mostRecentLeaveKingdomPermission = new ValueTuple<bool, TextObject>?(new ValueTuple<bool, TextObject>(isPossible, disabledReasonOrWarning));
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00024D0C File Offset: 0x00022F0C
		private void OnConfirmAbdicateLeadership()
		{
			Campaign.Current.KingdomManager.AbdicateTheThrone(this.Kingdom);
			KingdomDecision kingdomDecision = this.Kingdom.UnresolvedDecisions.LastOrDefault<KingdomDecision>();
			if (kingdomDecision != null)
			{
				this.ForceDecideDecision(kingdomDecision);
				return;
			}
			this.ExecuteClose();
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00024D50 File Offset: 0x00022F50
		private void OnConfirmLeaveKingdomWithOption(List<InquiryElement> obj)
		{
			InquiryElement inquiryElement = obj.FirstOrDefault<InquiryElement>();
			if (inquiryElement != null)
			{
				string text = inquiryElement.Identifier as string;
				if (text == "keep")
				{
					ChangeKingdomAction.ApplyByLeaveWithRebellionAgainstKingdom(TaleWorlds.CampaignSystem.Clan.PlayerClan, true);
				}
				else if (text == "dontkeep")
				{
					ChangeKingdomAction.ApplyByLeaveKingdom(TaleWorlds.CampaignSystem.Clan.PlayerClan, true);
				}
				this.ExecuteClose();
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00024DAB File Offset: 0x00022FAB
		private void OnConfirmLeaveKingdom()
		{
			if (TaleWorlds.CampaignSystem.Clan.PlayerClan.IsUnderMercenaryService)
			{
				ChangeKingdomAction.ApplyByLeaveKingdomAsMercenary(TaleWorlds.CampaignSystem.Clan.PlayerClan, true);
			}
			else
			{
				ChangeKingdomAction.ApplyByLeaveKingdom(TaleWorlds.CampaignSystem.Clan.PlayerClan, true);
			}
			this.ExecuteClose();
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00024DD8 File Offset: 0x00022FD8
		private void ExecuteChangeKingdomName()
		{
			InformationManager.ShowTextInquiry(new TextInquiryData(GameTexts.FindText("str_change_kingdom_name", null).ToString(), string.Empty, true, true, GameTexts.FindText("str_done", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action<string>(this.OnChangeKingdomNameDone), null, false, new Func<string, Tuple<bool, string>>(FactionHelper.IsKingdomNameApplicable), "", ""), false, false);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00024E4C File Offset: 0x0002304C
		private void OnChangeKingdomNameDone(string newKingdomName)
		{
			TextObject textObject = new TextObject(newKingdomName, null);
			TextObject textObject2 = GameTexts.FindText("str_generic_kingdom_name", null);
			TextObject textObject3 = GameTexts.FindText("str_generic_kingdom_short_name", null);
			textObject2.SetTextVariable("KINGDOM_NAME", textObject);
			textObject3.SetTextVariable("KINGDOM_SHORT_NAME", textObject);
			this.Kingdom.ChangeKingdomName(textObject2, textObject3, textObject2);
			this.OnRefresh();
			this.RefreshValues();
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00024EAD File Offset: 0x000230AD
		public void SelectArmy(Army army)
		{
			this.SetSelectedCategory(3);
			this.Army.SelectArmy(army);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00024EC2 File Offset: 0x000230C2
		public void SelectSettlement(Settlement settlement)
		{
			this.SetSelectedCategory(1);
			this.Settlement.SelectSettlement(settlement);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00024ED7 File Offset: 0x000230D7
		public void SelectClan(Clan clan)
		{
			this.SetSelectedCategory(0);
			this.Clan.SelectClan(clan);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00024EEC File Offset: 0x000230EC
		public void SelectPolicy(PolicyObject policy)
		{
			this.SetSelectedCategory(2);
			this.Policy.SelectPolicy(policy);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x00024F01 File Offset: 0x00023101
		public void SelectKingdom(Kingdom kingdom)
		{
			this.SetSelectedCategory(4);
			this.Diplomacy.SelectKingdom(kingdom);
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x00024F18 File Offset: 0x00023118
		public void SelectPreviousCategory()
		{
			int num = ((this._currentCategory == 0) ? (this._categoryCount - 1) : (this._currentCategory - 1));
			this.SetSelectedCategory(num);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00024F48 File Offset: 0x00023148
		public void SelectNextCategory()
		{
			int num = (this._currentCategory + 1) % this._categoryCount;
			this.SetSelectedCategory(num);
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00024F6C File Offset: 0x0002316C
		private void SetSelectedCategory(int index)
		{
			this.Clan.Show = false;
			this.Settlement.Show = false;
			this.Policy.Show = false;
			this.Army.Show = false;
			this.Diplomacy.Show = false;
			this._currentCategory = index;
			if (index == 0)
			{
				this.Clan.Show = true;
				return;
			}
			if (index == 1)
			{
				this.Settlement.Show = true;
				return;
			}
			if (index == 2)
			{
				this.Policy.Show = true;
				return;
			}
			if (index == 3)
			{
				this.Army.Show = true;
				return;
			}
			this._currentCategory = 4;
			this.Diplomacy.Show = true;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00025014 File Offset: 0x00023214
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._viewDataTracker.SetLastOpenedKingdomTabIndex(this._currentCategory);
			this.DoneInputKey.OnFinalize();
			this.PreviousTabInputKey.OnFinalize();
			this.NextTabInputKey.OnFinalize();
			this.Decision.OnFinalize();
			this.Clan.OnFinalize();
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0002506F File Offset: 0x0002326F
		// (set) Token: 0x060007D7 RID: 2007 RVA: 0x00025077 File Offset: 0x00023277
		[DataSourceProperty]
		public BasicTooltipViewModel KingdomActionHint
		{
			get
			{
				return this._kingdomActionHint;
			}
			set
			{
				if (value != this._kingdomActionHint)
				{
					this._kingdomActionHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "KingdomActionHint");
				}
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x060007D8 RID: 2008 RVA: 0x00025095 File Offset: 0x00023295
		// (set) Token: 0x060007D9 RID: 2009 RVA: 0x0002509D File Offset: 0x0002329D
		[DataSourceProperty]
		public BannerImageIdentifierVM KingdomBanner
		{
			get
			{
				return this._kingdomBanner;
			}
			set
			{
				if (value != this._kingdomBanner)
				{
					this._kingdomBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "KingdomBanner");
				}
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x000250BB File Offset: 0x000232BB
		// (set) Token: 0x060007DB RID: 2011 RVA: 0x000250C3 File Offset: 0x000232C3
		[DataSourceProperty]
		public HeroVM Leader
		{
			get
			{
				return this._leader;
			}
			set
			{
				if (value != this._leader)
				{
					this._leader = value;
					base.OnPropertyChangedWithValue<HeroVM>(value, "Leader");
				}
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x000250E1 File Offset: 0x000232E1
		// (set) Token: 0x060007DD RID: 2013 RVA: 0x000250E9 File Offset: 0x000232E9
		[DataSourceProperty]
		public KingdomArmyVM Army
		{
			get
			{
				return this._army;
			}
			set
			{
				if (value != this._army)
				{
					this._army = value;
					base.OnPropertyChangedWithValue<KingdomArmyVM>(value, "Army");
				}
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x00025107 File Offset: 0x00023307
		// (set) Token: 0x060007DF RID: 2015 RVA: 0x0002510F File Offset: 0x0002330F
		[DataSourceProperty]
		public KingdomSettlementVM Settlement
		{
			get
			{
				return this._settlement;
			}
			set
			{
				if (value != this._settlement)
				{
					this._settlement = value;
					base.OnPropertyChangedWithValue<KingdomSettlementVM>(value, "Settlement");
				}
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x0002512D File Offset: 0x0002332D
		// (set) Token: 0x060007E1 RID: 2017 RVA: 0x00025135 File Offset: 0x00023335
		[DataSourceProperty]
		public KingdomClanVM Clan
		{
			get
			{
				return this._clan;
			}
			set
			{
				if (value != this._clan)
				{
					this._clan = value;
					base.OnPropertyChangedWithValue<KingdomClanVM>(value, "Clan");
				}
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x00025153 File Offset: 0x00023353
		// (set) Token: 0x060007E3 RID: 2019 RVA: 0x0002515B File Offset: 0x0002335B
		[DataSourceProperty]
		public KingdomPoliciesVM Policy
		{
			get
			{
				return this._policy;
			}
			set
			{
				if (value != this._policy)
				{
					this._policy = value;
					base.OnPropertyChangedWithValue<KingdomPoliciesVM>(value, "Policy");
				}
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x00025179 File Offset: 0x00023379
		// (set) Token: 0x060007E5 RID: 2021 RVA: 0x00025181 File Offset: 0x00023381
		[DataSourceProperty]
		public KingdomDiplomacyVM Diplomacy
		{
			get
			{
				return this._diplomacy;
			}
			set
			{
				if (value != this._diplomacy)
				{
					this._diplomacy = value;
					base.OnPropertyChangedWithValue<KingdomDiplomacyVM>(value, "Diplomacy");
				}
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x0002519F File Offset: 0x0002339F
		// (set) Token: 0x060007E7 RID: 2023 RVA: 0x000251A7 File Offset: 0x000233A7
		[DataSourceProperty]
		public KingdomGiftFiefPopupVM GiftFief
		{
			get
			{
				return this._giftFief;
			}
			set
			{
				if (value != this._giftFief)
				{
					this._giftFief = value;
					base.OnPropertyChangedWithValue<KingdomGiftFiefPopupVM>(value, "GiftFief");
				}
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x000251C5 File Offset: 0x000233C5
		// (set) Token: 0x060007E9 RID: 2025 RVA: 0x000251CD File Offset: 0x000233CD
		[DataSourceProperty]
		public KingdomDecisionsVM Decision
		{
			get
			{
				return this._decision;
			}
			set
			{
				if (value != this._decision)
				{
					this._decision = value;
					base.OnPropertyChangedWithValue<KingdomDecisionsVM>(value, "Decision");
				}
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x000251EB File Offset: 0x000233EB
		// (set) Token: 0x060007EB RID: 2027 RVA: 0x000251F3 File Offset: 0x000233F3
		[DataSourceProperty]
		public HintViewModel ChangeKingdomNameHint
		{
			get
			{
				return this._changeKingdomNameHint;
			}
			set
			{
				if (value != this._changeKingdomNameHint)
				{
					this._changeKingdomNameHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ChangeKingdomNameHint");
				}
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x00025211 File Offset: 0x00023411
		// (set) Token: 0x060007ED RID: 2029 RVA: 0x00025219 File Offset: 0x00023419
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x0002523C File Offset: 0x0002343C
		// (set) Token: 0x060007EF RID: 2031 RVA: 0x00025244 File Offset: 0x00023444
		[DataSourceProperty]
		public bool CanSwitchTabs
		{
			get
			{
				return this._canSwitchTabs;
			}
			set
			{
				if (value != this._canSwitchTabs)
				{
					this._canSwitchTabs = value;
					base.OnPropertyChangedWithValue(value, "CanSwitchTabs");
				}
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x00025262 File Offset: 0x00023462
		// (set) Token: 0x060007F1 RID: 2033 RVA: 0x0002526A File Offset: 0x0002346A
		[DataSourceProperty]
		public bool PlayerHasKingdom
		{
			get
			{
				return this._playerHasKingdom;
			}
			set
			{
				if (value != this._playerHasKingdom)
				{
					this._playerHasKingdom = value;
					base.OnPropertyChangedWithValue(value, "PlayerHasKingdom");
				}
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x00025288 File Offset: 0x00023488
		// (set) Token: 0x060007F3 RID: 2035 RVA: 0x00025290 File Offset: 0x00023490
		[DataSourceProperty]
		public bool IsKingdomActionEnabled
		{
			get
			{
				return this._isKingdomActionEnabled;
			}
			set
			{
				if (value != this._isKingdomActionEnabled)
				{
					this._isKingdomActionEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsKingdomActionEnabled");
				}
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x000252AE File Offset: 0x000234AE
		// (set) Token: 0x060007F5 RID: 2037 RVA: 0x000252B6 File Offset: 0x000234B6
		[DataSourceProperty]
		public bool PlayerCanChangeKingdomName
		{
			get
			{
				return this._playerCanChangeKingdomName;
			}
			set
			{
				if (value != this._playerCanChangeKingdomName)
				{
					this._playerCanChangeKingdomName = value;
					base.OnPropertyChangedWithValue(value, "PlayerCanChangeKingdomName");
				}
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x000252D4 File Offset: 0x000234D4
		// (set) Token: 0x060007F7 RID: 2039 RVA: 0x000252DC File Offset: 0x000234DC
		[DataSourceProperty]
		public string LeaderText
		{
			get
			{
				return this._leaderText;
			}
			set
			{
				if (value != this._leaderText)
				{
					this._leaderText = value;
					base.OnPropertyChangedWithValue<string>(value, "LeaderText");
				}
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x000252FF File Offset: 0x000234FF
		// (set) Token: 0x060007F9 RID: 2041 RVA: 0x00025307 File Offset: 0x00023507
		[DataSourceProperty]
		public string KingdomActionText
		{
			get
			{
				return this._kingdomActionText;
			}
			set
			{
				if (value != this._kingdomActionText)
				{
					this._kingdomActionText = value;
					base.OnPropertyChangedWithValue<string>(value, "KingdomActionText");
				}
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x0002532A File Offset: 0x0002352A
		// (set) Token: 0x060007FB RID: 2043 RVA: 0x00025332 File Offset: 0x00023532
		[DataSourceProperty]
		public string ClansText
		{
			get
			{
				return this._clansText;
			}
			set
			{
				if (value != this._clansText)
				{
					this._clansText = value;
					base.OnPropertyChangedWithValue<string>(value, "ClansText");
				}
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x00025355 File Offset: 0x00023555
		// (set) Token: 0x060007FD RID: 2045 RVA: 0x0002535D File Offset: 0x0002355D
		[DataSourceProperty]
		public string DiplomacyText
		{
			get
			{
				return this._diplomacyText;
			}
			set
			{
				if (value != this._diplomacyText)
				{
					this._diplomacyText = value;
					base.OnPropertyChangedWithValue<string>(value, "DiplomacyText");
				}
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x00025380 File Offset: 0x00023580
		// (set) Token: 0x060007FF RID: 2047 RVA: 0x00025388 File Offset: 0x00023588
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

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000800 RID: 2048 RVA: 0x000253AB File Offset: 0x000235AB
		// (set) Token: 0x06000801 RID: 2049 RVA: 0x000253B3 File Offset: 0x000235B3
		[DataSourceProperty]
		public string FiefsText
		{
			get
			{
				return this._fiefsText;
			}
			set
			{
				if (value != this._fiefsText)
				{
					this._fiefsText = value;
					base.OnPropertyChangedWithValue<string>(value, "FiefsText");
				}
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x000253D6 File Offset: 0x000235D6
		// (set) Token: 0x06000803 RID: 2051 RVA: 0x000253DE File Offset: 0x000235DE
		[DataSourceProperty]
		public string PoliciesText
		{
			get
			{
				return this._policiesText;
			}
			set
			{
				if (value != this._policiesText)
				{
					this._policiesText = value;
					base.OnPropertyChangedWithValue<string>(value, "PoliciesText");
				}
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000804 RID: 2052 RVA: 0x00025401 File Offset: 0x00023601
		// (set) Token: 0x06000805 RID: 2053 RVA: 0x00025409 File Offset: 0x00023609
		[DataSourceProperty]
		public string ArmiesText
		{
			get
			{
				return this._armiesText;
			}
			set
			{
				if (value != this._armiesText)
				{
					this._armiesText = value;
					base.OnPropertyChangedWithValue<string>(value, "ArmiesText");
				}
			}
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x0002542C File Offset: 0x0002362C
		public void SetDoneInputKey(HotKey hotkey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
			this.Decision.SetDoneInputKey(hotkey);
			this.GiftFief.SetDoneInputKey(hotkey);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00025453 File Offset: 0x00023653
		public void SetCancelInputKey(HotKey hotkey)
		{
			this.GiftFief.SetCancelInputKey(hotkey);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00025461 File Offset: 0x00023661
		public void SetPreviousTabInputKey(HotKey hotkey)
		{
			this.PreviousTabInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00025470 File Offset: 0x00023670
		public void SetNextTabInputKey(HotKey hotkey)
		{
			this.NextTabInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x0002547F File Offset: 0x0002367F
		// (set) Token: 0x0600080B RID: 2059 RVA: 0x00025487 File Offset: 0x00023687
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

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x000254A5 File Offset: 0x000236A5
		// (set) Token: 0x0600080D RID: 2061 RVA: 0x000254AD File Offset: 0x000236AD
		public InputKeyItemVM PreviousTabInputKey
		{
			get
			{
				return this._previousTabInputKey;
			}
			set
			{
				if (value != this._previousTabInputKey)
				{
					this._previousTabInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "PreviousTabInputKey");
				}
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x0600080E RID: 2062 RVA: 0x000254CB File Offset: 0x000236CB
		// (set) Token: 0x0600080F RID: 2063 RVA: 0x000254D3 File Offset: 0x000236D3
		public InputKeyItemVM NextTabInputKey
		{
			get
			{
				return this._nextTabInputKey;
			}
			set
			{
				if (value != this._nextTabInputKey)
				{
					this._nextTabInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "NextTabInputKey");
				}
			}
		}

		// Token: 0x0400034D RID: 845
		private readonly Action _onClose;

		// Token: 0x0400034E RID: 846
		private readonly Action<Army> _onShowArmyOnMap;

		// Token: 0x0400034F RID: 847
		private readonly int _categoryCount;

		// Token: 0x04000350 RID: 848
		private readonly LeaveKingdomPermissionEvent _leaveKingdomPermissionEvent;

		// Token: 0x04000351 RID: 849
		private ValueTuple<bool, TextObject>? _mostRecentLeaveKingdomPermission;

		// Token: 0x04000352 RID: 850
		private readonly IViewDataTracker _viewDataTracker;

		// Token: 0x04000353 RID: 851
		private bool _isPlayerTheRuler;

		// Token: 0x04000354 RID: 852
		private int _currentCategory;

		// Token: 0x04000356 RID: 854
		private KingdomArmyVM _army;

		// Token: 0x04000357 RID: 855
		private KingdomSettlementVM _settlement;

		// Token: 0x04000358 RID: 856
		private KingdomClanVM _clan;

		// Token: 0x04000359 RID: 857
		private KingdomPoliciesVM _policy;

		// Token: 0x0400035A RID: 858
		private KingdomDiplomacyVM _diplomacy;

		// Token: 0x0400035B RID: 859
		private KingdomGiftFiefPopupVM _giftFief;

		// Token: 0x0400035C RID: 860
		private BannerImageIdentifierVM _kingdomBanner;

		// Token: 0x0400035D RID: 861
		private HeroVM _leader;

		// Token: 0x0400035E RID: 862
		private KingdomDecisionsVM _decision;

		// Token: 0x0400035F RID: 863
		private HintViewModel _changeKingdomNameHint;

		// Token: 0x04000360 RID: 864
		private string _name;

		// Token: 0x04000361 RID: 865
		private bool _canSwitchTabs;

		// Token: 0x04000362 RID: 866
		private bool _playerHasKingdom;

		// Token: 0x04000363 RID: 867
		private bool _isKingdomActionEnabled;

		// Token: 0x04000364 RID: 868
		private bool _playerCanChangeKingdomName;

		// Token: 0x04000365 RID: 869
		private string _kingdomActionText;

		// Token: 0x04000366 RID: 870
		private string _leaderText;

		// Token: 0x04000367 RID: 871
		private string _clansText;

		// Token: 0x04000368 RID: 872
		private string _fiefsText;

		// Token: 0x04000369 RID: 873
		private string _policiesText;

		// Token: 0x0400036A RID: 874
		private string _armiesText;

		// Token: 0x0400036B RID: 875
		private string _diplomacyText;

		// Token: 0x0400036C RID: 876
		private string _doneText;

		// Token: 0x0400036D RID: 877
		private BasicTooltipViewModel _kingdomActionHint;

		// Token: 0x0400036E RID: 878
		private InputKeyItemVM _doneInputKey;

		// Token: 0x0400036F RID: 879
		private InputKeyItemVM _previousTabInputKey;

		// Token: 0x04000370 RID: 880
		private InputKeyItemVM _nextTabInputKey;
	}
}
