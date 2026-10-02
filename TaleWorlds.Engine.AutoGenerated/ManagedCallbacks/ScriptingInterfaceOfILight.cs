using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x02000016 RID: 22
	internal class ScriptingInterfaceOfILight : ILight
	{
		// Token: 0x060002A5 RID: 677 RVA: 0x0001320C File Offset: 0x0001140C
		public Light CreatePointLight(float lightRadius)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfILight.call_CreatePointLightDelegate(lightRadius);
			Light light = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				light = new Light(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return light;
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00013256 File Offset: 0x00011456
		public void EnableShadow(UIntPtr lightpointer, bool shadowEnabled)
		{
			ScriptingInterfaceOfILight.call_EnableShadowDelegate(lightpointer, shadowEnabled);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00013264 File Offset: 0x00011464
		public void GetFrame(UIntPtr lightPointer, out MatrixFrame result)
		{
			ScriptingInterfaceOfILight.call_GetFrameDelegate(lightPointer, out result);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00013272 File Offset: 0x00011472
		public float GetIntensity(UIntPtr lightPointer)
		{
			return ScriptingInterfaceOfILight.call_GetIntensityDelegate(lightPointer);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0001327F File Offset: 0x0001147F
		public Vec3 GetLightColor(UIntPtr lightpointer)
		{
			return ScriptingInterfaceOfILight.call_GetLightColorDelegate(lightpointer);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0001328C File Offset: 0x0001148C
		public float GetRadius(UIntPtr lightpointer)
		{
			return ScriptingInterfaceOfILight.call_GetRadiusDelegate(lightpointer);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00013299 File Offset: 0x00011499
		public bool IsShadowEnabled(UIntPtr lightpointer)
		{
			return ScriptingInterfaceOfILight.call_IsShadowEnabledDelegate(lightpointer);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x000132A6 File Offset: 0x000114A6
		public void Release(UIntPtr lightpointer)
		{
			ScriptingInterfaceOfILight.call_ReleaseDelegate(lightpointer);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x000132B3 File Offset: 0x000114B3
		public void SetFrame(UIntPtr lightPointer, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfILight.call_SetFrameDelegate(lightPointer, ref frame);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000132C1 File Offset: 0x000114C1
		public void SetIntensity(UIntPtr lightPointer, float value)
		{
			ScriptingInterfaceOfILight.call_SetIntensityDelegate(lightPointer, value);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x000132CF File Offset: 0x000114CF
		public void SetLightColor(UIntPtr lightpointer, Vec3 color)
		{
			ScriptingInterfaceOfILight.call_SetLightColorDelegate(lightpointer, color);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x000132DD File Offset: 0x000114DD
		public void SetLightFlicker(UIntPtr lightpointer, float magnitude, float interval)
		{
			ScriptingInterfaceOfILight.call_SetLightFlickerDelegate(lightpointer, magnitude, interval);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x000132EC File Offset: 0x000114EC
		public void SetRadius(UIntPtr lightpointer, float radius)
		{
			ScriptingInterfaceOfILight.call_SetRadiusDelegate(lightpointer, radius);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x000132FA File Offset: 0x000114FA
		public void SetShadows(UIntPtr lightPointer, int shadowType)
		{
			ScriptingInterfaceOfILight.call_SetShadowsDelegate(lightPointer, shadowType);
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x00013308 File Offset: 0x00011508
		public void SetVisibility(UIntPtr lightpointer, bool value)
		{
			ScriptingInterfaceOfILight.call_SetVisibilityDelegate(lightpointer, value);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x00013316 File Offset: 0x00011516
		public void SetVolumetricProperties(UIntPtr lightpointer, bool volumelightenabled, float volumeparameter)
		{
			ScriptingInterfaceOfILight.call_SetVolumetricPropertiesDelegate(lightpointer, volumelightenabled, volumeparameter);
		}

		// Token: 0x0400021E RID: 542
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400021F RID: 543
		public static ScriptingInterfaceOfILight.CreatePointLightDelegate call_CreatePointLightDelegate;

		// Token: 0x04000220 RID: 544
		public static ScriptingInterfaceOfILight.EnableShadowDelegate call_EnableShadowDelegate;

		// Token: 0x04000221 RID: 545
		public static ScriptingInterfaceOfILight.GetFrameDelegate call_GetFrameDelegate;

		// Token: 0x04000222 RID: 546
		public static ScriptingInterfaceOfILight.GetIntensityDelegate call_GetIntensityDelegate;

		// Token: 0x04000223 RID: 547
		public static ScriptingInterfaceOfILight.GetLightColorDelegate call_GetLightColorDelegate;

		// Token: 0x04000224 RID: 548
		public static ScriptingInterfaceOfILight.GetRadiusDelegate call_GetRadiusDelegate;

		// Token: 0x04000225 RID: 549
		public static ScriptingInterfaceOfILight.IsShadowEnabledDelegate call_IsShadowEnabledDelegate;

		// Token: 0x04000226 RID: 550
		public static ScriptingInterfaceOfILight.ReleaseDelegate call_ReleaseDelegate;

		// Token: 0x04000227 RID: 551
		public static ScriptingInterfaceOfILight.SetFrameDelegate call_SetFrameDelegate;

		// Token: 0x04000228 RID: 552
		public static ScriptingInterfaceOfILight.SetIntensityDelegate call_SetIntensityDelegate;

		// Token: 0x04000229 RID: 553
		public static ScriptingInterfaceOfILight.SetLightColorDelegate call_SetLightColorDelegate;

		// Token: 0x0400022A RID: 554
		public static ScriptingInterfaceOfILight.SetLightFlickerDelegate call_SetLightFlickerDelegate;

		// Token: 0x0400022B RID: 555
		public static ScriptingInterfaceOfILight.SetRadiusDelegate call_SetRadiusDelegate;

		// Token: 0x0400022C RID: 556
		public static ScriptingInterfaceOfILight.SetShadowsDelegate call_SetShadowsDelegate;

		// Token: 0x0400022D RID: 557
		public static ScriptingInterfaceOfILight.SetVisibilityDelegate call_SetVisibilityDelegate;

		// Token: 0x0400022E RID: 558
		public static ScriptingInterfaceOfILight.SetVolumetricPropertiesDelegate call_SetVolumetricPropertiesDelegate;

		// Token: 0x02000298 RID: 664
		// (Invoke) Token: 0x0600108B RID: 4235
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreatePointLightDelegate(float lightRadius);

		// Token: 0x02000299 RID: 665
		// (Invoke) Token: 0x0600108F RID: 4239
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EnableShadowDelegate(UIntPtr lightpointer, [MarshalAs(UnmanagedType.U1)] bool shadowEnabled);

		// Token: 0x0200029A RID: 666
		// (Invoke) Token: 0x06001093 RID: 4243
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetFrameDelegate(UIntPtr lightPointer, out MatrixFrame result);

		// Token: 0x0200029B RID: 667
		// (Invoke) Token: 0x06001097 RID: 4247
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetIntensityDelegate(UIntPtr lightPointer);

		// Token: 0x0200029C RID: 668
		// (Invoke) Token: 0x0600109B RID: 4251
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetLightColorDelegate(UIntPtr lightpointer);

		// Token: 0x0200029D RID: 669
		// (Invoke) Token: 0x0600109F RID: 4255
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetRadiusDelegate(UIntPtr lightpointer);

		// Token: 0x0200029E RID: 670
		// (Invoke) Token: 0x060010A3 RID: 4259
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsShadowEnabledDelegate(UIntPtr lightpointer);

		// Token: 0x0200029F RID: 671
		// (Invoke) Token: 0x060010A7 RID: 4263
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseDelegate(UIntPtr lightpointer);

		// Token: 0x020002A0 RID: 672
		// (Invoke) Token: 0x060010AB RID: 4267
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFrameDelegate(UIntPtr lightPointer, ref MatrixFrame frame);

		// Token: 0x020002A1 RID: 673
		// (Invoke) Token: 0x060010AF RID: 4271
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetIntensityDelegate(UIntPtr lightPointer, float value);

		// Token: 0x020002A2 RID: 674
		// (Invoke) Token: 0x060010B3 RID: 4275
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetLightColorDelegate(UIntPtr lightpointer, Vec3 color);

		// Token: 0x020002A3 RID: 675
		// (Invoke) Token: 0x060010B7 RID: 4279
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetLightFlickerDelegate(UIntPtr lightpointer, float magnitude, float interval);

		// Token: 0x020002A4 RID: 676
		// (Invoke) Token: 0x060010BB RID: 4283
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRadiusDelegate(UIntPtr lightpointer, float radius);

		// Token: 0x020002A5 RID: 677
		// (Invoke) Token: 0x060010BF RID: 4287
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetShadowsDelegate(UIntPtr lightPointer, int shadowType);

		// Token: 0x020002A6 RID: 678
		// (Invoke) Token: 0x060010C3 RID: 4291
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVisibilityDelegate(UIntPtr lightpointer, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x020002A7 RID: 679
		// (Invoke) Token: 0x060010C7 RID: 4295
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVolumetricPropertiesDelegate(UIntPtr lightpointer, [MarshalAs(UnmanagedType.U1)] bool volumelightenabled, float volumeparameter);
	}
}
