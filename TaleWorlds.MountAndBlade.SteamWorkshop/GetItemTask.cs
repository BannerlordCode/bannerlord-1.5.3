using System;
using System.Xml;
using Steamworks;

namespace TaleWorlds.MountAndBlade.SteamWorkshop
{
	// Token: 0x02000003 RID: 3
	public class GetItemTask : ToolTask
	{
		// Token: 0x06000005 RID: 5 RVA: 0x00002138 File Offset: 0x00000338
		public override void LoadFrom(XmlNode xmlNode)
		{
			foreach (object obj in xmlNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.Name == "ItemId")
				{
					this._itemIdAsString = xmlNode2.Attributes["Value"].Value;
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000021B8 File Offset: 0x000003B8
		public override void DoJob()
		{
			Program.ItemId = new PublishedFileId_t(Convert.ToUInt64(this._itemIdAsString));
		}

		// Token: 0x04000002 RID: 2
		private string _itemIdAsString = "";
	}
}
