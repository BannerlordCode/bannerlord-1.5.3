using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.Conversation;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements.Locations;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.ClanManagement.Categories
{
	// Token: 0x02000142 RID: 322
	public class ClanMembersVM : ViewModel
	{
		// Token: 0x06001ED1 RID: 7889 RVA: 0x0006F1D8 File Offset: 0x0006D3D8
		public ClanMembersVM(Action onRefresh, Action<Hero> showHeroOnMap)
		{
			this._onRefresh = onRefresh;
			this._faction = Hero.MainHero.Clan;
			this._showHeroOnMap = showHeroOnMap;
			this._teleportationBehavior = Campaign.Current.GetCampaignBehavior<ITeleportationCampaignBehavior>();
			this.Family = new MBBindingList<ClanLordItemVM>();
			this.Companions = new MBBindingList<ClanLordItemVM>();
			MBBindingList<MBBindingList<ClanLordItemVM>> mbbindingList = new MBBindingList<MBBindingList<ClanLordItemVM>> { this.Family, this.Companions };
			this.SortController = new ClanMembersSortControllerVM(mbbindingList);
			this.RefreshMembersList();
			this.RefreshValues();
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x0006F268 File Offset: 0x0006D468
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TraitsText = GameTexts.FindText("str_traits_group", null).ToString();
			this.SkillsText = GameTexts.FindText("str_skills", null).ToString();
			this.NameText = GameTexts.FindText("str_sort_by_name_label", null).ToString();
			this.LocationText = GameTexts.FindText("str_tooltip_label_location", null).ToString();
			this.Family.ApplyActionOnAllItems(delegate(ClanLordItemVM x)
			{
				x.RefreshValues();
			});
			this.Companions.ApplyActionOnAllItems(delegate(ClanLordItemVM x)
			{
				x.RefreshValues();
			});
			this.SortController.RefreshValues();
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x0006F334 File Offset: 0x0006D534
		public void RefreshMembersList()
		{
			this.Family.Clear();
			this.Companions.Clear();
			this.SortController.ResetAllStates();
			List<Hero> list = new List<Hero>();
			foreach (Hero hero in this._faction.AliveLords)
			{
				if (!hero.IsDisabled)
				{
					if (hero == Hero.MainHero)
					{
						list.Insert(0, hero);
					}
					else
					{
						list.Add(hero);
					}
				}
			}
			IEnumerable<Hero> enumerable = this._faction.Companions.Where<Hero>((Hero m) => m.IsPlayerCompanion);
			foreach (Hero hero2 in list)
			{
				this.Family.Add(new ClanLordItemVM(hero2, this._teleportationBehavior, this._showHeroOnMap, new Action<ClanLordItemVM>(this.OnMemberSelection), new Action(this.OnRequestRecall), new Action(this.OnTalkWithMember)));
			}
			foreach (Hero hero3 in enumerable)
			{
				this.Companions.Add(new ClanLordItemVM(hero3, this._teleportationBehavior, this._showHeroOnMap, new Action<ClanLordItemVM>(this.OnMemberSelection), new Action(this.OnRequestRecall), new Action(this.OnTalkWithMember)));
			}
			GameTexts.SetVariable("RANK", GameTexts.FindText("str_family_group", null));
			GameTexts.SetVariable("NUMBER", this.Family.Count);
			this.FamilyText = GameTexts.FindText("str_RANK_with_NUM_between_parenthesis", null).ToString();
			GameTexts.SetVariable("STR1", GameTexts.FindText("str_companions_group", null));
			GameTexts.SetVariable("LEFT", this._faction.Companions.Count);
			GameTexts.SetVariable("RIGHT", this._faction.CompanionLimit);
			GameTexts.SetVariable("STR2", GameTexts.FindText("str_LEFT_over_RIGHT_in_paranthesis", null));
			this.CompanionsText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			this.OnMemberSelection(this.GetDefaultMember());
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x0006F5AC File Offset: 0x0006D7AC
		private ClanLordItemVM GetDefaultMember()
		{
			if (this.Family.Count > 0)
			{
				return this.Family[0];
			}
			if (this.Companions.Count <= 0)
			{
				return null;
			}
			return this.Companions[0];
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x0006F5E8 File Offset: 0x0006D7E8
		public void SelectMember(Hero hero)
		{
			bool flag = false;
			foreach (ClanLordItemVM clanLordItemVM in this.Family)
			{
				if (clanLordItemVM.GetHero() == hero)
				{
					this.OnMemberSelection(clanLordItemVM);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				foreach (ClanLordItemVM clanLordItemVM2 in this.Companions)
				{
					if (clanLordItemVM2.GetHero() == hero)
					{
						this.OnMemberSelection(clanLordItemVM2);
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				foreach (ClanLordItemVM clanLordItemVM3 in this.Family)
				{
					if (clanLordItemVM3.GetHero() == Hero.MainHero)
					{
						this.OnMemberSelection(clanLordItemVM3);
						flag = true;
						break;
					}
				}
			}
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x0006F6E4 File Offset: 0x0006D8E4
		private void OnMemberSelection(ClanLordItemVM member)
		{
			if (this.CurrentSelectedMember != null)
			{
				this.CurrentSelectedMember.IsSelected = false;
			}
			this.CurrentSelectedMember = member;
			if (member != null)
			{
				member.IsSelected = true;
			}
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x0006F70C File Offset: 0x0006D90C
		private void OnRequestRecall()
		{
			ClanLordItemVM currentSelectedMember = this.CurrentSelectedMember;
			Hero hero = ((currentSelectedMember != null) ? currentSelectedMember.GetHero() : null);
			if (hero != null)
			{
				int num = (int)Math.Ceiling((double)Campaign.Current.Models.DelayedTeleportationModel.GetTeleportationDelayAsHours(hero, PartyBase.MainParty).ResultNumber);
				MBTextManager.SetTextVariable("TRAVEL_DURATION", CampaignUIHelper.GetHoursAndDaysTextFromHourValue(num).ToString(), false);
				MBTextManager.SetTextVariable("HERO_NAME", hero.Name.ToString(), false);
				object obj = GameTexts.FindText("str_recall_member", null);
				TextObject textObject = GameTexts.FindText("str_recall_clan_member_inquiry", null);
				InformationManager.ShowInquiry(new InquiryData(obj.ToString(), textObject.ToString(), true, true, GameTexts.FindText("str_yes", null).ToString(), GameTexts.FindText("str_no", null).ToString(), new Action(this.OnConfirmRecall), null, "", 0f, null, null, null), false, false);
			}
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x0006F7F2 File Offset: 0x0006D9F2
		private void OnConfirmRecall()
		{
			TeleportHeroAction.ApplyDelayedTeleportToParty(this.CurrentSelectedMember.GetHero(), MobileParty.MainParty);
			Action onRefresh = this._onRefresh;
			if (onRefresh == null)
			{
				return;
			}
			onRefresh();
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x0006F819 File Offset: 0x0006DA19
		private void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x0006F82C File Offset: 0x0006DA2C
		private void OnTalkWithMember()
		{
			ClanLordItemVM currentSelectedMember = this.CurrentSelectedMember;
			bool flag;
			if (currentSelectedMember == null)
			{
				flag = null != null;
			}
			else
			{
				Hero hero = currentSelectedMember.GetHero();
				flag = ((hero != null) ? hero.CharacterObject : null) != null;
			}
			if (flag)
			{
				CharacterObject characterObject = this.CurrentSelectedMember.GetHero().CharacterObject;
				LocationComplex locationComplex = LocationComplex.Current;
				if (((locationComplex != null) ? locationComplex.GetLocationOfCharacter(LocationComplex.Current.GetFirstLocationCharacterOfCharacter(characterObject)) : null) == null)
				{
					CampaignMission.OpenConversationMission(new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false), new ConversationCharacterData(characterObject, PartyBase.MainParty, false, false, false, false, false, false), "", "", false);
					return;
				}
				Game.Current.GameStateManager.PopState(0);
				CampaignMapConversation.OpenConversation(new ConversationCharacterData(CharacterObject.PlayerCharacter, PartyBase.MainParty, false, false, false, false, false, false), new ConversationCharacterData(characterObject, PartyBase.MainParty, false, false, false, false, false, false));
			}
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x0006F900 File Offset: 0x0006DB00
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.Family.ApplyActionOnAllItems(delegate(ClanLordItemVM f)
			{
				f.OnFinalize();
			});
			this.Companions.ApplyActionOnAllItems(delegate(ClanLordItemVM f)
			{
				f.OnFinalize();
			});
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06001EDC RID: 7900 RVA: 0x0006F967 File Offset: 0x0006DB67
		// (set) Token: 0x06001EDD RID: 7901 RVA: 0x0006F96F File Offset: 0x0006DB6F
		[DataSourceProperty]
		public bool IsAnyValidMemberSelected
		{
			get
			{
				return this._isAnyValidMemberSelected;
			}
			set
			{
				if (value != this._isAnyValidMemberSelected)
				{
					this._isAnyValidMemberSelected = value;
					base.OnPropertyChangedWithValue(value, "IsAnyValidMemberSelected");
				}
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06001EDE RID: 7902 RVA: 0x0006F98D File Offset: 0x0006DB8D
		// (set) Token: 0x06001EDF RID: 7903 RVA: 0x0006F995 File Offset: 0x0006DB95
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

		// Token: 0x17000A99 RID: 2713
		// (get) Token: 0x06001EE0 RID: 7904 RVA: 0x0006F9B3 File Offset: 0x0006DBB3
		// (set) Token: 0x06001EE1 RID: 7905 RVA: 0x0006F9BB File Offset: 0x0006DBBB
		[DataSourceProperty]
		public string FamilyText
		{
			get
			{
				return this._familyText;
			}
			set
			{
				if (value != this._familyText)
				{
					this._familyText = value;
					base.OnPropertyChangedWithValue<string>(value, "FamilyText");
				}
			}
		}

		// Token: 0x17000A9A RID: 2714
		// (get) Token: 0x06001EE2 RID: 7906 RVA: 0x0006F9DE File Offset: 0x0006DBDE
		// (set) Token: 0x06001EE3 RID: 7907 RVA: 0x0006F9E6 File Offset: 0x0006DBE6
		[DataSourceProperty]
		public string TraitsText
		{
			get
			{
				return this._traitsText;
			}
			set
			{
				if (value != this._traitsText)
				{
					this._traitsText = value;
					base.OnPropertyChangedWithValue<string>(value, "TraitsText");
				}
			}
		}

		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06001EE4 RID: 7908 RVA: 0x0006FA09 File Offset: 0x0006DC09
		// (set) Token: 0x06001EE5 RID: 7909 RVA: 0x0006FA11 File Offset: 0x0006DC11
		[DataSourceProperty]
		public string SkillsText
		{
			get
			{
				return this._skillsText;
			}
			set
			{
				if (value != this._skillsText)
				{
					this._skillsText = value;
					base.OnPropertyChangedWithValue<string>(value, "SkillsText");
				}
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06001EE6 RID: 7910 RVA: 0x0006FA34 File Offset: 0x0006DC34
		// (set) Token: 0x06001EE7 RID: 7911 RVA: 0x0006FA3C File Offset: 0x0006DC3C
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

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06001EE8 RID: 7912 RVA: 0x0006FA5F File Offset: 0x0006DC5F
		// (set) Token: 0x06001EE9 RID: 7913 RVA: 0x0006FA67 File Offset: 0x0006DC67
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

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06001EEA RID: 7914 RVA: 0x0006FA8A File Offset: 0x0006DC8A
		// (set) Token: 0x06001EEB RID: 7915 RVA: 0x0006FA92 File Offset: 0x0006DC92
		[DataSourceProperty]
		public string CompanionsText
		{
			get
			{
				return this._companionsText;
			}
			set
			{
				if (value != this._companionsText)
				{
					this._companionsText = value;
					base.OnPropertyChangedWithValue<string>(value, "CompanionsText");
				}
			}
		}

		// Token: 0x17000A9F RID: 2719
		// (get) Token: 0x06001EEC RID: 7916 RVA: 0x0006FAB5 File Offset: 0x0006DCB5
		// (set) Token: 0x06001EED RID: 7917 RVA: 0x0006FABD File Offset: 0x0006DCBD
		[DataSourceProperty]
		public MBBindingList<ClanLordItemVM> Companions
		{
			get
			{
				return this._companions;
			}
			set
			{
				if (value != this._companions)
				{
					this._companions = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanLordItemVM>>(value, "Companions");
				}
			}
		}

		// Token: 0x17000AA0 RID: 2720
		// (get) Token: 0x06001EEE RID: 7918 RVA: 0x0006FADB File Offset: 0x0006DCDB
		// (set) Token: 0x06001EEF RID: 7919 RVA: 0x0006FAE3 File Offset: 0x0006DCE3
		[DataSourceProperty]
		public MBBindingList<ClanLordItemVM> Family
		{
			get
			{
				return this._family;
			}
			set
			{
				if (value != this._family)
				{
					this._family = value;
					base.OnPropertyChangedWithValue<MBBindingList<ClanLordItemVM>>(value, "Family");
				}
			}
		}

		// Token: 0x17000AA1 RID: 2721
		// (get) Token: 0x06001EF0 RID: 7920 RVA: 0x0006FB01 File Offset: 0x0006DD01
		// (set) Token: 0x06001EF1 RID: 7921 RVA: 0x0006FB09 File Offset: 0x0006DD09
		[DataSourceProperty]
		public ClanLordItemVM CurrentSelectedMember
		{
			get
			{
				return this._currentSelectedMember;
			}
			set
			{
				if (value != this._currentSelectedMember)
				{
					this._currentSelectedMember = value;
					base.OnPropertyChangedWithValue<ClanLordItemVM>(value, "CurrentSelectedMember");
					this.IsAnyValidMemberSelected = value != null;
				}
			}
		}

		// Token: 0x17000AA2 RID: 2722
		// (get) Token: 0x06001EF2 RID: 7922 RVA: 0x0006FB31 File Offset: 0x0006DD31
		// (set) Token: 0x06001EF3 RID: 7923 RVA: 0x0006FB39 File Offset: 0x0006DD39
		[DataSourceProperty]
		public ClanMembersSortControllerVM SortController
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
					base.OnPropertyChangedWithValue<ClanMembersSortControllerVM>(value, "SortController");
				}
			}
		}

		// Token: 0x04000E1F RID: 3615
		private readonly Clan _faction;

		// Token: 0x04000E20 RID: 3616
		private readonly Action _onRefresh;

		// Token: 0x04000E21 RID: 3617
		private readonly Action<Hero> _showHeroOnMap;

		// Token: 0x04000E22 RID: 3618
		private readonly ITeleportationCampaignBehavior _teleportationBehavior;

		// Token: 0x04000E23 RID: 3619
		private bool _isSelected;

		// Token: 0x04000E24 RID: 3620
		private MBBindingList<ClanLordItemVM> _companions;

		// Token: 0x04000E25 RID: 3621
		private MBBindingList<ClanLordItemVM> _family;

		// Token: 0x04000E26 RID: 3622
		private ClanLordItemVM _currentSelectedMember;

		// Token: 0x04000E27 RID: 3623
		private string _familyText;

		// Token: 0x04000E28 RID: 3624
		private string _traitsText;

		// Token: 0x04000E29 RID: 3625
		private string _companionsText;

		// Token: 0x04000E2A RID: 3626
		private string _skillsText;

		// Token: 0x04000E2B RID: 3627
		private string _nameText;

		// Token: 0x04000E2C RID: 3628
		private string _locationText;

		// Token: 0x04000E2D RID: 3629
		private bool _isAnyValidMemberSelected;

		// Token: 0x04000E2E RID: 3630
		private ClanMembersSortControllerVM _sortController;
	}
}
