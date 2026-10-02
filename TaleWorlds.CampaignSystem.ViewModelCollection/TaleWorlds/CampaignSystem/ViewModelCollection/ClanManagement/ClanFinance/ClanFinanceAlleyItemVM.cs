using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanFinance
{
	// Token: 0x02000137 RID: 311
	public class ClanFinanceAlleyItemVM : ClanFinanceIncomeItemBaseVM
	{
		// Token: 0x06001DBC RID: 7612 RVA: 0x0006B48C File Offset: 0x0006968C
		public ClanFinanceAlleyItemVM(Alley alley, Action<ClanCardSelectionInfo> openCardSelectionPopup, Action<ClanFinanceAlleyItemVM> onSelection, Action onRefresh)
			: base(null, onRefresh)
		{
			this.Alley = alley;
			this._alleyModel = Campaign.Current.Models.AlleyModel;
			this._alleyBehavior = Campaign.Current.GetCampaignBehavior<IAlleyCampaignBehavior>();
			this._onSelection = new Action<ClanFinanceIncomeItemBaseVM>(this.tempOnSelection);
			this._onSelectionT = onSelection;
			this._openCardSelectionPopup = openCardSelectionPopup;
			this.ManageAlleyHint = new HintViewModel();
			this._alleyOwner = this._alleyBehavior.GetAssignedClanMemberOfAlley(this.Alley);
			if (this._alleyOwner == null)
			{
				this._alleyOwner = this.Alley.Owner;
			}
			this.OwnerVisual = new CharacterImageIdentifierVM(CharacterCode.CreateFrom(this._alleyOwner.CharacterObject));
			Settlement settlement = this.Alley.Settlement;
			base.ImageName = ((((settlement != null) ? settlement.SettlementComponent : null) != null) ? this.Alley.Settlement.SettlementComponent.WaitMeshName : "");
			this.RefreshValues();
		}

		// Token: 0x06001DBD RID: 7613 RVA: 0x0006B588 File Offset: 0x00069788
		public override void RefreshValues()
		{
			base.RefreshValues();
			base.Name = this.Alley.Name.ToString();
			base.Location = this.Alley.Settlement.Name.ToString();
			base.Income = this._alleyModel.GetDailyIncomeOfAlley(this.Alley);
			this.IncomeText = GameTexts.FindText("str_plus_with_number", null).SetTextVariable("NUMBER", base.Income).ToString();
			this.ManageAlleyHint.HintText = new TextObject("{=dQBArrqh}Manage Alley", null);
			this.PopulateStatsList();
		}

		// Token: 0x06001DBE RID: 7614 RVA: 0x0006B625 File Offset: 0x00069825
		private void tempOnSelection(ClanFinanceIncomeItemBaseVM item)
		{
			this._onSelectionT(this);
		}

		// Token: 0x06001DBF RID: 7615 RVA: 0x0006B634 File Offset: 0x00069834
		protected override void PopulateStatsList()
		{
			base.PopulateStatsList();
			base.ItemProperties.Clear();
			string text = GameTexts.FindText("str_plus_with_number", null).SetTextVariable("NUMBER", this._alleyModel.GetDailyCrimeRatingOfAlley, 2).ToString();
			string text2 = new TextObject("{=LuC5ZZMu}{CRIMINAL_RATING} ({INCREASE}){CRIME_ICON}", null).SetTextVariable("CRIMINAL_RATING", this.Alley.Settlement.MapFaction.MainHeroCrimeRating, 2).SetTextVariable("INCREASE", text).SetTextVariable("CRIME_ICON", "{=!}<img src=\"SPGeneral\\MapOverlay\\Settlement\\icon_crime\" extend=\"12\">")
				.ToString();
			this.IncomeTextWithVisual = new TextObject("{=ePmSvu1s}{AMOUNT}{GOLD_ICON}", null).SetTextVariable("AMOUNT", base.Income).ToString();
			base.ItemProperties.Add(new SelectableItemPropertyVM(new TextObject("{=FkhJz0po}Location", null).ToString(), this.Alley.Settlement.Name.ToString(), false, null));
			base.ItemProperties.Add(new SelectableItemPropertyVM(new TextObject("{=5k4dxUEJ}Troops", null).ToString(), this._alleyBehavior.GetPlayerOwnedAlleyTroopCount(this.Alley).ToString(), false, null));
			base.ItemProperties.Add(new SelectableItemPropertyVM(new TextObject("{=QPoA6vvx}Income", null).ToString(), this.IncomeTextWithVisual, false, null));
			base.ItemProperties.Add(new SelectableItemPropertyVM(new TextObject("{=r0WIRUHo}Criminal Rating", null).ToString(), text2, false, null));
			string statusText = this.GetStatusText();
			if (!string.IsNullOrEmpty(statusText))
			{
				base.ItemProperties.Add(new SelectableItemPropertyVM(new TextObject("{=DXczLzml}Status", null).ToString(), statusText, false, null));
			}
		}

		// Token: 0x06001DC0 RID: 7616 RVA: 0x0006B7DC File Offset: 0x000699DC
		private string GetStatusText()
		{
			string text = string.Empty;
			List<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>> clanMembersAndAvailabilityDetailsForLeadingAnAlley = this._alleyModel.GetClanMembersAndAvailabilityDetailsForLeadingAnAlley(this.Alley);
			Hero assignedClanMemberOfAlley = this._alleyBehavior.GetAssignedClanMemberOfAlley(this.Alley);
			if (this._alleyBehavior.GetIsPlayerAlleyUnderAttack(this.Alley))
			{
				TextObject textObject = new TextObject("{=q1DVNQS7}Under Attack! ({RESPONSE_TIME} {?RESPONSE_TIME>1}days{?}day{\\?} left.)", null);
				textObject.SetTextVariable("RESPONSE_TIME", this._alleyBehavior.GetResponseTimeLeftForAttackInDays(this.Alley));
				text = textObject.ToString();
			}
			else if (assignedClanMemberOfAlley.IsDead)
			{
				text = new TextObject("{=KjuxDQfn}Alley leader is dead.", null).ToString();
			}
			else if (assignedClanMemberOfAlley.IsTraveling)
			{
				TextObject textObject2 = new TextObject("{=SFB2uYHa}Alley leader is traveling to the alley. ({LEFT_TIME} {?LEFT_TIME>1}hours{?}hour{\\?} left.)", null);
				textObject2.SetTextVariable("LEFT_TIME", MathF.Ceiling(TeleportationHelper.GetHoursLeftForTeleportingHeroToReachItsDestination(assignedClanMemberOfAlley)));
				text = textObject2.ToString();
			}
			else
			{
				for (int i = 0; i < clanMembersAndAvailabilityDetailsForLeadingAnAlley.Count; i++)
				{
					if (clanMembersAndAvailabilityDetailsForLeadingAnAlley[i].Item1 == Hero.MainHero && clanMembersAndAvailabilityDetailsForLeadingAnAlley[i].Item2 != DefaultAlleyModel.AlleyMemberAvailabilityDetail.Available)
					{
						text = new TextObject("{=NHZ1jNIF}Below Requirements", null).ToString();
						break;
					}
				}
			}
			return text;
		}

		// Token: 0x06001DC1 RID: 7617 RVA: 0x0006B8EC File Offset: 0x00069AEC
		private ClanCardSelectionItemPropertyInfo GetSkillProperty(Hero hero, SkillObject skill)
		{
			TextObject textObject = ClanCardSelectionItemPropertyInfo.CreateLabeledValueText(skill.Name, new TextObject("{=!}" + hero.GetSkillValue(skill), null));
			return new ClanCardSelectionItemPropertyInfo(TextObject.GetEmpty(), textObject);
		}

		// Token: 0x06001DC2 RID: 7618 RVA: 0x0006B92C File Offset: 0x00069B2C
		private IEnumerable<ClanCardSelectionItemPropertyInfo> GetHeroProperties(Hero hero, Alley alley, DefaultAlleyModel.AlleyMemberAvailabilityDetail detail)
		{
			if (detail == DefaultAlleyModel.AlleyMemberAvailabilityDetail.AvailableWithDelay)
			{
				string partyDistanceByTimeText = CampaignUIHelper.GetPartyDistanceByTimeText(Campaign.Current.Models.DelayedTeleportationModel.GetTeleportationDelayAsHours(hero, alley.Settlement.Party).ResultNumber, Campaign.Current.Models.DelayedTeleportationModel.DefaultTeleportationSpeed);
				yield return new ClanCardSelectionItemPropertyInfo(new TextObject("{=!}" + partyDistanceByTimeText, null));
			}
			yield return new ClanCardSelectionItemPropertyInfo(new TextObject("{=bz7Glmsm}Skills", null), TextObject.GetEmpty());
			yield return this.GetSkillProperty(hero, DefaultSkills.Tactics);
			yield return this.GetSkillProperty(hero, DefaultSkills.Leadership);
			yield return this.GetSkillProperty(hero, DefaultSkills.Steward);
			yield return this.GetSkillProperty(hero, DefaultSkills.Roguery);
			yield break;
		}

		// Token: 0x06001DC3 RID: 7619 RVA: 0x0006B951 File Offset: 0x00069B51
		private IEnumerable<ClanCardSelectionItemInfo> GetAvailableMembers()
		{
			yield return new ClanCardSelectionItemInfo(new TextObject("{=W3hmFcfv}Abandon Alley", null), false, TextObject.GetEmpty(), TextObject.GetEmpty(), false);
			List<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>> availabilityDetails = this._alleyModel.GetClanMembersAndAvailabilityDetailsForLeadingAnAlley(this.Alley);
			using (List<Hero>.Enumerator enumerator = Clan.PlayerClan.Heroes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Hero member = enumerator.Current;
					ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail> valueTuple = availabilityDetails.FirstOrDefault<ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail>>((ValueTuple<Hero, DefaultAlleyModel.AlleyMemberAvailabilityDetail> x) => x.Item1 == member);
					if (valueTuple.Item1 != null)
					{
						CharacterCode characterCode = CharacterCode.CreateFrom(member.CharacterObject);
						bool flag = valueTuple.Item2 != DefaultAlleyModel.AlleyMemberAvailabilityDetail.Available && valueTuple.Item2 != DefaultAlleyModel.AlleyMemberAvailabilityDetail.AvailableWithDelay;
						yield return new ClanCardSelectionItemInfo(member, member.Name, new CharacterImageIdentifier(characterCode), CardSelectionItemSpriteType.None, null, null, this.GetHeroProperties(member, this.Alley, valueTuple.Item2), flag, this._alleyModel.GetDisabledReasonTextForHero(member, this.Alley, valueTuple.Item2), null, false);
					}
				}
			}
			List<Hero>.Enumerator enumerator = default(List<Hero>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001DC4 RID: 7620 RVA: 0x0006B964 File Offset: 0x00069B64
		private void OnMemberSelection(List<object> members, Action closePopup)
		{
			if (members.Count > 0)
			{
				Hero hero = members[0] as Hero;
				if (hero != null)
				{
					this._alleyBehavior.ChangeAlleyMember(this.Alley, hero);
					Action onRefresh = this._onRefresh;
					if (onRefresh != null)
					{
						onRefresh();
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
					InformationManager.ShowInquiry(new InquiryData(new TextObject("{=W3hmFcfv}Abandon Alley", null).ToString(), new TextObject("{=pBVbKYwo}You will lose the ownership of the alley and the troops in it. Are you sure?", null).ToString(), true, true, new TextObject("{=aeouhelq}Yes", null).ToString(), new TextObject("{=8OkPHu4f}No", null).ToString(), delegate
					{
						this._alleyBehavior.AbandonAlleyFromClanMenu(this.Alley);
						Action onRefresh2 = this._onRefresh;
						if (onRefresh2 != null)
						{
							onRefresh2();
						}
						Action closePopup3 = closePopup;
						if (closePopup3 == null)
						{
							return;
						}
						closePopup3();
					}, null, "", 0f, null, null, null), false, false);
				}
			}
		}

		// Token: 0x06001DC5 RID: 7621 RVA: 0x0006BA40 File Offset: 0x00069C40
		public void ExecuteManageAlley()
		{
			ClanCardSelectionInfo clanCardSelectionInfo = new ClanCardSelectionInfo(new TextObject("{=dQBArrqh}Manage Alley", null), this.GetAvailableMembers(), new Action<List<object>, Action>(this.OnMemberSelection), false, 1, 0);
			this._openCardSelectionPopup(clanCardSelectionInfo);
		}

		// Token: 0x06001DC6 RID: 7622 RVA: 0x0006BA80 File Offset: 0x00069C80
		public void ExecuteBeginHeroHint()
		{
			InformationManager.ShowTooltip(typeof(Hero), new object[] { this._alleyOwner, true });
		}

		// Token: 0x06001DC7 RID: 7623 RVA: 0x0006BAA9 File Offset: 0x00069CA9
		public void ExecuteEndHeroHint()
		{
			InformationManager.HideTooltip();
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06001DC8 RID: 7624 RVA: 0x0006BAB0 File Offset: 0x00069CB0
		// (set) Token: 0x06001DC9 RID: 7625 RVA: 0x0006BAB8 File Offset: 0x00069CB8
		[DataSourceProperty]
		public HintViewModel ManageAlleyHint
		{
			get
			{
				return this._manageAlleyHint;
			}
			set
			{
				if (value != this._manageAlleyHint)
				{
					this._manageAlleyHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "ManageAlleyHint");
				}
			}
		}

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06001DCA RID: 7626 RVA: 0x0006BAD6 File Offset: 0x00069CD6
		// (set) Token: 0x06001DCB RID: 7627 RVA: 0x0006BADE File Offset: 0x00069CDE
		[DataSourceProperty]
		public CharacterImageIdentifierVM OwnerVisual
		{
			get
			{
				return this._ownerVisual;
			}
			set
			{
				if (value != this._ownerVisual)
				{
					this._ownerVisual = value;
					base.OnPropertyChangedWithValue<CharacterImageIdentifierVM>(value, "OwnerVisual");
				}
			}
		}

		// Token: 0x17000A3E RID: 2622
		// (get) Token: 0x06001DCC RID: 7628 RVA: 0x0006BAFC File Offset: 0x00069CFC
		// (set) Token: 0x06001DCD RID: 7629 RVA: 0x0006BB04 File Offset: 0x00069D04
		[DataSourceProperty]
		public string IncomeText
		{
			get
			{
				return this._incomeText;
			}
			set
			{
				if (value != this._incomeText)
				{
					this._incomeText = value;
					base.OnPropertyChangedWithValue<string>(value, "IncomeText");
				}
			}
		}

		// Token: 0x17000A3F RID: 2623
		// (get) Token: 0x06001DCE RID: 7630 RVA: 0x0006BB27 File Offset: 0x00069D27
		// (set) Token: 0x06001DCF RID: 7631 RVA: 0x0006BB2F File Offset: 0x00069D2F
		[DataSourceProperty]
		public string IncomeTextWithVisual
		{
			get
			{
				return this._incomeTextWithVisual;
			}
			set
			{
				if (value != this._incomeTextWithVisual)
				{
					this._incomeTextWithVisual = value;
					base.OnPropertyChangedWithValue<string>(value, "IncomeTextWithVisual");
				}
			}
		}

		// Token: 0x04000D98 RID: 3480
		public readonly Alley Alley;

		// Token: 0x04000D99 RID: 3481
		private readonly Hero _alleyOwner;

		// Token: 0x04000D9A RID: 3482
		private readonly IAlleyCampaignBehavior _alleyBehavior;

		// Token: 0x04000D9B RID: 3483
		private readonly AlleyModel _alleyModel;

		// Token: 0x04000D9C RID: 3484
		private readonly Action<ClanFinanceAlleyItemVM> _onSelectionT;

		// Token: 0x04000D9D RID: 3485
		private readonly Action<ClanCardSelectionInfo> _openCardSelectionPopup;

		// Token: 0x04000D9E RID: 3486
		private HintViewModel _manageAlleyHint;

		// Token: 0x04000D9F RID: 3487
		private CharacterImageIdentifierVM _ownerVisual;

		// Token: 0x04000DA0 RID: 3488
		private string _incomeText;

		// Token: 0x04000DA1 RID: 3489
		private string _incomeTextWithVisual;
	}
}
