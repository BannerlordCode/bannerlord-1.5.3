using System;
using System.Threading;
using System.Xml;
using Steamworks;

namespace TaleWorlds.MountAndBlade.SteamWorkshop
{
	// Token: 0x02000002 RID: 2
	public class CreateItemTask : ToolTask
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public override void LoadFrom(XmlNode xmlNode)
		{
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000204C File Offset: 0x0000024C
		public override void DoJob()
		{
			SteamAPICall_t steamAPICall_t = SteamUGC.CreateItem(new AppId_t(261550U), EWorkshopFileType.k_EWorkshopFileTypeFirst);
			new CallResult<CreateItemResult_t>(null).Set(steamAPICall_t, new CallResult<CreateItemResult_t>.APIDispatchDelegate(this.OnCreateItemResult));
			while (!CreateItemTask._createItemResult)
			{
				SteamAPI.RunCallbacks();
				Thread.Sleep(100);
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002098 File Offset: 0x00000298
		private void OnCreateItemResult(CreateItemResult_t result, bool check)
		{
			bool flag = false;
			if (result.m_eResult == EResult.k_EResultOK)
			{
				Program.ItemId = result.m_nPublishedFileId;
				Program.Log("Item created. Item ID is " + Program.ItemId);
				flag = true;
			}
			else if (result.m_eResult == EResult.k_EResultFail)
			{
				Program.Log("Creating item failed");
			}
			else if (result.m_eResult == EResult.k_EResultTimeout)
			{
				Program.Log("Creating item timeout");
			}
			else
			{
				Program.Log("Creating item failed with result: " + result.m_eResult);
			}
			CreateItemTask._createItemResult = true;
			if (!flag)
			{
				Program.ExitProgram(-20);
			}
		}

		// Token: 0x04000001 RID: 1
		private static bool _createItemResult;
	}
}
