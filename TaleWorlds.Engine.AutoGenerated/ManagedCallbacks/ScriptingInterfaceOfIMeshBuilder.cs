using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x0200001A RID: 26
	internal class ScriptingInterfaceOfIMeshBuilder : IMeshBuilder
	{
		// Token: 0x06000356 RID: 854 RVA: 0x000142C8 File Offset: 0x000124C8
		public Mesh CreateTilingButtonMesh(string baseMeshName, ref Vec2 meshSizeMin, ref Vec2 meshSizeMax, ref Vec2 borderThickness)
		{
			byte[] array = null;
			if (baseMeshName != null)
			{
				int byteCount = ScriptingInterfaceOfIMeshBuilder._utf8.GetByteCount(baseMeshName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMeshBuilder._utf8.GetBytes(baseMeshName, 0, baseMeshName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMeshBuilder.call_CreateTilingButtonMeshDelegate(array, ref meshSizeMin, ref meshSizeMax, ref borderThickness);
			Mesh mesh = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				mesh = new Mesh(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return mesh;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00014358 File Offset: 0x00012558
		public Mesh CreateTilingWindowMesh(string baseMeshName, ref Vec2 meshSizeMin, ref Vec2 meshSizeMax, ref Vec2 borderThickness, ref Vec2 backgroundBorderThickness)
		{
			byte[] array = null;
			if (baseMeshName != null)
			{
				int byteCount = ScriptingInterfaceOfIMeshBuilder._utf8.GetByteCount(baseMeshName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMeshBuilder._utf8.GetBytes(baseMeshName, 0, baseMeshName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMeshBuilder.call_CreateTilingWindowMeshDelegate(array, ref meshSizeMin, ref meshSizeMax, ref borderThickness, ref backgroundBorderThickness);
			Mesh mesh = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				mesh = new Mesh(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return mesh;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x000143EC File Offset: 0x000125EC
		public Mesh FinalizeMeshBuilder(int num_vertices, Vec3[] vertices, int num_face_corners, MeshBuilder.FaceCorner[] faceCorners, int num_faces, MeshBuilder.Face[] faces)
		{
			PinnedArrayData<Vec3> pinnedArrayData = new PinnedArrayData<Vec3>(vertices, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			PinnedArrayData<MeshBuilder.FaceCorner> pinnedArrayData2 = new PinnedArrayData<MeshBuilder.FaceCorner>(faceCorners, false);
			IntPtr pointer2 = pinnedArrayData2.Pointer;
			PinnedArrayData<MeshBuilder.Face> pinnedArrayData3 = new PinnedArrayData<MeshBuilder.Face>(faces, false);
			IntPtr pointer3 = pinnedArrayData3.Pointer;
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMeshBuilder.call_FinalizeMeshBuilderDelegate(num_vertices, pointer, num_face_corners, pointer2, num_faces, pointer3);
			pinnedArrayData.Dispose();
			pinnedArrayData2.Dispose();
			pinnedArrayData3.Dispose();
			Mesh mesh = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				mesh = new Mesh(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return mesh;
		}

		// Token: 0x040002CA RID: 714
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002CB RID: 715
		public static ScriptingInterfaceOfIMeshBuilder.CreateTilingButtonMeshDelegate call_CreateTilingButtonMeshDelegate;

		// Token: 0x040002CC RID: 716
		public static ScriptingInterfaceOfIMeshBuilder.CreateTilingWindowMeshDelegate call_CreateTilingWindowMeshDelegate;

		// Token: 0x040002CD RID: 717
		public static ScriptingInterfaceOfIMeshBuilder.FinalizeMeshBuilderDelegate call_FinalizeMeshBuilderDelegate;

		// Token: 0x02000340 RID: 832
		// (Invoke) Token: 0x0600132B RID: 4907
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateTilingButtonMeshDelegate(byte[] baseMeshName, ref Vec2 meshSizeMin, ref Vec2 meshSizeMax, ref Vec2 borderThickness);

		// Token: 0x02000341 RID: 833
		// (Invoke) Token: 0x0600132F RID: 4911
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateTilingWindowMeshDelegate(byte[] baseMeshName, ref Vec2 meshSizeMin, ref Vec2 meshSizeMax, ref Vec2 borderThickness, ref Vec2 backgroundBorderThickness);

		// Token: 0x02000342 RID: 834
		// (Invoke) Token: 0x06001333 RID: 4915
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer FinalizeMeshBuilderDelegate(int num_vertices, IntPtr vertices, int num_face_corners, IntPtr faceCorners, int num_faces, IntPtr faces);
	}
}
