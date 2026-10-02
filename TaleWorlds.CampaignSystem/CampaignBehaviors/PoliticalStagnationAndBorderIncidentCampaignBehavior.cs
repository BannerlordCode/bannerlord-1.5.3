using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000456 RID: 1110
	public class PoliticalStagnationAndBorderIncidentCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060047A3 RID: 18339 RVA: 0x0015FE24 File Offset: 0x0015E024
		public override void RegisterEvents()
		{
			CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, new Action(this.DailyTick));
			CampaignEvents.HourlyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(this.HourlyTickSettlement));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.NewGameCreated));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.LoadFinished));
		}

		// Token: 0x060047A4 RID: 18340 RVA: 0x0015FE8D File Offset: 0x0015E08D
		private void LoadFinished()
		{
			if (this._lastUpdateTimePerSettlement == null)
			{
				this.InitializeDictionary();
			}
		}

		// Token: 0x060047A5 RID: 18341 RVA: 0x0015FE9D File Offset: 0x0015E09D
		private void NewGameCreated(CampaignGameStarter obj)
		{
			this.InitializeDictionary();
		}

		// Token: 0x060047A6 RID: 18342 RVA: 0x0015FEA8 File Offset: 0x0015E0A8
		private void InitializeDictionary()
		{
			this._lastUpdateTimePerSettlement = new Dictionary<Settlement, CampaignTime>();
			foreach (Settlement settlement in Settlement.All)
			{
				if (settlement.IsFortification || settlement.IsVillage)
				{
					this._lastUpdateTimePerSettlement.Add(settlement, CampaignTime.Now - CampaignTime.Hours(MBRandom.RandomFloat * 3f));
				}
			}
		}

		// Token: 0x060047A7 RID: 18343 RVA: 0x0015FF34 File Offset: 0x0015E134
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<Dictionary<Settlement, CampaignTime>>("_lastUpdateTimePerSettlement", ref this._lastUpdateTimePerSettlement);
		}

		// Token: 0x060047A8 RID: 18344 RVA: 0x0015FF48 File Offset: 0x0015E148
		public void HourlyTickSettlement(Settlement settlement)
		{
			if (this._lastUpdateTimePerSettlement.ContainsKey(settlement) && this._lastUpdateTimePerSettlement[settlement].ElapsedHoursUntilNow > 3f)
			{
				this.UpdateNearbyValues(settlement, false);
				settlement.NearbyLandThreatIntensity *= 0.85f;
				settlement.NearbyLandAllyIntensity *= 0.8f;
				if (settlement.HasPort)
				{
					this.UpdateNearbyValues(settlement, true);
					settlement.NearbyNavalThreatIntensity *= 0.85f;
					settlement.NearbyNavalAllyIntensity *= 0.8f;
				}
				this._lastUpdateTimePerSettlement[settlement] = CampaignTime.Now;
			}
		}

		// Token: 0x060047A9 RID: 18345 RVA: 0x0015FFF4 File Offset: 0x0015E1F4
		private void UpdateNearbyValues(Settlement settlement, bool isCheckingNavalValues)
		{
			float settlementNearbyThreatAndAllyCheckRadius = Campaign.Current.Models.MobilePartyAIModel.GetSettlementNearbyThreatAndAllyCheckRadius(settlement, isCheckingNavalValues);
			LocatableSearchData<MobileParty> locatableSearchData = MobileParty.StartFindingLocatablesAroundPosition((isCheckingNavalValues ? settlement.PortPosition : settlement.GatePosition).ToVec2(), settlementNearbyThreatAndAllyCheckRadius);
			for (MobileParty mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData); mobileParty != null; mobileParty = MobileParty.FindNextLocatable(ref locatableSearchData))
			{
				if (!mobileParty.IsGarrison && !mobileParty.IsMilitia && mobileParty.IsActive)
				{
					if (mobileParty.Ai.IsAlerted && mobileParty.MapFaction == settlement.MapFaction && (mobileParty.IsCaravan || mobileParty.IsVillager))
					{
						if (mobileParty.IsCurrentlyAtSea && isCheckingNavalValues)
						{
							settlement.NearbyNavalThreatIntensity += 0.6f;
						}
						else if (!isCheckingNavalValues)
						{
							settlement.NearbyLandThreatIntensity += 0.6f;
						}
					}
					if (mobileParty.Aggressiveness > 0f)
					{
						if (mobileParty.CurrentSettlement == null && mobileParty.Army == null && (mobileParty.IsBandit || FactionManager.IsAtWarAgainstFaction(mobileParty.MapFaction, settlement.MapFaction)))
						{
							float threatValueOfEnemyToSettlement = this.GetThreatValueOfEnemyToSettlement(mobileParty, settlement);
							if (mobileParty.IsCurrentlyAtSea && isCheckingNavalValues)
							{
								settlement.NearbyNavalThreatIntensity += threatValueOfEnemyToSettlement * 3f;
							}
							else if (!isCheckingNavalValues)
							{
								settlement.NearbyLandThreatIntensity += threatValueOfEnemyToSettlement * 3f;
							}
						}
						else if (mobileParty.MapFaction == settlement.MapFaction)
						{
							bool flag = mobileParty.DefaultBehavior == AiBehavior.PatrolAroundPoint && !mobileParty.TargetPosition.IsOnLand;
							float num = this.GetThreatValueOfParty(mobileParty);
							if (flag)
							{
								num *= 0.5f;
							}
							if ((mobileParty.IsCurrentlyAtSea || flag) && isCheckingNavalValues)
							{
								settlement.NearbyNavalAllyIntensity += num * 3f;
							}
							else if (!isCheckingNavalValues)
							{
								settlement.NearbyLandAllyIntensity += num * 3f;
							}
						}
					}
				}
			}
		}

		// Token: 0x060047AA RID: 18346 RVA: 0x001601D6 File Offset: 0x0015E3D6
		private float GetThreatValueOfParty(MobileParty mobileParty)
		{
			return MathF.Min(1f, mobileParty.Party.EstimatedStrength / 400f * MathF.Min(1f, mobileParty.Aggressiveness));
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x00160204 File Offset: 0x0015E404
		private float GetThreatValueOfEnemyToSettlement(MobileParty mobileParty, Settlement settlement)
		{
			float num = this.GetThreatValueOfParty(mobileParty);
			if (mobileParty == MobileParty.MainParty)
			{
				num *= 2f;
			}
			if (!mobileParty.IsLordParty)
			{
				num *= 0.5f;
			}
			if (mobileParty.DefaultBehavior == AiBehavior.PatrolAroundPoint && mobileParty.TargetSettlement == settlement)
			{
				num *= 2f;
			}
			if (mobileParty.MapEvent != null && mobileParty.MapEvent.IsFieldBattle)
			{
				num = 3f * num;
			}
			return num;
		}

		// Token: 0x060047AC RID: 18348 RVA: 0x00160274 File Offset: 0x0015E474
		public void DailyTick()
		{
			foreach (Kingdom kingdom in Kingdom.All)
			{
				PoliticalStagnationAndBorderIncidentCampaignBehavior.UpdatePoliticallyStagnation(kingdom);
			}
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x001602C4 File Offset: 0x0015E4C4
		private static void UpdatePoliticallyStagnation(Kingdom kingdom)
		{
			float num = 1f + (float)MathF.Min(60, kingdom.Fiefs.Count) * 0.2f;
			float num2 = 2f + (float)MathF.Min(60, kingdom.Fiefs.Count) * 0.6f;
			int num3 = 1;
			foreach (Kingdom kingdom2 in Kingdom.All)
			{
				if (FactionManager.IsAtWarAgainstFaction(kingdom, kingdom2))
				{
					if ((float)kingdom2.Fiefs.Count >= num2)
					{
						num3 = -2;
						break;
					}
					if ((float)kingdom2.Fiefs.Count >= num)
					{
						num3 = -1;
					}
				}
			}
			kingdom.PoliticalStagnation += num3;
			if (kingdom.PoliticalStagnation < 0)
			{
				kingdom.PoliticalStagnation = 0;
				return;
			}
			if (kingdom.PoliticalStagnation > 300)
			{
				kingdom.PoliticalStagnation = 300;
			}
		}

		// Token: 0x04001453 RID: 5203
		private const float ThreatUpdateTimeThresholdInHours = 3f;

		// Token: 0x04001454 RID: 5204
		private Dictionary<Settlement, CampaignTime> _lastUpdateTimePerSettlement;
	}
}
