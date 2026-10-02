using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200002C RID: 44
	public class Army : ITrackableCampaignObject, ITrackableBase
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00012160 File Offset: 0x00010360
		private float MinimumDistanceToTargetWhileGatheringAsAttackerArmy
		{
			get
			{
				return Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(this.LeaderParty.NavigationCapability) * 0.66f;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000192 RID: 402 RVA: 0x0001217D File Offset: 0x0001037D
		public float GatheringPositionMaxDistanceToTheSettlement
		{
			get
			{
				return Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(this.LeaderParty.NavigationCapability) * 0.2f;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0001219A File Offset: 0x0001039A
		public float GatheringPositionMinDistanceToTheSettlement
		{
			get
			{
				return Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(this.LeaderParty.NavigationCapability) * 0.1f;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000194 RID: 404 RVA: 0x000121B7 File Offset: 0x000103B7
		public MBReadOnlyList<MobileParty> Parties
		{
			get
			{
				return this._parties;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000195 RID: 405 RVA: 0x000121BF File Offset: 0x000103BF
		public TextObject EncyclopediaLinkWithName
		{
			get
			{
				return this.ArmyOwner.EncyclopediaLinkWithName;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000196 RID: 406 RVA: 0x000121CC File Offset: 0x000103CC
		// (set) Token: 0x06000197 RID: 407 RVA: 0x000121D4 File Offset: 0x000103D4
		[SaveableProperty(3)]
		public Army.ArmyTypes ArmyType { get; set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000198 RID: 408 RVA: 0x000121DD File Offset: 0x000103DD
		// (set) Token: 0x06000199 RID: 409 RVA: 0x000121E5 File Offset: 0x000103E5
		[SaveableProperty(4)]
		public Hero ArmyOwner { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600019A RID: 410 RVA: 0x000121EE File Offset: 0x000103EE
		// (set) Token: 0x0600019B RID: 411 RVA: 0x000121F6 File Offset: 0x000103F6
		[SaveableProperty(5)]
		public float Cohesion { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00012200 File Offset: 0x00010400
		public float DailyCohesionChange
		{
			get
			{
				return Campaign.Current.Models.ArmyManagementCalculationModel.CalculateDailyCohesionChange(this, false).ResultNumber;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600019D RID: 413 RVA: 0x0001222B File Offset: 0x0001042B
		public ExplainedNumber DailyCohesionChangeExplanation
		{
			get
			{
				return Campaign.Current.Models.ArmyManagementCalculationModel.CalculateDailyCohesionChange(this, true);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00012243 File Offset: 0x00010443
		public int CohesionThresholdForDispersion
		{
			get
			{
				return Campaign.Current.Models.ArmyManagementCalculationModel.CohesionThresholdForDispersion;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00012259 File Offset: 0x00010459
		public bool IsDispersing
		{
			get
			{
				return this._armyIsDispersing;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00012261 File Offset: 0x00010461
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00012269 File Offset: 0x00010469
		[SaveableProperty(13)]
		public float Morale { get; private set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00012272 File Offset: 0x00010472
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x0001227A File Offset: 0x0001047A
		[SaveableProperty(14)]
		public MobileParty LeaderParty { get; private set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00012283 File Offset: 0x00010483
		public int LeaderPartyAndAttachedPartiesCount
		{
			get
			{
				return this.LeaderParty.AttachedParties.Count + 1;
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00012297 File Offset: 0x00010497
		public override string ToString()
		{
			return this.Name.ToString();
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x000122A4 File Offset: 0x000104A4
		public float EstimatedStrength
		{
			get
			{
				float num = this.LeaderParty.Party.EstimatedStrength;
				foreach (MobileParty mobileParty in this.LeaderParty.AttachedParties)
				{
					num += mobileParty.Party.EstimatedStrength;
				}
				return num;
			}
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00012318 File Offset: 0x00010518
		public float CalculateCurrentStrength()
		{
			float num = this.LeaderParty.Party.CalculateCurrentStrength();
			foreach (MobileParty mobileParty in this.LeaderParty.AttachedParties)
			{
				num += mobileParty.Party.CalculateCurrentStrength();
			}
			return num;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0001238C File Offset: 0x0001058C
		public float GetCustomStrength(BattleSideEnum side, MapEvent.PowerCalculationContext context)
		{
			float num = this.LeaderParty.Party.GetCustomStrength(side, context);
			foreach (MobileParty mobileParty in this.LeaderParty.AttachedParties)
			{
				num += mobileParty.Party.GetCustomStrength(side, context);
			}
			return num;
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00012404 File Offset: 0x00010604
		// (set) Token: 0x060001AA RID: 426 RVA: 0x0001240C File Offset: 0x0001060C
		public Kingdom Kingdom
		{
			get
			{
				return this._kingdom;
			}
			set
			{
				if (value != this._kingdom)
				{
					Kingdom kingdom = this._kingdom;
					if (kingdom != null)
					{
						kingdom.RemoveArmyInternal(this);
					}
					this._kingdom = value;
					Kingdom kingdom2 = this._kingdom;
					if (kingdom2 == null)
					{
						return;
					}
					kingdom2.AddArmyInternal(this);
				}
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00012441 File Offset: 0x00010641
		// (set) Token: 0x060001AC RID: 428 RVA: 0x00012449 File Offset: 0x00010649
		public IMapPoint AiBehaviorObject
		{
			get
			{
				return this._aiBehaviorObject;
			}
			set
			{
				if (value != this._aiBehaviorObject && this.Parties.Contains(MobileParty.MainParty) && this.LeaderParty != MobileParty.MainParty)
				{
					this.StopTrackingTargetSettlement();
					this.StartTrackingTargetSettlement(value);
				}
				this._aiBehaviorObject = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060001AD RID: 429 RVA: 0x00012487 File Offset: 0x00010687
		// (set) Token: 0x060001AE RID: 430 RVA: 0x0001248F File Offset: 0x0001068F
		[SaveableProperty(17)]
		public TextObject Name { get; private set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00012498 File Offset: 0x00010698
		private float InactivityThreshold
		{
			get
			{
				return (float)CampaignTime.HoursInDay * 2f;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x000124A8 File Offset: 0x000106A8
		public int TotalHealthyMembers
		{
			get
			{
				return this.LeaderParty.Party.NumberOfHealthyMembers + this.LeaderParty.AttachedParties.Sum<MobileParty>((MobileParty mobileParty) => mobileParty.Party.NumberOfHealthyMembers);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x000124F8 File Offset: 0x000106F8
		public int TotalManCount
		{
			get
			{
				return this.LeaderParty.Party.MemberRoster.TotalManCount + this.LeaderParty.AttachedParties.Sum<MobileParty>((MobileParty mobileParty) => mobileParty.Party.MemberRoster.TotalManCount);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x0001254C File Offset: 0x0001074C
		public int TotalRegularCount
		{
			get
			{
				return this.LeaderParty.Party.MemberRoster.TotalRegulars + this.LeaderParty.AttachedParties.Sum<MobileParty>((MobileParty mobileParty) => mobileParty.Party.MemberRoster.TotalRegulars);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0001259E File Offset: 0x0001079E
		public bool IsReady
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x000125A4 File Offset: 0x000107A4
		public Army(Kingdom kingdom, MobileParty leaderParty, Army.ArmyTypes armyType)
		{
			this.Kingdom = kingdom;
			this._parties = new MBList<MobileParty>();
			this._armyGatheringStartTime = 0f;
			this._creationTime = CampaignTime.Now;
			this.LeaderParty = leaderParty;
			this.LeaderParty.Army = this;
			this.ArmyOwner = this.LeaderParty.LeaderHero;
			this.UpdateName();
			this.ArmyType = armyType;
			this.AddEventHandlers();
			this.Cohesion = 100f;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00012624 File Offset: 0x00010824
		public void UpdateName()
		{
			this.Name = new TextObject("{=nbmctMLk}{LEADER_NAME}{.o} Army", null);
			this.Name.SetTextVariable("LEADER_NAME", (this.ArmyOwner != null) ? this.ArmyOwner.Name : ((this.LeaderParty.Owner != null) ? this.LeaderParty.Owner.Name : TextObject.GetEmpty()));
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0001268C File Offset: 0x0001088C
		private void AddEventHandlers()
		{
			if (this._creationTime == default(CampaignTime))
			{
				this._creationTime = CampaignTime.HoursFromNow(MBRandom.RandomFloat - 2f);
			}
			CampaignTime campaignTime = CampaignTime.Now - this._creationTime;
			CampaignTime campaignTime2 = CampaignTime.Hours(1f + (float)((int)campaignTime.ToHours)) - campaignTime;
			this._hourlyTickEvent = CampaignPeriodicEventManager.CreatePeriodicEvent(CampaignTime.Hours(1f), campaignTime2);
			this._hourlyTickEvent.AddHandler(new MBCampaignEvent.CampaignEventDelegate(this.HourlyTick));
			this._tickEvent = CampaignPeriodicEventManager.CreatePeriodicEvent(CampaignTime.Hours(0.1f), CampaignTime.Hours(1f));
			this._tickEvent.AddHandler(new MBCampaignEvent.CampaignEventDelegate(this.Tick));
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.OnSiegeEventStartedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeStarted));
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00012784 File Offset: 0x00010984
		private void OnSiegeStarted(SiegeEvent siegeEvent)
		{
			Settlement settlement;
			if (this.IsWaitingForArmyMembers() && (settlement = this.AiBehaviorObject as Settlement) != null && settlement == siegeEvent.BesiegedSettlement && this.LeaderParty.SiegeEvent == null && this.LeaderParty.MapEvent == null)
			{
				this.FindBestGatheringSettlementAndMoveTheLeader(settlement);
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x000127D4 File Offset: 0x000109D4
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			Settlement settlement2;
			if (this.IsWaitingForArmyMembers() && (settlement2 = this.AiBehaviorObject as Settlement) != null && settlement2 == settlement && settlement.MapFaction != this.LeaderParty.MapFaction && this.LeaderParty.SiegeEvent == null && this.LeaderParty.MapEvent == null)
			{
				this.FindBestGatheringSettlementAndMoveTheLeader(settlement);
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00012830 File Offset: 0x00010A30
		internal void OnAfterLoad()
		{
			this.AddEventHandlers();
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00012838 File Offset: 0x00010A38
		public bool DoesLeaderPartyAndAttachedPartiesContain(MobileParty party)
		{
			return this.LeaderParty == party || this.LeaderParty.AttachedParties.IndexOf(party) >= 0;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0001285C File Offset: 0x00010A5C
		public void BoostCohesionWithInfluence(float cohesionToGain, int cost)
		{
			if (this.LeaderParty.LeaderHero.Clan.Influence >= (float)cost)
			{
				ChangeClanInfluenceAction.Apply(this.LeaderParty.LeaderHero.Clan, (float)(-(float)cost));
				this.Cohesion += cohesionToGain;
				this._numberOfBoosts++;
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000128B8 File Offset: 0x00010AB8
		private void ThinkAboutCohesionBoost()
		{
			float num = 0f;
			foreach (MobileParty mobileParty in this.Parties)
			{
				float partySizeRatio = mobileParty.PartySizeRatio;
				num += partySizeRatio;
			}
			float num2 = num / (float)this.Parties.Count;
			float num3 = MathF.Min(1f, num2);
			float num4 = Campaign.Current.Models.TargetScoreCalculatingModel.CurrentObjectiveValue(this.LeaderParty);
			if (num4 > 0.01f)
			{
				num4 *= num3;
				num4 *= ((this._numberOfBoosts == 0) ? 1f : (1f / MathF.Pow(1f + (float)this._numberOfBoosts, 0.7f)));
				ArmyManagementCalculationModel armyManagementCalculationModel = Campaign.Current.Models.ArmyManagementCalculationModel;
				float num5 = MathF.Min(100f, 100f - this.Cohesion);
				int num6 = armyManagementCalculationModel.CalculateTotalInfluenceCost(this, num5);
				if (this.LeaderParty.Party.Owner.Clan.Influence > (float)num6)
				{
					float num7 = MathF.Min(9f, MathF.Sqrt(this.LeaderParty.Party.Owner.Clan.Influence / (float)num6));
					float num8 = ((this.LeaderParty.BesiegedSettlement != null) ? 2f : 1f);
					if (this.LeaderParty.BesiegedSettlement == null && this.LeaderParty.DefaultBehavior == AiBehavior.BesiegeSettlement)
					{
						float num9;
						if (this.LeaderParty.CurrentSettlement != null)
						{
							num9 = Campaign.Current.Models.MapDistanceModel.GetDistance(this.LeaderParty.CurrentSettlement, this.LeaderParty.TargetSettlement, this.LeaderParty.IsCurrentlyAtSea, this.LeaderParty.IsTargetingPort, this.LeaderParty.NavigationCapability);
						}
						else
						{
							float num10;
							num9 = Campaign.Current.Models.MapDistanceModel.GetDistance(this.LeaderParty, this.LeaderParty.TargetSettlement, this.LeaderParty.IsTargetingPort, this.LeaderParty.NavigationCapability, out num10);
						}
						float num11 = Campaign.Current.GetAverageDistanceBetweenClosestTwoTownsWithNavigationType(this.LeaderParty.NavigationCapability) * 2f;
						if (num9 < num11)
						{
							num8 += (1f - num9 / num11) * (1f - num9 / num11);
						}
					}
					float num12 = num4 * num8 * 0.25f * num7;
					if (MBRandom.RandomFloat < num12)
					{
						this.BoostCohesionWithInfluence(num5, num6);
					}
				}
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00012B44 File Offset: 0x00010D44
		public void RecalculateArmyMorale()
		{
			float num = 0f;
			foreach (MobileParty mobileParty in this.Parties)
			{
				num += mobileParty.Morale;
			}
			this.Morale = num / (float)this.Parties.Count;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00012BB4 File Offset: 0x00010DB4
		private void HourlyTick(MBCampaignEvent campaignEvent, object[] delegateParams)
		{
			bool flag = this.LeaderParty.CurrentSettlement != null && this.LeaderParty.CurrentSettlement.SiegeEvent != null;
			if (this.LeaderParty.MapEvent != null || flag)
			{
				return;
			}
			this.RecalculateArmyMorale();
			this.Cohesion += this.DailyCohesionChange / (float)CampaignTime.HoursInDay;
			if (this.LeaderParty != MobileParty.MainParty)
			{
				if (this._armyGatheringStartTime == 0f)
				{
					this.CheckAndSetArmyGatheringTime();
				}
				this.MoveLeaderToGatheringLocationIfNeeded();
				if (this.Cohesion < 50f)
				{
					this.ThinkAboutCohesionBoost();
					if (this.Cohesion < 30f && this.LeaderParty.MapEvent == null && this.LeaderParty.SiegeEvent == null)
					{
						DisbandArmyAction.ApplyByCohesionDepleted(this);
						return;
					}
				}
				if (this.LeaderParty.DefaultBehavior == AiBehavior.BesiegeSettlement && this.IsAnotherEnemyBesiegingTarget())
				{
					this.FinishArmyObjective();
				}
			}
			this.CheckArmyDispersion();
			this.ApplyHostileActionInfluenceAwards();
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00012CA8 File Offset: 0x00010EA8
		private void CheckAndSetArmyGatheringTime()
		{
			Settlement settlement;
			float num;
			if ((settlement = this.AiBehaviorObject as Settlement) != null && this.LeaderParty.DefaultBehavior == AiBehavior.GoToPoint && ((this.LeaderParty.CurrentSettlement != null) ? Campaign.Current.Models.MapDistanceModel.GetDistance(this.LeaderParty.CurrentSettlement, settlement, false, false, this.LeaderParty.DesiredAiNavigationType) : Campaign.Current.Models.MapDistanceModel.GetDistance(this.LeaderParty, settlement, false, this.LeaderParty.DesiredAiNavigationType, out num)) < this.GatheringPositionMaxDistanceToTheSettlement * 2f)
			{
				this._armyGatheringStartTime = Campaign.CurrentTime;
			}
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00012D54 File Offset: 0x00010F54
		private void Tick(MBCampaignEvent campaignEvent, object[] delegateParams)
		{
			foreach (MobileParty mobileParty in this._parties)
			{
				if (mobileParty.AttachedTo == null && mobileParty.Army != null && mobileParty.ShortTermTargetParty == this.LeaderParty && mobileParty.MapEvent == null && mobileParty.IsCurrentlyAtSea == this.LeaderParty.IsCurrentlyAtSea)
				{
					float num = (mobileParty.IsCurrentlyAtSea ? Campaign.Current.Models.EncounterModel.MaximumAllowedNavalDistanceForEncounteringMobilePartyInArmy : Campaign.Current.Models.EncounterModel.MaximumAllowedLandDistanceForEncounteringMobilePartyInArmy);
					if ((mobileParty.Position - this.LeaderParty.Position).LengthSquared < num)
					{
						this.AddPartyToMergedParties(mobileParty);
						if (mobileParty.IsMainParty)
						{
							Campaign.Current.CameraFollowParty = this.LeaderParty.Party;
						}
						CampaignEventDispatcher.Instance.OnArmyOverlaySetDirty();
					}
				}
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00012E70 File Offset: 0x00011070
		private void CheckArmyDispersion()
		{
			if (this.LeaderParty == MobileParty.MainParty)
			{
				if (this.Cohesion <= 0.1f)
				{
					DisbandArmyAction.ApplyByCohesionDepleted(this);
					return;
				}
			}
			else
			{
				int num = (this.LeaderParty.Party.IsStarving ? 1 : 0);
				using (List<MobileParty>.Enumerator enumerator = this.LeaderParty.AttachedParties.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Party.IsStarving)
						{
							num++;
						}
					}
				}
				if ((float)num / (float)this.LeaderPartyAndAttachedPartiesCount > 0.5f)
				{
					DisbandArmyAction.ApplyByFoodProblem(this);
					return;
				}
				if (MBRandom.RandomFloat < 0.25f)
				{
					if (!this.LeaderParty.MapFaction.FactionsAtWarWith.AnyQ<IFaction>((IFaction x) => x.Fiefs.Any<Town>()))
					{
						DisbandArmyAction.ApplyByNoActiveWar(this);
						return;
					}
				}
				if (this.Cohesion <= 0.1f)
				{
					DisbandArmyAction.ApplyByCohesionDepleted(this);
					return;
				}
				if (!this.LeaderParty.IsActive)
				{
					DisbandArmyAction.ApplyByUnknownReason(this);
				}
				this.CheckInactivity();
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00012F9C File Offset: 0x0001119C
		private void CheckInactivity()
		{
			if (!this.IsWaitingForArmyMembers())
			{
				AiBehavior aiBehavior = this.LeaderParty.DefaultBehavior;
				switch (aiBehavior)
				{
				case AiBehavior.Hold:
					this._inactivityCounter++;
					goto IL_00A1;
				case AiBehavior.None:
					goto IL_00A1;
				case AiBehavior.GoToSettlement:
					if (!this.LeaderParty.TargetSettlement.MapFaction.IsAtWarWith(this.LeaderParty.MapFaction))
					{
						this._inactivityCounter++;
						goto IL_00A1;
					}
					goto IL_00A1;
				case AiBehavior.AssaultSettlement:
				case AiBehavior.RaidSettlement:
				case AiBehavior.BesiegeSettlement:
					break;
				default:
					if (aiBehavior == AiBehavior.PatrolAroundPoint)
					{
						this._inactivityCounter++;
						goto IL_00A1;
					}
					if (aiBehavior != AiBehavior.DefendSettlement)
					{
						goto IL_00A1;
					}
					break;
				}
				this._inactivityCounter -= 2;
				IL_00A1:
				aiBehavior = this.LeaderParty.ShortTermBehavior;
				if (aiBehavior == AiBehavior.EngageParty)
				{
					this._inactivityCounter--;
				}
			}
			this._inactivityCounter = MBMath.ClampInt(this._inactivityCounter, 0, (int)this.InactivityThreshold);
			if ((float)this._inactivityCounter >= this.InactivityThreshold)
			{
				DisbandArmyAction.ApplyByInactivity(this);
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00013098 File Offset: 0x00011298
		private void MoveLeaderToGatheringLocationIfNeeded()
		{
			if (this.AiBehaviorObject != null && this.LeaderParty.SiegeEvent == null && this.LeaderParty.MapEvent == null && this.LeaderParty.ShortTermBehavior == AiBehavior.Hold && this.IsWaitingForArmyMembers())
			{
				Settlement settlement = this.AiBehaviorObject as Settlement;
				if (!settlement.IsUnderSiege && !settlement.IsUnderRaid)
				{
					CampaignVec2 campaignVec = (this.LeaderParty.IsTargetingPort ? settlement.PortPosition : settlement.GatePosition);
					this.SendLeaderPartyToReachablePointAroundPosition(campaignVec, 6f, 3f);
					return;
				}
				this.FindBestGatheringSettlementAndMoveTheLeader(settlement);
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00013130 File Offset: 0x00011330
		private void ApplyHostileActionInfluenceAwards()
		{
			if (this.LeaderParty.LeaderHero != null && this.LeaderParty.MapEvent != null)
			{
				if (this.LeaderParty.MapEvent.IsRaid && this.LeaderParty.MapEvent.DefenderSide.TroopCount == 0)
				{
					float hourlyInfluenceAwardForRaidingEnemyVillage = Campaign.Current.Models.DiplomacyModel.GetHourlyInfluenceAwardForRaidingEnemyVillage(this.LeaderParty);
					GainKingdomInfluenceAction.ApplyForRaidingEnemyVillage(this.LeaderParty, hourlyInfluenceAwardForRaidingEnemyVillage);
					return;
				}
				if (this.LeaderParty.BesiegedSettlement != null && this.LeaderParty.MapFaction.IsAtWarWith(this.LeaderParty.BesiegedSettlement.MapFaction))
				{
					float hourlyInfluenceAwardForBesiegingEnemyFortification = Campaign.Current.Models.DiplomacyModel.GetHourlyInfluenceAwardForBesiegingEnemyFortification(this.LeaderParty);
					GainKingdomInfluenceAction.ApplyForBesiegingEnemySettlement(this.LeaderParty, hourlyInfluenceAwardForBesiegingEnemyFortification);
				}
			}
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00013204 File Offset: 0x00011404
		public TextObject GetNotificationText()
		{
			if (this.LeaderParty != MobileParty.MainParty)
			{
				TextObject textObject = GameTexts.FindText("str_army_gather", null);
				StringHelpers.SetCharacterProperties("ARMY_LEADER", this.LeaderParty.LeaderHero.CharacterObject, textObject, false);
				textObject.SetTextVariable("SETTLEMENT_NAME", this.AiBehaviorObject.Name);
				return textObject;
			}
			return null;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00013261 File Offset: 0x00011461
		public TextObject GetLongTermBehaviorText(bool setWithLink = false)
		{
			if (this.LeaderParty.IsMainParty)
			{
				return this.GetLongTermBehaviorTextForPlayerParty();
			}
			return this.GetLongTermBehaviorTextForAILeadedParty(setWithLink);
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00013280 File Offset: 0x00011480
		private TextObject GetLongTermBehaviorTextForPlayerParty()
		{
			TextObject textObject;
			if (MobileParty.MainParty.TargetSettlement != null && MobileParty.MainParty.CurrentSettlement != MobileParty.MainParty.TargetSettlement)
			{
				textObject = GameTexts.FindText("str_army_going_to_settlement", null);
				textObject.SetTextVariable("SETTLEMENT_NAME", this.LeaderParty.Ai.AiBehaviorPartyBase.Name);
			}
			else if (MobileParty.MainParty.CurrentSettlement != null)
			{
				textObject = GameTexts.FindText("str_army_waiting_in_settlement", null);
				textObject.SetTextVariable("SETTLEMENT_NAME", MobileParty.MainParty.CurrentSettlement.Name);
			}
			else if (MobileParty.MainParty.TargetParty != null)
			{
				textObject = new TextObject("{=P4QFKVSU}Moving to {TARGET_PARTY}.", null);
				textObject.SetTextVariable("TARGET_PARTY", MobileParty.MainParty.TargetParty.Name);
			}
			else if (MobileParty.MainParty.IsMoving)
			{
				textObject = new TextObject("{=b9TbdM9A}Moving to a point.", null);
			}
			else
			{
				textObject = new TextObject("{=RClxLG6N}Holding.", null);
			}
			return textObject;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x00013374 File Offset: 0x00011574
		private TextObject GetLongTermBehaviorTextForAILeadedParty(bool setWithLink)
		{
			AiBehavior defaultBehavior = this.LeaderParty.DefaultBehavior;
			if (defaultBehavior <= AiBehavior.GoToPoint)
			{
				switch (defaultBehavior)
				{
				case AiBehavior.Hold:
					break;
				case AiBehavior.None:
				case AiBehavior.AssaultSettlement:
					goto IL_029A;
				case AiBehavior.GoToSettlement:
				{
					TextObject textObject = ((this.LeaderParty.CurrentSettlement == null) ? GameTexts.FindText("str_army_going_to_settlement", null) : GameTexts.FindText("str_army_waiting_in_settlement", null));
					textObject.SetTextVariable("SETTLEMENT_NAME", (setWithLink && this.AiBehaviorObject is Settlement) ? ((Settlement)this.AiBehaviorObject).EncyclopediaLinkWithName : (this.AiBehaviorObject.Name ?? this.LeaderParty.Ai.AiBehaviorPartyBase.Name));
					return textObject;
				}
				case AiBehavior.RaidSettlement:
				{
					Settlement settlement = (Settlement)this.AiBehaviorObject;
					TextObject textObject = ((this.LeaderParty.MapEvent != null && this.LeaderParty.MapEvent.IsRaid) ? GameTexts.FindText("str_army_raiding", null) : GameTexts.FindText("str_army_raiding_travelling", null));
					textObject.SetTextVariable("SETTLEMENT_NAME", setWithLink ? settlement.EncyclopediaLinkWithName : this.AiBehaviorObject.Name);
					return textObject;
				}
				case AiBehavior.BesiegeSettlement:
				{
					Settlement settlement2 = (Settlement)this.AiBehaviorObject;
					TextObject textObject = GameTexts.FindText((this.LeaderParty.SiegeEvent != null) ? "str_army_besieging" : "str_army_besieging_travelling", null);
					if (settlement2.IsVillage)
					{
						textObject = GameTexts.FindText("str_army_patrolling_travelling", null);
					}
					textObject.SetTextVariable("SETTLEMENT_NAME", setWithLink ? settlement2.EncyclopediaLinkWithName : this.AiBehaviorObject.Name);
					return textObject;
				}
				default:
					if (defaultBehavior != AiBehavior.GoToPoint)
					{
						goto IL_029A;
					}
					break;
				}
				if (this.IsWaitingForArmyMembers() && this.AiBehaviorObject != null)
				{
					TextObject textObject = GameTexts.FindText("str_army_gathering", null);
					textObject.SetTextVariable("SETTLEMENT_NAME", setWithLink ? ((Settlement)this.AiBehaviorObject).EncyclopediaLinkWithName : this.AiBehaviorObject.Name);
					return textObject;
				}
			}
			else
			{
				if (defaultBehavior == AiBehavior.PatrolAroundPoint)
				{
					TextObject textObject = GameTexts.FindText("str_army_patrolling_travelling", null);
					textObject.SetTextVariable("SETTLEMENT_NAME", setWithLink ? ((Settlement)this.AiBehaviorObject).EncyclopediaLinkWithName : this.AiBehaviorObject.Name);
					return textObject;
				}
				if (defaultBehavior == AiBehavior.DefendSettlement)
				{
					Settlement settlement2 = (Settlement)this.AiBehaviorObject;
					TextObject textObject = ((this.LeaderParty.Position.Distance(settlement2.Position) > Campaign.Current.EstimatedAverageLordPartySpeed * (float)CampaignTime.HoursInDay * 0.33f) ? GameTexts.FindText("str_army_defending_travelling", null) : GameTexts.FindText("str_army_defending", null));
					textObject.SetTextVariable("SETTLEMENT_NAME", setWithLink ? settlement2.EncyclopediaLinkWithName : this.AiBehaviorObject.Name);
					return textObject;
				}
			}
			IL_029A:
			if (this.LeaderParty.MapEvent != null)
			{
				TextObject textObject;
				if (this.LeaderParty.MapEvent.MapEventSettlement != null)
				{
					textObject = ((this.LeaderParty.MapEventSide == this.LeaderParty.MapEvent.DefenderSide) ? new TextObject("{=rGy8vjOv}Defending {TARGET_SETTLEMENT}.", null) : new TextObject("{=exnL6SS7}Attacking {TARGET_SETTLEMENT}.", null));
					textObject.SetTextVariable("TARGET_SETTLEMENT", this.LeaderParty.MapEvent.MapEventSettlement.Name);
					return textObject;
				}
				textObject = new TextObject("{=5bzk75Ql}Engaging {TARGET_PARTY}.", null);
				textObject.SetTextVariable("TARGET_PARTY", (this.LeaderParty.MapEventSide == this.LeaderParty.MapEvent.DefenderSide) ? this.LeaderParty.MapEvent.AttackerSide.LeaderParty.Name : this.LeaderParty.MapEvent.DefenderSide.LeaderParty.Name);
				return textObject;
			}
			else
			{
				if (this.LeaderParty.SiegeEvent != null)
				{
					TextObject textObject = new TextObject("{=JTxI3sW2}Besieging {TARGET_SETTLEMENT}.", null);
					textObject.SetTextVariable("TARGET_SETTLEMENT", this.LeaderParty.BesiegedSettlement.Name);
					return textObject;
				}
				return TextObject.GetEmpty();
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x00013740 File Offset: 0x00011940
		public void Gather(Settlement initialHostileSettlement, MBReadOnlyList<MobileParty> partiesToCallToArmy = null)
		{
			Settlement settlement = null;
			if (this.LeaderParty != MobileParty.MainParty)
			{
				this.FindBestGatheringSettlementAndMoveTheLeader(initialHostileSettlement);
				if (partiesToCallToArmy == null)
				{
					goto IL_0093;
				}
				using (List<MobileParty>.Enumerator enumerator = partiesToCallToArmy.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MobileParty mobileParty = enumerator.Current;
						mobileParty.Army = this;
					}
					goto IL_0093;
				}
			}
			Settlement settlement2;
			if ((settlement2 = SettlementHelper.FindNearestSettlementToMobileParty(MobileParty.MainParty, MobileParty.MainParty.NavigationCapability, (Settlement x) => x.IsFortification || x.IsVillage)) == null)
			{
				CampaignVec2 position = MobileParty.MainParty.Position;
				settlement2 = SettlementHelper.FindNearestSettlementToPoint(in position, null);
			}
			settlement = settlement2;
			IL_0093:
			GatherArmyAction.Apply(this.LeaderParty, settlement);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000137FC File Offset: 0x000119FC
		private void FindBestGatheringSettlementAndMoveTheLeader(Settlement focusSettlement)
		{
			Settlement settlement = null;
			Hero leaderHero = this.LeaderParty.LeaderHero;
			float num = float.MinValue;
			if (leaderHero != null && leaderHero.IsActive)
			{
				using (List<Settlement>.Enumerator enumerator = this.Kingdom.Settlements.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Settlement settlement2 = enumerator.Current;
						if (settlement2.IsFortification && !settlement2.IsUnderSiege)
						{
							float num2;
							if (this.LeaderParty.CurrentSettlement == null)
							{
								float num3;
								num2 = Campaign.Current.Models.MapDistanceModel.GetDistance(this.LeaderParty, settlement2, false, this.LeaderParty.NavigationCapability, out num3);
							}
							else
							{
								float num3;
								num2 = Campaign.Current.Models.MapDistanceModel.GetDistance(this.LeaderParty.CurrentSettlement, settlement2, false, false, this.LeaderParty.NavigationCapability, out num3);
							}
							if (num2 < Campaign.MapDiagonalSquared)
							{
								float distance = Campaign.Current.Models.MapDistanceModel.GetDistance(focusSettlement, settlement2, false, false, this.LeaderParty.NavigationCapability);
								if (distance < Campaign.MapDiagonalSquared && distance > this.MinimumDistanceToTargetWhileGatheringAsAttackerArmy)
								{
									float num4 = 0f;
									if (settlement == null)
									{
										num4 += 0.001f;
									}
									if (settlement2 != focusSettlement && settlement2.Party.MapEvent == null)
									{
										if (settlement2.MapFaction == this.Kingdom)
										{
											num4 += 10f;
										}
										else if (!FactionManager.IsAtWarAgainstFaction(settlement2.MapFaction, this.Kingdom))
										{
											num4 += 2f;
										}
										bool flag = false;
										foreach (Army army in this.Kingdom.Armies)
										{
											if (army != this && army.AiBehaviorObject == settlement2)
											{
												flag = true;
											}
										}
										if (!flag)
										{
											num4 += 10f;
										}
										float num5 = distance / (Campaign.MapDiagonalSquared * 0.1f);
										float num6 = 20f * (1f - num5);
										float num7 = (settlement2.Position.ToVec2() - this.LeaderParty.Position.ToVec2()).Length / (Campaign.MapDiagonalSquared * 0.1f);
										float num8 = 5f * (1f - num7);
										float num9 = num4 + num6 * 0.5f + num8 * 0.1f;
										if (num9 > num)
										{
											num = num9;
											settlement = settlement2;
										}
									}
								}
							}
						}
					}
					goto IL_0295;
				}
			}
			settlement = (Settlement)this.AiBehaviorObject;
			IL_0295:
			if (settlement == null)
			{
				settlement = SettlementHelper.FindNearestFortificationToMobileParty(this.LeaderParty, this.LeaderParty.NavigationCapability, null);
			}
			this.AiBehaviorObject = settlement;
			CampaignVec2 gatePosition = settlement.GatePosition;
			this.SendLeaderPartyToReachablePointAroundPosition(gatePosition, this.GatheringPositionMaxDistanceToTheSettlement, this.GatheringPositionMinDistanceToTheSettlement);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00013B10 File Offset: 0x00011D10
		public bool IsWaitingForArmyMembers()
		{
			if (this._armyGatheringStartTime > 0f)
			{
				float num = Campaign.CurrentTime - this._armyGatheringStartTime;
				float num2 = this.EstimatedStrength / this.Parties.SumQ<MobileParty>((MobileParty x) => x.Party.EstimatedStrength);
				bool flag;
				if (num < Campaign.Current.Models.ArmyManagementCalculationModel.MaximumWaitTime)
				{
					flag = num2 > 0.9f;
				}
				else
				{
					float num3 = (num - Campaign.Current.Models.ArmyManagementCalculationModel.MaximumWaitTime) * 0.01f;
					flag = num2 > 0.75f - num3;
				}
				return !flag;
			}
			return true;
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00013BC0 File Offset: 0x00011DC0
		private bool IsAnotherEnemyBesiegingTarget()
		{
			Settlement settlement = (Settlement)this.AiBehaviorObject;
			return this.ArmyType == Army.ArmyTypes.Besieger && settlement.IsUnderSiege && settlement.SiegeEvent.BesiegerCamp.MapFaction.IsAtWarWith(this.LeaderParty.MapFaction);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00013C0B File Offset: 0x00011E0B
		public void FinishArmyObjective()
		{
			this.LeaderParty.SetMoveModeHold();
			this.AiBehaviorObject = null;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00013C20 File Offset: 0x00011E20
		internal void DisperseInternal(Army.ArmyDispersionReason reason = Army.ArmyDispersionReason.Unknown)
		{
			if (this._armyIsDispersing)
			{
				return;
			}
			CampaignEventDispatcher.Instance.OnArmyDispersed(this, reason, this.Parties.Contains(MobileParty.MainParty));
			this._armyIsDispersing = true;
			int num = 0;
			for (int i = this.Parties.Count - 1; i >= num; i--)
			{
				MobileParty mobileParty = this.Parties[i];
				bool flag = mobileParty.AttachedTo == this.LeaderParty;
				mobileParty.Army = null;
				if (flag && mobileParty.CurrentSettlement == null && mobileParty.IsActive && (!this.LeaderParty.IsCurrentlyAtSea || mobileParty.HasNavalNavigationCapability))
				{
					MobileParty.NavigationType navigationType = (mobileParty.IsCurrentlyAtSea ? MobileParty.NavigationType.Naval : MobileParty.NavigationType.Default);
					mobileParty.Position = NavigationHelper.FindReachablePointAroundPosition(this.LeaderParty.Position, navigationType, 1f, 0f, false);
					mobileParty.SetMoveModeHold();
				}
			}
			this._parties.Clear();
			this.Kingdom = null;
			if (this.LeaderParty == MobileParty.MainParty)
			{
				MapState mapState = Game.Current.GameStateManager.ActiveState as MapState;
				if (mapState != null)
				{
					mapState.OnDispersePlayerLeadedArmy();
				}
			}
			this._hourlyTickEvent.DeletePeriodicEvent();
			this._tickEvent.DeletePeriodicEvent();
			this._armyIsDispersing = false;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00013D54 File Offset: 0x00011F54
		public Vec2 GetRelativePositionForParty(MobileParty mobileParty, Vec2 armyFacing)
		{
			float num = 0.5f;
			int num2 = -1;
			int num3 = 0;
			for (int i = 0; i < this.LeaderParty.AttachedParties.Count; i++)
			{
				MobileParty mobileParty2 = this.LeaderParty.AttachedParties[i];
				if (!this.LeaderParty.IsCurrentlyAtSea || mobileParty2.Ships.Count > 0)
				{
					if (mobileParty2 == mobileParty)
					{
						num2 = num3;
					}
					num3++;
				}
			}
			if (this.LeaderParty.IsCurrentlyAtSea && this.LeaderParty.Ships.Count == 0)
			{
				num2--;
			}
			float num4 = (float)MathF.Ceiling(-1f + MathF.Sqrt(1f + 8f * (float)(num3 - 1))) / 4f * num * 0.5f + num;
			int num5 = MathF.Ceiling((-1f + MathF.Sqrt(1f + 8f * (float)(num2 + 2))) / 2f) - 1;
			int num6 = num2 + 1 - num5 * (num5 + 1) / 2;
			bool flag = (num5 & 1) != 0;
			num6 = ((((num6 & 1) != 0) ? (-1 - num6) : num6) >> 1) * (flag ? (-1) : 1);
			float num7 = 1.25f;
			CampaignVec2 campaignVec = new CampaignVec2(this.LeaderParty.VisualPosition2DWithoutError + -armyFacing * 0.1f * (float)num3, !this.LeaderParty.IsCurrentlyAtSea);
			Vec2 vec = campaignVec.ToVec2() - (float)MathF.Sign((float)num6 - (((num5 & 1) != 0) ? 0.5f : 0f)) * armyFacing.LeftVec() * num4;
			int[] invalidTerrainTypesForNavigationType = Campaign.Current.Models.PartyNavigationModel.GetInvalidTerrainTypesForNavigationType(this.LeaderParty.IsCurrentlyAtSea ? MobileParty.NavigationType.Naval : MobileParty.NavigationType.Default);
			Vec2 lastPointOnNavigationMeshFromPositionToDestination = Campaign.Current.MapSceneWrapper.GetLastPointOnNavigationMeshFromPositionToDestination(campaignVec.Face, campaignVec.ToVec2(), vec, invalidTerrainTypesForNavigationType);
			if ((vec - lastPointOnNavigationMeshFromPositionToDestination).LengthSquared > 2.25E-06f)
			{
				num = num * (campaignVec - lastPointOnNavigationMeshFromPositionToDestination).Length / num4;
				num7 = num7 * (campaignVec - lastPointOnNavigationMeshFromPositionToDestination).Length / (num4 / 1.5f);
			}
			if (this.LeaderParty.IsCurrentlyAtSea)
			{
				num7 *= 3f;
				num *= 3f;
			}
			return new Vec2((flag ? (-num * 0.5f) : 0f) + (float)num6 * num + mobileParty.Party.RandomFloat(-0.25f, 0.25f) * 0.6f * num, ((float)(-(float)num5) + mobileParty.Party.RandomFloatWithSeed(1U, -0.25f, 0.25f)) * num7 * 0.3f);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00014010 File Offset: 0x00012210
		public void AddPartyToMergedParties(MobileParty mobileParty)
		{
			mobileParty.AttachedTo = this.LeaderParty;
			if (mobileParty.IsMainParty)
			{
				MapState mapState = GameStateManager.Current.ActiveState as MapState;
				if (mapState != null)
				{
					mapState.OnJoinArmy();
				}
				Hero leaderHero = this.LeaderParty.LeaderHero;
				if (leaderHero != null && leaderHero != Hero.MainHero && !leaderHero.HasMet)
				{
					leaderHero.SetHasMet();
				}
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00014070 File Offset: 0x00012270
		internal void OnRemovePartyInternal(MobileParty mobileParty)
		{
			mobileParty.Ai.SetInitiative(1f, 1f, 24f);
			this._parties.Remove(mobileParty);
			CampaignEventDispatcher.Instance.OnPartyRemovedFromArmy(mobileParty);
			if (this == MobileParty.MainParty.Army && !this._armyIsDispersing)
			{
				CampaignEventDispatcher.Instance.OnArmyOverlaySetDirty();
			}
			mobileParty.AttachedTo = null;
			if (this.LeaderParty == mobileParty && !this._armyIsDispersing)
			{
				DisbandArmyAction.ApplyByLeaderPartyRemoved(this);
			}
			if (mobileParty == MobileParty.MainParty)
			{
				Campaign.Current.CameraFollowParty = MobileParty.MainParty.Party;
				this.StopTrackingTargetSettlement();
			}
			Army army = mobileParty.Army;
			if (((army != null) ? army.LeaderParty : null) == mobileParty)
			{
				this.FinishArmyObjective();
				if (!this._armyIsDispersing)
				{
					Army army2 = mobileParty.Army;
					if (((army2 != null) ? army2.LeaderParty.LeaderHero : null) == null)
					{
						DisbandArmyAction.ApplyByArmyLeaderIsDead(mobileParty.Army);
					}
					else
					{
						DisbandArmyAction.ApplyByObjectiveFinished(mobileParty.Army);
					}
				}
			}
			else if (this.Parties.Count == 0 && !this._armyIsDispersing)
			{
				if (mobileParty.Army != null && MobileParty.MainParty.Army != null && mobileParty.Army == MobileParty.MainParty.Army && Hero.MainHero.IsPrisoner)
				{
					DisbandArmyAction.ApplyByPlayerTakenPrisoner(this);
				}
				else
				{
					DisbandArmyAction.ApplyByNotEnoughParty(this);
				}
			}
			mobileParty.Party.SetVisualAsDirty();
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000141C8 File Offset: 0x000123C8
		internal void OnAddPartyInternal(MobileParty mobileParty)
		{
			this._parties.Add(mobileParty);
			mobileParty.Ai.RethinkAtNextHourlyTick = true;
			CampaignEventDispatcher.Instance.OnPartyJoinedArmy(mobileParty);
			if (this == MobileParty.MainParty.Army && this.LeaderParty != MobileParty.MainParty)
			{
				this.StartTrackingTargetSettlement(this.AiBehaviorObject);
				CampaignEventDispatcher.Instance.OnArmyOverlaySetDirty();
			}
			if (!mobileParty.IsMainParty)
			{
				mobileParty.Ai.RethinkAtNextHourlyTick = true;
			}
			if (mobileParty != MobileParty.MainParty && this.LeaderParty != MobileParty.MainParty && this.LeaderParty.LeaderHero != null)
			{
				int num = -Campaign.Current.Models.ArmyManagementCalculationModel.CalculatePartyInfluenceCost(this.LeaderParty, mobileParty);
				ChangeClanInfluenceAction.Apply(this.LeaderParty.LeaderHero.Clan, (float)num);
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00014292 File Offset: 0x00012492
		private void SendLeaderPartyToReachablePointAroundPosition(CampaignVec2 centerPosition, float distanceLimit, float innerCenterMinimumDistanceLimit = 0f)
		{
			this.LeaderParty.SetMoveGoToPoint(NavigationHelper.FindReachablePointAroundPosition(centerPosition, MobileParty.NavigationType.Default, distanceLimit, innerCenterMinimumDistanceLimit, false), this.LeaderParty.NavigationCapability);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x000142B4 File Offset: 0x000124B4
		private void StartTrackingTargetSettlement(IMapPoint targetObject)
		{
			Settlement settlement = targetObject as Settlement;
			if (settlement != null)
			{
				Campaign.Current.VisualTrackerManager.RegisterObject(settlement);
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000142DC File Offset: 0x000124DC
		private void StopTrackingTargetSettlement()
		{
			Settlement settlement = this.AiBehaviorObject as Settlement;
			if (settlement != null)
			{
				Campaign.Current.VisualTrackerManager.RemoveTrackedObject(settlement, false);
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0001430C File Offset: 0x0001250C
		public void SetPositionAfterMapChange(CampaignVec2 newPosition)
		{
			this.LeaderParty.SetPositionAfterMapChange(newPosition);
			foreach (MobileParty mobileParty in this.LeaderParty.AttachedParties)
			{
				mobileParty.SetPositionAfterMapChange(newPosition);
			}
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00014370 File Offset: 0x00012570
		public void CheckPositionsForMapChangeAndUpdateIfNeeded()
		{
			if (!NavigationHelper.IsPositionValidForNavigationType(this.LeaderParty.Position, this.LeaderParty.NavigationCapability))
			{
				CampaignVec2 closestNavMeshFaceCenterPositionForPosition = NavigationHelper.GetClosestNavMeshFaceCenterPositionForPosition(this.LeaderParty.Position, Campaign.Current.Models.PartyNavigationModel.GetInvalidTerrainTypesForNavigationType(this.LeaderParty.NavigationCapability));
				this.LeaderParty.Position = NavigationHelper.FindReachablePointAroundPosition(closestNavMeshFaceCenterPositionForPosition, this.LeaderParty.NavigationCapability, 8f, 1f, false);
				foreach (MobileParty mobileParty in this.LeaderParty.AttachedParties)
				{
					mobileParty.SetPositionAfterMapChange(this.LeaderParty.Position);
				}
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00014448 File Offset: 0x00012648
		Banner ITrackableCampaignObject.GetBanner()
		{
			return this.LeaderParty.Banner;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00014455 File Offset: 0x00012655
		TextObject ITrackableBase.GetName()
		{
			return this.Name;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0001445D File Offset: 0x0001265D
		Vec3 ITrackableBase.GetPosition()
		{
			return this.LeaderParty.GetPositionAsVec3();
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0001446A File Offset: 0x0001266A
		internal static void AutoGeneratedStaticCollectObjectsArmy(object o, List<object> collectedObjects)
		{
			((Army)o).AutoGeneratedInstanceCollectObjects(collectedObjects);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00014478 File Offset: 0x00012678
		protected virtual void AutoGeneratedInstanceCollectObjects(List<object> collectedObjects)
		{
			collectedObjects.Add(this._parties);
			CampaignTime.AutoGeneratedStaticCollectObjectsCampaignTime(this._creationTime, collectedObjects);
			collectedObjects.Add(this._kingdom);
			collectedObjects.Add(this._aiBehaviorObject);
			collectedObjects.Add(this.ArmyOwner);
			collectedObjects.Add(this.LeaderParty);
			collectedObjects.Add(this.Name);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000144DE File Offset: 0x000126DE
		internal static object AutoGeneratedGetMemberValueArmyType(object o)
		{
			return ((Army)o).ArmyType;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000144F0 File Offset: 0x000126F0
		internal static object AutoGeneratedGetMemberValueArmyOwner(object o)
		{
			return ((Army)o).ArmyOwner;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x000144FD File Offset: 0x000126FD
		internal static object AutoGeneratedGetMemberValueCohesion(object o)
		{
			return ((Army)o).Cohesion;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0001450F File Offset: 0x0001270F
		internal static object AutoGeneratedGetMemberValueMorale(object o)
		{
			return ((Army)o).Morale;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00014521 File Offset: 0x00012721
		internal static object AutoGeneratedGetMemberValueLeaderParty(object o)
		{
			return ((Army)o).LeaderParty;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0001452E File Offset: 0x0001272E
		internal static object AutoGeneratedGetMemberValueName(object o)
		{
			return ((Army)o).Name;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0001453B File Offset: 0x0001273B
		internal static object AutoGeneratedGetMemberValue_parties(object o)
		{
			return ((Army)o)._parties;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00014548 File Offset: 0x00012748
		internal static object AutoGeneratedGetMemberValue_creationTime(object o)
		{
			return ((Army)o)._creationTime;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0001455A File Offset: 0x0001275A
		internal static object AutoGeneratedGetMemberValue_armyGatheringStartTime(object o)
		{
			return ((Army)o)._armyGatheringStartTime;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0001456C File Offset: 0x0001276C
		internal static object AutoGeneratedGetMemberValue_armyIsDispersing(object o)
		{
			return ((Army)o)._armyIsDispersing;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0001457E File Offset: 0x0001277E
		internal static object AutoGeneratedGetMemberValue_numberOfBoosts(object o)
		{
			return ((Army)o)._numberOfBoosts;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00014590 File Offset: 0x00012790
		internal static object AutoGeneratedGetMemberValue_kingdom(object o)
		{
			return ((Army)o)._kingdom;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0001459D File Offset: 0x0001279D
		internal static object AutoGeneratedGetMemberValue_aiBehaviorObject(object o)
		{
			return ((Army)o)._aiBehaviorObject;
		}

		// Token: 0x060001EA RID: 490 RVA: 0x000145AA File Offset: 0x000127AA
		internal static object AutoGeneratedGetMemberValue_inactivityCounter(object o)
		{
			return ((Army)o)._inactivityCounter;
		}

		// Token: 0x0400000A RID: 10
		private const float CheckingForBoostingCohesionThreshold = 50f;

		// Token: 0x0400000B RID: 11
		private const float DisbandCohesionThreshold = 30f;

		// Token: 0x0400000C RID: 12
		private const float StrengthThresholdRatioForGathering = 0.9f;

		// Token: 0x0400000D RID: 13
		private const float StrengthThresholdRatioForGatheringAfterTimeThreshold = 0.75f;

		// Token: 0x0400000E RID: 14
		[SaveableField(1)]
		private readonly MBList<MobileParty> _parties;

		// Token: 0x04000012 RID: 18
		[SaveableField(19)]
		private CampaignTime _creationTime;

		// Token: 0x04000013 RID: 19
		[SaveableField(7)]
		private float _armyGatheringStartTime;

		// Token: 0x04000014 RID: 20
		[SaveableField(10)]
		private bool _armyIsDispersing;

		// Token: 0x04000015 RID: 21
		[SaveableField(11)]
		private int _numberOfBoosts;

		// Token: 0x04000018 RID: 24
		[SaveableField(15)]
		private Kingdom _kingdom;

		// Token: 0x04000019 RID: 25
		[SaveableField(16)]
		private IMapPoint _aiBehaviorObject;

		// Token: 0x0400001B RID: 27
		[SaveableField(20)]
		private int _inactivityCounter;

		// Token: 0x0400001C RID: 28
		[CachedData]
		private MBCampaignEvent _hourlyTickEvent;

		// Token: 0x0400001D RID: 29
		[CachedData]
		private MBCampaignEvent _tickEvent;

		// Token: 0x0200051D RID: 1309
		public enum ArmyTypes
		{
			// Token: 0x0400166E RID: 5742
			Besieger,
			// Token: 0x0400166F RID: 5743
			Raider,
			// Token: 0x04001670 RID: 5744
			Defender,
			// Token: 0x04001671 RID: 5745
			Patrolling,
			// Token: 0x04001672 RID: 5746
			NumberOfArmyTypes
		}

		// Token: 0x0200051E RID: 1310
		public enum ArmyDispersionReason
		{
			// Token: 0x04001674 RID: 5748
			Unknown,
			// Token: 0x04001675 RID: 5749
			DismissalRequestedWithInfluence,
			// Token: 0x04001676 RID: 5750
			NotEnoughParty,
			// Token: 0x04001677 RID: 5751
			KingdomChanged,
			// Token: 0x04001678 RID: 5752
			CohesionDepleted,
			// Token: 0x04001679 RID: 5753
			ObjectiveFinished,
			// Token: 0x0400167A RID: 5754
			LeaderPartyRemoved,
			// Token: 0x0400167B RID: 5755
			PlayerTakenPrisoner,
			// Token: 0x0400167C RID: 5756
			CannotElectNewLeader,
			// Token: 0x0400167D RID: 5757
			LeaderCannotArrivePointOnTime,
			// Token: 0x0400167E RID: 5758
			ArmyLeaderIsDead,
			// Token: 0x0400167F RID: 5759
			FoodProblem,
			// Token: 0x04001680 RID: 5760
			NotEnoughTroop,
			// Token: 0x04001681 RID: 5761
			NoActiveWar,
			// Token: 0x04001682 RID: 5762
			NoShipToUse,
			// Token: 0x04001683 RID: 5763
			Inactivity
		}
	}
}
