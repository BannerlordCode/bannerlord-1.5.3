using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.WalkMode
{
	// Token: 0x0200005A RID: 90
	public class MissionMainAgentWalkModeControllerVM : ViewModel
	{
		// Token: 0x06000742 RID: 1858 RVA: 0x0001A089 File Offset: 0x00018289
		public MissionMainAgentWalkModeControllerVM()
		{
			this.ControlModes = new MBBindingList<WalkModeItemVM>();
			this.RefreshValues();
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001A0A2 File Offset: 0x000182A2
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.ControlModes.ApplyActionOnAllItems(delegate(WalkModeItemVM o)
			{
				o.OnFinalize();
			});
			this.ControlModes.Clear();
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0001A0E0 File Offset: 0x000182E0
		public void AddWalkMode(string typeId, TextObject name, MissionMainAgentWalkModeControllerVM.GetIsWalkModeActivatedDelegate getIsActive, MissionMainAgentWalkModeControllerVM.SetIsWalkModeActivatedDelegate setIsActive, MissionMainAgentWalkModeControllerVM.GetCanChangeWalkModeActivatedDelegate canChangeActive, HotKey hotKey, bool isHotkeyConsoleOnly)
		{
			WalkModeItemVM walkModeItemVM = new WalkModeItemVM(typeId, name, getIsActive, setIsActive, canChangeActive, new Action<WalkModeItemVM>(this.OnItemToggled));
			walkModeItemVM.SetToggleInputKey(hotKey, isHotkeyConsoleOnly);
			this.ControlModes.Add(walkModeItemVM);
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0001A11C File Offset: 0x0001831C
		public void AddWalkMode(string typeId, TextObject name, MissionMainAgentWalkModeControllerVM.GetIsWalkModeActivatedDelegate getIsActive, MissionMainAgentWalkModeControllerVM.SetIsWalkModeActivatedDelegate setIsActive, MissionMainAgentWalkModeControllerVM.GetCanChangeWalkModeActivatedDelegate canChangeActive, GameKey hotKey, bool isHotkeyConsoleOnly)
		{
			WalkModeItemVM walkModeItemVM = new WalkModeItemVM(typeId, name, getIsActive, setIsActive, canChangeActive, new Action<WalkModeItemVM>(this.OnItemToggled));
			walkModeItemVM.SetToggleInputKey(hotKey, isHotkeyConsoleOnly);
			this.ControlModes.Add(walkModeItemVM);
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0001A158 File Offset: 0x00018358
		private void OnItemToggled(WalkModeItemVM item)
		{
			this.LastUsedItem = item;
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0001A164 File Offset: 0x00018364
		public void SetEnabled(bool isEnabled)
		{
			this.IsEnabled = isEnabled;
			if (isEnabled)
			{
				for (int i = 0; i < this.ControlModes.Count; i++)
				{
					this.ControlModes[i].OnEnabled();
				}
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x0001A1A2 File Offset: 0x000183A2
		// (set) Token: 0x06000749 RID: 1865 RVA: 0x0001A1AA File Offset: 0x000183AA
		[DataSourceProperty]
		public MBBindingList<WalkModeItemVM> ControlModes
		{
			get
			{
				return this._controlModes;
			}
			set
			{
				if (value != this._controlModes)
				{
					this._controlModes = value;
					base.OnPropertyChangedWithValue<MBBindingList<WalkModeItemVM>>(value, "ControlModes");
				}
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x0001A1C8 File Offset: 0x000183C8
		// (set) Token: 0x0600074B RID: 1867 RVA: 0x0001A1D0 File Offset: 0x000183D0
		[DataSourceProperty]
		public WalkModeItemVM LastUsedItem
		{
			get
			{
				return this._lastUsedItem;
			}
			set
			{
				if (value != this._lastUsedItem)
				{
					this._lastUsedItem = value;
					base.OnPropertyChangedWithValue<WalkModeItemVM>(value, "LastUsedItem");
				}
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x0001A1EE File Offset: 0x000183EE
		// (set) Token: 0x0600074D RID: 1869 RVA: 0x0001A1F6 File Offset: 0x000183F6
		[DataSourceProperty]
		public bool IsEnabled
		{
			get
			{
				return this._isEnabled;
			}
			set
			{
				if (value != this._isEnabled)
				{
					this._isEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsEnabled");
				}
			}
		}

		// Token: 0x0400033C RID: 828
		private MBBindingList<WalkModeItemVM> _controlModes;

		// Token: 0x0400033D RID: 829
		private WalkModeItemVM _lastUsedItem;

		// Token: 0x0400033E RID: 830
		private bool _isEnabled;

		// Token: 0x020000EC RID: 236
		// (Invoke) Token: 0x06000D0B RID: 3339
		public delegate bool GetIsWalkModeActivatedDelegate();

		// Token: 0x020000ED RID: 237
		// (Invoke) Token: 0x06000D0F RID: 3343
		public delegate void SetIsWalkModeActivatedDelegate(bool value);

		// Token: 0x020000EE RID: 238
		// (Invoke) Token: 0x06000D13 RID: 3347
		public delegate bool GetCanChangeWalkModeActivatedDelegate();
	}
}
