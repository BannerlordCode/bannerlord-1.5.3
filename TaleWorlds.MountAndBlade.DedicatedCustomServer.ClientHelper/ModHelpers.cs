using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade.DedicatedCustomServer.ClientHelper
{
	// Token: 0x02000005 RID: 5
	internal static class ModHelpers
	{
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00002D64 File Offset: 0x00000F64
		public static string RootPath
		{
			get
			{
				return ModuleHelper.GetModuleFullPath("Multiplayer");
			}
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002D70 File Offset: 0x00000F70
		public static string GetSceneObjRootPath()
		{
			string text = Path.Combine(ModHelpers.RootPath, "SceneObj");
			if (!Directory.Exists(text))
			{
				ModLogger.Log("Multiplayer module didn't have 'SceneObj' directory; creating it now", 0, Debug.DebugColor.Green);
				Directory.CreateDirectory(text);
			}
			return text;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002DA9 File Offset: 0x00000FA9
		public static bool DoesSceneFolderAlreadyExist(string sceneName)
		{
			return Directory.Exists(Path.Combine(ModHelpers.GetSceneObjRootPath(), sceneName));
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002DBC File Offset: 0x00000FBC
		public static string GetTempFilePath(string anyIdentifier)
		{
			return Path.Combine(Path.GetTempPath(), "BL_" + anyIdentifier + "_" + Guid.NewGuid().ToString());
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002DF8 File Offset: 0x00000FF8
		public static string ReadSceneNameOfDirectory(string sceneDirectoryPath)
		{
			string text = null;
			using (StreamReader streamReader = new StreamReader(Path.Combine(sceneDirectoryPath, "scene.xscene")))
			{
				using (XmlReader xmlReader = XmlReader.Create(streamReader))
				{
					if (xmlReader.MoveToContent() == XmlNodeType.Element && xmlReader.Name == "scene")
					{
						text = xmlReader.GetAttribute("name");
					}
				}
				if (text == null)
				{
					throw new Exception("Couldn't retrieve name from 'scene.xscene'");
				}
				if (DedicatedCustomServerClientHelperSubModule.DebugMode)
				{
					text = text + "__" + Guid.NewGuid().ToString();
				}
			}
			return text;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002EB0 File Offset: 0x000010B0
		public static string WriteBufferToTempFile(byte[] buffer)
		{
			string tempFilePath = ModHelpers.GetTempFilePath("map_dl");
			using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
			{
				fileStream.Write(buffer, 0, buffer.Length);
				ModLogger.Log("Wrote buffer to temp file", 0, Debug.DebugColor.Green);
			}
			return tempFilePath;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002F08 File Offset: 0x00001108
		public static FileStream GetTempFileStream()
		{
			return new FileStream(ModHelpers.GetTempFilePath("map_dl"), FileMode.Create, FileAccess.Write);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002F1C File Offset: 0x0000111C
		public static string ExtractZipToTempDirectory(string sourceZipFilePath)
		{
			DirectoryInfo directoryInfo = Directory.CreateDirectory(Path.Combine(ModHelpers.GetSceneObjRootPath(), "temp_" + Guid.NewGuid().ToString()));
			using (ZipArchive zipArchive = ZipFile.OpenRead(sourceZipFilePath))
			{
				zipArchive.ExtractToDirectory(directoryInfo.FullName);
			}
			ModLogger.Log("Extracted zip to directory '" + directoryInfo.FullName + "'", 0, Debug.DebugColor.Green);
			return directoryInfo.FullName;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002FA8 File Offset: 0x000011A8
		public static async Task<string> DownloadToTempFile(HttpClient httpClient, string url, IProgress<ProgressUpdate> progress = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			string tempFilePath;
			using (FileStream tempFileStream = ModHelpers.GetTempFileStream())
			{
				tempFilePath = tempFileStream.Name;
				HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
				using (HttpResponseMessage response = httpResponseMessage)
				{
					if (!response.IsSuccessStatusCode)
					{
						string text = await response.Content.ReadAsStringAsync();
						throw new Exception(string.Format("Server responded with {0}: '{1}'", response.StatusCode, text));
					}
					long? contentLength = response.Content.Headers.ContentLength;
					using (Stream downloadStream = await response.Content.ReadAsStreamAsync())
					{
						if (progress == null || contentLength == null)
						{
							await downloadStream.CopyToAsync(tempFileStream);
						}
						else
						{
							byte[] buffer = new byte[81920];
							long totalBytesRead = 0L;
							int bytesRead;
							while ((bytesRead = await downloadStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) != 0)
							{
								await tempFileStream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
								totalBytesRead += (long)bytesRead;
								progress.Report(new ProgressUpdate(totalBytesRead, contentLength.Value));
							}
							buffer = null;
						}
					}
					Stream downloadStream = null;
					contentLength = null;
				}
				HttpResponseMessage response = null;
			}
			FileStream tempFileStream = null;
			return tempFilePath;
		}
	}
}
