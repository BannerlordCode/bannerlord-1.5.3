using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia.Pages
{
	// Token: 0x0200018B RID: 395
	[EncyclopediaModel(new Type[] { typeof(Settlement) })]
	public class DefaultEncyclopediaSettlementPage : EncyclopediaPage
	{
		// Token: 0x06001C59 RID: 7257 RVA: 0x00091E1D File Offset: 0x0009001D
		public DefaultEncyclopediaSettlementPage()
		{
			base.HomePageOrderIndex = 100;
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x00091E2D File Offset: 0x0009002D
		protected override IEnumerable<EncyclopediaListItem> InitializeListItems()
		{
			using (List<Settlement>.Enumerator enumerator = Settlement.All.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Settlement settlement = enumerator.Current;
					if (this.IsValidEncyclopediaItem(settlement))
					{
						yield return new EncyclopediaListItem(settlement, settlement.Name.ToString(), "", settlement.StringId, base.GetIdentifier(typeof(Settlement)), DefaultEncyclopediaSettlementPage.CanPlayerSeeValuesOf(settlement), delegate
						{
							InformationManager.ShowTooltip(typeof(Settlement), new object[] { settlement });
						});
					}
				}
			}
			List<Settlement>.Enumerator enumerator = default(List<Settlement>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x00091E40 File Offset: 0x00090040
		protected override IEnumerable<EncyclopediaFilterGroup> InitializeFilterItems()
		{
			List<EncyclopediaFilterGroup> list = new List<EncyclopediaFilterGroup>();
			List<EncyclopediaFilterItem> list2 = new List<EncyclopediaFilterItem>();
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=bOTQ7Pta}Town", null), (object s) => ((Settlement)s).IsTown));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=sVXa3zFx}Castle", null), (object s) => ((Settlement)s).IsCastle));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=Ua6CNLBZ}Village", null), (object s) => ((Settlement)s).IsVillage));
			List<EncyclopediaFilterItem> list3 = list2;
			list.Add(new EncyclopediaFilterGroup(list3, new TextObject("{=zMMqgxb1}Type", null)));
			List<EncyclopediaFilterItem> list4 = new List<EncyclopediaFilterItem>();
			using (List<CultureObject>.Enumerator enumerator = (from x in Game.Current.ObjectManager.GetObjectTypeList<CultureObject>()
				where x.IsMainCulture
				select x into f
				orderby f.Name.ToString()
				select f).ToList<CultureObject>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CultureObject culture = enumerator.Current;
					if (culture.StringId != "neutral_culture" && culture.CanHaveSettlement)
					{
						list4.Add(new EncyclopediaFilterItem(culture.Name, (object c) => ((Settlement)c).Culture == culture));
					}
				}
			}
			list.Add(new EncyclopediaFilterGroup(list4, GameTexts.FindText("str_culture", null)));
			return list;
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x0009201C File Offset: 0x0009021C
		protected override IEnumerable<EncyclopediaSortController> InitializeSortControllers()
		{
			return new List<EncyclopediaSortController>
			{
				new EncyclopediaSortController(GameTexts.FindText("str_garrison", null), new DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementGarrisonComparer()),
				new EncyclopediaSortController(GameTexts.FindText("str_food", null), new DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementFoodComparer()),
				new EncyclopediaSortController(GameTexts.FindText("str_security", null), new DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementSecurityComparer()),
				new EncyclopediaSortController(GameTexts.FindText("str_loyalty", null), new DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementLoyaltyComparer()),
				new EncyclopediaSortController(GameTexts.FindText("str_militia", null), new DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementMilitiaComparer()),
				new EncyclopediaSortController(GameTexts.FindText("str_prosperity", null), new DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementProsperityComparer())
			};
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x000920D0 File Offset: 0x000902D0
		public override string GetViewFullyQualifiedName()
		{
			return "EncyclopediaSettlementPage";
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x000920D7 File Offset: 0x000902D7
		public override TextObject GetName()
		{
			return GameTexts.FindText("str_settlements", null);
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x000920E4 File Offset: 0x000902E4
		public override TextObject GetDescriptionText()
		{
			return GameTexts.FindText("str_settlement_description", null);
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x000920F1 File Offset: 0x000902F1
		public override string GetStringID()
		{
			return "EncyclopediaSettlement";
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x000920F8 File Offset: 0x000902F8
		public override bool IsValidEncyclopediaItem(object o)
		{
			Settlement settlement = o as Settlement;
			return settlement != null && (settlement.IsFortification || settlement.IsVillage);
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x00092121 File Offset: 0x00090321
		private static bool CanPlayerSeeValuesOf(Settlement settlement)
		{
			return Campaign.Current.Models.InformationRestrictionModel.DoesPlayerKnowDetailsOf(settlement);
		}

		// Token: 0x02000607 RID: 1543
		private class EncyclopediaListSettlementGarrisonComparer : DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementComparer
		{
			// Token: 0x060052A4 RID: 21156 RVA: 0x00192E64 File Offset: 0x00191064
			private static int GarrisonComparison(Town t1, Town t2)
			{
				int num = ((t1.GarrisonParty != null) ? t1.GarrisonParty.MemberRoster.TotalManCount : 0);
				int num2 = ((t2.GarrisonParty != null) ? t2.GarrisonParty.MemberRoster.TotalManCount : 0);
				return num.CompareTo(num2);
			}

			// Token: 0x060052A5 RID: 21157 RVA: 0x00192EB4 File Offset: 0x001910B4
			protected override bool CompareVisibility(Settlement s1, Settlement s2, out int comparisonResult)
			{
				if (s1.IsTown && s2.IsTown)
				{
					if (s1.Town.GarrisonParty == null && s2.Town.GarrisonParty == null)
					{
						comparisonResult = 0;
						return true;
					}
					if (s1.Town.GarrisonParty == null)
					{
						comparisonResult = (base.IsAscending ? 2 : (-2));
						return true;
					}
					if (s2.Town.GarrisonParty == null)
					{
						comparisonResult = (base.IsAscending ? (-2) : 2);
						return true;
					}
				}
				return base.CompareVisibility(s1, s2, out comparisonResult);
			}

			// Token: 0x060052A6 RID: 21158 RVA: 0x00192F35 File Offset: 0x00191135
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareFiefs(x, y, new DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementComparer.SettlementVisibilityComparerDelegate(this.CompareVisibility), new Func<Town, Town, int>(DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementGarrisonComparer.GarrisonComparison));
			}

			// Token: 0x060052A7 RID: 21159 RVA: 0x00192F58 File Offset: 0x00191158
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Settlement settlement;
				if ((settlement = item.Object as Settlement) == null)
				{
					Debug.FailedAssert("Unable to get the garrison of a non-settlement object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaSettlementPage.cs", "GetComparedValueText", 163);
					return "";
				}
				if (settlement.IsVillage)
				{
					return this._emptyValue.ToString();
				}
				if (!DefaultEncyclopediaSettlementPage.CanPlayerSeeValuesOf(settlement))
				{
					return this._missingValue.ToString();
				}
				MobileParty garrisonParty = settlement.Town.GarrisonParty;
				return ((garrisonParty != null) ? garrisonParty.MemberRoster.TotalManCount.ToString() : null) ?? 0.ToString();
			}
		}

		// Token: 0x02000608 RID: 1544
		private class EncyclopediaListSettlementFoodComparer : DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementComparer
		{
			// Token: 0x060052A9 RID: 21161 RVA: 0x00192FF4 File Offset: 0x001911F4
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareFiefs(x, y, new DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementComparer.SettlementVisibilityComparerDelegate(this.CompareVisibility), new Func<Town, Town, int>(DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementFoodComparer.FoodComparison));
			}

			// Token: 0x060052AA RID: 21162 RVA: 0x00193018 File Offset: 0x00191218
			private static int FoodComparison(Town t1, Town t2)
			{
				return t1.FoodStocks.CompareTo(t2.FoodStocks);
			}

			// Token: 0x060052AB RID: 21163 RVA: 0x0019303C File Offset: 0x0019123C
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Settlement settlement;
				if ((settlement = item.Object as Settlement) == null)
				{
					Debug.FailedAssert("Unable to get the food stocks of a non-settlement object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaSettlementPage.cs", "GetComparedValueText", 198);
					return "";
				}
				if (settlement.IsVillage)
				{
					return this._emptyValue.ToString();
				}
				if (!DefaultEncyclopediaSettlementPage.CanPlayerSeeValuesOf(settlement))
				{
					return this._missingValue.ToString();
				}
				return ((int)settlement.Town.FoodStocks).ToString();
			}
		}

		// Token: 0x02000609 RID: 1545
		private class EncyclopediaListSettlementSecurityComparer : DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementComparer
		{
			// Token: 0x060052AD RID: 21165 RVA: 0x001930BB File Offset: 0x001912BB
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareFiefs(x, y, new DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementComparer.SettlementVisibilityComparerDelegate(this.CompareVisibility), new Func<Town, Town, int>(DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementSecurityComparer.SecurityComparison));
			}

			// Token: 0x060052AE RID: 21166 RVA: 0x001930E0 File Offset: 0x001912E0
			private static int SecurityComparison(Town t1, Town t2)
			{
				return t1.Security.CompareTo(t2.Security);
			}

			// Token: 0x060052AF RID: 21167 RVA: 0x00193104 File Offset: 0x00191304
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Settlement settlement;
				if ((settlement = item.Object as Settlement) == null)
				{
					Debug.FailedAssert("Unable to get the security of a non-settlement object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaSettlementPage.cs", "GetComparedValueText", 233);
					return "";
				}
				if (settlement.IsVillage)
				{
					return this._emptyValue.ToString();
				}
				if (!DefaultEncyclopediaSettlementPage.CanPlayerSeeValuesOf(settlement))
				{
					return this._missingValue.ToString();
				}
				return ((int)settlement.Town.Security).ToString();
			}
		}

		// Token: 0x0200060A RID: 1546
		private class EncyclopediaListSettlementLoyaltyComparer : DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementComparer
		{
			// Token: 0x060052B1 RID: 21169 RVA: 0x00193183 File Offset: 0x00191383
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareFiefs(x, y, new DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementComparer.SettlementVisibilityComparerDelegate(this.CompareVisibility), new Func<Town, Town, int>(DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementLoyaltyComparer.LoyaltyComparison));
			}

			// Token: 0x060052B2 RID: 21170 RVA: 0x001931A8 File Offset: 0x001913A8
			private static int LoyaltyComparison(Town t1, Town t2)
			{
				return t1.Loyalty.CompareTo(t2.Loyalty);
			}

			// Token: 0x060052B3 RID: 21171 RVA: 0x001931CC File Offset: 0x001913CC
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Settlement settlement;
				if ((settlement = item.Object as Settlement) == null)
				{
					Debug.FailedAssert("Unable to get the loyalty of a non-settlement object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaSettlementPage.cs", "GetComparedValueText", 268);
					return "";
				}
				if (settlement.IsVillage)
				{
					return this._emptyValue.ToString();
				}
				if (!DefaultEncyclopediaSettlementPage.CanPlayerSeeValuesOf(settlement))
				{
					return this._missingValue.ToString();
				}
				return ((int)settlement.Town.Loyalty).ToString();
			}
		}

		// Token: 0x0200060B RID: 1547
		private class EncyclopediaListSettlementMilitiaComparer : DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementComparer
		{
			// Token: 0x060052B5 RID: 21173 RVA: 0x0019324B File Offset: 0x0019144B
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareSettlements(x, y, new DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementComparer.SettlementVisibilityComparerDelegate(this.CompareVisibility), new Func<Settlement, Settlement, int>(DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementMilitiaComparer.MilitiaComparison));
			}

			// Token: 0x060052B6 RID: 21174 RVA: 0x00193270 File Offset: 0x00191470
			private static int MilitiaComparison(Settlement t1, Settlement t2)
			{
				return t1.Militia.CompareTo(t2.Militia);
			}

			// Token: 0x060052B7 RID: 21175 RVA: 0x00193294 File Offset: 0x00191494
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Settlement settlement;
				if ((settlement = item.Object as Settlement) == null)
				{
					Debug.FailedAssert("Unable to get the militia of a non-settlement object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaSettlementPage.cs", "GetComparedValueText", 299);
					return "";
				}
				if (!DefaultEncyclopediaSettlementPage.CanPlayerSeeValuesOf(settlement))
				{
					return this._missingValue.ToString();
				}
				return ((int)settlement.Militia).ToString();
			}
		}

		// Token: 0x0200060C RID: 1548
		private class EncyclopediaListSettlementProsperityComparer : DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementComparer
		{
			// Token: 0x060052B9 RID: 21177 RVA: 0x001932FA File Offset: 0x001914FA
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareFiefs(x, y, new DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementComparer.SettlementVisibilityComparerDelegate(this.CompareVisibility), new Func<Town, Town, int>(DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementProsperityComparer.ProsperityComparison));
			}

			// Token: 0x060052BA RID: 21178 RVA: 0x00193320 File Offset: 0x00191520
			private static int ProsperityComparison(Town t1, Town t2)
			{
				return t1.Prosperity.CompareTo(t2.Prosperity);
			}

			// Token: 0x060052BB RID: 21179 RVA: 0x00193344 File Offset: 0x00191544
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Settlement settlement;
				if ((settlement = item.Object as Settlement) == null)
				{
					Debug.FailedAssert("Unable to get the prosperity of a non-settlement object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaSettlementPage.cs", "GetComparedValueText", 334);
					return "";
				}
				if (settlement.IsVillage)
				{
					return this._emptyValue.ToString();
				}
				if (!DefaultEncyclopediaSettlementPage.CanPlayerSeeValuesOf(settlement))
				{
					return this._missingValue.ToString();
				}
				return ((int)settlement.Town.Prosperity).ToString();
			}
		}

		// Token: 0x0200060D RID: 1549
		public abstract class EncyclopediaListSettlementComparer : EncyclopediaListItemComparerBase
		{
			// Token: 0x060052BD RID: 21181 RVA: 0x001933C4 File Offset: 0x001915C4
			protected virtual bool CompareVisibility(Settlement s1, Settlement s2, out int comparisonResult)
			{
				bool flag = DefaultEncyclopediaSettlementPage.CanPlayerSeeValuesOf(s1);
				bool flag2 = DefaultEncyclopediaSettlementPage.CanPlayerSeeValuesOf(s2);
				if (!flag && !flag2)
				{
					comparisonResult = 0;
					return true;
				}
				if (!flag)
				{
					comparisonResult = (base.IsAscending ? 1 : (-1));
					return true;
				}
				if (!flag2)
				{
					comparisonResult = (base.IsAscending ? (-1) : 1);
					return true;
				}
				comparisonResult = 0;
				return false;
			}

			// Token: 0x060052BE RID: 21182 RVA: 0x00193414 File Offset: 0x00191614
			protected int CompareSettlements(EncyclopediaListItem x, EncyclopediaListItem y, DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementComparer.SettlementVisibilityComparerDelegate visibilityComparison, Func<Settlement, Settlement, int> comparison)
			{
				Settlement settlement;
				Settlement settlement2;
				if ((settlement = x.Object as Settlement) == null || (settlement2 = y.Object as Settlement) == null)
				{
					Debug.FailedAssert("Both objects should be settlements.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaSettlementPage.cs", "CompareSettlements", 383);
					return 0;
				}
				int num;
				if (visibilityComparison(settlement, settlement2, out num))
				{
					if (num == 0)
					{
						return base.ResolveEquality(x, y);
					}
					return num * (base.IsAscending ? 1 : (-1));
				}
				else
				{
					int num2 = comparison(settlement, settlement2) * (base.IsAscending ? 1 : (-1));
					if (num2 == 0)
					{
						return base.ResolveEquality(x, y);
					}
					return num2;
				}
			}

			// Token: 0x060052BF RID: 21183 RVA: 0x001934A8 File Offset: 0x001916A8
			protected int CompareFiefs(EncyclopediaListItem x, EncyclopediaListItem y, DefaultEncyclopediaSettlementPage.EncyclopediaListSettlementComparer.SettlementVisibilityComparerDelegate visibilityComparison, Func<Town, Town, int> comparison)
			{
				Settlement settlement;
				Settlement settlement2;
				if ((settlement = x.Object as Settlement) == null || (settlement2 = y.Object as Settlement) == null)
				{
					Debug.FailedAssert("Unable to compare loyalty of non-fief (castle or town) objects.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaSettlementPage.cs", "CompareFiefs", 407);
					return 0;
				}
				int num = settlement.IsVillage.CompareTo(settlement2.IsVillage);
				if (num != 0)
				{
					return num;
				}
				if (settlement.IsVillage && settlement2.IsVillage)
				{
					return base.ResolveEquality(x, y);
				}
				int num2;
				if (visibilityComparison(settlement, settlement2, out num2))
				{
					if (num2 == 0)
					{
						return base.ResolveEquality(x, y);
					}
					return num2 * (base.IsAscending ? 1 : (-1));
				}
				else
				{
					num = comparison(settlement.Town, settlement2.Town) * (base.IsAscending ? 1 : (-1));
					if (num == 0)
					{
						return base.ResolveEquality(x, y);
					}
					return num;
				}
			}

			// Token: 0x0200090C RID: 2316
			// (Invoke) Token: 0x06006CDB RID: 27867
			protected delegate bool SettlementVisibilityComparerDelegate(Settlement s1, Settlement s2, out int comparisonResult);
		}
	}
}
