using System;
using System.Numerics;
using TaleWorlds.Library;

namespace TaleWorlds.TwoDimension.Standalone
{
	// Token: 0x0200000D RID: 13
	public static class MatrixExtensions
	{
		// Token: 0x06000091 RID: 145 RVA: 0x00005A0C File Offset: 0x00003C0C
		public static Matrix4x4 ToMatrix4x4(this MatrixFrame matrixFrame)
		{
			return new Matrix4x4(matrixFrame.rotation.s.x, matrixFrame.rotation.s.y, matrixFrame.rotation.s.z, matrixFrame.rotation.s.w, matrixFrame.rotation.f.x, matrixFrame.rotation.f.y, matrixFrame.rotation.f.z, matrixFrame.rotation.f.w, matrixFrame.rotation.u.x, matrixFrame.rotation.u.y, matrixFrame.rotation.u.z, matrixFrame.rotation.u.w, matrixFrame.origin.x, matrixFrame.origin.y, matrixFrame.origin.z, matrixFrame.origin.w);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00005B0C File Offset: 0x00003D0C
		public static MatrixFrame ToMatrixFrame(this Matrix4x4 matrix)
		{
			return new MatrixFrame(matrix.M11, matrix.M12, matrix.M13, matrix.M14, matrix.M21, matrix.M22, matrix.M23, matrix.M24, matrix.M31, matrix.M32, matrix.M33, matrix.M34, matrix.M41, matrix.M42, matrix.M43, matrix.M44);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00005B80 File Offset: 0x00003D80
		public static bool AreAllComponentsValid(this Matrix4x4 matrix)
		{
			return !float.IsNaN(matrix.M11) && !float.IsNaN(matrix.M12) && !float.IsNaN(matrix.M13) && !float.IsNaN(matrix.M14) && !float.IsNaN(matrix.M21) && !float.IsNaN(matrix.M22) && !float.IsNaN(matrix.M23) && !float.IsNaN(matrix.M24) && !float.IsNaN(matrix.M31) && !float.IsNaN(matrix.M32) && !float.IsNaN(matrix.M33) && !float.IsNaN(matrix.M34) && !float.IsNaN(matrix.M41) && !float.IsNaN(matrix.M42) && !float.IsNaN(matrix.M43) && !float.IsNaN(matrix.M44) && !float.IsInfinity(matrix.M11) && !float.IsInfinity(matrix.M12) && !float.IsInfinity(matrix.M13) && !float.IsInfinity(matrix.M14) && !float.IsInfinity(matrix.M21) && !float.IsInfinity(matrix.M22) && !float.IsInfinity(matrix.M23) && !float.IsInfinity(matrix.M24) && !float.IsInfinity(matrix.M31) && !float.IsInfinity(matrix.M32) && !float.IsInfinity(matrix.M33) && !float.IsInfinity(matrix.M34) && !float.IsInfinity(matrix.M41) && !float.IsInfinity(matrix.M42) && !float.IsInfinity(matrix.M43) && !float.IsInfinity(matrix.M44);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005D74 File Offset: 0x00003F74
		public static bool AreAllComponentsValid(this MatrixFrame matrix)
		{
			return matrix.origin.IsValidXYZW && matrix.rotation.s.IsValidXYZW && matrix.rotation.f.IsValidXYZW && matrix.rotation.u.IsValidXYZW;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005DC8 File Offset: 0x00003FC8
		public static MatrixFrame CreateOrthographicOffCenter(float left, float right, float bottom, float top, float zNearPlane, float zFarPlane)
		{
			return Matrix4x4.CreateOrthographicOffCenter(left, right, bottom, top, zNearPlane, zFarPlane).ToMatrixFrame();
		}
	}
}
