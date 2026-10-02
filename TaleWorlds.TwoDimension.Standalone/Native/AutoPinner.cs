using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native
{
	// Token: 0x02000015 RID: 21
	internal class AutoPinner : IDisposable
	{
		// Token: 0x060000FD RID: 253 RVA: 0x00006883 File Offset: 0x00004A83
		public AutoPinner(object obj)
		{
			if (obj != null)
			{
				this._pinnedObject = GCHandle.Alloc(obj, GCHandleType.Pinned);
			}
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000689B File Offset: 0x00004A9B
		public static implicit operator IntPtr(AutoPinner autoPinner)
		{
			if (autoPinner._pinnedObject.IsAllocated)
			{
				return autoPinner._pinnedObject.AddrOfPinnedObject();
			}
			return IntPtr.Zero;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000068BB File Offset: 0x00004ABB
		public void Dispose()
		{
			if (this._pinnedObject.IsAllocated)
			{
				this._pinnedObject.Free();
			}
		}

		// Token: 0x04000070 RID: 112
		private GCHandle _pinnedObject;
	}
}
