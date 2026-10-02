using System;

namespace TaleWorlds.Engine
{
	// Token: 0x0200009F RID: 159
	public struct WeakMaterial
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000F0B RID: 3851 RVA: 0x00011A3E File Offset: 0x0000FC3E
		// (set) Token: 0x06000F0C RID: 3852 RVA: 0x00011A46 File Offset: 0x0000FC46
		public UIntPtr Pointer { get; private set; }

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x00011A4F File Offset: 0x0000FC4F
		public bool IsValid
		{
			get
			{
				return this.Pointer != UIntPtr.Zero;
			}
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x00011A61 File Offset: 0x0000FC61
		internal WeakMaterial(UIntPtr pointer)
		{
			this.Pointer = pointer;
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x00011A6A File Offset: 0x0000FC6A
		public Shader GetShader()
		{
			return EngineApplicationInterface.IMaterial.GetShader(this.Pointer);
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x00011A7C File Offset: 0x0000FC7C
		public ulong GetShaderFlags()
		{
			return EngineApplicationInterface.IMaterial.GetShaderFlags(this.Pointer);
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00011A8E File Offset: 0x0000FC8E
		public void SetShaderFlags(ulong flagEntry)
		{
			EngineApplicationInterface.IMaterial.SetShaderFlags(this.Pointer, flagEntry);
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x00011AA1 File Offset: 0x0000FCA1
		public void SetMeshVectorArgument(float x, float y, float z, float w)
		{
			EngineApplicationInterface.IMaterial.SetMeshVectorArgument(this.Pointer, x, y, z, w);
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x00011AB8 File Offset: 0x0000FCB8
		public void SetTexture(Material.MBTextureType textureType, Texture texture)
		{
			EngineApplicationInterface.IMaterial.SetTexture(this.Pointer, (int)textureType, texture.Pointer);
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x00011AD1 File Offset: 0x0000FCD1
		public void SetTextureAtSlot(int textureSlot, Texture texture)
		{
			EngineApplicationInterface.IMaterial.SetTextureAtSlot(this.Pointer, textureSlot, texture.Pointer);
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x00011AEA File Offset: 0x0000FCEA
		public void SetAreaMapScale(float scale)
		{
			EngineApplicationInterface.IMaterial.SetAreaMapScale(this.Pointer, scale);
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x00011AFD File Offset: 0x0000FCFD
		public void SetEnableSkinning(bool enable)
		{
			EngineApplicationInterface.IMaterial.SetEnableSkinning(this.Pointer, enable);
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x00011B10 File Offset: 0x0000FD10
		public bool UsingSkinning()
		{
			return EngineApplicationInterface.IMaterial.UsingSkinning(this.Pointer);
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x00011B22 File Offset: 0x0000FD22
		public Texture GetTexture(Material.MBTextureType textureType)
		{
			return EngineApplicationInterface.IMaterial.GetTexture(this.Pointer, (int)textureType);
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x00011B35 File Offset: 0x0000FD35
		public Texture GetTextureWithSlot(int textureSlot)
		{
			return EngineApplicationInterface.IMaterial.GetTexture(this.Pointer, textureSlot);
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000F1A RID: 3866 RVA: 0x00011B48 File Offset: 0x0000FD48
		// (set) Token: 0x06000F1B RID: 3867 RVA: 0x00011B5A File Offset: 0x0000FD5A
		public string Name
		{
			get
			{
				return EngineApplicationInterface.IMaterial.GetName(this.Pointer);
			}
			set
			{
				EngineApplicationInterface.IMaterial.SetName(this.Pointer, value);
			}
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x00011B6D File Offset: 0x0000FD6D
		public void AddMaterialShaderFlag(string flagName, bool showErrors)
		{
			EngineApplicationInterface.IMaterial.AddMaterialShaderFlag(this.Pointer, flagName, showErrors);
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x00011B81 File Offset: 0x0000FD81
		public void RemoveMaterialShaderFlag(string flagName)
		{
			EngineApplicationInterface.IMaterial.RemoveMaterialShaderFlag(this.Pointer, flagName);
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x00011B94 File Offset: 0x0000FD94
		public static bool operator ==(WeakMaterial weakMaterial1, WeakMaterial weakMaterial2)
		{
			return weakMaterial1.Pointer == weakMaterial2.Pointer;
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x00011BA9 File Offset: 0x0000FDA9
		public static bool operator !=(WeakMaterial weakMaterial1, WeakMaterial weakMaterial2)
		{
			return weakMaterial1.Pointer != weakMaterial2.Pointer;
		}

		// Token: 0x06000F20 RID: 3872 RVA: 0x00011BBE File Offset: 0x0000FDBE
		public override bool Equals(object obj)
		{
			return ((Material)obj).Pointer == this.Pointer;
		}

		// Token: 0x06000F21 RID: 3873 RVA: 0x00011BD8 File Offset: 0x0000FDD8
		public override int GetHashCode()
		{
			return this.Pointer.GetHashCode();
		}

		// Token: 0x04000208 RID: 520
		public static readonly WeakMaterial Invalid = new WeakMaterial(UIntPtr.Zero);
	}
}
