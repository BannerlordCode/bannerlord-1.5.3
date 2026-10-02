using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TaleWorlds.CampaignSystem.BattleWreckages;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;
using TaleWorlds.SaveSystem.Load;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000060 RID: 96
	public class CampaignObjectManager
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x0002883F File Offset: 0x00026A3F
		// (set) Token: 0x0600094A RID: 2378 RVA: 0x00028847 File Offset: 0x00026A47
		[SaveableProperty(80)]
		public MBReadOnlyList<Settlement> Settlements { get; private set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x00028850 File Offset: 0x00026A50
		public MBReadOnlyList<BattleWreckage> Wreckages
		{
			get
			{
				return this._wreckages;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x00028858 File Offset: 0x00026A58
		public MBReadOnlyList<MobileParty> MobileParties
		{
			get
			{
				return this._mobileParties;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x00028860 File Offset: 0x00026A60
		public MBReadOnlyList<MobileParty> CaravanParties
		{
			get
			{
				return this._caravanParties;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x00028868 File Offset: 0x00026A68
		public MBReadOnlyList<MobileParty> PatrolParties
		{
			get
			{
				return this._patrolParties;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x00028870 File Offset: 0x00026A70
		public MBReadOnlyList<MobileParty> MilitiaParties
		{
			get
			{
				return this._militiaParties;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x00028878 File Offset: 0x00026A78
		public MBReadOnlyList<MobileParty> GarrisonParties
		{
			get
			{
				return this._garrisonParties;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x00028880 File Offset: 0x00026A80
		public MBReadOnlyList<MobileParty> BanditParties
		{
			get
			{
				return this._banditParties;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x00028888 File Offset: 0x00026A88
		public MBReadOnlyList<MobileParty> VillagerParties
		{
			get
			{
				return this._villagerParties;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x00028890 File Offset: 0x00026A90
		public MBReadOnlyList<MobileParty> LordParties
		{
			get
			{
				return this._lordParties;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x00028898 File Offset: 0x00026A98
		public MBReadOnlyList<MobileParty> CustomParties
		{
			get
			{
				return this._customParties;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x000288A0 File Offset: 0x00026AA0
		public MBReadOnlyList<MobileParty> PartiesWithoutPartyComponent
		{
			get
			{
				return this._partiesWithoutPartyComponent;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000956 RID: 2390 RVA: 0x000288A8 File Offset: 0x00026AA8
		public MBReadOnlyList<Hero> AliveHeroes
		{
			get
			{
				return this._aliveHeroes;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000957 RID: 2391 RVA: 0x000288B0 File Offset: 0x00026AB0
		public MBReadOnlyList<Hero> DeadOrDisabledHeroes
		{
			get
			{
				return this._deadOrDisabledHeroes;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x000288B8 File Offset: 0x00026AB8
		public MBReadOnlyList<Clan> Clans
		{
			get
			{
				return this._clans;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x000288C0 File Offset: 0x00026AC0
		public MBReadOnlyList<Kingdom> Kingdoms
		{
			get
			{
				return this._kingdoms;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x000288C8 File Offset: 0x00026AC8
		public MBReadOnlyList<IFaction> Factions
		{
			get
			{
				return this._factions;
			}
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x000288D0 File Offset: 0x00026AD0
		public CampaignObjectManager()
		{
			this._objects = new CampaignObjectManager.ICampaignObjectType[6];
			this._mobileParties = new MBList<MobileParty>();
			this._caravanParties = new MBList<MobileParty>();
			this._patrolParties = new MBList<MobileParty>();
			this._militiaParties = new MBList<MobileParty>();
			this._garrisonParties = new MBList<MobileParty>();
			this._customParties = new MBList<MobileParty>();
			this._banditParties = new MBList<MobileParty>();
			this._villagerParties = new MBList<MobileParty>();
			this._lordParties = new MBList<MobileParty>();
			this._partiesWithoutPartyComponent = new MBList<MobileParty>();
			this._deadOrDisabledHeroes = new MBList<Hero>();
			this._aliveHeroes = new MBList<Hero>();
			this._clans = new MBList<Clan>();
			this._kingdoms = new MBList<Kingdom>();
			this._factions = new MBList<IFaction>();
			this._wreckages = new MBList<BattleWreckage>();
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x000289A0 File Offset: 0x00026BA0
		private void InitializeManagerObjectLists()
		{
			this._objects[4] = new CampaignObjectManager.CampaignObjectType<MobileParty>(this._mobileParties);
			this._objects[0] = new CampaignObjectManager.CampaignObjectType<Hero>(this._deadOrDisabledHeroes);
			this._objects[1] = new CampaignObjectManager.CampaignObjectType<Hero>(this._aliveHeroes);
			this._objects[2] = new CampaignObjectManager.CampaignObjectType<Clan>(this._clans);
			this._objects[3] = new CampaignObjectManager.CampaignObjectType<Kingdom>(this._kingdoms);
			this._objects[5] = new CampaignObjectManager.CampaignObjectType<BattleWreckage>(this._wreckages);
			this._objectTypesAndNextIds = new Dictionary<Type, uint>();
			foreach (CampaignObjectManager.ICampaignObjectType campaignObjectType in this._objects)
			{
				uint maxObjectSubId = campaignObjectType.GetMaxObjectSubId();
				uint num;
				if (this._objectTypesAndNextIds.TryGetValue(campaignObjectType.ObjectClass, out num))
				{
					if (num <= maxObjectSubId)
					{
						this._objectTypesAndNextIds[campaignObjectType.ObjectClass] = maxObjectSubId + 1U;
					}
				}
				else
				{
					this._objectTypesAndNextIds.Add(campaignObjectType.ObjectClass, maxObjectSubId + 1U);
				}
			}
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x00028A90 File Offset: 0x00026C90
		[LoadInitializationCallback]
		private void OnLoad(MetaData metaData, ObjectLoadData objectLoadData)
		{
			this._objects = new CampaignObjectManager.ICampaignObjectType[6];
			this._factions = new MBList<IFaction>();
			this._caravanParties = new MBList<MobileParty>();
			this._patrolParties = new MBList<MobileParty>();
			this._militiaParties = new MBList<MobileParty>();
			this._garrisonParties = new MBList<MobileParty>();
			this._customParties = new MBList<MobileParty>();
			this._banditParties = new MBList<MobileParty>();
			this._villagerParties = new MBList<MobileParty>();
			this._lordParties = new MBList<MobileParty>();
			this._partiesWithoutPartyComponent = new MBList<MobileParty>();
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion < ApplicationVersion.FromString("v1.5.0.113738", 0) && this._wreckages == null)
			{
				this._wreckages = new MBList<BattleWreckage>();
			}
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x00028B48 File Offset: 0x00026D48
		internal void PreAfterLoad()
		{
			CampaignObjectManager.ICampaignObjectType[] objects = this._objects;
			for (int i = 0; i < objects.Length; i++)
			{
				objects[i].PreAfterLoad();
			}
		}

		// Token: 0x0600095F RID: 2399 RVA: 0x00028B74 File Offset: 0x00026D74
		internal void AfterLoad()
		{
			CampaignObjectManager.ICampaignObjectType[] objects = this._objects;
			for (int i = 0; i < objects.Length; i++)
			{
				objects[i].AfterLoad();
			}
		}

		// Token: 0x06000960 RID: 2400 RVA: 0x00028BA0 File Offset: 0x00026DA0
		internal void InitializeOnLoad()
		{
			this.Settlements = MBObjectManager.Instance.GetObjectTypeList<Settlement>();
			foreach (Clan clan in this._clans)
			{
				if (!this._factions.Contains(clan))
				{
					this._factions.Add(clan);
				}
			}
			foreach (Kingdom kingdom in this._kingdoms)
			{
				if (!this._factions.Contains(kingdom))
				{
					this._factions.Add(kingdom);
				}
			}
			foreach (MobileParty mobileParty in this._mobileParties)
			{
				mobileParty.UpdatePartyComponentFlags();
				this.AddPartyToAppropriateList(mobileParty);
			}
			this.InitializeManagerObjectLists();
		}

		// Token: 0x06000961 RID: 2401 RVA: 0x00028CC0 File Offset: 0x00026EC0
		internal void InitializeOnNewGame()
		{
			List<Hero> objectTypeList = MBObjectManager.Instance.GetObjectTypeList<Hero>();
			MBReadOnlyList<MobileParty> objectTypeList2 = MBObjectManager.Instance.GetObjectTypeList<MobileParty>();
			MBReadOnlyList<Clan> objectTypeList3 = MBObjectManager.Instance.GetObjectTypeList<Clan>();
			MBReadOnlyList<Kingdom> objectTypeList4 = MBObjectManager.Instance.GetObjectTypeList<Kingdom>();
			this.Settlements = MBObjectManager.Instance.GetObjectTypeList<Settlement>();
			foreach (Hero hero in objectTypeList)
			{
				if (hero.HeroState == Hero.CharacterStates.Dead || hero.HeroState == Hero.CharacterStates.Disabled)
				{
					if (!this._deadOrDisabledHeroes.Contains(hero))
					{
						this._deadOrDisabledHeroes.Add(hero);
					}
				}
				else if (!this._aliveHeroes.Contains(hero))
				{
					this._aliveHeroes.Add(hero);
				}
			}
			foreach (Clan clan in objectTypeList3)
			{
				if (!this._clans.Contains(clan))
				{
					this._clans.Add(clan);
				}
				if (!this._factions.Contains(clan))
				{
					this._factions.Add(clan);
				}
			}
			foreach (Kingdom kingdom in objectTypeList4)
			{
				if (!this._kingdoms.Contains(kingdom))
				{
					this._kingdoms.Add(kingdom);
				}
				if (!this._factions.Contains(kingdom))
				{
					this._factions.Add(kingdom);
				}
			}
			foreach (MobileParty mobileParty in objectTypeList2)
			{
				this._mobileParties.Add(mobileParty);
				this.AddPartyToAppropriateList(mobileParty);
			}
			this.InitializeManagerObjectLists();
			this.InitializeCachedData();
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x00028ECC File Offset: 0x000270CC
		private void InitializeCachedData()
		{
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsVillage)
				{
					settlement.OwnerClan.OnBoundVillageAdded(settlement.Village);
				}
			}
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x00028F30 File Offset: 0x00027130
		internal void AddMobileParty(MobileParty party)
		{
			party.Id = new MBGUID(14U, Campaign.Current.CampaignObjectManager.GetNextUniqueObjectIdOfType<MobileParty>());
			this._mobileParties.Add(party);
			this.OnItemAdded<MobileParty>(CampaignObjectManager.CampaignObjects.MobileParty, party);
			this.AddPartyToAppropriateList(party);
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x00028F69 File Offset: 0x00027169
		internal void RemoveMobileParty(MobileParty party)
		{
			this._mobileParties.Remove(party);
			this.OnItemRemoved<MobileParty>(CampaignObjectManager.CampaignObjects.MobileParty, party);
			this.RemovePartyFromAppropriateList(party);
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x00028F87 File Offset: 0x00027187
		internal void BeforePartyComponentChanged(MobileParty party)
		{
			this.RemovePartyFromAppropriateList(party);
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x00028F90 File Offset: 0x00027190
		internal void AfterPartyComponentChanged(MobileParty party)
		{
			this.AddPartyToAppropriateList(party);
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x00028F99 File Offset: 0x00027199
		internal void AddHero(Hero hero)
		{
			hero.Id = new MBGUID(32U, Campaign.Current.CampaignObjectManager.GetNextUniqueObjectIdOfType<Hero>());
			this.OnHeroAdded(hero);
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x00028FBE File Offset: 0x000271BE
		internal void UnregisterDeadHero(Hero hero)
		{
			this._deadOrDisabledHeroes.Remove(hero);
			this.OnItemRemoved<Hero>(CampaignObjectManager.CampaignObjects.DeadOrDisabledHeroes, hero);
			CampaignEventDispatcher.Instance.OnHeroUnregistered(hero);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x00028FE0 File Offset: 0x000271E0
		private void OnHeroAdded(Hero hero)
		{
			if (hero.HeroState == Hero.CharacterStates.Dead || hero.HeroState == Hero.CharacterStates.Disabled)
			{
				this._deadOrDisabledHeroes.Add(hero);
				this.OnItemAdded<Hero>(CampaignObjectManager.CampaignObjects.DeadOrDisabledHeroes, hero);
				return;
			}
			this._aliveHeroes.Add(hero);
			this.OnItemAdded<Hero>(CampaignObjectManager.CampaignObjects.AliveHeroes, hero);
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x00029020 File Offset: 0x00027220
		internal void HeroStateChanged(Hero hero, Hero.CharacterStates oldState)
		{
			bool flag = oldState == Hero.CharacterStates.Dead || oldState == Hero.CharacterStates.Disabled;
			bool flag2 = hero.HeroState == Hero.CharacterStates.Dead || hero.HeroState == Hero.CharacterStates.Disabled;
			if (flag != flag2)
			{
				if (flag2)
				{
					if (this._aliveHeroes.Contains(hero))
					{
						this._aliveHeroes.Remove(hero);
					}
				}
				else if (this._deadOrDisabledHeroes.Contains(hero))
				{
					this._deadOrDisabledHeroes.Remove(hero);
				}
				this.OnHeroAdded(hero);
			}
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x00029093 File Offset: 0x00027293
		internal void AddWreckage(BattleWreckage battleWreckage)
		{
			battleWreckage.Id = new MBGUID(33U, Campaign.Current.CampaignObjectManager.GetNextUniqueObjectIdOfType<BattleWreckage>());
			this._wreckages.Add(battleWreckage);
			this.OnItemAdded<BattleWreckage>(CampaignObjectManager.CampaignObjects.Wreckage, battleWreckage);
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x000290C5 File Offset: 0x000272C5
		internal void RemoveWreckage(BattleWreckage battleWreckage)
		{
			this._wreckages.Remove(battleWreckage);
			this.OnItemRemoved<BattleWreckage>(CampaignObjectManager.CampaignObjects.Wreckage, battleWreckage);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x000290DC File Offset: 0x000272DC
		internal void AddClan(Clan clan)
		{
			clan.Id = new MBGUID(18U, Campaign.Current.CampaignObjectManager.GetNextUniqueObjectIdOfType<Clan>());
			this._clans.Add(clan);
			this.OnItemAdded<Clan>(CampaignObjectManager.CampaignObjects.Clans, clan);
			this._factions.Add(clan);
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x0002911A File Offset: 0x0002731A
		internal void RemoveClan(Clan clan)
		{
			if (this._clans.Contains(clan))
			{
				this._clans.Remove(clan);
				this.OnItemRemoved<Clan>(CampaignObjectManager.CampaignObjects.Clans, clan);
			}
			if (this._factions.Contains(clan))
			{
				this._factions.Remove(clan);
			}
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0002915A File Offset: 0x0002735A
		internal void AddKingdom(Kingdom kingdom)
		{
			kingdom.Id = new MBGUID(20U, Campaign.Current.CampaignObjectManager.GetNextUniqueObjectIdOfType<Kingdom>());
			this._kingdoms.Add(kingdom);
			this.OnItemAdded<Kingdom>(CampaignObjectManager.CampaignObjects.Kingdoms, kingdom);
			this._factions.Add(kingdom);
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x00029198 File Offset: 0x00027398
		private void AddPartyToAppropriateList(MobileParty party)
		{
			if (party.IsBandit)
			{
				this._banditParties.Add(party);
				return;
			}
			if (party.IsCaravan)
			{
				this._caravanParties.Add(party);
				return;
			}
			if (party.IsPatrolParty)
			{
				this._patrolParties.Add(party);
				return;
			}
			if (party.IsLordParty)
			{
				this._lordParties.Add(party);
				return;
			}
			if (party.IsMilitia)
			{
				this._militiaParties.Add(party);
				return;
			}
			if (party.IsVillager)
			{
				this._villagerParties.Add(party);
				return;
			}
			if (party.IsCustomParty)
			{
				this._customParties.Add(party);
				return;
			}
			if (party.IsGarrison)
			{
				this._garrisonParties.Add(party);
				return;
			}
			this._partiesWithoutPartyComponent.Add(party);
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0002925C File Offset: 0x0002745C
		private void RemovePartyFromAppropriateList(MobileParty party)
		{
			if (party.IsBandit)
			{
				this._banditParties.Remove(party);
				return;
			}
			if (party.IsCaravan)
			{
				this._caravanParties.Remove(party);
				return;
			}
			if (party.IsPatrolParty)
			{
				this._patrolParties.Remove(party);
				return;
			}
			if (party.IsLordParty)
			{
				this._lordParties.Remove(party);
				return;
			}
			if (party.IsMilitia)
			{
				this._militiaParties.Remove(party);
				return;
			}
			if (party.IsVillager)
			{
				this._villagerParties.Remove(party);
				return;
			}
			if (party.IsCustomParty)
			{
				this._customParties.Remove(party);
				return;
			}
			if (party.IsGarrison)
			{
				this._garrisonParties.Remove(party);
				return;
			}
			this._partiesWithoutPartyComponent.Remove(party);
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00029328 File Offset: 0x00027528
		private void OnItemAdded<T>(CampaignObjectManager.CampaignObjects targetList, T obj) where T : MBObjectBase
		{
			CampaignObjectManager.CampaignObjectType<T> campaignObjectType = (CampaignObjectManager.CampaignObjectType<T>)this._objects[(int)targetList];
			if (campaignObjectType != null)
			{
				campaignObjectType.OnItemAdded(obj);
			}
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00029350 File Offset: 0x00027550
		private void OnItemRemoved<T>(CampaignObjectManager.CampaignObjects targetList, T obj) where T : MBObjectBase
		{
			CampaignObjectManager.CampaignObjectType<T> campaignObjectType = (CampaignObjectManager.CampaignObjectType<T>)this._objects[(int)targetList];
			if (campaignObjectType != null)
			{
				campaignObjectType.UnregisterItem(obj);
			}
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00029378 File Offset: 0x00027578
		public T FindFirst<T>(Predicate<T> predicate) where T : MBObjectBase
		{
			foreach (CampaignObjectManager.ICampaignObjectType campaignObjectType in this._objects)
			{
				if (typeof(T) == campaignObjectType.ObjectClass)
				{
					T t = ((CampaignObjectManager.CampaignObjectType<T>)campaignObjectType).FindFirst(predicate);
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x000293D8 File Offset: 0x000275D8
		public MBReadOnlyList<T> FindAll<T>(Predicate<T> predicate) where T : MBObjectBase
		{
			MBList<T> mblist = new MBList<T>();
			foreach (CampaignObjectManager.ICampaignObjectType campaignObjectType in this._objects)
			{
				if (typeof(T) == campaignObjectType.ObjectClass)
				{
					MBReadOnlyList<T> mbreadOnlyList = ((CampaignObjectManager.CampaignObjectType<T>)campaignObjectType).FindAll(predicate);
					if (mbreadOnlyList != null)
					{
						mblist.AddRange(mbreadOnlyList);
					}
				}
			}
			return mblist;
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00029438 File Offset: 0x00027638
		private uint GetNextUniqueObjectIdOfType<T>() where T : MBObjectBase
		{
			uint num;
			if (this._objectTypesAndNextIds.TryGetValue(typeof(T), out num))
			{
				this._objectTypesAndNextIds[typeof(T)] = num + 1U;
			}
			return num;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00029478 File Offset: 0x00027678
		public T Find<T>(string id) where T : MBObjectBase
		{
			foreach (CampaignObjectManager.ICampaignObjectType campaignObjectType in this._objects)
			{
				if (campaignObjectType != null && typeof(T) == campaignObjectType.ObjectClass)
				{
					T t = ((CampaignObjectManager.CampaignObjectType<T>)campaignObjectType).Find(id);
					if (t != null)
					{
						return t;
					}
				}
			}
			return default(T);
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x000294DC File Offset: 0x000276DC
		public string FindNextUniqueStringId<T>(string id) where T : MBObjectBase
		{
			List<CampaignObjectManager.CampaignObjectType<T>> list = new List<CampaignObjectManager.CampaignObjectType<T>>();
			foreach (CampaignObjectManager.ICampaignObjectType campaignObjectType in this._objects)
			{
				if (campaignObjectType != null && typeof(T) == campaignObjectType.ObjectClass)
				{
					list.Add(campaignObjectType as CampaignObjectManager.CampaignObjectType<T>);
				}
			}
			return CampaignObjectManager.CampaignObjectType<T>.FindNextUniqueStringId(list, id);
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00029535 File Offset: 0x00027735
		internal static void AutoGeneratedStaticCollectObjectsCampaignObjectManager(object o, List<object> collectedObjects)
		{
			((CampaignObjectManager)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00029544 File Offset: 0x00027744
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._deadOrDisabledHeroes);
			collectedObjects.Add(this._aliveHeroes);
			collectedObjects.Add(this._clans);
			collectedObjects.Add(this._kingdoms);
			collectedObjects.Add(this._mobileParties);
			collectedObjects.Add(this._wreckages);
			collectedObjects.Add(this.Settlements);
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x000295A5 File Offset: 0x000277A5
		internal static object AutoGeneratedGetMemberValueSettlements(object o)
		{
			return ((CampaignObjectManager)o).Settlements;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x000295B2 File Offset: 0x000277B2
		internal static object AutoGeneratedGetMemberValue_deadOrDisabledHeroes(object o)
		{
			return ((CampaignObjectManager)o)._deadOrDisabledHeroes;
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x000295BF File Offset: 0x000277BF
		internal static object AutoGeneratedGetMemberValue_aliveHeroes(object o)
		{
			return ((CampaignObjectManager)o)._aliveHeroes;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x000295CC File Offset: 0x000277CC
		internal static object AutoGeneratedGetMemberValue_clans(object o)
		{
			return ((CampaignObjectManager)o)._clans;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x000295D9 File Offset: 0x000277D9
		internal static object AutoGeneratedGetMemberValue_kingdoms(object o)
		{
			return ((CampaignObjectManager)o)._kingdoms;
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x000295E6 File Offset: 0x000277E6
		internal static object AutoGeneratedGetMemberValue_mobileParties(object o)
		{
			return ((CampaignObjectManager)o)._mobileParties;
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x000295F3 File Offset: 0x000277F3
		internal static object AutoGeneratedGetMemberValue_wreckages(object o)
		{
			return ((CampaignObjectManager)o)._wreckages;
		}

		// Token: 0x040002D6 RID: 726
		internal const uint HeroObjectManagerTypeID = 32U;

		// Token: 0x040002D7 RID: 727
		internal const uint MobilePartyObjectManagerTypeID = 14U;

		// Token: 0x040002D8 RID: 728
		internal const uint ClanObjectManagerTypeID = 18U;

		// Token: 0x040002D9 RID: 729
		internal const uint KingdomObjectManagerTypeID = 20U;

		// Token: 0x040002DA RID: 730
		internal const uint WreckageObjectManagerTypeID = 33U;

		// Token: 0x040002DB RID: 731
		private CampaignObjectManager.ICampaignObjectType[] _objects;

		// Token: 0x040002DC RID: 732
		private Dictionary<Type, uint> _objectTypesAndNextIds;

		// Token: 0x040002DD RID: 733
		[SaveableField(20)]
		private readonly MBList<Hero> _deadOrDisabledHeroes;

		// Token: 0x040002DE RID: 734
		[SaveableField(30)]
		private readonly MBList<Hero> _aliveHeroes;

		// Token: 0x040002DF RID: 735
		[SaveableField(40)]
		private readonly MBList<Clan> _clans;

		// Token: 0x040002E0 RID: 736
		[SaveableField(50)]
		private readonly MBList<Kingdom> _kingdoms;

		// Token: 0x040002E1 RID: 737
		private MBList<IFaction> _factions;

		// Token: 0x040002E2 RID: 738
		[SaveableField(71)]
		private MBList<MobileParty> _mobileParties;

		// Token: 0x040002E3 RID: 739
		private MBList<MobileParty> _caravanParties;

		// Token: 0x040002E4 RID: 740
		private MBList<MobileParty> _patrolParties;

		// Token: 0x040002E5 RID: 741
		private MBList<MobileParty> _militiaParties;

		// Token: 0x040002E6 RID: 742
		private MBList<MobileParty> _garrisonParties;

		// Token: 0x040002E7 RID: 743
		private MBList<MobileParty> _banditParties;

		// Token: 0x040002E8 RID: 744
		private MBList<MobileParty> _villagerParties;

		// Token: 0x040002E9 RID: 745
		private MBList<MobileParty> _customParties;

		// Token: 0x040002EA RID: 746
		private MBList<MobileParty> _lordParties;

		// Token: 0x040002EB RID: 747
		private MBList<MobileParty> _partiesWithoutPartyComponent;

		// Token: 0x040002ED RID: 749
		[SaveableField(81)]
		private MBList<BattleWreckage> _wreckages;

		// Token: 0x02000541 RID: 1345
		private interface ICampaignObjectType : IEnumerable
		{
			// Token: 0x17000F48 RID: 3912
			// (get) Token: 0x06004F58 RID: 20312
			Type ObjectClass { get; }

			// Token: 0x06004F59 RID: 20313
			void PreAfterLoad();

			// Token: 0x06004F5A RID: 20314
			void AfterLoad();

			// Token: 0x06004F5B RID: 20315
			uint GetMaxObjectSubId();
		}

		// Token: 0x02000542 RID: 1346
		private class CampaignObjectType<T> : CampaignObjectManager.ICampaignObjectType, IEnumerable, IEnumerable<T> where T : MBObjectBase
		{
			// Token: 0x17000F49 RID: 3913
			// (get) Token: 0x06004F5C RID: 20316 RVA: 0x0018CB5A File Offset: 0x0018AD5A
			// (set) Token: 0x06004F5D RID: 20317 RVA: 0x0018CB62 File Offset: 0x0018AD62
			public uint MaxCreatedPostfixIndex { get; private set; }

			// Token: 0x06004F5E RID: 20318 RVA: 0x0018CB6C File Offset: 0x0018AD6C
			public CampaignObjectType(IEnumerable<T> registeredObjects)
			{
				this._registeredObjects = registeredObjects;
				foreach (T t in this._registeredObjects)
				{
					ValueTuple<string, uint> idParts = CampaignObjectManager.CampaignObjectType<T>.GetIdParts(t.StringId);
					if (idParts.Item2 > this.MaxCreatedPostfixIndex)
					{
						this.MaxCreatedPostfixIndex = idParts.Item2;
					}
				}
			}

			// Token: 0x17000F4A RID: 3914
			// (get) Token: 0x06004F5F RID: 20319 RVA: 0x0018CBE8 File Offset: 0x0018ADE8
			Type CampaignObjectManager.ICampaignObjectType.ObjectClass
			{
				get
				{
					return typeof(T);
				}
			}

			// Token: 0x06004F60 RID: 20320 RVA: 0x0018CBF4 File Offset: 0x0018ADF4
			public void PreAfterLoad()
			{
				foreach (T t in this._registeredObjects.ToList<T>())
				{
					t.PreAfterLoadInternal();
				}
			}

			// Token: 0x06004F61 RID: 20321 RVA: 0x0018CC50 File Offset: 0x0018AE50
			public void AfterLoad()
			{
				foreach (T t in this._registeredObjects.ToList<T>())
				{
					t.IsReady = true;
					t.AfterLoadInternal();
				}
			}

			// Token: 0x06004F62 RID: 20322 RVA: 0x0018CCB8 File Offset: 0x0018AEB8
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				return this._registeredObjects.GetEnumerator();
			}

			// Token: 0x06004F63 RID: 20323 RVA: 0x0018CCC5 File Offset: 0x0018AEC5
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._registeredObjects.GetEnumerator();
			}

			// Token: 0x06004F64 RID: 20324 RVA: 0x0018CCD4 File Offset: 0x0018AED4
			public uint GetMaxObjectSubId()
			{
				uint num = 0U;
				foreach (T t in this._registeredObjects)
				{
					if (t.Id.SubId > num)
					{
						num = t.Id.SubId;
					}
				}
				return num;
			}

			// Token: 0x06004F65 RID: 20325 RVA: 0x0018CD48 File Offset: 0x0018AF48
			public void OnItemAdded(T item)
			{
				ValueTuple<string, uint> idParts = CampaignObjectManager.CampaignObjectType<T>.GetIdParts(item.StringId);
				if (idParts.Item2 > this.MaxCreatedPostfixIndex)
				{
					this.MaxCreatedPostfixIndex = idParts.Item2;
				}
				this.RegisterItem(item);
			}

			// Token: 0x06004F66 RID: 20326 RVA: 0x0018CD87 File Offset: 0x0018AF87
			private void RegisterItem(T item)
			{
				item.IsReady = true;
			}

			// Token: 0x06004F67 RID: 20327 RVA: 0x0018CD95 File Offset: 0x0018AF95
			public void UnregisterItem(T item)
			{
				item.IsReady = false;
			}

			// Token: 0x06004F68 RID: 20328 RVA: 0x0018CDA4 File Offset: 0x0018AFA4
			public T Find(string id)
			{
				foreach (T t in this._registeredObjects)
				{
					if (t.StringId == id)
					{
						return t;
					}
				}
				return default(T);
			}

			// Token: 0x06004F69 RID: 20329 RVA: 0x0018CE0C File Offset: 0x0018B00C
			public T FindFirst(Predicate<T> predicate)
			{
				foreach (T t in this._registeredObjects)
				{
					if (predicate(t))
					{
						return t;
					}
				}
				return default(T);
			}

			// Token: 0x06004F6A RID: 20330 RVA: 0x0018CE6C File Offset: 0x0018B06C
			public MBReadOnlyList<T> FindAll(Predicate<T> predicate)
			{
				MBList<T> mblist = new MBList<T>();
				foreach (T t in this._registeredObjects)
				{
					if (predicate == null || predicate(t))
					{
						mblist.Add(t);
					}
				}
				return mblist;
			}

			// Token: 0x06004F6B RID: 20331 RVA: 0x0018CECC File Offset: 0x0018B0CC
			public static string FindNextUniqueStringId(List<CampaignObjectManager.CampaignObjectType<T>> lists, string id)
			{
				if (!CampaignObjectManager.CampaignObjectType<T>.Exist(lists, id))
				{
					return id;
				}
				ValueTuple<string, uint> idParts = CampaignObjectManager.CampaignObjectType<T>.GetIdParts(id);
				string item = idParts.Item1;
				uint num = idParts.Item2;
				num = MathF.Max(num, lists.Max<CampaignObjectManager.CampaignObjectType<T>, uint>((CampaignObjectManager.CampaignObjectType<T> x) => x.MaxCreatedPostfixIndex));
				num += 1U;
				return item + num;
			}

			// Token: 0x06004F6C RID: 20332 RVA: 0x0018CF34 File Offset: 0x0018B134
			[return: TupleElementNames(new string[] { "str", "number" })]
			private static ValueTuple<string, uint> GetIdParts(string stringId)
			{
				int num = stringId.Length - 1;
				while (num > 0 && char.IsDigit(stringId[num]))
				{
					num--;
				}
				string text = stringId.Substring(0, num + 1);
				uint num2 = 0U;
				if (num < stringId.Length - 1)
				{
					uint.TryParse(stringId.Substring(num + 1, stringId.Length - num - 1), out num2);
				}
				return new ValueTuple<string, uint>(text, num2);
			}

			// Token: 0x06004F6D RID: 20333 RVA: 0x0018CF9C File Offset: 0x0018B19C
			private static bool Exist(List<CampaignObjectManager.CampaignObjectType<T>> lists, string id)
			{
				using (List<CampaignObjectManager.CampaignObjectType<T>>.Enumerator enumerator = lists.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Find(id) != null)
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x04001707 RID: 5895
			private readonly IEnumerable<T> _registeredObjects;
		}

		// Token: 0x02000543 RID: 1347
		private enum CampaignObjects
		{
			// Token: 0x0400170A RID: 5898
			DeadOrDisabledHeroes,
			// Token: 0x0400170B RID: 5899
			AliveHeroes,
			// Token: 0x0400170C RID: 5900
			Clans,
			// Token: 0x0400170D RID: 5901
			Kingdoms,
			// Token: 0x0400170E RID: 5902
			MobileParty,
			// Token: 0x0400170F RID: 5903
			Wreckage,
			// Token: 0x04001710 RID: 5904
			ObjectCount
		}
	}
}
