using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.ClanPartyItem;
using TaleWorlds.Core;
using TaleWorlds.Core.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories
{
	// Token: 0x02000144 RID: 324
	public class ClanPartiesVM : ViewModel
	{
		// Token: 0x17000AAF RID: 2735
		// (get) Token: 0x06001F14 RID: 7956 RVA: 0x0007008F File Offset: 0x0006E28F
		// (set) Token: 0x06001F15 RID: 7957 RVA: 0x00070097 File Offset: 0x0006E297
		public int TotalExpense { get; private set; }

		// Token: 0x17000AB0 RID: 2736
		// (get) Token: 0x06001F16 RID: 7958 RVA: 0x000700A0 File Offset: 0x0006E2A0
		// (set) Token: 0x06001F17 RID: 7959 RVA: 0x000700A8 File Offset: 0x0006E2A8
		public int TotalIncome { get; private set; }

		// Token: 0x06001F18 RID: 7960 RVA: 0x000700B4 File Offset: 0x0006E2B4
		public ClanPartiesVM(Action onExpenseChange, Action<Hero> openPartyAsManage, Action onRefresh, Action<ClanCardSelectionInfo> openCardSelectionPopup)
		{
			this._onExpenseChange = onExpenseChange;
			this._onRefresh = onRefresh;
			this._disbandBehavior = Campaign.Current.GetCampaignBehavior<IDisbandPartyCampaignBehavior>();
			this._teleportationBehavior = Campaign.Current.GetCampaignBehavior<ITeleportationCampaignBehavior>();
			this._emptyClanPartiesCampaignBehavior = Campaign.Current.GetCampaignBehavior<IEmptyClanPartiesCampaignBehavior>();
			this._openPartyAsManage = openPartyAsManage;
			this._openCardSelectionPopup = openCardSelectionPopup;
			this._faction = Hero.MainHero.Clan;
			this.Parties = new MBBindingList<ClanPartyItemVM>();
			this.Garrisons = new MBBindingList<ClanPartyItemVM>();
			this.Caravans = new MBBindingList<ClanPartyItemVM>();
			MBBindingList<MBBindingList<ClanPartyItemVM>> mbbindingList = new MBBindingList<MBBindingList<ClanPartyItemVM>> { this.Parties, this.Garrisons, this.Caravans };
			this.SortController = new ClanPartiesSortControllerVM(mbbindingList);
			this.CreateNewPartyActionHint = new HintViewModel();
			this.RefreshPartiesList();
			this.RefreshValues();
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x000701CC File Offset: 0x0006E3CC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.SizeText = GameTexts.FindText("str_clan_party_size", null).ToString();
			this.MoraleText = GameTexts.FindText("str_morale", null).ToString();
			this.LocationText = GameTexts.FindText("str_tooltip_label_location", null).ToString();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.CreateNewPartyText = GameTexts.FindText("str_clan_create_new_party", null).ToString();
			this.GarrisonsText = GameTexts.FindText("str_clan_garrisons", null).ToString();
			this.CaravansText = GameTexts.FindText("str_clan_caravans", null).ToString();
			this.RefreshPartiesList();
			this.Parties.ApplyActionOnAllItems(delegate(ClanPartyItemVM x)
			{
				x.RefreshValues();
			});
			this.Garrisons.ApplyActionOnAllItems(delegate(ClanPartyItemVM x)
			{
				x.RefreshValues();
			});
			this.Caravans.ApplyActionOnAllItems(delegate(ClanPartyItemVM x)
			{
				x.RefreshValues();
			});
			this.SortController.RefreshValues();
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x00070308 File Offset: 0x0006E508
		public void RefreshTotalExpense()
		{
			IEnumerable<ClanPartyItemVM> enumerable = from p in this.Parties.Union<ClanPartyItemVM>(this.Garrisons).Union<ClanPartyItemVM>(this.Caravans)
				where p.ShouldPartyHaveExpense
				select p;
			int num;
			if (enumerable == null)
			{
				num = 0;
			}
			else
			{
				num = enumerable.Sum<ClanPartyItemVM>((ClanPartyItemVM p) => p.Expense);
			}
			this.TotalExpense = num;
			this.TotalIncome = this.Caravans.Sum<ClanPartyItemVM>((ClanPartyItemVM p) => p.Income);
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x000703B8 File Offset: 0x0006E5B8
		public void RefreshPartiesList()
		{
			this.Parties.Clear();
			this.Garrisons.Clear();
			this.Caravans.Clear();
			this.SortController.ResetAllStates();
			foreach (WarPartyComponent warPartyComponent in this._faction.WarPartyComponents)
			{
				if (warPartyComponent.MobileParty == MobileParty.MainParty)
				{
					this.Parties.Insert(0, new ClanPartyItemWithPartyVM(warPartyComponent.Party, new Action<ClanPartyItemVM>(this.OnPartySelection), new Action(this.OnAnyExpenseChange), new Action(this.OnShowChangeLeaderPopup), new Action<ClanRoleItemVM>(this.OnShowChangeRolePopup), ClanPartyItemVM.ClanPartyType.Main, this._disbandBehavior, this._teleportationBehavior));
				}
				else
				{
					this.Parties.Add(new ClanPartyItemWithPartyVM(warPartyComponent.Party, new Action<ClanPartyItemVM>(this.OnPartySelection), new Action(this.OnAnyExpenseChange), new Action(this.OnShowChangeLeaderPopup), new Action<ClanRoleItemVM>(this.OnShowChangeRolePopup), ClanPartyItemVM.ClanPartyType.Member, this._disbandBehavior, this._teleportationBehavior));
				}
			}
			using (IEnumerator<CaravanPartyComponent> enumerator2 = this._faction.Heroes.SelectMany<Hero, CaravanPartyComponent>((Hero h) => h.OwnedCaravans).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CaravanPartyComponent party = enumerator2.Current;
					if (!this.Caravans.Any<ClanPartyItemVM>(delegate(ClanPartyItemVM c)
					{
						if (c.Party == null)
						{
							return c.Leader == party.MobileParty.LeaderHero;
						}
						return c.Party.MobileParty == party.MobileParty;
					}))
					{
						this.Caravans.Add(new ClanPartyItemWithPartyVM(party.Party, new Action<ClanPartyItemVM>(this.OnPartySelection), new Action(this.OnAnyExpenseChange), new Action(this.OnShowChangeLeaderPopup), new Action<ClanRoleItemVM>(this.OnShowChangeRolePopup), ClanPartyItemVM.ClanPartyType.Caravan, this._disbandBehavior, this._teleportationBehavior));
					}
				}
			}
			using (IEnumerator<MobileParty> enumerator3 = (from a in this._faction.Settlements
				where a.Town != null
				select a into s
				select s.Town.GarrisonParty).GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					MobileParty garrison = enumerator3.Current;
					if (garrison != null && !this.Garrisons.Any<ClanPartyItemVM>(delegate(ClanPartyItemVM c)
					{
						if (c.Party == null)
						{
							return c.Leader == garrison.LeaderHero;
						}
						return c.Party.MobileParty == garrison;
					}))
					{
						this.Garrisons.Add(new ClanPartyItemWithPartyVM(garrison.Party, new Action<ClanPartyItemVM>(this.OnPartySelection), new Action(this.OnAnyExpenseChange), new Action(this.OnShowChangeLeaderPopup), new Action<ClanRoleItemVM>(this.OnShowChangeRolePopup), ClanPartyItemVM.ClanPartyType.Garrison, this._disbandBehavior, this._teleportationBehavior));
					}
				}
			}
			int num = this._faction.WarPartyComponents.Count;
			if (this._emptyClanPartiesCampaignBehavior != null && this._faction == Hero.MainHero.Clan)
			{
				foreach (Hero hero in this._emptyClanPartiesCampaignBehavior.GetEmptyClanPartyLeaders())
				{
					this.Parties.Add(new ClanPartyItemWithHeroVM(hero, new Action<ClanPartyItemVM>(this.OnPartySelection), new Action(this.OnAnyExpenseChange), new Action(this.OnShowChangeLeaderPopup), new Action<ClanRoleItemVM>(this.OnShowChangeRolePopup), ClanPartyItemVM.ClanPartyType.Member, this._disbandBehavior, this._teleportationBehavior));
				}
				num += this._emptyClanPartiesCampaignBehavior.GetEmptyClanPartyLeaders().Count;
			}
			this._faction.Heroes.Where<Hero>((Hero h) => !h.IsDisabled).Union<Hero>(this._faction.Companions).Any<Hero>((Hero h) => h.IsActive && h.PartyBelongedToAsPrisoner == null && !h.IsChild && h.CanLeadParty() && (h.PartyBelongedTo == null || h.PartyBelongedTo.LeaderHero != h));
			TextObject textObject;
			this.CanCreateNewParty = this.GetCanCreateNewParty(out textObject);
			this.CreateNewPartyActionHint.HintText = textObject;
			GameTexts.SetVariable("CURRENT", num);
			GameTexts.SetVariable("LIMIT", this._faction.WarPartyLimit);
			this.PartiesText = GameTexts.FindText("str_clan_parties", null).ToString();
			GameTexts.SetVariable("CURRENT", this.Caravans.Count);
			this.CaravansText = GameTexts.FindText("str_clan_caravans", null).ToString();
			GameTexts.SetVariable("CURRENT", this.Garrisons.Count);
			this.GarrisonsText = GameTexts.FindText("str_clan_garrisons", null).ToString();
			this.OnPartySelection(this.GetDefaultMember());
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x000708DC File Offset: 0x0006EADC
		private bool GetCanCreateNewParty(out TextObject disabledReason)
		{
			IEnumerable<Hero> enumerable = from h in this._faction.Heroes.Where<Hero>((Hero h) => !h.IsDisabled).Union<Hero>(this._faction.Companions)
				where h.IsActive && h.PartyBelongedToAsPrisoner == null && !h.IsChild && h.CanLeadParty() && (h.PartyBelongedTo == null || h.PartyBelongedTo.LeaderHero != h)
				select h;
			bool flag = !enumerable.IsEmpty<Hero>();
			int partyGoldLowerThreshold = Campaign.Current.Models.ClanFinanceModel.PartyGoldLowerThreshold;
			bool flag2 = enumerable.Any<Hero>((Hero h) => Hero.MainHero.Gold > partyGoldLowerThreshold - h.Gold);
			TextObject textObject;
			if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
			{
				disabledReason = textObject;
				return false;
			}
			if (MobileParty.MainParty.IsCurrentlyAtSea || MobileParty.MainParty.IsInRaftState)
			{
				disabledReason = GameTexts.FindText("str_cannot_perform_action_while_sailing", null);
				return false;
			}
			int num = this._faction.WarPartyLimit - this._faction.WarPartyComponents.Count;
			if (this._emptyClanPartiesCampaignBehavior != null && this._faction == Hero.MainHero.Clan)
			{
				num -= this._emptyClanPartiesCampaignBehavior.GetEmptyClanPartyLeaders().Count;
			}
			if (num <= 0)
			{
				disabledReason = GameTexts.FindText("str_clan_doesnt_have_empty_party_slots", null);
				return false;
			}
			if (!flag)
			{
				disabledReason = GameTexts.FindText("str_clan_doesnt_have_available_heroes", null);
				return false;
			}
			if (!flag2)
			{
				disabledReason = new TextObject("{=VSUqbvbE}You don't have enough gold to create a new party.", null);
				return false;
			}
			disabledReason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x00070A4A File Offset: 0x0006EC4A
		private void OnAnyExpenseChange()
		{
			this.RefreshTotalExpense();
			this._onExpenseChange();
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x00070A5D File Offset: 0x0006EC5D
		private ClanPartyItemVM GetDefaultMember()
		{
			return this.Parties.FirstOrDefault<ClanPartyItemVM>();
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x00070A6A File Offset: 0x0006EC6A
		public void ExecuteCreateNewParty()
		{
			if (this.CanCreateNewParty)
			{
				if (this.GetNewPartyLeaderCandidates().Any<ClanCardSelectionItemInfo>())
				{
					this.OnShowNewPartyPopup();
					return;
				}
				MBInformationManager.AddQuickInformation(new TextObject("{=qZvNIVGV}There is no one available in your clan who can lead a party right now.", null), 0, null, null, "");
			}
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x00070AA0 File Offset: 0x0006ECA0
		public void SelectParty(PartyBase party)
		{
			foreach (ClanPartyItemVM clanPartyItemVM in this.Parties)
			{
				if (clanPartyItemVM.Party == party)
				{
					this.OnPartySelection(clanPartyItemVM);
					break;
				}
			}
			foreach (ClanPartyItemVM clanPartyItemVM2 in this.Caravans)
			{
				if (clanPartyItemVM2.Party == party)
				{
					this.OnPartySelection(clanPartyItemVM2);
					break;
				}
			}
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x00070B40 File Offset: 0x0006ED40
		private void OnPartySelection(ClanPartyItemVM party)
		{
			if (this.CurrentSelectedParty != null)
			{
				this.CurrentSelectedParty.IsSelected = false;
			}
			this.CurrentSelectedParty = party;
			if (party != null)
			{
				party.IsSelected = true;
			}
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x00070B68 File Offset: 0x0006ED68
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.Parties.ApplyActionOnAllItems(delegate(ClanPartyItemVM p)
			{
				p.OnFinalize();
			});
			this.Garrisons.ApplyActionOnAllItems(delegate(ClanPartyItemVM p)
			{
				p.OnFinalize();
			});
			this.Caravans.ApplyActionOnAllItems(delegate(ClanPartyItemVM p)
			{
				p.OnFinalize();
			});
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x00070BFC File Offset: 0x0006EDFC
		public void OnShowNewPartyPopup()
		{
			ClanCardSelectionInfo clanCardSelectionInfo = new ClanCardSelectionInfo(new TextObject("{=0Q4Xo2BQ}Select the Leader of the New Party", null), this.GetNewPartyLeaderCandidates(), new Action<List<object>, Action>(this.OnNewPartyCreationOver), false, 1, 0);
			Action<ClanCardSelectionInfo> openCardSelectionPopup = this._openCardSelectionPopup;
			if (openCardSelectionPopup == null)
			{
				return;
			}
			openCardSelectionPopup(clanCardSelectionInfo);
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x00070C41 File Offset: 0x0006EE41
		private IEnumerable<ClanCardSelectionItemInfo> GetNewPartyLeaderCandidates()
		{
			int partyGoldLowerThreshold = Campaign.Current.Models.ClanFinanceModel.PartyGoldLowerThreshold;
			foreach (Hero hero in this._faction.Heroes.Where<Hero>((Hero h) => !h.IsDisabled).Union<Hero>(this._faction.Companions))
			{
				if ((hero.IsActive || hero.IsReleased || hero.IsFugitive) && !hero.IsChild && hero != Hero.MainHero && hero.CanBeGovernorOrHavePartyRole())
				{
					bool flag = false;
					TextObject textObject = TextObject.GetEmpty();
					if (hero.PartyBelongedToAsPrisoner != null)
					{
						textObject = new TextObject("{=vOojEcIf}You cannot assign a prisoner member as a new party leader", null);
					}
					else if (hero.IsReleased)
					{
						textObject = new TextObject("{=OhNYkblK}This hero has just escaped from captors and will be available after some time.", null);
					}
					else if (hero.PartyBelongedTo != null && hero.PartyBelongedTo.LeaderHero == hero)
					{
						textObject = new TextObject("{=aFYwbosi}This hero is already leading a party.", null);
					}
					else if (hero.PartyBelongedTo != null && hero.PartyBelongedTo.LeaderHero != Hero.MainHero)
					{
						textObject = new TextObject("{=FjJi1DJb}This hero is already a part of an another party.", null);
					}
					else if (hero.GovernorOf != null)
					{
						textObject = new TextObject("{=Hz8XO8wk}Governors cannot lead a mobile party and be a governor at the same time.", null);
					}
					else if (hero.HeroState == Hero.CharacterStates.Disabled)
					{
						textObject = new TextObject("{=slzfQzl3}This hero is lost", null);
					}
					else if (hero.HeroState == Hero.CharacterStates.Fugitive)
					{
						textObject = new TextObject("{=dD3kRDHi}This hero is a fugitive and running from their captors. They will be available after some time.", null);
					}
					else if (partyGoldLowerThreshold - hero.Gold > Hero.MainHero.Gold)
					{
						textObject = new TextObject("{=xpCdwmlX}You don't have enough gold to make {HERO.NAME} a party leader.", null);
						textObject.SetCharacterProperties("HERO", hero.CharacterObject, false);
					}
					else if (hero.PartyBelongedTo != null && hero.PartyBelongedTo.IsCurrentlyAtSea)
					{
						textObject = new TextObject("{=1ELK1UbN}{HERO.NAME} is currently sailing.", null);
						textObject.SetCharacterProperties("HERO", hero.CharacterObject, false);
					}
					else
					{
						flag = true;
					}
					yield return new ClanCardSelectionItemInfo(hero, hero.Name, new CharacterImageIdentifier(CampaignUIHelper.GetCharacterCode(hero.CharacterObject, false)), CardSelectionItemSpriteType.None, null, null, this.GetNewPartyLeaderCandidateProperties(hero), !flag, textObject, null, false);
				}
			}
			IEnumerator<Hero> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x00070C51 File Offset: 0x0006EE51
		private IEnumerable<ClanCardSelectionItemPropertyInfo> GetNewPartyLeaderCandidateProperties(Hero hero)
		{
			TextObject textObject = ClanCardSelectionItemPropertyInfo.CreateLabeledValueText(new TextObject("{=IXwOaa98}Party Size", null), new TextObject(CampaignUIHelper.GetPartySizeLimitForLeader(hero), null));
			yield return new ClanCardSelectionItemPropertyInfo(textObject);
			TextObject textObject2 = new TextObject("{=hwrQqWir}No Skills", null);
			int num = 0;
			foreach (SkillObject skillObject in this._leaderAssignmentRelevantSkills)
			{
				TextObject textObject3 = new TextObject("{=!}{SKILL_VALUE}", null);
				textObject3.SetTextVariable("SKILL_VALUE", hero.GetSkillValue(skillObject));
				TextObject textObject4 = ClanCardSelectionItemPropertyInfo.CreateLabeledValueText(skillObject.Name, textObject3);
				if (num == 0)
				{
					textObject2 = textObject4;
				}
				else
				{
					TextObject textObject5 = GameTexts.FindText("str_string_newline_newline_string", null);
					textObject5.SetTextVariable("STR1", textObject2);
					textObject5.SetTextVariable("STR2", textObject4);
					textObject2 = textObject5;
				}
				num++;
			}
			yield return new ClanCardSelectionItemPropertyInfo(GameTexts.FindText("str_skills", null), textObject2);
			yield break;
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x00070C68 File Offset: 0x0006EE68
		private void OnNewPartyCreationOver(List<object> selectedItems, Action closePopup)
		{
			if (selectedItems.Count == 1)
			{
				Hero newLeader = selectedItems.FirstOrDefault<object>() as Hero;
				int partyGoldLowerThreshold = Campaign.Current.Models.ClanFinanceModel.PartyGoldLowerThreshold;
				if (newLeader.Gold < partyGoldLowerThreshold)
				{
					string text = new TextObject("{=DAYoD0aW}Create Party", null).ToString();
					string text2 = new TextObject("{=fRz2DJf4}Creating the party will cost you {PARTY_COST}{GOLD_ICON}. Are you sure?", null).SetTextVariable("PARTY_COST", partyGoldLowerThreshold - newLeader.Gold).SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">").ToString();
					InformationManager.ShowInquiry(new InquiryData(text, text2, true, true, new TextObject("{=aeouhelq}Yes", null).ToString(), new TextObject("{=3CpNUnVl}Cancel", null).ToString(), delegate
					{
						Action closePopup4 = closePopup;
						if (closePopup4 != null)
						{
							closePopup4();
						}
						this.CreateNewClanParty(newLeader, partyGoldLowerThreshold);
					}, null, "", 0f, null, null, null), false, false);
					return;
				}
				Action closePopup2 = closePopup;
				if (closePopup2 != null)
				{
					closePopup2();
				}
				this.CreateNewClanParty(newLeader, partyGoldLowerThreshold);
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

		// Token: 0x06001F27 RID: 7975 RVA: 0x00070DB4 File Offset: 0x0006EFB4
		private void CreateNewClanParty(Hero newLeader, int partyGoldLowerThreshold)
		{
			if (newLeader.PartyBelongedTo == MobileParty.MainParty)
			{
				this._openPartyAsManage(newLeader);
				return;
			}
			MobileParty mobileParty = MobilePartyHelper.CreateNewClanMobileParty(newLeader, this._faction);
			if (newLeader.Gold < partyGoldLowerThreshold)
			{
				GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, newLeader, partyGoldLowerThreshold - newLeader.Gold, false);
			}
			mobileParty.SetMoveModeHold();
			this._onRefresh();
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x00070E18 File Offset: 0x0006F018
		public void OnShowChangeLeaderPopup()
		{
			ClanCardSelectionInfo clanCardSelectionInfo = new ClanCardSelectionInfo(GameTexts.FindText("str_change_party_leader", null), this.GetChangeLeaderCandidates(), new Action<List<object>, Action>(this.OnChangeLeaderOver), false, 1, 0);
			Action<ClanCardSelectionInfo> openCardSelectionPopup = this._openCardSelectionPopup;
			if (openCardSelectionPopup == null)
			{
				return;
			}
			openCardSelectionPopup(clanCardSelectionInfo);
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x00070E5D File Offset: 0x0006F05D
		private IEnumerable<ClanCardSelectionItemInfo> GetChangeLeaderCandidates()
		{
			TextObject textObject;
			bool canDisbandParty = this.GetCanDisbandParty(out textObject);
			yield return new ClanCardSelectionItemInfo(GameTexts.FindText("str_disband_party", null), !canDisbandParty, textObject, null, false);
			foreach (Hero hero in this._faction.Heroes.Where<Hero>((Hero h) => !h.IsDisabled).Union<Hero>(this._faction.Companions))
			{
				if ((hero.IsActive || hero.IsReleased || hero.IsFugitive || hero.IsTraveling) && !hero.IsChild && hero != Hero.MainHero && hero.CanLeadParty())
				{
					Hero hero2 = hero;
					ClanPartyMemberItemVM leaderMember = this.CurrentSelectedParty.LeaderMember;
					if (hero2 != ((leaderMember != null) ? leaderMember.HeroObject : null))
					{
						Hero hero3 = hero;
						bool flag = true;
						PartyBase party = this.CurrentSelectedParty.Party;
						TextObject textObject2;
						bool flag2 = FactionHelper.IsMainClanMemberAvailableForPartyLeaderChange(hero3, flag, (party != null) ? party.MobileParty : null, out textObject2);
						CharacterImageIdentifier characterImageIdentifier = new CharacterImageIdentifier(CampaignUIHelper.GetCharacterCode(hero.CharacterObject, false));
						yield return new ClanCardSelectionItemInfo(hero, hero.Name, characterImageIdentifier, CardSelectionItemSpriteType.None, null, null, this.GetChangeLeaderCandidateProperties(hero), !flag2, textObject2, null, false);
					}
				}
			}
			IEnumerator<Hero> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x00070E6D File Offset: 0x0006F06D
		private IEnumerable<ClanCardSelectionItemPropertyInfo> GetChangeLeaderCandidateProperties(Hero hero)
		{
			TextObject teleportationDelayText = CampaignUIHelper.GetTeleportationDelayText(hero, this.CurrentSelectedParty.Party);
			yield return new ClanCardSelectionItemPropertyInfo(teleportationDelayText);
			PartyBase party = this.CurrentSelectedParty.Party;
			TextObject textObject = ((((party != null) ? party.LeaderHero : null) != null || this.CurrentSelectedParty.Leader == null) ? CampaignUIHelper.GetPartySizeLimitWithDifferentLeader(this.CurrentSelectedParty.Party, hero) : CampaignUIHelper.GetPartySizeLimitWithDifferentLeader(this.CurrentSelectedParty.Leader, hero));
			TextObject textObject2 = ClanCardSelectionItemPropertyInfo.CreateLabeledValueText(new TextObject("{=IXwOaa98}Party Size", null), textObject);
			yield return new ClanCardSelectionItemPropertyInfo(textObject2);
			TextObject textObject3 = new TextObject("{=hwrQqWir}No Skills", null);
			int num = 0;
			foreach (SkillObject skillObject in this._leaderAssignmentRelevantSkills)
			{
				TextObject textObject4 = new TextObject("{=!}{SKILL_VALUE}", null);
				textObject4.SetTextVariable("SKILL_VALUE", hero.GetSkillValue(skillObject));
				TextObject textObject5 = ClanCardSelectionItemPropertyInfo.CreateLabeledValueText(skillObject.Name, textObject4);
				if (num == 0)
				{
					textObject3 = textObject5;
				}
				else
				{
					TextObject textObject6 = GameTexts.FindText("str_string_newline_newline_string", null);
					textObject6.SetTextVariable("STR1", textObject3);
					textObject6.SetTextVariable("STR2", textObject5);
					textObject3 = textObject6;
				}
				num++;
			}
			yield return new ClanCardSelectionItemPropertyInfo(GameTexts.FindText("str_skills", null), textObject3);
			yield break;
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x00070E84 File Offset: 0x0006F084
		private void OnChangeLeaderOver(List<object> selectedItems, Action closePopup)
		{
			if (selectedItems.Count == 1)
			{
				Hero newLeader = selectedItems.FirstOrDefault<object>() as Hero;
				bool isDisband = newLeader == null;
				int partyGoldLowerThreshold = Campaign.Current.Models.ClanFinanceModel.PartyGoldLowerThreshold;
				ClanPartyItemVM currentSelectedParty = this.CurrentSelectedParty;
				PartyBase partyBase = ((currentSelectedParty != null) ? currentSelectedParty.Party : null);
				MobileParty mobileParty = ((partyBase != null) ? partyBase.MobileParty : null);
				DelayedTeleportationModel delayedTeleportationModel = Campaign.Current.Models.DelayedTeleportationModel;
				int num = ((!isDisband && mobileParty != null) ? ((int)Math.Ceiling((double)delayedTeleportationModel.GetTeleportationDelayAsHours(newLeader, mobileParty.Party).ResultNumber)) : 0);
				MBTextManager.SetTextVariable("TRAVEL_DURATION", CampaignUIHelper.GetHoursAndDaysTextFromHourValue(num).ToString(), false);
				Hero newLeader2 = newLeader;
				if (((newLeader2 != null) ? newLeader2.CharacterObject : null) != null)
				{
					StringHelpers.SetCharacterProperties("LEADER", newLeader.CharacterObject, null, false);
					MBTextManager.SetTextVariable("PARTY_COST", partyGoldLowerThreshold - newLeader.Gold);
					MBTextManager.SetTextVariable("GOLD_ICON", "{=!}<img src=\"General\\Icons\\Coin@2x\" extend=\"6\">", false);
					MBTextManager.SetTextVariable("DOES_LEADER_NEED_GOLD", (partyGoldLowerThreshold > newLeader.Gold) ? 1 : 0);
				}
				if (isDisband && partyBase != null && partyBase.Ships.Count > 0)
				{
					MBTextManager.SetTextVariable("DOES_DISBANDING_PARTY_HAVE_SHIP", true);
				}
				object obj = GameTexts.FindText(isDisband ? "str_disband_party" : "str_change_clan_party_leader", null);
				TextObject textObject = GameTexts.FindText(isDisband ? "str_disband_party_inquiry" : ((num == 0) ? "str_change_clan_party_leader_instantly_inquiry" : "str_change_clan_party_leader_inquiry"), null);
				InformationManager.ShowInquiry(new InquiryData(obj.ToString(), textObject.ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
				{
					Action closePopup3 = closePopup;
					if (closePopup3 != null)
					{
						closePopup3();
					}
					this.OnPartyLeaderChanged(newLeader);
					if (isDisband)
					{
						this.OnDisbandCurrentParty();
					}
					else if (newLeader.Gold < partyGoldLowerThreshold)
					{
						GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, newLeader, partyGoldLowerThreshold - newLeader.Gold, false);
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

		// Token: 0x06001F2C RID: 7980 RVA: 0x000710BC File Offset: 0x0006F2BC
		private void OnPartyLeaderChanged(Hero newLeader)
		{
			ClanPartyItemVM currentSelectedParty = this.CurrentSelectedParty;
			bool flag;
			if (currentSelectedParty == null)
			{
				flag = null != null;
			}
			else
			{
				PartyBase party = currentSelectedParty.Party;
				flag = ((party != null) ? party.LeaderHero : null) != null;
			}
			if (flag)
			{
				if (newLeader == null)
				{
					Hero leaderHero = this.CurrentSelectedParty.Party.LeaderHero;
					this.CurrentSelectedParty.Party.MobileParty.RemovePartyLeader();
					MakeHeroFugitiveAction.Apply(leaderHero, false);
				}
				else
				{
					TeleportHeroAction.ApplyDelayedTeleportToParty(this.CurrentSelectedParty.Party.LeaderHero, MobileParty.MainParty);
				}
			}
			else if (this.CurrentSelectedParty.Leader != null && this.CurrentSelectedParty is ClanPartyItemWithHeroVM && newLeader == null)
			{
				IEmptyClanPartiesCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IEmptyClanPartiesCampaignBehavior>();
				if (campaignBehavior != null)
				{
					campaignBehavior.DisbandCachedLordPartyForPlayerClan(this.CurrentSelectedParty.Leader);
				}
			}
			if (newLeader != null)
			{
				if (this.CurrentSelectedParty.Leader != null && this.CurrentSelectedParty is ClanPartyItemWithHeroVM)
				{
					ClanPartyItemVM currentSelectedParty2 = this.CurrentSelectedParty;
					int partyGoldLowerThreshold = Campaign.Current.Models.ClanFinanceModel.PartyGoldLowerThreshold;
					this.CreateNewClanParty(newLeader, partyGoldLowerThreshold);
					IEmptyClanPartiesCampaignBehavior campaignBehavior2 = Campaign.Current.GetCampaignBehavior<IEmptyClanPartiesCampaignBehavior>();
					if (campaignBehavior2 == null)
					{
						return;
					}
					campaignBehavior2.TransferCachedLordPartyToNewPartyForPlayerClan(currentSelectedParty2.Leader, newLeader.PartyBelongedTo.Party);
					return;
				}
				else
				{
					TeleportHeroAction.ApplyDelayedTeleportToPartyAsPartyLeader(newLeader, this.CurrentSelectedParty.Party.MobileParty);
				}
			}
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x000711F0 File Offset: 0x0006F3F0
		private void OnDisbandCurrentParty()
		{
			if (!(this.CurrentSelectedParty is ClanPartyItemWithHeroVM))
			{
				DisbandPartyAction.StartDisband(this.CurrentSelectedParty.Party.MobileParty);
				return;
			}
			IEmptyClanPartiesCampaignBehavior campaignBehavior = Campaign.Current.GetCampaignBehavior<IEmptyClanPartiesCampaignBehavior>();
			if (campaignBehavior == null)
			{
				return;
			}
			campaignBehavior.DisbandCachedLordPartyForPlayerClan(this.CurrentSelectedParty.Leader);
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x00071240 File Offset: 0x0006F440
		private bool GetCanDisbandParty(out TextObject cannotDisbandReason)
		{
			bool flag = false;
			cannotDisbandReason = TextObject.GetEmpty();
			ClanPartyItemVM currentSelectedParty = this.CurrentSelectedParty;
			MobileParty mobileParty;
			if (currentSelectedParty == null)
			{
				mobileParty = null;
			}
			else
			{
				PartyBase party = currentSelectedParty.Party;
				mobileParty = ((party != null) ? party.MobileParty : null);
			}
			MobileParty mobileParty2 = mobileParty;
			if (mobileParty2 != null)
			{
				TextObject textObject;
				if (!CampaignUIHelper.GetMapScreenActionIsEnabledWithReason(out textObject))
				{
					cannotDisbandReason = textObject;
				}
				else if (mobileParty2.IsMilitia)
				{
					cannotDisbandReason = GameTexts.FindText("str_cannot_disband_milita_party", null);
				}
				else if (mobileParty2.IsGarrison)
				{
					cannotDisbandReason = GameTexts.FindText("str_cannot_disband_garrison_party", null);
				}
				else if (mobileParty2.IsMainParty)
				{
					cannotDisbandReason = GameTexts.FindText("str_cannot_disband_main_party", null);
				}
				else if (this.CurrentSelectedParty.IsDisbanding)
				{
					cannotDisbandReason = GameTexts.FindText("str_cannot_disband_already_disbanding_party", null);
				}
				else if (mobileParty2.MapEvent != null || mobileParty2.SiegeEvent != null)
				{
					cannotDisbandReason = GameTexts.FindText("str_cannot_disband_during_battle", null);
				}
				else
				{
					flag = true;
				}
			}
			if (this.CurrentSelectedParty is ClanPartyItemWithHeroVM)
			{
				if (this.CurrentSelectedParty.IsDisbanding)
				{
					cannotDisbandReason = GameTexts.FindText("str_cannot_disband_already_disbanding_party", null);
				}
				else
				{
					flag = true;
				}
			}
			return flag;
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x0007133C File Offset: 0x0006F53C
		public void OnShowChangeRolePopup(ClanRoleItemVM role)
		{
			ClanPartyItemVM currentSelectedParty = this.CurrentSelectedParty;
			bool flag;
			if (currentSelectedParty == null)
			{
				flag = null != null;
			}
			else
			{
				PartyBase party = currentSelectedParty.Party;
				flag = ((party != null) ? party.MobileParty : null) != null;
			}
			if (flag && role != null)
			{
				this._currentSelectedRole = role.Role;
				ClanCardSelectionInfo clanCardSelectionInfo = new ClanCardSelectionInfo(new TextObject("{=gxdqiK8m}Assign as {PARTY_ROLE}", null).SetTextVariable("PARTY_ROLE", GameTexts.FindText("role", this._currentSelectedRole.ToString())), this.GetChangeRoleCandidates(), new Action<List<object>, Action>(this.OnChangeRoleOver), false, 1, 0);
				Action<ClanCardSelectionInfo> openCardSelectionPopup = this._openCardSelectionPopup;
				if (openCardSelectionPopup == null)
				{
					return;
				}
				openCardSelectionPopup(clanCardSelectionInfo);
			}
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x000713D5 File Offset: 0x0006F5D5
		private IEnumerable<ClanCardSelectionItemInfo> GetChangeRoleCandidates()
		{
			Hero roleHolder = this.CurrentSelectedParty.Party.MobileParty.GetRoleHolder(this._currentSelectedRole);
			yield return new ClanCardSelectionItemInfo(new TextObject("{=DWYEZAMC}No Assignee", null), false, null, null, roleHolder == null);
			MBBindingList<ClanPartyMemberItemVM> mbbindingList = new MBBindingList<ClanPartyMemberItemVM>();
			foreach (ClanPartyMemberItemVM clanPartyMemberItemVM in this.CurrentSelectedParty.HeroMembers)
			{
				mbbindingList.Add(clanPartyMemberItemVM);
			}
			mbbindingList.Sort(new ClanPartiesVM.ClanRoleMemberComparer(this._currentSelectedRole));
			foreach (ClanPartyMemberItemVM clanPartyMemberItemVM2 in mbbindingList)
			{
				Hero heroObject = clanPartyMemberItemVM2.HeroObject;
				CharacterImageIdentifier characterImageIdentifier = new CharacterImageIdentifier(CampaignUIHelper.GetCharacterCode(heroObject.CharacterObject, false));
				TextObject textObject;
				bool flag = !this.IsHeroAssignableForRole(heroObject, this._currentSelectedRole, this.CurrentSelectedParty.Party.MobileParty, out textObject);
				yield return new ClanCardSelectionItemInfo(heroObject, heroObject.Name, characterImageIdentifier, CardSelectionItemSpriteType.Skill, Campaign.Current.Models.ClanMemberPartyRoleModel.GetRelevantSkillForPartyRole(this._currentSelectedRole).ToString(), SkillHelper.GetHeroRelevantSkillValueForPartyRole(heroObject, this._currentSelectedRole).ToString(), this.GetChangeRoleCandidateProperties(heroObject, this._currentSelectedRole), flag, textObject, null, roleHolder == heroObject);
			}
			IEnumerator<ClanPartyMemberItemVM> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x000713E8 File Offset: 0x0006F5E8
		private bool IsHeroAssignableForRole(Hero hero, PartyRole role, MobileParty party, out TextObject reason)
		{
			reason = null;
			if (!Campaign.Current.Models.ClanMemberPartyRoleModel.IsHeroAssignableForPartyRole(hero, role, party))
			{
				if (!Campaign.Current.Models.ClanMemberPartyRoleModel.DoesHeroHaveEnoughSkillForPartyRole(hero, role, party))
				{
					reason = GameTexts.FindText("str_character_role_disabled_tooltip", null).SetTextVariable("SKILL_NAME", Campaign.Current.Models.ClanMemberPartyRoleModel.GetRelevantSkillForPartyRole(role).Name.ToString()).SetTextVariable("MIN_SKILL_AMOUNT", 0);
					return false;
				}
				reason = new TextObject("{=DBabgrcC}This hero is not available right now.", null);
				return false;
			}
			else
			{
				if (!party.GetHeroPartyRoles(hero).Contains(this._currentSelectedRole) && !party.CanAssignMoreRolesToHero(hero))
				{
					reason = new TextObject("{=3O5eo4Ws}This hero is already assigned to the maximum amount of roles allowed.", null);
					return false;
				}
				return true;
			}
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x000714AD File Offset: 0x0006F6AD
		private IEnumerable<ClanCardSelectionItemPropertyInfo> GetChangeRoleCandidateProperties(Hero hero, PartyRole role)
		{
			GameTexts.SetVariable("newline", "\n");
			IEnumerable<SkillEffect> enumerable = SkillEffect.All.Where<SkillEffect>((SkillEffect x) => x.Role == role);
			IEnumerable<PerkObject> perks = PerkObject.All.Where<PerkObject>((PerkObject x) => hero.GetPerkValue(x) && (x.PrimaryRole == role || x.SecondaryRole == role));
			if (SkillHelper.GetHeroRelevantSkillValueForPartyRole(hero, role) > 0 && (enumerable.Count<SkillEffect>() > 0 || perks.Count<PerkObject>() > 0))
			{
				int num = 0;
				TextObject textObject = null;
				foreach (SkillEffect skillEffect in enumerable)
				{
					TextObject effectDescriptionForSkillLevel = SkillHelper.GetEffectDescriptionForSkillLevel(skillEffect, SkillHelper.GetHeroRelevantSkillValueForPartyRole(hero, role));
					if (num == 0)
					{
						textObject = effectDescriptionForSkillLevel;
					}
					else
					{
						TextObject textObject2 = GameTexts.FindText("str_string_newline_newline_string", null);
						textObject2.SetTextVariable("STR1", textObject);
						textObject2.SetTextVariable("STR2", effectDescriptionForSkillLevel);
						textObject = textObject2;
					}
					num++;
				}
				yield return new ClanCardSelectionItemPropertyInfo(new TextObject("{=DKJIp6xG}Effects", null), (num > 0) ? textObject : new TextObject("{=trbPP3ae}No effects", null));
				int num2 = 0;
				TextObject textObject3 = null;
				foreach (PerkObject perkObject in perks)
				{
					TextObject textObject4 = ClanCardSelectionItemPropertyInfo.CreateLabeledValueText(perkObject.Name, perkObject.PrimaryDescription);
					if (num2 == 0)
					{
						textObject3 = textObject4;
					}
					else
					{
						TextObject textObject5 = GameTexts.FindText("str_string_newline_newline_string", null);
						textObject5.SetTextVariable("STR1", textObject3);
						textObject5.SetTextVariable("STR2", textObject4);
						textObject3 = textObject5;
					}
					num2++;
				}
				yield return new ClanCardSelectionItemPropertyInfo(new TextObject("{=Avy8Gua1}Perks", null), (num2 > 0) ? textObject3 : new TextObject("{=oSfsqBwJ}No perks", null));
			}
			else
			{
				yield return new ClanCardSelectionItemPropertyInfo(new TextObject("{=a1snO91x}No relevant perks/effects", null), TextObject.GetEmpty());
			}
			yield break;
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x000714C4 File Offset: 0x0006F6C4
		private void OnChangeRoleOver(List<object> selectedItems, Action closePopup)
		{
			if (selectedItems.Count == 1)
			{
				ClanPartyItemVM currentSelectedParty = this.CurrentSelectedParty;
				MobileParty mobileParty = ((currentSelectedParty != null) ? currentSelectedParty.Party.MobileParty : null);
				Hero hero;
				if ((hero = selectedItems[0] as Hero) != null && mobileParty != null)
				{
					if (!mobileParty.GetHeroPartyRoles(hero).Contains(this._currentSelectedRole) && !mobileParty.CanAssignMoreRolesToHero(hero))
					{
						TextObject textObject = new TextObject("{=lra67ngl}{HERO.NAME} is assigned to the maximum amount of roles, and will be unassigned from {?HERO.GENDER}her{?}his{\\?} role as {OTHER_ROLE}. Continue anyway?", null).SetTextVariable("OTHER_ROLE", GameTexts.FindText("role", mobileParty.GetHeroPartyRoles(hero).FirstOrDefault<PartyRole>().ToString()));
						textObject.SetCharacterProperties("HERO", hero.CharacterObject, false);
						InformationManager.ShowInquiry(new InquiryData(new TextObject("{=RFXFM2Az}Maximum Roles Reached", null).ToString(), textObject.ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), delegate
						{
							this.AssignHeroToRole(selectedItems[0] as Hero, this._currentSelectedRole);
							Action closePopup4 = closePopup;
							if (closePopup4 == null)
							{
								return;
							}
							closePopup4();
						}, null, "", 0f, null, null, null), false, false);
						return;
					}
					this.AssignHeroToRole(selectedItems[0] as Hero, this._currentSelectedRole);
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
					this.AssignHeroToRole(null, this._currentSelectedRole);
					Action closePopup3 = closePopup;
					if (closePopup3 == null)
					{
						return;
					}
					closePopup3();
				}
			}
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x00071650 File Offset: 0x0006F850
		private void AssignHeroToRole(Hero hero, PartyRole role)
		{
			ClanPartyItemVM currentSelectedParty = this.CurrentSelectedParty;
			MobileParty mobileParty = ((currentSelectedParty != null) ? currentSelectedParty.Party.MobileParty : null);
			if (mobileParty == null)
			{
				Debug.FailedAssert("No MobileParty selected while assigning hero to a role!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem.ViewModelCollection\\ClanManagement\\Categories\\ClanPartiesVM.cs", "AssignHeroToRole", 887);
				return;
			}
			if (hero == null || !mobileParty.GetHeroPartyRoles(hero).Contains(role))
			{
				switch (role)
				{
				case PartyRole.Surgeon:
					mobileParty.SetPartySurgeon(hero);
					break;
				case PartyRole.Engineer:
					mobileParty.SetPartyEngineer(hero);
					break;
				case PartyRole.Scout:
					mobileParty.SetPartyScout(hero);
					break;
				case PartyRole.Quartermaster:
					mobileParty.SetPartyQuartermaster(hero);
					break;
				case PartyRole.FirstMate:
					mobileParty.SetPartyFirstMate(hero);
					break;
				case PartyRole.Navigator:
					mobileParty.SetPartyNavigator(hero);
					break;
				}
				Game game = Game.Current;
				if (game != null)
				{
					game.EventManager.TriggerEvent<ClanRoleAssignedThroughClanScreenEvent>(new ClanRoleAssignedThroughClanScreenEvent(role, hero));
				}
				ClanPartyItemVM currentSelectedParty2 = this.CurrentSelectedParty;
				if (currentSelectedParty2 == null)
				{
					return;
				}
				currentSelectedParty2.Roles.ApplyActionOnAllItems(delegate(ClanRoleItemVM x)
				{
					x.Refresh();
				});
			}
		}

		// Token: 0x17000AB1 RID: 2737
		// (get) Token: 0x06001F35 RID: 7989 RVA: 0x00071759 File Offset: 0x0006F959
		// (set) Token: 0x06001F36 RID: 7990 RVA: 0x00071761 File Offset: 0x0006F961
		[DataSourceProperty]
		public HintViewModel CreateNewPartyActionHint
		{
			get
			{
				return this._createNewPartyActionHint;
			}
			set
			{
				if (value != this._createNewPartyActionHint)
				{
					this._createNewPartyActionHint = value;
					base.OnPropertyChangedWithValue<HintViewModel>(value, "CreateNewPartyActionHint");
				}
			}
		}

		// Token: 0x17000AB2 RID: 2738
		// (get) Token: 0x06001F37 RID: 7991 RVA: 0x0007177F File Offset: 0x0006F97F
		// (set) Token: 0x06001F38 RID: 7992 RVA: 0x00071787 File Offset: 0x0006F987
		[DataSourceProperty]
		public bool IsAnyValidPartySelected
		{
			get
			{
				return this._isAnyValidPartySelected;
			}
			set
			{
				if (value != this._isAnyValidPartySelected)
				{
					this._isAnyValidPartySelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyValidPartySelected");
				}
			}
		}

		// Token: 0x17000AB3 RID: 2739
		// (get) Token: 0x06001F39 RID: 7993 RVA: 0x000717A5 File Offset: 0x0006F9A5
		// (set) Token: 0x06001F3A RID: 7994 RVA: 0x000717AD File Offset: 0x0006F9AD
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

		// Token: 0x17000AB4 RID: 2740
		// (get) Token: 0x06001F3B RID: 7995 RVA: 0x000717D0 File Offset: 0x0006F9D0
		// (set) Token: 0x06001F3C RID: 7996 RVA: 0x000717D8 File Offset: 0x0006F9D8
		[DataSourceProperty]
		public string CaravansText
		{
			get
			{
				return this._caravansText;
			}
			set
			{
				if (value != this._caravansText)
				{
					this._caravansText = value;
					base.OnPropertyChangedWithValue<string>(value, "CaravansText");
				}
			}
		}

		// Token: 0x17000AB5 RID: 2741
		// (get) Token: 0x06001F3D RID: 7997 RVA: 0x000717FB File Offset: 0x0006F9FB
		// (set) Token: 0x06001F3E RID: 7998 RVA: 0x00071803 File Offset: 0x0006FA03
		[DataSourceProperty]
		public string GarrisonsText
		{
			get
			{
				return this._garrisonsText;
			}
			set
			{
				if (value != this._garrisonsText)
				{
					this._garrisonsText = value;
					base.OnPropertyChangedWithValue<string>(value, "GarrisonsText");
				}
			}
		}

		// Token: 0x17000AB6 RID: 2742
		// (get) Token: 0x06001F3F RID: 7999 RVA: 0x00071826 File Offset: 0x0006FA26
		// (set) Token: 0x06001F40 RID: 8000 RVA: 0x0007182E File Offset: 0x0006FA2E
		[DataSourceProperty]
		public string PartiesText
		{
			get
			{
				return this._partiesText;
			}
			set
			{
				if (value != this._partiesText)
				{
					this._partiesText = value;
					base.OnPropertyChangedWithValue<string>(value, "PartiesText");
				}
			}
		}

		// Token: 0x17000AB7 RID: 2743
		// (get) Token: 0x06001F41 RID: 8001 RVA: 0x00071851 File Offset: 0x0006FA51
		// (set) Token: 0x06001F42 RID: 8002 RVA: 0x00071859 File Offset: 0x0006FA59
		[DataSourceProperty]
		public string MoraleText
		{
			get
			{
				return this._moraleText;
			}
			set
			{
				if (value != this._moraleText)
				{
					this._moraleText = value;
					base.OnPropertyChangedWithValue<string>(value, "MoraleText");
				}
			}
		}

		// Token: 0x17000AB8 RID: 2744
		// (get) Token: 0x06001F43 RID: 8003 RVA: 0x0007187C File Offset: 0x0006FA7C
		// (set) Token: 0x06001F44 RID: 8004 RVA: 0x00071884 File Offset: 0x0006FA84
		[DataSourceProperty]
		public string LocationText
		{
			get
			{
				return this._locationText;
			}
			set
			{
				if (value != this._locationText)
				{
					this._locationText = value;
					base.OnPropertyChangedWithValue<string>(value, "LocationText");
				}
			}
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06001F45 RID: 8005 RVA: 0x000718A7 File Offset: 0x0006FAA7
		// (set) Token: 0x06001F46 RID: 8006 RVA: 0x000718AF File Offset: 0x0006FAAF
		[DataSourceProperty]
		public string CreateNewPartyText
		{
			get
			{
				return this._createNewPartyText;
			}
			set
			{
				if (value != this._createNewPartyText)
				{
					this._createNewPartyText = value;
					base.OnPropertyChangedWithValue<string>(value, "CreateNewPartyText");
				}
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06001F47 RID: 8007 RVA: 0x000718D2 File Offset: 0x0006FAD2
		// (set) Token: 0x06001F48 RID: 8008 RVA: 0x000718DA File Offset: 0x0006FADA
		[DataSourceProperty]
		public string SizeText
		{
			get
			{
				return this._sizeText;
			}
			set
			{
				if (value != this._sizeText)
				{
					this._sizeText = value;
					base.OnPropertyChangedWithValue<string>(value, "SizeText");
				}
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06001F49 RID: 8009 RVA: 0x000718FD File Offset: 0x0006FAFD
		// (set) Token: 0x06001F4A RID: 8010 RVA: 0x00071905 File Offset: 0x0006FB05
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

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06001F4B RID: 8011 RVA: 0x00071923 File Offset: 0x0006FB23
		// (set) Token: 0x06001F4C RID: 8012 RVA: 0x0007192B File Offset: 0x0006FB2B
		[DataSourceProperty]
		public bool CanCreateNewParty
		{
			get
			{
				return this._canCreateNewParty;
			}
			set
			{
				if (value != this._canCreateNewParty)
				{
					this._canCreateNewParty = value;
					base.OnPropertyChangedWithValue(value, "CanCreateNewParty");
				}
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06001F4D RID: 8013 RVA: 0x00071949 File Offset: 0x0006FB49
		// (set) Token: 0x06001F4E RID: 8014 RVA: 0x00071951 File Offset: 0x0006FB51
		[DataSourceProperty]
		public MBBindingList<ClanPartyItemVM> Parties
		{
			get
			{
				return this._parties;
			}
			set
			{
				if (value != this._parties)
				{
					this._parties = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanPartyItemVM>>(value, "Parties");
				}
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06001F4F RID: 8015 RVA: 0x0007196F File Offset: 0x0006FB6F
		// (set) Token: 0x06001F50 RID: 8016 RVA: 0x00071977 File Offset: 0x0006FB77
		[DataSourceProperty]
		public MBBindingList<ClanPartyItemVM> Caravans
		{
			get
			{
				return this._caravans;
			}
			set
			{
				if (value != this._caravans)
				{
					this._caravans = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanPartyItemVM>>(value, "Caravans");
				}
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06001F51 RID: 8017 RVA: 0x00071995 File Offset: 0x0006FB95
		// (set) Token: 0x06001F52 RID: 8018 RVA: 0x0007199D File Offset: 0x0006FB9D
		[DataSourceProperty]
		public MBBindingList<ClanPartyItemVM> Garrisons
		{
			get
			{
				return this._garrisons;
			}
			set
			{
				if (value != this._garrisons)
				{
					this._garrisons = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanPartyItemVM>>(value, "Garrisons");
				}
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06001F53 RID: 8019 RVA: 0x000719BB File Offset: 0x0006FBBB
		// (set) Token: 0x06001F54 RID: 8020 RVA: 0x000719C3 File Offset: 0x0006FBC3
		[DataSourceProperty]
		public ClanPartyItemVM CurrentSelectedParty
		{
			get
			{
				return this._currentSelectedParty;
			}
			set
			{
				if (value != this._currentSelectedParty)
				{
					this._currentSelectedParty = value;
					base.OnPropertyChangedWithValue<ClanPartyItemVM>(value, "CurrentSelectedParty");
					this.IsAnyValidPartySelected = value != null;
				}
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06001F55 RID: 8021 RVA: 0x000719EB File Offset: 0x0006FBEB
		// (set) Token: 0x06001F56 RID: 8022 RVA: 0x000719F3 File Offset: 0x0006FBF3
		[DataSourceProperty]
		public ClanPartiesSortControllerVM SortController
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
					base.OnPropertyChangedWithValue<ClanPartiesSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x04000E42 RID: 3650
		private Action _onExpenseChange;

		// Token: 0x04000E43 RID: 3651
		private Action<Hero> _openPartyAsManage;

		// Token: 0x04000E44 RID: 3652
		private Action<ClanCardSelectionInfo> _openCardSelectionPopup;

		// Token: 0x04000E45 RID: 3653
		private PartyRole _currentSelectedRole;

		// Token: 0x04000E46 RID: 3654
		private readonly IDisbandPartyCampaignBehavior _disbandBehavior;

		// Token: 0x04000E47 RID: 3655
		private readonly ITeleportationCampaignBehavior _teleportationBehavior;

		// Token: 0x04000E48 RID: 3656
		private readonly IEmptyClanPartiesCampaignBehavior _emptyClanPartiesCampaignBehavior;

		// Token: 0x04000E49 RID: 3657
		private readonly Action _onRefresh;

		// Token: 0x04000E4A RID: 3658
		private readonly Clan _faction;

		// Token: 0x04000E4B RID: 3659
		private readonly IEnumerable<SkillObject> _leaderAssignmentRelevantSkills = new List<SkillObject>
		{
			DefaultSkills.Engineering,
			DefaultSkills.Steward,
			DefaultSkills.Scouting,
			DefaultSkills.Medicine
		};

		// Token: 0x04000E4C RID: 3660
		private MBBindingList<ClanPartyItemVM> _parties;

		// Token: 0x04000E4D RID: 3661
		private MBBindingList<ClanPartyItemVM> _garrisons;

		// Token: 0x04000E4E RID: 3662
		private MBBindingList<ClanPartyItemVM> _caravans;

		// Token: 0x04000E4F RID: 3663
		private ClanPartyItemVM _currentSelectedParty;

		// Token: 0x04000E50 RID: 3664
		private HintViewModel _createNewPartyActionHint;

		// Token: 0x04000E51 RID: 3665
		private bool _canCreateNewParty;

		// Token: 0x04000E52 RID: 3666
		private bool _isSelected;

		// Token: 0x04000E53 RID: 3667
		private string _nameText;

		// Token: 0x04000E54 RID: 3668
		private string _moraleText;

		// Token: 0x04000E55 RID: 3669
		private string _locationText;

		// Token: 0x04000E56 RID: 3670
		private string _sizeText;

		// Token: 0x04000E57 RID: 3671
		private string _createNewPartyText;

		// Token: 0x04000E58 RID: 3672
		private string _partiesText;

		// Token: 0x04000E59 RID: 3673
		private string _caravansText;

		// Token: 0x04000E5A RID: 3674
		private string _garrisonsText;

		// Token: 0x04000E5B RID: 3675
		private bool _isAnyValidPartySelected;

		// Token: 0x04000E5C RID: 3676
		private ClanPartiesSortControllerVM _sortController;

		// Token: 0x020002C4 RID: 708
		private class ClanRoleMemberComparer : IComparer<ClanPartyMemberItemVM>
		{
			// Token: 0x060027BC RID: 10172 RVA: 0x000858B4 File Offset: 0x00083AB4
			public ClanRoleMemberComparer(PartyRole role)
			{
				this._role = role;
			}

			// Token: 0x060027BD RID: 10173 RVA: 0x000858C4 File Offset: 0x00083AC4
			public int Compare(ClanPartyMemberItemVM x, ClanPartyMemberItemVM y)
			{
				int num = SkillHelper.GetHeroRelevantSkillValueForPartyRole(y.HeroObject, this._role).CompareTo(SkillHelper.GetHeroRelevantSkillValueForPartyRole(x.HeroObject, this._role));
				if (num == 0)
				{
					return x.HeroObject.Name.ToString().CompareTo(y.HeroObject.Name.ToString());
				}
				return num;
			}

			// Token: 0x040013A5 RID: 5029
			private readonly PartyRole _role;
		}
	}
}
