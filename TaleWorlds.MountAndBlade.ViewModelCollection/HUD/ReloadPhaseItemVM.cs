using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000059 RID: 89
	public class ReloadPhaseItemVM : ViewModel
	{
		// Token: 0x0600073C RID: 1852 RVA: 0x0001A01D File Offset: 0x0001821D
		public ReloadPhaseItemVM(float progress, float relativeDurationToMaxDuration)
		{
			this.Update(progress, relativeDurationToMaxDuration);
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0001A02D File Offset: 0x0001822D
		public void Update(float progress, float relativeDurationToMaxDuration)
		{
			this.Progress = progress;
			this.RelativeDurationToMaxDuration = relativeDurationToMaxDuration;
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0001A03D File Offset: 0x0001823D
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x0001A045 File Offset: 0x00018245
		[DataSourceProperty]
		public float Progress
		{
			get
			{
				return this._progress;
			}
			set
			{
				if (value != this._progress)
				{
					this._progress = value;
					base.OnPropertyChangedWithValue(value, "Progress");
				}
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x0001A063 File Offset: 0x00018263
		// (set) Token: 0x06000741 RID: 1857 RVA: 0x0001A06B File Offset: 0x0001826B
		[DataSourceProperty]
		public float RelativeDurationToMaxDuration
		{
			get
			{
				return this._relativeDurationToMaxDuration;
			}
			set
			{
				if (value != this._relativeDurationToMaxDuration)
				{
					this._relativeDurationToMaxDuration = value;
					base.OnPropertyChangedWithValue(value, "RelativeDurationToMaxDuration");
				}
			}
		}

		// Token: 0x0400033A RID: 826
		private float _progress;

		// Token: 0x0400033B RID: 827
		private float _relativeDurationToMaxDuration;
	}
}
