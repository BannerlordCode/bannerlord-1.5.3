using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace TaleWorlds.Diamond
{
	// Token: 0x02000021 RID: 33
	public class MessageJsonConverter : JsonConverter
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00002E28 File Offset: 0x00001028
		public override bool CanConvert(Type objectType)
		{
			return typeof(Message).IsAssignableFrom(objectType);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00002E3C File Offset: 0x0000103C
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			serializer.Error += this.OnSerializerError;
			JObject jobject = JObject.Load(reader);
			string text = (string)jobject["_type"];
			Type type;
			if (text.StartsWith("Messages.") && MessageJsonConverter._knownTypes.TryGetValue(text, out type))
			{
				Message message = (Message)Activator.CreateInstance(type);
				serializer.Populate(jobject.CreateReader(), message);
				return message;
			}
			return null;
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00002EAD File Offset: 0x000010AD
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00002EB0 File Offset: 0x000010B0
		private void OnSerializerError(object sender, ErrorEventArgs e)
		{
			if (e.ErrorContext != null)
			{
				Exception error = e.ErrorContext.Error;
			}
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00002EC8 File Offset: 0x000010C8
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

		// Token: 0x04000036 RID: 54
		private static readonly Dictionary<string, Type> _knownTypes = (from t in (from a in AppDomain.CurrentDomain.GetAssemblies()
				where !a.GlobalAssemblyCache
				select a).SelectMany<Assembly, Type>((Assembly a) => a.GetTypes())
			where t.IsSubclassOf(typeof(Message))
			select t).ToDictionary<Type, string, Type>((Type item) => item.FullName, (Type item) => item);
	}
}
