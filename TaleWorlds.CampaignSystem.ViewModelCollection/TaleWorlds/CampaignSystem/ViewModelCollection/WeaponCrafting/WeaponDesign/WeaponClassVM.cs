using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x0200010F RID: 271
	public class WeaponClassVM : ViewModel
	{
		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x060017DC RID: 6108 RVA: 0x0005BA6C File Offset: 0x00059C6C
		// (set) Token: 0x060017DD RID: 6109 RVA: 0x0005BA74 File Offset: 0x00059C74
		public int NewlyUnlockedPieceCount { get; set; }

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x060017DE RID: 6110 RVA: 0x0005BA7D File Offset: 0x00059C7D
		public CraftingTemplate Template { get; }

		// Token: 0x060017DF RID: 6111 RVA: 0x0005BA88 File Offset: 0x00059C88
		public WeaponClassVM(int selectionIndex, CraftingTemplate template, Action<int> onSelect)
		{
			this._onSelect = onSelect;
			this.SelectionIndex = selectionIndex;
			this.Template = template;
			this._selectedPieces = new Dictionary<CraftingPiece.PieceTypes, string>
			{
				{
					CraftingPiece.PieceTypes.Blade,
					null
				},
				{
					CraftingPiece.PieceTypes.Guard,
					null
				},
				{
					CraftingPiece.PieceTypes.Handle,
					null
				},
				{
					CraftingPiece.PieceTypes.Pommel,
					null
				}
			};
			this.RefreshValues();
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x0005BAE4 File Offset: 0x00059CE4
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.TemplateName = this.Template.TemplateName.ToString();
			this.UnlockedPiecesLabelText = new TextObject("{=OGbskMfz}Unlocked Parts:", null).ToString();
			this.WeaponType = this.Template.StringId;
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x0005BB34 File Offset: 0x00059D34
		public void RegisterSelectedPiece(CraftingPiece.PieceTypes type, string pieceID)
		{
			string text;
			if (this._selectedPieces.TryGetValue(type, out text) && text != pieceID)
			{
				this._selectedPieces[type] = pieceID;
			}
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x0005BB68 File Offset: 0x00059D68
		public string GetSelectedPieceData(CraftingPiece.PieceTypes type)
		{
			string text;
			if (this._selectedPieces.TryGetValue(type, out text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x0005BB88 File Offset: 0x00059D88
		public void ExecuteSelect()
		{
			Action<int> onSelect = this._onSelect;
			if (onSelect == null)
			{
				return;
			}
			onSelect(this.SelectionIndex);
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x060017E4 RID: 6116 RVA: 0x0005BBA0 File Offset: 0x00059DA0
		// (set) Token: 0x060017E5 RID: 6117 RVA: 0x0005BBA8 File Offset: 0x00059DA8
		[DataSourceProperty]
		public bool HasNewlyUnlockedPieces
		{
			get
			{
				return this._hasNewlyUnlockedPieces;
			}
			set
			{
				if (value != this._hasNewlyUnlockedPieces)
				{
					this._hasNewlyUnlockedPieces = value;
					base.OnPropertyChangedWithValue(value, "HasNewlyUnlockedPieces");
				}
			}
		}

		// Token: 0x170007ED RID: 2029
		// (get) Token: 0x060017E6 RID: 6118 RVA: 0x0005BBC6 File Offset: 0x00059DC6
		// (set) Token: 0x060017E7 RID: 6119 RVA: 0x0005BBCE File Offset: 0x00059DCE
		[DataSourceProperty]
		public string UnlockedPiecesLabelText
		{
			get
			{
				return this._unlockedPiecesLabelText;
			}
			set
			{
				if (value != this._unlockedPiecesLabelText)
				{
					this._unlockedPiecesLabelText = value;
					base.OnPropertyChangedWithValue<string>(value, "UnlockedPiecesLabelText");
				}
			}
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x060017E8 RID: 6120 RVA: 0x0005BBF1 File Offset: 0x00059DF1
		// (set) Token: 0x060017E9 RID: 6121 RVA: 0x0005BBF9 File Offset: 0x00059DF9
		[DataSourceProperty]
		public int UnlockedPiecesCount
		{
			get
			{
				return this._unlockedPiecesCount;
			}
			set
			{
				if (value != this._unlockedPiecesCount)
				{
					this._unlockedPiecesCount = value;
					base.OnPropertyChangedWithValue(value, "UnlockedPiecesCount");
				}
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x060017EA RID: 6122 RVA: 0x0005BC17 File Offset: 0x00059E17
		// (set) Token: 0x060017EB RID: 6123 RVA: 0x0005BC1F File Offset: 0x00059E1F
		[DataSourceProperty]
		public string TemplateName
		{
			get
			{
				return this._templateName;
			}
			set
			{
				if (value != this._templateName)
				{
					this._templateName = value;
					base.OnPropertyChangedWithValue<string>(value, "TemplateName");
				}
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x060017EC RID: 6124 RVA: 0x0005BC42 File Offset: 0x00059E42
		// (set) Token: 0x060017ED RID: 6125 RVA: 0x0005BC4A File Offset: 0x00059E4A
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

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x060017EE RID: 6126 RVA: 0x0005BC68 File Offset: 0x00059E68
		// (set) Token: 0x060017EF RID: 6127 RVA: 0x0005BC70 File Offset: 0x00059E70
		[DataSourceProperty]
		public int SelectionIndex
		{
			get
			{
				return this._selectionIndex;
			}
			set
			{
				if (value != this._selectionIndex)
				{
					this._selectionIndex = value;
					base.OnPropertyChangedWithValue(value, "SelectionIndex");
				}
			}
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x060017F0 RID: 6128 RVA: 0x0005BC8E File Offset: 0x00059E8E
		// (set) Token: 0x060017F1 RID: 6129 RVA: 0x0005BC96 File Offset: 0x00059E96
		[DataSourceProperty]
		public string WeaponType
		{
			get
			{
				return this._weaponType;
			}
			set
			{
				if (value != this._weaponType)
				{
					this._weaponType = value;
					base.OnPropertyChangedWithValue<string>(value, "WeaponType");
				}
			}
		}

		// Token: 0x04000AE2 RID: 2786
		private Action<int> _onSelect;

		// Token: 0x04000AE3 RID: 2787
		private Dictionary<CraftingPiece.PieceTypes, string> _selectedPieces;

		// Token: 0x04000AE4 RID: 2788
		private bool _hasNewlyUnlockedPieces;

		// Token: 0x04000AE5 RID: 2789
		private string _unlockedPiecesLabelText;

		// Token: 0x04000AE6 RID: 2790
		private int _unlockedPiecesCount;

		// Token: 0x04000AE7 RID: 2791
		private string _templateName;

		// Token: 0x04000AE8 RID: 2792
		private bool _isSelected;

		// Token: 0x04000AE9 RID: 2793
		private int _selectionIndex;

		// Token: 0x04000AEA RID: 2794
		private string _weaponType;
	}
}
