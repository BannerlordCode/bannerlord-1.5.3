using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x0200002B RID: 43
	internal class ScriptingInterfaceOfITerrainEdit : ITerrainEdit
	{
		// Token: 0x06000601 RID: 1537 RVA: 0x000198C4 File Offset: 0x00017AC4
		public int AddEmptyLayer(UIntPtr scenePointer, string layerName)
		{
			byte[] array = null;
			if (layerName != null)
			{
				int byteCount = ScriptingInterfaceOfITerrainEdit._utf8.GetByteCount(layerName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITerrainEdit._utf8.GetBytes(layerName, 0, layerName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfITerrainEdit.call_AddEmptyLayerDelegate(scenePointer, array);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00019920 File Offset: 0x00017B20
		public int AddLayerFromMaterial(UIntPtr scenePointer, string layerPrefabName)
		{
			byte[] array = null;
			if (layerPrefabName != null)
			{
				int byteCount = ScriptingInterfaceOfITerrainEdit._utf8.GetByteCount(layerPrefabName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITerrainEdit._utf8.GetBytes(layerPrefabName, 0, layerPrefabName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfITerrainEdit.call_AddLayerFromMaterialDelegate(scenePointer, array);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001997C File Offset: 0x00017B7C
		public void AddPlacedFlora(Scene scene, string floraKindName, ref MatrixFrame frame)
		{
			UIntPtr uintPtr = ((scene != null) ? scene.Pointer : UIntPtr.Zero);
			byte[] array = null;
			if (floraKindName != null)
			{
				int byteCount = ScriptingInterfaceOfITerrainEdit._utf8.GetByteCount(floraKindName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITerrainEdit._utf8.GetBytes(floraKindName, 0, floraKindName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfITerrainEdit.call_AddPlacedFloraDelegate(uintPtr, array, ref frame);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x000199F0 File Offset: 0x00017BF0
		public int AddProceduralFloraToLayer(Scene scene, int layerIndex, string floraKindName, float density, int seedIndex, float sizeMin, float sizeMax, float colonyRadius, float colonyThreshold, float weightOffset)
		{
			UIntPtr uintPtr = ((scene != null) ? scene.Pointer : UIntPtr.Zero);
			byte[] array = null;
			if (floraKindName != null)
			{
				int byteCount = ScriptingInterfaceOfITerrainEdit._utf8.GetByteCount(floraKindName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITerrainEdit._utf8.GetBytes(floraKindName, 0, floraKindName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfITerrainEdit.call_AddProceduralFloraToLayerDelegate(uintPtr, layerIndex, array, density, seedIndex, sizeMin, sizeMax, colonyRadius, colonyThreshold, weightOffset);
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00019A74 File Offset: 0x00017C74
		public void CreateTerrain(UIntPtr scenePointer, int nodeDimX, int nodeDimY, float nodeSize, float minHeight, float maxHeight, int heightmapDetailLevel, string baseLayerName)
		{
			byte[] array = null;
			if (baseLayerName != null)
			{
				int byteCount = ScriptingInterfaceOfITerrainEdit._utf8.GetByteCount(baseLayerName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITerrainEdit._utf8.GetBytes(baseLayerName, 0, baseLayerName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfITerrainEdit.call_CreateTerrainDelegate(scenePointer, nodeDimX, nodeDimY, nodeSize, minHeight, maxHeight, heightmapDetailLevel, array);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00019ADD File Offset: 0x00017CDD
		public void Finalize(UIntPtr scenePointer)
		{
			ScriptingInterfaceOfITerrainEdit.call_FinalizeDelegate(scenePointer);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00019AEC File Offset: 0x00017CEC
		public void FinalizeFlora(Scene scene)
		{
			UIntPtr uintPtr = ((scene != null) ? scene.Pointer : UIntPtr.Zero);
			ScriptingInterfaceOfITerrainEdit.call_FinalizeFloraDelegate(uintPtr);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00019B1B File Offset: 0x00017D1B
		public void SetLayerPropertyFloat(UIntPtr scenePointer, int layerIndex, int propertyId, float value)
		{
			ScriptingInterfaceOfITerrainEdit.call_SetLayerPropertyFloatDelegate(scenePointer, layerIndex, propertyId, value);
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x00019B2C File Offset: 0x00017D2C
		public void SetLayerPropertyString(UIntPtr scenePointer, int layerIndex, int propertyId, string value)
		{
			byte[] array = null;
			if (value != null)
			{
				int byteCount = ScriptingInterfaceOfITerrainEdit._utf8.GetByteCount(value);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITerrainEdit._utf8.GetBytes(value, 0, value.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfITerrainEdit.call_SetLayerPropertyStringDelegate(scenePointer, layerIndex, propertyId, array);
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00019B90 File Offset: 0x00017D90
		public void SetLayerTexture(UIntPtr scenePointer, int layerIndex, int textureType, string textureName)
		{
			byte[] array = null;
			if (textureName != null)
			{
				int byteCount = ScriptingInterfaceOfITerrainEdit._utf8.GetByteCount(textureName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfITerrainEdit._utf8.GetBytes(textureName, 0, textureName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfITerrainEdit.call_SetLayerTextureDelegate(scenePointer, layerIndex, textureType, array);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00019BF4 File Offset: 0x00017DF4
		public void SetNodeHeightData(Scene scene, int nodeX, int nodeY, float[] heights, int heightCount)
		{
			UIntPtr uintPtr = ((scene != null) ? scene.Pointer : UIntPtr.Zero);
			PinnedArrayData<float> pinnedArrayData = new PinnedArrayData<float>(heights, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfITerrainEdit.call_SetNodeHeightDataDelegate(uintPtr, nodeX, nodeY, pointer, heightCount);
			pinnedArrayData.Dispose();
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00019C44 File Offset: 0x00017E44
		public void SetNodeLayerWeightData(Scene scene, int nodeX, int nodeY, int layerIndex, float[] weights, int weightCount)
		{
			UIntPtr uintPtr = ((scene != null) ? scene.Pointer : UIntPtr.Zero);
			PinnedArrayData<float> pinnedArrayData = new PinnedArrayData<float>(weights, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfITerrainEdit.call_SetNodeLayerWeightDataDelegate(uintPtr, nodeX, nodeY, layerIndex, pointer, weightCount);
			pinnedArrayData.Dispose();
		}

		// Token: 0x04000553 RID: 1363
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000554 RID: 1364
		public static ScriptingInterfaceOfITerrainEdit.AddEmptyLayerDelegate call_AddEmptyLayerDelegate;

		// Token: 0x04000555 RID: 1365
		public static ScriptingInterfaceOfITerrainEdit.AddLayerFromMaterialDelegate call_AddLayerFromMaterialDelegate;

		// Token: 0x04000556 RID: 1366
		public static ScriptingInterfaceOfITerrainEdit.AddPlacedFloraDelegate call_AddPlacedFloraDelegate;

		// Token: 0x04000557 RID: 1367
		public static ScriptingInterfaceOfITerrainEdit.AddProceduralFloraToLayerDelegate call_AddProceduralFloraToLayerDelegate;

		// Token: 0x04000558 RID: 1368
		public static ScriptingInterfaceOfITerrainEdit.CreateTerrainDelegate call_CreateTerrainDelegate;

		// Token: 0x04000559 RID: 1369
		public static ScriptingInterfaceOfITerrainEdit.FinalizeDelegate call_FinalizeDelegate;

		// Token: 0x0400055A RID: 1370
		public static ScriptingInterfaceOfITerrainEdit.FinalizeFloraDelegate call_FinalizeFloraDelegate;

		// Token: 0x0400055B RID: 1371
		public static ScriptingInterfaceOfITerrainEdit.SetLayerPropertyFloatDelegate call_SetLayerPropertyFloatDelegate;

		// Token: 0x0400055C RID: 1372
		public static ScriptingInterfaceOfITerrainEdit.SetLayerPropertyStringDelegate call_SetLayerPropertyStringDelegate;

		// Token: 0x0400055D RID: 1373
		public static ScriptingInterfaceOfITerrainEdit.SetLayerTextureDelegate call_SetLayerTextureDelegate;

		// Token: 0x0400055E RID: 1374
		public static ScriptingInterfaceOfITerrainEdit.SetNodeHeightDataDelegate call_SetNodeHeightDataDelegate;

		// Token: 0x0400055F RID: 1375
		public static ScriptingInterfaceOfITerrainEdit.SetNodeLayerWeightDataDelegate call_SetNodeLayerWeightDataDelegate;

		// Token: 0x020005B8 RID: 1464
		// (Invoke) Token: 0x06001D0B RID: 7435
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int AddEmptyLayerDelegate(UIntPtr scenePointer, byte[] layerName);

		// Token: 0x020005B9 RID: 1465
		// (Invoke) Token: 0x06001D0F RID: 7439
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int AddLayerFromMaterialDelegate(UIntPtr scenePointer, byte[] layerPrefabName);

		// Token: 0x020005BA RID: 1466
		// (Invoke) Token: 0x06001D13 RID: 7443
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddPlacedFloraDelegate(UIntPtr scene, byte[] floraKindName, ref MatrixFrame frame);

		// Token: 0x020005BB RID: 1467
		// (Invoke) Token: 0x06001D17 RID: 7447
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int AddProceduralFloraToLayerDelegate(UIntPtr scene, int layerIndex, byte[] floraKindName, float density, int seedIndex, float sizeMin, float sizeMax, float colonyRadius, float colonyThreshold, float weightOffset);

		// Token: 0x020005BC RID: 1468
		// (Invoke) Token: 0x06001D1B RID: 7451
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void CreateTerrainDelegate(UIntPtr scenePointer, int nodeDimX, int nodeDimY, float nodeSize, float minHeight, float maxHeight, int heightmapDetailLevel, byte[] baseLayerName);

		// Token: 0x020005BD RID: 1469
		// (Invoke) Token: 0x06001D1F RID: 7455
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FinalizeDelegate(UIntPtr scenePointer);

		// Token: 0x020005BE RID: 1470
		// (Invoke) Token: 0x06001D23 RID: 7459
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FinalizeFloraDelegate(UIntPtr scene);

		// Token: 0x020005BF RID: 1471
		// (Invoke) Token: 0x06001D27 RID: 7463
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetLayerPropertyFloatDelegate(UIntPtr scenePointer, int layerIndex, int propertyId, float value);

		// Token: 0x020005C0 RID: 1472
		// (Invoke) Token: 0x06001D2B RID: 7467
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetLayerPropertyStringDelegate(UIntPtr scenePointer, int layerIndex, int propertyId, byte[] value);

		// Token: 0x020005C1 RID: 1473
		// (Invoke) Token: 0x06001D2F RID: 7471
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetLayerTextureDelegate(UIntPtr scenePointer, int layerIndex, int textureType, byte[] textureName);

		// Token: 0x020005C2 RID: 1474
		// (Invoke) Token: 0x06001D33 RID: 7475
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetNodeHeightDataDelegate(UIntPtr scene, int nodeX, int nodeY, IntPtr heights, int heightCount);

		// Token: 0x020005C3 RID: 1475
		// (Invoke) Token: 0x06001D37 RID: 7479
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetNodeLayerWeightDataDelegate(UIntPtr scene, int nodeX, int nodeY, int layerIndex, IntPtr weights, int weightCount);
	}
}
