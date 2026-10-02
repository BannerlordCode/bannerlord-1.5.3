using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200006B RID: 107
	public abstract class GameHandler : IEntityComponent
	{
		// Token: 0x0600079F RID: 1951 RVA: 0x00019F59 File Offset: 0x00018159
		void IEntityComponent.OnInitialize()
		{
			this.OnInitialize();
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x00019F61 File Offset: 0x00018161
		void IEntityComponent.OnFinalize()
		{
			this.OnFinalize();
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x00019F69 File Offset: 0x00018169
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00019F6B File Offset: 0x0001816B
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00019F6D File Offset: 0x0001816D
		protected internal virtual void OnTick(float dt)
		{
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00019F6F File Offset: 0x0001816F
		protected internal virtual void OnGameStart()
		{
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00019F71 File Offset: 0x00018171
		protected internal virtual void OnGameEnd()
		{
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00019F73 File Offset: 0x00018173
		protected internal virtual void OnGameNetworkBegin()
		{
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00019F75 File Offset: 0x00018175
		protected internal virtual void OnGameNetworkEnd()
		{
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00019F77 File Offset: 0x00018177
		protected internal virtual void OnEarlyPlayerConnect(VirtualPlayer peer)
		{
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00019F79 File Offset: 0x00018179
		protected internal virtual void OnPlayerConnect(VirtualPlayer peer)
		{
		}

		// Token: 0x060007AA RID: 1962 RVA: 0x00019F7B File Offset: 0x0001817B
		protected internal virtual void OnPlayerDisconnect(VirtualPlayer peer)
		{
		}

		// Token: 0x060007AB RID: 1963
		public abstract void OnBeforeSave();

		// Token: 0x060007AC RID: 1964
		public abstract void OnAfterSave();
	}
}
