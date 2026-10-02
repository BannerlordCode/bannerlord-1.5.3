using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TaleWorlds.Diamond;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x0200014B RID: 331
	public class PlayerStatsBaseJsonConverter : JsonConverter
	{
		// Token: 0x0600093A RID: 2362 RVA: 0x0000D92A File Offset: 0x0000BB2A
		public override bool CanConvert(Type objectType)
		{
			return typeof(AccessObject).IsAssignableFrom(objectType);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x0000D93C File Offset: 0x0000BB3C
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jobject = JObject.Load(reader);
			string text = (string)jobject["gameType"];
			if (text == null)
			{
				text = (string)jobject["GameType"];
			}
			PlayerStatsBase playerStatsBase;
			if (text == "Skirmish")
			{
				playerStatsBase = new PlayerStatsSkirmish();
			}
			else if (text == "Captain")
			{
				playerStatsBase = new PlayerStatsCaptain();
			}
			else if (text == "TeamDeathmatch")
			{
				playerStatsBase = new PlayerStatsTeamDeathmatch();
			}
			else if (text == "Siege")
			{
				playerStatsBase = new PlayerStatsSiege();
			}
			else if (text == "Duel")
			{
				playerStatsBase = new PlayerStatsDuel();
			}
			else
			{
				if (!(text == "Battle"))
				{
					return null;
				}
				playerStatsBase = new PlayerStatsBattle();
			}
			serializer.Populate(jobject.CreateReader(), playerStatsBase);
			return playerStatsBase;
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x0600093C RID: 2364 RVA: 0x0000DA04 File Offset: 0x0000BC04
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x0000DA07 File Offset: 0x0000BC07
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}
	}
}
