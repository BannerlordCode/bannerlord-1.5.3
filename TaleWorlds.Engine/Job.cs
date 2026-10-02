using System;

namespace TaleWorlds.Engine
{
	// Token: 0x02000055 RID: 85
	public class Job
	{
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060008B3 RID: 2227 RVA: 0x00006E28 File Offset: 0x00005028
		// (set) Token: 0x060008B4 RID: 2228 RVA: 0x00006E30 File Offset: 0x00005030
		public bool Finished { get; protected set; }

		// Token: 0x060008B5 RID: 2229 RVA: 0x00006E39 File Offset: 0x00005039
		public virtual void DoJob(float dt)
		{
		}
	}
}
