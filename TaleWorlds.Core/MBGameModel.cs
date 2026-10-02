using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000AE RID: 174
	public abstract class MBGameModel<T> : GameModel where T : GameModel
	{
		// Token: 0x17000323 RID: 803
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0001E1C6 File Offset: 0x0001C3C6
		// (set) Token: 0x0600092B RID: 2347 RVA: 0x0001E1CE File Offset: 0x0001C3CE
		private protected T BaseModel { protected get; private set; }

		// Token: 0x0600092C RID: 2348 RVA: 0x0001E1D7 File Offset: 0x0001C3D7
		public void Initialize(T baseModel)
		{
			this.BaseModel = baseModel;
		}
	}
}
