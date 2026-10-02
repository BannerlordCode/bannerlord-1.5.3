using System;
using System.Collections.Generic;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200008D RID: 141
	[EngineClass("rglSkeleton")]
	public sealed class Skeleton : NativeObject
	{
		// Token: 0x06000C7D RID: 3197 RVA: 0x0000DF14 File Offset: 0x0000C114
		internal Skeleton(UIntPtr pointer)
		{
			base.Construct(pointer);
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0000DF23 File Offset: 0x0000C123
		public static Skeleton CreateFromModel(string modelName)
		{
			return EngineApplicationInterface.ISkeleton.CreateFromModel(modelName);
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0000DF30 File Offset: 0x0000C130
		public static Skeleton CreateFromModelWithNullAnimTree(GameEntity entity, string modelName, float boneScale = 1f)
		{
			return EngineApplicationInterface.ISkeleton.CreateFromModelWithNullAnimTree(entity.Pointer, modelName, boneScale);
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x0000DF44 File Offset: 0x0000C144
		public bool IsValid
		{
			get
			{
				return base.Pointer != UIntPtr.Zero;
			}
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0000DF56 File Offset: 0x0000C156
		public string GetName()
		{
			return EngineApplicationInterface.ISkeleton.GetName(this);
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0000DF63 File Offset: 0x0000C163
		public string GetBoneName(sbyte boneIndex)
		{
			return EngineApplicationInterface.ISkeleton.GetBoneName(this, boneIndex);
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x0000DF71 File Offset: 0x0000C171
		public sbyte GetBoneChildAtIndex(sbyte boneIndex, sbyte childIndex)
		{
			return EngineApplicationInterface.ISkeleton.GetBoneChildAtIndex(this, boneIndex, childIndex);
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x0000DF80 File Offset: 0x0000C180
		public sbyte GetBoneChildCount(sbyte boneIndex)
		{
			return EngineApplicationInterface.ISkeleton.GetBoneChildCount(this, boneIndex);
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0000DF8E File Offset: 0x0000C18E
		public sbyte GetParentBoneIndex(sbyte boneIndex)
		{
			return EngineApplicationInterface.ISkeleton.GetParentBoneIndex(this, boneIndex);
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x0000DF9C File Offset: 0x0000C19C
		public void AddMeshToBone(UIntPtr mesh, sbyte boneIndex)
		{
			EngineApplicationInterface.ISkeleton.AddMeshToBone(base.Pointer, mesh, boneIndex);
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x0000DFB0 File Offset: 0x0000C1B0
		public void Freeze(bool p)
		{
			EngineApplicationInterface.ISkeleton.Freeze(base.Pointer, p);
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0000DFC3 File Offset: 0x0000C1C3
		public bool IsFrozen()
		{
			return EngineApplicationInterface.ISkeleton.IsFrozen(base.Pointer);
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0000DFD5 File Offset: 0x0000C1D5
		public void SetBoneLocalFrame(sbyte boneIndex, MatrixFrame localFrame)
		{
			EngineApplicationInterface.ISkeleton.SetBoneLocalFrame(base.Pointer, boneIndex, ref localFrame);
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0000DFEA File Offset: 0x0000C1EA
		public sbyte GetBoneCount()
		{
			return EngineApplicationInterface.ISkeleton.GetBoneCount(base.Pointer);
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0000DFFC File Offset: 0x0000C1FC
		public void GetBoneBody(sbyte boneIndex, ref CapsuleData data)
		{
			EngineApplicationInterface.ISkeleton.GetBoneBody(base.Pointer, boneIndex, ref data);
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0000E010 File Offset: 0x0000C210
		public static bool SkeletonModelExist(string skeletonModelName)
		{
			return EngineApplicationInterface.ISkeleton.SkeletonModelExist(skeletonModelName);
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0000E01D File Offset: 0x0000C21D
		public void ForceUpdateBoneFrames()
		{
			EngineApplicationInterface.ISkeleton.ForceUpdateBoneFrames(base.Pointer);
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x0000E030 File Offset: 0x0000C230
		public MatrixFrame GetBoneEntitialFrameWithIndex(sbyte boneIndex)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			EngineApplicationInterface.ISkeleton.GetBoneEntitialFrameWithIndex(base.Pointer, boneIndex, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0000E05C File Offset: 0x0000C25C
		public MatrixFrame GetBoneEntitialFrameWithName(string boneName)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			EngineApplicationInterface.ISkeleton.GetBoneEntitialFrameWithName(base.Pointer, boneName, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000C90 RID: 3216 RVA: 0x0000E085 File Offset: 0x0000C285
		public RagdollState GetCurrentRagdollState()
		{
			return EngineApplicationInterface.ISkeleton.GetCurrentRagdollState(base.Pointer);
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x0000E097 File Offset: 0x0000C297
		public void ActivateRagdoll()
		{
			EngineApplicationInterface.ISkeleton.ActivateRagdoll(base.Pointer);
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0000E0A9 File Offset: 0x0000C2A9
		public sbyte GetSkeletonBoneMapping(sbyte boneIndex)
		{
			return EngineApplicationInterface.ISkeleton.GetSkeletonBoneMapping(base.Pointer, boneIndex);
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0000E0BC File Offset: 0x0000C2BC
		public void AddMesh(Mesh mesh)
		{
			EngineApplicationInterface.ISkeleton.AddMesh(base.Pointer, mesh.Pointer);
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0000E0D4 File Offset: 0x0000C2D4
		public void ClearComponents()
		{
			EngineApplicationInterface.ISkeleton.ClearComponents(base.Pointer);
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0000E0E6 File Offset: 0x0000C2E6
		public void AddComponent(GameEntityComponent component)
		{
			EngineApplicationInterface.ISkeleton.AddComponent(base.Pointer, component.Pointer);
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0000E0FE File Offset: 0x0000C2FE
		public bool HasComponent(GameEntityComponent component)
		{
			return EngineApplicationInterface.ISkeleton.HasComponent(base.Pointer, component.Pointer);
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0000E116 File Offset: 0x0000C316
		public void RemoveComponent(GameEntityComponent component)
		{
			EngineApplicationInterface.ISkeleton.RemoveComponent(base.Pointer, component.Pointer);
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0000E12E File Offset: 0x0000C32E
		public void ClearMeshes(bool clearBoneComponents = true)
		{
			EngineApplicationInterface.ISkeleton.ClearMeshes(base.Pointer, clearBoneComponents);
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0000E141 File Offset: 0x0000C341
		public int GetComponentCount(GameEntity.ComponentType componentType)
		{
			return EngineApplicationInterface.ISkeleton.GetComponentCount(base.Pointer, componentType);
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0000E154 File Offset: 0x0000C354
		public void UpdateEntitialFramesFromLocalFrames()
		{
			EngineApplicationInterface.ISkeleton.UpdateEntitialFramesFromLocalFrames(base.Pointer);
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x0000E166 File Offset: 0x0000C366
		public void ResetFrames()
		{
			EngineApplicationInterface.ISkeleton.ResetFrames(base.Pointer);
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0000E178 File Offset: 0x0000C378
		public GameEntityComponent GetComponentAtIndex(GameEntity.ComponentType componentType, int index)
		{
			return EngineApplicationInterface.ISkeleton.GetComponentAtIndex(base.Pointer, componentType, index);
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x0000E18C File Offset: 0x0000C38C
		public void SetUsePreciseBoundingVolume(bool value)
		{
			EngineApplicationInterface.ISkeleton.SetUsePreciseBoundingVolume(base.Pointer, value);
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x0000E1A0 File Offset: 0x0000C3A0
		public MatrixFrame GetBoneEntitialRestFrame(sbyte boneIndex, bool useBoneMapping)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			EngineApplicationInterface.ISkeleton.GetBoneEntitialRestFrame(base.Pointer, boneIndex, useBoneMapping, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000C9F RID: 3231 RVA: 0x0000E1CC File Offset: 0x0000C3CC
		public MatrixFrame GetBoneLocalRestFrame(sbyte boneIndex, bool useBoneMapping = true)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			EngineApplicationInterface.ISkeleton.GetBoneLocalRestFrame(base.Pointer, boneIndex, useBoneMapping, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0000E1F8 File Offset: 0x0000C3F8
		public MatrixFrame GetBoneEntitialRestFrame(sbyte boneIndex)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			EngineApplicationInterface.ISkeleton.GetBoneEntitialRestFrame(base.Pointer, boneIndex, true, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0000E224 File Offset: 0x0000C424
		public MatrixFrame GetBoneEntitialFrameAtChannel(int channelNo, sbyte boneIndex)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			EngineApplicationInterface.ISkeleton.GetBoneEntitialFrameAtChannel(base.Pointer, channelNo, boneIndex, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0000E250 File Offset: 0x0000C450
		public MatrixFrame GetBoneEntitialFrame(sbyte boneIndex)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			EngineApplicationInterface.ISkeleton.GetBoneEntitialFrame(base.Pointer, boneIndex, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0000E279 File Offset: 0x0000C479
		public int GetBoneComponentCount(sbyte boneIndex)
		{
			return EngineApplicationInterface.ISkeleton.GetBoneComponentCount(base.Pointer, boneIndex);
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0000E28C File Offset: 0x0000C48C
		public GameEntityComponent GetBoneComponentAtIndex(sbyte boneIndex, int componentIndex)
		{
			return EngineApplicationInterface.ISkeleton.GetBoneComponentAtIndex(base.Pointer, boneIndex, componentIndex);
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0000E2A0 File Offset: 0x0000C4A0
		public bool HasBoneComponent(sbyte boneIndex, GameEntityComponent component)
		{
			return EngineApplicationInterface.ISkeleton.HasBoneComponent(base.Pointer, boneIndex, component);
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0000E2B4 File Offset: 0x0000C4B4
		public void AddComponentToBone(sbyte boneIndex, GameEntityComponent component)
		{
			EngineApplicationInterface.ISkeleton.AddComponentToBone(base.Pointer, boneIndex, component);
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0000E2C8 File Offset: 0x0000C4C8
		public void RemoveBoneComponent(sbyte boneIndex, GameEntityComponent component)
		{
			EngineApplicationInterface.ISkeleton.RemoveBoneComponent(base.Pointer, boneIndex, component);
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0000E2DC File Offset: 0x0000C4DC
		public void ClearMeshesAtBone(sbyte boneIndex)
		{
			EngineApplicationInterface.ISkeleton.ClearMeshesAtBone(base.Pointer, boneIndex);
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0000E2EF File Offset: 0x0000C4EF
		public void TickAnimations(float dt, MatrixFrame globalFrame, bool tickAnimsForChildren)
		{
			EngineApplicationInterface.ISkeleton.TickAnimations(base.Pointer, ref globalFrame, dt, tickAnimsForChildren);
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0000E305 File Offset: 0x0000C505
		public void TickAnimationsAndForceUpdate(float dt, MatrixFrame globalFrame, bool tickAnimsForChildren)
		{
			EngineApplicationInterface.ISkeleton.TickAnimationsAndForceUpdate(base.Pointer, ref globalFrame, dt, tickAnimsForChildren);
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0000E31B File Offset: 0x0000C51B
		public float GetAnimationParameterAtChannel(int channelNo)
		{
			return EngineApplicationInterface.ISkeleton.GetSkeletonAnimationParameterAtChannel(base.Pointer, channelNo);
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0000E32E File Offset: 0x0000C52E
		public void SetAnimationParameterAtChannel(int channelNo, float parameter)
		{
			EngineApplicationInterface.ISkeleton.SetSkeletonAnimationParameterAtChannel(base.Pointer, channelNo, parameter);
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x0000E342 File Offset: 0x0000C542
		public float GetAnimationSpeedAtChannel(int channelNo)
		{
			return EngineApplicationInterface.ISkeleton.GetSkeletonAnimationSpeedAtChannel(base.Pointer, channelNo);
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0000E355 File Offset: 0x0000C555
		public void SetAnimationSpeedAtChannel(int channelNo, float speed)
		{
			EngineApplicationInterface.ISkeleton.SetSkeletonAnimationSpeedAtChannel(base.Pointer, channelNo, speed);
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x0000E369 File Offset: 0x0000C569
		public void SetUptoDate(bool value)
		{
			EngineApplicationInterface.ISkeleton.SetSkeletonUptoDate(base.Pointer, value);
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0000E37C File Offset: 0x0000C57C
		public string GetAnimationAtChannel(int channelNo)
		{
			return EngineApplicationInterface.ISkeleton.GetAnimationAtChannel(base.Pointer, channelNo);
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0000E38F File Offset: 0x0000C58F
		public int GetAnimationIndexAtChannel(int channelNo)
		{
			return EngineApplicationInterface.ISkeleton.GetAnimationIndexAtChannel(base.Pointer, channelNo);
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0000E3A2 File Offset: 0x0000C5A2
		public void EnableScriptDrivenPostIntegrateCallback()
		{
			EngineApplicationInterface.ISkeleton.EnableScriptDrivenPostIntegrateCallback(base.Pointer);
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0000E3B4 File Offset: 0x0000C5B4
		public void ResetCloths()
		{
			EngineApplicationInterface.ISkeleton.ResetCloths(base.Pointer);
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0000E3C6 File Offset: 0x0000C5C6
		public IEnumerable<Mesh> GetAllMeshes()
		{
			NativeObjectArray nativeObjectArray = NativeObjectArray.Create();
			EngineApplicationInterface.ISkeleton.GetAllMeshes(this, nativeObjectArray);
			foreach (NativeObject nativeObject in ((IEnumerable<NativeObject>)nativeObjectArray))
			{
				Mesh mesh = (Mesh)nativeObject;
				yield return mesh;
			}
			IEnumerator<NativeObject> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0000E3D6 File Offset: 0x0000C5D6
		public static sbyte GetBoneIndexFromName(string skeletonModelName, string boneName)
		{
			return EngineApplicationInterface.ISkeleton.GetBoneIndexFromName(skeletonModelName, boneName);
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0000E3E4 File Offset: 0x0000C5E4
		internal Transformation GetEntitialOutTransform(UIntPtr animResultPointer, sbyte boneIndex)
		{
			return EngineApplicationInterface.ISkeleton.GetEntitialOutTransform(base.Pointer, animResultPointer, boneIndex);
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0000E3F8 File Offset: 0x0000C5F8
		internal void SetOutBoneDisplacement(UIntPtr animResultPointer, sbyte boneIndex, Vec3 displacement)
		{
			EngineApplicationInterface.ISkeleton.SetOutBoneDisplacement(base.Pointer, animResultPointer, boneIndex, displacement);
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0000E40D File Offset: 0x0000C60D
		internal void SetOutQuat(UIntPtr animResultPointer, sbyte boneIndex, Mat3 rotation)
		{
			EngineApplicationInterface.ISkeleton.SetOutQuat(base.Pointer, animResultPointer, boneIndex, rotation);
		}

		// Token: 0x040001C6 RID: 454
		public const sbyte MaxBoneCount = 64;
	}
}
