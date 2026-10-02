using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x0200003D RID: 61
	internal abstract class MemberLoadData : VariableLoadData
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000C518 File Offset: 0x0000A718
		// (set) Token: 0x06000269 RID: 617 RVA: 0x0000C520 File Offset: 0x0000A720
		public ObjectLoadData ObjectLoadData { get; private set; }

		// Token: 0x0600026A RID: 618 RVA: 0x0000C529 File Offset: 0x0000A729
		protected MemberLoadData(ObjectLoadData objectLoadData, IReader reader)
			: base(objectLoadData.Context, reader)
		{
			this.ObjectLoadData = objectLoadData;
		}
	}
}
