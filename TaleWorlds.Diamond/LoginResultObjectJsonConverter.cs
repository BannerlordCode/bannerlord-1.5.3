using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaleWorlds.Diamond
{
	// Token: 0x0200001E RID: 30
	public class LoginResultObjectJsonConverter : JsonConverter
	{
		// Token: 0x060000A5 RID: 165 RVA: 0x00002C58 File Offset: 0x00000E58
		public override bool CanConvert(Type objectType)
		{
			return typeof(LoginResultObject).IsAssignableFrom(objectType);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00002C6C File Offset: 0x00000E6C
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jobject = JObject.Load(reader);
			string text = (string)jobject["_type"];
			Type type;
			if (LoginResultObjectJsonConverter._knownTypes.TryGetValue(text, out type))
			{
				LoginResultObject loginResultObject = (LoginResultObject)Activator.CreateInstance(type);
				serializer.Populate(jobject.CreateReader(), loginResultObject);
				return loginResultObject;
			}
			return null;
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x00002CBD File Offset: 0x00000EBD
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00002CC0 File Offset: 0x00000EC0
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

		// Token: 0x04000032 RID: 50
		private static readonly Dictionary<string, Type> _knownTypes = (from t in (from a in AppDomain.CurrentDomain.GetAssemblies()
				where !a.GlobalAssemblyCache
				select a).SelectMany<Assembly, Type>((Assembly a) => a.GetTypes())
			where t.IsSubclassOf(typeof(LoginResultObject))
			select t).ToDictionary<Type, string, Type>((Type item) => item.FullName, (Type item) => item);
	}
}
