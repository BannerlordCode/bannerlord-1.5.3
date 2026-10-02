using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000E9 RID: 233
	[NullableContext(1)]
	[Nullable(0)]
	public class RegexConverter : JsonConverter
	{
		// Token: 0x06000C63 RID: 3171 RVA: 0x00031F30 File Offset: 0x00030130
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			Regex regex = (Regex)value;
			BsonWriter bsonWriter = writer as BsonWriter;
			if (bsonWriter != null)
			{
				this.WriteBson(bsonWriter, regex);
				return;
			}
			this.WriteJson(writer, regex, serializer);
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x00031F6A File Offset: 0x0003016A
		private bool HasFlag(RegexOptions options, RegexOptions flag)
		{
			return (options & flag) == flag;
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x00031F74 File Offset: 0x00030174
		private void WriteBson(BsonWriter writer, Regex regex)
		{
			string text = null;
			if (this.HasFlag(regex.Options, RegexOptions.IgnoreCase))
			{
				text += "i";
			}
			if (this.HasFlag(regex.Options, RegexOptions.Multiline))
			{
				text += "m";
			}
			if (this.HasFlag(regex.Options, RegexOptions.Singleline))
			{
				text += "s";
			}
			text += "u";
			if (this.HasFlag(regex.Options, RegexOptions.ExplicitCapture))
			{
				text += "x";
			}
			writer.WriteRegex(regex.ToString(), text);
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x0003200C File Offset: 0x0003020C
		private void WriteJson(JsonWriter writer, Regex regex, JsonSerializer serializer)
		{
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Pattern") : "Pattern");
			writer.WriteValue(regex.ToString());
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Options") : "Options");
			serializer.Serialize(writer, regex.Options);
			writer.WriteEndObject();
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x00032088 File Offset: 0x00030288
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			JsonToken tokenType = reader.TokenType;
			if (tokenType == JsonToken.StartObject)
			{
				return this.ReadRegexObject(reader, serializer);
			}
			if (tokenType == JsonToken.String)
			{
				return this.ReadRegexString(reader);
			}
			if (tokenType != JsonToken.Null)
			{
				throw JsonSerializationException.Create(reader, "Unexpected token when reading Regex.");
			}
			return null;
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x000320CC File Offset: 0x000302CC
		private object ReadRegexString(JsonReader reader)
		{
			string text = (string)reader.Value;
			if (text.Length > 0 && text[0] == '/')
			{
				int num = text.LastIndexOf('/');
				if (num > 0)
				{
					string text2 = text.Substring(1, num - 1);
					RegexOptions regexOptions = MiscellaneousUtils.GetRegexOptions(text.Substring(num + 1));
					return new Regex(text2, regexOptions);
				}
			}
			throw JsonSerializationException.Create(reader, "Regex pattern must be enclosed by slashes.");
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x00032134 File Offset: 0x00030334
		private Regex ReadRegexObject(JsonReader reader, JsonSerializer serializer)
		{
			string text = null;
			RegexOptions? regexOptions = null;
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if (tokenType != JsonToken.PropertyName)
				{
					if (tokenType != JsonToken.Comment)
					{
						if (tokenType == JsonToken.EndObject)
						{
							if (text == null)
							{
								throw JsonSerializationException.Create(reader, "Error deserializing Regex. No pattern found.");
							}
							return new Regex(text, regexOptions.GetValueOrDefault());
						}
					}
				}
				else
				{
					string text2 = reader.Value.ToString();
					if (!reader.Read())
					{
						throw JsonSerializationException.Create(reader, "Unexpected end when reading Regex.");
					}
					if (string.Equals(text2, "Pattern", StringComparison.OrdinalIgnoreCase))
					{
						text = (string)reader.Value;
					}
					else if (string.Equals(text2, "Options", StringComparison.OrdinalIgnoreCase))
					{
						regexOptions = new RegexOptions?(serializer.Deserialize<RegexOptions>(reader));
					}
					else
					{
						reader.Skip();
					}
				}
			}
			throw JsonSerializationException.Create(reader, "Unexpected end when reading Regex.");
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x000321FE File Offset: 0x000303FE
		public override bool CanConvert(Type objectType)
		{
			return objectType.Name == "Regex" && this.IsRegex(objectType);
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x0003221B File Offset: 0x0003041B
		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool IsRegex(Type objectType)
		{
			return objectType == typeof(Regex);
		}

		// Token: 0x040003FC RID: 1020
		private const string PatternName = "Pattern";

		// Token: 0x040003FD RID: 1021
		private const string OptionsName = "Options";
	}
}
