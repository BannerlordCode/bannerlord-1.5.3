using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Generic
{
	// Token: 0x02000027 RID: 39
	public class StringItemWithActionVM : ViewModel
	{
		// Token: 0x060001BA RID: 442 RVA: 0x00005C36 File Offset: 0x00003E36
		public StringItemWithActionVM(Action<object> onExecute, string item, object identifier)
		{
			this._onExecute = onExecute;
			this.Identifier = identifier;
			this.ActionText = item;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00005C53 File Offset: 0x00003E53
		public void ExecuteAction()
		{
			this._onExecute(this.Identifier);
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00005C66 File Offset: 0x00003E66
		// (set) Token: 0x060001BD RID: 445 RVA: 0x00005C6E File Offset: 0x00003E6E
		[DataSourceProperty]
		public string ActionText
		{
			get
			{
				return this._actionText;
			}
			set
			{
				if (value != this._actionText)
				{
					this._actionText = value;
					base.OnPropertyChangedWithValue<string>(value, "ActionText");
				}
			}
		}

		// Token: 0x040000B2 RID: 178
		public object Identifier;

		// Token: 0x040000B3 RID: 179
		protected Action<object> _onExecute;

		// Token: 0x040000B4 RID: 180
		private string _actionText;
	}
}
