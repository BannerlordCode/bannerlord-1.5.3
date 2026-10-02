using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Information
{
	// Token: 0x02000016 RID: 22
	public class HintVM : TooltipBaseVM
	{
		// Token: 0x0600011B RID: 283 RVA: 0x0000451D File Offset: 0x0000271D
		public HintVM(Type type, object[] args)
			: base(type, args)
		{
			base.InvokeRefreshData<HintVM>(this);
			base.IsActive = true;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00004540 File Offset: 0x00002740
		protected override void OnFinalizeInternal()
		{
			base.IsActive = false;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000454C File Offset: 0x0000274C
		public static void RefreshGenericHintTooltip(HintVM hint, object[] args)
		{
			string text = args[0] as string;
			hint.Text = text;
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00004569 File Offset: 0x00002769
		// (set) Token: 0x0600011F RID: 287 RVA: 0x00004571 File Offset: 0x00002771
		[DataSourceProperty]
		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				if (this._text != value)
				{
					this._text = value;
					base.OnPropertyChangedWithValue<string>(value, "Text");
				}
			}
		}

		// Token: 0x04000079 RID: 121
		private string _text = "";
	}
}
