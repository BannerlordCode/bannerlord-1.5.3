using System;

namespace TaleWorlds.Library.Information
{
	// Token: 0x020000AD RID: 173
	public class TooltipTriggerVM : ViewModel
	{
		// Token: 0x06000691 RID: 1681 RVA: 0x00016DB0 File Offset: 0x00014FB0
		public TooltipTriggerVM(Type linkedTooltipType, params object[] args)
		{
			this._linkedTooltipType = linkedTooltipType;
			this._args = args;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00016DC6 File Offset: 0x00014FC6
		public void ExecuteBeginHint()
		{
			InformationManager.ShowTooltip(this._linkedTooltipType, this._args);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00016DD9 File Offset: 0x00014FD9
		public void ExecuteEndHint()
		{
			InformationManager.HideTooltip();
		}

		// Token: 0x040001F9 RID: 505
		private Type _linkedTooltipType;

		// Token: 0x040001FA RID: 506
		private object[] _args;
	}
}
