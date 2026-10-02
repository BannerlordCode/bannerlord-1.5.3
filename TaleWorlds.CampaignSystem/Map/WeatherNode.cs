using System;
using TaleWorlds.CampaignSystem.ComponentInterfaces;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x0200022E RID: 558
	public class WeatherNode
	{
		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x0600218A RID: 8586 RVA: 0x00094E40 File Offset: 0x00093040
		// (set) Token: 0x0600218B RID: 8587 RVA: 0x00094E48 File Offset: 0x00093048
		public bool IsVisuallyDirty { get; private set; }

		// Token: 0x0600218C RID: 8588 RVA: 0x00094E51 File Offset: 0x00093051
		public WeatherNode(CampaignVec2 position)
		{
			this.Position = position;
			this.CurrentWeatherEvent = MapWeatherModel.WeatherEvent.Clear;
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x00094E67 File Offset: 0x00093067
		public void SetVisualDirty()
		{
			this.IsVisuallyDirty = true;
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x00094E70 File Offset: 0x00093070
		public void OnVisualUpdated()
		{
			this.IsVisuallyDirty = false;
		}

		// Token: 0x040009CF RID: 2511
		public CampaignVec2 Position;

		// Token: 0x040009D1 RID: 2513
		public MapWeatherModel.WeatherEvent CurrentWeatherEvent;
	}
}
