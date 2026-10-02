using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x02000017 RID: 23
	internal class ScriptingInterfaceOfIMBMapScene : IMBMapScene
	{
		// Token: 0x06000261 RID: 609 RVA: 0x0000BD5C File Offset: 0x00009F5C
		public Vec3 GetAccessiblePointNearPosition(UIntPtr scenePointer, Vec2 position, bool isRegionMap0, float radius)
		{
			return ScriptingInterfaceOfIMBMapScene.call_GetAccessiblePointNearPositionDelegate(scenePointer, position, isRegionMap0, radius);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000BD70 File Offset: 0x00009F70
		public void GetBattleSceneIndexMap(UIntPtr scenePointer, byte[] indexData)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(indexData, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (indexData != null) ? indexData.Length : 0);
			ScriptingInterfaceOfIMBMapScene.call_GetBattleSceneIndexMapDelegate(scenePointer, managedArray);
			pinnedArrayData.Dispose();
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000BDB2 File Offset: 0x00009FB2
		public void GetBattleSceneIndexMapResolution(UIntPtr scenePointer, ref int width, ref int height)
		{
			ScriptingInterfaceOfIMBMapScene.call_GetBattleSceneIndexMapResolutionDelegate(scenePointer, ref width, ref height);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000BDC4 File Offset: 0x00009FC4
		public void GetColorGradeGridData(UIntPtr scenePointer, byte[] snowData, string textureName)
		{
			PinnedArrayData<byte> pinnedArrayData = new PinnedArrayData<byte>(snowData, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ManagedArray managedArray = new ManagedArray(pointer, (snowData != null) ? snowData.Length : 0);
			byte[] array = null;
			if (textureName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBMapScene._utf8.GetByteCount(textureName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBMapScene._utf8.GetBytes(textureName, 0, textureName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBMapScene.call_GetColorGradeGridDataDelegate(scenePointer, managedArray, array);
			pinnedArrayData.Dispose();
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000BE4D File Offset: 0x0000A04D
		public bool GetMouseVisible()
		{
			return ScriptingInterfaceOfIMBMapScene.call_GetMouseVisibleDelegate();
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000BE5C File Offset: 0x0000A05C
		public Vec2 GetNearestFaceCenterForPositionWithPath(UIntPtr scenePointer, int startFaceIndex, bool targetRegionMap0, float distMax, int[] excludedFaceIds, int excludedFaceIdCount)
		{
			PinnedArrayData<int> pinnedArrayData = new PinnedArrayData<int>(excludedFaceIds, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			Vec2 vec = ScriptingInterfaceOfIMBMapScene.call_GetNearestFaceCenterForPositionWithPathDelegate(scenePointer, startFaceIndex, targetRegionMap0, distMax, pointer, excludedFaceIdCount);
			pinnedArrayData.Dispose();
			return vec;
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000BE94 File Offset: 0x0000A094
		public Vec2 GetNearestFaceCenterPositionForPosition(UIntPtr scenePointer, Vec3 position, bool isRegionMap0, int[] excludedFaceIds, int excludedFaceIdCount, float heightLimit)
		{
			PinnedArrayData<int> pinnedArrayData = new PinnedArrayData<int>(excludedFaceIds, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			Vec2 vec = ScriptingInterfaceOfIMBMapScene.call_GetNearestFaceCenterPositionForPositionDelegate(scenePointer, position, isRegionMap0, pointer, excludedFaceIdCount, heightLimit);
			pinnedArrayData.Dispose();
			return vec;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000BECC File Offset: 0x0000A0CC
		public float GetSeasonTimeFactor(UIntPtr scenePointer)
		{
			return ScriptingInterfaceOfIMBMapScene.call_GetSeasonTimeFactorDelegate(scenePointer);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000BED9 File Offset: 0x0000A0D9
		public void LoadAtmosphereData(UIntPtr scenePointer)
		{
			ScriptingInterfaceOfIMBMapScene.call_LoadAtmosphereDataDelegate(scenePointer);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000BEE6 File Offset: 0x0000A0E6
		public void RemoveZeroCornerBodies(UIntPtr scenePointer)
		{
			ScriptingInterfaceOfIMBMapScene.call_RemoveZeroCornerBodiesDelegate(scenePointer);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000BEF3 File Offset: 0x0000A0F3
		public void SendMouseKeyEvent(int keyId, bool isDown)
		{
			ScriptingInterfaceOfIMBMapScene.call_SendMouseKeyEventDelegate(keyId, isDown);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000BF01 File Offset: 0x0000A101
		public void SetFrameForAtmosphere(UIntPtr scenePointer, float tod, float cameraElevation, bool forceLoadTextures)
		{
			ScriptingInterfaceOfIMBMapScene.call_SetFrameForAtmosphereDelegate(scenePointer, tod, cameraElevation, forceLoadTextures);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000BF12 File Offset: 0x0000A112
		public void SetMousePos(int posX, int posY)
		{
			ScriptingInterfaceOfIMBMapScene.call_SetMousePosDelegate(posX, posY);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000BF20 File Offset: 0x0000A120
		public void SetMouseVisible(bool value)
		{
			ScriptingInterfaceOfIMBMapScene.call_SetMouseVisibleDelegate(value);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000BF30 File Offset: 0x0000A130
		public void SetPoliticalColor(UIntPtr scenePointer, string value)
		{
			byte[] array = null;
			if (value != null)
			{
				int byteCount = ScriptingInterfaceOfIMBMapScene._utf8.GetByteCount(value);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBMapScene._utf8.GetBytes(value, 0, value.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBMapScene.call_SetPoliticalColorDelegate(scenePointer, array);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000BF8B File Offset: 0x0000A18B
		public void SetSeasonTimeFactor(UIntPtr scenePointer, float seasonTimeFactor)
		{
			ScriptingInterfaceOfIMBMapScene.call_SetSeasonTimeFactorDelegate(scenePointer, seasonTimeFactor);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000BF99 File Offset: 0x0000A199
		public void SetTerrainDynamicParams(UIntPtr scenePointer, Vec3 dynamic_params)
		{
			ScriptingInterfaceOfIMBMapScene.call_SetTerrainDynamicParamsDelegate(scenePointer, dynamic_params);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000BFA7 File Offset: 0x0000A1A7
		public void TickAmbientSounds(UIntPtr scenePointer, int terrainType)
		{
			ScriptingInterfaceOfIMBMapScene.call_TickAmbientSoundsDelegate(scenePointer, terrainType);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000BFB5 File Offset: 0x0000A1B5
		public void TickStepSound(UIntPtr scenePointer, UIntPtr visualsPointer, int faceIndexTerrainType, TerrainTypeSoundSlot soundType, int partySize)
		{
			ScriptingInterfaceOfIMBMapScene.call_TickStepSoundDelegate(scenePointer, visualsPointer, faceIndexTerrainType, soundType, partySize);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000BFC8 File Offset: 0x0000A1C8
		public void TickVisuals(UIntPtr scenePointer, float tod, UIntPtr[] ticked_map_meshes, int tickedMapMeshesCount)
		{
			PinnedArrayData<UIntPtr> pinnedArrayData = new PinnedArrayData<UIntPtr>(ticked_map_meshes, false);
			IntPtr pointer = pinnedArrayData.Pointer;
			ScriptingInterfaceOfIMBMapScene.call_TickVisualsDelegate(scenePointer, tod, pointer, tickedMapMeshesCount);
			pinnedArrayData.Dispose();
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000BFFC File Offset: 0x0000A1FC
		public void ValidateTerrainSoundIds()
		{
			ScriptingInterfaceOfIMBMapScene.call_ValidateTerrainSoundIdsDelegate();
		}

		// Token: 0x040001E0 RID: 480
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040001E1 RID: 481
		public static ScriptingInterfaceOfIMBMapScene.GetAccessiblePointNearPositionDelegate call_GetAccessiblePointNearPositionDelegate;

		// Token: 0x040001E2 RID: 482
		public static ScriptingInterfaceOfIMBMapScene.GetBattleSceneIndexMapDelegate call_GetBattleSceneIndexMapDelegate;

		// Token: 0x040001E3 RID: 483
		public static ScriptingInterfaceOfIMBMapScene.GetBattleSceneIndexMapResolutionDelegate call_GetBattleSceneIndexMapResolutionDelegate;

		// Token: 0x040001E4 RID: 484
		public static ScriptingInterfaceOfIMBMapScene.GetColorGradeGridDataDelegate call_GetColorGradeGridDataDelegate;

		// Token: 0x040001E5 RID: 485
		public static ScriptingInterfaceOfIMBMapScene.GetMouseVisibleDelegate call_GetMouseVisibleDelegate;

		// Token: 0x040001E6 RID: 486
		public static ScriptingInterfaceOfIMBMapScene.GetNearestFaceCenterForPositionWithPathDelegate call_GetNearestFaceCenterForPositionWithPathDelegate;

		// Token: 0x040001E7 RID: 487
		public static ScriptingInterfaceOfIMBMapScene.GetNearestFaceCenterPositionForPositionDelegate call_GetNearestFaceCenterPositionForPositionDelegate;

		// Token: 0x040001E8 RID: 488
		public static ScriptingInterfaceOfIMBMapScene.GetSeasonTimeFactorDelegate call_GetSeasonTimeFactorDelegate;

		// Token: 0x040001E9 RID: 489
		public static ScriptingInterfaceOfIMBMapScene.LoadAtmosphereDataDelegate call_LoadAtmosphereDataDelegate;

		// Token: 0x040001EA RID: 490
		public static ScriptingInterfaceOfIMBMapScene.RemoveZeroCornerBodiesDelegate call_RemoveZeroCornerBodiesDelegate;

		// Token: 0x040001EB RID: 491
		public static ScriptingInterfaceOfIMBMapScene.SendMouseKeyEventDelegate call_SendMouseKeyEventDelegate;

		// Token: 0x040001EC RID: 492
		public static ScriptingInterfaceOfIMBMapScene.SetFrameForAtmosphereDelegate call_SetFrameForAtmosphereDelegate;

		// Token: 0x040001ED RID: 493
		public static ScriptingInterfaceOfIMBMapScene.SetMousePosDelegate call_SetMousePosDelegate;

		// Token: 0x040001EE RID: 494
		public static ScriptingInterfaceOfIMBMapScene.SetMouseVisibleDelegate call_SetMouseVisibleDelegate;

		// Token: 0x040001EF RID: 495
		public static ScriptingInterfaceOfIMBMapScene.SetPoliticalColorDelegate call_SetPoliticalColorDelegate;

		// Token: 0x040001F0 RID: 496
		public static ScriptingInterfaceOfIMBMapScene.SetSeasonTimeFactorDelegate call_SetSeasonTimeFactorDelegate;

		// Token: 0x040001F1 RID: 497
		public static ScriptingInterfaceOfIMBMapScene.SetTerrainDynamicParamsDelegate call_SetTerrainDynamicParamsDelegate;

		// Token: 0x040001F2 RID: 498
		public static ScriptingInterfaceOfIMBMapScene.TickAmbientSoundsDelegate call_TickAmbientSoundsDelegate;

		// Token: 0x040001F3 RID: 499
		public static ScriptingInterfaceOfIMBMapScene.TickStepSoundDelegate call_TickStepSoundDelegate;

		// Token: 0x040001F4 RID: 500
		public static ScriptingInterfaceOfIMBMapScene.TickVisualsDelegate call_TickVisualsDelegate;

		// Token: 0x040001F5 RID: 501
		public static ScriptingInterfaceOfIMBMapScene.ValidateTerrainSoundIdsDelegate call_ValidateTerrainSoundIdsDelegate;

		// Token: 0x02000243 RID: 579
		// (Invoke) Token: 0x06000BFC RID: 3068
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetAccessiblePointNearPositionDelegate(UIntPtr scenePointer, Vec2 position, [MarshalAs(UnmanagedType.U1)] bool isRegionMap0, float radius);

		// Token: 0x02000244 RID: 580
		// (Invoke) Token: 0x06000C00 RID: 3072
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBattleSceneIndexMapDelegate(UIntPtr scenePointer, ManagedArray indexData);

		// Token: 0x02000245 RID: 581
		// (Invoke) Token: 0x06000C04 RID: 3076
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBattleSceneIndexMapResolutionDelegate(UIntPtr scenePointer, ref int width, ref int height);

		// Token: 0x02000246 RID: 582
		// (Invoke) Token: 0x06000C08 RID: 3080
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetColorGradeGridDataDelegate(UIntPtr scenePointer, ManagedArray snowData, byte[] textureName);

		// Token: 0x02000247 RID: 583
		// (Invoke) Token: 0x06000C0C RID: 3084
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool GetMouseVisibleDelegate();

		// Token: 0x02000248 RID: 584
		// (Invoke) Token: 0x06000C10 RID: 3088
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec2 GetNearestFaceCenterForPositionWithPathDelegate(UIntPtr scenePointer, int startFaceIndex, [MarshalAs(UnmanagedType.U1)] bool targetRegionMap0, float distMax, IntPtr excludedFaceIds, int excludedFaceIdCount);

		// Token: 0x02000249 RID: 585
		// (Invoke) Token: 0x06000C14 RID: 3092
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec2 GetNearestFaceCenterPositionForPositionDelegate(UIntPtr scenePointer, Vec3 position, [MarshalAs(UnmanagedType.U1)] bool isRegionMap0, IntPtr excludedFaceIds, int excludedFaceIdCount, float heightLimit);

		// Token: 0x0200024A RID: 586
		// (Invoke) Token: 0x06000C18 RID: 3096
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetSeasonTimeFactorDelegate(UIntPtr scenePointer);

		// Token: 0x0200024B RID: 587
		// (Invoke) Token: 0x06000C1C RID: 3100
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void LoadAtmosphereDataDelegate(UIntPtr scenePointer);

		// Token: 0x0200024C RID: 588
		// (Invoke) Token: 0x06000C20 RID: 3104
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveZeroCornerBodiesDelegate(UIntPtr scenePointer);

		// Token: 0x0200024D RID: 589
		// (Invoke) Token: 0x06000C24 RID: 3108
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SendMouseKeyEventDelegate(int keyId, [MarshalAs(UnmanagedType.U1)] bool isDown);

		// Token: 0x0200024E RID: 590
		// (Invoke) Token: 0x06000C28 RID: 3112
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFrameForAtmosphereDelegate(UIntPtr scenePointer, float tod, float cameraElevation, [MarshalAs(UnmanagedType.U1)] bool forceLoadTextures);

		// Token: 0x0200024F RID: 591
		// (Invoke) Token: 0x06000C2C RID: 3116
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMousePosDelegate(int posX, int posY);

		// Token: 0x02000250 RID: 592
		// (Invoke) Token: 0x06000C30 RID: 3120
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetMouseVisibleDelegate([MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x02000251 RID: 593
		// (Invoke) Token: 0x06000C34 RID: 3124
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetPoliticalColorDelegate(UIntPtr scenePointer, byte[] value);

		// Token: 0x02000252 RID: 594
		// (Invoke) Token: 0x06000C38 RID: 3128
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSeasonTimeFactorDelegate(UIntPtr scenePointer, float seasonTimeFactor);

		// Token: 0x02000253 RID: 595
		// (Invoke) Token: 0x06000C3C RID: 3132
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetTerrainDynamicParamsDelegate(UIntPtr scenePointer, Vec3 dynamic_params);

		// Token: 0x02000254 RID: 596
		// (Invoke) Token: 0x06000C40 RID: 3136
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TickAmbientSoundsDelegate(UIntPtr scenePointer, int terrainType);

		// Token: 0x02000255 RID: 597
		// (Invoke) Token: 0x06000C44 RID: 3140
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TickStepSoundDelegate(UIntPtr scenePointer, UIntPtr visualsPointer, int faceIndexTerrainType, TerrainTypeSoundSlot soundType, int partySize);

		// Token: 0x02000256 RID: 598
		// (Invoke) Token: 0x06000C48 RID: 3144
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TickVisualsDelegate(UIntPtr scenePointer, float tod, IntPtr ticked_map_meshes, int tickedMapMeshesCount);

		// Token: 0x02000257 RID: 599
		// (Invoke) Token: 0x06000C4C RID: 3148
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ValidateTerrainSoundIdsDelegate();
	}
}
