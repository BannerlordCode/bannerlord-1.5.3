using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Workshops;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;

namespace SandBox.Objects.AreaMarkers
{
	// Token: 0x02000047 RID: 71
	public class WorkshopAreaMarker : AreaMarker
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000F928 File Offset: 0x0000DB28
		public override string Tag
		{
			get
			{
				Workshop workshop = this.GetWorkshop();
				if (workshop == null)
				{
					return null;
				}
				return workshop.Tag;
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000F93C File Offset: 0x0000DB3C
		public Workshop GetWorkshop()
		{
			Workshop workshop = null;
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			if (settlement != null && settlement.IsTown && settlement.Town.Workshops.Length > this.AreaIndex && this.AreaIndex > 0)
			{
				workshop = settlement.Town.Workshops[this.AreaIndex];
			}
			return workshop;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000F994 File Offset: 0x0000DB94
		protected override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			if (MBEditor.HelpersEnabled() && this.CheckToggle)
			{
				float distanceSquared = this.AreaRadius * this.AreaRadius;
				List<GameEntity> list = new List<GameEntity>();
				base.Scene.GetEntities(ref list);
				foreach (GameEntity gameEntity in list)
				{
					gameEntity.HasTag("shop_prop");
				}
				foreach (UsableMachine usableMachine in (from x in list.FindAllWithType<UsableMachine>()
					where x.GameEntity.GlobalPosition.DistanceSquared(this.GameEntity.GlobalPosition) <= distanceSquared
					select x).ToList<UsableMachine>())
				{
				}
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000FA88 File Offset: 0x0000DC88
		public WorkshopType GetWorkshopType()
		{
			Workshop workshop = this.GetWorkshop();
			if (workshop == null)
			{
				return null;
			}
			return workshop.WorkshopType;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000FA9B File Offset: 0x0000DC9B
		public override TextObject GetName()
		{
			Workshop workshop = this.GetWorkshop();
			if (workshop == null)
			{
				return null;
			}
			return workshop.Name;
		}
	}
}
