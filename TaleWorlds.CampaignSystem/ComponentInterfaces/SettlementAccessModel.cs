using System;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x0200018F RID: 399
	public abstract class SettlementAccessModel : MBGameModel<SettlementAccessModel>
	{
		// Token: 0x06001C84 RID: 7300
		public abstract void CanMainHeroEnterSettlement(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails);

		// Token: 0x06001C85 RID: 7301
		public abstract void CanMainHeroEnterLordsHall(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails);

		// Token: 0x06001C86 RID: 7302
		public abstract void CanMainHeroEnterDungeon(Settlement settlement, out SettlementAccessModel.AccessDetails accessDetails);

		// Token: 0x06001C87 RID: 7303
		public abstract bool CanMainHeroAccessLocation(Settlement settlement, string locationId, out bool disableOption, out TextObject disabledText);

		// Token: 0x06001C88 RID: 7304
		public abstract bool CanMainHeroDoSettlementAction(Settlement settlement, SettlementAccessModel.SettlementAction settlementAction, out bool disableOption, out TextObject disabledText);

		// Token: 0x06001C89 RID: 7305
		public abstract bool IsRequestMeetingOptionAvailable(Settlement settlement, out bool disableOption, out TextObject disabledText);

		// Token: 0x02000623 RID: 1571
		public enum AccessLevel
		{
			// Token: 0x040019FD RID: 6653
			NoAccess,
			// Token: 0x040019FE RID: 6654
			LimitedAccess,
			// Token: 0x040019FF RID: 6655
			FullAccess
		}

		// Token: 0x02000624 RID: 1572
		public enum AccessMethod
		{
			// Token: 0x04001A01 RID: 6657
			None,
			// Token: 0x04001A02 RID: 6658
			Direct,
			// Token: 0x04001A03 RID: 6659
			ByRequest
		}

		// Token: 0x02000625 RID: 1573
		public enum AccessLimitationReason
		{
			// Token: 0x04001A05 RID: 6661
			None,
			// Token: 0x04001A06 RID: 6662
			HostileFaction,
			// Token: 0x04001A07 RID: 6663
			RelationshipWithOwner,
			// Token: 0x04001A08 RID: 6664
			CrimeRating,
			// Token: 0x04001A09 RID: 6665
			VillageIsLooted,
			// Token: 0x04001A0A RID: 6666
			Disguised,
			// Token: 0x04001A0B RID: 6667
			ClanTier,
			// Token: 0x04001A0C RID: 6668
			LocationEmpty
		}

		// Token: 0x02000626 RID: 1574
		public enum LimitedAccessSolution
		{
			// Token: 0x04001A0E RID: 6670
			None,
			// Token: 0x04001A0F RID: 6671
			Bribe,
			// Token: 0x04001A10 RID: 6672
			Disguise
		}

		// Token: 0x02000627 RID: 1575
		public enum PreliminaryActionObligation
		{
			// Token: 0x04001A12 RID: 6674
			None,
			// Token: 0x04001A13 RID: 6675
			Optional
		}

		// Token: 0x02000628 RID: 1576
		public enum PreliminaryActionType
		{
			// Token: 0x04001A15 RID: 6677
			None,
			// Token: 0x04001A16 RID: 6678
			FaceCharges
		}

		// Token: 0x02000629 RID: 1577
		public enum SettlementAction
		{
			// Token: 0x04001A18 RID: 6680
			RecruitTroops,
			// Token: 0x04001A19 RID: 6681
			Craft,
			// Token: 0x04001A1A RID: 6682
			WalkAroundTheArena,
			// Token: 0x04001A1B RID: 6683
			JoinTournament,
			// Token: 0x04001A1C RID: 6684
			WatchTournament,
			// Token: 0x04001A1D RID: 6685
			Trade,
			// Token: 0x04001A1E RID: 6686
			WaitInSettlement,
			// Token: 0x04001A1F RID: 6687
			ManageTown
		}

		// Token: 0x0200062A RID: 1578
		public struct AccessDetails
		{
			// Token: 0x04001A20 RID: 6688
			public SettlementAccessModel.AccessLevel AccessLevel;

			// Token: 0x04001A21 RID: 6689
			public SettlementAccessModel.AccessMethod AccessMethod;

			// Token: 0x04001A22 RID: 6690
			public SettlementAccessModel.AccessLimitationReason AccessLimitationReason;

			// Token: 0x04001A23 RID: 6691
			public SettlementAccessModel.LimitedAccessSolution LimitedAccessSolution;

			// Token: 0x04001A24 RID: 6692
			public SettlementAccessModel.PreliminaryActionObligation PreliminaryActionObligation;

			// Token: 0x04001A25 RID: 6693
			public SettlementAccessModel.PreliminaryActionType PreliminaryActionType;
		}
	}
}
