using System;
using System.Linq;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes
{
	// Token: 0x02000082 RID: 130
	public class ExpelClanDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000A6F RID: 2671 RVA: 0x0002D684 File Offset: 0x0002B884
		public ExpelClanFromKingdomDecision ExpelDecision
		{
			get
			{
				ExpelClanFromKingdomDecision expelClanFromKingdomDecision;
				if ((expelClanFromKingdomDecision = this._expelDecision) == null)
				{
					expelClanFromKingdomDecision = (this._expelDecision = this._decision as ExpelClanFromKingdomDecision);
				}
				return expelClanFromKingdomDecision;
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000A70 RID: 2672 RVA: 0x0002D6AF File Offset: 0x0002B8AF
		public Clan Clan
		{
			get
			{
				return this.ExpelDecision.ClanToExpel;
			}
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x0002D6BC File Offset: 0x0002B8BC
		public ExpelClanDecisionItemVM(ExpelClanFromKingdomDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			base.DecisionType = 2;
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x0002D6D0 File Offset: 0x0002B8D0
		protected override void InitValues()
		{
			base.InitValues();
			base.DecisionType = 2;
			this.Members = new MBBindingList<HeroVM>();
			this.Fiefs = new MBBindingList<EncyclopediaSettlementVM>();
			GameTexts.SetVariable("RENOWN", this.Clan.Renown);
			string text = "STR1";
			TextObject encyclopediaText = this.Clan.EncyclopediaText;
			GameTexts.SetVariable(text, (encyclopediaText != null) ? encyclopediaText.ToString() : null);
			GameTexts.SetVariable("STR2", GameTexts.FindText("str_encyclopedia_renown", null).ToString());
			this.InformationText = GameTexts.FindText("str_STR1_space_STR2", null).ToString();
			this.Leader = new HeroVM(this.Clan.Leader, false);
			this.LeaderText = GameTexts.FindText("str_leader", null).ToString();
			this.MembersText = GameTexts.FindText("str_members", null).ToString();
			this.SettlementsText = GameTexts.FindText("str_fiefs", null).ToString();
			this.NameText = this.Clan.Name.ToString();
			int num = 0;
			float num2 = 0f;
			EncyclopediaPage pageOf = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(Hero));
			foreach (Hero hero in this.Clan.Heroes)
			{
				if (hero.IsAlive && hero.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && pageOf.IsValidEncyclopediaItem(hero))
				{
					if (hero != this.Leader.Hero)
					{
						this.Members.Add(new HeroVM(hero, false));
					}
					num += hero.Gold;
				}
			}
			foreach (Hero hero2 in this.Clan.Companions)
			{
				if (hero2.IsAlive && hero2.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && pageOf.IsValidEncyclopediaItem(hero2))
				{
					if (hero2 != this.Leader.Hero)
					{
						this.Members.Add(new HeroVM(hero2, false));
					}
					num += hero2.Gold;
				}
			}
			foreach (MobileParty mobileParty in MobileParty.AllLordParties)
			{
				if (mobileParty.ActualClan == this.Clan && !mobileParty.IsDisbanding)
				{
					num2 += mobileParty.Party.CalculateCurrentStrength();
				}
			}
			this.ProsperityText = num.ToString();
			this.ProsperityHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetClanProsperityTooltip(this.Clan));
			this.StrengthText = num2.ToString();
			this.StrengthHint = new BasicTooltipViewModel(() => CampaignUIHelper.GetClanStrengthTooltip(this.Clan));
			foreach (Town town in from s in this.Clan.Fiefs
				orderby s.IsCastle, s.IsTown
				select s)
			{
				if (town.Settlement.OwnerClan == this.Clan)
				{
					this.Fiefs.Add(new EncyclopediaSettlementVM(town.Settlement));
				}
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000A73 RID: 2675 RVA: 0x0002DA98 File Offset: 0x0002BC98
		// (set) Token: 0x06000A74 RID: 2676 RVA: 0x0002DAA0 File Offset: 0x0002BCA0
		[DataSourceProperty]
		public MBBindingList<HeroVM> Members
		{
			get
			{
				return this._members;
			}
			set
			{
				if (value != this._members)
				{
					this._members = value;
					base.OnPropertyChangedWithValue<MBBindingList<HeroVM>>(value, "Members");
				}
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000A75 RID: 2677 RVA: 0x0002DABE File Offset: 0x0002BCBE
		// (set) Token: 0x06000A76 RID: 2678 RVA: 0x0002DAC6 File Offset: 0x0002BCC6
		[DataSourceProperty]
		public MBBindingList<EncyclopediaSettlementVM> Fiefs
		{
			get
			{
				return this._fiefs;
			}
			set
			{
				if (value != this._fiefs)
				{
					this._fiefs = value;
					base.OnPropertyChangedWithValue<MBBindingList<EncyclopediaSettlementVM>>(value, "Fiefs");
				}
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000A77 RID: 2679 RVA: 0x0002DAE4 File Offset: 0x0002BCE4
		// (set) Token: 0x06000A78 RID: 2680 RVA: 0x0002DAEC File Offset: 0x0002BCEC
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

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x0002DB0A File Offset: 0x0002BD0A
		// (set) Token: 0x06000A7A RID: 2682 RVA: 0x0002DB12 File Offset: 0x0002BD12
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

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0002DB35 File Offset: 0x0002BD35
		// (set) Token: 0x06000A7C RID: 2684 RVA: 0x0002DB3D File Offset: 0x0002BD3D
		[DataSourceProperty]
		public string MembersText
		{
			get
			{
				return this._membersText;
			}
			set
			{
				if (value != this._membersText)
				{
					this._membersText = value;
					base.OnPropertyChangedWithValue<string>(value, "MembersText");
				}
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x0002DB60 File Offset: 0x0002BD60
		// (set) Token: 0x06000A7E RID: 2686 RVA: 0x0002DB68 File Offset: 0x0002BD68
		[DataSourceProperty]
		public string SettlementsText
		{
			get
			{
				return this._settlementsText;
			}
			set
			{
				if (value != this._settlementsText)
				{
					this._settlementsText = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementsText");
				}
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x0002DB8B File Offset: 0x0002BD8B
		// (set) Token: 0x06000A80 RID: 2688 RVA: 0x0002DB93 File Offset: 0x0002BD93
		[DataSourceProperty]
		public string InformationText
		{
			get
			{
				return this._informationText;
			}
			set
			{
				if (value != this._informationText)
				{
					this._informationText = value;
					base.OnPropertyChangedWithValue<string>(value, "InformationText");
				}
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x0002DBB6 File Offset: 0x0002BDB6
		// (set) Token: 0x06000A82 RID: 2690 RVA: 0x0002DBBE File Offset: 0x0002BDBE
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

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x0002DBE1 File Offset: 0x0002BDE1
		// (set) Token: 0x06000A84 RID: 2692 RVA: 0x0002DBE9 File Offset: 0x0002BDE9
		[DataSourceProperty]
		public string ProsperityText
		{
			get
			{
				return this._prosperityText;
			}
			set
			{
				if (value != this._prosperityText)
				{
					this._prosperityText = value;
					base.OnPropertyChangedWithValue<string>(value, "ProsperityText");
				}
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x0002DC0C File Offset: 0x0002BE0C
		// (set) Token: 0x06000A86 RID: 2694 RVA: 0x0002DC14 File Offset: 0x0002BE14
		[DataSourceProperty]
		public string StrengthText
		{
			get
			{
				return this._strengthText;
			}
			set
			{
				if (value != this._strengthText)
				{
					this._strengthText = value;
					base.OnPropertyChangedWithValue<string>(value, "StrengthText");
				}
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x0002DC37 File Offset: 0x0002BE37
		// (set) Token: 0x06000A88 RID: 2696 RVA: 0x0002DC3F File Offset: 0x0002BE3F
		[DataSourceProperty]
		public BasicTooltipViewModel ProsperityHint
		{
			get
			{
				return this._prosperityHint;
			}
			set
			{
				if (value != this._prosperityHint)
				{
					this._prosperityHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "ProsperityHint");
				}
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x0002DC5D File Offset: 0x0002BE5D
		// (set) Token: 0x06000A8A RID: 2698 RVA: 0x0002DC65 File Offset: 0x0002BE65
		[DataSourceProperty]
		public BasicTooltipViewModel StrengthHint
		{
			get
			{
				return this._strengthHint;
			}
			set
			{
				if (value != this._strengthHint)
				{
					this._strengthHint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "StrengthHint");
				}
			}
		}

		// Token: 0x04000498 RID: 1176
		private ExpelClanFromKingdomDecision _expelDecision;

		// Token: 0x04000499 RID: 1177
		private MBBindingList<HeroVM> _members;

		// Token: 0x0400049A RID: 1178
		private MBBindingList<EncyclopediaSettlementVM> _fiefs;

		// Token: 0x0400049B RID: 1179
		private HeroVM _leader;

		// Token: 0x0400049C RID: 1180
		private string _nameText;

		// Token: 0x0400049D RID: 1181
		private string _membersText;

		// Token: 0x0400049E RID: 1182
		private string _settlementsText;

		// Token: 0x0400049F RID: 1183
		private string _leaderText;

		// Token: 0x040004A0 RID: 1184
		private string _informationText;

		// Token: 0x040004A1 RID: 1185
		private string _prosperityText;

		// Token: 0x040004A2 RID: 1186
		private string _strengthText;

		// Token: 0x040004A3 RID: 1187
		private BasicTooltipViewModel _prosperityHint;

		// Token: 0x040004A4 RID: 1188
		private BasicTooltipViewModel _strengthHint;
	}
}
