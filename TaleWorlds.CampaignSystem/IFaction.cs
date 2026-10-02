using System;
using TaleWorlds.CampaignSystem.Party.PartyComponents;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000099 RID: 153
	[SaveableInterface(22001)]
	public interface IFaction
	{
		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x060012F6 RID: 4854
		TextObject Name { get; }

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x060012F7 RID: 4855
		string StringId { get; }

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x060012F8 RID: 4856
		MBGUID Id { get; }

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x060012F9 RID: 4857
		TextObject InformalName { get; }

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x060012FA RID: 4858
		string EncyclopediaLink { get; }

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x060012FB RID: 4859
		TextObject EncyclopediaLinkWithName { get; }

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x060012FC RID: 4860
		TextObject EncyclopediaText { get; }

		// Token: 0x1700050A RID: 1290
		// (get) Token: 0x060012FD RID: 4861
		CultureObject Culture { get; }

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x060012FE RID: 4862
		Settlement InitialHomeSettlement { get; }

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x060012FF RID: 4863
		uint Color { get; }

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06001300 RID: 4864
		uint Color2 { get; }

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06001301 RID: 4865
		CharacterObject BasicTroop { get; }

		// Token: 0x1700050F RID: 1295
		// (get) Token: 0x06001302 RID: 4866
		Hero Leader { get; }

		// Token: 0x17000510 RID: 1296
		// (get) Token: 0x06001303 RID: 4867
		Banner Banner { get; }

		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06001304 RID: 4868
		MBReadOnlyList<Settlement> Settlements { get; }

		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06001305 RID: 4869
		MBReadOnlyList<Town> Fiefs { get; }

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06001306 RID: 4870
		MBReadOnlyList<Hero> AliveLords { get; }

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001307 RID: 4871
		MBReadOnlyList<Hero> DeadLords { get; }

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x06001308 RID: 4872
		MBReadOnlyList<Hero> Heroes { get; }

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06001309 RID: 4873
		MBReadOnlyList<WarPartyComponent> WarPartyComponents { get; }

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x0600130A RID: 4874
		bool IsBanditFaction { get; }

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x0600130B RID: 4875
		bool IsMinorFaction { get; }

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x0600130C RID: 4876
		bool IsKingdomFaction { get; }

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x0600130D RID: 4877
		bool IsRebelClan { get; }

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x0600130E RID: 4878
		bool IsClan { get; }

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x0600130F RID: 4879
		bool IsOutlaw { get; }

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06001310 RID: 4880
		bool IsMapFaction { get; }

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001311 RID: 4881
		bool HasNavalNavigationCapability { get; }

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06001312 RID: 4882
		IFaction MapFaction { get; }

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06001313 RID: 4883
		float CurrentTotalStrength { get; }

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06001314 RID: 4884
		Settlement FactionMidSettlement { get; }

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06001315 RID: 4885
		float DistanceToClosestNonAllyFortification { get; }

		// Token: 0x06001316 RID: 4886
		bool IsAtWarWith(IFaction other);

		// Token: 0x06001317 RID: 4887
		StanceLink GetStanceWith(IFaction other);

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x06001318 RID: 4888
		MBReadOnlyList<IFaction> FactionsAtWarWith { get; }

		// Token: 0x06001319 RID: 4889
		void UpdateFactionsAtWarWith();

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x0600131A RID: 4890
		// (set) Token: 0x0600131B RID: 4891
		int TributeWallet { get; set; }

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x0600131C RID: 4892
		// (set) Token: 0x0600131D RID: 4893
		float MainHeroCrimeRating { get; set; }

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x0600131E RID: 4894
		float DailyCrimeRatingChange { get; }

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x0600131F RID: 4895
		float Aggressiveness { get; }

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x06001320 RID: 4896
		bool IsEliminated { get; }

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06001321 RID: 4897
		ExplainedNumber DailyCrimeRatingChangeExplained { get; }

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06001322 RID: 4898
		// (set) Token: 0x06001323 RID: 4899
		CampaignTime NotAttackableByPlayerUntilTime { get; set; }
	}
}
