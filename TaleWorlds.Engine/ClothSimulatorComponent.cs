using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000010 RID: 16
	[EngineClass("rglCloth_simulator_component")]
	public sealed class ClothSimulatorComponent : GameEntityComponent
	{
		// Token: 0x0600006F RID: 111 RVA: 0x00003399 File Offset: 0x00001599
		internal ClothSimulatorComponent(UIntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000033A2 File Offset: 0x000015A2
		public void SetMaxDistanceMultiplier(float multiplier)
		{
			EngineApplicationInterface.IClothSimulatorComponent.SetMaxDistanceMultiplier(base.Pointer, multiplier);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000033B5 File Offset: 0x000015B5
		public void SetForcedWind(Vec3 windVector, bool isLocal)
		{
			EngineApplicationInterface.IClothSimulatorComponent.SetForcedWind(base.Pointer, windVector, isLocal);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000033C9 File Offset: 0x000015C9
		public void DisableForcedWind()
		{
			EngineApplicationInterface.IClothSimulatorComponent.DisableForcedWind(base.Pointer);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000033DB File Offset: 0x000015DB
		public void SetForcedGustStrength(float gustStrength)
		{
			EngineApplicationInterface.IClothSimulatorComponent.SetForcedGustStrength(base.Pointer, gustStrength);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000033EE File Offset: 0x000015EE
		public void SetResetRequired()
		{
			EngineApplicationInterface.IClothSimulatorComponent.SetResetRequired(base.Pointer);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00003400 File Offset: 0x00001600
		public void DisableMorphAnimation()
		{
			EngineApplicationInterface.IClothSimulatorComponent.DisableMorphAnimation(base.Pointer);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003412 File Offset: 0x00001612
		public void SetMorphBuffer(float morphKey)
		{
			EngineApplicationInterface.IClothSimulatorComponent.SetMorphAnimation(base.Pointer, morphKey);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003425 File Offset: 0x00001625
		public int GetNumberOfMorphKeys()
		{
			return EngineApplicationInterface.IClothSimulatorComponent.GetNumberOfMorphKeys(base.Pointer);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003437 File Offset: 0x00001637
		public void SetVectorArgument(float x, float y, float z, float w)
		{
			EngineApplicationInterface.IClothSimulatorComponent.SetVectorArgument(base.Pointer, x, y, z, w);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x0000344E File Offset: 0x0000164E
		public void GetMorphAnimLeftPoints(Vec3[] leftPoints)
		{
			EngineApplicationInterface.IClothSimulatorComponent.GetMorphAnimLeftPoints(base.Pointer, leftPoints);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x00003461 File Offset: 0x00001661
		public void GetMorphAnimRightPoints(Vec3[] rightPoints)
		{
			EngineApplicationInterface.IClothSimulatorComponent.GetMorphAnimRightPoints(base.Pointer, rightPoints);
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003474 File Offset: 0x00001674
		public void GetMorphAnimCenterPoints(Vec3[] centerPoints)
		{
			EngineApplicationInterface.IClothSimulatorComponent.GetMorphAnimCenterPoints(base.Pointer, centerPoints);
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00003487 File Offset: 0x00001687
		public void SetForcedVelocity(in Vec3 forcedVelocity)
		{
			EngineApplicationInterface.IClothSimulatorComponent.SetForcedVelocity(base.Pointer, in forcedVelocity);
		}
	}
}
