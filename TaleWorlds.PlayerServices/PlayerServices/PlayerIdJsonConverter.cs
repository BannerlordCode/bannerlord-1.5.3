using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaleWorlds.PlayerServices
{
	// Token: 0x02000004 RID: 4
	public class PlayerIdJsonConverter : JsonConverter
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00002933 File Offset: 0x00000B33
		public override bool CanConvert(Type objectType)
		{
			return typeof(PlayerId).IsAssignableFrom(objectType);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002945 File Offset: 0x00000B45
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return PlayerId.FromString((string)JObject.Load(reader)["_playerId"]);
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002966 File Offset: 0x00000B66
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x0000296C File Offset: 0x00000B6C
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JProperty jproperty = new JProperty("_playerId", ((PlayerId)value).ToString());
			new JObject { jproperty }.WriteTo(writer, Array.Empty<JsonConverter>());
		}
	}
}
