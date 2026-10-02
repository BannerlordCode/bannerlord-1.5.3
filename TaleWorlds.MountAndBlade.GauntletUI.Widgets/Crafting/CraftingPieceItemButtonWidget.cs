using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Crafting
{
	// Token: 0x0200016B RID: 363
	public class CraftingPieceItemButtonWidget : ButtonWidget
	{
		// Token: 0x0600133D RID: 4925 RVA: 0x00034DB0 File Offset: 0x00032FB0
		public CraftingPieceItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x00034DB9 File Offset: 0x00032FB9
		private void UpdateSelfBrush()
		{
			if (this.DontHavePieceBrush == null || this.HasPieceBrush == null)
			{
				return;
			}
			base.Brush = (this.PlayerHasPiece ? this.HasPieceBrush : this.DontHavePieceBrush);
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x00034DE8 File Offset: 0x00032FE8
		private void UpdateMaterialBrush()
		{
			if (this.DontHavePieceMaterialBrush == null || this.HasPieceMaterialBrush == null || this.ImageIdentifier == null)
			{
				return;
			}
			this.ImageIdentifier.Brush = (this.PlayerHasPiece ? this.HasPieceMaterialBrush : this.DontHavePieceMaterialBrush);
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06001340 RID: 4928 RVA: 0x00034E24 File Offset: 0x00033024
		// (set) Token: 0x06001341 RID: 4929 RVA: 0x00034E2C File Offset: 0x0003302C
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

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06001342 RID: 4930 RVA: 0x00034E44 File Offset: 0x00033044
		// (set) Token: 0x06001343 RID: 4931 RVA: 0x00034E4C File Offset: 0x0003304C
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

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06001344 RID: 4932 RVA: 0x00034E6A File Offset: 0x0003306A
		// (set) Token: 0x06001345 RID: 4933 RVA: 0x00034E72 File Offset: 0x00033072
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

		// Token: 0x170006D4 RID: 1748
		// (get) Token: 0x06001346 RID: 4934 RVA: 0x00034E8A File Offset: 0x0003308A
		// (set) Token: 0x06001347 RID: 4935 RVA: 0x00034E92 File Offset: 0x00033092
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

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06001348 RID: 4936 RVA: 0x00034EAA File Offset: 0x000330AA
		// (set) Token: 0x06001349 RID: 4937 RVA: 0x00034EB2 File Offset: 0x000330B2
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

		// Token: 0x170006D6 RID: 1750
		// (get) Token: 0x0600134A RID: 4938 RVA: 0x00034ECA File Offset: 0x000330CA
		// (set) Token: 0x0600134B RID: 4939 RVA: 0x00034ED2 File Offset: 0x000330D2
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

		// Token: 0x040008C5 RID: 2245
		private ImageIdentifierWidget _imageIdentifier;

		// Token: 0x040008C6 RID: 2246
		private bool _playerHasPiece;

		// Token: 0x040008C7 RID: 2247
		private Brush _hasPieceBrush;

		// Token: 0x040008C8 RID: 2248
		private Brush _dontHavePieceBrush;

		// Token: 0x040008C9 RID: 2249
		private Brush _hasPieceMaterialBrush;

		// Token: 0x040008CA RID: 2250
		private Brush _dontHavePieceMaterialBrush;
	}
}
