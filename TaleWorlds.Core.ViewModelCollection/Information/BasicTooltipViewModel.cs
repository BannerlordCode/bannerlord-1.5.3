using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.Information
{
	// Token: 0x02000012 RID: 18
	public class BasicTooltipViewModel : ViewModel
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x00003BAC File Offset: 0x00001DAC
		public BasicTooltipViewModel(Func<string> hintTextDelegate)
		{
			this._hintProperty = hintTextDelegate;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00003BBB File Offset: 0x00001DBB
		public BasicTooltipViewModel(Func<List<TooltipProperty>> tooltipPropertiesDelegate)
		{
			this._tooltipProperties = tooltipPropertiesDelegate;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00003BCA File Offset: 0x00001DCA
		public BasicTooltipViewModel(Action preBuiltTooltipCallback)
		{
			this._preBuiltTooltipCallback = preBuiltTooltipCallback;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00003BD9 File Offset: 0x00001DD9
		public BasicTooltipViewModel()
		{
			this._hintProperty = null;
			this._tooltipProperties = null;
			this._preBuiltTooltipCallback = null;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00003BF6 File Offset: 0x00001DF6
		public void SetToolipCallback(Func<List<TooltipProperty>> tooltipPropertiesDelegate)
		{
			this._tooltipProperties = tooltipPropertiesDelegate;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00003BFF File Offset: 0x00001DFF
		public void SetGenericTooltipCallback(Action preBuiltTooltipCallback)
		{
			this._preBuiltTooltipCallback = preBuiltTooltipCallback;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00003C08 File Offset: 0x00001E08
		public void SetHintCallback(Func<string> hintProperty)
		{
			this._hintProperty = hintProperty;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00003C14 File Offset: 0x00001E14
		public void ExecuteBeginHint()
		{
			if (this._hintProperty == null && this._tooltipProperties == null && this._preBuiltTooltipCallback == null)
			{
				return;
			}
			if (this._hintProperty != null)
			{
				Func<List<TooltipProperty>> tooltipProperties = this._tooltipProperties;
			}
			if (this._tooltipProperties != null)
			{
				InformationManager.ShowTooltip(typeof(List<TooltipProperty>), new object[] { this._tooltipProperties() });
				return;
			}
			if (this._hintProperty != null)
			{
				string text = this._hintProperty();
				if (!string.IsNullOrEmpty(text))
				{
					MBInformationManager.ShowHint(text);
					return;
				}
			}
			else if (this._preBuiltTooltipCallback != null)
			{
				this._preBuiltTooltipCallback();
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00003CAB File Offset: 0x00001EAB
		public void ExecuteEndHint()
		{
			MBInformationManager.HideInformations();
		}

		// Token: 0x04000060 RID: 96
		private Func<string> _hintProperty;

		// Token: 0x04000061 RID: 97
		private Func<List<TooltipProperty>> _tooltipProperties;

		// Token: 0x04000062 RID: 98
		private Action _preBuiltTooltipCallback;
	}
}
