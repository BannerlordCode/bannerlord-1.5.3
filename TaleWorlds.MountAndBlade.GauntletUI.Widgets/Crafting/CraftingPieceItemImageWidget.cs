using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x0200016C RID: 364
	public class CraftingPieceItemImageWidget : ImageWidget
	{
		// Token: 0x0600134C RID: 4940 RVA: 0x00034EEA File Offset: 0x000330EA
		public CraftingPieceItemImageWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x00034EF3 File Offset: 0x000330F3
		private void UpdateSelfBrush()
		{
			if (this.DontHavePieceBrush == null || this.HasPieceBrush == null)
			{
				return;
			}
			base.Brush = (this.PlayerHasPiece ? this.HasPieceBrush : this.DontHavePieceBrush);
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x00034F22 File Offset: 0x00033122
		private void UpdateMaterialBrush()
		{
			if (this.DontHavePieceMaterialBrush == null || this.HasPieceMaterialBrush == null || this.ImageIdentifier == null)
			{
				return;
			}
			this.ImageIdentifier.Brush = (this.PlayerHasPiece ? this.HasPieceMaterialBrush : this.DontHavePieceMaterialBrush);
		}

		// Token: 0x170006D7 RID: 1751
		// (get) Token: 0x0600134F RID: 4943 RVA: 0x00034F5E File Offset: 0x0003315E
		// (set) Token: 0x06001350 RID: 4944 RVA: 0x00034F66 File Offset: 0x00033166
		public ImageIdentifierWidget ImageIdentifier
		{
			get
			{
				return this._imageIdentifier;
			}
			set
			{
				if (this._imageIdentifier != value)
				{
					this._imageIdentifier = value;
					this.UpdateMaterialBrush();
				}
			}
		}

		// Token: 0x170006D8 RID: 1752
		// (get) Token: 0x06001351 RID: 4945 RVA: 0x00034F7E File Offset: 0x0003317E
		// (set) Token: 0x06001352 RID: 4946 RVA: 0x00034F86 File Offset: 0x00033186
		public bool PlayerHasPiece
		{
			get
			{
				return this._playerHasPiece;
			}
			set
			{
				if (this._playerHasPiece != value)
				{
					this._playerHasPiece = value;
					this.UpdateSelfBrush();
					this.UpdateMaterialBrush();
				}
			}
		}

		// Token: 0x170006D9 RID: 1753
		// (get) Token: 0x06001353 RID: 4947 RVA: 0x00034FA4 File Offset: 0x000331A4
		// (set) Token: 0x06001354 RID: 4948 RVA: 0x00034FAC File Offset: 0x000331AC
		public Brush HasPieceBrush
		{
			get
			{
				return this._hasPieceBrush;
			}
			set
			{
				if (this._hasPieceBrush != value)
				{
					this._hasPieceBrush = value;
					this.UpdateSelfBrush();
				}
			}
		}

		// Token: 0x170006DA RID: 1754
		// (get) Token: 0x06001355 RID: 4949 RVA: 0x00034FC4 File Offset: 0x000331C4
		// (set) Token: 0x06001356 RID: 4950 RVA: 0x00034FCC File Offset: 0x000331CC
		public Brush DontHavePieceBrush
		{
			get
			{
				return this._dontHavePieceBrush;
			}
			set
			{
				if (this._dontHavePieceBrush != value)
				{
					this._dontHavePieceBrush = value;
					this.UpdateSelfBrush();
				}
			}
		}

		// Token: 0x170006DB RID: 1755
		// (get) Token: 0x06001357 RID: 4951 RVA: 0x00034FE4 File Offset: 0x000331E4
		// (set) Token: 0x06001358 RID: 4952 RVA: 0x00034FEC File Offset: 0x000331EC
		public Brush HasPieceMaterialBrush
		{
			get
			{
				return this._hasPieceMaterialBrush;
			}
			set
			{
				if (this._hasPieceMaterialBrush != value)
				{
					this._hasPieceMaterialBrush = value;
					this.UpdateMaterialBrush();
				}
			}
		}

		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06001359 RID: 4953 RVA: 0x00035004 File Offset: 0x00033204
		// (set) Token: 0x0600135A RID: 4954 RVA: 0x0003500C File Offset: 0x0003320C
		public Brush DontHavePieceMaterialBrush
		{
			get
			{
				return this._dontHavePieceMaterialBrush;
			}
			set
			{
				if (this._dontHavePieceMaterialBrush != value)
				{
					this._dontHavePieceMaterialBrush = value;
					this.UpdateMaterialBrush();
				}
			}
		}

		// Token: 0x040008CB RID: 2251
		private ImageIdentifierWidget _imageIdentifier;

		// Token: 0x040008CC RID: 2252
		private bool _playerHasPiece;

		// Token: 0x040008CD RID: 2253
		private Brush _hasPieceBrush;

		// Token: 0x040008CE RID: 2254
		private Brush _dontHavePieceBrush;

		// Token: 0x040008CF RID: 2255
		private Brush _hasPieceMaterialBrush;

		// Token: 0x040008D0 RID: 2256
		private Brush _dontHavePieceMaterialBrush;
	}
}
