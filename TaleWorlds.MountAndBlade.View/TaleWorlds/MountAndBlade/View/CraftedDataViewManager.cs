using System;
using System.Collections.Generic;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000014 RID: 20
	public class CraftedDataViewManager
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00004870 File Offset: 0x00002A70
		public static void Initialize()
		{
			CraftedDataViewManager._craftedDataViews = new Dictionary<WeaponDesign, CraftedDataView>();
		}

		// Token: 0x0600008E RID: 142 RVA: 0x0000487C File Offset: 0x00002A7C
		public static void Clear()
		{
			foreach (CraftedDataView craftedDataView in CraftedDataViewManager._craftedDataViews.Values)
			{
				craftedDataView.Clear();
			}
			CraftedDataViewManager._craftedDataViews.Clear();
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000048DC File Offset: 0x00002ADC
		public static CraftedDataView GetCraftedDataView(WeaponDesign craftedData)
		{
			if (craftedData != null)
			{
				CraftedDataView craftedDataView;
				if (!CraftedDataViewManager._craftedDataViews.TryGetValue(craftedData, out craftedDataView))
				{
					craftedDataView = new CraftedDataView(craftedData);
					CraftedDataViewManager._craftedDataViews.Add(craftedData, craftedDataView);
				}
				return craftedDataView;
			}
			return null;
		}

		// Token: 0x0400001A RID: 26
		private static Dictionary<WeaponDesign, CraftedDataView> _craftedDataViews;
	}
}
