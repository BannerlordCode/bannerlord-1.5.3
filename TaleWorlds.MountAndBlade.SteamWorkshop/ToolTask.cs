using System;
using System.Xml;

namespace TaleWorlds.MountAndBlade.SteamWorkshop
{
	// Token: 0x02000006 RID: 6
	public abstract class ToolTask
	{
		// Token: 0x0600002C RID: 44
		public abstract void LoadFrom(XmlNode xmlNode);

		// Token: 0x0600002D RID: 45
		public abstract void DoJob();
	}
}
