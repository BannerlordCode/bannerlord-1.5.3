using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Armory
{
	// Token: 0x020000B4 RID: 180
	public class MultiplayerArmoryCosmeticCategoryButtonWidget : ButtonWidget
	{
		// Token: 0x06000976 RID: 2422 RVA: 0x0001AD71 File Offset: 0x00018F71
		public MultiplayerArmoryCosmeticCategoryButtonWidget(UIContext context)
			: base(context)
		{
			this.CosmeticTypeName = string.Empty;
			this.CosmeticCategoryName = string.Empty;
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0001AD90 File Offset: 0x00018F90
		private void UpdateCategorySprite()
		{
			if (string.IsNullOrEmpty(this.CosmeticCategoryName) || string.IsNullOrEmpty(this.CosmeticTypeName))
			{
				return;
			}
			Sprite sprite = null;
			if (this.CosmeticTypeName == "Clothing")
			{
				sprite = this.GetClothingCategorySprite(this.CosmeticCategoryName);
			}
			else if (this.CosmeticTypeName == "Taunt")
			{
				sprite = this.GetTauntCategorySprite(this.CosmeticCategoryName);
			}
			if (sprite != null)
			{
				base.Brush.DefaultLayer.Sprite = sprite;
				base.Brush.Sprite = sprite;
			}
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x0001AE1A File Offset: 0x0001901A
		private Sprite GetClothingCategorySprite(string clothingCategory)
		{
			Brush clothingCategorySpriteBrush = this.ClothingCategorySpriteBrush;
			if (clothingCategorySpriteBrush == null)
			{
				return null;
			}
			BrushLayer layer = clothingCategorySpriteBrush.GetLayer(clothingCategory);
			if (layer == null)
			{
				return null;
			}
			return layer.Sprite;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x0001AE39 File Offset: 0x00019039
		private Sprite GetTauntCategorySprite(string tauntCategory)
		{
			Brush tauntCategorySpriteBrush = this.TauntCategorySpriteBrush;
			if (tauntCategorySpriteBrush == null)
			{
				return null;
			}
			BrushLayer layer = tauntCategorySpriteBrush.GetLayer(tauntCategory);
			if (layer == null)
			{
				return null;
			}
			return layer.Sprite;
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x0001AE58 File Offset: 0x00019058
		// (set) Token: 0x0600097B RID: 2427 RVA: 0x0001AE60 File Offset: 0x00019060
		[DataSourceProperty]
		public Brush ClothingCategorySpriteBrush
		{
			get
			{
				return this._clothingCategorySpriteBrush;
			}
			set
			{
				if (value != this._clothingCategorySpriteBrush)
				{
					this._clothingCategorySpriteBrush = value;
					base.OnPropertyChanged<Brush>(value, "ClothingCategorySpriteBrush");
					this.UpdateCategorySprite();
				}
			}
		}

		// Token: 0x17000350 RID: 848
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x0001AE84 File Offset: 0x00019084
		// (set) Token: 0x0600097D RID: 2429 RVA: 0x0001AE8C File Offset: 0x0001908C
		[DataSourceProperty]
		public Brush TauntCategorySpriteBrush
		{
			get
			{
				return this._tauntCategorySpriteBrush;
			}
			set
			{
				if (value != this._tauntCategorySpriteBrush)
				{
					this._tauntCategorySpriteBrush = value;
					base.OnPropertyChanged<Brush>(value, "TauntCategorySpriteBrush");
					this.UpdateCategorySprite();
				}
			}
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x0001AEB0 File Offset: 0x000190B0
		// (set) Token: 0x0600097F RID: 2431 RVA: 0x0001AEB8 File Offset: 0x000190B8
		[DataSourceProperty]
		public string CosmeticTypeName
		{
			get
			{
				return this._cosmeticTypeName;
			}
			set
			{
				if (value != this._cosmeticTypeName)
				{
					this._cosmeticTypeName = value;
					base.OnPropertyChanged<string>(value, "CosmeticTypeName");
					this.UpdateCategorySprite();
				}
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x0001AEE1 File Offset: 0x000190E1
		// (set) Token: 0x06000981 RID: 2433 RVA: 0x0001AEE9 File Offset: 0x000190E9
		[DataSourceProperty]
		public string CosmeticCategoryName
		{
			get
			{
				return this._cosmeticCategoryName;
			}
			set
			{
				if (value != this._cosmeticCategoryName)
				{
					this._cosmeticCategoryName = value;
					base.OnPropertyChanged<string>(value, "CosmeticCategoryName");
					this.UpdateCategorySprite();
				}
			}
		}

		// Token: 0x04000442 RID: 1090
		private const string _clothingTypeName = "Clothing";

		// Token: 0x04000443 RID: 1091
		private const string _tauntTypeName = "Taunt";

		// Token: 0x04000444 RID: 1092
		private Brush _clothingCategorySpriteBrush;

		// Token: 0x04000445 RID: 1093
		private Brush _tauntCategorySpriteBrush;

		// Token: 0x04000446 RID: 1094
		private string _cosmeticTypeName;

		// Token: 0x04000447 RID: 1095
		private string _cosmeticCategoryName;
	}
}
