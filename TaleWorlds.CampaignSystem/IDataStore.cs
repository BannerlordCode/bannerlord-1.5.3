using System;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x02000037 RID: 55
	public interface IDataStore
	{
		// Token: 0x06000383 RID: 899
		bool SyncData<T>(string key, ref T data);

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000384 RID: 900
		bool IsSaving { get; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000385 RID: 901
		bool IsLoading { get; }
	}
}
