using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TaleWorlds.Library
{
	// Token: 0x0200007F RID: 127
	public class PlatformFileHelperPC : IPlatformFileHelper
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x0000FBE2 File Offset: 0x0000DDE2
		private string DocumentsPath
		{
			get
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x0000FBEA File Offset: 0x0000DDEA
		private string ProgramDataPath
		{
			get
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000FBF3 File Offset: 0x0000DDF3
		public PlatformFileHelperPC(string applicationName)
		{
			this.ApplicationName = applicationName;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000FC04 File Offset: 0x0000DE04
		public SaveResult SaveFile(PlatformFilePath path, byte[] data)
		{
			SaveResult saveResult = SaveResult.PlatformFileHelperFailure;
			PlatformFileHelperPC.Error = "";
			try
			{
				this.CreateDirectory(path.FolderPath);
				File.WriteAllBytes(this.GetFileFullPath(path), data);
				saveResult = SaveResult.Success;
			}
			catch (Exception ex)
			{
				PlatformFileHelperPC.Error = ex.Message;
				saveResult = SaveResult.PlatformFileHelperFailure;
			}
			return saveResult;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000FC5C File Offset: 0x0000DE5C
		public SaveResult SaveFileString(PlatformFilePath path, string data)
		{
			SaveResult saveResult = SaveResult.PlatformFileHelperFailure;
			PlatformFileHelperPC.Error = "";
			try
			{
				this.CreateDirectory(path.FolderPath);
				File.WriteAllText(this.GetFileFullPath(path), data, Encoding.UTF8);
				saveResult = SaveResult.Success;
			}
			catch (Exception ex)
			{
				PlatformFileHelperPC.Error = ex.Message;
				saveResult = SaveResult.PlatformFileHelperFailure;
			}
			return saveResult;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000FCB8 File Offset: 0x0000DEB8
		public Task<SaveResult> SaveFileAsync(PlatformFilePath path, byte[] data)
		{
			return Task.FromResult<SaveResult>(this.SaveFile(path, data));
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000FCC7 File Offset: 0x0000DEC7
		public Task<SaveResult> SaveFileStringAsync(PlatformFilePath path, string data)
		{
			return Task.FromResult<SaveResult>(this.SaveFileString(path, data));
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000FCD8 File Offset: 0x0000DED8
		public SaveResult AppendLineToFileString(PlatformFilePath path, string data)
		{
			SaveResult saveResult = SaveResult.PlatformFileHelperFailure;
			PlatformFileHelperPC.Error = "";
			try
			{
				this.CreateDirectory(path.FolderPath);
				File.AppendAllText(this.GetFileFullPath(path), "\n" + data, Encoding.UTF8);
				saveResult = SaveResult.Success;
			}
			catch (Exception ex)
			{
				PlatformFileHelperPC.Error = ex.Message;
				saveResult = SaveResult.PlatformFileHelperFailure;
			}
			return saveResult;
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000FD3C File Offset: 0x0000DF3C
		private string GetDirectoryFullPath(PlatformDirectoryPath directoryPath)
		{
			string text = "";
			switch (directoryPath.Type)
			{
			case PlatformFileType.User:
				text = Path.Combine(this.DocumentsPath, this.ApplicationName);
				break;
			case PlatformFileType.Application:
				text = Path.Combine(this.ProgramDataPath, this.ApplicationName);
				break;
			case PlatformFileType.Temporary:
				text = Path.Combine(this.DocumentsPath, this.ApplicationName, "Temp");
				break;
			}
			return Path.Combine(text, directoryPath.Path);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000FDB5 File Offset: 0x0000DFB5
		public string GetFileFullPath(PlatformFilePath filePath)
		{
			return Path.GetFullPath(Path.Combine(this.GetDirectoryFullPath(filePath.FolderPath), filePath.FileName));
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000FDD3 File Offset: 0x0000DFD3
		public bool FileExists(PlatformFilePath path)
		{
			return File.Exists(this.GetFileFullPath(path));
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000FDE4 File Offset: 0x0000DFE4
		public async Task<string> GetFileContentStringAsync(PlatformFilePath path)
		{
			string text;
			if (!this.FileExists(path))
			{
				text = null;
			}
			else
			{
				string fileFullPath = this.GetFileFullPath(path);
				string text2 = string.Empty;
				using (FileStream sourceStream = File.Open(fileFullPath, FileMode.Open))
				{
					byte[] buffer = new byte[sourceStream.Length];
					await sourceStream.ReadAsync(buffer, 0, (int)sourceStream.Length);
					text2 = Encoding.UTF8.GetString(buffer);
					buffer = null;
				}
				FileStream sourceStream = null;
				string @string = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
				if (text2.StartsWith(@string, StringComparison.Ordinal))
				{
					text2 = text2.Remove(0, @string.Length);
				}
				text = text2;
			}
			return text;
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0000FE34 File Offset: 0x0000E034
		public string GetFileContentString(PlatformFilePath path)
		{
			if (!this.FileExists(path))
			{
				return null;
			}
			string fileFullPath = this.GetFileFullPath(path);
			string text = null;
			PlatformFileHelperPC.Error = "";
			try
			{
				text = File.ReadAllText(fileFullPath, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				PlatformFileHelperPC.Error = ex.Message;
				Debug.Print(PlatformFileHelperPC.Error, 0, Debug.DebugColor.White, 17592186044416UL);
			}
			return text;
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0000FEA4 File Offset: 0x0000E0A4
		public byte[] GetMetaDataContent(PlatformFilePath path)
		{
			if (!this.FileExists(path))
			{
				return null;
			}
			string fileFullPath = this.GetFileFullPath(path);
			try
			{
				using (FileStream fileStream = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read))
				{
					using (BinaryReader binaryReader = new BinaryReader(fileStream))
					{
						int num = binaryReader.ReadInt32();
						if ((long)num > fileStream.Length - fileStream.Position)
						{
							return null;
						}
						byte[] array = new byte[num + 4];
						BitConverter.GetBytes(num).CopyTo(array, 0);
						if (binaryReader.Read(array, 4, num) < num)
						{
							return null;
						}
						return array;
					}
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0000FF64 File Offset: 0x0000E164
		public byte[] GetFileContent(PlatformFilePath path)
		{
			if (!this.FileExists(path))
			{
				return null;
			}
			string fileFullPath = this.GetFileFullPath(path);
			byte[] array = null;
			PlatformFileHelperPC.Error = "";
			try
			{
				array = File.ReadAllBytes(fileFullPath);
			}
			catch (Exception ex)
			{
				PlatformFileHelperPC.Error = ex.Message;
				Debug.Print(PlatformFileHelperPC.Error, 0, Debug.DebugColor.White, 17592186044416UL);
			}
			return array;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0000FFCC File Offset: 0x0000E1CC
		public bool DeleteFile(PlatformFilePath path)
		{
			string fileFullPath = this.GetFileFullPath(path);
			if (!this.FileExists(path))
			{
				return false;
			}
			bool flag;
			try
			{
				File.Delete(fileFullPath);
				flag = true;
			}
			catch (Exception ex)
			{
				PlatformFileHelperPC.Error = ex.Message;
				Debug.Print(PlatformFileHelperPC.Error, 0, Debug.DebugColor.White, 17592186044416UL);
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x0001002C File Offset: 0x0000E22C
		public void CreateDirectory(PlatformDirectoryPath path)
		{
			Directory.CreateDirectory(this.GetDirectoryFullPath(path));
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x0001003C File Offset: 0x0000E23C
		public PlatformFilePath[] GetFiles(PlatformDirectoryPath path, string searchPattern, SearchOption searchOption)
		{
			string directoryFullPath = this.GetDirectoryFullPath(path);
			DirectoryInfo directoryInfo = new DirectoryInfo(directoryFullPath);
			PlatformFilePath[] array = null;
			PlatformFileHelperPC.Error = "";
			if (directoryInfo.Exists)
			{
				try
				{
					FileInfo[] files = directoryInfo.GetFiles(searchPattern, searchOption);
					array = new PlatformFilePath[files.Length];
					for (int i = 0; i < files.Length; i++)
					{
						FileInfo fileInfo = files[i];
						fileInfo.FullName.Substring(directoryFullPath.Length);
						PlatformFilePath platformFilePath = new PlatformFilePath(path, fileInfo.Name);
						array[i] = platformFilePath;
					}
					return array;
				}
				catch (Exception ex)
				{
					PlatformFileHelperPC.Error = ex.Message;
					return array;
				}
			}
			array = new PlatformFilePath[0];
			return array;
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x000100E8 File Offset: 0x0000E2E8
		public void RenameFile(PlatformFilePath filePath, string newName)
		{
			string fileFullPath = this.GetFileFullPath(filePath);
			string fileFullPath2 = this.GetFileFullPath(new PlatformFilePath(filePath.FolderPath, newName));
			File.Move(fileFullPath, fileFullPath2);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x00010115 File Offset: 0x0000E315
		public string GetError()
		{
			return PlatformFileHelperPC.Error;
		}

		// Token: 0x0400016B RID: 363
		private readonly string ApplicationName;

		// Token: 0x0400016C RID: 364
		private static string Error;
	}
}
