using System;
using System.Runtime.CompilerServices;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200042C RID: 1068
	public interface IStatisticsCampaignBehavior : ICampaignBehavior
	{
		// Token: 0x060043C0 RID: 17344
		void OnDefectionPersuasionSucess();

		// Token: 0x060043C1 RID: 17345
		void OnPlayerAcceptedRansomOffer(int ransomPrice);

		// Token: 0x060043C2 RID: 17346
		int GetHighestTournamentRank();

		// Token: 0x060043C3 RID: 17347
		int GetNumberOfTournamentWins();

		// Token: 0x060043C4 RID: 17348
		int GetNumberOfChildrenBorn();

		// Token: 0x060043C5 RID: 17349
		int GetNumberOfPrisonersRecruited();

		// Token: 0x060043C6 RID: 17350
		int GetNumberOfTroopsRecruited();

		// Token: 0x060043C7 RID: 17351
		int GetNumberOfClansDefected();

		// Token: 0x060043C8 RID: 17352
		int GetNumberOfIssuesSolved();

		// Token: 0x060043C9 RID: 17353
		int GetTotalInfluenceEarned();

		// Token: 0x060043CA RID: 17354
		int GetTotalCrimeRatingGained();

		// Token: 0x060043CB RID: 17355
		int GetNumberOfBattlesWon();

		// Token: 0x060043CC RID: 17356
		int GetNumberOfBattlesLost();

		// Token: 0x060043CD RID: 17357
		int GetLargestBattleWonAsLeader();

		// Token: 0x060043CE RID: 17358
		int GetLargestArmyFormedByPlayer();

		// Token: 0x060043CF RID: 17359
		int GetNumberOfEnemyClansDestroyed();

		// Token: 0x060043D0 RID: 17360
		int GetNumberOfHeroesKilledInBattle();

		// Token: 0x060043D1 RID: 17361
		int GetNumberOfTroopsKnockedOrKilledAsParty();

		// Token: 0x060043D2 RID: 17362
		int GetNumberOfTroopsKnockedOrKilledByPlayer();

		// Token: 0x060043D3 RID: 17363
		int GetNumberOfHeroPrisonersTaken();

		// Token: 0x060043D4 RID: 17364
		int GetNumberOfTroopPrisonersTaken();

		// Token: 0x060043D5 RID: 17365
		int GetNumberOfTownsCaptured();

		// Token: 0x060043D6 RID: 17366
		int GetNumberOfHideoutsCleared();

		// Token: 0x060043D7 RID: 17367
		int GetNumberOfCastlesCaptured();

		// Token: 0x060043D8 RID: 17368
		int GetNumberOfVillagesRaided();

		// Token: 0x060043D9 RID: 17369
		int GetNumberOfCraftingPartsUnlocked();

		// Token: 0x060043DA RID: 17370
		int GetNumberOfWeaponsCrafted();

		// Token: 0x060043DB RID: 17371
		int GetNumberOfCraftingOrdersCompleted();

		// Token: 0x060043DC RID: 17372
		int GetNumberOfCompanionsHired();

		// Token: 0x060043DD RID: 17373
		ulong GetTotalTimePlayedInSeconds();

		// Token: 0x060043DE RID: 17374
		ulong GetTotalDenarsEarned();

		// Token: 0x060043DF RID: 17375
		ulong GetDenarsEarnedFromCaravans();

		// Token: 0x060043E0 RID: 17376
		ulong GetDenarsEarnedFromWorkshops();

		// Token: 0x060043E1 RID: 17377
		ulong GetDenarsEarnedFromRansoms();

		// Token: 0x060043E2 RID: 17378
		ulong GetDenarsEarnedFromTaxes();

		// Token: 0x060043E3 RID: 17379
		ulong GetDenarsEarnedFromTributes();

		// Token: 0x060043E4 RID: 17380
		ulong GetDenarsPaidAsTributes();

		// Token: 0x060043E5 RID: 17381
		CampaignTime GetTotalTimePlayed();

		// Token: 0x060043E6 RID: 17382
		CampaignTime GetTimeSpentAsPrisoner();

		// Token: 0x060043E7 RID: 17383
		ValueTuple<string, int> GetMostExpensiveItemCrafted();

		// Token: 0x060043E8 RID: 17384
		[return: TupleElementNames(new string[] { "name", "value" })]
		ValueTuple<string, int> GetCompanionWithMostKills();

		// Token: 0x060043E9 RID: 17385
		[return: TupleElementNames(new string[] { "name", "value" })]
		ValueTuple<string, int> GetCompanionWithMostIssuesSolved();
	}
}
