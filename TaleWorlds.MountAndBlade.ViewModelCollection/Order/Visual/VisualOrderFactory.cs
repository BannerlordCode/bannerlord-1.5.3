using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x0200002A RID: 42
	public static class VisualOrderFactory
	{
		// Token: 0x06000344 RID: 836 RVA: 0x0000BF94 File Offset: 0x0000A194
		public static void RegisterProvider(VisualOrderProvider provider)
		{
			if (VisualOrderFactory._providers.Contains(provider) || VisualOrderFactory._providers.Any<VisualOrderProvider>((VisualOrderProvider p) => p.GetType() == provider.GetType()))
			{
				Debug.FailedAssert("Provider of type already registered: " + provider.GetType().Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Order\\Visual\\VisualOrderFactory.cs", "RegisterProvider", 22);
				return;
			}
			VisualOrderFactory._providers.Add(provider);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000C014 File Offset: 0x0000A214
		public static void UnregisterProvider(VisualOrderProvider provider)
		{
			if (!VisualOrderFactory._providers.Contains(provider))
			{
				Debug.FailedAssert("Provider of type was not registered: " + provider.GetType().Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Order\\Visual\\VisualOrderFactory.cs", "UnregisterProvider", 33);
				return;
			}
			VisualOrderFactory._providers.Remove(provider);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000C064 File Offset: 0x0000A264
		public static MBReadOnlyList<VisualOrderSet> GetOrders()
		{
			for (int i = VisualOrderFactory._providers.Count - 1; i >= 0; i--)
			{
				VisualOrderProvider visualOrderProvider = VisualOrderFactory._providers[i];
				if (visualOrderProvider != null && visualOrderProvider.IsAvailable())
				{
					return visualOrderProvider.GetOrders();
				}
			}
			MBList<Formation> formationsIncludingEmpty = Mission.Current.PlayerTeam.FormationsIncludingEmpty;
			for (int j = 0; j < formationsIncludingEmpty.Count; j++)
			{
				if (formationsIncludingEmpty[j].CountOfUnits > 0)
				{
					Debug.FailedAssert("There are troops in formations but none of the order providers are available!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Order\\Visual\\VisualOrderFactory.cs", "GetOrders", 56);
					break;
				}
			}
			return new MBList<VisualOrderSet>();
		}

		// Token: 0x04000176 RID: 374
		private static List<VisualOrderProvider> _providers = new List<VisualOrderProvider>();
	}
}
