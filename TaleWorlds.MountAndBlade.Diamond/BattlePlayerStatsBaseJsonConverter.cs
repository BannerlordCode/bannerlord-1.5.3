using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x020000F4 RID: 244
	public class BattlePlayerStatsBaseJsonConverter : JsonConverter
	{
		// Token: 0x060004E0 RID: 1248 RVA: 0x00005748 File Offset: 0x00003948
		public override bool CanConvert(Type objectType)
		{
			return typeof(BattlePlayerStatsBase).IsAssignableFrom(objectType);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x0000575C File Offset: 0x0000395C
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			JObject jobject = JObject.Load(reader);
			string text = (string)jobject["GameType"];
			BattlePlayerStatsBase battlePlayerStatsBase;
			if (text == "Skirmish")
			{
				battlePlayerStatsBase = new BattlePlayerStatsSkirmish();
			}
			else if (text == "Captain")
			{
				battlePlayerStatsBase = new BattlePlayerStatsCaptain();
			}
			else if (text == "Siege")
			{
				battlePlayerStatsBase = new BattlePlayerStatsSiege();
			}
			else if (text == "TeamDeathmatch")
			{
				battlePlayerStatsBase = new BattlePlayerStatsTeamDeathmatch();
			}
			else if (text == "Duel")
			{
				battlePlayerStatsBase = new BattlePlayerStatsDuel();
			}
			else
			{
				if (!(text == "Battle"))
				{
					return null;
				}
				battlePlayerStatsBase = new BattlePlayerStatsBattle();
			}
			serializer.Populate(jobject.CreateReader(), battlePlayerStatsBase);
			return battlePlayerStatsBase;
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x0000581C File Offset: 0x00003A1C
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0000581F File Offset: 0x00003A1F
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}
	}
}
