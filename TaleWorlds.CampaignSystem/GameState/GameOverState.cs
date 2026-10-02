using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003AB RID: 939
	public class GameOverState : GameState
	{
		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x060036CE RID: 14030 RVA: 0x000DF2FB File Offset: 0x000DD4FB
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x060036CF RID: 14031 RVA: 0x000DF2FE File Offset: 0x000DD4FE
		// (set) Token: 0x060036D0 RID: 14032 RVA: 0x000DF306 File Offset: 0x000DD506
		public IGameOverStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x060036D1 RID: 14033 RVA: 0x000DF30F File Offset: 0x000DD50F
		// (set) Token: 0x060036D2 RID: 14034 RVA: 0x000DF317 File Offset: 0x000DD517
		public GameOverState.GameOverReason Reason { get; private set; }

		// Token: 0x060036D3 RID: 14035 RVA: 0x000DF320 File Offset: 0x000DD520
		public GameOverState()
		{
		}

		// Token: 0x060036D4 RID: 14036 RVA: 0x000DF328 File Offset: 0x000DD528
		public GameOverState(GameOverState.GameOverReason reason)
		{
			this.Reason = reason;
		}

		// Token: 0x060036D5 RID: 14037 RVA: 0x000DF337 File Offset: 0x000DD537
		public static GameOverState CreateForVictory()
		{
			Game game = Game.Current;
			if (game == null)
			{
				return null;
			}
			return game.GameStateManager.CreateState<GameOverState>(new object[] { GameOverState.GameOverReason.Victory });
		}

		// Token: 0x060036D6 RID: 14038 RVA: 0x000DF35D File Offset: 0x000DD55D
		public static GameOverState CreateForRetirement()
		{
			Game game = Game.Current;
			if (game == null)
			{
				return null;
			}
			return game.GameStateManager.CreateState<GameOverState>(new object[] { GameOverState.GameOverReason.Retirement });
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x000DF383 File Offset: 0x000DD583
		public static GameOverState CreateForClanDestroyed()
		{
			Game game = Game.Current;
			if (game == null)
			{
				return null;
			}
			return game.GameStateManager.CreateState<GameOverState>(new object[] { GameOverState.GameOverReason.ClanDestroyed });
		}

		// Token: 0x04000F68 RID: 3944
		private IGameOverStateHandler _handler;

		// Token: 0x020007A1 RID: 1953
		public enum GameOverReason
		{
			// Token: 0x04001FD4 RID: 8148
			Retirement,
			// Token: 0x04001FD5 RID: 8149
			ClanDestroyed,
			// Token: 0x04001FD6 RID: 8150
			Victory
		}
	}
}
