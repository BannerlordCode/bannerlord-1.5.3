using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000085 RID: 133
	public struct Ray
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00011E5D File Offset: 0x0001005D
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x00011E65 File Offset: 0x00010065
		public Vec3 Origin
		{
			get
			{
				return this._origin;
			}
			private set
			{
				this._origin = value;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00011E6E File Offset: 0x0001006E
		// (set) Token: 0x060004DA RID: 1242 RVA: 0x00011E76 File Offset: 0x00010076
		public Vec3 Direction
		{
			get
			{
				return this._direction;
			}
			private set
			{
				this._direction = value;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060004DB RID: 1243 RVA: 0x00011E7F File Offset: 0x0001007F
		// (set) Token: 0x060004DC RID: 1244 RVA: 0x00011E87 File Offset: 0x00010087
		public float MaxDistance
		{
			get
			{
				return this._maxDistance;
			}
			private set
			{
				this._maxDistance = value;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00011E90 File Offset: 0x00010090
		public Vec3 EndPoint
		{
			get
			{
				return this.Origin + this.Direction * this.MaxDistance;
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x00011EAE File Offset: 0x000100AE
		public Ray(Vec3 origin, Vec3 direction, float maxDistance = 3.4028235E+38f)
		{
			this = default(Ray);
			this.Reset(origin, direction, maxDistance);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00011EC0 File Offset: 0x000100C0
		public Ray(Vec3 origin, Vec3 direction, bool useDirectionLenForMaxDistance)
		{
			this._origin = origin;
			this._direction = direction;
			float num = this._direction.Normalize();
			if (useDirectionLenForMaxDistance)
			{
				this._maxDistance = num;
				return;
			}
			this._maxDistance = float.MaxValue;
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00011EFD File Offset: 0x000100FD
		public void Reset(Vec3 origin, Vec3 direction, float maxDistance = 3.4028235E+38f)
		{
			this._origin = origin;
			this._direction = direction;
			this._maxDistance = maxDistance;
			this._direction.Normalize();
		}

		// Token: 0x04000177 RID: 375
		private Vec3 _origin;

		// Token: 0x04000178 RID: 376
		private Vec3 _direction;

		// Token: 0x04000179 RID: 377
		private float _maxDistance;
	}
}
