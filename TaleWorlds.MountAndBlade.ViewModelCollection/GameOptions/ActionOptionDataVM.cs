using System;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x02000069 RID: 105
	public class ActionOptionDataVM : GenericOptionDataVM
	{
		// Token: 0x0600082A RID: 2090 RVA: 0x0001C1E0 File Offset: 0x0001A3E0
		public ActionOptionDataVM(Action onAction, OptionsVM optionsVM, IOptionData option, TextObject name, TextObject optionActionName, TextObject description)
			: base(optionsVM, option, name, description, OptionsVM.OptionsDataType.ActionOption)
		{
			this._onAction = onAction;
			this._optionActionName = optionActionName;
			this.RefreshValues();
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0001C204 File Offset: 0x0001A404
		public override void RefreshValues()
		{
			base.RefreshValues();
			if (this._optionActionName != null)
			{
				this.ActionName = this._optionActionName.ToString();
			}
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0001C22B File Offset: 0x0001A42B
		private void ExecuteAction()
		{
			Action onAction = this._onAction;
			if (onAction == null)
			{
				return;
			}
			onAction.DynamicInvokeWithLog(Array.Empty<object>());
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0001C243 File Offset: 0x0001A443
		public override void Cancel()
		{
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0001C245 File Offset: 0x0001A445
		public override bool IsChanged()
		{
			return false;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0001C248 File Offset: 0x0001A448
		public override void ResetData()
		{
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0001C24A File Offset: 0x0001A44A
		public override void SetValue(float value)
		{
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0001C24C File Offset: 0x0001A44C
		public override void UpdateValue()
		{
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0001C24E File Offset: 0x0001A44E
		public override void ApplyValue()
		{
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x0001C250 File Offset: 0x0001A450
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x0001C258 File Offset: 0x0001A458
		[DataSourceProperty]
		public string ActionName
		{
			get
			{
				return this._actionName;
			}
			set
			{
				if (value != this._actionName)
				{
					this._actionName = value;
					base.OnPropertyChangedWithValue<string>(value, "ActionName");
				}
			}
		}

		// Token: 0x040003B6 RID: 950
		private readonly Action _onAction;

		// Token: 0x040003B7 RID: 951
		private readonly TextObject _optionActionName;

		// Token: 0x040003B8 RID: 952
		private string _actionName;
	}
}
