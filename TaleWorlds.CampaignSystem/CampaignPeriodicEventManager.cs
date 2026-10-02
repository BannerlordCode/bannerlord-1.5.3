using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000042 RID: 66
	public class CampaignPeriodicEventManager
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600086B RID: 2155 RVA: 0x000268CC File Offset: 0x00024ACC
		private double DeltaHours
		{
			get
			{
				return CampaignTime.DeltaTime.ToHours;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x000268E8 File Offset: 0x00024AE8
		private double DeltaDays
		{
			get
			{
				return CampaignTime.DeltaTime.ToDays;
			}
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00026904 File Offset: 0x00024B04
		internal CampaignPeriodicEventManager()
		{
			this._mobilePartyHourlyTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._mobilePartyDailyTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._hourlyTickMobilePartyTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._hourlyTickSettlementTicker = new CampaignPeriodicEventManager.PeriodicTicker<Settlement>();
			this._dailyTickSettlementTicker = new CampaignPeriodicEventManager.PeriodicTicker<Settlement>();
			this._hourlyTickClanTicker = new CampaignPeriodicEventManager.PeriodicTicker<Clan>();
			this._dailyTickPartyTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._dailyTickTownTicker = new CampaignPeriodicEventManager.PeriodicTicker<Town>();
			this._dailyTickHeroTicker = new CampaignPeriodicEventManager.PeriodicTicker<Hero>();
			this._dailyTickClanTicker = new CampaignPeriodicEventManager.PeriodicTicker<Clan>();
			this._quarterDailyPartyTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._caravanMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._garrisonMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._militiaMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._villagerMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._customMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._patrolPartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._banditMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._lordMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			this._partiesWithoutPartyComponentsPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x000269F4 File Offset: 0x00024BF4
		[LoadInitializationCallback]
		private void OnLoad(MetaData metaData)
		{
			if (this._caravanMobilePartyPartialHourlyAiEventTicker == null)
			{
				this._caravanMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._garrisonMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._militiaMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._villagerMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._customMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._banditMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._lordMobilePartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._partiesWithoutPartyComponentsPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
				this._quarterDailyPartyTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			}
			if (MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0)) && this._patrolPartyPartialHourlyAiEventTicker == null)
			{
				this._patrolPartyPartialHourlyAiEventTicker = new CampaignPeriodicEventManager.PeriodicTicker<MobileParty>();
			}
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00026AA0 File Offset: 0x00024CA0
		internal void InitializeTickers()
		{
			this.MinimumPeriodicEventInterval = CampaignTime.Hours(0.05f);
			this._lastGameTime = CampaignTime.Zero;
			MBList<Settlement> mblist = this.ShuffleSettlements();
			this._mobilePartyHourlyTicker.Initialize(MobileParty.All, delegate(MobileParty x)
			{
				x.HourlyTick();
			}, false);
			this._mobilePartyDailyTicker.Initialize(MobileParty.All, delegate(MobileParty x)
			{
				x.DailyTick();
			}, false);
			this._hourlyTickMobilePartyTicker.Initialize(MobileParty.All, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.HourlyTickParty(x);
			}, false);
			this._hourlyTickSettlementTicker.Initialize(mblist, delegate(Settlement x)
			{
				CampaignEventDispatcher.Instance.HourlyTickSettlement(x);
			}, false);
			this._dailyTickSettlementTicker.Initialize(mblist, delegate(Settlement x)
			{
				CampaignEventDispatcher.Instance.DailyTickSettlement(x);
			}, false);
			this._hourlyTickClanTicker.Initialize(Clan.All, delegate(Clan x)
			{
				CampaignEventDispatcher.Instance.HourlyTickClan(x);
			}, false);
			this._dailyTickPartyTicker.Initialize(MobileParty.All, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.DailyTickParty(x);
			}, false);
			this._dailyTickTownTicker.Initialize(Town.AllTowns, delegate(Town x)
			{
				CampaignEventDispatcher.Instance.DailyTickTown(x);
			}, false);
			this._dailyTickHeroTicker.Initialize(Hero.AllAliveHeroes, delegate(Hero x)
			{
				CampaignEventDispatcher.Instance.DailyTickHero(x);
			}, false);
			this._dailyTickClanTicker.Initialize(Clan.All, delegate(Clan x)
			{
				CampaignEventDispatcher.Instance.DailyTickClan(x);
			}, false);
			bool flag = false;
			this._caravanMobilePartyPartialHourlyAiEventTicker.Initialize(MobileParty.AllCaravanParties, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, flag);
			this._garrisonMobilePartyPartialHourlyAiEventTicker.Initialize(MobileParty.AllGarrisonParties, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, flag);
			this._militiaMobilePartyPartialHourlyAiEventTicker.Initialize(MobileParty.AllMilitiaParties, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, flag);
			this._villagerMobilePartyPartialHourlyAiEventTicker.Initialize(MobileParty.AllVillagerParties, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, flag);
			this._customMobilePartyPartialHourlyAiEventTicker.Initialize(MobileParty.AllCustomParties, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, flag);
			this._patrolPartyPartialHourlyAiEventTicker.Initialize(MobileParty.AllPatrolParties, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, flag);
			this._banditMobilePartyPartialHourlyAiEventTicker.Initialize(MobileParty.AllBanditParties, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, flag);
			this._lordMobilePartyPartialHourlyAiEventTicker.Initialize(MobileParty.AllLordParties, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, flag);
			this._partiesWithoutPartyComponentsPartialHourlyAiEventTicker.Initialize(MobileParty.AllPartiesWithoutPartyComponent, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.TickPartialHourlyAi(x);
			}, flag);
			this._quarterDailyPartyTicker.Initialize(MobileParty.All, delegate(MobileParty x)
			{
				CampaignEventDispatcher.Instance.QuarterDailyPartyTick(x);
			}, false);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00026E8C File Offset: 0x0002508C
		private MBList<Settlement> ShuffleSettlements()
		{
			Stack<Settlement> stack = new Stack<Settlement>();
			Stack<Settlement> stack2 = new Stack<Settlement>();
			Stack<Settlement> stack3 = new Stack<Settlement>();
			Stack<Settlement> stack4 = new Stack<Settlement>();
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsVillage)
				{
					stack.Push(settlement);
				}
				else if (settlement.IsCastle)
				{
					stack2.Push(settlement);
				}
				else if (settlement.IsTown)
				{
					stack3.Push(settlement);
				}
				else
				{
					stack4.Push(settlement);
				}
			}
			float num = (float)Settlement.All.Count;
			float num2 = (float)stack.Count / num;
			float num3 = (float)stack2.Count / num;
			float num4 = (float)stack3.Count / num;
			float num5 = (float)stack4.Count / num;
			float num6 = num2;
			float num7 = num3;
			float num8 = num4;
			float num9 = num5;
			MBList<Settlement> mblist = new MBList<Settlement>();
			while (mblist.Count != Settlement.All.Count)
			{
				num6 += num2;
				if (num6 >= 1f && !stack.IsEmpty<Settlement>())
				{
					mblist.Add(stack.Pop());
					num6 -= 1f;
				}
				num7 += num3;
				if (num7 >= 1f && !stack2.IsEmpty<Settlement>())
				{
					mblist.Add(stack2.Pop());
					num7 -= 1f;
				}
				num8 += num4;
				if (num8 >= 1f && !stack3.IsEmpty<Settlement>())
				{
					mblist.Add(stack3.Pop());
					num8 -= 1f;
				}
				num9 += num5;
				if (num9 >= 1f && !stack4.IsEmpty<Settlement>())
				{
					mblist.Add(stack4.Pop());
					num9 -= 1f;
				}
			}
			return mblist;
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00027060 File Offset: 0x00025260
		internal void TickPeriodicEvents()
		{
			this.PeriodicHourlyTick();
			this.PeriodicDailyTick();
			this.PeriodicQuarterDailyTick();
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x00027074 File Offset: 0x00025274
		private void PeriodicQuarterDailyTick()
		{
			double deltaDays = this.DeltaDays;
			this._quarterDailyPartyTicker.PeriodicTickSome(deltaDays * 4.0);
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0002709E File Offset: 0x0002529E
		internal void MobilePartyHourlyTick()
		{
			this._mobilePartyHourlyTicker.PeriodicTickSome(this.DeltaHours);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x000270B4 File Offset: 0x000252B4
		internal void TickPartialHourlyAi()
		{
			this._caravanMobilePartyPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
			this._garrisonMobilePartyPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
			this._militiaMobilePartyPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
			this._villagerMobilePartyPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
			this._customMobilePartyPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
			this._patrolPartyPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
			this._banditMobilePartyPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
			this._lordMobilePartyPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
			this._partiesWithoutPartyComponentsPartialHourlyAiEventTicker.PeriodicTickSome(this.DeltaHours * 0.99);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x000271B4 File Offset: 0x000253B4
		private void PeriodicHourlyTick()
		{
			double deltaHours = this.DeltaHours;
			this._hourlyTickMobilePartyTicker.PeriodicTickSome(deltaHours);
			this._hourlyTickSettlementTicker.PeriodicTickSome(deltaHours);
			this._hourlyTickClanTicker.PeriodicTickSome(deltaHours);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x000271EC File Offset: 0x000253EC
		private void PeriodicDailyTick()
		{
			double deltaDays = this.DeltaDays;
			this._dailyTickPartyTicker.PeriodicTickSome(deltaDays);
			this._mobilePartyDailyTicker.PeriodicTickSome(deltaDays);
			this._dailyTickTownTicker.PeriodicTickSome(deltaDays);
			this._dailyTickSettlementTicker.PeriodicTickSome(deltaDays);
			this._dailyTickHeroTicker.PeriodicTickSome(deltaDays);
			this._dailyTickClanTicker.PeriodicTickSome(deltaDays);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x00027248 File Offset: 0x00025448
		public static MBCampaignEvent CreatePeriodicEvent(CampaignTime triggerPeriod, CampaignTime initialWait)
		{
			MBCampaignEvent mbcampaignEvent = new MBCampaignEvent(triggerPeriod, initialWait);
			Campaign.Current.CustomPeriodicCampaignEvents.Add(mbcampaignEvent);
			return mbcampaignEvent;
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00027270 File Offset: 0x00025470
		private void DeleteMarkedPeriodicEvents()
		{
			List<MBCampaignEvent> customPeriodicCampaignEvents = Campaign.Current.CustomPeriodicCampaignEvents;
			for (int i = customPeriodicCampaignEvents.Count - 1; i >= 0; i--)
			{
				if (customPeriodicCampaignEvents[i].isEventDeleted)
				{
					customPeriodicCampaignEvents.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x000272B0 File Offset: 0x000254B0
		internal void OnTick(float dt)
		{
			this.SignalPeriodicEvents();
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x000272B8 File Offset: 0x000254B8
		private void SignalPeriodicEvents()
		{
			if ((this._lastGameTime + this.MinimumPeriodicEventInterval).IsPast)
			{
				this._lastGameTime = CampaignTime.Now;
				List<MBCampaignEvent> customPeriodicCampaignEvents = Campaign.Current.CustomPeriodicCampaignEvents;
				for (int i = customPeriodicCampaignEvents.Count - 1; i >= 0; i--)
				{
					customPeriodicCampaignEvents[i].CheckUpdate();
				}
				this.DeleteMarkedPeriodicEvents();
				MapState mapState = Game.Current.GameStateManager.ActiveState as MapState;
				if (mapState == null)
				{
					return;
				}
				mapState.OnSignalPeriodicEvents();
			}
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x00027339 File Offset: 0x00025539
		internal static void AutoGeneratedStaticCollectObjectsCampaignPeriodicEventManager(object o, List<object> collectedObjects)
		{
			((CampaignPeriodicEventManager)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00027348 File Offset: 0x00025548
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._mobilePartyHourlyTicker);
			collectedObjects.Add(this._mobilePartyDailyTicker);
			collectedObjects.Add(this._dailyTickPartyTicker);
			collectedObjects.Add(this._hourlyTickMobilePartyTicker);
			collectedObjects.Add(this._hourlyTickSettlementTicker);
			collectedObjects.Add(this._hourlyTickClanTicker);
			collectedObjects.Add(this._dailyTickTownTicker);
			collectedObjects.Add(this._dailyTickSettlementTicker);
			collectedObjects.Add(this._dailyTickHeroTicker);
			collectedObjects.Add(this._dailyTickClanTicker);
			collectedObjects.Add(this._quarterDailyPartyTicker);
			collectedObjects.Add(this._caravanMobilePartyPartialHourlyAiEventTicker);
			collectedObjects.Add(this._garrisonMobilePartyPartialHourlyAiEventTicker);
			collectedObjects.Add(this._militiaMobilePartyPartialHourlyAiEventTicker);
			collectedObjects.Add(this._villagerMobilePartyPartialHourlyAiEventTicker);
			collectedObjects.Add(this._customMobilePartyPartialHourlyAiEventTicker);
			collectedObjects.Add(this._banditMobilePartyPartialHourlyAiEventTicker);
			collectedObjects.Add(this._lordMobilePartyPartialHourlyAiEventTicker);
			collectedObjects.Add(this._partiesWithoutPartyComponentsPartialHourlyAiEventTicker);
			collectedObjects.Add(this._patrolPartyPartialHourlyAiEventTicker);
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00027445 File Offset: 0x00025645
		internal static object AutoGeneratedGetMemberValue_mobilePartyHourlyTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._mobilePartyHourlyTicker;
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00027452 File Offset: 0x00025652
		internal static object AutoGeneratedGetMemberValue_mobilePartyDailyTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._mobilePartyDailyTicker;
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x0002745F File Offset: 0x0002565F
		internal static object AutoGeneratedGetMemberValue_dailyTickPartyTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._dailyTickPartyTicker;
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0002746C File Offset: 0x0002566C
		internal static object AutoGeneratedGetMemberValue_hourlyTickMobilePartyTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._hourlyTickMobilePartyTicker;
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00027479 File Offset: 0x00025679
		internal static object AutoGeneratedGetMemberValue_hourlyTickSettlementTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._hourlyTickSettlementTicker;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00027486 File Offset: 0x00025686
		internal static object AutoGeneratedGetMemberValue_hourlyTickClanTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._hourlyTickClanTicker;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00027493 File Offset: 0x00025693
		internal static object AutoGeneratedGetMemberValue_dailyTickTownTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._dailyTickTownTicker;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x000274A0 File Offset: 0x000256A0
		internal static object AutoGeneratedGetMemberValue_dailyTickSettlementTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._dailyTickSettlementTicker;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x000274AD File Offset: 0x000256AD
		internal static object AutoGeneratedGetMemberValue_dailyTickHeroTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._dailyTickHeroTicker;
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x000274BA File Offset: 0x000256BA
		internal static object AutoGeneratedGetMemberValue_dailyTickClanTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._dailyTickClanTicker;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x000274C7 File Offset: 0x000256C7
		internal static object AutoGeneratedGetMemberValue_quarterDailyPartyTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._quarterDailyPartyTicker;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x000274D4 File Offset: 0x000256D4
		internal static object AutoGeneratedGetMemberValue_caravanMobilePartyPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._caravanMobilePartyPartialHourlyAiEventTicker;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x000274E1 File Offset: 0x000256E1
		internal static object AutoGeneratedGetMemberValue_garrisonMobilePartyPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._garrisonMobilePartyPartialHourlyAiEventTicker;
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x000274EE File Offset: 0x000256EE
		internal static object AutoGeneratedGetMemberValue_militiaMobilePartyPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._militiaMobilePartyPartialHourlyAiEventTicker;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x000274FB File Offset: 0x000256FB
		internal static object AutoGeneratedGetMemberValue_villagerMobilePartyPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._villagerMobilePartyPartialHourlyAiEventTicker;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x00027508 File Offset: 0x00025708
		internal static object AutoGeneratedGetMemberValue_customMobilePartyPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._customMobilePartyPartialHourlyAiEventTicker;
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00027515 File Offset: 0x00025715
		internal static object AutoGeneratedGetMemberValue_banditMobilePartyPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._banditMobilePartyPartialHourlyAiEventTicker;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00027522 File Offset: 0x00025722
		internal static object AutoGeneratedGetMemberValue_lordMobilePartyPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._lordMobilePartyPartialHourlyAiEventTicker;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0002752F File Offset: 0x0002572F
		internal static object AutoGeneratedGetMemberValue_partiesWithoutPartyComponentsPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._partiesWithoutPartyComponentsPartialHourlyAiEventTicker;
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0002753C File Offset: 0x0002573C
		internal static object AutoGeneratedGetMemberValue_patrolPartyPartialHourlyAiEventTicker(object o)
		{
			return ((CampaignPeriodicEventManager)o)._patrolPartyPartialHourlyAiEventTicker;
		}

		// Token: 0x040002AA RID: 682
		[SaveableField(120)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _mobilePartyHourlyTicker;

		// Token: 0x040002AB RID: 683
		[SaveableField(130)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _mobilePartyDailyTicker;

		// Token: 0x040002AC RID: 684
		[SaveableField(140)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _dailyTickPartyTicker;

		// Token: 0x040002AD RID: 685
		[SaveableField(150)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _hourlyTickMobilePartyTicker;

		// Token: 0x040002AE RID: 686
		[SaveableField(160)]
		private CampaignPeriodicEventManager.PeriodicTicker<Settlement> _hourlyTickSettlementTicker;

		// Token: 0x040002AF RID: 687
		[SaveableField(170)]
		private CampaignPeriodicEventManager.PeriodicTicker<Clan> _hourlyTickClanTicker;

		// Token: 0x040002B0 RID: 688
		[SaveableField(180)]
		private CampaignPeriodicEventManager.PeriodicTicker<Town> _dailyTickTownTicker;

		// Token: 0x040002B1 RID: 689
		[SaveableField(190)]
		private CampaignPeriodicEventManager.PeriodicTicker<Settlement> _dailyTickSettlementTicker;

		// Token: 0x040002B2 RID: 690
		[SaveableField(200)]
		private CampaignPeriodicEventManager.PeriodicTicker<Hero> _dailyTickHeroTicker;

		// Token: 0x040002B3 RID: 691
		[SaveableField(210)]
		private CampaignPeriodicEventManager.PeriodicTicker<Clan> _dailyTickClanTicker;

		// Token: 0x040002B4 RID: 692
		[SaveableField(320)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _quarterDailyPartyTicker;

		// Token: 0x040002B5 RID: 693
		[SaveableField(230)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _caravanMobilePartyPartialHourlyAiEventTicker;

		// Token: 0x040002B6 RID: 694
		[SaveableField(250)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _garrisonMobilePartyPartialHourlyAiEventTicker;

		// Token: 0x040002B7 RID: 695
		[SaveableField(260)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _militiaMobilePartyPartialHourlyAiEventTicker;

		// Token: 0x040002B8 RID: 696
		[SaveableField(270)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _villagerMobilePartyPartialHourlyAiEventTicker;

		// Token: 0x040002B9 RID: 697
		[SaveableField(280)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _customMobilePartyPartialHourlyAiEventTicker;

		// Token: 0x040002BA RID: 698
		[SaveableField(290)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _banditMobilePartyPartialHourlyAiEventTicker;

		// Token: 0x040002BB RID: 699
		[SaveableField(300)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _lordMobilePartyPartialHourlyAiEventTicker;

		// Token: 0x040002BC RID: 700
		[SaveableField(310)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _partiesWithoutPartyComponentsPartialHourlyAiEventTicker;

		// Token: 0x040002BD RID: 701
		[SaveableField(330)]
		private CampaignPeriodicEventManager.PeriodicTicker<MobileParty> _patrolPartyPartialHourlyAiEventTicker;

		// Token: 0x040002BE RID: 702
		private CampaignTime MinimumPeriodicEventInterval;

		// Token: 0x040002BF RID: 703
		private CampaignTime _lastGameTime;

		// Token: 0x02000532 RID: 1330
		internal class PeriodicTicker<T>
		{
			// Token: 0x17000F32 RID: 3890
			// (get) Token: 0x06004EEA RID: 20202 RVA: 0x0018C5B6 File Offset: 0x0018A7B6
			// (set) Token: 0x06004EEB RID: 20203 RVA: 0x0018C5BE File Offset: 0x0018A7BE
			[SaveableProperty(1)]
			private double TickDebt { get; set; }

			// Token: 0x17000F33 RID: 3891
			// (get) Token: 0x06004EEC RID: 20204 RVA: 0x0018C5C7 File Offset: 0x0018A7C7
			// (set) Token: 0x06004EED RID: 20205 RVA: 0x0018C5CF File Offset: 0x0018A7CF
			[SaveableProperty(2)]
			private int Index { get; set; }

			// Token: 0x06004EEE RID: 20206 RVA: 0x0018C5D8 File Offset: 0x0018A7D8
			internal PeriodicTicker()
			{
				this.TickDebt = 0.0;
				this.Index = -1;
			}

			// Token: 0x06004EEF RID: 20207 RVA: 0x0018C601 File Offset: 0x0018A801
			internal void Initialize(MBReadOnlyList<T> list, Action<T> action, bool doParallel)
			{
				this._list = list;
				this._action = action;
				this._doParallel = doParallel;
			}

			// Token: 0x06004EF0 RID: 20208 RVA: 0x0018C618 File Offset: 0x0018A818
			internal void PeriodicTickSome(double timeUnitsElapsed)
			{
				if (this._list.Count == 0)
				{
					this.TickDebt = 0.0;
					return;
				}
				this.TickDebt += timeUnitsElapsed * (double)this._list.Count;
				while (this.TickDebt > 1.0)
				{
					this.Index++;
					if (this.Index >= this._list.Count)
					{
						this.Index = 0;
					}
					if (this._doParallel)
					{
						this._currentFrameToTickListFlattened.Add(this._list[this.Index]);
					}
					else
					{
						this._action(this._list[this.Index]);
					}
					this.TickDebt -= 1.0;
				}
				if (this._doParallel && this._currentFrameToTickListFlattened.Count > 0)
				{
					TWParallel.For(0, this._currentFrameToTickListFlattened.Count, delegate(int startInclusive, int endExclusive)
					{
						for (int i = startInclusive; i < endExclusive; i++)
						{
							this._action(this._currentFrameToTickListFlattened[i]);
						}
					}, 1);
					this._currentFrameToTickListFlattened.Clear();
				}
			}

			// Token: 0x06004EF1 RID: 20209 RVA: 0x0018C738 File Offset: 0x0018A938
			public override string ToString()
			{
				object[] array = new object[7];
				array[0] = "PeriodicTicker  @";
				int num = 1;
				object obj;
				if (this.Index != -1)
				{
					T t = this._list[this.Index];
					obj = t.ToString();
				}
				else
				{
					obj = "null";
				}
				array[num] = obj;
				array[2] = "\t\t(";
				array[3] = this.Index;
				array[4] = " / ";
				array[5] = this._list.Count;
				array[6] = ")";
				return string.Concat(array);
			}

			// Token: 0x06004EF2 RID: 20210 RVA: 0x0018C7C3 File Offset: 0x0018A9C3
			protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
			{
			}

			// Token: 0x040016C4 RID: 5828
			private readonly List<T> _currentFrameToTickListFlattened = new List<T>();

			// Token: 0x040016C7 RID: 5831
			private bool _doParallel;

			// Token: 0x040016C8 RID: 5832
			private MBReadOnlyList<T> _list;

			// Token: 0x040016C9 RID: 5833
			private Action<T> _action;
		}
	}
}
