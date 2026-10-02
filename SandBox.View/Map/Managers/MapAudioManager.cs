using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine;

namespace SandBox.View.Map.Managers
{
	// Token: 0x0200007B RID: 123
	internal class MapAudioManager : CampaignEntityVisualComponent
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x00028F17 File Offset: 0x00027117
		public override int Priority
		{
			get
			{
				return 70;
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00028F1B File Offset: 0x0002711B
		public MapAudioManager()
		{
			this._mapScene = Campaign.Current.MapSceneWrapper as MapScene;
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00028F38 File Offset: 0x00027138
		public override void OnVisualTick(MapScreen screen, float realDt, float dt)
		{
			if (CampaignTime.Now.GetSeasonOfYear != this._lastCachedSeason)
			{
				SoundManager.SetGlobalParameter("Season", (float)CampaignTime.Now.GetSeasonOfYear);
				this._lastCachedSeason = CampaignTime.Now.GetSeasonOfYear;
			}
			if (Math.Abs(this._lastCameraZ - this._mapScene.Scene.LastFinalRenderCameraPosition.Z) > 0.1f)
			{
				SoundManager.SetGlobalParameter("CampaignCameraHeight", this._mapScene.Scene.LastFinalRenderCameraPosition.Z);
				this._lastCameraZ = this._mapScene.Scene.LastFinalRenderCameraPosition.Z;
			}
			if ((int)CampaignTime.Now.CurrentHourInDay == this._lastHourUpdate)
			{
				SoundManager.SetGlobalParameter("Daytime", CampaignTime.Now.CurrentHourInDay);
				this._lastHourUpdate = (int)CampaignTime.Now.CurrentHourInDay;
			}
		}

		// Token: 0x04000285 RID: 645
		private const string SeasonParameterId = "Season";

		// Token: 0x04000286 RID: 646
		private const string CameraHeightParameterId = "CampaignCameraHeight";

		// Token: 0x04000287 RID: 647
		private const string TimeOfDayParameterId = "Daytime";

		// Token: 0x04000288 RID: 648
		private const string WeatherEventIntensityParameterId = "Rainfall";

		// Token: 0x04000289 RID: 649
		private CampaignTime.Seasons _lastCachedSeason;

		// Token: 0x0400028A RID: 650
		private float _lastCameraZ;

		// Token: 0x0400028B RID: 651
		private int _lastHourUpdate;

		// Token: 0x0400028C RID: 652
		private MapScene _mapScene;
	}
}
