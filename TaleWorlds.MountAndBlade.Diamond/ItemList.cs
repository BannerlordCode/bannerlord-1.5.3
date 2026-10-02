using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade.Diamond
{
	// Token: 0x02000125 RID: 293
	internal class ItemList
	{
		// Token: 0x060006A3 RID: 1699 RVA: 0x00008720 File Offset: 0x00006920
		static ItemList()
		{
			XmlDocument xmlDocument = new XmlDocument();
			string text = ModuleHelper.GetModuleFullPath("Native") + "ModuleData/mpitems.xml";
			if (ConfigurationManager.GetAppSettings("MultiplayerItemsFileName") != null)
			{
				text = ConfigurationManager.GetAppSettings("MultiplayerItemsFileName");
			}
			xmlDocument.Load(text);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("Items");
			if (xmlNode == null)
			{
				throw new Exception("'Items' node is not defined in mpitems.xml");
			}
			Debug.Print("---" + xmlNode.Name, 0, Debug.DebugColor.White, 17592186044416UL);
			foreach (object obj in xmlNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.NodeType == XmlNodeType.Element)
				{
					ItemInnerData itemInnerData = new ItemInnerData();
					itemInnerData.Deserialize(xmlNode2);
					Debug.Print(itemInnerData.TypeId, 0, Debug.DebugColor.White, 17592186044416UL);
					if (!ItemList._items.ContainsKey(itemInnerData.TypeId))
					{
						ItemList._items.Add(itemInnerData.TypeId, itemInnerData);
					}
					else
					{
						Debug.Print("--- Item type id already exists, check mpitems.xml for item type Id:" + itemInnerData.TypeId, 0, Debug.DebugColor.White, 17592186044416UL);
					}
				}
			}
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00008874 File Offset: 0x00006A74
		internal static ItemType GetItemTypeOf(string typeId)
		{
			return ItemList._items[typeId].Type;
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00008886 File Offset: 0x00006A86
		internal static bool IsItemValid(string itemId, string modifierId)
		{
			return ItemList._items.ContainsKey(itemId);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x00008893 File Offset: 0x00006A93
		internal static int GetPriceOf(string itemId, string modifierId)
		{
			return ItemList._items[itemId].Price;
		}

		// Token: 0x040002F9 RID: 761
		private static Dictionary<string, ItemInnerData> _items = new Dictionary<string, ItemInnerData>();
	}
}
