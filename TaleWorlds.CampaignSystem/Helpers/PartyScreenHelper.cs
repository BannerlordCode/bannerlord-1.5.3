using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encyclopedia;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Helpers
{
	// Token: 0x02000025 RID: 37
	public static class PartyScreenHelper
	{
		// Token: 0x06000123 RID: 291 RVA: 0x0000E694 File Offset: 0x0000C894
		public static PartyState GetActivePartyState()
		{
			GameStateManager gameStateManager = GameStateManager.Current;
			PartyState partyState;
			if ((partyState = ((gameStateManager != null) ? gameStateManager.ActiveState : null) as PartyState) != null)
			{
				return partyState;
			}
			Debug.FailedAssert("GetActivePartyState requested but the active state is not PartyState!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "GetActivePartyState", 8153);
			return null;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000E6D8 File Offset: 0x0000C8D8
		private static void OpenPartyScreen(bool isDonating = false)
		{
			Game game = Game.Current;
			PartyScreenLogic partyScreenLogic = new PartyScreenLogic();
			PartyScreenLogicInitializationData partyScreenLogicInitializationData = new PartyScreenLogicInitializationData
			{
				LeftOwnerParty = null,
				RightOwnerParty = PartyBase.MainParty,
				LeftMemberRoster = TroopRoster.CreateDummyTroopRoster(),
				LeftPrisonerRoster = TroopRoster.CreateDummyTroopRoster(),
				RightMemberRoster = PartyBase.MainParty.MemberRoster,
				RightPrisonerRoster = PartyBase.MainParty.PrisonRoster,
				LeftLeaderHero = null,
				RightLeaderHero = PartyBase.MainParty.LeaderHero,
				LeftPartyMembersSizeLimit = 0,
				LeftPartyPrisonersSizeLimit = 0,
				RightPartyMembersSizeLimit = PartyBase.MainParty.PartySizeLimit,
				RightPartyPrisonersSizeLimit = PartyBase.MainParty.PrisonerSizeLimit,
				LeftPartyName = null,
				RightPartyName = PartyBase.MainParty.Name,
				TroopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenHelper.TroopTransferableDelegate),
				PartyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenHelper.DefaultDoneHandler),
				PartyPresentationDoneButtonConditionDelegate = null,
				PartyPresentationCancelButtonActivateDelegate = null,
				PartyPresentationCancelButtonDelegate = null,
				IsDismissMode = true,
				IsTroopUpgradesDisabled = false,
				Header = null,
				PartyScreenClosedDelegate = null,
				TransferHealthiesGetWoundedsFirst = false,
				ShowProgressBar = false,
				MemberTransferState = PartyScreenLogic.TransferState.Transferable,
				PrisonerTransferState = PartyScreenLogic.TransferState.Transferable,
				AccompanyingTransferState = PartyScreenLogic.TransferState.NotTransferable
			};
			partyScreenLogic.Initialize(partyScreenLogicInitializationData);
			PartyState partyState = game.GameStateManager.CreateState<PartyState>();
			partyState.PartyScreenLogic = partyScreenLogic;
			partyState.IsDonating = isDonating;
			partyState.PartyScreenMode = PartyScreenHelper.PartyScreenMode.Normal;
			game.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000E866 File Offset: 0x0000CA66
		public static void CloseScreen(bool isForced, bool fromCancel = false)
		{
			PartyScreenHelper.ClosePartyPresentation(isForced, fromCancel);
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000E870 File Offset: 0x0000CA70
		private static void ClosePartyPresentation(bool isForced, bool fromCancel)
		{
			PartyState activePartyState = PartyScreenHelper.GetActivePartyState();
			PartyScreenLogic partyScreenLogic = ((activePartyState != null) ? activePartyState.PartyScreenLogic : null);
			if (partyScreenLogic == null)
			{
				Debug.FailedAssert("Trying to close party screen when it's already closed!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Helpers.cs", "ClosePartyPresentation", 8220);
				return;
			}
			bool flag = true;
			if (!fromCancel)
			{
				flag = partyScreenLogic != null && partyScreenLogic.DoneLogic(isForced);
			}
			if (flag)
			{
				if (partyScreenLogic != null)
				{
					partyScreenLogic.OnPartyScreenClosed(fromCancel);
				}
				if (activePartyState != null)
				{
					activePartyState.PartyScreenLogic = null;
				}
				Game.Current.GameStateManager.PopState(0);
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000E8E8 File Offset: 0x0000CAE8
		public static void OpenScreenAsCheat()
		{
			if (!Game.Current.CheatMode)
			{
				MBInformationManager.AddQuickInformation(new TextObject("{=!}Cheat mode is not enabled!", null), 0, null, null, "");
				return;
			}
			Game game = Game.Current;
			PartyScreenLogic partyScreenLogic = new PartyScreenLogic();
			PartyScreenLogicInitializationData partyScreenLogicInitializationData = new PartyScreenLogicInitializationData
			{
				LeftOwnerParty = null,
				RightOwnerParty = PartyBase.MainParty,
				LeftMemberRoster = PartyScreenHelper.GetRosterWithAllGameTroops(),
				LeftPrisonerRoster = TroopRoster.CreateDummyTroopRoster(),
				RightMemberRoster = PartyBase.MainParty.MemberRoster,
				RightPrisonerRoster = PartyBase.MainParty.PrisonRoster,
				LeftLeaderHero = null,
				RightLeaderHero = PartyBase.MainParty.LeaderHero,
				LeftPartyMembersSizeLimit = 0,
				LeftPartyPrisonersSizeLimit = 0,
				RightPartyMembersSizeLimit = PartyBase.MainParty.PartySizeLimit,
				RightPartyPrisonersSizeLimit = PartyBase.MainParty.PrisonerSizeLimit,
				LeftPartyName = null,
				RightPartyName = PartyBase.MainParty.Name,
				TroopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenHelper.TroopTransferableDelegate),
				PartyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenHelper.DefaultDoneHandler),
				PartyPresentationDoneButtonConditionDelegate = null,
				PartyPresentationCancelButtonActivateDelegate = null,
				PartyPresentationCancelButtonDelegate = null,
				IsDismissMode = true,
				IsTroopUpgradesDisabled = false,
				Header = null,
				PartyScreenClosedDelegate = null,
				TransferHealthiesGetWoundedsFirst = false,
				ShowProgressBar = false,
				MemberTransferState = PartyScreenLogic.TransferState.Transferable,
				PrisonerTransferState = PartyScreenLogic.TransferState.Transferable,
				AccompanyingTransferState = PartyScreenLogic.TransferState.NotTransferable
			};
			partyScreenLogic.Initialize(partyScreenLogicInitializationData);
			PartyState partyState = game.GameStateManager.CreateState<PartyState>();
			partyState.PartyScreenLogic = partyScreenLogic;
			partyState.IsDonating = false;
			game.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x0000EA94 File Offset: 0x0000CC94
		private static TroopRoster GetRosterWithAllGameTroops()
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			List<CharacterObject> list = new List<CharacterObject>();
			EncyclopediaPage pageOf = Campaign.Current.EncyclopediaManager.GetPageOf(typeof(CharacterObject));
			for (int i = 0; i < CharacterObject.All.Count; i++)
			{
				CharacterObject characterObject = CharacterObject.All[i];
				if (pageOf.IsValidEncyclopediaItem(characterObject))
				{
					list.Add(characterObject);
				}
			}
			list.Sort((CharacterObject a, CharacterObject b) => a.Name.ToString().CompareTo(b.Name.ToString()));
			for (int j = 0; j < list.Count; j++)
			{
				CharacterObject characterObject2 = list[j];
				troopRoster.AddToCounts(characterObject2, PartyScreenHelper._countToAddForEachTroopCheatMode, false, 0, 0, true, -1);
			}
			return troopRoster;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000EB53 File Offset: 0x0000CD53
		public static void OpenScreenAsNormal()
		{
			if (Game.Current.CheatMode)
			{
				PartyScreenHelper.OpenScreenAsCheat();
				return;
			}
			PartyScreenHelper.OpenPartyScreen(false);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000EB70 File Offset: 0x0000CD70
		public static void OpenScreenAsRansom()
		{
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.IsDonating = false;
			partyState.PartyScreenMode = PartyScreenHelper.PartyScreenMode.Ransom;
			PartyScreenLogic partyScreenLogic = new PartyScreenLogic();
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRoster2 = TroopRoster.CreateDummyTroopRoster();
			PartyScreenLogic.TransferState transferState = PartyScreenLogic.TransferState.NotTransferable;
			PartyScreenLogic.TransferState transferState2 = PartyScreenLogic.TransferState.TransferableWithTrade;
			PartyScreenLogic.TransferState transferState3 = PartyScreenLogic.TransferState.NotTransferable;
			IsTroopTransferableDelegate isTroopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenHelper.TroopTransferableDelegate);
			PartyScreenHelper.PartyScreenMode partyScreenMode = partyState.PartyScreenMode;
			PartyBase partyBase = null;
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenHelper.SellPrisonersDoneHandler);
			TextObject textObject = new TextObject("{=SvahUNo6}Ransom Prisoners", null);
			PartyScreenLogicInitializationData partyScreenLogicInitializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(troopRoster, troopRoster2, transferState, transferState2, transferState3, isTroopTransferableDelegate, partyScreenMode, partyBase, GameTexts.FindText("str_ransom_broker", null), textObject, null, 0, 0, partyPresentationDoneButtonDelegate, null, null, null, null, false, false, false, false, 0);
			partyScreenLogicInitializationData.RightMemberRoster = MobileParty.MainParty.MemberRoster.CloneRosterData();
			partyScreenLogicInitializationData.RightPrisonerRoster = MobileParty.MainParty.PrisonRoster.CloneRosterData();
			partyScreenLogicInitializationData.DoNotApplyGoldTransactions = true;
			partyScreenLogic.Initialize(partyScreenLogicInitializationData);
			partyState.PartyScreenLogic = partyScreenLogic;
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000EC50 File Offset: 0x0000CE50
		public static void OpenScreenAsLoot(TroopRoster leftMemberRoster, TroopRoster leftPrisonerRoster, TextObject leftPartyName, int leftPartySizeLimit, PartyScreenClosedDelegate partyScreenClosedDelegate = null)
		{
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.IsDonating = false;
			partyState.PartyScreenMode = PartyScreenHelper.PartyScreenMode.Loot;
			PartyScreenLogic partyScreenLogic = new PartyScreenLogic();
			PartyScreenLogic.TransferState transferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState transferState2 = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState transferState3 = PartyScreenLogic.TransferState.Transferable;
			IsTroopTransferableDelegate isTroopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenHelper.TroopTransferableDelegate);
			PartyScreenHelper.PartyScreenMode partyScreenMode = partyState.PartyScreenMode;
			PartyBase partyBase = null;
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenHelper.DefaultDoneHandler);
			PartyScreenLogicInitializationData partyScreenLogicInitializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(leftMemberRoster, leftPrisonerRoster, transferState, transferState2, transferState3, isTroopTransferableDelegate, partyScreenMode, partyBase, leftPartyName, new TextObject("{=EOQcQa5l}Aftermath", null), null, leftPartySizeLimit, 0, partyPresentationDoneButtonDelegate, null, null, null, partyScreenClosedDelegate, false, false, false, false, 0);
			partyScreenLogic.Initialize(partyScreenLogicInitializationData);
			partyState.PartyScreenLogic = partyScreenLogic;
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000ECF0 File Offset: 0x0000CEF0
		public static void OpenScreenAsManageTroopsAndPrisoners(MobileParty leftParty, PartyScreenClosedDelegate onPartyScreenClosed = null)
		{
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.IsDonating = false;
			partyState.PartyScreenMode = PartyScreenHelper.PartyScreenMode.Normal;
			PartyScreenLogic partyScreenLogic = new PartyScreenLogic();
			PartyScreenLogic.TransferState transferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState transferState2 = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState transferState3 = PartyScreenLogic.TransferState.Transferable;
			IsTroopTransferableDelegate isTroopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenHelper.ClanManageTroopAndPrisonerTransferableDelegate);
			PartyScreenHelper.PartyScreenMode partyScreenMode = partyState.PartyScreenMode;
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenHelper.ManageTroopsAndPrisonersDoneHandler);
			PartyScreenLogicInitializationData partyScreenLogicInitializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainPartyAndOther(leftParty, transferState, transferState2, transferState3, isTroopTransferableDelegate, partyScreenMode, new TextObject("{=uQgNPJnc}Manage Troops", null), partyPresentationDoneButtonDelegate, null, null, null, onPartyScreenClosed, false, false, true, false);
			partyScreenLogic.Initialize(partyScreenLogicInitializationData);
			partyState.PartyScreenLogic = partyScreenLogic;
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000ED84 File Offset: 0x0000CF84
		public static void OpenScreenAsManagePlayerClanPartyClosed(PartyBase leftOwnerParty, TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, PartyBase rightOwnerParty, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, bool fromCancel)
		{
			if (leftOwnerParty.MemberRoster.TotalManCount <= 0)
			{
				if (leftOwnerParty.Ships.Count > 0)
				{
					for (int i = leftOwnerParty.Ships.Count - 1; i >= 0; i--)
					{
						ChangeShipOwnerAction.ApplyByTransferring(rightOwnerParty, leftOwnerParty.Ships[i]);
					}
				}
				DestroyPartyAction.Apply(null, leftOwnerParty.MobileParty);
			}
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000EDE4 File Offset: 0x0000CFE4
		public static void OpenScreenAsReceiveTroops(TroopRoster leftMemberParty, TextObject leftPartyName, PartyScreenClosedDelegate partyScreenClosedDelegate = null)
		{
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.IsDonating = false;
			partyState.PartyScreenMode = PartyScreenHelper.PartyScreenMode.TroopsManage;
			PartyScreenLogic partyScreenLogic = new PartyScreenLogic();
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			PartyScreenLogic.TransferState transferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState transferState2 = PartyScreenLogic.TransferState.NotTransferable;
			PartyScreenLogic.TransferState transferState3 = PartyScreenLogic.TransferState.Transferable;
			IsTroopTransferableDelegate isTroopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenHelper.TroopTransferableDelegate);
			PartyScreenHelper.PartyScreenMode partyScreenMode = partyState.PartyScreenMode;
			PartyBase partyBase = null;
			int totalManCount = leftMemberParty.TotalManCount;
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenHelper.DefaultDoneHandler);
			PartyScreenLogicInitializationData partyScreenLogicInitializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(leftMemberParty, troopRoster, transferState, transferState2, transferState3, isTroopTransferableDelegate, partyScreenMode, partyBase, leftPartyName, new TextObject("{=uQgNPJnc}Manage Troops", null), null, totalManCount, 0, partyPresentationDoneButtonDelegate, null, null, null, partyScreenClosedDelegate, false, false, false, false, 0);
			partyScreenLogic.Initialize(partyScreenLogicInitializationData);
			partyState.PartyScreenLogic = partyScreenLogic;
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000EE8C File Offset: 0x0000D08C
		public static void OpenScreenAsManageTroops(MobileParty leftParty)
		{
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.IsDonating = false;
			partyState.PartyScreenMode = PartyScreenHelper.PartyScreenMode.TroopsManage;
			PartyScreenLogic partyScreenLogic = new PartyScreenLogic();
			PartyScreenLogicInitializationData partyScreenLogicInitializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainPartyAndOther(leftParty, PartyScreenLogic.TransferState.Transferable, PartyScreenLogic.TransferState.NotTransferable, PartyScreenLogic.TransferState.Transferable, new IsTroopTransferableDelegate(PartyScreenHelper.ClanManageTroopTransferableDelegate), partyState.PartyScreenMode, new TextObject("{=uQgNPJnc}Manage Troops", null), new PartyPresentationDoneButtonDelegate(PartyScreenHelper.DefaultDoneHandler), null, null, null, null, false, false, true, false);
			partyScreenLogic.Initialize(partyScreenLogicInitializationData);
			partyScreenLogic.CanTalkToHeroDelegate = delegate(Hero hero, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase party, out TextObject reason)
			{
				reason = new TextObject("{=SfpoMma5}You can't talk to your companions while managing the garrison.", null);
				return false;
			};
			partyState.PartyScreenLogic = partyScreenLogic;
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000EF3C File Offset: 0x0000D13C
		public static void OpenScreenAsDonateTroops(MobileParty leftParty)
		{
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.IsDonating = leftParty.Owner.Clan != Clan.PlayerClan;
			partyState.PartyScreenMode = PartyScreenHelper.PartyScreenMode.TroopsManage;
			PartyScreenLogic partyScreenLogic = new PartyScreenLogic();
			PartyScreenLogic.TransferState transferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState transferState2 = PartyScreenLogic.TransferState.NotTransferable;
			PartyScreenLogic.TransferState transferState3 = PartyScreenLogic.TransferState.Transferable;
			IsTroopTransferableDelegate isTroopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenHelper.DonateModeTroopTransferableDelegate);
			PartyScreenHelper.PartyScreenMode partyScreenMode = partyState.PartyScreenMode;
			PartyPresentationDoneButtonConditionDelegate partyPresentationDoneButtonConditionDelegate = new PartyPresentationDoneButtonConditionDelegate(PartyScreenHelper.DonateDonePossibleDelegate);
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenHelper.DefaultDoneHandler);
			PartyScreenLogicInitializationData partyScreenLogicInitializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainPartyAndOther(leftParty, transferState, transferState2, transferState3, isTroopTransferableDelegate, partyScreenMode, new TextObject("{=4YfjgtO2}Donate Troops", null), partyPresentationDoneButtonDelegate, partyPresentationDoneButtonConditionDelegate, null, null, null, false, false, true, false);
			partyScreenLogic.Initialize(partyScreenLogicInitializationData);
			partyState.PartyScreenLogic = partyScreenLogic;
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000EFEC File Offset: 0x0000D1EC
		public static void OpenScreenAsDonateGarrisonWithCurrentSettlement()
		{
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.IsDonating = true;
			partyState.PartyScreenMode = PartyScreenHelper.PartyScreenMode.TroopsManage;
			PartyScreenLogic partyScreenLogic = new PartyScreenLogic();
			if (Hero.MainHero.CurrentSettlement.Town.GarrisonParty == null)
			{
				Hero.MainHero.CurrentSettlement.AddGarrisonParty();
			}
			MobileParty garrisonParty = Hero.MainHero.CurrentSettlement.Town.GarrisonParty;
			int num = Math.Max(garrisonParty.Party.PartySizeLimit - garrisonParty.Party.NumberOfAllMembers, 0);
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRoster2 = TroopRoster.CreateDummyTroopRoster();
			PartyScreenLogic.TransferState transferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState transferState2 = PartyScreenLogic.TransferState.NotTransferable;
			PartyScreenLogic.TransferState transferState3 = PartyScreenLogic.TransferState.Transferable;
			IsTroopTransferableDelegate isTroopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenHelper.TroopTransferableDelegate);
			PartyScreenHelper.PartyScreenMode partyScreenMode = partyState.PartyScreenMode;
			PartyBase partyBase = null;
			TextObject name = garrisonParty.Name;
			int num2 = num;
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenHelper.DonateGarrisonDoneHandler);
			PartyScreenLogicInitializationData partyScreenLogicInitializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(troopRoster, troopRoster2, transferState, transferState2, transferState3, isTroopTransferableDelegate, partyScreenMode, partyBase, name, new TextObject("{=uQgNPJnc}Manage Troops", null), null, num2, 0, partyPresentationDoneButtonDelegate, null, null, null, null, false, false, false, false, 0);
			partyScreenLogic.Initialize(partyScreenLogicInitializationData);
			partyState.PartyScreenLogic = partyScreenLogic;
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000F0F0 File Offset: 0x0000D2F0
		public static void OpenScreenAsDonatePrisoners()
		{
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.IsDonating = true;
			partyState.PartyScreenMode = PartyScreenHelper.PartyScreenMode.PrisonerManage;
			PartyScreenLogic partyScreenLogic = new PartyScreenLogic();
			if (Hero.MainHero.CurrentSettlement.Town.GarrisonParty == null)
			{
				Hero.MainHero.CurrentSettlement.AddGarrisonParty();
			}
			TroopRoster prisonRoster = Hero.MainHero.CurrentSettlement.Party.PrisonRoster;
			int num = Math.Max(Hero.MainHero.CurrentSettlement.Party.PrisonerSizeLimit - prisonRoster.Count, 0);
			TextObject textObject = new TextObject("{=SDzIAtiA}Prisoners of {SETTLEMENT_NAME}", null);
			textObject.SetTextVariable("SETTLEMENT_NAME", Hero.MainHero.CurrentSettlement.Name);
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRoster2 = prisonRoster;
			PartyScreenLogic.TransferState transferState = PartyScreenLogic.TransferState.NotTransferable;
			PartyScreenLogic.TransferState transferState2 = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState transferState3 = PartyScreenLogic.TransferState.NotTransferable;
			IsTroopTransferableDelegate isTroopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenHelper.DonatePrisonerTransferableDelegate);
			PartyScreenHelper.PartyScreenMode partyScreenMode = partyState.PartyScreenMode;
			PartyBase partyBase = null;
			TextObject textObject2 = textObject;
			int num2 = num;
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenHelper.DonatePrisonersDoneHandler);
			PartyPresentationDoneButtonConditionDelegate partyPresentationDoneButtonConditionDelegate = new PartyPresentationDoneButtonConditionDelegate(PartyScreenHelper.DonateDonePossibleDelegate);
			PartyScreenLogicInitializationData partyScreenLogicInitializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(troopRoster, troopRoster2, transferState, transferState2, transferState3, isTroopTransferableDelegate, partyScreenMode, partyBase, textObject2, new TextObject("{=Z212GSiV}Leave Prisoners", null), null, 0, num2, partyPresentationDoneButtonDelegate, partyPresentationDoneButtonConditionDelegate, null, null, null, false, false, false, false, 0);
			partyScreenLogic.Initialize(partyScreenLogicInitializationData);
			partyState.PartyScreenLogic = partyScreenLogic;
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000F228 File Offset: 0x0000D428
		private static Tuple<bool, TextObject> DonateDonePossibleDelegate(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, int leftLimitNum, int rightLimitNum)
		{
			PartyState activePartyState = PartyScreenHelper.GetActivePartyState();
			PartyScreenLogic partyScreenLogic = ((activePartyState != null) ? activePartyState.PartyScreenLogic : null);
			if (partyScreenLogic.IsThereAnyChanges())
			{
				if (partyScreenLogic.CurrentData.TransferredPrisonersHistory.Any<Tuple<CharacterObject, int>>((Tuple<CharacterObject, int> p) => p.Item2 > 0))
				{
					return new Tuple<bool, TextObject>(false, new TextObject("{=hI7eDbXs}You cannot take prisoners.", null));
				}
				if (partyScreenLogic.HaveRightSideGainedTroops())
				{
					return new Tuple<bool, TextObject>(false, new TextObject("{=pvkl6pZh}You cannot take troops.", null));
				}
				if ((partyScreenLogic.MemberTransferState != PartyScreenLogic.TransferState.NotTransferable || partyScreenLogic.AccompanyingTransferState != PartyScreenLogic.TransferState.NotTransferable) && partyScreenLogic.LeftPartyMembersSizeLimit < partyScreenLogic.MemberRosters[0].TotalManCount)
				{
					return new Tuple<bool, TextObject>(false, new TextObject("{=R7wiHjcL}Donated troops exceed party capacity.", null));
				}
				if (partyScreenLogic.PrisonerTransferState != PartyScreenLogic.TransferState.NotTransferable && partyScreenLogic.LeftPartyPrisonersSizeLimit < partyScreenLogic.PrisonerRosters[0].TotalManCount)
				{
					return new Tuple<bool, TextObject>(false, new TextObject("{=3nfPGbN0}Donated prisoners exceed party capacity.", null));
				}
			}
			return new Tuple<bool, TextObject>(true, TextObject.GetEmpty());
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000F320 File Offset: 0x0000D520
		public static bool DonatePrisonerTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return side == PartyScreenLogic.PartyRosterSide.Right && type == PartyScreenLogic.TroopType.Prisoner;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000F32C File Offset: 0x0000D52C
		public static void OpenScreenAsManagePrisoners()
		{
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.IsDonating = false;
			partyState.PartyScreenMode = PartyScreenHelper.PartyScreenMode.PrisonerManage;
			PartyScreenLogic partyScreenLogic = new PartyScreenLogic();
			TroopRoster prisonRoster = Hero.MainHero.CurrentSettlement.Party.PrisonRoster;
			TextObject textObject = new TextObject("{=SDzIAtiA}Prisoners of {SETTLEMENT_NAME}", null);
			textObject.SetTextVariable("SETTLEMENT_NAME", Hero.MainHero.CurrentSettlement.Name);
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRoster2 = prisonRoster;
			PartyScreenLogic.TransferState transferState = PartyScreenLogic.TransferState.NotTransferable;
			PartyScreenLogic.TransferState transferState2 = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState transferState3 = PartyScreenLogic.TransferState.NotTransferable;
			IsTroopTransferableDelegate isTroopTransferableDelegate = new IsTroopTransferableDelegate(PartyScreenHelper.TroopTransferableDelegate);
			PartyScreenHelper.PartyScreenMode partyScreenMode = partyState.PartyScreenMode;
			PartyBase partyBase = null;
			TextObject textObject2 = textObject;
			int prisonerSizeLimit = Hero.MainHero.CurrentSettlement.Party.PrisonerSizeLimit;
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenHelper.ManageGarrisonDoneHandler);
			PartyScreenLogicInitializationData partyScreenLogicInitializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(troopRoster, troopRoster2, transferState, transferState2, transferState3, isTroopTransferableDelegate, partyScreenMode, partyBase, textObject2, new TextObject("{=aadTnAEg}Manage Prisoners", null), null, 0, prisonerSizeLimit, partyPresentationDoneButtonDelegate, null, null, null, null, false, false, false, false, 0);
			partyScreenLogic.Initialize(partyScreenLogicInitializationData);
			partyState.PartyScreenLogic = partyScreenLogic;
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000F41C File Offset: 0x0000D61C
		public static bool TroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase leftOwnerParty)
		{
			Hero hero = ((leftOwnerParty != null) ? leftOwnerParty.LeaderHero : null);
			bool flag;
			if ((hero == null || hero.Clan != Clan.PlayerClan) && (leftOwnerParty == null || !leftOwnerParty.IsMobile || !leftOwnerParty.MobileParty.IsCaravan || leftOwnerParty.Owner != Hero.MainHero))
			{
				if (leftOwnerParty != null && leftOwnerParty.IsMobile && leftOwnerParty.MobileParty.IsGarrison)
				{
					Settlement currentSettlement = leftOwnerParty.MobileParty.CurrentSettlement;
					flag = ((currentSettlement != null) ? currentSettlement.OwnerClan : null) == Clan.PlayerClan;
				}
				else
				{
					flag = false;
				}
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			return !character.IsHero || (character.IsHero && character.HeroObject.Clan != Clan.PlayerClan && (!character.HeroObject.IsPlayerCompanion || (character.HeroObject.IsPlayerCompanion && flag2)));
		}

		// Token: 0x06000137 RID: 311 RVA: 0x0000F4EA File Offset: 0x0000D6EA
		public static bool ClanManageTroopAndPrisonerTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return !character.IsHero || character.HeroObject.IsPrisoner;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x0000F501 File Offset: 0x0000D701
		public static bool ClanManageTroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return !character.IsHero;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000F50C File Offset: 0x0000D70C
		public static bool DonateModeTroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return !character.IsHero && side == PartyScreenLogic.PartyRosterSide.Right;
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000F51C File Offset: 0x0000D71C
		public static void OpenScreenWithCondition(IsTroopTransferableDelegate isTroopTransferable, PartyPresentationDoneButtonConditionDelegate doneButtonCondition, PartyPresentationDoneButtonDelegate onDoneClicked, PartyPresentationCancelButtonDelegate onCancelClicked, PartyScreenLogic.TransferState memberTransferState, PartyScreenLogic.TransferState prisonerTransferState, TextObject leftPartyName, int limit, bool showProgressBar, bool isDonating, PartyScreenHelper.PartyScreenMode screenMode = PartyScreenHelper.PartyScreenMode.Normal, TroopRoster memberRosterLeft = null, TroopRoster prisonerRosterLeft = null)
		{
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.IsDonating = isDonating;
			partyState.PartyScreenMode = screenMode;
			if (memberRosterLeft == null)
			{
				memberRosterLeft = TroopRoster.CreateDummyTroopRoster();
			}
			if (prisonerRosterLeft == null)
			{
				prisonerRosterLeft = TroopRoster.CreateDummyTroopRoster();
			}
			PartyScreenLogic partyScreenLogic = new PartyScreenLogic();
			PartyScreenLogicInitializationData partyScreenLogicInitializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(memberRosterLeft, prisonerRosterLeft, memberTransferState, prisonerTransferState, PartyScreenLogic.TransferState.NotTransferable, isTroopTransferable, partyState.PartyScreenMode, null, leftPartyName, new TextObject("{=nZaeTlj8}Exchange Troops", null), null, limit, 0, onDoneClicked, doneButtonCondition, onCancelClicked, null, null, false, false, false, showProgressBar, 0);
			partyScreenLogic.Initialize(partyScreenLogicInitializationData);
			partyState.PartyScreenLogic = partyScreenLogic;
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x0000F5CC File Offset: 0x0000D7CC
		public static void OpenScreenForManagingAlley(bool isNewAlley, TroopRoster memberRosterLeft, IsTroopTransferableDelegate isTroopTransferable, PartyPresentationDoneButtonConditionDelegate doneButtonCondition, PartyPresentationDoneButtonDelegate onDoneClicked, TextObject leftPartyName, PartyPresentationCancelButtonDelegate onCancelButtonClicked)
		{
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.PartyScreenMode = PartyScreenHelper.PartyScreenMode.TroopsManage;
			PartyScreenLogic partyScreenLogic = new PartyScreenLogic();
			TroopRoster troopRoster = (isNewAlley ? MobileParty.MainParty.MemberRoster.CloneRosterData() : MobileParty.MainParty.MemberRoster);
			TroopRosterElement troopRosterElement = memberRosterLeft.GetTroopRoster().Find((TroopRosterElement x) => x.Character.IsHero);
			if (troopRoster.Contains(troopRosterElement.Character))
			{
				troopRoster.RemoveTroop(troopRosterElement.Character, 1, default(UniqueTroopDescriptor), 0);
			}
			PartyScreenLogicInitializationData partyScreenLogicInitializationData = new PartyScreenLogicInitializationData
			{
				TroopTransferableDelegate = isTroopTransferable,
				PartyPresentationDoneButtonConditionDelegate = doneButtonCondition,
				PartyPresentationDoneButtonDelegate = onDoneClicked,
				LeftMemberRoster = memberRosterLeft,
				LeftPrisonerRoster = TroopRoster.CreateDummyTroopRoster(),
				PartyPresentationCancelButtonDelegate = onCancelButtonClicked,
				RightMemberRoster = troopRoster,
				RightPrisonerRoster = TroopRoster.CreateDummyTroopRoster(),
				LeftPartyMembersSizeLimit = Campaign.Current.Models.AlleyModel.MaximumTroopCountInPlayerOwnedAlley + 1,
				RightPartyMembersSizeLimit = PartyBase.MainParty.PartySizeLimit,
				RightPartyPrisonersSizeLimit = PartyBase.MainParty.PrisonerSizeLimit,
				MemberTransferState = PartyScreenLogic.TransferState.Transferable,
				PrisonerTransferState = PartyScreenLogic.TransferState.NotTransferable,
				AccompanyingTransferState = PartyScreenLogic.TransferState.NotTransferable,
				PartyScreenMode = PartyScreenHelper.PartyScreenMode.TroopsManage,
				IsTroopUpgradesDisabled = true,
				ShowProgressBar = true,
				TransferHealthiesGetWoundedsFirst = false,
				IsDismissMode = false,
				QuestModeWageDaysMultiplier = 0,
				Header = null,
				RightOwnerParty = PartyBase.MainParty,
				RightPartyName = PartyBase.MainParty.Name
			};
			partyScreenLogic.Initialize(partyScreenLogicInitializationData);
			partyState.PartyScreenLogic = partyScreenLogic;
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x0000F788 File Offset: 0x0000D988
		public static void OpenScreenAsQuest(TroopRoster leftMemberRoster, TextObject leftPartyName, int leftPartySizeLimit, int questDaysMultiplier, PartyPresentationDoneButtonConditionDelegate doneButtonCondition, PartyScreenClosedDelegate onPartyScreenClosed, IsTroopTransferableDelegate isTroopTransferable, PartyPresentationCancelButtonActivateDelegate partyPresentationCancelButtonActivateDelegate = null)
		{
			Debug.Print("PartyScreenManager::OpenScreenAsQuest", 0, Debug.DebugColor.White, 17592186044416UL);
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.IsDonating = false;
			partyState.PartyScreenMode = PartyScreenHelper.PartyScreenMode.QuestTroopManage;
			PartyScreenLogic partyScreenLogic = new PartyScreenLogic();
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			PartyScreenLogic.TransferState transferState = PartyScreenLogic.TransferState.Transferable;
			PartyScreenLogic.TransferState transferState2 = PartyScreenLogic.TransferState.NotTransferable;
			PartyScreenLogic.TransferState transferState3 = PartyScreenLogic.TransferState.Transferable;
			PartyScreenHelper.PartyScreenMode partyScreenMode = partyState.PartyScreenMode;
			PartyBase partyBase = null;
			PartyPresentationDoneButtonDelegate partyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenHelper.ManageTroopsAndPrisonersDoneHandler);
			PartyScreenLogicInitializationData partyScreenLogicInitializationData = PartyScreenLogicInitializationData.CreateBasicInitDataWithMainParty(leftMemberRoster, troopRoster, transferState, transferState2, transferState3, isTroopTransferable, partyScreenMode, partyBase, leftPartyName, new TextObject("{=nZaeTlj8}Exchange Troops", null), null, leftPartySizeLimit, 0, partyPresentationDoneButtonDelegate, doneButtonCondition, null, partyPresentationCancelButtonActivateDelegate, onPartyScreenClosed, false, true, false, true, questDaysMultiplier);
			partyScreenLogic.Initialize(partyScreenLogicInitializationData);
			partyState.PartyScreenLogic = partyScreenLogic;
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000F840 File Offset: 0x0000DA40
		public static void OpenScreenWithDummyRoster(TroopRoster leftMemberRoster, TroopRoster leftPrisonerRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonerRoster, TextObject leftPartyName, TextObject rightPartyName, int leftPartySizeLimit, int rightPartySizeLimit, PartyPresentationDoneButtonConditionDelegate doneButtonCondition, PartyScreenClosedDelegate onPartyScreenClosed, IsTroopTransferableDelegate isTroopTransferable, CanTalkToHeroDelegate canTalkToTroopDelegate = null, PartyPresentationCancelButtonActivateDelegate partyPresentationCancelButtonActivateDelegate = null)
		{
			Debug.Print("PartyScreenManager::OpenScreenWithDummyRoster", 0, Debug.DebugColor.White, 17592186044416UL);
			PartyScreenLogic partyScreenLogic = new PartyScreenLogic();
			PartyScreenLogicInitializationData partyScreenLogicInitializationData = new PartyScreenLogicInitializationData
			{
				LeftOwnerParty = null,
				RightOwnerParty = MobileParty.MainParty.Party,
				LeftMemberRoster = leftMemberRoster,
				LeftPrisonerRoster = leftPrisonerRoster,
				RightMemberRoster = rightMemberRoster,
				RightPrisonerRoster = rightPrisonerRoster,
				LeftLeaderHero = null,
				RightLeaderHero = PartyBase.MainParty.LeaderHero,
				LeftPartyMembersSizeLimit = leftPartySizeLimit,
				LeftPartyPrisonersSizeLimit = 0,
				RightPartyMembersSizeLimit = rightPartySizeLimit,
				RightPartyPrisonersSizeLimit = 0,
				LeftPartyName = leftPartyName,
				RightPartyName = rightPartyName,
				TroopTransferableDelegate = isTroopTransferable,
				CanTalkToTroopDelegate = (canTalkToTroopDelegate ?? new CanTalkToHeroDelegate(PartyScreenHelper.CanTalkToHero)),
				PartyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenHelper.ManageTroopsAndPrisonersDoneHandler),
				PartyPresentationDoneButtonConditionDelegate = doneButtonCondition,
				PartyPresentationCancelButtonActivateDelegate = partyPresentationCancelButtonActivateDelegate,
				PartyPresentationCancelButtonDelegate = null,
				PartyScreenClosedDelegate = onPartyScreenClosed,
				IsDismissMode = true,
				IsTroopUpgradesDisabled = true,
				Header = null,
				TransferHealthiesGetWoundedsFirst = true,
				ShowProgressBar = false,
				MemberTransferState = PartyScreenLogic.TransferState.Transferable,
				PrisonerTransferState = PartyScreenLogic.TransferState.NotTransferable,
				AccompanyingTransferState = PartyScreenLogic.TransferState.Transferable
			};
			partyScreenLogic.Initialize(partyScreenLogicInitializationData);
			PartyState partyState = Game.Current.GameStateManager.CreateState<PartyState>();
			partyState.PartyScreenLogic = partyScreenLogic;
			partyState.IsDonating = false;
			partyState.PartyScreenMode = PartyScreenHelper.PartyScreenMode.TroopsManage;
			Game.Current.GameStateManager.PushState(partyState, 0);
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000F9D0 File Offset: 0x0000DBD0
		public static void OpenScreenWithDummyRosterWithMainParty(TroopRoster leftMemberRoster, TroopRoster leftPrisonerRoster, TextObject leftPartyName, int leftPartySizeLimit, PartyPresentationDoneButtonConditionDelegate doneButtonCondition, PartyScreenClosedDelegate onPartyScreenClosed, IsTroopTransferableDelegate isTroopTransferable, PartyPresentationCancelButtonActivateDelegate partyPresentationCancelButtonActivateDelegate = null)
		{
			Debug.Print("PartyScreenManager::OpenScreenWithDummyRosterWithMainParty", 0, Debug.DebugColor.White, 17592186044416UL);
			PartyScreenHelper.OpenScreenWithDummyRoster(leftMemberRoster, leftPrisonerRoster, MobileParty.MainParty.MemberRoster, MobileParty.MainParty.PrisonRoster, leftPartyName, MobileParty.MainParty.Name, leftPartySizeLimit, MobileParty.MainParty.Party.PartySizeLimit, doneButtonCondition, onPartyScreenClosed, isTroopTransferable, null, partyPresentationCancelButtonActivateDelegate);
		}

		// Token: 0x0600013F RID: 319 RVA: 0x0000FA34 File Offset: 0x0000DC34
		public static void OpenScreenAsCreateClanPartyForHero(Hero hero, PartyScreenClosedDelegate onScreenClosed = null, IsTroopTransferableDelegate isTroopTransferable = null)
		{
			TroopRoster troopRoster = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRoster2 = TroopRoster.CreateDummyTroopRoster();
			TroopRoster troopRoster3 = MobileParty.MainParty.MemberRoster.CloneRosterData();
			TroopRoster troopRoster4 = MobileParty.MainParty.PrisonRoster.CloneRosterData();
			troopRoster.AddToCounts(hero.CharacterObject, 1, false, 0, 0, true, -1);
			if (troopRoster3.Contains(hero.CharacterObject))
			{
				troopRoster3.AddToCounts(hero.CharacterObject, -1, false, 0, 0, true, -1);
			}
			CanTalkToHeroDelegate canTalkToHeroDelegate = delegate(Hero heroCharacter, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase leftOwnerParty, out TextObject cantTalkReason)
			{
				cantTalkReason = new TextObject("{=8muhth8y}You can't talk to your companions while creating a party.", null);
				return false;
			};
			TextObject textObject = GameTexts.FindText("str_lord_party_name", null);
			textObject.SetCharacterProperties("TROOP", hero.CharacterObject, false);
			PartyScreenHelper.OpenScreenWithDummyRoster(troopRoster, troopRoster2, troopRoster3, troopRoster4, textObject, MobileParty.MainParty.Name, Campaign.Current.Models.PartySizeLimitModel.GetAssumedPartySizeForLordParty(hero, hero.Clan.MapFaction, hero.Clan), MobileParty.MainParty.Party.PartySizeLimit, null, onScreenClosed ?? new PartyScreenClosedDelegate(PartyScreenHelper.OpenScreenAsCreateClanPartyForHeroPartyScreenClosed), isTroopTransferable ?? new IsTroopTransferableDelegate(PartyScreenHelper.OpenScreenAsCreateClanPartyForHeroTroopTransferableDelegate), canTalkToHeroDelegate, null);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x0000FB50 File Offset: 0x0000DD50
		private static void OpenScreenAsCreateClanPartyForHeroPartyScreenClosed(PartyBase leftOwnerParty, TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, PartyBase rightOwnerParty, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, bool fromCancel)
		{
			if (!fromCancel)
			{
				Hero hero = null;
				for (int i = 0; i < leftMemberRoster.data.Length; i++)
				{
					CharacterObject character = leftMemberRoster.data[i].Character;
					if (character != null && character.IsHero)
					{
						hero = leftMemberRoster.data[i].Character.HeroObject;
					}
				}
				int partyGoldLowerThreshold = Campaign.Current.Models.ClanFinanceModel.PartyGoldLowerThreshold;
				if (hero.Gold < partyGoldLowerThreshold)
				{
					GiveGoldAction.ApplyBetweenCharacters(Hero.MainHero, hero, partyGoldLowerThreshold - hero.Gold, false);
				}
				MobileParty mobileParty = MobilePartyHelper.CreateNewClanMobileParty(hero, hero.Clan);
				foreach (TroopRosterElement troopRosterElement in leftMemberRoster.GetTroopRoster())
				{
					if (troopRosterElement.Character != hero.CharacterObject)
					{
						mobileParty.MemberRoster.Add(troopRosterElement);
						rightOwnerParty.MemberRoster.AddToCounts(troopRosterElement.Character, -troopRosterElement.Number, false, -troopRosterElement.WoundedNumber, -troopRosterElement.Xp, true, -1);
					}
				}
				foreach (TroopRosterElement troopRosterElement2 in leftPrisonRoster.GetTroopRoster())
				{
					mobileParty.PrisonRoster.Add(troopRosterElement2);
					rightOwnerParty.PrisonRoster.AddToCounts(troopRosterElement2.Character, -troopRosterElement2.Number, false, -troopRosterElement2.WoundedNumber, -troopRosterElement2.Xp, true, -1);
				}
			}
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000FCF4 File Offset: 0x0000DEF4
		private static bool OpenScreenAsCreateClanPartyForHeroTroopTransferableDelegate(CharacterObject character, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty)
		{
			return !character.IsHero;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x0000FCFF File Offset: 0x0000DEFF
		private static bool SellPrisonersDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
		{
			SellPrisonersAction.ApplyForSelectedPrisoners(MobileParty.MainParty.Party, null, leftPrisonRoster);
			return true;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x0000FD14 File Offset: 0x0000DF14
		private static bool DonateGarrisonDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
		{
			Settlement currentSettlement = Hero.MainHero.CurrentSettlement;
			MobileParty mobileParty = currentSettlement.Town.GarrisonParty;
			if (mobileParty == null)
			{
				currentSettlement.AddGarrisonParty();
				mobileParty = currentSettlement.Town.GarrisonParty;
			}
			for (int i = 0; i < leftMemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = leftMemberRoster.GetElementCopyAtIndex(i);
				mobileParty.AddElementToMemberRoster(elementCopyAtIndex.Character, elementCopyAtIndex.Number, false);
				if (elementCopyAtIndex.Character.IsHero)
				{
					EnterSettlementAction.ApplyForCharacterOnly(elementCopyAtIndex.Character.HeroObject, currentSettlement);
				}
			}
			return true;
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000FD9C File Offset: 0x0000DF9C
		private static bool DonatePrisonersDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster leftSideTransferredPrisonerRoster, FlattenedTroopRoster rightSideTransferredPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
		{
			if (!rightSideTransferredPrisonerRoster.IsEmpty<FlattenedTroopRosterElement>())
			{
				Settlement currentSettlement = Hero.MainHero.CurrentSettlement;
				foreach (CharacterObject characterObject in rightSideTransferredPrisonerRoster.Troops)
				{
					if (characterObject.IsHero)
					{
						EnterSettlementAction.ApplyForPrisoner(characterObject.HeroObject, currentSettlement);
					}
				}
				CampaignEventDispatcher.Instance.OnPrisonerDonatedToSettlement(rightParty.MobileParty, rightSideTransferredPrisonerRoster, currentSettlement);
			}
			return true;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000FE20 File Offset: 0x0000E020
		private static bool ManageGarrisonDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
		{
			Settlement currentSettlement = Hero.MainHero.CurrentSettlement;
			for (int i = 0; i < leftMemberRoster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = leftMemberRoster.GetElementCopyAtIndex(i);
				if (elementCopyAtIndex.Character.IsHero)
				{
					EnterSettlementAction.ApplyForCharacterOnly(elementCopyAtIndex.Character.HeroObject, currentSettlement);
				}
			}
			for (int j = 0; j < leftPrisonRoster.Count; j++)
			{
				TroopRosterElement elementCopyAtIndex2 = leftPrisonRoster.GetElementCopyAtIndex(j);
				if (elementCopyAtIndex2.Character.IsHero)
				{
					EnterSettlementAction.ApplyForPrisoner(elementCopyAtIndex2.Character.HeroObject, currentSettlement);
				}
			}
			return true;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000FEAA File Offset: 0x0000E0AA
		private static bool CanTalkToHero(Hero hero, PartyScreenLogic.TroopType type, PartyScreenLogic.PartyRosterSide side, PartyBase LeftOwnerParty, out TextObject cantTalkReason)
		{
			cantTalkReason = TextObject.GetEmpty();
			return side == PartyScreenLogic.PartyRosterSide.Right;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000FEB8 File Offset: 0x0000E0B8
		private static bool ManageTroopsAndPrisonersDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
		{
			return true;
		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000FEBB File Offset: 0x0000E0BB
		private static bool DefaultDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
		{
			PartyScreenHelper.HandleReleasedAndTakenPrisoners(takenPrisonerRoster, releasedPrisonerRoster);
			return true;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000FEC7 File Offset: 0x0000E0C7
		private static void HandleReleasedAndTakenPrisoners(FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster)
		{
			if (!releasedPrisonerRoster.IsEmpty<FlattenedTroopRosterElement>())
			{
				EndCaptivityAction.ApplyByReleasedByChoice(releasedPrisonerRoster);
			}
			if (!takenPrisonerRoster.IsEmpty<FlattenedTroopRosterElement>())
			{
				TakePrisonerAction.ApplyByTakenFromPartyScreen(takenPrisonerRoster);
			}
		}

		// Token: 0x04000005 RID: 5
		private static readonly int _countToAddForEachTroopCheatMode = 10;

		// Token: 0x02000512 RID: 1298
		public enum PartyScreenMode
		{
			// Token: 0x04001632 RID: 5682
			Normal,
			// Token: 0x04001633 RID: 5683
			Shared,
			// Token: 0x04001634 RID: 5684
			Loot,
			// Token: 0x04001635 RID: 5685
			Ransom,
			// Token: 0x04001636 RID: 5686
			PrisonerManage,
			// Token: 0x04001637 RID: 5687
			TroopsManage,
			// Token: 0x04001638 RID: 5688
			QuestTroopManage
		}
	}
}
