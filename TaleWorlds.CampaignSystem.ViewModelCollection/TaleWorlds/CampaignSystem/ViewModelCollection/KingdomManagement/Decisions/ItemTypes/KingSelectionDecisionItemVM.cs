using System;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Decisions.ItemTypes
{
	// Token: 0x02000084 RID: 132
	public class KingSelectionDecisionItemVM : DecisionItemBaseVM
	{
		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0002DCA5 File Offset: 0x0002BEA5
		public IFaction TargetFaction
		{
			get
			{
				return (this._decision as KingSelectionKingdomDecision).Kingdom;
			}
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0002DCB7 File Offset: 0x0002BEB7
		public KingSelectionDecisionItemVM(KingSelectionKingdomDecision decision, Action onDecisionOver)
			: base(decision, onDecisionOver)
		{
			this._kingSelectionDecision = decision;
			base.DecisionType = 6;
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0002DCD0 File Offset: 0x0002BED0
		protected override void InitValues()
		{
			base.InitValues();
			TextObject textObject = GameTexts.FindText("str_kingdom_decision_king_selection", null);
			textObject.SetTextVariable("FACTION", this.TargetFaction.Name);
			this.NameText = textObject.ToString();
			this.FactionBanner = new BannerImageIdentifierVM(this.TargetFaction.Banner, true);
			this.FactionName = this.TargetFaction.Culture.Name.ToString();
			bool flag = true;
			bool flag2 = true;
			int num = 0;
			int num2 = 0;
			foreach (Settlement settlement in this.TargetFaction.Settlements)
			{
				if (settlement.IsTown)
				{
					if (flag)
					{
						this.SettlementsListText = settlement.EncyclopediaLinkWithName.ToString();
						flag = false;
					}
					else
					{
						GameTexts.SetVariable("LEFT", this.SettlementsListText);
						GameTexts.SetVariable("RIGHT", settlement.EncyclopediaLinkWithName);
						this.SettlementsListText = GameTexts.FindText("str_LEFT_comma_RIGHT", null).ToString();
					}
					num++;
				}
				else if (settlement.IsCastle)
				{
					if (flag2)
					{
						this.CastlesListText = settlement.EncyclopediaLinkWithName.ToString();
						flag2 = false;
					}
					else
					{
						GameTexts.SetVariable("LEFT", this.CastlesListText);
						GameTexts.SetVariable("RIGHT", settlement.EncyclopediaLinkWithName);
						this.CastlesListText = GameTexts.FindText("str_LEFT_comma_RIGHT", null).ToString();
					}
					num2++;
				}
			}
			TextObject textObject2 = GameTexts.FindText("str_settlements", null);
			TextObject textObject3 = GameTexts.FindText("str_STR_in_parentheses", null);
			textObject3.SetTextVariable("STR", num);
			TextObject textObject4 = GameTexts.FindText("str_LEFT_RIGHT", null);
			textObject4.SetTextVariable("LEFT", textObject2);
			textObject4.SetTextVariable("RIGHT", textObject3);
			this.SettlementsText = textObject4.ToString();
			TextObject textObject5 = GameTexts.FindText("str_castles", null);
			TextObject textObject6 = GameTexts.FindText("str_STR_in_parentheses", null);
			textObject6.SetTextVariable("STR", num2);
			TextObject textObject7 = GameTexts.FindText("str_LEFT_RIGHT", null);
			textObject7.SetTextVariable("LEFT", textObject5);
			textObject7.SetTextVariable("RIGHT", textObject6);
			this.CastlesText = textObject7.ToString();
			this.TotalStrengthText = GameTexts.FindText("str_total_strength", null).ToString();
			this.TotalStrength = (int)this.TargetFaction.CurrentTotalStrength;
			this.ActivePoliciesText = GameTexts.FindText("str_active_policies", null).ToString();
			Kingdom kingdom = this.TargetFaction as Kingdom;
			foreach (PolicyObject policyObject in kingdom.ActivePolicies)
			{
				if (policyObject == kingdom.ActivePolicies[0])
				{
					this.ActivePoliciesListText = policyObject.Name.ToString();
				}
				else
				{
					GameTexts.SetVariable("LEFT", this.ActivePoliciesListText);
					GameTexts.SetVariable("RIGHT", policyObject.Name.ToString());
					this.ActivePoliciesListText = GameTexts.FindText("str_LEFT_comma_RIGHT", null).ToString();
				}
			}
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0002E000 File Offset: 0x0002C200
		private void ExecuteLocationLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x0002E012 File Offset: 0x0002C212
		// (set) Token: 0x06000A93 RID: 2707 RVA: 0x0002E01A File Offset: 0x0002C21A
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

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x0002E03D File Offset: 0x0002C23D
		// (set) Token: 0x06000A95 RID: 2709 RVA: 0x0002E045 File Offset: 0x0002C245
		[DataSourceProperty]
		public string FactionName
		{
			get
			{
				return this._factionName;
			}
			set
			{
				if (value != this._factionName)
				{
					this._factionName = value;
					base.OnPropertyChangedWithValue<string>(value, "FactionName");
				}
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x0002E068 File Offset: 0x0002C268
		// (set) Token: 0x06000A97 RID: 2711 RVA: 0x0002E070 File Offset: 0x0002C270
		[DataSourceProperty]
		public BannerImageIdentifierVM FactionBanner
		{
			get
			{
				return this._factionBanner;
			}
			set
			{
				if (value != this._factionBanner)
				{
					this._factionBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "FactionBanner");
				}
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x0002E08E File Offset: 0x0002C28E
		// (set) Token: 0x06000A99 RID: 2713 RVA: 0x0002E096 File Offset: 0x0002C296
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

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x0002E0B9 File Offset: 0x0002C2B9
		// (set) Token: 0x06000A9B RID: 2715 RVA: 0x0002E0C1 File Offset: 0x0002C2C1
		[DataSourceProperty]
		public string SettlementsListText
		{
			get
			{
				return this._settlementsListText;
			}
			set
			{
				if (value != this._settlementsListText)
				{
					this._settlementsListText = value;
					base.OnPropertyChangedWithValue<string>(value, "SettlementsListText");
				}
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000A9C RID: 2716 RVA: 0x0002E0E4 File Offset: 0x0002C2E4
		// (set) Token: 0x06000A9D RID: 2717 RVA: 0x0002E0EC File Offset: 0x0002C2EC
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

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000A9E RID: 2718 RVA: 0x0002E10F File Offset: 0x0002C30F
		// (set) Token: 0x06000A9F RID: 2719 RVA: 0x0002E117 File Offset: 0x0002C317
		[DataSourceProperty]
		public string CastlesListText
		{
			get
			{
				return this._castlesListText;
			}
			set
			{
				if (value != this._castlesListText)
				{
					this._castlesListText = value;
					base.OnPropertyChangedWithValue<string>(value, "CastlesListText");
				}
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x0002E13A File Offset: 0x0002C33A
		// (set) Token: 0x06000AA1 RID: 2721 RVA: 0x0002E142 File Offset: 0x0002C342
		[DataSourceProperty]
		public string TotalStrengthText
		{
			get
			{
				return this._totalStrengthText;
			}
			set
			{
				if (value != this._totalStrengthText)
				{
					this._totalStrengthText = value;
					base.OnPropertyChangedWithValue<string>(value, "TotalStrengthText");
				}
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x0002E165 File Offset: 0x0002C365
		// (set) Token: 0x06000AA3 RID: 2723 RVA: 0x0002E16D File Offset: 0x0002C36D
		[DataSourceProperty]
		public int TotalStrength
		{
			get
			{
				return this._totalStrength;
			}
			set
			{
				if (value != this._totalStrength)
				{
					this._totalStrength = value;
					base.OnPropertyChangedWithValue(value, "TotalStrength");
				}
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x0002E18B File Offset: 0x0002C38B
		// (set) Token: 0x06000AA5 RID: 2725 RVA: 0x0002E193 File Offset: 0x0002C393
		[DataSourceProperty]
		public string ActivePoliciesText
		{
			get
			{
				return this._activePoliciesText;
			}
			set
			{
				if (value != this._activePoliciesText)
				{
					this._activePoliciesText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActivePoliciesText");
				}
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x0002E1B6 File Offset: 0x0002C3B6
		// (set) Token: 0x06000AA7 RID: 2727 RVA: 0x0002E1BE File Offset: 0x0002C3BE
		[DataSourceProperty]
		public string ActivePoliciesListText
		{
			get
			{
				return this._activePoliciesListText;
			}
			set
			{
				if (value != this._activePoliciesListText)
				{
					this._activePoliciesListText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActivePoliciesListText");
				}
			}
		}

		// Token: 0x040004A5 RID: 1189
		private readonly KingSelectionKingdomDecision _kingSelectionDecision;

		// Token: 0x040004A6 RID: 1190
		private string _nameText;

		// Token: 0x040004A7 RID: 1191
		private string _factionName;

		// Token: 0x040004A8 RID: 1192
		private BannerImageIdentifierVM _factionBanner;

		// Token: 0x040004A9 RID: 1193
		private string _settlementsText;

		// Token: 0x040004AA RID: 1194
		private string _settlementsListText;

		// Token: 0x040004AB RID: 1195
		private string _castlesText;

		// Token: 0x040004AC RID: 1196
		private string _castlesListText;

		// Token: 0x040004AD RID: 1197
		private int _totalStrength;

		// Token: 0x040004AE RID: 1198
		private string _totalStrengthText;

		// Token: 0x040004AF RID: 1199
		private string _activePoliciesText;

		// Token: 0x040004B0 RID: 1200
		private string _activePoliciesListText;
	}
}
