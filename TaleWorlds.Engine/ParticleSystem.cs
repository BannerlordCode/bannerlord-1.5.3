using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000073 RID: 115
	[EngineClass("rglParticle_system_instanced")]
	public sealed class ParticleSystem : GameEntityComponent
	{
		// Token: 0x06000A84 RID: 2692 RVA: 0x0000AAAE File Offset: 0x00008CAE
		internal ParticleSystem(UIntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x06000A85 RID: 2693 RVA: 0x0000AAB7 File Offset: 0x00008CB7
		public static ParticleSystem CreateParticleSystemAttachedToBone(string systemName, Skeleton skeleton, sbyte boneIndex, ref MatrixFrame boneLocalFrame)
		{
			return ParticleSystem.CreateParticleSystemAttachedToBone(ParticleSystemManager.GetRuntimeIdByName(systemName), skeleton, boneIndex, ref boneLocalFrame);
		}

		// Token: 0x06000A86 RID: 2694 RVA: 0x0000AAC7 File Offset: 0x00008CC7
		public static ParticleSystem CreateParticleSystemAttachedToBone(int systemRuntimeId, Skeleton skeleton, sbyte boneIndex, ref MatrixFrame boneLocalFrame)
		{
			return EngineApplicationInterface.IParticleSystem.CreateParticleSystemAttachedToBone(systemRuntimeId, skeleton.Pointer, boneIndex, ref boneLocalFrame);
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x0000AADC File Offset: 0x00008CDC
		public static ParticleSystem CreateParticleSystemAttachedToEntity(string systemName, GameEntity parentEntity, ref MatrixFrame boneLocalFrame)
		{
			return ParticleSystem.CreateParticleSystemAttachedToEntity(ParticleSystemManager.GetRuntimeIdByName(systemName), parentEntity, ref boneLocalFrame);
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x0000AAEB File Offset: 0x00008CEB
		public static ParticleSystem CreateParticleSystemAttachedToEntity(string systemName, WeakGameEntity parentEntity, ref MatrixFrame boneLocalFrame)
		{
			return ParticleSystem.CreateParticleSystemAttachedToEntity(ParticleSystemManager.GetRuntimeIdByName(systemName), parentEntity, ref boneLocalFrame);
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x0000AAFA File Offset: 0x00008CFA
		public static ParticleSystem CreateParticleSystemAttachedToEntity(int systemRuntimeId, GameEntity parentEntity, ref MatrixFrame boneLocalFrame)
		{
			return EngineApplicationInterface.IParticleSystem.CreateParticleSystemAttachedToEntity(systemRuntimeId, parentEntity.Pointer, ref boneLocalFrame);
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0000AB0E File Offset: 0x00008D0E
		public static ParticleSystem CreateParticleSystemAttachedToEntity(int systemRuntimeId, WeakGameEntity parentEntity, ref MatrixFrame boneLocalFrame)
		{
			return EngineApplicationInterface.IParticleSystem.CreateParticleSystemAttachedToEntity(systemRuntimeId, parentEntity.Pointer, ref boneLocalFrame);
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x0000AB23 File Offset: 0x00008D23
		public void AddMesh(Mesh mesh)
		{
			EngineApplicationInterface.IMetaMesh.AddMesh(base.Pointer, mesh.Pointer, 0U);
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x0000AB3C File Offset: 0x00008D3C
		public void SetEnable(bool enable)
		{
			EngineApplicationInterface.IParticleSystem.SetEnable(base.Pointer, enable);
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x0000AB4F File Offset: 0x00008D4F
		public void SetRuntimeEmissionRateMultiplier(float multiplier)
		{
			EngineApplicationInterface.IParticleSystem.SetRuntimeEmissionRateMultiplier(base.Pointer, multiplier);
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x0000AB62 File Offset: 0x00008D62
		public void Restart()
		{
			EngineApplicationInterface.IParticleSystem.Restart(base.Pointer);
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x0000AB74 File Offset: 0x00008D74
		public void SetLocalFrame(in MatrixFrame newLocalFrame)
		{
			EngineApplicationInterface.IParticleSystem.SetLocalFrame(base.Pointer, in newLocalFrame);
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x0000AB87 File Offset: 0x00008D87
		public void SetPreviousGlobalFrame(in MatrixFrame globalFrame)
		{
			EngineApplicationInterface.IParticleSystem.SetPreviousGlobalFrame(base.Pointer, in globalFrame);
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0000AB9C File Offset: 0x00008D9C
		public MatrixFrame GetLocalFrame()
		{
			MatrixFrame identity = MatrixFrame.Identity;
			EngineApplicationInterface.IParticleSystem.GetLocalFrame(base.Pointer, ref identity);
			return identity;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x0000ABC2 File Offset: 0x00008DC2
		public bool HasAliveParticles()
		{
			return EngineApplicationInterface.IParticleSystem.HasAliveParticles(base.Pointer);
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x0000ABD4 File Offset: 0x00008DD4
		public void SetDontRemoveFromEntity(bool value)
		{
			EngineApplicationInterface.IParticleSystem.SetDontRemoveFromEntity(base.Pointer, value);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x0000ABE7 File Offset: 0x00008DE7
		public void SetParticleEffectByName(string effectName)
		{
			EngineApplicationInterface.IParticleSystem.SetParticleEffectByName(base.Pointer, effectName);
		}
	}
}
