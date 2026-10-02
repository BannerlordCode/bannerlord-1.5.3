using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using psai.net;

namespace psai.Editor
{
	// Token: 0x02000008 RID: 8
	public class PsaiProject : ICloneable
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00002E50 File Offset: 0x00001050
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00002E58 File Offset: 0x00001058
		public string InitialExportDirectory { get; set; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00002E61 File Offset: 0x00001061
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00002E69 File Offset: 0x00001069
		public string SerializedByProtocolVersion { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00002E72 File Offset: 0x00001072
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00002E7A File Offset: 0x0000107A
		public ProjectProperties Properties
		{
			get
			{
				return this._projectProperties;
			}
			set
			{
				this._projectProperties = value;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00002E83 File Offset: 0x00001083
		// (set) Token: 0x06000068 RID: 104 RVA: 0x00002E8B File Offset: 0x0000108B
		public List<Theme> Themes
		{
			get
			{
				return this._themes;
			}
			set
			{
				this._themes = value;
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002E94 File Offset: 0x00001094
		public void Init()
		{
			this._projectProperties = new ProjectProperties();
			this._themes.Clear();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002EAC File Offset: 0x000010AC
		public static PsaiProject LoadProjectFromStream(StreamReader reader, string path)
		{
			PsaiProject psaiProject = null;
			try
			{
				psaiProject = (PsaiProject)PsaiProject._serializer.Deserialize(reader);
				reader.Close();
				psaiProject.ReconstructIds(path);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return psaiProject;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00002EF0 File Offset: 0x000010F0
		public static PsaiProject LoadProjectFromXmlFile(string filename)
		{
			PsaiProject psaiProject;
			try
			{
				StreamReader streamReader = new StreamReader(filename);
				if (streamReader != null)
				{
					psaiProject = PsaiProject.LoadProjectFromStream(streamReader, filename);
				}
				else
				{
					psaiProject = null;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return psaiProject;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00002F28 File Offset: 0x00001128
		public void SaveAsXmlFile(string filename)
		{
			this.PrepareForXmlSerialization();
			try
			{
				TextWriter textWriter = new StreamWriter(filename);
				PsaiProject._serializer.Serialize(textWriter, this);
				textWriter.Close();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00002F68 File Offset: 0x00001168
		public void Report(bool reportGroups, bool reportSegments)
		{
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00002F6C File Offset: 0x0000116C
		public bool ConvertProjectFile_From_Legacy_To_0_9_12(string pathToProjectFile)
		{
			if (File.Exists(pathToProjectFile))
			{
				Stream stream = new FileStream(pathToProjectFile, FileMode.Open, FileAccess.Read);
				XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
				using (XmlReader.Create(stream, xmlReaderSettings))
				{
				}
			}
			return false;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002FB4 File Offset: 0x000011B4
		public void ReconstructReferencesAfterXmlDeserialization()
		{
			foreach (Theme theme in this._themes)
			{
				foreach (Group group in theme.Groups)
				{
					group.Theme = theme;
					foreach (Segment segment in group.Segments)
					{
						segment.Group = group;
					}
				}
			}
			foreach (Theme theme2 in this._themes)
			{
				theme2.ManuallyBlockedTargetThemes.Clear();
				foreach (int num in theme2.Serialization_ManuallyBlockedThemeIds)
				{
					Theme themeById = this.GetThemeById(num);
					if (themeById != null)
					{
						theme2.ManuallyBlockedTargetThemes.Add(themeById);
					}
				}
				foreach (Group group2 in theme2.Groups)
				{
					group2.ManuallyBlockedGroups.Clear();
					if (group2.Serialization_ManuallyBlockedGroupIds == null)
					{
						group2.Serialization_ManuallyBlockedGroupIds = new List<int>();
					}
					foreach (int num2 in group2.Serialization_ManuallyBlockedGroupIds)
					{
						Group groupBySerializationId = this.GetGroupBySerializationId(num2);
						if (groupBySerializationId != null)
						{
							group2.ManuallyBlockedGroups.Add(groupBySerializationId);
						}
					}
					group2.ManuallyLinkedGroups.Clear();
					if (group2.Serialization_ManuallyLinkedGroupIds == null)
					{
						group2.Serialization_ManuallyLinkedGroupIds = new List<int>();
					}
					foreach (int num3 in group2.Serialization_ManuallyLinkedGroupIds)
					{
						Group groupBySerializationId2 = this.GetGroupBySerializationId(num3);
						if (groupBySerializationId2 != null)
						{
							group2.ManuallyLinkedGroups.Add(groupBySerializationId2);
						}
					}
					group2.ManualBridgeSnippetsOfTargetGroups.Clear();
					if (group2.Serialization_ManualBridgeSegmentIds == null)
					{
						group2.Serialization_ManualBridgeSegmentIds = new List<int>();
					}
					foreach (int num4 in group2.Serialization_ManualBridgeSegmentIds)
					{
						Segment snippetById = this.GetSnippetById(num4);
						if (snippetById != null)
						{
							group2.ManualBridgeSnippetsOfTargetGroups.Add(snippetById);
						}
					}
					foreach (Segment segment2 in group2.Segments)
					{
						segment2.ManuallyBlockedSnippets = new HashSet<Segment>();
						if (segment2.Serialization_ManuallyBlockedSegmentIds == null)
						{
							segment2.Serialization_ManuallyBlockedSegmentIds = new List<int>();
						}
						foreach (int num5 in segment2.Serialization_ManuallyBlockedSegmentIds)
						{
							Segment snippetById2 = this.GetSnippetById(num5);
							segment2.ManuallyBlockedSnippets.Add(snippetById2);
						}
						segment2.ManuallyLinkedSnippets = new HashSet<Segment>();
						if (segment2.Serialization_ManuallyLinkedSegmentIds == null)
						{
							segment2.Serialization_ManuallyLinkedSegmentIds = new List<int>();
						}
						foreach (int num6 in segment2.Serialization_ManuallyLinkedSegmentIds)
						{
							Segment snippetById3 = this.GetSnippetById(num6);
							segment2.ManuallyLinkedSnippets.Add(snippetById3);
						}
					}
				}
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000034A8 File Offset: 0x000016A8
		public void MergeProjects(PsaiProject project)
		{
			this._themes.AddRange(project._themes);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000034BC File Offset: 0x000016BC
		public void ReconstructIds(string path)
		{
			foreach (Theme theme in this._themes)
			{
				theme.Id = int.Parse(this._projectProperties.ModuleIdPrefix + theme.Id);
				foreach (Group group in theme.Groups)
				{
					group.Serialization_Id = int.Parse(this._projectProperties.ModuleIdPrefix + group.Serialization_Id);
					if (group.Serialization_ManuallyBlockedGroupIds == null)
					{
						group.Serialization_ManuallyBlockedGroupIds = new List<int>();
					}
					for (int i = 0; i < group.Serialization_ManuallyBlockedGroupIds.Count; i++)
					{
						group.Serialization_ManuallyBlockedGroupIds[i] = int.Parse(this._projectProperties.ModuleIdPrefix + group.Serialization_ManuallyBlockedGroupIds[i]);
					}
					if (group.Serialization_ManuallyLinkedGroupIds == null)
					{
						group.Serialization_ManuallyLinkedGroupIds = new List<int>();
					}
					for (int j = 0; j < group.Serialization_ManuallyLinkedGroupIds.Count; j++)
					{
						group.Serialization_ManuallyLinkedGroupIds[j] = int.Parse(this._projectProperties.ModuleIdPrefix + group.Serialization_ManuallyLinkedGroupIds[j]);
					}
					if (group.Serialization_ManualBridgeSegmentIds == null)
					{
						group.Serialization_ManualBridgeSegmentIds = new List<int>();
					}
					for (int k = 0; k < group.Serialization_ManualBridgeSegmentIds.Count; k++)
					{
						group.Serialization_ManualBridgeSegmentIds[k] = int.Parse(this._projectProperties.ModuleIdPrefix + group.Serialization_ManualBridgeSegmentIds[k]);
					}
					foreach (Segment segment in group.Segments)
					{
						segment.Id = int.Parse(this._projectProperties.ModuleIdPrefix + segment.Id);
						if (segment.Serialization_ManuallyBlockedSegmentIds == null)
						{
							segment.Serialization_ManuallyBlockedSegmentIds = new List<int>();
						}
						for (int l = 0; l < segment.Serialization_ManuallyBlockedSegmentIds.Count; l++)
						{
							segment.Serialization_ManuallyBlockedSegmentIds[l] = int.Parse(this._projectProperties.ModuleIdPrefix + segment.Serialization_ManuallyBlockedSegmentIds[l]);
						}
						if (segment.Serialization_ManuallyLinkedSegmentIds == null)
						{
							segment.Serialization_ManuallyLinkedSegmentIds = new List<int>();
						}
						for (int m = 0; m < segment.Serialization_ManuallyLinkedSegmentIds.Count; m++)
						{
							segment.Serialization_ManuallyLinkedSegmentIds[m] = int.Parse(this._projectProperties.ModuleIdPrefix + segment.Serialization_ManuallyLinkedSegmentIds[m]);
						}
						segment.AudioData.FilePathRelativeToProjectDir = segment.AudioData.FilePathRelativeToProjectDir;
						segment.AudioData.ModuleID = path;
					}
				}
				for (int n = 0; n < theme.Serialization_ManuallyBlockedThemeIds.Count; n++)
				{
					theme.Serialization_ManuallyBlockedThemeIds[n] = int.Parse(this._projectProperties.ModuleIdPrefix + theme.Serialization_ManuallyBlockedThemeIds[n]);
				}
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000388C File Offset: 0x00001A8C
		public void DebugCheckProjectIntegrity()
		{
			foreach (Theme theme in this._themes)
			{
				foreach (Group group in theme.Groups)
				{
					foreach (Segment segment in group.Segments)
					{
						foreach (int num in segment.Serialization_ManuallyBlockedSegmentIds)
						{
							this.GetSnippetById(num);
						}
						foreach (int num2 in segment.Serialization_ManuallyLinkedSegmentIds)
						{
							this.GetSnippetById(num2);
						}
						this.GetThemeById(segment.ThemeId);
					}
					foreach (int num3 in group.Serialization_ManuallyBlockedGroupIds)
					{
						this.GetGroupBySerializationId(num3);
					}
					foreach (int num4 in group.Serialization_ManuallyLinkedGroupIds)
					{
						this.GetGroupBySerializationId(num4);
					}
					foreach (int num5 in group.Serialization_ManualBridgeSegmentIds)
					{
						this.GetSnippetById(num5);
					}
				}
				foreach (int num6 in theme.Serialization_ManuallyBlockedThemeIds)
				{
					this.GetThemeById(num6);
				}
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003B84 File Offset: 0x00001D84
		public Soundtrack BuildPsaiDotNetSoundtrackFromProject()
		{
			Soundtrack soundtrack = new Soundtrack();
			foreach (Theme theme in this.Themes)
			{
				if (theme.ThemeTypeInt == 6)
				{
					foreach (Segment segment in theme.GetSegmentsOfAllGroups())
					{
						segment.IsUsableAtEnd = true;
						segment.IsUsableInMiddle = true;
						segment.IsUsableAtEnd = true;
					}
				}
				soundtrack.m_themes.Add(theme.Id, theme.CreatePsaiDotNetVersion());
			}
			foreach (Segment segment2 in this.GetSegmentsOfAllThemes())
			{
				segment2.BuildCompatibleSegmentsSet(this);
				Segment segment3 = segment2.CreatePsaiDotNetVersion(this);
				soundtrack.m_snippets.Add(segment3.Id, segment3);
				soundtrack.getThemeById(segment3.ThemeId).m_segments.Add(segment3);
			}
			soundtrack.BuildAllIndirectionSequences();
			return soundtrack;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003CC4 File Offset: 0x00001EC4
		private void PrepareForXmlSerialization()
		{
			this.SerializedByProtocolVersion = PsaiProject.SERIALIZATION_PROTOCOL_VERSION;
			int num = 1;
			foreach (Theme theme in this._themes)
			{
				foreach (Group group in theme.Groups)
				{
					group.Serialization_Id = num;
					num++;
				}
			}
			foreach (Theme theme2 in this._themes)
			{
				theme2.Serialization_ManuallyBlockedThemeIds = new List<int>();
				foreach (Theme theme3 in theme2.ManuallyBlockedTargetThemes)
				{
					if (theme3 != null)
					{
						theme2.Serialization_ManuallyBlockedThemeIds.Add(theme3.Id);
					}
				}
				foreach (Group group2 in theme2.Groups)
				{
					group2.Serialization_ManuallyBlockedGroupIds = new List<int>();
					foreach (Group group3 in group2.ManuallyBlockedGroups)
					{
						if (group3 != null)
						{
							group2.Serialization_ManuallyBlockedGroupIds.Add(group3.Serialization_Id);
						}
					}
					group2.Serialization_ManuallyLinkedGroupIds = new List<int>();
					foreach (Group group4 in group2.ManuallyLinkedGroups)
					{
						if (group4 != null)
						{
							group2.Serialization_ManuallyLinkedGroupIds.Add(group4.Serialization_Id);
						}
					}
					group2.Serialization_ManualBridgeSegmentIds = new List<int>();
					foreach (Segment segment in group2.ManualBridgeSnippetsOfTargetGroups)
					{
						if (segment != null)
						{
							group2.Serialization_ManualBridgeSegmentIds.Add(segment.Id);
						}
					}
					foreach (Segment segment2 in group2.Segments)
					{
						segment2.Serialization_ManuallyBlockedSegmentIds = new List<int>();
						foreach (Segment segment3 in segment2.ManuallyBlockedSnippets)
						{
							if (segment3 != null)
							{
								segment2.Serialization_ManuallyBlockedSegmentIds.Add(segment3.Id);
							}
						}
						segment2.Serialization_ManuallyLinkedSegmentIds = new List<int>();
						foreach (Segment segment4 in segment2.ManuallyLinkedSnippets)
						{
							if (segment4 != null)
							{
								segment2.Serialization_ManuallyLinkedSegmentIds.Add(segment4.Id);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000040F0 File Offset: 0x000022F0
		public HashSet<Segment> GetSegmentsOfAllThemes()
		{
			HashSet<Segment> hashSet = new HashSet<Segment>();
			foreach (Theme theme in this.Themes)
			{
				hashSet.UnionWith(theme.GetSegmentsOfAllGroups());
			}
			return hashSet;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004150 File Offset: 0x00002350
		public Theme GetThemeById(int themeId)
		{
			foreach (Theme theme in this.Themes)
			{
				if (theme.Id == themeId)
				{
					return theme;
				}
			}
			return null;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000041AC File Offset: 0x000023AC
		public Segment GetSnippetById(int id)
		{
			foreach (Theme theme in this.Themes)
			{
				foreach (Segment segment in theme.GetSegmentsOfAllGroups())
				{
					if (segment.Id == id)
					{
						return segment;
					}
				}
			}
			return null;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004244 File Offset: 0x00002444
		public Group GetGroupBySerializationId(int id)
		{
			foreach (Theme theme in this._themes)
			{
				foreach (Group group in theme.Groups)
				{
					if (group.Serialization_Id == id)
					{
						return group;
					}
				}
			}
			return null;
		}

		// Token: 0x06000079 RID: 121 RVA: 0x000042DC File Offset: 0x000024DC
		public void AddPsaiMusicEntity(PsaiMusicEntity entity)
		{
			this.AddPsaiMusicEntity(entity, -1);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000042E8 File Offset: 0x000024E8
		public void AddPsaiMusicEntity(PsaiMusicEntity entity, int targetIndex)
		{
			if (entity is Segment)
			{
				Segment segment = (Segment)entity;
				if (this.GetSnippetById(segment.Id) != null)
				{
					segment.Id = this.GetNextFreeSnippetId(segment.Id);
				}
				if (segment.Group != null)
				{
					segment.Group.AddSegment(segment, targetIndex);
					return;
				}
			}
			else if (entity is Group)
			{
				Group group = (Group)entity;
				if (group.Theme != null)
				{
					group.Theme.Groups.Add(group);
					return;
				}
			}
			else if (entity is Theme)
			{
				Theme theme = (Theme)entity;
				if (this.GetThemeById(theme.Id) != null)
				{
					theme.Id = this.GetNextFreeThemeId(theme.Id);
				}
				this.Themes.Add(theme);
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000043A0 File Offset: 0x000025A0
		public void DeleteMusicEntity(PsaiMusicEntity entity)
		{
			if (entity is Segment)
			{
				Segment segment = (Segment)entity;
				if (segment.Group != null)
				{
					segment.Group.RemoveSegment(segment);
				}
				using (HashSet<Segment>.Enumerator enumerator = this.GetSegmentsOfAllThemes().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Segment segment2 = enumerator.Current;
						if (segment2.ManuallyLinkedSnippets.Contains(segment))
						{
							segment2.ManuallyLinkedSnippets.Remove(segment);
						}
						if (segment2.ManuallyBlockedSnippets.Contains(segment))
						{
							segment2.ManuallyBlockedSnippets.Remove(segment);
						}
					}
					return;
				}
			}
			if (entity is Group)
			{
				Group group = (Group)entity;
				if (group.Theme != null)
				{
					group.Theme.Groups.Remove(group);
				}
				using (HashSet<Group>.Enumerator enumerator2 = this.GetGroupsOfAllThemes().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Group group2 = enumerator2.Current;
						if (group2.ManuallyBlockedGroups.Contains(group))
						{
							group2.ManuallyBlockedGroups.Remove(group);
						}
						if (group2.ManuallyLinkedGroups.Contains(group))
						{
							group2.ManuallyLinkedGroups.Remove(group);
						}
					}
					return;
				}
			}
			if (entity is Theme)
			{
				Theme theme = (Theme)entity;
				this.Themes.Remove(theme);
				foreach (Theme theme2 in this.Themes)
				{
					if (theme2.ManuallyBlockedTargetThemes.Contains(theme))
					{
						theme2.ManuallyBlockedTargetThemes.Remove(theme);
					}
				}
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x00004568 File Offset: 0x00002768
		public int GetHighestSegmentId()
		{
			int num = 0;
			foreach (Segment segment in this.GetSegmentsOfAllThemes())
			{
				if (segment.Id > num)
				{
					num = segment.Id;
				}
			}
			return num;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000045C8 File Offset: 0x000027C8
		public int GetNextFreeSnippetId(int idToStartSearchFrom)
		{
			int num = idToStartSearchFrom;
			if (num <= 1)
			{
				num = 1;
			}
			while (this.GetSnippetById(num) != null)
			{
				num++;
			}
			return num;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000045F0 File Offset: 0x000027F0
		public HashSet<Group> GetGroupsOfAllThemes()
		{
			HashSet<Group> hashSet = new HashSet<Group>();
			foreach (Theme theme in this.Themes)
			{
				foreach (Group group in theme.Groups)
				{
					hashSet.Add(group);
				}
			}
			return hashSet;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00004684 File Offset: 0x00002884
		public bool CheckIfSnippetIsManualBridgeSnippetForSourceGroup(Segment snippet, Group sourceGroup)
		{
			return sourceGroup.ManualBridgeSnippetsOfTargetGroups.Contains(snippet);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004692 File Offset: 0x00002892
		public bool CheckIfThereIsAtLeastOneBridgeSnippetFromSourceGroupToTargetGroup(Group sourceGroup, Group targetGroup)
		{
			return targetGroup.ContainsAtLeastOneAutomaticBridgeSegment() || targetGroup.ContainsAtLeastOneManualBridgeSegmentForSourceGroup(sourceGroup);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000046A8 File Offset: 0x000028A8
		public bool CheckIfSnippetIsManualBridgeSnippetToAnyGroup(Segment snippet, bool getGroups, out List<Group> groups)
		{
			groups = new List<Group>();
			foreach (Theme theme in this.Themes)
			{
				foreach (Group group in theme.Groups)
				{
					if (group.ManualBridgeSnippetsOfTargetGroups.Contains(snippet))
					{
						if (!getGroups)
						{
							return true;
						}
						groups.Add(group);
					}
				}
			}
			return groups.Count > 0;
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004760 File Offset: 0x00002960
		public void DoUpdateAllParentThemeIdsAndGroupsOfChildPsaiEntities()
		{
			foreach (Theme theme in this.Themes)
			{
				foreach (Group group in theme.Groups)
				{
					group.Theme = theme;
					foreach (Segment segment in group.Segments)
					{
						segment.Group = group;
						segment.ThemeId = theme.Id;
					}
				}
			}
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004840 File Offset: 0x00002A40
		public int GetNextFreeThemeId(int idToStartSearchFrom)
		{
			int num = idToStartSearchFrom;
			if (num <= 1)
			{
				num = 1;
			}
			while (this.GetThemeById(num) != null)
			{
				num++;
			}
			return num;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00004868 File Offset: 0x00002A68
		public bool CheckIfThemeIdIsInUse(int themeId)
		{
			using (List<Theme>.Enumerator enumerator = this.Themes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Id == themeId)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000048C4 File Offset: 0x00002AC4
		public List<Segment> GetSnippetsById(int id)
		{
			List<Segment> list = new List<Segment>();
			foreach (Segment segment in this.GetSegmentsOfAllThemes())
			{
				if (segment.Id == id)
				{
					list.Add(segment);
				}
			}
			return list;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004928 File Offset: 0x00002B28
		public object Clone()
		{
			PsaiProject psaiProject = new PsaiProject();
			psaiProject.Properties = (ProjectProperties)this.Properties.Clone();
			psaiProject.Themes.Clear();
			foreach (Theme theme in this.Themes)
			{
				Theme theme2 = (Theme)theme.Clone();
				psaiProject.AddPsaiMusicEntity(theme2);
			}
			HashSet<Segment> segmentsOfAllThemes = this.GetSegmentsOfAllThemes();
			HashSet<Segment> segmentsOfAllThemes2 = psaiProject.GetSegmentsOfAllThemes();
			Dictionary<Theme, Theme> dictionary = new Dictionary<Theme, Theme>();
			List<Theme>.Enumerator enumerator2 = this.Themes.GetEnumerator();
			List<Theme>.Enumerator enumerator3 = psaiProject.Themes.GetEnumerator();
			while (enumerator2.MoveNext())
			{
				enumerator3.MoveNext();
				dictionary.Add(enumerator2.Current, enumerator3.Current);
			}
			Dictionary<Segment, Segment> dictionary2 = new Dictionary<Segment, Segment>();
			HashSet<Segment>.Enumerator enumerator4 = segmentsOfAllThemes.GetEnumerator();
			HashSet<Segment>.Enumerator enumerator5 = segmentsOfAllThemes2.GetEnumerator();
			while (enumerator4.MoveNext())
			{
				enumerator5.MoveNext();
				dictionary2.Add(enumerator4.Current, enumerator5.Current);
			}
			Dictionary<Group, Group> dictionary3 = new Dictionary<Group, Group>();
			foreach (Theme theme3 in dictionary.Keys)
			{
				Theme theme4 = dictionary[theme3];
				foreach (Theme theme5 in theme3.ManuallyBlockedTargetThemes)
				{
					if (dictionary.Keys.Contains(theme5))
					{
						theme4.ManuallyBlockedTargetThemes.Add(dictionary[theme5]);
					}
				}
				for (int i = 0; i < theme3.Groups.Count; i++)
				{
					Group group = theme3.Groups[i];
					Group group2 = theme4.Groups[i];
					dictionary3.Add(group, group2);
				}
			}
			foreach (Group group3 in dictionary3.Keys)
			{
				foreach (Group group4 in group3.ManuallyLinkedGroups)
				{
					if (dictionary3.Keys.Contains(group4))
					{
						dictionary3[group3].ManuallyLinkedGroups.Add(dictionary3[group4]);
					}
				}
				foreach (Group group5 in group3.ManuallyBlockedGroups)
				{
					if (dictionary3.Keys.Contains(group5))
					{
						dictionary3[group3].ManuallyBlockedGroups.Add(dictionary3[group5]);
					}
				}
				foreach (Segment segment in group3.ManualBridgeSnippetsOfTargetGroups)
				{
					if (dictionary2.Keys.Contains(segment))
					{
						Segment segment2 = dictionary2[segment];
						dictionary3[group3].ManualBridgeSnippetsOfTargetGroups.Add(segment2);
					}
				}
			}
			foreach (Segment segment3 in segmentsOfAllThemes)
			{
				foreach (Segment segment4 in segment3.ManuallyBlockedSnippets)
				{
					if (dictionary2.Keys.Contains(segment4))
					{
						dictionary2[segment3].ManuallyBlockedSnippets.Add(dictionary2[segment4]);
					}
				}
				foreach (Segment segment5 in segment3.ManuallyLinkedSnippets)
				{
					if (dictionary2.Keys.Contains(segment5))
					{
						dictionary2[segment3].ManuallyLinkedSnippets.Add(dictionary2[segment5]);
					}
				}
			}
			return psaiProject;
		}

		// Token: 0x04000034 RID: 52
		public static readonly string SERIALIZATION_PROTOCOL_VERSION = "1.0";

		// Token: 0x04000035 RID: 53
		private ProjectProperties _projectProperties = new ProjectProperties();

		// Token: 0x04000036 RID: 54
		private List<Theme> _themes = new List<Theme>();

		// Token: 0x04000037 RID: 55
		private static XmlSerializer _serializer = new XmlSerializer(typeof(PsaiProject));
	}
}
