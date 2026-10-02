using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Encyclopedia.Pages
{
	// Token: 0x02000189 RID: 393
	[EncyclopediaModel(new Type[] { typeof(Kingdom) })]
	public class DefaultEncyclopediaFactionPage : EncyclopediaPage
	{
		// Token: 0x06001C44 RID: 7236 RVA: 0x00091757 File Offset: 0x0008F957
		public DefaultEncyclopediaFactionPage()
		{
			base.HomePageOrderIndex = 400;
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x0009176A File Offset: 0x0008F96A
		public override string GetViewFullyQualifiedName()
		{
			return "EncyclopediaFactionPage";
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x00091771 File Offset: 0x0008F971
		public override TextObject GetName()
		{
			return GameTexts.FindText("str_kingdoms_group", null);
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x0009177E File Offset: 0x0008F97E
		public override TextObject GetDescriptionText()
		{
			return GameTexts.FindText("str_faction_description", null);
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x0009178B File Offset: 0x0008F98B
		public override string GetStringID()
		{
			return "EncyclopediaKingdom";
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x00091792 File Offset: 0x0008F992
		public override MBObjectBase GetObject(string typeName, string stringID)
		{
			return Campaign.Current.CampaignObjectManager.Find<Kingdom>(stringID);
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x000917A4 File Offset: 0x0008F9A4
		public override bool IsValidEncyclopediaItem(object o)
		{
			IFaction faction = o as IFaction;
			return faction != null && !faction.IsBanditFaction;
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x000917C6 File Offset: 0x0008F9C6
		protected override IEnumerable<EncyclopediaListItem> InitializeListItems()
		{
			using (List<Kingdom>.Enumerator enumerator = Kingdom.All.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Kingdom kingdom = enumerator.Current;
					if (this.IsValidEncyclopediaItem(kingdom))
					{
						yield return new EncyclopediaListItem(kingdom, kingdom.Name.ToString(), "", kingdom.StringId, base.GetIdentifier(typeof(Kingdom)), true, delegate
						{
							InformationManager.ShowTooltip(typeof(Kingdom), new object[] { kingdom });
						});
					}
				}
			}
			List<Kingdom>.Enumerator enumerator = default(List<Kingdom>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x000917D8 File Offset: 0x0008F9D8
		protected override IEnumerable<EncyclopediaFilterGroup> InitializeFilterItems()
		{
			List<EncyclopediaFilterGroup> list = new List<EncyclopediaFilterGroup>();
			List<EncyclopediaFilterItem> list2 = new List<EncyclopediaFilterItem>();
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=SlubkZ1A}Eliminated", null), (object f) => ((IFaction)f).IsEliminated));
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=YRbSBxqT}Active", null), (object f) => !((IFaction)f).IsEliminated));
			List<EncyclopediaFilterItem> list3 = list2;
			list.Add(new EncyclopediaFilterGroup(list3, new TextObject("{=DXczLzml}Status", null)));
			return list;
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x00091874 File Offset: 0x0008FA74
		protected override IEnumerable<EncyclopediaSortController> InitializeSortControllers()
		{
			return new List<EncyclopediaSortController>
			{
				new EncyclopediaSortController(GameTexts.FindText("str_total_strength", null), new DefaultEncyclopediaFactionPage.EncyclopediaListKingdomTotalStrengthComparer()),
				new EncyclopediaSortController(GameTexts.FindText("str_fiefs", null), new DefaultEncyclopediaFactionPage.EncyclopediaListKingdomFiefsComparer()),
				new EncyclopediaSortController(GameTexts.FindText("str_clans", null), new DefaultEncyclopediaFactionPage.EncyclopediaListKingdomClanComparer())
			};
		}

		// Token: 0x020005F8 RID: 1528
		private class EncyclopediaListKingdomTotalStrengthComparer : DefaultEncyclopediaFactionPage.EncyclopediaListKingdomComparer
		{
			// Token: 0x0600525B RID: 21083 RVA: 0x0019216B File Offset: 0x0019036B
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareKingdoms(x, y, DefaultEncyclopediaFactionPage.EncyclopediaListKingdomTotalStrengthComparer._comparison);
			}

			// Token: 0x0600525C RID: 21084 RVA: 0x0019217C File Offset: 0x0019037C
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Kingdom kingdom;
				if ((kingdom = item.Object as Kingdom) != null)
				{
					return ((int)kingdom.CurrentTotalStrength).ToString();
				}
				Debug.FailedAssert("Unable to get the total strength of a non-kingdom object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaFactionPage.cs", "GetComparedValueText", 118);
				return "";
			}

			// Token: 0x040019A0 RID: 6560
			private static Func<Kingdom, Kingdom, int> _comparison = (Kingdom k1, Kingdom k2) => k1.CurrentTotalStrength.CompareTo(k2.CurrentTotalStrength);
		}

		// Token: 0x020005F9 RID: 1529
		private class EncyclopediaListKingdomFiefsComparer : DefaultEncyclopediaFactionPage.EncyclopediaListKingdomComparer
		{
			// Token: 0x0600525F RID: 21087 RVA: 0x001921E2 File Offset: 0x001903E2
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareKingdoms(x, y, DefaultEncyclopediaFactionPage.EncyclopediaListKingdomFiefsComparer._comparison);
			}

			// Token: 0x06005260 RID: 21088 RVA: 0x001921F4 File Offset: 0x001903F4
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Kingdom kingdom;
				if ((kingdom = item.Object as Kingdom) != null)
				{
					return kingdom.Fiefs.Count.ToString();
				}
				Debug.FailedAssert("Unable to get the fief count from a non-kingdom object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaFactionPage.cs", "GetComparedValueText", 139);
				return "";
			}

			// Token: 0x040019A1 RID: 6561
			private static Func<Kingdom, Kingdom, int> _comparison = (Kingdom k1, Kingdom k2) => k1.Fiefs.Count.CompareTo(k2.Fiefs.Count);
		}

		// Token: 0x020005FA RID: 1530
		private class EncyclopediaListKingdomClanComparer : DefaultEncyclopediaFactionPage.EncyclopediaListKingdomComparer
		{
			// Token: 0x06005263 RID: 21091 RVA: 0x00192261 File Offset: 0x00190461
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareKingdoms(x, y, DefaultEncyclopediaFactionPage.EncyclopediaListKingdomClanComparer._comparison);
			}

			// Token: 0x06005264 RID: 21092 RVA: 0x00192270 File Offset: 0x00190470
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Kingdom kingdom;
				if ((kingdom = item.Object as Kingdom) != null)
				{
					return kingdom.Clans.Count.ToString();
				}
				Debug.FailedAssert("Unable to get the clan count from a non-kingdom object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaFactionPage.cs", "GetComparedValueText", 160);
				return "";
			}

			// Token: 0x040019A2 RID: 6562
			private static Func<Kingdom, Kingdom, int> _comparison = (Kingdom k1, Kingdom k2) => k1.Clans.Count.CompareTo(k2.Clans.Count);
		}

		// Token: 0x020005FB RID: 1531
		public abstract class EncyclopediaListKingdomComparer : EncyclopediaListItemComparerBase
		{
			// Token: 0x06005267 RID: 21095 RVA: 0x001922E0 File Offset: 0x001904E0
			public int CompareKingdoms(EncyclopediaListItem x, EncyclopediaListItem y, Func<Kingdom, Kingdom, int> comparison)
			{
				Kingdom kingdom;
				Kingdom kingdom2;
				if ((kingdom = x.Object as Kingdom) == null || (kingdom2 = y.Object as Kingdom) == null)
				{
					Debug.FailedAssert("Both objects should be kingdoms.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaFactionPage.cs", "CompareKingdoms", 175);
					return 0;
				}
				int num = comparison(kingdom, kingdom2) * (base.IsAscending ? 1 : (-1));
				if (num == 0)
				{
					return base.ResolveEquality(x, y);
				}
				return num;
			}
		}
	}
}
