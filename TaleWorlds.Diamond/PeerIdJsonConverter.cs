using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000023 RID: 35
	public class PeerIdJsonConverter : JsonConverter
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x000032E9 File Offset: 0x000014E9
		public override bool CanConvert(Type objectType)
		{
			return typeof(PeerId).IsAssignableFrom(objectType);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x000032FB File Offset: 0x000014FB
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return PeerId.FromString((string)JObject.Load(reader)["_peerId"]);
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x0000331C File Offset: 0x0000151C
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00003320 File Offset: 0x00001520
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JProperty jproperty = new JProperty("_peerId", ((PeerId)value).ToString());
			new JObject { jproperty }.WriteTo(writer, Array.Empty<JsonConverter>());
		}
	}
}
