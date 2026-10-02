using System;
using Newtonsoft.Json;

namespace Jose
{
	// Token: 0x02000018 RID: 24
	public class NewtonsoftMapper : IJsonMapper
	{
		// Token: 0x0600006F RID: 111 RVA: 0x00004224 File Offset: 0x00002424
		public string Serialize(object obj)
		{
			return JsonConvert.SerializeObject(obj, Formatting.None);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000422D File Offset: 0x0000242D
		public T Parse<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json, new JsonConverter[]
			{
				new NestedDictionariesConverter()
			});
		}
	}
}
