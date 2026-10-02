using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace TaleWorlds.Library
{
	// Token: 0x02000073 RID: 115
	public class NavigationPath : ISerializable
	{
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x0000E94A File Offset: 0x0000CB4A
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x0000E952 File Offset: 0x0000CB52
		public Vec2[] PathPoints { get; private set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000420 RID: 1056 RVA: 0x0000E95B File Offset: 0x0000CB5B
		// (set) Token: 0x06000421 RID: 1057 RVA: 0x0000E963 File Offset: 0x0000CB63
		[CachedData]
		public int Size { get; set; }

		// Token: 0x06000422 RID: 1058 RVA: 0x0000E96C File Offset: 0x0000CB6C
		public NavigationPath()
		{
			this.PathPoints = new Vec2[128];
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0000E984 File Offset: 0x0000CB84
		protected NavigationPath(SerializationInfo info, StreamingContext context)
		{
			this.PathPoints = new Vec2[128];
			this.Size = info.GetInt32("s");
			for (int i = 0; i < this.Size; i++)
			{
				float single = info.GetSingle("x" + i);
				float single2 = info.GetSingle("y" + i);
				this.PathPoints[i] = new Vec2(single, single2);
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000EA0C File Offset: 0x0000CC0C
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("s", this.Size);
			for (int i = 0; i < this.Size; i++)
			{
				info.AddValue("x" + i, this.PathPoints[i].x);
				info.AddValue("y" + i, this.PathPoints[i].y);
			}
		}

		// Token: 0x17000066 RID: 102
		public Vec2 this[int i]
		{
			get
			{
				return this.PathPoints[i];
			}
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000EA97 File Offset: 0x0000CC97
		public void OverridePathPointAtIndex(int index, in Vec2 newValue)
		{
			this.PathPoints[index] = newValue;
		}

		// Token: 0x04000149 RID: 329
		private const int PathSize = 128;
	}
}
