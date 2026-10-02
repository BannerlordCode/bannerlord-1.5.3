using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x02000169 RID: 361
	public class BadgeCondition
	{
		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000A13 RID: 2579 RVA: 0x0000F91A File Offset: 0x0000DB1A
		// (set) Token: 0x06000A14 RID: 2580 RVA: 0x0000F922 File Offset: 0x0000DB22
		public ConditionType Type { get; private set; }

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0000F92B File Offset: 0x0000DB2B
		// (set) Token: 0x06000A16 RID: 2582 RVA: 0x0000F933 File Offset: 0x0000DB33
		public ConditionGroupType GroupType { get; private set; }

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x0000F93C File Offset: 0x0000DB3C
		// (set) Token: 0x06000A18 RID: 2584 RVA: 0x0000F944 File Offset: 0x0000DB44
		public TextObject Description { get; private set; }

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x0000F94D File Offset: 0x0000DB4D
		// (set) Token: 0x06000A1A RID: 2586 RVA: 0x0000F955 File Offset: 0x0000DB55
		public string StringId { get; private set; }

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x0000F95E File Offset: 0x0000DB5E
		// (set) Token: 0x06000A1C RID: 2588 RVA: 0x0000F966 File Offset: 0x0000DB66
		public IReadOnlyDictionary<string, string> Parameters { get; private set; }

		// Token: 0x06000A1D RID: 2589 RVA: 0x0000F970 File Offset: 0x0000DB70
		public BadgeCondition(int index, XmlNode node)
		{
			XmlAttributeCollection attributes = node.Attributes;
			ConditionType conditionType;
			if (!Enum.TryParse<ConditionType>((attributes != null) ? attributes["type"].Value : null, true, out conditionType))
			{
				conditionType = ConditionType.Custom;
				Debug.FailedAssert("No 'type' was provided for a condition", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeCondition.cs", ".ctor", 47);
			}
			this.Type = conditionType;
			ConditionGroupType conditionGroupType = ConditionGroupType.Any;
			XmlAttributeCollection attributes2 = node.Attributes;
			bool flag;
			if (attributes2 == null)
			{
				flag = null != null;
			}
			else
			{
				XmlAttribute xmlAttribute = attributes2["group_type"];
				flag = ((xmlAttribute != null) ? xmlAttribute.Value : null) != null;
			}
			if (flag && !Enum.TryParse<ConditionGroupType>(node.Attributes["group_type"].Value, true, out conditionGroupType))
			{
				Debug.FailedAssert("Provided 'group_type' was wrong for a condition", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeCondition.cs", ".ctor", 54);
			}
			this.GroupType = conditionGroupType;
			XmlAttributeCollection attributes3 = node.Attributes;
			this.Description = new TextObject((attributes3 != null) ? attributes3["description"].Value : null, null);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "Parameter")
				{
					string text = xmlNode.Attributes["name"].Value.Trim();
					string text2 = xmlNode.Attributes["value"].Value.Trim();
					dictionary[text] = text2;
				}
			}
			this.Parameters = dictionary;
			XmlAttributeCollection attributes4 = node.Attributes;
			string text3;
			if (attributes4 == null)
			{
				text3 = null;
			}
			else
			{
				XmlAttribute xmlAttribute2 = attributes4["id"];
				text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
			}
			this.StringId = text3;
			string text4;
			if (this.StringId == null && this.Parameters.TryGetValue("property", out text4))
			{
				this.StringId = text4 + ((this.GroupType == ConditionGroupType.Party) ? ".Party" : ((this.GroupType == ConditionGroupType.Solo) ? ".Solo" : ""));
			}
			if (this.StringId == null)
			{
				this.StringId = "condition." + index;
			}
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0000FB98 File Offset: 0x0000DD98
		public bool Check(string value)
		{
			ConditionType type = this.Type;
			if (type != ConditionType.PlayerData)
			{
				return false;
			}
			string text;
			if (!this.Parameters.TryGetValue("value", out text))
			{
				Debug.FailedAssert("Given condition doesn't have a value parameter", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeCondition.cs", "Check", 94);
				return false;
			}
			return value == text;
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0000FBE8 File Offset: 0x0000DDE8
		public bool Check(int value)
		{
			ConditionType type = this.Type;
			if (type - ConditionType.PlayerDataNumeric > 2)
			{
				return false;
			}
			string text;
			if (this.Parameters.TryGetValue("value", out text))
			{
				int num;
				if (!int.TryParse(text, out num))
				{
					Debug.FailedAssert("Given condition value parameter is not valid number", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeCondition.cs", "Check", 115);
					return false;
				}
				return value == num;
			}
			else
			{
				string text2;
				bool flag = this.Parameters.TryGetValue("min_value", out text2);
				string text3;
				bool flag2 = this.Parameters.TryGetValue("max_value", out text3);
				int minValue = int.MinValue;
				int maxValue = int.MaxValue;
				if (flag && !int.TryParse(text2, out minValue))
				{
					Debug.FailedAssert("Given condition min_value parameter is not valid number", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeCondition.cs", "Check", 129);
					return false;
				}
				if (flag2 && !int.TryParse(text3, out maxValue))
				{
					Debug.FailedAssert("Given condition max_value parameter is not valid number", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Diamond\\MultiplayerBadges\\BadgeCondition.cs", "Check", 134);
					return false;
				}
				return (flag || flag2) && value >= minValue && value <= maxValue;
			}
		}
	}
}
