using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors.BarterBehaviors
{
	// Token: 0x0200048B RID: 1163
	public class ItemBarterBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004B0D RID: 19213 RVA: 0x0017AB3D File Offset: 0x00178D3D
		public override void RegisterEvents()
		{
			CampaignEvents.BarterablesRequested.AddNonSerializedListener(this, new Action<BarterData>(this.CheckForBarters));
		}

		// Token: 0x06004B0E RID: 19214 RVA: 0x0017AB56 File Offset: 0x00178D56
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06004B0F RID: 19215 RVA: 0x0017AB58 File Offset: 0x00178D58
		public void CheckForBarters(BarterData args)
		{
			CampaignVec2 campaignVec;
			if (args.OffererHero != null)
			{
				campaignVec = args.OffererHero.GetCampaignPosition();
			}
			else if (args.OffererParty != null)
			{
				campaignVec = args.OffererParty.MobileParty.Position;
			}
			else
			{
				campaignVec = args.OtherHero.GetCampaignPosition();
			}
			if (campaignVec.IsValid())
			{
				List<Settlement> closestSettlements = this._distanceCache.GetClosestSettlements(campaignVec.ToVec2());
				if (args.OffererParty != null && args.OtherParty != null)
				{
					for (int i = 0; i < args.OffererParty.ItemRoster.Count; i++)
					{
						ItemRosterElement elementCopyAtIndex = args.OffererParty.ItemRoster.GetElementCopyAtIndex(i);
						if (elementCopyAtIndex.Amount > 0 && elementCopyAtIndex.EquipmentElement.GetBaseValue() > 100)
						{
							int num = this.CalculateAverageItemValueInNearbySettlements(elementCopyAtIndex.EquipmentElement, args.OffererParty, closestSettlements);
							Barterable barterable = new ItemBarterable(args.OffererHero, args.OtherHero, args.OffererParty, args.OtherParty, elementCopyAtIndex, num);
							args.AddBarterable<ItemBarterGroup>(barterable, false);
						}
					}
					for (int j = 0; j < args.OtherParty.ItemRoster.Count; j++)
					{
						ItemRosterElement elementCopyAtIndex2 = args.OtherParty.ItemRoster.GetElementCopyAtIndex(j);
						if (elementCopyAtIndex2.Amount > 0 && elementCopyAtIndex2.EquipmentElement.GetBaseValue() > 100)
						{
							int num2 = this.CalculateAverageItemValueInNearbySettlements(elementCopyAtIndex2.EquipmentElement, args.OtherParty, closestSettlements);
							Barterable barterable2 = new ItemBarterable(args.OtherHero, args.OffererHero, args.OtherParty, args.OffererParty, elementCopyAtIndex2, num2);
							args.AddBarterable<ItemBarterGroup>(barterable2, false);
						}
					}
				}
			}
		}

		// Token: 0x06004B10 RID: 19216 RVA: 0x0017ACFC File Offset: 0x00178EFC
		private int CalculateAverageItemValueInNearbySettlements(EquipmentElement itemRosterElement, PartyBase involvedParty, List<Settlement> nearbySettlements)
		{
			int num = 0;
			if (!nearbySettlements.IsEmpty<Settlement>())
			{
				foreach (Settlement settlement in nearbySettlements)
				{
					num += settlement.Town.GetItemPrice(itemRosterElement, involvedParty.MobileParty, true);
				}
				num /= nearbySettlements.Count;
			}
			return num;
		}

		// Token: 0x040014E3 RID: 5347
		private const int ItemValueThreshold = 100;

		// Token: 0x040014E4 RID: 5348
		private ItemBarterBehavior.SettlementDistanceCache _distanceCache = new ItemBarterBehavior.SettlementDistanceCache();

		// Token: 0x020008C6 RID: 2246
		private class SettlementDistanceCache
		{
			// Token: 0x06006C65 RID: 27749 RVA: 0x001DBFB1 File Offset: 0x001DA1B1
			public SettlementDistanceCache()
			{
				this._latestHeroPosition = new Vec2(-1f, -1f);
				this._sortedSettlements = new List<ItemBarterBehavior.SettlementDistanceCache.SettlementDistancePair>(64);
				this._closestSettlements = new List<Settlement>(3);
			}

			// Token: 0x06006C66 RID: 27750 RVA: 0x001DBFE8 File Offset: 0x001DA1E8
			public List<Settlement> GetClosestSettlements(Vec2 position)
			{
				if (!position.NearlyEquals(this._latestHeroPosition, 1E-05f))
				{
					this._latestHeroPosition = position;
					MBReadOnlyList<Town> allTowns = Campaign.Current.AllTowns;
					int count = allTowns.Count;
					for (int i = 0; i < count; i++)
					{
						Settlement settlement = allTowns[i].Settlement;
						this._sortedSettlements.Add(new ItemBarterBehavior.SettlementDistanceCache.SettlementDistancePair(position.DistanceSquared(settlement.Position.ToVec2()), settlement));
					}
					this._sortedSettlements.Sort();
					this._closestSettlements.Clear();
					this._closestSettlements.Add(this._sortedSettlements[0].Settlement);
					this._closestSettlements.Add(this._sortedSettlements[1].Settlement);
					this._closestSettlements.Add(this._sortedSettlements[2].Settlement);
					this._sortedSettlements.Clear();
				}
				return this._closestSettlements;
			}

			// Token: 0x0400261B RID: 9755
			private Vec2 _latestHeroPosition;

			// Token: 0x0400261C RID: 9756
			private List<ItemBarterBehavior.SettlementDistanceCache.SettlementDistancePair> _sortedSettlements;

			// Token: 0x0400261D RID: 9757
			private List<Settlement> _closestSettlements;

			// Token: 0x02000950 RID: 2384
			private struct SettlementDistancePair : IComparable<ItemBarterBehavior.SettlementDistanceCache.SettlementDistancePair>
			{
				// Token: 0x06006DF4 RID: 28148 RVA: 0x001DDFC7 File Offset: 0x001DC1C7
				public SettlementDistancePair(float distance, Settlement settlement)
				{
					this._distance = distance;
					this.Settlement = settlement;
				}

				// Token: 0x06006DF5 RID: 28149 RVA: 0x001DDFD7 File Offset: 0x001DC1D7
				public int CompareTo(ItemBarterBehavior.SettlementDistanceCache.SettlementDistancePair other)
				{
					if (this._distance == other._distance)
					{
						return 0;
					}
					if (this._distance > other._distance)
					{
						return 1;
					}
					return -1;
				}

				// Token: 0x040027AD RID: 10157
				private float _distance;

				// Token: 0x040027AE RID: 10158
				public Settlement Settlement;
			}
		}
	}
}
