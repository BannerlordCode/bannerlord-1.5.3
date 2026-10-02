using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Encyclopedia.Pages
{
	// Token: 0x0200018C RID: 396
	[EncyclopediaModel(new Type[] { typeof(ShipHull) })]
	public class DefaultEncyclopediaShipPage : EncyclopediaPage
	{
		// Token: 0x06001C63 RID: 7267 RVA: 0x00092138 File Offset: 0x00090338
		public DefaultEncyclopediaShipPage()
		{
			base.HomePageOrderIndex = 200;
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x0009214C File Offset: 0x0009034C
		public override bool IsRelevant()
		{
			MBObjectManager instance = MBObjectManager.Instance;
			if (instance == null)
			{
				return false;
			}
			MBReadOnlyList<ShipHull> objectTypeList = instance.GetObjectTypeList<ShipHull>();
			int? num = ((objectTypeList != null) ? new int?(objectTypeList.Count) : null);
			int num2 = 0;
			return (num.GetValueOrDefault() > num2) & (num != null);
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x00092197 File Offset: 0x00090397
		protected override IEnumerable<EncyclopediaListItem> InitializeListItems()
		{
			List<ShipHull> list = new List<ShipHull>();
			foreach (CultureObject cultureObject in MBObjectManager.Instance.GetObjectTypeList<CultureObject>())
			{
				if (cultureObject.IsMainCulture && cultureObject.AvailableShipHulls.Count > 0)
				{
					list.AddRange(cultureObject.AvailableShipHulls);
				}
			}
			MBReadOnlyList<ShipHull> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<ShipHull>();
			list.AddRange(objectTypeList.Where<ShipHull>((ShipHull x) => x.StringId == "fishing_ship" || x.StringId == "southern_fishing_ship"));
			using (IEnumerator<ShipHull> enumerator2 = list.Distinct<ShipHull>().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ShipHull shipHull = enumerator2.Current;
					TextObject textObject = null;
					if (this.IsValidEncyclopediaItem(shipHull))
					{
						textObject = shipHull.Name;
					}
					string text = ((textObject != null) ? textObject.ToString() : null) ?? string.Empty;
					yield return new EncyclopediaListItem(shipHull, text, "", shipHull.StringId, base.GetIdentifier(typeof(ShipHull)), DefaultEncyclopediaShipPage.CanPlayerSeeValuesOf(shipHull), delegate
					{
						InformationManager.ShowTooltip(typeof(ShipHull), new object[] { shipHull });
					});
				}
			}
			IEnumerator<ShipHull> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x000921A8 File Offset: 0x000903A8
		protected override IEnumerable<EncyclopediaFilterGroup> InitializeFilterItems()
		{
			List<EncyclopediaFilterGroup> list = new List<EncyclopediaFilterGroup>();
			List<EncyclopediaFilterItem> list2 = new List<EncyclopediaFilterItem>();
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
						list2.Add(new EncyclopediaFilterItem(culture.Name, (object c) => culture.AvailableShipHulls.Contains((ShipHull)c)));
					}
				}
			}
			list.Add(new EncyclopediaFilterGroup(list2, GameTexts.FindText("str_culture", null)));
			List<EncyclopediaFilterItem> list3 = new List<EncyclopediaFilterItem>();
			list3.Add(new EncyclopediaFilterItem(GameTexts.FindText("str_ship_type", "heavy"), delegate(object s)
			{
				ShipHull shipHull;
				return (shipHull = s as ShipHull) != null && shipHull.Type == ShipHull.ShipType.Heavy;
			}));
			list3.Add(new EncyclopediaFilterItem(GameTexts.FindText("str_ship_type", "medium"), delegate(object s)
			{
				ShipHull shipHull2;
				return (shipHull2 = s as ShipHull) != null && shipHull2.Type == ShipHull.ShipType.Medium;
			}));
			list3.Add(new EncyclopediaFilterItem(GameTexts.FindText("str_ship_type", "light"), delegate(object s)
			{
				ShipHull shipHull3;
				return (shipHull3 = s as ShipHull) != null && shipHull3.Type == ShipHull.ShipType.Light;
			}));
			List<EncyclopediaFilterItem> list4 = list3;
			list.Add(new EncyclopediaFilterGroup(list4, new TextObject("{=sqdzHOPe}Class", null)));
			List<EncyclopediaFilterItem> list5 = new List<EncyclopediaFilterItem>
			{
				new EncyclopediaFilterItem(new TextObject("{=bXJLb0BE}Hybrid", null), delegate(object s)
				{
					ShipHull shipHull4;
					MissionShipObject @object;
					return (shipHull4 = s as ShipHull) != null && (@object = MBObjectManager.Instance.GetObject<MissionShipObject>(shipHull4.MissionShipObjectId)) != null && this.HasSailOfType(@object, SailType.Square) && this.HasSailOfType(@object, SailType.Lateen);
				}),
				new EncyclopediaFilterItem(new TextObject("{=kNxD2oer}Lateen", null), delegate(object s)
				{
					ShipHull shipHull5;
					return (shipHull5 = s as ShipHull) != null && this.HasSailOfType(MBObjectManager.Instance.GetObject<MissionShipObject>(shipHull5.MissionShipObjectId), SailType.Lateen);
				}),
				new EncyclopediaFilterItem(new TextObject("{=squareSail}Square", null), delegate(object s)
				{
					ShipHull shipHull6;
					return (shipHull6 = s as ShipHull) != null && this.HasSailOfType(MBObjectManager.Instance.GetObject<MissionShipObject>(shipHull6.MissionShipObjectId), SailType.Square);
				})
			};
			list.Add(new EncyclopediaFilterGroup(list5, new TextObject("{=UIb3IW3f}Sail Type", null)));
			return list;
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x00092414 File Offset: 0x00090614
		private bool HasSailOfType(MissionShipObject ship, SailType sailType)
		{
			return ship != null && ship.HasSails && ship.Sails.Any<ShipSail>((ShipSail x) => x.Type == sailType);
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x00092454 File Offset: 0x00090654
		protected override IEnumerable<EncyclopediaSortController> InitializeSortControllers()
		{
			return new List<EncyclopediaSortController>
			{
				new EncyclopediaSortController(new TextObject("{=sqdzHOPe}Class", null), new DefaultEncyclopediaShipPage.EncyclopediaListShipClassComparer()),
				new EncyclopediaSortController(new TextObject("{=UbZL2BJQ}Hitpoints", null), new DefaultEncyclopediaShipPage.EncyclopediaListShipHealthComparer()),
				new EncyclopediaSortController(new TextObject("{=FQ2m5e5E}Slots", null), new DefaultEncyclopediaShipPage.EncyclopediaListShipSlotCountComparer())
			};
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x000924B7 File Offset: 0x000906B7
		public override string GetViewFullyQualifiedName()
		{
			return "EncyclopediaShipPage";
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000924BE File Offset: 0x000906BE
		public override string GetStringID()
		{
			return "EncyclopediaShip";
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x000924C5 File Offset: 0x000906C5
		public override TextObject GetName()
		{
			return GameTexts.FindText("str_encyclopedia_ships", null);
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000924D2 File Offset: 0x000906D2
		public override MBObjectBase GetObject(string typeName, string stringID)
		{
			return MBObjectManager.Instance.GetObject<ShipHull>(stringID);
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000924E0 File Offset: 0x000906E0
		public override bool IsValidEncyclopediaItem(object o)
		{
			ShipHull shipHull;
			return (shipHull = o as ShipHull) != null && shipHull.IsReady && shipHull.IsInitialized;
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x00092509 File Offset: 0x00090709
		private static bool CanPlayerSeeValuesOf(ShipHull shipHull)
		{
			return true;
		}

		// Token: 0x02000612 RID: 1554
		private class EncyclopediaListShipClassComparer : DefaultEncyclopediaShipPage.EncyclopediaListShipComparer
		{
			// Token: 0x060052D5 RID: 21205 RVA: 0x00193810 File Offset: 0x00191A10
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareShips(x, y, DefaultEncyclopediaShipPage.EncyclopediaListShipClassComparer._comparison);
			}

			// Token: 0x060052D6 RID: 21206 RVA: 0x00193820 File Offset: 0x00191A20
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				ShipHull shipHull;
				if ((shipHull = item.Object as ShipHull) == null)
				{
					Debug.FailedAssert("Unable to get the class of a ship object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaShipPage.cs", "GetComparedValueText", 164);
					return "";
				}
				if (!DefaultEncyclopediaShipPage.CanPlayerSeeValuesOf(shipHull))
				{
					return this._missingValue.ToString();
				}
				return shipHull.Type.ToString();
			}

			// Token: 0x040019D4 RID: 6612
			private static Func<ShipHull, ShipHull, int> _comparison = (ShipHull s1, ShipHull s2) => s1.Type.CompareTo(s2.Type);
		}

		// Token: 0x02000613 RID: 1555
		private class EncyclopediaListShipSlotCountComparer : DefaultEncyclopediaShipPage.EncyclopediaListShipComparer
		{
			// Token: 0x060052D9 RID: 21209 RVA: 0x001938A2 File Offset: 0x00191AA2
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareShips(x, y, DefaultEncyclopediaShipPage.EncyclopediaListShipSlotCountComparer._comparison);
			}

			// Token: 0x060052DA RID: 21210 RVA: 0x001938B4 File Offset: 0x00191AB4
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				ShipHull shipHull;
				if ((shipHull = item.Object as ShipHull) == null)
				{
					Debug.FailedAssert("Unable to get the availableSlotCount of a ship object.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaShipPage.cs", "GetComparedValueText", 192);
					return "";
				}
				if (!DefaultEncyclopediaShipPage.CanPlayerSeeValuesOf(shipHull))
				{
					return this._missingValue.ToString();
				}
				return shipHull.AvailableSlots.Count.ToString();
			}

			// Token: 0x040019D5 RID: 6613
			private static Func<ShipHull, ShipHull, int> _comparison = (ShipHull s1, ShipHull s2) => s1.AvailableSlots.Count.CompareTo(s2.AvailableSlots.Count);
		}

		// Token: 0x02000614 RID: 1556
		private class EncyclopediaListShipHealthComparer : DefaultEncyclopediaShipPage.EncyclopediaListShipComparer
		{
			// Token: 0x060052DD RID: 21213 RVA: 0x00193935 File Offset: 0x00191B35
			public override int Compare(EncyclopediaListItem x, EncyclopediaListItem y)
			{
				return base.CompareShips(x, y, DefaultEncyclopediaShipPage.EncyclopediaListShipHealthComparer._comparison);
			}

			// Token: 0x060052DE RID: 21214 RVA: 0x00193944 File Offset: 0x00191B44
			public override string GetComparedValueText(EncyclopediaListItem item)
			{
				ShipHull shipHull;
				if ((shipHull = item.Object as ShipHull) == null)
				{
					Debug.FailedAssert("Unable to get the hitPoints between a ship object and the player.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaShipPage.cs", "GetComparedValueText", 222);
					return "";
				}
				if (!DefaultEncyclopediaShipPage.CanPlayerSeeValuesOf(shipHull))
				{
					return this._missingValue.ToString();
				}
				int maxHitPoints = shipHull.MaxHitPoints;
				MBTextManager.SetTextVariable("NUMBER", maxHitPoints);
				return maxHitPoints.ToString();
			}

			// Token: 0x040019D6 RID: 6614
			private static Func<ShipHull, ShipHull, int> _comparison = (ShipHull s1, ShipHull s2) => s1.MaxHitPoints.CompareTo(s2.MaxHitPoints);
		}

		// Token: 0x02000615 RID: 1557
		public abstract class EncyclopediaListShipComparer : EncyclopediaListItemComparerBase
		{
			// Token: 0x060052E1 RID: 21217 RVA: 0x001939CC File Offset: 0x00191BCC
			protected bool CompareVisibility(ShipHull s1, ShipHull s2, out int comparisonResult)
			{
				bool flag = DefaultEncyclopediaShipPage.CanPlayerSeeValuesOf(s1);
				bool flag2 = DefaultEncyclopediaShipPage.CanPlayerSeeValuesOf(s2);
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

			// Token: 0x060052E2 RID: 21218 RVA: 0x00193A1C File Offset: 0x00191C1C
			protected int CompareShips(EncyclopediaListItem x, EncyclopediaListItem y, Func<ShipHull, ShipHull, int> comparison)
			{
				ShipHull shipHull;
				ShipHull shipHull2;
				if ((shipHull = x.Object as ShipHull) == null || (shipHull2 = y.Object as ShipHull) == null)
				{
					Debug.FailedAssert("Both objects should be shipHull.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\Pages\\DefaultEncyclopediaShipPage.cs", "CompareShips", 271);
					return 0;
				}
				int num;
				if (this.CompareVisibility(shipHull, shipHull2, out num))
				{
					if (num == 0)
					{
						return base.ResolveEquality(x, y);
					}
					return num * (base.IsAscending ? 1 : (-1));
				}
				else
				{
					int num2 = comparison(shipHull, shipHull2) * (base.IsAscending ? 1 : (-1));
					if (num2 == 0)
					{
						return base.ResolveEquality(x, y);
					}
					return num2;
				}
			}

			// Token: 0x02000910 RID: 2320
			// (Invoke) Token: 0x06006CE8 RID: 27880
			protected delegate bool ShipVisibilityComparerDelegate(ShipHull s1, ShipHull s2, out int comparisonResult);
		}
	}
}
