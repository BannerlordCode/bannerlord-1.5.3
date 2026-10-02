using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine;

namespace SandBox.View.Map.Managers
{
	// Token: 0x02000075 RID: 117
	public abstract class EntityVisualManagerBase : CampaignEntityVisualComponent
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x00026DF4 File Offset: 0x00024FF4
		public Scene MapScene
		{
			get
			{
				if (this._mapScene == null && Campaign.Current != null && Campaign.Current.MapSceneWrapper != null)
				{
					this._mapScene = ((MapScene)Campaign.Current.MapSceneWrapper).Scene;
				}
				return this._mapScene;
			}
		}

		// Token: 0x04000246 RID: 582
		private Scene _mapScene;
	}
}
