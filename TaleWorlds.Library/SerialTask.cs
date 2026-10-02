using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200008D RID: 141
	public class SerialTask : ITask
	{
		// Token: 0x0600050D RID: 1293 RVA: 0x00012530 File Offset: 0x00010730
		public SerialTask(SerialTask.DelegateDefinition function)
		{
			this._instance = function;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0001253F File Offset: 0x0001073F
		void ITask.Invoke()
		{
			this._instance();
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0001254C File Offset: 0x0001074C
		void ITask.Wait()
		{
		}

		// Token: 0x04000193 RID: 403
		private SerialTask.DelegateDefinition _instance;

		// Token: 0x020000E3 RID: 227
		// (Invoke) Token: 0x060007A6 RID: 1958
		public delegate void DelegateDefinition();
	}
}
