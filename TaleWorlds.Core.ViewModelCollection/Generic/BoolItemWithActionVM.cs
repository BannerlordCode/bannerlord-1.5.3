using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Generic
{
	// Token: 0x02000026 RID: 38
	public class BoolItemWithActionVM : ViewModel
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x00005BE0 File Offset: 0x00003DE0
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x00005BE8 File Offset: 0x00003DE8
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

		// Token: 0x060001B8 RID: 440 RVA: 0x00005C06 File Offset: 0x00003E06
		public void ExecuteAction()
		{
			this._onExecute(this.Identifier);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00005C19 File Offset: 0x00003E19
		public BoolItemWithActionVM(Action<object> onExecute, bool isActive, object identifier)
		{
			this._onExecute = onExecute;
			this.Identifier = identifier;
			this.IsActive = isActive;
		}

		// Token: 0x040000AF RID: 175
		public object Identifier;

		// Token: 0x040000B0 RID: 176
		protected Action<object> _onExecute;

		// Token: 0x040000B1 RID: 177
		private bool _isActive;
	}
}
