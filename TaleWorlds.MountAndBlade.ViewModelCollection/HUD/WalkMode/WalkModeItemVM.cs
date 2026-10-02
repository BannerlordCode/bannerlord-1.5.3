using System;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.HUD.WalkMode
{
	// Token: 0x0200005B RID: 91
	public class WalkModeItemVM : ViewModel
	{
		// Token: 0x0600074E RID: 1870 RVA: 0x0001A214 File Offset: 0x00018414
		public WalkModeItemVM(string typeId, TextObject description, MissionMainAgentWalkModeControllerVM.GetIsWalkModeActivatedDelegate getIsActive, MissionMainAgentWalkModeControllerVM.SetIsWalkModeActivatedDelegate setIsActive, MissionMainAgentWalkModeControllerVM.GetCanChangeWalkModeActivatedDelegate canChangeActive, Action<WalkModeItemVM> onToggle)
		{
			this._getIsActive = getIsActive;
			this._setIsActive = setIsActive;
			this._canChangeActive = canChangeActive;
			this._descriptionTextObj = description;
			this._onToggle = onToggle;
			this.IsActive = this._getIsActive();
			this.TypeId = typeId;
			this.RefreshValues();
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0001A26B File Offset: 0x0001846B
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Description = this._descriptionTextObj.ToString();
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0001A284 File Offset: 0x00018484
		public override void OnFinalize()
		{
			base.OnFinalize();
			InputKeyItemVM toggleInputKey = this.ToggleInputKey;
			if (toggleInputKey == null)
			{
				return;
			}
			toggleInputKey.OnFinalize();
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0001A29C File Offset: 0x0001849C
		public void OnEnabled()
		{
			this.IsActive = this._getIsActive();
			this.IsDisabled = !this._canChangeActive();
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001A2C4 File Offset: 0x000184C4
		public void ToggleState()
		{
			this.IsDisabled = !this._canChangeActive();
			if (!this.IsDisabled)
			{
				this.IsActive = !this.IsActive;
				this._setIsActive(this.IsActive);
				this._onToggle(this);
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x0001A319 File Offset: 0x00018519
		// (set) Token: 0x06000754 RID: 1876 RVA: 0x0001A321 File Offset: 0x00018521
		[DataSourceProperty]
		public InputKeyItemVM ToggleInputKey
		{
			get
			{
				return this._toggleInputKey;
			}
			set
			{
				if (value != this._toggleInputKey)
				{
					this._toggleInputKey = value;
					base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "ToggleInputKey");
				}
			}
		}

		// Token: 0x06000755 RID: 1877 RVA: 0x0001A33F File Offset: 0x0001853F
		public void SetToggleInputKey(HotKey hotKey, bool isHotKeyConsoleOnly)
		{
			InputKeyItemVM toggleInputKey = this.ToggleInputKey;
			if (toggleInputKey != null)
			{
				toggleInputKey.OnFinalize();
			}
			this.ToggleInputKey = InputKeyItemVM.CreateFromHotKey(hotKey, isHotKeyConsoleOnly);
		}

		// Token: 0x06000756 RID: 1878 RVA: 0x0001A35F File Offset: 0x0001855F
		public void SetToggleInputKey(GameKey gameKey, bool isHotKeyConsoleOnly)
		{
			InputKeyItemVM toggleInputKey = this.ToggleInputKey;
			if (toggleInputKey != null)
			{
				toggleInputKey.OnFinalize();
			}
			this.ToggleInputKey = InputKeyItemVM.CreateFromGameKey(gameKey, isHotKeyConsoleOnly);
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x0001A37F File Offset: 0x0001857F
		// (set) Token: 0x06000758 RID: 1880 RVA: 0x0001A387 File Offset: 0x00018587
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
				}
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x0001A3A5 File Offset: 0x000185A5
		// (set) Token: 0x0600075A RID: 1882 RVA: 0x0001A3AD File Offset: 0x000185AD
		[DataSourceProperty]
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
			set
			{
				if (value != this._isDisabled)
				{
					this._isDisabled = value;
					base.OnPropertyChangedWithValue(value, "IsDisabled");
				}
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x0001A3CB File Offset: 0x000185CB
		// (set) Token: 0x0600075C RID: 1884 RVA: 0x0001A3D3 File Offset: 0x000185D3
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x0001A3F6 File Offset: 0x000185F6
		// (set) Token: 0x0600075E RID: 1886 RVA: 0x0001A3FE File Offset: 0x000185FE
		[DataSourceProperty]
		public string TypeId
		{
			get
			{
				return this._typeId;
			}
			set
			{
				if (value != this._typeId)
				{
					this._typeId = value;
					base.OnPropertyChangedWithValue<string>(value, "TypeId");
				}
			}
		}

		// Token: 0x0400033F RID: 831
		private readonly MissionMainAgentWalkModeControllerVM.GetIsWalkModeActivatedDelegate _getIsActive;

		// Token: 0x04000340 RID: 832
		private readonly MissionMainAgentWalkModeControllerVM.SetIsWalkModeActivatedDelegate _setIsActive;

		// Token: 0x04000341 RID: 833
		private readonly MissionMainAgentWalkModeControllerVM.GetCanChangeWalkModeActivatedDelegate _canChangeActive;

		// Token: 0x04000342 RID: 834
		private readonly Action<WalkModeItemVM> _onToggle;

		// Token: 0x04000343 RID: 835
		private readonly TextObject _descriptionTextObj;

		// Token: 0x04000344 RID: 836
		private InputKeyItemVM _toggleInputKey;

		// Token: 0x04000345 RID: 837
		private bool _isActive;

		// Token: 0x04000346 RID: 838
		private bool _isDisabled;

		// Token: 0x04000347 RID: 839
		private string _description;

		// Token: 0x04000348 RID: 840
		private string _typeId;
	}
}
