using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000084 RID: 132
	[Serializable]
	public struct Quaternion
	{
		// Token: 0x060004AF RID: 1199 RVA: 0x00010A2D File Offset: 0x0000EC2D
		public Quaternion(float x, float y, float z, float w)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
			this.W = w;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00010A4C File Offset: 0x0000EC4C
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00010A5E File Offset: 0x0000EC5E
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00010A74 File Offset: 0x0000EC74
		public static bool operator ==(Quaternion a, Quaternion b)
		{
			return a == b || (a != null && b != null && (a.X == b.X && a.Y == b.Y && a.Z == b.Z) && a.W == b.W);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00010ADD File Offset: 0x0000ECDD
		public static bool operator !=(Quaternion a, Quaternion b)
		{
			return !(a == b);
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00010AE9 File Offset: 0x0000ECE9
		public static Quaternion operator +(Quaternion a, Quaternion b)
		{
			return new Quaternion(a.X + b.X, a.Y + b.Y, a.Z + b.Z, a.W + b.W);
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00010B24 File Offset: 0x0000ED24
		public static Quaternion operator -(Quaternion a, Quaternion b)
		{
			return new Quaternion(a.X - b.X, a.Y - b.Y, a.Z - b.Z, a.W - b.W);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00010B5F File Offset: 0x0000ED5F
		public static Quaternion operator *(Quaternion a, float b)
		{
			return new Quaternion(a.X * b, a.Y * b, a.Z * b, a.W * b);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00010B86 File Offset: 0x0000ED86
		public static Quaternion operator *(float s, Quaternion v)
		{
			return v * s;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00010B90 File Offset: 0x0000ED90
		public static Quaternion operator *(Quaternion a, Quaternion b)
		{
			float num = a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z;
			float num2 = a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y;
			float num3 = a.W * b.Y - a.X * b.Z + a.Y * b.W + a.Z * b.X;
			float num4 = a.W * b.Z + a.X * b.Y - a.Y * b.X + a.Z * b.W;
			return new Quaternion(num2, num3, num4, num);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00010C84 File Offset: 0x0000EE84
		public static Quaternion operator /(Quaternion v, float s)
		{
			return new Quaternion(v.X / s, v.Y / s, v.Z / s, v.W / s);
		}

		// Token: 0x1700007A RID: 122
		public float this[int i]
		{
			get
			{
				float num;
				switch (i)
				{
				case 0:
					num = this.W;
					break;
				case 1:
					num = this.X;
					break;
				case 2:
					num = this.Y;
					break;
				case 3:
					num = this.Z;
					break;
				default:
					throw new IndexOutOfRangeException("Quaternion out of bounds.");
				}
				return num;
			}
			set
			{
				switch (i)
				{
				case 0:
					this.W = value;
					return;
				case 1:
					this.X = value;
					return;
				case 2:
					this.Y = value;
					return;
				case 3:
					this.Z = value;
					return;
				default:
					throw new IndexOutOfRangeException("Quaternion out of bounds.");
				}
			}
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00010D58 File Offset: 0x0000EF58
		public float Normalize()
		{
			float num = MathF.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W);
			if (num <= 1E-07f)
			{
				this.X = 0f;
				this.Y = 0f;
				this.Z = 0f;
				this.W = 1f;
			}
			else
			{
				float num2 = 1f / num;
				this.X *= num2;
				this.Y *= num2;
				this.Z *= num2;
				this.W *= num2;
			}
			return num;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x00010E1C File Offset: 0x0000F01C
		public float SafeNormalize()
		{
			double num = Math.Sqrt((double)this.X * (double)this.X + (double)this.Y * (double)this.Y + (double)this.Z * (double)this.Z + (double)this.W * (double)this.W);
			if (num <= 1E-07)
			{
				this.X = 0f;
				this.Y = 0f;
				this.Z = 0f;
				this.W = 1f;
			}
			else
			{
				this.X = (float)((double)this.X / num);
				this.Y = (float)((double)this.Y / num);
				this.Z = (float)((double)this.Z / num);
				this.W = (float)((double)this.W / num);
			}
			return (float)num;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x00010EEC File Offset: 0x0000F0EC
		public float NormalizeWeighted()
		{
			float num = this.X * this.X + this.Y * this.Y + this.Z * this.Z;
			if (num <= 1E-09f)
			{
				this.X = 1f;
				this.Y = 0f;
				this.Z = 0f;
				this.W = 0f;
			}
			else
			{
				this.W = MathF.Sqrt(1f - num);
			}
			return num;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00010F6C File Offset: 0x0000F16C
		public void SetToRotationX(float angle)
		{
			float num;
			float num2;
			MathF.SinCos(angle * 0.5f, out num, out num2);
			this.X = num;
			this.Y = 0f;
			this.Z = 0f;
			this.W = num2;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00010FB0 File Offset: 0x0000F1B0
		public void SetToRotationY(float angle)
		{
			float num;
			float num2;
			MathF.SinCos(angle * 0.5f, out num, out num2);
			this.X = 0f;
			this.Y = num;
			this.Z = 0f;
			this.W = num2;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00010FF4 File Offset: 0x0000F1F4
		public void SetToRotationZ(float angle)
		{
			float num;
			float num2;
			MathF.SinCos(angle * 0.5f, out num, out num2);
			this.X = 0f;
			this.Y = 0f;
			this.Z = num;
			this.W = num2;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00011035 File Offset: 0x0000F235
		public void Flip()
		{
			this.X = -this.X;
			this.Y = -this.Y;
			this.Z = -this.Z;
			this.W = -this.W;
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x0001106B File Offset: 0x0000F26B
		public bool IsIdentity
		{
			get
			{
				return this.X == 0f && this.Y == 0f && this.Z == 0f && this.W == 1f;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x000110A4 File Offset: 0x0000F2A4
		public bool IsUnit
		{
			get
			{
				return MBMath.ApproximatelyEquals(this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W, 1f, 0.2f);
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x000110F7 File Offset: 0x0000F2F7
		public static Quaternion Identity
		{
			get
			{
				return new Quaternion(0f, 0f, 0f, 1f);
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00011114 File Offset: 0x0000F314
		public Quaternion TransformToParent(Quaternion q)
		{
			return new Quaternion
			{
				X = this.Y * q.Z - this.Z * q.Y + this.W * q.X + this.X * q.W,
				Y = this.Z * q.X - this.X * q.Z + this.W * q.Y + this.Y * q.W,
				Z = this.X * q.Y - this.Y * q.X + this.W * q.Z + this.Z * q.W,
				W = this.W * q.W - (this.X * q.X + this.Y * q.Y + this.Z * q.Z)
			};
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00011224 File Offset: 0x0000F424
		public Quaternion TransformToLocal(Quaternion q)
		{
			return new Quaternion
			{
				X = this.Z * q.Y - this.Y * q.Z + this.W * q.X - this.X * q.W,
				Y = this.X * q.Z - this.Z * q.X + this.W * q.Y - this.Y * q.W,
				Z = this.Y * q.X - this.X * q.Y + this.W * q.Z - this.Z * q.W,
				W = this.W * q.W + (this.X * q.X + this.Y * q.Y + this.Z * q.Z)
			};
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00011334 File Offset: 0x0000F534
		public Quaternion TransformToLocalWithoutNormalize(Quaternion q)
		{
			return new Quaternion
			{
				X = this.Z * q.Y - this.Y * q.Z + this.W * q.X - this.X * q.W,
				Y = this.X * q.Z - this.Z * q.X + this.W * q.Y - this.Y * q.W,
				Z = this.Y * q.X - this.X * q.Y + this.W * q.Z - this.Z * q.W,
				W = this.W * q.W + (this.X * q.X + this.Y * q.Y + this.Z * q.Z)
			};
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00011444 File Offset: 0x0000F644
		public static Quaternion Slerp(Quaternion from, Quaternion to, float t)
		{
			float num = from.Dotp4(to);
			float num2;
			if (num < 0f)
			{
				num = -num;
				num2 = -1f;
			}
			else
			{
				num2 = 1f;
			}
			float num6;
			float num7;
			if (0.9995f >= num)
			{
				float num3 = MathF.Acos(num);
				float num4 = 1f / MathF.Sin(num3);
				float num5 = t * num3;
				num6 = MathF.Sin(num3 - num5) * num4;
				num7 = MathF.Sin(num5) * num4;
			}
			else
			{
				num6 = 1f - t;
				num7 = t;
			}
			num7 *= num2;
			Quaternion quaternion = default(Quaternion);
			quaternion.X = num6 * from.X + num7 * to.X;
			quaternion.Y = num6 * from.Y + num7 * to.Y;
			quaternion.Z = num6 * from.Z + num7 * to.Z;
			quaternion.W = num6 * from.W + num7 * to.W;
			quaternion.Normalize();
			return quaternion;
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x0001153C File Offset: 0x0000F73C
		public static Quaternion Lerp(Quaternion from, Quaternion to, float t)
		{
			float num = from.Dotp4(to);
			float num2 = 1f - t;
			float num3;
			if (num < 0f)
			{
				num = -num;
				num3 = -t;
			}
			else
			{
				num3 = t;
			}
			return new Quaternion
			{
				X = num2 * from.X + num3 * to.X,
				Y = num2 * from.Y + num3 * to.Y,
				Z = num2 * from.Z + num3 * to.Z,
				W = num2 * from.W + num3 * to.W
			};
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000115D8 File Offset: 0x0000F7D8
		public static Mat3 Mat3FromQuaternion(Quaternion quat)
		{
			Mat3 mat = default(Mat3);
			float num = quat.X + quat.X;
			float num2 = quat.Y + quat.Y;
			float num3 = quat.Z + quat.Z;
			float num4 = quat.X * num;
			float num5 = quat.X * num2;
			float num6 = quat.X * num3;
			float num7 = quat.Y * num2;
			float num8 = quat.Y * num3;
			float num9 = quat.Z * num3;
			float num10 = quat.W * num;
			float num11 = quat.W * num2;
			float num12 = quat.W * num3;
			mat.s.x = 1f - (num7 + num9);
			mat.s.y = num5 + num12;
			mat.s.z = num6 - num11;
			mat.f.x = num5 - num12;
			mat.f.y = 1f - (num4 + num9);
			mat.f.z = num8 + num10;
			mat.u.x = num6 + num11;
			mat.u.y = num8 - num10;
			mat.u.z = 1f - (num4 + num7);
			return mat;
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x00011720 File Offset: 0x0000F920
		public static Quaternion QuaternionFromEulerAngles(float yaw, float pitch, float roll)
		{
			float num = yaw * 0.017453292f;
			float num2 = pitch * 0.017453292f;
			float num3 = roll * 0.017453292f;
			float num4 = MathF.Cos(num * 0.5f);
			float num5 = MathF.Sin(num * 0.5f);
			float num6 = MathF.Cos(num2 * 0.5f);
			float num7 = MathF.Sin(num2 * 0.5f);
			float num8 = MathF.Cos(num3 * 0.5f);
			float num9 = MathF.Sin(num3 * 0.5f);
			float num10 = num8 * num6 * num4 + num9 * num7 * num5;
			float num11 = num9 * num6 * num4 - num8 * num7 * num5;
			float num12 = num8 * num7 * num4 + num9 * num6 * num5;
			float num13 = num8 * num6 * num5 - num9 * num7 * num4;
			return new Quaternion(num11, num12, num13, num10);
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x000117EC File Offset: 0x0000F9EC
		public static Quaternion QuaternionFromMat3(Mat3 m)
		{
			Quaternion quaternion = default(Quaternion);
			float num;
			if (m.u.z < 0f)
			{
				if (m.s.x > m.f.y)
				{
					num = 1f + m.s.x - m.f.y - m.u.z;
					quaternion.W = m.f.z - m.u.y;
					quaternion.X = num;
					quaternion.Y = m.s.y + m.f.x;
					quaternion.Z = m.u.x + m.s.z;
				}
				else
				{
					num = 1f - m.s.x + m.f.y - m.u.z;
					quaternion.W = m.u.x - m.s.z;
					quaternion.X = m.s.y + m.f.x;
					quaternion.Y = num;
					quaternion.Z = m.f.z + m.u.y;
				}
			}
			else if (m.s.x < -m.f.y)
			{
				num = 1f - m.s.x - m.f.y + m.u.z;
				quaternion.W = m.s.y - m.f.x;
				quaternion.X = m.u.x + m.s.z;
				quaternion.Y = m.f.z + m.u.y;
				quaternion.Z = num;
			}
			else
			{
				num = 1f + m.s.x + m.f.y + m.u.z;
				quaternion.W = num;
				quaternion.X = m.f.z - m.u.y;
				quaternion.Y = m.u.x - m.s.z;
				quaternion.Z = m.s.y - m.f.x;
			}
			float num2 = 0.5f / MathF.Sqrt(num);
			quaternion.W *= num2;
			quaternion.X *= num2;
			quaternion.Y *= num2;
			quaternion.Z *= num2;
			return quaternion;
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00011ACC File Offset: 0x0000FCCC
		public static void AxisAngleFromQuaternion(out Vec3 axis, out float angle, Quaternion quat)
		{
			axis = default(Vec3);
			float w = quat.W;
			if (w > 0.9999999f)
			{
				axis.x = 1f;
				axis.y = 0f;
				axis.z = 0f;
				angle = 0f;
				return;
			}
			float num = MathF.Sqrt(1f - w * w);
			if (num < 0.0001f)
			{
				num = 1f;
			}
			axis.x = quat.X / num;
			axis.y = quat.Y / num;
			axis.z = quat.Z / num;
			angle = MathF.Acos(w) * 2f;
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00011B70 File Offset: 0x0000FD70
		public static Quaternion QuaternionFromAxisAngle(Vec3 axis, float angle)
		{
			Quaternion quaternion = default(Quaternion);
			float num;
			float num2;
			MathF.SinCos(angle * 0.5f, out num, out num2);
			quaternion.X = axis.x * num;
			quaternion.Y = axis.y * num;
			quaternion.Z = axis.z * num;
			quaternion.W = num2;
			return quaternion;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x00011BCC File Offset: 0x0000FDCC
		public static Vec3 EulerAngleFromQuaternion(Quaternion quat)
		{
			float w = quat.W;
			float x = quat.X;
			float y = quat.Y;
			float z = quat.Z;
			float num = w * w;
			float num2 = x * x;
			float num3 = y * y;
			float num4 = z * z;
			return new Vec3
			{
				z = MathF.Atan2(2f * (x * y + z * w), num2 - num3 - num4 + num),
				x = MathF.Atan2(2f * (y * z + x * w), -num2 - num3 + num4 + num),
				y = MathF.Asin(-2f * (x * z - y * w))
			};
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00011C78 File Offset: 0x0000FE78
		public static Quaternion FindShortestArcAsQuaternion(Vec3 v0, Vec3 v1)
		{
			Vec3 vec = Vec3.CrossProduct(v0, v1);
			float num = Vec3.DotProduct(v0, v1);
			if ((double)num < -0.9999900000002526)
			{
				Vec3 vec2 = default(Vec3);
				if (MathF.Abs(v0.z) < 0.8f)
				{
					vec2 = Vec3.CrossProduct(v0, new Vec3(0f, 0f, 1f, -1f));
				}
				else
				{
					vec2 = Vec3.CrossProduct(v0, new Vec3(1f, 0f, 0f, -1f));
				}
				vec2.Normalize();
				return new Quaternion(vec2.x, vec2.y, vec2.z, 0f);
			}
			float num2 = MathF.Sqrt((1f + num) * 2f);
			float num3 = 1f / num2;
			return new Quaternion(vec.x * num3, vec.y * num3, vec.z * num3, num2 * 0.5f);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00011D6A File Offset: 0x0000FF6A
		public float Dotp4(Quaternion q2)
		{
			return this.X * q2.X + this.Y * q2.Y + this.Z * q2.Z + this.W * q2.W;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00011DA3 File Offset: 0x0000FFA3
		public Mat3 ToMat3()
		{
			return Quaternion.Mat3FromQuaternion(this);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00011DB0 File Offset: 0x0000FFB0
		public bool InverseDirection(Quaternion q2)
		{
			return this.Dotp4(q2) < 0f;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00011DC0 File Offset: 0x0000FFC0
		public Quaternion Conjugate()
		{
			return new Quaternion(-this.X, -this.Y, -this.Z, this.W);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00011DE4 File Offset: 0x0000FFE4
		public Quaternion Inverse()
		{
			float num = this.X * this.X + this.Y * this.Y + this.Z * this.Z + this.W * this.W;
			if (num == 0f)
			{
				Debug.FailedAssert("Cannot invert a quaternion with zero norm.", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\Quaternion.cs", "Inverse", 608);
				return this;
			}
			return this.Conjugate() / num;
		}

		// Token: 0x04000173 RID: 371
		public float W;

		// Token: 0x04000174 RID: 372
		public float X;

		// Token: 0x04000175 RID: 373
		public float Y;

		// Token: 0x04000176 RID: 374
		public float Z;
	}
}
