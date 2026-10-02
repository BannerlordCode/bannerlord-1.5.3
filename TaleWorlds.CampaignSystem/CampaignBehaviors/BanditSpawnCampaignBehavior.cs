using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x020003ED RID: 1005
	public class BanditSpawnCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x17000E78 RID: 3704
		// (get) Token: 0x06003CC5 RID: 15557 RVA: 0x000FA503 File Offset: 0x000F8703
		private float BanditSpawnRadiusAsDays
		{
			get
			{
				return 0.5f * Campaign.Current.EstimatedAverageBanditPartySpeed * (float)CampaignTime.HoursInDay;
			}
		}

		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x06003CC6 RID: 15558 RVA: 0x000FA51C File Offset: 0x000F871C
		private float _radiusAroundPlayerPartySquared
		{
			get
			{
				float seeingRange = MobileParty.MainParty.SeeingRange;
				return seeingRange * seeingRange;
			}
		}

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x06003CC7 RID: 15559 RVA: 0x000FA52A File Offset: 0x000F872A
		private float _numberOfMinimumBanditPartiesInAHideoutToInfestIt
		{
			get
			{
				return (float)Campaign.Current.Models.BanditDensityModel.NumberOfMinimumBanditPartiesInAHideoutToInfestIt;
			}
		}

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x06003CC8 RID: 15560 RVA: 0x000FA541 File Offset: 0x000F8741
		private int _numberOfMaxBanditPartiesAroundEachHideout
		{
			get
			{
				return Campaign.Current.Models.BanditDensityModel.NumberOfMaximumBanditPartiesAroundEachHideout;
			}
		}

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x06003CC9 RID: 15561 RVA: 0x000FA557 File Offset: 0x000F8757
		private int _numberOfMaxHideoutsAtEachBanditFaction
		{
			get
			{
				return Campaign.Current.Models.BanditDensityModel.NumberOfMaximumHideoutsAtEachBanditFaction;
			}
		}

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x06003CCA RID: 15562 RVA: 0x000FA56D File Offset: 0x000F876D
		private int _numberOfInitialHideoutsAtEachBanditFaction
		{
			get
			{
				return Campaign.Current.Models.BanditDensityModel.NumberOfInitialHideoutsAtEachBanditFaction;
			}
		}

		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x06003CCB RID: 15563 RVA: 0x000FA583 File Offset: 0x000F8783
		private int _numberOfMaximumBanditPartiesInEachHideout
		{
			get
			{
				return Campaign.Current.Models.BanditDensityModel.NumberOfMaximumBanditPartiesInEachHideout;
			}
		}

		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x06003CCC RID: 15564 RVA: 0x000FA599 File Offset: 0x000F8799
		private int _numberOfMaxBanditCountPerClanHideout
		{
			get
			{
				return this._numberOfMaxBanditPartiesAroundEachHideout + this._numberOfMaximumBanditPartiesInEachHideout;
			}
		}

		// Token: 0x06003CCD RID: 15565 RVA: 0x000FA5A8 File Offset: 0x000F87A8
		public override void RegisterEvents()
		{
			CampaignEvents.MobilePartyCreated.AddNonSerializedListener(this, new Action<MobileParty>(this.MobilePartyCreated));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.MobilePartyDestroyed));
			CampaignEvents.SettlementEntered.AddNonSerializedListener(this, new Action<MobileParty, Settlement, Hero>(this.OnSettlementEntered));
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.HourlyTickClanEvent.AddNonSerializedListener(this, new Action<Clan>(this.HourlyTickClan));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.OnHomeHideoutChangedEvent.AddNonSerializedListener(this, new Action<BanditPartyComponent, Hideout>(this.OnHomeHideoutChanged));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter, int>(this.OnNewGameCreatedPartialFollowUp));
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x000FA670 File Offset: 0x000F8870
		private void MobilePartyDestroyed(MobileParty party, PartyBase destroyerParty)
		{
			if (party.IsBandit && party.ActualClan != null && (this.IsBanditFaction(party.ActualClan) || FactionHelper.IsLooterFaction(party.ActualClan)))
			{
				int num = 0;
				this._banditCountsPerHideout.TryGetValue(party.HomeSettlement, out num);
				this._banditCountsPerHideout[party.HomeSettlement] = num - 1;
			}
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x000FA6D4 File Offset: 0x000F88D4
		private void MobilePartyCreated(MobileParty party)
		{
			if (party.IsBandit && party.ActualClan != null && (this.IsBanditFaction(party.ActualClan) || FactionHelper.IsLooterFaction(party.ActualClan)))
			{
				int num = 0;
				this._banditCountsPerHideout.TryGetValue(party.HomeSettlement, out num);
				this._banditCountsPerHideout[party.HomeSettlement] = num + 1;
			}
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x000FA736 File Offset: 0x000F8936
		private void OnGameLoaded(CampaignGameStarter starter)
		{
			this.CacheHideouts();
			this.CacheBanditCounts();
		}

		// Token: 0x06003CD1 RID: 15569 RVA: 0x000FA744 File Offset: 0x000F8944
		public override void SyncData(IDataStore dataStore)
		{
		}

		// Token: 0x06003CD2 RID: 15570 RVA: 0x000FA746 File Offset: 0x000F8946
		private void OnNewGameCreatedPartialFollowUp(CampaignGameStarter starter, int i)
		{
			if (i == 10)
			{
				this.CacheHideouts();
				if (this._numberOfInitialHideoutsAtEachBanditFaction > 0)
				{
					this.InitializeInitialHideouts();
					return;
				}
			}
			else if (i == 11)
			{
				this.SpawnBanditsAroundHideoutAtNewGame();
				this.SpawnLootersAtNewGame();
				this.CacheBanditCounts();
			}
		}

		// Token: 0x06003CD3 RID: 15571 RVA: 0x000FA77C File Offset: 0x000F897C
		private void CacheHideouts()
		{
			foreach (Hideout hideout in Hideout.All)
			{
				List<Hideout> list;
				if (!this._hideouts.TryGetValue(hideout.Settlement.Culture, out list))
				{
					this._hideouts[hideout.Settlement.Culture] = new List<Hideout>();
				}
				this._hideouts[hideout.Settlement.Culture].Add(hideout);
			}
		}

		// Token: 0x06003CD4 RID: 15572 RVA: 0x000FA818 File Offset: 0x000F8A18
		private void CacheBanditCounts()
		{
			this._banditCountsPerHideout = new Dictionary<Settlement, int>();
			foreach (MobileParty mobileParty in MobileParty.AllBanditParties)
			{
				if (this.IsBanditFaction(mobileParty.ActualClan) || FactionHelper.IsLooterFaction(mobileParty.ActualClan))
				{
					int num = 0;
					this._banditCountsPerHideout.TryGetValue(mobileParty.HomeSettlement, out num);
					this._banditCountsPerHideout[mobileParty.HomeSettlement] = num + 1;
				}
			}
		}

		// Token: 0x06003CD5 RID: 15573 RVA: 0x000FA8B4 File Offset: 0x000F8AB4
		public void InitializeInitialHideouts()
		{
			foreach (Clan clan in Clan.BanditFactions)
			{
				if (this.IsBanditFaction(clan))
				{
					this.SpawnHideoutsAndBanditsPartiallyOnNewGame(clan);
				}
			}
		}

		// Token: 0x06003CD6 RID: 15574 RVA: 0x000FA90C File Offset: 0x000F8B0C
		private void SpawnHideoutsAndBanditsPartiallyOnNewGame(Clan banditClan)
		{
			for (int i = 0; i < this._numberOfInitialHideoutsAtEachBanditFaction; i++)
			{
				this.FillANewHideoutWithBandits(banditClan);
			}
		}

		// Token: 0x06003CD7 RID: 15575 RVA: 0x000FA934 File Offset: 0x000F8B34
		public void OnSettlementEntered(MobileParty mobileParty, Settlement settlement, Hero hero)
		{
			this.CheckForSpawningBanditBoss(settlement, mobileParty);
			if (Campaign.Current.GameStarted && mobileParty != null && mobileParty.IsBandit && settlement.IsHideout)
			{
				if (!settlement.IsVisible && settlement.Hideout.IsInfested && mobileParty.IsVisible)
				{
					settlement.IsVisible = true;
					CampaignEventDispatcher.Instance.OnHideoutSpotted(MobileParty.MainParty.Party, settlement.Party);
				}
				int num = 0;
				foreach (ItemRosterElement itemRosterElement in mobileParty.ItemRoster)
				{
					int num2 = (itemRosterElement.EquipmentElement.Item.IsFood ? MBRandom.RoundRandomized((float)mobileParty.MemberRoster.TotalManCount * ((3f + 6f * MBRandom.RandomFloat) / (float)itemRosterElement.EquipmentElement.Item.Value)) : 0);
					if (itemRosterElement.Amount > num2)
					{
						int num3 = itemRosterElement.Amount - num2;
						num += num3 * itemRosterElement.EquipmentElement.Item.Value;
					}
				}
				if (num > 0)
				{
					if (mobileParty.IsPartyTradeActive)
					{
						mobileParty.PartyTradeGold += (int)(0.25f * (float)num);
					}
					settlement.SettlementComponent.ChangeGold((int)(0.25f * (float)num));
				}
			}
		}

		// Token: 0x06003CD8 RID: 15576 RVA: 0x000FAAB0 File Offset: 0x000F8CB0
		private void CheckForSpawningBanditBoss(Settlement settlement, MobileParty mobileParty)
		{
			if (settlement.IsHideout && settlement.IsVisible)
			{
				if (settlement.Parties.Any<MobileParty>((MobileParty x) => x.IsBandit || x.IsBanditBossParty))
				{
					CultureObject culture = settlement.Culture;
					MobileParty mobileParty2 = settlement.Parties.FirstOrDefault<MobileParty>((MobileParty x) => x.IsBanditBossParty);
					if (mobileParty2 == null)
					{
						this.AddBossParty(settlement, culture);
						return;
					}
					if (!mobileParty2.MemberRoster.Contains(culture.BanditBoss))
					{
						mobileParty2.MemberRoster.AddToCounts(culture.BanditBoss, 1, false, 0, 0, true, -1);
					}
				}
			}
		}

		// Token: 0x06003CD9 RID: 15577 RVA: 0x000FAB68 File Offset: 0x000F8D68
		private void AddBossParty(Settlement settlement, CultureObject culture)
		{
			PartyTemplateObject banditBossPartyTemplate = culture.BanditBossPartyTemplate;
			if (banditBossPartyTemplate != null)
			{
				this.AddBanditToHideout(settlement.Hideout, banditBossPartyTemplate, true).Ai.DisableAi();
			}
		}

		// Token: 0x06003CDA RID: 15578 RVA: 0x000FAB98 File Offset: 0x000F8D98
		public void DailyTick()
		{
			if (this._numberOfMaxHideoutsAtEachBanditFaction > 0)
			{
				this.AddNewHideouts();
			}
			foreach (MobileParty mobileParty in MobileParty.AllBanditParties)
			{
				if (mobileParty.IsPartyTradeActive)
				{
					mobileParty.PartyTradeGold = (int)((double)mobileParty.PartyTradeGold * 0.95 + (double)(50f * (float)mobileParty.Party.MemberRoster.TotalManCount * 0.05f));
					if (MBRandom.RandomFloat < 0.03f && mobileParty.MapEvent != null)
					{
						foreach (ItemObject itemObject in Items.All)
						{
							if (itemObject.IsFood)
							{
								int num = (FactionHelper.IsLooterFaction(mobileParty.MapFaction) ? 8 : 16);
								int num2 = MBRandom.RoundRandomized((float)mobileParty.MemberRoster.TotalManCount * (1f / (float)itemObject.Value) * (float)num * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat);
								if (num2 > 0)
								{
									mobileParty.ItemRoster.AddToCounts(itemObject, num2);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06003CDB RID: 15579 RVA: 0x000FAD14 File Offset: 0x000F8F14
		private void HourlyTickClan(Clan clan)
		{
			if (Campaign.Current.IsNight && clan.IsBanditFaction)
			{
				if (FactionHelper.IsLooterFaction(clan))
				{
					this.SpawnLooters(clan, 0.07f, false);
					return;
				}
				if (this.IsBanditFaction(clan))
				{
					this.SpawnBanditsAroundHideout(clan, 0.1f);
				}
			}
		}

		// Token: 0x06003CDC RID: 15580 RVA: 0x000FAD60 File Offset: 0x000F8F60
		private void SpawnBanditsAroundHideout(Clan clan, float ratio)
		{
			int count = clan.WarPartyComponents.Count;
			int num = MBRandom.RoundRandomized((float)(this.GetInfestedHideoutCount(clan) * this._numberOfMaxBanditCountPerClanHideout - count) * ratio);
			for (int i = 0; i < num; i++)
			{
				this.SpawnBanditParty(clan);
			}
		}

		// Token: 0x06003CDD RID: 15581 RVA: 0x000FADA8 File Offset: 0x000F8FA8
		private void SpawnLooters(Clan clan, float ratio, bool uniformDistribution)
		{
			int count = clan.WarPartyComponents.Count;
			int num = MBRandom.RoundRandomized((float)(this.GetCurrentLimitForLooters(clan) - count) * ratio);
			for (int i = 0; i < num; i++)
			{
				this.SpawnLooterParty(clan, uniformDistribution);
			}
		}

		// Token: 0x06003CDE RID: 15582 RVA: 0x000FADE8 File Offset: 0x000F8FE8
		private void AddNewHideouts()
		{
			List<ValueTuple<ValueTuple<Clan, int>, float>> list = new List<ValueTuple<ValueTuple<Clan, int>, float>>();
			foreach (Clan clan in Clan.BanditFactions)
			{
				if (this.IsBanditFaction(clan))
				{
					int infestedHideoutCount = this.GetInfestedHideoutCount(clan);
					if (infestedHideoutCount < this._numberOfMaxHideoutsAtEachBanditFaction)
					{
						list.Add(new ValueTuple<ValueTuple<Clan, int>, float>(new ValueTuple<Clan, int>(clan, infestedHideoutCount), 1f - (float)infestedHideoutCount / (float)this._numberOfMaxHideoutsAtEachBanditFaction));
					}
				}
			}
			int num;
			ValueTuple<Clan, int> valueTuple = MBRandom.ChooseWeighted<ValueTuple<Clan, int>>(list, out num);
			Clan item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			if (item != null)
			{
				float num2 = (((float)item2 < (float)this._numberOfMaxHideoutsAtEachBanditFaction * 0.5f) ? (0.2f + (float)(this._numberOfMaxHideoutsAtEachBanditFaction - item2) * 0.1f) : (0.1f + 0.5f * MathF.Pow(1f - 0.25f * ((float)item2 - (float)this._numberOfMaxHideoutsAtEachBanditFaction * 0.5f), 3f)));
				if (MBRandom.RandomFloat < num2)
				{
					this.FillANewHideoutWithBandits(item);
				}
			}
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x000FAF00 File Offset: 0x000F9100
		private void FillANewHideoutWithBandits(Clan faction)
		{
			Hideout hideout = this.SelectANonInfestedHideoutOfSameCultureByWeight(faction);
			if (hideout != null)
			{
				int num = 0;
				while ((float)num < this._numberOfMinimumBanditPartiesInAHideoutToInfestIt)
				{
					this.AddBanditToHideout(hideout, null, false);
					num++;
				}
			}
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x000FAF34 File Offset: 0x000F9134
		public MobileParty AddBanditToHideout(Hideout hideoutComponent, PartyTemplateObject overridenPartyTemplate = null, bool isBanditBossParty = false)
		{
			if (hideoutComponent.Owner.Settlement.Culture.IsBandit)
			{
				Clan clan = null;
				foreach (Clan clan2 in Clan.BanditFactions)
				{
					if (hideoutComponent.Owner.Settlement.Culture == clan2.Culture && (this.IsBanditFaction(clan2) || FactionHelper.IsLooterFaction(clan2)))
					{
						clan = clan2;
					}
				}
				PartyTemplateObject partyTemplateObject = overridenPartyTemplate ?? clan.DefaultPartyTemplate;
				MobileParty mobileParty = BanditPartyComponent.CreateBanditParty(clan.StringId + "_1", clan, hideoutComponent, isBanditBossParty, partyTemplateObject, hideoutComponent.Owner.Settlement.GatePosition);
				this.InitializeBanditParty(mobileParty, clan);
				mobileParty.SetMoveGoToSettlement(hideoutComponent.Owner.Settlement, mobileParty.NavigationCapability, false);
				mobileParty.RecalculateShortTermBehavior();
				EnterSettlementAction.ApplyForParty(mobileParty, hideoutComponent.Owner.Settlement);
				return mobileParty;
			}
			return null;
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x000FB038 File Offset: 0x000F9238
		private Hideout SelectBanditHideout(Clan faction)
		{
			MBList<ValueTuple<Hideout, float>> mblist = new MBList<ValueTuple<Hideout, float>>();
			foreach (Hideout hideout in Hideout.All)
			{
				if (hideout.Settlement.Culture == faction.Culture && hideout.IsInfested)
				{
					mblist.Add(new ValueTuple<Hideout, float>(hideout, this.GetSpawnChanceInSettlement(hideout.Settlement)));
				}
			}
			if (mblist.Count != 0)
			{
				return MBRandom.ChooseWeighted<Hideout>(mblist);
			}
			return this.SelectAHideoutByCheckingCultureAndInfestedState(faction);
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x000FB0D4 File Offset: 0x000F92D4
		private float GetSpawnChanceInSettlement(Settlement settlement)
		{
			if (this._banditCountsPerHideout.ContainsKey(settlement) && this._banditCountsPerHideout[settlement] != 0)
			{
				return 1f / MathF.Pow((float)this._banditCountsPerHideout[settlement], 2f);
			}
			return 1f;
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x000FB120 File Offset: 0x000F9320
		private void OnHomeHideoutChanged(BanditPartyComponent banditPartyComponent, Hideout oldHomeHideout)
		{
			int num = 0;
			this._banditCountsPerHideout.TryGetValue(oldHomeHideout.Settlement, out num);
			this._banditCountsPerHideout[oldHomeHideout.Settlement] = num - 1;
			num = 0;
			this._banditCountsPerHideout.TryGetValue(banditPartyComponent.HomeSettlement, out num);
			this._banditCountsPerHideout[banditPartyComponent.HomeSettlement] = num + 1;
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x000FB184 File Offset: 0x000F9384
		private Hideout SelectAHideoutByCheckingCultureAndInfestedState(Clan faction)
		{
			List<Hideout> list = new List<Hideout>();
			bool flag = false;
			bool flag2 = false;
			foreach (Hideout hideout in Hideout.All)
			{
				bool flag3 = hideout.Settlement.Culture == faction.Culture;
				bool isInfested = hideout.IsInfested;
				if (!flag2 && flag3)
				{
					flag2 = true;
					list.Clear();
				}
				if (flag2 && !flag && isInfested)
				{
					flag = true;
					list.Clear();
				}
				if ((!flag2 || flag3) && (!flag || isInfested))
				{
					list.Add(hideout);
				}
			}
			return list.GetRandomElement<Hideout>();
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x000FB244 File Offset: 0x000F9444
		private Hideout SelectANonInfestedHideoutOfSameCultureByWeight(Clan faction)
		{
			float averageDistanceBetweenClosestTwoTownsWithNavigationType = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(MobileParty.NavigationType.Default);
			float num = averageDistanceBetweenClosestTwoTownsWithNavigationType * 0.33f * averageDistanceBetweenClosestTwoTownsWithNavigationType * 0.33f;
			List<ValueTuple<Hideout, float>> list = new List<ValueTuple<Hideout, float>>();
			foreach (Hideout hideout in Hideout.All)
			{
				if (!hideout.IsInfested && hideout.Settlement.Culture == faction.Culture)
				{
					int num2 = 1;
					if (hideout.Settlement.LastThreatTime.ElapsedDaysUntilNow > 1.5f)
					{
						float num3 = Campaign.MapDiagonalSquared;
						float num4 = Campaign.MapDiagonalSquared;
						foreach (Hideout hideout2 in Hideout.All)
						{
							if (hideout != hideout2 && hideout2.IsInfested)
							{
								float num5 = hideout.Settlement.Position.DistanceSquared(hideout2.Settlement.Position);
								if (hideout.Settlement.Culture == hideout2.Settlement.Culture && num5 < num3)
								{
									num3 = num5;
								}
								if (num5 < num4)
								{
									num4 = num5;
								}
							}
							num2 = (int)MathF.Max(averageDistanceBetweenClosestTwoTownsWithNavigationType * 0.015f, num3 / num + averageDistanceBetweenClosestTwoTownsWithNavigationType * 0.076f * (num4 / num));
						}
					}
					list.Add(new ValueTuple<Hideout, float>(hideout, (float)num2));
				}
			}
			return MBRandom.ChooseWeighted<Hideout>(list);
		}

		// Token: 0x06003CE6 RID: 15590 RVA: 0x000FB3FC File Offset: 0x000F95FC
		public void SpawnBanditsAroundHideoutAtNewGame()
		{
			foreach (Clan clan in Clan.BanditFactions)
			{
				if (this.IsBanditFaction(clan))
				{
					this.SpawnBanditsAroundHideout(clan, MBRandom.RandomFloatRanged(0.5f, 0.75f));
				}
			}
		}

		// Token: 0x06003CE7 RID: 15591 RVA: 0x000FB460 File Offset: 0x000F9660
		public void SpawnLootersAtNewGame()
		{
			foreach (Clan clan in Clan.BanditFactions)
			{
				if (FactionHelper.IsLooterFaction(clan))
				{
					this.SpawnLooters(clan, MBRandom.RandomFloatRanged(0.5f, 0.75f), true);
				}
			}
		}

		// Token: 0x06003CE8 RID: 15592 RVA: 0x000FB4C4 File Offset: 0x000F96C4
		private void SpawnLooterParty(Clan selectedFaction, bool uniformDistribution)
		{
			Settlement settlement = this.SelectARandomSettlementForLooterParty(uniformDistribution);
			CampaignVec2 spawnPositionAroundSettlement = this.GetSpawnPositionAroundSettlement(selectedFaction, settlement);
			MobileParty mobileParty = BanditPartyComponent.CreateLooterParty(selectedFaction.StringId + "_1", selectedFaction, settlement, false, selectedFaction.DefaultPartyTemplate, spawnPositionAroundSettlement);
			if (Campaign.Current.Options.IsRisenBanditsEnabled)
			{
				BanditSpawnCampaignBehavior.TryApplyRisenBanditsRosterBoostToBanditParty(mobileParty);
			}
			this.InitializeBanditParty(mobileParty, selectedFaction);
			mobileParty.SetMovePatrolAroundPoint(mobileParty.Position, MobileParty.NavigationType.Default);
		}

		// Token: 0x06003CE9 RID: 15593 RVA: 0x000FB530 File Offset: 0x000F9730
		private void SpawnBanditParty(Clan selectedFaction)
		{
			Hideout hideout = this.SelectBanditHideout(selectedFaction);
			CampaignVec2 spawnPositionAroundSettlement = this.GetSpawnPositionAroundSettlement(selectedFaction, hideout.Settlement);
			MobileParty mobileParty = BanditPartyComponent.CreateBanditParty(selectedFaction.StringId + "_1", selectedFaction, hideout, false, selectedFaction.DefaultPartyTemplate, spawnPositionAroundSettlement);
			if (Campaign.Current.Options.IsRisenBanditsEnabled)
			{
				BanditSpawnCampaignBehavior.TryApplyRisenBanditsRosterBoostToBanditParty(mobileParty);
			}
			this.InitializeBanditParty(mobileParty, selectedFaction);
			mobileParty.SetMovePatrolAroundPoint(mobileParty.Position, mobileParty.NavigationCapability);
		}

		// Token: 0x06003CEA RID: 15594 RVA: 0x000FB5A4 File Offset: 0x000F97A4
		private float GetSpawnRadiusForClan(Clan selectedFaction)
		{
			return this.BanditSpawnRadiusAsDays * (FactionHelper.IsLooterFaction(selectedFaction) ? 1.5f : 1f);
		}

		// Token: 0x06003CEB RID: 15595 RVA: 0x000FB5C4 File Offset: 0x000F97C4
		private int GetInfestedHideoutCount(Clan banditFaction)
		{
			int num = 0;
			foreach (Hideout hideout in this._hideouts[banditFaction.Culture])
			{
				if (hideout.IsInfested && hideout.MapFaction == banditFaction)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x000FB634 File Offset: 0x000F9834
		private int GetCurrentLimitForLooters(Clan clan)
		{
			return Math.Min(Hideout.All.Count<Hideout>((Hideout x) => x.IsInfested) * 7, Campaign.Current.Models.BanditDensityModel.GetMaxSupportedNumberOfLootersForClan(clan));
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x000FB688 File Offset: 0x000F9888
		private Settlement SelectARandomSettlementForLooterParty(bool uniformDistribution)
		{
			MBList<ValueTuple<Settlement, float>> mblist = new MBList<ValueTuple<Settlement, float>>();
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsTown || settlement.IsVillage)
				{
					mblist.Add(new ValueTuple<Settlement, float>(settlement, this.GetSpawnChanceInSettlement(settlement)));
				}
			}
			return MBRandom.ChooseWeighted<Settlement>(mblist);
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x000FB704 File Offset: 0x000F9904
		private void GiveFoodToBanditParty(MobileParty banditParty)
		{
			int num = (FactionHelper.IsLooterFaction(banditParty.MapFaction) ? 8 : 16);
			foreach (ItemObject itemObject in Items.All)
			{
				if (itemObject.IsFood)
				{
					int num2 = MBRandom.RoundRandomized((float)banditParty.MemberRoster.TotalManCount * (1f / (float)itemObject.Value) * (float)num * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat * MBRandom.RandomFloat);
					if (num2 > 0)
					{
						banditParty.ItemRoster.AddToCounts(itemObject, num2);
					}
				}
			}
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x000FB7B8 File Offset: 0x000F99B8
		private CampaignVec2 GetSpawnPositionAroundSettlement(Clan clan, Settlement settlement)
		{
			CampaignVec2 campaignVec = NavigationHelper.FindPointAroundPosition(settlement.GatePosition, MobileParty.NavigationType.Default, this.GetSpawnRadiusForClan(clan), 0f, true, false);
			if (campaignVec.DistanceSquared(MobileParty.MainParty.Position) < this._radiusAroundPlayerPartySquared)
			{
				for (int i = 0; i < 15; i++)
				{
					CampaignVec2 campaignVec2 = NavigationHelper.FindReachablePointAroundPosition(campaignVec, MobileParty.NavigationType.Default, this.GetSpawnRadiusForClan(clan), 0f, false);
					if (NavigationHelper.IsPositionValidForNavigationType(campaignVec2, MobileParty.NavigationType.Default))
					{
						float num2;
						float num = DistanceHelper.FindClosestDistanceFromMobilePartyToPoint(MobileParty.MainParty, campaignVec2, MobileParty.NavigationType.Default, out num2);
						if (num * num > this._radiusAroundPlayerPartySquared)
						{
							campaignVec = campaignVec2;
							break;
						}
					}
				}
			}
			return campaignVec;
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x000FB841 File Offset: 0x000F9A41
		private bool IsBanditFaction(Clan clan)
		{
			return !clan.HasNavalNavigationCapability && clan.IsBanditFaction && clan.Culture.CanHaveSettlement;
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x000FB860 File Offset: 0x000F9A60
		private void InitializeBanditParty(MobileParty banditParty, Clan faction)
		{
			banditParty.Party.SetVisualAsDirty();
			banditParty.ActualClan = faction;
			banditParty.Aggressiveness = 1f - 0.2f * MBRandom.RandomFloat;
			BanditSpawnCampaignBehavior.CreatePartyTrade(banditParty);
			this.GiveFoodToBanditParty(banditParty);
		}

		// Token: 0x06003CF2 RID: 15602 RVA: 0x000FB898 File Offset: 0x000F9A98
		private static void CreatePartyTrade(MobileParty banditParty)
		{
			int num = (int)(10f * (float)banditParty.Party.MemberRoster.TotalManCount * (0.5f + 1f * MBRandom.RandomFloat));
			banditParty.InitializePartyTrade(num);
		}

		// Token: 0x06003CF3 RID: 15603 RVA: 0x000FB8D8 File Offset: 0x000F9AD8
		public static void TryApplyRisenBanditsRosterBoostToBanditParty(MobileParty mobileParty)
		{
			float num = 2.5f;
			float num2 = 5f;
			float num3 = 9f;
			if (mobileParty.MemberRoster.Count == 0)
			{
				return;
			}
			float randomFloat = MBRandom.RandomFloat;
			float num4;
			if (randomFloat >= 0.9f)
			{
				num4 = num3;
			}
			else
			{
				if (randomFloat < 0.35f)
				{
					return;
				}
				num4 = MBRandom.RandomFloatRanged(num, num2);
			}
			foreach (TroopRosterElement troopRosterElement in mobileParty.MemberRoster.GetTroopRoster().ToList<TroopRosterElement>())
			{
				CharacterObject character = troopRosterElement.Character;
				if (character != null && !character.IsHero)
				{
					int num5 = MBRandom.RoundRandomized((float)troopRosterElement.Number * num4);
					if (num5 > 0)
					{
						mobileParty.MemberRoster.AddToCounts(character, num5, false, 0, 0, true, -1);
					}
				}
			}
		}

		// Token: 0x040012A8 RID: 4776
		private const float BanditStartGoldPerBandit = 10f;

		// Token: 0x040012A9 RID: 4777
		private const float BanditLongTermGoldPerBandit = 50f;

		// Token: 0x040012AA RID: 4778
		private const float HideoutInfestCooldownAfterFightInDays = 1.5f;

		// Token: 0x040012AB RID: 4779
		private Dictionary<CultureObject, List<Hideout>> _hideouts = new Dictionary<CultureObject, List<Hideout>>();

		// Token: 0x040012AC RID: 4780
		private Dictionary<Settlement, int> _banditCountsPerHideout = new Dictionary<Settlement, int>();
	}
}
