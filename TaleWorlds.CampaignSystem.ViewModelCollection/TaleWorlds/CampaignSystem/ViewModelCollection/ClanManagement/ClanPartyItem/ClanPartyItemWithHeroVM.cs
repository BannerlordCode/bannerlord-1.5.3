using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanPartyItem
{
	// Token: 0x02000135 RID: 309
	public class ClanPartyItemWithHeroVM : ClanPartyItemVM
	{
		// Token: 0x170009CE RID: 2510
		// (get) Token: 0x06001CD2 RID: 7378 RVA: 0x000690CB File Offset: 0x000672CB
		// (set) Token: 0x06001CD3 RID: 7379 RVA: 0x000690D3 File Offset: 0x000672D3
		public override int Expense { get; protected set; }

		// Token: 0x170009CF RID: 2511
		// (get) Token: 0x06001CD4 RID: 7380 RVA: 0x000690DC File Offset: 0x000672DC
		// (set) Token: 0x06001CD5 RID: 7381 RVA: 0x000690E4 File Offset: 0x000672E4
		public override int Income { get; protected set; }

		// Token: 0x170009D0 RID: 2512
		// (get) Token: 0x06001CD6 RID: 7382 RVA: 0x000690ED File Offset: 0x000672ED
		public override Hero Leader { get; }

		// Token: 0x170009D1 RID: 2513
		// (get) Token: 0x06001CD7 RID: 7383 RVA: 0x000690F5 File Offset: 0x000672F5
		public override CampaignVec2 Position
		{
			get
			{
				return CampaignVec2.Invalid;
			}
		}

		// Token: 0x06001CD8 RID: 7384 RVA: 0x000690FC File Offset: 0x000672FC
		public ClanPartyItemWithHeroVM(Hero hero, Action<ClanPartyItemVM> onAssignment, Action onExpenseChange, Action onShowChangeLeaderPopup, Action<ClanRoleItemVM> onShowChangeRolePopup, ClanPartyItemVM.ClanPartyType type, IDisbandPartyCampaignBehavior disbandBehavior, ITeleportationCampaignBehavior teleportationBehavior)
		{
			this.Leader = hero;
			this.HasHeroMembers = true;
			this.IsPendingPartyCreation = true;
			base.AreCommandControlsVisible = type == ClanPartyItemVM.ClanPartyType.Member;
			if (this.Leader != null)
			{
				CharacterCode characterCode = ClanPartyItemVM.GetCharacterCode(this.Leader.CharacterObject);
				this.LeaderVisual = new CharacterImageIdentifierVM(characterCode);
				this.CharacterModel = new CharacterViewModel(CharacterViewModel.StanceTypes.None);
				CharacterViewModel characterModel = this.CharacterModel;
				BasicCharacterObject characterObject = this.Leader.CharacterObject;
				int num = -1;
				Banner banner = this.Leader.MapFaction.Banner;
				characterModel.FillFrom(characterObject, num, (banner != null) ? banner.BannerCode : null);
				CharacterViewModel characterModel2 = this.CharacterModel;
				IFaction mapFaction = this.Leader.MapFaction;
				characterModel2.ArmorColor1 = ((mapFaction != null) ? mapFaction.Color : 0U);
				CharacterViewModel characterModel3 = this.CharacterModel;
				IFaction mapFaction2 = this.Leader.MapFaction;
				characterModel3.ArmorColor2 = ((mapFaction2 != null) ? mapFaction2.Color2 : 0U);
				base.AllowRaiding = this.Leader.CanRaid;
				base.DonateTroopsToGarrisons = this.Leader.CanDonateTroopsToGarrison;
				base.MayJoinOtherArmies = this.Leader.CanJoinArmy;
				base.HasFleet = this.Leader.CanHaveFleet;
			}
			else
			{
				this.LeaderVisual = new CharacterImageIdentifierVM(null);
				this.CharacterModel = new CharacterViewModel();
			}
			this._onAssignment = onAssignment;
			this._onShowChangeLeaderPopup = onShowChangeLeaderPopup;
			this.IsDisbanding = false;
			TextObject empty = TextObject.GetEmpty();
			this.IsChangeLeaderVisible = true;
			this.IsChangeLeaderEnabled = this.IsChangeLeaderVisible && CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out empty);
			this.ChangeLeaderHint = new HintViewModel(this.IsChangeLeaderEnabled ? this._changeLeaderHintText : empty, null);
			this.ActionsDisabledHint = new HintViewModel();
			TextObject textObject;
			this.CanUseActions = CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject);
			this.ActionsDisabledHint.HintText = (this.CanUseActions ? TextObject.GetEmpty() : textObject);
			this.AutoRecruitmentHint = null;
			this.IsAutoRecruitmentVisible = false;
			this.AutoRecruitmentValue = false;
			this.HeroMembers = new MBBindingList<ClanPartyMemberItemVM>();
			this.Roles = new MBBindingList<ClanRoleItemVM>();
			this.InfantryHint = null;
			this.CavalryHint = null;
			this.RangedHint = null;
			this.HorseArcherHint = null;
			this.InArmyHint = new HintViewModel();
			base.SmallShipHint = new HintViewModel(new TextObject("{=SeXdiWJL}Small Ships", null), null);
			base.MediumShipHint = new HintViewModel(new TextObject("{=XcIDr42e}Medium Ships", null), null);
			base.LargeShipHint = new HintViewModel(new TextObject("{=ReqtAxsC}Large Ships", null), null);
			this.RefreshValues();
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x00069370 File Offset: 0x00067570
		public override void UpdateProperties()
		{
			this.MembersText = GameTexts.FindText("str_members", null).ToString();
			this.AssigneesText = GameTexts.FindText("str_clan_assignee_title", null).ToString();
			this.RolesText = GameTexts.FindText("str_clan_role_title", null).ToString();
			this.PartyLeaderRoleEffectsText = GameTexts.FindText("str_clan_party_leader_roles_and_effects", null).ToString();
			this.AutoRecruitmentText = GameTexts.FindText("str_clan_auto_recruitment", null).ToString();
			TextObject textObject = new TextObject("{=shL0WElC}{TROOP.NAME}{.o} Party", null);
			textObject.SetCharacterProperties("TROOP", this.Leader.CharacterObject, false);
			this.Name = textObject.ToString();
			IEmptyClanPartiesCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IEmptyClanPartiesCampaignBehavior>();
			this.ShipCount = campaignBehavior.GetShipCountForCachedLordPartyForPlayerClan(this.Leader);
			this.PartyLocationText = CampaignUIHelper.GetHeroBehaviorText(this.Leader, null);
			TextObject textObject2 = GameTexts.FindText("str_LEFT_over_RIGHT", null);
			textObject2.SetTextVariable("LEFT", 0);
			textObject2.SetTextVariable("RIGHT", CampaignUIHelper.GetPartySizeLimitForLeader(this.Leader));
			this.PartySizeText = textObject2.ToString();
			TextObject textObject3 = GameTexts.FindText("str_LEFT_colon_RIGHT", null);
			textObject3.SetTextVariable("LEFT", GameTexts.FindText("str_party_morale_party_size", null));
			textObject3.SetTextVariable("RIGHT", textObject2);
			this.PartySizeSubTitleText = textObject3.ToString();
			this.HeroMembers.Clear();
			ClanPartyMemberItemVM clanPartyMemberItemVM = new ClanPartyMemberItemVM(this.Leader, null);
			this.HeroMembers.Add(clanPartyMemberItemVM);
			if (clanPartyMemberItemVM.IsLeader)
			{
				this.LeaderMember = clanPartyMemberItemVM;
			}
			if (this.IsMembersAndRolesVisible)
			{
				this.Roles.ApplyActionOnAllItems(delegate(ClanRoleItemVM x)
				{
					x.OnFinalize();
				});
				this.Roles.Clear();
				foreach (PartyRole partyRole in Campaign.Current.Models.ClanMemberPartyRoleModel.GetAssignablePartyRoles())
				{
					this.Roles.Add(new ClanRoleItemVM(null, partyRole, this.HeroMembers, new Action<ClanRoleItemVM>(this.OnRoleSelectionToggled)));
				}
			}
			this.RefreshFleetComposition();
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x000695B0 File Offset: 0x000677B0
		private void RefreshFleetComposition()
		{
			List<Ship> shipsForCachedLordPartyForPlayerClan = Campaign.Current.GetCampaignBehavior<IEmptyClanPartiesCampaignBehavior>().GetShipsForCachedLordPartyForPlayerClan(this.Leader);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (Ship ship in shipsForCachedLordPartyForPlayerClan)
			{
				ShipHull shipHull = ship.ShipHull;
				ShipHull.ShipType? shipType = ((shipHull != null) ? new ShipHull.ShipType?(shipHull.Type) : null);
				if (shipType != null)
				{
					switch (shipType.GetValueOrDefault())
					{
					case ShipHull.ShipType.Light:
						num++;
						break;
					case ShipHull.ShipType.Medium:
						num2++;
						break;
					case ShipHull.ShipType.Heavy:
						num3++;
						break;
					}
				}
			}
			base.SmallShipCount = num;
			base.MediumShipCount = num2;
			base.LargeShipCount = num3;
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x00069680 File Offset: 0x00067880
		public override void ExecuteChangeLeader()
		{
			Action onShowChangeLeaderPopup = this._onShowChangeLeaderPopup;
			if (onShowChangeLeaderPopup == null)
			{
				return;
			}
			onShowChangeLeaderPopup();
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x00069692 File Offset: 0x00067892
		public override void OnPartySelection()
		{
			this._onAssignment(this);
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000696A0 File Offset: 0x000678A0
		private void OnRoleSelectionToggled(ClanRoleItemVM role)
		{
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06001CDE RID: 7390 RVA: 0x000696A2 File Offset: 0x000678A2
		// (set) Token: 0x06001CDF RID: 7391 RVA: 0x000696AA File Offset: 0x000678AA
		[DataSourceProperty]
		public override CharacterViewModel CharacterModel
		{
			get
			{
				return this._characterModel;
			}
			set
			{
				if (value != this._characterModel)
				{
					this._characterModel = value;
					base.OnPropertyChangedWithValue<CharacterViewModel>(value, "CharacterModel");
				}
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x06001CE0 RID: 7392 RVA: 0x000696C8 File Offset: 0x000678C8
		// (set) Token: 0x06001CE1 RID: 7393 RVA: 0x000696D0 File Offset: 0x000678D0
		[DataSourceProperty]
		public override CharacterImageIdentifierVM LeaderVisual
		{
			get
			{
				return this._leaderVisual;
			}
			set
			{
				if (value != this._leaderVisual)
				{
					this._leaderVisual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "LeaderVisual");
				}
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x06001CE2 RID: 7394 RVA: 0x000696EE File Offset: 0x000678EE
		// (set) Token: 0x06001CE3 RID: 7395 RVA: 0x000696F6 File Offset: 0x000678F6
		[DataSourceProperty]
		public override bool IsSelected
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

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x06001CE4 RID: 7396 RVA: 0x00069714 File Offset: 0x00067914
		// (set) Token: 0x06001CE5 RID: 7397 RVA: 0x00069717 File Offset: 0x00067917
		[DataSourceProperty]
		public override bool HasHeroMembers
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x06001CE6 RID: 7398 RVA: 0x00069719 File Offset: 0x00067919
		// (set) Token: 0x06001CE7 RID: 7399 RVA: 0x00069721 File Offset: 0x00067921
		[DataSourceProperty]
		public override bool IsClanRoleSelectionHighlightEnabled
		{
			get
			{
				return this._isClanRoleSelectionHighlightEnabled;
			}
			set
			{
				if (value != this._isClanRoleSelectionHighlightEnabled)
				{
					this._isClanRoleSelectionHighlightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsClanRoleSelectionHighlightEnabled");
				}
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x06001CE8 RID: 7400 RVA: 0x0006973F File Offset: 0x0006793F
		// (set) Token: 0x06001CE9 RID: 7401 RVA: 0x00069747 File Offset: 0x00067947
		[DataSourceProperty]
		public override bool IsRoleSelectionPopupVisible
		{
			get
			{
				return this._isRoleSelectionPopupVisible;
			}
			set
			{
				if (value != this._isRoleSelectionPopupVisible)
				{
					this._isRoleSelectionPopupVisible = value;
					base.OnPropertyChangedWithValue(value, "IsRoleSelectionPopupVisible");
				}
			}
		}

		// Token: 0x170009D8 RID: 2520
		// (get) Token: 0x06001CEA RID: 7402 RVA: 0x00069765 File Offset: 0x00067965
		// (set) Token: 0x06001CEB RID: 7403 RVA: 0x0006976D File Offset: 0x0006796D
		[DataSourceProperty]
		public override bool IsPendingPartyCreation
		{
			get
			{
				return this._isPendingPartyCreation;
			}
			set
			{
				if (value != this._isPendingPartyCreation)
				{
					this._isPendingPartyCreation = value;
					base.OnPropertyChangedWithValue(value, "IsPendingPartyCreation");
				}
			}
		}

		// Token: 0x170009D9 RID: 2521
		// (get) Token: 0x06001CEC RID: 7404 RVA: 0x0006978B File Offset: 0x0006798B
		// (set) Token: 0x06001CED RID: 7405 RVA: 0x00069793 File Offset: 0x00067993
		[DataSourceProperty]
		public override bool IsDisbanding
		{
			get
			{
				return this._isDisbanding;
			}
			set
			{
				if (value != this._isDisbanding)
				{
					this._isDisbanding = value;
					base.OnPropertyChangedWithValue(value, "IsDisbanding");
				}
			}
		}

		// Token: 0x170009DA RID: 2522
		// (get) Token: 0x06001CEE RID: 7406 RVA: 0x000697B1 File Offset: 0x000679B1
		// (set) Token: 0x06001CEF RID: 7407 RVA: 0x000697B9 File Offset: 0x000679B9
		[DataSourceProperty]
		public override bool IsInArmy
		{
			get
			{
				return this._isInArmy;
			}
			set
			{
				if (value != this._isInArmy)
				{
					this._isInArmy = value;
					base.OnPropertyChangedWithValue(value, "IsInArmy");
				}
			}
		}

		// Token: 0x170009DB RID: 2523
		// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x000697D7 File Offset: 0x000679D7
		// (set) Token: 0x06001CF1 RID: 7409 RVA: 0x000697DF File Offset: 0x000679DF
		[DataSourceProperty]
		public override bool CanUseActions
		{
			get
			{
				return this._canUseActions;
			}
			set
			{
				if (value != this._canUseActions)
				{
					this._canUseActions = value;
					base.OnPropertyChangedWithValue(value, "CanUseActions");
				}
			}
		}

		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x000697FD File Offset: 0x000679FD
		// (set) Token: 0x06001CF3 RID: 7411 RVA: 0x00069805 File Offset: 0x00067A05
		[DataSourceProperty]
		public override bool IsChangeLeaderVisible
		{
			get
			{
				return this._isChangeLeaderVisible;
			}
			set
			{
				if (value != this._isChangeLeaderVisible)
				{
					this._isChangeLeaderVisible = value;
					base.OnPropertyChangedWithValue(value, "IsChangeLeaderVisible");
				}
			}
		}

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x00069823 File Offset: 0x00067A23
		// (set) Token: 0x06001CF5 RID: 7413 RVA: 0x0006982B File Offset: 0x00067A2B
		[DataSourceProperty]
		public override bool IsChangeLeaderEnabled
		{
			get
			{
				return this._isChangeLeaderEnabled;
			}
			set
			{
				if (value != this._isChangeLeaderEnabled)
				{
					this._isChangeLeaderEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsChangeLeaderEnabled");
				}
			}
		}

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x00069849 File Offset: 0x00067A49
		// (set) Token: 0x06001CF7 RID: 7415 RVA: 0x00069851 File Offset: 0x00067A51
		[DataSourceProperty]
		public override HintViewModel ActionsDisabledHint
		{
			get
			{
				return this._actionsDisabledHint;
			}
			set
			{
				if (value != this._actionsDisabledHint)
				{
					this._actionsDisabledHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ActionsDisabledHint");
				}
			}
		}

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x0006986F File Offset: 0x00067A6F
		// (set) Token: 0x06001CF9 RID: 7417 RVA: 0x00069872 File Offset: 0x00067A72
		[DataSourceProperty]
		public override bool IsCaravan
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x06001CFA RID: 7418 RVA: 0x00069874 File Offset: 0x00067A74
		// (set) Token: 0x06001CFB RID: 7419 RVA: 0x00069877 File Offset: 0x00067A77
		[DataSourceProperty]
		public override bool ShouldPartyHaveExpense
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06001CFC RID: 7420 RVA: 0x00069879 File Offset: 0x00067A79
		// (set) Token: 0x06001CFD RID: 7421 RVA: 0x0006987C File Offset: 0x00067A7C
		[DataSourceProperty]
		public override bool HasCompanion
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x06001CFE RID: 7422 RVA: 0x0006987E File Offset: 0x00067A7E
		// (set) Token: 0x06001CFF RID: 7423 RVA: 0x00069881 File Offset: 0x00067A81
		[DataSourceProperty]
		public override bool IsAutoRecruitmentVisible
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x06001D00 RID: 7424 RVA: 0x00069883 File Offset: 0x00067A83
		// (set) Token: 0x06001D01 RID: 7425 RVA: 0x00069886 File Offset: 0x00067A86
		[DataSourceProperty]
		public override bool AutoRecruitmentValue
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x06001D02 RID: 7426 RVA: 0x00069888 File Offset: 0x00067A88
		// (set) Token: 0x06001D03 RID: 7427 RVA: 0x0006988B File Offset: 0x00067A8B
		[DataSourceProperty]
		public override bool IsMembersAndRolesVisible
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x06001D04 RID: 7428 RVA: 0x0006988D File Offset: 0x00067A8D
		// (set) Token: 0x06001D05 RID: 7429 RVA: 0x00069890 File Offset: 0x00067A90
		[DataSourceProperty]
		public override bool IsMainHeroParty
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x06001D06 RID: 7430 RVA: 0x00069892 File Offset: 0x00067A92
		// (set) Token: 0x06001D07 RID: 7431 RVA: 0x0006989A File Offset: 0x00067A9A
		[DataSourceProperty]
		public override ClanFinanceExpenseItemVM ExpenseItem
		{
			get
			{
				return this._expenseItem;
			}
			set
			{
				if (value != this._expenseItem)
				{
					this._expenseItem = value;
					base.OnPropertyChangedWithValue<ClanFinanceExpenseItemVM>(value, "ExpenseItem");
				}
			}
		}

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x000698B8 File Offset: 0x00067AB8
		// (set) Token: 0x06001D09 RID: 7433 RVA: 0x000698C0 File Offset: 0x00067AC0
		[DataSourceProperty]
		public override ClanPartyMemberItemVM LeaderMember
		{
			get
			{
				return this._leaderMember;
			}
			set
			{
				if (value != this._leaderMember)
				{
					this._leaderMember = value;
					base.OnPropertyChangedWithValue<ClanPartyMemberItemVM>(value, "LeaderMember");
				}
			}
		}

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x000698DE File Offset: 0x00067ADE
		// (set) Token: 0x06001D0B RID: 7435 RVA: 0x000698E6 File Offset: 0x00067AE6
		[DataSourceProperty]
		public override string PartySizeText
		{
			get
			{
				return this._partySizeText;
			}
			set
			{
				if (value != this._partySizeText)
				{
					this._partySizeText = value;
					base.OnPropertyChanged("PartyStrengthText");
				}
			}
		}

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x06001D0C RID: 7436 RVA: 0x00069908 File Offset: 0x00067B08
		// (set) Token: 0x06001D0D RID: 7437 RVA: 0x00069910 File Offset: 0x00067B10
		[DataSourceProperty]
		public override string ShipCountText
		{
			get
			{
				return this._shipCountText;
			}
			set
			{
				if (value != this._shipCountText)
				{
					this._shipCountText = value;
					base.OnPropertyChangedWithValue<string>(value, "ShipCountText");
				}
			}
		}

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x06001D0E RID: 7438 RVA: 0x00069933 File Offset: 0x00067B33
		// (set) Token: 0x06001D0F RID: 7439 RVA: 0x0006993B File Offset: 0x00067B3B
		[DataSourceProperty]
		public override string MembersText
		{
			get
			{
				return this._membersText;
			}
			set
			{
				if (value != null)
				{
					this._membersText = value;
					base.OnPropertyChangedWithValue<string>(value, "MembersText");
				}
			}
		}

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x06001D10 RID: 7440 RVA: 0x00069953 File Offset: 0x00067B53
		// (set) Token: 0x06001D11 RID: 7441 RVA: 0x0006995B File Offset: 0x00067B5B
		[DataSourceProperty]
		public override string AssigneesText
		{
			get
			{
				return this._assigneesText;
			}
			set
			{
				if (value != this._assigneesText)
				{
					this._assigneesText = value;
					base.OnPropertyChangedWithValue<string>(value, "AssigneesText");
				}
			}
		}

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06001D12 RID: 7442 RVA: 0x0006997E File Offset: 0x00067B7E
		// (set) Token: 0x06001D13 RID: 7443 RVA: 0x00069986 File Offset: 0x00067B86
		[DataSourceProperty]
		public override string RolesText
		{
			get
			{
				return this._rolesText;
			}
			set
			{
				if (value != this._rolesText)
				{
					this._rolesText = value;
					base.OnPropertyChangedWithValue<string>(value, "RolesText");
				}
			}
		}

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06001D14 RID: 7444 RVA: 0x000699A9 File Offset: 0x00067BA9
		// (set) Token: 0x06001D15 RID: 7445 RVA: 0x000699B1 File Offset: 0x00067BB1
		[DataSourceProperty]
		public override string PartyLeaderRoleEffectsText
		{
			get
			{
				return this._partyLeaderRoleEffectsText;
			}
			set
			{
				if (value != this._partyLeaderRoleEffectsText)
				{
					this._partyLeaderRoleEffectsText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartyLeaderRoleEffectsText");
				}
			}
		}

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06001D16 RID: 7446 RVA: 0x000699D4 File Offset: 0x00067BD4
		// (set) Token: 0x06001D17 RID: 7447 RVA: 0x000699DC File Offset: 0x00067BDC
		[DataSourceProperty]
		public override string PartyLocationText
		{
			get
			{
				return this._partyLocationText;
			}
			set
			{
				if (value != this._partyLocationText)
				{
					this._partyLocationText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartyLocationText");
				}
			}
		}

		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06001D18 RID: 7448 RVA: 0x000699FF File Offset: 0x00067BFF
		// (set) Token: 0x06001D19 RID: 7449 RVA: 0x00069A07 File Offset: 0x00067C07
		[DataSourceProperty]
		public override string Name
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

		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x06001D1A RID: 7450 RVA: 0x00069A2A File Offset: 0x00067C2A
		// (set) Token: 0x06001D1B RID: 7451 RVA: 0x00069A32 File Offset: 0x00067C32
		[DataSourceProperty]
		public override string PartySizeSubTitleText
		{
			get
			{
				return this._partySizeSubTitleText;
			}
			set
			{
				if (value != this._partySizeSubTitleText)
				{
					this._partySizeSubTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartySizeSubTitleText");
				}
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06001D1C RID: 7452 RVA: 0x00069A55 File Offset: 0x00067C55
		// (set) Token: 0x06001D1D RID: 7453 RVA: 0x00069A5D File Offset: 0x00067C5D
		[DataSourceProperty]
		public override string PartyWageSubTitleText
		{
			get
			{
				return this._partyWageSubTitleText;
			}
			set
			{
				if (value != this._partyWageSubTitleText)
				{
					this._partyWageSubTitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartyWageSubTitleText");
				}
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06001D1E RID: 7454 RVA: 0x00069A80 File Offset: 0x00067C80
		// (set) Token: 0x06001D1F RID: 7455 RVA: 0x00069A88 File Offset: 0x00067C88
		[DataSourceProperty]
		public override int InfantryCount
		{
			get
			{
				return this._infantryCount;
			}
			set
			{
				if (value != this._infantryCount)
				{
					this._infantryCount = value;
					base.OnPropertyChangedWithValue(value, "InfantryCount");
				}
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06001D20 RID: 7456 RVA: 0x00069AA6 File Offset: 0x00067CA6
		// (set) Token: 0x06001D21 RID: 7457 RVA: 0x00069AAE File Offset: 0x00067CAE
		[DataSourceProperty]
		public override int RangedCount
		{
			get
			{
				return this._rangedCount;
			}
			set
			{
				if (value != this._rangedCount)
				{
					this._rangedCount = value;
					base.OnPropertyChangedWithValue(value, "RangedCount");
				}
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x06001D22 RID: 7458 RVA: 0x00069ACC File Offset: 0x00067CCC
		// (set) Token: 0x06001D23 RID: 7459 RVA: 0x00069AD4 File Offset: 0x00067CD4
		[DataSourceProperty]
		public override int CavalryCount
		{
			get
			{
				return this._cavalryCount;
			}
			set
			{
				if (value != this._cavalryCount)
				{
					this._cavalryCount = value;
					base.OnPropertyChangedWithValue(value, "CavalryCount");
				}
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x06001D24 RID: 7460 RVA: 0x00069AF2 File Offset: 0x00067CF2
		// (set) Token: 0x06001D25 RID: 7461 RVA: 0x00069AFA File Offset: 0x00067CFA
		[DataSourceProperty]
		public override int HorseArcherCount
		{
			get
			{
				return this._horseArcherCount;
			}
			set
			{
				if (value != this._horseArcherCount)
				{
					this._horseArcherCount = value;
					base.OnPropertyChangedWithValue(value, "HorseArcherCount");
				}
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x06001D26 RID: 7462 RVA: 0x00069B18 File Offset: 0x00067D18
		// (set) Token: 0x06001D27 RID: 7463 RVA: 0x00069B20 File Offset: 0x00067D20
		[DataSourceProperty]
		public override int ShipCount
		{
			get
			{
				return this._shipCount;
			}
			set
			{
				if (value != this._shipCount)
				{
					this._shipCount = value;
					base.OnPropertyChangedWithValue(value, "ShipCount");
				}
			}
		}

		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06001D28 RID: 7464 RVA: 0x00069B3E File Offset: 0x00067D3E
		// (set) Token: 0x06001D29 RID: 7465 RVA: 0x00069B46 File Offset: 0x00067D46
		[DataSourceProperty]
		public override string InArmyText
		{
			get
			{
				return this._inArmyText;
			}
			set
			{
				if (value != this._inArmyText)
				{
					this._inArmyText = value;
					base.OnPropertyChangedWithValue<string>(value, "InArmyText");
				}
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06001D2A RID: 7466 RVA: 0x00069B69 File Offset: 0x00067D69
		// (set) Token: 0x06001D2B RID: 7467 RVA: 0x00069B71 File Offset: 0x00067D71
		[DataSourceProperty]
		public override string DisbandingText
		{
			get
			{
				return this._disbandingText;
			}
			set
			{
				if (value != this._disbandingText)
				{
					this._disbandingText = value;
					base.OnPropertyChangedWithValue<string>(value, "DisbandingText");
				}
			}
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06001D2C RID: 7468 RVA: 0x00069B94 File Offset: 0x00067D94
		// (set) Token: 0x06001D2D RID: 7469 RVA: 0x00069B9C File Offset: 0x00067D9C
		[DataSourceProperty]
		public override string AutoRecruitmentText
		{
			get
			{
				return this._autoRecruitmentText;
			}
			set
			{
				if (value != this._autoRecruitmentText)
				{
					this._autoRecruitmentText = value;
					base.OnPropertyChangedWithValue<string>(value, "AutoRecruitmentText");
				}
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x06001D2E RID: 7470 RVA: 0x00069BBF File Offset: 0x00067DBF
		// (set) Token: 0x06001D2F RID: 7471 RVA: 0x00069BC7 File Offset: 0x00067DC7
		[DataSourceProperty]
		public override HintViewModel AutoRecruitmentHint
		{
			get
			{
				return this._autoRecruitmentHint;
			}
			set
			{
				if (value != this._autoRecruitmentHint)
				{
					this._autoRecruitmentHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "AutoRecruitmentHint");
				}
			}
		}

		// Token: 0x170009FB RID: 2555
		// (get) Token: 0x06001D30 RID: 7472 RVA: 0x00069BE5 File Offset: 0x00067DE5
		// (set) Token: 0x06001D31 RID: 7473 RVA: 0x00069BED File Offset: 0x00067DED
		[DataSourceProperty]
		public override HintViewModel LeaderIsMovingToPartyHint
		{
			get
			{
				return this._leaderIsMovingToPartyHint;
			}
			set
			{
				if (value != this._leaderIsMovingToPartyHint)
				{
					this._leaderIsMovingToPartyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "LeaderIsMovingToPartyHint");
				}
			}
		}

		// Token: 0x170009FC RID: 2556
		// (get) Token: 0x06001D32 RID: 7474 RVA: 0x00069C0B File Offset: 0x00067E0B
		// (set) Token: 0x06001D33 RID: 7475 RVA: 0x00069C13 File Offset: 0x00067E13
		[DataSourceProperty]
		public override HintViewModel InArmyHint
		{
			get
			{
				return this._inArmyHint;
			}
			set
			{
				if (value != this._inArmyHint)
				{
					this._inArmyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "InArmyHint");
				}
			}
		}

		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x06001D34 RID: 7476 RVA: 0x00069C31 File Offset: 0x00067E31
		// (set) Token: 0x06001D35 RID: 7477 RVA: 0x00069C39 File Offset: 0x00067E39
		[DataSourceProperty]
		public override HintViewModel ChangeLeaderHint
		{
			get
			{
				return this._changeLeaderHint;
			}
			set
			{
				if (value != this._changeLeaderHint)
				{
					this._changeLeaderHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ChangeLeaderHint");
				}
			}
		}

		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x06001D36 RID: 7478 RVA: 0x00069C57 File Offset: 0x00067E57
		// (set) Token: 0x06001D37 RID: 7479 RVA: 0x00069C5F File Offset: 0x00067E5F
		[DataSourceProperty]
		public override BasicTooltipViewModel InfantryHint
		{
			get
			{
				return this._infantryHint;
			}
			set
			{
				if (value != this._infantryHint)
				{
					this._infantryHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "InfantryHint");
				}
			}
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x06001D38 RID: 7480 RVA: 0x00069C7D File Offset: 0x00067E7D
		// (set) Token: 0x06001D39 RID: 7481 RVA: 0x00069C85 File Offset: 0x00067E85
		[DataSourceProperty]
		public override BasicTooltipViewModel RangedHint
		{
			get
			{
				return this._rangedHint;
			}
			set
			{
				if (value != this._rangedHint)
				{
					this._rangedHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "RangedHint");
				}
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x06001D3A RID: 7482 RVA: 0x00069CA3 File Offset: 0x00067EA3
		// (set) Token: 0x06001D3B RID: 7483 RVA: 0x00069CAB File Offset: 0x00067EAB
		[DataSourceProperty]
		public override BasicTooltipViewModel CavalryHint
		{
			get
			{
				return this._cavalryHint;
			}
			set
			{
				if (value != this._cavalryHint)
				{
					this._cavalryHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "CavalryHint");
				}
			}
		}

		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x06001D3C RID: 7484 RVA: 0x00069CC9 File Offset: 0x00067EC9
		// (set) Token: 0x06001D3D RID: 7485 RVA: 0x00069CD1 File Offset: 0x00067ED1
		[DataSourceProperty]
		public override BasicTooltipViewModel HorseArcherHint
		{
			get
			{
				return this._horseArcherHint;
			}
			set
			{
				if (value != this._horseArcherHint)
				{
					this._horseArcherHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "HorseArcherHint");
				}
			}
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x06001D3E RID: 7486 RVA: 0x00069CEF File Offset: 0x00067EEF
		// (set) Token: 0x06001D3F RID: 7487 RVA: 0x00069CF7 File Offset: 0x00067EF7
		[DataSourceProperty]
		public override MBBindingList<ClanPartyMemberItemVM> HeroMembers
		{
			get
			{
				return this._heroMembers;
			}
			set
			{
				if (value != this._heroMembers)
				{
					this._heroMembers = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanPartyMemberItemVM>>(value, "HeroMembers");
				}
			}
		}

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x06001D40 RID: 7488 RVA: 0x00069D15 File Offset: 0x00067F15
		// (set) Token: 0x06001D41 RID: 7489 RVA: 0x00069D1D File Offset: 0x00067F1D
		[DataSourceProperty]
		public override MBBindingList<ClanRoleItemVM> Roles
		{
			get
			{
				return this._roles;
			}
			set
			{
				if (value != this._roles)
				{
					this._roles = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanRoleItemVM>>(value, "Roles");
				}
			}
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x06001D42 RID: 7490 RVA: 0x00069D3B File Offset: 0x00067F3B
		public override bool IsLeaderTeleporting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000D2B RID: 3371
		private readonly Action<ClanPartyItemVM> _onAssignment;

		// Token: 0x04000D2C RID: 3372
		private readonly Action _onShowChangeLeaderPopup;

		// Token: 0x04000D2D RID: 3373
		private readonly TextObject _changeLeaderHintText = GameTexts.FindText("str_change_party_leader", null);

		// Token: 0x04000D31 RID: 3377
		private ClanFinanceExpenseItemVM _expenseItem;

		// Token: 0x04000D32 RID: 3378
		private ClanPartyMemberItemVM _leaderMember;

		// Token: 0x04000D33 RID: 3379
		private CharacterImageIdentifierVM _leaderVisual;

		// Token: 0x04000D34 RID: 3380
		private bool _isSelected;

		// Token: 0x04000D35 RID: 3381
		private string _partyLocationText;

		// Token: 0x04000D36 RID: 3382
		private string _partySizeText;

		// Token: 0x04000D37 RID: 3383
		private string _shipCountText;

		// Token: 0x04000D38 RID: 3384
		private string _membersText;

		// Token: 0x04000D39 RID: 3385
		private string _assigneesText;

		// Token: 0x04000D3A RID: 3386
		private string _rolesText;

		// Token: 0x04000D3B RID: 3387
		private string _partyLeaderRoleEffectsText;

		// Token: 0x04000D3C RID: 3388
		private string _name;

		// Token: 0x04000D3D RID: 3389
		private string _partySizeSubTitleText;

		// Token: 0x04000D3E RID: 3390
		private string _partyWageSubTitleText;

		// Token: 0x04000D3F RID: 3391
		private int _infantryCount;

		// Token: 0x04000D40 RID: 3392
		private int _rangedCount;

		// Token: 0x04000D41 RID: 3393
		private int _cavalryCount;

		// Token: 0x04000D42 RID: 3394
		private int _horseArcherCount;

		// Token: 0x04000D43 RID: 3395
		private int _shipCount;

		// Token: 0x04000D44 RID: 3396
		private string _inArmyText;

		// Token: 0x04000D45 RID: 3397
		private string _disbandingText;

		// Token: 0x04000D46 RID: 3398
		private string _autoRecruitmentText;

		// Token: 0x04000D47 RID: 3399
		private bool _isPendingPartyCreation;

		// Token: 0x04000D48 RID: 3400
		private bool _isDisbanding;

		// Token: 0x04000D49 RID: 3401
		private bool _isInArmy;

		// Token: 0x04000D4A RID: 3402
		private bool _canUseActions;

		// Token: 0x04000D4B RID: 3403
		private bool _isChangeLeaderVisible;

		// Token: 0x04000D4C RID: 3404
		private bool _isChangeLeaderEnabled;

		// Token: 0x04000D4D RID: 3405
		private bool _isClanRoleSelectionHighlightEnabled;

		// Token: 0x04000D4E RID: 3406
		private bool _isRoleSelectionPopupVisible;

		// Token: 0x04000D4F RID: 3407
		private HintViewModel _leaderIsMovingToPartyHint;

		// Token: 0x04000D50 RID: 3408
		private HintViewModel _actionsDisabledHint;

		// Token: 0x04000D51 RID: 3409
		private CharacterViewModel _characterModel;

		// Token: 0x04000D52 RID: 3410
		private HintViewModel _autoRecruitmentHint;

		// Token: 0x04000D53 RID: 3411
		private HintViewModel _inArmyHint;

		// Token: 0x04000D54 RID: 3412
		private HintViewModel _changeLeaderHint;

		// Token: 0x04000D55 RID: 3413
		private BasicTooltipViewModel _infantryHint;

		// Token: 0x04000D56 RID: 3414
		private BasicTooltipViewModel _rangedHint;

		// Token: 0x04000D57 RID: 3415
		private BasicTooltipViewModel _cavalryHint;

		// Token: 0x04000D58 RID: 3416
		private BasicTooltipViewModel _horseArcherHint;

		// Token: 0x04000D59 RID: 3417
		private MBBindingList<ClanPartyMemberItemVM> _heroMembers;

		// Token: 0x04000D5A RID: 3418
		private MBBindingList<ClanRoleItemVM> _roles;
	}
}
