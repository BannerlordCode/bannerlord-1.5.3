using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD
{
	// Token: 0x02000055 RID: 85
	public class ControllerEquippedItemVM : EquipmentActionItemVM
	{
		// Token: 0x060006EF RID: 1775 RVA: 0x00019184 File Offset: 0x00017384
		public ControllerEquippedItemVM(string item, string itemTypeAsString, object identifier, HotKey key, Action<EquipmentActionItemVM> onSelection)
			: base(item, itemTypeAsString, identifier, onSelection, false)
		{
			if (key != null)
			{
				this.ShortcutKey = InputKeyItemVM.CreateFromHotKey(key, true);
			}
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x000191A4 File Offset: 0x000173A4
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM shortcutKey = this.ShortcutKey;
			if (shortcutKey != null)
			{
				shortcutKey.OnFinalize();
			}
			this.ShortcutKey = null;
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x000191C4 File Offset: 0x000173C4
		// (set) Token: 0x060006F2 RID: 1778 RVA: 0x000191CC File Offset: 0x000173CC
		[DataSourceProperty]
		public InputKeyItemVM ShortcutKey
		{
			get
			{
				return this._shortcutKey;
			}
			set
			{
				if (value != this._shortcutKey)
				{
					this._shortcutKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ShortcutKey");
				}
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060006F3 RID: 1779 RVA: 0x000191EA File Offset: 0x000173EA
		// (set) Token: 0x060006F4 RID: 1780 RVA: 0x000191F2 File Offset: 0x000173F2
		[DataSourceProperty]
		public float DropProgress
		{
			get
			{
				return this._dropProgress;
			}
			set
			{
				if (value != this._dropProgress)
				{
					this._dropProgress = value;
					base.OnPropertyChangedWithValue(value, "DropProgress");
				}
			}
		}

		// Token: 0x04000316 RID: 790
		private InputKeyItemVM _shortcutKey;

		// Token: 0x04000317 RID: 791
		private float _dropProgress;
	}
}
