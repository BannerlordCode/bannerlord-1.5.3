using System;
using Helpers;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.GameComponents
{
	// Token: 0x02000168 RID: 360
	public class DefaultTroopSacrificeModel : TroopSacrificeModel
	{
		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06001B89 RID: 7049 RVA: 0x0008E443 File Offset: 0x0008C643
		public override int BreakOutArmyLeaderRelationPenalty
		{
			get
			{
				return -5;
			}
		}

		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06001B8A RID: 7050 RVA: 0x0008E447 File Offset: 0x0008C647
		public override int BreakOutArmyMemberRelationPenalty
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x0008E44A File Offset: 0x0008C64A
		public override ExplainedNumber GetLostTroopCountForBreakingInBesiegedSettlement(MobileParty party, SiegeEvent siegeEvent)
		{
			return this.GetLostTroopCount(party, siegeEvent, party.IsTargetingPort && party.IsCurrentlyAtSea);
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x0008E465 File Offset: 0x0008C665
		public override ExplainedNumber GetLostTroopCountForBreakingOutOfBesiegedSettlement(MobileParty party, SiegeEvent siegeEvent, bool isBreakingOutFromPort)
		{
			return this.GetLostTroopCount(party, siegeEvent, isBreakingOutFromPort);
		}

		// Token: 0x06001B8D RID: 7053 RVA: 0x0008E470 File Offset: 0x0008C670
		public override int GetNumberOfTroopsSacrificedForTryingToGetAway(BattleSideEnum playerBattleSide, MapEvent mapEvent)
		{
			mapEvent.RecalculateStrengthOfSides();
			MapEventSide mapEventSide = mapEvent.GetMapEventSide(playerBattleSide);
			float num = mapEvent.StrengthOfSide[(int)playerBattleSide] + 1f;
			float num2 = mapEvent.StrengthOfSide[(int)playerBattleSide.GetOppositeSide()] / num;
			int num3 = PartyBase.MainParty.NumberOfRegularMembers;
			if (MobileParty.MainParty.Army != null)
			{
				foreach (MobileParty mobileParty in MobileParty.MainParty.Army.LeaderParty.AttachedParties)
				{
					num3 += mobileParty.Party.NumberOfRegularMembers;
				}
			}
			int num4 = mapEventSide.CountTroops((FlattenedTroopRosterElement x) => x.State == RosterTroopState.Active && !x.Troop.IsHero);
			float num5 = (float)num3 * MathF.Pow(MathF.Min(num2, 3f), 1.3f) * 0.1f + 5f;
			ExplainedNumber explainedNumber = new ExplainedNumber(num5, false, null);
			SkillHelper.AddSkillBonusForCharacter(DefaultSkillEffects.TacticsTroopSacrificeReduction, CharacterObject.PlayerCharacter, ref explainedNumber);
			explainedNumber = new ExplainedNumber((float)MathF.Max(1, MathF.Round(explainedNumber.ResultNumber)), false, null);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.SwiftRegroup, MobileParty.MainParty, false, ref explainedNumber);
			if (explainedNumber.ResultNumber <= (float)num4)
			{
				return MathF.Round(explainedNumber.ResultNumber);
			}
			return -1;
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x0008E5D4 File Offset: 0x0008C7D4
		private ExplainedNumber GetLostTroopCount(MobileParty party, SiegeEvent siegeEvent, bool isFromPort)
		{
			if (isFromPort && !siegeEvent.IsBlockadeActive)
			{
				return new ExplainedNumber(0f, false, null);
			}
			int num = 5;
			float num2 = 0f;
			foreach (PartyBase partyBase in siegeEvent.BesiegerCamp.GetInvolvedPartiesForEventType(MapEvent.BattleTypes.Siege))
			{
				num2 += (isFromPort ? partyBase.GetCustomStrength(BattleSideEnum.Attacker, MapEvent.PowerCalculationContext.SeaBattle) : partyBase.GetCustomStrength(BattleSideEnum.Attacker, MapEvent.PowerCalculationContext.PlainBattle));
			}
			float num3;
			int num4;
			if (party.Army != null && party.Army.LeaderParty == party)
			{
				num3 = (isFromPort ? party.Army.LeaderParty.Party.GetCustomStrength(BattleSideEnum.Defender, MapEvent.PowerCalculationContext.SeaBattle) : party.Army.LeaderParty.Party.GetCustomStrength(BattleSideEnum.Defender, MapEvent.PowerCalculationContext.PlainBattle));
				foreach (MobileParty mobileParty in party.Army.LeaderParty.AttachedParties)
				{
					num3 += (isFromPort ? mobileParty.Party.GetCustomStrength(BattleSideEnum.Defender, MapEvent.PowerCalculationContext.SeaBattle) : mobileParty.Party.GetCustomStrength(BattleSideEnum.Defender, MapEvent.PowerCalculationContext.PlainBattle));
				}
				num4 = party.Army.TotalRegularCount;
			}
			else
			{
				num3 = (isFromPort ? party.Party.GetCustomStrength(BattleSideEnum.Defender, MapEvent.PowerCalculationContext.SeaBattle) : party.Party.GetCustomStrength(BattleSideEnum.Defender, MapEvent.PowerCalculationContext.PlainBattle));
				num4 = party.MemberRoster.TotalRegulars;
			}
			float num5 = MathF.Clamp(0.12f * MathF.Pow((num2 + 1f) / (num3 + 1f), 0.25f), 0.12f, 0.24f);
			ExplainedNumber explainedNumber = new ExplainedNumber(num5 * (float)num4, false, null);
			SkillHelper.AddSkillBonusForCharacter(DefaultSkillEffects.TacticsTroopSacrificeReduction, CharacterObject.PlayerCharacter, ref explainedNumber);
			explainedNumber = new ExplainedNumber((float)(num + (int)explainedNumber.ResultNumber), false, null);
			BattleEnvironment battleEnvironment = (isFromPort ? BattleEnvironment.Naval : MobileParty.MainParty.CurrentBattleEnvironment);
			PerkHelper.AddPerkBonusForParty(DefaultPerks.Tactics.Improviser, battleEnvironment, MobileParty.MainParty, false, ref explainedNumber);
			return explainedNumber;
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x0008E7E8 File Offset: 0x0008C9E8
		public override bool CanPlayerGetAwayFromEncounter(out TextObject explanation)
		{
			explanation = TextObject.GetEmpty();
			int num = PartyBase.MainParty.NumberOfHealthyMembers - PartyBase.MainParty.MemberRoster.TotalHeroes;
			if (MobileParty.MainParty.Army != null && (MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty || MobileParty.MainParty.AttachedTo != null))
			{
				foreach (MobileParty mobileParty in MobileParty.MainParty.Army.LeaderParty.AttachedParties)
				{
					num += mobileParty.Party.NumberOfHealthyMembers - mobileParty.Party.MemberRoster.TotalHeroes;
				}
			}
			if (num <= 8 || Campaign.Current.Models.TroopSacrificeModel.GetNumberOfTroopsSacrificedForTryingToGetAway(PlayerEncounter.Current.PlayerSide, PlayerEncounter.Battle) == -1)
			{
				explanation = new TextObject("{=MTbOGRCF}You don't have enough men!", null);
				return false;
			}
			return true;
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x0008E8EC File Offset: 0x0008CAEC
		public override void GetShipsToSacrificeForTryingToGetAway(BattleSideEnum playerBattleSide, MapEvent mapEvent, out MBList<Ship> shipsToCapture, out Ship shipToTakeDamage, out float damageToApplyForLastShip)
		{
			shipsToCapture = new MBList<Ship>();
			shipToTakeDamage = null;
			damageToApplyForLastShip = 0f;
		}

		// Token: 0x04000938 RID: 2360
		public const int MinimumNumberOfTroopsRequiredForGetAway = 8;
	}
}
