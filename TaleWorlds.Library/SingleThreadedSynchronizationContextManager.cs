using System;
using System.Threading;

namespace TaleWorlds.Library
{
	// Token: 0x0200008F RID: 143
	public static class SingleThreadedSynchronizationContextManager
	{
		// Token: 0x06000514 RID: 1300 RVA: 0x00012718 File Offset: 0x00010918
		public static void Initialize()
		{
			if (SingleThreadedSynchronizationContextManager._synchronizationContext == null)
			{
				SingleThreadedSynchronizationContextManager._synchronizationContext = new SingleThreadedSynchronizationContext();
				SynchronizationContext.SetSynchronizationContext(SingleThreadedSynchronizationContextManager._synchronizationContext);
			}
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00012735 File Offset: 0x00010935
		public static void Tick()
		{
			SingleThreadedSynchronizationContextManager._synchronizationContext.Tick();
		}

		// Token: 0x04000198 RID: 408
		private static SingleThreadedSynchronizationContext _synchronizationContext;
	}
}
