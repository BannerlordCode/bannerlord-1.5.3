using System;
using System.Globalization;
using System.Xml;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges
{
	// Token: 0x02000165 RID: 357
	public class Badge
	{
		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060009FF RID: 2559 RVA: 0x0000F6E5 File Offset: 0x0000D8E5
		public int Index { get; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0000F6ED File Offset: 0x0000D8ED
		public BadgeType Type { get; }

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0000F6F5 File Offset: 0x0000D8F5
		// (set) Token: 0x06000A02 RID: 2562 RVA: 0x0000F6FD File Offset: 0x0000D8FD
		public string StringId { get; private set; }

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000A03 RID: 2563 RVA: 0x0000F706 File Offset: 0x0000D906
		// (set) Token: 0x06000A04 RID: 2564 RVA: 0x0000F70E File Offset: 0x0000D90E
		public string GroupId { get; private set; }

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x0000F717 File Offset: 0x0000D917
		// (set) Token: 0x06000A06 RID: 2566 RVA: 0x0000F71F File Offset: 0x0000D91F
		public TextObject Name { get; private set; }

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x0000F728 File Offset: 0x0000D928
		// (set) Token: 0x06000A08 RID: 2568 RVA: 0x0000F730 File Offset: 0x0000D930
		public TextObject Description { get; private set; }

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000A09 RID: 2569 RVA: 0x0000F739 File Offset: 0x0000D939
		// (set) Token: 0x06000A0A RID: 2570 RVA: 0x0000F741 File Offset: 0x0000D941
		public bool IsVisibleOnlyWhenEarned { get; private set; }

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x0000F74A File Offset: 0x0000D94A
		// (set) Token: 0x06000A0C RID: 2572 RVA: 0x0000F752 File Offset: 0x0000D952
		public DateTime PeriodStart { get; private set; }

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0000F75B File Offset: 0x0000D95B
		// (set) Token: 0x06000A0E RID: 2574 RVA: 0x0000F763 File Offset: 0x0000D963
		public DateTime PeriodEnd { get; private set; }

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x0000F76C File Offset: 0x0000D96C
		public bool IsActive
		{
			get
			{
				return DateTime.UtcNow >= this.PeriodStart && DateTime.UtcNow <= this.PeriodEnd;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0000F792 File Offset: 0x0000D992
		public bool IsTimed
		{
			get
			{
				return this.PeriodStart > DateTime.MinValue || this.PeriodEnd < DateTime.MaxValue;
			}
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0000F7B8 File Offset: 0x0000D9B8
		public Badge(int index, BadgeType badgeType)
		{
			this.Index = index;
			this.Type = badgeType;
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0000F7D0 File Offset: 0x0000D9D0
		public virtual void Deserialize(XmlNode node)
		{
			this.StringId = node.Attributes["id"].Value;
			XmlAttributeCollection attributes = node.Attributes;
			string text;
			if (attributes == null)
			{
				text = null;
			}
			else
			{
				XmlAttribute xmlAttribute = attributes["group_id"];
				text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
			}
			string text2 = text;
			this.GroupId = (string.IsNullOrWhiteSpace(text2) ? null : text2);
			string value = node.Attributes["name"].Value;
			string value2 = node.Attributes["description"].Value;
			XmlAttribute xmlAttribute2 = node.Attributes["is_visible_only_when_earned"];
			this.IsVisibleOnlyWhenEarned = Convert.ToBoolean((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
			XmlAttribute xmlAttribute3 = node.Attributes["period_start"];
			DateTime dateTime;
			this.PeriodStart = (DateTime.TryParse((xmlAttribute3 != null) ? xmlAttribute3.Value : null, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime) ? DateTime.SpecifyKind(dateTime, DateTimeKind.Utc) : DateTime.MinValue);
			XmlAttribute xmlAttribute4 = node.Attributes["period_end"];
			DateTime dateTime2;
			this.PeriodEnd = (DateTime.TryParse((xmlAttribute4 != null) ? xmlAttribute4.Value : null, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime2) ? DateTime.SpecifyKind(dateTime2, DateTimeKind.Utc) : DateTime.MaxValue);
			this.Name = new TextObject(value, null);
			this.Description = new TextObject(value2, null);
		}
	}
}
