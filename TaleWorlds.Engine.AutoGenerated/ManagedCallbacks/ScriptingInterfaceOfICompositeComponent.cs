using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x0200000C RID: 12
	internal class ScriptingInterfaceOfICompositeComponent : ICompositeComponent
	{
		// Token: 0x0600009F RID: 159 RVA: 0x0000F347 File Offset: 0x0000D547
		public void AddComponent(UIntPtr pointer, UIntPtr componentPointer)
		{
			ScriptingInterfaceOfICompositeComponent.call_AddComponentDelegate(pointer, componentPointer);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000F358 File Offset: 0x0000D558
		public void AddMultiMesh(UIntPtr compositeComponentPointer, string multiMeshName)
		{
			byte[] array = null;
			if (multiMeshName != null)
			{
				int byteCount = ScriptingInterfaceOfICompositeComponent._utf8.GetByteCount(multiMeshName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfICompositeComponent._utf8.GetBytes(multiMeshName, 0, multiMeshName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfICompositeComponent.call_AddMultiMeshDelegate(compositeComponentPointer, array);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x0000F3B4 File Offset: 0x0000D5B4
		public void AddPrefabEntity(UIntPtr pointer, UIntPtr scenePointer, string prefabName)
		{
			byte[] array = null;
			if (prefabName != null)
			{
				int byteCount = ScriptingInterfaceOfICompositeComponent._utf8.GetByteCount(prefabName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfICompositeComponent._utf8.GetBytes(prefabName, 0, prefabName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfICompositeComponent.call_AddPrefabEntityDelegate(pointer, scenePointer, array);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x0000F410 File Offset: 0x0000D610
		public CompositeComponent CreateCompositeComponent()
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfICompositeComponent.call_CreateCompositeComponentDelegate();
			CompositeComponent compositeComponent = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				compositeComponent = new CompositeComponent(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return compositeComponent;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000F45C File Offset: 0x0000D65C
		public CompositeComponent CreateCopy(UIntPtr pointer)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfICompositeComponent.call_CreateCopyDelegate(pointer);
			CompositeComponent compositeComponent = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				compositeComponent = new CompositeComponent(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return compositeComponent;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000F4A6 File Offset: 0x0000D6A6
		public void GetBoundingBox(UIntPtr compositeComponentPointer, ref BoundingBox outBoundingBox)
		{
			ScriptingInterfaceOfICompositeComponent.call_GetBoundingBoxDelegate(compositeComponentPointer, ref outBoundingBox);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000F4B4 File Offset: 0x0000D6B4
		public uint GetFactor1(UIntPtr compositeComponentPointer)
		{
			return ScriptingInterfaceOfICompositeComponent.call_GetFactor1Delegate(compositeComponentPointer);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x0000F4C1 File Offset: 0x0000D6C1
		public uint GetFactor2(UIntPtr compositeComponentPointer)
		{
			return ScriptingInterfaceOfICompositeComponent.call_GetFactor2Delegate(compositeComponentPointer);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x0000F4D0 File Offset: 0x0000D6D0
		public MetaMesh GetFirstMetaMesh(UIntPtr compositeComponentPointer)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfICompositeComponent.call_GetFirstMetaMeshDelegate(compositeComponentPointer);
			MetaMesh metaMesh = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				metaMesh = new MetaMesh(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return metaMesh;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x0000F51A File Offset: 0x0000D71A
		public void GetFrame(UIntPtr compositeComponentPointer, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfICompositeComponent.call_GetFrameDelegate(compositeComponentPointer, ref outFrame);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x0000F528 File Offset: 0x0000D728
		public Vec3 GetVectorUserData(UIntPtr compositeComponentPointer)
		{
			return ScriptingInterfaceOfICompositeComponent.call_GetVectorUserDataDelegate(compositeComponentPointer);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x0000F535 File Offset: 0x0000D735
		public bool IsVisible(UIntPtr compositeComponentPointer)
		{
			return ScriptingInterfaceOfICompositeComponent.call_IsVisibleDelegate(compositeComponentPointer);
		}

		// Token: 0x060000AB RID: 171 RVA: 0x0000F542 File Offset: 0x0000D742
		public void Release(UIntPtr compositeComponentPointer)
		{
			ScriptingInterfaceOfICompositeComponent.call_ReleaseDelegate(compositeComponentPointer);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x0000F54F File Offset: 0x0000D74F
		public void SetFactor1(UIntPtr compositeComponentPointer, uint factorColor1)
		{
			ScriptingInterfaceOfICompositeComponent.call_SetFactor1Delegate(compositeComponentPointer, factorColor1);
		}

		// Token: 0x060000AD RID: 173 RVA: 0x0000F55D File Offset: 0x0000D75D
		public void SetFactor2(UIntPtr compositeComponentPointer, uint factorColor2)
		{
			ScriptingInterfaceOfICompositeComponent.call_SetFactor2Delegate(compositeComponentPointer, factorColor2);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000F56B File Offset: 0x0000D76B
		public void SetFrame(UIntPtr compositeComponentPointer, ref MatrixFrame meshFrame)
		{
			ScriptingInterfaceOfICompositeComponent.call_SetFrameDelegate(compositeComponentPointer, ref meshFrame);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000F579 File Offset: 0x0000D779
		public void SetMaterial(UIntPtr compositeComponentPointer, UIntPtr materialPointer)
		{
			ScriptingInterfaceOfICompositeComponent.call_SetMaterialDelegate(compositeComponentPointer, materialPointer);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000F587 File Offset: 0x0000D787
		public void SetVectorArgument(UIntPtr compositeComponentPointer, float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			ScriptingInterfaceOfICompositeComponent.call_SetVectorArgumentDelegate(compositeComponentPointer, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000F59A File Offset: 0x0000D79A
		public void SetVectorUserData(UIntPtr compositeComponentPointer, ref Vec3 vectorArg)
		{
			ScriptingInterfaceOfICompositeComponent.call_SetVectorUserDataDelegate(compositeComponentPointer, ref vectorArg);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x0000F5A8 File Offset: 0x0000D7A8
		public void SetVisibilityMask(UIntPtr compositeComponentPointer, VisibilityMaskFlags visibilityMask)
		{
			ScriptingInterfaceOfICompositeComponent.call_SetVisibilityMaskDelegate(compositeComponentPointer, visibilityMask);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000F5B6 File Offset: 0x0000D7B6
		public void SetVisible(UIntPtr compositeComponentPointer, bool visible)
		{
			ScriptingInterfaceOfICompositeComponent.call_SetVisibleDelegate(compositeComponentPointer, visible);
		}

		// Token: 0x04000034 RID: 52
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000035 RID: 53
		public static ScriptingInterfaceOfICompositeComponent.AddComponentDelegate call_AddComponentDelegate;

		// Token: 0x04000036 RID: 54
		public static ScriptingInterfaceOfICompositeComponent.AddMultiMeshDelegate call_AddMultiMeshDelegate;

		// Token: 0x04000037 RID: 55
		public static ScriptingInterfaceOfICompositeComponent.AddPrefabEntityDelegate call_AddPrefabEntityDelegate;

		// Token: 0x04000038 RID: 56
		public static ScriptingInterfaceOfICompositeComponent.CreateCompositeComponentDelegate call_CreateCompositeComponentDelegate;

		// Token: 0x04000039 RID: 57
		public static ScriptingInterfaceOfICompositeComponent.CreateCopyDelegate call_CreateCopyDelegate;

		// Token: 0x0400003A RID: 58
		public static ScriptingInterfaceOfICompositeComponent.GetBoundingBoxDelegate call_GetBoundingBoxDelegate;

		// Token: 0x0400003B RID: 59
		public static ScriptingInterfaceOfICompositeComponent.GetFactor1Delegate call_GetFactor1Delegate;

		// Token: 0x0400003C RID: 60
		public static ScriptingInterfaceOfICompositeComponent.GetFactor2Delegate call_GetFactor2Delegate;

		// Token: 0x0400003D RID: 61
		public static ScriptingInterfaceOfICompositeComponent.GetFirstMetaMeshDelegate call_GetFirstMetaMeshDelegate;

		// Token: 0x0400003E RID: 62
		public static ScriptingInterfaceOfICompositeComponent.GetFrameDelegate call_GetFrameDelegate;

		// Token: 0x0400003F RID: 63
		public static ScriptingInterfaceOfICompositeComponent.GetVectorUserDataDelegate call_GetVectorUserDataDelegate;

		// Token: 0x04000040 RID: 64
		public static ScriptingInterfaceOfICompositeComponent.IsVisibleDelegate call_IsVisibleDelegate;

		// Token: 0x04000041 RID: 65
		public static ScriptingInterfaceOfICompositeComponent.ReleaseDelegate call_ReleaseDelegate;

		// Token: 0x04000042 RID: 66
		public static ScriptingInterfaceOfICompositeComponent.SetFactor1Delegate call_SetFactor1Delegate;

		// Token: 0x04000043 RID: 67
		public static ScriptingInterfaceOfICompositeComponent.SetFactor2Delegate call_SetFactor2Delegate;

		// Token: 0x04000044 RID: 68
		public static ScriptingInterfaceOfICompositeComponent.SetFrameDelegate call_SetFrameDelegate;

		// Token: 0x04000045 RID: 69
		public static ScriptingInterfaceOfICompositeComponent.SetMaterialDelegate call_SetMaterialDelegate;

		// Token: 0x04000046 RID: 70
		public static ScriptingInterfaceOfICompositeComponent.SetVectorArgumentDelegate call_SetVectorArgumentDelegate;

		// Token: 0x04000047 RID: 71
		public static ScriptingInterfaceOfICompositeComponent.SetVectorUserDataDelegate call_SetVectorUserDataDelegate;

		// Token: 0x04000048 RID: 72
		public static ScriptingInterfaceOfICompositeComponent.SetVisibilityMaskDelegate call_SetVisibilityMaskDelegate;

		// Token: 0x04000049 RID: 73
		public static ScriptingInterfaceOfICompositeComponent.SetVisibleDelegate call_SetVisibleDelegate;

		// Token: 0x020000B8 RID: 184
		// (Invoke) Token: 0x0600090B RID: 2315
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddComponentDelegate(UIntPtr pointer, UIntPtr componentPointer);

		// Token: 0x020000B9 RID: 185
		// (Invoke) Token: 0x0600090F RID: 2319
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMultiMeshDelegate(UIntPtr compositeComponentPointer, byte[] multiMeshName);

		// Token: 0x020000BA RID: 186
		// (Invoke) Token: 0x06000913 RID: 2323
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddPrefabEntityDelegate(UIntPtr pointer, UIntPtr scenePointer, byte[] prefabName);

		// Token: 0x020000BB RID: 187
		// (Invoke) Token: 0x06000917 RID: 2327
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateCompositeComponentDelegate();

		// Token: 0x020000BC RID: 188
		// (Invoke) Token: 0x0600091B RID: 2331
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateCopyDelegate(UIntPtr pointer);

		// Token: 0x020000BD RID: 189
		// (Invoke) Token: 0x0600091F RID: 2335
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoundingBoxDelegate(UIntPtr compositeComponentPointer, ref BoundingBox outBoundingBox);

		// Token: 0x020000BE RID: 190
		// (Invoke) Token: 0x06000923 RID: 2339
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate uint GetFactor1Delegate(UIntPtr compositeComponentPointer);

		// Token: 0x020000BF RID: 191
		// (Invoke) Token: 0x06000927 RID: 2343
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate uint GetFactor2Delegate(UIntPtr compositeComponentPointer);

		// Token: 0x020000C0 RID: 192
		// (Invoke) Token: 0x0600092B RID: 2347
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetFirstMetaMeshDelegate(UIntPtr compositeComponentPointer);

		// Token: 0x020000C1 RID: 193
		// (Invoke) Token: 0x0600092F RID: 2351
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetFrameDelegate(UIntPtr compositeComponentPointer, ref MatrixFrame outFrame);

		// Token: 0x020000C2 RID: 194
		// (Invoke) Token: 0x06000933 RID: 2355
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetVectorUserDataDelegate(UIntPtr compositeComponentPointer);

		// Token: 0x020000C3 RID: 195
		// (Invoke) Token: 0x06000937 RID: 2359
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsVisibleDelegate(UIntPtr compositeComponentPointer);

		// Token: 0x020000C4 RID: 196
		// (Invoke) Token: 0x0600093B RID: 2363
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseDelegate(UIntPtr compositeComponentPointer);

		// Token: 0x020000C5 RID: 197
		// (Invoke) Token: 0x0600093F RID: 2367
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFactor1Delegate(UIntPtr compositeComponentPointer, uint factorColor1);

		// Token: 0x020000C6 RID: 198
		// (Invoke) Token: 0x06000943 RID: 2371
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFactor2Delegate(UIntPtr compositeComponentPointer, uint factorColor2);

		// Token: 0x020000C7 RID: 199
		// (Invoke) Token: 0x06000947 RID: 2375
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFrameDelegate(UIntPtr compositeComponentPointer, ref MatrixFrame meshFrame);

		// Token: 0x020000C8 RID: 200
		// (Invoke) Token: 0x0600094B RID: 2379
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMaterialDelegate(UIntPtr compositeComponentPointer, UIntPtr materialPointer);

		// Token: 0x020000C9 RID: 201
		// (Invoke) Token: 0x0600094F RID: 2383
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVectorArgumentDelegate(UIntPtr compositeComponentPointer, float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3);

		// Token: 0x020000CA RID: 202
		// (Invoke) Token: 0x06000953 RID: 2387
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVectorUserDataDelegate(UIntPtr compositeComponentPointer, ref Vec3 vectorArg);

		// Token: 0x020000CB RID: 203
		// (Invoke) Token: 0x06000957 RID: 2391
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVisibilityMaskDelegate(UIntPtr compositeComponentPointer, VisibilityMaskFlags visibilityMask);

		// Token: 0x020000CC RID: 204
		// (Invoke) Token: 0x0600095B RID: 2395
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetVisibleDelegate(UIntPtr compositeComponentPointer, [MarshalAs(UnmanagedType.U1)] bool visible);
	}
}
