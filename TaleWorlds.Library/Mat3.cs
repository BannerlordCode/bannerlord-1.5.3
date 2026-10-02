using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000064 RID: 100
	[Serializable]
	public struct Mat3
	{
		// Token: 0x060002C7 RID: 711 RVA: 0x00008901 File Offset: 0x00006B01
		public Mat3(in Vec3 s, in Vec3 f, in Vec3 u)
		{
			this.s = s;
			this.f = f;
			this.u = u;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00008928 File Offset: 0x00006B28
		public Mat3(float sx, float sy, float sz, float fx, float fy, float fz, float ux, float uy, float uz)
		{
			this.s = new Vec3(sx, sy, sz, -1f);
			this.f = new Vec3(fx, fy, fz, -1f);
			this.u = new Vec3(ux, uy, uz, -1f);
		}

		// Token: 0x17000042 RID: 66
		public Vec3 this[int i]
		{
			get
			{
				switch (i)
				{
				case 0:
					return this.s;
				case 1:
					return this.f;
				case 2:
					return this.u;
				default:
					throw new IndexOutOfRangeException("Vec3 out of bounds.");
				}
			}
			set
			{
				switch (i)
				{
				case 0:
					this.s = value;
					return;
				case 1:
					this.f = value;
					return;
				case 2:
					this.u = value;
					return;
				default:
					throw new IndexOutOfRangeException("Vec3 out of bounds.");
				}
			}
		}

		// Token: 0x060002CB RID: 715 RVA: 0x000089E4 File Offset: 0x00006BE4
		public void RotateAboutSide(float a)
		{
			float num;
			float num2;
			MathF.SinCos(a, out num, out num2);
			Vec3 vec = this.f * num2 + this.u * num;
			Vec3 vec2 = this.u * num2 - this.f * num;
			this.u = vec2;
			this.f = vec;
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00008A48 File Offset: 0x00006C48
		public void RotateAboutForward(float a)
		{
			float num;
			float num2;
			MathF.SinCos(a, out num, out num2);
			Vec3 vec = this.s * num2 - this.u * num;
			Vec3 vec2 = this.u * num2 + this.s * num;
			this.s = vec;
			this.u = vec2;
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00008AAC File Offset: 0x00006CAC
		public void RotateAboutUp(float a)
		{
			float num;
			float num2;
			MathF.SinCos(a, out num, out num2);
			Vec3 vec = this.s * num2 + this.f * num;
			Vec3 vec2 = this.f * num2 - this.s * num;
			this.s = vec;
			this.f = vec2;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00008B10 File Offset: 0x00006D10
		public void RotateAboutAnArbitraryVector(in Vec3 v, float a)
		{
			this.s = this.s.RotateAboutAnArbitraryVector(v, a);
			this.f = this.f.RotateAboutAnArbitraryVector(v, a);
			this.u = this.u.RotateAboutAnArbitraryVector(v, a);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00008B68 File Offset: 0x00006D68
		public bool IsOrthonormal()
		{
			bool flag = this.s.IsUnit && this.f.IsUnit && this.u.IsUnit;
			float num = Vec3.DotProduct(this.s, this.f);
			if (num > 0.01f || num < -0.01f)
			{
				flag = false;
			}
			else
			{
				Vec3 vec = Vec3.CrossProduct(this.s, this.f);
				if (!this.u.NearlyEquals(in vec, 0.01f))
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x00008BED File Offset: 0x00006DED
		public bool IsLeftHanded()
		{
			return Vec3.DotProduct(Vec3.CrossProduct(this.s, this.f), this.u) < 0f;
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00008C12 File Offset: 0x00006E12
		public bool NearlyEquals(in Mat3 rhs, float epsilon = 1E-05f)
		{
			return this.s.NearlyEquals(in rhs.s, epsilon) && this.f.NearlyEquals(in rhs.f, epsilon) && this.u.NearlyEquals(in rhs.u, epsilon);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00008C50 File Offset: 0x00006E50
		public Vec3 TransformToParent(in Vec3 v)
		{
			return new Vec3(this.s.x * v.x + this.f.x * v.y + this.u.x * v.z, this.s.y * v.x + this.f.y * v.y + this.u.y * v.z, this.s.z * v.x + this.f.z * v.y + this.u.z * v.z, -1f);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00008D10 File Offset: 0x00006F10
		public Vec2 TransformToParent(in Vec2 v)
		{
			return new Vec2(this.s.x * v.x + this.f.x * v.y, this.s.y * v.x + this.f.y * v.y);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00008D6C File Offset: 0x00006F6C
		public Vec3 TransformToLocal(in Vec3 v)
		{
			return new Vec3(this.s.x * v.x + this.s.y * v.y + this.s.z * v.z, this.f.x * v.x + this.f.y * v.y + this.f.z * v.z, this.u.x * v.x + this.u.y * v.y + this.u.z * v.z, -1f);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00008E2C File Offset: 0x0000702C
		public Vec2 TransformToLocal(in Vec2 v)
		{
			return new Vec2(this.s.x * v.x + this.s.y * v.y, this.f.x * v.x + this.f.y * v.y);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00008E88 File Offset: 0x00007088
		public Mat3 TransformToParent(in Mat3 m)
		{
			Vec3 vec = this.TransformToParent(in m.s);
			Vec3 vec2 = this.TransformToParent(in m.f);
			Vec3 vec3 = this.TransformToParent(in m.u);
			return new Mat3(in vec, in vec2, in vec3);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00008EC8 File Offset: 0x000070C8
		public Mat3 TransformToLocal(in Mat3 m)
		{
			Mat3 mat;
			mat.s = this.TransformToLocal(in m.s);
			mat.f = this.TransformToLocal(in m.f);
			mat.u = this.TransformToLocal(in m.u);
			return mat;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x00008F10 File Offset: 0x00007110
		public void Orthonormalize()
		{
			this.f.Normalize();
			this.s = Vec3.CrossProduct(this.f, this.u);
			this.s.Normalize();
			this.u = Vec3.CrossProduct(this.s, this.f);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00008F63 File Offset: 0x00007163
		public void OrthonormalizeAccordingToForwardAndKeepUpAsZAxis()
		{
			this.f.z = 0f;
			this.f.Normalize();
			this.u = Vec3.Up;
			this.s = Vec3.CrossProduct(this.f, this.u);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00008FA4 File Offset: 0x000071A4
		public Mat3 GetUnitRotation(float removedScale)
		{
			float num = 1f / removedScale;
			Vec3 vec = this.s * num;
			Vec3 vec2 = this.f * num;
			Vec3 vec3 = this.u * num;
			return new Mat3(in vec, in vec2, in vec3);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00008FEC File Offset: 0x000071EC
		public Vec3 MakeUnit()
		{
			return new Vec3
			{
				x = this.s.Normalize(),
				y = this.f.Normalize(),
				z = this.u.Normalize()
			};
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00009038 File Offset: 0x00007238
		public bool IsUnit()
		{
			return this.s.IsUnit && this.f.IsUnit && this.u.IsUnit;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x00009061 File Offset: 0x00007261
		public void ApplyScaleLocal(float scaleAmount)
		{
			this.s *= scaleAmount;
			this.f *= scaleAmount;
			this.u *= scaleAmount;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000909C File Offset: 0x0000729C
		public void ApplyScaleLocal(in Vec3 scaleAmountXYZ)
		{
			this.s *= scaleAmountXYZ.x;
			this.f *= scaleAmountXYZ.y;
			this.u *= scaleAmountXYZ.z;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x000090EE File Offset: 0x000072EE
		public bool HasScale()
		{
			return !this.s.IsUnit || !this.f.IsUnit || !this.u.IsUnit;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000911A File Offset: 0x0000731A
		public Vec3 GetScaleVector()
		{
			return new Vec3(this.s.Length, this.f.Length, this.u.Length, -1f);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x00009147 File Offset: 0x00007347
		public Vec3 GetScaleVectorSquared()
		{
			return new Vec3(this.s.LengthSquared, this.f.LengthSquared, this.u.LengthSquared, -1f);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00009174 File Offset: 0x00007374
		public void ToQuaternion(out Quaternion quat)
		{
			quat = Quaternion.QuaternionFromMat3(this);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00009187 File Offset: 0x00007387
		public Quaternion ToQuaternion()
		{
			return Quaternion.QuaternionFromMat3(this);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00009194 File Offset: 0x00007394
		public static Mat3 Lerp(in Mat3 m1, in Mat3 m2, float alpha)
		{
			Mat3 identity = Mat3.Identity;
			identity.f = Vec3.Lerp(m1.f, m2.f, alpha);
			identity.u = Vec3.Lerp(m1.u, m2.u, alpha);
			identity.Orthonormalize();
			return identity;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000091E4 File Offset: 0x000073E4
		public static Mat3 LerpNonOrthogonal(in Mat3 m1, in Mat3 m2, float alpha)
		{
			Mat3 identity = Mat3.Identity;
			identity.f = Vec3.Lerp(m1.f, m2.f, alpha);
			identity.u = Vec3.Lerp(m1.u, m2.u, alpha);
			identity.s = Vec3.Lerp(m1.s, m2.s, alpha);
			return identity;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00009244 File Offset: 0x00007444
		public static Mat3 Slerp(in Mat3 m1, in Mat3 m2, float alpha)
		{
			return Quaternion.Slerp(Quaternion.QuaternionFromMat3(m1), Quaternion.QuaternionFromMat3(m2), alpha).ToMat3();
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00009278 File Offset: 0x00007478
		public static Mat3 SlerpFPSIndependent(in Mat3 m1, in Mat3 m2, float alpha)
		{
			float num = MathF.Pow(2f, -alpha);
			return Quaternion.Slerp(Quaternion.QuaternionFromMat3(m2), Quaternion.QuaternionFromMat3(m1), num).ToMat3();
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x000092B8 File Offset: 0x000074B8
		public static Mat3 CreateMat3WithForward(in Vec3 direction)
		{
			Mat3 identity = Mat3.Identity;
			identity.f = direction;
			identity.f.Normalize();
			if (MathF.Abs(identity.f.z) < 0.99f)
			{
				identity.u = new Vec3(0f, 0f, 1f, -1f);
			}
			else
			{
				identity.u = new Vec3(0f, 1f, 0f, -1f);
			}
			identity.s = Vec3.CrossProduct(identity.f, identity.u);
			identity.s.Normalize();
			identity.u = Vec3.CrossProduct(identity.s, identity.f);
			identity.u.Normalize();
			return identity;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000938C File Offset: 0x0000758C
		public static Mat3 CreateDiagonalMat3(in Vec3 diagonalData)
		{
			Vec3 vec = new Vec3(diagonalData.x, 0f, 0f, -1f);
			Vec3 vec2 = new Vec3(0f, diagonalData.y, 0f, -1f);
			Vec3 vec3 = new Vec3(0f, 0f, diagonalData.z, -1f);
			return new Mat3(in vec, in vec2, in vec3);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x000093F8 File Offset: 0x000075F8
		public Vec3 GetEulerAngles()
		{
			Mat3 mat = this;
			mat.Orthonormalize();
			return new Vec3(MathF.Asin(mat.f.z), MathF.Atan2(-mat.s.z, mat.u.z), MathF.Atan2(-mat.f.x, mat.f.y), -1f);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x00009468 File Offset: 0x00007668
		public Mat3 Transpose()
		{
			return new Mat3(this.s.x, this.f.x, this.u.x, this.s.y, this.f.y, this.u.y, this.s.z, this.f.z, this.u.z);
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060002EC RID: 748 RVA: 0x000094E0 File Offset: 0x000076E0
		public static Mat3 Identity
		{
			get
			{
				Vec3 vec = new Vec3(1f, 0f, 0f, -1f);
				Vec3 vec2 = new Vec3(0f, 1f, 0f, -1f);
				Vec3 vec3 = new Vec3(0f, 0f, 1f, -1f);
				return new Mat3(in vec, in vec2, in vec3);
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00009548 File Offset: 0x00007748
		public static Mat3 operator *(in Mat3 v, float a)
		{
			Vec3 vec = v.s * a;
			Vec3 vec2 = v.f * a;
			Vec3 vec3 = v.u * a;
			return new Mat3(in vec, in vec2, in vec3);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00009587 File Offset: 0x00007787
		public static bool operator ==(in Mat3 m1, in Mat3 m2)
		{
			return m1.f == m2.f && m1.u == m2.u;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x000095AF File Offset: 0x000077AF
		public static bool operator !=(in Mat3 m1, in Mat3 m2)
		{
			return m1.f != m2.f || m1.u != m2.u;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x000095D8 File Offset: 0x000077D8
		public override string ToString()
		{
			string text = "Mat3: ";
			text = string.Concat(new object[]
			{
				text,
				"s: ",
				this.s.x,
				", ",
				this.s.y,
				", ",
				this.s.z,
				";"
			});
			text = string.Concat(new object[]
			{
				text,
				"f: ",
				this.f.x,
				", ",
				this.f.y,
				", ",
				this.f.z,
				";"
			});
			text = string.Concat(new object[]
			{
				text,
				"u: ",
				this.u.x,
				", ",
				this.u.y,
				", ",
				this.u.z,
				";"
			});
			return text + "\n";
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00009734 File Offset: 0x00007934
		public override bool Equals(object obj)
		{
			Mat3 mat = (Mat3)obj;
			return (in this) == (in mat);
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00009750 File Offset: 0x00007950
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00009764 File Offset: 0x00007964
		public bool IsIdentity()
		{
			return this.s.x == 1f && this.s.y == 0f && this.s.z == 0f && this.f.x == 0f && this.f.y == 1f && this.f.z == 0f && this.u.x == 0f && this.u.y == 0f && this.u.z == 1f;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00009818 File Offset: 0x00007A18
		public bool IsZero()
		{
			return this.s.x == 0f && this.s.y == 0f && this.s.z == 0f && this.f.x == 0f && this.f.y == 0f && this.f.z == 0f && this.u.x == 0f && this.u.y == 0f && this.u.z == 0f;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x000098CC File Offset: 0x00007ACC
		public bool IsUniformScaled()
		{
			Vec3 scaleVectorSquared = this.GetScaleVectorSquared();
			return MBMath.ApproximatelyEquals(scaleVectorSquared.x, scaleVectorSquared.y, 0.01f) && MBMath.ApproximatelyEquals(scaleVectorSquared.x, scaleVectorSquared.z, 0.01f);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00009910 File Offset: 0x00007B10
		public void ApplyEulerAngles(in Vec3 eulerAngles)
		{
			this.RotateAboutUp(eulerAngles.z);
			this.RotateAboutSide(eulerAngles.x);
			this.RotateAboutForward(eulerAngles.y);
		}

		// Token: 0x04000128 RID: 296
		public Vec3 s;

		// Token: 0x04000129 RID: 297
		public Vec3 f;

		// Token: 0x0400012A RID: 298
		public Vec3 u;
	}
}
