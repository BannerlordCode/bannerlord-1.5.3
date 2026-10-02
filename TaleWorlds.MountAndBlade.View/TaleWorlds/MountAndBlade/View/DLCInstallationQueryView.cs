using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000016 RID: 22
	public class DLCInstallationQueryView
	{
		// Token: 0x06000092 RID: 146 RVA: 0x00004C7A File Offset: 0x00002E7A
		public void Initialize()
		{
			EngineController.OnDLCInstalledCallback += this.OnModuleInstallComplete;
			EngineController.OnDLCLoadedCallback += this.OnModuleActivated;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004CA0 File Offset: 0x00002EA0
		private void OnModuleActivated()
		{
			MBInformationManager.AddQuickInformation(Module.CurrentModule.GlobalTextManager.FindText("str_content_activated_notification", null), 1000, null, null, "");
			InitialState initialState;
			if ((initialState = Module.CurrentModule.GlobalGameStateManager.ActiveState as InitialState) != null)
			{
				initialState.RefreshContentState();
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00004CF4 File Offset: 0x00002EF4
		private void OnModuleInstallComplete()
		{
			MBInformationManager.AddQuickInformation(Module.CurrentModule.GlobalTextManager.FindText("str_content_installed_notification", null), 1000, null, null, "");
			if (!(Module.CurrentModule.GlobalGameStateManager.ActiveState is InitialState))
			{
				this.CreateInstallationCompleteQuery();
			}
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004D48 File Offset: 0x00002F48
		private void CreateInstallationCompleteQuery()
		{
			string text;
			string text2;
			this.GetQueryTexts(out text, out text2);
			InformationManager.ShowInquiry(new InquiryData(text, text2, true, false, new TextObject("{=yS7PvrTD}OK", null).ToString(), null, null, null, "", 0f, null, null, null), false, false);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00004D8F File Offset: 0x00002F8F
		private void GetQueryTexts(out string title, out string description)
		{
			title = Module.CurrentModule.GlobalTextManager.FindText("str_dlc_installed_title", null).ToString();
			description = Module.CurrentModule.GlobalTextManager.FindText("str_dlc_installed_description", null).ToString();
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00004DC9 File Offset: 0x00002FC9
		public void OnFinalize()
		{
			EngineController.OnDLCInstalledCallback -= this.OnModuleInstallComplete;
			EngineController.OnDLCLoadedCallback -= this.OnModuleActivated;
		}
	}
}
