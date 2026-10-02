using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions
{
	// Token: 0x0200007E RID: 126
	public class KingdomDecisionsVM : ViewModel
	{
		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x060009EE RID: 2542 RVA: 0x0002B689 File Offset: 0x00029889
		public bool IsCurrentDecisionActive
		{
			get
			{
				DecisionItemBaseVM currentDecision = this.CurrentDecision;
				return currentDecision != null && currentDecision.IsActive;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x060009EF RID: 2543 RVA: 0x0002B69C File Offset: 0x0002989C
		// (set) Token: 0x060009F0 RID: 2544 RVA: 0x0002B6A4 File Offset: 0x000298A4
		private bool _shouldCheckForDecision { get; set; } = true;

		// Token: 0x060009F1 RID: 2545 RVA: 0x0002B6B0 File Offset: 0x000298B0
		public KingdomDecisionsVM(Action refreshKingdomManagement)
		{
			this._refreshKingdomManagement = refreshKingdomManagement;
			this._examinedDecisionsSinceInit = new List<KingdomDecision>();
			this._examinedDecisionsSinceInit.AddRange(Clan.PlayerClan.Kingdom.UnresolvedDecisions.Where<KingdomDecision>((KingdomDecision d) => d.ShouldBeCancelled()));
			this._solvedDecisionsSinceInit = new List<KingdomDecision>();
			CampaignEvents.KingdomDecisionConcluded.AddNonSerializedListener(this, new Action<KingdomDecision, DecisionOutcome, bool>(this.OnKingdomDecisionConcluded));
			this.IsRefreshed = true;
			this.RefreshValues();
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0002B749 File Offset: 0x00029949
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TitleText = GameTexts.FindText("str_kingdom_decisions", null).ToString();
			DecisionItemBaseVM currentDecision = this.CurrentDecision;
			if (currentDecision == null)
			{
				return;
			}
			currentDecision.RefreshValues();
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0002B778 File Offset: 0x00029978
		public void OnFrameTick()
		{
			this.IsActive = this.IsCurrentDecisionActive;
			IEnumerable<KingdomDecision> enumerable = Clan.PlayerClan.Kingdom.UnresolvedDecisions.Except<KingdomDecision>(this._examinedDecisionsSinceInit);
			if (this._shouldCheckForDecision)
			{
				if (this.CurrentDecision != null)
				{
					DecisionItemBaseVM currentDecision = this.CurrentDecision;
					if (currentDecision == null || currentDecision.IsActive)
					{
						return;
					}
				}
				if (enumerable.Any<KingdomDecision>())
				{
					KingdomDecision kingdomDecision = this._solvedDecisionsSinceInit.LastOrDefault<KingdomDecision>();
					KingdomDecision kingdomDecision2 = ((kingdomDecision != null) ? kingdomDecision.GetFollowUpDecision() : null);
					if (kingdomDecision2 != null)
					{
						this.HandleDecision(kingdomDecision2);
						return;
					}
					this.HandleNextDecision();
				}
			}
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0002B804 File Offset: 0x00029A04
		public void HandleNextDecision()
		{
			this.HandleDecision(Clan.PlayerClan.Kingdom.UnresolvedDecisions.Except<KingdomDecision>(this._examinedDecisionsSinceInit).FirstOrDefault<KingdomDecision>());
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0002B82C File Offset: 0x00029A2C
		public void HandleDecision(KingdomDecision curDecision)
		{
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				this._shouldCheckForDecision = false;
				return;
			}
			KingdomDecision curDecision2 = curDecision;
			if (curDecision2 != null && !curDecision2.ShouldBeCancelled())
			{
				this._shouldCheckForDecision = false;
				this._examinedDecisionsSinceInit.Add(curDecision);
				if (curDecision.IsPlayerParticipant)
				{
					TextObject generalTitle = new KingdomElection(curDecision).GetGeneralTitle();
					GameTexts.SetVariable("DECISION_NAME", generalTitle.ToString());
					string text = (curDecision.NeedsPlayerResolution ? GameTexts.FindText("str_you_need_to_resolve_decision", null).ToString() : GameTexts.FindText("str_do_you_want_to_resolve_decision", null).ToString());
					if (!curDecision.NeedsPlayerResolution && curDecision.TriggerTime.IsFuture)
					{
						GameTexts.SetVariable("HOUR", ((int)curDecision.TriggerTime.RemainingHoursFromNow).ToString());
						GameTexts.SetVariable("newline", "\n");
						GameTexts.SetVariable("STR1", text);
						GameTexts.SetVariable("STR2", GameTexts.FindText("str_decision_will_be_resolved_in_hours", null));
						text = GameTexts.FindText("str_string_newline_string", null).ToString();
					}
					this._queryData = new InquiryData(GameTexts.FindText("str_decision", null).ToString(), text, true, !curDecision.NeedsPlayerResolution, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), delegate
					{
						this.RefreshWith(curDecision);
					}, delegate
					{
						this._shouldCheckForDecision = true;
					}, "", 0f, null, null, null);
					this._shouldCheckForDecision = false;
					InformationManager.ShowInquiry(this._queryData, false, false);
					return;
				}
			}
			else
			{
				this._shouldCheckForDecision = false;
				this._queryData = null;
			}
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0002BA10 File Offset: 0x00029C10
		public void RefreshWith(KingdomDecision decision)
		{
			if (decision.IsSingleClanDecision())
			{
				KingdomElection kingdomElection = new KingdomElection(decision);
				kingdomElection.StartElection();
				kingdomElection.ApplySelection();
				InformationManager.ShowInquiry(new InquiryData(GameTexts.FindText("str_decision_outcome", null).ToString(), kingdomElection.GetChosenOutcomeText().ToString(), true, false, GameTexts.FindText("str_ok", null).ToString(), "", delegate
				{
					this.OnSingleDecisionOver();
				}, null, "", 0f, null, null, null), false, false);
				return;
			}
			this._shouldCheckForDecision = false;
			this.CurrentDecision = this.GetDecisionItemBasedOnType(decision);
			this.CurrentDecision.SetDoneInputKey(this.DoneInputKey);
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0002BAB6 File Offset: 0x00029CB6
		private void OnSingleDecisionOver()
		{
			this._refreshKingdomManagement();
			this._shouldCheckForDecision = true;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0002BACA File Offset: 0x00029CCA
		private void OnDecisionOver()
		{
			this._refreshKingdomManagement();
			DecisionItemBaseVM currentDecision = this.CurrentDecision;
			if (currentDecision != null)
			{
				currentDecision.OnFinalize();
			}
			this.CurrentDecision = null;
			this._shouldCheckForDecision = true;
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0002BAF6 File Offset: 0x00029CF6
		private void OnKingdomDecisionConcluded(KingdomDecision decision, DecisionOutcome outcome, bool isPlayerInvolved)
		{
			if (isPlayerInvolved)
			{
				this._solvedDecisionsSinceInit.Add(decision);
			}
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0002BB08 File Offset: 0x00029D08
		private DecisionItemBaseVM GetDecisionItemBasedOnType(KingdomDecision decision)
		{
			SettlementClaimantDecision settlementClaimantDecision;
			if ((settlementClaimantDecision = decision as SettlementClaimantDecision) != null)
			{
				return new SettlementDecisionItemVM(settlementClaimantDecision.Settlement, decision, new Action(this.OnDecisionOver));
			}
			SettlementClaimantPreliminaryDecision settlementClaimantPreliminaryDecision;
			if ((settlementClaimantPreliminaryDecision = decision as SettlementClaimantPreliminaryDecision) != null)
			{
				return new SettlementDecisionItemVM(settlementClaimantPreliminaryDecision.Settlement, decision, new Action(this.OnDecisionOver));
			}
			ExpelClanFromKingdomDecision expelClanFromKingdomDecision;
			if ((expelClanFromKingdomDecision = decision as ExpelClanFromKingdomDecision) != null)
			{
				return new ExpelClanDecisionItemVM(expelClanFromKingdomDecision, new Action(this.OnDecisionOver));
			}
			KingdomPolicyDecision kingdomPolicyDecision;
			if ((kingdomPolicyDecision = decision as KingdomPolicyDecision) != null)
			{
				return new PolicyDecisionItemVM(kingdomPolicyDecision, new Action(this.OnDecisionOver));
			}
			DeclareWarDecision declareWarDecision;
			if ((declareWarDecision = decision as DeclareWarDecision) != null)
			{
				return new DeclareWarDecisionItemVM(declareWarDecision, new Action(this.OnDecisionOver));
			}
			MakePeaceKingdomDecision makePeaceKingdomDecision;
			if ((makePeaceKingdomDecision = decision as MakePeaceKingdomDecision) != null)
			{
				return new MakePeaceDecisionItemVM(makePeaceKingdomDecision, new Action(this.OnDecisionOver));
			}
			KingSelectionKingdomDecision kingSelectionKingdomDecision;
			if ((kingSelectionKingdomDecision = decision as KingSelectionKingdomDecision) != null)
			{
				return new KingSelectionDecisionItemVM(kingSelectionKingdomDecision, new Action(this.OnDecisionOver));
			}
			StartAllianceDecision startAllianceDecision;
			if ((startAllianceDecision = decision as StartAllianceDecision) != null)
			{
				return new StartAllianceDecisionItemVM(startAllianceDecision, new Action(this.OnDecisionOver));
			}
			ProposeCallToWarAgreementDecision proposeCallToWarAgreementDecision;
			if ((proposeCallToWarAgreementDecision = decision as ProposeCallToWarAgreementDecision) != null)
			{
				return new ProposeCallToWarAgreementDecisionItemVM(proposeCallToWarAgreementDecision, new Action(this.OnDecisionOver));
			}
			AcceptCallToWarAgreementDecision acceptCallToWarAgreementDecision;
			if ((acceptCallToWarAgreementDecision = decision as AcceptCallToWarAgreementDecision) != null)
			{
				return new AcceptingCallToWarAgreementDecisionItemVM(acceptCallToWarAgreementDecision, new Action(this.OnDecisionOver));
			}
			TradeAgreementDecision tradeAgreementDecision;
			if ((tradeAgreementDecision = decision as TradeAgreementDecision) != null)
			{
				return new TradeAgreementDecisionItemVM(tradeAgreementDecision, new Action(this.OnDecisionOver));
			}
			Debug.FailedAssert("No defined decision type for this decision! This shouldn't happen", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\KingdomManagement\\Decisions\\KingdomDecisionsVM.cs", "GetDecisionItemBasedOnType", 215);
			return new DecisionItemBaseVM(decision, new Action(this.OnDecisionOver));
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0002BC99 File Offset: 0x00029E99
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.DoneInputKey.OnFinalize();
			DecisionItemBaseVM currentDecision = this.CurrentDecision;
			if (currentDecision != null)
			{
				currentDecision.OnFinalize();
			}
			this.CurrentDecision = null;
			CampaignEvents.KingdomDecisionConcluded.ClearListeners(this);
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0002BCCF File Offset: 0x00029ECF
		public void SetDoneInputKey(HotKey hotKey)
		{
			this.DoneInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x060009FD RID: 2557 RVA: 0x0002BCDE File Offset: 0x00029EDE
		// (set) Token: 0x060009FE RID: 2558 RVA: 0x0002BCE6 File Offset: 0x00029EE6
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

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x060009FF RID: 2559 RVA: 0x0002BD04 File Offset: 0x00029F04
		// (set) Token: 0x06000A00 RID: 2560 RVA: 0x0002BD0C File Offset: 0x00029F0C
		[DataSourceProperty]
		public DecisionItemBaseVM CurrentDecision
		{
			get
			{
				return this._currentDecision;
			}
			set
			{
				if (value != this._currentDecision)
				{
					this._currentDecision = value;
					base.OnPropertyChangedWithValue<DecisionItemBaseVM>(value, "CurrentDecision");
				}
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0002BD2A File Offset: 0x00029F2A
		// (set) Token: 0x06000A02 RID: 2562 RVA: 0x0002BD32 File Offset: 0x00029F32
		[DataSourceProperty]
		public int NotificationCount
		{
			get
			{
				return this._notificationCount;
			}
			set
			{
				if (value != this._notificationCount)
				{
					this._notificationCount = value;
					base.OnPropertyChangedWithValue(value, "NotificationCount");
				}
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000A03 RID: 2563 RVA: 0x0002BD50 File Offset: 0x00029F50
		// (set) Token: 0x06000A04 RID: 2564 RVA: 0x0002BD58 File Offset: 0x00029F58
		[DataSourceProperty]
		public bool IsRefreshed
		{
			get
			{
				return this._isRefreshed;
			}
			set
			{
				if (value != this._isRefreshed)
				{
					this._isRefreshed = value;
					base.OnPropertyChangedWithValue(value, "IsRefreshed");
				}
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x0002BD76 File Offset: 0x00029F76
		// (set) Token: 0x06000A06 RID: 2566 RVA: 0x0002BD7E File Offset: 0x00029F7E
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

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x0002BD9C File Offset: 0x00029F9C
		// (set) Token: 0x06000A08 RID: 2568 RVA: 0x0002BDA4 File Offset: 0x00029FA4
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

		// Token: 0x04000460 RID: 1120
		private List<KingdomDecision> _examinedDecisionsSinceInit;

		// Token: 0x04000461 RID: 1121
		private List<KingdomDecision> _solvedDecisionsSinceInit;

		// Token: 0x04000462 RID: 1122
		private readonly Action _refreshKingdomManagement;

		// Token: 0x04000464 RID: 1124
		private InquiryData _queryData;

		// Token: 0x04000465 RID: 1125
		private InputKeyItemVM _doneInputKey;

		// Token: 0x04000466 RID: 1126
		private bool _isRefreshed;

		// Token: 0x04000467 RID: 1127
		private bool _isActive;

		// Token: 0x04000468 RID: 1128
		private int _notificationCount;

		// Token: 0x04000469 RID: 1129
		private string _titleText;

		// Token: 0x0400046A RID: 1130
		private DecisionItemBaseVM _currentDecision;
	}
}
