using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Multiplayer.Admin;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.AdminPanel
{
	// Token: 0x020000B5 RID: 181
	public class MultiplayerAdminPanelStringOptionVM : MultiplayerAdminPanelOptionBaseVM
	{
		// Token: 0x0600113B RID: 4411 RVA: 0x000362CF File Offset: 0x000344CF
		public MultiplayerAdminPanelStringOptionVM(IAdminPanelOption<string> option)
			: base(option)
		{
			this._option = option;
			this.Text = this._option.GetValue();
			this.IsStringOption = true;
		}

		// Token: 0x0600113C RID: 4412 RVA: 0x000362F7 File Offset: 0x000344F7
		public override void UpdateValues()
		{
			base.UpdateValues();
			this.Text = this._option.GetValue();
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x0600113D RID: 4413 RVA: 0x00036310 File Offset: 0x00034510
		// (set) Token: 0x0600113E RID: 4414 RVA: 0x00036318 File Offset: 0x00034518
		[DataSourceProperty]
		public bool IsStringOption
		{
			get
			{
				return this._isStringOption;
			}
			set
			{
				if (value != this._isStringOption)
				{
					this._isStringOption = value;
					base.OnPropertyChangedWithValue(value, "IsStringOption");
				}
			}
		}

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x0600113F RID: 4415 RVA: 0x00036336 File Offset: 0x00034536
		// (set) Token: 0x06001140 RID: 4416 RVA: 0x0003633E File Offset: 0x0003453E
		[DataSourceProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (value != this._text)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
					IAdminPanelOption<string> option = this._option;
					if (option == null)
					{
						return;
					}
					option.SetValue(value);
				}
			}
		}

		// Token: 0x04000819 RID: 2073
		private new readonly IAdminPanelOption<string> _option;

		// Token: 0x0400081A RID: 2074
		private bool _isStringOption;

		// Token: 0x0400081B RID: 2075
		private string _text;
	}
}
