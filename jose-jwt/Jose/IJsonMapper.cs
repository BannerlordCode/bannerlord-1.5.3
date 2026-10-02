using System;

namespace Jose
{
	// Token: 0x02000017 RID: 23
	public interface IJsonMapper
	{
		// Token: 0x0600006D RID: 109
		string Serialize(object obj);

		// Token: 0x0600006E RID: 110
		T Parse<T>(string json);
	}
}
