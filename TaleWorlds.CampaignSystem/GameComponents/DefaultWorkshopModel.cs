using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000171 RID: 369
	public class DefaultWorkshopModel : WorkshopModel
	{
		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x06001BB6 RID: 7094 RVA: 0x00090099 File Offset: 0x0008E299
		public override int WarehouseCapacity
		{
			get
			{
				return 6000;
			}
		}

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x06001BB7 RID: 7095 RVA: 0x000900A0 File Offset: 0x0008E2A0
		public override int DaysForPlayerSaveWorkshopFromBankruptcy
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06001BB8 RID: 7096 RVA: 0x000900A3 File Offset: 0x0008E2A3
		public override int CapitalLowLimit
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06001BB9 RID: 7097 RVA: 0x000900AA File Offset: 0x0008E2AA
		public override int InitialCapital
		{
			get
			{
				return 10000;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06001BBA RID: 7098 RVA: 0x000900B1 File Offset: 0x0008E2B1
		public override int DailyExpense
		{
			get
			{
				return 100;
			}
		}

		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06001BBB RID: 7099 RVA: 0x000900B5 File Offset: 0x0008E2B5
		public override int DefaultWorkshopCountInSettlement
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x06001BBC RID: 7100 RVA: 0x000900B8 File Offset: 0x0008E2B8
		public override int MaximumWorkshopsPlayerCanHave
		{
			get
			{
				return this.GetMaxWorkshopCountForClanTier(Campaign.Current.Models.ClanTierModel.MaxClanTier);
			}
		}

		// Token: 0x06001BBD RID: 7101 RVA: 0x000900D4 File Offset: 0x0008E2D4
		public override ExplainedNumber GetEffectiveConversionSpeedOfProduction(Workshop workshop, float speed, bool includeDescription)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(speed, includeDescription, null);
			Settlement settlement = workshop.Settlement;
			if (settlement.OwnerClan.Kingdom != null)
			{
				if (settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.ForgivenessOfDebts))
				{
					explainedNumber.AddFactor(-0.05f, DefaultPolicies.ForgivenessOfDebts.Name);
				}
				if (settlement.OwnerClan.Kingdom.ActivePolicies.Contains(DefaultPolicies.StateMonopolies))
				{
					explainedNumber.AddFactor(-0.1f, DefaultPolicies.StateMonopolies.Name);
				}
			}
			if (settlement.IsFortification)
			{
				settlement.Town.AddEffectOfBuildings(BuildingEffectEnum.WorkshopProduction, ref explainedNumber);
				Hero governor = settlement.Town.Governor;
				if (governor != null && governor.CurrentSettlement == settlement.Town.Settlement)
				{
					TraitEffectHelper.ApplyTraitEffect(governor, DefaultPersonalityTraitEffects.CalculatingWorkshopEffect, ref explainedNumber);
				}
			}
			PerkHelper.AddPerkBonusForTown(DefaultPerks.Trade.MercenaryConnections, settlement.Town, true, ref explainedNumber);
			PerkHelper.AddPerkBonusForCharacter(DefaultPerks.Steward.Sweatshops, BattleEnvironment.Any, workshop.Owner.CharacterObject, true, ref explainedNumber);
			return explainedNumber;
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x000901D8 File Offset: 0x0008E3D8
		public override int GetMaxWorkshopCountForClanTier(int tier)
		{
			return tier + 1;
		}

		// Token: 0x06001BBF RID: 7103 RVA: 0x000901DD File Offset: 0x0008E3DD
		public override int GetCostForPlayer(Workshop workshop)
		{
			return workshop.WorkshopType.EquipmentCost + (int)workshop.Settlement.Town.Prosperity * 4 + this.InitialCapital / 5;
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x00090207 File Offset: 0x0008E407
		public override int GetCostForNotable(Workshop workshop)
		{
			return (workshop.WorkshopType.EquipmentCost + (int)workshop.Settlement.Town.Prosperity / 2 + workshop.Capital) / 2;
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x00090234 File Offset: 0x0008E434
		public override Hero GetNotableOwnerForWorkshop(Workshop workshop)
		{
			List<ValueTuple<Hero, float>> list = new List<ValueTuple<Hero, float>>();
			foreach (Hero hero in workshop.Settlement.Notables)
			{
				if (hero.IsAlive && hero != workshop.Owner)
				{
					int count = hero.OwnedWorkshops.Count;
					float num = Math.Max(hero.Power, 0f) / MathF.Pow(10f, (float)count);
					list.Add(new ValueTuple<Hero, float>(hero, num));
				}
			}
			return MBRandom.ChooseWeighted<Hero>(list);
		}

		// Token: 0x06001BC2 RID: 7106 RVA: 0x000902DC File Offset: 0x0008E4DC
		public override int GetConvertProductionCost(WorkshopType workshopType)
		{
			return workshopType.EquipmentCost;
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x000902E4 File Offset: 0x0008E4E4
		public override bool CanPlayerSellWorkshop(Workshop workshop, out TextObject explanation)
		{
			Campaign.Current.Models.WorkshopModel.GetCostForNotable(workshop);
			Hero notableOwnerForWorkshop = Campaign.Current.Models.WorkshopModel.GetNotableOwnerForWorkshop(workshop);
			explanation = ((notableOwnerForWorkshop == null) ? new TextObject("{=oqPf2Gdp}There isn't any prospective buyer in the town.", null) : null);
			return notableOwnerForWorkshop != null;
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x00090334 File Offset: 0x0008E534
		public override float GetTradeXpPerWarehouseProduction(EquipmentElement production)
		{
			return (float)production.GetBaseValue() * 0.1f;
		}
	}
}
