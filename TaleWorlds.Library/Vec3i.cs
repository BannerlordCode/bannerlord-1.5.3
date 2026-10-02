using System;

namespace TaleWorlds.Library
{
	// Token: 0x020000A3 RID: 163
	[Serializable]
	public struct Vec3i
	{
		// Token: 0x06000625 RID: 1573 RVA: 0x00015855 File Offset: 0x00013A55
		public Vec3i(int x = 0, int y = 0, int z = 0)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x0001586C File Offset: 0x00013A6C
		public static bool operator ==(Vec3i v1, Vec3i v2)
		{
			return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x0001589A File Offset: 0x00013A9A
		public static bool operator !=(Vec3i v1, Vec3i v2)
		{
			return v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x000158CB File Offset: 0x00013ACB
		public Vec3 ToVec3()
		{
			return new Vec3((float)this.X, (float)this.Y, (float)this.Z, -1f);
		}

		// Token: 0x170000AE RID: 174
		public int this[int index]
		{
			get
			{
				if (index == 0)
				{
					return this.X;
				}
				if (index != 1)
				{
					return this.Z;
				}
				return this.Y;
			}
			set
			{
				if (index == 0)
				{
					this.X = value;
					return;
				}
				if (index == 1)
				{
					this.Y = value;
					return;
				}
				this.Z = value;
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00015929 File Offset: 0x00013B29
		public static Vec3i operator *(Vec3i v, int mult)
		{
			return new Vec3i(v.X * mult, v.Y * mult, v.Z * mult);
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00015948 File Offset: 0x00013B48
		public static Vec3i operator +(Vec3i v1, Vec3i v2)
		{
			return new Vec3i(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x00015976 File Offset: 0x00013B76
		public static Vec3i operator -(Vec3i v1, Vec3i v2)
		{
			return new Vec3i(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x000159A4 File Offset: 0x00013BA4
		public override bool Equals(object obj)
		{
			return obj != null && !(base.GetType() != obj.GetType()) && (((Vec3i)obj).X == this.X && ((Vec3i)obj).Y == this.Y) && ((Vec3i)obj).Z == this.Z;
		}

		// Token: 0x0600062F RID: 1583 RVA: 0x00015A0E File Offset: 0x00013C0E
		public override int GetHashCode()
		{
			return (((this.X * 397) ^ this.Y) * 397) ^ this.Z;
		}

		// Token: 0x06000630 RID: 1584 RVA: 0x00015A30 File Offset: 0x00013C30
		public override string ToString()
		{
			return string.Format("{0}: {1}, {2}: {3}, {4}: {5}", new object[] { "X", this.X, "Y", this.Y, "Z", this.Z });
		}

		// Token: 0x040001D3 RID: 467
		public int X;

		// Token: 0x040001D4 RID: 468
		public int Y;

		// Token: 0x040001D5 RID: 469
		public int Z;

		// Token: 0x040001D6 RID: 470
		public static readonly Vec3i Zero = new Vec3i(0, 0, 0);
	}
}
