using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000013 RID: 19
	[EngineClass("rglDecal")]
	public sealed class Decal : GameEntityComponent
	{
		// Token: 0x06000097 RID: 151 RVA: 0x000038D4 File Offset: 0x00001AD4
		internal Decal(UIntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x06000098 RID: 152 RVA: 0x000038DD File Offset: 0x00001ADD
		public static Decal CreateDecal(string name = null)
		{
			return EngineApplicationInterface.IDecal.CreateDecal(name);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x000038EA File Offset: 0x00001AEA
		public Decal CreateCopy()
		{
			return EngineApplicationInterface.IDecal.CreateCopy(base.Pointer);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000038FC File Offset: 0x00001AFC
		public void CheckAndRegisterToDecalSet()
		{
			EngineApplicationInterface.IDecal.CheckAndRegisterToDecalSet(base.Pointer);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x0000390E File Offset: 0x00001B0E
		public void SetIsVisible(bool value)
		{
			EngineApplicationInterface.IDecal.SetIsVisible(base.Pointer, value);
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00003921 File Offset: 0x00001B21
		public bool IsValid
		{
			get
			{
				return base.Pointer != UIntPtr.Zero;
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003933 File Offset: 0x00001B33
		public uint GetFactor1()
		{
			return EngineApplicationInterface.IDecal.GetFactor1(base.Pointer);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003945 File Offset: 0x00001B45
		public void OverrideRoadBoundaryP0(Vec2 data)
		{
			EngineApplicationInterface.IDecal.OverrideRoadBoundaryP0(base.Pointer, in data);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00003959 File Offset: 0x00001B59
		public void OverrideRoadBoundaryP1(Vec2 data)
		{
			EngineApplicationInterface.IDecal.OverrideRoadBoundaryP1(base.Pointer, in data);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000396D File Offset: 0x00001B6D
		public void SetFactor1Linear(uint linearFactorColor1)
		{
			EngineApplicationInterface.IDecal.SetFactor1Linear(base.Pointer, linearFactorColor1);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00003980 File Offset: 0x00001B80
		public void SetFactor1(uint factorColor1)
		{
			EngineApplicationInterface.IDecal.SetFactor1(base.Pointer, factorColor1);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00003993 File Offset: 0x00001B93
		public void SetAlpha(float alpha)
		{
			EngineApplicationInterface.IDecal.SetAlpha(base.Pointer, alpha);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000039A6 File Offset: 0x00001BA6
		public void SetVectorArgument(float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			EngineApplicationInterface.IDecal.SetVectorArgument(base.Pointer, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000039BD File Offset: 0x00001BBD
		public void SetVectorArgument2(float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			EngineApplicationInterface.IDecal.SetVectorArgument2(base.Pointer, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000039D4 File Offset: 0x00001BD4
		public Material GetMaterial()
		{
			return EngineApplicationInterface.IDecal.GetMaterial(base.Pointer);
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000039E6 File Offset: 0x00001BE6
		public void SetMaterial(Material material)
		{
			EngineApplicationInterface.IDecal.SetMaterial(base.Pointer, material.Pointer);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000039FE File Offset: 0x00001BFE
		public void SetFrame(MatrixFrame Frame)
		{
			this.Frame = Frame;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00003A08 File Offset: 0x00001C08
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x00003A30 File Offset: 0x00001C30
		public MatrixFrame Frame
		{
			get
			{
				MatrixFrame matrixFrame = default(MatrixFrame);
				EngineApplicationInterface.IDecal.GetFrame(base.Pointer, ref matrixFrame);
				return matrixFrame;
			}
			set
			{
				EngineApplicationInterface.IDecal.SetFrame(base.Pointer, ref value);
			}
		}
	}
}
