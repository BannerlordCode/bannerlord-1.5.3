using System;
using System.Collections.Generic;
using System.Xml;

namespace TaleWorlds.Engine
{
	// Token: 0x02000076 RID: 118
	public class PerformanceAnalyzer
	{
		// Token: 0x06000AA9 RID: 2729 RVA: 0x0000ADB0 File Offset: 0x00008FB0
		public void Start(string name)
		{
			PerformanceAnalyzer.PerformanceObject performanceObject = new PerformanceAnalyzer.PerformanceObject(name);
			this.currentObject = performanceObject;
			this.objects.Add(performanceObject);
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0000ADD7 File Offset: 0x00008FD7
		public void End()
		{
			this.currentObject = null;
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0000ADE0 File Offset: 0x00008FE0
		public void FinalizeAndWrite(string filePath)
		{
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				XmlNode xmlNode = xmlDocument.CreateElement("objects");
				xmlDocument.AppendChild(xmlNode);
				foreach (PerformanceAnalyzer.PerformanceObject performanceObject in this.objects)
				{
					XmlNode xmlNode2 = xmlDocument.CreateElement("object");
					performanceObject.Write(xmlNode2, xmlDocument);
					xmlNode.AppendChild(xmlNode2);
				}
				xmlDocument.Save(filePath);
			}
			catch (Exception ex)
			{
				MBDebug.ShowWarning("Exception occurred while trying to write " + filePath + ": " + ex.ToString());
			}
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x0000AE98 File Offset: 0x00009098
		public void Tick(float dt)
		{
			if (this.currentObject != null)
			{
				this.currentObject.AddFps(Utilities.GetFps(), Utilities.GetMainFps(), Utilities.GetRendererFps());
			}
		}

		// Token: 0x04000163 RID: 355
		private List<PerformanceAnalyzer.PerformanceObject> objects = new List<PerformanceAnalyzer.PerformanceObject>();

		// Token: 0x04000164 RID: 356
		private PerformanceAnalyzer.PerformanceObject currentObject;

		// Token: 0x020000CF RID: 207
		private class PerformanceObject
		{
			// Token: 0x170000D3 RID: 211
			// (get) Token: 0x06001029 RID: 4137 RVA: 0x00014E7A File Offset: 0x0001307A
			private float AverageMainFps
			{
				get
				{
					if (this.frameCount > 0)
					{
						return this.totalMainFps / (float)this.frameCount;
					}
					return 0f;
				}
			}

			// Token: 0x170000D4 RID: 212
			// (get) Token: 0x0600102A RID: 4138 RVA: 0x00014E99 File Offset: 0x00013099
			private float AverageRendererFps
			{
				get
				{
					if (this.frameCount > 0)
					{
						return this.totalRendererFps / (float)this.frameCount;
					}
					return 0f;
				}
			}

			// Token: 0x170000D5 RID: 213
			// (get) Token: 0x0600102B RID: 4139 RVA: 0x00014EB8 File Offset: 0x000130B8
			private float AverageFps
			{
				get
				{
					if (this.frameCount > 0)
					{
						return this.totalFps / (float)this.frameCount;
					}
					return 0f;
				}
			}

			// Token: 0x0600102C RID: 4140 RVA: 0x00014ED7 File Offset: 0x000130D7
			public void AddFps(float fps, float main, float renderer)
			{
				this.frameCount++;
				this.totalFps += fps;
				this.totalMainFps += main;
				this.totalRendererFps += renderer;
			}

			// Token: 0x0600102D RID: 4141 RVA: 0x00014F14 File Offset: 0x00013114
			public void Write(XmlNode node, XmlDocument document)
			{
				XmlAttribute xmlAttribute = document.CreateAttribute("name");
				xmlAttribute.Value = this.name;
				node.Attributes.Append(xmlAttribute);
				XmlAttribute xmlAttribute2 = document.CreateAttribute("frameCount");
				xmlAttribute2.Value = this.frameCount.ToString();
				node.Attributes.Append(xmlAttribute2);
				XmlAttribute xmlAttribute3 = document.CreateAttribute("averageFps");
				xmlAttribute3.Value = this.AverageFps.ToString();
				node.Attributes.Append(xmlAttribute3);
				XmlAttribute xmlAttribute4 = document.CreateAttribute("averageMainFps");
				xmlAttribute4.Value = this.AverageMainFps.ToString();
				node.Attributes.Append(xmlAttribute4);
				XmlAttribute xmlAttribute5 = document.CreateAttribute("averageRendererFps");
				xmlAttribute5.Value = this.AverageRendererFps.ToString();
				node.Attributes.Append(xmlAttribute5);
			}

			// Token: 0x0600102E RID: 4142 RVA: 0x00014FFD File Offset: 0x000131FD
			public PerformanceObject(string objectName)
			{
				this.name = objectName;
			}

			// Token: 0x04000438 RID: 1080
			private string name;

			// Token: 0x04000439 RID: 1081
			private int frameCount;

			// Token: 0x0400043A RID: 1082
			private float totalMainFps;

			// Token: 0x0400043B RID: 1083
			private float totalRendererFps;

			// Token: 0x0400043C RID: 1084
			private float totalFps;
		}
	}
}
