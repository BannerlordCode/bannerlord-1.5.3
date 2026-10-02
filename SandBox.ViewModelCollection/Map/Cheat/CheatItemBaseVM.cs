using System;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Map.Cheat
{
	// Token: 0x02000050 RID: 80
	public abstract class CheatItemBaseVM : ViewModel
	{
		// Token: 0x060004FB RID: 1275 RVA: 0x00013497 File Offset: 0x00011697
		public CheatItemBaseVM()
		{
		}

		// Token: 0x060004FC RID: 1276
		public abstract void ExecuteAction();

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x0001349F File Offset: 0x0001169F
		// (set) Token: 0x060004FE RID: 1278 RVA: 0x000134A7 File Offset: 0x000116A7
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x04000282 RID: 642
		private string _name;
	}
}
