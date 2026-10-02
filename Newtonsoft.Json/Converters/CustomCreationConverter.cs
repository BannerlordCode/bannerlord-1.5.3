using System;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000DF RID: 223
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class CustomCreationConverter<[Nullable(2)] T> : JsonConverter
	{
		// Token: 0x06000C2A RID: 3114 RVA: 0x00030929 File Offset: 0x0002EB29
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			throw new NotSupportedException("CustomCreationConverter should only be used while deserializing.");
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x00030938 File Offset: 0x0002EB38
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			T t = this.Create(objectType);
			if (t == null)
			{
				throw new JsonSerializationException("No object created.");
			}
			serializer.Populate(reader, t);
			return t;
		}

		// Token: 0x06000C2C RID: 3116
		public abstract T Create(Type objectType);

		// Token: 0x06000C2D RID: 3117 RVA: 0x00030980 File Offset: 0x0002EB80
		public override bool CanConvert(Type objectType)
		{
			return typeof(T).IsAssignableFrom(objectType);
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x00030992 File Offset: 0x0002EB92
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}
	}
}
