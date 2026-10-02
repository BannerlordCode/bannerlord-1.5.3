using System;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.Admin.Internal
{
	// Token: 0x02000079 RID: 121
	internal class AdminPanelMultiSelectionItem : IAdminPanelMultiSelectionItem
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060003BC RID: 956 RVA: 0x0001055F File Offset: 0x0000E75F
		public string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060003BD RID: 957 RVA: 0x00010567 File Offset: 0x0000E767
		public string DisplayName
		{
			get
			{
				string displayName = this._displayName;
				if (displayName == null)
				{
					return null;
				}
				return displayName.ToString();
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060003BE RID: 958 RVA: 0x0001057A File Offset: 0x0000E77A
		public bool IsFallbackValue
		{
			get
			{
				return this._isFallbackValue;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060003BF RID: 959 RVA: 0x00010582 File Offset: 0x0000E782
		public bool IsDisabled
		{
			get
			{
				return this._isDisabled;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x0001058A File Offset: 0x0000E78A
		public bool CanBeApplied
		{
			get
			{
				return this._canBeApplied;
			}
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00010592 File Offset: 0x0000E792
		public AdminPanelMultiSelectionItem(string value, TextObject displayName, bool isFallbackValue = false, bool isDisabled = false, bool canBeApplied = true)
		{
			this._value = value;
			this._displayName = ((displayName != null) ? displayName.ToString() : null);
			this._isFallbackValue = isFallbackValue;
			this._isDisabled = isDisabled;
			this._canBeApplied = canBeApplied;
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x000105CA File Offset: 0x0000E7CA
		public void SetIsFallbackValue(bool value)
		{
			this._isFallbackValue = value;
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x000105D3 File Offset: 0x0000E7D3
		public void SetIsDisabled(bool value)
		{
			this._isDisabled = value;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x000105DC File Offset: 0x0000E7DC
		public void SetCanBeApplied(bool value)
		{
			this._canBeApplied = value;
		}

		// Token: 0x0400011A RID: 282
		private string _value;

		// Token: 0x0400011B RID: 283
		private string _displayName;

		// Token: 0x0400011C RID: 284
		private bool _isFallbackValue;

		// Token: 0x0400011D RID: 285
		private bool _isDisabled;

		// Token: 0x0400011E RID: 286
		private bool _canBeApplied;
	}
}
