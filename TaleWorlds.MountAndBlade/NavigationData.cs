using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E8 RID: 744
	[EngineStruct("Navigation_data", false, null)]
	[Serializable]
	public struct NavigationData
	{
		// Token: 0x06002B47 RID: 11079 RVA: 0x000A7084 File Offset: 0x000A5284
		public NavigationData(Vec3 startPoint, Vec3 endPoint, float agentRadius)
		{
			this.Points = new Vec2[1024];
			this.StartPoint = startPoint;
			this.EndPoint = endPoint;
			this.Points[0] = startPoint.AsVec2;
			this.Points[1] = endPoint.AsVec2;
			this.PointSize = 2;
			this.AgentRadius = agentRadius;
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x000A70E4 File Offset: 0x000A52E4
		[Conditional("DEBUG")]
		public void TickDebug()
		{
			for (int i = 0; i < this.PointSize - 1; i++)
			{
			}
		}

		// Token: 0x04001071 RID: 4209
		private const int MaxPathSize = 1024;

		// Token: 0x04001072 RID: 4210
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
		public Vec2[] Points;

		// Token: 0x04001073 RID: 4211
		public Vec3 StartPoint;

		// Token: 0x04001074 RID: 4212
		public Vec3 EndPoint;

		// Token: 0x04001075 RID: 4213
		public readonly int PointSize;

		// Token: 0x04001076 RID: 4214
		public readonly float AgentRadius;
	}
}
