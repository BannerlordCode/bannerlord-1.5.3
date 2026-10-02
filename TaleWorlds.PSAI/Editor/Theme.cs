using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using psai.net;

namespace psai.Editor
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	public class Theme : PsaiMusicEntity, ICloneable
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x000059E6 File Offset: 0x00003BE6
		public static bool ConvertPlaycountVsRandomWeightingToBooleanPlaycountPreferred(float weightingPlaycountVsRandom)
		{
			return weightingPlaycountVsRandom >= Theme.PLAYCOUNT_VS_RANDOM_WEIGHTING_IF_PLAYCOUNT_PREFERRED;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x000059F3 File Offset: 0x00003BF3
		public override string GetClassString()
		{
			if (this.ThemeTypeInt == 6)
			{
				return "Highlight Layer";
			}
			return "Theme";
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00005A0C File Offset: 0x00003C0C
		public override List<PsaiMusicEntity> GetChildren()
		{
			List<PsaiMusicEntity> list = new List<PsaiMusicEntity>();
			for (int i = 0; i < this.Groups.Count; i++)
			{
				list.Add(this._groups[i]);
			}
			return list;
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00005A48 File Offset: 0x00003C48
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00005A50 File Offset: 0x00003C50
		public int Id
		{
			get
			{
				return this._id;
			}
			set
			{
				this._id = value;
				this.SetAsParentThemeForAllGroupsAndSegments();
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00005A5F File Offset: 0x00003C5F
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00005A67 File Offset: 0x00003C67
		public string Description { get; set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00005A70 File Offset: 0x00003C70
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00005A78 File Offset: 0x00003C78
		public int ThemeTypeInt { get; set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00005A81 File Offset: 0x00003C81
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00005A89 File Offset: 0x00003C89
		public List<int> Serialization_ManuallyBlockedThemeIds { get; set; }

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000FD RID: 253 RVA: 0x00005A9B File Offset: 0x00003C9B
		// (set) Token: 0x060000FC RID: 252 RVA: 0x00005A92 File Offset: 0x00003C92
		[XmlIgnore]
		public HashSet<Theme> ManuallyBlockedTargetThemes
		{
			get
			{
				return this._manuallyBlockedThemes;
			}
			private set
			{
				this._manuallyBlockedThemes = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00005AA3 File Offset: 0x00003CA3
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00005AAB File Offset: 0x00003CAB
		public float IntensityAfterRest
		{
			get
			{
				return this._intensityAfterRest;
			}
			set
			{
				this._intensityAfterRest = value;
				if (this._intensityAfterRest <= 0f)
				{
					this._intensityAfterRest = 0.01f;
				}
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00005ACC File Offset: 0x00003CCC
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00005AD4 File Offset: 0x00003CD4
		public int MusicPhaseSecondsAfterRest { get; set; }

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00005ADD File Offset: 0x00003CDD
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00005AE5 File Offset: 0x00003CE5
		public int MusicPhaseSecondsGeneral { get; set; }

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00005AEE File Offset: 0x00003CEE
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00005AF6 File Offset: 0x00003CF6
		public int RestSecondsMin { get; set; }

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00005AFF File Offset: 0x00003CFF
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00005B07 File Offset: 0x00003D07
		public int RestSecondsMax { get; set; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00005B10 File Offset: 0x00003D10
		// (set) Token: 0x06000109 RID: 265 RVA: 0x00005B18 File Offset: 0x00003D18
		public int FadeoutMs { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00005B21 File Offset: 0x00003D21
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00005B29 File Offset: 0x00003D29
		public int Priority { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00005B32 File Offset: 0x00003D32
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00005B3A File Offset: 0x00003D3A
		public float WeightingSwitchGroups { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00005B43 File Offset: 0x00003D43
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00005B4B File Offset: 0x00003D4B
		public float WeightingIntensityVsVariance { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00005B54 File Offset: 0x00003D54
		// (set) Token: 0x06000111 RID: 273 RVA: 0x00005B5C File Offset: 0x00003D5C
		public float WeightingLowPlaycountVsRandom { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00005B65 File Offset: 0x00003D65
		// (set) Token: 0x06000113 RID: 275 RVA: 0x00005B6D File Offset: 0x00003D6D
		public List<Group> Groups
		{
			get
			{
				return this._groups;
			}
			set
			{
				this._groups = value;
			}
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005B76 File Offset: 0x00003D76
		public Theme()
		{
			this.Initialize();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005B8F File Offset: 0x00003D8F
		public Theme(int id)
		{
			this.Initialize();
			this.Id = id;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005BAF File Offset: 0x00003DAF
		public Theme(int id, string name)
		{
			this.Initialize();
			this.Id = id;
			base.Name = name;
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005BD6 File Offset: 0x00003DD6
		public override PsaiMusicEntity GetParent()
		{
			return null;
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00005BDC File Offset: 0x00003DDC
		private void Initialize()
		{
			base.Name = Theme.DEFAULT_NAME;
			this.ThemeTypeInt = Theme.DEFAULT_THEMETYPEINT;
			this.IntensityAfterRest = Theme.DEFAULT_INTENSITY_AFTER_REST;
			this.MusicPhaseSecondsAfterRest = Theme.DEFAULT_THEME_DURATION_SECONDS;
			this.MusicPhaseSecondsGeneral = Theme.DEFAULT_THEME_DURATION_SECONDS_AFTER_REST;
			this.WeightingSwitchGroups = Theme.DEFAULT_WEIGHTING_COMPATIBILITY;
			this.WeightingIntensityVsVariance = Theme.DEFAULT_WEIGHTING_INTENSITY;
			this.WeightingLowPlaycountVsRandom = Theme.DEFAULT_WEIGHTING_LOW_PLAYCOUNT_VS_RANDOM;
			this.Priority = Theme.DEFAULT_PRIORITY;
			this.RestSecondsMin = Theme.DEFAULT_REST_SECONDS_MIN;
			this.RestSecondsMax = Theme.DEFAULT_REST_SECONDS_MAX;
			this.FadeoutMs = Theme.DEFAULT_FADEOUT_MS;
			this._groups = new List<Group>();
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00005C78 File Offset: 0x00003E78
		public override string ToString()
		{
			return "Theme '" + base.Name + "'";
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00005C90 File Offset: 0x00003E90
		public bool AddGroup(Group groupToAdd)
		{
			using (List<Group>.Enumerator enumerator = this._groups.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Name.Equals(groupToAdd.Name))
					{
						return false;
					}
				}
			}
			this._groups.Add(groupToAdd);
			foreach (Segment segment in this.GetSegmentsOfAllGroups())
			{
				segment.ThemeId = this.Id;
			}
			return true;
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005D48 File Offset: 0x00003F48
		public void DeleteGroup(Group group)
		{
			if (group != this._groups[0])
			{
				this._groups.Remove(group);
			}
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005D68 File Offset: 0x00003F68
		public HashSet<Segment> GetSegmentsOfAllGroups()
		{
			HashSet<Segment> hashSet = new HashSet<Segment>();
			foreach (Group group in this._groups)
			{
				foreach (Segment segment in group.Segments)
				{
					hashSet.Add(segment);
				}
			}
			return hashSet;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005DFC File Offset: 0x00003FFC
		public HashSet<string> GetAudioDataRelativeFilePathsUsedByThisTheme()
		{
			HashSet<string> hashSet = new HashSet<string>();
			foreach (Segment segment in this.GetSegmentsOfAllGroups())
			{
				if (!hashSet.Contains(segment.AudioData.FilePathRelativeToProjectDir))
				{
					hashSet.Add(segment.AudioData.FilePathRelativeToProjectDir);
				}
			}
			return hashSet;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005E74 File Offset: 0x00004074
		public override CompatibilitySetting GetCompatibilitySetting(PsaiMusicEntity targetEntity)
		{
			if (targetEntity is Theme && this.ManuallyBlockedTargetThemes.Contains((Theme)targetEntity))
			{
				return CompatibilitySetting.blocked;
			}
			return CompatibilitySetting.neutral;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005E94 File Offset: 0x00004094
		public override CompatibilityType GetCompatibilityType(PsaiMusicEntity targetEntity, out CompatibilityReason reason)
		{
			if (targetEntity is Theme)
			{
				Theme theme = targetEntity as Theme;
				ThemeInterruptionBehavior themeInterruptionBehavior = Theme.GetThemeInterruptionBehavior((ThemeType)this.ThemeTypeInt, (ThemeType)theme.ThemeTypeInt);
				if (Theme.ThemeInterruptionBehaviorRequiresEvaluationOfSegmentCompatibilities(themeInterruptionBehavior))
				{
					if (this.ManuallyBlockedTargetThemes.Contains(targetEntity as Theme))
					{
						reason = CompatibilityReason.manual_setting_within_same_hierarchy;
						return CompatibilityType.blocked_manually;
					}
					reason = CompatibilityReason.default_behavior_of_psai;
					return CompatibilityType.allowed_implicitly;
				}
				else if (themeInterruptionBehavior == ThemeInterruptionBehavior.never)
				{
					reason = CompatibilityReason.target_theme_will_never_interrupt_source;
					return CompatibilityType.logically_impossible;
				}
			}
			reason = CompatibilityReason.not_set;
			return CompatibilityType.undefined;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00005EF5 File Offset: 0x000040F5
		public override int GetIndexPositionWithinParentEntity(PsaiProject parentProject)
		{
			return parentProject.Themes.IndexOf(this);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00005F04 File Offset: 0x00004104
		public override bool PropertyDifferencesAffectCompatibilities(PsaiMusicEntity otherEntity)
		{
			if (otherEntity is Theme)
			{
				Theme theme = otherEntity as Theme;
				if (this.ThemeTypeInt != theme.ThemeTypeInt)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00005F34 File Offset: 0x00004134
		public void SetAsParentThemeForAllGroupsAndSegments()
		{
			foreach (Group group in this.Groups)
			{
				group.Theme = this;
			}
			foreach (Segment segment in this.GetSegmentsOfAllGroups())
			{
				segment.ThemeId = this.Id;
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005FCC File Offset: 0x000041CC
		public Theme CreatePsaiDotNetVersion()
		{
			return new Theme
			{
				id = this.Id,
				Name = base.Name,
				themeType = (ThemeType)this.ThemeTypeInt,
				intensityAfterRest = this.IntensityAfterRest,
				musicDurationGeneral = this.MusicPhaseSecondsGeneral,
				musicDurationAfterRest = this.MusicPhaseSecondsAfterRest,
				restSecondsMin = this.RestSecondsMin,
				restSecondsMax = this.RestSecondsMax,
				priority = this.Priority,
				weightings = 
				{
					switchGroups = this.WeightingSwitchGroups,
					intensityVsVariety = this.WeightingIntensityVsVariance,
					lowPlaycountVsRandom = this.WeightingLowPlaycountVsRandom
				}
			};
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00006080 File Offset: 0x00004280
		public static Theme getTestTheme1()
		{
			Theme theme = new Theme(1, "Forest");
			theme.ThemeTypeInt = 1;
			Group group = new Group(theme, "wald_streicher");
			Group group2 = new Group(theme, "wald_choir");
			Segment segment = new Segment(101, "wald_streicher_1", 1, 0.4f);
			Segment segment2 = new Segment(102, "wald_streicher_2", 2, 0.4f);
			Segment segment3 = new Segment(103, "wald_streicher_3", 2, 0.6f);
			Segment segment4 = new Segment(104, "wald_streicher_4", 4, 0.6f);
			Segment segment5 = new Segment(105, "wald_streicher_5", 1, 1f);
			Segment segment6 = new Segment(106, "wald_streicher_6", 4, 1f);
			Segment segment7 = new Segment(111, "wald_choir_1", 1, 0.4f);
			Segment segment8 = new Segment(112, "wald_choir_2", 2, 0.4f);
			Segment segment9 = new Segment(113, "wald_choir_3", 2, 0.6f);
			Segment segment10 = new Segment(114, "wald_choir_4", 1, 0.6f);
			Segment segment11 = new Segment(115, "wald_choir_5", 4, 1f);
			Segment segment12 = new Segment(116, "wald_choir_6", 4, 1f);
			group.AddSegment(segment);
			group.AddSegment(segment2);
			group.AddSegment(segment3);
			group.AddSegment(segment4);
			group.AddSegment(segment5);
			group.AddSegment(segment6);
			group2.AddSegment(segment7);
			group2.AddSegment(segment8);
			group2.AddSegment(segment9);
			group2.AddSegment(segment10);
			group2.AddSegment(segment11);
			group2.AddSegment(segment12);
			theme.AddGroup(group);
			theme.AddGroup(group2);
			return theme;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006214 File Offset: 0x00004414
		public static Theme getTestTheme2()
		{
			Theme theme = new Theme(2, "Cave");
			theme.ThemeTypeInt = 1;
			Group group = new Group(theme, "cave horns");
			Group group2 = new Group(theme, "cave choir");
			Segment segment = new Segment(201, "cave_horns_1", 1, 0.4f);
			Segment segment2 = new Segment(202, "cave_horns_2", 2, 0.4f);
			Segment segment3 = new Segment(203, "cave_horns_3", 2, 0.6f);
			Segment segment4 = new Segment(204, "cave_horns_4", 2, 0.6f);
			Segment segment5 = new Segment(205, "cave_horns_5", 2, 1f);
			Segment segment6 = new Segment(206, "cave_horns_6", 4, 1f);
			Segment segment7 = new Segment(211, "cave_choir_1", 1, 0.4f);
			Segment segment8 = new Segment(212, "cave_choir_2", 2, 0.4f);
			Segment segment9 = new Segment(213, "cave_choir_3", 2, 0.6f);
			Segment segment10 = new Segment(214, "cave_choir_4", 2, 0.6f);
			Segment segment11 = new Segment(215, "cave_choir_5", 2, 1f);
			Segment segment12 = new Segment(216, "cave_choir_6", 4, 1f);
			group.AddSegment(segment);
			group.AddSegment(segment2);
			group.AddSegment(segment3);
			group.AddSegment(segment4);
			group.AddSegment(segment5);
			group.AddSegment(segment6);
			group2.AddSegment(segment7);
			group2.AddSegment(segment8);
			group2.AddSegment(segment9);
			group2.AddSegment(segment10);
			group2.AddSegment(segment11);
			group2.AddSegment(segment12);
			theme.AddGroup(group);
			theme.AddGroup(group2);
			return theme;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000063CC File Offset: 0x000045CC
		public override object Clone()
		{
			Theme theme = (Theme)base.MemberwiseClone();
			theme.Groups = new List<Group>();
			theme._manuallyBlockedThemes = new HashSet<Theme>();
			foreach (Group group in this.Groups)
			{
				theme.AddGroup((Group)group.Clone());
			}
			foreach (Theme theme2 in this._manuallyBlockedThemes)
			{
				theme._manuallyBlockedThemes.Add(theme2);
			}
			return theme;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00006498 File Offset: 0x00004698
		public override PsaiMusicEntity ShallowCopy()
		{
			return (Theme)base.MemberwiseClone();
		}

		// Token: 0x04000059 RID: 89
		internal static float PLAYCOUNT_VS_RANDOM_WEIGHTING_IF_PLAYCOUNT_PREFERRED = 0.8f;

		// Token: 0x0400005A RID: 90
		private static readonly string DEFAULT_NAME = "new_theme";

		// Token: 0x0400005B RID: 91
		private static readonly int DEFAULT_PRIORITY = 1;

		// Token: 0x0400005C RID: 92
		private static readonly int DEFAULT_REST_SECONDS_MIN = 30;

		// Token: 0x0400005D RID: 93
		private static readonly int DEFAULT_REST_SECONDS_MAX = 60;

		// Token: 0x0400005E RID: 94
		private static readonly int DEFAULT_FADEOUT_MS = 20;

		// Token: 0x0400005F RID: 95
		private static readonly int DEFAULT_THEME_DURATION_SECONDS = 60;

		// Token: 0x04000060 RID: 96
		private static readonly float DEFAULT_INTENSITY_AFTER_REST = 0.5f;

		// Token: 0x04000061 RID: 97
		private static readonly int DEFAULT_THEME_DURATION_SECONDS_AFTER_REST = 40;

		// Token: 0x04000062 RID: 98
		private static readonly float DEFAULT_WEIGHTING_COMPATIBILITY = 0.5f;

		// Token: 0x04000063 RID: 99
		private static readonly float DEFAULT_WEIGHTING_INTENSITY = 0.5f;

		// Token: 0x04000064 RID: 100
		private static readonly float DEFAULT_WEIGHTING_LOW_PLAYCOUNT_VS_RANDOM = 0f;

		// Token: 0x04000065 RID: 101
		private static readonly int DEFAULT_THEMETYPEINT = 1;

		// Token: 0x04000066 RID: 102
		private List<Group> _groups;

		// Token: 0x04000067 RID: 103
		private HashSet<Theme> _manuallyBlockedThemes = new HashSet<Theme>();

		// Token: 0x04000068 RID: 104
		private float _intensityAfterRest;

		// Token: 0x04000069 RID: 105
		private int _id;
	}
}
