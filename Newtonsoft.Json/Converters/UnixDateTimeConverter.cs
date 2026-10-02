using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000EB RID: 235
	[NullableContext(1)]
	[Nullable(0)]
	public class UnixDateTimeConverter : DateTimeConverterBase
	{
		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x00032524 File Offset: 0x00030724
		// (set) Token: 0x06000C7D RID: 3197 RVA: 0x0003252C File Offset: 0x0003072C
		public bool AllowPreEpoch { get; set; }

		// Token: 0x06000C7E RID: 3198 RVA: 0x00032535 File Offset: 0x00030735
		public UnixDateTimeConverter()
			: this(false)
		{
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0003253E File Offset: 0x0003073E
		public UnixDateTimeConverter(bool allowPreEpoch)
		{
			this.AllowPreEpoch = allowPreEpoch;
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x00032550 File Offset: 0x00030750
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			long num;
			if (value is DateTime)
			{
				num = (long)(((DateTime)value).ToUniversalTime() - UnixDateTimeConverter.UnixEpoch).TotalSeconds;
			}
			else
			{
				if (!(value is DateTimeOffset))
				{
					throw new JsonSerializationException("Expected date object value.");
				}
				num = (long)(((DateTimeOffset)value).ToUniversalTime() - UnixDateTimeConverter.UnixEpoch).TotalSeconds;
			}
			if (!this.AllowPreEpoch && num < 0L)
			{
				throw new JsonSerializationException("Cannot convert date value that is before Unix epoch of 00:00:00 UTC on 1 January 1970.");
			}
			writer.WriteValue(num);
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x000325E4 File Offset: 0x000307E4
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			bool flag = ReflectionUtils.IsNullable(objectType);
			if (reader.TokenType == JsonToken.Null)
			{
				if (!flag)
				{
					throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				return null;
			}
			else
			{
				long num;
				if (reader.TokenType == JsonToken.Integer)
				{
					num = (long)reader.Value;
				}
				else
				{
					if (reader.TokenType != JsonToken.String)
					{
						throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected Integer or String, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
					}
					if (!long.TryParse((string)reader.Value, out num))
					{
						throw JsonSerializationException.Create(reader, "Cannot convert invalid value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
					}
				}
				if (!this.AllowPreEpoch && num < 0L)
				{
					throw JsonSerializationException.Create(reader, "Cannot convert value that is before Unix epoch of 00:00:00 UTC on 1 January 1970 to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
				}
				DateTime dateTime = UnixDateTimeConverter.UnixEpoch.AddSeconds((double)num);
				if ((flag ? Nullable.GetUnderlyingType(objectType) : objectType) == typeof(DateTimeOffset))
				{
					return new DateTimeOffset(dateTime, TimeSpan.Zero);
				}
				return dateTime;
			}
		}

		// Token: 0x04000400 RID: 1024
		internal static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
	}
}
