using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x0200000A RID: 10
	internal class ScriptingInterfaceOfICamera : ICamera
	{
		// Token: 0x06000070 RID: 112 RVA: 0x0000EF63 File Offset: 0x0000D163
		public bool CheckEntityVisibility(UIntPtr cameraPointer, UIntPtr entityPointer)
		{
			return ScriptingInterfaceOfICamera.call_CheckEntityVisibilityDelegate(cameraPointer, entityPointer);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000EF71 File Offset: 0x0000D171
		public void ConstructCameraFromPositionElevationBearing(Vec3 position, float elevation, float bearing, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfICamera.call_ConstructCameraFromPositionElevationBearingDelegate(position, elevation, bearing, ref outFrame);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000EF84 File Offset: 0x0000D184
		public Camera CreateCamera()
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfICamera.call_CreateCameraDelegate();
			Camera camera = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				camera = new Camera(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return camera;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000EFCD File Offset: 0x0000D1CD
		public bool EnclosesPoint(UIntPtr cameraPointer, Vec3 pointInWorldSpace)
		{
			return ScriptingInterfaceOfICamera.call_EnclosesPointDelegate(cameraPointer, pointInWorldSpace);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000EFDB File Offset: 0x0000D1DB
		public void FillParametersFrom(UIntPtr cameraPointer, UIntPtr otherCameraPointer)
		{
			ScriptingInterfaceOfICamera.call_FillParametersFromDelegate(cameraPointer, otherCameraPointer);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x0000EFE9 File Offset: 0x0000D1E9
		public float GetAspectRatio(UIntPtr cameraPointer)
		{
			return ScriptingInterfaceOfICamera.call_GetAspectRatioDelegate(cameraPointer);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000EFF8 File Offset: 0x0000D1F8
		public GameEntity GetEntity(UIntPtr cameraPointer)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfICamera.call_GetEntityDelegate(cameraPointer);
			GameEntity gameEntity = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				gameEntity = new GameEntity(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntity;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x0000F042 File Offset: 0x0000D242
		public float GetFar(UIntPtr cameraPointer)
		{
			return ScriptingInterfaceOfICamera.call_GetFarDelegate(cameraPointer);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x0000F04F File Offset: 0x0000D24F
		public float GetFovHorizontal(UIntPtr cameraPointer)
		{
			return ScriptingInterfaceOfICamera.call_GetFovHorizontalDelegate(cameraPointer);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000F05C File Offset: 0x0000D25C
		public float GetFovVertical(UIntPtr cameraPointer)
		{
			return ScriptingInterfaceOfICamera.call_GetFovVerticalDelegate(cameraPointer);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000F069 File Offset: 0x0000D269
		public void GetFrame(UIntPtr cameraPointer, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfICamera.call_GetFrameDelegate(cameraPointer, ref outFrame);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000F077 File Offset: 0x0000D277
		public float GetHorizontalFov(UIntPtr cameraPointer)
		{
			return ScriptingInterfaceOfICamera.call_GetHorizontalFovDelegate(cameraPointer);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x0000F084 File Offset: 0x0000D284
		public float GetNear(UIntPtr cameraPointer)
		{
			return ScriptingInterfaceOfICamera.call_GetNearDelegate(cameraPointer);
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000F094 File Offset: 0x0000D294
		public void GetNearPlanePoints(UIntPtr cameraPointer, Vec3[] nearPlanePoints)
		{
			PinnedArrayData<Vec3> pinnedArrayData = new PinnedArrayData<Vec3>(nearPlanePoints, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfICamera.call_GetNearPlanePointsDelegate(cameraPointer, pointer);
			pinnedArrayData.Dispose();
		}

		// Token: 0x0600007E RID: 126 RVA: 0x0000F0C8 File Offset: 0x0000D2C8
		public void GetNearPlanePointsStatic(ref MatrixFrame cameraFrame, float verticalFov, float aspectRatioXY, float newDNear, float newDFar, Vec3[] nearPlanePoints)
		{
			PinnedArrayData<Vec3> pinnedArrayData = new PinnedArrayData<Vec3>(nearPlanePoints, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfICamera.call_GetNearPlanePointsStaticDelegate(ref cameraFrame, verticalFov, aspectRatioXY, newDNear, newDFar, pointer);
			pinnedArrayData.Dispose();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x0000F100 File Offset: 0x0000D300
		public void GetViewProjMatrix(UIntPtr cameraPointer, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfICamera.call_GetViewProjMatrixDelegate(cameraPointer, ref frame);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x0000F10E File Offset: 0x0000D30E
		public void LookAt(UIntPtr cameraPointer, Vec3 position, Vec3 target, Vec3 upVector)
		{
			ScriptingInterfaceOfICamera.call_LookAtDelegate(cameraPointer, position, target, upVector);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000F11F File Offset: 0x0000D31F
		public void Release(UIntPtr cameraPointer)
		{
			ScriptingInterfaceOfICamera.call_ReleaseDelegate(cameraPointer);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000F12C File Offset: 0x0000D32C
		public void ReleaseCameraEntity(UIntPtr cameraPointer)
		{
			ScriptingInterfaceOfICamera.call_ReleaseCameraEntityDelegate(cameraPointer);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x0000F139 File Offset: 0x0000D339
		public void RenderFrustrum(UIntPtr cameraPointer)
		{
			ScriptingInterfaceOfICamera.call_RenderFrustrumDelegate(cameraPointer);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x0000F146 File Offset: 0x0000D346
		public void ScreenSpaceRayProjection(UIntPtr cameraPointer, Vec2 screenPosition, ref Vec3 rayBegin, ref Vec3 rayEnd)
		{
			ScriptingInterfaceOfICamera.call_ScreenSpaceRayProjectionDelegate(cameraPointer, screenPosition, ref rayBegin, ref rayEnd);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000F157 File Offset: 0x0000D357
		public void SetEntity(UIntPtr cameraPointer, UIntPtr entityId)
		{
			ScriptingInterfaceOfICamera.call_SetEntityDelegate(cameraPointer, entityId);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000F165 File Offset: 0x0000D365
		public void SetFovHorizontal(UIntPtr cameraPointer, float horizontalFov, float aspectRatio, float newDNear, float newDFar)
		{
			ScriptingInterfaceOfICamera.call_SetFovHorizontalDelegate(cameraPointer, horizontalFov, aspectRatio, newDNear, newDFar);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x0000F178 File Offset: 0x0000D378
		public void SetFovVertical(UIntPtr cameraPointer, float verticalFov, float aspectRatio, float newDNear, float newDFar)
		{
			ScriptingInterfaceOfICamera.call_SetFovVerticalDelegate(cameraPointer, verticalFov, aspectRatio, newDNear, newDFar);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000F18B File Offset: 0x0000D38B
		public void SetFrame(UIntPtr cameraPointer, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfICamera.call_SetFrameDelegate(cameraPointer, ref frame);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000F199 File Offset: 0x0000D399
		public void SetPosition(UIntPtr cameraPointer, Vec3 position)
		{
			ScriptingInterfaceOfICamera.call_SetPositionDelegate(cameraPointer, position);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000F1A8 File Offset: 0x0000D3A8
		public void SetViewVolume(UIntPtr cameraPointer, bool perspective, float dLeft, float dRight, float dBottom, float dTop, float dNear, float dFar)
		{
			ScriptingInterfaceOfICamera.call_SetViewVolumeDelegate(cameraPointer, perspective, dLeft, dRight, dBottom, dTop, dNear, dFar);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000F1CC File Offset: 0x0000D3CC
		public void ViewportPointToWorldRay(UIntPtr cameraPointer, ref Vec3 rayBegin, ref Vec3 rayEnd, Vec3 viewportPoint)
		{
			ScriptingInterfaceOfICamera.call_ViewportPointToWorldRayDelegate(cameraPointer, ref rayBegin, ref rayEnd, viewportPoint);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x0000F1DD File Offset: 0x0000D3DD
		public Vec3 WorldPointToViewportPoint(UIntPtr cameraPointer, ref Vec3 worldPoint)
		{
			return ScriptingInterfaceOfICamera.call_WorldPointToViewportPointDelegate(cameraPointer, ref worldPoint);
		}

		// Token: 0x04000008 RID: 8
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000009 RID: 9
		public static ScriptingInterfaceOfICamera.CheckEntityVisibilityDelegate call_CheckEntityVisibilityDelegate;

		// Token: 0x0400000A RID: 10
		public static ScriptingInterfaceOfICamera.ConstructCameraFromPositionElevationBearingDelegate call_ConstructCameraFromPositionElevationBearingDelegate;

		// Token: 0x0400000B RID: 11
		public static ScriptingInterfaceOfICamera.CreateCameraDelegate call_CreateCameraDelegate;

		// Token: 0x0400000C RID: 12
		public static ScriptingInterfaceOfICamera.EnclosesPointDelegate call_EnclosesPointDelegate;

		// Token: 0x0400000D RID: 13
		public static ScriptingInterfaceOfICamera.FillParametersFromDelegate call_FillParametersFromDelegate;

		// Token: 0x0400000E RID: 14
		public static ScriptingInterfaceOfICamera.GetAspectRatioDelegate call_GetAspectRatioDelegate;

		// Token: 0x0400000F RID: 15
		public static ScriptingInterfaceOfICamera.GetEntityDelegate call_GetEntityDelegate;

		// Token: 0x04000010 RID: 16
		public static ScriptingInterfaceOfICamera.GetFarDelegate call_GetFarDelegate;

		// Token: 0x04000011 RID: 17
		public static ScriptingInterfaceOfICamera.GetFovHorizontalDelegate call_GetFovHorizontalDelegate;

		// Token: 0x04000012 RID: 18
		public static ScriptingInterfaceOfICamera.GetFovVerticalDelegate call_GetFovVerticalDelegate;

		// Token: 0x04000013 RID: 19
		public static ScriptingInterfaceOfICamera.GetFrameDelegate call_GetFrameDelegate;

		// Token: 0x04000014 RID: 20
		public static ScriptingInterfaceOfICamera.GetHorizontalFovDelegate call_GetHorizontalFovDelegate;

		// Token: 0x04000015 RID: 21
		public static ScriptingInterfaceOfICamera.GetNearDelegate call_GetNearDelegate;

		// Token: 0x04000016 RID: 22
		public static ScriptingInterfaceOfICamera.GetNearPlanePointsDelegate call_GetNearPlanePointsDelegate;

		// Token: 0x04000017 RID: 23
		public static ScriptingInterfaceOfICamera.GetNearPlanePointsStaticDelegate call_GetNearPlanePointsStaticDelegate;

		// Token: 0x04000018 RID: 24
		public static ScriptingInterfaceOfICamera.GetViewProjMatrixDelegate call_GetViewProjMatrixDelegate;

		// Token: 0x04000019 RID: 25
		public static ScriptingInterfaceOfICamera.LookAtDelegate call_LookAtDelegate;

		// Token: 0x0400001A RID: 26
		public static ScriptingInterfaceOfICamera.ReleaseDelegate call_ReleaseDelegate;

		// Token: 0x0400001B RID: 27
		public static ScriptingInterfaceOfICamera.ReleaseCameraEntityDelegate call_ReleaseCameraEntityDelegate;

		// Token: 0x0400001C RID: 28
		public static ScriptingInterfaceOfICamera.RenderFrustrumDelegate call_RenderFrustrumDelegate;

		// Token: 0x0400001D RID: 29
		public static ScriptingInterfaceOfICamera.ScreenSpaceRayProjectionDelegate call_ScreenSpaceRayProjectionDelegate;

		// Token: 0x0400001E RID: 30
		public static ScriptingInterfaceOfICamera.SetEntityDelegate call_SetEntityDelegate;

		// Token: 0x0400001F RID: 31
		public static ScriptingInterfaceOfICamera.SetFovHorizontalDelegate call_SetFovHorizontalDelegate;

		// Token: 0x04000020 RID: 32
		public static ScriptingInterfaceOfICamera.SetFovVerticalDelegate call_SetFovVerticalDelegate;

		// Token: 0x04000021 RID: 33
		public static ScriptingInterfaceOfICamera.SetFrameDelegate call_SetFrameDelegate;

		// Token: 0x04000022 RID: 34
		public static ScriptingInterfaceOfICamera.SetPositionDelegate call_SetPositionDelegate;

		// Token: 0x04000023 RID: 35
		public static ScriptingInterfaceOfICamera.SetViewVolumeDelegate call_SetViewVolumeDelegate;

		// Token: 0x04000024 RID: 36
		public static ScriptingInterfaceOfICamera.ViewportPointToWorldRayDelegate call_ViewportPointToWorldRayDelegate;

		// Token: 0x04000025 RID: 37
		public static ScriptingInterfaceOfICamera.WorldPointToViewportPointDelegate call_WorldPointToViewportPointDelegate;

		// Token: 0x0200008E RID: 142
		// (Invoke) Token: 0x06000863 RID: 2147
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool CheckEntityVisibilityDelegate(UIntPtr cameraPointer, UIntPtr entityPointer);

		// Token: 0x0200008F RID: 143
		// (Invoke) Token: 0x06000867 RID: 2151
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ConstructCameraFromPositionElevationBearingDelegate(Vec3 position, float elevation, float bearing, ref MatrixFrame outFrame);

		// Token: 0x02000090 RID: 144
		// (Invoke) Token: 0x0600086B RID: 2155
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateCameraDelegate();

		// Token: 0x02000091 RID: 145
		// (Invoke) Token: 0x0600086F RID: 2159
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool EnclosesPointDelegate(UIntPtr cameraPointer, Vec3 pointInWorldSpace);

		// Token: 0x02000092 RID: 146
		// (Invoke) Token: 0x06000873 RID: 2163
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FillParametersFromDelegate(UIntPtr cameraPointer, UIntPtr otherCameraPointer);

		// Token: 0x02000093 RID: 147
		// (Invoke) Token: 0x06000877 RID: 2167
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAspectRatioDelegate(UIntPtr cameraPointer);

		// Token: 0x02000094 RID: 148
		// (Invoke) Token: 0x0600087B RID: 2171
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetEntityDelegate(UIntPtr cameraPointer);

		// Token: 0x02000095 RID: 149
		// (Invoke) Token: 0x0600087F RID: 2175
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetFarDelegate(UIntPtr cameraPointer);

		// Token: 0x02000096 RID: 150
		// (Invoke) Token: 0x06000883 RID: 2179
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetFovHorizontalDelegate(UIntPtr cameraPointer);

		// Token: 0x02000097 RID: 151
		// (Invoke) Token: 0x06000887 RID: 2183
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetFovVerticalDelegate(UIntPtr cameraPointer);

		// Token: 0x02000098 RID: 152
		// (Invoke) Token: 0x0600088B RID: 2187
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetFrameDelegate(UIntPtr cameraPointer, ref MatrixFrame outFrame);

		// Token: 0x02000099 RID: 153
		// (Invoke) Token: 0x0600088F RID: 2191
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetHorizontalFovDelegate(UIntPtr cameraPointer);

		// Token: 0x0200009A RID: 154
		// (Invoke) Token: 0x06000893 RID: 2195
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetNearDelegate(UIntPtr cameraPointer);

		// Token: 0x0200009B RID: 155
		// (Invoke) Token: 0x06000897 RID: 2199
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetNearPlanePointsDelegate(UIntPtr cameraPointer, IntPtr nearPlanePoints);

		// Token: 0x0200009C RID: 156
		// (Invoke) Token: 0x0600089B RID: 2203
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetNearPlanePointsStaticDelegate(ref MatrixFrame cameraFrame, float verticalFov, float aspectRatioXY, float newDNear, float newDFar, IntPtr nearPlanePoints);

		// Token: 0x0200009D RID: 157
		// (Invoke) Token: 0x0600089F RID: 2207
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetViewProjMatrixDelegate(UIntPtr cameraPointer, ref MatrixFrame frame);

		// Token: 0x0200009E RID: 158
		// (Invoke) Token: 0x060008A3 RID: 2211
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void LookAtDelegate(UIntPtr cameraPointer, Vec3 position, Vec3 target, Vec3 upVector);

		// Token: 0x0200009F RID: 159
		// (Invoke) Token: 0x060008A7 RID: 2215
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseDelegate(UIntPtr cameraPointer);

		// Token: 0x020000A0 RID: 160
		// (Invoke) Token: 0x060008AB RID: 2219
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseCameraEntityDelegate(UIntPtr cameraPointer);

		// Token: 0x020000A1 RID: 161
		// (Invoke) Token: 0x060008AF RID: 2223
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RenderFrustrumDelegate(UIntPtr cameraPointer);

		// Token: 0x020000A2 RID: 162
		// (Invoke) Token: 0x060008B3 RID: 2227
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ScreenSpaceRayProjectionDelegate(UIntPtr cameraPointer, Vec2 screenPosition, ref Vec3 rayBegin, ref Vec3 rayEnd);

		// Token: 0x020000A3 RID: 163
		// (Invoke) Token: 0x060008B7 RID: 2231
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEntityDelegate(UIntPtr cameraPointer, UIntPtr entityId);

		// Token: 0x020000A4 RID: 164
		// (Invoke) Token: 0x060008BB RID: 2235
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFovHorizontalDelegate(UIntPtr cameraPointer, float horizontalFov, float aspectRatio, float newDNear, float newDFar);

		// Token: 0x020000A5 RID: 165
		// (Invoke) Token: 0x060008BF RID: 2239
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFovVerticalDelegate(UIntPtr cameraPointer, float verticalFov, float aspectRatio, float newDNear, float newDFar);

		// Token: 0x020000A6 RID: 166
		// (Invoke) Token: 0x060008C3 RID: 2243
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFrameDelegate(UIntPtr cameraPointer, ref MatrixFrame frame);

		// Token: 0x020000A7 RID: 167
		// (Invoke) Token: 0x060008C7 RID: 2247
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetPositionDelegate(UIntPtr cameraPointer, Vec3 position);

		// Token: 0x020000A8 RID: 168
		// (Invoke) Token: 0x060008CB RID: 2251
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetViewVolumeDelegate(UIntPtr cameraPointer, [MarshalAs(UnmanagedType.U1)] bool perspective, float dLeft, float dRight, float dBottom, float dTop, float dNear, float dFar);

		// Token: 0x020000A9 RID: 169
		// (Invoke) Token: 0x060008CF RID: 2255
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ViewportPointToWorldRayDelegate(UIntPtr cameraPointer, ref Vec3 rayBegin, ref Vec3 rayEnd, Vec3 viewportPoint);

		// Token: 0x020000AA RID: 170
		// (Invoke) Token: 0x060008D3 RID: 2259
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 WorldPointToViewportPointDelegate(UIntPtr cameraPointer, ref Vec3 worldPoint);
	}
}
