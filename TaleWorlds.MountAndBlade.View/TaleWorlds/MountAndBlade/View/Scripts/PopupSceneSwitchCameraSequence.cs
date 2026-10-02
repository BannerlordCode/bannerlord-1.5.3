using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Scripts
{
	// Token: 0x02000061 RID: 97
	public class PopupSceneSwitchCameraSequence : PopupSceneSequence
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x0001BA14 File Offset: 0x00019C14
		protected override void OnInit()
		{
			this._switchEntity = base.GameEntity.Scene.GetFirstEntityWithName(this.EntityName);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0001BA40 File Offset: 0x00019C40
		public override void OnInitialState()
		{
			if (this._switchEntity != null)
			{
				GameEntity gameEntity = base.GameEntity.Scene.FindEntityWithTag("customcamera");
				if (gameEntity != null)
				{
					gameEntity.RemoveTag("customcamera");
				}
				this._switchEntity.AddTag("customcamera");
			}
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0001BA93 File Offset: 0x00019C93
		public override void OnPositiveState()
		{
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0001BA95 File Offset: 0x00019C95
		public override void OnNegativeState()
		{
		}

		// Token: 0x0400021A RID: 538
		public string EntityName = "";

		// Token: 0x0400021B RID: 539
		private GameEntity _switchEntity;
	}
}
