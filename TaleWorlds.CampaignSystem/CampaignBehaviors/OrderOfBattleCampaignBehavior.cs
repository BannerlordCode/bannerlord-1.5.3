using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000442 RID: 1090
	public class OrderOfBattleCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x06004615 RID: 17941 RVA: 0x00154E6C File Offset: 0x0015306C
		public OrderOfBattleCampaignBehavior()
		{
			this._siegeFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
			this._siegeArmyFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
			this._fieldBattleFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
			this._fieldBattleArmyFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
		}

		// Token: 0x06004616 RID: 17942 RVA: 0x00154EA0 File Offset: 0x001530A0
		public override void RegisterEvents()
		{
			CampaignEvents.OnHeroUnregisteredEvent.AddNonSerializedListener(this, new Action<Hero>(this.OnHeroUnregistered));
		}

		// Token: 0x06004617 RID: 17943 RVA: 0x00154EBC File Offset: 0x001530BC
		public override void SyncData(IDataStore dataStore)
		{
			if (dataStore.SyncData<List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>>("_siegeFormationInfos", ref this._siegeFormationInfos) && this._siegeFormationInfos == null)
			{
				this._siegeFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
			}
			if (dataStore.SyncData<List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>>("_siegeArmyFormationInfos", ref this._siegeArmyFormationInfos) && this._siegeArmyFormationInfos == null)
			{
				this._siegeArmyFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
			}
			if (dataStore.SyncData<List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>>("_formationInfos", ref this._fieldBattleFormationInfos) && this._fieldBattleFormationInfos == null)
			{
				this._fieldBattleFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
			}
			if (dataStore.SyncData<List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>>("_fieldBattleArmyFormationInfos", ref this._fieldBattleArmyFormationInfos) && this._fieldBattleArmyFormationInfos == null)
			{
				this._fieldBattleArmyFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>();
			}
		}

		// Token: 0x06004618 RID: 17944 RVA: 0x00154F64 File Offset: 0x00153164
		public OrderOfBattleCampaignBehavior.OrderOfBattleFormationData GetFormationDataAtIndex(int formationIndex, bool isSiegeBattle, bool isInArmy)
		{
			if (isSiegeBattle)
			{
				if (isInArmy)
				{
					if (this._siegeArmyFormationInfos.Count > formationIndex)
					{
						return this._siegeArmyFormationInfos[formationIndex];
					}
					return null;
				}
				else
				{
					if (this._siegeFormationInfos.Count > formationIndex)
					{
						return this._siegeFormationInfos[formationIndex];
					}
					return null;
				}
			}
			else if (isInArmy)
			{
				if (this._fieldBattleArmyFormationInfos.Count > formationIndex)
				{
					return this._fieldBattleArmyFormationInfos[formationIndex];
				}
				return null;
			}
			else
			{
				if (this._fieldBattleFormationInfos.Count > formationIndex)
				{
					return this._fieldBattleFormationInfos[formationIndex];
				}
				return null;
			}
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x00154FED File Offset: 0x001531ED
		public void SetFormationInfos(List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData> formationInfos, bool isSiegeBattle, bool isInArmy)
		{
			if (isSiegeBattle)
			{
				if (isInArmy)
				{
					this._siegeArmyFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>(formationInfos);
					return;
				}
				this._siegeFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>(formationInfos);
				return;
			}
			else
			{
				if (isInArmy)
				{
					this._fieldBattleArmyFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>(formationInfos);
					return;
				}
				this._fieldBattleFormationInfos = new List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData>(formationInfos);
				return;
			}
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x0015502C File Offset: 0x0015322C
		private void OnHeroUnregistered(Hero hero)
		{
			int i = this._siegeFormationInfos.Count - 1;
			Func<Hero, bool> <>9__0;
			while (i >= 0)
			{
				OrderOfBattleCampaignBehavior.OrderOfBattleFormationData orderOfBattleFormationData = this._siegeFormationInfos[i];
				if (orderOfBattleFormationData.Captain == hero)
				{
					goto IL_0055;
				}
				Hero[] heroTroops = orderOfBattleFormationData.HeroTroops;
				if (heroTroops != null && heroTroops.Contains(hero))
				{
					goto IL_0055;
				}
				IL_00D3:
				i--;
				continue;
				IL_0055:
				Hero[] heroTroops2 = orderOfBattleFormationData.HeroTroops;
				Hero[] array;
				if (heroTroops2 == null)
				{
					array = null;
				}
				else
				{
					Func<Hero, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = (Hero t) => t != hero);
					}
					array = heroTroops2.Where<Hero>(func).ToArray<Hero>();
				}
				Hero[] array2 = array;
				Hero hero2 = ((orderOfBattleFormationData.Captain == hero) ? null : orderOfBattleFormationData.Captain);
				this._siegeFormationInfos[i] = new OrderOfBattleCampaignBehavior.OrderOfBattleFormationData(hero2, array2, orderOfBattleFormationData.FormationClass, orderOfBattleFormationData.PrimaryClassWeight, orderOfBattleFormationData.SecondaryClassWeight, orderOfBattleFormationData.Filters);
				goto IL_00D3;
			}
			int j = this._fieldBattleFormationInfos.Count - 1;
			Func<Hero, bool> <>9__1;
			while (j >= 0)
			{
				OrderOfBattleCampaignBehavior.OrderOfBattleFormationData orderOfBattleFormationData2 = this._fieldBattleFormationInfos[j];
				if (orderOfBattleFormationData2.Captain == hero)
				{
					goto IL_012E;
				}
				Hero[] heroTroops3 = orderOfBattleFormationData2.HeroTroops;
				if (heroTroops3 != null && heroTroops3.Contains(hero))
				{
					goto IL_012E;
				}
				IL_01B6:
				j--;
				continue;
				IL_012E:
				Hero[] heroTroops4 = orderOfBattleFormationData2.HeroTroops;
				Hero[] array3;
				if (heroTroops4 == null)
				{
					array3 = null;
				}
				else
				{
					Func<Hero, bool> func2;
					if ((func2 = <>9__1) == null)
					{
						func2 = (<>9__1 = (Hero t) => t != hero);
					}
					array3 = heroTroops4.Where<Hero>(func2).ToArray<Hero>();
				}
				Hero[] array4 = array3;
				Hero hero3 = ((orderOfBattleFormationData2.Captain == hero) ? null : orderOfBattleFormationData2.Captain);
				this._fieldBattleFormationInfos[j] = new OrderOfBattleCampaignBehavior.OrderOfBattleFormationData(hero3, array4, orderOfBattleFormationData2.FormationClass, orderOfBattleFormationData2.PrimaryClassWeight, orderOfBattleFormationData2.SecondaryClassWeight, orderOfBattleFormationData2.Filters);
				goto IL_01B6;
			}
		}

		// Token: 0x04001429 RID: 5161
		private List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData> _siegeFormationInfos;

		// Token: 0x0400142A RID: 5162
		private List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData> _siegeArmyFormationInfos;

		// Token: 0x0400142B RID: 5163
		private List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData> _fieldBattleFormationInfos;

		// Token: 0x0400142C RID: 5164
		private List<OrderOfBattleCampaignBehavior.OrderOfBattleFormationData> _fieldBattleArmyFormationInfos;

		// Token: 0x02000877 RID: 2167
		public class OrderOfBattleFormationData
		{
			// Token: 0x06006B08 RID: 27400 RVA: 0x001DA620 File Offset: 0x001D8820
			public OrderOfBattleFormationData(Hero captain, Hero[] heroTroops, DeploymentFormationClass formationClass, int primaryWeight, int secondaryWeight, Dictionary<FormationFilterType, bool> filters)
			{
				this.Captain = captain;
				this.HeroTroops = heroTroops;
				this.FormationClass = formationClass;
				this.PrimaryClassWeight = primaryWeight;
				this.SecondaryClassWeight = secondaryWeight;
				this.Filters = new Dictionary<FormationFilterType, bool>();
				foreach (FormationFilterType formationFilterType in filters.Keys)
				{
					this.Filters.Add(formationFilterType, filters[formationFilterType]);
				}
			}

			// Token: 0x06006B09 RID: 27401 RVA: 0x001DA6B8 File Offset: 0x001D88B8
			internal static void AutoGeneratedStaticCollectObjectsOrderOfBattleFormationData(object o, List<object> collectedObjects)
			{
				((OrderOfBattleCampaignBehavior.OrderOfBattleFormationData)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
			}

			// Token: 0x06006B0A RID: 27402 RVA: 0x001DA6C6 File Offset: 0x001D88C6
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
				collectedObjects.Add(this.Captain);
				collectedObjects.Add(this.Filters);
				collectedObjects.Add(this.HeroTroops);
			}

			// Token: 0x06006B0B RID: 27403 RVA: 0x001DA6EC File Offset: 0x001D88EC
			internal static object AutoGeneratedGetMemberValueCaptain(object o)
			{
				return ((OrderOfBattleCampaignBehavior.OrderOfBattleFormationData)o).Captain;
			}

			// Token: 0x06006B0C RID: 27404 RVA: 0x001DA6F9 File Offset: 0x001D88F9
			internal static object AutoGeneratedGetMemberValueFormationClass(object o)
			{
				return ((OrderOfBattleCampaignBehavior.OrderOfBattleFormationData)o).FormationClass;
			}

			// Token: 0x06006B0D RID: 27405 RVA: 0x001DA70B File Offset: 0x001D890B
			internal static object AutoGeneratedGetMemberValuePrimaryClassWeight(object o)
			{
				return ((OrderOfBattleCampaignBehavior.OrderOfBattleFormationData)o).PrimaryClassWeight;
			}

			// Token: 0x06006B0E RID: 27406 RVA: 0x001DA71D File Offset: 0x001D891D
			internal static object AutoGeneratedGetMemberValueSecondaryClassWeight(object o)
			{
				return ((OrderOfBattleCampaignBehavior.OrderOfBattleFormationData)o).SecondaryClassWeight;
			}

			// Token: 0x06006B0F RID: 27407 RVA: 0x001DA72F File Offset: 0x001D892F
			internal static object AutoGeneratedGetMemberValueFilters(object o)
			{
				return ((OrderOfBattleCampaignBehavior.OrderOfBattleFormationData)o).Filters;
			}

			// Token: 0x06006B10 RID: 27408 RVA: 0x001DA73C File Offset: 0x001D893C
			internal static object AutoGeneratedGetMemberValueHeroTroops(object o)
			{
				return ((OrderOfBattleCampaignBehavior.OrderOfBattleFormationData)o).HeroTroops;
			}

			// Token: 0x040024D8 RID: 9432
			[SaveableField(1)]
			public readonly Hero Captain;

			// Token: 0x040024D9 RID: 9433
			[SaveableField(2)]
			public readonly DeploymentFormationClass FormationClass;

			// Token: 0x040024DA RID: 9434
			[SaveableField(3)]
			public readonly int PrimaryClassWeight;

			// Token: 0x040024DB RID: 9435
			[SaveableField(4)]
			public readonly int SecondaryClassWeight;

			// Token: 0x040024DC RID: 9436
			[SaveableField(5)]
			public readonly Dictionary<FormationFilterType, bool> Filters;

			// Token: 0x040024DD RID: 9437
			[SaveableField(6)]
			public readonly Hero[] HeroTroops;
		}
	}
}
