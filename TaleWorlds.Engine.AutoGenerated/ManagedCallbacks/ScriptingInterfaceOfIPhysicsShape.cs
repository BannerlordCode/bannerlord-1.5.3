using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x02000021 RID: 33
	internal class ScriptingInterfaceOfIPhysicsShape : IPhysicsShape
	{
		// Token: 0x060003E2 RID: 994 RVA: 0x00015350 File Offset: 0x00013550
		public void AddCapsule(UIntPtr shapePointer, ref CapsuleData data)
		{
			ScriptingInterfaceOfIPhysicsShape.call_AddCapsuleDelegate(shapePointer, ref data);
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00015360 File Offset: 0x00013560
		public void AddPreloadQueueWithName(string bodyName, ref Vec3 scale)
		{
			byte[] array = null;
			if (bodyName != null)
			{
				int byteCount = ScriptingInterfaceOfIPhysicsShape._utf8.GetByteCount(bodyName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIPhysicsShape._utf8.GetBytes(bodyName, 0, bodyName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIPhysicsShape.call_AddPreloadQueueWithNameDelegate(array, ref scale);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000153BB File Offset: 0x000135BB
		public void AddSphere(UIntPtr shapePointer, ref Vec3 origin, float radius)
		{
			ScriptingInterfaceOfIPhysicsShape.call_AddSphereDelegate(shapePointer, ref origin, radius);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000153CA File Offset: 0x000135CA
		public int CapsuleCount(UIntPtr shapePointer)
		{
			return ScriptingInterfaceOfIPhysicsShape.call_CapsuleCountDelegate(shapePointer);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x000153D7 File Offset: 0x000135D7
		public void clear(UIntPtr shapePointer)
		{
			ScriptingInterfaceOfIPhysicsShape.call_clearDelegate(shapePointer);
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000153E4 File Offset: 0x000135E4
		public PhysicsShape CreateBodyCopy(UIntPtr bodyPointer)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIPhysicsShape.call_CreateBodyCopyDelegate(bodyPointer);
			PhysicsShape physicsShape = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				physicsShape = new PhysicsShape(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return physicsShape;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001542E File Offset: 0x0001362E
		public void GetBoundingBox(UIntPtr shapePointer, out BoundingBox boundingBox)
		{
			ScriptingInterfaceOfIPhysicsShape.call_GetBoundingBoxDelegate(shapePointer, out boundingBox);
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x0001543C File Offset: 0x0001363C
		public Vec3 GetBoundingBoxCenter(UIntPtr shapePointer)
		{
			return ScriptingInterfaceOfIPhysicsShape.call_GetBoundingBoxCenterDelegate(shapePointer);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00015449 File Offset: 0x00013649
		public void GetCapsule(UIntPtr shapePointer, ref CapsuleData data, int index)
		{
			ScriptingInterfaceOfIPhysicsShape.call_GetCapsuleDelegate(shapePointer, ref data, index);
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00015458 File Offset: 0x00013658
		public void GetCapsuleWithMaterial(UIntPtr shapePointer, ref CapsuleData data, ref int materialIndex, int index)
		{
			ScriptingInterfaceOfIPhysicsShape.call_GetCapsuleWithMaterialDelegate(shapePointer, ref data, ref materialIndex, index);
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001546C File Offset: 0x0001366C
		public int GetDominantMaterialForTriangleMesh(PhysicsShape shape, int meshIndex)
		{
			UIntPtr uintPtr = ((shape != null) ? shape.Pointer : UIntPtr.Zero);
			return ScriptingInterfaceOfIPhysicsShape.call_GetDominantMaterialForTriangleMeshDelegate(uintPtr, meshIndex);
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0001549C File Offset: 0x0001369C
		public PhysicsShape GetFromResource(string bodyName, bool mayReturnNull)
		{
			byte[] array = null;
			if (bodyName != null)
			{
				int byteCount = ScriptingInterfaceOfIPhysicsShape._utf8.GetByteCount(bodyName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIPhysicsShape._utf8.GetBytes(bodyName, 0, bodyName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIPhysicsShape.call_GetFromResourceDelegate(array, mayReturnNull);
			PhysicsShape physicsShape = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				physicsShape = new PhysicsShape(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return physicsShape;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0001552C File Offset: 0x0001372C
		public string GetName(PhysicsShape shape)
		{
			UIntPtr uintPtr = ((shape != null) ? shape.Pointer : UIntPtr.Zero);
			if (ScriptingInterfaceOfIPhysicsShape.call_GetNameDelegate(uintPtr) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00015565 File Offset: 0x00013765
		public void GetSphere(UIntPtr shapePointer, ref SphereData data, int sphereIndex)
		{
			ScriptingInterfaceOfIPhysicsShape.call_GetSphereDelegate(shapePointer, ref data, sphereIndex);
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00015574 File Offset: 0x00013774
		public void GetSphereWithMaterial(UIntPtr shapePointer, ref SphereData data, ref int materialIndex, int sphereIndex)
		{
			ScriptingInterfaceOfIPhysicsShape.call_GetSphereWithMaterialDelegate(shapePointer, ref data, ref materialIndex, sphereIndex);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00015588 File Offset: 0x00013788
		public void GetTriangle(UIntPtr pointer, Vec3[] data, int meshIndex, int triangleIndex)
		{
			PinnedArrayData<Vec3> pinnedArrayData = new PinnedArrayData<Vec3>(data, false);
			IntPtr pointer2 = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIPhysicsShape.call_GetTriangleDelegate(pointer, pointer2, meshIndex, triangleIndex);
			pinnedArrayData.Dispose();
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x000155BC File Offset: 0x000137BC
		public void InitDescription(UIntPtr shapePointer)
		{
			ScriptingInterfaceOfIPhysicsShape.call_InitDescriptionDelegate(shapePointer);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x000155C9 File Offset: 0x000137C9
		public void Prepare(UIntPtr shapePointer)
		{
			ScriptingInterfaceOfIPhysicsShape.call_PrepareDelegate(shapePointer);
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000155D6 File Offset: 0x000137D6
		public void ProcessPreloadQueue()
		{
			ScriptingInterfaceOfIPhysicsShape.call_ProcessPreloadQueueDelegate();
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x000155E2 File Offset: 0x000137E2
		public void SetCapsule(UIntPtr shapePointer, ref CapsuleData data, int index)
		{
			ScriptingInterfaceOfIPhysicsShape.call_SetCapsuleDelegate(shapePointer, ref data, index);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x000155F1 File Offset: 0x000137F1
		public int SphereCount(UIntPtr pointer)
		{
			return ScriptingInterfaceOfIPhysicsShape.call_SphereCountDelegate(pointer);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x000155FE File Offset: 0x000137FE
		public void Transform(UIntPtr shapePointer, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfIPhysicsShape.call_TransformDelegate(shapePointer, ref frame);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001560C File Offset: 0x0001380C
		public int TriangleCountInTriangleMesh(UIntPtr pointer, int meshIndex)
		{
			return ScriptingInterfaceOfIPhysicsShape.call_TriangleCountInTriangleMeshDelegate(pointer, meshIndex);
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0001561A File Offset: 0x0001381A
		public int TriangleMeshCount(UIntPtr pointer)
		{
			return ScriptingInterfaceOfIPhysicsShape.call_TriangleMeshCountDelegate(pointer);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00015627 File Offset: 0x00013827
		public void UnloadDynamicBodies()
		{
			ScriptingInterfaceOfIPhysicsShape.call_UnloadDynamicBodiesDelegate();
		}

		// Token: 0x0400034D RID: 845
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400034E RID: 846
		public static ScriptingInterfaceOfIPhysicsShape.AddCapsuleDelegate call_AddCapsuleDelegate;

		// Token: 0x0400034F RID: 847
		public static ScriptingInterfaceOfIPhysicsShape.AddPreloadQueueWithNameDelegate call_AddPreloadQueueWithNameDelegate;

		// Token: 0x04000350 RID: 848
		public static ScriptingInterfaceOfIPhysicsShape.AddSphereDelegate call_AddSphereDelegate;

		// Token: 0x04000351 RID: 849
		public static ScriptingInterfaceOfIPhysicsShape.CapsuleCountDelegate call_CapsuleCountDelegate;

		// Token: 0x04000352 RID: 850
		public static ScriptingInterfaceOfIPhysicsShape.clearDelegate call_clearDelegate;

		// Token: 0x04000353 RID: 851
		public static ScriptingInterfaceOfIPhysicsShape.CreateBodyCopyDelegate call_CreateBodyCopyDelegate;

		// Token: 0x04000354 RID: 852
		public static ScriptingInterfaceOfIPhysicsShape.GetBoundingBoxDelegate call_GetBoundingBoxDelegate;

		// Token: 0x04000355 RID: 853
		public static ScriptingInterfaceOfIPhysicsShape.GetBoundingBoxCenterDelegate call_GetBoundingBoxCenterDelegate;

		// Token: 0x04000356 RID: 854
		public static ScriptingInterfaceOfIPhysicsShape.GetCapsuleDelegate call_GetCapsuleDelegate;

		// Token: 0x04000357 RID: 855
		public static ScriptingInterfaceOfIPhysicsShape.GetCapsuleWithMaterialDelegate call_GetCapsuleWithMaterialDelegate;

		// Token: 0x04000358 RID: 856
		public static ScriptingInterfaceOfIPhysicsShape.GetDominantMaterialForTriangleMeshDelegate call_GetDominantMaterialForTriangleMeshDelegate;

		// Token: 0x04000359 RID: 857
		public static ScriptingInterfaceOfIPhysicsShape.GetFromResourceDelegate call_GetFromResourceDelegate;

		// Token: 0x0400035A RID: 858
		public static ScriptingInterfaceOfIPhysicsShape.GetNameDelegate call_GetNameDelegate;

		// Token: 0x0400035B RID: 859
		public static ScriptingInterfaceOfIPhysicsShape.GetSphereDelegate call_GetSphereDelegate;

		// Token: 0x0400035C RID: 860
		public static ScriptingInterfaceOfIPhysicsShape.GetSphereWithMaterialDelegate call_GetSphereWithMaterialDelegate;

		// Token: 0x0400035D RID: 861
		public static ScriptingInterfaceOfIPhysicsShape.GetTriangleDelegate call_GetTriangleDelegate;

		// Token: 0x0400035E RID: 862
		public static ScriptingInterfaceOfIPhysicsShape.InitDescriptionDelegate call_InitDescriptionDelegate;

		// Token: 0x0400035F RID: 863
		public static ScriptingInterfaceOfIPhysicsShape.PrepareDelegate call_PrepareDelegate;

		// Token: 0x04000360 RID: 864
		public static ScriptingInterfaceOfIPhysicsShape.ProcessPreloadQueueDelegate call_ProcessPreloadQueueDelegate;

		// Token: 0x04000361 RID: 865
		public static ScriptingInterfaceOfIPhysicsShape.SetCapsuleDelegate call_SetCapsuleDelegate;

		// Token: 0x04000362 RID: 866
		public static ScriptingInterfaceOfIPhysicsShape.SphereCountDelegate call_SphereCountDelegate;

		// Token: 0x04000363 RID: 867
		public static ScriptingInterfaceOfIPhysicsShape.TransformDelegate call_TransformDelegate;

		// Token: 0x04000364 RID: 868
		public static ScriptingInterfaceOfIPhysicsShape.TriangleCountInTriangleMeshDelegate call_TriangleCountInTriangleMeshDelegate;

		// Token: 0x04000365 RID: 869
		public static ScriptingInterfaceOfIPhysicsShape.TriangleMeshCountDelegate call_TriangleMeshCountDelegate;

		// Token: 0x04000366 RID: 870
		public static ScriptingInterfaceOfIPhysicsShape.UnloadDynamicBodiesDelegate call_UnloadDynamicBodiesDelegate;

		// Token: 0x020003BC RID: 956
		// (Invoke) Token: 0x0600151B RID: 5403
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddCapsuleDelegate(UIntPtr shapePointer, ref CapsuleData data);

		// Token: 0x020003BD RID: 957
		// (Invoke) Token: 0x0600151F RID: 5407
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddPreloadQueueWithNameDelegate(byte[] bodyName, ref Vec3 scale);

		// Token: 0x020003BE RID: 958
		// (Invoke) Token: 0x06001523 RID: 5411
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddSphereDelegate(UIntPtr shapePointer, ref Vec3 origin, float radius);

		// Token: 0x020003BF RID: 959
		// (Invoke) Token: 0x06001527 RID: 5415
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int CapsuleCountDelegate(UIntPtr shapePointer);

		// Token: 0x020003C0 RID: 960
		// (Invoke) Token: 0x0600152B RID: 5419
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void clearDelegate(UIntPtr shapePointer);

		// Token: 0x020003C1 RID: 961
		// (Invoke) Token: 0x0600152F RID: 5423
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateBodyCopyDelegate(UIntPtr bodyPointer);

		// Token: 0x020003C2 RID: 962
		// (Invoke) Token: 0x06001533 RID: 5427
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoundingBoxDelegate(UIntPtr shapePointer, out BoundingBox boundingBox);

		// Token: 0x020003C3 RID: 963
		// (Invoke) Token: 0x06001537 RID: 5431
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetBoundingBoxCenterDelegate(UIntPtr shapePointer);

		// Token: 0x020003C4 RID: 964
		// (Invoke) Token: 0x0600153B RID: 5435
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetCapsuleDelegate(UIntPtr shapePointer, ref CapsuleData data, int index);

		// Token: 0x020003C5 RID: 965
		// (Invoke) Token: 0x0600153F RID: 5439
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetCapsuleWithMaterialDelegate(UIntPtr shapePointer, ref CapsuleData data, ref int materialIndex, int index);

		// Token: 0x020003C6 RID: 966
		// (Invoke) Token: 0x06001543 RID: 5443
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetDominantMaterialForTriangleMeshDelegate(UIntPtr shape, int meshIndex);

		// Token: 0x020003C7 RID: 967
		// (Invoke) Token: 0x06001547 RID: 5447
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetFromResourceDelegate(byte[] bodyName, [MarshalAs(UnmanagedType.U1)] bool mayReturnNull);

		// Token: 0x020003C8 RID: 968
		// (Invoke) Token: 0x0600154B RID: 5451
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNameDelegate(UIntPtr shape);

		// Token: 0x020003C9 RID: 969
		// (Invoke) Token: 0x0600154F RID: 5455
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetSphereDelegate(UIntPtr shapePointer, ref SphereData data, int sphereIndex);

		// Token: 0x020003CA RID: 970
		// (Invoke) Token: 0x06001553 RID: 5459
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetSphereWithMaterialDelegate(UIntPtr shapePointer, ref SphereData data, ref int materialIndex, int sphereIndex);

		// Token: 0x020003CB RID: 971
		// (Invoke) Token: 0x06001557 RID: 5463
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetTriangleDelegate(UIntPtr pointer, IntPtr data, int meshIndex, int triangleIndex);

		// Token: 0x020003CC RID: 972
		// (Invoke) Token: 0x0600155B RID: 5467
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void InitDescriptionDelegate(UIntPtr shapePointer);

		// Token: 0x020003CD RID: 973
		// (Invoke) Token: 0x0600155F RID: 5471
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PrepareDelegate(UIntPtr shapePointer);

		// Token: 0x020003CE RID: 974
		// (Invoke) Token: 0x06001563 RID: 5475
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ProcessPreloadQueueDelegate();

		// Token: 0x020003CF RID: 975
		// (Invoke) Token: 0x06001567 RID: 5479
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetCapsuleDelegate(UIntPtr shapePointer, ref CapsuleData data, int index);

		// Token: 0x020003D0 RID: 976
		// (Invoke) Token: 0x0600156B RID: 5483
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int SphereCountDelegate(UIntPtr pointer);

		// Token: 0x020003D1 RID: 977
		// (Invoke) Token: 0x0600156F RID: 5487
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TransformDelegate(UIntPtr shapePointer, ref MatrixFrame frame);

		// Token: 0x020003D2 RID: 978
		// (Invoke) Token: 0x06001573 RID: 5491
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int TriangleCountInTriangleMeshDelegate(UIntPtr pointer, int meshIndex);

		// Token: 0x020003D3 RID: 979
		// (Invoke) Token: 0x06001577 RID: 5495
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int TriangleMeshCountDelegate(UIntPtr pointer);

		// Token: 0x020003D4 RID: 980
		// (Invoke) Token: 0x0600157B RID: 5499
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UnloadDynamicBodiesDelegate();
	}
}
