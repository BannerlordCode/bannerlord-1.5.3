using System;

namespace TaleWorlds.Library.Http
{
	// Token: 0x020000B4 RID: 180
	public interface IHttpRequestTask
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060006C3 RID: 1731
		HttpRequestTaskState State { get; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060006C4 RID: 1732
		bool Successful { get; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060006C5 RID: 1733
		string ResponseData { get; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060006C6 RID: 1734
		Exception Exception { get; }

		// Token: 0x060006C7 RID: 1735
		void Start();
	}
}
