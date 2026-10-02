using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x020000A4 RID: 164
	public class MultiplayerLobbyGameTypeCardButtonWidget : ButtonWidget
	{
		// Token: 0x060008DD RID: 2269 RVA: 0x00019921 File Offset: 0x00017B21
		public MultiplayerLobbyGameTypeCardButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0001992C File Offset: 0x00017B2C
		protected override void RefreshState()
		{
			base.RefreshState();
			if (!base.OverrideDefaultStateSwitchingEnabled)
			{
				if (base.IsDisabled)
				{
					this.SetState(base.IsSelected ? "SelectedDisabled" : "Disabled");
				}
				else if (base.IsSelected)
				{
					this.SetState("Selected");
				}
				else if (base.IsPressed)
				{
					this.SetState("Pressed");
				}
				else if (base.IsHovered)
				{
					this.SetState("Hovered");
				}
				else
				{
					this.SetState("Default");
				}
			}
			if (base.UpdateChildrenStates)
			{
				for (int i = 0; i < base.ChildCount; i++)
				{
					base.GetChild(i).SetState(base.CurrentState);
				}
			}
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x000199E0 File Offset: 0x00017BE0
		private void UpdateGameTypeImage()
		{
			if (this.GameTypeImageWidget == null || string.IsNullOrEmpty(this.GameTypeId))
			{
				return;
			}
			Sprite sprite = base.Context.SpriteData.GetSprite("MPLobby\\Matchmaking\\GameTypeCards\\" + this.GameTypeId);
			foreach (Style style in this.GameTypeImageWidget.Brush.Styles)
			{
				StyleLayer[] layers = style.GetLayers();
				for (int i = 0; i < layers.Length; i++)
				{
					layers[i].Sprite = sprite;
				}
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060008E0 RID: 2272 RVA: 0x00019A8C File Offset: 0x00017C8C
		// (set) Token: 0x060008E1 RID: 2273 RVA: 0x00019A94 File Offset: 0x00017C94
		[Editor(false)]
		public string GameTypeId
		{
			get
			{
				return this._gameTypeId;
			}
			set
			{
				if (this._gameTypeId != value)
				{
					this._gameTypeId = value;
					base.OnPropertyChanged<string>(value, "GameTypeId");
					this.UpdateGameTypeImage();
				}
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060008E2 RID: 2274 RVA: 0x00019ABD File Offset: 0x00017CBD
		// (set) Token: 0x060008E3 RID: 2275 RVA: 0x00019AC5 File Offset: 0x00017CC5
		[Editor(false)]
		public BrushWidget GameTypeImageWidget
		{
			get
			{
				return this._gameTypeImageWidget;
			}
			set
			{
				if (this._gameTypeImageWidget != value)
				{
					this._gameTypeImageWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "GameTypeImageWidget");
					this.UpdateGameTypeImage();
				}
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x060008E4 RID: 2276 RVA: 0x00019AE9 File Offset: 0x00017CE9
		// (set) Token: 0x060008E5 RID: 2277 RVA: 0x00019AF1 File Offset: 0x00017CF1
		[Editor(false)]
		public Widget CheckboxWidget
		{
			get
			{
				return this._checkboxWidget;
			}
			set
			{
				if (this._checkboxWidget != value)
				{
					this._checkboxWidget = value;
					base.OnPropertyChanged<Widget>(value, "CheckboxWidget");
				}
			}
		}

		// Token: 0x04000404 RID: 1028
		private string _gameTypeId;

		// Token: 0x04000405 RID: 1029
		private BrushWidget _gameTypeImageWidget;

		// Token: 0x04000406 RID: 1030
		private Widget _checkboxWidget;
	}
}
