using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000EE RID: 238
	[Serializable]
	public class BadgeDataEntry
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x0000536C File Offset: 0x0000356C
		// (set) Token: 0x0600049B RID: 1179 RVA: 0x00005374 File Offset: 0x00003574
		[JsonProperty]
		public PlayerId PlayerId { get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x0600049C RID: 1180 RVA: 0x0000537D File Offset: 0x0000357D
		// (set) Token: 0x0600049D RID: 1181 RVA: 0x00005385 File Offset: 0x00003585
		[JsonProperty]
		public string BadgeId { get; set; }

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x0000538E File Offset: 0x0000358E
		// (set) Token: 0x0600049F RID: 1183 RVA: 0x00005396 File Offset: 0x00003596
		[JsonProperty]
		public string ConditionId { get; set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060004A0 RID: 1184 RVA: 0x0000539F File Offset: 0x0000359F
		// (set) Token: 0x060004A1 RID: 1185 RVA: 0x000053A7 File Offset: 0x000035A7
		[JsonProperty]
		public int Count { get; set; }

		// Token: 0x060004A3 RID: 1187 RVA: 0x000053B8 File Offset: 0x000035B8
		public static Dictionary<ValueTuple<PlayerId, string, string>, int> ToDictionary(List<BadgeDataEntry> entries)
		{
			Dictionary<ValueTuple<PlayerId, string, string>, int> dictionary = new Dictionary<ValueTuple<PlayerId, string, string>, int>();
			if (entries != null)
			{
				foreach (BadgeDataEntry badgeDataEntry in entries)
				{
					dictionary.Add(new ValueTuple<PlayerId, string, string>(badgeDataEntry.PlayerId, badgeDataEntry.BadgeId, badgeDataEntry.ConditionId), badgeDataEntry.Count);
				}
			}
			return dictionary;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0000542C File Offset: 0x0000362C
		public static List<BadgeDataEntry> ToList(Dictionary<ValueTuple<PlayerId, string, string>, int> dictionary)
		{
			List<BadgeDataEntry> list = new List<BadgeDataEntry>();
			if (dictionary != null)
			{
				foreach (KeyValuePair<ValueTuple<PlayerId, string, string>, int> keyValuePair in dictionary)
				{
					list.Add(new BadgeDataEntry
					{
						PlayerId = keyValuePair.Key.Item1,
						BadgeId = keyValuePair.Key.Item2,
						ConditionId = keyValuePair.Key.Item3,
						Count = keyValuePair.Value
					});
				}
			}
			return list;
		}
	}
}
