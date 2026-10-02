using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200031D RID: 797
	public class MPPerkSelectionManager
	{
		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06002DCE RID: 11726 RVA: 0x000B1F9A File Offset: 0x000B019A
		public static MPPerkSelectionManager Instance
		{
			get
			{
				MPPerkSelectionManager mpperkSelectionManager;
				if ((mpperkSelectionManager = MPPerkSelectionManager._instance) == null)
				{
					mpperkSelectionManager = (MPPerkSelectionManager._instance = new MPPerkSelectionManager());
				}
				return mpperkSelectionManager;
			}
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x000B1FB0 File Offset: 0x000B01B0
		public static void FreeInstance()
		{
			if (MPPerkSelectionManager._instance != null)
			{
				Dictionary<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>> selections = MPPerkSelectionManager._instance._selections;
				if (selections != null)
				{
					selections.Clear();
				}
				Dictionary<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>> pendingChanges = MPPerkSelectionManager._instance._pendingChanges;
				if (pendingChanges != null)
				{
					pendingChanges.Clear();
				}
				MPPerkSelectionManager._instance = null;
			}
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x000B1FEC File Offset: 0x000B01EC
		public void InitializeForUser(string username, PlayerId playerId)
		{
			if (this._playerIdOfSelectionsOwner != playerId)
			{
				Dictionary<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>> selections = this._selections;
				if (selections != null)
				{
					selections.Clear();
				}
				this._playerIdOfSelectionsOwner = playerId;
				this._xmlPath = new PlatformFilePath(EngineFilePaths.ConfigsPath, "MPDefaultPerks_" + playerId + ".xml");
				try
				{
					PlatformFilePath platformFilePath = new PlatformFilePath(EngineFilePaths.ConfigsPath, "MPDefaultPerks_" + username + ".xml");
					if (FileHelper.FileExists(platformFilePath))
					{
						FileHelper.CopyFile(platformFilePath, this._xmlPath);
						FileHelper.DeleteFile(platformFilePath);
					}
				}
				catch (Exception)
				{
				}
				Dictionary<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>> dictionary = this.LoadSelectionsForUserFromXML();
				this._selections = dictionary ?? new Dictionary<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>>();
			}
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x000B20AC File Offset: 0x000B02AC
		public void ResetPendingChanges()
		{
			Dictionary<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>> pendingChanges = this._pendingChanges;
			if (pendingChanges != null)
			{
				pendingChanges.Clear();
			}
			Action onAfterResetPendingChanges = this.OnAfterResetPendingChanges;
			if (onAfterResetPendingChanges == null)
			{
				return;
			}
			onAfterResetPendingChanges();
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x000B20D0 File Offset: 0x000B02D0
		public void TryToApplyAndSavePendingChanges()
		{
			if (this._pendingChanges != null)
			{
				foreach (KeyValuePair<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>> keyValuePair in this._pendingChanges)
				{
					if (this._selections.ContainsKey(keyValuePair.Key))
					{
						this._selections.Remove(keyValuePair.Key);
					}
					this._selections.Add(keyValuePair.Key, keyValuePair.Value);
				}
				this._pendingChanges.Clear();
				List<KeyValuePair<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>>> selections = new List<KeyValuePair<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>>>();
				foreach (KeyValuePair<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>> keyValuePair2 in this._selections)
				{
					selections.Add(new KeyValuePair<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>>(keyValuePair2.Key, keyValuePair2.Value));
				}
				((ITask)AsyncTask.CreateWithDelegate(new ManagedDelegate
				{
					Instance = delegate
					{
						MPPerkSelectionManager instance = MPPerkSelectionManager.Instance;
						lock (instance)
						{
							this.SaveAsXML(selections);
						}
					}
				}, true)).Invoke();
			}
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x000B2208 File Offset: 0x000B0408
		public List<MPPerkSelectionManager.MPPerkSelection> GetSelectionsForHeroClass(MultiplayerClassDivisions.MPHeroClass currentHeroClass)
		{
			List<MPPerkSelectionManager.MPPerkSelection> list = new List<MPPerkSelectionManager.MPPerkSelection>();
			if ((this._pendingChanges == null || !this._pendingChanges.TryGetValue(currentHeroClass, out list)) && this._selections != null)
			{
				this._selections.TryGetValue(currentHeroClass, out list);
			}
			return list;
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x000B2250 File Offset: 0x000B0450
		public void SetSelectionsForHeroClassTemporarily(MultiplayerClassDivisions.MPHeroClass currentHeroClass, List<MPPerkSelectionManager.MPPerkSelection> perkChoices)
		{
			if (this._pendingChanges == null)
			{
				this._pendingChanges = new Dictionary<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>>();
			}
			List<MPPerkSelectionManager.MPPerkSelection> list;
			if (!this._pendingChanges.TryGetValue(currentHeroClass, out list))
			{
				list = new List<MPPerkSelectionManager.MPPerkSelection>();
				this._pendingChanges.Add(currentHeroClass, list);
			}
			else
			{
				list.Clear();
			}
			int count = perkChoices.Count;
			for (int i = 0; i < count; i++)
			{
				list.Add(perkChoices[i]);
			}
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x000B22BC File Offset: 0x000B04BC
		private Dictionary<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>> LoadSelectionsForUserFromXML()
		{
			Dictionary<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>> dictionary = null;
			MPPerkSelectionManager instance = MPPerkSelectionManager.Instance;
			lock (instance)
			{
				bool flag2 = FileHelper.FileExists(this._xmlPath);
				if (flag2)
				{
					dictionary = new Dictionary<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>>();
					try
					{
						MBReadOnlyList<MultiplayerClassDivisions.MPHeroClass> mpheroClasses = MultiplayerClassDivisions.GetMPHeroClasses();
						int count = mpheroClasses.Count;
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(this._xmlPath);
						foreach (object obj in xmlDocument.DocumentElement.ChildNodes)
						{
							XmlNode xmlNode = (XmlNode)obj;
							XmlNode xmlNode2 = xmlNode.Attributes["id"];
							MultiplayerClassDivisions.MPHeroClass mpheroClass = null;
							string value = xmlNode2.Value;
							for (int i = 0; i < count; i++)
							{
								if (mpheroClasses[i].StringId == value)
								{
									mpheroClass = mpheroClasses[i];
									break;
								}
							}
							if (mpheroClass != null)
							{
								List<MPPerkSelectionManager.MPPerkSelection> list = new List<MPPerkSelectionManager.MPPerkSelection>(2);
								foreach (object obj2 in xmlNode.ChildNodes)
								{
									XmlNode xmlNode3 = (XmlNode)obj2;
									XmlAttribute xmlAttribute = xmlNode3.Attributes["index"];
									XmlAttribute xmlAttribute2 = xmlNode3.Attributes["listIndex"];
									if (xmlAttribute != null && xmlAttribute2 != null)
									{
										int num = Convert.ToInt32(xmlAttribute.Value);
										int num2 = Convert.ToInt32(xmlAttribute2.Value);
										list.Add(new MPPerkSelectionManager.MPPerkSelection(num, num2));
									}
									else
									{
										flag2 = false;
									}
								}
								dictionary.Add(mpheroClass, list);
							}
							else
							{
								flag2 = false;
							}
						}
					}
					catch
					{
						flag2 = false;
					}
				}
				if (!flag2)
				{
					dictionary = null;
				}
			}
			return dictionary;
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x000B24DC File Offset: 0x000B06DC
		private bool SaveAsXML(List<KeyValuePair<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>>> selections)
		{
			bool flag = true;
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.InsertBefore(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null), xmlDocument.DocumentElement);
				XmlElement xmlElement = xmlDocument.CreateElement("HeroClasses");
				xmlDocument.AppendChild(xmlElement);
				foreach (KeyValuePair<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>> keyValuePair in selections)
				{
					MultiplayerClassDivisions.MPHeroClass key = keyValuePair.Key;
					List<MPPerkSelectionManager.MPPerkSelection> value = keyValuePair.Value;
					XmlElement xmlElement2 = xmlDocument.CreateElement("HeroClass");
					xmlElement2.SetAttribute("id", key.StringId);
					xmlElement.AppendChild(xmlElement2);
					foreach (MPPerkSelectionManager.MPPerkSelection mpperkSelection in value)
					{
						XmlElement xmlElement3 = xmlDocument.CreateElement("PerkSelection");
						xmlElement3.SetAttribute("index", mpperkSelection.Index.ToString());
						xmlElement3.SetAttribute("listIndex", mpperkSelection.ListIndex.ToString());
						xmlElement2.AppendChild(xmlElement3);
					}
				}
				xmlDocument.Save(this._xmlPath);
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x04001214 RID: 4628
		private static MPPerkSelectionManager _instance;

		// Token: 0x04001215 RID: 4629
		public Action OnAfterResetPendingChanges;

		// Token: 0x04001216 RID: 4630
		private Dictionary<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>> _selections;

		// Token: 0x04001217 RID: 4631
		private Dictionary<MultiplayerClassDivisions.MPHeroClass, List<MPPerkSelectionManager.MPPerkSelection>> _pendingChanges;

		// Token: 0x04001218 RID: 4632
		private PlatformFilePath _xmlPath;

		// Token: 0x04001219 RID: 4633
		private PlayerId _playerIdOfSelectionsOwner;

		// Token: 0x0200060E RID: 1550
		public struct MPPerkSelection
		{
			// Token: 0x06004057 RID: 16471 RVA: 0x000FBE68 File Offset: 0x000FA068
			public MPPerkSelection(int index, int listIndex)
			{
				this.Index = index;
				this.ListIndex = listIndex;
			}

			// Token: 0x040020BA RID: 8378
			public readonly int Index;

			// Token: 0x040020BB RID: 8379
			public readonly int ListIndex;
		}
	}
}
