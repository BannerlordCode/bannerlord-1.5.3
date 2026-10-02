using System;
using System.Collections.Generic;
using System.Xml;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x0200016C RID: 364
	public class ConditionalBadge : Badge
	{
		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x000104FE File Offset: 0x0000E6FE
		// (set) Token: 0x06000A30 RID: 2608 RVA: 0x00010506 File Offset: 0x0000E706
		public IReadOnlyList<BadgeCondition> BadgeConditions { get; private set; }

		// Token: 0x06000A31 RID: 2609 RVA: 0x0001050F File Offset: 0x0000E70F
		public ConditionalBadge(int index, BadgeType badgeType)
			: base(index, badgeType)
		{
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0001051C File Offset: 0x0000E71C
		public override void Deserialize(XmlNode node)
		{
			base.Deserialize(node);
			List<BadgeCondition> list = new List<BadgeCondition>();
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.Name == "Condition")
				{
					BadgeCondition badgeCondition = new BadgeCondition(list.Count, xmlNode);
					list.Add(badgeCondition);
				}
			}
			this.BadgeConditions = list;
		}
	}
}
