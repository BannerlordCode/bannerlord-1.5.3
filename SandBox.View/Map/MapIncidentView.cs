using System;
using TaleWorlds.CampaignSystem.Incidents;

namespace SandBox.View.Map
{
	// Token: 0x02000052 RID: 82
	public class MapIncidentView : MapView
	{
		// Token: 0x060002AD RID: 685 RVA: 0x000181B1 File Offset: 0x000163B1
		public MapIncidentView()
		{
		}

		// Token: 0x060002AE RID: 686 RVA: 0x000181B9 File Offset: 0x000163B9
		public MapIncidentView(Incident incident)
		{
			this.Incident = incident;
		}

		// Token: 0x04000177 RID: 375
		public readonly Incident Incident;
	}
}
