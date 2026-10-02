using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x02000030 RID: 48
	internal class ScriptingInterfaceOfITwoDimensionView : ITwoDimensionView
	{
		// Token: 0x06000640 RID: 1600 RVA: 0x0001A5E7 File Offset: 0x000187E7
		public bool AddCachedTextMesh(UIntPtr pointer, UIntPtr material, ref TwoDimensionTextMeshDrawData meshDrawData)
		{
			return ScriptingInterfaceOfITwoDimensionView.call_AddCachedTextMeshDelegate(pointer, material, ref meshDrawData);
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0001A5F6 File Offset: 0x000187F6
		public void AddNewMesh(UIntPtr pointer, UIntPtr material, ref TwoDimensionMeshDrawData meshDrawData)
		{
			ScriptingInterfaceOfITwoDimensionView.call_AddNewMeshDelegate(pointer, material, ref meshDrawData);
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0001A605 File Offset: 0x00018805
		public void AddNewQuadMesh(UIntPtr pointer, UIntPtr material, ref TwoDimensionMeshDrawData meshDrawData)
		{
			ScriptingInterfaceOfITwoDimensionView.call_AddNewQuadMeshDelegate(pointer, material, ref meshDrawData);
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0001A614 File Offset: 0x00018814
		public void AddNewTextMesh(UIntPtr pointer, float[] vertices, float[] uvs, uint[] indices, int vertexCount, int indexCount, UIntPtr material, ref TwoDimensionTextMeshDrawData meshDrawData)
		{
			PinnedArrayData<float> pinnedArrayData = new PinnedArrayData<float>(vertices, false);
			IntPtr pointer2 = pinnedArrayData.Pointer;
			PinnedArrayData<float> pinnedArrayData2 = new PinnedArrayData<float>(uvs, false);
			IntPtr pointer3 = pinnedArrayData2.Pointer;
			PinnedArrayData<uint> pinnedArrayData3 = new PinnedArrayData<uint>(indices, false);
			IntPtr pointer4 = pinnedArrayData3.Pointer;
			ScriptingInterfaceOfITwoDimensionView.call_AddNewTextMeshDelegate(pointer, pointer2, pointer3, pointer4, vertexCount, indexCount, material, ref meshDrawData);
			pinnedArrayData.Dispose();
			pinnedArrayData2.Dispose();
			pinnedArrayData3.Dispose();
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0001A682 File Offset: 0x00018882
		public void BeginFrame(UIntPtr pointer)
		{
			ScriptingInterfaceOfITwoDimensionView.call_BeginFrameDelegate(pointer);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x0001A68F File Offset: 0x0001888F
		public void Clear(UIntPtr pointer)
		{
			ScriptingInterfaceOfITwoDimensionView.call_ClearDelegate(pointer);
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x0001A69C File Offset: 0x0001889C
		public TwoDimensionView CreateTwoDimensionView(string viewName)
		{
			byte[] array = null;
			if (viewName != null)
			{
				int byteCount = ScriptingInterfaceOfITwoDimensionView._utf8.GetByteCount(viewName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITwoDimensionView._utf8.GetBytes(viewName, 0, viewName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfITwoDimensionView.call_CreateTwoDimensionViewDelegate(array);
			TwoDimensionView twoDimensionView = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				twoDimensionView = new TwoDimensionView(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return twoDimensionView;
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0001A728 File Offset: 0x00018928
		public void EndFrame(UIntPtr pointer)
		{
			ScriptingInterfaceOfITwoDimensionView.call_EndFrameDelegate(pointer);
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0001A735 File Offset: 0x00018935
		public UIntPtr GetOrCreateMaterial(UIntPtr pointer, UIntPtr mainTexture, UIntPtr overlayTexture)
		{
			return ScriptingInterfaceOfITwoDimensionView.call_GetOrCreateMaterialDelegate(pointer, mainTexture, overlayTexture);
		}

		// Token: 0x0400058D RID: 1421
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400058E RID: 1422
		public static ScriptingInterfaceOfITwoDimensionView.AddCachedTextMeshDelegate call_AddCachedTextMeshDelegate;

		// Token: 0x0400058F RID: 1423
		public static ScriptingInterfaceOfITwoDimensionView.AddNewMeshDelegate call_AddNewMeshDelegate;

		// Token: 0x04000590 RID: 1424
		public static ScriptingInterfaceOfITwoDimensionView.AddNewQuadMeshDelegate call_AddNewQuadMeshDelegate;

		// Token: 0x04000591 RID: 1425
		public static ScriptingInterfaceOfITwoDimensionView.AddNewTextMeshDelegate call_AddNewTextMeshDelegate;

		// Token: 0x04000592 RID: 1426
		public static ScriptingInterfaceOfITwoDimensionView.BeginFrameDelegate call_BeginFrameDelegate;

		// Token: 0x04000593 RID: 1427
		public static ScriptingInterfaceOfITwoDimensionView.ClearDelegate call_ClearDelegate;

		// Token: 0x04000594 RID: 1428
		public static ScriptingInterfaceOfITwoDimensionView.CreateTwoDimensionViewDelegate call_CreateTwoDimensionViewDelegate;

		// Token: 0x04000595 RID: 1429
		public static ScriptingInterfaceOfITwoDimensionView.EndFrameDelegate call_EndFrameDelegate;

		// Token: 0x04000596 RID: 1430
		public static ScriptingInterfaceOfITwoDimensionView.GetOrCreateMaterialDelegate call_GetOrCreateMaterialDelegate;

		// Token: 0x020005ED RID: 1517
		// (Invoke) Token: 0x06001DDF RID: 7647
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool AddCachedTextMeshDelegate(UIntPtr pointer, UIntPtr material, ref TwoDimensionTextMeshDrawData meshDrawData);

		// Token: 0x020005EE RID: 1518
		// (Invoke) Token: 0x06001DE3 RID: 7651
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddNewMeshDelegate(UIntPtr pointer, UIntPtr material, ref TwoDimensionMeshDrawData meshDrawData);

		// Token: 0x020005EF RID: 1519
		// (Invoke) Token: 0x06001DE7 RID: 7655
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddNewQuadMeshDelegate(UIntPtr pointer, UIntPtr material, ref TwoDimensionMeshDrawData meshDrawData);

		// Token: 0x020005F0 RID: 1520
		// (Invoke) Token: 0x06001DEB RID: 7659
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddNewTextMeshDelegate(UIntPtr pointer, IntPtr vertices, IntPtr uvs, IntPtr indices, int vertexCount, int indexCount, UIntPtr material, ref TwoDimensionTextMeshDrawData meshDrawData);

		// Token: 0x020005F1 RID: 1521
		// (Invoke) Token: 0x06001DEF RID: 7663
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void BeginFrameDelegate(UIntPtr pointer);

		// Token: 0x020005F2 RID: 1522
		// (Invoke) Token: 0x06001DF3 RID: 7667
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearDelegate(UIntPtr pointer);

		// Token: 0x020005F3 RID: 1523
		// (Invoke) Token: 0x06001DF7 RID: 7671
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateTwoDimensionViewDelegate(byte[] viewName);

		// Token: 0x020005F4 RID: 1524
		// (Invoke) Token: 0x06001DFB RID: 7675
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EndFrameDelegate(UIntPtr pointer);

		// Token: 0x020005F5 RID: 1525
		// (Invoke) Token: 0x06001DFF RID: 7679
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr GetOrCreateMaterialDelegate(UIntPtr pointer, UIntPtr mainTexture, UIntPtr overlayTexture);
	}
}
