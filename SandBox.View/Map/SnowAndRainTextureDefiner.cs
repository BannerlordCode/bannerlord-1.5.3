using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine;

namespace SandBox.View.Map
{
	// Token: 0x02000061 RID: 97
	public class SnowAndRainTextureDefiner : ScriptComponentBehavior
	{
		// Token: 0x060003C5 RID: 965 RVA: 0x0001E0E8 File Offset: 0x0001C2E8
		protected override void OnInit()
		{
			this.SetDataToScene();
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x0001E0F0 File Offset: 0x0001C2F0
		protected override void OnTerrainReload(int step)
		{
			if (step == 1)
			{
				this.SetDataToScene();
			}
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x0001E0FC File Offset: 0x0001C2FC
		protected override void OnEditorInit()
		{
			if (base.GameEntity.Scene.ContainsTerrain)
			{
				base.GameEntity.Scene.SetDynamicSnowTexture(this.SnowAndRainTexture);
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x0001E138 File Offset: 0x0001C338
		protected override void OnEditorVariableChanged(string variableName)
		{
			if (variableName == "SnowAndRainTexture" && base.GameEntity.Scene.ContainsTerrain)
			{
				base.GameEntity.Scene.SetDynamicSnowTexture(this.SnowAndRainTexture);
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001E180 File Offset: 0x0001C380
		private void SetDataToScene()
		{
			if (this.SnowAndRainTexture != null)
			{
				((MapScene)Campaign.Current.MapSceneWrapper).SetSnowAndRainDataWithDimension(this.SnowAndRainTexture, this.WeatherNodeGridWidthAndHeight);
			}
		}

		// Token: 0x040001EB RID: 491
		[EditorVisibleScriptComponentVariable(true)]
		public Texture SnowAndRainTexture;

		// Token: 0x040001EC RID: 492
		[EditorVisibleScriptComponentVariable(true)]
		public int WeatherNodeGridWidthAndHeight;
	}
}
