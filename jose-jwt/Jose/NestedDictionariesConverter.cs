using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Jose
{
	// Token: 0x02000019 RID: 25
	internal class NestedDictionariesConverter : CustomCreationConverter<IDictionary<string, object>>
	{
		// Token: 0x06000072 RID: 114 RVA: 0x00004243 File Offset: 0x00002443
		public override IDictionary<string, object> Create(Type objectType)
		{
			return new Dictionary<string, object>();
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000424A File Offset: 0x0000244A
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(object) || base.CanConvert(objectType);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004262 File Offset: 0x00002462
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.StartObject || reader.TokenType == JsonToken.Null)
			{
				return base.ReadJson(reader, objectType, existingValue, serializer);
			}
			return serializer.Deserialize(reader);
		}
	}
}
