using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x020000A1 RID: 161
	public interface IViewDataTracker
	{
		// Token: 0x0600134C RID: 4940
		void SetInventoryLocks(IEnumerable<string> locks);

		// Token: 0x0600134D RID: 4941
		IEnumerable<string> GetInventoryLocks();

		// Token: 0x0600134E RID: 4942
		bool GetMapBarExtendedState();

		// Token: 0x0600134F RID: 4943
		void SetMapBarExtendedState(bool value);

		// Token: 0x06001350 RID: 4944
		void SetPartyTroopLocks(IEnumerable<string> locks);

		// Token: 0x06001351 RID: 4945
		void SetPartyPrisonerLocks(IEnumerable<string> locks);

		// Token: 0x06001352 RID: 4946
		void SetPartySortType(int sortType);

		// Token: 0x06001353 RID: 4947
		void SetIsPartySortAscending(bool isAscending);

		// Token: 0x06001354 RID: 4948
		IEnumerable<string> GetPartyTroopLocks();

		// Token: 0x06001355 RID: 4949
		IEnumerable<string> GetPartyPrisonerLocks();

		// Token: 0x06001356 RID: 4950
		int GetPartySortType();

		// Token: 0x06001357 RID: 4951
		bool GetIsPartySortAscending();

		// Token: 0x06001358 RID: 4952
		void AddEncyclopediaBookmarkToItem(Concept concept);

		// Token: 0x06001359 RID: 4953
		void AddEncyclopediaBookmarkToItem(Kingdom kingdom);

		// Token: 0x0600135A RID: 4954
		void AddEncyclopediaBookmarkToItem(Settlement settlement);

		// Token: 0x0600135B RID: 4955
		void AddEncyclopediaBookmarkToItem(CharacterObject unit);

		// Token: 0x0600135C RID: 4956
		void AddEncyclopediaBookmarkToItem(Hero item);

		// Token: 0x0600135D RID: 4957
		void AddEncyclopediaBookmarkToItem(ShipHull shipHull);

		// Token: 0x0600135E RID: 4958
		void AddEncyclopediaBookmarkToItem(Clan clan);

		// Token: 0x0600135F RID: 4959
		void RemoveEncyclopediaBookmarkFromItem(Hero hero);

		// Token: 0x06001360 RID: 4960
		void RemoveEncyclopediaBookmarkFromItem(ShipHull shipHull);

		// Token: 0x06001361 RID: 4961
		void RemoveEncyclopediaBookmarkFromItem(Clan clan);

		// Token: 0x06001362 RID: 4962
		void RemoveEncyclopediaBookmarkFromItem(Concept concept);

		// Token: 0x06001363 RID: 4963
		void RemoveEncyclopediaBookmarkFromItem(Kingdom kingdom);

		// Token: 0x06001364 RID: 4964
		void RemoveEncyclopediaBookmarkFromItem(Settlement settlement);

		// Token: 0x06001365 RID: 4965
		void RemoveEncyclopediaBookmarkFromItem(CharacterObject unit);

		// Token: 0x06001366 RID: 4966
		bool IsEncyclopediaBookmarked(Hero hero);

		// Token: 0x06001367 RID: 4967
		bool IsEncyclopediaBookmarked(ShipHull shipHull);

		// Token: 0x06001368 RID: 4968
		bool IsEncyclopediaBookmarked(Clan clan);

		// Token: 0x06001369 RID: 4969
		bool IsEncyclopediaBookmarked(Concept concept);

		// Token: 0x0600136A RID: 4970
		bool IsEncyclopediaBookmarked(Kingdom kingdom);

		// Token: 0x0600136B RID: 4971
		bool IsEncyclopediaBookmarked(Settlement settlement);

		// Token: 0x0600136C RID: 4972
		bool IsEncyclopediaBookmarked(CharacterObject unit);

		// Token: 0x0600136D RID: 4973
		void SetQuestSelection(QuestBase selection);

		// Token: 0x0600136E RID: 4974
		QuestBase GetQuestSelection();

		// Token: 0x0600136F RID: 4975
		void SetQuestSortTypeSelection(int questSortTypeSelection);

		// Token: 0x06001370 RID: 4976
		int GetQuestSortTypeSelection();

		// Token: 0x06001371 RID: 4977
		void InventorySetSortPreference(int inventoryMode, int sortOption, int sortState);

		// Token: 0x06001372 RID: 4978
		Tuple<int, int> InventoryGetSortPreference(int inventoryMode);

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06001373 RID: 4979
		bool IsPartyNotificationActive { get; }

		// Token: 0x06001374 RID: 4980
		TextObject GetPartyNotificationText();

		// Token: 0x06001375 RID: 4981
		void ClearPartyNotification();

		// Token: 0x06001376 RID: 4982
		void UpdatePartyNotification();

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06001377 RID: 4983
		bool IsQuestNotificationActive { get; }

		// Token: 0x17000539 RID: 1337
		// (get) Token: 0x06001378 RID: 4984
		IReadOnlyList<JournalLog> UnExaminedQuestLogs { get; }

		// Token: 0x06001379 RID: 4985
		TextObject GetQuestNotificationText();

		// Token: 0x0600137A RID: 4986
		void OnQuestLogExamined(JournalLog log);

		// Token: 0x1700053A RID: 1338
		// (get) Token: 0x0600137B RID: 4987
		List<Army> UnExaminedArmies { get; }

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x0600137C RID: 4988
		int NumOfKingdomArmyNotifications { get; }

		// Token: 0x0600137D RID: 4989
		void OnArmyExamined(Army army);

		// Token: 0x1700053C RID: 1340
		// (get) Token: 0x0600137E RID: 4990
		bool IsCharacterNotificationActive { get; }

		// Token: 0x0600137F RID: 4991
		void ClearCharacterNotification();

		// Token: 0x06001380 RID: 4992
		TextObject GetCharacterNotificationText();

		// Token: 0x06001381 RID: 4993
		MBReadOnlyList<ItemRosterElement> GetPlunderItems();

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06001382 RID: 4994
		IReadOnlyList<Figurehead> UnexaminedFigureheads { get; }

		// Token: 0x06001383 RID: 4995
		void OnFigureheadExamined(Figurehead figurehead);

		// Token: 0x06001384 RID: 4996
		void RemoveCraftingPieceNewlyUnlockedList(CraftingPiece craftingPiece);

		// Token: 0x06001385 RID: 4997
		int GetLastOpenedKingdomTabIndex();

		// Token: 0x06001386 RID: 4998
		void SetLastOpenedKingdomTabIndex(int tabIndex);

		// Token: 0x06001387 RID: 4999
		int GetLastOpenedClanTabIndex();

		// Token: 0x06001388 RID: 5000
		void SetLastOpenedClanTabIndex(int tabIndex);

		// Token: 0x06001389 RID: 5001
		bool IsCraftingPieceNewlyUnlocked(CraftingPiece craftingPiece);
	}
}
