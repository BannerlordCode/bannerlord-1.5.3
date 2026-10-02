using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Siege
{
	// Token: 0x020002F1 RID: 753
	public interface ISiegeEventSide
	{
		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x06002933 RID: 10547
		SiegeEvent SiegeEvent { get; }

		// Token: 0x06002934 RID: 10548
		IEnumerable<PartyBase> GetInvolvedPartiesForEventType(MapEvent.BattleTypes mapEventType = MapEvent.BattleTypes.Siege);

		// Token: 0x06002935 RID: 10549
		PartyBase GetNextInvolvedPartyForEventType(ref int partyIndex, MapEvent.BattleTypes mapEventType = MapEvent.BattleTypes.Siege);

		// Token: 0x06002936 RID: 10550
		bool HasInvolvedPartyForEventType(PartyBase party, MapEvent.BattleTypes mapEventType = MapEvent.BattleTypes.Siege);

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x06002937 RID: 10551
		SiegeStrategy SiegeStrategy { get; }

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x06002938 RID: 10552
		BattleSideEnum BattleSide { get; }

		// Token: 0x06002939 RID: 10553
		void OnTroopsKilledOnSide(int killCount);

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x0600293A RID: 10554
		int NumberOfTroopsKilledOnSide { get; }

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x0600293B RID: 10555
		SiegeEvent.SiegeEnginesContainer SiegeEngines { get; }

		// Token: 0x0600293C RID: 10556
		void AddSiegeEngineMissile(SiegeEvent.SiegeEngineMissile missile);

		// Token: 0x0600293D RID: 10557
		void RemoveDeprecatedMissiles();

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x0600293E RID: 10558
		MBReadOnlyList<SiegeEvent.SiegeEngineMissile> SiegeEngineMissiles { get; }

		// Token: 0x0600293F RID: 10559
		void SetSiegeStrategy(SiegeStrategy strategy);

		// Token: 0x06002940 RID: 10560
		void InitializeSiegeEventSide();

		// Token: 0x06002941 RID: 10561
		void GetAttackTarget(ISiegeEventSide siegeEventSide, SiegeEngineType siegeEngine, int siegeEngineSlot, out SiegeBombardTargets targetType, out int targetIndex);

		// Token: 0x06002942 RID: 10562
		void FinalizeSiegeEvent();
	}
}
