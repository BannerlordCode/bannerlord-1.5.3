using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Objects
{
	// Token: 0x020003A8 RID: 936
	public class AreaMarker : MissionObject, ITrackableBase
	{
		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x0600359D RID: 13725 RVA: 0x000DD835 File Offset: 0x000DBA35
		public virtual string Tag
		{
			get
			{
				return "area_marker_" + this.AreaIndex;
			}
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x000DD84C File Offset: 0x000DBA4C
		protected internal override void OnEditorTick(float dt)
		{
			if (this.CheckToggle)
			{
				MBEditor.HelpersEnabled();
			}
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x000DD85C File Offset: 0x000DBA5C
		protected internal override void OnEditorInit()
		{
			this.CheckToggle = false;
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x000DD868 File Offset: 0x000DBA68
		public bool IsPositionInRange(Vec3 position)
		{
			return position.DistanceSquared(base.GameEntity.GlobalPosition) <= this.AreaRadius * this.AreaRadius;
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x000DD89C File Offset: 0x000DBA9C
		public virtual List<UsableMachine> GetUsableMachinesInRange(string excludeTag = null)
		{
			float distanceSquared = this.AreaRadius * this.AreaRadius;
			return (from x in Mission.Current.ActiveMissionObjects.FindAllWithType<UsableMachine>()
				where !x.IsDeactivated && x.GameEntity.GlobalPosition.DistanceSquared(this.GameEntity.GlobalPosition) <= distanceSquared && !x.GameEntity.HasTag(excludeTag)
				select x).ToList<UsableMachine>();
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x000DD8F8 File Offset: 0x000DBAF8
		public virtual List<UsableMachine> GetUsableMachinesWithTagInRange(string tag)
		{
			float distanceSquared = this.AreaRadius * this.AreaRadius;
			return (from x in Mission.Current.ActiveMissionObjects.FindAllWithType<UsableMachine>()
				where x.GameEntity.GlobalPosition.DistanceSquared(this.GameEntity.GlobalPosition) <= distanceSquared && x.GameEntity.HasTag(tag)
				select x).ToList<UsableMachine>();
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x000DD954 File Offset: 0x000DBB54
		public virtual List<GameEntity> GetGameEntitiesWithTagInRange(string tag)
		{
			float distanceSquared = this.AreaRadius * this.AreaRadius;
			return (from x in Mission.Current.Scene.FindEntitiesWithTag(tag)
				where x.GlobalPosition.DistanceSquared(this.GameEntity.GlobalPosition) <= distanceSquared && x.HasTag(tag)
				select x).ToList<GameEntity>();
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x000DD9B4 File Offset: 0x000DBBB4
		public virtual TextObject GetName()
		{
			return new TextObject(base.GameEntity.Name, null);
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x000DD9D8 File Offset: 0x000DBBD8
		public virtual Vec3 GetPosition()
		{
			return base.GameEntity.GlobalPosition;
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x000DD9F3 File Offset: 0x000DBBF3
		TextObject ITrackableBase.GetName()
		{
			return this.GetName();
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x000DD9FB File Offset: 0x000DBBFB
		Vec3 ITrackableBase.GetPosition()
		{
			return this.GetPosition();
		}

		// Token: 0x040016DA RID: 5850
		public float AreaRadius = 3f;

		// Token: 0x040016DB RID: 5851
		public int AreaIndex;

		// Token: 0x040016DC RID: 5852
		public bool CheckToggle;
	}
}
