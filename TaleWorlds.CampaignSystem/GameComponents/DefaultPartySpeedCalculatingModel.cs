using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000140 RID: 320
	public class DefaultPartySpeedCalculatingModel : PartySpeedModel
	{
		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x060019F6 RID: 6646 RVA: 0x00081C96 File Offset: 0x0007FE96
		public override float BaseSpeed
		{
			get
			{
				return 4.6f;
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x060019F7 RID: 6647 RVA: 0x00081C9D File Offset: 0x0007FE9D
		public override float MinimumSpeed
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x060019F8 RID: 6648 RVA: 0x00081CA4 File Offset: 0x0007FEA4
		private ExplainedNumber CalculateLandBaseSpeed(MobileParty mobileParty, bool includeDescriptions = false, int additionalTroopOnFootCount = 0, int additionalTroopOnHorseCount = 0)
		{
			PartyBase party = mobileParty.Party;
			int num = 0;
			float num2 = 0f;
			int num3 = 0;
			int num4 = mobileParty.MemberRoster.TotalManCount + additionalTroopOnFootCount + additionalTroopOnHorseCount;
			this.AddCargoStats(mobileParty, ref num, ref num2, ref num3);
			float num5 = mobileParty.TotalWeightCarried;
			int num6 = (int)Campaign.Current.Models.InventoryCapacityModel.CalculateInventoryCapacity(mobileParty, mobileParty.IsCurrentlyAtSea, false, additionalTroopOnFootCount, additionalTroopOnHorseCount, 0, false).ResultNumber;
			int num7 = party.NumberOfMenWithHorse + additionalTroopOnHorseCount;
			int num8 = party.NumberOfMenWithoutHorse + additionalTroopOnFootCount;
			int num9 = party.MemberRoster.TotalWounded;
			int num10 = party.PrisonRoster.TotalManCount;
			int num11 = party.PartySizeLimit;
			float morale = mobileParty.Morale;
			if (mobileParty.AttachedParties.Count != 0)
			{
				foreach (MobileParty mobileParty2 in mobileParty.AttachedParties)
				{
					this.AddCargoStats(mobileParty2, ref num, ref num2, ref num3);
					num4 += mobileParty2.MemberRoster.TotalManCount;
					num5 += mobileParty2.TotalWeightCarried;
					num6 += mobileParty2.InventoryCapacity;
					num7 += mobileParty2.Party.NumberOfMenWithHorse;
					num8 += mobileParty2.Party.NumberOfMenWithoutHorse;
					num9 += mobileParty2.MemberRoster.TotalWounded;
					num10 += mobileParty2.PrisonRoster.TotalManCount;
					num11 += mobileParty2.Party.PartySizeLimit;
				}
			}
			float num12 = this.CalculateBaseSpeedForParty(num4);
			ExplainedNumber explainedNumber = new ExplainedNumber(num12, includeDescriptions, null);
			bool flag = Campaign.Current.Models.MapWeatherModel.GetWeatherEffectOnTerrainForPosition(mobileParty.Position.ToVec2()) == MapWeatherModel.WeatherEventEffectOnTerrain.Wet;
			this.GetFootmenPerkBonus(mobileParty, num4, num8, ref explainedNumber);
			float cavalryRatioModifier = this.GetCavalryRatioModifier(num4, num7);
			int num13 = MathF.Min(num8, num);
			float mountedFootmenRatioModifier = this.GetMountedFootmenRatioModifier(num4, num13);
			explainedNumber.AddFactor(cavalryRatioModifier, DefaultPartySpeedCalculatingModel._textCavalry);
			explainedNumber.AddFactor(mountedFootmenRatioModifier, DefaultPartySpeedCalculatingModel._textMountedFootmen);
			if (flag)
			{
				float num14 = cavalryRatioModifier * 0.3f;
				float num15 = mountedFootmenRatioModifier * 0.3f;
				explainedNumber.AddFactor(-num14, DefaultPartySpeedCalculatingModel._textCavalryWeatherPenalty);
				explainedNumber.AddFactor(-num15, DefaultPartySpeedCalculatingModel._textMountedFootmenWeatherPenalty);
			}
			if (mountedFootmenRatioModifier > 0f && mobileParty.LeaderHero != null && mobileParty.LeaderHero.GetPerkValue(DefaultPerks.Riding.NomadicTraditions))
			{
				float num16 = mountedFootmenRatioModifier * DefaultPerks.Riding.NomadicTraditions.PrimaryBonus;
				explainedNumber.AddFactor(num16, DefaultPerks.Riding.NomadicTraditions.Name);
			}
			float num17 = MathF.Min(num5, (float)num6);
			if (num17 > 0f)
			{
				float cargoEffect = this.GetCargoEffect(num17, num6);
				explainedNumber.AddFactor(cargoEffect, DefaultPartySpeedCalculatingModel._textCargo);
			}
			if (num2 > (float)num6)
			{
				ExplainedNumber overburdenedEffect = this.GetOverburdenedEffect(mobileParty, num2 - (float)num6, num6, includeDescriptions);
				explainedNumber.AddFromExplainedNumber(overburdenedEffect, DefaultPartySpeedCalculatingModel._textOverburdened);
			}
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Riding.SweepingWind, mobileParty, false, ref explainedNumber);
			if (num4 > num11)
			{
				float overPartySizeEffect = this.GetOverPartySizeEffect(num4, num11);
				Clan actualClan = mobileParty.ActualClan;
				if (((actualClan != null) ? actualClan.StringId : null) == "deserters")
				{
					explainedNumber.AddFactor(overPartySizeEffect * 0.5f, DefaultPartySpeedCalculatingModel._textOverPartySize);
				}
				else
				{
					explainedNumber.AddFactor(overPartySizeEffect, DefaultPartySpeedCalculatingModel._textOverPartySize);
				}
			}
			num3 += MathF.Max(0, num - num13);
			if (!mobileParty.IsVillager)
			{
				float herdingModifier = this.GetHerdingModifier(num4, num3);
				explainedNumber.AddFactor(herdingModifier, DefaultPartySpeedCalculatingModel._textHerd);
				Hero hero = null;
				if (mobileParty.HasPerk(DefaultPerks.Riding.Shepherd, out hero, false))
				{
					float num18 = herdingModifier * DefaultPerks.Riding.Shepherd.PrimaryBonus;
					explainedNumber.AddFactor(num18, DefaultPerks.Riding.Shepherd.Name);
				}
			}
			float woundedModifier = this.GetWoundedModifier(num4, num9, mobileParty);
			explainedNumber.AddFactor(woundedModifier, DefaultPartySpeedCalculatingModel._textWounded);
			if (!mobileParty.IsCaravan)
			{
				if (mobileParty.Party.NumberOfPrisoners > mobileParty.Party.PrisonerSizeLimit)
				{
					float overPrisonerSizeEffect = this.GetOverPrisonerSizeEffect(mobileParty);
					explainedNumber.AddFactor(overPrisonerSizeEffect, DefaultPartySpeedCalculatingModel._textOverPrisonerSize);
				}
				float sizeModifierPrisoner = this.GetSizeModifierPrisoner(num4, num10);
				explainedNumber.AddFactor(1f / sizeModifierPrisoner - 1f, DefaultPartySpeedCalculatingModel._textPrisoners);
			}
			if (morale > 70f)
			{
				explainedNumber.AddFactor(0.05f * ((morale - 70f) / 30f), DefaultPartySpeedCalculatingModel._textHighMorale);
			}
			if (morale < 30f)
			{
				explainedNumber.AddFactor(-0.1f * (1f - mobileParty.Morale / 30f), DefaultPartySpeedCalculatingModel._textLowMorale);
			}
			if (mobileParty == MobileParty.MainParty)
			{
				float playerMapMovementSpeedBonusMultiplier = Campaign.Current.Models.DifficultyModel.GetPlayerMapMovementSpeedBonusMultiplier();
				if (playerMapMovementSpeedBonusMultiplier > 0f)
				{
					explainedNumber.AddFactor(playerMapMovementSpeedBonusMultiplier, GameTexts.FindText("str_game_difficulty", null));
				}
			}
			if (mobileParty.IsCaravan)
			{
				explainedNumber.AddFactor(0.1f, DefaultPartySpeedCalculatingModel._textCaravan);
			}
			if (mobileParty.IsDisorganized)
			{
				explainedNumber.AddFactor(-0.4f, DefaultPartySpeedCalculatingModel._textDisorganized);
			}
			explainedNumber.LimitMin(this.MinimumSpeed);
			return explainedNumber;
		}

		// Token: 0x060019F9 RID: 6649 RVA: 0x00082190 File Offset: 0x00080390
		public override ExplainedNumber CalculateBaseSpeed(MobileParty mobileParty, bool includeDescriptions = false, int additionalTroopOnFootCount = 0, int additionalTroopOnHorseCount = 0)
		{
			return this.CalculateLandBaseSpeed(mobileParty, includeDescriptions, additionalTroopOnFootCount, additionalTroopOnHorseCount);
		}

		// Token: 0x060019FA RID: 6650 RVA: 0x0008219D File Offset: 0x0008039D
		public override int GetSkeletalCrewCount(MobileParty party)
		{
			return 0;
		}

		// Token: 0x060019FB RID: 6651 RVA: 0x000821A0 File Offset: 0x000803A0
		private void AddCargoStats(MobileParty mobileParty, ref int numberOfAvailableMounts, ref float totalWeightCarried, ref int herdSize)
		{
			ItemRoster itemRoster = mobileParty.ItemRoster;
			int numberOfPackAnimals = itemRoster.NumberOfPackAnimals;
			int numberOfLivestockAnimals = itemRoster.NumberOfLivestockAnimals;
			herdSize += numberOfPackAnimals + numberOfLivestockAnimals;
			numberOfAvailableMounts += itemRoster.NumberOfMounts;
			totalWeightCarried += mobileParty.TotalWeightCarried;
		}

		// Token: 0x060019FC RID: 6652 RVA: 0x000821E2 File Offset: 0x000803E2
		private float CalculateBaseSpeedForParty(int menCount)
		{
			return this.BaseSpeed * MathF.Pow(200f / (200f + (float)menCount), 0.4f);
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x00082204 File Offset: 0x00080404
		private ExplainedNumber GetOverburdenedEffect(MobileParty party, float totalWeightCarried, int partyCapacity, bool includeDescriptions)
		{
			ExplainedNumber explainedNumber = new ExplainedNumber(-0.4f * (totalWeightCarried / (float)partyCapacity), includeDescriptions, null);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Athletics.Energetic, party, true, ref explainedNumber);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Unburdened, party, true, ref explainedNumber);
			return explainedNumber;
		}

		// Token: 0x060019FE RID: 6654 RVA: 0x00082244 File Offset: 0x00080444
		public override ExplainedNumber CalculateFinalSpeed(MobileParty mobileParty, ExplainedNumber finalSpeed)
		{
			if (mobileParty.IsCustomParty && !((CustomPartyComponent)mobileParty.PartyComponent).BaseSpeed.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				finalSpeed = new ExplainedNumber(((CustomPartyComponent)mobileParty.PartyComponent).BaseSpeed, false, null);
			}
			TerrainType faceTerrainType = Campaign.Current.MapSceneWrapper.GetFaceTerrainType(mobileParty.CurrentNavigationFace);
			Hero effectiveScout = mobileParty.EffectiveScout;
			if (faceTerrainType == TerrainType.Forest)
			{
				float num = 0f;
				bool flag = effectiveScout != null && effectiveScout.GetPerkValue(DefaultPerks.Scouting.ForestKin);
				if (flag)
				{
					for (int i = 0; i < mobileParty.MemberRoster.Count; i++)
					{
						if (!mobileParty.MemberRoster.GetCharacterAtIndex(i).IsMounted)
						{
							num += (float)mobileParty.MemberRoster.GetElementNumber(i);
						}
					}
				}
				float num2 = ((flag && num / (float)mobileParty.MemberRoster.TotalManCount >= 0.75f) ? (-0.2f * -DefaultPerks.Scouting.ForestKin.PrimaryBonus) : (-0.2f));
				finalSpeed.AddFactor(num2, DefaultPartySpeedCalculatingModel._movingInForest);
				if (PartyBaseHelper.HasFeat(mobileParty.Party, DefaultCulturalFeats.BattanianForestSpeedFeat))
				{
					float num3 = DefaultCulturalFeats.BattanianForestSpeedFeat.EffectBonus * 0.2f;
					finalSpeed.AddFactor(num3, this._culture);
				}
			}
			else if (!mobileParty.IsCurrentlyAtSea && (faceTerrainType == TerrainType.Water || faceTerrainType == TerrainType.River || faceTerrainType == TerrainType.UnderBridge || faceTerrainType == TerrainType.Bridge || faceTerrainType == TerrainType.Fording))
			{
				finalSpeed.AddFactor(-0.3f, DefaultPartySpeedCalculatingModel._fordEffect);
			}
			else if (faceTerrainType == TerrainType.Desert || faceTerrainType == TerrainType.Dune)
			{
				if (!PartyBaseHelper.HasFeat(mobileParty.Party, DefaultCulturalFeats.AseraiDesertFeat))
				{
					finalSpeed.AddFactor(-0.1f, DefaultPartySpeedCalculatingModel._desert);
				}
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.DesertBorn, mobileParty, true, ref finalSpeed);
			}
			else if (faceTerrainType == TerrainType.Plain || faceTerrainType == TerrainType.Steppe)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Pathfinder, mobileParty, true, ref finalSpeed);
			}
			MapWeatherModel.WeatherEvent weatherEventInPosition = Campaign.Current.Models.MapWeatherModel.GetWeatherEventInPosition(mobileParty.Position.ToVec2());
			if (weatherEventInPosition == MapWeatherModel.WeatherEvent.Snowy || weatherEventInPosition == MapWeatherModel.WeatherEvent.Blizzard)
			{
				finalSpeed.AddFactor(-0.125f, DefaultPartySpeedCalculatingModel._snow);
			}
			if (!mobileParty.IsCurrentlyAtSea)
			{
				if (Campaign.Current.IsNight)
				{
					finalSpeed.AddFactor(-0.25f, DefaultPartySpeedCalculatingModel._night);
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.NightRunner, mobileParty, true, ref finalSpeed);
				}
				else
				{
					PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.DayTraveler, mobileParty, true, ref finalSpeed);
				}
			}
			if (effectiveScout != null)
			{
				if (!mobileParty.IsCurrentlyAtSea)
				{
					PerkHelper.AddEpicPerkBonusForCharacter(DefaultPerks.Scouting.UncannyInsight, mobileParty.CurrentBattleEnvironment, effectiveScout.CharacterObject, DefaultSkills.Scouting, true, ref finalSpeed, Campaign.Current.Models.CharacterDevelopmentModel.MinSkillRequiredForEpicPerkBonus);
					if (mobileParty.Morale > 75f)
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.ForcedMarch, mobileParty, true, ref finalSpeed);
					}
				}
				if (mobileParty.DefaultBehavior == AiBehavior.EngageParty)
				{
					MobileParty targetParty = mobileParty.TargetParty;
					if (targetParty != null && !targetParty.IsCurrentlyAtSea && targetParty.MapFaction.IsAtWarWith(mobileParty.MapFaction))
					{
						PerkHelper.AddPerkBonusForParty(DefaultPerks.Scouting.Tracker, mobileParty, false, ref finalSpeed);
					}
				}
			}
			Army army = mobileParty.Army;
			if (((army != null) ? army.LeaderParty : null) != null && mobileParty.Army.LeaderParty != mobileParty && mobileParty.AttachedTo != mobileParty.Army.LeaderParty)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.CallToArms, mobileParty.Army.LeaderParty, true, ref finalSpeed);
			}
			if (Campaign.Current.Options.IsIncreasedGlobalMovementSpeedEnabled)
			{
				finalSpeed.AddFactor(0.25f, null);
			}
			finalSpeed.LimitMin(this.MinimumSpeed);
			return finalSpeed;
		}

		// Token: 0x060019FF RID: 6655 RVA: 0x000825BA File Offset: 0x000807BA
		private float GetCargoEffect(float weightCarried, int partyCapacity)
		{
			return -0.02f * weightCarried / (float)partyCapacity;
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x000825C7 File Offset: 0x000807C7
		private float GetOverPartySizeEffect(int totalMenCount, int partySize)
		{
			return 1f / ((float)totalMenCount / (float)partySize) - 1f;
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x000825DC File Offset: 0x000807DC
		private float GetOverPrisonerSizeEffect(MobileParty mobileParty)
		{
			int prisonerSizeLimit = mobileParty.Party.PrisonerSizeLimit;
			int numberOfPrisoners = mobileParty.Party.NumberOfPrisoners;
			return 1f / ((float)numberOfPrisoners / (float)prisonerSizeLimit) - 1f;
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x00082612 File Offset: 0x00080812
		private float GetHerdingModifier(int totalMenCount, int herdSize)
		{
			herdSize -= totalMenCount;
			if (herdSize <= 0)
			{
				return 0f;
			}
			if (totalMenCount == 0)
			{
				return -0.8f;
			}
			return MathF.Max(-0.8f, -0.3f * ((float)herdSize / (float)totalMenCount));
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x00082644 File Offset: 0x00080844
		private float GetWoundedModifier(int totalMenCount, int numWounded, MobileParty party)
		{
			if (numWounded <= totalMenCount / 4)
			{
				return 0f;
			}
			if (totalMenCount == 0)
			{
				return -0.5f;
			}
			float num = MathF.Max(-0.8f, -0.05f * (float)numWounded / (float)totalMenCount);
			ExplainedNumber explainedNumber = new ExplainedNumber(num, false, null);
			if (!party.IsCurrentlyAtSea)
			{
				PerkHelper.AddPerkBonusForParty(DefaultPerks.Medicine.Sledges, party, true, ref explainedNumber);
			}
			return explainedNumber.ResultNumber;
		}

		// Token: 0x06001A04 RID: 6660 RVA: 0x000826A4 File Offset: 0x000808A4
		private float GetCavalryRatioModifier(int totalMenCount, int totalCavalryCount)
		{
			if (totalMenCount == 0 || totalCavalryCount == 0)
			{
				return 0f;
			}
			return 0.3f * (float)totalCavalryCount / (float)totalMenCount;
		}

		// Token: 0x06001A05 RID: 6661 RVA: 0x000826BD File Offset: 0x000808BD
		private float GetMountedFootmenRatioModifier(int totalMenCount, int totalMountedFootmenCount)
		{
			if (totalMenCount == 0 || totalMountedFootmenCount == 0)
			{
				return 0f;
			}
			return 0.15f * (float)totalMountedFootmenCount / (float)totalMenCount;
		}

		// Token: 0x06001A06 RID: 6662 RVA: 0x000826D8 File Offset: 0x000808D8
		private void GetFootmenPerkBonus(MobileParty party, int totalMenCount, int totalFootmenCount, ref ExplainedNumber result)
		{
			if (totalMenCount == 0)
			{
				return;
			}
			float num = (float)totalFootmenCount / (float)totalMenCount;
			Hero hero = null;
			if (!num.ApproximatelyEqualsTo(0f, 1E-05f) && party.HasPerk(DefaultPerks.Athletics.Strong, out hero, true))
			{
				float num2 = num * DefaultPerks.Athletics.Strong.SecondaryBonus;
				result.AddFactor(num2, DefaultPerks.Athletics.Strong.Name);
			}
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x00082732 File Offset: 0x00080932
		private float GetSizeModifierWounded(int totalMenCount, int totalWoundedMenCount)
		{
			return MathF.Pow((10f + (float)totalMenCount) / (10f + (float)totalMenCount - (float)totalWoundedMenCount), 0.33f);
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x00082752 File Offset: 0x00080952
		private float GetSizeModifierPrisoner(int totalMenCount, int totalPrisonerCount)
		{
			return MathF.Pow((10f + (float)totalMenCount + (float)totalPrisonerCount) / (10f + (float)totalMenCount), 0.33f);
		}

		// Token: 0x04000885 RID: 2181
		private static readonly TextObject _textCargo = new TextObject("{=fSGY71wd}Cargo within capacity", null);

		// Token: 0x04000886 RID: 2182
		private static readonly TextObject _textOverburdened = new TextObject("{=xgO3cCgR}Overburdened", null);

		// Token: 0x04000887 RID: 2183
		private static readonly TextObject _textOverPartySize = new TextObject("{=bO5gL3FI}Men within party size", null);

		// Token: 0x04000888 RID: 2184
		private static readonly TextObject _textOverPrisonerSize = new TextObject("{=Ix8YjLPD}Men within prisoner size", null);

		// Token: 0x04000889 RID: 2185
		private static readonly TextObject _textCavalry = new TextObject("{=YVGtcLHF}Cavalry", null);

		// Token: 0x0400088A RID: 2186
		private static readonly TextObject _textCavalryWeatherPenalty = new TextObject("{=Cb0k9KM8}Cavalry weather penalty", null);

		// Token: 0x0400088B RID: 2187
		private static readonly TextObject _textKhuzaitCavalryBonus = new TextObject("{=yi07dBks}Khuzait cavalry bonus", null);

		// Token: 0x0400088C RID: 2188
		private static readonly TextObject _textMountedFootmen = new TextObject("{=5bSWSaPl}Footmen on horses", null);

		// Token: 0x0400088D RID: 2189
		private static readonly TextObject _textMountedFootmenWeatherPenalty = new TextObject("{=JAKoFNgt}Footmen on horses weather penalty", null);

		// Token: 0x0400088E RID: 2190
		private static readonly TextObject _textWounded = new TextObject("{=aLsVKIRy}Wounded members", null);

		// Token: 0x0400088F RID: 2191
		private static readonly TextObject _textPrisoners = new TextObject("{=N6QTvjMf}Prisoners", null);

		// Token: 0x04000890 RID: 2192
		private static readonly TextObject _textHerd = new TextObject("{=NhAMSaWU}Herding", null);

		// Token: 0x04000891 RID: 2193
		private static readonly TextObject _textHighMorale = new TextObject("{=aDQcIGfH}High morale", null);

		// Token: 0x04000892 RID: 2194
		private static readonly TextObject _textLowMorale = new TextObject("{=ydspCDIy}Low morale", null);

		// Token: 0x04000893 RID: 2195
		private static readonly TextObject _textCaravan = new TextObject("{=vvabqi2w}Caravan", null);

		// Token: 0x04000894 RID: 2196
		private static readonly TextObject _textDisorganized = new TextObject("{=JuwBb2Yg}Disorganized", null);

		// Token: 0x04000895 RID: 2197
		private static readonly TextObject _movingInForest = new TextObject("{=rTFaZCdY}Forest", null);

		// Token: 0x04000896 RID: 2198
		private static readonly TextObject _fordEffect = new TextObject("{=NT5fwUuJ}Fording", null);

		// Token: 0x04000897 RID: 2199
		private static readonly TextObject _night = new TextObject("{=fAxjyMt5}Night", null);

		// Token: 0x04000898 RID: 2200
		private static readonly TextObject _snow = new TextObject("{=vLjgcdgB}Snow", null);

		// Token: 0x04000899 RID: 2201
		private static readonly TextObject _desert = new TextObject("{=ecUwABe2}Desert", null);

		// Token: 0x0400089A RID: 2202
		private static readonly TextObject _sturgiaSnowBonus = new TextObject("{=0VfEGekD}Sturgia snow bonus", null);

		// Token: 0x0400089B RID: 2203
		private readonly TextObject _culture = GameTexts.FindText("str_culture", null);

		// Token: 0x0400089C RID: 2204
		private const float MovingAtForestEffect = -0.2f;

		// Token: 0x0400089D RID: 2205
		private const float MovingAtWaterEffect = -0.3f;

		// Token: 0x0400089E RID: 2206
		private const float MovingAtNightEffect = -0.25f;

		// Token: 0x0400089F RID: 2207
		private const float MovingOnSnowEffect = -0.125f;

		// Token: 0x040008A0 RID: 2208
		private const float MovingInDesertEffect = -0.1f;

		// Token: 0x040008A1 RID: 2209
		private const float CavalryEffect = 0.3f;

		// Token: 0x040008A2 RID: 2210
		private const float MountedFootMenEffect = 0.15f;

		// Token: 0x040008A3 RID: 2211
		private const float HerdEffect = -0.4f;

		// Token: 0x040008A4 RID: 2212
		private const float WoundedEffect = -0.05f;

		// Token: 0x040008A5 RID: 2213
		private const float CargoEffect = -0.02f;

		// Token: 0x040008A6 RID: 2214
		private const float OverburdenedEffect = -0.4f;

		// Token: 0x040008A7 RID: 2215
		private const float HighMoraleThreshold = 70f;

		// Token: 0x040008A8 RID: 2216
		private const float LowMoraleThreshold = 30f;

		// Token: 0x040008A9 RID: 2217
		private const float HighMoraleEffect = 0.05f;

		// Token: 0x040008AA RID: 2218
		private const float LowMoraleEffect = -0.1f;

		// Token: 0x040008AB RID: 2219
		private const float DisorganizedEffect = -0.4f;
	}
}
