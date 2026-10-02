using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000E5 RID: 229
	[NullableContext(1)]
	[Nullable(0)]
	public class ExpandoObjectConverter : JsonConverter
	{
		// Token: 0x06000C49 RID: 3145 RVA: 0x0003172D File Offset: 0x0002F92D
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x0003172F File Offset: 0x0002F92F
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			return this.ReadValue(reader);
		}

		// Token: 0x06000C4B RID: 3147 RVA: 0x00031738 File Offset: 0x0002F938
		[return: Nullable(2)]
		private object ReadValue(JsonReader reader)
		{
			if (!reader.MoveToContent())
			{
				throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
			}
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.StartObject)
			{
				return this.ReadObject(reader);
			}
			if (tokenType == JsonToken.StartArray)
			{
				return this.ReadList(reader);
			}
			if (JsonTokenUtils.IsPrimitiveToken(reader.TokenType))
			{
				return reader.Value;
			}
			throw JsonSerializationException.Create(reader, "Unexpected token when converting ExpandoObject: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
		}

		// Token: 0x06000C4C RID: 3148 RVA: 0x000317B0 File Offset: 0x0002F9B0
		private object ReadList(JsonReader reader)
		{
			IList<object> list = new List<object>();
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.Comment)
				{
					if (tokenType == JsonToken.EndArray)
					{
						return list;
					}
					object obj = this.ReadValue(reader);
					list.Add(obj);
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
		}

		// Token: 0x06000C4D RID: 3149 RVA: 0x000317FC File Offset: 0x0002F9FC
		private object ReadObject(JsonReader reader)
		{
			IDictionary<string, object> dictionary = new ExpandoObject();
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.PropertyName)
				{
					if (tokenType != JsonToken.Comment)
					{
						if (tokenType == JsonToken.EndObject)
						{
							return dictionary;
						}
					}
				}
				else
				{
					string text = reader.Value.ToString();
					if (!reader.Read())
					{
						throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
					}
					object obj = this.ReadValue(reader);
					dictionary[text] = obj;
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading ExpandoObject.");
		}

		// Token: 0x06000C4E RID: 3150 RVA: 0x0003186E File Offset: 0x0002FA6E
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ExpandoObject);
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x00031880 File Offset: 0x0002FA80
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}
	}
}
