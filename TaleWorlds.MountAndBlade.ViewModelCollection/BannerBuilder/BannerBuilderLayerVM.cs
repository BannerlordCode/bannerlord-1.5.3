using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.BannerBuilder
{
	// Token: 0x02000088 RID: 136
	public class BannerBuilderLayerVM : ViewModel
	{
		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000B0B RID: 2827 RVA: 0x00027321 File Offset: 0x00025521
		// (set) Token: 0x06000B0C RID: 2828 RVA: 0x00027329 File Offset: 0x00025529
		public BannerData Data { get; private set; }

		// Token: 0x06000B0D RID: 2829 RVA: 0x00027334 File Offset: 0x00025534
		public BannerBuilderLayerVM(BannerData data, int layerIndex)
		{
			this.Data = data;
			this.LayerIndex = layerIndex;
			this._rotationValue = this.Data.Rotation;
			this._positionValue = this.Data.Position;
			this._sizeValue = this.Data.Size;
			this._isDrawStrokeActive = this.Data.DrawStroke;
			this._isMirrorActive = this.Data.Mirror;
			this.Refresh();
			this.IsLayerPattern = layerIndex == 0;
			this.CanDeleteLayer = !this.IsLayerPattern;
			this.TotalAreaSize = 1528;
			this.EditableAreaSize = 512;
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x000273E0 File Offset: 0x000255E0
		public void Refresh()
		{
			this.IconID = this.Data.MeshId;
			this.IconIDAsString = this.IconID.ToString();
			uint color = BannerManager.Instance.ReadOnlyColorPalette[this.Data.ColorId].Color;
			this.Color1 = Color.FromUint(color);
			uint color2 = BannerManager.Instance.ReadOnlyColorPalette[this.Data.ColorId2].Color;
			this.Color2 = Color.FromUint(color2);
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00027470 File Offset: 0x00025670
		public void ExecuteDelete()
		{
			Action<BannerBuilderLayerVM> onDeletion = BannerBuilderLayerVM._onDeletion;
			if (onDeletion == null)
			{
				return;
			}
			onDeletion(this);
		}

		// Token: 0x06000B10 RID: 2832 RVA: 0x00027482 File Offset: 0x00025682
		public void ExecuteSelection()
		{
			Action<BannerBuilderLayerVM> onSelection = BannerBuilderLayerVM._onSelection;
			if (onSelection == null)
			{
				return;
			}
			onSelection(this);
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x00027494 File Offset: 0x00025694
		public void SetLayerIndex(int newIndex)
		{
			this.LayerIndex = newIndex;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x0002749D File Offset: 0x0002569D
		public void ExecuteSelectColor1()
		{
			Action<int, Action<BannerBuilderColorItemVM>> onColorSelection = BannerBuilderLayerVM._onColorSelection;
			if (onColorSelection == null)
			{
				return;
			}
			onColorSelection(this.Data.ColorId, new Action<BannerBuilderColorItemVM>(this.OnSelectColor1));
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x000274C8 File Offset: 0x000256C8
		private void OnSelectColor1(BannerBuilderColorItemVM selectedColor)
		{
			this.Data.ColorId = selectedColor.ColorID;
			this.Color1 = Color.FromUint(selectedColor.BannerColor.Color);
			this.ExecuteUpdateBanner();
		}

		// Token: 0x06000B14 RID: 2836 RVA: 0x00027505 File Offset: 0x00025705
		public void ExecuteSelectColor2()
		{
			Action<int, Action<BannerBuilderColorItemVM>> onColorSelection = BannerBuilderLayerVM._onColorSelection;
			if (onColorSelection == null)
			{
				return;
			}
			onColorSelection(this.Data.ColorId2, new Action<BannerBuilderColorItemVM>(this.OnSelectColor2));
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00027530 File Offset: 0x00025730
		private void OnSelectColor2(BannerBuilderColorItemVM selectedColor)
		{
			this.Data.ColorId2 = selectedColor.ColorID;
			this.Color2 = Color.FromUint(selectedColor.BannerColor.Color);
			this.ExecuteUpdateBanner();
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00027570 File Offset: 0x00025770
		public void ExecuteSwapColors()
		{
			int colorId = this.Data.ColorId2;
			this.Data.ColorId2 = this.Data.ColorId;
			this.Data.ColorId = colorId;
			Color color = this.Color2;
			Color color2 = this.Color1;
			this.Color1 = color;
			this.Color2 = color2;
			this.Refresh();
			this.ExecuteUpdateBanner();
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x000275D7 File Offset: 0x000257D7
		public void ExecuteCenterSigil()
		{
			this.PositionValue = new Vec2((float)this.TotalAreaSize / 2f, (float)this.TotalAreaSize / 2f);
			this.ExecuteUpdateBanner();
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00027604 File Offset: 0x00025804
		public void ExecuteResetSize()
		{
			float num = (float)(this.IsLayerPattern ? this.TotalAreaSize : 483);
			this.SizeValue = new Vec2(num, num);
			this.ExecuteUpdateBanner();
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x0002763B File Offset: 0x0002583B
		public void ExecuteUpdateBanner()
		{
			Action refresh = BannerBuilderLayerVM._refresh;
			if (refresh == null)
			{
				return;
			}
			refresh();
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000B1A RID: 2842 RVA: 0x0002764C File Offset: 0x0002584C
		// (set) Token: 0x06000B1B RID: 2843 RVA: 0x00027654 File Offset: 0x00025854
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000B1C RID: 2844 RVA: 0x00027672 File Offset: 0x00025872
		// (set) Token: 0x06000B1D RID: 2845 RVA: 0x0002767A File Offset: 0x0002587A
		[DataSourceProperty]
		public bool CanDeleteLayer
		{
			get
			{
				return this._canDeleteLayer;
			}
			set
			{
				if (value != this._canDeleteLayer)
				{
					this._canDeleteLayer = value;
					base.OnPropertyChangedWithValue(value, "CanDeleteLayer");
				}
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06000B1E RID: 2846 RVA: 0x00027698 File Offset: 0x00025898
		// (set) Token: 0x06000B1F RID: 2847 RVA: 0x000276A0 File Offset: 0x000258A0
		[DataSourceProperty]
		public bool IsLayerPattern
		{
			get
			{
				return this._isLayerPattern;
			}
			set
			{
				if (value != this._isLayerPattern)
				{
					this._isLayerPattern = value;
					base.OnPropertyChangedWithValue(value, "IsLayerPattern");
				}
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06000B20 RID: 2848 RVA: 0x000276BE File Offset: 0x000258BE
		// (set) Token: 0x06000B21 RID: 2849 RVA: 0x000276C6 File Offset: 0x000258C6
		[DataSourceProperty]
		public bool IsDrawStrokeActive
		{
			get
			{
				return this._isDrawStrokeActive;
			}
			set
			{
				if (value != this._isDrawStrokeActive)
				{
					this._isDrawStrokeActive = value;
					base.OnPropertyChangedWithValue(value, "IsDrawStrokeActive");
					this.Data.DrawStroke = value;
					this.ExecuteUpdateBanner();
				}
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06000B22 RID: 2850 RVA: 0x000276F6 File Offset: 0x000258F6
		// (set) Token: 0x06000B23 RID: 2851 RVA: 0x000276FE File Offset: 0x000258FE
		[DataSourceProperty]
		public bool IsMirrorActive
		{
			get
			{
				return this._isMirrorActive;
			}
			set
			{
				if (value != this._isMirrorActive)
				{
					this._isMirrorActive = value;
					base.OnPropertyChangedWithValue(value, "IsMirrorActive");
					this.Data.Mirror = value;
					this.ExecuteUpdateBanner();
				}
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000B24 RID: 2852 RVA: 0x0002772E File Offset: 0x0002592E
		// (set) Token: 0x06000B25 RID: 2853 RVA: 0x00027736 File Offset: 0x00025936
		[DataSourceProperty]
		public float RotationValue
		{
			get
			{
				return this._rotationValue;
			}
			set
			{
				if (value != this._rotationValue)
				{
					this._rotationValue = value;
					this.Data.RotationValue = value;
					base.OnPropertyChangedWithValue(value, "RotationValue");
					base.OnPropertyChanged("RotationValue360");
				}
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000B26 RID: 2854 RVA: 0x0002776B File Offset: 0x0002596B
		// (set) Token: 0x06000B27 RID: 2855 RVA: 0x0002777A File Offset: 0x0002597A
		[DataSourceProperty]
		public int RotationValue360
		{
			get
			{
				return (int)(this._rotationValue * 360f);
			}
			set
			{
				if (value != (int)(this._rotationValue * 360f))
				{
					this.RotationValue = (float)value / 360f;
					base.OnPropertyChangedWithValue(value, "RotationValue360");
				}
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000B28 RID: 2856 RVA: 0x000277A6 File Offset: 0x000259A6
		// (set) Token: 0x06000B29 RID: 2857 RVA: 0x000277AE File Offset: 0x000259AE
		[DataSourceProperty]
		public int IconID
		{
			get
			{
				return this._iconID;
			}
			set
			{
				if (value != this._iconID)
				{
					this._iconID = value;
					base.OnPropertyChangedWithValue(value, "IconID");
				}
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x000277CC File Offset: 0x000259CC
		// (set) Token: 0x06000B2B RID: 2859 RVA: 0x000277D4 File Offset: 0x000259D4
		[DataSourceProperty]
		public int LayerIndex
		{
			get
			{
				return this._layerIndex;
			}
			set
			{
				if (value != this._layerIndex)
				{
					this._layerIndex = value;
					base.OnPropertyChangedWithValue(value, "LayerIndex");
				}
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x000277F2 File Offset: 0x000259F2
		// (set) Token: 0x06000B2D RID: 2861 RVA: 0x000277FA File Offset: 0x000259FA
		[DataSourceProperty]
		public int EditableAreaSize
		{
			get
			{
				return this._editableAreaSize;
			}
			set
			{
				if (value != this._editableAreaSize)
				{
					this._editableAreaSize = value;
					base.OnPropertyChangedWithValue(value, "EditableAreaSize");
				}
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06000B2E RID: 2862 RVA: 0x00027818 File Offset: 0x00025A18
		// (set) Token: 0x06000B2F RID: 2863 RVA: 0x00027820 File Offset: 0x00025A20
		[DataSourceProperty]
		public int TotalAreaSize
		{
			get
			{
				return this._totalAreaSize;
			}
			set
			{
				if (value != this._totalAreaSize)
				{
					this._totalAreaSize = value;
					base.OnPropertyChangedWithValue(value, "TotalAreaSize");
				}
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x0002783E File Offset: 0x00025A3E
		// (set) Token: 0x06000B31 RID: 2865 RVA: 0x00027846 File Offset: 0x00025A46
		[DataSourceProperty]
		public string IconIDAsString
		{
			get
			{
				return this._iconIDAsString;
			}
			set
			{
				if (value != this._iconIDAsString)
				{
					this._iconIDAsString = value;
					base.OnPropertyChangedWithValue<string>(value, "IconIDAsString");
				}
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x00027869 File Offset: 0x00025A69
		// (set) Token: 0x06000B33 RID: 2867 RVA: 0x00027871 File Offset: 0x00025A71
		[DataSourceProperty]
		public Color Color1
		{
			get
			{
				return this._color1;
			}
			set
			{
				if (value != this._color1)
				{
					this._color1 = value;
					base.OnPropertyChangedWithValue(value, "Color1");
					this.Color1AsStr = value.ToString();
				}
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06000B34 RID: 2868 RVA: 0x000278A7 File Offset: 0x00025AA7
		// (set) Token: 0x06000B35 RID: 2869 RVA: 0x000278AF File Offset: 0x00025AAF
		[DataSourceProperty]
		public Color Color2
		{
			get
			{
				return this._color2;
			}
			set
			{
				if (value != this._color2)
				{
					this._color2 = value;
					base.OnPropertyChangedWithValue(value, "Color2");
					this.Color2AsStr = value.ToString();
				}
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000B36 RID: 2870 RVA: 0x000278E5 File Offset: 0x00025AE5
		// (set) Token: 0x06000B37 RID: 2871 RVA: 0x000278ED File Offset: 0x00025AED
		[DataSourceProperty]
		public string Color1AsStr
		{
			get
			{
				return this._color1AsStr;
			}
			set
			{
				if (value != this._color1AsStr)
				{
					this._color1AsStr = value;
					base.OnPropertyChangedWithValue<string>(value, "Color1AsStr");
				}
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x00027910 File Offset: 0x00025B10
		// (set) Token: 0x06000B39 RID: 2873 RVA: 0x00027918 File Offset: 0x00025B18
		[DataSourceProperty]
		public string Color2AsStr
		{
			get
			{
				return this._color2AsStr;
			}
			set
			{
				if (value != this._color2AsStr)
				{
					this._color2AsStr = value;
					base.OnPropertyChangedWithValue<string>(value, "Color2AsStr");
				}
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06000B3A RID: 2874 RVA: 0x0002793B File Offset: 0x00025B3B
		// (set) Token: 0x06000B3B RID: 2875 RVA: 0x00027944 File Offset: 0x00025B44
		[DataSourceProperty]
		public Vec2 PositionValue
		{
			get
			{
				return this._positionValue;
			}
			set
			{
				if (this._positionValue != value)
				{
					this._positionValue = value;
					base.OnPropertyChangedWithValue(value, "PositionValue");
					base.OnPropertyChanged("PositionValueX");
					base.OnPropertyChanged("PositionValueY");
					this.Data.Position = value;
				}
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x00027994 File Offset: 0x00025B94
		// (set) Token: 0x06000B3D RID: 2877 RVA: 0x000279A8 File Offset: 0x00025BA8
		[DataSourceProperty]
		public float PositionValueX
		{
			get
			{
				return (float)Math.Round((double)this._positionValue.X);
			}
			set
			{
				value = (float)Math.Round((double)value);
				if (value != this._positionValue.X)
				{
					this.PositionValue = new Vec2(value, this._positionValue.Y);
					this.Data.Position = this._positionValue;
					base.OnPropertyChangedWithValue(value, "PositionValueX");
				}
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06000B3E RID: 2878 RVA: 0x00027A01 File Offset: 0x00025C01
		// (set) Token: 0x06000B3F RID: 2879 RVA: 0x00027A18 File Offset: 0x00025C18
		[DataSourceProperty]
		public float PositionValueY
		{
			get
			{
				return (float)Math.Round((double)this._positionValue.Y);
			}
			set
			{
				value = (float)Math.Round((double)value);
				if (value != this._positionValue.Y)
				{
					this.PositionValue = new Vec2(this._positionValue.X, value);
					this.Data.Position = this._positionValue;
					base.OnPropertyChangedWithValue(value, "PositionValueY");
				}
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00027A71 File Offset: 0x00025C71
		// (set) Token: 0x06000B41 RID: 2881 RVA: 0x00027A7C File Offset: 0x00025C7C
		[DataSourceProperty]
		public Vec2 SizeValue
		{
			get
			{
				return this._sizeValue;
			}
			set
			{
				if (this._sizeValue != value)
				{
					this._sizeValue = value;
					base.OnPropertyChangedWithValue(value, "SizeValue");
					base.OnPropertyChanged("SizeValueX");
					base.OnPropertyChanged("SizeValueY");
					this.Data.Size = value;
				}
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x00027ACC File Offset: 0x00025CCC
		// (set) Token: 0x06000B43 RID: 2883 RVA: 0x00027AE0 File Offset: 0x00025CE0
		[DataSourceProperty]
		public float SizeValueX
		{
			get
			{
				return (float)Math.Round((double)this._sizeValue.X);
			}
			set
			{
				value = (float)Math.Round((double)value);
				if (value != this._sizeValue.X)
				{
					this.SizeValue = new Vec2(value, this._sizeValue.Y);
					this.Data.Size = this._sizeValue;
					base.OnPropertyChangedWithValue(value, "SizeValueX");
				}
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06000B44 RID: 2884 RVA: 0x00027B39 File Offset: 0x00025D39
		// (set) Token: 0x06000B45 RID: 2885 RVA: 0x00027B50 File Offset: 0x00025D50
		[DataSourceProperty]
		public float SizeValueY
		{
			get
			{
				return (float)Math.Round((double)this._sizeValue.Y);
			}
			set
			{
				value = (float)Math.Round((double)value);
				if (value != this._sizeValue.Y)
				{
					this.SizeValue = new Vec2(this._sizeValue.X, value);
					this.Data.Size = this._sizeValue;
					base.OnPropertyChangedWithValue(value, "SizeValueY");
				}
			}
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x00027BA9 File Offset: 0x00025DA9
		public static void SetLayerActions(Action refresh, Action<BannerBuilderLayerVM> onSelection, Action<BannerBuilderLayerVM> onDeletion, Action<int, Action<BannerBuilderColorItemVM>> onColorSelection)
		{
			BannerBuilderLayerVM._onSelection = onSelection;
			BannerBuilderLayerVM._onDeletion = onDeletion;
			BannerBuilderLayerVM._onColorSelection = onColorSelection;
			BannerBuilderLayerVM._refresh = refresh;
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00027BC3 File Offset: 0x00025DC3
		public static void ResetLayerActions()
		{
			BannerBuilderLayerVM._onSelection = null;
			BannerBuilderLayerVM._onDeletion = null;
			BannerBuilderLayerVM._onColorSelection = null;
			BannerBuilderLayerVM._refresh = null;
		}

		// Token: 0x0400051B RID: 1307
		private static Action<BannerBuilderLayerVM> _onSelection;

		// Token: 0x0400051C RID: 1308
		private static Action<BannerBuilderLayerVM> _onDeletion;

		// Token: 0x0400051D RID: 1309
		private static Action<int, Action<BannerBuilderColorItemVM>> _onColorSelection;

		// Token: 0x0400051E RID: 1310
		private static Action _refresh;

		// Token: 0x0400051F RID: 1311
		private int _iconID;

		// Token: 0x04000520 RID: 1312
		private string _iconIDAsString;

		// Token: 0x04000521 RID: 1313
		private Color _color1;

		// Token: 0x04000522 RID: 1314
		private Color _color2;

		// Token: 0x04000523 RID: 1315
		private string _color1AsStr;

		// Token: 0x04000524 RID: 1316
		private string _color2AsStr;

		// Token: 0x04000525 RID: 1317
		private bool _isSelected;

		// Token: 0x04000526 RID: 1318
		private bool _canDeleteLayer;

		// Token: 0x04000527 RID: 1319
		private bool _isLayerPattern;

		// Token: 0x04000528 RID: 1320
		private bool _isDrawStrokeActive;

		// Token: 0x04000529 RID: 1321
		private bool _isMirrorActive;

		// Token: 0x0400052A RID: 1322
		private int _editableAreaSize;

		// Token: 0x0400052B RID: 1323
		private int _totalAreaSize;

		// Token: 0x0400052C RID: 1324
		private int _layerIndex;

		// Token: 0x0400052D RID: 1325
		private float _rotationValue;

		// Token: 0x0400052E RID: 1326
		private Vec2 _positionValue;

		// Token: 0x0400052F RID: 1327
		private Vec2 _sizeValue;
	}
}
