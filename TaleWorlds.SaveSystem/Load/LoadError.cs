using System;

namespace TaleWorlds.SaveSystem.Load
{
	// Token: 0x0200003B RID: 59
	public class LoadError
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000C455 File Offset: 0x0000A655
		// (set) Token: 0x06000259 RID: 601 RVA: 0x0000C45D File Offset: 0x0000A65D
		public string Message { get; private set; }

		// Token: 0x0600025A RID: 602 RVA: 0x0000C466 File Offset: 0x0000A666
		internal LoadError(string message)
		{
			this.Message = message;
		}
	}
}
