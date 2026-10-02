using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Settlements.Locations
{
	// Token: 0x020003E2 RID: 994
	public sealed class LocationComplexTemplate : MBObjectBase
	{
		// Token: 0x06003BF2 RID: 15346 RVA: 0x000F2E9C File Offset: 0x000F109C
		public override void Deserialize(MBObjectManager objectManager, XmlNode node)
		{
			base.Deserialize(objectManager, node);
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "Location")
				{
					if (xmlNode.Attributes == null)
					{
						throw new TWXmlLoadException("node.Attributes != null");
					}
					string value = xmlNode.Attributes["id"].Value;
					TextObject textObject = new TextObject(xmlNode.Attributes["name"].Value, null);
					string[] array = new string[]
					{
						(xmlNode.Attributes["scene_name"] != null) ? xmlNode.Attributes["scene_name"].Value : "",
						(xmlNode.Attributes["scene_name_1"] != null) ? xmlNode.Attributes["scene_name_1"].Value : "",
						(xmlNode.Attributes["scene_name_2"] != null) ? xmlNode.Attributes["scene_name_2"].Value : "",
						(xmlNode.Attributes["scene_name_3"] != null) ? xmlNode.Attributes["scene_name_3"].Value : ""
					};
					int num = int.Parse(xmlNode.Attributes["max_prosperity"].Value);
					bool flag = bool.Parse(xmlNode.Attributes["indoor"].Value);
					bool flag2 = xmlNode.Attributes["can_be_reserved"] != null && bool.Parse(xmlNode.Attributes["can_be_reserved"].Value);
					string innerText = xmlNode.Attributes["player_can_enter"].InnerText;
					string innerText2 = xmlNode.Attributes["player_can_see"].InnerText;
					string innerText3 = xmlNode.Attributes["ai_can_exit"].InnerText;
					string innerText4 = xmlNode.Attributes["ai_can_enter"].InnerText;
					this.Locations.Add(new Location(value, textObject, textObject, num, flag, flag2, innerText, innerText2, innerText3, innerText4, array, null));
				}
				if (xmlNode.Name == "Passages")
				{
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (xmlNode2.Name == "Passage")
						{
							if (xmlNode2.Attributes == null)
							{
								throw new TWXmlLoadException("node.Attributes != null");
							}
							string value2 = xmlNode2.Attributes["location_1"].Value;
							string value3 = xmlNode2.Attributes["location_2"].Value;
							this.Passages.Add(new KeyValuePair<string, string>(value2, value3));
						}
					}
				}
			}
			foreach (KeyValuePair<string, string> keyValuePair in this.Passages)
			{
			}
		}

		// Token: 0x0400124A RID: 4682
		public List<Location> Locations = new List<Location>();

		// Token: 0x0400124B RID: 4683
		public List<KeyValuePair<string, string>> Passages = new List<KeyValuePair<string, string>>();
	}
}
