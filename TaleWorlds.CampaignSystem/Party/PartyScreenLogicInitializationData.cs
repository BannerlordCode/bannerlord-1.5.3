using System;
using Helpers;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x02000317 RID: 791
	public struct PartyScreenLogicInitializationData
	{
		// Token: 0x06002E3C RID: 11836 RVA: 0x000C1B94 File Offset: 0x000BFD94
		public static PartyScreenLogicInitializationData CreateBasicInitDataWithMainParty(TroopRoster leftMemberRoster, TroopRoster leftPrisonerRoster, PartyScreenLogic.TransferState memberTransferState, PartyScreenLogic.TransferState prisonerTransferState, PartyScreenLogic.TransferState accompanyingTransferState, IsTroopTransferableDelegate troopTransferableDelegate, PartyScreenHelper.PartyScreenMode partyScreenMode, PartyBase leftOwnerParty = null, TextObject leftPartyName = null, TextObject header = null, Hero leftLeaderHero = null, int leftPartyMembersSizeLimit = 0, int leftPartyPrisonersSizeLimit = 0, PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = null, PartyPresentationDoneButtonConditionDelegate partyPresentationDoneButtonConditionDelegate = null, PartyPresentationCancelButtonDelegate partyPresentationCancelButtonDelegate = null, PartyPresentationCancelButtonActivateDelegate partyPresentationCancelButtonActivateDelegate = null, PartyScreenClosedDelegate partyScreenClosedDelegate = null, bool isDismissMode = false, bool transferHealthiesGetWoundedsFirst = false, bool isTroopUpgradesDisabled = false, bool showProgressBar = false, int questModeWageDaysMultiplier = 0)
		{
			return new PartyScreenLogicInitializationData
			{
				LeftOwnerParty = leftOwnerParty,
				RightOwnerParty = PartyBase.MainParty,
				LeftMemberRoster = leftMemberRoster,
				LeftPrisonerRoster = leftPrisonerRoster,
				RightMemberRoster = PartyBase.MainParty.MemberRoster,
				RightPrisonerRoster = PartyBase.MainParty.PrisonRoster,
				LeftLeaderHero = leftLeaderHero,
				RightLeaderHero = PartyBase.MainParty.LeaderHero,
				LeftPartyMembersSizeLimit = leftPartyMembersSizeLimit,
				LeftPartyPrisonersSizeLimit = leftPartyPrisonersSizeLimit,
				RightPartyMembersSizeLimit = PartyBase.MainParty.PartySizeLimit,
				RightPartyPrisonersSizeLimit = PartyBase.MainParty.PrisonerSizeLimit,
				LeftPartyName = leftPartyName,
				RightPartyName = PartyBase.MainParty.Name,
				TroopTransferableDelegate = troopTransferableDelegate,
				PartyScreenMode = partyScreenMode,
				PartyPresentationDoneButtonDelegate = partyPresentationDoneButtonDelegate,
				PartyPresentationDoneButtonConditionDelegate = partyPresentationDoneButtonConditionDelegate,
				PartyPresentationCancelButtonActivateDelegate = partyPresentationCancelButtonActivateDelegate,
				PartyPresentationCancelButtonDelegate = partyPresentationCancelButtonDelegate,
				IsDismissMode = isDismissMode,
				IsTroopUpgradesDisabled = isTroopUpgradesDisabled,
				Header = header,
				PartyScreenClosedDelegate = partyScreenClosedDelegate,
				TransferHealthiesGetWoundedsFirst = transferHealthiesGetWoundedsFirst,
				ShowProgressBar = showProgressBar,
				MemberTransferState = memberTransferState,
				PrisonerTransferState = prisonerTransferState,
				AccompanyingTransferState = accompanyingTransferState,
				QuestModeWageDaysMultiplier = questModeWageDaysMultiplier
			};
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x000C1CE8 File Offset: 0x000BFEE8
		public static PartyScreenLogicInitializationData CreateBasicInitDataWithMainPartyAndOther(MobileParty party, PartyScreenLogic.TransferState memberTransferState, PartyScreenLogic.TransferState prisonerTransferState, PartyScreenLogic.TransferState accompanyingTransferState, IsTroopTransferableDelegate troopTransferableDelegate, PartyScreenHelper.PartyScreenMode partyScreenMode, TextObject header = null, PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = null, PartyPresentationDoneButtonConditionDelegate partyPresentationDoneButtonConditionDelegate = null, PartyPresentationCancelButtonDelegate partyPresentationCancelButtonDelegate = null, PartyPresentationCancelButtonActivateDelegate partyPresentationCancelButtonActivateDelegate = null, PartyScreenClosedDelegate partyScreenClosedDelegate = null, bool isDismissMode = false, bool transferHealthiesGetWoundedsFirst = false, bool isTroopUpgradesDisabled = true, bool showProgressBar = false)
		{
			return new PartyScreenLogicInitializationData
			{
				LeftOwnerParty = party.Party,
				RightOwnerParty = PartyBase.MainParty,
				LeftMemberRoster = party.MemberRoster,
				LeftPrisonerRoster = party.PrisonRoster,
				RightMemberRoster = PartyBase.MainParty.MemberRoster,
				RightPrisonerRoster = PartyBase.MainParty.PrisonRoster,
				LeftLeaderHero = party.LeaderHero,
				RightLeaderHero = PartyBase.MainParty.LeaderHero,
				LeftPartyMembersSizeLimit = party.Party.PartySizeLimit,
				LeftPartyPrisonersSizeLimit = party.Party.PrisonerSizeLimit,
				RightPartyMembersSizeLimit = PartyBase.MainParty.PartySizeLimit,
				RightPartyPrisonersSizeLimit = PartyBase.MainParty.PrisonerSizeLimit,
				LeftPartyName = party.Name,
				RightPartyName = PartyBase.MainParty.Name,
				TroopTransferableDelegate = troopTransferableDelegate,
				PartyScreenMode = partyScreenMode,
				PartyPresentationDoneButtonDelegate = partyPresentationDoneButtonDelegate,
				PartyPresentationDoneButtonConditionDelegate = partyPresentationDoneButtonConditionDelegate,
				PartyPresentationCancelButtonActivateDelegate = partyPresentationCancelButtonActivateDelegate,
				PartyPresentationCancelButtonDelegate = partyPresentationCancelButtonDelegate,
				IsDismissMode = isDismissMode,
				IsTroopUpgradesDisabled = isTroopUpgradesDisabled,
				Header = header,
				PartyScreenClosedDelegate = partyScreenClosedDelegate,
				TransferHealthiesGetWoundedsFirst = transferHealthiesGetWoundedsFirst,
				ShowProgressBar = showProgressBar,
				MemberTransferState = memberTransferState,
				PrisonerTransferState = prisonerTransferState,
				AccompanyingTransferState = accompanyingTransferState
			};
		}

		// Token: 0x04000D55 RID: 3413
		public TroopRoster LeftMemberRoster;

		// Token: 0x04000D56 RID: 3414
		public TroopRoster LeftPrisonerRoster;

		// Token: 0x04000D57 RID: 3415
		public TroopRoster RightMemberRoster;

		// Token: 0x04000D58 RID: 3416
		public TroopRoster RightPrisonerRoster;

		// Token: 0x04000D59 RID: 3417
		public PartyBase LeftOwnerParty;

		// Token: 0x04000D5A RID: 3418
		public PartyBase RightOwnerParty;

		// Token: 0x04000D5B RID: 3419
		public TextObject LeftPartyName;

		// Token: 0x04000D5C RID: 3420
		public TextObject RightPartyName;

		// Token: 0x04000D5D RID: 3421
		public TextObject Header;

		// Token: 0x04000D5E RID: 3422
		public Hero LeftLeaderHero;

		// Token: 0x04000D5F RID: 3423
		public Hero RightLeaderHero;

		// Token: 0x04000D60 RID: 3424
		public int LeftPartyMembersSizeLimit;

		// Token: 0x04000D61 RID: 3425
		public int LeftPartyPrisonersSizeLimit;

		// Token: 0x04000D62 RID: 3426
		public int RightPartyMembersSizeLimit;

		// Token: 0x04000D63 RID: 3427
		public int RightPartyPrisonersSizeLimit;

		// Token: 0x04000D64 RID: 3428
		public PartyPresentationDoneButtonDelegate PartyPresentationDoneButtonDelegate;

		// Token: 0x04000D65 RID: 3429
		public PartyPresentationDoneButtonConditionDelegate PartyPresentationDoneButtonConditionDelegate;

		// Token: 0x04000D66 RID: 3430
		public PartyPresentationCancelButtonActivateDelegate PartyPresentationCancelButtonActivateDelegate;

		// Token: 0x04000D67 RID: 3431
		public IsTroopTransferableDelegate TroopTransferableDelegate;

		// Token: 0x04000D68 RID: 3432
		public CanTalkToHeroDelegate CanTalkToTroopDelegate;

		// Token: 0x04000D69 RID: 3433
		public PartyPresentationCancelButtonDelegate PartyPresentationCancelButtonDelegate;

		// Token: 0x04000D6A RID: 3434
		public PartyScreenClosedDelegate PartyScreenClosedDelegate;

		// Token: 0x04000D6B RID: 3435
		public bool DoNotApplyGoldTransactions;

		// Token: 0x04000D6C RID: 3436
		public bool IsDismissMode;

		// Token: 0x04000D6D RID: 3437
		public bool TransferHealthiesGetWoundedsFirst;

		// Token: 0x04000D6E RID: 3438
		public bool IsTroopUpgradesDisabled;

		// Token: 0x04000D6F RID: 3439
		public bool ShowProgressBar;

		// Token: 0x04000D70 RID: 3440
		public int QuestModeWageDaysMultiplier;

		// Token: 0x04000D71 RID: 3441
		public PartyScreenLogic.TransferState MemberTransferState;

		// Token: 0x04000D72 RID: 3442
		public PartyScreenLogic.TransferState PrisonerTransferState;

		// Token: 0x04000D73 RID: 3443
		public PartyScreenLogic.TransferState AccompanyingTransferState;

		// Token: 0x04000D74 RID: 3444
		public PartyScreenHelper.PartyScreenMode PartyScreenMode;
	}
}
