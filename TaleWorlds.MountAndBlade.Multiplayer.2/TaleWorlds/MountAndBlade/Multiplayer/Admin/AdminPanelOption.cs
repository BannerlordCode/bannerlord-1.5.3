using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin
{
	// Token: 0x02000077 RID: 119
	internal class AdminPanelOption<T> : IAdminPanelOptionInternal<T>, IAdminPanelOptionInternal, IAdminPanelOption<T>, IAdminPanelOption
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600038A RID: 906 RVA: 0x00010087 File Offset: 0x0000E287
		// (set) Token: 0x0600038B RID: 907 RVA: 0x0001008F File Offset: 0x0000E28F
		private protected T DefaultValue { protected get; private set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00010098 File Offset: 0x0000E298
		// (set) Token: 0x0600038D RID: 909 RVA: 0x000100A0 File Offset: 0x0000E2A0
		private protected T InitialValue { protected get; private set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600038E RID: 910 RVA: 0x000100A9 File Offset: 0x0000E2A9
		// (set) Token: 0x0600038F RID: 911 RVA: 0x000100B1 File Offset: 0x0000E2B1
		private protected T CurrentValue { protected get; private set; }

		// Token: 0x06000390 RID: 912 RVA: 0x000100BA File Offset: 0x0000E2BA
		public AdminPanelOption(string uniqueId)
		{
			this._uniqueId = uniqueId;
			this._onValueChangedAdditionalCallbacks = new List<Action>();
			this._optionType = MultiplayerOptions.OptionType.NumOfSlots;
			this._accessMode = MultiplayerOptions.MultiplayerOptionsAccessMode.NumAccessModes;
		}

		// Token: 0x06000391 RID: 913 RVA: 0x000100E3 File Offset: 0x0000E2E3
		protected virtual void OnValueChanged(T previousValue, T newValue)
		{
			this.OnRefresh();
		}

		// Token: 0x06000392 RID: 914 RVA: 0x000100EB File Offset: 0x0000E2EB
		protected virtual bool OnGetCanRevertToDefaultValue()
		{
			return true;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000100F0 File Offset: 0x0000E2F0
		protected virtual T GetOptionValue(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode accessMode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions)
		{
			switch (optionType.GetOptionProperty().OptionValueType)
			{
			case MultiplayerOptions.OptionValueType.Bool:
				return (T)((object)optionType.GetBoolValue(accessMode));
			case MultiplayerOptions.OptionValueType.Integer:
				return (T)((object)optionType.GetIntValue(accessMode));
			case MultiplayerOptions.OptionValueType.Enum:
				Debug.FailedAssert("Unsupported option value type", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Admin\\Internal\\AdminPanelOption.cs", "GetOptionValue", 63);
				break;
			case MultiplayerOptions.OptionValueType.String:
				return (T)((object)optionType.GetStrValue(accessMode));
			}
			return default(T);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00010174 File Offset: 0x0000E374
		protected virtual bool AreEqualValues(T first, T second)
		{
			return (first == null && second == null) || ((first == null || second != null) && (first != null || second == null) && first.Equals(second));
		}

		// Token: 0x06000395 RID: 917 RVA: 0x000101CA File Offset: 0x0000E3CA
		public void AddValueChangedCallback(Action callback)
		{
			this._onValueChangedAdditionalCallbacks.Add(callback);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000101D8 File Offset: 0x0000E3D8
		public void RemoveValueChangedCallback(Action callback)
		{
			this._onValueChangedAdditionalCallbacks.Remove(callback);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000101E7 File Offset: 0x0000E3E7
		public virtual void OnFinalize()
		{
			this._onValueChangedAdditionalCallbacks.Clear();
			this._onRefresh = null;
			this._onApplied = null;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00010202 File Offset: 0x0000E402
		protected virtual void OnRefresh()
		{
			Action onRefresh = this._onRefresh;
			if (onRefresh == null)
			{
				return;
			}
			onRefresh();
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00010214 File Offset: 0x0000E414
		public AdminPanelOption<T> BuildOptionType(MultiplayerOptions.OptionType optionType, MultiplayerOptions.MultiplayerOptionsAccessMode accessMode = MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions, bool buildDefaultValue = true, bool buildInitialValue = true)
		{
			this._optionType = optionType;
			this._accessMode = accessMode;
			if (buildDefaultValue)
			{
				T optionValue = this.GetOptionValue(optionType, MultiplayerOptions.MultiplayerOptionsAccessMode.DefaultMapOptions);
				this.BuildDefaultValue(optionValue);
			}
			if (buildInitialValue)
			{
				T optionValue2 = this.GetOptionValue(optionType, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
				this.BuildInitialValue(optionValue2);
			}
			return this;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00010259 File Offset: 0x0000E459
		public AdminPanelOption<T> BuildIsRequired(bool isRequired)
		{
			this._isRequired = isRequired;
			return this;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00010263 File Offset: 0x0000E463
		public AdminPanelOption<T> BuildRequiresRestart(bool requiresRestart)
		{
			this._requiresRestart = requiresRestart;
			return this;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x0001026D File Offset: 0x0000E46D
		public AdminPanelOption<T> BuildName(TextObject name)
		{
			this._nameTextObj = name;
			return this;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00010277 File Offset: 0x0000E477
		public AdminPanelOption<T> BuildDescription(TextObject description)
		{
			this._descriptionTextObj = description;
			return this;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00010281 File Offset: 0x0000E481
		public AdminPanelOption<T> BuildInitialValue(T value)
		{
			this.InitialValue = value;
			this.SetValue(this.InitialValue);
			return this;
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00010297 File Offset: 0x0000E497
		public AdminPanelOption<T> BuildDefaultValue(T value)
		{
			this.DefaultValue = value;
			this.SetValue(this.DefaultValue);
			return this;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x000102AD File Offset: 0x0000E4AD
		public AdminPanelOption<T> BuildOnAppliedCallback(Action<T> onApplied)
		{
			this._onApplied = onApplied;
			return this;
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060003A1 RID: 929 RVA: 0x000102B7 File Offset: 0x0000E4B7
		public string UniqueId
		{
			get
			{
				return this._uniqueId;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x000102BF File Offset: 0x0000E4BF
		public bool IsRequired
		{
			get
			{
				return this._isRequired;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x000102C7 File Offset: 0x0000E4C7
		public bool RequiresMissionRestart
		{
			get
			{
				return this._requiresRestart;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x000102CF File Offset: 0x0000E4CF
		public bool IsDirty
		{
			get
			{
				return !this.AreEqualValues(this.InitialValue, this.CurrentValue);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x000102E6 File Offset: 0x0000E4E6
		public bool CanRevertToDefaultValue
		{
			get
			{
				return !this.AreEqualValues(this.DefaultValue, this.CurrentValue) && this.OnGetCanRevertToDefaultValue();
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00010304 File Offset: 0x0000E504
		public string Name
		{
			get
			{
				TextObject nameTextObj = this._nameTextObj;
				return ((nameTextObj != null) ? nameTextObj.ToString() : null) ?? string.Empty;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00010321 File Offset: 0x0000E521
		public string Description
		{
			get
			{
				TextObject descriptionTextObj = this._descriptionTextObj;
				return ((descriptionTextObj != null) ? descriptionTextObj.ToString() : null) ?? string.Empty;
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x0001033E File Offset: 0x0000E53E
		public T GetValue()
		{
			return this.CurrentValue;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00010348 File Offset: 0x0000E548
		public void SetValue(T value)
		{
			T currentValue = this.CurrentValue;
			this.CurrentValue = value;
			if (!this.AreEqualValues(currentValue, this.CurrentValue))
			{
				this.OnValueChanged(currentValue, this.CurrentValue);
			}
			for (int i = 0; i < this._onValueChangedAdditionalCallbacks.Count; i++)
			{
				Action action = this._onValueChangedAdditionalCallbacks[i];
				if (action != null)
				{
					action();
				}
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x000103AC File Offset: 0x0000E5AC
		public virtual bool GetIsAvailable()
		{
			return true;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x000103AF File Offset: 0x0000E5AF
		public void OnApplyChanges()
		{
			this.InitialValue = this.CurrentValue;
			Action<T> onApplied = this._onApplied;
			if (onApplied == null)
			{
				return;
			}
			onApplied(this.CurrentValue);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x000103D3 File Offset: 0x0000E5D3
		public void RevertChanges()
		{
			this.SetValue(this.InitialValue);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x000103E1 File Offset: 0x0000E5E1
		public void RestoreDefaults()
		{
			this.SetValue(this.DefaultValue);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x000103EF File Offset: 0x0000E5EF
		public void SetOnRefreshCallback(Action callback)
		{
			this._onRefresh = callback;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x000103F8 File Offset: 0x0000E5F8
		public virtual bool GetIsDisabled(out string reason)
		{
			reason = string.Empty;
			return false;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00010402 File Offset: 0x0000E602
		public MultiplayerOptions.OptionType GetOptionType()
		{
			return this._optionType;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001040A File Offset: 0x0000E60A
		public MultiplayerOptions.MultiplayerOptionsAccessMode GetOptionAccessMode()
		{
			return this._accessMode;
		}

		// Token: 0x04000107 RID: 263
		private MultiplayerOptions.OptionType _optionType;

		// Token: 0x04000108 RID: 264
		private MultiplayerOptions.MultiplayerOptionsAccessMode _accessMode;

		// Token: 0x0400010C RID: 268
		private readonly string _uniqueId;

		// Token: 0x0400010D RID: 269
		private bool _isRequired;

		// Token: 0x0400010E RID: 270
		private bool _requiresRestart;

		// Token: 0x0400010F RID: 271
		private Action _onRefresh;

		// Token: 0x04000110 RID: 272
		private List<Action> _onValueChangedAdditionalCallbacks;

		// Token: 0x04000111 RID: 273
		private Action<T> _onApplied;

		// Token: 0x04000112 RID: 274
		private TextObject _nameTextObj;

		// Token: 0x04000113 RID: 275
		private TextObject _descriptionTextObj;
	}
}
