using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.Core.ViewModelCollection.Information
{
	// Token: 0x02000015 RID: 21
	public class HintViewModel : ViewModel
	{
		// Token: 0x06000117 RID: 279 RVA: 0x000044CE File Offset: 0x000026CE
		public HintViewModel()
		{
			this.HintText = TextObject.GetEmpty();
		}

		// Token: 0x06000118 RID: 280 RVA: 0x000044E1 File Offset: 0x000026E1
		public HintViewModel(TextObject hintText, string uniqueName = null)
		{
			this.HintText = hintText;
			this._uniqueName = uniqueName;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x000044F7 File Offset: 0x000026F7
		public void ExecuteBeginHint()
		{
			if (!TextObject.IsNullOrEmpty(this.HintText))
			{
				MBInformationManager.ShowHint(this.HintText.ToString());
			}
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00004516 File Offset: 0x00002716
		public void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x04000077 RID: 119
		public TextObject HintText;

		// Token: 0x04000078 RID: 120
		private readonly string _uniqueName;
	}
}
