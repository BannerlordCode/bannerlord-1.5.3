using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Encyclopedia.Pages
{
	// Token: 0x0200018A RID: 394
	[EncyclopediaModel(new Type[] { typeof(Hero) })]
	public class DefaultEncyclopediaHeroPage : EncyclopediaPage
	{
		// Token: 0x06001C4E RID: 7246 RVA: 0x000918D7 File Offset: 0x0008FAD7
		public DefaultEncyclopediaHeroPage()
		{
			base.HomePageOrderIndex = 200;
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x000918EA File Offset: 0x0008FAEA
		protected override IEnumerable<EncyclopediaListItem> InitializeListItems()
		{
			int comingOfAge = Campaign.Current.Models.AgeModel.HeroComesOfAge;
			TextObject heroName = new TextObject("{=TauRjAud}{NAME} of the {FACTION}", null);
			string text = string.Empty;
			using (List<Hero>.Enumerator enumerator = Hero.AllAliveHeroes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Hero hero2 = enumerator.Current;
					if (this.IsValidEncyclopediaItem(hero2) && !hero2.IsNotable && hero2.Age >= (float)comingOfAge)
					{
						if (hero2.Clan != null)
						{
							heroName.SetTextVariable("NAME", hero2.FirstName ?? hero2.Name);
							TextObject textObject = heroName;
							string text2 = "FACTION";
							Clan clan = hero2.Clan;
							textObject.SetTextVariable(text2, ((clan != null) ? clan.Name : null) ?? TextObject.GetEmpty());
							text = heroName.ToString();
						}
						else
						{
							text = hero2.Name.ToString();
						}
						yield return new EncyclopediaListItem(hero2, text, "", hero2.StringId, base.GetIdentifier(typeof(Hero)), DefaultEncyclopediaHeroPage.CanPlayerSeeValuesOf(hero2), delegate
						{
							InformationManager.ShowTooltip(typeof(Hero), new object[] { hero2, false });
						});
					}
				}
			}
			List<Hero>.Enumerator enumerator = default(List<Hero>.Enumerator);
			using (List<Hero>.Enumerator enumerator = Hero.DeadOrDisabledHeroes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Hero hero = enumerator.Current;
					if (this.IsValidEncyclopediaItem(hero) && !hero.IsNotable && hero.Age >= (float)comingOfAge)
					{
						if (hero.Clan != null)
						{
							heroName.SetTextVariable("NAME", hero.FirstName ?? hero.Name);
							TextObject textObject2 = heroName;
							string text3 = "FACTION";
							Clan clan2 = hero.Clan;
							textObject2.SetTextVariable(text3, ((clan2 != null) ? clan2.Name : null) ?? TextObject.GetEmpty());
							yield return new EncyclopediaListItem(hero, heroName.ToString(), "", hero.StringId, base.GetIdentifier(typeof(Hero)), DefaultEncyclopediaHeroPage.CanPlayerSeeValuesOf(hero), delegate
							{
								InformationManager.ShowTooltip(typeof(Hero), new object[] { hero, false });
							});
						}
						else
						{
							yield return new EncyclopediaListItem(hero, hero.Name.ToString(), "", hero.StringId, base.GetIdentifier(typeof(Hero)), DefaultEncyclopediaHeroPage.CanPlayerSeeValuesOf(hero), delegate
							{
								InformationManager.ShowTooltip(typeof(Hero), new object[] { hero, false });
							});
						}
					}
				}
			}
			enumerator = default(List<Hero>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x000918FC File Offset: 0x0008FAFC
		protected override IEnumerable<EncyclopediaFilterGroup> InitializeFilterItems()
		{
			List<EncyclopediaFilterGroup> list = new List<EncyclopediaFilterGroup>();
			List<EncyclopediaFilterItem> list2 = new List<EncyclopediaFilterItem>();
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=5xi0t1dD}Met Before", null), (object h) => ((Hero)h).HasMet));
			list.Add(new EncyclopediaFilterGroup(list2, new TextObject("{=BlidMNGT}Relation", null)));
			List<EncyclopediaFilterItem> list3 = new List<EncyclopediaFilterItem>();
			list3.Add(new EncyclopediaFilterItem(new TextObject("{=oAb4NqO5}Male", null), (object h) => !((Hero)h).IsFemale));
			list3.Add(new EncyclopediaFilterItem(new TextObject("{=2YUUGQvG}Female", null), (object h) => ((Hero)h).IsFemale));
			list.Add(new EncyclopediaFilterGroup(list3, new TextObject("{=fGFMqlGz}Gender", null)));
			List<EncyclopediaFilterItem> list4 = new List<EncyclopediaFilterItem>();
			list4.Add(new EncyclopediaFilterItem(new TextObject("{=uvjOVy5P}Dead", null), (object h) => !((Hero)h).IsAlive));
			list4.Add(new EncyclopediaFilterItem(new TextObject("{=3TmLIou4}Alive", null), (object h) => ((Hero)h).IsAlive));
			list.Add(new EncyclopediaFilterGroup(list4, new TextObject("{=DXczLzml}Status", null)));
			List<EncyclopediaFilterItem> list5 = new List<EncyclopediaFilterItem>();
			using (List<CultureObject>.Enumerator enumerator = (from x in Game.Current.ObjectManager.GetObjectTypeList<CultureObject>()
				where x.IsMainCulture
				select x into f
				orderby f.Name.ToString()
				select f).ToList<CultureObject>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CultureObject culture = enumerator.Current;
					if (culture.StringId != "neutral_culture" && !culture.IsBandit)
					{
						list5.Add(new EncyclopediaFilterItem(culture.Name, (object c) => ((Hero)c).Culture == culture));
					}
				}
			}
			list.Add(new EncyclopediaFilterGroup(list5, GameTexts.FindText("str_culture", null)));
			List<EncyclopediaFilterItem> list6 = new List<EncyclopediaFilterItem>();
			list6.Add(new EncyclopediaFilterItem(new TextObject("{=9aaKyNQM}Kingdom Leader", null), (object h) => ((Hero)h).IsKingdomLeader));
			list6.Add(new EncyclopediaFilterItem(new TextObject("{=pqfz386V}Clan Leader", null), (object h) => ((Hero)h).IsClanLeader));
			list6.Add(new EncyclopediaFilterItem(new TextObject("{=4vleNtxb}Lord/Lady", null), (object h) => ((Hero)h).IsLord));
			list6.Add(new EncyclopediaFilterItem(new TextObject("{=vmMqs3Ck}Noble", null), delegate(object h)
			{
				Clan clan = ((Hero)h).Clan;
				return clan != null && clan.IsNoble;
			}));
			list6.Add(new EncyclopediaFilterItem(new TextObject("{=FLa5OuyK}Wanderer", null), (object h) => ((Hero)h).IsWanderer));
			list.Add(new EncyclopediaFilterGroup(list6, new TextObject("{=GZxFIeiJ}Occupation", null)));
			List<EncyclopediaFilterItem> list7 = new List<EncyclopediaFilterItem>();
			list7.Add(new EncyclopediaFilterItem(new TextObject("{=qIAgh9VL}Not Married", null), (object h) => ((Hero)h).Spouse == null));
			list7.Add(new EncyclopediaFilterItem(new TextObject("{=xeawD38S}Married", null), (object h) => ((Hero)h).Spouse != null));
			list.Add(new EncyclopediaFilterGroup(list7, new TextObject("{=PMio7set}Marital Status", null)));
			return list;
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x00091D38 File Offset: 0x0008FF38
		protected override IEnumerable<EncyclopediaSortController> InitializeSortControllers()
		{
			return new List<EncyclopediaSortController>
			{
				new EncyclopediaSortController(new TextObject("{=jaaQijQs}Age", null), new DefaultEncyclopediaHeroPage.EncyclopediaListHeroAgeComparer()),
				new EncyclopediaSortController(new TextObject("{=BlidMNGT}Relation", null), new DefaultEncyclopediaHeroPage.EncyclopediaListHeroRelationComparer())
			};
		}

		// Token: 0x06001C52 RID: 7250 RVA: 0x00091D75 File Offset: 0x0008FF75
		public override string GetViewFullyQualifiedName()
		{
			return "EncyclopediaHeroPage";
		}

		// Token: 0x06001C53 RID: 7251 RVA: 0x00091D7C File Offset: 0x0008FF7C
		public override string GetStringID()
		{
			return "EncyclopediaHero";
		}

		// Token: 0x06001C54 RID: 7252 RVA: 0x00091D83 File Offset: 0x0008FF83
		public override TextObject GetName()
		{
			return GameTexts.FindText("str_encyclopedia_heroes", null);
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x00091D90 File Offset: 0x0008FF90
		public override TextObject GetDescriptionText()
		{
			return GameTexts.FindText("str_hero_description", null);
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x00091D9D File Offset: 0x0008FF9D
		public override MBObjectBase GetObject(string typeName, string stringID)
		{
			return Campaign.Current.CampaignObjectManager.Find<Hero>(stringID);
		}

		// Token: 0x06001C57 RID: 7255 RVA: 0x00091DB0 File Offset: 0x0008FFB0
		public override bool IsValidEncyclopediaItem(object o)
		{
			Hero hero = o as Hero;
			if (hero != null && !hero.IsTemplate && hero.IsReady)
			{
				IFaction mapFaction = hero.MapFaction;
				if ((mapFaction == null || !mapFaction.IsBanditFaction) && !hero.CharacterObject.HiddenInEncyclopedia)
				{
					return !hero.HiddenInEncyclopedia;
				}
			}
			return false;
		}

		// Token: 0x06001C58 RID: 7256 RVA: 0x00091E06 File Offset: 0x00090006
		private static bool CanPlayerSeeValuesOf(Hero hero)
		{
			return Campaign.Current.Models.InformationRestrictionModel.DoesPlayerKnowDetailsOf(hero);
		}

		// Token: 0x020005FF RID: 1535
		private class EncyclopediaListHeroAgeComparer : DefaultEncyclopediaHeroPage.EncyclopediaListHeroComparer
		{
			// Token: 0x06005278 RID: 21112 RVA: 0x00192594 File Offset: 0x00190794
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareHeroes(x, y, DefaultEncyclopediaHeroPage.EncyclopediaListHeroAgeComparer._comparison);
			}

			// Token: 0x06005279 RID: 21113 RVA: 0x001925A4 File Offset: 0x001907A4
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Hero hero;
				if ((hero = item.Object as Hero) == null)
				{
					Debug.FailedAssert("Unable to get the age of a non-hero object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaHeroPage.cs", "GetComparedValueText", 180);
					return "";
				}
				if (!DefaultEncyclopediaHeroPage.CanPlayerSeeValuesOf(hero))
				{
					return this._missingValue.ToString();
				}
				return ((int)hero.Age).ToString();
			}

			// Token: 0x040019AC RID: 6572
			private static Func<Hero, Hero, int> _comparison = (Hero h1, Hero h2) => h1.Age.CompareTo(h2.Age);
		}

		// Token: 0x02000600 RID: 1536
		private class EncyclopediaListHeroRelationComparer : DefaultEncyclopediaHeroPage.EncyclopediaListHeroComparer
		{
			// Token: 0x0600527C RID: 21116 RVA: 0x00192621 File Offset: 0x00190821
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareHeroes(x, y, DefaultEncyclopediaHeroPage.EncyclopediaListHeroRelationComparer._comparison);
			}

			// Token: 0x0600527D RID: 21117 RVA: 0x00192630 File Offset: 0x00190830
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Hero hero;
				if ((hero = item.Object as Hero) == null)
				{
					Debug.FailedAssert("Unable to get the relation between a non-hero object and the player.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaHeroPage.cs", "GetComparedValueText", 210);
					return "";
				}
				if (!DefaultEncyclopediaHeroPage.CanPlayerSeeValuesOf(hero))
				{
					return this._missingValue.ToString();
				}
				int num = (int)hero.GetRelationWithPlayer();
				MBTextManager.SetTextVariable("NUMBER", num);
				if (num <= 0)
				{
					return num.ToString();
				}
				return GameTexts.FindText("str_plus_with_number", null).ToString();
			}

			// Token: 0x040019AD RID: 6573
			private static Func<Hero, Hero, int> _comparison = (Hero h1, Hero h2) => h1.GetRelationWithPlayer().CompareTo(h2.GetRelationWithPlayer());
		}

		// Token: 0x02000601 RID: 1537
		public abstract class EncyclopediaListHeroComparer : EncyclopediaListItemComparerBase
		{
			// Token: 0x06005280 RID: 21120 RVA: 0x001926D0 File Offset: 0x001908D0
			protected bool CompareVisibility(Hero h1, Hero h2, out int comparisonResult)
			{
				bool flag = DefaultEncyclopediaHeroPage.CanPlayerSeeValuesOf(h1);
				bool flag2 = DefaultEncyclopediaHeroPage.CanPlayerSeeValuesOf(h2);
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

			// Token: 0x06005281 RID: 21121 RVA: 0x00192720 File Offset: 0x00190920
			protected int CompareHeroes(EncyclopediaListItem x, EncyclopediaListItem y, Func<Hero, Hero, int> comparison)
			{
				Hero hero;
				Hero hero2;
				if ((hero = x.Object as Hero) == null || (hero2 = y.Object as Hero) == null)
				{
					Debug.FailedAssert("Both objects should be heroes.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaHeroPage.cs", "CompareHeroes", 259);
					return 0;
				}
				int num;
				if (this.CompareVisibility(hero, hero2, out num))
				{
					if (num == 0)
					{
						return base.ResolveEquality(x, y);
					}
					return num * (base.IsAscending ? 1 : (-1));
				}
				else
				{
					int num2 = comparison(hero, hero2) * (base.IsAscending ? 1 : (-1));
					if (num2 == 0)
					{
						return base.ResolveEquality(x, y);
					}
					return num2;
				}
			}

			// Token: 0x0200090B RID: 2315
			// (Invoke) Token: 0x06006CD7 RID: 27863
			protected delegate bool HeroVisibilityComparerDelegate(Hero h1, Hero h2, out int comparisonResult);
		}
	}
}
