using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using Steamworks;
using TaleWorlds.ModuleManager;

namespace TaleWorlds.MountAndBlade.SteamWorkshop
{
	// Token: 0x02000007 RID: 7
	public class UpdateItemTask : ToolTask
	{
		// Token: 0x0600002F RID: 47 RVA: 0x000024AC File Offset: 0x000006AC
		public UpdateItemTask()
		{
			this._gotModuleFolder = false;
			this._moduleFolder = "";
			this._gotItemDescription = false;
			this._itemDescription = "A Bannerlord Mod";
			this._gotVisibility = false;
			this._visibility = ItemVisibility.Private;
			this._gotTags = false;
			this._tags = new List<string>();
			this._gotImage = false;
			this._image = "";
			this._changeNotes = "Minor changes.";
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002520 File Offset: 0x00000720
		public override void LoadFrom(XmlNode xmlNode)
		{
			foreach (object obj in xmlNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (!(xmlNode2 is XmlComment))
				{
					if (xmlNode2.Name == "ModuleFolder")
					{
						this._gotModuleFolder = true;
						this._moduleFolder = xmlNode2.Attributes["Value"].Value;
					}
					else if (xmlNode2.Name == "ItemDescription")
					{
						this._gotItemDescription = true;
						this._itemDescription = xmlNode2.Attributes["Value"].Value;
					}
					else
					{
						if (xmlNode2.Name == "Tags")
						{
							this._gotTags = true;
							using (IEnumerator enumerator2 = xmlNode2.ChildNodes.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									object obj2 = enumerator2.Current;
									XmlNode xmlNode3 = (XmlNode)obj2;
									if (!(xmlNode3 is XmlComment))
									{
										string value = xmlNode3.Attributes["Value"].Value;
										this._tags.Add(value);
									}
								}
								continue;
							}
						}
						if (xmlNode2.Name == "Image")
						{
							this._gotImage = true;
							this._image = xmlNode2.Attributes["Value"].Value;
						}
						else if (xmlNode2.Name == "ChangeNotes")
						{
							this._changeNotes = xmlNode2.Attributes["Value"].Value;
						}
						else if (xmlNode2.Name == "Visibility")
						{
							this._gotVisibility = true;
							string text = xmlNode2.Attributes["Value"].Value.ToLower();
							if (text == ItemVisibility.Private.ToString().ToLower())
							{
								this._visibility = ItemVisibility.Private;
							}
							else if (text == ItemVisibility.Public.ToString().ToLower())
							{
								this._visibility = ItemVisibility.Public;
							}
							else if (text == ItemVisibility.FriendsOnly.ToString().ToLower())
							{
								this._visibility = ItemVisibility.FriendsOnly;
							}
						}
					}
				}
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000027B4 File Offset: 0x000009B4
		public override void DoJob()
		{
			AppId_t appId_t = new AppId_t(261550U);
			ModuleInfo moduleInfo = null;
			if (this._gotModuleFolder)
			{
				moduleInfo = new ModuleInfo();
				moduleInfo.LoadWithFullPath(this._moduleFolder);
			}
			UGCUpdateHandle_t ugcupdateHandle_t = SteamUGC.StartItemUpdate(appId_t, Program.ItemId);
			if (this._gotModuleFolder)
			{
				SteamUGC.SetItemTitle(ugcupdateHandle_t, moduleInfo.Name);
			}
			if (this._gotItemDescription)
			{
				SteamUGC.SetItemDescription(ugcupdateHandle_t, this._itemDescription);
			}
			if (this._gotVisibility)
			{
				ERemoteStoragePublishedFileVisibility eremoteStoragePublishedFileVisibility;
				switch (this._visibility)
				{
				case ItemVisibility.Public:
					eremoteStoragePublishedFileVisibility = ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic;
					break;
				case ItemVisibility.FriendsOnly:
					eremoteStoragePublishedFileVisibility = ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityFriendsOnly;
					break;
				case ItemVisibility.Private:
					eremoteStoragePublishedFileVisibility = ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				SteamUGC.SetItemVisibility(ugcupdateHandle_t, eremoteStoragePublishedFileVisibility);
			}
			if (this._gotTags)
			{
				SteamUGC.SetItemTags(ugcupdateHandle_t, this._tags);
			}
			if (this._gotModuleFolder)
			{
				SteamUGC.SetItemContent(ugcupdateHandle_t, this._moduleFolder);
			}
			if (this._gotImage)
			{
				SteamUGC.SetItemPreview(ugcupdateHandle_t, this._image);
			}
			SteamAPICall_t steamAPICall_t = SteamUGC.SubmitItemUpdate(ugcupdateHandle_t, this._changeNotes);
			new CallResult<SubmitItemUpdateResult_t>(null).Set(steamAPICall_t, new CallResult<SubmitItemUpdateResult_t>.APIDispatchDelegate(this.OnSubmitItemUpdateResult));
			while (!this._submitItemUpdateResult)
			{
				ulong num;
				ulong num2;
				EItemUpdateStatus itemUpdateProgress = SteamUGC.GetItemUpdateProgress(ugcupdateHandle_t, out num, out num2);
				Program.Log(string.Concat(new object[] { "Status: ", itemUpdateProgress, " ", num, " / ", num2 }));
				SteamAPI.RunCallbacks();
				Thread.Sleep(100);
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002928 File Offset: 0x00000B28
		private void OnSubmitItemUpdateResult(SubmitItemUpdateResult_t result, bool check)
		{
			bool flag = false;
			if (result.m_eResult == EResult.k_EResultOK)
			{
				Program.Log("Uploading done!");
				flag = true;
			}
			else if (result.m_eResult == EResult.k_EResultFail)
			{
				Program.Log("Uploading item failed!");
			}
			else if (result.m_eResult == EResult.k_EResultTimeout)
			{
				Program.Log("Uploading item timeout!");
			}
			else if (result.m_eResult == EResult.k_EResultFileNotFound)
			{
				Program.Log("Uploading item failed. File not found!");
			}
			else
			{
				Program.Log("Uploading item failed with result: " + result.m_eResult);
			}
			this._submitItemUpdateResult = true;
			if (!flag)
			{
				Program.ExitProgram(-20);
			}
		}

		// Token: 0x04000007 RID: 7
		private bool _gotModuleFolder;

		// Token: 0x04000008 RID: 8
		private string _moduleFolder;

		// Token: 0x04000009 RID: 9
		private bool _gotItemDescription;

		// Token: 0x0400000A RID: 10
		private string _itemDescription;

		// Token: 0x0400000B RID: 11
		private bool _gotVisibility;

		// Token: 0x0400000C RID: 12
		private ItemVisibility _visibility;

		// Token: 0x0400000D RID: 13
		private bool _gotTags;

		// Token: 0x0400000E RID: 14
		private List<string> _tags;

		// Token: 0x0400000F RID: 15
		private bool _gotImage;

		// Token: 0x04000010 RID: 16
		private string _image;

		// Token: 0x04000011 RID: 17
		private string _changeNotes;

		// Token: 0x04000012 RID: 18
		private bool _submitItemUpdateResult;
	}
}
