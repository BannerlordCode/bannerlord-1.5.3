using System;
using TaleWorlds.Library;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x02000037 RID: 55
	internal class ElementLoadData : VariableLoadData
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000B7B7 File Offset: 0x000099B7
		// (set) Token: 0x0600023C RID: 572 RVA: 0x0000B7BF File Offset: 0x000099BF
		public ContainerLoadData ContainerLoadData { get; private set; }

		// Token: 0x0600023D RID: 573 RVA: 0x0000B7C8 File Offset: 0x000099C8
		internal ElementLoadData(ContainerLoadData containerLoadData, IReader reader)
			: base(containerLoadData.Context, reader)
		{
			this.ContainerLoadData = containerLoadData;
		}
	}
}
