using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.View.Scripts
{
	// Token: 0x02000066 RID: 102
	public class MapColorGradeManager : ScriptComponentBehavior
	{
		// Token: 0x060003EE RID: 1006 RVA: 0x0001DD90 File Offset: 0x0001BF90
		private void Init()
		{
			if (base.Scene.ContainsTerrain)
			{
				Vec2i vec2i;
				float num;
				int num2;
				int num3;
				base.Scene.GetTerrainData(out vec2i, out num, out num2, out num3);
				this.terrainSize.x = (float)vec2i.X * num;
				this.terrainSize.y = (float)vec2i.Y * num;
			}
			this.colorGradeGridMapping.Add(1, this.defaultColorGradeTextureName);
			this.colorGradeGridMapping.Add(2, "worldmap_colorgrade_night");
			this.ReadColorGradesXml();
			MBMapScene.GetColorGradeGridData(base.Scene, this.colorGradeGrid, this.colorGradeGridName);
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x0001DE25 File Offset: 0x0001C025
		protected override void OnInit()
		{
			base.OnInit();
			this.Init();
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001DE33 File Offset: 0x0001C033
		protected override void OnEditorInit()
		{
			base.OnEditorInit();
			this.Init();
			this.TimeOfDay = base.Scene.TimeOfDay;
			this.lastSceneTimeOfDay = this.TimeOfDay;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x0001DE5E File Offset: 0x0001C05E
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick;
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x0001DE61 File Offset: 0x0001C061
		protected override void OnTick(float dt)
		{
			this.TimeOfDay = base.Scene.TimeOfDay;
			this.SeasonTimeFactor = MBMapScene.GetSeasonTimeFactor(base.Scene);
			this.ApplyAtmosphere(false);
			this.ApplyColorGrade(dt);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x0001DE94 File Offset: 0x0001C094
		protected override void OnEditorTick(float dt)
		{
			if (base.Scene.TimeOfDay != this.lastSceneTimeOfDay)
			{
				this.TimeOfDay = base.Scene.TimeOfDay;
				this.lastSceneTimeOfDay = this.TimeOfDay;
			}
			if (base.Scene.ContainsTerrain)
			{
				Vec2i vec2i;
				float num;
				int num2;
				int num3;
				base.Scene.GetTerrainData(out vec2i, out num, out num2, out num3);
				this.terrainSize.x = (float)vec2i.X * num;
				this.terrainSize.y = (float)vec2i.Y * num;
			}
			else
			{
				this.terrainSize.x = 1f;
				this.terrainSize.y = 1f;
			}
			if (this.AtmosphereSimulationEnabled)
			{
				this.TimeOfDay += dt;
				if (this.TimeOfDay >= 24f)
				{
					this.TimeOfDay -= 24f;
				}
				this.ApplyAtmosphere(false);
			}
			if (this.ColorGradeEnabled)
			{
				this.ApplyColorGrade(dt);
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x0001DF88 File Offset: 0x0001C188
		protected override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "ColorGradeEnabled")
			{
				if (!this.ColorGradeEnabled)
				{
					base.Scene.SetColorGradeBlend("", "", -1f);
					this.lastColorGrade = 0;
					return;
				}
			}
			else
			{
				if (variableName == "TimeOfDay")
				{
					this.ApplyAtmosphere(false);
					return;
				}
				if (variableName == "SeasonTimeFactor")
				{
					this.ApplyAtmosphere(false);
				}
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x0001DFFC File Offset: 0x0001C1FC
		private void ReadColorGradesXml()
		{
			List<string> list;
			XmlDocument mergedXmlForNative = MBObjectManager.GetMergedXmlForNative("soln_worldmap_color_grades", out list);
			if (mergedXmlForNative == null)
			{
				return;
			}
			XmlNode xmlNode = mergedXmlForNative.SelectSingleNode("worldmap_color_grades");
			if (xmlNode == null)
			{
				return;
			}
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("color_grade_grid");
			if (xmlNode2 != null && xmlNode2.Attributes["name"] != null)
			{
				this.colorGradeGridName = xmlNode2.Attributes["name"].Value;
			}
			XmlNode xmlNode3 = xmlNode.SelectSingleNode("color_grade_default");
			if (xmlNode3 != null && xmlNode3.Attributes["name"] != null)
			{
				this.defaultColorGradeTextureName = xmlNode3.Attributes["name"].Value;
				this.colorGradeGridMapping[1] = this.defaultColorGradeTextureName;
			}
			XmlNode xmlNode4 = xmlNode.SelectSingleNode("color_grade_night");
			if (xmlNode4 != null && xmlNode4.Attributes["name"] != null)
			{
				this.colorGradeGridMapping[2] = xmlNode4.Attributes["name"].Value;
			}
			XmlNodeList xmlNodeList = xmlNode.SelectNodes("color_grade");
			if (xmlNodeList != null)
			{
				foreach (object obj in xmlNodeList)
				{
					XmlNode xmlNode5 = (XmlNode)obj;
					byte b;
					if (xmlNode5.Attributes["name"] != null && xmlNode5.Attributes["value"] != null && byte.TryParse(xmlNode5.Attributes["value"].Value, out b))
					{
						this.colorGradeGridMapping[b] = xmlNode5.Attributes["name"].Value;
					}
				}
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0001E1C4 File Offset: 0x0001C3C4
		public void ApplyAtmosphere(bool forceLoadTextures)
		{
			this.TimeOfDay = MBMath.ClampFloat(this.TimeOfDay, 0f, 23.99f);
			this.SeasonTimeFactor = MBMath.ClampFloat(this.SeasonTimeFactor, 0f, 1f);
			MBMapScene.SetFrameForAtmosphere(base.Scene, this.TimeOfDay * 10f, base.Scene.LastFinalRenderCameraFrame.origin.z, forceLoadTextures);
			float num = 0.55f;
			float num2 = -0.1f;
			float seasonTimeFactor = this.SeasonTimeFactor;
			Vec3 vec = new Vec3(0f, 0.65f, 0f, -1f);
			vec.x = MBMath.Lerp(num, num2, seasonTimeFactor, 1E-05f);
			MBMapScene.SetTerrainDynamicParams(base.Scene, vec);
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x0001E284 File Offset: 0x0001C484
		public void ApplyColorGrade(float dt)
		{
			Vec3 origin = base.Scene.LastFinalRenderCameraFrame.origin;
			float num = 1f;
			int num2 = MathF.Floor(origin.x / this.terrainSize.X * 512f);
			int num3 = MathF.Floor(origin.y / this.terrainSize.Y * 512f);
			num2 = MBMath.ClampIndex(num2, 0, 512);
			num3 = MBMath.ClampIndex(num3, 0, 512);
			byte b = this.colorGradeGrid[num3 * 512 + num2];
			if (origin.z > 400f)
			{
				b = 1;
			}
			if (this.TimeOfDay > 22f || this.TimeOfDay < 2f)
			{
				b = 2;
			}
			if (MBMapScene.GetApplyRainColorGrade() && origin.z < 50f)
			{
				b = 160;
				num = 0.2f;
			}
			if (this.lastColorGrade != b)
			{
				string text = "";
				string text2 = "";
				if (!this.colorGradeGridMapping.TryGetValue(this.lastColorGrade, out text))
				{
					text = this.defaultColorGradeTextureName;
				}
				if (!this.colorGradeGridMapping.TryGetValue(b, out text2))
				{
					text2 = this.defaultColorGradeTextureName;
				}
				if (this.primaryTransitionRecord == null)
				{
					this.primaryTransitionRecord = new MapColorGradeManager.ColorGradeBlendRecord
					{
						color1 = text,
						color2 = text2,
						alpha = 0f
					};
				}
				else
				{
					this.secondaryTransitionRecord = new MapColorGradeManager.ColorGradeBlendRecord
					{
						color1 = this.primaryTransitionRecord.color2,
						color2 = text2,
						alpha = 0f
					};
				}
				this.lastColorGrade = b;
			}
			if (this.primaryTransitionRecord != null)
			{
				if (this.primaryTransitionRecord.alpha < 1f)
				{
					this.primaryTransitionRecord.alpha = MathF.Min(this.primaryTransitionRecord.alpha + dt * (1f / num), 1f);
					base.Scene.SetColorGradeBlend(this.primaryTransitionRecord.color1, this.primaryTransitionRecord.color2, this.primaryTransitionRecord.alpha);
					return;
				}
				this.primaryTransitionRecord = null;
				if (this.secondaryTransitionRecord != null)
				{
					this.primaryTransitionRecord = new MapColorGradeManager.ColorGradeBlendRecord(this.secondaryTransitionRecord);
					this.secondaryTransitionRecord = null;
				}
			}
		}

		// Token: 0x04000250 RID: 592
		public bool ColorGradeEnabled;

		// Token: 0x04000251 RID: 593
		public bool AtmosphereSimulationEnabled;

		// Token: 0x04000252 RID: 594
		public float TimeOfDay;

		// Token: 0x04000253 RID: 595
		public float SeasonTimeFactor;

		// Token: 0x04000254 RID: 596
		private string colorGradeGridName = "worldmap_colorgrade_grid";

		// Token: 0x04000255 RID: 597
		private const int colorGradeGridSize = 262144;

		// Token: 0x04000256 RID: 598
		private byte[] colorGradeGrid = new byte[262144];

		// Token: 0x04000257 RID: 599
		private Dictionary<byte, string> colorGradeGridMapping = new Dictionary<byte, string>();

		// Token: 0x04000258 RID: 600
		private MapColorGradeManager.ColorGradeBlendRecord primaryTransitionRecord;

		// Token: 0x04000259 RID: 601
		private MapColorGradeManager.ColorGradeBlendRecord secondaryTransitionRecord;

		// Token: 0x0400025A RID: 602
		private byte lastColorGrade;

		// Token: 0x0400025B RID: 603
		private Vec2 terrainSize = new Vec2(1f, 1f);

		// Token: 0x0400025C RID: 604
		private string defaultColorGradeTextureName = "worldmap_colorgrade_stratosphere";

		// Token: 0x0400025D RID: 605
		private const float transitionSpeedFactor = 1f;

		// Token: 0x0400025E RID: 606
		private float lastSceneTimeOfDay;

		// Token: 0x020000D7 RID: 215
		private class ColorGradeBlendRecord
		{
			// Token: 0x06000656 RID: 1622 RVA: 0x0002B5F4 File Offset: 0x000297F4
			public ColorGradeBlendRecord()
			{
				this.color1 = "";
				this.color2 = "";
				this.alpha = 0f;
			}

			// Token: 0x06000657 RID: 1623 RVA: 0x0002B61D File Offset: 0x0002981D
			public ColorGradeBlendRecord(MapColorGradeManager.ColorGradeBlendRecord other)
			{
				this.color1 = other.color1;
				this.color2 = other.color2;
				this.alpha = other.alpha;
			}

			// Token: 0x040003DA RID: 986
			public string color1;

			// Token: 0x040003DB RID: 987
			public string color2;

			// Token: 0x040003DC RID: 988
			public float alpha;
		}
	}
}
