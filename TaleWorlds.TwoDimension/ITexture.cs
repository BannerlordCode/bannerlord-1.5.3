using System;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000035 RID: 53
	public interface ITexture
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000266 RID: 614
		bool IsValid { get; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000267 RID: 615
		int Width { get; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000268 RID: 616
		int Height { get; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000269 RID: 617
		// (set) Token: 0x0600026A RID: 618
		string Name { get; set; }

		// Token: 0x0600026B RID: 619
		void Release();

		// Token: 0x0600026C RID: 620
		bool IsLoaded();
	}
}
