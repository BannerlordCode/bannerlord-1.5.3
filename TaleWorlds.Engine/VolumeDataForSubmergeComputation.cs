using System;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200004E RID: 78
	[EngineStruct("rglWater_renderer::Volume_data_for_submerge_computation", false, null)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct VolumeDataForSubmergeComputation
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x00005D51 File Offset: 0x00003F51
		public float Height
		{
			get
			{
				return this.LocalScale[(int)this.DynamicUpAxis];
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x00005D64 File Offset: 0x00003F64
		public float Width
		{
			get
			{
				return this.LocalScale[(int)((this.DynamicUpAxis + 1) % (FloaterVolumeDynamicUpAxis)3)];
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x00005D7B File Offset: 0x00003F7B
		public float Depth
		{
			get
			{
				return this.LocalScale[(int)((this.DynamicUpAxis + 2) % (FloaterVolumeDynamicUpAxis)3)];
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x00005D92 File Offset: 0x00003F92
		public Vec3 Up
		{
			get
			{
				return this.LocalFrame.rotation[(int)this.DynamicUpAxis];
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x00005DAA File Offset: 0x00003FAA
		public Vec3 Side
		{
			get
			{
				return this.LocalFrame.rotation[(int)((this.DynamicUpAxis + 1) % (FloaterVolumeDynamicUpAxis)3)];
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060007FE RID: 2046 RVA: 0x00005DC6 File Offset: 0x00003FC6
		public Vec3 Forward
		{
			get
			{
				return this.LocalFrame.rotation[(int)((this.DynamicUpAxis + 2) % (FloaterVolumeDynamicUpAxis)3)];
			}
		}

		// Token: 0x040000AD RID: 173
		public Vec3 DynamicLocalBottomPos;

		// Token: 0x040000AE RID: 174
		public MatrixFrame LocalFrame;

		// Token: 0x040000AF RID: 175
		public Vec3 LocalScale;

		// Token: 0x040000B0 RID: 176
		public FloaterVolumeDynamicUpAxis DynamicUpAxis;

		// Token: 0x040000B1 RID: 177
		public Vec3 OutGlobalWaterSurfaceNormal;

		// Token: 0x040000B2 RID: 178
		public float InOutWaterHeightWrtVolume;
	}
}
