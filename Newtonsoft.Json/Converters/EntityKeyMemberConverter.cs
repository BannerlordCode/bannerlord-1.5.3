using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x020000E4 RID: 228
	[NullableContext(1)]
	[Nullable(0)]
	public class EntityKeyMemberConverter : JsonConverter
	{
		// Token: 0x06000C43 RID: 3139 RVA: 0x000314E0 File Offset: 0x0002F6E0
		public override void WriteJson(JsonWriter writer, [Nullable(2)] object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
				return;
			}
			EntityKeyMemberConverter.EnsureReflectionObject(value.GetType());
			DefaultContractResolver defaultContractResolver = serializer.ContractResolver as DefaultContractResolver;
			string text = (string)EntityKeyMemberConverter._reflectionObject.GetValue(value, "Key");
			object value2 = EntityKeyMemberConverter._reflectionObject.GetValue(value, "Value");
			Type type = ((value2 != null) ? value2.GetType() : null);
			writer.WriteStartObject();
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Key") : "Key");
			writer.WriteValue(text);
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Type") : "Type");
			writer.WriteValue((type != null) ? type.FullName : null);
			writer.WritePropertyName((defaultContractResolver != null) ? defaultContractResolver.GetResolvedPropertyName("Value") : "Value");
			if (type != null)
			{
				string text2;
				if (JsonSerializerInternalWriter.TryConvertToString(value2, type, out text2))
				{
					writer.WriteValue(text2);
				}
				else
				{
					writer.WriteValue(value2);
				}
			}
			else
			{
				writer.WriteNull();
			}
			writer.WriteEndObject();
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x000315E8 File Offset: 0x0002F7E8
		private static void ReadAndAssertProperty(JsonReader reader, string propertyName)
		{
			reader.ReadAndAssert();
			if (reader.TokenType == JsonToken.PropertyName)
			{
				object value = reader.Value;
				if (string.Equals((value != null) ? value.ToString() : null, propertyName, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
			}
			throw new JsonSerializationException("Expected JSON property '{0}'.".FormatWith(CultureInfo.InvariantCulture, propertyName));
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00031638 File Offset: 0x0002F838
		[return: Nullable(2)]
		public override object ReadJson(JsonReader reader, Type objectType, [Nullable(2)] object existingValue, JsonSerializer serializer)
		{
			EntityKeyMemberConverter.EnsureReflectionObject(objectType);
			object obj = EntityKeyMemberConverter._reflectionObject.Creator(Array.Empty<object>());
			EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Key");
			reader.ReadAndAssert();
			ReflectionObject reflectionObject = EntityKeyMemberConverter._reflectionObject;
			object obj2 = obj;
			string text = "Key";
			object value = reader.Value;
			reflectionObject.SetValue(obj2, text, (value != null) ? value.ToString() : null);
			EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Type");
			reader.ReadAndAssert();
			object value2 = reader.Value;
			Type type = Type.GetType((value2 != null) ? value2.ToString() : null);
			EntityKeyMemberConverter.ReadAndAssertProperty(reader, "Value");
			reader.ReadAndAssert();
			EntityKeyMemberConverter._reflectionObject.SetValue(obj, "Value", serializer.Deserialize(reader, type));
			reader.ReadAndAssert();
			return obj;
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x000316ED File Offset: 0x0002F8ED
		private static void EnsureReflectionObject(Type objectType)
		{
			if (EntityKeyMemberConverter._reflectionObject == null)
			{
				EntityKeyMemberConverter._reflectionObject = ReflectionObject.Create(objectType, new string[] { "Key", "Value" });
			}
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00031717 File Offset: 0x0002F917
		public override bool CanConvert(Type objectType)
		{
			return objectType.AssignableToTypeName("System.Data.EntityKeyMember", false);
		}

		// Token: 0x040003F0 RID: 1008
		private const string EntityKeyMemberFullTypeName = "System.Data.EntityKeyMember";

		// Token: 0x040003F1 RID: 1009
		private const string KeyPropertyName = "Key";

		// Token: 0x040003F2 RID: 1010
		private const string TypePropertyName = "Type";

		// Token: 0x040003F3 RID: 1011
		private const string ValuePropertyName = "Value";

		// Token: 0x040003F4 RID: 1012
		[Nullable(2)]
		private static ReflectionObject _reflectionObject;
	}
}
