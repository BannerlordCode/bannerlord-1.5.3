using System;

namespace TaleWorlds.Library
{
	// Token: 0x02000088 RID: 136
	public class ResourceDepotFile
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060004F3 RID: 1267 RVA: 0x00012326 File Offset: 0x00010526
		// (set) Token: 0x060004F4 RID: 1268 RVA: 0x0001232E File Offset: 0x0001052E
		public ResourceDepotLocation ResourceDepotLocation { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060004F5 RID: 1269 RVA: 0x00012337 File Offset: 0x00010537
		public string BasePath
		{
			get
			{
				return this.ResourceDepotLocation.BasePath;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x00012344 File Offset: 0x00010544
		public string Location
		{
			get
			{
				return this.ResourceDepotLocation.Path;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060004F7 RID: 1271 RVA: 0x00012351 File Offset: 0x00010551
		// (set) Token: 0x060004F8 RID: 1272 RVA: 0x00012359 File Offset: 0x00010559
		public string FileName { get; private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00012362 File Offset: 0x00010562
		// (set) Token: 0x060004FA RID: 1274 RVA: 0x0001236A File Offset: 0x0001056A
		public string FullPath { get; private set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x00012373 File Offset: 0x00010573
		// (set) Token: 0x060004FC RID: 1276 RVA: 0x0001237B File Offset: 0x0001057B
		public string FullPathLowerCase { get; private set; }

		// Token: 0x060004FD RID: 1277 RVA: 0x00012384 File Offset: 0x00010584
		public ResourceDepotFile(ResourceDepotLocation resourceDepotLocation, string fileName, string fullPath)
		{
			this.ResourceDepotLocation = resourceDepotLocation;
			this.FileName = fileName;
			this.FullPath = fullPath;
			this.FullPathLowerCase = fullPath.ToLower();
		}
	}
}
