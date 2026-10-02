using System;
using System.Runtime.InteropServices;

namespace TaleWorlds.TwoDimension.Standalone.Native.Windows
{
	// Token: 0x02000039 RID: 57
	public static class DXGI
	{
		// Token: 0x0600013D RID: 317
		[DllImport("dxgi.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern int CreateDXGIFactory(ref Guid riid, out IntPtr factory);

		// Token: 0x04000157 RID: 343
		public static Guid IID_IDXGIAdapter = new Guid("2411E7E1-12AC-4CCF-BD14-9798E8534DC0");

		// Token: 0x04000158 RID: 344
		public static Guid IID_IDXGIFactory = new Guid("7B7166EC-21C7-44AE-B21A-C9AE321AE369");

		// Token: 0x0200007E RID: 126
		[Guid("7B7166EC-21C7-44AE-B21A-C9AE321AE369")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IDXGIFactory
		{
			// Token: 0x0600023C RID: 572
			int SetPrivateData();

			// Token: 0x0600023D RID: 573
			int SetPrivateDataInterface();

			// Token: 0x0600023E RID: 574
			int GetPrivateData();

			// Token: 0x0600023F RID: 575
			int GetParent();

			// Token: 0x06000240 RID: 576
			[PreserveSig]
			int EnumAdapters(uint index, out DXGI.IDXGIAdapter adapter);
		}

		// Token: 0x0200007F RID: 127
		[Guid("2411E7E1-12AC-4CCF-BD14-9798E8534DC0")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IDXGIAdapter
		{
			// Token: 0x06000241 RID: 577
			[PreserveSig]
			int SetPrivateData();

			// Token: 0x06000242 RID: 578
			[PreserveSig]
			int SetPrivateDataInterface();

			// Token: 0x06000243 RID: 579
			[PreserveSig]
			int GetPrivateData();

			// Token: 0x06000244 RID: 580
			[PreserveSig]
			int GetParent();

			// Token: 0x06000245 RID: 581
			[PreserveSig]
			int EnumOutputs(uint Output, [MarshalAs(UnmanagedType.Interface)] out DXGI.IDXGIOutput ppOutput);

			// Token: 0x06000246 RID: 582
			[PreserveSig]
			int GetDesc(out DXGI.DXGI_ADAPTER_DESC desc);
		}

		// Token: 0x02000080 RID: 128
		[Guid("AE02EEDB-C735-4690-8D52-5A8DC20213AA")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IDXGIOutput
		{
			// Token: 0x06000247 RID: 583
			int SetPrivateData();

			// Token: 0x06000248 RID: 584
			int SetPrivateDataInterface();

			// Token: 0x06000249 RID: 585
			int GetPrivateData();

			// Token: 0x0600024A RID: 586
			int GetParent();

			// Token: 0x0600024B RID: 587
			int GetDesc(out DXGI.DXGI_OUTPUT_DESC desc);

			// Token: 0x0600024C RID: 588
			int GetDisplayModeList();

			// Token: 0x0600024D RID: 589
			int FindClosestMatchingMode();

			// Token: 0x0600024E RID: 590
			int WaitForVBlank();

			// Token: 0x0600024F RID: 591
			int TakeOwnership();

			// Token: 0x06000250 RID: 592
			int ReleaseOwnership();

			// Token: 0x06000251 RID: 593
			int GetGammaControlCapabilities();

			// Token: 0x06000252 RID: 594
			int SetGammaControl();

			// Token: 0x06000253 RID: 595
			int GetGammaControl();

			// Token: 0x06000254 RID: 596
			int SetDisplaySurface();

			// Token: 0x06000255 RID: 597
			int GetDisplaySurfaceData();

			// Token: 0x06000256 RID: 598
			int GetFrameStatistics();
		}

		// Token: 0x02000081 RID: 129
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct DXGI_ADAPTER_DESC
		{
			// Token: 0x04000202 RID: 514
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string Description;

			// Token: 0x04000203 RID: 515
			public uint VendorId;

			// Token: 0x04000204 RID: 516
			public uint DeviceId;

			// Token: 0x04000205 RID: 517
			public uint SubSysId;

			// Token: 0x04000206 RID: 518
			public uint Revision;

			// Token: 0x04000207 RID: 519
			public UIntPtr DedicatedVideoMemory;

			// Token: 0x04000208 RID: 520
			public UIntPtr DedicatedSystemMemory;

			// Token: 0x04000209 RID: 521
			public UIntPtr SharedSystemMemory;

			// Token: 0x0400020A RID: 522
			public UIntPtr AdapterLuid;
		}

		// Token: 0x02000082 RID: 130
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct DXGI_OUTPUT_DESC
		{
			// Token: 0x0400020B RID: 523
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			public string DeviceName;

			// Token: 0x0400020C RID: 524
			public DXGI.RECT DesktopCoordinates;

			// Token: 0x0400020D RID: 525
			public bool AttachedToDesktop;

			// Token: 0x0400020E RID: 526
			public uint Rotation;

			// Token: 0x0400020F RID: 527
			public IntPtr Monitor;
		}

		// Token: 0x02000083 RID: 131
		public struct RECT
		{
			// Token: 0x06000257 RID: 599 RVA: 0x00007191 File Offset: 0x00005391
			public override bool Equals(object o)
			{
				return o != null && o is DXGI.RECT && this == (DXGI.RECT)o;
			}

			// Token: 0x06000258 RID: 600 RVA: 0x000071B3 File Offset: 0x000053B3
			public override int GetHashCode()
			{
				return 0;
			}

			// Token: 0x06000259 RID: 601 RVA: 0x000071B6 File Offset: 0x000053B6
			public static bool operator ==(DXGI.RECT r1, DXGI.RECT r2)
			{
				return r1.bottom == r2.bottom && r1.right == r2.right && r1.top == r2.top && r1.left == r2.left;
			}

			// Token: 0x0600025A RID: 602 RVA: 0x000071F2 File Offset: 0x000053F2
			public static bool operator !=(DXGI.RECT r1, DXGI.RECT r2)
			{
				return r1.bottom != r2.bottom || r1.right != r2.right || r1.top != r2.top || r1.left != r2.left;
			}

			// Token: 0x04000210 RID: 528
			public int left;

			// Token: 0x04000211 RID: 529
			public int top;

			// Token: 0x04000212 RID: 530
			public int right;

			// Token: 0x04000213 RID: 531
			public int bottom;
		}
	}
}
