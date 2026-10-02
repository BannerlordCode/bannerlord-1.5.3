using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.Objects.Usables;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Objects;

namespace SandBox.Objects.AreaMarkers
{
	// Token: 0x02000045 RID: 69
	public class CommonAreaMarker : AreaMarker
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000F659 File Offset: 0x0000D859
		// (set) Token: 0x0600029A RID: 666 RVA: 0x0000F661 File Offset: 0x0000D861
		public List<MatrixFrame> HiddenSpawnFrames { get; private set; }

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000F66A File Offset: 0x0000D86A
		public override string Tag
		{
			get
			{
				Alley alley = this.GetAlley();
				if (alley == null)
				{
					return null;
				}
				return alley.Tag;
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000F67D File Offset: 0x0000D87D
		protected override void OnInit()
		{
			this.HiddenSpawnFrames = new List<MatrixFrame>();
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000F68C File Offset: 0x0000D88C
		public override List<UsableMachine> GetUsableMachinesInRange(string excludeTag = null)
		{
			List<UsableMachine> usableMachinesInRange = base.GetUsableMachinesInRange(null);
			for (int i = usableMachinesInRange.Count - 1; i >= 0; i--)
			{
				UsableMachine usableMachine = usableMachinesInRange[i];
				string[] tags = usableMachine.GameEntity.Tags;
				if (usableMachine.GameEntity.HasScriptOfType<Passage>() || (!tags.Contains("npc_common") && !tags.Contains("npc_common_limited") && !tags.Contains("sp_guard") && !tags.Contains("sp_guard_unarmed") && !tags.Contains("sp_notable")))
				{
					usableMachinesInRange.RemoveAt(i);
				}
			}
			List<GameEntity> list = Mission.Current.Scene.FindEntitiesWithTag("sp_common_hidden").ToList<GameEntity>();
			GameEntity gameEntity = null;
			float num = float.MaxValue;
			float num2 = this.AreaRadius * this.AreaRadius;
			for (int j = list.Count - 1; j >= 0; j--)
			{
				float num3 = list[j].GlobalPosition.DistanceSquared(base.GameEntity.GlobalPosition);
				if (num3 < num)
				{
					gameEntity = list[j];
					num = num3;
				}
				if (num3 < num2)
				{
					this.HiddenSpawnFrames.Add(list[j].GetGlobalFrame());
				}
			}
			if (this.HiddenSpawnFrames.IsEmpty<MatrixFrame>() && gameEntity != null)
			{
				this.HiddenSpawnFrames.Add(gameEntity.GetGlobalFrame());
			}
			return usableMachinesInRange;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000F7F8 File Offset: 0x0000D9F8
		public Alley GetAlley()
		{
			Alley alley = null;
			Settlement settlement = PlayerEncounter.LocationEncounter.Settlement;
			if (settlement != null && ((settlement != null) ? settlement.Alleys : null) != null && this.AreaIndex > 0 && this.AreaIndex <= settlement.Alleys.Count)
			{
				alley = settlement.Alleys[this.AreaIndex - 1];
			}
			return alley;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000F854 File Offset: 0x0000DA54
		public override TextObject GetName()
		{
			Alley alley = this.GetAlley();
			if (alley == null)
			{
				return null;
			}
			return alley.Name;
		}

		// Token: 0x04000127 RID: 295
		public string Type = "";
	}
}
