using System;
using System.Text;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.CampaignStartingOptions
{
	// Token: 0x02000061 RID: 97
	public class StartingOptionCategoryVM : ViewModel
	{
		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x00016098 File Offset: 0x00014298
		public string CategoryId { get; }

		// Token: 0x060005EB RID: 1515 RVA: 0x000160A0 File Offset: 0x000142A0
		public StartingOptionCategoryVM(string categoryId, TextObject name)
		{
			this.CategoryId = categoryId;
			this._nameText = name;
			this.Options = new MBBindingList<StartingOptionVM>();
			this.RefreshValues();
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x000160C8 File Offset: 0x000142C8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._nameText.ToString();
			this.Options.ApplyActionOnAllItems(delegate(StartingOptionVM x)
			{
				x.RefreshValues();
			});
			this.RefreshDescription();
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0001611C File Offset: 0x0001431C
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.Options.ApplyActionOnAllItems(delegate(StartingOptionVM x)
			{
				x.OnFinalize();
			});
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0001614E File Offset: 0x0001434E
		public void UpdateOptionStates()
		{
			this.Options.ApplyActionOnAllItems(delegate(StartingOptionVM x)
			{
				x.UpdateOptionState();
			});
			this.RefreshDescription();
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x00016180 File Offset: 0x00014380
		public void RefreshDescription()
		{
			this.DescriptionText = this.GetDescription();
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00016190 File Offset: 0x00014390
		public virtual string GetDescription()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.Options.Count; i++)
			{
				StartingOptionVM startingOptionVM = this.Options[i];
				if (StartingOptionCategoryVM.IsOptionRelevant(startingOptionVM))
				{
					int optionType = startingOptionVM.OptionType;
					if (optionType != 0)
					{
						if (optionType == 2)
						{
							StringBuilder stringBuilder2 = stringBuilder;
							TextObject composedDescription = startingOptionVM.GetComposedDescription();
							StartingOptionCategoryVM.AppendEntry(stringBuilder2, (composedDescription != null) ? composedDescription.ToString() : null, "\n");
						}
					}
					else if (startingOptionVM.ValueAsBoolean)
					{
						StartingOptionCategoryVM.AppendEntry(stringBuilder, startingOptionVM.Name, "\n");
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0001621C File Offset: 0x0001441C
		protected static bool IsOptionRelevant(StartingOptionVM option)
		{
			return option != null && !option.IsDisabled && !option.IsHidden;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x00016234 File Offset: 0x00014434
		protected static void AppendEntry(StringBuilder builder, string text, string separator)
		{
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			if (builder.Length > 0)
			{
				builder.Append(separator);
			}
			builder.Append(text);
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x00016258 File Offset: 0x00014458
		// (set) Token: 0x060005F4 RID: 1524 RVA: 0x00016260 File Offset: 0x00014460
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

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x00016283 File Offset: 0x00014483
		// (set) Token: 0x060005F6 RID: 1526 RVA: 0x0001628B File Offset: 0x0001448B
		[DataSourceProperty]
		public string DescriptionText
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				if (value != this._descriptionText)
				{
					this._descriptionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DescriptionText");
				}
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x000162AE File Offset: 0x000144AE
		// (set) Token: 0x060005F8 RID: 1528 RVA: 0x000162B6 File Offset: 0x000144B6
		[DataSourceProperty]
		public MBBindingList<StartingOptionVM> Options
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
					base.OnPropertyChangedWithValue<MBBindingList<StartingOptionVM>>(value, "Options");
				}
			}
		}

		// Token: 0x040002F4 RID: 756
		private readonly TextObject _nameText;

		// Token: 0x040002F5 RID: 757
		private string _name;

		// Token: 0x040002F6 RID: 758
		private string _descriptionText;

		// Token: 0x040002F7 RID: 759
		private MBBindingList<StartingOptionVM> _options;
	}
}
