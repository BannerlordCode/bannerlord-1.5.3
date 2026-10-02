using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000130 RID: 304
	public class DefaultMapVisibilityModel : MapVisibilityModel
	{
		// Token: 0x06001924 RID: 6436 RVA: 0x0007A687 File Offset: 0x00078887
		public override float MaximumSeeingRange()
		{
			return 60f;
		}

		// Token: 0x06001925 RID: 6437 RVA: 0x0007A68E File Offset: 0x0007888E
		public override float GetPartySeeingRangeBase(MobileParty party)
		{
			if (!Campaign.Current.IsNight)
			{
				return 12f;
			}
			return 6f;
		}

		// Token: 0x06001926 RID: 6438 RVA: 0x0007A6A8 File Offset: 0x000788A8
		public override ExplainedNumber GetPartySpottingRange(MobileParty party, bool includeDescriptions = false)
		{
			float partySeeingRangeBase = Campaign.Current.Models.MapVisibilityModel.GetPartySeeingRangeBase(party);
			ExplainedNumber explainedNumber = new ExplainedNumber(partySeeingRangeBase, includeDescriptions, null);
			SkillHelper.AddSkillBonusForParty(DefaultSkillEffects.TrackingSpottingDistance, party, ref explainedNumber);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Bow.EagleEye, party, false, ref explainedNumber);
			Hero effectiveScout = party.EffectiveScout;
			if (effectiveScout != null)
			{
				TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(party.CurrentNavigationFace);
				if (faceTerrainType == TerrainType.Forest && PartyBaseHelper.HasFeat(party.Party, DefaultCulturalFeats.BattanianForestSpeedFeat))
				{
					explainedNumber.AddFactor(0.15f, GameTexts.FindText("str_culture", null));
				}
				if (!party.IsCurrentlyAtSea)
				{
					if ((faceTerrainType == TerrainType.Plain || faceTerrainType == TerrainType.Steppe) && effectiveScout.GetPerkValue(DefaultPerks.Scouting.WaterDiviner))
					{
						explainedNumber.AddFactor(DefaultPerks.Scouting.WaterDiviner.PrimaryBonus, DefaultPerks.Scouting.WaterDiviner.Name);
					}
					if (Campaign.Current.IsNight)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.NightRunner, party, false, ref explainedNumber);
					}
					else
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.DayTraveler, party, false, ref explainedNumber);
					}
				}
				if (!party.IsMoving && party.StationaryStartTime.ElapsedHoursUntilNow >= 1f)
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.VantagePoint, party, true, ref explainedNumber);
				}
				if (effectiveScout.GetPerkValue(DefaultPerks.Scouting.MountedScouts) && !party.IsCurrentlyAtSea)
				{
					float num = 0f;
					for (int i = 0; i < party.MemberRoster.Count; i++)
					{
						if (party.MemberRoster.GetCharacterAtIndex(i).DefaultFormationClass.Equals(FormationClass.Cavalry))
						{
							num += (float)party.MemberRoster.GetElementNumber(i);
						}
					}
					if (num / (float)party.MemberRoster.TotalManCount >= 0.5f)
					{
						explainedNumber.AddFactor(DefaultPerks.Scouting.MountedScouts.PrimaryBonus, DefaultPerks.Scouting.MountedScouts.Name);
					}
				}
			}
			explainedNumber.LimitMax(Campaign.Current.Models.MapVisibilityModel.MaximumSeeingRange(), new TextObject("{=6qv6Hdww}Limit", null));
			return explainedNumber;
		}

		// Token: 0x06001927 RID: 6439 RVA: 0x0007A8A4 File Offset: 0x00078AA4
		public override float GetPartySpottingRatioForMainPartySeeingRange(MobileParty party)
		{
			float num = 1f;
			if (Campaign.Current.MapSceneWrapper.GetFaceTerrainType(party.CurrentNavigationFace) == TerrainType.Forest)
			{
				float num2 = -0.3f;
				Hero hero = null;
				if (MobileParty.MainParty.HasPerk(DefaultPerks.Scouting.KeenSight, out hero, false))
				{
					num2 += num2 * DefaultPerks.Scouting.KeenSight.PrimaryBonus;
				}
				num += num2;
			}
			int num3 = ((party.Army != null && party.Army.LeaderParty == party) ? party.Army.TotalManCount : party.MemberRoster.TotalManCount);
			return MBMath.ClampFloat(1.1f - 0.5f * MathF.Pow(2.7182817f, (float)(-(float)num3) / 200f), 0f, 1f) * num;
		}

		// Token: 0x06001928 RID: 6440 RVA: 0x0007A960 File Offset: 0x00078B60
		public override float GetHideoutSpottingDistance()
		{
			float num = MobileParty.MainParty.SeeingRange * 1.2f;
			Hero hero = null;
			if (MobileParty.MainParty.HasPerk(DefaultPerks.Scouting.RumourNetwork, out hero, true))
			{
				return num * (1f + DefaultPerks.Scouting.RumourNetwork.SecondaryBonus);
			}
			return num;
		}

		// Token: 0x06001929 RID: 6441 RVA: 0x0007A9AC File Offset: 0x00078BAC
		public override void GetMobilePartyVisibilityAndInspectedState(MobileParty mobileParty, Vec2[] points, float seeingRange, out bool isVisible, out bool isInspected, out bool isDistanceDependent)
		{
			isVisible = false;
			isInspected = false;
			isDistanceDependent = false;
			if (this.ShouldMobilePartyBeVisible(mobileParty))
			{
				isVisible = true;
				isInspected = true;
				isDistanceDependent = false;
				return;
			}
			if (!mobileParty.IsActive || mobileParty.IsGarrison || mobileParty.IsMilitia || mobileParty.CurrentSettlement != null)
			{
				isVisible = mobileParty.IsVisible;
				isInspected = mobileParty.IsInspected;
				isDistanceDependent = false;
				return;
			}
			Vec2 vec;
			if (this.TryGetBestPoint(points, mobileParty.Position.ToVec2(), seeingRange, out vec))
			{
				this.CalculateMobilePartyVisibilityAndInspected(vec, mobileParty, out isVisible, out isInspected, seeingRange);
				isDistanceDependent = true;
			}
		}

		// Token: 0x0600192A RID: 6442 RVA: 0x0007AA40 File Offset: 0x00078C40
		public override void GetSettlementInspectedState(Settlement settlement, Vec2[] points, float seeingRange, out bool isInspected, out bool isDistanceDependent)
		{
			isInspected = false;
			isDistanceDependent = false;
			Vec2 vec;
			if (this.TryGetBestPoint(points, settlement.GatePosition.ToVec2(), seeingRange, out vec) || (settlement.HasPort && this.TryGetBestPoint(points, settlement.PortPosition.ToVec2(), seeingRange, out vec)))
			{
				isInspected = this.CalculateSettlementInspected(vec, settlement, seeingRange);
				isDistanceDependent = true;
			}
		}

		// Token: 0x0600192B RID: 6443 RVA: 0x0007AAA4 File Offset: 0x00078CA4
		private bool ShouldMobilePartyBeVisible(MobileParty mobileParty)
		{
			return Campaign.Current.TrueSight || (PlayerCaptivity.CaptorParty != null && PlayerCaptivity.CaptorParty == mobileParty.Party) || (mobileParty.SiegeEvent != null && mobileParty.SiegeEvent.BesiegedSettlement.IsInspected) || (mobileParty.MapEvent != null && mobileParty.MapEvent.IsRaid && mobileParty.MapEvent.MapEventSettlement.IsInspected);
		}

		// Token: 0x0600192C RID: 6444 RVA: 0x0007AB1B File Offset: 0x00078D1B
		private bool CalculateSettlementInspected(Vec2 fromPosition, IMapPoint mapPoint, float mainPartySeeingRange)
		{
			return this.CalculateVisibilityRangeOfMapPoint(fromPosition, mapPoint, mainPartySeeingRange) <= 1.5f;
		}

		// Token: 0x0600192D RID: 6445 RVA: 0x0007AB30 File Offset: 0x00078D30
		private float CalculateVisibilityRangeOfMapPoint(Vec2 fromPosition, IMapPoint mapPoint, float mainPartySeeingRange)
		{
			return (fromPosition - mapPoint.Position.ToVec2()).Length / mainPartySeeingRange;
		}

		// Token: 0x0600192E RID: 6446 RVA: 0x0007AB60 File Offset: 0x00078D60
		private bool TryGetBestPoint(Vec2[] points, Vec2 targetPosition, float seeingRange, out Vec2 point)
		{
			point = default(Vec2);
			Vec2 vec = Vec2.Invalid;
			float num = float.MaxValue;
			for (int i = 0; i < points.Length; i++)
			{
				float num2 = targetPosition.DistanceSquared(points[i]);
				if (num2 < num)
				{
					num = num2;
					vec = points[i];
				}
			}
			if (num < seeingRange * seeingRange)
			{
				point = vec;
				return true;
			}
			return false;
		}

		// Token: 0x0600192F RID: 6447 RVA: 0x0007ABC0 File Offset: 0x00078DC0
		private void CalculateMobilePartyVisibilityAndInspected(Vec2 fromPosition, MobileParty mobileParty, out bool isVisible, out bool isInspected, float mainPartySeeingRange)
		{
			isInspected = false;
			isVisible = false;
			if (mobileParty.Army != null && mobileParty.Army.LeaderParty.AttachedParties.IndexOf(mobileParty) >= 0)
			{
				isVisible = mobileParty.Army.LeaderParty.IsVisible;
				return;
			}
			float num = this.CalculateVisibilityRangeOfMapPoint(fromPosition, mobileParty, mainPartySeeingRange);
			if (num < 1f)
			{
				float partySpottingRatioForMainPartySeeingRange = Campaign.Current.Models.MapVisibilityModel.GetPartySpottingRatioForMainPartySeeingRange(mobileParty);
				isVisible = mobileParty.IsActive && num <= partySpottingRatioForMainPartySeeingRange;
				if (isVisible)
				{
					isInspected = true;
				}
			}
		}

		// Token: 0x04000830 RID: 2096
		private const float PartySpottingDifficultyInForests = 0.3f;
	}
}
