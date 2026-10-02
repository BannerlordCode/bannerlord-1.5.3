using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200021A RID: 538
	public struct FormationDeploymentOrder
	{
		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06001F75 RID: 8053 RVA: 0x0006D6F9 File Offset: 0x0006B8F9
		// (set) Token: 0x06001F76 RID: 8054 RVA: 0x0006D701 File Offset: 0x0006B901
		public int Key { get; private set; }

		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06001F77 RID: 8055 RVA: 0x0006D70A File Offset: 0x0006B90A
		// (set) Token: 0x06001F78 RID: 8056 RVA: 0x0006D712 File Offset: 0x0006B912
		public int Offset { get; private set; }

		// Token: 0x06001F79 RID: 8057 RVA: 0x0006D71C File Offset: 0x0006B91C
		private FormationDeploymentOrder(FormationClass formationClass, int offset = 0)
		{
			int formationClassPriority = FormationDeploymentOrder.GetFormationClassPriority(formationClass);
			this.Offset = MathF.Max(0, offset);
			this.Key = formationClassPriority + this.Offset * 11;
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x0006D74E File Offset: 0x0006B94E
		public static FormationDeploymentOrder GetDeploymentOrder(FormationClass fClass, int offset = 0)
		{
			return new FormationDeploymentOrder(fClass, offset);
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x0006D757 File Offset: 0x0006B957
		public static FormationDeploymentOrder.DeploymentOrderComparer GetComparer()
		{
			FormationDeploymentOrder.DeploymentOrderComparer deploymentOrderComparer;
			if ((deploymentOrderComparer = FormationDeploymentOrder._comparer) == null)
			{
				deploymentOrderComparer = (FormationDeploymentOrder._comparer = new FormationDeploymentOrder.DeploymentOrderComparer());
			}
			return deploymentOrderComparer;
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x0006D770 File Offset: 0x0006B970
		private static int GetFormationClassPriority(FormationClass fClass)
		{
			switch (fClass)
			{
			case FormationClass.Infantry:
				return 2;
			case FormationClass.Ranged:
				return 5;
			case FormationClass.Cavalry:
				return 4;
			case FormationClass.HorseArcher:
				return 6;
			case FormationClass.NumberOfDefaultFormations:
				return 0;
			case FormationClass.HeavyInfantry:
				return 1;
			case FormationClass.LightCavalry:
				return 7;
			case FormationClass.HeavyCavalry:
				return 3;
			case FormationClass.NumberOfRegularFormations:
				return 9;
			case FormationClass.Bodyguard:
				return 8;
			default:
				return 10;
			}
		}

		// Token: 0x04000AC4 RID: 2756
		private static FormationDeploymentOrder.DeploymentOrderComparer _comparer;

		// Token: 0x02000529 RID: 1321
		public class DeploymentOrderComparer : IComparer<FormationDeploymentOrder>
		{
			// Token: 0x06003CCE RID: 15566 RVA: 0x000F42C4 File Offset: 0x000F24C4
			public int Compare(FormationDeploymentOrder a, FormationDeploymentOrder b)
			{
				return a.Key - b.Key;
			}
		}
	}
}
