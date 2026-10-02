using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000346 RID: 838
	public class ScenePropDecal : ScriptComponentBehavior
	{
		// Token: 0x06002F1A RID: 12058 RVA: 0x000B6DE0 File Offset: 0x000B4FE0
		protected internal override void OnInit()
		{
			base.OnInit();
			this.SetUpMaterial();
		}

		// Token: 0x06002F1B RID: 12059 RVA: 0x000B6DEE File Offset: 0x000B4FEE
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this.SetUpMaterial();
		}

		// Token: 0x06002F1C RID: 12060 RVA: 0x000B6DFC File Offset: 0x000B4FFC
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			this.SetUpMaterial();
		}

		// Token: 0x06002F1D RID: 12061 RVA: 0x000B6E0C File Offset: 0x000B500C
		private void EnsureUniqueMaterial()
		{
			Material fromResource = Material.GetFromResource(this.MaterialName);
			this.UniqueMaterial = fromResource.CreateCopy();
		}

		// Token: 0x06002F1E RID: 12062 RVA: 0x000B6E34 File Offset: 0x000B5034
		private void SetUpMaterial()
		{
			this.EnsureUniqueMaterial();
			Texture texture = Texture.CheckAndGetFromResource(this.DiffuseTexture);
			Texture texture2 = Texture.CheckAndGetFromResource(this.NormalTexture);
			Texture texture3 = Texture.CheckAndGetFromResource(this.SpecularTexture);
			Texture texture4 = Texture.CheckAndGetFromResource(this.MaskTexture);
			if (texture != null)
			{
				this.UniqueMaterial.SetTexture(Material.MBTextureType.DiffuseMap, texture);
			}
			if (texture2 != null)
			{
				this.UniqueMaterial.SetTexture(Material.MBTextureType.BumpMap, texture2);
			}
			if (texture3 != null)
			{
				this.UniqueMaterial.SetTexture(Material.MBTextureType.SpecularMap, texture3);
			}
			if (texture4 != null)
			{
				this.UniqueMaterial.SetTexture(Material.MBTextureType.DiffuseMap2, texture4);
				this.UniqueMaterial.AddMaterialShaderFlag("use_areamap", false);
			}
			this.UniqueMaterial.SetAlphaTestValue(this.AlphaTestValue);
			base.GameEntity.SetMaterialForAllMeshes(this.UniqueMaterial);
			Mesh firstMesh = base.GameEntity.GetFirstMesh();
			if (firstMesh != null)
			{
				if (this.UniqueMaterial != null)
				{
					firstMesh.SetVectorArgument(this.TilingSize, this.TilingSize, this.TilingOffset, this.TilingOffset);
				}
				firstMesh.SetVectorArgument2(this.TextureSweepX, this.TextureSweepY, 0f, this.UseBaseNormals ? 1f : 0f);
			}
		}

		// Token: 0x040012D3 RID: 4819
		public string DiffuseTexture;

		// Token: 0x040012D4 RID: 4820
		public string NormalTexture;

		// Token: 0x040012D5 RID: 4821
		public string SpecularTexture;

		// Token: 0x040012D6 RID: 4822
		public string MaskTexture;

		// Token: 0x040012D7 RID: 4823
		public bool UseBaseNormals;

		// Token: 0x040012D8 RID: 4824
		public float TilingSize = 1f;

		// Token: 0x040012D9 RID: 4825
		public float TilingOffset;

		// Token: 0x040012DA RID: 4826
		public float AlphaTestValue;

		// Token: 0x040012DB RID: 4827
		public float TextureSweepX;

		// Token: 0x040012DC RID: 4828
		public float TextureSweepY;

		// Token: 0x040012DD RID: 4829
		public string MaterialName = "deferred_decal_material";

		// Token: 0x040012DE RID: 4830
		protected Material UniqueMaterial;
	}
}
