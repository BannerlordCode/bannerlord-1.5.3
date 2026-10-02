using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting.WeaponDesign
{
	// Token: 0x0200010B RID: 267
	public class WeaponClassSelectionPopupVM : ViewModel
	{
		// Token: 0x060017C7 RID: 6087 RVA: 0x0005B7D4 File Offset: 0x000599D4
		public WeaponClassSelectionPopupVM(List<CraftingTemplate> templatesList, Action<int> onSelect, Func<CraftingTemplate, int> getUnlockedPiecesCount, Func<CraftingTemplate, int> getUninspectedPiecesCount)
		{
			this.WeaponClasses = new MBBindingList<WeaponClassVM>();
			this._onSelect = onSelect;
			this._templatesList = templatesList;
			this._getUnlockedPiecesCount = getUnlockedPiecesCount;
			this._getUninspectedPiecesCount = getUninspectedPiecesCount;
			foreach (CraftingTemplate craftingTemplate in this._templatesList)
			{
				this.WeaponClasses.Add(new WeaponClassVM(this._templatesList.IndexOf(craftingTemplate), craftingTemplate, new Action<int>(this.ExecuteSelectWeaponClass)));
			}
			this.RefreshList();
			this.RefreshValues();
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x0005B884 File Offset: 0x00059A84
		private void RefreshList()
		{
			foreach (WeaponClassVM weaponClassVM in this.WeaponClasses)
			{
				WeaponClassVM weaponClassVM2 = weaponClassVM;
				Func<CraftingTemplate, int> getUnlockedPiecesCount = this._getUnlockedPiecesCount;
				weaponClassVM2.UnlockedPiecesCount = ((getUnlockedPiecesCount != null) ? getUnlockedPiecesCount(weaponClassVM.Template) : 0);
				WeaponClassVM weaponClassVM3 = weaponClassVM;
				Func<CraftingTemplate, int> getUninspectedPiecesCount = this._getUninspectedPiecesCount;
				weaponClassVM3.HasNewlyUnlockedPieces = getUninspectedPiecesCount != null && getUninspectedPiecesCount(weaponClassVM.Template) > 0;
			}
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x0005B90C File Offset: 0x00059B0C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PopupHeader = new TextObject("{=wZGj3qO1}Choose What to Craft", null).ToString();
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x0005B92A File Offset: 0x00059B2A
		public void ExecuteSelectWeaponClass(int index)
		{
			if (this.WeaponClasses[index].IsSelected)
			{
				this.ExecuteClosePopup();
				return;
			}
			Action<int> onSelect = this._onSelect;
			if (onSelect != null)
			{
				onSelect(index);
			}
			this.ExecuteClosePopup();
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x0005B95E File Offset: 0x00059B5E
		public void ExecuteClosePopup()
		{
			this.IsVisible = false;
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x0005B967 File Offset: 0x00059B67
		public void ExecuteOpenPopup()
		{
			this.IsVisible = true;
			this.RefreshList();
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x060017CD RID: 6093 RVA: 0x0005B976 File Offset: 0x00059B76
		// (set) Token: 0x060017CE RID: 6094 RVA: 0x0005B97E File Offset: 0x00059B7E
		[DataSourceProperty]
		public string PopupHeader
		{
			get
			{
				return this._popupHeader;
			}
			set
			{
				if (value != this._popupHeader)
				{
					this._popupHeader = value;
					base.OnPropertyChangedWithValue<string>(value, "PopupHeader");
				}
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x060017CF RID: 6095 RVA: 0x0005B9A1 File Offset: 0x00059BA1
		// (set) Token: 0x060017D0 RID: 6096 RVA: 0x0005B9A9 File Offset: 0x00059BA9
		[DataSourceProperty]
		public bool IsVisible
		{
			get
			{
				return this._isVisible;
			}
			set
			{
				if (value != this._isVisible)
				{
					this._isVisible = value;
					base.OnPropertyChangedWithValue(value, "IsVisible");
					Game game = Game.Current;
					if (game == null)
					{
						return;
					}
					game.EventManager.TriggerEvent<CraftingWeaponClassSelectionOpenedEvent>(new CraftingWeaponClassSelectionOpenedEvent(this._isVisible));
				}
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x0005B9E6 File Offset: 0x00059BE6
		// (set) Token: 0x060017D2 RID: 6098 RVA: 0x0005B9EE File Offset: 0x00059BEE
		[DataSourceProperty]
		public MBBindingList<WeaponClassVM> WeaponClasses
		{
			get
			{
				return this._weaponClasses;
			}
			set
			{
				if (value != this._weaponClasses)
				{
					this._weaponClasses = value;
					base.OnPropertyChangedWithValue<MBBindingList<WeaponClassVM>>(value, "WeaponClasses");
				}
			}
		}

		// Token: 0x04000AD6 RID: 2774
		private readonly Action<int> _onSelect;

		// Token: 0x04000AD7 RID: 2775
		private readonly List<CraftingTemplate> _templatesList;

		// Token: 0x04000AD8 RID: 2776
		private readonly Func<CraftingTemplate, int> _getUnlockedPiecesCount;

		// Token: 0x04000AD9 RID: 2777
		private readonly Func<CraftingTemplate, int> _getUninspectedPiecesCount;

		// Token: 0x04000ADA RID: 2778
		private string _popupHeader;

		// Token: 0x04000ADB RID: 2779
		private bool _isVisible;

		// Token: 0x04000ADC RID: 2780
		private MBBindingList<WeaponClassVM> _weaponClasses;
	}
}
