using System;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000024 RID: 36
	public class MultiplayerStarter
	{
		// Token: 0x060001BF RID: 447 RVA: 0x00007FB6 File Offset: 0x000061B6
		public MultiplayerStarter(MBObjectManager objectManager)
		{
			this._objectManager = objectManager;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00007FC5 File Offset: 0x000061C5
		public void LoadXMLFromFile(string xmlPath, string xsdPath)
		{
			this._objectManager.LoadOneXmlFromFile(xmlPath, xsdPath, false);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00007FD5 File Offset: 0x000061D5
		public void ClearEmptyObjects()
		{
			this._objectManager.UnregisterNonReadyObjects();
		}

		// Token: 0x04000069 RID: 105
		private readonly MBObjectManager _objectManager;
	}
}
