using System;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.Core
{
	// Token: 0x02000057 RID: 87
	public class DummyCommunicator : ICommunicator
	{
		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x000183ED File Offset: 0x000165ED
		public VirtualPlayer VirtualPlayer { get; }

		// Token: 0x060006EF RID: 1775 RVA: 0x000183F5 File Offset: 0x000165F5
		public void OnSynchronizeComponentTo(VirtualPlayer peer, PeerComponent component)
		{
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x000183F7 File Offset: 0x000165F7
		public void OnAddComponent(PeerComponent component)
		{
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x000183F9 File Offset: 0x000165F9
		public void OnRemoveComponent(PeerComponent component)
		{
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x000183FB File Offset: 0x000165FB
		public bool IsNetworkActive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x000183FE File Offset: 0x000165FE
		public bool IsConnectionActive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x00018401 File Offset: 0x00016601
		public bool IsServerPeer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x00018404 File Offset: 0x00016604
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x00018407 File Offset: 0x00016607
		public bool IsSynchronized
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x060006F7 RID: 1783 RVA: 0x00018409 File Offset: 0x00016609
		private DummyCommunicator(int index, string name)
		{
			this.VirtualPlayer = new VirtualPlayer(index, name, PlayerId.Empty, this);
		}

		// Token: 0x060006F8 RID: 1784 RVA: 0x00018424 File Offset: 0x00016624
		public static DummyCommunicator CreateAsServer(int index, string name)
		{
			return new DummyCommunicator(index, name);
		}

		// Token: 0x060006F9 RID: 1785 RVA: 0x0001842D File Offset: 0x0001662D
		public static DummyCommunicator CreateAsClient(string name, int index)
		{
			return new DummyCommunicator(index, name);
		}
	}
}
