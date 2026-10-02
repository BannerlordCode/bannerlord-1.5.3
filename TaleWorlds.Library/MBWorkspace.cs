using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000072 RID: 114
	public class MBWorkspace<T> where T : IMBCollection, new()
	{
		// Token: 0x0600041A RID: 1050 RVA: 0x0000E8F9 File Offset: 0x0000CAF9
		public T StartUsingWorkspace()
		{
			this._isBeingUsed = true;
			if (this._workspace == null)
			{
				this._workspace = new T();
			}
			return this._workspace;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0000E920 File Offset: 0x0000CB20
		public void StopUsingWorkspace()
		{
			this._isBeingUsed = false;
			this._workspace.Clear();
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x0000E93A File Offset: 0x0000CB3A
		public T GetWorkspace()
		{
			return this._workspace;
		}

		// Token: 0x04000147 RID: 327
		private bool _isBeingUsed;

		// Token: 0x04000148 RID: 328
		private T _workspace;
	}
}
