using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000243 RID: 579
	public class GameLoadingState : GameState
	{
		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x060021AA RID: 8618 RVA: 0x00076F53 File Offset: 0x00075153
		public override bool CanBeDisabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x060021AB RID: 8619 RVA: 0x00076F56 File Offset: 0x00075156
		public override bool IsMusicMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x00076F61 File Offset: 0x00075161
		public void SetLoadingParameters(MBGameManager gameLoader)
		{
			Game.OnGameCreated += this.OnGameCreated;
			this._gameLoader = gameLoader;
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x00076F7B File Offset: 0x0007517B
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (!this._loadingFinished)
			{
				this._loadingFinished = this._gameLoader.DoLoadingForGameManager();
				return;
			}
			GameStateManager.Current = Game.Current.GameStateManager;
			this._gameLoader.OnLoadFinished();
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x00076FB8 File Offset: 0x000751B8
		private void OnGameCreated()
		{
			Game.OnGameCreated -= this.OnGameCreated;
			Game.Current.OnItemDeserializedEvent += delegate(ItemObject itemObject)
			{
				if (itemObject.Type == ItemObject.ItemTypeEnum.HandArmor)
				{
					Utilities.RegisterMeshForGPUMorph(itemObject.MultiMeshName);
				}
			};
		}

		// Token: 0x04000CED RID: 3309
		private bool _loadingFinished;

		// Token: 0x04000CEE RID: 3310
		private MBGameManager _gameLoader;
	}
}
