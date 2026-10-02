using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Party
{
	// Token: 0x02000314 RID: 788
	public class PartyScreenLogic
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06002D97 RID: 11671 RVA: 0x000BD730 File Offset: 0x000BB930
		// (remove) Token: 0x06002D98 RID: 11672 RVA: 0x000BD768 File Offset: 0x000BB968
		public event PartyScreenLogic.PartyGoldDelegate PartyGoldChange;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06002D99 RID: 11673 RVA: 0x000BD7A0 File Offset: 0x000BB9A0
		// (remove) Token: 0x06002D9A RID: 11674 RVA: 0x000BD7D8 File Offset: 0x000BB9D8
		public event PartyScreenLogic.PartyMoraleDelegate PartyMoraleChange;

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06002D9B RID: 11675 RVA: 0x000BD810 File Offset: 0x000BBA10
		// (remove) Token: 0x06002D9C RID: 11676 RVA: 0x000BD848 File Offset: 0x000BBA48
		public event PartyScreenLogic.PartyInfluenceDelegate PartyInfluenceChange;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06002D9D RID: 11677 RVA: 0x000BD880 File Offset: 0x000BBA80
		// (remove) Token: 0x06002D9E RID: 11678 RVA: 0x000BD8B8 File Offset: 0x000BBAB8
		public event PartyScreenLogic.PartyHorseDelegate PartyHorseChange;

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06002D9F RID: 11679 RVA: 0x000BD8F0 File Offset: 0x000BBAF0
		// (remove) Token: 0x06002DA0 RID: 11680 RVA: 0x000BD928 File Offset: 0x000BBB28
		public event PartyScreenLogic.PresentationUpdate Update;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06002DA1 RID: 11681 RVA: 0x000BD960 File Offset: 0x000BBB60
		// (remove) Token: 0x06002DA2 RID: 11682 RVA: 0x000BD998 File Offset: 0x000BBB98
		public event PartyScreenClosedDelegate PartyScreenClosedEvent;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06002DA3 RID: 11683 RVA: 0x000BD9D0 File Offset: 0x000BBBD0
		// (remove) Token: 0x06002DA4 RID: 11684 RVA: 0x000BDA08 File Offset: 0x000BBC08
		public event PartyScreenLogic.AfterResetDelegate AfterReset;

		// Token: 0x17000B1F RID: 2847
		// (get) Token: 0x06002DA5 RID: 11685 RVA: 0x000BDA3D File Offset: 0x000BBC3D
		// (set) Token: 0x06002DA6 RID: 11686 RVA: 0x000BDA45 File Offset: 0x000BBC45
		public PartyScreenLogic.TroopSortType ActiveOtherPartySortType
		{
			get
			{
				return this._activeOtherPartySortType;
			}
			set
			{
				this._activeOtherPartySortType = value;
			}
		}

		// Token: 0x17000B20 RID: 2848
		// (get) Token: 0x06002DA7 RID: 11687 RVA: 0x000BDA4E File Offset: 0x000BBC4E
		// (set) Token: 0x06002DA8 RID: 11688 RVA: 0x000BDA56 File Offset: 0x000BBC56
		public PartyScreenLogic.TroopSortType ActiveMainPartySortType
		{
			get
			{
				return this._activeMainPartySortType;
			}
			set
			{
				this._activeMainPartySortType = value;
			}
		}

		// Token: 0x17000B21 RID: 2849
		// (get) Token: 0x06002DA9 RID: 11689 RVA: 0x000BDA5F File Offset: 0x000BBC5F
		// (set) Token: 0x06002DAA RID: 11690 RVA: 0x000BDA67 File Offset: 0x000BBC67
		public bool IsOtherPartySortAscending
		{
			get
			{
				return this._isOtherPartySortAscending;
			}
			set
			{
				this._isOtherPartySortAscending = value;
			}
		}

		// Token: 0x17000B22 RID: 2850
		// (get) Token: 0x06002DAB RID: 11691 RVA: 0x000BDA70 File Offset: 0x000BBC70
		// (set) Token: 0x06002DAC RID: 11692 RVA: 0x000BDA78 File Offset: 0x000BBC78
		public bool IsMainPartySortAscending
		{
			get
			{
				return this._isMainPartySortAscending;
			}
			set
			{
				this._isMainPartySortAscending = value;
			}
		}

		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06002DAD RID: 11693 RVA: 0x000BDA81 File Offset: 0x000BBC81
		// (set) Token: 0x06002DAE RID: 11694 RVA: 0x000BDA89 File Offset: 0x000BBC89
		public PartyScreenLogic.TransferState MemberTransferState { get; private set; }

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06002DAF RID: 11695 RVA: 0x000BDA92 File Offset: 0x000BBC92
		// (set) Token: 0x06002DB0 RID: 11696 RVA: 0x000BDA9A File Offset: 0x000BBC9A
		public PartyScreenLogic.TransferState PrisonerTransferState { get; private set; }

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06002DB1 RID: 11697 RVA: 0x000BDAA3 File Offset: 0x000BBCA3
		// (set) Token: 0x06002DB2 RID: 11698 RVA: 0x000BDAAB File Offset: 0x000BBCAB
		public PartyScreenLogic.TransferState AccompanyingTransferState { get; private set; }

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06002DB3 RID: 11699 RVA: 0x000BDAB4 File Offset: 0x000BBCB4
		// (set) Token: 0x06002DB4 RID: 11700 RVA: 0x000BDABC File Offset: 0x000BBCBC
		public TextObject LeftPartyName { get; private set; }

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06002DB5 RID: 11701 RVA: 0x000BDAC5 File Offset: 0x000BBCC5
		// (set) Token: 0x06002DB6 RID: 11702 RVA: 0x000BDACD File Offset: 0x000BBCCD
		public TextObject RightPartyName { get; private set; }

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x000BDAD6 File Offset: 0x000BBCD6
		// (set) Token: 0x06002DB8 RID: 11704 RVA: 0x000BDADE File Offset: 0x000BBCDE
		public TextObject Header { get; private set; }

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06002DB9 RID: 11705 RVA: 0x000BDAE7 File Offset: 0x000BBCE7
		// (set) Token: 0x06002DBA RID: 11706 RVA: 0x000BDAEF File Offset: 0x000BBCEF
		public int LeftPartyMembersSizeLimit { get; private set; }

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06002DBB RID: 11707 RVA: 0x000BDAF8 File Offset: 0x000BBCF8
		// (set) Token: 0x06002DBC RID: 11708 RVA: 0x000BDB00 File Offset: 0x000BBD00
		public int LeftPartyPrisonersSizeLimit { get; private set; }

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06002DBD RID: 11709 RVA: 0x000BDB09 File Offset: 0x000BBD09
		// (set) Token: 0x06002DBE RID: 11710 RVA: 0x000BDB11 File Offset: 0x000BBD11
		public int RightPartyMembersSizeLimit { get; private set; }

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06002DBF RID: 11711 RVA: 0x000BDB1A File Offset: 0x000BBD1A
		// (set) Token: 0x06002DC0 RID: 11712 RVA: 0x000BDB22 File Offset: 0x000BBD22
		public int RightPartyPrisonersSizeLimit { get; private set; }

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06002DC1 RID: 11713 RVA: 0x000BDB2B File Offset: 0x000BBD2B
		// (set) Token: 0x06002DC2 RID: 11714 RVA: 0x000BDB33 File Offset: 0x000BBD33
		public bool DoNotApplyGoldTransactions { get; private set; }

		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06002DC3 RID: 11715 RVA: 0x000BDB3C File Offset: 0x000BBD3C
		// (set) Token: 0x06002DC4 RID: 11716 RVA: 0x000BDB44 File Offset: 0x000BBD44
		public bool ShowProgressBar { get; private set; }

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06002DC5 RID: 11717 RVA: 0x000BDB4D File Offset: 0x000BBD4D
		// (set) Token: 0x06002DC6 RID: 11718 RVA: 0x000BDB55 File Offset: 0x000BBD55
		public string DoneReasonString { get; private set; }

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06002DC7 RID: 11719 RVA: 0x000BDB5E File Offset: 0x000BBD5E
		// (set) Token: 0x06002DC8 RID: 11720 RVA: 0x000BDB66 File Offset: 0x000BBD66
		public bool IsTroopUpgradesDisabled { get; private set; }

		// Token: 0x17000B31 RID: 2865
		// (get) Token: 0x06002DC9 RID: 11721 RVA: 0x000BDB6F File Offset: 0x000BBD6F
		// (set) Token: 0x06002DCA RID: 11722 RVA: 0x000BDB77 File Offset: 0x000BBD77
		public CharacterObject RightPartyLeader { get; private set; }

		// Token: 0x17000B32 RID: 2866
		// (get) Token: 0x06002DCB RID: 11723 RVA: 0x000BDB80 File Offset: 0x000BBD80
		// (set) Token: 0x06002DCC RID: 11724 RVA: 0x000BDB88 File Offset: 0x000BBD88
		public CharacterObject LeftPartyLeader { get; private set; }

		// Token: 0x17000B33 RID: 2867
		// (get) Token: 0x06002DCD RID: 11725 RVA: 0x000BDB91 File Offset: 0x000BBD91
		// (set) Token: 0x06002DCE RID: 11726 RVA: 0x000BDB99 File Offset: 0x000BBD99
		public PartyBase LeftOwnerParty { get; private set; }

		// Token: 0x17000B34 RID: 2868
		// (get) Token: 0x06002DCF RID: 11727 RVA: 0x000BDBA2 File Offset: 0x000BBDA2
		// (set) Token: 0x06002DD0 RID: 11728 RVA: 0x000BDBAA File Offset: 0x000BBDAA
		public PartyBase RightOwnerParty { get; private set; }

		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06002DD1 RID: 11729 RVA: 0x000BDBB3 File Offset: 0x000BBDB3
		// (set) Token: 0x06002DD2 RID: 11730 RVA: 0x000BDBBB File Offset: 0x000BBDBB
		public PartyScreenData CurrentData { get; private set; }

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06002DD3 RID: 11731 RVA: 0x000BDBC4 File Offset: 0x000BBDC4
		// (set) Token: 0x06002DD4 RID: 11732 RVA: 0x000BDBCC File Offset: 0x000BBDCC
		public bool TransferHealthiesGetWoundedsFirst { get; private set; }

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06002DD5 RID: 11733 RVA: 0x000BDBD5 File Offset: 0x000BBDD5
		// (set) Token: 0x06002DD6 RID: 11734 RVA: 0x000BDBDD File Offset: 0x000BBDDD
		public int QuestModeWageDaysMultiplier { get; private set; }

		// Token: 0x17000B38 RID: 2872
		// (get) Token: 0x06002DD7 RID: 11735 RVA: 0x000BDBE6 File Offset: 0x000BBDE6
		// (set) Token: 0x06002DD8 RID: 11736 RVA: 0x000BDBEE File Offset: 0x000BBDEE
		public Game Game
		{
			get
			{
				return this._game;
			}
			set
			{
				this._game = value;
			}
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x000BDBF8 File Offset: 0x000BBDF8
		public PartyScreenLogic()
		{
			this._game = Game.Current;
			this.MemberRosters = new TroopRoster[2];
			this.PrisonerRosters = new TroopRoster[2];
			this.CurrentData = new PartyScreenData();
			this._initialData = new PartyScreenData();
			this._defaultComparers = new Dictionary<PartyScreenLogic.TroopSortType, PartyScreenLogic.TroopComparer>
			{
				{
					PartyScreenLogic.TroopSortType.Custom,
					new PartyScreenLogic.TroopDefaultComparer()
				},
				{
					PartyScreenLogic.TroopSortType.Type,
					new PartyScreenLogic.TroopTypeComparer()
				},
				{
					PartyScreenLogic.TroopSortType.Name,
					new PartyScreenLogic.TroopNameComparer()
				},
				{
					PartyScreenLogic.TroopSortType.Count,
					new PartyScreenLogic.TroopCountComparer()
				},
				{
					PartyScreenLogic.TroopSortType.Tier,
					new PartyScreenLogic.TroopTierComparer()
				}
			};
			this.IsTroopUpgradesDisabled = false;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x000BDC94 File Offset: 0x000BBE94
		public void Initialize(PartyScreenLogicInitializationData initializationData)
		{
			this.MemberRosters[1] = initializationData.RightMemberRoster;
			this.PrisonerRosters[1] = initializationData.RightPrisonerRoster;
			this.MemberRosters[0] = initializationData.LeftMemberRoster;
			this.PrisonerRosters[0] = initializationData.LeftPrisonerRoster;
			Hero rightLeaderHero = initializationData.RightLeaderHero;
			this.RightPartyLeader = ((rightLeaderHero != null) ? rightLeaderHero.CharacterObject : null);
			Hero leftLeaderHero = initializationData.LeftLeaderHero;
			this.LeftPartyLeader = ((leftLeaderHero != null) ? leftLeaderHero.CharacterObject : null);
			this.RightOwnerParty = initializationData.RightOwnerParty;
			this.LeftOwnerParty = initializationData.LeftOwnerParty;
			this.RightPartyName = initializationData.RightPartyName;
			this.RightPartyMembersSizeLimit = initializationData.RightPartyMembersSizeLimit;
			this.RightPartyPrisonersSizeLimit = initializationData.RightPartyPrisonersSizeLimit;
			this.LeftPartyName = initializationData.LeftPartyName;
			this.LeftPartyMembersSizeLimit = initializationData.LeftPartyMembersSizeLimit;
			this.LeftPartyPrisonersSizeLimit = initializationData.LeftPartyPrisonersSizeLimit;
			this.Header = initializationData.Header;
			this.QuestModeWageDaysMultiplier = initializationData.QuestModeWageDaysMultiplier;
			this.TransferHealthiesGetWoundedsFirst = initializationData.TransferHealthiesGetWoundedsFirst;
			this.SetPartyGoldChangeAmount(0);
			this.SetHorseChangeAmount(0);
			this.SetInfluenceChangeAmount(0, 0, 0);
			this.SetMoraleChangeAmount(0f);
			this.CurrentData.BindRostersFrom(this.MemberRosters[1], this.PrisonerRosters[1], this.MemberRosters[0], this.PrisonerRosters[0], this.RightOwnerParty, this.LeftOwnerParty);
			this._initialData.InitializeCopyFrom(initializationData.RightOwnerParty, initializationData.LeftOwnerParty);
			this._initialData.CopyFromPartyAndRoster(this.MemberRosters[1], this.PrisonerRosters[1], this.MemberRosters[0], this.PrisonerRosters[0], this.RightOwnerParty);
			if (initializationData.PartyPresentationDoneButtonDelegate == null)
			{
				Debug.FailedAssert("Done handler is given null for party screen!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Party\\PartyScreenLogic.cs", "Initialize", 242);
				initializationData.PartyPresentationDoneButtonDelegate = new PartyPresentationDoneButtonDelegate(PartyScreenLogic.DefaultDoneHandler);
			}
			this.PartyPresentationDoneButtonDelegate = initializationData.PartyPresentationDoneButtonDelegate;
			this.PartyPresentationDoneButtonConditionDelegate = initializationData.PartyPresentationDoneButtonConditionDelegate;
			this.PartyPresentationCancelButtonActivateDelegate = initializationData.PartyPresentationCancelButtonActivateDelegate;
			this.PartyPresentationCancelButtonDelegate = initializationData.PartyPresentationCancelButtonDelegate;
			this.IsTroopUpgradesDisabled = initializationData.IsTroopUpgradesDisabled || initializationData.RightOwnerParty == null;
			this.MemberTransferState = initializationData.MemberTransferState;
			this.PrisonerTransferState = initializationData.PrisonerTransferState;
			this.AccompanyingTransferState = initializationData.AccompanyingTransferState;
			this.IsTroopTransferableDelegate = initializationData.TroopTransferableDelegate;
			this.CanTalkToHeroDelegate = initializationData.CanTalkToTroopDelegate;
			this.PartyPresentationCancelButtonActivateDelegate = initializationData.PartyPresentationCancelButtonActivateDelegate;
			this.PartyPresentationCancelButtonDelegate = initializationData.PartyPresentationCancelButtonDelegate;
			this.PartyScreenClosedEvent = initializationData.PartyScreenClosedDelegate;
			this.DoNotApplyGoldTransactions = initializationData.DoNotApplyGoldTransactions;
			this.ShowProgressBar = initializationData.ShowProgressBar;
			if (this._partyScreenMode == PartyScreenHelper.PartyScreenMode.QuestTroopManage)
			{
				int num = -this.MemberRosters[0].Sum((TroopRosterElement t) => t.Character.TroopWage * t.Number * this.QuestModeWageDaysMultiplier);
				this._initialData.PartyGoldChangeAmount = num;
				this.SetPartyGoldChangeAmount(num);
			}
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x000BDF5B File Offset: 0x000BC15B
		private void SetPartyGoldChangeAmount(int newTotalAmount)
		{
			this.CurrentData.PartyGoldChangeAmount = newTotalAmount;
			PartyScreenLogic.PartyGoldDelegate partyGoldChange = this.PartyGoldChange;
			if (partyGoldChange == null)
			{
				return;
			}
			partyGoldChange();
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x000BDF79 File Offset: 0x000BC179
		private void SetMoraleChangeAmount(float newAmount)
		{
			this.CurrentData.PartyMoraleChangeAmount = newAmount;
			PartyScreenLogic.PartyMoraleDelegate partyMoraleChange = this.PartyMoraleChange;
			if (partyMoraleChange == null)
			{
				return;
			}
			partyMoraleChange();
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x000BDF97 File Offset: 0x000BC197
		private void SetHorseChangeAmount(int newAmount)
		{
			this.CurrentData.PartyHorseChangeAmount = newAmount;
			PartyScreenLogic.PartyHorseDelegate partyHorseChange = this.PartyHorseChange;
			if (partyHorseChange == null)
			{
				return;
			}
			partyHorseChange();
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x000BDFB5 File Offset: 0x000BC1B5
		private void SetInfluenceChangeAmount(int heroInfluence, int troopInfluence, int prisonerInfluence)
		{
			this.CurrentData.PartyInfluenceChangeAmount = new ValueTuple<int, int, int>(heroInfluence, troopInfluence, prisonerInfluence);
			PartyScreenLogic.PartyInfluenceDelegate partyInfluenceChange = this.PartyInfluenceChange;
			if (partyInfluenceChange == null)
			{
				return;
			}
			partyInfluenceChange();
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x000BDFDC File Offset: 0x000BC1DC
		private void ProcessCommand(PartyScreenLogic.PartyCommand command)
		{
			switch (command.Code)
			{
			case PartyScreenLogic.PartyCommandCode.TransferTroop:
				this.TransferTroop(command, true);
				return;
			case PartyScreenLogic.PartyCommandCode.UpgradeTroop:
				this.UpgradeTroop(command);
				return;
			case PartyScreenLogic.PartyCommandCode.TransferPartyLeaderTroop:
				this.TransferPartyLeaderTroop(command);
				return;
			case PartyScreenLogic.PartyCommandCode.TransferTroopToLeaderSlot:
				this.TransferTroopToLeaderSlot(command);
				return;
			case PartyScreenLogic.PartyCommandCode.ShiftTroop:
				this.ShiftTroop(command);
				return;
			case PartyScreenLogic.PartyCommandCode.RecruitTroop:
				this.RecruitPrisoner(command);
				return;
			case PartyScreenLogic.PartyCommandCode.ExecuteTroop:
				this.ExecuteTroop(command);
				return;
			case PartyScreenLogic.PartyCommandCode.TransferAllTroops:
				this.TransferAllTroops(command);
				return;
			case PartyScreenLogic.PartyCommandCode.SortTroops:
				this.SortTroops(command);
				return;
			default:
				return;
			}
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x000BE063 File Offset: 0x000BC263
		public void AddCommand(PartyScreenLogic.PartyCommand command)
		{
			this.ProcessCommand(command);
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x000BE06C File Offset: 0x000BC26C
		public bool ValidateCommand(PartyScreenLogic.PartyCommand command)
		{
			if (command.Code == PartyScreenLogic.PartyCommandCode.TransferTroop || command.Code == PartyScreenLogic.PartyCommandCode.TransferTroopToLeaderSlot)
			{
				CharacterObject character = command.Character;
				if (character == CharacterObject.PlayerCharacter)
				{
					return false;
				}
				int num;
				if (command.Type == PartyScreenLogic.TroopType.Member)
				{
					num = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(character);
					bool flag = num != -1 && this.MemberRosters[(int)command.RosterSide].GetElementNumber(num) >= command.TotalNumber;
					bool flag2 = command.RosterSide != PartyScreenLogic.PartyRosterSide.Left || command.Index != 0;
					return flag && flag2;
				}
				num = this.PrisonerRosters[(int)command.RosterSide].FindIndexOfTroop(character);
				return num != -1 && this.PrisonerRosters[(int)command.RosterSide].GetElementNumber(num) >= command.TotalNumber;
			}
			else if (command.Code == PartyScreenLogic.PartyCommandCode.ShiftTroop)
			{
				CharacterObject character2 = command.Character;
				if (character2 == this.LeftPartyLeader || character2 == this.RightPartyLeader || ((command.RosterSide != PartyScreenLogic.PartyRosterSide.Left || (this.LeftPartyLeader != null && command.Index == 0)) && (command.RosterSide != PartyScreenLogic.PartyRosterSide.Right || (this.RightPartyLeader != null && command.Index == 0))))
				{
					return false;
				}
				int num2;
				if (command.Type == PartyScreenLogic.TroopType.Member)
				{
					num2 = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(character2);
					return num2 != -1 && num2 != command.Index;
				}
				num2 = this.PrisonerRosters[(int)command.RosterSide].FindIndexOfTroop(character2);
				return num2 != -1 && num2 != command.Index;
			}
			else
			{
				if (command.Code == PartyScreenLogic.PartyCommandCode.TransferPartyLeaderTroop)
				{
					CharacterObject character3 = command.Character;
					BasicCharacterObject playerTroop = this._game.PlayerTroop;
					return false;
				}
				if (command.Code == PartyScreenLogic.PartyCommandCode.UpgradeTroop)
				{
					CharacterObject character4 = command.Character;
					int num3 = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(character4);
					if (num3 == -1 || this.MemberRosters[(int)command.RosterSide].GetElementNumber(num3) < command.TotalNumber || character4.UpgradeTargets.Length == 0)
					{
						return false;
					}
					if (command.UpgradeTarget >= character4.UpgradeTargets.Length)
					{
						MBInformationManager.AddQuickInformation(new TextObject("{=kaQ7DsW3}Character does not have upgrade target.", null), 0, null, null, "");
						return false;
					}
					CharacterObject characterObject = character4.UpgradeTargets[command.UpgradeTarget];
					int upgradeXpCost = character4.GetUpgradeXpCost(PartyBase.MainParty, command.UpgradeTarget);
					int upgradeGoldCost = character4.GetUpgradeGoldCost(PartyBase.MainParty, command.UpgradeTarget);
					if (this.MemberRosters[(int)command.RosterSide].GetElementXp(num3) < upgradeXpCost * command.TotalNumber)
					{
						MBInformationManager.AddQuickInformation(new TextObject("{=m1bIfPf1}Character does not have enough experience for upgrade.", null), 0, null, null, "");
						return false;
					}
					CharacterObject characterObject2 = ((command.RosterSide == PartyScreenLogic.PartyRosterSide.Left) ? this.LeftPartyLeader : this.RightPartyLeader);
					int? num4 = ((characterObject2 != null) ? new int?(characterObject2.HeroObject.Gold) : null) + this.CurrentData.PartyGoldChangeAmount;
					int num5 = upgradeGoldCost * command.TotalNumber;
					if (!((num4.GetValueOrDefault() >= num5) & (num4 != null)))
					{
						MBTextManager.SetTextVariable("VALUE", upgradeGoldCost);
						MBInformationManager.AddQuickInformation(GameTexts.FindText("str_gold_needed_for_upgrade", null), 0, null, null, "");
						return false;
					}
					if (characterObject.UpgradeRequiresItemFromCategory == null)
					{
						return true;
					}
					foreach (ItemRosterElement itemRosterElement in this.RightOwnerParty.ItemRoster)
					{
						if (itemRosterElement.EquipmentElement.Item.ItemCategory == characterObject.UpgradeRequiresItemFromCategory)
						{
							return true;
						}
					}
					MBTextManager.SetTextVariable("REQUIRED_ITEM", characterObject.UpgradeRequiresItemFromCategory.GetName(), false);
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_item_needed_for_upgrade", null), 0, null, null, "");
					return false;
				}
				else
				{
					if (command.Code == PartyScreenLogic.PartyCommandCode.RecruitTroop)
					{
						return this.IsPrisonerRecruitable(command.Type, command.Character, command.RosterSide);
					}
					if (command.Code == PartyScreenLogic.PartyCommandCode.ExecuteTroop)
					{
						return this.IsExecutable(command.Type, command.Character, command.RosterSide);
					}
					if (command.Code == PartyScreenLogic.PartyCommandCode.TransferAllTroops)
					{
						return this.GetRoster(command.RosterSide, command.Type).Count != 0;
					}
					if (command.Code == PartyScreenLogic.PartyCommandCode.SortTroops)
					{
						return this.GetActiveSortTypeForSide(command.RosterSide) != command.SortType || this.GetIsAscendingSortForSide(command.RosterSide) != command.IsSortAscending;
					}
					throw new MBUnknownTypeException("Unknown command type in ValidateCommand.");
				}
			}
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x000BE520 File Offset: 0x000BC720
		private void OnReset(bool fromCancel)
		{
			PartyScreenLogic.AfterResetDelegate afterReset = this.AfterReset;
			if (afterReset == null)
			{
				return;
			}
			afterReset(this, fromCancel);
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x000BE534 File Offset: 0x000BC734
		protected void TransferTroopToLeaderSlot(PartyScreenLogic.PartyCommand command)
		{
			bool flag = false;
			if (this.ValidateCommand(command))
			{
				CharacterObject character = command.Character;
				if (command.Type == PartyScreenLogic.TroopType.Member)
				{
					int num = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(character);
					TroopRosterElement elementCopyAtIndex = this.MemberRosters[(int)command.RosterSide].GetElementCopyAtIndex(num);
					int num2 = command.TotalNumber * (elementCopyAtIndex.Xp / elementCopyAtIndex.Number);
					this.MemberRosters[(int)command.RosterSide].AddToCounts(character, -command.TotalNumber, false, -command.WoundedNumber, 0, true, num);
					this.MemberRosters[(int)(PartyScreenLogic.PartyRosterSide.Right - command.RosterSide)].AddToCounts(character, command.TotalNumber, false, command.WoundedNumber, 0, true, 0);
					if (elementCopyAtIndex.Number != command.TotalNumber)
					{
						this.MemberRosters[(int)command.RosterSide].AddXpToTroop(character, -num2);
					}
					this.MemberRosters[(int)(PartyScreenLogic.PartyRosterSide.Right - command.RosterSide)].AddXpToTroop(character, num2);
				}
				flag = true;
			}
			if (flag)
			{
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update == null)
				{
					return;
				}
				update(command);
			}
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x000BE654 File Offset: 0x000BC854
		protected void TransferTroop(PartyScreenLogic.PartyCommand command, bool invokeUpdate)
		{
			bool flag = false;
			if (this.ValidateCommand(command))
			{
				CharacterObject troop = command.Character;
				if (command.Type == PartyScreenLogic.TroopType.Member)
				{
					TroopRoster troopRoster = this.MemberRosters[(int)command.RosterSide];
					TroopRoster troopRoster2 = this.MemberRosters[(int)(PartyScreenLogic.PartyRosterSide.Right - command.RosterSide)];
					int num = troopRoster.FindIndexOfTroop(troop);
					TroopRosterElement elementCopyAtIndex = troopRoster.GetElementCopyAtIndex(num);
					int num2 = ((troop.UpgradeTargets.Length != 0) ? troop.UpgradeTargets.Max<CharacterObject>((CharacterObject x) => Campaign.Current.Models.PartyTroopUpgradeModel.GetXpCostForUpgrade(PartyBase.MainParty, troop, x)) : 0);
					int num4;
					if (command.RosterSide == PartyScreenLogic.PartyRosterSide.Right)
					{
						int num3 = (elementCopyAtIndex.Number - command.TotalNumber) * num2;
						num4 = ((elementCopyAtIndex.Xp >= num3 && num3 >= 0) ? (elementCopyAtIndex.Xp - num3) : 0);
					}
					else
					{
						int num5 = command.TotalNumber * num2;
						num4 = ((elementCopyAtIndex.Xp > num5 && num5 >= 0) ? num5 : elementCopyAtIndex.Xp);
						troopRoster.AddXpToTroop(troop, -num4);
					}
					troopRoster.AddToCounts(troop, -command.TotalNumber, false, -command.WoundedNumber, 0, false, -1);
					int num6 = command.Index;
					if (num6 == troopRoster2.Count && troopRoster2.Contains(troop))
					{
						num6 = troopRoster2.Count - 1;
					}
					troopRoster2.AddToCounts(troop, command.TotalNumber, false, command.WoundedNumber, 0, false, num6);
					troopRoster2.AddXpToTroop(troop, num4);
				}
				else
				{
					TroopRoster troopRoster3 = this.PrisonerRosters[(int)command.RosterSide];
					TroopRoster troopRoster4 = this.PrisonerRosters[(int)(PartyScreenLogic.PartyRosterSide.Right - command.RosterSide)];
					int num7 = troopRoster3.FindIndexOfTroop(troop);
					TroopRosterElement elementCopyAtIndex2 = troopRoster3.GetElementCopyAtIndex(num7);
					int conformityNeededToRecruitPrisoner = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetConformityNeededToRecruitPrisoner(elementCopyAtIndex2.Character);
					int num9;
					if (command.RosterSide == PartyScreenLogic.PartyRosterSide.Right)
					{
						this.UpdatePrisonerTransferHistory(troop, -command.TotalNumber);
						int num8 = (elementCopyAtIndex2.Number - command.TotalNumber) * conformityNeededToRecruitPrisoner;
						num9 = ((elementCopyAtIndex2.Xp >= num8 && num8 >= 0) ? (elementCopyAtIndex2.Xp - num8) : 0);
					}
					else
					{
						this.UpdatePrisonerTransferHistory(troop, command.TotalNumber);
						int num10 = command.TotalNumber * conformityNeededToRecruitPrisoner;
						num9 = ((elementCopyAtIndex2.Xp > num10 && num10 >= 0) ? num10 : elementCopyAtIndex2.Xp);
						troopRoster3.AddXpToTroop(troop, -num9);
					}
					troopRoster3.AddToCounts(troop, -command.TotalNumber, false, -command.WoundedNumber, 0, false, -1);
					int num11 = command.Index;
					if (num11 == troopRoster4.Count && troopRoster4.Contains(troop))
					{
						num11 = troopRoster4.Count - 1;
					}
					troopRoster4.AddToCounts(troop, command.TotalNumber, false, command.WoundedNumber, 0, false, num11);
					troopRoster4.AddXpToTroop(troop, num9);
					if (this.CurrentData.RightRecruitableData.ContainsKey(troop))
					{
						this.CurrentData.RightRecruitableData[troop] = MathF.Max(MathF.Min(this.CurrentData.RightRecruitableData[troop], this.PrisonerRosters[1].GetElementNumber(troop)), Campaign.Current.Models.PrisonerRecruitmentCalculationModel.CalculateRecruitableNumber(PartyBase.MainParty, troop));
					}
				}
				flag = true;
			}
			if (flag)
			{
				if (this.PrisonerTransferState == PartyScreenLogic.TransferState.TransferableWithTrade && command.Type == PartyScreenLogic.TroopType.Prisoner)
				{
					int num12 = ((command.RosterSide == PartyScreenLogic.PartyRosterSide.Right) ? 1 : (-1));
					this.SetPartyGoldChangeAmount(this.CurrentData.PartyGoldChangeAmount + Campaign.Current.Models.RansomValueCalculationModel.PrisonerRansomValue(command.Character, Hero.MainHero) * command.TotalNumber * num12);
				}
				if (this._partyScreenMode == PartyScreenHelper.PartyScreenMode.QuestTroopManage)
				{
					int num13 = ((command.RosterSide == PartyScreenLogic.PartyRosterSide.Right) ? (-1) : 1);
					this.SetPartyGoldChangeAmount(this.CurrentData.PartyGoldChangeAmount + command.Character.TroopWage * command.TotalNumber * this.QuestModeWageDaysMultiplier * num13);
				}
				PartyState activePartyState = PartyScreenHelper.GetActivePartyState();
				if (activePartyState != null && activePartyState.IsDonating)
				{
					Settlement currentSettlement = Hero.MainHero.CurrentSettlement;
					float num14 = 0f;
					float num15 = 0f;
					float num16 = 0f;
					foreach (TroopTradeDifference troopTradeDifference in this.CurrentData.GetTroopTradeDifferencesFromTo(this._initialData, PartyScreenLogic.PartyRosterSide.Left))
					{
						int differenceCount = troopTradeDifference.DifferenceCount;
						if (differenceCount > 0)
						{
							if (!troopTradeDifference.IsPrisoner)
							{
								num15 += (float)differenceCount * Campaign.Current.Models.PrisonerDonationModel.CalculateInfluenceGainAfterTroopDonation(PartyBase.MainParty, troopTradeDifference.Troop, currentSettlement);
							}
							else if (troopTradeDifference.Troop.IsHero)
							{
								num14 += Campaign.Current.Models.PrisonerDonationModel.CalculateInfluenceGainAfterPrisonerDonation(PartyBase.MainParty, troopTradeDifference.Troop, currentSettlement);
							}
							else
							{
								num16 += (float)differenceCount * Campaign.Current.Models.PrisonerDonationModel.CalculateInfluenceGainAfterPrisonerDonation(PartyBase.MainParty, troopTradeDifference.Troop, currentSettlement);
							}
						}
					}
					this.SetInfluenceChangeAmount((int)num14, (int)num15, (int)num16);
				}
				if (invokeUpdate)
				{
					PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
					if (updateDelegate != null)
					{
						updateDelegate(command);
					}
					PartyScreenLogic.PresentationUpdate update = this.Update;
					if (update == null)
					{
						return;
					}
					update(command);
				}
			}
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x000BEBE4 File Offset: 0x000BCDE4
		protected void ShiftTroop(PartyScreenLogic.PartyCommand command)
		{
			bool flag = false;
			if (this.ValidateCommand(command))
			{
				CharacterObject character = command.Character;
				if (command.Type == PartyScreenLogic.TroopType.Member)
				{
					int num = this.MemberRosters[(int)command.RosterSide].FindIndexOfTroop(character);
					int num2 = ((num < command.Index) ? (command.Index - 1) : command.Index);
					this.MemberRosters[(int)command.RosterSide].ShiftTroopToIndex(num, num2);
				}
				else
				{
					int num3 = this.PrisonerRosters[(int)command.RosterSide].FindIndexOfTroop(character);
					this.PrisonerRosters[(int)command.RosterSide].GetElementCopyAtIndex(num3);
					int num4 = ((num3 < command.Index) ? (command.Index - 1) : command.Index);
					this.PrisonerRosters[(int)command.RosterSide].ShiftTroopToIndex(num3, num4);
				}
				flag = true;
			}
			if (flag)
			{
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update == null)
				{
					return;
				}
				update(command);
			}
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x000BECD7 File Offset: 0x000BCED7
		protected void TransferPartyLeaderTroop(PartyScreenLogic.PartyCommand command)
		{
			if (this.ValidateCommand(command))
			{
				PartyBase partyBase = ((command.RosterSide == PartyScreenLogic.PartyRosterSide.Left) ? this.LeftOwnerParty : this.RightOwnerParty);
			}
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x000BECFC File Offset: 0x000BCEFC
		protected void UpgradeTroop(PartyScreenLogic.PartyCommand command)
		{
			if (this.ValidateCommand(command))
			{
				CharacterObject character = command.Character;
				CharacterObject characterObject = character.UpgradeTargets[command.UpgradeTarget];
				TroopRoster roster = this.GetRoster(command.RosterSide, command.Type);
				int num = roster.FindIndexOfTroop(character);
				int num2 = character.GetUpgradeXpCost(PartyBase.MainParty, command.UpgradeTarget) * command.TotalNumber;
				roster.SetElementXp(num, roster.GetElementXp(num) - num2);
				List<ValueTuple<EquipmentElement, int>> list = null;
				this.SetPartyGoldChangeAmount(this.CurrentData.PartyGoldChangeAmount - character.GetUpgradeGoldCost(PartyBase.MainParty, command.UpgradeTarget) * command.TotalNumber);
				if (characterObject.UpgradeRequiresItemFromCategory != null)
				{
					list = this.RemoveItemFromItemRoster(characterObject.UpgradeRequiresItemFromCategory, command.TotalNumber);
				}
				int num3 = 0;
				foreach (TroopRosterElement troopRosterElement in roster.GetTroopRoster())
				{
					if (troopRosterElement.Character == character && command.TotalNumber > troopRosterElement.Number - troopRosterElement.WoundedNumber)
					{
						num3 = command.TotalNumber - (troopRosterElement.Number - troopRosterElement.WoundedNumber);
					}
				}
				roster.AddToCounts(character, -command.TotalNumber, false, -num3, 0, true, -1);
				roster.AddToCounts(characterObject, command.TotalNumber, false, num3, 0, true, command.Index);
				this.AddUpgradeToHistory(character, characterObject, command.TotalNumber);
				this.AddUsedHorsesToHistory(list);
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate == null)
				{
					return;
				}
				updateDelegate(command);
			}
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x000BEE8C File Offset: 0x000BD08C
		protected void RecruitPrisoner(PartyScreenLogic.PartyCommand command)
		{
			bool flag = false;
			if (this.ValidateCommand(command))
			{
				CharacterObject character = command.Character;
				TroopRoster troopRoster = this.PrisonerRosters[(int)command.RosterSide];
				int num = MathF.Min(this.CurrentData.RightRecruitableData[character], command.TotalNumber);
				if (num > 0)
				{
					Dictionary<CharacterObject, int> rightRecruitableData = this.CurrentData.RightRecruitableData;
					CharacterObject characterObject = character;
					rightRecruitableData[characterObject] -= num;
					int conformityNeededToRecruitPrisoner = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetConformityNeededToRecruitPrisoner(character);
					troopRoster.AddXpToTroop(character, -conformityNeededToRecruitPrisoner * num);
					troopRoster.AddToCounts(character, -num, false, 0, 0, true, -1);
					this.MemberRosters[(int)command.RosterSide].AddToCounts(command.Character, num, false, 0, 0, true, command.Index);
					this.AddRecruitToHistory(character, num);
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			if (flag)
			{
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update == null)
				{
					return;
				}
				update(command);
			}
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x000BEF90 File Offset: 0x000BD190
		protected void ExecuteTroop(PartyScreenLogic.PartyCommand command)
		{
			bool flag = false;
			if (this.ValidateCommand(command))
			{
				CharacterObject character = command.Character;
				this.PrisonerRosters[(int)command.RosterSide].AddToCounts(character, -1, false, 0, 0, true, -1);
				KillCharacterAction.ApplyByExecution(character.HeroObject, Hero.MainHero, true, false);
				flag = true;
			}
			if (flag)
			{
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update != null)
				{
					update(command);
				}
				if (command.RosterSide == PartyScreenLogic.PartyRosterSide.Left)
				{
					this._initialData.LeftPrisonerRoster.AddToCounts(command.Character, -1, false, 0, 0, true, -1);
					return;
				}
				if (PartyScreenLogic.PartyRosterSide.Right == command.RosterSide)
				{
					this._initialData.RightPrisonerRoster.AddToCounts(command.Character, -1, false, 0, 0, true, -1);
				}
			}
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x000BF050 File Offset: 0x000BD250
		protected void TransferAllTroops(PartyScreenLogic.PartyCommand command)
		{
			if (this.ValidateCommand(command))
			{
				PartyScreenLogic.PartyRosterSide partyRosterSide = PartyScreenLogic.PartyRosterSide.Right - command.RosterSide;
				TroopRoster roster = this.GetRoster(command.RosterSide, command.Type);
				List<TroopRosterElement> listFromRoster = this.GetListFromRoster(roster);
				int num = -1;
				if (command.RosterSide == PartyScreenLogic.PartyRosterSide.Right)
				{
					if (command.Type == PartyScreenLogic.TroopType.Prisoner)
					{
						num = this.LeftPartyPrisonersSizeLimit - this.PrisonerRosters[0].TotalManCount;
					}
					else
					{
						num = this.LeftPartyMembersSizeLimit - this.MemberRosters[0].TotalManCount;
					}
				}
				else if (command.RosterSide == PartyScreenLogic.PartyRosterSide.Left)
				{
					if (command.Type == PartyScreenLogic.TroopType.Prisoner)
					{
						num = this.RightPartyPrisonersSizeLimit - this.PrisonerRosters[1].TotalManCount;
					}
					else
					{
						num = this.RightPartyMembersSizeLimit - this.MemberRosters[1].TotalManCount;
					}
				}
				if (num <= 0)
				{
					num = listFromRoster.Sum<TroopRosterElement>((TroopRosterElement x) => x.Number);
				}
				IEnumerable<string> enumerable = ((command.Type == PartyScreenLogic.TroopType.Member) ? Campaign.Current.GetCampaignBehavior<IViewDataTracker>().GetPartyTroopLocks() : Campaign.Current.GetCampaignBehavior<IViewDataTracker>().GetPartyPrisonerLocks());
				int num2 = 0;
				while (num2 < listFromRoster.Count && num > 0)
				{
					TroopRosterElement troopRosterElement = listFromRoster[num2];
					if ((command.RosterSide != PartyScreenLogic.PartyRosterSide.Right || !enumerable.Contains(troopRosterElement.Character.StringId)) && this.IsTroopTransferable(command.Type, troopRosterElement.Character, (int)command.RosterSide))
					{
						PartyScreenLogic.PartyCommand partyCommand = new PartyScreenLogic.PartyCommand();
						int num3 = MBMath.ClampInt(troopRosterElement.Number, 0, num);
						partyCommand.FillForTransferTroop(command.RosterSide, command.Type, troopRosterElement.Character, num3, troopRosterElement.WoundedNumber, -1);
						this.TransferTroop(partyCommand, false);
						num -= num3;
					}
					num2++;
				}
				PartyScreenLogic.TroopSortType activeSortTypeForSide = this.GetActiveSortTypeForSide(partyRosterSide);
				if (activeSortTypeForSide != PartyScreenLogic.TroopSortType.Custom)
				{
					TroopRoster roster2 = this.GetRoster(partyRosterSide, PartyScreenLogic.TroopType.Member);
					TroopRoster roster3 = this.GetRoster(partyRosterSide, PartyScreenLogic.TroopType.Prisoner);
					this.SortRoster(roster2, activeSortTypeForSide);
					this.SortRoster(roster3, activeSortTypeForSide);
				}
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update == null)
				{
					return;
				}
				update(command);
			}
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000BF268 File Offset: 0x000BD468
		protected void SortTroops(PartyScreenLogic.PartyCommand command)
		{
			if (this.ValidateCommand(command))
			{
				this.SetActiveSortTypeForSide(command.RosterSide, command.SortType);
				this.SetIsAscendingForSide(command.RosterSide, command.IsSortAscending);
				this.UpdateComparersAscendingOrder(command.IsSortAscending);
				if (command.SortType != PartyScreenLogic.TroopSortType.Custom)
				{
					TroopRoster roster = this.GetRoster(command.RosterSide, PartyScreenLogic.TroopType.Member);
					TroopRoster roster2 = this.GetRoster(command.RosterSide, PartyScreenLogic.TroopType.Prisoner);
					this.SortRoster(roster, command.SortType);
					this.SortRoster(roster2, command.SortType);
				}
				PartyScreenLogic.PresentationUpdate updateDelegate = this.UpdateDelegate;
				if (updateDelegate != null)
				{
					updateDelegate(command);
				}
				PartyScreenLogic.PresentationUpdate update = this.Update;
				if (update == null)
				{
					return;
				}
				update(command);
			}
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x000BF314 File Offset: 0x000BD514
		public int GetIndexToInsertTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, TroopRosterElement troop)
		{
			PartyScreenLogic.TroopSortType activeSortTypeForSide = this.GetActiveSortTypeForSide(side);
			if (activeSortTypeForSide != PartyScreenLogic.TroopSortType.Custom)
			{
				return -1;
			}
			PartyScreenLogic.TroopComparer comparer = this.GetComparer(activeSortTypeForSide);
			TroopRoster roster = this.GetRoster(side, type);
			for (int i = 0; i < roster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = roster.GetElementCopyAtIndex(i);
				if (!elementCopyAtIndex.Character.IsHero)
				{
					if (elementCopyAtIndex.Character.StringId == troop.Character.StringId)
					{
						return -1;
					}
					if (comparer.Compare(elementCopyAtIndex, troop) < 0)
					{
						return i;
					}
				}
			}
			return -1;
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x000BF396 File Offset: 0x000BD596
		public PartyScreenLogic.TroopSortType GetActiveSortTypeForSide(PartyScreenLogic.PartyRosterSide side)
		{
			if (side == PartyScreenLogic.PartyRosterSide.Left)
			{
				return this.ActiveOtherPartySortType;
			}
			if (side == PartyScreenLogic.PartyRosterSide.Right)
			{
				return this.ActiveMainPartySortType;
			}
			return PartyScreenLogic.TroopSortType.Invalid;
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x000BF3AE File Offset: 0x000BD5AE
		private void SetActiveSortTypeForSide(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopSortType sortType)
		{
			if (side == PartyScreenLogic.PartyRosterSide.Left)
			{
				this.ActiveOtherPartySortType = sortType;
				return;
			}
			if (side == PartyScreenLogic.PartyRosterSide.Right)
			{
				this.ActiveMainPartySortType = sortType;
			}
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000BF3C6 File Offset: 0x000BD5C6
		public bool GetIsAscendingSortForSide(PartyScreenLogic.PartyRosterSide side)
		{
			if (side == PartyScreenLogic.PartyRosterSide.Left)
			{
				return this.IsOtherPartySortAscending;
			}
			return side == PartyScreenLogic.PartyRosterSide.Right && this.IsMainPartySortAscending;
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000BF3DE File Offset: 0x000BD5DE
		private void SetIsAscendingForSide(PartyScreenLogic.PartyRosterSide side, bool isAscending)
		{
			if (side == PartyScreenLogic.PartyRosterSide.Left)
			{
				this.IsOtherPartySortAscending = isAscending;
				return;
			}
			if (side == PartyScreenLogic.PartyRosterSide.Right)
			{
				this.IsMainPartySortAscending = isAscending;
			}
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x000BF3F8 File Offset: 0x000BD5F8
		private List<TroopRosterElement> GetListFromRoster(TroopRoster roster)
		{
			List<TroopRosterElement> list = new List<TroopRosterElement>();
			for (int i = 0; i < roster.Count; i++)
			{
				list.Add(roster.GetElementCopyAtIndex(i));
			}
			return list;
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x000BF42C File Offset: 0x000BD62C
		private void SyncRosterWithList(TroopRoster roster, List<TroopRosterElement> list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				TroopRosterElement troopRosterElement = list[i];
				int num = roster.FindIndexOfTroop(troopRosterElement.Character);
				roster.SwapTroopsAtIndices(num, i);
			}
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x000BF468 File Offset: 0x000BD668
		[Conditional("DEBUG")]
		private void EnsureRosterIsSyncedWithList(TroopRoster roster, List<TroopRosterElement> list)
		{
			if (roster.Count != list.Count)
			{
				Debug.FailedAssert("Roster count is not synced with the list count", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Party\\PartyScreenLogic.cs", "EnsureRosterIsSyncedWithList", 1081);
				return;
			}
			for (int i = 0; i < roster.Count; i++)
			{
				if (roster.GetCharacterAtIndex(i).StringId != list[i].Character.StringId)
				{
					Debug.FailedAssert("Roster is not synced with the list at index: " + i, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Party\\PartyScreenLogic.cs", "EnsureRosterIsSyncedWithList", 1091);
					return;
				}
			}
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x000BF4F8 File Offset: 0x000BD6F8
		private void SortRoster(TroopRoster originalRoster, PartyScreenLogic.TroopSortType sortType)
		{
			PartyScreenLogic.TroopComparer troopComparer = this._defaultComparers[sortType];
			if (!this.IsRosterOrdered(originalRoster, troopComparer))
			{
				List<TroopRosterElement> listFromRoster = this.GetListFromRoster(originalRoster);
				listFromRoster.Sort(this._defaultComparers[sortType]);
				this.SyncRosterWithList(originalRoster, listFromRoster);
			}
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x000BF540 File Offset: 0x000BD740
		private bool IsRosterOrdered(TroopRoster roster, PartyScreenLogic.TroopComparer comparer)
		{
			for (int i = 1; i < roster.Count; i++)
			{
				TroopRosterElement elementCopyAtIndex = roster.GetElementCopyAtIndex(i - 1);
				TroopRosterElement elementCopyAtIndex2 = roster.GetElementCopyAtIndex(i);
				if (comparer.Compare(elementCopyAtIndex, elementCopyAtIndex2) >= 1)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x000BF580 File Offset: 0x000BD780
		public bool IsDoneActive()
		{
			object obj = Hero.MainHero.Gold < -this.CurrentData.PartyGoldChangeAmount && this.CurrentData.PartyGoldChangeAmount < 0;
			PartyPresentationDoneButtonConditionDelegate partyPresentationDoneButtonConditionDelegate = this.PartyPresentationDoneButtonConditionDelegate;
			Tuple<bool, TextObject> tuple = ((partyPresentationDoneButtonConditionDelegate != null) ? partyPresentationDoneButtonConditionDelegate(this.MemberRosters[0], this.PrisonerRosters[0], this.MemberRosters[1], this.PrisonerRosters[1], this.LeftPartyMembersSizeLimit, 0) : null);
			bool flag = this.PartyPresentationDoneButtonConditionDelegate == null || (tuple != null && tuple.Item1);
			this.DoneReasonString = null;
			object obj2 = obj;
			if (obj2 != null)
			{
				this.DoneReasonString = GameTexts.FindText("str_inventory_popup_player_not_enough_gold", null).ToString();
			}
			else
			{
				string text;
				if (tuple == null)
				{
					text = null;
				}
				else
				{
					TextObject item = tuple.Item2;
					text = ((item != null) ? item.ToString() : null);
				}
				this.DoneReasonString = text ?? string.Empty;
			}
			return obj2 == 0 && flag;
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x000BF656 File Offset: 0x000BD856
		public bool IsCancelActive()
		{
			return this.PartyPresentationCancelButtonActivateDelegate == null || this.PartyPresentationCancelButtonActivateDelegate();
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x000BF670 File Offset: 0x000BD870
		public bool DoneLogic(bool isForced)
		{
			if (Hero.MainHero.Gold < -this.CurrentData.PartyGoldChangeAmount && this.CurrentData.PartyGoldChangeAmount < 0)
			{
				MBInformationManager.AddQuickInformation(GameTexts.FindText("str_inventory_popup_player_not_enough_gold", null), 0, null, null, "");
				return false;
			}
			FlattenedTroopRoster flattenedTroopRoster = new FlattenedTroopRoster(4);
			FlattenedTroopRoster flattenedTroopRoster2 = new FlattenedTroopRoster(4);
			foreach (Tuple<CharacterObject, int> tuple in this.CurrentData.TransferredPrisonersHistory)
			{
				int num = MathF.Abs(tuple.Item2);
				if (tuple.Item2 < 0)
				{
					flattenedTroopRoster.Add(tuple.Item1, num, 0);
				}
				else if (tuple.Item2 > 0)
				{
					flattenedTroopRoster2.Add(tuple.Item1, num, 0);
				}
			}
			if (Settlement.CurrentSettlement != null && !flattenedTroopRoster2.IsEmpty<FlattenedTroopRosterElement>())
			{
				CampaignEventDispatcher.Instance.OnPrisonersChangeInSettlement(Settlement.CurrentSettlement, flattenedTroopRoster2, null, true);
			}
			bool flag = this.PartyPresentationDoneButtonDelegate(this.MemberRosters[0], this.PrisonerRosters[0], this.MemberRosters[1], this.PrisonerRosters[1], flattenedTroopRoster2, flattenedTroopRoster, isForced, this.LeftOwnerParty, this.RightOwnerParty);
			if (flag)
			{
				if (!this.DoNotApplyGoldTransactions)
				{
					GiveGoldAction.ApplyBetweenCharacters(null, Hero.MainHero, this.CurrentData.PartyGoldChangeAmount, false);
				}
				if (this.CurrentData.PartyInfluenceChangeAmount.Item2 != 0)
				{
					GainKingdomInfluenceAction.ApplyForLeavingTroopToGarrison(Hero.MainHero, (float)this.CurrentData.PartyInfluenceChangeAmount.Item2);
				}
				this.FireCampaignRelatedEvents();
				this.SetPartyGoldChangeAmount(0);
				this.SetHorseChangeAmount(0);
				this.SetInfluenceChangeAmount(0, 0, 0);
				this.SetMoraleChangeAmount(0f);
				this.CurrentData.UpgradedTroopsHistory = new List<Tuple<CharacterObject, CharacterObject, int>>();
				this.CurrentData.TransferredPrisonersHistory = new List<Tuple<CharacterObject, int>>();
				this.CurrentData.RecruitedPrisonersHistory = new List<Tuple<CharacterObject, int>>();
				this.CurrentData.UsedUpgradeHorsesHistory = new List<Tuple<EquipmentElement, int>>();
				this._initialData.CopyFromScreenData(this.CurrentData);
			}
			return flag;
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x000BF874 File Offset: 0x000BDA74
		public void OnPartyScreenClosed(bool fromCancel)
		{
			if (fromCancel)
			{
				PartyPresentationCancelButtonDelegate partyPresentationCancelButtonDelegate = this.PartyPresentationCancelButtonDelegate;
				if (partyPresentationCancelButtonDelegate != null)
				{
					partyPresentationCancelButtonDelegate();
				}
			}
			PartyScreenClosedDelegate partyScreenClosedEvent = this.PartyScreenClosedEvent;
			if (partyScreenClosedEvent == null)
			{
				return;
			}
			partyScreenClosedEvent(this.LeftOwnerParty, this.MemberRosters[0], this.PrisonerRosters[0], this.RightOwnerParty, this.MemberRosters[1], this.PrisonerRosters[1], fromCancel);
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x000BF8D4 File Offset: 0x000BDAD4
		private void UpdateComparersAscendingOrder(bool isAscending)
		{
			foreach (KeyValuePair<PartyScreenLogic.TroopSortType, PartyScreenLogic.TroopComparer> keyValuePair in this._defaultComparers)
			{
				keyValuePair.Value.SetIsAscending(isAscending);
			}
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x000BF930 File Offset: 0x000BDB30
		private void FireCampaignRelatedEvents()
		{
			foreach (Tuple<CharacterObject, CharacterObject, int> tuple in this.CurrentData.UpgradedTroopsHistory)
			{
				CampaignEventDispatcher.Instance.OnPlayerUpgradedTroops(tuple.Item1, tuple.Item2, tuple.Item3);
			}
			FlattenedTroopRoster flattenedTroopRoster = new FlattenedTroopRoster(4);
			foreach (Tuple<CharacterObject, int> tuple2 in this.CurrentData.RecruitedPrisonersHistory)
			{
				flattenedTroopRoster.Add(tuple2.Item1, tuple2.Item2, 0);
			}
			if (!flattenedTroopRoster.IsEmpty<FlattenedTroopRosterElement>())
			{
				CampaignEventDispatcher.Instance.OnMainPartyPrisonerRecruited(flattenedTroopRoster);
			}
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x000BFA10 File Offset: 0x000BDC10
		public bool IsTroopTransferable(PartyScreenLogic.TroopType troopType, CharacterObject character, int side)
		{
			return this.IsTroopRosterTransferable(troopType) && !character.IsNotTransferableInPartyScreen && character != CharacterObject.PlayerCharacter && (this.IsTroopTransferableDelegate == null || this.IsTroopTransferableDelegate(character, troopType, (PartyScreenLogic.PartyRosterSide)side, this.LeftOwnerParty));
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x000BFA4C File Offset: 0x000BDC4C
		public bool IsTroopRosterTransferable(PartyScreenLogic.TroopType troopType)
		{
			if (troopType == PartyScreenLogic.TroopType.Prisoner)
			{
				return this.PrisonerTransferState == PartyScreenLogic.TransferState.Transferable || this.PrisonerTransferState == PartyScreenLogic.TransferState.TransferableWithTrade;
			}
			return troopType == PartyScreenLogic.TroopType.Member && (this.MemberTransferState == PartyScreenLogic.TransferState.Transferable || this.MemberTransferState == PartyScreenLogic.TransferState.TransferableWithTrade);
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x000BFA81 File Offset: 0x000BDC81
		public bool IsPrisonerRecruitable(PartyScreenLogic.TroopType troopType, CharacterObject character, PartyScreenLogic.PartyRosterSide side)
		{
			return side == PartyScreenLogic.PartyRosterSide.Right && troopType == PartyScreenLogic.TroopType.Prisoner && !character.IsHero && this.CurrentData.RightRecruitableData.ContainsKey(character) && this.CurrentData.RightRecruitableData[character] > 0;
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x000BFAC0 File Offset: 0x000BDCC0
		public string GetRecruitableReasonString(CharacterObject character, bool isRecruitable, int troopCount, out bool showStackModifierText)
		{
			showStackModifierText = false;
			if (isRecruitable)
			{
				showStackModifierText = true;
				if (this.RightOwnerParty.PartySizeLimit <= this.MemberRosters[1].TotalManCount)
				{
					return GameTexts.FindText("str_recruit_party_size_limit", null).ToString();
				}
				return GameTexts.FindText("str_recruit_prisoner", null).ToString();
			}
			else
			{
				if (character.IsHero)
				{
					return GameTexts.FindText("str_cannot_recruit_hero", null).ToString();
				}
				return GameTexts.FindText("str_cannot_recruit_prisoner", null).ToString();
			}
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x000BFB40 File Offset: 0x000BDD40
		public bool IsExecutable(PartyScreenLogic.TroopType troopType, CharacterObject character, PartyScreenLogic.PartyRosterSide side)
		{
			return troopType == PartyScreenLogic.TroopType.Prisoner && side == PartyScreenLogic.PartyRosterSide.Right && character.IsHero && character.HeroObject.Age >= (float)Campaign.Current.Models.AgeModel.HeroComesOfAge && PlayerEncounter.Current == null && FaceGen.GetMaturityTypeWithAge(character.Age) > BodyMeshMaturityType.Tween;
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x000BFB96 File Offset: 0x000BDD96
		public string GetExecutableReasonString(CharacterObject character, bool isExecutable)
		{
			if (isExecutable)
			{
				return GameTexts.FindText("str_execute_prisoner", null).ToString();
			}
			if (!character.IsHero)
			{
				return GameTexts.FindText("str_cannot_execute_nonhero", null).ToString();
			}
			return GameTexts.FindText("str_cannot_execute_hero", null).ToString();
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x000BFBD8 File Offset: 0x000BDDD8
		public int GetCurrentQuestCurrentCount(bool includePrisoners, bool includeMembers)
		{
			int num = 0;
			if (includeMembers)
			{
				num += this.MemberRosters[0].Sum((TroopRosterElement item) => item.Number - item.WoundedNumber);
			}
			if (includePrisoners)
			{
				num += this.PrisonerRosters[0].Sum((TroopRosterElement item) => item.Number - item.WoundedNumber);
			}
			return num;
		}

		// Token: 0x06002E03 RID: 11779 RVA: 0x000BFC4C File Offset: 0x000BDE4C
		public int GetCurrentQuestRequiredCount()
		{
			return this.LeftPartyMembersSizeLimit;
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x000BFC54 File Offset: 0x000BDE54
		private static bool DefaultDoneHandler(TroopRoster leftMemberRoster, TroopRoster leftPrisonRoster, TroopRoster rightMemberRoster, TroopRoster rightPrisonRoster, FlattenedTroopRoster takenPrisonerRoster, FlattenedTroopRoster releasedPrisonerRoster, bool isForced, PartyBase leftParty = null, PartyBase rightParty = null)
		{
			return true;
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x000BFC58 File Offset: 0x000BDE58
		private void AddUpgradeToHistory(CharacterObject fromTroop, CharacterObject toTroop, int num)
		{
			Tuple<CharacterObject, CharacterObject, int> tuple = this.CurrentData.UpgradedTroopsHistory.Find((Tuple<CharacterObject, CharacterObject, int> t) => t.Item1 == fromTroop && t.Item2 == toTroop);
			if (tuple != null)
			{
				int item = tuple.Item3;
				this.CurrentData.UpgradedTroopsHistory.Remove(tuple);
				this.CurrentData.UpgradedTroopsHistory.Add(new Tuple<CharacterObject, CharacterObject, int>(fromTroop, toTroop, num + item));
				return;
			}
			this.CurrentData.UpgradedTroopsHistory.Add(new Tuple<CharacterObject, CharacterObject, int>(fromTroop, toTroop, num));
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x000BFCFC File Offset: 0x000BDEFC
		private void AddUsedHorsesToHistory(List<ValueTuple<EquipmentElement, int>> usedHorses)
		{
			if (usedHorses != null)
			{
				using (List<ValueTuple<EquipmentElement, int>>.Enumerator enumerator = usedHorses.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ValueTuple<EquipmentElement, int> usedHorse = enumerator.Current;
						Tuple<EquipmentElement, int> tuple = this.CurrentData.UsedUpgradeHorsesHistory.Find((Tuple<EquipmentElement, int> t) => t.Equals(usedHorse.Item1));
						if (tuple != null)
						{
							int item = tuple.Item2;
							this.CurrentData.UsedUpgradeHorsesHistory.Remove(tuple);
							this.CurrentData.UsedUpgradeHorsesHistory.Add(new Tuple<EquipmentElement, int>(usedHorse.Item1, item + usedHorse.Item2));
						}
						else
						{
							this.CurrentData.UsedUpgradeHorsesHistory.Add(new Tuple<EquipmentElement, int>(usedHorse.Item1, usedHorse.Item2));
						}
					}
				}
				PartyScreenData currentData = this.CurrentData;
				this.SetHorseChangeAmount(currentData.PartyHorseChangeAmount += usedHorses.Sum<ValueTuple<EquipmentElement, int>>((ValueTuple<EquipmentElement, int> t) => t.Item2));
			}
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000BFE30 File Offset: 0x000BE030
		private void UpdatePrisonerTransferHistory(CharacterObject troop, int amount)
		{
			Tuple<CharacterObject, int> tuple = this.CurrentData.TransferredPrisonersHistory.Find((Tuple<CharacterObject, int> t) => t.Item1 == troop);
			if (tuple != null)
			{
				int item = tuple.Item2;
				this.CurrentData.TransferredPrisonersHistory.Remove(tuple);
				this.CurrentData.TransferredPrisonersHistory.Add(new Tuple<CharacterObject, int>(troop, amount + item));
				return;
			}
			this.CurrentData.TransferredPrisonersHistory.Add(new Tuple<CharacterObject, int>(troop, amount));
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x000BFEC0 File Offset: 0x000BE0C0
		private void AddRecruitToHistory(CharacterObject troop, int amount)
		{
			Tuple<CharacterObject, int> tuple = this.CurrentData.RecruitedPrisonersHistory.Find((Tuple<CharacterObject, int> t) => t.Item1 == troop);
			if (tuple != null)
			{
				int item = tuple.Item2;
				this.CurrentData.RecruitedPrisonersHistory.Remove(tuple);
				this.CurrentData.RecruitedPrisonersHistory.Add(new Tuple<CharacterObject, int>(troop, amount + item));
			}
			else
			{
				this.CurrentData.RecruitedPrisonersHistory.Add(new Tuple<CharacterObject, int>(troop, amount));
			}
			float prisonerRecruitmentMoraleEffect = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.GetPrisonerRecruitmentMoraleEffect(this.RightOwnerParty, troop, amount);
			this.SetMoraleChangeAmount(this.CurrentData.PartyMoraleChangeAmount + prisonerRecruitmentMoraleEffect);
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x000BFF84 File Offset: 0x000BE184
		private string GetItemLockStringID(EquipmentElement equipmentElement)
		{
			return equipmentElement.Item.StringId + ((equipmentElement.ItemModifier != null) ? equipmentElement.ItemModifier.StringId : "");
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x000BFFB4 File Offset: 0x000BE1B4
		private List<ValueTuple<EquipmentElement, int>> RemoveItemFromItemRoster(ItemCategory itemCategory, int numOfItemsLeftToRemove = 1)
		{
			List<ValueTuple<EquipmentElement, int>> list = new List<ValueTuple<EquipmentElement, int>>();
			IEnumerable<string> lockedItems = Campaign.Current.GetCampaignBehavior<IViewDataTracker>().GetInventoryLocks();
			foreach (ItemRosterElement itemRosterElement in from x in this.RightOwnerParty.ItemRoster.Where<ItemRosterElement>(delegate(ItemRosterElement x)
				{
					ItemObject item = x.EquipmentElement.Item;
					return ((item != null) ? item.ItemCategory : null) == itemCategory;
				})
				orderby x.EquipmentElement.Item.Value
				orderby lockedItems.Contains(this.GetItemLockStringID(x.EquipmentElement))
				select x)
			{
				int num = MathF.Min(numOfItemsLeftToRemove, itemRosterElement.Amount);
				this.RightOwnerParty.ItemRoster.AddToCounts(itemRosterElement.EquipmentElement, -num);
				numOfItemsLeftToRemove -= num;
				list.Add(new ValueTuple<EquipmentElement, int>(itemRosterElement.EquipmentElement, num));
				if (numOfItemsLeftToRemove <= 0)
				{
					break;
				}
			}
			if (numOfItemsLeftToRemove > 0)
			{
				Debug.FailedAssert("Couldn't find enough upgrade req items in the inventory.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Party\\PartyScreenLogic.cs", "RemoveItemFromItemRoster", 1509);
			}
			return list;
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x000C00DC File Offset: 0x000BE2DC
		public void Reset(bool fromCancel)
		{
			this.ResetLogic(fromCancel);
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x000C00E5 File Offset: 0x000BE2E5
		private void ResetLogic(bool fromCancel)
		{
			if (this.CurrentData != this._initialData)
			{
				this.CurrentData.ResetUsing(this._initialData);
				PartyScreenLogic.AfterResetDelegate afterReset = this.AfterReset;
				if (afterReset == null)
				{
					return;
				}
				afterReset(this, fromCancel);
			}
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x000C011D File Offset: 0x000BE31D
		public void SavePartyScreenData()
		{
			this._savedData = new PartyScreenData();
			this._savedData.InitializeCopyFrom(this.CurrentData.RightParty, this.CurrentData.LeftParty);
			this._savedData.CopyFromScreenData(this.CurrentData);
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x000C015C File Offset: 0x000BE35C
		public void ResetToLastSavedPartyScreenData(bool fromCancel)
		{
			if (this.CurrentData != this._savedData)
			{
				this.CurrentData.ResetUsing(this._savedData);
				PartyScreenLogic.AfterResetDelegate afterReset = this.AfterReset;
				if (afterReset == null)
				{
					return;
				}
				afterReset(this, fromCancel);
			}
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x000C0194 File Offset: 0x000BE394
		public void RemoveZeroCounts()
		{
			for (int i = 0; i < this.MemberRosters.Length; i++)
			{
				this.MemberRosters[i].RemoveZeroCounts();
			}
			for (int j = 0; j < this.PrisonerRosters.Length; j++)
			{
				this.PrisonerRosters[j].RemoveZeroCounts();
			}
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x000C01E1 File Offset: 0x000BE3E1
		public int GetTroopRecruitableAmount(CharacterObject troop)
		{
			if (!this.CurrentData.RightRecruitableData.ContainsKey(troop))
			{
				return 0;
			}
			return this.CurrentData.RightRecruitableData[troop];
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x000C0209 File Offset: 0x000BE409
		public TroopRoster GetRoster(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType troopType)
		{
			if (troopType == PartyScreenLogic.TroopType.Member)
			{
				return this.MemberRosters[(int)side];
			}
			if (troopType == PartyScreenLogic.TroopType.Prisoner)
			{
				return this.PrisonerRosters[(int)side];
			}
			return null;
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x000C0226 File Offset: 0x000BE426
		internal void OnDoneEvent(List<TroopTradeDifference> freshlySellList)
		{
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x000C0228 File Offset: 0x000BE428
		public bool IsThereAnyChanges()
		{
			return this._initialData.IsThereAnyTroopTradeDifferenceBetween(this.CurrentData);
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x000C023C File Offset: 0x000BE43C
		public bool HaveRightSideGainedTroops()
		{
			foreach (TroopTradeDifference troopTradeDifference in this._initialData.GetTroopTradeDifferencesFromTo(this.CurrentData, PartyScreenLogic.PartyRosterSide.None))
			{
				if (!troopTradeDifference.IsPrisoner && troopTradeDifference.FromCount < troopTradeDifference.ToCount)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x000C02B8 File Offset: 0x000BE4B8
		public PartyScreenLogic.TroopComparer GetComparer(PartyScreenLogic.TroopSortType sortType)
		{
			return this._defaultComparers[sortType];
		}

		// Token: 0x04000D0F RID: 3343
		public PartyPresentationDoneButtonDelegate PartyPresentationDoneButtonDelegate;

		// Token: 0x04000D10 RID: 3344
		public PartyPresentationDoneButtonConditionDelegate PartyPresentationDoneButtonConditionDelegate;

		// Token: 0x04000D11 RID: 3345
		public PartyPresentationCancelButtonActivateDelegate PartyPresentationCancelButtonActivateDelegate;

		// Token: 0x04000D12 RID: 3346
		public PartyPresentationCancelButtonDelegate PartyPresentationCancelButtonDelegate;

		// Token: 0x04000D13 RID: 3347
		public PartyScreenLogic.PresentationUpdate UpdateDelegate;

		// Token: 0x04000D14 RID: 3348
		public IsTroopTransferableDelegate IsTroopTransferableDelegate;

		// Token: 0x04000D15 RID: 3349
		public CanTalkToHeroDelegate CanTalkToHeroDelegate;

		// Token: 0x04000D1D RID: 3357
		private PartyScreenLogic.TroopSortType _activeOtherPartySortType;

		// Token: 0x04000D1E RID: 3358
		private PartyScreenLogic.TroopSortType _activeMainPartySortType;

		// Token: 0x04000D1F RID: 3359
		private bool _isOtherPartySortAscending;

		// Token: 0x04000D20 RID: 3360
		private bool _isMainPartySortAscending;

		// Token: 0x04000D36 RID: 3382
		public TroopRoster[] MemberRosters;

		// Token: 0x04000D37 RID: 3383
		public TroopRoster[] PrisonerRosters;

		// Token: 0x04000D38 RID: 3384
		public bool IsConsumablesChanges;

		// Token: 0x04000D39 RID: 3385
		private PartyScreenHelper.PartyScreenMode _partyScreenMode;

		// Token: 0x04000D3A RID: 3386
		private readonly Dictionary<PartyScreenLogic.TroopSortType, PartyScreenLogic.TroopComparer> _defaultComparers;

		// Token: 0x04000D3B RID: 3387
		private readonly PartyScreenData _initialData;

		// Token: 0x04000D3C RID: 3388
		private PartyScreenData _savedData;

		// Token: 0x04000D3D RID: 3389
		private Game _game;

		// Token: 0x020006D4 RID: 1748
		public enum TroopSortType
		{
			// Token: 0x04001C14 RID: 7188
			Invalid = -1,
			// Token: 0x04001C15 RID: 7189
			Custom,
			// Token: 0x04001C16 RID: 7190
			Type,
			// Token: 0x04001C17 RID: 7191
			Name,
			// Token: 0x04001C18 RID: 7192
			Count,
			// Token: 0x04001C19 RID: 7193
			Tier
		}

		// Token: 0x020006D5 RID: 1749
		public enum PartyRosterSide : byte
		{
			// Token: 0x04001C1B RID: 7195
			None = 99,
			// Token: 0x04001C1C RID: 7196
			Right = 1,
			// Token: 0x04001C1D RID: 7197
			Left = 0
		}

		// Token: 0x020006D6 RID: 1750
		[Flags]
		public enum TroopType
		{
			// Token: 0x04001C1F RID: 7199
			Member = 1,
			// Token: 0x04001C20 RID: 7200
			Prisoner = 2,
			// Token: 0x04001C21 RID: 7201
			None = 3
		}

		// Token: 0x020006D7 RID: 1751
		public enum PartyCommandCode
		{
			// Token: 0x04001C23 RID: 7203
			TransferTroop,
			// Token: 0x04001C24 RID: 7204
			UpgradeTroop,
			// Token: 0x04001C25 RID: 7205
			TransferPartyLeaderTroop,
			// Token: 0x04001C26 RID: 7206
			TransferTroopToLeaderSlot,
			// Token: 0x04001C27 RID: 7207
			ShiftTroop,
			// Token: 0x04001C28 RID: 7208
			RecruitTroop,
			// Token: 0x04001C29 RID: 7209
			ExecuteTroop,
			// Token: 0x04001C2A RID: 7210
			TransferAllTroops,
			// Token: 0x04001C2B RID: 7211
			SortTroops
		}

		// Token: 0x020006D8 RID: 1752
		public enum TransferState
		{
			// Token: 0x04001C2D RID: 7213
			NotTransferable,
			// Token: 0x04001C2E RID: 7214
			Transferable,
			// Token: 0x04001C2F RID: 7215
			TransferableWithTrade
		}

		// Token: 0x020006D9 RID: 1753
		// (Invoke) Token: 0x0600564F RID: 22095
		public delegate void PresentationUpdate(PartyScreenLogic.PartyCommand command);

		// Token: 0x020006DA RID: 1754
		// (Invoke) Token: 0x06005653 RID: 22099
		public delegate void PartyGoldDelegate();

		// Token: 0x020006DB RID: 1755
		// (Invoke) Token: 0x06005657 RID: 22103
		public delegate void PartyMoraleDelegate();

		// Token: 0x020006DC RID: 1756
		// (Invoke) Token: 0x0600565B RID: 22107
		public delegate void PartyInfluenceDelegate();

		// Token: 0x020006DD RID: 1757
		// (Invoke) Token: 0x0600565F RID: 22111
		public delegate void PartyHorseDelegate();

		// Token: 0x020006DE RID: 1758
		// (Invoke) Token: 0x06005663 RID: 22115
		public delegate void AfterResetDelegate(PartyScreenLogic partyScreenLogic, bool fromCancel);

		// Token: 0x020006DF RID: 1759
		public class PartyCommand : ISerializableObject
		{
			// Token: 0x17000FE0 RID: 4064
			// (get) Token: 0x06005666 RID: 22118 RVA: 0x0019BDC7 File Offset: 0x00199FC7
			// (set) Token: 0x06005667 RID: 22119 RVA: 0x0019BDCF File Offset: 0x00199FCF
			public PartyScreenLogic.PartyCommandCode Code { get; private set; }

			// Token: 0x17000FE1 RID: 4065
			// (get) Token: 0x06005668 RID: 22120 RVA: 0x0019BDD8 File Offset: 0x00199FD8
			// (set) Token: 0x06005669 RID: 22121 RVA: 0x0019BDE0 File Offset: 0x00199FE0
			public PartyScreenLogic.PartyRosterSide RosterSide { get; private set; }

			// Token: 0x17000FE2 RID: 4066
			// (get) Token: 0x0600566A RID: 22122 RVA: 0x0019BDE9 File Offset: 0x00199FE9
			// (set) Token: 0x0600566B RID: 22123 RVA: 0x0019BDF1 File Offset: 0x00199FF1
			public CharacterObject Character { get; private set; }

			// Token: 0x17000FE3 RID: 4067
			// (get) Token: 0x0600566C RID: 22124 RVA: 0x0019BDFA File Offset: 0x00199FFA
			// (set) Token: 0x0600566D RID: 22125 RVA: 0x0019BE02 File Offset: 0x0019A002
			public int TotalNumber { get; private set; }

			// Token: 0x17000FE4 RID: 4068
			// (get) Token: 0x0600566E RID: 22126 RVA: 0x0019BE0B File Offset: 0x0019A00B
			// (set) Token: 0x0600566F RID: 22127 RVA: 0x0019BE13 File Offset: 0x0019A013
			public int WoundedNumber { get; private set; }

			// Token: 0x17000FE5 RID: 4069
			// (get) Token: 0x06005670 RID: 22128 RVA: 0x0019BE1C File Offset: 0x0019A01C
			// (set) Token: 0x06005671 RID: 22129 RVA: 0x0019BE24 File Offset: 0x0019A024
			public int Index { get; private set; }

			// Token: 0x17000FE6 RID: 4070
			// (get) Token: 0x06005672 RID: 22130 RVA: 0x0019BE2D File Offset: 0x0019A02D
			// (set) Token: 0x06005673 RID: 22131 RVA: 0x0019BE35 File Offset: 0x0019A035
			public int UpgradeTarget { get; private set; }

			// Token: 0x17000FE7 RID: 4071
			// (get) Token: 0x06005674 RID: 22132 RVA: 0x0019BE3E File Offset: 0x0019A03E
			// (set) Token: 0x06005675 RID: 22133 RVA: 0x0019BE46 File Offset: 0x0019A046
			public PartyScreenLogic.TroopType Type { get; private set; }

			// Token: 0x17000FE8 RID: 4072
			// (get) Token: 0x06005676 RID: 22134 RVA: 0x0019BE4F File Offset: 0x0019A04F
			// (set) Token: 0x06005677 RID: 22135 RVA: 0x0019BE57 File Offset: 0x0019A057
			public PartyScreenLogic.TroopSortType SortType { get; private set; }

			// Token: 0x17000FE9 RID: 4073
			// (get) Token: 0x06005678 RID: 22136 RVA: 0x0019BE60 File Offset: 0x0019A060
			// (set) Token: 0x06005679 RID: 22137 RVA: 0x0019BE68 File Offset: 0x0019A068
			public bool IsSortAscending { get; private set; }

			// Token: 0x0600567B RID: 22139 RVA: 0x0019BE79 File Offset: 0x0019A079
			public void FillForTransferTroop(PartyScreenLogic.PartyRosterSide fromSide, PartyScreenLogic.TroopType type, CharacterObject character, int totalNumber, int woundedNumber, int targetIndex)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.TransferTroop;
				this.RosterSide = fromSide;
				this.TotalNumber = totalNumber;
				this.WoundedNumber = woundedNumber;
				this.Character = character;
				this.Type = type;
				this.Index = targetIndex;
			}

			// Token: 0x0600567C RID: 22140 RVA: 0x0019BEAF File Offset: 0x0019A0AF
			public void FillForShiftTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character, int targetIndex)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.ShiftTroop;
				this.RosterSide = side;
				this.Character = character;
				this.Type = type;
				this.Index = targetIndex;
			}

			// Token: 0x0600567D RID: 22141 RVA: 0x0019BED5 File Offset: 0x0019A0D5
			public void FillForTransferTroopToLeaderSlot(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character, int totalNumber, int woundedNumber, int targetIndex)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.TransferTroopToLeaderSlot;
				this.RosterSide = side;
				this.TotalNumber = totalNumber;
				this.WoundedNumber = woundedNumber;
				this.Character = character;
				this.Type = type;
				this.Index = targetIndex;
			}

			// Token: 0x0600567E RID: 22142 RVA: 0x0019BF0B File Offset: 0x0019A10B
			public void FillForTransferPartyLeaderTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character, int totalNumber)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.TransferPartyLeaderTroop;
				this.RosterSide = side;
				this.TotalNumber = totalNumber;
				this.Character = character;
				this.Type = type;
			}

			// Token: 0x0600567F RID: 22143 RVA: 0x0019BF31 File Offset: 0x0019A131
			public void FillForUpgradeTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character, int number, int upgradeTargetType, int index)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.UpgradeTroop;
				this.RosterSide = side;
				this.TotalNumber = number;
				this.Character = character;
				this.UpgradeTarget = upgradeTargetType;
				this.Type = type;
				this.Index = index;
			}

			// Token: 0x06005680 RID: 22144 RVA: 0x0019BF67 File Offset: 0x0019A167
			public void FillForRecruitTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character, int number, int index)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.RecruitTroop;
				this.RosterSide = side;
				this.Character = character;
				this.Type = type;
				this.TotalNumber = number;
				this.Index = index;
			}

			// Token: 0x06005681 RID: 22145 RVA: 0x0019BF95 File Offset: 0x0019A195
			public void FillForExecuteTroop(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type, CharacterObject character)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.ExecuteTroop;
				this.RosterSide = side;
				this.Character = character;
				this.Type = type;
			}

			// Token: 0x06005682 RID: 22146 RVA: 0x0019BFB3 File Offset: 0x0019A1B3
			public void FillForTransferAllTroops(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopType type)
			{
				this.Code = PartyScreenLogic.PartyCommandCode.TransferAllTroops;
				this.RosterSide = side;
				this.Type = type;
			}

			// Token: 0x06005683 RID: 22147 RVA: 0x0019BFCA File Offset: 0x0019A1CA
			public void FillForSortTroops(PartyScreenLogic.PartyRosterSide side, PartyScreenLogic.TroopSortType sortType, bool isAscending)
			{
				this.RosterSide = side;
				this.Code = PartyScreenLogic.PartyCommandCode.SortTroops;
				this.SortType = sortType;
				this.IsSortAscending = isAscending;
			}

			// Token: 0x06005684 RID: 22148 RVA: 0x0019BFE8 File Offset: 0x0019A1E8
			void ISerializableObject.SerializeTo(IWriter writer)
			{
				writer.WriteByte((byte)this.Code);
				writer.WriteByte((byte)this.RosterSide);
				writer.WriteUInt(this.Character.Id.InternalValue);
				writer.WriteInt(this.TotalNumber);
				writer.WriteInt(this.WoundedNumber);
				writer.WriteInt(this.UpgradeTarget);
				writer.WriteByte((byte)this.Type);
			}

			// Token: 0x06005685 RID: 22149 RVA: 0x0019C058 File Offset: 0x0019A258
			void ISerializableObject.DeserializeFrom(IReader reader)
			{
				this.Code = (PartyScreenLogic.PartyCommandCode)reader.ReadByte();
				this.RosterSide = (PartyScreenLogic.PartyRosterSide)reader.ReadByte();
				MBGUID mbguid = new MBGUID(reader.ReadUInt());
				this.Character = (CharacterObject)MBObjectManager.Instance.GetObject(mbguid);
				this.TotalNumber = reader.ReadInt();
				this.WoundedNumber = reader.ReadInt();
				this.UpgradeTarget = reader.ReadInt();
				this.Type = (PartyScreenLogic.TroopType)reader.ReadByte();
			}
		}

		// Token: 0x020006E0 RID: 1760
		public abstract class TroopComparer : IComparer<TroopRosterElement>
		{
			// Token: 0x06005686 RID: 22150 RVA: 0x0019C0D0 File Offset: 0x0019A2D0
			public void SetIsAscending(bool isAscending)
			{
				this._isAscending = isAscending;
			}

			// Token: 0x06005687 RID: 22151 RVA: 0x0019C0D9 File Offset: 0x0019A2D9
			private int GetHeroComparisonResult(TroopRosterElement x, TroopRosterElement y)
			{
				if (x.Character.HeroObject != null)
				{
					if (x.Character.HeroObject == Hero.MainHero)
					{
						return -2;
					}
					if (y.Character.HeroObject == null)
					{
						return -1;
					}
				}
				return 0;
			}

			// Token: 0x06005688 RID: 22152 RVA: 0x0019C110 File Offset: 0x0019A310
			public int Compare(TroopRosterElement x, TroopRosterElement y)
			{
				int num = (this._isAscending ? 1 : (-1));
				int num2 = this.GetHeroComparisonResult(x, y);
				if (num2 != 0)
				{
					return num2;
				}
				num2 = this.GetHeroComparisonResult(y, x);
				if (num2 != 0)
				{
					return num2 * -1;
				}
				return this.CompareTroops(x, y) * num;
			}

			// Token: 0x06005689 RID: 22153
			protected abstract int CompareTroops(TroopRosterElement x, TroopRosterElement y);

			// Token: 0x04001C3A RID: 7226
			private bool _isAscending;
		}

		// Token: 0x020006E1 RID: 1761
		private class TroopDefaultComparer : PartyScreenLogic.TroopComparer
		{
			// Token: 0x0600568B RID: 22155 RVA: 0x0019C15A File Offset: 0x0019A35A
			protected override int CompareTroops(TroopRosterElement x, TroopRosterElement y)
			{
				return 0;
			}
		}

		// Token: 0x020006E2 RID: 1762
		private class TroopTypeComparer : PartyScreenLogic.TroopComparer
		{
			// Token: 0x0600568D RID: 22157 RVA: 0x0019C168 File Offset: 0x0019A368
			protected override int CompareTroops(TroopRosterElement x, TroopRosterElement y)
			{
				int defaultFormationClass = (int)x.Character.DefaultFormationClass;
				int defaultFormationClass2 = (int)y.Character.DefaultFormationClass;
				return defaultFormationClass.CompareTo(defaultFormationClass2);
			}
		}

		// Token: 0x020006E3 RID: 1763
		private class TroopNameComparer : PartyScreenLogic.TroopComparer
		{
			// Token: 0x0600568F RID: 22159 RVA: 0x0019C19D File Offset: 0x0019A39D
			protected override int CompareTroops(TroopRosterElement x, TroopRosterElement y)
			{
				return x.Character.Name.ToString().CompareTo(y.Character.Name.ToString());
			}
		}

		// Token: 0x020006E4 RID: 1764
		private class TroopCountComparer : PartyScreenLogic.TroopComparer
		{
			// Token: 0x06005691 RID: 22161 RVA: 0x0019C1CC File Offset: 0x0019A3CC
			protected override int CompareTroops(TroopRosterElement x, TroopRosterElement y)
			{
				return x.Number.CompareTo(y.Number);
			}
		}

		// Token: 0x020006E5 RID: 1765
		private class TroopTierComparer : PartyScreenLogic.TroopComparer
		{
			// Token: 0x06005693 RID: 22163 RVA: 0x0019C1F8 File Offset: 0x0019A3F8
			protected override int CompareTroops(TroopRosterElement x, TroopRosterElement y)
			{
				return x.Character.Tier.CompareTo(y.Character.Tier);
			}
		}
	}
}
