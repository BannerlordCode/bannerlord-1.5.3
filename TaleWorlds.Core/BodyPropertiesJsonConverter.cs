using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaleWorlds.Core
{
	// Token: 0x02000023 RID: 35
	public class BodyPropertiesJsonConverter : JsonConverter
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x00007177 File Offset: 0x00005377
		public override bool CanConvert(Type objectType)
		{
			return typeof(BodyProperties).IsAssignableFrom(objectType);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000718C File Offset: 0x0000538C
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			BodyProperties bodyProperties;
			BodyProperties.FromString((string)JObject.Load(reader)["_data"], out bodyProperties);
			return bodyProperties;
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001AB RID: 427 RVA: 0x000071BC File Offset: 0x000053BC
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000071C0 File Offset: 0x000053C0
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JProperty jproperty = new JProperty("_data", ((BodyProperties)value).ToString());
			new JObject { jproperty }.WriteTo(writer, Array.Empty<JsonConverter>());
		}
	}
}
