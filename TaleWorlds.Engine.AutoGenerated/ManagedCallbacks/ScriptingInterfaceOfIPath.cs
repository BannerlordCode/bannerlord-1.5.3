using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x0200001F RID: 31
	internal class ScriptingInterfaceOfIPath : IPath
	{
		// Token: 0x060003C6 RID: 966 RVA: 0x0001514B File Offset: 0x0001334B
		public int AddPathPoint(UIntPtr ptr, int newNodeIndex)
		{
			return ScriptingInterfaceOfIPath.call_AddPathPointDelegate(ptr, newNodeIndex);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00015159 File Offset: 0x00013359
		public void DeletePathPoint(UIntPtr ptr, int newNodeIndex)
		{
			ScriptingInterfaceOfIPath.call_DeletePathPointDelegate(ptr, newNodeIndex);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00015167 File Offset: 0x00013367
		public float GetArcLength(UIntPtr ptr, int firstPoint)
		{
			return ScriptingInterfaceOfIPath.call_GetArcLengthDelegate(ptr, firstPoint);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00015175 File Offset: 0x00013375
		public void GetHermiteFrameAndColorForDistance(UIntPtr ptr, out MatrixFrame frame, out Vec3 color, float distance)
		{
			ScriptingInterfaceOfIPath.call_GetHermiteFrameAndColorForDistanceDelegate(ptr, out frame, out color, distance);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00015186 File Offset: 0x00013386
		public void GetHermiteFrameForDistance(UIntPtr ptr, ref MatrixFrame frame, float distance)
		{
			ScriptingInterfaceOfIPath.call_GetHermiteFrameForDistanceDelegate(ptr, ref frame, distance);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00015195 File Offset: 0x00013395
		public void GetHermiteFrameForDt(UIntPtr ptr, ref MatrixFrame frame, float phase, int firstPoint)
		{
			ScriptingInterfaceOfIPath.call_GetHermiteFrameForDtDelegate(ptr, ref frame, phase, firstPoint);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000151A6 File Offset: 0x000133A6
		public string GetName(UIntPtr ptr)
		{
			if (ScriptingInterfaceOfIPath.call_GetNameDelegate(ptr) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000151BD File Offset: 0x000133BD
		public void GetNearestHermiteFrameWithValidAlphaForDistance(UIntPtr ptr, ref MatrixFrame frame, float distance, bool searchForward, float alphaThreshold)
		{
			ScriptingInterfaceOfIPath.call_GetNearestHermiteFrameWithValidAlphaForDistanceDelegate(ptr, ref frame, distance, searchForward, alphaThreshold);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x000151D0 File Offset: 0x000133D0
		public int GetNumberOfPoints(UIntPtr ptr)
		{
			return ScriptingInterfaceOfIPath.call_GetNumberOfPointsDelegate(ptr);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x000151E0 File Offset: 0x000133E0
		public void GetPoints(UIntPtr ptr, MatrixFrame[] points)
		{
			PinnedArrayData<MatrixFrame> pinnedArrayData = new PinnedArrayData<MatrixFrame>(points, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIPath.call_GetPointsDelegate(ptr, pointer);
			pinnedArrayData.Dispose();
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00015211 File Offset: 0x00013411
		public float GetTotalLength(UIntPtr ptr)
		{
			return ScriptingInterfaceOfIPath.call_GetTotalLengthDelegate(ptr);
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001521E File Offset: 0x0001341E
		public int GetVersion(UIntPtr ptr)
		{
			return ScriptingInterfaceOfIPath.call_GetVersionDelegate(ptr);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0001522B File Offset: 0x0001342B
		public bool HasValidAlphaAtPathPoint(UIntPtr ptr, int nodeIndex, float alphaThreshold)
		{
			return ScriptingInterfaceOfIPath.call_HasValidAlphaAtPathPointDelegate(ptr, nodeIndex, alphaThreshold);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0001523A File Offset: 0x0001343A
		public void SetFrameOfPoint(UIntPtr ptr, int pointIndex, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfIPath.call_SetFrameOfPointDelegate(ptr, pointIndex, ref frame);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00015249 File Offset: 0x00013449
		public void SetTangentPositionOfPoint(UIntPtr ptr, int pointIndex, int tangentIndex, ref Vec3 position)
		{
			ScriptingInterfaceOfIPath.call_SetTangentPositionOfPointDelegate(ptr, pointIndex, tangentIndex, ref position);
		}

		// Token: 0x04000333 RID: 819
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000334 RID: 820
		public static ScriptingInterfaceOfIPath.AddPathPointDelegate call_AddPathPointDelegate;

		// Token: 0x04000335 RID: 821
		public static ScriptingInterfaceOfIPath.DeletePathPointDelegate call_DeletePathPointDelegate;

		// Token: 0x04000336 RID: 822
		public static ScriptingInterfaceOfIPath.GetArcLengthDelegate call_GetArcLengthDelegate;

		// Token: 0x04000337 RID: 823
		public static ScriptingInterfaceOfIPath.GetHermiteFrameAndColorForDistanceDelegate call_GetHermiteFrameAndColorForDistanceDelegate;

		// Token: 0x04000338 RID: 824
		public static ScriptingInterfaceOfIPath.GetHermiteFrameForDistanceDelegate call_GetHermiteFrameForDistanceDelegate;

		// Token: 0x04000339 RID: 825
		public static ScriptingInterfaceOfIPath.GetHermiteFrameForDtDelegate call_GetHermiteFrameForDtDelegate;

		// Token: 0x0400033A RID: 826
		public static ScriptingInterfaceOfIPath.GetNameDelegate call_GetNameDelegate;

		// Token: 0x0400033B RID: 827
		public static ScriptingInterfaceOfIPath.GetNearestHermiteFrameWithValidAlphaForDistanceDelegate call_GetNearestHermiteFrameWithValidAlphaForDistanceDelegate;

		// Token: 0x0400033C RID: 828
		public static ScriptingInterfaceOfIPath.GetNumberOfPointsDelegate call_GetNumberOfPointsDelegate;

		// Token: 0x0400033D RID: 829
		public static ScriptingInterfaceOfIPath.GetPointsDelegate call_GetPointsDelegate;

		// Token: 0x0400033E RID: 830
		public static ScriptingInterfaceOfIPath.GetTotalLengthDelegate call_GetTotalLengthDelegate;

		// Token: 0x0400033F RID: 831
		public static ScriptingInterfaceOfIPath.GetVersionDelegate call_GetVersionDelegate;

		// Token: 0x04000340 RID: 832
		public static ScriptingInterfaceOfIPath.HasValidAlphaAtPathPointDelegate call_HasValidAlphaAtPathPointDelegate;

		// Token: 0x04000341 RID: 833
		public static ScriptingInterfaceOfIPath.SetFrameOfPointDelegate call_SetFrameOfPointDelegate;

		// Token: 0x04000342 RID: 834
		public static ScriptingInterfaceOfIPath.SetTangentPositionOfPointDelegate call_SetTangentPositionOfPointDelegate;

		// Token: 0x020003A4 RID: 932
		// (Invoke) Token: 0x060014BB RID: 5307
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int AddPathPointDelegate(UIntPtr ptr, int newNodeIndex);

		// Token: 0x020003A5 RID: 933
		// (Invoke) Token: 0x060014BF RID: 5311
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void DeletePathPointDelegate(UIntPtr ptr, int newNodeIndex);

		// Token: 0x020003A6 RID: 934
		// (Invoke) Token: 0x060014C3 RID: 5315
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetArcLengthDelegate(UIntPtr ptr, int firstPoint);

		// Token: 0x020003A7 RID: 935
		// (Invoke) Token: 0x060014C7 RID: 5319
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetHermiteFrameAndColorForDistanceDelegate(UIntPtr ptr, out MatrixFrame frame, out Vec3 color, float distance);

		// Token: 0x020003A8 RID: 936
		// (Invoke) Token: 0x060014CB RID: 5323
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetHermiteFrameForDistanceDelegate(UIntPtr ptr, ref MatrixFrame frame, float distance);

		// Token: 0x020003A9 RID: 937
		// (Invoke) Token: 0x060014CF RID: 5327
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetHermiteFrameForDtDelegate(UIntPtr ptr, ref MatrixFrame frame, float phase, int firstPoint);

		// Token: 0x020003AA RID: 938
		// (Invoke) Token: 0x060014D3 RID: 5331
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNameDelegate(UIntPtr ptr);

		// Token: 0x020003AB RID: 939
		// (Invoke) Token: 0x060014D7 RID: 5335
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetNearestHermiteFrameWithValidAlphaForDistanceDelegate(UIntPtr ptr, ref MatrixFrame frame, float distance, [MarshalAs(UnmanagedType.U1)] bool searchForward, float alphaThreshold);

		// Token: 0x020003AC RID: 940
		// (Invoke) Token: 0x060014DB RID: 5339
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNumberOfPointsDelegate(UIntPtr ptr);

		// Token: 0x020003AD RID: 941
		// (Invoke) Token: 0x060014DF RID: 5343
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetPointsDelegate(UIntPtr ptr, IntPtr points);

		// Token: 0x020003AE RID: 942
		// (Invoke) Token: 0x060014E3 RID: 5347
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetTotalLengthDelegate(UIntPtr ptr);

		// Token: 0x020003AF RID: 943
		// (Invoke) Token: 0x060014E7 RID: 5351
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetVersionDelegate(UIntPtr ptr);

		// Token: 0x020003B0 RID: 944
		// (Invoke) Token: 0x060014EB RID: 5355
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasValidAlphaAtPathPointDelegate(UIntPtr ptr, int nodeIndex, float alphaThreshold);

		// Token: 0x020003B1 RID: 945
		// (Invoke) Token: 0x060014EF RID: 5359
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFrameOfPointDelegate(UIntPtr ptr, int pointIndex, ref MatrixFrame frame);

		// Token: 0x020003B2 RID: 946
		// (Invoke) Token: 0x060014F3 RID: 5363
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetTangentPositionOfPointDelegate(UIntPtr ptr, int pointIndex, int tangentIndex, ref Vec3 position);
	}
}
