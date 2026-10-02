using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x0200016F RID: 367
	public class KillTracker : GameBadgeTracker
	{
		// Token: 0x06000A43 RID: 2627 RVA: 0x00010620 File Offset: 0x0000E820
		public KillTracker(string badgeId, BadgeCondition condition, Dictionary<ValueTuple<PlayerId, string, string>, int> dataDictionary)
		{
			this._badgeId = badgeId;
			this._condition = condition;
			this._dataDictionary = dataDictionary;
			this._faction = null;
			this._troop = null;
			string text;
			if (condition.Parameters.TryGetValue("faction", out text))
			{
				this._faction = text;
			}
			string text2;
			if (condition.Parameters.TryGetValue("troop", out text2))
			{
				this._troop = text2;
			}
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0001068C File Offset: 0x0000E88C
		public override void OnKill(KillData killData)
		{
			if (killData.KillerId.IsValid && killData.VictimId.IsValid && !killData.KillerId.Equals(killData.VictimId) && (this._faction == null || this._faction == killData.KillerFaction) && (this._troop == null || this._troop == killData.KillerTroop))
			{
				int num;
				if (!this._dataDictionary.TryGetValue(new ValueTuple<PlayerId, string, string>(killData.KillerId, this._badgeId, this._condition.StringId), out num))
				{
					num = 0;
				}
				this._dataDictionary[new ValueTuple<PlayerId, string, string>(killData.KillerId, this._badgeId, this._condition.StringId)] = num + 1;
			}
		}

		// Token: 0x04000525 RID: 1317
		private readonly string _badgeId;

		// Token: 0x04000526 RID: 1318
		private readonly BadgeCondition _condition;

		// Token: 0x04000527 RID: 1319
		private readonly Dictionary<ValueTuple<PlayerId, string, string>, int> _dataDictionary;

		// Token: 0x04000528 RID: 1320
		private readonly string _faction;

		// Token: 0x04000529 RID: 1321
		private readonly string _troop;
	}
}
