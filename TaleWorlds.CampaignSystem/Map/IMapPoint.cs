using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.Map
{
	// Token: 0x02000227 RID: 551
	public interface IMapPoint
	{
		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x06002145 RID: 8517
		TextObject Name { get; }

		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06002146 RID: 8518
		CampaignVec2 Position { get; }

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06002147 RID: 8519
		PathFaceRecord CurrentNavigationFace { get; }

		// Token: 0x06002148 RID: 8520
		Vec3 GetPositionAsVec3();

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06002149 RID: 8521
		IFaction MapFaction { get; }

		// Token: 0x17000819 RID: 2073
		// (get) Token: 0x0600214A RID: 8522
		bool IsInspected { get; }

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x0600214B RID: 8523
		bool IsVisible { get; }

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x0600214C RID: 8524
		// (set) Token: 0x0600214D RID: 8525
		bool IsActive { get; set; }
	}
}
