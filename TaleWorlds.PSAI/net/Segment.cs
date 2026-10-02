using System;
using System.Collections.Generic;
using System.Text;

namespace psai.net
{
	// Token: 0x02000020 RID: 32
	public class Segment
	{
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000215 RID: 533 RVA: 0x000097FF File Offset: 0x000079FF
		// (set) Token: 0x06000216 RID: 534 RVA: 0x00009807 File Offset: 0x00007A07
		public int Id { get; set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000217 RID: 535 RVA: 0x00009810 File Offset: 0x00007A10
		// (set) Token: 0x06000218 RID: 536 RVA: 0x00009818 File Offset: 0x00007A18
		public float Intensity { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000219 RID: 537 RVA: 0x00009821 File Offset: 0x00007A21
		// (set) Token: 0x0600021A RID: 538 RVA: 0x00009829 File Offset: 0x00007A29
		public int ThemeId { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600021B RID: 539 RVA: 0x00009832 File Offset: 0x00007A32
		// (set) Token: 0x0600021C RID: 540 RVA: 0x0000983A File Offset: 0x00007A3A
		public string Name { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600021D RID: 541 RVA: 0x00009843 File Offset: 0x00007A43
		// (set) Token: 0x0600021E RID: 542 RVA: 0x0000984B File Offset: 0x00007A4B
		public int Playcount { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00009854 File Offset: 0x00007A54
		// (set) Token: 0x06000220 RID: 544 RVA: 0x0000985C File Offset: 0x00007A5C
		public int MaxPreBeatMsOfCompatibleSnippetsWithinSameTheme { get; set; }

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000221 RID: 545 RVA: 0x00009865 File Offset: 0x00007A65
		// (set) Token: 0x06000222 RID: 546 RVA: 0x0000986D File Offset: 0x00007A6D
		public List<Follower> Followers { get; private set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000223 RID: 547 RVA: 0x00009876 File Offset: 0x00007A76
		// (set) Token: 0x06000224 RID: 548 RVA: 0x0000987E File Offset: 0x00007A7E
		public int SnippetTypeBitfield
		{
			get
			{
				return this._snippetTypeBitfield;
			}
			set
			{
				this._snippetTypeBitfield = value;
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x00009887 File Offset: 0x00007A87
		public Segment()
		{
			this.Followers = new List<Follower>();
			this._mapDirectTransitionToThemeIsPossible = new Dictionary<int, bool>();
			this.MapOfNextTransitionSegmentToTheme = new Dictionary<int, Segment>();
		}

		// Token: 0x06000226 RID: 550 RVA: 0x000098B0 File Offset: 0x00007AB0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.Name);
			stringBuilder.Append(" (");
			stringBuilder.Append(this.Id);
			stringBuilder.Append(")");
			stringBuilder.Append(" ");
			stringBuilder.Append(Segment.GetStringFromSegmentSuitabilities(this.SnippetTypeBitfield));
			stringBuilder.Append(" [");
			stringBuilder.Append(this.Intensity.ToString("F2"));
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		// Token: 0x06000227 RID: 551 RVA: 0x00009949 File Offset: 0x00007B49
		public bool IsUsableAs(SegmentSuitability snippetType)
		{
			return (this.SnippetTypeBitfield & (int)snippetType) > 0;
		}

		// Token: 0x06000228 RID: 552 RVA: 0x00009956 File Offset: 0x00007B56
		public bool IsUsableOnlyAs(SegmentSuitability snippetType)
		{
			return (this.SnippetTypeBitfield & (int)snippetType) == (int)snippetType;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x00009964 File Offset: 0x00007B64
		private void SetSnippetTypeFlag(SegmentSuitability snippetType)
		{
			this.SnippetTypeBitfield |= (int)snippetType;
		}

		// Token: 0x0600022A RID: 554 RVA: 0x00009984 File Offset: 0x00007B84
		private void ClearSnippetTypeFlag(SegmentSuitability snippetType)
		{
			this.SnippetTypeBitfield &= (int)(~(int)snippetType);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x000099A4 File Offset: 0x00007BA4
		public Segment ReturnSegmentWithLowestIntensityDifference(List<Segment> argSnippets)
		{
			float num = 1f;
			Segment segment = null;
			for (int i = 0; i < argSnippets.Count; i++)
			{
				Segment segment2 = argSnippets[i];
				if (segment2 != this)
				{
					float num2 = Math.Abs(segment2.Intensity - this.Intensity);
					if (num2 == 0f)
					{
						return segment2;
					}
					if (num2 < num)
					{
						num = num2;
						segment = segment2;
					}
				}
			}
			return segment;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00009A00 File Offset: 0x00007C00
		internal bool CheckIfAnyDirectOrIndirectTransitionIsPossible(Soundtrack soundtrack, int targetThemeId)
		{
			return this.CheckIfAtLeastOneDirectTransitionOrLayeringIsPossible(soundtrack, targetThemeId) || this.MapOfNextTransitionSegmentToTheme.ContainsKey(targetThemeId);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00009A1C File Offset: 0x00007C1C
		public bool CheckIfAtLeastOneDirectTransitionOrLayeringIsPossible(Soundtrack soundtrack, int targetThemeId)
		{
			bool flag;
			if (this._mapDirectTransitionToThemeIsPossible.TryGetValue(targetThemeId, out flag))
			{
				return flag;
			}
			foreach (Follower follower in this.Followers)
			{
				if (soundtrack.GetSegmentById(follower.snippetId).ThemeId == targetThemeId)
				{
					this._mapDirectTransitionToThemeIsPossible[targetThemeId] = true;
					return true;
				}
			}
			this._mapDirectTransitionToThemeIsPossible[targetThemeId] = false;
			return false;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00009AB0 File Offset: 0x00007CB0
		public static string GetStringFromSegmentSuitabilities(int snippetTypeBitfield)
		{
			StringBuilder stringBuilder = new StringBuilder(20);
			stringBuilder.Append("[ ");
			if (snippetTypeBitfield == 0)
			{
				stringBuilder.Append("NULL ");
			}
			if ((snippetTypeBitfield & 1) > 0)
			{
				stringBuilder.Append("START ");
			}
			if ((snippetTypeBitfield & 2) > 0)
			{
				stringBuilder.Append("MID ");
			}
			if ((snippetTypeBitfield & 8) > 0)
			{
				stringBuilder.Append("BRIDGE ");
			}
			if ((snippetTypeBitfield & 4) > 0)
			{
				stringBuilder.Append("END ");
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		// Token: 0x04000129 RID: 297
		public AudioData audioData;

		// Token: 0x04000131 RID: 305
		public Dictionary<int, Segment> MapOfNextTransitionSegmentToTheme;

		// Token: 0x04000132 RID: 306
		private Dictionary<int, bool> _mapDirectTransitionToThemeIsPossible;

		// Token: 0x04000133 RID: 307
		private int _snippetTypeBitfield;

		// Token: 0x04000134 RID: 308
		public Segment nextSnippetToShortestEndSequence;
	}
}
