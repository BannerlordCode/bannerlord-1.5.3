using System;

namespace TaleWorlds.Core
{
	// Token: 0x0200006F RID: 111
	public abstract class GameManagerComponent : IEntityComponent
	{
		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x0001A408 File Offset: 0x00018608
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x0001A410 File Offset: 0x00018610
		public GameManagerBase GameManager { get; internal set; }

		// Token: 0x060007DD RID: 2013 RVA: 0x0001A419 File Offset: 0x00018619
		void IEntityComponent.OnInitialize()
		{
			this.OnInitialize();
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0001A421 File Offset: 0x00018621
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x0001A423 File Offset: 0x00018623
		void IEntityComponent.OnFinalize()
		{
			this.OnFinalize();
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x0001A42B File Offset: 0x0001862B
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0001A42D File Offset: 0x0001862D
		protected internal virtual void OnTick()
		{
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x0001A42F File Offset: 0x0001862F
		protected internal virtual void OnPlayerDisconnect(VirtualPlayer peer)
		{
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0001A431 File Offset: 0x00018631
		protected internal virtual void OnEarlyPlayerConnect(VirtualPlayer peer)
		{
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0001A433 File Offset: 0x00018633
		protected internal virtual void OnPlayerConnect(VirtualPlayer peer)
		{
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x0001A435 File Offset: 0x00018635
		protected internal virtual void OnGameNetworkBegin()
		{
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0001A437 File Offset: 0x00018637
		protected internal virtual void OnGameNetworkEnd()
		{
		}
	}
}
