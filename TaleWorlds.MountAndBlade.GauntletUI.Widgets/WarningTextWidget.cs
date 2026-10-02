using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000047 RID: 71
	public class WarningTextWidget : TextWidget
	{
		// Token: 0x06000400 RID: 1024 RVA: 0x0000C9D5 File Offset: 0x0000ABD5
		public WarningTextWidget(UIContext context)
			: base(context)
		{
			base.UseGlobalTimeForAnimation = true;
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x0000C9E5 File Offset: 0x0000ABE5
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x0000C9ED File Offset: 0x0000ABED
		[Editor(false)]
		public bool IsWarned
		{
			get
			{
				return this._isWarned;
			}
			set
			{
				if (this._isWarned != value)
				{
					this._isWarned = value;
					base.OnPropertyChanged(value, "IsWarned");
					this.SetState(this._isWarned ? "Warned" : "Default");
				}
			}
		}

		// Token: 0x040001A6 RID: 422
		private bool _isWarned;
	}
}
