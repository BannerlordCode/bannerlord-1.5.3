using System;
using System.Collections.Generic;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x0200016B RID: 363
	public class BadgeOwnerKillTracker : GameBadgeTracker
	{
		// Token: 0x06000A2C RID: 2604 RVA: 0x00010370 File Offset: 0x0000E570
		public BadgeOwnerKillTracker(string badgeId, BadgeCondition condition, Dictionary<ValueTuple<PlayerId, string, string>, int> dataDictionary)
		{
			this._badgeId = badgeId;
			this._condition = condition;
			this._playerBadgeMap = new Dictionary<PlayerId, bool>();
			this._dataDictionary = dataDictionary;
			this._requiredBadges = new List<string>();
			foreach (KeyValuePair<string, string> keyValuePair in condition.Parameters)
			{
				if (keyValuePair.Key.StartsWith("required_badge."))
				{
					this._requiredBadges.Add(keyValuePair.Value);
				}
			}
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0001040C File Offset: 0x0000E60C
		public override void OnPlayerJoin(PlayerData playerData)
		{
			this._playerBadgeMap[playerData.PlayerId] = this._requiredBadges.Contains(playerData.ShownBadgeId);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x00010430 File Offset: 0x0000E630
		public override void OnKill(KillData killData)
		{
			bool flag;
			if (killData.KillerId.IsValid && killData.VictimId.IsValid && !killData.KillerId.Equals(killData.VictimId) && this._playerBadgeMap.TryGetValue(killData.VictimId, out flag) && flag)
			{
				this._playerBadgeMap[killData.KillerId] = true;
				int num;
				if (!this._dataDictionary.TryGetValue(new ValueTuple<PlayerId, string, string>(killData.KillerId, this._badgeId, this._condition.StringId), out num))
				{
					num = 0;
				}
				this._dataDictionary[new ValueTuple<PlayerId, string, string>(killData.KillerId, this._badgeId, this._condition.StringId)] = num + 1;
			}
		}

		// Token: 0x04000519 RID: 1305
		private readonly string _badgeId;

		// Token: 0x0400051A RID: 1306
		private readonly BadgeCondition _condition;

		// Token: 0x0400051B RID: 1307
		private readonly List<string> _requiredBadges;

		// Token: 0x0400051C RID: 1308
		private readonly Dictionary<ValueTuple<PlayerId, string, string>, int> _dataDictionary;

		// Token: 0x0400051D RID: 1309
		private readonly Dictionary<PlayerId, bool> _playerBadgeMap;
	}
}
