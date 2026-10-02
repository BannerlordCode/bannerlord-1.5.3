using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TaleWorlds.Diamond.Rest
{
	// Token: 0x02000033 RID: 51
	public class RestDataJsonConverter : JsonConverter<RestData>
	{
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00003E27 File Offset: 0x00002027
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00003E2A File Offset: 0x0000202A
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00003E30 File Offset: 0x00002030
		private RestData Create(Type objectType, JObject jObject)
		{
			if (jObject == null)
			{
				throw new ArgumentNullException("jObject");
			}
			string text = null;
			if (jObject["TypeName"] != null)
			{
				text = jObject["TypeName"].Value<string>();
			}
			else if (jObject["typeName"] != null)
			{
				text = jObject["typeName"].Value<string>();
			}
			if (text != null)
			{
				return Activator.CreateInstance(Type.GetType(text)) as RestData;
			}
			return null;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00003EA0 File Offset: 0x000020A0
		public T ReadJson<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00003EA8 File Offset: 0x000020A8
		public override RestData ReadJson(JsonReader reader, Type objectType, RestData existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			if (serializer == null)
			{
				throw new ArgumentNullException("serializer");
			}
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			JObject jobject = JObject.Load(reader);
			RestData restData = this.Create(objectType, jobject);
			serializer.Populate(jobject.CreateReader(), restData);
			return restData;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00003EFD File Offset: 0x000020FD
		public override void WriteJson(JsonWriter writer, RestData value, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}
