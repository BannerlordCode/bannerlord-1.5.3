using System;

namespace TaleWorlds.Core
{
	// Token: 0x020000C4 RID: 196
	public abstract class PeerComponent : IEntityComponent
	{
		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x000231B4 File Offset: 0x000213B4
		// (set) Token: 0x06000AD3 RID: 2771 RVA: 0x000231BC File Offset: 0x000213BC
		public VirtualPlayer Peer
		{
			get
			{
				return this._peer;
			}
			set
			{
				this._peer = value;
			}
		}

		// Token: 0x06000AD4 RID: 2772 RVA: 0x000231C5 File Offset: 0x000213C5
		public virtual void Initialize()
		{
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x000231C7 File Offset: 0x000213C7
		public string Name
		{
			get
			{
				return this.Peer.UserName;
			}
		}

		// Token: 0x170003BF RID: 959
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x000231D4 File Offset: 0x000213D4
		public bool IsMine
		{
			get
			{
				return this.Peer.IsMine;
			}
		}

		// Token: 0x06000AD7 RID: 2775 RVA: 0x000231E1 File Offset: 0x000213E1
		public T GetComponent<T>() where T : PeerComponent
		{
			return this.Peer.GetComponent<T>();
		}

		// Token: 0x06000AD8 RID: 2776 RVA: 0x000231EE File Offset: 0x000213EE
		public virtual void OnInitialize()
		{
		}

		// Token: 0x06000AD9 RID: 2777 RVA: 0x000231F0 File Offset: 0x000213F0
		public virtual void OnFinalize()
		{
		}

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x000231F2 File Offset: 0x000213F2
		// (set) Token: 0x06000ADB RID: 2779 RVA: 0x000231FA File Offset: 0x000213FA
		public uint TypeId { get; set; }

		// Token: 0x04000606 RID: 1542
		private VirtualPlayer _peer;
	}
}
