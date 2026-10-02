using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x0200014C RID: 332
	public class DefaultRaidModel : RaidModel
	{
		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06001A4D RID: 6733 RVA: 0x00084A10 File Offset: 0x00082C10
		private MBReadOnlyList<ValueTuple<ItemObject, float>> CommonLootItemSpawnChances
		{
			get
			{
				if (this._commonLootItems == null)
				{
					List<ValueTuple<ItemObject, float>> list = new List<ValueTuple<ItemObject, float>>
					{
						new ValueTuple<ItemObject, float>(DefaultItems.Hides, 1f),
						new ValueTuple<ItemObject, float>(DefaultItems.HardWood, 1f),
						new ValueTuple<ItemObject, float>(DefaultItems.Tools, 1f),
						new ValueTuple<ItemObject, float>(DefaultItems.Grain, 1f),
						new ValueTuple<ItemObject, float>(Campaign.Current.ObjectManager.GetObject<ItemObject>("linen"), 1f),
						new ValueTuple<ItemObject, float>(Campaign.Current.ObjectManager.GetObject<ItemObject>("sheep"), 1f),
						new ValueTuple<ItemObject, float>(Campaign.Current.ObjectManager.GetObject<ItemObject>("mule"), 1f),
						new ValueTuple<ItemObject, float>(Campaign.Current.ObjectManager.GetObject<ItemObject>("pottery"), 1f)
					};
					for (int i = list.Count - 1; i >= 0; i--)
					{
						ItemObject item = list[i].Item1;
						float num = 100f / ((float)item.Value + 1f);
						list[i] = new ValueTuple<ItemObject, float>(item, num);
					}
					this._commonLootItems = new MBReadOnlyList<ValueTuple<ItemObject, float>>(list);
				}
				return this._commonLootItems;
			}
		}

		// Token: 0x06001A4E RID: 6734 RVA: 0x00084B68 File Offset: 0x00082D68
		public override ExplainedNumber CalculateHitDamage(MapEventSide attackerSide, float settlementHitPoints)
		{
			float num = (MathF.Sqrt((float)attackerSide.TroopCount) + 5f) / 900f;
			ExplainedNumber explainedNumber = new ExplainedNumber(num * (float)CampaignTime.DeltaTime.ToHours, false, null);
			foreach (MapEventParty mapEventParty in attackerSide.Parties)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Roguery.NoRestForTheWicked, mapEventParty.Party.MobileParty, false, ref explainedNumber);
			}
			return explainedNumber;
		}

		// Token: 0x06001A4F RID: 6735 RVA: 0x00084C04 File Offset: 0x00082E04
		public override float GetRaidLootMultiplier(PartyBase receivingParty)
		{
			float num = 1f;
			MobileParty mobileParty = receivingParty.MobileParty;
			Hero hero;
			if (mobileParty == null)
			{
				hero = null;
			}
			else
			{
				Army army = mobileParty.Army;
				if (army == null)
				{
					hero = null;
				}
				else
				{
					MobileParty leaderParty = army.LeaderParty;
					hero = ((leaderParty != null) ? leaderParty.LeaderHero : null);
				}
			}
			Hero hero2 = hero ?? ((mobileParty != null) ? mobileParty.LeaderHero : null);
			if (hero2 != null)
			{
				num += TraitEffectHelper.GetTraitEffectBonus(hero2, DefaultPersonalityTraitEffects.MercyRaidLootEffect);
			}
			return num;
		}

		// Token: 0x06001A50 RID: 6736 RVA: 0x00084C65 File Offset: 0x00082E65
		public override MBReadOnlyList<ValueTuple<ItemObject, float>> GetCommonLootItemScores()
		{
			return this.CommonLootItemSpawnChances;
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x06001A51 RID: 6737 RVA: 0x00084C6D File Offset: 0x00082E6D
		public override int GoldRewardForEachLostHearth
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x040008B3 RID: 2227
		private MBReadOnlyList<ValueTuple<ItemObject, float>> _commonLootItems;
	}
}
