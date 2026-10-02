using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaleWorlds.Diamond
{
	// Token: 0x0200000A RID: 10
	public class FunctionResultJsonConverter : JsonConverter
	{
		// Token: 0x06000037 RID: 55 RVA: 0x00002858 File Offset: 0x00000A58
		public override bool CanConvert(Type objectType)
		{
			return typeof(FunctionResult).IsAssignableFrom(objectType);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x0000286C File Offset: 0x00000A6C
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			JObject jobject = JObject.Load(reader);
			string text = (string)jobject["_type"];
			Type type;
			if (FunctionResultJsonConverter._knownTypes.TryGetValue(text, out type))
			{
				FunctionResult functionResult = (FunctionResult)Activator.CreateInstance(type);
				serializer.Populate(jobject.CreateReader(), functionResult);
				return functionResult;
			}
			return null;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000039 RID: 57 RVA: 0x000028C9 File Offset: 0x00000AC9
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000028CC File Offset: 0x00000ACC
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JProperty jproperty = new JProperty("_type", value.GetType().FullName);
			JObject jobject = new JObject();
			jobject.Add(jproperty);
			foreach (PropertyInfo propertyInfo in value.GetType().GetProperties())
			{
				if (propertyInfo.CanRead)
				{
					object value2 = propertyInfo.GetValue(value);
					if (value2 != null)
					{
						jobject.Add(propertyInfo.Name, JToken.FromObject(value2, serializer));
					}
				}
			}
			jobject.WriteTo(writer, Array.Empty<JsonConverter>());
		}

		// Token: 0x04000010 RID: 16
		private static readonly Dictionary<string, Type> _knownTypes = (from t in (from a in AppDomain.CurrentDomain.GetAssemblies()
				where !a.GlobalAssemblyCache
				select a).SelectMany<Assembly, Type>((Assembly a) => a.GetTypes())
			where t.IsSubclassOf(typeof(FunctionResult))
			select t).ToDictionary<Type, string, Type>((Type item) => item.FullName, (Type item) => item);
	}
}
