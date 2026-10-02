using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Issues
{
	// Token: 0x02000396 RID: 918
	public class DefaultIssueEffects
	{
		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x0600362C RID: 13868 RVA: 0x000DE290 File Offset: 0x000DC490
		private static DefaultIssueEffects Instance
		{
			get
			{
				return Campaign.Current.DefaultIssueEffects;
			}
		}

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x0600362D RID: 13869 RVA: 0x000DE29C File Offset: 0x000DC49C
		public static IssueEffect SettlementLoyalty
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementLoyalty;
			}
		}

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x0600362E RID: 13870 RVA: 0x000DE2A8 File Offset: 0x000DC4A8
		public static IssueEffect SettlementSecurity
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementSecurity;
			}
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x0600362F RID: 13871 RVA: 0x000DE2B4 File Offset: 0x000DC4B4
		public static IssueEffect SettlementMilitia
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementMilitia;
			}
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x06003630 RID: 13872 RVA: 0x000DE2C0 File Offset: 0x000DC4C0
		public static IssueEffect SettlementProsperity
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementProsperity;
			}
		}

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x06003631 RID: 13873 RVA: 0x000DE2CC File Offset: 0x000DC4CC
		public static IssueEffect VillageHearth
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectVillageHearth;
			}
		}

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x06003632 RID: 13874 RVA: 0x000DE2D8 File Offset: 0x000DC4D8
		public static IssueEffect SettlementFood
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementFood;
			}
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x06003633 RID: 13875 RVA: 0x000DE2E4 File Offset: 0x000DC4E4
		public static IssueEffect SettlementTax
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementTax;
			}
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x06003634 RID: 13876 RVA: 0x000DE2F0 File Offset: 0x000DC4F0
		public static IssueEffect SettlementGarrison
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectSettlementGarrison;
			}
		}

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x06003635 RID: 13877 RVA: 0x000DE2FC File Offset: 0x000DC4FC
		public static IssueEffect HalfVillageProduction
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectHalfVillageProduction;
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x06003636 RID: 13878 RVA: 0x000DE308 File Offset: 0x000DC508
		public static IssueEffect IssueOwnerPower
		{
			get
			{
				return DefaultIssueEffects.Instance._issueEffectIssueOwnerPower;
			}
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06003637 RID: 13879 RVA: 0x000DE314 File Offset: 0x000DC514
		public static IssueEffect ClanInfluence
		{
			get
			{
				return DefaultIssueEffects.Instance._clanInfluence;
			}
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x000DE320 File Offset: 0x000DC520
		public DefaultIssueEffects()
		{
			this.RegisterAll();
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x000DE330 File Offset: 0x000DC530
		private void RegisterAll()
		{
			this._issueEffectSettlementLoyalty = this.Create("issue_effect_settlement_loyalty");
			this._issueEffectSettlementSecurity = this.Create("issue_effect_settlement_security");
			this._issueEffectSettlementMilitia = this.Create("issue_effect_settlement_militia");
			this._issueEffectSettlementProsperity = this.Create("issue_effect_settlement_prosperity");
			this._issueEffectVillageHearth = this.Create("issue_effect_village_hearth");
			this._issueEffectSettlementFood = this.Create("issue_effect_settlement_food");
			this._issueEffectSettlementTax = this.Create("issue_effect_settlement_tax");
			this._issueEffectSettlementGarrison = this.Create("issue_effect_settlement_garrison");
			this._issueEffectHalfVillageProduction = this.Create("issue_effect_half_village_production");
			this._issueEffectIssueOwnerPower = this.Create("issue_effect_issue_owner_power");
			this._clanInfluence = this.Create("issue_effect_clan_influence");
			this.InitializeAll();
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x000DE3FE File Offset: 0x000DC5FE
		private IssueEffect Create(string stringId)
		{
			return Game.Current.ObjectManager.RegisterPresumedObject<IssueEffect>(new IssueEffect(stringId));
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x000DE418 File Offset: 0x000DC618
		private void InitializeAll()
		{
			this._issueEffectSettlementLoyalty.Initialize(new TextObject("{=YO0x7ZAo}Loyalty", null), new TextObject("{=xAWvm25T}Effects settlement's loyalty.", null));
			this._issueEffectSettlementSecurity.Initialize(new TextObject("{=MqCH7R4A}Security", null), new TextObject("{=h117Qj3E}Effects settlement's security.", null));
			this._issueEffectSettlementMilitia.Initialize(new TextObject("{=gsVtO9A7}Militia", null), new TextObject("{=dTmPV82D}Effects settlement's militia.", null));
			this._issueEffectSettlementProsperity.Initialize(new TextObject("{=IagYTD5O}Prosperity", null), new TextObject("{=ETye0JMY}Effects settlement's prosperity.", null));
			this._issueEffectVillageHearth.Initialize(new TextObject("{=f5X5uU0m}Village Hearth", null), new TextObject("{=7TbVhbT9}Effects village's hearth.", null));
			this._issueEffectSettlementFood.Initialize(new TextObject("{=qSi4DlT4}Food", null), new TextObject("{=onDsUkUl}Effects settlement's food.", null));
			this._issueEffectSettlementTax.Initialize(new TextObject("{=2awf1tei}Tax", null), new TextObject("{=q2Ovtr1s}Effects settlement's tax.", null));
			this._issueEffectSettlementGarrison.Initialize(new TextObject("{=jlgjLDo7}Garrison", null), new TextObject("{=WJ7SnBgN}Effects settlement's garrison.", null));
			this._issueEffectHalfVillageProduction.Initialize(new TextObject("{=bGyrPe8c}Production", null), new TextObject("{=arbaXvQf}Effects village's production.", null));
			this._issueEffectIssueOwnerPower.Initialize(new TextObject("{=gGXelWQX}Issue owner power", null), new TextObject("{=tjudHtDB}Effects the power of issue owner in the settlement.", null));
			this._clanInfluence.Initialize(new TextObject("{=KN6khbSl}Clan Influence", null), new TextObject("{=y2aLOwOs}Effects the influence of clan.", null));
		}

		// Token: 0x04000F2F RID: 3887
		private IssueEffect _issueEffectSettlementGarrison;

		// Token: 0x04000F30 RID: 3888
		private IssueEffect _issueEffectSettlementLoyalty;

		// Token: 0x04000F31 RID: 3889
		private IssueEffect _issueEffectSettlementSecurity;

		// Token: 0x04000F32 RID: 3890
		private IssueEffect _issueEffectSettlementMilitia;

		// Token: 0x04000F33 RID: 3891
		private IssueEffect _issueEffectSettlementProsperity;

		// Token: 0x04000F34 RID: 3892
		private IssueEffect _issueEffectVillageHearth;

		// Token: 0x04000F35 RID: 3893
		private IssueEffect _issueEffectSettlementFood;

		// Token: 0x04000F36 RID: 3894
		private IssueEffect _issueEffectSettlementTax;

		// Token: 0x04000F37 RID: 3895
		private IssueEffect _issueEffectHalfVillageProduction;

		// Token: 0x04000F38 RID: 3896
		private IssueEffect _issueEffectIssueOwnerPower;

		// Token: 0x04000F39 RID: 3897
		private IssueEffect _clanInfluence;
	}
}
