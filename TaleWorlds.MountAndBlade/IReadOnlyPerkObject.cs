using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200031B RID: 795
	public interface IReadOnlyPerkObject
	{
		// Token: 0x1700086C RID: 2156
		// (get) Token: 0x06002D8B RID: 11659
		TextObject Name { get; }

		// Token: 0x1700086D RID: 2157
		// (get) Token: 0x06002D8C RID: 11660
		TextObject Description { get; }

		// Token: 0x1700086E RID: 2158
		// (get) Token: 0x06002D8D RID: 11661
		List<string> GameModes { get; }

		// Token: 0x1700086F RID: 2159
		// (get) Token: 0x06002D8E RID: 11662
		int PerkListIndex { get; }

		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06002D8F RID: 11663
		string IconId { get; }

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06002D90 RID: 11664
		string HeroIdleAnimOverride { get; }

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06002D91 RID: 11665
		string HeroMountIdleAnimOverride { get; }

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06002D92 RID: 11666
		string TroopIdleAnimOverride { get; }

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06002D93 RID: 11667
		string TroopMountIdleAnimOverride { get; }

		// Token: 0x06002D94 RID: 11668
		int GetExtraTroopCount(bool isWarmup);

		// Token: 0x06002D95 RID: 11669
		List<ValueTuple<EquipmentIndex, EquipmentElement>> GetAlternativeEquipments(bool isWarmup, bool isPlayer, List<ValueTuple<EquipmentIndex, EquipmentElement>> alternativeEquipments, bool getAllEquipments = false);

		// Token: 0x06002D96 RID: 11670
		float GetDrivenPropertyBonusOnSpawn(bool isWarmup, bool isPlayer, DrivenProperty drivenProperty, float baseValue);

		// Token: 0x06002D97 RID: 11671
		float GetHitpoints(bool isWarmup, bool isPlayer);

		// Token: 0x06002D98 RID: 11672
		MPPerkObject Clone(MissionPeer peer);
	}
}
