using System;
using System.Xml;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000126 RID: 294
	internal class ItemInnerData
	{
		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x000088AD File Offset: 0x00006AAD
		// (set) Token: 0x060006A9 RID: 1705 RVA: 0x000088B5 File Offset: 0x00006AB5
		internal string TypeId { get; private set; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x000088BE File Offset: 0x00006ABE
		// (set) Token: 0x060006AB RID: 1707 RVA: 0x000088C6 File Offset: 0x00006AC6
		internal ItemType Type { get; private set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x000088CF File Offset: 0x00006ACF
		// (set) Token: 0x060006AD RID: 1709 RVA: 0x000088D7 File Offset: 0x00006AD7
		internal int Price { get; private set; }

		// Token: 0x060006AE RID: 1710 RVA: 0x000088E0 File Offset: 0x00006AE0
		internal void Deserialize(XmlNode node)
		{
			this.TypeId = node.Attributes["id"].Value;
			this.Price = ((node.Attributes["value"] != null) ? int.Parse(node.Attributes["value"].Value) : 0);
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "flags")
				{
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.Name == "flag" && xmlNode2.Attributes["name"].Value == "type")
						{
							string value = xmlNode2.Attributes["value"].Value;
							this.Type = (ItemType)Enum.Parse(typeof(ItemType), value, true);
						}
					}
				}
			}
		}
	}
}
