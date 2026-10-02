using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x02000018 RID: 24
	internal class ScriptingInterfaceOfIMaterial : IMaterial
	{
		// Token: 0x060002F2 RID: 754 RVA: 0x00013760 File Offset: 0x00011960
		public void AddMaterialShaderFlag(UIntPtr materialPointer, string flagName, bool showErrors)
		{
			byte[] array = null;
			if (flagName != null)
			{
				int byteCount = ScriptingInterfaceOfIMaterial._utf8.GetByteCount(flagName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMaterial._utf8.GetBytes(flagName, 0, flagName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMaterial.call_AddMaterialShaderFlagDelegate(materialPointer, array, showErrors);
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x000137BC File Offset: 0x000119BC
		public Material CreateCopy(UIntPtr materialPointer)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMaterial.call_CreateCopyDelegate(materialPointer);
			Material material = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				material = new Material(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return material;
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00013806 File Offset: 0x00011A06
		public int GetAlphaBlendMode(UIntPtr materialPointer)
		{
			return ScriptingInterfaceOfIMaterial.call_GetAlphaBlendModeDelegate(materialPointer);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00013813 File Offset: 0x00011A13
		public float GetAlphaTestValue(UIntPtr materialPointer)
		{
			return ScriptingInterfaceOfIMaterial.call_GetAlphaTestValueDelegate(materialPointer);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00013820 File Offset: 0x00011A20
		public Material GetDefaultMaterial()
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMaterial.call_GetDefaultMaterialDelegate();
			Material material = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				material = new Material(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return material;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00013869 File Offset: 0x00011A69
		public MaterialFlags GetFlags(UIntPtr materialPointer)
		{
			return ScriptingInterfaceOfIMaterial.call_GetFlagsDelegate(materialPointer);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00013878 File Offset: 0x00011A78
		public Material GetFromResource(string materialName)
		{
			byte[] array = null;
			if (materialName != null)
			{
				int byteCount = ScriptingInterfaceOfIMaterial._utf8.GetByteCount(materialName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMaterial._utf8.GetBytes(materialName, 0, materialName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMaterial.call_GetFromResourceDelegate(array);
			Material material = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				material = new Material(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return material;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x00013904 File Offset: 0x00011B04
		public string GetName(UIntPtr materialPointer)
		{
			if (ScriptingInterfaceOfIMaterial.call_GetNameDelegate(materialPointer) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0001391C File Offset: 0x00011B1C
		public Material GetOutlineMaterial(UIntPtr materialPointer)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMaterial.call_GetOutlineMaterialDelegate(materialPointer);
			Material material = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				material = new Material(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return material;
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00013968 File Offset: 0x00011B68
		public Shader GetShader(UIntPtr materialPointer)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMaterial.call_GetShaderDelegate(materialPointer);
			Shader shader = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				shader = new Shader(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return shader;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x000139B2 File Offset: 0x00011BB2
		public ulong GetShaderFlags(UIntPtr materialPointer)
		{
			return ScriptingInterfaceOfIMaterial.call_GetShaderFlagsDelegate(materialPointer);
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000139C0 File Offset: 0x00011BC0
		public Texture GetTexture(UIntPtr materialPointer, int textureType)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMaterial.call_GetTextureDelegate(materialPointer, textureType);
			Texture texture = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				texture = new Texture(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return texture;
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00013A0B File Offset: 0x00011C0B
		public void Release(UIntPtr materialPointer)
		{
			ScriptingInterfaceOfIMaterial.call_ReleaseDelegate(materialPointer);
		}

		// Token: 0x060002FF RID: 767 RVA: 0x00013A18 File Offset: 0x00011C18
		public void RemoveMaterialShaderFlag(UIntPtr materialPointer, string flagName)
		{
			byte[] array = null;
			if (flagName != null)
			{
				int byteCount = ScriptingInterfaceOfIMaterial._utf8.GetByteCount(flagName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMaterial._utf8.GetBytes(flagName, 0, flagName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMaterial.call_RemoveMaterialShaderFlagDelegate(materialPointer, array);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x00013A73 File Offset: 0x00011C73
		public void SetAlphaBlendMode(UIntPtr materialPointer, int alphaBlendMode)
		{
			ScriptingInterfaceOfIMaterial.call_SetAlphaBlendModeDelegate(materialPointer, alphaBlendMode);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00013A81 File Offset: 0x00011C81
		public void SetAlphaTestValue(UIntPtr materialPointer, float alphaTestValue)
		{
			ScriptingInterfaceOfIMaterial.call_SetAlphaTestValueDelegate(materialPointer, alphaTestValue);
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00013A8F File Offset: 0x00011C8F
		public void SetAreaMapScale(UIntPtr materialPointer, float scale)
		{
			ScriptingInterfaceOfIMaterial.call_SetAreaMapScaleDelegate(materialPointer, scale);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00013A9D File Offset: 0x00011C9D
		public void SetEnableSkinning(UIntPtr materialPointer, bool enable)
		{
			ScriptingInterfaceOfIMaterial.call_SetEnableSkinningDelegate(materialPointer, enable);
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00013AAB File Offset: 0x00011CAB
		public void SetFlags(UIntPtr materialPointer, MaterialFlags flags)
		{
			ScriptingInterfaceOfIMaterial.call_SetFlagsDelegate(materialPointer, flags);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00013AB9 File Offset: 0x00011CB9
		public void SetMeshVectorArgument(UIntPtr materialPointer, float x, float y, float z, float w)
		{
			ScriptingInterfaceOfIMaterial.call_SetMeshVectorArgumentDelegate(materialPointer, x, y, z, w);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00013ACC File Offset: 0x00011CCC
		public void SetName(UIntPtr materialPointer, string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIMaterial._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMaterial._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMaterial.call_SetNameDelegate(materialPointer, array);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00013B27 File Offset: 0x00011D27
		public void SetShader(UIntPtr materialPointer, UIntPtr shaderPointer)
		{
			ScriptingInterfaceOfIMaterial.call_SetShaderDelegate(materialPointer, shaderPointer);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00013B35 File Offset: 0x00011D35
		public void SetShaderFlags(UIntPtr materialPointer, ulong shaderFlags)
		{
			ScriptingInterfaceOfIMaterial.call_SetShaderFlagsDelegate(materialPointer, shaderFlags);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00013B43 File Offset: 0x00011D43
		public void SetTexture(UIntPtr materialPointer, int textureType, UIntPtr texturePointer)
		{
			ScriptingInterfaceOfIMaterial.call_SetTextureDelegate(materialPointer, textureType, texturePointer);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00013B52 File Offset: 0x00011D52
		public void SetTextureAtSlot(UIntPtr materialPointer, int textureSlotIndex, UIntPtr texturePointer)
		{
			ScriptingInterfaceOfIMaterial.call_SetTextureAtSlotDelegate(materialPointer, textureSlotIndex, texturePointer);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00013B61 File Offset: 0x00011D61
		public bool UsingSkinning(UIntPtr materialPointer)
		{
			return ScriptingInterfaceOfIMaterial.call_UsingSkinningDelegate(materialPointer);
		}

		// Token: 0x04000269 RID: 617
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400026A RID: 618
		public static ScriptingInterfaceOfIMaterial.AddMaterialShaderFlagDelegate call_AddMaterialShaderFlagDelegate;

		// Token: 0x0400026B RID: 619
		public static ScriptingInterfaceOfIMaterial.CreateCopyDelegate call_CreateCopyDelegate;

		// Token: 0x0400026C RID: 620
		public static ScriptingInterfaceOfIMaterial.GetAlphaBlendModeDelegate call_GetAlphaBlendModeDelegate;

		// Token: 0x0400026D RID: 621
		public static ScriptingInterfaceOfIMaterial.GetAlphaTestValueDelegate call_GetAlphaTestValueDelegate;

		// Token: 0x0400026E RID: 622
		public static ScriptingInterfaceOfIMaterial.GetDefaultMaterialDelegate call_GetDefaultMaterialDelegate;

		// Token: 0x0400026F RID: 623
		public static ScriptingInterfaceOfIMaterial.GetFlagsDelegate call_GetFlagsDelegate;

		// Token: 0x04000270 RID: 624
		public static ScriptingInterfaceOfIMaterial.GetFromResourceDelegate call_GetFromResourceDelegate;

		// Token: 0x04000271 RID: 625
		public static ScriptingInterfaceOfIMaterial.GetNameDelegate call_GetNameDelegate;

		// Token: 0x04000272 RID: 626
		public static ScriptingInterfaceOfIMaterial.GetOutlineMaterialDelegate call_GetOutlineMaterialDelegate;

		// Token: 0x04000273 RID: 627
		public static ScriptingInterfaceOfIMaterial.GetShaderDelegate call_GetShaderDelegate;

		// Token: 0x04000274 RID: 628
		public static ScriptingInterfaceOfIMaterial.GetShaderFlagsDelegate call_GetShaderFlagsDelegate;

		// Token: 0x04000275 RID: 629
		public static ScriptingInterfaceOfIMaterial.GetTextureDelegate call_GetTextureDelegate;

		// Token: 0x04000276 RID: 630
		public static ScriptingInterfaceOfIMaterial.ReleaseDelegate call_ReleaseDelegate;

		// Token: 0x04000277 RID: 631
		public static ScriptingInterfaceOfIMaterial.RemoveMaterialShaderFlagDelegate call_RemoveMaterialShaderFlagDelegate;

		// Token: 0x04000278 RID: 632
		public static ScriptingInterfaceOfIMaterial.SetAlphaBlendModeDelegate call_SetAlphaBlendModeDelegate;

		// Token: 0x04000279 RID: 633
		public static ScriptingInterfaceOfIMaterial.SetAlphaTestValueDelegate call_SetAlphaTestValueDelegate;

		// Token: 0x0400027A RID: 634
		public static ScriptingInterfaceOfIMaterial.SetAreaMapScaleDelegate call_SetAreaMapScaleDelegate;

		// Token: 0x0400027B RID: 635
		public static ScriptingInterfaceOfIMaterial.SetEnableSkinningDelegate call_SetEnableSkinningDelegate;

		// Token: 0x0400027C RID: 636
		public static ScriptingInterfaceOfIMaterial.SetFlagsDelegate call_SetFlagsDelegate;

		// Token: 0x0400027D RID: 637
		public static ScriptingInterfaceOfIMaterial.SetMeshVectorArgumentDelegate call_SetMeshVectorArgumentDelegate;

		// Token: 0x0400027E RID: 638
		public static ScriptingInterfaceOfIMaterial.SetNameDelegate call_SetNameDelegate;

		// Token: 0x0400027F RID: 639
		public static ScriptingInterfaceOfIMaterial.SetShaderDelegate call_SetShaderDelegate;

		// Token: 0x04000280 RID: 640
		public static ScriptingInterfaceOfIMaterial.SetShaderFlagsDelegate call_SetShaderFlagsDelegate;

		// Token: 0x04000281 RID: 641
		public static ScriptingInterfaceOfIMaterial.SetTextureDelegate call_SetTextureDelegate;

		// Token: 0x04000282 RID: 642
		public static ScriptingInterfaceOfIMaterial.SetTextureAtSlotDelegate call_SetTextureAtSlotDelegate;

		// Token: 0x04000283 RID: 643
		public static ScriptingInterfaceOfIMaterial.UsingSkinningDelegate call_UsingSkinningDelegate;

		// Token: 0x020002E1 RID: 737
		// (Invoke) Token: 0x060011AF RID: 4527
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMaterialShaderFlagDelegate(UIntPtr materialPointer, byte[] flagName, [MarshalAs(UnmanagedType.U1)] bool showErrors);

		// Token: 0x020002E2 RID: 738
		// (Invoke) Token: 0x060011B3 RID: 4531
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateCopyDelegate(UIntPtr materialPointer);

		// Token: 0x020002E3 RID: 739
		// (Invoke) Token: 0x060011B7 RID: 4535
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAlphaBlendModeDelegate(UIntPtr materialPointer);

		// Token: 0x020002E4 RID: 740
		// (Invoke) Token: 0x060011BB RID: 4539
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAlphaTestValueDelegate(UIntPtr materialPointer);

		// Token: 0x020002E5 RID: 741
		// (Invoke) Token: 0x060011BF RID: 4543
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetDefaultMaterialDelegate();

		// Token: 0x020002E6 RID: 742
		// (Invoke) Token: 0x060011C3 RID: 4547
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate MaterialFlags GetFlagsDelegate(UIntPtr materialPointer);

		// Token: 0x020002E7 RID: 743
		// (Invoke) Token: 0x060011C7 RID: 4551
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetFromResourceDelegate(byte[] materialName);

		// Token: 0x020002E8 RID: 744
		// (Invoke) Token: 0x060011CB RID: 4555
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNameDelegate(UIntPtr materialPointer);

		// Token: 0x020002E9 RID: 745
		// (Invoke) Token: 0x060011CF RID: 4559
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetOutlineMaterialDelegate(UIntPtr materialPointer);

		// Token: 0x020002EA RID: 746
		// (Invoke) Token: 0x060011D3 RID: 4563
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetShaderDelegate(UIntPtr materialPointer);

		// Token: 0x020002EB RID: 747
		// (Invoke) Token: 0x060011D7 RID: 4567
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate ulong GetShaderFlagsDelegate(UIntPtr materialPointer);

		// Token: 0x020002EC RID: 748
		// (Invoke) Token: 0x060011DB RID: 4571
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetTextureDelegate(UIntPtr materialPointer, int textureType);

		// Token: 0x020002ED RID: 749
		// (Invoke) Token: 0x060011DF RID: 4575
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ReleaseDelegate(UIntPtr materialPointer);

		// Token: 0x020002EE RID: 750
		// (Invoke) Token: 0x060011E3 RID: 4579
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveMaterialShaderFlagDelegate(UIntPtr materialPointer, byte[] flagName);

		// Token: 0x020002EF RID: 751
		// (Invoke) Token: 0x060011E7 RID: 4583
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAlphaBlendModeDelegate(UIntPtr materialPointer, int alphaBlendMode);

		// Token: 0x020002F0 RID: 752
		// (Invoke) Token: 0x060011EB RID: 4587
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAlphaTestValueDelegate(UIntPtr materialPointer, float alphaTestValue);

		// Token: 0x020002F1 RID: 753
		// (Invoke) Token: 0x060011EF RID: 4591
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAreaMapScaleDelegate(UIntPtr materialPointer, float scale);

		// Token: 0x020002F2 RID: 754
		// (Invoke) Token: 0x060011F3 RID: 4595
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEnableSkinningDelegate(UIntPtr materialPointer, [MarshalAs(UnmanagedType.U1)] bool enable);

		// Token: 0x020002F3 RID: 755
		// (Invoke) Token: 0x060011F7 RID: 4599
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFlagsDelegate(UIntPtr materialPointer, MaterialFlags flags);

		// Token: 0x020002F4 RID: 756
		// (Invoke) Token: 0x060011FB RID: 4603
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMeshVectorArgumentDelegate(UIntPtr materialPointer, float x, float y, float z, float w);

		// Token: 0x020002F5 RID: 757
		// (Invoke) Token: 0x060011FF RID: 4607
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetNameDelegate(UIntPtr materialPointer, byte[] name);

		// Token: 0x020002F6 RID: 758
		// (Invoke) Token: 0x06001203 RID: 4611
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetShaderDelegate(UIntPtr materialPointer, UIntPtr shaderPointer);

		// Token: 0x020002F7 RID: 759
		// (Invoke) Token: 0x06001207 RID: 4615
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetShaderFlagsDelegate(UIntPtr materialPointer, ulong shaderFlags);

		// Token: 0x020002F8 RID: 760
		// (Invoke) Token: 0x0600120B RID: 4619
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetTextureDelegate(UIntPtr materialPointer, int textureType, UIntPtr texturePointer);

		// Token: 0x020002F9 RID: 761
		// (Invoke) Token: 0x0600120F RID: 4623
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetTextureAtSlotDelegate(UIntPtr materialPointer, int textureSlotIndex, UIntPtr texturePointer);

		// Token: 0x020002FA RID: 762
		// (Invoke) Token: 0x06001213 RID: 4627
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool UsingSkinningDelegate(UIntPtr materialPointer);
	}
}
