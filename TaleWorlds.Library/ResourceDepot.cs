using System;
using System.Collections.Generic;
using System.IO;

namespace TaleWorlds.Library
{
	// Token: 0x02000087 RID: 135
	public class ResourceDepot
	{
		// Token: 0x14000015 RID: 21
		// (add) Token: 0x060004E5 RID: 1253 RVA: 0x00011F20 File Offset: 0x00010120
		// (remove) Token: 0x060004E6 RID: 1254 RVA: 0x00011F58 File Offset: 0x00010158
		public event ResourceChangeEvent OnResourceChange;

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x00011F8D File Offset: 0x0001018D
		public MBReadOnlyList<ResourceDepotLocation> ResourceLocations
		{
			get
			{
				return this._resourceLocations;
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00011F95 File Offset: 0x00010195
		public ResourceDepot()
		{
			this._resourceLocations = new MBList<ResourceDepotLocation>();
			this._files = new Dictionary<string, ResourceDepotFile>();
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x00011FB4 File Offset: 0x000101B4
		public void AddLocation(string basePath, string location)
		{
			basePath = basePath.Replace('\\', '/');
			location = location.Replace('\\', '/');
			ResourceDepotLocation resourceDepotLocation = new ResourceDepotLocation(basePath, location, basePath + location);
			this._resourceLocations.Add(resourceDepotLocation);
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x00011FF4 File Offset: 0x000101F4
		public void CollectResources()
		{
			this._files.Clear();
			foreach (ResourceDepotLocation resourceDepotLocation in this._resourceLocations)
			{
				Debug.Print("ResourceDepot:CollectResources: " + resourceDepotLocation.FullPath + "\n", 0, Debug.DebugColor.White, 17592186044416UL);
				string fullPath = resourceDepotLocation.FullPath;
				foreach (string text in Directory.GetFiles(resourceDepotLocation.BasePath + resourceDepotLocation.Path, "*", SearchOption.AllDirectories))
				{
					text = text.Replace('\\', '/');
					string text2 = text.Replace('\\', '/').Substring(fullPath.Length);
					string text3 = text2.ToLower();
					ResourceDepotFile resourceDepotFile = new ResourceDepotFile(resourceDepotLocation, text2, text);
					if (this._files.ContainsKey(text3))
					{
						this._files[text3] = resourceDepotFile;
					}
					else
					{
						this._files.Add(text3, resourceDepotFile);
					}
				}
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001211C File Offset: 0x0001031C
		public string[] GetFiles(string subDirectory, string extension, bool excludeSubContents = false)
		{
			string text = extension.ToLower();
			List<string> list = new List<string>();
			foreach (ResourceDepotFile resourceDepotFile in this._files.Values)
			{
				string text2 = (resourceDepotFile.BasePath + resourceDepotFile.Location + subDirectory).Replace('\\', '/').ToLower();
				string fullPath = resourceDepotFile.FullPath;
				string fullPathLowerCase = resourceDepotFile.FullPathLowerCase;
				bool flag = (!excludeSubContents && fullPathLowerCase.StartsWith(text2)) || (excludeSubContents && string.Equals(Directory.GetParent(text2).FullName, text2, StringComparison.CurrentCultureIgnoreCase));
				bool flag2 = fullPathLowerCase.EndsWith(text, StringComparison.OrdinalIgnoreCase);
				if (flag && flag2)
				{
					list.Add(fullPath);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000121F8 File Offset: 0x000103F8
		public string GetFilePath(string file)
		{
			file = file.Replace('\\', '/');
			return this._files[file.ToLower()].FullPath;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001221C File Offset: 0x0001041C
		public IEnumerable<string> GetFilesEndingWith(string fileEndName)
		{
			fileEndName = fileEndName.Replace('\\', '/');
			foreach (KeyValuePair<string, ResourceDepotFile> keyValuePair in this._files)
			{
				if (keyValuePair.Key.EndsWith(fileEndName.ToLower()))
				{
					yield return keyValuePair.Value.FullPath;
				}
			}
			Dictionary<string, ResourceDepotFile>.Enumerator enumerator = default(Dictionary<string, ResourceDepotFile>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00012234 File Offset: 0x00010434
		public void StartWatchingChangesInDepot()
		{
			foreach (ResourceDepotLocation resourceDepotLocation in this._resourceLocations)
			{
				resourceDepotLocation.StartWatchingChanges(new FileSystemEventHandler(this.OnAnyChangeInDepotLocations), new RenamedEventHandler(this.OnAnyRenameInDepotLocations));
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001229C File Offset: 0x0001049C
		public void StopWatchingChangesInDepot()
		{
			foreach (ResourceDepotLocation resourceDepotLocation in this._resourceLocations)
			{
				resourceDepotLocation.StopWatchingChanges();
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x000122EC File Offset: 0x000104EC
		private void OnAnyChangeInDepotLocations(object source, FileSystemEventArgs e)
		{
			this._isThereAnyUnhandledChange = true;
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x000122F5 File Offset: 0x000104F5
		private void OnAnyRenameInDepotLocations(object source, RenamedEventArgs e)
		{
			this._isThereAnyUnhandledChange = true;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x000122FE File Offset: 0x000104FE
		public void CheckForChanges()
		{
			if (this._isThereAnyUnhandledChange)
			{
				this.CollectResources();
				ResourceChangeEvent onResourceChange = this.OnResourceChange;
				if (onResourceChange != null)
				{
					onResourceChange();
				}
				this._isThereAnyUnhandledChange = false;
			}
		}

		// Token: 0x0400017B RID: 379
		private readonly MBList<ResourceDepotLocation> _resourceLocations;

		// Token: 0x0400017C RID: 380
		private readonly Dictionary<string, ResourceDepotFile> _files;

		// Token: 0x0400017D RID: 381
		private bool _isThereAnyUnhandledChange;
	}
}
