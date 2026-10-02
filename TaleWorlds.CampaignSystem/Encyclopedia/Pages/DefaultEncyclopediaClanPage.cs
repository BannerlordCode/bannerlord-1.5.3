using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Encyclopedia.Pages
{
	// Token: 0x02000187 RID: 391
	[EncyclopediaModel(new Type[] { typeof(Clan) })]
	public class DefaultEncyclopediaClanPage : EncyclopediaPage
	{
		// Token: 0x06001C31 RID: 7217 RVA: 0x000911A7 File Offset: 0x0008F3A7
		public DefaultEncyclopediaClanPage()
		{
			base.HomePageOrderIndex = 500;
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x000911BA File Offset: 0x0008F3BA
		protected override IEnumerable<EncyclopediaListItem> InitializeListItems()
		{
			using (IEnumerator<Clan> enumerator = Clan.NonBanditFactions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Clan clan = enumerator.Current;
					if (this.IsValidEncyclopediaItem(clan))
					{
						yield return new EncyclopediaListItem(clan, clan.Name.ToString(), "", clan.StringId, base.GetIdentifier(typeof(Clan)), true, delegate
						{
							InformationManager.ShowTooltip(typeof(Clan), new object[] { clan });
						});
					}
				}
			}
			IEnumerator<Clan> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x000911CC File Offset: 0x0008F3CC
		protected override IEnumerable<EncyclopediaFilterGroup> InitializeFilterItems()
		{
			List<EncyclopediaFilterGroup> list = new List<EncyclopediaFilterGroup>();
			List<EncyclopediaFilterItem> list2 = new List<EncyclopediaFilterItem>();
			list2.Add(new EncyclopediaFilterItem(new TextObject("{=QwpHoMJu}Minor", null), (object f) => ((IFaction)f).IsMinorFaction));
			List<EncyclopediaFilterItem> list3 = list2;
			list.Add(new EncyclopediaFilterGroup(list3, new TextObject("{=zMMqgxb1}Type", null)));
			List<EncyclopediaFilterItem> list4 = new List<EncyclopediaFilterItem>();
			list4.Add(new EncyclopediaFilterItem(new TextObject("{=b8TV0bRy}Has Blood Feud", null), (object f) => ((IFaction)f).IsClan && ((Clan)f).HasBloodFeudWithPlayer));
			List<EncyclopediaFilterItem> list5 = list4;
			list.Add(new EncyclopediaFilterGroup(list5, new TextObject("{=L7wn49Uz}Diplomacy", null)));
			List<EncyclopediaFilterItem> list6 = new List<EncyclopediaFilterItem>();
			list6.Add(new EncyclopediaFilterItem(new TextObject("{=SlubkZ1A}Eliminated", null), (object f) => ((IFaction)f).IsEliminated));
			list6.Add(new EncyclopediaFilterItem(new TextObject("{=YRbSBxqT}Active", null), (object f) => !((IFaction)f).IsEliminated));
			list.Add(new EncyclopediaFilterGroup(list6, new TextObject("{=DXczLzml}Status", null)));
			List<EncyclopediaFilterItem> list7 = new List<EncyclopediaFilterItem>();
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
						list7.Add(new EncyclopediaFilterItem(culture.Name, (object c) => ((IFaction)c).Culture == culture));
					}
				}
			}
			list.Add(new EncyclopediaFilterGroup(list7, GameTexts.FindText("str_culture", null)));
			return list;
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x00091418 File Offset: 0x0008F618
		protected override IEnumerable<EncyclopediaSortController> InitializeSortControllers()
		{
			return new List<EncyclopediaSortController>
			{
				new EncyclopediaSortController(new TextObject("{=qtII2HbK}Wealth", null), new DefaultEncyclopediaClanPage.EncyclopediaListClanWealthComparer()),
				new EncyclopediaSortController(new TextObject("{=cc1d7mkq}Tier", null), new DefaultEncyclopediaClanPage.EncyclopediaListClanTierComparer()),
				new EncyclopediaSortController(GameTexts.FindText("str_strength", null), new DefaultEncyclopediaClanPage.EncyclopediaListClanStrengthComparer()),
				new EncyclopediaSortController(GameTexts.FindText("str_fiefs", null), new DefaultEncyclopediaClanPage.EncyclopediaListClanFiefComparer()),
				new EncyclopediaSortController(GameTexts.FindText("str_members", null), new DefaultEncyclopediaClanPage.EncyclopediaListClanMemberComparer())
			};
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x000914B1 File Offset: 0x0008F6B1
		public override string GetViewFullyQualifiedName()
		{
			return "EncyclopediaClanPage";
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x000914B8 File Offset: 0x0008F6B8
		public override TextObject GetName()
		{
			return GameTexts.FindText("str_clans", null);
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x000914C5 File Offset: 0x0008F6C5
		public override TextObject GetDescriptionText()
		{
			return GameTexts.FindText("str_clan_description", null);
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x000914D2 File Offset: 0x0008F6D2
		public override string GetStringID()
		{
			return "EncyclopediaClan";
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x000914D9 File Offset: 0x0008F6D9
		public override MBObjectBase GetObject(string typeName, string stringID)
		{
			return Campaign.Current.CampaignObjectManager.Find<Clan>(stringID);
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x000914EB File Offset: 0x0008F6EB
		public override bool IsValidEncyclopediaItem(object o)
		{
			return o is IFaction;
		}

		// Token: 0x020005EC RID: 1516
		private class EncyclopediaListClanWealthComparer : DefaultEncyclopediaClanPage.EncyclopediaListClanComparer
		{
			// Token: 0x0600521C RID: 21020 RVA: 0x001918EC File Offset: 0x0018FAEC
			private string GetClanWealthStatusText(Clan _clan)
			{
				string text = string.Empty;
				if (_clan.Leader.Gold < 15000)
				{
					text = new TextObject("{=SixPXaNh}Very Poor", null).ToString();
				}
				else if (_clan.Leader.Gold < 45000)
				{
					text = new TextObject("{=poorWealthStatus}Poor", null).ToString();
				}
				else if (_clan.Leader.Gold < 135000)
				{
					text = new TextObject("{=averageWealthStatus}Average", null).ToString();
				}
				else if (_clan.Leader.Gold < 405000)
				{
					text = new TextObject("{=UbRqC0Yz}Rich", null).ToString();
				}
				else
				{
					text = new TextObject("{=oJmRg2ms}Very Rich", null).ToString();
				}
				return text;
			}

			// Token: 0x0600521D RID: 21021 RVA: 0x001919A8 File Offset: 0x0018FBA8
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareClans(x, y, DefaultEncyclopediaClanPage.EncyclopediaListClanWealthComparer._comparison);
			}

			// Token: 0x0600521E RID: 21022 RVA: 0x001919B8 File Offset: 0x0018FBB8
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Clan clan;
				if ((clan = item.Object as Clan) != null)
				{
					return this.GetClanWealthStatusText(clan);
				}
				Debug.FailedAssert("Unable to get the gold of a non-clan object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaClanPage.cs", "GetComparedValueText", 161);
				return "";
			}

			// Token: 0x0400197F RID: 6527
			private static Func<Clan, Clan, int> _comparison = (Clan c1, Clan c2) => c1.Gold.CompareTo(c2.Gold);
		}

		// Token: 0x020005ED RID: 1517
		private class EncyclopediaListClanTierComparer : DefaultEncyclopediaClanPage.EncyclopediaListClanComparer
		{
			// Token: 0x06005221 RID: 21025 RVA: 0x00191A19 File Offset: 0x0018FC19
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareClans(x, y, DefaultEncyclopediaClanPage.EncyclopediaListClanTierComparer._comparison);
			}

			// Token: 0x06005222 RID: 21026 RVA: 0x00191A28 File Offset: 0x0018FC28
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Clan clan;
				if ((clan = item.Object as Clan) != null)
				{
					return clan.Tier.ToString();
				}
				Debug.FailedAssert("Unable to get the tier of a non-clan object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaClanPage.cs", "GetComparedValueText", 182);
				return "";
			}

			// Token: 0x04001980 RID: 6528
			private static Func<Clan, Clan, int> _comparison = (Clan c1, Clan c2) => c1.Tier.CompareTo(c2.Tier);
		}

		// Token: 0x020005EE RID: 1518
		private class EncyclopediaListClanStrengthComparer : DefaultEncyclopediaClanPage.EncyclopediaListClanComparer
		{
			// Token: 0x06005225 RID: 21029 RVA: 0x00191A90 File Offset: 0x0018FC90
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareClans(x, y, DefaultEncyclopediaClanPage.EncyclopediaListClanStrengthComparer._comparison);
			}

			// Token: 0x06005226 RID: 21030 RVA: 0x00191AA0 File Offset: 0x0018FCA0
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Clan clan;
				if ((clan = item.Object as Clan) != null)
				{
					return ((int)clan.CurrentTotalStrength).ToString();
				}
				Debug.FailedAssert("Unable to get the strength of a non-clan object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaClanPage.cs", "GetComparedValueText", 203);
				return "";
			}

			// Token: 0x04001981 RID: 6529
			private static Func<Clan, Clan, int> _comparison = (Clan c1, Clan c2) => c1.CurrentTotalStrength.CompareTo(c2.CurrentTotalStrength);
		}

		// Token: 0x020005EF RID: 1519
		private class EncyclopediaListClanFiefComparer : DefaultEncyclopediaClanPage.EncyclopediaListClanComparer
		{
			// Token: 0x06005229 RID: 21033 RVA: 0x00191B09 File Offset: 0x0018FD09
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareClans(x, y, DefaultEncyclopediaClanPage.EncyclopediaListClanFiefComparer._comparison);
			}

			// Token: 0x0600522A RID: 21034 RVA: 0x00191B18 File Offset: 0x0018FD18
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Clan clan;
				if ((clan = item.Object as Clan) != null)
				{
					return clan.Fiefs.Count.ToString();
				}
				Debug.FailedAssert("Unable to get the fief count of a non-clan object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaClanPage.cs", "GetComparedValueText", 224);
				return "";
			}

			// Token: 0x04001982 RID: 6530
			private static Func<Clan, Clan, int> _comparison = (Clan c1, Clan c2) => c1.Fiefs.Count.CompareTo(c2.Fiefs.Count);
		}

		// Token: 0x020005F0 RID: 1520
		private class EncyclopediaListClanMemberComparer : DefaultEncyclopediaClanPage.EncyclopediaListClanComparer
		{
			// Token: 0x0600522D RID: 21037 RVA: 0x00191B85 File Offset: 0x0018FD85
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareClans(x, y, DefaultEncyclopediaClanPage.EncyclopediaListClanMemberComparer._comparison);
			}

			// Token: 0x0600522E RID: 21038 RVA: 0x00191B94 File Offset: 0x0018FD94
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				Clan clan;
				if ((clan = item.Object as Clan) != null)
				{
					return clan.Heroes.Count.ToString();
				}
				Debug.FailedAssert("Unable to get members of a non-clan object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaClanPage.cs", "GetComparedValueText", 245);
				return "";
			}

			// Token: 0x04001983 RID: 6531
			private static Func<Clan, Clan, int> _comparison = (Clan c1, Clan c2) => c1.Heroes.Count.CompareTo(c2.Heroes.Count);
		}

		// Token: 0x020005F1 RID: 1521
		public abstract class EncyclopediaListClanComparer : EncyclopediaListItemComparerBase
		{
			// Token: 0x06005231 RID: 21041 RVA: 0x00191C04 File Offset: 0x0018FE04
			public int CompareClans(EncyclopediaListItem x, EncyclopediaListItem y, Func<Clan, Clan, int> comparison)
			{
				Clan clan;
				Clan clan2;
				if ((clan = x.Object as Clan) == null || (clan2 = y.Object as Clan) == null)
				{
					Debug.FailedAssert("Both objects should be clans.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaClanPage.cs", "CompareClans", 260);
					return 0;
				}
				int num = comparison(clan, clan2) * (base.IsAscending ? 1 : (-1));
				if (num == 0)
				{
					return base.ResolveEquality(x, y);
				}
				return num;
			}
		}
	}
}
