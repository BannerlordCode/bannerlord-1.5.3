using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x02000011 RID: 17
	[EngineClass("rglComposite_component")]
	public sealed class CompositeComponent : GameEntityComponent
	{
		// Token: 0x0600007D RID: 125 RVA: 0x0000349A File Offset: 0x0000169A
		internal CompositeComponent(UIntPtr pointer)
			: base(pointer)
		{
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000034A3 File Offset: 0x000016A3
		public bool IsValid
		{
			get
			{
				return base.Pointer != UIntPtr.Zero;
			}
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000034B5 File Offset: 0x000016B5
		public static bool IsNull(CompositeComponent component)
		{
			return component == null || component.Pointer == UIntPtr.Zero;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x000034D2 File Offset: 0x000016D2
		public static CompositeComponent CreateCompositeComponent()
		{
			return EngineApplicationInterface.ICompositeComponent.CreateCompositeComponent();
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000034DE File Offset: 0x000016DE
		public CompositeComponent CreateCopy()
		{
			return EngineApplicationInterface.ICompositeComponent.CreateCopy(base.Pointer);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000034F0 File Offset: 0x000016F0
		public void AddComponent(GameEntityComponent component)
		{
			EngineApplicationInterface.ICompositeComponent.AddComponent(base.Pointer, component.Pointer);
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00003508 File Offset: 0x00001708
		public void AddPrefabEntity(string prefabName, Scene scene)
		{
			EngineApplicationInterface.ICompositeComponent.AddPrefabEntity(base.Pointer, scene.Pointer, prefabName);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003521 File Offset: 0x00001721
		public void Dispose()
		{
			if (this.IsValid)
			{
				this.Release();
				GC.SuppressFinalize(this);
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00003537 File Offset: 0x00001737
		private void Release()
		{
			EngineApplicationInterface.ICompositeComponent.Release(base.Pointer);
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000354C File Offset: 0x0000174C
		~CompositeComponent()
		{
			this.Dispose();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00003578 File Offset: 0x00001778
		public uint GetFactor1()
		{
			return EngineApplicationInterface.ICompositeComponent.GetFactor1(base.Pointer);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000358A File Offset: 0x0000178A
		public uint GetFactor2()
		{
			return EngineApplicationInterface.ICompositeComponent.GetFactor2(base.Pointer);
		}

		// Token: 0x06000089 RID: 137 RVA: 0x0000359C File Offset: 0x0000179C
		public void SetFactor1(uint factorColor1)
		{
			EngineApplicationInterface.ICompositeComponent.SetFactor1(base.Pointer, factorColor1);
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000035AF File Offset: 0x000017AF
		public void SetFactor2(uint factorColor2)
		{
			EngineApplicationInterface.ICompositeComponent.SetFactor2(base.Pointer, factorColor2);
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000035C2 File Offset: 0x000017C2
		public void SetVectorArgument(float vectorArgument0, float vectorArgument1, float vectorArgument2, float vectorArgument3)
		{
			EngineApplicationInterface.ICompositeComponent.SetVectorArgument(base.Pointer, vectorArgument0, vectorArgument1, vectorArgument2, vectorArgument3);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000035D9 File Offset: 0x000017D9
		public void SetMaterial(Material material)
		{
			EngineApplicationInterface.ICompositeComponent.SetMaterial(base.Pointer, material.Pointer);
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000035F4 File Offset: 0x000017F4
		// (set) Token: 0x0600008E RID: 142 RVA: 0x0000361C File Offset: 0x0000181C
		public MatrixFrame Frame
		{
			get
			{
				MatrixFrame matrixFrame = default(MatrixFrame);
				EngineApplicationInterface.ICompositeComponent.GetFrame(base.Pointer, ref matrixFrame);
				return matrixFrame;
			}
			set
			{
				EngineApplicationInterface.ICompositeComponent.SetFrame(base.Pointer, ref value);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00003630 File Offset: 0x00001830
		// (set) Token: 0x06000090 RID: 144 RVA: 0x00003642 File Offset: 0x00001842
		public Vec3 VectorUserData
		{
			get
			{
				return EngineApplicationInterface.ICompositeComponent.GetVectorUserData(base.Pointer);
			}
			set
			{
				EngineApplicationInterface.ICompositeComponent.SetVectorUserData(base.Pointer, ref value);
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00003656 File Offset: 0x00001856
		public void SetVisibilityMask(VisibilityMaskFlags visibilityMask)
		{
			EngineApplicationInterface.ICompositeComponent.SetVisibilityMask(base.Pointer, visibilityMask);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00003669 File Offset: 0x00001869
		public override MetaMesh GetFirstMetaMesh()
		{
			return EngineApplicationInterface.ICompositeComponent.GetFirstMetaMesh(base.Pointer);
		}

		// Token: 0x06000093 RID: 147 RVA: 0x0000367B File Offset: 0x0000187B
		public void AddMultiMesh(string MultiMeshName)
		{
			EngineApplicationInterface.ICompositeComponent.AddMultiMesh(base.Pointer, MultiMeshName);
		}

		// Token: 0x06000094 RID: 148 RVA: 0x0000368E File Offset: 0x0000188E
		public void SetVisible(bool visible)
		{
			EngineApplicationInterface.ICompositeComponent.SetVisible(base.Pointer, visible);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x000036A1 File Offset: 0x000018A1
		public bool GetVisible()
		{
			return EngineApplicationInterface.ICompositeComponent.IsVisible(base.Pointer);
		}
	}
}
