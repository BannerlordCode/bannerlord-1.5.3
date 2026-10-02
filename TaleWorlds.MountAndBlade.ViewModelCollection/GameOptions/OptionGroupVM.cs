using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.GameOptions
{
	// Token: 0x02000072 RID: 114
	public class OptionGroupVM : ViewModel
	{
		// Token: 0x060008E6 RID: 2278 RVA: 0x0001D90C File Offset: 0x0001BB0C
		public OptionGroupVM(TextObject groupName, OptionsVM optionsBase, IEnumerable<IOptionData> optionsList)
		{
			this._groupName = groupName;
			this.Options = new MBBindingList<GenericOptionDataVM>();
			foreach (IOptionData optionData in optionsList)
			{
				this.Options.Add(optionsBase.GetOptionItem(optionData));
			}
			this.RefreshValues();
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0001D980 File Offset: 0x0001BB80
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._groupName.ToString();
			this.Options.ApplyActionOnAllItems(delegate(GenericOptionDataVM x)
			{
				x.RefreshValues();
			});
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0001D9D0 File Offset: 0x0001BBD0
		internal List<IOptionData> GetManagedOptions()
		{
			List<IOptionData> list = new List<IOptionData>();
			foreach (GenericOptionDataVM genericOptionDataVM in this.Options)
			{
				if (!genericOptionDataVM.IsNative)
				{
					list.Add(genericOptionDataVM.GetOptionData());
				}
			}
			return list;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0001DA34 File Offset: 0x0001BC34
		internal bool IsChanged()
		{
			return this.Options.Any<GenericOptionDataVM>((GenericOptionDataVM o) => o.IsChanged());
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0001DA60 File Offset: 0x0001BC60
		internal void Cancel()
		{
			this.Options.ApplyActionOnAllItems(delegate(GenericOptionDataVM o)
			{
				o.Cancel();
			});
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0001DA8C File Offset: 0x0001BC8C
		internal void InitializeDependentConfigs(Action<IOptionData, float> updateDependentConfigs)
		{
			this.Options.ApplyActionOnAllItems(delegate(GenericOptionDataVM o)
			{
				updateDependentConfigs(o.GetOptionData(), o.GetOptionData().GetValue(false));
			});
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x0001DABD File Offset: 0x0001BCBD
		// (set) Token: 0x060008ED RID: 2285 RVA: 0x0001DAC5 File Offset: 0x0001BCC5
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

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x0001DAE8 File Offset: 0x0001BCE8
		// (set) Token: 0x060008EF RID: 2287 RVA: 0x0001DAF0 File Offset: 0x0001BCF0
		[DataSourceProperty]
		public MBBindingList<GenericOptionDataVM> Options
		{
			get
			{
				return this._options;
			}
			set
			{
				if (value != this._options)
				{
					this._options = value;
					base.OnPropertyChangedWithValue<MBBindingList<GenericOptionDataVM>>(value, "Options");
				}
			}
		}

		// Token: 0x04000400 RID: 1024
		private readonly TextObject _groupName;

		// Token: 0x04000401 RID: 1025
		private const string ControllerIdentificationModifier = "_controller";

		// Token: 0x04000402 RID: 1026
		private string _name;

		// Token: 0x04000403 RID: 1027
		private MBBindingList<GenericOptionDataVM> _options;
	}
}
