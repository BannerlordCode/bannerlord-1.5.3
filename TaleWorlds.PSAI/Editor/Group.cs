using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace psai.Editor
{
	// Token: 0x02000003 RID: 3
	[Serializable]
	public class Group : PsaiMusicEntity, ICloneable
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000027B0 File Offset: 0x000009B0
		// (set) Token: 0x0600002C RID: 44 RVA: 0x000027B8 File Offset: 0x000009B8
		public List<Segment> Segments
		{
			get
			{
				return this.m_segments;
			}
			set
			{
				this.m_segments = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600002D RID: 45 RVA: 0x000027C1 File Offset: 0x000009C1
		// (set) Token: 0x0600002E RID: 46 RVA: 0x000027C9 File Offset: 0x000009C9
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				this._description = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600002F RID: 47 RVA: 0x000027D2 File Offset: 0x000009D2
		// (set) Token: 0x06000030 RID: 48 RVA: 0x000027DA File Offset: 0x000009DA
		public int Serialization_Id { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000031 RID: 49 RVA: 0x000027E3 File Offset: 0x000009E3
		// (set) Token: 0x06000032 RID: 50 RVA: 0x000027EB File Offset: 0x000009EB
		public List<int> Serialization_ManuallyBlockedGroupIds { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000033 RID: 51 RVA: 0x000027F4 File Offset: 0x000009F4
		// (set) Token: 0x06000034 RID: 52 RVA: 0x000027FC File Offset: 0x000009FC
		public List<int> Serialization_ManuallyLinkedGroupIds { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000035 RID: 53 RVA: 0x00002805 File Offset: 0x00000A05
		// (set) Token: 0x06000036 RID: 54 RVA: 0x0000280D File Offset: 0x00000A0D
		public List<int> Serialization_ManualBridgeSegmentIds { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000037 RID: 55 RVA: 0x00002816 File Offset: 0x00000A16
		// (set) Token: 0x06000038 RID: 56 RVA: 0x0000281E File Offset: 0x00000A1E
		[XmlIgnore]
		public HashSet<Segment> ManualBridgeSnippetsOfTargetGroups
		{
			get
			{
				return this._manualBridgeSnippetsOfTargetGroups;
			}
			set
			{
				this._manualBridgeSnippetsOfTargetGroups = value;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002830 File Offset: 0x00000A30
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002827 File Offset: 0x00000A27
		[XmlIgnore]
		public HashSet<Group> ManuallyBlockedGroups
		{
			get
			{
				return this._manuallyBlockedGroups;
			}
			set
			{
				this._manuallyBlockedGroups = value;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002841 File Offset: 0x00000A41
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002838 File Offset: 0x00000A38
		[XmlIgnore]
		public HashSet<Group> ManuallyLinkedGroups
		{
			get
			{
				return this._manuallyLinkedGroups;
			}
			set
			{
				this._manuallyLinkedGroups = value;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00002852 File Offset: 0x00000A52
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002849 File Offset: 0x00000A49
		[XmlIgnore]
		public Theme Theme { get; set; }

		// Token: 0x0600003F RID: 63 RVA: 0x0000285A File Offset: 0x00000A5A
		public override string GetClassString()
		{
			return "Group";
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002864 File Offset: 0x00000A64
		public Group()
		{
			base.Name = "new_group";
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000028B9 File Offset: 0x00000AB9
		public Group(Theme parentTheme, string name)
			: this()
		{
			this.Theme = parentTheme;
			base.Name = name;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000028CF File Offset: 0x00000ACF
		public Group(Theme parentTheme)
			: this()
		{
			this.Theme = parentTheme;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x000028E0 File Offset: 0x00000AE0
		~Group()
		{
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002908 File Offset: 0x00000B08
		public void AddSegment(Segment snippet)
		{
			this.AddSnippet_internal(snippet, -1);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002912 File Offset: 0x00000B12
		public void AddSegment(Segment snippet, int index)
		{
			this.AddSnippet_internal(snippet, index);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000291C File Offset: 0x00000B1C
		private void AddSnippet_internal(Segment snippet, int insertIndex)
		{
			snippet.Group = this;
			if (this.Theme != null)
			{
				snippet.ThemeId = this.Theme.Id;
			}
			if (insertIndex < 0 || insertIndex >= this.m_segments.Count)
			{
				this.m_segments.Add(snippet);
				return;
			}
			this.m_segments.Insert(insertIndex, snippet);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002975 File Offset: 0x00000B75
		public void RemoveSegment(Segment snippet)
		{
			this.m_segments.Remove(snippet);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002984 File Offset: 0x00000B84
		public bool HasAtLeastOneBridgeSegmentToTargetGroup(Group targetGroup)
		{
			using (HashSet<Segment>.Enumerator enumerator = this._manualBridgeSnippetsOfTargetGroups.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Group == targetGroup)
					{
						return true;
					}
				}
			}
			return targetGroup.ContainsAtLeastOneAutomaticBridgeSegment();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000029E4 File Offset: 0x00000BE4
		public bool ContainsAtLeastOneManualBridgeSegmentForSourceGroup(Group sourceGroup)
		{
			using (HashSet<Segment>.Enumerator enumerator = sourceGroup.ManualBridgeSnippetsOfTargetGroups.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Group == this)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002A40 File Offset: 0x00000C40
		public bool ContainsAtLeastOneAutomaticBridgeSegment()
		{
			using (List<Segment>.Enumerator enumerator = this.Segments.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsAutomaticBridgeSegment)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002A9C File Offset: 0x00000C9C
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Group '",
				base.Name,
				"' (",
				this.Theme.Name,
				")"
			});
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002AD8 File Offset: 0x00000CD8
		public override CompatibilitySetting GetCompatibilitySetting(PsaiMusicEntity targetEntity)
		{
			if (targetEntity is Group)
			{
				if (this.ManuallyBlockedGroups.Contains((Group)targetEntity))
				{
					return CompatibilitySetting.blocked;
				}
				if (this.ManuallyLinkedGroups.Contains((Group)targetEntity))
				{
					return CompatibilitySetting.allowed;
				}
			}
			return CompatibilitySetting.neutral;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002B10 File Offset: 0x00000D10
		public override CompatibilityType GetCompatibilityType(PsaiMusicEntity targetEntity, out CompatibilityReason reason)
		{
			if (!(targetEntity is Group))
			{
				reason = CompatibilityReason.not_set;
				return CompatibilityType.undefined;
			}
			Group group = (Group)targetEntity;
			PsaiMusicEntity theme = this.Theme;
			Theme theme2 = group.Theme;
			if (theme.GetCompatibilityType(theme2, out reason) == CompatibilityType.logically_impossible)
			{
				return CompatibilityType.logically_impossible;
			}
			if (this.ManuallyBlockedGroups.Contains(group))
			{
				reason = CompatibilityReason.manual_setting_within_same_hierarchy;
				return CompatibilityType.blocked_manually;
			}
			if (this.ManuallyLinkedGroups.Contains(group))
			{
				reason = CompatibilityReason.manual_setting_within_same_hierarchy;
				return CompatibilityType.allowed_manually;
			}
			CompatibilityType compatibilityType = this.Theme.GetCompatibilityType(group.Theme, out reason);
			if (compatibilityType == CompatibilityType.allowed_manually)
			{
				reason = CompatibilityReason.manual_setting_of_parent_entity;
				return CompatibilityType.allowed_implicitly;
			}
			if (compatibilityType == CompatibilityType.blocked_manually)
			{
				reason = CompatibilityReason.manual_setting_of_parent_entity;
				return CompatibilityType.blocked_implicitly;
			}
			reason = CompatibilityReason.inherited_from_parent_hierarchy;
			return compatibilityType;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002B9C File Offset: 0x00000D9C
		public void SetAsParentGroupForAllSegments()
		{
			foreach (Segment segment in this.Segments)
			{
				segment.Group = this;
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002BF0 File Offset: 0x00000DF0
		public override PsaiMusicEntity GetParent()
		{
			return this.Theme;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002BF8 File Offset: 0x00000DF8
		public override List<PsaiMusicEntity> GetChildren()
		{
			List<PsaiMusicEntity> list = new List<PsaiMusicEntity>();
			list.Capacity = this.m_segments.Count;
			for (int i = 0; i < this.m_segments.Count; i++)
			{
				list.Add(this.m_segments[i]);
			}
			return list;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002C45 File Offset: 0x00000E45
		public override int GetIndexPositionWithinParentEntity(PsaiProject parentProject)
		{
			return this.Theme.Groups.IndexOf(this);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002C58 File Offset: 0x00000E58
		public override object Clone()
		{
			Group group = (Group)base.MemberwiseClone();
			group.Segments = new List<Segment>();
			group.ManualBridgeSnippetsOfTargetGroups = new HashSet<Segment>();
			group.ManuallyBlockedGroups = new HashSet<Group>();
			group.ManuallyLinkedGroups = new HashSet<Group>();
			foreach (Segment segment in this.Segments)
			{
				group.AddSegment((Segment)segment.Clone());
			}
			group.ManuallyBlockedGroups = new HashSet<Group>();
			group.ManuallyLinkedGroups = new HashSet<Group>();
			foreach (Group group2 in this.ManuallyBlockedGroups)
			{
				group.ManuallyBlockedGroups.Add(group2);
			}
			foreach (Group group3 in this.ManuallyLinkedGroups)
			{
				group.ManuallyLinkedGroups.Add(group3);
			}
			return group;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002D98 File Offset: 0x00000F98
		public override PsaiMusicEntity ShallowCopy()
		{
			Group group = (Group)base.MemberwiseClone();
			foreach (Segment segment in this.Segments)
			{
				segment.Group = group;
			}
			return group;
		}

		// Token: 0x0400000F RID: 15
		[NonSerialized]
		private List<Segment> m_segments = new List<Segment>();

		// Token: 0x04000010 RID: 16
		[NonSerialized]
		private HashSet<Segment> _manualBridgeSnippetsOfTargetGroups = new HashSet<Segment>();

		// Token: 0x04000011 RID: 17
		[NonSerialized]
		private HashSet<Group> _manuallyBlockedGroups = new HashSet<Group>();

		// Token: 0x04000012 RID: 18
		[NonSerialized]
		private HashSet<Group> _manuallyLinkedGroups = new HashSet<Group>();

		// Token: 0x04000013 RID: 19
		private string _description = "";
	}
}
