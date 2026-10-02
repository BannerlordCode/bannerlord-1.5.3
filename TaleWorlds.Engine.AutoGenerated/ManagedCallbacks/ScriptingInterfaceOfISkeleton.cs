using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x02000027 RID: 39
	internal class ScriptingInterfaceOfISkeleton : ISkeleton
	{
		// Token: 0x0600057A RID: 1402 RVA: 0x00018504 File Offset: 0x00016704
		public void ActivateRagdoll(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfISkeleton.call_ActivateRagdollDelegate(skeletonPointer);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00018511 File Offset: 0x00016711
		public void AddComponent(UIntPtr skeletonPointer, UIntPtr componentPointer)
		{
			ScriptingInterfaceOfISkeleton.call_AddComponentDelegate(skeletonPointer, componentPointer);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00018520 File Offset: 0x00016720
		public void AddComponentToBone(UIntPtr skeletonPointer, sbyte boneIndex, GameEntityComponent component)
		{
			UIntPtr uintPtr = ((component != null) ? component.Pointer : UIntPtr.Zero);
			ScriptingInterfaceOfISkeleton.call_AddComponentToBoneDelegate(skeletonPointer, boneIndex, uintPtr);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00018551 File Offset: 0x00016751
		public void AddMesh(UIntPtr skeletonPointer, UIntPtr mesnPointer)
		{
			ScriptingInterfaceOfISkeleton.call_AddMeshDelegate(skeletonPointer, mesnPointer);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001855F File Offset: 0x0001675F
		public void AddMeshToBone(UIntPtr skeletonPointer, UIntPtr multiMeshPointer, sbyte bone_index)
		{
			ScriptingInterfaceOfISkeleton.call_AddMeshToBoneDelegate(skeletonPointer, multiMeshPointer, bone_index);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00018570 File Offset: 0x00016770
		public void AddPrefabEntityToBone(UIntPtr skeletonPointer, string prefab_name, sbyte boneIndex)
		{
			byte[] array = null;
			if (prefab_name != null)
			{
				int byteCount = ScriptingInterfaceOfISkeleton._utf8.GetByteCount(prefab_name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISkeleton._utf8.GetBytes(prefab_name, 0, prefab_name.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfISkeleton.call_AddPrefabEntityToBoneDelegate(skeletonPointer, array, boneIndex);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x000185CC File Offset: 0x000167CC
		public void ClearComponents(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfISkeleton.call_ClearComponentsDelegate(skeletonPointer);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x000185D9 File Offset: 0x000167D9
		public void ClearMeshes(UIntPtr skeletonPointer, bool clearBoneComponents)
		{
			ScriptingInterfaceOfISkeleton.call_ClearMeshesDelegate(skeletonPointer, clearBoneComponents);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x000185E7 File Offset: 0x000167E7
		public void ClearMeshesAtBone(UIntPtr skeletonPointer, sbyte boneIndex)
		{
			ScriptingInterfaceOfISkeleton.call_ClearMeshesAtBoneDelegate(skeletonPointer, boneIndex);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x000185F8 File Offset: 0x000167F8
		public Skeleton CreateFromModel(string skeletonModelName)
		{
			byte[] array = null;
			if (skeletonModelName != null)
			{
				int byteCount = ScriptingInterfaceOfISkeleton._utf8.GetByteCount(skeletonModelName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISkeleton._utf8.GetBytes(skeletonModelName, 0, skeletonModelName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfISkeleton.call_CreateFromModelDelegate(array);
			Skeleton skeleton = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				skeleton = new Skeleton(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return skeleton;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00018684 File Offset: 0x00016884
		public Skeleton CreateFromModelWithNullAnimTree(UIntPtr entityPointer, string skeletonModelName, float scale)
		{
			byte[] array = null;
			if (skeletonModelName != null)
			{
				int byteCount = ScriptingInterfaceOfISkeleton._utf8.GetByteCount(skeletonModelName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISkeleton._utf8.GetBytes(skeletonModelName, 0, skeletonModelName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfISkeleton.call_CreateFromModelWithNullAnimTreeDelegate(entityPointer, array, scale);
			Skeleton skeleton = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				skeleton = new Skeleton(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return skeleton;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00018712 File Offset: 0x00016912
		public void EnableScriptDrivenPostIntegrateCallback(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfISkeleton.call_EnableScriptDrivenPostIntegrateCallbackDelegate(skeletonPointer);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001871F File Offset: 0x0001691F
		public void ForceUpdateBoneFrames(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfISkeleton.call_ForceUpdateBoneFramesDelegate(skeletonPointer);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001872C File Offset: 0x0001692C
		public void Freeze(UIntPtr skeletonPointer, bool isFrozen)
		{
			ScriptingInterfaceOfISkeleton.call_FreezeDelegate(skeletonPointer, isFrozen);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001873C File Offset: 0x0001693C
		public void GetAllMeshes(Skeleton skeleton, NativeObjectArray nativeObjectArray)
		{
			UIntPtr uintPtr = ((skeleton != null) ? skeleton.Pointer : UIntPtr.Zero);
			UIntPtr uintPtr2 = ((nativeObjectArray != null) ? nativeObjectArray.Pointer : UIntPtr.Zero);
			ScriptingInterfaceOfISkeleton.call_GetAllMeshesDelegate(uintPtr, uintPtr2);
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00018783 File Offset: 0x00016983
		public string GetAnimationAtChannel(UIntPtr skeletonPointer, int channelNo)
		{
			if (ScriptingInterfaceOfISkeleton.call_GetAnimationAtChannelDelegate(skeletonPointer, channelNo) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001879B File Offset: 0x0001699B
		public int GetAnimationIndexAtChannel(UIntPtr skeletonPointer, int channelNo)
		{
			return ScriptingInterfaceOfISkeleton.call_GetAnimationIndexAtChannelDelegate(skeletonPointer, channelNo);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x000187A9 File Offset: 0x000169A9
		public void GetBoneBody(UIntPtr skeletonPointer, sbyte boneIndex, ref CapsuleData data)
		{
			ScriptingInterfaceOfISkeleton.call_GetBoneBodyDelegate(skeletonPointer, boneIndex, ref data);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x000187B8 File Offset: 0x000169B8
		public sbyte GetBoneChildAtIndex(Skeleton skeleton, sbyte boneIndex, sbyte childIndex)
		{
			UIntPtr uintPtr = ((skeleton != null) ? skeleton.Pointer : UIntPtr.Zero);
			return ScriptingInterfaceOfISkeleton.call_GetBoneChildAtIndexDelegate(uintPtr, boneIndex, childIndex);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x000187EC File Offset: 0x000169EC
		public sbyte GetBoneChildCount(Skeleton skeleton, sbyte boneIndex)
		{
			UIntPtr uintPtr = ((skeleton != null) ? skeleton.Pointer : UIntPtr.Zero);
			return ScriptingInterfaceOfISkeleton.call_GetBoneChildCountDelegate(uintPtr, boneIndex);
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x0001881C File Offset: 0x00016A1C
		public GameEntityComponent GetBoneComponentAtIndex(UIntPtr skeletonPointer, sbyte boneIndex, int componentIndex)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfISkeleton.call_GetBoneComponentAtIndexDelegate(skeletonPointer, boneIndex, componentIndex);
			GameEntityComponent gameEntityComponent = NativeObject.CreateNativeObjectWrapper<GameEntityComponent>(nativeObjectPointer);
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntityComponent;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x0001885F File Offset: 0x00016A5F
		public int GetBoneComponentCount(UIntPtr skeletonPointer, sbyte boneIndex)
		{
			return ScriptingInterfaceOfISkeleton.call_GetBoneComponentCountDelegate(skeletonPointer, boneIndex);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x0001886D File Offset: 0x00016A6D
		public sbyte GetBoneCount(UIntPtr skeletonPointer)
		{
			return ScriptingInterfaceOfISkeleton.call_GetBoneCountDelegate(skeletonPointer);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0001887A File Offset: 0x00016A7A
		public void GetBoneEntitialFrame(UIntPtr skeletonPointer, sbyte boneIndex, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfISkeleton.call_GetBoneEntitialFrameDelegate(skeletonPointer, boneIndex, ref outFrame);
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00018889 File Offset: 0x00016A89
		public void GetBoneEntitialFrameAtChannel(UIntPtr skeletonPointer, int channelNo, sbyte boneIndex, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfISkeleton.call_GetBoneEntitialFrameAtChannelDelegate(skeletonPointer, channelNo, boneIndex, ref outFrame);
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001889A File Offset: 0x00016A9A
		public void GetBoneEntitialFrameWithIndex(UIntPtr skeletonPointer, sbyte boneIndex, ref MatrixFrame outEntitialFrame)
		{
			ScriptingInterfaceOfISkeleton.call_GetBoneEntitialFrameWithIndexDelegate(skeletonPointer, boneIndex, ref outEntitialFrame);
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x000188AC File Offset: 0x00016AAC
		public void GetBoneEntitialFrameWithName(UIntPtr skeletonPointer, string boneName, ref MatrixFrame outEntitialFrame)
		{
			byte[] array = null;
			if (boneName != null)
			{
				int byteCount = ScriptingInterfaceOfISkeleton._utf8.GetByteCount(boneName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISkeleton._utf8.GetBytes(boneName, 0, boneName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfISkeleton.call_GetBoneEntitialFrameWithNameDelegate(skeletonPointer, array, ref outEntitialFrame);
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00018908 File Offset: 0x00016B08
		public void GetBoneEntitialRestFrame(UIntPtr skeletonPointer, sbyte boneIndex, bool useBoneMapping, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfISkeleton.call_GetBoneEntitialRestFrameDelegate(skeletonPointer, boneIndex, useBoneMapping, ref outFrame);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x0001891C File Offset: 0x00016B1C
		public sbyte GetBoneIndexFromName(string skeletonModelName, string boneName)
		{
			byte[] array = null;
			if (skeletonModelName != null)
			{
				int byteCount = ScriptingInterfaceOfISkeleton._utf8.GetByteCount(skeletonModelName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISkeleton._utf8.GetBytes(skeletonModelName, 0, skeletonModelName.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (boneName != null)
			{
				int byteCount2 = ScriptingInterfaceOfISkeleton._utf8.GetByteCount(boneName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfISkeleton._utf8.GetBytes(boneName, 0, boneName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			return ScriptingInterfaceOfISkeleton.call_GetBoneIndexFromNameDelegate(array, array2);
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x000189B9 File Offset: 0x00016BB9
		public void GetBoneLocalRestFrame(UIntPtr skeletonPointer, sbyte boneIndex, bool useBoneMapping, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfISkeleton.call_GetBoneLocalRestFrameDelegate(skeletonPointer, boneIndex, useBoneMapping, ref outFrame);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x000189CC File Offset: 0x00016BCC
		public string GetBoneName(Skeleton skeleton, sbyte boneIndex)
		{
			UIntPtr uintPtr = ((skeleton != null) ? skeleton.Pointer : UIntPtr.Zero);
			if (ScriptingInterfaceOfISkeleton.call_GetBoneNameDelegate(uintPtr, boneIndex) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00018A08 File Offset: 0x00016C08
		public GameEntityComponent GetComponentAtIndex(UIntPtr skeletonPointer, GameEntity.ComponentType componentType, int index)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfISkeleton.call_GetComponentAtIndexDelegate(skeletonPointer, componentType, index);
			GameEntityComponent gameEntityComponent = NativeObject.CreateNativeObjectWrapper<GameEntityComponent>(nativeObjectPointer);
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntityComponent;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00018A4B File Offset: 0x00016C4B
		public int GetComponentCount(UIntPtr skeletonPointer, GameEntity.ComponentType componentType)
		{
			return ScriptingInterfaceOfISkeleton.call_GetComponentCountDelegate(skeletonPointer, componentType);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00018A59 File Offset: 0x00016C59
		public RagdollState GetCurrentRagdollState(UIntPtr skeletonPointer)
		{
			return ScriptingInterfaceOfISkeleton.call_GetCurrentRagdollStateDelegate(skeletonPointer);
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x00018A66 File Offset: 0x00016C66
		public Transformation GetEntitialOutTransform(UIntPtr skeletonPointer, UIntPtr animResultPointer, sbyte boneIndex)
		{
			return ScriptingInterfaceOfISkeleton.call_GetEntitialOutTransformDelegate(skeletonPointer, animResultPointer, boneIndex);
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00018A78 File Offset: 0x00016C78
		public string GetName(Skeleton skeleton)
		{
			UIntPtr uintPtr = ((skeleton != null) ? skeleton.Pointer : UIntPtr.Zero);
			if (ScriptingInterfaceOfISkeleton.call_GetNameDelegate(uintPtr) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x00018AB4 File Offset: 0x00016CB4
		public sbyte GetParentBoneIndex(Skeleton skeleton, sbyte boneIndex)
		{
			UIntPtr uintPtr = ((skeleton != null) ? skeleton.Pointer : UIntPtr.Zero);
			return ScriptingInterfaceOfISkeleton.call_GetParentBoneIndexDelegate(uintPtr, boneIndex);
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x00018AE4 File Offset: 0x00016CE4
		public float GetSkeletonAnimationParameterAtChannel(UIntPtr skeletonPointer, int channelNo)
		{
			return ScriptingInterfaceOfISkeleton.call_GetSkeletonAnimationParameterAtChannelDelegate(skeletonPointer, channelNo);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00018AF2 File Offset: 0x00016CF2
		public float GetSkeletonAnimationSpeedAtChannel(UIntPtr skeletonPointer, int channelNo)
		{
			return ScriptingInterfaceOfISkeleton.call_GetSkeletonAnimationSpeedAtChannelDelegate(skeletonPointer, channelNo);
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00018B00 File Offset: 0x00016D00
		public sbyte GetSkeletonBoneMapping(UIntPtr skeletonPointer, sbyte boneIndex)
		{
			return ScriptingInterfaceOfISkeleton.call_GetSkeletonBoneMappingDelegate(skeletonPointer, boneIndex);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00018B10 File Offset: 0x00016D10
		public bool HasBoneComponent(UIntPtr skeletonPointer, sbyte boneIndex, GameEntityComponent component)
		{
			UIntPtr uintPtr = ((component != null) ? component.Pointer : UIntPtr.Zero);
			return ScriptingInterfaceOfISkeleton.call_HasBoneComponentDelegate(skeletonPointer, boneIndex, uintPtr);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x00018B41 File Offset: 0x00016D41
		public bool HasComponent(UIntPtr skeletonPointer, UIntPtr componentPointer)
		{
			return ScriptingInterfaceOfISkeleton.call_HasComponentDelegate(skeletonPointer, componentPointer);
		}

		// Token: 0x060005A4 RID: 1444 RVA: 0x00018B4F File Offset: 0x00016D4F
		public bool IsFrozen(UIntPtr skeletonPointer)
		{
			return ScriptingInterfaceOfISkeleton.call_IsFrozenDelegate(skeletonPointer);
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x00018B5C File Offset: 0x00016D5C
		public void RemoveBoneComponent(UIntPtr skeletonPointer, sbyte boneIndex, GameEntityComponent component)
		{
			UIntPtr uintPtr = ((component != null) ? component.Pointer : UIntPtr.Zero);
			ScriptingInterfaceOfISkeleton.call_RemoveBoneComponentDelegate(skeletonPointer, boneIndex, uintPtr);
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x00018B8D File Offset: 0x00016D8D
		public void RemoveComponent(UIntPtr SkeletonPointer, UIntPtr componentPointer)
		{
			ScriptingInterfaceOfISkeleton.call_RemoveComponentDelegate(SkeletonPointer, componentPointer);
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00018B9B File Offset: 0x00016D9B
		public void ResetCloths(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfISkeleton.call_ResetClothsDelegate(skeletonPointer);
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00018BA8 File Offset: 0x00016DA8
		public void ResetFrames(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfISkeleton.call_ResetFramesDelegate(skeletonPointer);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00018BB5 File Offset: 0x00016DB5
		public void SetBoneLocalFrame(UIntPtr skeletonPointer, sbyte boneIndex, ref MatrixFrame localFrame)
		{
			ScriptingInterfaceOfISkeleton.call_SetBoneLocalFrameDelegate(skeletonPointer, boneIndex, ref localFrame);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x00018BC4 File Offset: 0x00016DC4
		public void SetOutBoneDisplacement(UIntPtr skeletonPointer, UIntPtr animResultPointer, sbyte boneIndex, Vec3 displacement)
		{
			ScriptingInterfaceOfISkeleton.call_SetOutBoneDisplacementDelegate(skeletonPointer, animResultPointer, boneIndex, displacement);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00018BD5 File Offset: 0x00016DD5
		public void SetOutQuat(UIntPtr skeletonPointer, UIntPtr animResultPointer, sbyte boneIndex, Mat3 rotation)
		{
			ScriptingInterfaceOfISkeleton.call_SetOutQuatDelegate(skeletonPointer, animResultPointer, boneIndex, rotation);
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x00018BE6 File Offset: 0x00016DE6
		public void SetSkeletonAnimationParameterAtChannel(UIntPtr skeletonPointer, int channelNo, float parameter)
		{
			ScriptingInterfaceOfISkeleton.call_SetSkeletonAnimationParameterAtChannelDelegate(skeletonPointer, channelNo, parameter);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x00018BF5 File Offset: 0x00016DF5
		public void SetSkeletonAnimationSpeedAtChannel(UIntPtr skeletonPointer, int channelNo, float speed)
		{
			ScriptingInterfaceOfISkeleton.call_SetSkeletonAnimationSpeedAtChannelDelegate(skeletonPointer, channelNo, speed);
		}

		// Token: 0x060005AE RID: 1454 RVA: 0x00018C04 File Offset: 0x00016E04
		public void SetSkeletonUptoDate(UIntPtr skeletonPointer, bool value)
		{
			ScriptingInterfaceOfISkeleton.call_SetSkeletonUptoDateDelegate(skeletonPointer, value);
		}

		// Token: 0x060005AF RID: 1455 RVA: 0x00018C12 File Offset: 0x00016E12
		public void SetUsePreciseBoundingVolume(UIntPtr skeletonPointer, bool value)
		{
			ScriptingInterfaceOfISkeleton.call_SetUsePreciseBoundingVolumeDelegate(skeletonPointer, value);
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00018C20 File Offset: 0x00016E20
		public bool SkeletonModelExist(string skeletonModelName)
		{
			byte[] array = null;
			if (skeletonModelName != null)
			{
				int byteCount = ScriptingInterfaceOfISkeleton._utf8.GetByteCount(skeletonModelName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfISkeleton._utf8.GetBytes(skeletonModelName, 0, skeletonModelName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfISkeleton.call_SkeletonModelExistDelegate(array);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00018C7A File Offset: 0x00016E7A
		public void TickAnimations(UIntPtr skeletonPointer, ref MatrixFrame globalFrame, float dt, bool tickAnimsForChildren)
		{
			ScriptingInterfaceOfISkeleton.call_TickAnimationsDelegate(skeletonPointer, ref globalFrame, dt, tickAnimsForChildren);
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00018C8B File Offset: 0x00016E8B
		public void TickAnimationsAndForceUpdate(UIntPtr skeletonPointer, ref MatrixFrame globalFrame, float dt, bool tickAnimsForChildren)
		{
			ScriptingInterfaceOfISkeleton.call_TickAnimationsAndForceUpdateDelegate(skeletonPointer, ref globalFrame, dt, tickAnimsForChildren);
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00018C9C File Offset: 0x00016E9C
		public void UpdateEntitialFramesFromLocalFrames(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfISkeleton.call_UpdateEntitialFramesFromLocalFramesDelegate(skeletonPointer);
		}

		// Token: 0x040004D0 RID: 1232
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040004D1 RID: 1233
		public static ScriptingInterfaceOfISkeleton.ActivateRagdollDelegate call_ActivateRagdollDelegate;

		// Token: 0x040004D2 RID: 1234
		public static ScriptingInterfaceOfISkeleton.AddComponentDelegate call_AddComponentDelegate;

		// Token: 0x040004D3 RID: 1235
		public static ScriptingInterfaceOfISkeleton.AddComponentToBoneDelegate call_AddComponentToBoneDelegate;

		// Token: 0x040004D4 RID: 1236
		public static ScriptingInterfaceOfISkeleton.AddMeshDelegate call_AddMeshDelegate;

		// Token: 0x040004D5 RID: 1237
		public static ScriptingInterfaceOfISkeleton.AddMeshToBoneDelegate call_AddMeshToBoneDelegate;

		// Token: 0x040004D6 RID: 1238
		public static ScriptingInterfaceOfISkeleton.AddPrefabEntityToBoneDelegate call_AddPrefabEntityToBoneDelegate;

		// Token: 0x040004D7 RID: 1239
		public static ScriptingInterfaceOfISkeleton.ClearComponentsDelegate call_ClearComponentsDelegate;

		// Token: 0x040004D8 RID: 1240
		public static ScriptingInterfaceOfISkeleton.ClearMeshesDelegate call_ClearMeshesDelegate;

		// Token: 0x040004D9 RID: 1241
		public static ScriptingInterfaceOfISkeleton.ClearMeshesAtBoneDelegate call_ClearMeshesAtBoneDelegate;

		// Token: 0x040004DA RID: 1242
		public static ScriptingInterfaceOfISkeleton.CreateFromModelDelegate call_CreateFromModelDelegate;

		// Token: 0x040004DB RID: 1243
		public static ScriptingInterfaceOfISkeleton.CreateFromModelWithNullAnimTreeDelegate call_CreateFromModelWithNullAnimTreeDelegate;

		// Token: 0x040004DC RID: 1244
		public static ScriptingInterfaceOfISkeleton.EnableScriptDrivenPostIntegrateCallbackDelegate call_EnableScriptDrivenPostIntegrateCallbackDelegate;

		// Token: 0x040004DD RID: 1245
		public static ScriptingInterfaceOfISkeleton.ForceUpdateBoneFramesDelegate call_ForceUpdateBoneFramesDelegate;

		// Token: 0x040004DE RID: 1246
		public static ScriptingInterfaceOfISkeleton.FreezeDelegate call_FreezeDelegate;

		// Token: 0x040004DF RID: 1247
		public static ScriptingInterfaceOfISkeleton.GetAllMeshesDelegate call_GetAllMeshesDelegate;

		// Token: 0x040004E0 RID: 1248
		public static ScriptingInterfaceOfISkeleton.GetAnimationAtChannelDelegate call_GetAnimationAtChannelDelegate;

		// Token: 0x040004E1 RID: 1249
		public static ScriptingInterfaceOfISkeleton.GetAnimationIndexAtChannelDelegate call_GetAnimationIndexAtChannelDelegate;

		// Token: 0x040004E2 RID: 1250
		public static ScriptingInterfaceOfISkeleton.GetBoneBodyDelegate call_GetBoneBodyDelegate;

		// Token: 0x040004E3 RID: 1251
		public static ScriptingInterfaceOfISkeleton.GetBoneChildAtIndexDelegate call_GetBoneChildAtIndexDelegate;

		// Token: 0x040004E4 RID: 1252
		public static ScriptingInterfaceOfISkeleton.GetBoneChildCountDelegate call_GetBoneChildCountDelegate;

		// Token: 0x040004E5 RID: 1253
		public static ScriptingInterfaceOfISkeleton.GetBoneComponentAtIndexDelegate call_GetBoneComponentAtIndexDelegate;

		// Token: 0x040004E6 RID: 1254
		public static ScriptingInterfaceOfISkeleton.GetBoneComponentCountDelegate call_GetBoneComponentCountDelegate;

		// Token: 0x040004E7 RID: 1255
		public static ScriptingInterfaceOfISkeleton.GetBoneCountDelegate call_GetBoneCountDelegate;

		// Token: 0x040004E8 RID: 1256
		public static ScriptingInterfaceOfISkeleton.GetBoneEntitialFrameDelegate call_GetBoneEntitialFrameDelegate;

		// Token: 0x040004E9 RID: 1257
		public static ScriptingInterfaceOfISkeleton.GetBoneEntitialFrameAtChannelDelegate call_GetBoneEntitialFrameAtChannelDelegate;

		// Token: 0x040004EA RID: 1258
		public static ScriptingInterfaceOfISkeleton.GetBoneEntitialFrameWithIndexDelegate call_GetBoneEntitialFrameWithIndexDelegate;

		// Token: 0x040004EB RID: 1259
		public static ScriptingInterfaceOfISkeleton.GetBoneEntitialFrameWithNameDelegate call_GetBoneEntitialFrameWithNameDelegate;

		// Token: 0x040004EC RID: 1260
		public static ScriptingInterfaceOfISkeleton.GetBoneEntitialRestFrameDelegate call_GetBoneEntitialRestFrameDelegate;

		// Token: 0x040004ED RID: 1261
		public static ScriptingInterfaceOfISkeleton.GetBoneIndexFromNameDelegate call_GetBoneIndexFromNameDelegate;

		// Token: 0x040004EE RID: 1262
		public static ScriptingInterfaceOfISkeleton.GetBoneLocalRestFrameDelegate call_GetBoneLocalRestFrameDelegate;

		// Token: 0x040004EF RID: 1263
		public static ScriptingInterfaceOfISkeleton.GetBoneNameDelegate call_GetBoneNameDelegate;

		// Token: 0x040004F0 RID: 1264
		public static ScriptingInterfaceOfISkeleton.GetComponentAtIndexDelegate call_GetComponentAtIndexDelegate;

		// Token: 0x040004F1 RID: 1265
		public static ScriptingInterfaceOfISkeleton.GetComponentCountDelegate call_GetComponentCountDelegate;

		// Token: 0x040004F2 RID: 1266
		public static ScriptingInterfaceOfISkeleton.GetCurrentRagdollStateDelegate call_GetCurrentRagdollStateDelegate;

		// Token: 0x040004F3 RID: 1267
		public static ScriptingInterfaceOfISkeleton.GetEntitialOutTransformDelegate call_GetEntitialOutTransformDelegate;

		// Token: 0x040004F4 RID: 1268
		public static ScriptingInterfaceOfISkeleton.GetNameDelegate call_GetNameDelegate;

		// Token: 0x040004F5 RID: 1269
		public static ScriptingInterfaceOfISkeleton.GetParentBoneIndexDelegate call_GetParentBoneIndexDelegate;

		// Token: 0x040004F6 RID: 1270
		public static ScriptingInterfaceOfISkeleton.GetSkeletonAnimationParameterAtChannelDelegate call_GetSkeletonAnimationParameterAtChannelDelegate;

		// Token: 0x040004F7 RID: 1271
		public static ScriptingInterfaceOfISkeleton.GetSkeletonAnimationSpeedAtChannelDelegate call_GetSkeletonAnimationSpeedAtChannelDelegate;

		// Token: 0x040004F8 RID: 1272
		public static ScriptingInterfaceOfISkeleton.GetSkeletonBoneMappingDelegate call_GetSkeletonBoneMappingDelegate;

		// Token: 0x040004F9 RID: 1273
		public static ScriptingInterfaceOfISkeleton.HasBoneComponentDelegate call_HasBoneComponentDelegate;

		// Token: 0x040004FA RID: 1274
		public static ScriptingInterfaceOfISkeleton.HasComponentDelegate call_HasComponentDelegate;

		// Token: 0x040004FB RID: 1275
		public static ScriptingInterfaceOfISkeleton.IsFrozenDelegate call_IsFrozenDelegate;

		// Token: 0x040004FC RID: 1276
		public static ScriptingInterfaceOfISkeleton.RemoveBoneComponentDelegate call_RemoveBoneComponentDelegate;

		// Token: 0x040004FD RID: 1277
		public static ScriptingInterfaceOfISkeleton.RemoveComponentDelegate call_RemoveComponentDelegate;

		// Token: 0x040004FE RID: 1278
		public static ScriptingInterfaceOfISkeleton.ResetClothsDelegate call_ResetClothsDelegate;

		// Token: 0x040004FF RID: 1279
		public static ScriptingInterfaceOfISkeleton.ResetFramesDelegate call_ResetFramesDelegate;

		// Token: 0x04000500 RID: 1280
		public static ScriptingInterfaceOfISkeleton.SetBoneLocalFrameDelegate call_SetBoneLocalFrameDelegate;

		// Token: 0x04000501 RID: 1281
		public static ScriptingInterfaceOfISkeleton.SetOutBoneDisplacementDelegate call_SetOutBoneDisplacementDelegate;

		// Token: 0x04000502 RID: 1282
		public static ScriptingInterfaceOfISkeleton.SetOutQuatDelegate call_SetOutQuatDelegate;

		// Token: 0x04000503 RID: 1283
		public static ScriptingInterfaceOfISkeleton.SetSkeletonAnimationParameterAtChannelDelegate call_SetSkeletonAnimationParameterAtChannelDelegate;

		// Token: 0x04000504 RID: 1284
		public static ScriptingInterfaceOfISkeleton.SetSkeletonAnimationSpeedAtChannelDelegate call_SetSkeletonAnimationSpeedAtChannelDelegate;

		// Token: 0x04000505 RID: 1285
		public static ScriptingInterfaceOfISkeleton.SetSkeletonUptoDateDelegate call_SetSkeletonUptoDateDelegate;

		// Token: 0x04000506 RID: 1286
		public static ScriptingInterfaceOfISkeleton.SetUsePreciseBoundingVolumeDelegate call_SetUsePreciseBoundingVolumeDelegate;

		// Token: 0x04000507 RID: 1287
		public static ScriptingInterfaceOfISkeleton.SkeletonModelExistDelegate call_SkeletonModelExistDelegate;

		// Token: 0x04000508 RID: 1288
		public static ScriptingInterfaceOfISkeleton.TickAnimationsDelegate call_TickAnimationsDelegate;

		// Token: 0x04000509 RID: 1289
		public static ScriptingInterfaceOfISkeleton.TickAnimationsAndForceUpdateDelegate call_TickAnimationsAndForceUpdateDelegate;

		// Token: 0x0400050A RID: 1290
		public static ScriptingInterfaceOfISkeleton.UpdateEntitialFramesFromLocalFramesDelegate call_UpdateEntitialFramesFromLocalFramesDelegate;

		// Token: 0x02000539 RID: 1337
		// (Invoke) Token: 0x06001B0F RID: 6927
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ActivateRagdollDelegate(UIntPtr skeletonPointer);

		// Token: 0x0200053A RID: 1338
		// (Invoke) Token: 0x06001B13 RID: 6931
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddComponentDelegate(UIntPtr skeletonPointer, UIntPtr componentPointer);

		// Token: 0x0200053B RID: 1339
		// (Invoke) Token: 0x06001B17 RID: 6935
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddComponentToBoneDelegate(UIntPtr skeletonPointer, sbyte boneIndex, UIntPtr component);

		// Token: 0x0200053C RID: 1340
		// (Invoke) Token: 0x06001B1B RID: 6939
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMeshDelegate(UIntPtr skeletonPointer, UIntPtr mesnPointer);

		// Token: 0x0200053D RID: 1341
		// (Invoke) Token: 0x06001B1F RID: 6943
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddMeshToBoneDelegate(UIntPtr skeletonPointer, UIntPtr multiMeshPointer, sbyte bone_index);

		// Token: 0x0200053E RID: 1342
		// (Invoke) Token: 0x06001B23 RID: 6947
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void AddPrefabEntityToBoneDelegate(UIntPtr skeletonPointer, byte[] prefab_name, sbyte boneIndex);

		// Token: 0x0200053F RID: 1343
		// (Invoke) Token: 0x06001B27 RID: 6951
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearComponentsDelegate(UIntPtr skeletonPointer);

		// Token: 0x02000540 RID: 1344
		// (Invoke) Token: 0x06001B2B RID: 6955
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearMeshesDelegate(UIntPtr skeletonPointer, [MarshalAs(UnmanagedType.U1)] bool clearBoneComponents);

		// Token: 0x02000541 RID: 1345
		// (Invoke) Token: 0x06001B2F RID: 6959
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ClearMeshesAtBoneDelegate(UIntPtr skeletonPointer, sbyte boneIndex);

		// Token: 0x02000542 RID: 1346
		// (Invoke) Token: 0x06001B33 RID: 6963
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateFromModelDelegate(byte[] skeletonModelName);

		// Token: 0x02000543 RID: 1347
		// (Invoke) Token: 0x06001B37 RID: 6967
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateFromModelWithNullAnimTreeDelegate(UIntPtr entityPointer, byte[] skeletonModelName, float scale);

		// Token: 0x02000544 RID: 1348
		// (Invoke) Token: 0x06001B3B RID: 6971
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void EnableScriptDrivenPostIntegrateCallbackDelegate(UIntPtr skeletonPointer);

		// Token: 0x02000545 RID: 1349
		// (Invoke) Token: 0x06001B3F RID: 6975
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ForceUpdateBoneFramesDelegate(UIntPtr skeletonPointer);

		// Token: 0x02000546 RID: 1350
		// (Invoke) Token: 0x06001B43 RID: 6979
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void FreezeDelegate(UIntPtr skeletonPointer, [MarshalAs(UnmanagedType.U1)] bool isFrozen);

		// Token: 0x02000547 RID: 1351
		// (Invoke) Token: 0x06001B47 RID: 6983
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetAllMeshesDelegate(UIntPtr skeleton, UIntPtr nativeObjectArray);

		// Token: 0x02000548 RID: 1352
		// (Invoke) Token: 0x06001B4B RID: 6987
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAnimationAtChannelDelegate(UIntPtr skeletonPointer, int channelNo);

		// Token: 0x02000549 RID: 1353
		// (Invoke) Token: 0x06001B4F RID: 6991
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAnimationIndexAtChannelDelegate(UIntPtr skeletonPointer, int channelNo);

		// Token: 0x0200054A RID: 1354
		// (Invoke) Token: 0x06001B53 RID: 6995
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneBodyDelegate(UIntPtr skeletonPointer, sbyte boneIndex, ref CapsuleData data);

		// Token: 0x0200054B RID: 1355
		// (Invoke) Token: 0x06001B57 RID: 6999
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate sbyte GetBoneChildAtIndexDelegate(UIntPtr skeleton, sbyte boneIndex, sbyte childIndex);

		// Token: 0x0200054C RID: 1356
		// (Invoke) Token: 0x06001B5B RID: 7003
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate sbyte GetBoneChildCountDelegate(UIntPtr skeleton, sbyte boneIndex);

		// Token: 0x0200054D RID: 1357
		// (Invoke) Token: 0x06001B5F RID: 7007
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetBoneComponentAtIndexDelegate(UIntPtr skeletonPointer, sbyte boneIndex, int componentIndex);

		// Token: 0x0200054E RID: 1358
		// (Invoke) Token: 0x06001B63 RID: 7011
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetBoneComponentCountDelegate(UIntPtr skeletonPointer, sbyte boneIndex);

		// Token: 0x0200054F RID: 1359
		// (Invoke) Token: 0x06001B67 RID: 7015
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate sbyte GetBoneCountDelegate(UIntPtr skeletonPointer);

		// Token: 0x02000550 RID: 1360
		// (Invoke) Token: 0x06001B6B RID: 7019
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialFrameDelegate(UIntPtr skeletonPointer, sbyte boneIndex, ref MatrixFrame outFrame);

		// Token: 0x02000551 RID: 1361
		// (Invoke) Token: 0x06001B6F RID: 7023
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialFrameAtChannelDelegate(UIntPtr skeletonPointer, int channelNo, sbyte boneIndex, ref MatrixFrame outFrame);

		// Token: 0x02000552 RID: 1362
		// (Invoke) Token: 0x06001B73 RID: 7027
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialFrameWithIndexDelegate(UIntPtr skeletonPointer, sbyte boneIndex, ref MatrixFrame outEntitialFrame);

		// Token: 0x02000553 RID: 1363
		// (Invoke) Token: 0x06001B77 RID: 7031
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialFrameWithNameDelegate(UIntPtr skeletonPointer, byte[] boneName, ref MatrixFrame outEntitialFrame);

		// Token: 0x02000554 RID: 1364
		// (Invoke) Token: 0x06001B7B RID: 7035
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialRestFrameDelegate(UIntPtr skeletonPointer, sbyte boneIndex, [MarshalAs(UnmanagedType.U1)] bool useBoneMapping, ref MatrixFrame outFrame);

		// Token: 0x02000555 RID: 1365
		// (Invoke) Token: 0x06001B7F RID: 7039
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate sbyte GetBoneIndexFromNameDelegate(byte[] skeletonModelName, byte[] boneName);

		// Token: 0x02000556 RID: 1366
		// (Invoke) Token: 0x06001B83 RID: 7043
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneLocalRestFrameDelegate(UIntPtr skeletonPointer, sbyte boneIndex, [MarshalAs(UnmanagedType.U1)] bool useBoneMapping, ref MatrixFrame outFrame);

		// Token: 0x02000557 RID: 1367
		// (Invoke) Token: 0x06001B87 RID: 7047
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetBoneNameDelegate(UIntPtr skeleton, sbyte boneIndex);

		// Token: 0x02000558 RID: 1368
		// (Invoke) Token: 0x06001B8B RID: 7051
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetComponentAtIndexDelegate(UIntPtr skeletonPointer, GameEntity.ComponentType componentType, int index);

		// Token: 0x02000559 RID: 1369
		// (Invoke) Token: 0x06001B8F RID: 7055
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetComponentCountDelegate(UIntPtr skeletonPointer, GameEntity.ComponentType componentType);

		// Token: 0x0200055A RID: 1370
		// (Invoke) Token: 0x06001B93 RID: 7059
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate RagdollState GetCurrentRagdollStateDelegate(UIntPtr skeletonPointer);

		// Token: 0x0200055B RID: 1371
		// (Invoke) Token: 0x06001B97 RID: 7063
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Transformation GetEntitialOutTransformDelegate(UIntPtr skeletonPointer, UIntPtr animResultPointer, sbyte boneIndex);

		// Token: 0x0200055C RID: 1372
		// (Invoke) Token: 0x06001B9B RID: 7067
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNameDelegate(UIntPtr skeleton);

		// Token: 0x0200055D RID: 1373
		// (Invoke) Token: 0x06001B9F RID: 7071
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate sbyte GetParentBoneIndexDelegate(UIntPtr skeleton, sbyte boneIndex);

		// Token: 0x0200055E RID: 1374
		// (Invoke) Token: 0x06001BA3 RID: 7075
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetSkeletonAnimationParameterAtChannelDelegate(UIntPtr skeletonPointer, int channelNo);

		// Token: 0x0200055F RID: 1375
		// (Invoke) Token: 0x06001BA7 RID: 7079
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetSkeletonAnimationSpeedAtChannelDelegate(UIntPtr skeletonPointer, int channelNo);

		// Token: 0x02000560 RID: 1376
		// (Invoke) Token: 0x06001BAB RID: 7083
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate sbyte GetSkeletonBoneMappingDelegate(UIntPtr skeletonPointer, sbyte boneIndex);

		// Token: 0x02000561 RID: 1377
		// (Invoke) Token: 0x06001BAF RID: 7087
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasBoneComponentDelegate(UIntPtr skeletonPointer, sbyte boneIndex, UIntPtr component);

		// Token: 0x02000562 RID: 1378
		// (Invoke) Token: 0x06001BB3 RID: 7091
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasComponentDelegate(UIntPtr skeletonPointer, UIntPtr componentPointer);

		// Token: 0x02000563 RID: 1379
		// (Invoke) Token: 0x06001BB7 RID: 7095
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsFrozenDelegate(UIntPtr skeletonPointer);

		// Token: 0x02000564 RID: 1380
		// (Invoke) Token: 0x06001BBB RID: 7099
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveBoneComponentDelegate(UIntPtr skeletonPointer, sbyte boneIndex, UIntPtr component);

		// Token: 0x02000565 RID: 1381
		// (Invoke) Token: 0x06001BBF RID: 7103
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RemoveComponentDelegate(UIntPtr SkeletonPointer, UIntPtr componentPointer);

		// Token: 0x02000566 RID: 1382
		// (Invoke) Token: 0x06001BC3 RID: 7107
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ResetClothsDelegate(UIntPtr skeletonPointer);

		// Token: 0x02000567 RID: 1383
		// (Invoke) Token: 0x06001BC7 RID: 7111
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void ResetFramesDelegate(UIntPtr skeletonPointer);

		// Token: 0x02000568 RID: 1384
		// (Invoke) Token: 0x06001BCB RID: 7115
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetBoneLocalFrameDelegate(UIntPtr skeletonPointer, sbyte boneIndex, ref MatrixFrame localFrame);

		// Token: 0x02000569 RID: 1385
		// (Invoke) Token: 0x06001BCF RID: 7119
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetOutBoneDisplacementDelegate(UIntPtr skeletonPointer, UIntPtr animResultPointer, sbyte boneIndex, Vec3 displacement);

		// Token: 0x0200056A RID: 1386
		// (Invoke) Token: 0x06001BD3 RID: 7123
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetOutQuatDelegate(UIntPtr skeletonPointer, UIntPtr animResultPointer, sbyte boneIndex, Mat3 rotation);

		// Token: 0x0200056B RID: 1387
		// (Invoke) Token: 0x06001BD7 RID: 7127
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSkeletonAnimationParameterAtChannelDelegate(UIntPtr skeletonPointer, int channelNo, float parameter);

		// Token: 0x0200056C RID: 1388
		// (Invoke) Token: 0x06001BDB RID: 7131
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSkeletonAnimationSpeedAtChannelDelegate(UIntPtr skeletonPointer, int channelNo, float speed);

		// Token: 0x0200056D RID: 1389
		// (Invoke) Token: 0x06001BDF RID: 7135
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSkeletonUptoDateDelegate(UIntPtr skeletonPointer, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200056E RID: 1390
		// (Invoke) Token: 0x06001BE3 RID: 7139
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetUsePreciseBoundingVolumeDelegate(UIntPtr skeletonPointer, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200056F RID: 1391
		// (Invoke) Token: 0x06001BE7 RID: 7143
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool SkeletonModelExistDelegate(byte[] skeletonModelName);

		// Token: 0x02000570 RID: 1392
		// (Invoke) Token: 0x06001BEB RID: 7147
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TickAnimationsDelegate(UIntPtr skeletonPointer, ref MatrixFrame globalFrame, float dt, [MarshalAs(UnmanagedType.U1)] bool tickAnimsForChildren);

		// Token: 0x02000571 RID: 1393
		// (Invoke) Token: 0x06001BEF RID: 7151
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TickAnimationsAndForceUpdateDelegate(UIntPtr skeletonPointer, ref MatrixFrame globalFrame, float dt, [MarshalAs(UnmanagedType.U1)] bool tickAnimsForChildren);

		// Token: 0x02000572 RID: 1394
		// (Invoke) Token: 0x06001BF3 RID: 7155
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void UpdateEntitialFramesFromLocalFramesDelegate(UIntPtr skeletonPointer);
	}
}
