using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.BoardGame
{
	// Token: 0x02000190 RID: 400
	public class BoardGameInstructionVisualWidget : Widget
	{
		// Token: 0x060014D4 RID: 5332 RVA: 0x00038D14 File Offset: 0x00036F14
		public BoardGameInstructionVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00038D20 File Offset: 0x00036F20
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.Sprite == null)
			{
				int siblingIndex = base.ParentWidget.ParentWidget.GetSiblingIndex();
				if (!string.IsNullOrEmpty(this.GameType))
				{
					base.Sprite = base.Context.SpriteData.GetSprite(this.GameType + siblingIndex);
				}
			}
			if (base.Sprite != null)
			{
				base.SuggestedWidth = (float)base.Sprite.Width * 0.5f;
				base.SuggestedHeight = (float)base.Sprite.Height * 0.5f;
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x060014D6 RID: 5334 RVA: 0x00038DB9 File Offset: 0x00036FB9
		// (set) Token: 0x060014D7 RID: 5335 RVA: 0x00038DC1 File Offset: 0x00036FC1
		[Editor(false)]
		public string GameType
		{
			get
			{
				return this._gameType;
			}
			set
			{
				if (this._gameType != value)
				{
					this._gameType = value;
				}
			}
		}

		// Token: 0x0400097D RID: 2429
		private const float ScaleCoeff = 0.5f;

		// Token: 0x0400097E RID: 2430
		private string _gameType;
	}
}
