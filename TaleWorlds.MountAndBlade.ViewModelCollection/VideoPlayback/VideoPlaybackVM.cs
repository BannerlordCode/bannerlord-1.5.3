using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.VideoPlayback
{
	// Token: 0x0200000C RID: 12
	public class VideoPlaybackVM : ViewModel
	{
		// Token: 0x060000BE RID: 190 RVA: 0x00004028 File Offset: 0x00002228
		public void Tick(float totalElapsedTimeInVideoInSeconds)
		{
			if (this.subTitleLines != null)
			{
				SRTHelper.SubtitleItem itemInTimeframe = this.GetItemInTimeframe(totalElapsedTimeInVideoInSeconds);
				if (itemInTimeframe != null)
				{
					this.SubtitleText = string.Join("\n", itemInTimeframe.Lines);
					return;
				}
				this.SubtitleText = string.Empty;
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000406C File Offset: 0x0000226C
		public SRTHelper.SubtitleItem GetItemInTimeframe(float timeInSecondsInVideo)
		{
			int num = (int)(timeInSecondsInVideo * 1000f);
			for (int i = 0; i < this.subTitleLines.Count; i++)
			{
				if (this.subTitleLines[i].StartTime < num && this.subTitleLines[i].EndTime > num)
				{
					return this.subTitleLines[i];
				}
			}
			return null;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x000040CE File Offset: 0x000022CE
		public void SetSubtitles(List<SRTHelper.SubtitleItem> lines)
		{
			this.subTitleLines = lines;
			this.SubtitleText = string.Empty;
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x000040E2 File Offset: 0x000022E2
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x000040EA File Offset: 0x000022EA
		[DataSourceProperty]
		public string SubtitleText
		{
			get
			{
				return this._subtitleText;
			}
			set
			{
				if (value != this._subtitleText)
				{
					this._subtitleText = value;
					base.OnPropertyChangedWithValue<string>(value, "SubtitleText");
				}
			}
		}

		// Token: 0x04000050 RID: 80
		private List<SRTHelper.SubtitleItem> subTitleLines;

		// Token: 0x04000051 RID: 81
		private string _subtitleText;
	}
}
