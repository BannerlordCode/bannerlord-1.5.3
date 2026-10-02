using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace TaleWorlds.MountAndBlade.Launcher.Library.UserDatas
{
	// Token: 0x0200001E RID: 30
	public class UserDataManager
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00005DDA File Offset: 0x00003FDA
		// (set) Token: 0x06000135 RID: 309 RVA: 0x00005DE2 File Offset: 0x00003FE2
		public UserData UserData { get; private set; }

		// Token: 0x06000136 RID: 310 RVA: 0x00005DEC File Offset: 0x00003FEC
		public UserDataManager()
		{
			this.UserData = new UserData();
			string text = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			text += "\\Mount and Blade II Bannerlord\\Configs\\";
			if (!Directory.Exists(text))
			{
				try
				{
					Directory.CreateDirectory(text);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
			this._filePath = text + "LauncherData.xml";
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005E58 File Offset: 0x00004058
		public bool HasUserData()
		{
			return File.Exists(this._filePath);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005E68 File Offset: 0x00004068
		public void LoadUserData()
		{
			if (!File.Exists(this._filePath))
			{
				return;
			}
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserData));
			try
			{
				using (XmlReader xmlReader = XmlReader.Create(this._filePath))
				{
					this.UserData = (UserData)xmlSerializer.Deserialize(xmlReader);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005EE4 File Offset: 0x000040E4
		public void SaveUserData()
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserData));
			try
			{
				using (XmlWriter xmlWriter = XmlWriter.Create(this._filePath, new XmlWriterSettings
				{
					Indent = true
				}))
				{
					xmlSerializer.Serialize(xmlWriter, this.UserData);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		// Token: 0x04000096 RID: 150
		private const string DataFolder = "\\Mount and Blade II Bannerlord\\Configs\\";

		// Token: 0x04000097 RID: 151
		private const string FileName = "LauncherData.xml";

		// Token: 0x04000098 RID: 152
		private readonly string _filePath;
	}
}
