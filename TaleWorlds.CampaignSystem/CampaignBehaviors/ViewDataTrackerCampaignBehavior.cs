using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.LogEntries;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Roster;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200046F RID: 1135
	public class ViewDataTrackerCampaignBehavior : CampaignBehaviorBase, IViewDataTracker
	{
		// Token: 0x06004967 RID: 18791 RVA: 0x001702EC File Offset: 0x0016E4EC
		public ViewDataTrackerCampaignBehavior()
		{
			this._inventoryItemLocks = new List<string>();
			this._partyPrisonerLocks = new List<string>();
			this._partyTroopLocks = new List<string>();
			this._encyclopediaBookmarkedClans = new List<Clan>();
			this._encyclopediaBookmarkedConcepts = new List<Concept>();
			this._encyclopediaBookmarkedHeroes = new List<Hero>();
			this._encyclopediaBookmarkedShips = new List<ShipHull>();
			this._encyclopediaBookmarkedKingdoms = new List<Kingdom>();
			this._encyclopediaBookmarkedSettlements = new List<Settlement>();
			this._encyclopediaBookmarkedUnits = new List<CharacterObject>();
			this._inventorySortPreferences = new Dictionary<int, Tuple<int, int>>();
			this._plunderItems = new List<ItemRosterElement>();
			this._unexaminedFigureheads = new List<Figurehead>();
			this._uninspectedCraftingPieces = new List<string>();
		}

		// Token: 0x17000EAD RID: 3757
		// (get) Token: 0x06004968 RID: 18792 RVA: 0x001703F8 File Offset: 0x0016E5F8
		// (set) Token: 0x06004969 RID: 18793 RVA: 0x00170400 File Offset: 0x0016E600
		public bool IsPartyNotificationActive { get; private set; }

		// Token: 0x0600496A RID: 18794 RVA: 0x00170409 File Offset: 0x0016E609
		public TextObject GetPartyNotificationText()
		{
			return this._recruitNotificationText.CopyTextObject().SetTextVariable("NUMBER", this._numOfRecruitablePrisoners);
		}

		// Token: 0x0600496B RID: 18795 RVA: 0x00170426 File Offset: 0x0016E626
		public void ClearPartyNotification()
		{
			this.IsPartyNotificationActive = false;
			this._numOfRecruitablePrisoners = 0;
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x00170436 File Offset: 0x0016E636
		public void UpdatePartyNotification()
		{
			this.UpdatePrisonerRecruitValue();
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x00170440 File Offset: 0x0016E640
		private void UpdatePrisonerRecruitValue()
		{
			Dictionary<CharacterObject, int> dictionary = new Dictionary<CharacterObject, int>();
			foreach (TroopRosterElement troopRosterElement in MobileParty.MainParty.PrisonRoster.GetTroopRoster())
			{
				int num = Campaign.Current.Models.PrisonerRecruitmentCalculationModel.CalculateRecruitableNumber(PartyBase.MainParty, troopRosterElement.Character);
				int num2;
				if (this._examinedPrisonerCharacterList.TryGetValue(troopRosterElement.Character, out num2))
				{
					if (num2 != num)
					{
						this._examinedPrisonerCharacterList[troopRosterElement.Character] = num;
						if (num2 < num)
						{
							this.IsPartyNotificationActive = true;
							this._numOfRecruitablePrisoners += num - num2;
						}
					}
				}
				else
				{
					this._examinedPrisonerCharacterList.Add(troopRosterElement.Character, num);
					if (num > 0)
					{
						this.IsPartyNotificationActive = true;
						this._numOfRecruitablePrisoners += num;
					}
				}
				dictionary.Add(troopRosterElement.Character, num);
			}
			this._examinedPrisonerCharacterList = dictionary;
		}

		// Token: 0x17000EAE RID: 3758
		// (get) Token: 0x0600496E RID: 18798 RVA: 0x0017054C File Offset: 0x0016E74C
		public bool IsQuestNotificationActive
		{
			get
			{
				return this.UnExaminedQuestLogs.Count > 0;
			}
		}

		// Token: 0x17000EAF RID: 3759
		// (get) Token: 0x0600496F RID: 18799 RVA: 0x0017055C File Offset: 0x0016E75C
		public IReadOnlyList<JournalLog> UnExaminedQuestLogs
		{
			get
			{
				if (this._isUnExaminedQuestLogJournalEntriesDirty)
				{
					this.UpdateJournalLogEntries();
				}
				for (int i = this._unExaminedQuestLogs.Count - 1; i >= 0; i--)
				{
					JournalLogEntry journalLogEntry = this._unExaminedQuestLogJournalEntries[this._unExaminedQuestLogs[i]];
					CampaignTime campaignTime = journalLogEntry.KeepInHistoryTime + journalLogEntry.GameTime;
					if (!journalLogEntry.IsValid() || campaignTime.IsPast)
					{
						this._unExaminedQuestLogJournalEntries.Remove(this._unExaminedQuestLogs[i]);
						this._unExaminedQuestLogs.RemoveAt(i);
					}
				}
				if (this._unExaminedQuestLogsReadOnly == null || this._unExaminedQuestLogs.Count != this._unExaminedQuestLogsReadOnly.Count)
				{
					this._unExaminedQuestLogsReadOnly = this._unExaminedQuestLogs.AsReadOnly();
				}
				return this._unExaminedQuestLogsReadOnly;
			}
		}

		// Token: 0x06004970 RID: 18800 RVA: 0x00170625 File Offset: 0x0016E825
		public TextObject GetQuestNotificationText()
		{
			return this._questNotificationText.CopyTextObject().SetTextVariable("NUMBER", this.UnExaminedQuestLogs.Count);
		}

		// Token: 0x06004971 RID: 18801 RVA: 0x00170648 File Offset: 0x0016E848
		public void OnQuestLogExamined(JournalLog log)
		{
			if (this._unExaminedQuestLogs.Contains(log))
			{
				this._unExaminedQuestLogs.Remove(log);
				this._unExaminedQuestLogsReadOnly = this._unExaminedQuestLogs.AsReadOnly();
				IEnumerable<JournalLog> entries = this._unExaminedQuestLogJournalEntries[log].GetEntries();
				if (this._unExaminedQuestLogs.All<JournalLog>((JournalLog x) => !entries.Contains(x)))
				{
					this._unExaminedQuestLogJournalEntries.Remove(log);
				}
			}
		}

		// Token: 0x06004972 RID: 18802 RVA: 0x001706C4 File Offset: 0x0016E8C4
		private void OnQuestLogAdded(QuestBase obj, bool hideInformation)
		{
			this._unExaminedQuestLogs.Add(obj.JournalEntries[obj.JournalEntries.Count - 1]);
			this._unExaminedQuestLogsReadOnly = this._unExaminedQuestLogs.AsReadOnly();
			this._isUnExaminedQuestLogJournalEntriesDirty = true;
		}

		// Token: 0x06004973 RID: 18803 RVA: 0x00170701 File Offset: 0x0016E901
		private void OnIssueLogAdded(IssueBase obj, bool hideInformation)
		{
			this._unExaminedQuestLogs.Add(obj.JournalEntries[obj.JournalEntries.Count - 1]);
			this._unExaminedQuestLogsReadOnly = this._unExaminedQuestLogs.AsReadOnly();
			this._isUnExaminedQuestLogJournalEntriesDirty = true;
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x00170740 File Offset: 0x0016E940
		private void UpdateJournalLogEntries()
		{
			this._unExaminedQuestLogJournalEntries.Clear();
			JournalLogEntry[] array = Campaign.Current.LogEntryHistory.GetGameActionLogs<JournalLogEntry>((JournalLogEntry x) => true).ToArray<JournalLogEntry>();
			for (int i = this._unExaminedQuestLogs.Count - 1; i >= 0; i--)
			{
				JournalLog unExaminedQuestLog = this._unExaminedQuestLogs[i];
				JournalLogEntry journalLogEntry = array.FirstOrDefault<JournalLogEntry>((JournalLogEntry x) => x.GetEntries().Contains(unExaminedQuestLog));
				if (journalLogEntry == null)
				{
					this._unExaminedQuestLogs.RemoveAt(i);
				}
				else
				{
					this._unExaminedQuestLogJournalEntries.Add(unExaminedQuestLog, journalLogEntry);
				}
			}
			if (this._unExaminedQuestLogs.Count != this._unExaminedQuestLogsReadOnly.Count)
			{
				this._unExaminedQuestLogsReadOnly = this._unExaminedQuestLogs.AsReadOnly();
			}
			this._isUnExaminedQuestLogJournalEntriesDirty = false;
		}

		// Token: 0x17000EB0 RID: 3760
		// (get) Token: 0x06004975 RID: 18805 RVA: 0x00170822 File Offset: 0x0016EA22
		public List<Army> UnExaminedArmies
		{
			get
			{
				return this._unExaminedArmies;
			}
		}

		// Token: 0x17000EB1 RID: 3761
		// (get) Token: 0x06004976 RID: 18806 RVA: 0x0017082A File Offset: 0x0016EA2A
		public int NumOfKingdomArmyNotifications
		{
			get
			{
				return this.UnExaminedArmies.Count;
			}
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x00170837 File Offset: 0x0016EA37
		public void OnArmyExamined(Army army)
		{
			this._unExaminedArmies.Remove(army);
		}

		// Token: 0x06004978 RID: 18808 RVA: 0x00170848 File Offset: 0x0016EA48
		private void OnArmyDispersed(Army arg1, Army.ArmyDispersionReason arg2, bool isPlayersArmy)
		{
			Army army;
			if (isPlayersArmy && (army = this._unExaminedArmies.SingleOrDefault<Army>((Army a) => a == arg1)) != null)
			{
				this._unExaminedArmies.Remove(army);
			}
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x0017088D File Offset: 0x0016EA8D
		private void OnNewArmyCreated(Army army)
		{
			if (army.Kingdom == Hero.MainHero.MapFaction && army.LeaderParty != MobileParty.MainParty)
			{
				this._unExaminedArmies.Add(army);
			}
		}

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x0600497A RID: 18810 RVA: 0x001708BA File Offset: 0x0016EABA
		public bool IsCharacterNotificationActive
		{
			get
			{
				return this._isCharacterNotificationActive;
			}
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x001708C2 File Offset: 0x0016EAC2
		public void ClearCharacterNotification()
		{
			this._isCharacterNotificationActive = false;
			this._numOfPerks = 0;
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x001708D2 File Offset: 0x0016EAD2
		public TextObject GetCharacterNotificationText()
		{
			return this._characterNotificationText.CopyTextObject().SetTextVariable("NUMBER", this._numOfPerks);
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x001708EF File Offset: 0x0016EAEF
		private void OnHeroGainedSkill(Hero hero, SkillObject skill, int change = 1, bool shouldNotify = true)
		{
			if ((hero == Hero.MainHero || hero.Clan == Clan.PlayerClan) && PerkHelper.AvailablePerkCountOfHero(hero) > 0)
			{
				this._isCharacterNotificationActive = shouldNotify;
				this._numOfPerks++;
			}
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x00170925 File Offset: 0x0016EB25
		private void OnHeroLevelledUp(Hero hero, bool shouldNotify)
		{
			if (hero == Hero.MainHero)
			{
				this._isCharacterNotificationActive = shouldNotify;
			}
		}

		// Token: 0x0600497F RID: 18815 RVA: 0x00170936 File Offset: 0x0016EB36
		public int GetLastOpenedKingdomTabIndex()
		{
			return this._lastOpenedKingdomTabIndex;
		}

		// Token: 0x06004980 RID: 18816 RVA: 0x0017093E File Offset: 0x0016EB3E
		public void SetLastOpenedKingdomTabIndex(int tabIndex)
		{
			this._lastOpenedKingdomTabIndex = tabIndex;
		}

		// Token: 0x06004981 RID: 18817 RVA: 0x00170947 File Offset: 0x0016EB47
		public int GetLastOpenedClanTabIndex()
		{
			return this._lastOpenedClanTabIndex;
		}

		// Token: 0x06004982 RID: 18818 RVA: 0x0017094F File Offset: 0x0016EB4F
		public void SetLastOpenedClanTabIndex(int tabIndex)
		{
			this._lastOpenedClanTabIndex = tabIndex;
		}

		// Token: 0x06004983 RID: 18819 RVA: 0x00170958 File Offset: 0x0016EB58
		private void OnGameLoaded(CampaignGameStarter campaignGameStarter)
		{
			this._unExaminedQuestLogsReadOnly = this._unExaminedQuestLogs.AsReadOnly();
			this.UpdatePartyNotification();
			this.UpdatePrisonerRecruitValue();
			this.UpdateJournalLogEntries();
		}

		// Token: 0x06004984 RID: 18820 RVA: 0x0017097D File Offset: 0x0016EB7D
		public bool GetMapBarExtendedState()
		{
			return this._isMapBarExtended;
		}

		// Token: 0x06004985 RID: 18821 RVA: 0x00170985 File Offset: 0x0016EB85
		public void SetMapBarExtendedState(bool isExtended)
		{
			this._isMapBarExtended = isExtended;
		}

		// Token: 0x06004986 RID: 18822 RVA: 0x00170990 File Offset: 0x0016EB90
		public void SetInventoryLocks(IEnumerable<string> locks)
		{
			this._inventoryItemLocks.Clear();
			foreach (string text in locks)
			{
				this._inventoryItemLocks.Add(text);
			}
		}

		// Token: 0x06004987 RID: 18823 RVA: 0x001709E8 File Offset: 0x0016EBE8
		public IEnumerable<string> GetInventoryLocks()
		{
			return this._inventoryItemLocks;
		}

		// Token: 0x06004988 RID: 18824 RVA: 0x001709F0 File Offset: 0x0016EBF0
		public void InventorySetSortPreference(int inventoryMode, int sortOption, int sortState)
		{
			this._inventorySortPreferences[inventoryMode] = new Tuple<int, int>(sortOption, sortState);
		}

		// Token: 0x06004989 RID: 18825 RVA: 0x00170A08 File Offset: 0x0016EC08
		public Tuple<int, int> InventoryGetSortPreference(int inventoryMode)
		{
			Tuple<int, int> tuple;
			if (this._inventorySortPreferences.TryGetValue(inventoryMode, out tuple))
			{
				return tuple;
			}
			return new Tuple<int, int>(0, 0);
		}

		// Token: 0x0600498A RID: 18826 RVA: 0x00170A30 File Offset: 0x0016EC30
		public void SetPartyTroopLocks(IEnumerable<string> locks)
		{
			this._partyTroopLocks.Clear();
			foreach (string text in locks)
			{
				this._partyTroopLocks.Add(text);
			}
		}

		// Token: 0x0600498B RID: 18827 RVA: 0x00170A88 File Offset: 0x0016EC88
		public void SetPartyPrisonerLocks(IEnumerable<string> locks)
		{
			this._partyPrisonerLocks.Clear();
			foreach (string text in locks)
			{
				this._partyPrisonerLocks.Add(text);
			}
		}

		// Token: 0x0600498C RID: 18828 RVA: 0x00170AE0 File Offset: 0x0016ECE0
		public void SetPartySortType(int sortType)
		{
			this._partySortType = sortType;
		}

		// Token: 0x0600498D RID: 18829 RVA: 0x00170AE9 File Offset: 0x0016ECE9
		public void SetIsPartySortAscending(bool isAscending)
		{
			this._isPartySortAscending = isAscending;
		}

		// Token: 0x0600498E RID: 18830 RVA: 0x00170AF2 File Offset: 0x0016ECF2
		public IEnumerable<string> GetPartyTroopLocks()
		{
			return this._partyTroopLocks;
		}

		// Token: 0x0600498F RID: 18831 RVA: 0x00170AFA File Offset: 0x0016ECFA
		public IEnumerable<string> GetPartyPrisonerLocks()
		{
			return this._partyPrisonerLocks;
		}

		// Token: 0x06004990 RID: 18832 RVA: 0x00170B02 File Offset: 0x0016ED02
		public int GetPartySortType()
		{
			return this._partySortType;
		}

		// Token: 0x06004991 RID: 18833 RVA: 0x00170B0A File Offset: 0x0016ED0A
		public bool GetIsPartySortAscending()
		{
			return this._isPartySortAscending;
		}

		// Token: 0x06004992 RID: 18834 RVA: 0x00170B12 File Offset: 0x0016ED12
		public void AddEncyclopediaBookmarkToItem(Hero item)
		{
			this._encyclopediaBookmarkedHeroes.Add(item);
		}

		// Token: 0x06004993 RID: 18835 RVA: 0x00170B20 File Offset: 0x0016ED20
		public void AddEncyclopediaBookmarkToItem(ShipHull shipHull)
		{
			this._encyclopediaBookmarkedShips.Add(shipHull);
		}

		// Token: 0x06004994 RID: 18836 RVA: 0x00170B2E File Offset: 0x0016ED2E
		public void AddEncyclopediaBookmarkToItem(Clan clan)
		{
			this._encyclopediaBookmarkedClans.Add(clan);
		}

		// Token: 0x06004995 RID: 18837 RVA: 0x00170B3C File Offset: 0x0016ED3C
		public void AddEncyclopediaBookmarkToItem(Concept concept)
		{
			this._encyclopediaBookmarkedConcepts.Add(concept);
		}

		// Token: 0x06004996 RID: 18838 RVA: 0x00170B4A File Offset: 0x0016ED4A
		public void AddEncyclopediaBookmarkToItem(Kingdom kingdom)
		{
			this._encyclopediaBookmarkedKingdoms.Add(kingdom);
		}

		// Token: 0x06004997 RID: 18839 RVA: 0x00170B58 File Offset: 0x0016ED58
		public void AddEncyclopediaBookmarkToItem(Settlement settlement)
		{
			this._encyclopediaBookmarkedSettlements.Add(settlement);
		}

		// Token: 0x06004998 RID: 18840 RVA: 0x00170B66 File Offset: 0x0016ED66
		public void AddEncyclopediaBookmarkToItem(CharacterObject unit)
		{
			this._encyclopediaBookmarkedUnits.Add(unit);
		}

		// Token: 0x06004999 RID: 18841 RVA: 0x00170B74 File Offset: 0x0016ED74
		public void RemoveEncyclopediaBookmarkFromItem(Hero hero)
		{
			this._encyclopediaBookmarkedHeroes.Remove(hero);
		}

		// Token: 0x0600499A RID: 18842 RVA: 0x00170B83 File Offset: 0x0016ED83
		public void RemoveEncyclopediaBookmarkFromItem(ShipHull shipHull)
		{
			this._encyclopediaBookmarkedShips.Remove(shipHull);
		}

		// Token: 0x0600499B RID: 18843 RVA: 0x00170B92 File Offset: 0x0016ED92
		public void RemoveEncyclopediaBookmarkFromItem(Clan clan)
		{
			this._encyclopediaBookmarkedClans.Remove(clan);
		}

		// Token: 0x0600499C RID: 18844 RVA: 0x00170BA1 File Offset: 0x0016EDA1
		public void RemoveEncyclopediaBookmarkFromItem(Concept concept)
		{
			this._encyclopediaBookmarkedConcepts.Remove(concept);
		}

		// Token: 0x0600499D RID: 18845 RVA: 0x00170BB0 File Offset: 0x0016EDB0
		public void RemoveEncyclopediaBookmarkFromItem(Kingdom kingdom)
		{
			this._encyclopediaBookmarkedKingdoms.Remove(kingdom);
		}

		// Token: 0x0600499E RID: 18846 RVA: 0x00170BBF File Offset: 0x0016EDBF
		public void RemoveEncyclopediaBookmarkFromItem(Settlement settlement)
		{
			this._encyclopediaBookmarkedSettlements.Remove(settlement);
		}

		// Token: 0x0600499F RID: 18847 RVA: 0x00170BCE File Offset: 0x0016EDCE
		public void RemoveEncyclopediaBookmarkFromItem(CharacterObject unit)
		{
			this._encyclopediaBookmarkedUnits.Remove(unit);
		}

		// Token: 0x060049A0 RID: 18848 RVA: 0x00170BDD File Offset: 0x0016EDDD
		public bool IsEncyclopediaBookmarked(Hero hero)
		{
			return this._encyclopediaBookmarkedHeroes.Contains(hero);
		}

		// Token: 0x060049A1 RID: 18849 RVA: 0x00170BEB File Offset: 0x0016EDEB
		public bool IsEncyclopediaBookmarked(ShipHull shipHull)
		{
			return this._encyclopediaBookmarkedShips.Contains(shipHull);
		}

		// Token: 0x060049A2 RID: 18850 RVA: 0x00170BF9 File Offset: 0x0016EDF9
		public bool IsEncyclopediaBookmarked(Clan clan)
		{
			return this._encyclopediaBookmarkedClans.Contains(clan);
		}

		// Token: 0x060049A3 RID: 18851 RVA: 0x00170C07 File Offset: 0x0016EE07
		public bool IsEncyclopediaBookmarked(Concept concept)
		{
			return this._encyclopediaBookmarkedConcepts.Contains(concept);
		}

		// Token: 0x060049A4 RID: 18852 RVA: 0x00170C15 File Offset: 0x0016EE15
		public bool IsEncyclopediaBookmarked(Kingdom kingdom)
		{
			return this._encyclopediaBookmarkedKingdoms.Contains(kingdom);
		}

		// Token: 0x060049A5 RID: 18853 RVA: 0x00170C23 File Offset: 0x0016EE23
		public bool IsEncyclopediaBookmarked(Settlement settlement)
		{
			return this._encyclopediaBookmarkedSettlements.Contains(settlement);
		}

		// Token: 0x060049A6 RID: 18854 RVA: 0x00170C31 File Offset: 0x0016EE31
		public bool IsEncyclopediaBookmarked(CharacterObject unit)
		{
			return this._encyclopediaBookmarkedUnits.Contains(unit);
		}

		// Token: 0x060049A7 RID: 18855 RVA: 0x00170C3F File Offset: 0x0016EE3F
		public void SetQuestSelection(QuestBase selection)
		{
			this._questSelection = selection;
		}

		// Token: 0x060049A8 RID: 18856 RVA: 0x00170C48 File Offset: 0x0016EE48
		public QuestBase GetQuestSelection()
		{
			return this._questSelection;
		}

		// Token: 0x060049A9 RID: 18857 RVA: 0x00170C50 File Offset: 0x0016EE50
		public MBReadOnlyList<ItemRosterElement> GetPlunderItems()
		{
			return new MBReadOnlyList<ItemRosterElement>(this._plunderItems);
		}

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x060049AA RID: 18858 RVA: 0x00170C5D File Offset: 0x0016EE5D
		public IReadOnlyList<Figurehead> UnexaminedFigureheads
		{
			get
			{
				return this._unexaminedFigureheads;
			}
		}

		// Token: 0x060049AB RID: 18859 RVA: 0x00170C65 File Offset: 0x0016EE65
		private void OnFigureheadUnlocked(Figurehead newFigurehead)
		{
			this._unexaminedFigureheads.Add(newFigurehead);
		}

		// Token: 0x060049AC RID: 18860 RVA: 0x00170C73 File Offset: 0x0016EE73
		public void OnFigureheadExamined(Figurehead figurehead)
		{
			this._unexaminedFigureheads.Remove(figurehead);
		}

		// Token: 0x060049AD RID: 18861 RVA: 0x00170C82 File Offset: 0x0016EE82
		public void RemoveCraftingPieceNewlyUnlockedList(CraftingPiece craftingPiece)
		{
			this._uninspectedCraftingPieces.Remove(craftingPiece.StringId);
		}

		// Token: 0x060049AE RID: 18862 RVA: 0x00170C96 File Offset: 0x0016EE96
		private void OnCraftingPieceUnlocked(CraftingPiece craftingPiece)
		{
			this._uninspectedCraftingPieces.Add(craftingPiece.StringId);
		}

		// Token: 0x060049AF RID: 18863 RVA: 0x00170CA9 File Offset: 0x0016EEA9
		public bool IsCraftingPieceNewlyUnlocked(CraftingPiece craftingPiece)
		{
			return this._uninspectedCraftingPieces.Contains(craftingPiece.StringId);
		}

		// Token: 0x060049B0 RID: 18864 RVA: 0x00170CBC File Offset: 0x0016EEBC
		public override void RegisterEvents()
		{
			CampaignEvents.HeroGainedSkill.AddNonSerializedListener(this, new Action<Hero, SkillObject, int, bool>(this.OnHeroGainedSkill));
			CampaignEvents.HeroLevelledUp.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroLevelledUp));
			CampaignEvents.ArmyCreated.AddNonSerializedListener(this, new Action<Army>(this.OnNewArmyCreated));
			CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, new Action<Army, Army.ArmyDispersionReason, bool>(this.OnArmyDispersed));
			CampaignEvents.QuestLogAddedEvent.AddNonSerializedListener(this, new Action<QuestBase, bool>(this.OnQuestLogAdded));
			CampaignEvents.IssueLogAddedEvent.AddNonSerializedListener(this, new Action<IssueBase, bool>(this.OnIssueLogAdded));
			CampaignEvents.OnGameLoadedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnGameLoaded));
			CampaignEvents.ItemsLooted.AddNonSerializedListener(this, new Action<MobileParty, ItemRoster>(this.OnPlayerPlunderedItems));
			CampaignEvents.RaidCompletedEvent.AddNonSerializedListener(this, new Action<BattleSideEnum, RaidEventComponent>(this.OnRaidCompleted));
			CampaignEvents.OnFigureheadUnlockedEvent.AddNonSerializedListener(this, new Action<Figurehead>(this.OnFigureheadUnlocked));
			CampaignEvents.CraftingPartUnlockedEvent.AddNonSerializedListener(this, new Action<CraftingPiece>(this.OnCraftingPieceUnlocked));
		}

		// Token: 0x060049B1 RID: 18865 RVA: 0x00170DC6 File Offset: 0x0016EFC6
		private void OnRaidCompleted(BattleSideEnum winnerSide, RaidEventComponent raidEvent)
		{
			if (raidEvent.IsPlayerMapEvent)
			{
				this._plunderItems.Clear();
			}
		}

		// Token: 0x060049B2 RID: 18866 RVA: 0x00170DDC File Offset: 0x0016EFDC
		private void OnPlayerPlunderedItems(MobileParty mobileParty, ItemRoster items)
		{
			if (mobileParty == MobileParty.MainParty)
			{
				for (int i = 0; i < items.Count; i++)
				{
					ItemRosterElement itemRosterElement = items[i];
					bool flag = false;
					for (int j = 0; j < this._plunderItems.Count; j++)
					{
						if (this._plunderItems[j].EquipmentElement.IsEqualTo(itemRosterElement.EquipmentElement))
						{
							ItemRosterElement itemRosterElement2 = this._plunderItems[j];
							itemRosterElement2.Amount += itemRosterElement.Amount;
							this._plunderItems[j] = itemRosterElement2;
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this._plunderItems.Add(itemRosterElement);
					}
				}
			}
		}

		// Token: 0x060049B3 RID: 18867 RVA: 0x00170E96 File Offset: 0x0016F096
		public void SetQuestSortTypeSelection(int questSortTypeSelection)
		{
			this._questSortTypeSelection = questSortTypeSelection;
		}

		// Token: 0x060049B4 RID: 18868 RVA: 0x00170E9F File Offset: 0x0016F09F
		public int GetQuestSortTypeSelection()
		{
			return this._questSortTypeSelection;
		}

		// Token: 0x060049B5 RID: 18869 RVA: 0x00170EA8 File Offset: 0x0016F0A8
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<bool>("_isMapBarExtended", ref this._isMapBarExtended);
			dataStore.SyncData<List<string>>("_inventoryItemLocks", ref this._inventoryItemLocks);
			dataStore.SyncData<Dictionary<int, Tuple<int, int>>>("_inventorySortPreferences", ref this._inventorySortPreferences);
			dataStore.SyncData<int>("_partySortType", ref this._partySortType);
			dataStore.SyncData<bool>("_isPartySortAscending", ref this._isPartySortAscending);
			dataStore.SyncData<List<string>>("_partyTroopLocks", ref this._partyTroopLocks);
			dataStore.SyncData<List<string>>("_partyPrisonerLocks", ref this._partyPrisonerLocks);
			dataStore.SyncData<List<Hero>>("_encyclopediaBookmarkedHeroes", ref this._encyclopediaBookmarkedHeroes);
			dataStore.SyncData<List<ShipHull>>("_encyclopediaBookmarkedShips", ref this._encyclopediaBookmarkedShips);
			dataStore.SyncData<List<Clan>>("_encyclopediaBookmarkedClans", ref this._encyclopediaBookmarkedClans);
			dataStore.SyncData<List<Concept>>("_encyclopediaBookmarkedConcepts", ref this._encyclopediaBookmarkedConcepts);
			dataStore.SyncData<List<Kingdom>>("_encyclopediaBookmarkedKingdoms", ref this._encyclopediaBookmarkedKingdoms);
			dataStore.SyncData<List<Settlement>>("_encyclopediaBookmarkedSettlements", ref this._encyclopediaBookmarkedSettlements);
			dataStore.SyncData<List<CharacterObject>>("_encyclopediaBookmarkedUnits", ref this._encyclopediaBookmarkedUnits);
			dataStore.SyncData<QuestBase>("_questSelection", ref this._questSelection);
			dataStore.SyncData<List<JournalLog>>("_unExaminedQuestLogs", ref this._unExaminedQuestLogs);
			dataStore.SyncData<List<Army>>("_unExaminedArmies", ref this._unExaminedArmies);
			dataStore.SyncData<bool>("_isCharacterNotificationActive", ref this._isCharacterNotificationActive);
			dataStore.SyncData<int>("_numOfPerks", ref this._numOfPerks);
			dataStore.SyncData<Dictionary<CharacterObject, int>>("_examinedPrisonerCharacterList", ref this._examinedPrisonerCharacterList);
			dataStore.SyncData<List<ItemRosterElement>>("_plunderItems", ref this._plunderItems);
			dataStore.SyncData<List<Figurehead>>("_unexaminedFigureheads", ref this._unexaminedFigureheads);
			dataStore.SyncData<List<string>>("_uninspectedCraftingPieces", ref this._uninspectedCraftingPieces);
		}

		// Token: 0x040014A6 RID: 5286
		private readonly TextObject _characterNotificationText = new TextObject("{=rlqjkZ9Q}You have {NUMBER} new perks available for selection.", null);

		// Token: 0x040014A7 RID: 5287
		private readonly TextObject _questNotificationText = new TextObject("{=FAIYN0vN}You have {NUMBER} new updates to your quests.", null);

		// Token: 0x040014A8 RID: 5288
		private readonly TextObject _recruitNotificationText = new TextObject("{=PJMbfSPJ}You have {NUMBER} new prisoners to recruit.", null);

		// Token: 0x040014AA RID: 5290
		private Dictionary<CharacterObject, int> _examinedPrisonerCharacterList = new Dictionary<CharacterObject, int>();

		// Token: 0x040014AB RID: 5291
		private int _numOfRecruitablePrisoners;

		// Token: 0x040014AC RID: 5292
		private List<JournalLog> _unExaminedQuestLogs = new List<JournalLog>();

		// Token: 0x040014AD RID: 5293
		private IReadOnlyList<JournalLog> _unExaminedQuestLogsReadOnly;

		// Token: 0x040014AE RID: 5294
		private readonly Dictionary<JournalLog, JournalLogEntry> _unExaminedQuestLogJournalEntries = new Dictionary<JournalLog, JournalLogEntry>();

		// Token: 0x040014AF RID: 5295
		private bool _isUnExaminedQuestLogJournalEntriesDirty;

		// Token: 0x040014B0 RID: 5296
		private List<Army> _unExaminedArmies = new List<Army>();

		// Token: 0x040014B1 RID: 5297
		private bool _isCharacterNotificationActive;

		// Token: 0x040014B2 RID: 5298
		private int _numOfPerks;

		// Token: 0x040014B3 RID: 5299
		private int _lastOpenedKingdomTabIndex;

		// Token: 0x040014B4 RID: 5300
		private int _lastOpenedClanTabIndex;

		// Token: 0x040014B5 RID: 5301
		private bool _isMapBarExtended;

		// Token: 0x040014B6 RID: 5302
		private List<string> _inventoryItemLocks;

		// Token: 0x040014B7 RID: 5303
		[SaveableField(21)]
		private Dictionary<int, Tuple<int, int>> _inventorySortPreferences;

		// Token: 0x040014B8 RID: 5304
		private int _partySortType;

		// Token: 0x040014B9 RID: 5305
		private bool _isPartySortAscending;

		// Token: 0x040014BA RID: 5306
		private List<string> _partyTroopLocks;

		// Token: 0x040014BB RID: 5307
		private List<string> _partyPrisonerLocks;

		// Token: 0x040014BC RID: 5308
		private List<Hero> _encyclopediaBookmarkedHeroes;

		// Token: 0x040014BD RID: 5309
		private List<ShipHull> _encyclopediaBookmarkedShips;

		// Token: 0x040014BE RID: 5310
		private List<Clan> _encyclopediaBookmarkedClans;

		// Token: 0x040014BF RID: 5311
		private List<Concept> _encyclopediaBookmarkedConcepts;

		// Token: 0x040014C0 RID: 5312
		private List<Kingdom> _encyclopediaBookmarkedKingdoms;

		// Token: 0x040014C1 RID: 5313
		private List<Settlement> _encyclopediaBookmarkedSettlements;

		// Token: 0x040014C2 RID: 5314
		private List<CharacterObject> _encyclopediaBookmarkedUnits;

		// Token: 0x040014C3 RID: 5315
		private QuestBase _questSelection;

		// Token: 0x040014C4 RID: 5316
		[SaveableField(51)]
		private int _questSortTypeSelection;

		// Token: 0x040014C5 RID: 5317
		private List<ItemRosterElement> _plunderItems;

		// Token: 0x040014C6 RID: 5318
		private List<Figurehead> _unexaminedFigureheads;

		// Token: 0x040014C7 RID: 5319
		private List<string> _uninspectedCraftingPieces;
	}
}
