using System;
using System.Collections.Generic;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x02000023 RID: 35
	public class MaterialPool<T> where T : Material, new()
	{
		// Token: 0x0600015C RID: 348 RVA: 0x0000708F File Offset: 0x0000528F
		public MaterialPool(int initialBufferSize)
		{
			this._materialList = new List<T>(initialBufferSize);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x000070A4 File Offset: 0x000052A4
		public T New()
		{
			if (this._nextAvailableIndex < this._materialList.Count)
			{
				T t = this._materialList[this._nextAvailableIndex];
				this._nextAvailableIndex++;
				return t;
			}
			T t2 = new T();
			this._materialList.Add(t2);
			this._nextAvailableIndex++;
			return t2;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007105 File Offset: 0x00005305
		public void ResetAll()
		{
			this._nextAvailableIndex = 0;
		}

		// Token: 0x040000B4 RID: 180
		private List<T> _materialList;

		// Token: 0x040000B5 RID: 181
		private int _nextAvailableIndex;
	}
}
