using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using TaleWorlds.Diamond;
using TaleWorlds.PlayerServices;

namespace Messages.FromBattleServer.ToBattleServerManager
{
	// Token: 0x020000D8 RID: 216
	[MessageDescription("BattleServer", "BattleServerManager", false)]
	[Serializable]
	public class FriendlyDamageKickPlayerMessage : Message
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00004C92 File Offset: 0x00002E92
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x00004C9A File Offset: 0x00002E9A
		[JsonProperty]
		public PlayerId PlayerId { get; private set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00004CA3 File Offset: 0x00002EA3
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x00004CAB File Offset: 0x00002EAB
		[TupleElementNames(new string[] { "killCount", "damage" })]
		[JsonProperty]
		public Dictionary<int, ValueTuple<int, float>> RoundDamageMap
		{
			[return: TupleElementNames(new string[] { "killCount", "damage" })]
			get;
			[param: TupleElementNames(new string[] { "killCount", "damage" })]
			private set;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00004CB4 File Offset: 0x00002EB4
		public FriendlyDamageKickPlayerMessage()
		{
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00004CBC File Offset: 0x00002EBC
		public FriendlyDamageKickPlayerMessage(PlayerId playerId, [TupleElementNames(new string[] { "killCount", "damage" })] Dictionary<int, ValueTuple<int, float>> roundDamageMap)
		{
			this.PlayerId = playerId;
			this.RoundDamageMap = roundDamageMap;
		}
	}
}
