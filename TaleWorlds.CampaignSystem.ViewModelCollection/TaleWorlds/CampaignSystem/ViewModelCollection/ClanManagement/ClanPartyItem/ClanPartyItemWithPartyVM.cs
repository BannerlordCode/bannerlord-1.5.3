using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanPartyItem
{
	// Token: 0x02000136 RID: 310
	public class ClanPartyItemWithPartyVM : ClanPartyItemVM
	{
		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x06001D43 RID: 7491 RVA: 0x00069D3E File Offset: 0x00067F3E
		// (set) Token: 0x06001D44 RID: 7492 RVA: 0x00069D46 File Offset: 0x00067F46
		public override int Expense { get; protected set; }

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x06001D45 RID: 7493 RVA: 0x00069D4F File Offset: 0x00067F4F
		// (set) Token: 0x06001D46 RID: 7494 RVA: 0x00069D57 File Offset: 0x00067F57
		public override int Income { get; protected set; }

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x06001D47 RID: 7495 RVA: 0x00069D60 File Offset: 0x00067F60
		public override Hero Leader
		{
			get
			{
				CharacterObject leader = this._leader;
				if (leader == null)
				{
					return null;
				}
				return leader.HeroObject;
			}
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x06001D48 RID: 7496 RVA: 0x00069D73 File Offset: 0x00067F73
		public override CampaignVec2 Position
		{
			get
			{
				PartyBase party = base.Party;
				if (party == null)
				{
					return CampaignVec2.Invalid;
				}
				return party.Position;
			}
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x06001D49 RID: 7497 RVA: 0x00069D8A File Offset: 0x00067F8A
		public override bool IsLeaderTeleporting
		{
			get
			{
				return this._isLeaderTeleporting;
			}
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x00069D94 File Offset: 0x00067F94
		public ClanPartyItemWithPartyVM(PartyBase party, Action<ClanPartyItemVM> onAssignment, Action onExpenseChange, Action onShowChangeLeaderPopup, Action<ClanRoleItemVM> onShowChangeRolePopup, ClanPartyItemVM.ClanPartyType type, IDisbandPartyCampaignBehavior disbandBehavior, ITeleportationCampaignBehavior teleportationBehavior)
		{
			base.Party = party;
			this._type = type;
			this._disbandBehavior = disbandBehavior;
			this._leader = CampaignUIHelper.GetVisualPartyLeader(base.Party);
			this.HasHeroMembers = party.IsMobile;
			this.IsPendingPartyCreation = false;
			if (this._leader == null)
			{
				TroopRosterElement troopRosterElement = base.Party.MemberRoster.GetTroopRoster().FirstOrDefault<TroopRosterElement>();
				if (!troopRosterElement.Equals(default(TroopRosterElement)))
				{
					this._leader = troopRosterElement.Character;
				}
				else
				{
					IFaction mapFaction = base.Party.MapFaction;
					this._leader = ((mapFaction != null) ? mapFaction.BasicTroop : null);
				}
			}
			CharacterObject leader = this._leader;
			if ((leader == null || !leader.IsHero) && party.IsMobile && (this._type == ClanPartyItemVM.ClanPartyType.Member || this._type == ClanPartyItemVM.ClanPartyType.Caravan))
			{
				Hero teleportingLeaderHero = CampaignUIHelper.GetTeleportingLeaderHero(party.MobileParty, teleportationBehavior);
				this._leader = ((teleportingLeaderHero != null) ? teleportingLeaderHero.CharacterObject : null);
				this._isLeaderTeleporting = this._leader != null;
			}
			if (this._leader != null)
			{
				CharacterCode characterCode = ClanPartyItemVM.GetCharacterCode(this._leader);
				this.LeaderVisual = new CharacterImageIdentifierVM(characterCode);
				this.CharacterModel = new CharacterViewModel(CharacterViewModel.StanceTypes.None);
				CharacterViewModel characterModel = this.CharacterModel;
				BasicCharacterObject leader2 = this._leader;
				int num = -1;
				Banner banner = base.Party.Banner;
				characterModel.FillFrom(leader2, num, (banner != null) ? banner.BannerCode : null);
				CharacterViewModel characterModel2 = this.CharacterModel;
				IFaction mapFaction2 = base.Party.MapFaction;
				characterModel2.ArmorColor1 = ((mapFaction2 != null) ? mapFaction2.Color : 0U);
				CharacterViewModel characterModel3 = this.CharacterModel;
				IFaction mapFaction3 = base.Party.MapFaction;
				characterModel3.ArmorColor2 = ((mapFaction3 != null) ? mapFaction3.Color2 : 0U);
			}
			else
			{
				this.LeaderVisual = new CharacterImageIdentifierVM(null);
				this.CharacterModel = new CharacterViewModel();
			}
			this._onAssignment = onAssignment;
			this._onExpenseChange = onExpenseChange;
			this._onShowChangeLeaderPopup = onShowChangeLeaderPopup;
			this._onShowChangeRolePopup = onShowChangeRolePopup;
			bool flag;
			if (!base.Party.MobileParty.IsDisbanding)
			{
				IDisbandPartyCampaignBehavior disbandBehavior2 = this._disbandBehavior;
				flag = disbandBehavior2 != null && disbandBehavior2.IsPartyWaitingForDisband(party.MobileParty);
			}
			else
			{
				flag = true;
			}
			this.IsDisbanding = flag;
			bool flag2 = !party.MobileParty.IsMilitia && !party.MobileParty.IsVillager && party.MobileParty.IsActive && !this.IsDisbanding;
			this.ShouldPartyHaveExpense = flag2 && (type == ClanPartyItemVM.ClanPartyType.Garrison || type == ClanPartyItemVM.ClanPartyType.Member);
			this.IsCaravan = type == ClanPartyItemVM.ClanPartyType.Caravan;
			base.AreCommandControlsVisible = type == ClanPartyItemVM.ClanPartyType.Member;
			TextObject empty = TextObject.GetEmpty();
			this.IsChangeLeaderVisible = type == ClanPartyItemVM.ClanPartyType.Caravan || type == ClanPartyItemVM.ClanPartyType.Member;
			this.IsChangeLeaderEnabled = this.IsChangeLeaderVisible && CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out empty);
			this.ChangeLeaderHint = new HintViewModel(this.IsChangeLeaderEnabled ? this._changeLeaderHintText : empty, null);
			if (this.ShouldPartyHaveExpense)
			{
				if (party.MobileParty != null)
				{
					this.ExpenseItem = new ClanFinanceExpenseItemVM(party.MobileParty);
					this.OnExpenseChange();
				}
				else
				{
					Debug.FailedAssert("This party should have expense info but it doesn't", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\ClanManagement\\ClanPartyItem\\ClanPartyItemWithPartyVM.cs", ".ctor", 115);
				}
			}
			if (this.IsCaravan)
			{
				this.Income = Campaign.Current.Models.ClanFinanceModel.CalculateOwnerIncomeFromCaravan(party.MobileParty);
			}
			this.AutoRecruitmentHint = new HintViewModel(GameTexts.FindText("str_clan_auto_recruitment_hint", null), null);
			this.IsAutoRecruitmentVisible = party.MobileParty.IsGarrison;
			this.AutoRecruitmentValue = party.MobileParty.IsGarrison && base.Party.MobileParty.CurrentSettlement.Town.GarrisonAutoRecruitmentIsEnabled;
			if (this.Leader != null)
			{
				base.AllowRaiding = this.Leader.CanRaid;
				base.DonateTroopsToGarrisons = this.Leader.CanDonateTroopsToGarrison;
				base.MayJoinOtherArmies = this.Leader.CanJoinArmy;
				base.HasFleet = this.Leader.CanHaveFleet;
			}
			this.HeroMembers = new MBBindingList<ClanPartyMemberItemVM>();
			this.Roles = new MBBindingList<ClanRoleItemVM>();
			base.SmallShipHint = new HintViewModel(new TextObject("{=SeXdiWJL}Small Ships", null), null);
			base.MediumShipHint = new HintViewModel(new TextObject("{=XcIDr42e}Medium Ships", null), null);
			base.LargeShipHint = new HintViewModel(new TextObject("{=ReqtAxsC}Large Ships", null), null);
			this.InfantryHint = new BasicTooltipViewModel(() => this.GetPartyTroopInfo(base.Party, FormationClass.Infantry));
			this.CavalryHint = new BasicTooltipViewModel(() => this.GetPartyTroopInfo(base.Party, FormationClass.Cavalry));
			this.RangedHint = new BasicTooltipViewModel(() => this.GetPartyTroopInfo(base.Party, FormationClass.Ranged));
			this.HorseArcherHint = new BasicTooltipViewModel(() => this.GetPartyTroopInfo(base.Party, FormationClass.HorseArcher));
			this.ActionsDisabledHint = new HintViewModel();
			this.InArmyHint = new HintViewModel();
			this.RefreshValues();
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x0006A244 File Offset: 0x00068444
		public override void UpdateProperties()
		{
			this.MembersText = GameTexts.FindText("str_members", null).ToString();
			this.AssigneesText = GameTexts.FindText("str_clan_assignee_title", null).ToString();
			this.RolesText = GameTexts.FindText("str_clan_role_title", null).ToString();
			this.PartyLeaderRoleEffectsText = GameTexts.FindText("str_clan_party_leader_roles_and_effects", null).ToString();
			this.AutoRecruitmentText = GameTexts.FindText("str_clan_auto_recruitment", null).ToString();
			if (base.Party == PartyBase.MainParty && Hero.MainHero.IsPrisoner)
			{
				TextObject textObject = new TextObject("{=shL0WElC}{TROOP.NAME}{.o} Party", null);
				textObject.SetCharacterProperties("TROOP", Hero.MainHero.CharacterObject, false);
				this.Name = textObject.ToString();
			}
			else if (this._isLeaderTeleporting)
			{
				TextObject textObject2 = new TextObject("{=P5YtNXHR}{LEADER.NAME}{.o} Party", null);
				StringHelpers.SetCharacterProperties("LEADER", this._leader, textObject2, false);
				this.Name = textObject2.ToString();
			}
			else
			{
				this.Name = base.Party.Name.ToString();
			}
			this.IsMainHeroParty = this._type == ClanPartyItemVM.ClanPartyType.Main;
			this.PartyLocationText = CampaignUIHelper.GetPartyLocationText(base.Party.MobileParty);
			GameTexts.SetVariable("LEFT", base.Party.MobileParty.MemberRoster.TotalManCount);
			PartyBase party = base.Party;
			if (((party != null) ? party.LeaderHero : null) != null)
			{
				GameTexts.SetVariable("RIGHT", base.Party.PartySizeLimit);
			}
			else if (this.Leader != null)
			{
				this.LeaderIsMovingToPartyHint = new HintViewModel(new TextObject("{=g08mptth}Moving to a party to be the new leader", null), null);
				GameTexts.SetVariable("RIGHT", CampaignUIHelper.GetPartySizeLimitForLeader(this.Leader));
			}
			else
			{
				GameTexts.SetVariable("RIGHT", base.Party.MobileParty.MemberRoster.TotalManCount);
			}
			string text = GameTexts.FindText("str_LEFT_over_RIGHT", null).ToString();
			string text2 = GameTexts.FindText("str_party_morale_party_size", null).ToString();
			this.PartySizeText = text;
			GameTexts.SetVariable("LEFT", text2);
			GameTexts.SetVariable("RIGHT", text);
			this.PartySizeSubTitleText = GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString();
			GameTexts.SetVariable("LEFT", GameTexts.FindText("str_party_wage", null));
			GameTexts.SetVariable("RIGHT", base.Party.MobileParty.TotalWage);
			this.PartyWageSubTitleText = GameTexts.FindText("str_LEFT_colon_RIGHT", null).ToString();
			this.InArmyText = "";
			if (base.Party.MobileParty.Army != null)
			{
				this.IsInArmy = true;
				TextObject textObject3 = GameTexts.FindText("str_clan_in_army_hint", null);
				TextObject textObject4 = textObject3;
				string text3 = "ARMY_LEADER";
				MobileParty leaderParty = base.Party.MobileParty.Army.LeaderParty;
				string text4;
				if (leaderParty == null)
				{
					text4 = null;
				}
				else
				{
					Hero leaderHero = leaderParty.LeaderHero;
					text4 = ((leaderHero != null) ? leaderHero.Name.ToString() : null);
				}
				textObject4.SetTextVariable(text3, text4 ?? string.Empty);
				this.InArmyHint = new HintViewModel(textObject3, null);
				this.InArmyText = GameTexts.FindText("str_in_army", null).ToString();
			}
			this.DisbandingText = "";
			this.IsMembersAndRolesVisible = !this.IsDisbanding && this._type != ClanPartyItemVM.ClanPartyType.Garrison;
			if (this.IsDisbanding)
			{
				this.DisbandingText = GameTexts.FindText("str_disbanding", null).ToString();
			}
			if (this._leader != null)
			{
				CharacterViewModel characterModel = this.CharacterModel;
				BasicCharacterObject leader = this._leader;
				int num = -1;
				Banner banner = base.Party.Banner;
				characterModel.FillFrom(leader, num, (banner != null) ? banner.BannerCode : null);
				CharacterViewModel characterModel2 = this.CharacterModel;
				IFaction mapFaction = base.Party.MapFaction;
				characterModel2.ArmorColor1 = ((mapFaction != null) ? mapFaction.Color : 0U);
				CharacterViewModel characterModel3 = this.CharacterModel;
				IFaction mapFaction2 = base.Party.MapFaction;
				characterModel3.ArmorColor2 = ((mapFaction2 != null) ? mapFaction2.Color2 : 0U);
			}
			this.HeroMembers.Clear();
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			foreach (TroopRosterElement troopRosterElement in base.Party.MemberRoster.GetTroopRoster())
			{
				Hero heroObject = troopRosterElement.Character.HeroObject;
				if (heroObject != null && heroObject.Clan == Clan.PlayerClan && heroObject.GovernorOf == null)
				{
					ClanPartyMemberItemVM clanPartyMemberItemVM = new ClanPartyMemberItemVM(troopRosterElement.Character.HeroObject, base.Party.MobileParty);
					this.HeroMembers.Add(clanPartyMemberItemVM);
					if (clanPartyMemberItemVM.IsLeader)
					{
						this.LeaderMember = clanPartyMemberItemVM;
					}
				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.Infantry))
				{
					num2 += troopRosterElement.Number;
				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.Ranged))
				{
					num3 += troopRosterElement.Number;
				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.Cavalry))
				{
					num4 += troopRosterElement.Number;
				}
				else if (troopRosterElement.Character.DefaultFormationClass.Equals(FormationClass.HorseArcher))
				{
					num5 += troopRosterElement.Number;
				}
			}
			if (this._isLeaderTeleporting)
			{
				ClanPartyMemberItemVM clanPartyMemberItemVM2 = new ClanPartyMemberItemVM(this._leader.HeroObject, base.Party.MobileParty);
				this.LeaderMember = clanPartyMemberItemVM2;
				this.HeroMembers.Insert(0, clanPartyMemberItemVM2);
			}
			this.HasCompanion = this.HeroMembers.Count > 1;
			if (this.IsMembersAndRolesVisible)
			{
				this.Roles.ApplyActionOnAllItems(delegate(ClanRoleItemVM x)
				{
					x.OnFinalize();
				});
				this.Roles.Clear();
				foreach (PartyRole partyRole in Campaign.Current.Models.ClanMemberPartyRoleModel.GetAssignablePartyRoles())
				{
					this.Roles.Add(new ClanRoleItemVM(base.Party.MobileParty, partyRole, this.HeroMembers, new Action<ClanRoleItemVM>(this.OnRoleSelectionToggled)));
				}
			}
			this.InfantryCount = num2;
			this.RangedCount = num3;
			this.CavalryCount = num4;
			this.HorseArcherCount = num5;
			TextObject textObject5;
			bool mapScreenActionIsEnabledWithReason = CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject5);
			this.CanUseActions = mapScreenActionIsEnabledWithReason && !this.IsDisbanding;
			if (!mapScreenActionIsEnabledWithReason)
			{
				this.AutoRecruitmentHint.HintText = this.ActionsDisabledHint.HintText;
				if (this.ExpenseItem != null)
				{
					this.ExpenseItem.IsEnabled = this.CanUseActions;
					this.ExpenseItem.WageLimitHint.HintText = this.ActionsDisabledHint.HintText;
				}
				foreach (ClanRoleItemVM clanRoleItemVM in this.Roles)
				{
					clanRoleItemVM.SetEnabled(false, this.ActionsDisabledHint.HintText);
				}
				this.ActionsDisabledHint.HintText = textObject5;
			}
			else if (this.IsDisbanding)
			{
				this.ActionsDisabledHint.HintText = new TextObject("{=BHFxYCpv}You cannot perform this action while the party is disbanding", null);
			}
			else
			{
				this.ActionsDisabledHint.HintText = TextObject.GetEmpty();
			}
			this.ShipCount = base.Party.Ships.Count;
			this.ShipCountText = GameTexts.FindText("str_LEFT_colon_RIGHT", null).SetTextVariable("LEFT", new TextObject("{=7Q8ufo5X}Ships", null).ToString()).SetTextVariable("RIGHT", this.ShipCount)
				.ToString();
			this.RefreshFleetComposition();
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x0006AA44 File Offset: 0x00068C44
		private void RefreshFleetComposition()
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			foreach (Ship ship in base.Party.Ships)
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

		// Token: 0x06001D4D RID: 7501 RVA: 0x0006AB08 File Offset: 0x00068D08
		private void OnExpenseChange()
		{
			this._onExpenseChange();
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x0006AB15 File Offset: 0x00068D15
		public override void OnPartySelection()
		{
			this._onAssignment(this);
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x0006AB23 File Offset: 0x00068D23
		public override void ExecuteChangeLeader()
		{
			Action onShowChangeLeaderPopup = this._onShowChangeLeaderPopup;
			if (onShowChangeLeaderPopup == null)
			{
				return;
			}
			onShowChangeLeaderPopup();
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x0006AB35 File Offset: 0x00068D35
		private void ExecuteLocationLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x0006AB48 File Offset: 0x00068D48
		private void OnAutoRecruitChanged(bool value)
		{
			if (base.Party.IsMobile && base.Party.MobileParty.IsGarrison)
			{
				Settlement homeSettlement = base.Party.MobileParty.HomeSettlement;
				if (((homeSettlement != null) ? homeSettlement.Town : null) != null)
				{
					base.Party.MobileParty.HomeSettlement.Town.GarrisonAutoRecruitmentIsEnabled = value;
				}
			}
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x0006ABAD File Offset: 0x00068DAD
		private void OnRoleSelectionToggled(ClanRoleItemVM role)
		{
			Action<ClanRoleItemVM> onShowChangeRolePopup = this._onShowChangeRolePopup;
			if (onShowChangeRolePopup == null)
			{
				return;
			}
			onShowChangeRolePopup(role);
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x06001D53 RID: 7507 RVA: 0x0006ABC0 File Offset: 0x00068DC0
		// (set) Token: 0x06001D54 RID: 7508 RVA: 0x0006ABC8 File Offset: 0x00068DC8
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

		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x06001D55 RID: 7509 RVA: 0x0006ABE6 File Offset: 0x00068DE6
		// (set) Token: 0x06001D56 RID: 7510 RVA: 0x0006ABEE File Offset: 0x00068DEE
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

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x06001D57 RID: 7511 RVA: 0x0006AC0C File Offset: 0x00068E0C
		// (set) Token: 0x06001D58 RID: 7512 RVA: 0x0006AC14 File Offset: 0x00068E14
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

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x06001D59 RID: 7513 RVA: 0x0006AC32 File Offset: 0x00068E32
		// (set) Token: 0x06001D5A RID: 7514 RVA: 0x0006AC3A File Offset: 0x00068E3A
		[DataSourceProperty]
		public override bool HasHeroMembers
		{
			get
			{
				return this._hasHeroMembers;
			}
			set
			{
				if (value != this._hasHeroMembers)
				{
					this._hasHeroMembers = value;
					base.OnPropertyChangedWithValue(value, "HasHeroMembers");
				}
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06001D5B RID: 7515 RVA: 0x0006AC58 File Offset: 0x00068E58
		// (set) Token: 0x06001D5C RID: 7516 RVA: 0x0006AC60 File Offset: 0x00068E60
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

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06001D5D RID: 7517 RVA: 0x0006AC7E File Offset: 0x00068E7E
		// (set) Token: 0x06001D5E RID: 7518 RVA: 0x0006AC86 File Offset: 0x00068E86
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

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x06001D5F RID: 7519 RVA: 0x0006ACA4 File Offset: 0x00068EA4
		// (set) Token: 0x06001D60 RID: 7520 RVA: 0x0006ACAC File Offset: 0x00068EAC
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

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x06001D61 RID: 7521 RVA: 0x0006ACCA File Offset: 0x00068ECA
		// (set) Token: 0x06001D62 RID: 7522 RVA: 0x0006ACD2 File Offset: 0x00068ED2
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

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x06001D63 RID: 7523 RVA: 0x0006ACF0 File Offset: 0x00068EF0
		// (set) Token: 0x06001D64 RID: 7524 RVA: 0x0006ACF8 File Offset: 0x00068EF8
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

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x06001D65 RID: 7525 RVA: 0x0006AD16 File Offset: 0x00068F16
		// (set) Token: 0x06001D66 RID: 7526 RVA: 0x0006AD1E File Offset: 0x00068F1E
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

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x06001D67 RID: 7527 RVA: 0x0006AD3C File Offset: 0x00068F3C
		// (set) Token: 0x06001D68 RID: 7528 RVA: 0x0006AD44 File Offset: 0x00068F44
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

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x06001D69 RID: 7529 RVA: 0x0006AD62 File Offset: 0x00068F62
		// (set) Token: 0x06001D6A RID: 7530 RVA: 0x0006AD6A File Offset: 0x00068F6A
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

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x06001D6B RID: 7531 RVA: 0x0006AD88 File Offset: 0x00068F88
		// (set) Token: 0x06001D6C RID: 7532 RVA: 0x0006AD90 File Offset: 0x00068F90
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

		// Token: 0x17000A17 RID: 2583
		// (get) Token: 0x06001D6D RID: 7533 RVA: 0x0006ADAE File Offset: 0x00068FAE
		// (set) Token: 0x06001D6E RID: 7534 RVA: 0x0006ADB6 File Offset: 0x00068FB6
		[DataSourceProperty]
		public override bool IsCaravan
		{
			get
			{
				return this._isCaravan;
			}
			set
			{
				if (value != this._isCaravan)
				{
					this._isCaravan = value;
					base.OnPropertyChangedWithValue(value, "IsCaravan");
				}
			}
		}

		// Token: 0x17000A18 RID: 2584
		// (get) Token: 0x06001D6F RID: 7535 RVA: 0x0006ADD4 File Offset: 0x00068FD4
		// (set) Token: 0x06001D70 RID: 7536 RVA: 0x0006ADDC File Offset: 0x00068FDC
		[DataSourceProperty]
		public override bool ShouldPartyHaveExpense
		{
			get
			{
				return this._shouldPartyHaveExpense;
			}
			set
			{
				if (value != this._shouldPartyHaveExpense)
				{
					this._shouldPartyHaveExpense = value;
					base.OnPropertyChangedWithValue(value, "ShouldPartyHaveExpense");
				}
			}
		}

		// Token: 0x17000A19 RID: 2585
		// (get) Token: 0x06001D71 RID: 7537 RVA: 0x0006ADFA File Offset: 0x00068FFA
		// (set) Token: 0x06001D72 RID: 7538 RVA: 0x0006AE02 File Offset: 0x00069002
		[DataSourceProperty]
		public override bool HasCompanion
		{
			get
			{
				return this._hasCompanion;
			}
			set
			{
				if (value != this._hasCompanion)
				{
					this._hasCompanion = value;
					base.OnPropertyChangedWithValue(value, "HasCompanion");
				}
			}
		}

		// Token: 0x17000A1A RID: 2586
		// (get) Token: 0x06001D73 RID: 7539 RVA: 0x0006AE20 File Offset: 0x00069020
		// (set) Token: 0x06001D74 RID: 7540 RVA: 0x0006AE28 File Offset: 0x00069028
		[DataSourceProperty]
		public override bool IsAutoRecruitmentVisible
		{
			get
			{
				return this._isAutoRecruitmentVisible;
			}
			set
			{
				if (value != this._isAutoRecruitmentVisible)
				{
					this._isAutoRecruitmentVisible = value;
					base.OnPropertyChangedWithValue(value, "IsAutoRecruitmentVisible");
				}
			}
		}

		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x06001D75 RID: 7541 RVA: 0x0006AE46 File Offset: 0x00069046
		// (set) Token: 0x06001D76 RID: 7542 RVA: 0x0006AE4E File Offset: 0x0006904E
		[DataSourceProperty]
		public override bool AutoRecruitmentValue
		{
			get
			{
				return this._autoRecruitmentValue;
			}
			set
			{
				if (value != this._autoRecruitmentValue)
				{
					this._autoRecruitmentValue = value;
					base.OnPropertyChangedWithValue(value, "AutoRecruitmentValue");
					this.OnAutoRecruitChanged(value);
				}
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06001D77 RID: 7543 RVA: 0x0006AE73 File Offset: 0x00069073
		// (set) Token: 0x06001D78 RID: 7544 RVA: 0x0006AE7B File Offset: 0x0006907B
		[DataSourceProperty]
		public override bool IsMembersAndRolesVisible
		{
			get
			{
				return this._isMembersAndRolesVisible;
			}
			set
			{
				if (value != this._isMembersAndRolesVisible)
				{
					this._isMembersAndRolesVisible = value;
					base.OnPropertyChangedWithValue(value, "IsMembersAndRolesVisible");
				}
			}
		}

		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06001D79 RID: 7545 RVA: 0x0006AE99 File Offset: 0x00069099
		// (set) Token: 0x06001D7A RID: 7546 RVA: 0x0006AEA1 File Offset: 0x000690A1
		[DataSourceProperty]
		public override bool IsMainHeroParty
		{
			get
			{
				return this._isMainHeroParty;
			}
			set
			{
				if (value != this._isMainHeroParty)
				{
					this._isMainHeroParty = value;
					base.OnPropertyChangedWithValue(value, "IsMainHeroParty");
				}
			}
		}

		// Token: 0x17000A1E RID: 2590
		// (get) Token: 0x06001D7B RID: 7547 RVA: 0x0006AEBF File Offset: 0x000690BF
		// (set) Token: 0x06001D7C RID: 7548 RVA: 0x0006AEC7 File Offset: 0x000690C7
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

		// Token: 0x17000A1F RID: 2591
		// (get) Token: 0x06001D7D RID: 7549 RVA: 0x0006AEE5 File Offset: 0x000690E5
		// (set) Token: 0x06001D7E RID: 7550 RVA: 0x0006AEED File Offset: 0x000690ED
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

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06001D7F RID: 7551 RVA: 0x0006AF0B File Offset: 0x0006910B
		// (set) Token: 0x06001D80 RID: 7552 RVA: 0x0006AF13 File Offset: 0x00069113
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

		// Token: 0x17000A21 RID: 2593
		// (get) Token: 0x06001D81 RID: 7553 RVA: 0x0006AF35 File Offset: 0x00069135
		// (set) Token: 0x06001D82 RID: 7554 RVA: 0x0006AF3D File Offset: 0x0006913D
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

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x06001D83 RID: 7555 RVA: 0x0006AF60 File Offset: 0x00069160
		// (set) Token: 0x06001D84 RID: 7556 RVA: 0x0006AF68 File Offset: 0x00069168
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

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06001D85 RID: 7557 RVA: 0x0006AF80 File Offset: 0x00069180
		// (set) Token: 0x06001D86 RID: 7558 RVA: 0x0006AF88 File Offset: 0x00069188
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

		// Token: 0x17000A24 RID: 2596
		// (get) Token: 0x06001D87 RID: 7559 RVA: 0x0006AFAB File Offset: 0x000691AB
		// (set) Token: 0x06001D88 RID: 7560 RVA: 0x0006AFB3 File Offset: 0x000691B3
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

		// Token: 0x17000A25 RID: 2597
		// (get) Token: 0x06001D89 RID: 7561 RVA: 0x0006AFD6 File Offset: 0x000691D6
		// (set) Token: 0x06001D8A RID: 7562 RVA: 0x0006AFDE File Offset: 0x000691DE
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

		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06001D8B RID: 7563 RVA: 0x0006B001 File Offset: 0x00069201
		// (set) Token: 0x06001D8C RID: 7564 RVA: 0x0006B009 File Offset: 0x00069209
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

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06001D8D RID: 7565 RVA: 0x0006B02C File Offset: 0x0006922C
		// (set) Token: 0x06001D8E RID: 7566 RVA: 0x0006B034 File Offset: 0x00069234
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

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06001D8F RID: 7567 RVA: 0x0006B057 File Offset: 0x00069257
		// (set) Token: 0x06001D90 RID: 7568 RVA: 0x0006B05F File Offset: 0x0006925F
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

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06001D91 RID: 7569 RVA: 0x0006B082 File Offset: 0x00069282
		// (set) Token: 0x06001D92 RID: 7570 RVA: 0x0006B08A File Offset: 0x0006928A
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

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06001D93 RID: 7571 RVA: 0x0006B0AD File Offset: 0x000692AD
		// (set) Token: 0x06001D94 RID: 7572 RVA: 0x0006B0B5 File Offset: 0x000692B5
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

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06001D95 RID: 7573 RVA: 0x0006B0D3 File Offset: 0x000692D3
		// (set) Token: 0x06001D96 RID: 7574 RVA: 0x0006B0DB File Offset: 0x000692DB
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

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x06001D97 RID: 7575 RVA: 0x0006B0F9 File Offset: 0x000692F9
		// (set) Token: 0x06001D98 RID: 7576 RVA: 0x0006B101 File Offset: 0x00069301
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

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06001D99 RID: 7577 RVA: 0x0006B11F File Offset: 0x0006931F
		// (set) Token: 0x06001D9A RID: 7578 RVA: 0x0006B127 File Offset: 0x00069327
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

		// Token: 0x17000A2E RID: 2606
		// (get) Token: 0x06001D9B RID: 7579 RVA: 0x0006B145 File Offset: 0x00069345
		// (set) Token: 0x06001D9C RID: 7580 RVA: 0x0006B14D File Offset: 0x0006934D
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

		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x06001D9D RID: 7581 RVA: 0x0006B16B File Offset: 0x0006936B
		// (set) Token: 0x06001D9E RID: 7582 RVA: 0x0006B173 File Offset: 0x00069373
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

		// Token: 0x17000A30 RID: 2608
		// (get) Token: 0x06001D9F RID: 7583 RVA: 0x0006B196 File Offset: 0x00069396
		// (set) Token: 0x06001DA0 RID: 7584 RVA: 0x0006B19E File Offset: 0x0006939E
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

		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x06001DA1 RID: 7585 RVA: 0x0006B1C1 File Offset: 0x000693C1
		// (set) Token: 0x06001DA2 RID: 7586 RVA: 0x0006B1C9 File Offset: 0x000693C9
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

		// Token: 0x17000A32 RID: 2610
		// (get) Token: 0x06001DA3 RID: 7587 RVA: 0x0006B1EC File Offset: 0x000693EC
		// (set) Token: 0x06001DA4 RID: 7588 RVA: 0x0006B1F4 File Offset: 0x000693F4
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

		// Token: 0x17000A33 RID: 2611
		// (get) Token: 0x06001DA5 RID: 7589 RVA: 0x0006B212 File Offset: 0x00069412
		// (set) Token: 0x06001DA6 RID: 7590 RVA: 0x0006B21A File Offset: 0x0006941A
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

		// Token: 0x17000A34 RID: 2612
		// (get) Token: 0x06001DA7 RID: 7591 RVA: 0x0006B238 File Offset: 0x00069438
		// (set) Token: 0x06001DA8 RID: 7592 RVA: 0x0006B240 File Offset: 0x00069440
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

		// Token: 0x17000A35 RID: 2613
		// (get) Token: 0x06001DA9 RID: 7593 RVA: 0x0006B25E File Offset: 0x0006945E
		// (set) Token: 0x06001DAA RID: 7594 RVA: 0x0006B266 File Offset: 0x00069466
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

		// Token: 0x17000A36 RID: 2614
		// (get) Token: 0x06001DAB RID: 7595 RVA: 0x0006B284 File Offset: 0x00069484
		// (set) Token: 0x06001DAC RID: 7596 RVA: 0x0006B28C File Offset: 0x0006948C
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

		// Token: 0x17000A37 RID: 2615
		// (get) Token: 0x06001DAD RID: 7597 RVA: 0x0006B2AA File Offset: 0x000694AA
		// (set) Token: 0x06001DAE RID: 7598 RVA: 0x0006B2B2 File Offset: 0x000694B2
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

		// Token: 0x17000A38 RID: 2616
		// (get) Token: 0x06001DAF RID: 7599 RVA: 0x0006B2D0 File Offset: 0x000694D0
		// (set) Token: 0x06001DB0 RID: 7600 RVA: 0x0006B2D8 File Offset: 0x000694D8
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

		// Token: 0x17000A39 RID: 2617
		// (get) Token: 0x06001DB1 RID: 7601 RVA: 0x0006B2F6 File Offset: 0x000694F6
		// (set) Token: 0x06001DB2 RID: 7602 RVA: 0x0006B2FE File Offset: 0x000694FE
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

		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x06001DB3 RID: 7603 RVA: 0x0006B31C File Offset: 0x0006951C
		// (set) Token: 0x06001DB4 RID: 7604 RVA: 0x0006B324 File Offset: 0x00069524
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

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06001DB5 RID: 7605 RVA: 0x0006B342 File Offset: 0x00069542
		// (set) Token: 0x06001DB6 RID: 7606 RVA: 0x0006B34A File Offset: 0x0006954A
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

		// Token: 0x06001DB7 RID: 7607 RVA: 0x0006B368 File Offset: 0x00069568
		private List<TooltipProperty> GetPartyTroopInfo(PartyBase party, FormationClass formationClass)
		{
			List<TooltipProperty> list = new List<TooltipProperty>();
			list.Add(new TooltipProperty("", GameTexts.FindText("str_formation_class_string", formationClass.GetName()).ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.Title));
			foreach (TroopRosterElement troopRosterElement in base.Party.MemberRoster.GetTroopRoster())
			{
				if (!troopRosterElement.Character.IsHero && troopRosterElement.Character.DefaultFormationClass.Equals(formationClass))
				{
					list.Add(new TooltipProperty(troopRosterElement.Character.Name.ToString(), troopRosterElement.Number.ToString(), 0, false, TooltipProperty.TooltipPropertyFlags.None));
				}
			}
			return list;
		}

		// Token: 0x04000D5D RID: 3421
		private readonly Action<ClanPartyItemVM> _onAssignment;

		// Token: 0x04000D5E RID: 3422
		private readonly Action _onExpenseChange;

		// Token: 0x04000D5F RID: 3423
		private readonly Action _onShowChangeLeaderPopup;

		// Token: 0x04000D60 RID: 3424
		private readonly Action<ClanRoleItemVM> _onShowChangeRolePopup;

		// Token: 0x04000D61 RID: 3425
		private readonly ClanPartyItemVM.ClanPartyType _type;

		// Token: 0x04000D62 RID: 3426
		private readonly TextObject _changeLeaderHintText = GameTexts.FindText("str_change_party_leader", null);

		// Token: 0x04000D63 RID: 3427
		private readonly IDisbandPartyCampaignBehavior _disbandBehavior;

		// Token: 0x04000D64 RID: 3428
		private readonly bool _isLeaderTeleporting;

		// Token: 0x04000D65 RID: 3429
		private readonly CharacterObject _leader;

		// Token: 0x04000D66 RID: 3430
		private ClanFinanceExpenseItemVM _expenseItem;

		// Token: 0x04000D67 RID: 3431
		private ClanPartyMemberItemVM _leaderMember;

		// Token: 0x04000D68 RID: 3432
		private CharacterImageIdentifierVM _leaderVisual;

		// Token: 0x04000D69 RID: 3433
		private bool _isMainHeroParty;

		// Token: 0x04000D6A RID: 3434
		private bool _isSelected;

		// Token: 0x04000D6B RID: 3435
		private bool _hasHeroMembers;

		// Token: 0x04000D6C RID: 3436
		private string _partyLocationText;

		// Token: 0x04000D6D RID: 3437
		private string _partySizeText;

		// Token: 0x04000D6E RID: 3438
		private string _shipCountText;

		// Token: 0x04000D6F RID: 3439
		private string _membersText;

		// Token: 0x04000D70 RID: 3440
		private string _assigneesText;

		// Token: 0x04000D71 RID: 3441
		private string _rolesText;

		// Token: 0x04000D72 RID: 3442
		private string _partyLeaderRoleEffectsText;

		// Token: 0x04000D73 RID: 3443
		private string _name;

		// Token: 0x04000D74 RID: 3444
		private string _partySizeSubTitleText;

		// Token: 0x04000D75 RID: 3445
		private string _partyWageSubTitleText;

		// Token: 0x04000D76 RID: 3446
		private int _infantryCount;

		// Token: 0x04000D77 RID: 3447
		private int _rangedCount;

		// Token: 0x04000D78 RID: 3448
		private int _cavalryCount;

		// Token: 0x04000D79 RID: 3449
		private int _horseArcherCount;

		// Token: 0x04000D7A RID: 3450
		private int _shipCount;

		// Token: 0x04000D7B RID: 3451
		private string _inArmyText;

		// Token: 0x04000D7C RID: 3452
		private string _disbandingText;

		// Token: 0x04000D7D RID: 3453
		private string _autoRecruitmentText;

		// Token: 0x04000D7E RID: 3454
		private bool _autoRecruitmentValue;

		// Token: 0x04000D7F RID: 3455
		private bool _isAutoRecruitmentVisible;

		// Token: 0x04000D80 RID: 3456
		private bool _shouldPartyHaveExpense;

		// Token: 0x04000D81 RID: 3457
		private bool _hasCompanion;

		// Token: 0x04000D82 RID: 3458
		private bool _isMembersAndRolesVisible;

		// Token: 0x04000D83 RID: 3459
		private bool _isPendingPartyCreation;

		// Token: 0x04000D84 RID: 3460
		private bool _isCaravan;

		// Token: 0x04000D85 RID: 3461
		private bool _isDisbanding;

		// Token: 0x04000D86 RID: 3462
		private bool _isInArmy;

		// Token: 0x04000D87 RID: 3463
		private bool _canUseActions;

		// Token: 0x04000D88 RID: 3464
		private bool _isChangeLeaderVisible;

		// Token: 0x04000D89 RID: 3465
		private bool _isChangeLeaderEnabled;

		// Token: 0x04000D8A RID: 3466
		private bool _isClanRoleSelectionHighlightEnabled;

		// Token: 0x04000D8B RID: 3467
		private bool _isRoleSelectionPopupVisible;

		// Token: 0x04000D8C RID: 3468
		private HintViewModel _leaderIsMovingToPartyHint;

		// Token: 0x04000D8D RID: 3469
		private HintViewModel _actionsDisabledHint;

		// Token: 0x04000D8E RID: 3470
		private CharacterViewModel _characterModel;

		// Token: 0x04000D8F RID: 3471
		private HintViewModel _autoRecruitmentHint;

		// Token: 0x04000D90 RID: 3472
		private HintViewModel _inArmyHint;

		// Token: 0x04000D91 RID: 3473
		private HintViewModel _changeLeaderHint;

		// Token: 0x04000D92 RID: 3474
		private BasicTooltipViewModel _infantryHint;

		// Token: 0x04000D93 RID: 3475
		private BasicTooltipViewModel _rangedHint;

		// Token: 0x04000D94 RID: 3476
		private BasicTooltipViewModel _cavalryHint;

		// Token: 0x04000D95 RID: 3477
		private BasicTooltipViewModel _horseArcherHint;

		// Token: 0x04000D96 RID: 3478
		private MBBindingList<ClanPartyMemberItemVM> _heroMembers;

		// Token: 0x04000D97 RID: 3479
		private MBBindingList<ClanRoleItemVM> _roles;
	}
}
