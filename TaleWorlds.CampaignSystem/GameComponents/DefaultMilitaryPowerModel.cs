using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000133 RID: 307
	public class DefaultMilitaryPowerModel : MilitaryPowerModel
	{
		// Token: 0x0600196E RID: 6510 RVA: 0x0007C700 File Offset: 0x0007A900
		public override float GetTroopPower(CharacterObject troop, BattleSideEnum side, MapEvent.PowerCalculationContext context, float leaderModifier)
		{
			float defaultTroopPower = Campaign.Current.Models.MilitaryPowerModel.GetDefaultTroopPower(troop);
			float num = 0f;
			if (context != MapEvent.PowerCalculationContext.Estimated)
			{
				num = Campaign.Current.Models.MilitaryPowerModel.GetContextModifier(troop, side, context);
			}
			return defaultTroopPower * (1f + leaderModifier + num);
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x0007C750 File Offset: 0x0007A950
		public override float GetPowerOfParty(PartyBase party, BattleSideEnum side, MapEvent.PowerCalculationContext context)
		{
			float num = 0f;
			Hero leaderHero = party.LeaderHero;
			float num2 = ((leaderHero != null) ? leaderHero.PowerModifier : 0f);
			for (int i = 0; i < party.MemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = party.MemberRoster.GetElementCopyAtIndex(i);
				if (elementCopyAtIndex.Character != null)
				{
					float troopPower = Campaign.Current.Models.MilitaryPowerModel.GetTroopPower(elementCopyAtIndex.Character, side, context, num2);
					num += (float)(elementCopyAtIndex.Number - elementCopyAtIndex.WoundedNumber) * troopPower;
				}
			}
			float num3 = 1f;
			if (party.IsMobile)
			{
				if (context == MapEvent.PowerCalculationContext.Estimated)
				{
					num3 = MBMath.Map(party.MobileParty.Morale, 20f, 40f, 0.7f, 1f);
				}
				else if (party.MobileParty.Morale < 30f)
				{
					num3 = 0.7f;
				}
			}
			return num * num3;
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x0007C834 File Offset: 0x0007AA34
		public override float GetPowerModifierOfHero(Hero leaderHero)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (leaderHero != null)
			{
				foreach (PerkObject perkObject in PerkObject.All)
				{
					if (perkObject.PrimaryRole == PartyRole.Captain && leaderHero.GetPerkValue(perkObject))
					{
						float num5 = perkObject.RequiredSkillValue / (float)Campaign.Current.Models.CharacterDevelopmentModel.MaxSkillRequiredForEpicPerkBonus;
						if (num5 <= 0.3f)
						{
							num++;
						}
						else if (num5 <= 0.6f)
						{
							num2++;
						}
						else if (num5 <= 0.9f)
						{
							num3++;
						}
						else
						{
							num4++;
						}
					}
				}
			}
			return (float)num * 0.01f + (float)num2 * 0.02f + (float)num3 * 0.03f + (float)num4 * 0.06f;
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x0007C918 File Offset: 0x0007AB18
		public override float GetContextModifier(CharacterObject troop, BattleSideEnum battleSide, MapEvent.PowerCalculationContext context)
		{
			DefaultMilitaryPowerModel.PowerFlags powerFlags = this.GetTroopPowerContext(troop);
			switch (context)
			{
			case MapEvent.PowerCalculationContext.PlainBattle:
			case MapEvent.PowerCalculationContext.SteppeBattle:
			case MapEvent.PowerCalculationContext.DesertBattle:
			case MapEvent.PowerCalculationContext.DuneBattle:
			case MapEvent.PowerCalculationContext.SnowBattle:
				powerFlags |= DefaultMilitaryPowerModel.PowerFlags.Flat;
				break;
			case MapEvent.PowerCalculationContext.ForestBattle:
				powerFlags |= DefaultMilitaryPowerModel.PowerFlags.Forest;
				break;
			case MapEvent.PowerCalculationContext.RiverCrossingBattle:
			case MapEvent.PowerCalculationContext.SeaBattle:
			case MapEvent.PowerCalculationContext.OpenSeaBattle:
			case MapEvent.PowerCalculationContext.RiverBattle:
				powerFlags |= DefaultMilitaryPowerModel.PowerFlags.RiverCrossing;
				break;
			case MapEvent.PowerCalculationContext.Village:
			case MapEvent.PowerCalculationContext.NavalRaid:
				powerFlags |= DefaultMilitaryPowerModel.PowerFlags.Village;
				break;
			case MapEvent.PowerCalculationContext.Siege:
				powerFlags |= DefaultMilitaryPowerModel.PowerFlags.Siege;
				break;
			}
			powerFlags |= this.GetBattleSideContext(battleSide);
			return DefaultMilitaryPowerModel._battleModifiers[(uint)powerFlags];
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0007C9AC File Offset: 0x0007ABAC
		public override MapEvent.PowerCalculationContext GetContextForPosition(CampaignVec2 position)
		{
			TerrainType terrainTypeAtPosition = Campaign.Current.MapSceneWrapper.GetTerrainTypeAtPosition(in position);
			if (position.IsOnLand)
			{
				MapWeatherModel.WeatherEvent weatherEventInPosition = Campaign.Current.Models.MapWeatherModel.GetWeatherEventInPosition(position.ToVec2());
				if (weatherEventInPosition == MapWeatherModel.WeatherEvent.Snowy || weatherEventInPosition == MapWeatherModel.WeatherEvent.Blizzard)
				{
					return MapEvent.PowerCalculationContext.SnowBattle;
				}
			}
			switch (terrainTypeAtPosition)
			{
			case TerrainType.Plain:
				return MapEvent.PowerCalculationContext.PlainBattle;
			case TerrainType.Desert:
				return MapEvent.PowerCalculationContext.DesertBattle;
			case TerrainType.Snow:
				return MapEvent.PowerCalculationContext.SnowBattle;
			case TerrainType.Forest:
				return MapEvent.PowerCalculationContext.ForestBattle;
			case TerrainType.Steppe:
				return MapEvent.PowerCalculationContext.SteppeBattle;
			case TerrainType.Fording:
				if (!position.IsOnLand)
				{
					return MapEvent.PowerCalculationContext.RiverCrossingBattle;
				}
				return MapEvent.PowerCalculationContext.PlainBattle;
			case TerrainType.Lake:
				return MapEvent.PowerCalculationContext.RiverCrossingBattle;
			case TerrainType.Water:
				return MapEvent.PowerCalculationContext.SeaBattle;
			case TerrainType.River:
				return MapEvent.PowerCalculationContext.RiverCrossingBattle;
			case TerrainType.Swamp:
				return MapEvent.PowerCalculationContext.PlainBattle;
			case TerrainType.Dune:
				return MapEvent.PowerCalculationContext.DuneBattle;
			case TerrainType.Bridge:
				return MapEvent.PowerCalculationContext.PlainBattle;
			case TerrainType.CoastalSea:
				return MapEvent.PowerCalculationContext.SeaBattle;
			case TerrainType.OpenSea:
				return MapEvent.PowerCalculationContext.OpenSeaBattle;
			case TerrainType.UnderBridge:
				return MapEvent.PowerCalculationContext.RiverCrossingBattle;
			}
			return MapEvent.PowerCalculationContext.PlainBattle;
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x0007CA94 File Offset: 0x0007AC94
		public override float GetDefaultTroopPower(CharacterObject troop)
		{
			int num = (troop.IsHero ? (troop.HeroObject.Level / 4 + 1) : troop.Tier);
			float num2 = (float)((2 + num) * (10 + num)) * 0.02f;
			if (troop.IsHero)
			{
				num2 *= 1.5f;
			}
			return num2;
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x0007CAE2 File Offset: 0x0007ACE2
		public override float GetContextModifier(Ship ship, BattleSideEnum battleSideEnum, MapEvent.PowerCalculationContext context)
		{
			return 0f;
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x0007CAE9 File Offset: 0x0007ACE9
		private DefaultMilitaryPowerModel.PowerFlags GetTroopPowerContext(CharacterObject troop)
		{
			if (troop.HasMount())
			{
				if (!troop.IsRanged)
				{
					return DefaultMilitaryPowerModel.PowerFlags.Cavalry;
				}
				return DefaultMilitaryPowerModel.PowerFlags.HorseArcher;
			}
			else
			{
				if (troop.IsRanged)
				{
					return DefaultMilitaryPowerModel.PowerFlags.Archer;
				}
				return DefaultMilitaryPowerModel.PowerFlags.Infantry;
			}
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x0007CB0C File Offset: 0x0007AD0C
		private DefaultMilitaryPowerModel.PowerFlags GetBattleSideContext(BattleSideEnum battleSide)
		{
			if (battleSide != BattleSideEnum.Attacker)
			{
				return DefaultMilitaryPowerModel.PowerFlags.Defender;
			}
			return DefaultMilitaryPowerModel.PowerFlags.Attacker;
		}

		// Token: 0x04000845 RID: 2117
		private const float LowTierCaptainPerkPowerBoost = 0.01f;

		// Token: 0x04000846 RID: 2118
		private const float MidTierCaptainPerkPowerBoost = 0.02f;

		// Token: 0x04000847 RID: 2119
		private const float HighTierCaptainPerkPowerBoost = 0.03f;

		// Token: 0x04000848 RID: 2120
		private const float UltraTierCaptainPerkPowerBoost = 0.06f;

		// Token: 0x04000849 RID: 2121
		private static readonly Dictionary<uint, float> _battleModifiers = new Dictionary<uint, float>
		{
			{ 69U, 0f },
			{ 133U, 0.05f },
			{ 261U, 0f },
			{ 517U, 0.05f },
			{ 1029U, 0f },
			{ 70U, 0f },
			{ 134U, 0.05f },
			{ 262U, 0.05f },
			{ 518U, 0.05f },
			{ 1030U, 0f },
			{ 73U, -0.2f },
			{ 137U, -0.1f },
			{ 265U, 0f },
			{ 521U, -0.1f },
			{ 1033U, 0f },
			{ 74U, 0.3f },
			{ 138U, 0.05f },
			{ 266U, 0.1f },
			{ 522U, -0.5f },
			{ 1034U, 0f },
			{ 81U, -0.1f },
			{ 145U, 0f },
			{ 273U, -0.15f },
			{ 529U, -0.2f },
			{ 1041U, 0.25f },
			{ 82U, -0.1f },
			{ 146U, -0.1f },
			{ 274U, -0.05f },
			{ 530U, -0.15f },
			{ 1042U, 0.1f },
			{ 97U, -0.2f },
			{ 161U, 0.1f },
			{ 289U, -0.1f },
			{ 545U, -0.3f },
			{ 1057U, 0.3f },
			{ 98U, 0.3f },
			{ 162U, 0f },
			{ 290U, 0f },
			{ 546U, -0.25f },
			{ 1058U, 0.15f }
		};

		// Token: 0x020005C3 RID: 1475
		[Flags]
		private enum PowerFlags
		{
			// Token: 0x040018F9 RID: 6393
			None = 0,
			// Token: 0x040018FA RID: 6394
			Attacker = 1,
			// Token: 0x040018FB RID: 6395
			Defender = 2,
			// Token: 0x040018FC RID: 6396
			Infantry = 4,
			// Token: 0x040018FD RID: 6397
			Archer = 8,
			// Token: 0x040018FE RID: 6398
			Cavalry = 16,
			// Token: 0x040018FF RID: 6399
			HorseArcher = 32,
			// Token: 0x04001900 RID: 6400
			Siege = 64,
			// Token: 0x04001901 RID: 6401
			Village = 128,
			// Token: 0x04001902 RID: 6402
			RiverCrossing = 256,
			// Token: 0x04001903 RID: 6403
			Forest = 512,
			// Token: 0x04001904 RID: 6404
			Flat = 1024
		}
	}
}
