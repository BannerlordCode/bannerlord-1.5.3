using System;
using System.IO;
using System.Xml;

namespace TaleWorlds.Library
{
	// Token: 0x02000077 RID: 119
	public class ParameterFile
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x0000F2CC File Offset: 0x0000D4CC
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x0000F2D4 File Offset: 0x0000D4D4
		public string Path { get; private set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x0000F2DD File Offset: 0x0000D4DD
		// (set) Token: 0x0600044D RID: 1101 RVA: 0x0000F2E5 File Offset: 0x0000D4E5
		public DateTime LastCheckedTime { get; private set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600044E RID: 1102 RVA: 0x0000F2EE File Offset: 0x0000D4EE
		// (set) Token: 0x0600044F RID: 1103 RVA: 0x0000F2F6 File Offset: 0x0000D4F6
		public ParameterContainer ParameterContainer { get; private set; }

		// Token: 0x06000450 RID: 1104 RVA: 0x0000F2FF File Offset: 0x0000D4FF
		public ParameterFile(string path)
		{
			this.ParameterContainer = new ParameterContainer();
			this.Path = path;
			this.LastCheckedTime = DateTime.MinValue;
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x0000F324 File Offset: 0x0000D524
		public bool CheckIfNeedsToBeRefreshed()
		{
			return File.GetLastWriteTime(this.Path) > this.LastCheckedTime;
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000F33C File Offset: 0x0000D53C
		public void Refresh()
		{
			this.ParameterContainer.ClearParameters();
			DateTime lastWriteTime = File.GetLastWriteTime(this.Path);
			XmlDocument xmlDocument = new XmlDocument();
			try
			{
				xmlDocument.Load(this.Path);
			}
			catch
			{
				this._failedAttemptsCount++;
				if (this._failedAttemptsCount >= 100)
				{
					Debug.FailedAssert("Could not load parameters file", "C:\\BuildAgent\\work\\mb3\\TaleWorlds.Shared\\Source\\Base\\TaleWorlds.Library\\ParameterFile.cs", "Refresh", 47);
				}
				return;
			}
			this._failedAttemptsCount = 0;
			foreach (object obj in xmlDocument.FirstChild.ChildNodes)
			{
				XmlElement xmlElement = (XmlElement)obj;
				string attribute = xmlElement.GetAttribute("name");
				string attribute2 = xmlElement.GetAttribute("value");
				this.ParameterContainer.AddParameter(attribute, attribute2, true);
			}
			this.LastCheckedTime = lastWriteTime;
		}

		// Token: 0x04000154 RID: 340
		private int _failedAttemptsCount;

		// Token: 0x04000155 RID: 341
		private const int MaxFailedAttemptsCount = 100;
	}
}
