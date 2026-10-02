using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby
{
	// Token: 0x02000029 RID: 41
	public class MPLobbyBlockerStateVM : ViewModel
	{
		// Token: 0x06000302 RID: 770 RVA: 0x0000BE24 File Offset: 0x0000A024
		public MPLobbyBlockerStateVM(Action<bool> setNavigationRestriction)
		{
			this._setNavigationRestriction = setNavigationRestriction;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000BE33 File Offset: 0x0000A033
		public void OnLobbyStateIsBlocker(TextObject description)
		{
			this._descriptionObj = description;
			this.IsEnabled = true;
			this.Description = this._descriptionObj.ToString();
		}

		// Token: 0x06000304 RID: 772 RVA: 0x0000BE54 File Offset: 0x0000A054
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject descriptionObj = this._descriptionObj;
			this.Description = ((descriptionObj != null) ? descriptionObj.ToString() : null);
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000BE74 File Offset: 0x0000A074
		public void OnLobbyStateNotBlocker()
		{
			this.IsEnabled = false;
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000BE7D File Offset: 0x0000A07D
		// (set) Token: 0x06000307 RID: 775 RVA: 0x0000BE85 File Offset: 0x0000A085
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
					this._setNavigationRestriction(value);
				}
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000BEAF File Offset: 0x0000A0AF
		// (set) Token: 0x06000309 RID: 777 RVA: 0x0000BEB7 File Offset: 0x0000A0B7
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

		// Token: 0x0400018F RID: 399
		private Action<bool> _setNavigationRestriction;

		// Token: 0x04000190 RID: 400
		private TextObject _descriptionObj;

		// Token: 0x04000191 RID: 401
		private bool _isEnabled;

		// Token: 0x04000192 RID: 402
		private string _description;
	}
}
