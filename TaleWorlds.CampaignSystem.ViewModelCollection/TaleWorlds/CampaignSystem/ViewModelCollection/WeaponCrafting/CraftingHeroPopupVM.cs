using System;
using TaleWorlds.CampaignSystem.ViewModelCollection.Input;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.WeaponCrafting
{
	// Token: 0x020000FF RID: 255
	public class CraftingHeroPopupVM : ViewModel
	{
		// Token: 0x060016BB RID: 5819 RVA: 0x00058C08 File Offset: 0x00056E08
		public CraftingHeroPopupVM(Func<MBBindingList<CraftingAvailableHeroItemVM>> getCraftingHeroes)
		{
			this.GetCraftingHeroes = getCraftingHeroes;
			this.SelectHeroText = new TextObject("{=xaeXEj8J}Select character for smithing", null).ToString();
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x00058C2D File Offset: 0x00056E2D
		public void ExecuteOpenPopup()
		{
			this.IsVisible = true;
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x00058C36 File Offset: 0x00056E36
		public void ExecuteClosePopup()
		{
			this.IsVisible = false;
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x00058C3F File Offset: 0x00056E3F
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM exitInputKey = this.ExitInputKey;
			if (exitInputKey == null)
			{
				return;
			}
			exitInputKey.OnFinalize();
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x060016BF RID: 5823 RVA: 0x00058C57 File Offset: 0x00056E57
		// (set) Token: 0x060016C0 RID: 5824 RVA: 0x00058C5F File Offset: 0x00056E5F
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
				}
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x060016C1 RID: 5825 RVA: 0x00058C7D File Offset: 0x00056E7D
		// (set) Token: 0x060016C2 RID: 5826 RVA: 0x00058C85 File Offset: 0x00056E85
		[DataSourceProperty]
		public string SelectHeroText
		{
			get
			{
				return this._selectHeroText;
			}
			set
			{
				if (value != this._selectHeroText)
				{
					this._selectHeroText = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectHeroText");
				}
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x060016C3 RID: 5827 RVA: 0x00058CA8 File Offset: 0x00056EA8
		[DataSourceProperty]
		public MBBindingList<CraftingAvailableHeroItemVM> CraftingHeroes
		{
			get
			{
				return this.GetCraftingHeroes();
			}
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x00058CB5 File Offset: 0x00056EB5
		public void SetExitInputKey(HotKey hotKey)
		{
			this.ExitInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, true);
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x060016C5 RID: 5829 RVA: 0x00058CC4 File Offset: 0x00056EC4
		// (set) Token: 0x060016C6 RID: 5830 RVA: 0x00058CCC File Offset: 0x00056ECC
		[DataSourceProperty]
		public InputKeyItemVM ExitInputKey
		{
			get
			{
				return this._exitInputKey;
			}
			set
			{
				if (value != this._exitInputKey)
				{
					this._exitInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ExitInputKey");
				}
			}
		}

		// Token: 0x04000A58 RID: 2648
		private readonly Func<MBBindingList<CraftingAvailableHeroItemVM>> GetCraftingHeroes;

		// Token: 0x04000A59 RID: 2649
		private bool _isVisible;

		// Token: 0x04000A5A RID: 2650
		private string _selectHeroText;

		// Token: 0x04000A5B RID: 2651
		private InputKeyItemVM _exitInputKey;
	}
}
