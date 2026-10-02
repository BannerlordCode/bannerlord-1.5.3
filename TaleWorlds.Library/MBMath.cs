using System;
using System.Collections.Generic;
using System.Linq;

namespace TaleWorlds.Library
{
	// Token: 0x0200006B RID: 107
	public static class MBMath
	{
		// Token: 0x06000396 RID: 918 RVA: 0x0000C682 File Offset: 0x0000A882
		public static float ToRadians(this float f)
		{
			return f * 0.017453292f;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x0000C68B File Offset: 0x0000A88B
		public static float ToDegrees(this float f)
		{
			return f * 57.295776f;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x0000C694 File Offset: 0x0000A894
		public static bool ApproximatelyEqualsTo(this float f, float comparedValue, float epsilon = 1E-05f)
		{
			return Math.Abs(f - comparedValue) <= epsilon;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0000C6A4 File Offset: 0x0000A8A4
		public static bool ApproximatelyEquals(float first, float second, float epsilon = 1E-05f)
		{
			return Math.Abs(first - second) <= epsilon;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x0000C6B4 File Offset: 0x0000A8B4
		public static bool IsValidValue(float f)
		{
			return !float.IsNaN(f) && !float.IsInfinity(f);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0000C6C9 File Offset: 0x0000A8C9
		public static int ClampIndex(int value, int minValue, int maxValue)
		{
			return MBMath.ClampInt(value, minValue, maxValue - 1);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0000C6D5 File Offset: 0x0000A8D5
		public static int ClampInt(int value, int minValue, int maxValue)
		{
			if (value < minValue)
			{
				return minValue;
			}
			if (value > maxValue)
			{
				return maxValue;
			}
			return value;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0000C6E4 File Offset: 0x0000A8E4
		public static float ClampFloat(float value, float minValue, float maxValue)
		{
			if (value < minValue)
			{
				return minValue;
			}
			if (value > maxValue)
			{
				return maxValue;
			}
			return value;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x0000C6F3 File Offset: 0x0000A8F3
		public static void ClampUnit(ref float value)
		{
			value = MBMath.ClampFloat(value, 0f, 1f);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x0000C708 File Offset: 0x0000A908
		public static int GetNumberOfBitsToRepresentNumber(uint value)
		{
			int num = 0;
			for (uint num2 = value; num2 > 0U; num2 >>= 1)
			{
				num++;
			}
			return num;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0000C728 File Offset: 0x0000A928
		public static IEnumerable<ValueTuple<T, int>> DistributeShares<T>(int totalAward, IEnumerable<T> stakeHolders, Func<T, int> shareFunction)
		{
			List<ValueTuple<T, int>> sharesList = new List<ValueTuple<T, int>>(20);
			int num = 0;
			foreach (T t in stakeHolders)
			{
				int num2 = shareFunction(t);
				sharesList.Add(new ValueTuple<T, int>(t, num2));
				num += num2;
			}
			if (num > 0)
			{
				int remainingShares = num;
				int remaingAward = totalAward;
				int i = 0;
				while (i < sharesList.Count && remaingAward > 0)
				{
					int item = sharesList[i].Item2;
					int num3 = MathF.Round((float)remaingAward * (float)item / (float)remainingShares);
					if (num3 > remaingAward)
					{
						num3 = remaingAward;
					}
					remaingAward -= num3;
					remainingShares -= item;
					yield return new ValueTuple<T, int>(sharesList[i].Item1, num3);
					int num4 = i + 1;
					i = num4;
				}
			}
			yield break;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0000C748 File Offset: 0x0000A948
		public static int GetNumberOfBitsToRepresentNumber(ulong value)
		{
			int num = 0;
			for (ulong num2 = value; num2 > 0UL; num2 >>= 1)
			{
				num++;
			}
			return num;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0000C769 File Offset: 0x0000A969
		public static float Lerp(float valueFrom, float valueTo, float amount, float minimumDifference = 1E-05f)
		{
			if (Math.Abs(valueFrom - valueTo) <= minimumDifference)
			{
				return valueTo;
			}
			return valueFrom + (valueTo - valueFrom) * amount;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0000C77F File Offset: 0x0000A97F
		public static float LerpFPSIndependent(float valueFrom, float valueTo, float amount)
		{
			return MBMath.Lerp(valueTo, valueFrom, MathF.Pow(2f, -amount), 1E-05f);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0000C799 File Offset: 0x0000A999
		public static float LinearExtrapolation(float valueFrom, float valueTo, float amount)
		{
			return valueFrom + (valueTo - valueFrom) * amount;
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x0000C7A4 File Offset: 0x0000A9A4
		public static Vec3 Lerp(Vec3 vecFrom, Vec3 vecTo, float amount, float minimumDifference)
		{
			return new Vec3(MBMath.Lerp(vecFrom.x, vecTo.x, amount, minimumDifference), MBMath.Lerp(vecFrom.y, vecTo.y, amount, minimumDifference), MBMath.Lerp(vecFrom.z, vecTo.z, amount, minimumDifference), -1f);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0000C7F4 File Offset: 0x0000A9F4
		public static Vec2 Lerp(Vec2 vecFrom, Vec2 vecTo, float amount, float minimumDifference)
		{
			return new Vec2(MBMath.Lerp(vecFrom.x, vecTo.x, amount, minimumDifference), MBMath.Lerp(vecFrom.y, vecTo.y, amount, minimumDifference));
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x0000C821 File Offset: 0x0000AA21
		public static float Map(float input, float inputMinimum, float inputMaximum, float outputMinimum, float outputMaximum)
		{
			input = MBMath.ClampFloat(input, inputMinimum, inputMaximum);
			return (input - inputMinimum) * (outputMaximum - outputMinimum) / (inputMaximum - inputMinimum) + outputMinimum;
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0000C83C File Offset: 0x0000AA3C
		public static Mat3 Lerp(ref Mat3 matFrom, ref Mat3 matTo, float amount, float minimumDifference)
		{
			Vec3 vec = MBMath.Lerp(matFrom.s, matTo.s, amount, minimumDifference);
			Vec3 vec2 = MBMath.Lerp(matFrom.f, matTo.f, amount, minimumDifference);
			Vec3 vec3 = MBMath.Lerp(matFrom.u, matTo.u, amount, minimumDifference);
			return new Mat3(in vec, in vec2, in vec3);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000C890 File Offset: 0x0000AA90
		public static float LerpRadians(float valueFrom, float valueTo, float amount, float minChange, float maxChange)
		{
			float smallestDifferenceBetweenTwoAngles = MBMath.GetSmallestDifferenceBetweenTwoAngles(valueFrom, valueTo);
			if (Math.Abs(smallestDifferenceBetweenTwoAngles) <= minChange)
			{
				return valueTo;
			}
			float num = (float)Math.Sign(smallestDifferenceBetweenTwoAngles) * MBMath.ClampFloat(Math.Abs(smallestDifferenceBetweenTwoAngles * amount), minChange, maxChange);
			return MBMath.WrapAngle(valueFrom + num);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000C8D4 File Offset: 0x0000AAD4
		public static float SplitLerp(float value1, float value2, float value3, float cutOff, float amount, float minimumDifference)
		{
			if (amount <= cutOff)
			{
				float num = amount / cutOff;
				return MBMath.Lerp(value1, value2, num, minimumDifference);
			}
			float num2 = 1f - cutOff;
			float num3 = (amount - cutOff) / num2;
			return MBMath.Lerp(value2, value3, num3, minimumDifference);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0000C911 File Offset: 0x0000AB11
		public static float InverseLerp(float valueFrom, float valueTo, float value)
		{
			return (value - valueFrom) / (valueTo - valueFrom);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0000C91C File Offset: 0x0000AB1C
		public static float SmoothStep(float edge0, float edge1, float value)
		{
			float num = MBMath.ClampFloat((value - edge0) / (edge1 - edge0), 0f, 1f);
			return num * num * (3f - 2f * num);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0000C954 File Offset: 0x0000AB54
		public static float BilinearLerp(float topLeft, float topRight, float botLeft, float botRight, float x, float y)
		{
			float num = MBMath.Lerp(topLeft, topRight, x, 1E-05f);
			float num2 = MBMath.Lerp(botLeft, botRight, x, 1E-05f);
			return MBMath.Lerp(num, num2, y, 1E-05f);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0000C98C File Offset: 0x0000AB8C
		public static float GetSmallestDifferenceBetweenTwoAngles(float fromAngle, float toAngle)
		{
			float num = toAngle - fromAngle;
			if (num > 3.1415927f)
			{
				num = -6.2831855f + num;
			}
			if (num < -3.1415927f)
			{
				num = 6.2831855f + num;
			}
			return num;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0000C9C0 File Offset: 0x0000ABC0
		public static float ClampAngle(float angle, float restrictionCenter, float restrictionRange)
		{
			restrictionRange /= 2f;
			float smallestDifferenceBetweenTwoAngles = MBMath.GetSmallestDifferenceBetweenTwoAngles(restrictionCenter, angle);
			if (smallestDifferenceBetweenTwoAngles > restrictionRange)
			{
				angle = restrictionCenter + restrictionRange;
			}
			else if (smallestDifferenceBetweenTwoAngles < -restrictionRange)
			{
				angle = restrictionCenter - restrictionRange;
			}
			if (angle > 3.1415927f)
			{
				angle -= 6.2831855f;
			}
			else if (angle < -3.1415927f)
			{
				angle += 6.2831855f;
			}
			return angle;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0000CA18 File Offset: 0x0000AC18
		public static float WrapAngle(float angle)
		{
			angle = (float)Math.IEEERemainder((double)angle, 6.283185307179586);
			if (angle <= -3.1415927f)
			{
				angle += 6.2831855f;
			}
			else if (angle > 3.1415927f)
			{
				angle -= 6.2831855f;
			}
			return angle;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0000CA52 File Offset: 0x0000AC52
		public static float WrapAngleSafe(float angle)
		{
			while (angle <= -3.1415927f)
			{
				angle += 6.2831855f;
			}
			while (angle > 3.1415927f)
			{
				angle -= 6.2831855f;
			}
			return angle;
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x0000CA7B File Offset: 0x0000AC7B
		public static bool IsBetween(float numberToCheck, float bottom, float top)
		{
			return numberToCheck > bottom && numberToCheck < top;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0000CA87 File Offset: 0x0000AC87
		public static bool IsBetween(int value, int minValue, int maxValue)
		{
			return value >= minValue && value < maxValue;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0000CA93 File Offset: 0x0000AC93
		public static bool IsBetweenInclusive(float numberToCheck, float bottom, float top)
		{
			return numberToCheck >= bottom && numberToCheck <= top;
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x0000CAA2 File Offset: 0x0000ACA2
		public static uint ColorFromRGBA(float red, float green, float blue, float alpha)
		{
			return ((uint)(alpha * 255f) << 24) + ((uint)(red * 255f) << 16) + ((uint)(green * 255f) << 8) + (uint)(blue * 255f);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0000CAD0 File Offset: 0x0000ACD0
		public static Color HSBtoRGB(float hue, float saturation, float brightness, float outputAlpha)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = brightness * saturation;
			float num5 = num4 * (1f - MathF.Abs(hue * 0.016666668f % 2f - 1f));
			float num6 = brightness - num4;
			switch ((int)(hue * 0.016666668f % 6f))
			{
			case 0:
				num = num4;
				num2 = num5;
				num3 = 0f;
				break;
			case 1:
				num = num5;
				num2 = num4;
				num3 = 0f;
				break;
			case 2:
				num = 0f;
				num2 = num4;
				num3 = num5;
				break;
			case 3:
				num = 0f;
				num2 = num5;
				num3 = num4;
				break;
			case 4:
				num = num5;
				num2 = 0f;
				num3 = num4;
				break;
			case 5:
				num = num4;
				num2 = 0f;
				num3 = num5;
				break;
			}
			return new Color(num + num6, num2 + num6, num3 + num6, outputAlpha);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0000CBAC File Offset: 0x0000ADAC
		public static Vec3 RGBtoHSB(Color rgb)
		{
			Vec3 vec = new Vec3(0f, 0f, 0f, -1f);
			float num = MathF.Min(MathF.Min(rgb.Red, rgb.Green), rgb.Blue);
			float num2 = MathF.Max(MathF.Max(rgb.Red, rgb.Green), rgb.Blue);
			float num3 = num2 - num;
			vec.z = num2;
			if (MathF.Abs(num3) < 0.0001f)
			{
				vec.x = 0f;
			}
			else if (MathF.Abs(num2 - rgb.Red) < 0.0001f)
			{
				vec.x = 60f * ((rgb.Green - rgb.Blue) / num3 % 6f);
			}
			else if (MathF.Abs(num2 - rgb.Green) < 0.0001f)
			{
				vec.x = 60f * ((rgb.Blue - rgb.Red) / num3 + 2f);
			}
			else
			{
				vec.x = 60f * ((rgb.Red - rgb.Green) / num3 + 4f);
			}
			vec.x %= 360f;
			if (vec.x < 0f)
			{
				vec.x += 360f;
			}
			if (MathF.Abs(num2) < 0.0001f)
			{
				vec.y = 0f;
			}
			else
			{
				vec.y = num3 / num2;
			}
			return vec;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x0000CD20 File Offset: 0x0000AF20
		public static Vec3 GammaCorrectRGB(float gamma, Vec3 rgb)
		{
			float num = 1f / gamma;
			rgb.x = MathF.Pow(rgb.x, num);
			rgb.y = MathF.Pow(rgb.y, num);
			rgb.z = MathF.Pow(rgb.z, num);
			return rgb;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0000CD70 File Offset: 0x0000AF70
		public static float GetSignedDistanceOfPointToLineSegment(in Vec2 lineSegmentBegin, in Vec2 lineSegmentEnd, in Vec2 point)
		{
			Vec2 vec = lineSegmentBegin - point;
			Vec2 vec2 = lineSegmentEnd - lineSegmentBegin;
			return Vec2.Determinant(in vec, in vec2);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000CDAC File Offset: 0x0000AFAC
		public static float GetDistanceSquareOfPointToLineSegment(in Vec2 lineSegmentBegin, in Vec2 lineSegmentEnd, Vec2 point)
		{
			float num = Vec2.DotProduct(point - lineSegmentBegin, lineSegmentEnd - lineSegmentBegin) / (lineSegmentEnd - lineSegmentBegin).LengthSquared;
			if (num < 0f)
			{
				return (point - lineSegmentBegin).LengthSquared;
			}
			if (num <= 1f)
			{
				return (point - (lineSegmentBegin + num * (lineSegmentEnd - lineSegmentBegin))).LengthSquared;
			}
			return (point - lineSegmentEnd).LengthSquared;
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0000CE60 File Offset: 0x0000B060
		public static Vec2 ProjectPointOntoLine(Vec2 point, Vec2 lineStart, Vec2 lineEnd)
		{
			Vec2 vec = lineEnd - lineStart;
			float num = Vec2.DotProduct(point - lineStart, vec) / Vec2.DotProduct(vec, vec);
			return lineStart + num * vec;
		}

		// Token: 0x060003BC RID: 956 RVA: 0x0000CE98 File Offset: 0x0000B098
		public static Vec2 ClampToAxisAlignedRectangle(Vec2 point, Vec2 lineStart, Vec2 lineEnd)
		{
			float num = Math.Min(lineStart.x, lineEnd.x);
			float num2 = Math.Max(lineStart.x, lineEnd.x);
			float num3 = Math.Min(lineStart.y, lineEnd.y);
			float num4 = Math.Max(lineStart.y, lineEnd.y);
			return new Vec2(MathF.Clamp(point.x, num, num2), MathF.Clamp(point.y, num3, num4));
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0000CF0C File Offset: 0x0000B10C
		public static bool GetRayPlaneIntersectionPoint(in Vec3 planeNormal, in Vec3 planeCenter, in Vec3 rayOrigin, in Vec3 rayDirection, out float t)
		{
			float num = Vec3.DotProduct(planeNormal, rayDirection);
			if (num > 1E-06f)
			{
				Vec3 vec = planeCenter - rayOrigin;
				t = Vec3.DotProduct(vec, planeNormal) / num;
				return t >= 0f;
			}
			t = -1f;
			return false;
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000CF6D File Offset: 0x0000B16D
		public static bool PointLiesAheadOfPlane(in Vec3 planeNormal, in Vec3 planeCenter, in Vec3 point)
		{
			return Vec3.DotProduct(planeNormal, point - planeCenter) >= 0f;
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000CF98 File Offset: 0x0000B198
		public static Vec2 GetClosestPointOnLineSegmentToPoint(in Vec2 lineSegmentBegin, in Vec2 lineSegmentEnd, in Vec2 point)
		{
			Vec2 vec = lineSegmentEnd - lineSegmentBegin;
			if (!vec.IsNonZero())
			{
				return lineSegmentBegin;
			}
			float num = Vec2.DotProduct(point - lineSegmentBegin, vec) / Vec2.DotProduct(vec, vec);
			if (num < 0f)
			{
				return lineSegmentBegin;
			}
			if (num > 1f)
			{
				return lineSegmentEnd;
			}
			return lineSegmentBegin + vec * num;
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0000D018 File Offset: 0x0000B218
		public static Vec3 GetClosestPointOnLineSegmentToPoint(in Vec3 lineSegmentBegin, in Vec3 lineSegmentEnd, in Vec3 point)
		{
			Vec3 vec = lineSegmentEnd - lineSegmentBegin;
			if (!vec.IsNonZero)
			{
				return lineSegmentBegin;
			}
			float num = Vec3.DotProduct(point - lineSegmentBegin, vec) / Vec3.DotProduct(vec, vec);
			if (num < 0f)
			{
				return lineSegmentBegin;
			}
			if (num > 1f)
			{
				return lineSegmentEnd;
			}
			return lineSegmentBegin + vec * num;
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x0000D098 File Offset: 0x0000B298
		public static bool CheckLineToLineSegmentIntersection(Vec2 lineOrigin, Vec2 lineDirection, Vec2 segmentA, Vec2 segmentB, out float t, out Vec2 intersect)
		{
			t = float.MaxValue;
			intersect = Vec2.Zero;
			Vec2 vec = lineOrigin - segmentA;
			Vec2 vec2 = segmentB - segmentA;
			Vec2 vec3 = new Vec2(-lineDirection.y, lineDirection.x);
			float num = vec2.DotProduct(vec3);
			if (MathF.Abs(num) < 1E-05f)
			{
				return false;
			}
			float num2 = vec2.x * vec.y - vec2.y * vec.x;
			t = num2 / num;
			intersect = lineOrigin + lineDirection * t;
			float num3 = vec.DotProduct(vec3) / num;
			return num3 >= 0f && num3 <= 1f;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0000D154 File Offset: 0x0000B354
		public static bool IntersectLineSegmentWithTriangle(in Vec3 segStart, in Vec3 segEnd, in Vec3 triA, in Vec3 triB, in Vec3 triC)
		{
			Vec3 vec = triB - triA;
			Vec3 vec2 = triC - triA;
			Vec3 vec3 = segEnd - segStart;
			Vec3 vec4 = Vec3.CrossProduct(vec3, vec2);
			float num = Vec3.DotProduct(vec, vec4);
			if (MathF.Abs(num) < 1E-06f)
			{
				return false;
			}
			float num2 = 1f / num;
			Vec3 vec5 = segStart - triA;
			float num3 = num2 * Vec3.DotProduct(vec5, vec4);
			if (num3 < 0f || num3 > 1f)
			{
				return false;
			}
			Vec3 vec6 = Vec3.CrossProduct(vec5, vec);
			float num4 = num2 * Vec3.DotProduct(vec3, vec6);
			if (num4 < 0f || num3 + num4 > 1f)
			{
				return false;
			}
			float num5 = num2 * Vec3.DotProduct(vec2, vec6);
			return num5 >= 0f && num5 <= 1f;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x0000D248 File Offset: 0x0000B448
		public static bool IntersectLineSegmentWithBoundingBox(in Vec3 start, in Vec3 end, in Vec3 min, in Vec3 max)
		{
			Vec3 vec = end - start;
			float num = 0f;
			float num2 = 1f;
			for (int i = 0; i < 3; i++)
			{
				Vec3 vec2 = start;
				float num3 = vec2[i];
				float num4 = vec[i];
				vec2 = min;
				float num5 = vec2[i];
				vec2 = max;
				float num6 = vec2[i];
				if (MathF.Abs(num4) < 1E-08f)
				{
					if (num3 < num5 || num3 > num6)
					{
						return false;
					}
				}
				else
				{
					float num7 = 1f / num4;
					float num8 = (num5 - num3) * num7;
					float num9 = (num6 - num3) * num7;
					if (num8 > num9)
					{
						float num10 = num9;
						float num11 = num8;
						num8 = num10;
						num9 = num11;
					}
					if (num8 > num)
					{
						num = num8;
					}
					if (num9 < num2)
					{
						num2 = num9;
					}
					if (num > num2)
					{
						return false;
					}
				}
			}
			return num2 >= 0f && num <= 1f;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0000D33C File Offset: 0x0000B53C
		public static bool CheckLineSegmentToLineSegmentIntersection(Vec2 segment1Start, Vec2 segment1End, Vec2 segment2Start, Vec2 segment2End)
		{
			return Vec2.GetWindingOrder(segment1Start, segment2Start, segment2End) != Vec2.GetWindingOrder(segment1End, segment2Start, segment2End) && Vec2.GetWindingOrder(segment1Start, segment1End, segment2Start) != Vec2.GetWindingOrder(segment1Start, segment1End, segment2End);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x0000D368 File Offset: 0x0000B568
		public static bool CheckPointInsidePolygon(in Vec2 v0, in Vec2 v1, in Vec2 v2, in Vec2 v3, in Vec2 point)
		{
			int num = 0;
			if (point.y < v0.y != point.y < v1.y && point.x < v0.x + (point.y - v0.y) / (v1.y - v0.y) * (v1.x - v0.x))
			{
				num++;
			}
			if (point.y < v1.y != point.y < v2.y && point.x < v1.x + (point.y - v1.y) / (v2.y - v1.y) * (v2.x - v1.x))
			{
				num++;
			}
			if (point.y < v2.y != point.y < v3.y && point.x < v2.x + (point.y - v2.y) / (v3.y - v2.y) * (v3.x - v2.x))
			{
				num++;
			}
			if (point.y < v3.y != point.y < v0.y && point.x < v3.x + (point.y - v3.y) / (v0.y - v3.y) * (v0.x - v3.x))
			{
				num++;
			}
			return num % 2 == 1;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0000D50C File Offset: 0x0000B70C
		public static bool CheckPolygonIntersection(Vec2[] polygon1, Vec2[] polygon2)
		{
			for (int i = 0; i < polygon1.Length; i++)
			{
				Vec2 vec = polygon1[i];
				Vec2 vec2 = polygon1[(i + 1) % polygon1.Length];
				for (int j = 0; j < polygon2.Length; j++)
				{
					Vec2 vec3 = polygon2[j];
					Vec2 vec4 = polygon2[(j + 1) % polygon2.Length];
					if (MBMath.CheckLineSegmentToLineSegmentIntersection(vec, vec2, vec3, vec4))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0000D574 File Offset: 0x0000B774
		public static bool CheckPolygonLineSegmentIntersection(MBList<Vec2> polygon, Vec2 segmentStart, Vec2 segmentEnd)
		{
			for (int i = 0; i < polygon.Count; i++)
			{
				Vec2 vec = polygon[i];
				Vec2 vec2 = polygon[(i + 1) % polygon.Count];
				if (MBMath.CheckLineSegmentToLineSegmentIntersection(vec, vec2, segmentStart, segmentEnd))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0000D5B8 File Offset: 0x0000B7B8
		public static bool IntersectRayWithPolygon(Vec2 rayOrigin, Vec2 rayDir, MBList<Vec2> polygon, out Vec2 intersectionPoint)
		{
			List<ValueTuple<float, Vec2>> list = new List<ValueTuple<float, Vec2>>();
			for (int j = 0; j < polygon.Count; j++)
			{
				Vec2 vec = polygon[j];
				Vec2 vec2 = polygon[(j + 1) % polygon.Count];
				float num;
				Vec2 vec3;
				if (MBMath.CheckLineToLineSegmentIntersection(rayOrigin, rayDir, vec, vec2, out num, out vec3) && num > 0f)
				{
					list.Add(new ValueTuple<float, Vec2>(num, vec3));
				}
			}
			list = list.OrderBy<ValueTuple<float, Vec2>, float>((ValueTuple<float, Vec2> i) => i.Item1).ToList<ValueTuple<float, Vec2>>();
			if (list.Count != 0)
			{
				intersectionPoint = list[0].Item2;
				return true;
			}
			intersectionPoint = rayOrigin;
			return false;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0000D66C File Offset: 0x0000B86C
		public static string ToOrdinal(int number)
		{
			if (number < 0)
			{
				return number.ToString();
			}
			long num = (long)(number % 100);
			if (num >= 11L && num <= 13L)
			{
				return number + "th";
			}
			switch (number % 10)
			{
			case 1:
				return number + "st";
			case 2:
				return number + "nd";
			case 3:
				return number + "rd";
			default:
				return number + "th";
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0000D708 File Offset: 0x0000B908
		public static int IndexOfMax<T>(MBReadOnlyList<T> array, Func<T, int> func)
		{
			int num = int.MinValue;
			int num2 = -1;
			for (int i = 0; i < array.Count; i++)
			{
				int num3 = func(array[i]);
				if (num3 > num)
				{
					num = num3;
					num2 = i;
				}
			}
			return num2;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x0000D748 File Offset: 0x0000B948
		public static T MaxElement<T>(IEnumerable<T> collection, Func<T, float> func)
		{
			float num = float.MinValue;
			T t = default(T);
			foreach (T t2 in collection)
			{
				float num2 = func(t2);
				if (num2 > num)
				{
					num = num2;
					t = t2;
				}
			}
			return t;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000D7AC File Offset: 0x0000B9AC
		public static ValueTuple<T, T> MaxElements2<T>(IEnumerable<T> collection, Func<T, float> func)
		{
			float num = float.MinValue;
			float num2 = float.MinValue;
			T t = default(T);
			T t2 = default(T);
			foreach (T t3 in collection)
			{
				float num3 = func(t3);
				if (num3 > num2)
				{
					if (num3 > num)
					{
						num2 = num;
						t2 = t;
						num = num3;
						t = t3;
					}
					else
					{
						num2 = num3;
						t2 = t3;
					}
				}
			}
			return new ValueTuple<T, T>(t, t2);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000D83C File Offset: 0x0000BA3C
		public static ValueTuple<T, T, T> MaxElements3<T>(IEnumerable<T> collection, Func<T, float> func)
		{
			float num = float.MinValue;
			float num2 = float.MinValue;
			float num3 = float.MinValue;
			T t = default(T);
			T t2 = default(T);
			T t3 = default(T);
			foreach (T t4 in collection)
			{
				float num4 = func(t4);
				if (num4 > num3)
				{
					if (num4 > num2)
					{
						num3 = num2;
						t3 = t2;
						if (num4 > num)
						{
							num2 = num;
							t2 = t;
							num = num4;
							t = t4;
						}
						else
						{
							num2 = num4;
							t2 = t4;
						}
					}
					else
					{
						num3 = num4;
						t3 = t4;
					}
				}
			}
			return new ValueTuple<T, T, T>(t, t2, t3);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000D8F4 File Offset: 0x0000BAF4
		public static ValueTuple<T, T, T, T> MaxElements4<T>(IEnumerable<T> collection, Func<T, float> func)
		{
			float num = float.MinValue;
			float num2 = float.MinValue;
			float num3 = float.MinValue;
			float num4 = float.MinValue;
			T t = default(T);
			T t2 = default(T);
			T t3 = default(T);
			T t4 = default(T);
			foreach (T t5 in collection)
			{
				float num5 = func(t5);
				if (num5 > num4)
				{
					if (num5 > num3)
					{
						num4 = num3;
						t4 = t3;
						if (num5 > num2)
						{
							num3 = num2;
							t3 = t2;
							if (num5 > num)
							{
								num2 = num;
								t2 = t;
								num = num5;
								t = t5;
							}
							else
							{
								num2 = num5;
								t2 = t5;
							}
						}
						else
						{
							num3 = num5;
							t3 = t5;
						}
					}
					else
					{
						num4 = num5;
						t4 = t5;
					}
				}
			}
			return new ValueTuple<T, T, T, T>(t, t2, t3, t4);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000D9D4 File Offset: 0x0000BBD4
		public static ValueTuple<T, T, T, T, T> MaxElements5<T>(IEnumerable<T> collection, Func<T, float> func)
		{
			float num = float.MinValue;
			float num2 = float.MinValue;
			float num3 = float.MinValue;
			float num4 = float.MinValue;
			float num5 = float.MinValue;
			T t = default(T);
			T t2 = default(T);
			T t3 = default(T);
			T t4 = default(T);
			T t5 = default(T);
			foreach (T t6 in collection)
			{
				float num6 = func(t6);
				if (num6 > num5)
				{
					if (num6 > num4)
					{
						num5 = num4;
						t5 = t4;
						if (num6 > num3)
						{
							num4 = num3;
							t4 = t3;
							if (num6 > num2)
							{
								num3 = num2;
								t3 = t2;
								if (num6 > num)
								{
									num2 = num;
									t2 = t;
									num = num6;
									t = t6;
								}
								else
								{
									num2 = num6;
									t2 = t6;
								}
							}
							else
							{
								num3 = num6;
								t3 = t6;
							}
						}
						else
						{
							num4 = num6;
							t4 = t6;
						}
					}
					else
					{
						num5 = num6;
						t5 = t6;
					}
				}
			}
			return new ValueTuple<T, T, T, T, T>(t, t2, t3, t4, t5);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000DADC File Offset: 0x0000BCDC
		public static IList<T> TopologySort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> getDependencies)
		{
			List<T> list = new List<T>();
			Dictionary<T, bool> dictionary = new Dictionary<T, bool>();
			foreach (T t in source)
			{
				MBMath.Visit<T>(t, getDependencies, list, dictionary);
			}
			return list;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000DB34 File Offset: 0x0000BD34
		private static void Visit<T>(T item, Func<T, IEnumerable<T>> getDependencies, List<T> sorted, Dictionary<T, bool> visited)
		{
			bool flag;
			if (visited.TryGetValue(item, out flag))
			{
				return;
			}
			visited[item] = true;
			IEnumerable<T> enumerable = getDependencies(item);
			if (enumerable != null)
			{
				foreach (T t in enumerable)
				{
					MBMath.Visit<T>(t, getDependencies, sorted, visited);
				}
			}
			visited[item] = false;
			sorted.Add(item);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000DBAC File Offset: 0x0000BDAC
		public static Vec3 FindPlaneLineIntersectionPointWithNormal(Vec3 planeP1, Vec3 planeNormal, Vec3 mouseP1, Vec3 mouseP2, out bool exceptionZero)
		{
			float num = planeNormal.x * mouseP2.x - planeNormal.x * mouseP1.x + planeNormal.y * mouseP2.y - planeNormal.y * mouseP1.y + planeNormal.z * mouseP2.z - planeNormal.z * mouseP1.z;
			if (num == 0f)
			{
				exceptionZero = true;
				return planeNormal;
			}
			exceptionZero = false;
			float num2 = (-planeNormal.x * mouseP1.x + planeNormal.x * planeP1.x - planeNormal.y * mouseP1.y + planeNormal.y * planeP1.y - planeNormal.z * mouseP1.z + planeNormal.z * planeP1.z) / num;
			return new Vec3(mouseP1.x + (mouseP2.x - mouseP1.x) * num2, mouseP1.y + (mouseP2.y - mouseP1.y) * num2, mouseP1.z + (mouseP2.z - mouseP1.z) * num2, -1f);
		}

		// Token: 0x0400013B RID: 315
		public const float TwoPI = 6.2831855f;

		// Token: 0x0400013C RID: 316
		public const float PI = 3.1415927f;

		// Token: 0x0400013D RID: 317
		public const float HalfPI = 1.5707964f;

		// Token: 0x0400013E RID: 318
		public const float E = 2.7182817f;

		// Token: 0x0400013F RID: 319
		public const float DegreesToRadians = 0.017453292f;

		// Token: 0x04000140 RID: 320
		public const float RadiansToDegrees = 57.295776f;

		// Token: 0x04000141 RID: 321
		public const float Epsilon = 1E-05f;
	}
}
