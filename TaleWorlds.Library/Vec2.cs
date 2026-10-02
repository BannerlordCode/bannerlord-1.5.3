using System;
using System.Numerics;

namespace TaleWorlds.Library
{
	// Token: 0x020000A0 RID: 160
	[Serializable]
	public struct Vec2
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00013BB2 File Offset: 0x00011DB2
		public float X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x00013BBA File Offset: 0x00011DBA
		public float Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00013BC2 File Offset: 0x00011DC2
		public Vec2(float a, float b)
		{
			this.x = a;
			this.y = b;
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00013BD2 File Offset: 0x00011DD2
		public Vec2(Vec2 v)
		{
			this.x = v.x;
			this.y = v.y;
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00013BEC File Offset: 0x00011DEC
		public Vec2(Vector2 v)
		{
			this.x = v.X;
			this.y = v.Y;
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00013C06 File Offset: 0x00011E06
		public Vec3 ToVec3(float z = 0f)
		{
			return new Vec3(this.x, this.y, z, -1f);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00013C1F File Offset: 0x00011E1F
		public static explicit operator Vector2(Vec2 vec2)
		{
			return new Vector2(vec2.x, vec2.y);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00013C32 File Offset: 0x00011E32
		public static implicit operator Vec2(Vector2 vec2)
		{
			return new Vec2(vec2.X, vec2.Y);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00013C48 File Offset: 0x00011E48
		public float Normalize()
		{
			float length = this.Length;
			if (length > 1E-05f)
			{
				this.x /= length;
				this.y /= length;
			}
			else
			{
				this.x = 0f;
				this.y = 1f;
			}
			return length;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00013C9C File Offset: 0x00011E9C
		public Vec2 Normalized()
		{
			Vec2 vec = this;
			vec.Normalize();
			return vec;
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00013CBC File Offset: 0x00011EBC
		public void ClampMagnitude(float min, float max)
		{
			float num = this.Normalize();
			this *= MathF.Clamp(num, min, max);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00013CEC File Offset: 0x00011EEC
		public static WindingOrder GetWindingOrder(Vec2 first, Vec2 second, Vec2 third)
		{
			Vec2 vec = second - first;
			float num = Vec2.CCW(third - second, vec);
			if (num > 0f)
			{
				return WindingOrder.Ccw;
			}
			if (num < 0f)
			{
				return WindingOrder.Cw;
			}
			return WindingOrder.None;
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00013D24 File Offset: 0x00011F24
		public static float CCW(Vec2 va, Vec2 vb)
		{
			return va.x * vb.y - va.y * vb.x;
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060005AE RID: 1454 RVA: 0x00013D41 File Offset: 0x00011F41
		public float Length
		{
			get
			{
				return MathF.Sqrt(this.x * this.x + this.y * this.y);
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x00013D63 File Offset: 0x00011F63
		public float LengthSquared
		{
			get
			{
				return this.x * this.x + this.y * this.y;
			}
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00013D80 File Offset: 0x00011F80
		public override bool Equals(object obj)
		{
			return obj != null && !(base.GetType() != obj.GetType()) && ((Vec2)obj).x == this.x && ((Vec2)obj).y == this.y;
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00013DD7 File Offset: 0x00011FD7
		public override int GetHashCode()
		{
			return (int)(1001f * this.x + 10039f * this.y);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00013DF3 File Offset: 0x00011FF3
		public static bool operator ==(Vec2 v1, Vec2 v2)
		{
			return v1.x == v2.x && v1.y == v2.y;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00013E13 File Offset: 0x00012013
		public static bool operator !=(Vec2 v1, Vec2 v2)
		{
			return v1.x != v2.x || v1.y != v2.y;
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00013E36 File Offset: 0x00012036
		public static Vec2 operator -(Vec2 v)
		{
			return new Vec2(-v.x, -v.y);
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00013E4B File Offset: 0x0001204B
		public static Vec2 operator +(Vec2 v1, Vec2 v2)
		{
			return new Vec2(v1.x + v2.x, v1.y + v2.y);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00013E6C File Offset: 0x0001206C
		public static Vec2 operator -(Vec2 v1, Vec2 v2)
		{
			return new Vec2(v1.x - v2.x, v1.y - v2.y);
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00013E8D File Offset: 0x0001208D
		public static Vec2 operator *(Vec2 v, float f)
		{
			return new Vec2(v.x * f, v.y * f);
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00013EA4 File Offset: 0x000120A4
		public static Vec2 operator *(float f, Vec2 v)
		{
			return new Vec2(v.x * f, v.y * f);
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00013EBB File Offset: 0x000120BB
		public static Vec2 operator /(float f, Vec2 v)
		{
			return new Vec2(f / v.x, f / v.y);
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00013ED2 File Offset: 0x000120D2
		public static Vec2 operator /(Vec2 v, float f)
		{
			return new Vec2(v.x / f, v.y / f);
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00013EEC File Offset: 0x000120EC
		public bool IsUnit()
		{
			float lengthSquared = this.LengthSquared;
			return lengthSquared > 0.98010004f && lengthSquared < 1.0201f;
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00013F14 File Offset: 0x00012114
		public bool IsNonZero()
		{
			float num = 1E-05f;
			return this.x > num || this.x < -num || this.y > num || this.y < -num;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00013F4F File Offset: 0x0001214F
		public bool NearlyEquals(Vec2 v, float epsilon = 1E-05f)
		{
			return MathF.Abs(this.x - v.x) < epsilon && MathF.Abs(this.y - v.y) < epsilon;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x00013F80 File Offset: 0x00012180
		public void RotateCCW(float angleInRadians)
		{
			float num;
			float num2;
			MathF.SinCos(angleInRadians, out num, out num2);
			float num3 = this.x * num2 - this.y * num;
			this.y = this.y * num2 + this.x * num;
			this.x = num3;
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x00013FC7 File Offset: 0x000121C7
		public float DotProduct(Vec2 v)
		{
			return v.x * this.x + v.y * this.y;
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x00013FE4 File Offset: 0x000121E4
		public static float DotProduct(Vec2 va, Vec2 vb)
		{
			return va.x * vb.x + va.y * vb.y;
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00014001 File Offset: 0x00012201
		public static Vec2 ElementWiseProduct(Vec2 va, Vec2 vb)
		{
			return new Vec2(va.x * vb.x, va.y * vb.y);
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x00014022 File Offset: 0x00012222
		public float RotationInRadians
		{
			get
			{
				return MathF.Atan2(-this.x, this.y);
			}
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00014036 File Offset: 0x00012236
		public static Vec2 FromRotation(float rotation)
		{
			return new Vec2(-MathF.Sin(rotation), MathF.Cos(rotation));
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0001404A File Offset: 0x0001224A
		public Vec2 TransformToLocalUnitF(Vec2 a)
		{
			return new Vec2(this.y * a.x - this.x * a.y, this.x * a.x + this.y * a.y);
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x00014087 File Offset: 0x00012287
		public Vec2 TransformToParentUnitF(Vec2 a)
		{
			return new Vec2(this.y * a.x + this.x * a.y, -this.x * a.x + this.y * a.y);
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x000140C5 File Offset: 0x000122C5
		public Vec2 TransformToLocalUnitFLeftHanded(Vec2 a)
		{
			return new Vec2(-this.y * a.x + this.x * a.y, this.x * a.x + this.y * a.y);
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00014103 File Offset: 0x00012303
		public Vec2 TransformToParentUnitFLeftHanded(Vec2 a)
		{
			return new Vec2(-this.y * a.x + this.x * a.y, this.x * a.x + this.y * a.y);
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00014141 File Offset: 0x00012341
		public Vec2 RightVec()
		{
			return new Vec2(this.y, -this.x);
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00014155 File Offset: 0x00012355
		public Vec2 LeftVec()
		{
			return new Vec2(-this.y, this.x);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00014169 File Offset: 0x00012369
		public static Vec2 Max(Vec2 v1, Vec2 v2)
		{
			return new Vec2(MathF.Max(v1.x, v2.x), MathF.Max(v1.y, v2.y));
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00014192 File Offset: 0x00012392
		public static Vec2 Max(Vec2 v1, float f)
		{
			return new Vec2(MathF.Max(v1.x, f), MathF.Max(v1.y, f));
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x000141B1 File Offset: 0x000123B1
		public static Vec2 Min(Vec2 v1, Vec2 v2)
		{
			return new Vec2(MathF.Min(v1.x, v2.x), MathF.Min(v1.y, v2.y));
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000141DA File Offset: 0x000123DA
		public static Vec2 Min(Vec2 v1, float f)
		{
			return new Vec2(MathF.Min(v1.x, f), MathF.Min(v1.y, f));
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x000141F9 File Offset: 0x000123F9
		public override string ToString()
		{
			return string.Concat(new object[] { "(Vec2) X: ", this.x, " Y: ", this.y });
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00014232 File Offset: 0x00012432
		public float DistanceSquared(Vec2 v)
		{
			return (v.x - this.x) * (v.x - this.x) + (v.y - this.y) * (v.y - this.y);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0001426B File Offset: 0x0001246B
		public float Distance(Vec2 v)
		{
			return MathF.Sqrt((v.x - this.x) * (v.x - this.x) + (v.y - this.y) * (v.y - this.y));
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x000142AC File Offset: 0x000124AC
		public static float DistanceToLine(Vec2 line1, Vec2 line2, Vec2 point)
		{
			float num = line2.x - line1.x;
			float num2 = line2.y - line1.y;
			return MathF.Abs(num * (line1.y - point.y) - (line1.x - point.x) * num2) / MathF.Sqrt(num * num + num2 * num2);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00014306 File Offset: 0x00012506
		public static float DistanceToLineSegmentSquared(Vec2 line1, Vec2 line2, Vec2 point)
		{
			return point.DistanceSquared(MBMath.GetClosestPointOnLineSegmentToPoint(in line1, in line2, in point));
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x0001431A File Offset: 0x0001251A
		public float DistanceToLineSegment(Vec2 v, Vec2 w, out Vec2 closestPointOnLineSegment)
		{
			return MathF.Sqrt(this.DistanceSquaredToLineSegment(v, w, out closestPointOnLineSegment));
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0001432C File Offset: 0x0001252C
		public float DistanceSquaredToLineSegment(Vec2 v, Vec2 w, out Vec2 closestPointOnLineSegment)
		{
			Vec2 vec = this;
			float num = v.DistanceSquared(w);
			if (num == 0f)
			{
				closestPointOnLineSegment = v;
			}
			else
			{
				float num2 = Vec2.DotProduct(vec - v, w - v) / num;
				if (num2 < 0f)
				{
					closestPointOnLineSegment = v;
				}
				else if (num2 > 1f)
				{
					closestPointOnLineSegment = w;
				}
				else
				{
					Vec2 vec2 = v + (w - v) * num2;
					closestPointOnLineSegment = vec2;
				}
			}
			return vec.DistanceSquared(closestPointOnLineSegment);
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x000143BA File Offset: 0x000125BA
		public static Vec2 Abs(Vec2 vec)
		{
			return new Vec2(MathF.Abs(vec.x), MathF.Abs(vec.y));
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x000143D7 File Offset: 0x000125D7
		public static Vec2 Lerp(Vec2 v1, Vec2 v2, float alpha)
		{
			return v1 * (1f - alpha) + v2 * alpha;
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x000143F4 File Offset: 0x000125F4
		public static Vec2 Slerp(Vec2 start, Vec2 end, float percent)
		{
			float num = Vec2.DotProduct(start, end);
			num = MBMath.ClampFloat(num, -1f, 1f);
			float num2 = MathF.Acos(num) * percent;
			Vec2 vec = end - start * num;
			vec.Normalize();
			return start * MathF.Cos(num2) + vec * MathF.Sin(num2);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00014458 File Offset: 0x00012658
		public float AngleBetween(Vec2 vector2)
		{
			float num = this.x * vector2.y - vector2.x * this.y;
			float num2 = this.x * vector2.x + this.y * vector2.y;
			return MathF.Atan2(num, num2);
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x000144A2 File Offset: 0x000126A2
		public bool IsValid
		{
			get
			{
				return !float.IsNaN(this.x) && !float.IsNaN(this.y) && !float.IsInfinity(this.x) && !float.IsInfinity(this.y);
			}
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x000144DB File Offset: 0x000126DB
		public static float Determinant(in Vec2 vec1, in Vec2 vec2)
		{
			return vec1.x * vec2.y - vec1.y * vec2.x;
		}

		// Token: 0x040001BC RID: 444
		public float x;

		// Token: 0x040001BD RID: 445
		public float y;

		// Token: 0x040001BE RID: 446
		public static readonly Vec2 Side = new Vec2(1f, 0f);

		// Token: 0x040001BF RID: 447
		public static readonly Vec2 Forward = new Vec2(0f, 1f);

		// Token: 0x040001C0 RID: 448
		public static readonly Vec2 One = new Vec2(1f, 1f);

		// Token: 0x040001C1 RID: 449
		public static readonly Vec2 Zero = new Vec2(0f, 0f);

		// Token: 0x040001C2 RID: 450
		public static readonly Vec2 Invalid = new Vec2(float.NaN, float.NaN);
	}
}
