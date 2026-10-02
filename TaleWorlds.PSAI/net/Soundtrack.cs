using System;
using System.Collections.Generic;

namespace psai.net
{
	// Token: 0x02000021 RID: 33
	public class Soundtrack
	{
		// Token: 0x0600022F RID: 559 RVA: 0x00009B3A File Offset: 0x00007D3A
		public Soundtrack()
		{
			this.m_themes = new Dictionary<int, Theme>();
			this.m_snippets = new Dictionary<int, Segment>();
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00009B58 File Offset: 0x00007D58
		public void Clear()
		{
			this.m_themes.Clear();
			this.m_snippets.Clear();
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00009B70 File Offset: 0x00007D70
		public Theme getThemeById(int id)
		{
			Theme theme;
			this.m_themes.TryGetValue(id, out theme);
			return theme;
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00009B90 File Offset: 0x00007D90
		public Segment GetSegmentById(int id)
		{
			Segment segment;
			this.m_snippets.TryGetValue(id, out segment);
			return segment;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00009BB0 File Offset: 0x00007DB0
		public SoundtrackInfo getSoundtrackInfo()
		{
			SoundtrackInfo soundtrackInfo = new SoundtrackInfo();
			soundtrackInfo.themeCount = this.m_themes.Count;
			soundtrackInfo.themeIds = new int[this.m_themes.Count];
			int num = 0;
			foreach (int num2 in this.m_themes.Keys)
			{
				soundtrackInfo.themeIds[num] = num2;
				num++;
			}
			return soundtrackInfo;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00009C40 File Offset: 0x00007E40
		public ThemeInfo getThemeInfo(int themeId)
		{
			Theme themeById = this.getThemeById(themeId);
			if (themeById != null)
			{
				ThemeInfo themeInfo = new ThemeInfo();
				themeInfo.id = themeById.id;
				themeInfo.type = themeById.themeType;
				themeInfo.name = themeById.Name;
				themeInfo.segmentIds = new int[themeById.m_segments.Count];
				for (int i = 0; i < themeById.m_segments.Count; i++)
				{
					themeInfo.segmentIds[i] = themeById.m_segments[i].Id;
				}
				return themeInfo;
			}
			return null;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00009CCC File Offset: 0x00007ECC
		public SegmentInfo getSegmentInfo(int snippetId)
		{
			SegmentInfo segmentInfo = new SegmentInfo();
			Segment segmentById = this.GetSegmentById(snippetId);
			if (segmentById != null)
			{
				segmentInfo.id = segmentById.Id;
				segmentInfo.intensity = segmentById.Intensity;
				segmentInfo.segmentSuitabilitiesBitfield = segmentById.SnippetTypeBitfield;
				segmentInfo.themeId = segmentById.ThemeId;
				segmentInfo.playcount = segmentById.Playcount;
				segmentInfo.name = segmentById.Name;
				segmentInfo.fullLengthInMilliseconds = segmentById.audioData.GetFullLengthInMilliseconds();
				segmentInfo.preBeatLengthInMilliseconds = segmentById.audioData.GetPreBeatZoneInMilliseconds();
				segmentInfo.postBeatLengthInMilliseconds = segmentById.audioData.GetPostBeatZoneInMilliseconds();
			}
			return segmentInfo;
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00009D68 File Offset: 0x00007F68
		public void UpdateMaxPreBeatMsOfCompatibleMiddleOrBridgeSnippets()
		{
			foreach (Segment segment in this.m_snippets.Values)
			{
				segment.MaxPreBeatMsOfCompatibleSnippetsWithinSameTheme = 0;
				int count = segment.Followers.Count;
				for (int i = 0; i < count; i++)
				{
					int snippetId = segment.Followers[i].snippetId;
					Segment segmentById = this.GetSegmentById(snippetId);
					if (segmentById != null && (segmentById.SnippetTypeBitfield & 10) > 0)
					{
						int preBeatZoneInMilliseconds = segmentById.audioData.GetPreBeatZoneInMilliseconds();
						if (segment.MaxPreBeatMsOfCompatibleSnippetsWithinSameTheme < preBeatZoneInMilliseconds)
						{
							segment.MaxPreBeatMsOfCompatibleSnippetsWithinSameTheme = preBeatZoneInMilliseconds;
						}
					}
				}
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00009E30 File Offset: 0x00008030
		public void BuildAllIndirectionSequences()
		{
			foreach (Theme theme in this.m_themes.Values)
			{
				theme.BuildSequencesToEndSegmentForAllSnippets();
				foreach (Theme theme2 in this.m_themes.Values)
				{
					if (theme != theme2 && theme2.themeType != ThemeType.highlightLayer && Theme.ThemeInterruptionBehaviorRequiresEvaluationOfSegmentCompatibilities(Theme.GetThemeInterruptionBehavior(theme.themeType, theme2.themeType)))
					{
						theme.BuildSequencesToTargetThemeForAllSegments(this, theme2);
					}
				}
			}
		}

		// Token: 0x04000135 RID: 309
		public Dictionary<int, Theme> m_themes;

		// Token: 0x04000136 RID: 310
		public Dictionary<int, Segment> m_snippets;
	}
}
