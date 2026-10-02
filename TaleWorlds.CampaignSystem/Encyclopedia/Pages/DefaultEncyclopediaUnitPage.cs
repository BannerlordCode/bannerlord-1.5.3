using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Encyclopedia.Pages
{
	// Token: 0x0200018D RID: 397
	[EncyclopediaModel(new Type[] { typeof(CharacterObject) })]
	public class DefaultEncyclopediaUnitPage : EncyclopediaPage
	{
		// Token: 0x06001C72 RID: 7282 RVA: 0x000925AC File Offset: 0x000907AC
		public DefaultEncyclopediaUnitPage()
		{
			base.HomePageOrderIndex = 300;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x000925BF File Offset: 0x000907BF
		protected override IEnumerable<EncyclopediaListItem> InitializeListItems()
		{
			using (List<CharacterObject>.Enumerator enumerator = CharacterObject.All.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharacterObject character = enumerator.Current;
					if (this.IsValidEncyclopediaItem(character))
					{
						yield return new EncyclopediaListItem(character, character.Name.ToString(), "", character.StringId, base.GetIdentifier(typeof(CharacterObject)), true, delegate
						{
							InformationManager.ShowTooltip(typeof(CharacterObject), new object[] { character });
						});
					}
				}
			}
			List<CharacterObject>.Enumerator enumerator = default(List<CharacterObject>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x000925D0 File Offset: 0x000907D0
		protected override IEnumerable<EncyclopediaFilterGroup> InitializeFilterItems()
		{
			List<EncyclopediaFilterGroup> list = new List<EncyclopediaFilterGroup>();
			List<EncyclopediaFilterItem> typeFilterItems = this.GetTypeFilterItems();
			list.Add(new EncyclopediaFilterGroup(typeFilterItems, new TextObject("{=zMMqgxb1}Type", null)));
			List<EncyclopediaFilterItem> occupationFilterItems = this.GetOccupationFilterItems();
			list.Add(new EncyclopediaFilterGroup(occupationFilterItems, new TextObject("{=GZxFIeiJ}Occupation", null)));
			List<EncyclopediaFilterItem> cultureFilterItems = this.GetCultureFilterItems();
			list.Add(new EncyclopediaFilterGroup(cultureFilterItems, GameTexts.FindText("str_culture", null)));
			List<EncyclopediaFilterItem> outlawFilterItems = this.GetOutlawFilterItems();
			list.Add(new EncyclopediaFilterGroup(outlawFilterItems, GameTexts.FindText("str_outlaw", null)));
			return list;
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x0009265C File Offset: 0x0009085C
		protected virtual List<EncyclopediaFilterItem> GetTypeFilterItems()
		{
			List<EncyclopediaFilterItem> list = new List<EncyclopediaFilterItem>();
			list.Add(new EncyclopediaFilterItem(new TextObject("{=1Bm1Wk1v}Infantry", null), (object s) => ((CharacterObject)s).IsInfantry));
			list.Add(new EncyclopediaFilterItem(new TextObject("{=bIiBytSB}Archers", null), (object s) => ((CharacterObject)s).IsRanged && !((CharacterObject)s).IsMounted));
			list.Add(new EncyclopediaFilterItem(new TextObject("{=YVGtcLHF}Cavalry", null), (object s) => ((CharacterObject)s).IsMounted && !((CharacterObject)s).IsRanged));
			list.Add(new EncyclopediaFilterItem(new TextObject("{=I1CMeL9R}Mounted Archers", null), (object s) => ((CharacterObject)s).IsRanged && ((CharacterObject)s).IsMounted));
			return list;
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x00092744 File Offset: 0x00090944
		protected virtual List<EncyclopediaFilterItem> GetOccupationFilterItems()
		{
			List<EncyclopediaFilterItem> list = new List<EncyclopediaFilterItem>();
			list.Add(new EncyclopediaFilterItem(GameTexts.FindText("str_occupation", "Soldier"), (object s) => ((CharacterObject)s).Occupation == Occupation.Soldier));
			list.Add(new EncyclopediaFilterItem(GameTexts.FindText("str_occupation", "Mercenary"), (object s) => ((CharacterObject)s).Occupation == Occupation.Mercenary));
			list.Add(new EncyclopediaFilterItem(GameTexts.FindText("str_occupation", "Bandit"), (object s) => ((CharacterObject)s).Occupation == Occupation.Bandit));
			return list;
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x00092804 File Offset: 0x00090A04
		protected virtual List<EncyclopediaFilterItem> GetCultureFilterItems()
		{
			List<EncyclopediaFilterItem> list = new List<EncyclopediaFilterItem>();
			using (List<CultureObject>.Enumerator enumerator = (from x in Game.Current.ObjectManager.GetObjectTypeList<CultureObject>()
				where x.IsMainCulture
				select x into f
				orderby f.Name.ToString()
				select f).ToList<CultureObject>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CultureObject culture = enumerator.Current;
					if (!culture.IsBandit && culture.StringId != "neutral_culture")
					{
						list.Add(new EncyclopediaFilterItem(culture.Name, (object c) => ((CharacterObject)c).Culture == culture));
					}
				}
			}
			return list;
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x00092900 File Offset: 0x00090B00
		protected virtual List<EncyclopediaFilterItem> GetOutlawFilterItems()
		{
			List<EncyclopediaFilterItem> list = new List<EncyclopediaFilterItem>();
			using (List<CultureObject>.Enumerator enumerator = (from x in Game.Current.ObjectManager.GetObjectTypeList<CultureObject>()
				orderby !x.IsMainCulture descending
				select x).ThenBy<CultureObject, string>((CultureObject f) => f.Name.ToString()).ToList<CultureObject>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CultureObject culture = enumerator.Current;
					if (culture.IsBandit)
					{
						list.Add(new EncyclopediaFilterItem(culture.Name, (object c) => ((CharacterObject)c).Culture == culture));
					}
				}
			}
			return list;
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x000929E4 File Offset: 0x00090BE4
		protected override IEnumerable<EncyclopediaSortController> InitializeSortControllers()
		{
			return new List<EncyclopediaSortController>
			{
				new EncyclopediaSortController(new TextObject("{=cc1d7mkq}Tier", null), new DefaultEncyclopediaUnitPage.EncyclopediaListUnitTierComparer()),
				new EncyclopediaSortController(GameTexts.FindText("str_level_tag", null), new DefaultEncyclopediaUnitPage.EncyclopediaListUnitLevelComparer())
			};
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x00092A21 File Offset: 0x00090C21
		public override string GetViewFullyQualifiedName()
		{
			return "EncyclopediaUnitPage";
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x00092A28 File Offset: 0x00090C28
		public override TextObject GetName()
		{
			return GameTexts.FindText("str_encyclopedia_troops", null);
		}

		// Token: 0x06001C7C RID: 7292 RVA: 0x00092A35 File Offset: 0x00090C35
		public override TextObject GetDescriptionText()
		{
			return GameTexts.FindText("str_unit_description", null);
		}

		// Token: 0x06001C7D RID: 7293 RVA: 0x00092A42 File Offset: 0x00090C42
		public override string GetStringID()
		{
			return "EncyclopediaUnit";
		}

		// Token: 0x06001C7E RID: 7294 RVA: 0x00092A4C File Offset: 0x00090C4C
		public override bool IsValidEncyclopediaItem(object o)
		{
			CharacterObject characterObject = o as CharacterObject;
			return characterObject != null && !characterObject.IsTemplate && characterObject != null && !characterObject.HiddenInEncyclopedia && ((characterObject != null) ? characterObject.HeroObject : null) == null && (characterObject.Occupation == Occupation.Soldier || characterObject.Occupation == Occupation.Mercenary || characterObject.Occupation == Occupation.Bandit || characterObject.Occupation == Occupation.Gangster || characterObject.Occupation == Occupation.CaravanGuard || (characterObject.Occupation == Occupation.Villager && characterObject.UpgradeTargets.Length != 0));
		}

		// Token: 0x0200061B RID: 1563
		private class EncyclopediaListUnitTierComparer : DefaultEncyclopediaUnitPage.EncyclopediaListUnitComparer
		{
			// Token: 0x060052FB RID: 21243 RVA: 0x00193E9F File Offset: 0x0019209F
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareUnits(x, y, DefaultEncyclopediaUnitPage.EncyclopediaListUnitTierComparer._comparison);
			}

			// Token: 0x060052FC RID: 21244 RVA: 0x00193EB0 File Offset: 0x001920B0
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				CharacterObject characterObject;
				if ((characterObject = item.Object as CharacterObject) != null)
				{
					return characterObject.Tier.ToString();
				}
				Debug.FailedAssert("Unable to get the tier of a non-character object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaUnitPage.cs", "GetComparedValueText", 175);
				return "";
			}

			// Token: 0x040019E6 RID: 6630
			private static Func<CharacterObject, CharacterObject, int> _comparison = (CharacterObject c1, CharacterObject c2) => c1.Tier.CompareTo(c2.Tier);
		}

		// Token: 0x0200061C RID: 1564
		private class EncyclopediaListUnitLevelComparer : DefaultEncyclopediaUnitPage.EncyclopediaListUnitComparer
		{
			// Token: 0x060052FF RID: 21247 RVA: 0x00193F18 File Offset: 0x00192118
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareUnits(x, y, DefaultEncyclopediaUnitPage.EncyclopediaListUnitLevelComparer._comparison);
			}

			// Token: 0x06005300 RID: 21248 RVA: 0x00193F28 File Offset: 0x00192128
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				CharacterObject characterObject;
				if ((characterObject = item.Object as CharacterObject) != null)
				{
					return characterObject.Level.ToString();
				}
				Debug.FailedAssert("Unable to get the level of a non-character object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaUnitPage.cs", "GetComparedValueText", 196);
				return "";
			}

			// Token: 0x040019E7 RID: 6631
			private static Func<CharacterObject, CharacterObject, int> _comparison = (CharacterObject c1, CharacterObject c2) => c1.Level.CompareTo(c2.Level);
		}

		// Token: 0x0200061D RID: 1565
		public abstract class EncyclopediaListUnitComparer : EncyclopediaListItemComparerBase
		{
			// Token: 0x06005303 RID: 21251 RVA: 0x00193F90 File Offset: 0x00192190
			public int CompareUnits(EncyclopediaListItem x, EncyclopediaListItem y, Func<CharacterObject, CharacterObject, int> comparison)
			{
				CharacterObject characterObject;
				CharacterObject characterObject2;
				if ((characterObject = x.Object as CharacterObject) == null || (characterObject2 = y.Object as CharacterObject) == null)
				{
					Debug.FailedAssert("Both objects should be character objects.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaUnitPage.cs", "CompareUnits", 211);
					return 0;
				}
				int num = comparison(characterObject, characterObject2) * (base.IsAscending ? 1 : (-1));
				if (num == 0)
				{
					return base.ResolveEquality(x, y);
				}
				return num;
			}
		}
	}
}
