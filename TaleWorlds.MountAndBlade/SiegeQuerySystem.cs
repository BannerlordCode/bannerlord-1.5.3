using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000181 RID: 385
	public class SiegeQuerySystem
	{
		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001463 RID: 5219 RVA: 0x0004A8E9 File Offset: 0x00048AE9
		public int LeftRegionMemberCount
		{
			get
			{
				return this._leftRegionMemberCount.Value;
			}
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001464 RID: 5220 RVA: 0x0004A8F6 File Offset: 0x00048AF6
		public int LeftCloseAttackerCount
		{
			get
			{
				return this._leftCloseAttackerCount.Value;
			}
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001465 RID: 5221 RVA: 0x0004A903 File Offset: 0x00048B03
		public int MiddleRegionMemberCount
		{
			get
			{
				return this._middleRegionMemberCount.Value;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001466 RID: 5222 RVA: 0x0004A910 File Offset: 0x00048B10
		public int MiddleCloseAttackerCount
		{
			get
			{
				return this._middleCloseAttackerCount.Value;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001467 RID: 5223 RVA: 0x0004A91D File Offset: 0x00048B1D
		public int RightRegionMemberCount
		{
			get
			{
				return this._rightRegionMemberCount.Value;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x06001468 RID: 5224 RVA: 0x0004A92A File Offset: 0x00048B2A
		public int RightCloseAttackerCount
		{
			get
			{
				return this._rightCloseAttackerCount.Value;
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06001469 RID: 5225 RVA: 0x0004A937 File Offset: 0x00048B37
		public int InsideAttackerCount
		{
			get
			{
				return this._insideAttackerCount.Value;
			}
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x0600146A RID: 5226 RVA: 0x0004A944 File Offset: 0x00048B44
		public int LeftDefenderCount
		{
			get
			{
				return this._leftDefenderCount.Value;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x0600146B RID: 5227 RVA: 0x0004A951 File Offset: 0x00048B51
		public int MiddleDefenderCount
		{
			get
			{
				return this._middleDefenderCount.Value;
			}
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x0004A95E File Offset: 0x00048B5E
		public int RightDefenderCount
		{
			get
			{
				return this._rightDefenderCount.Value;
			}
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0004A96C File Offset: 0x00048B6C
		public SiegeQuerySystem(Team team, IEnumerable<SiegeLane> lanes)
		{
			Mission mission = Mission.Current;
			this._attackerTeam = mission.AttackerTeam;
			Team defenderTeam = mission.DefenderTeam;
			SiegeLane siegeLane = lanes.FirstOrDefault<SiegeLane>((SiegeLane l) => l.LaneSide == FormationAI.BehaviorSide.Left);
			SiegeLane siegeLane2 = lanes.FirstOrDefault<SiegeLane>((SiegeLane l) => l.LaneSide == FormationAI.BehaviorSide.Middle);
			lanes.FirstOrDefault<SiegeLane>((SiegeLane l) => l.LaneSide == FormationAI.BehaviorSide.Right);
			Mission mission2 = Mission.Current;
			WeakGameEntity weakGameEntity = mission2.Scene.FindWeakEntityWithTag("left_defender_origin");
			if (weakGameEntity.IsValid)
			{
				this.LeftDefenderOrigin = weakGameEntity.GlobalPosition;
			}
			else
			{
				this.LeftDefenderOrigin = (siegeLane.DefenderOrigin.AsVec2.IsNonZero() ? siegeLane.DefenderOrigin.GetGroundVec3() : Vec3.Zero);
			}
			WeakGameEntity weakGameEntity2 = mission2.Scene.FindWeakEntityWithTag("left_attacker_origin");
			if (weakGameEntity2.IsValid)
			{
				this.LeftAttackerOrigin = weakGameEntity2.GlobalPosition;
			}
			else
			{
				this.LeftAttackerOrigin = (siegeLane.AttackerOrigin.AsVec2.IsNonZero() ? siegeLane.AttackerOrigin.GetGroundVec3() : Vec3.Zero);
			}
			WeakGameEntity weakGameEntity3 = mission2.Scene.FindWeakEntityWithTag("middle_defender_origin");
			if (weakGameEntity3.IsValid)
			{
				this.MidDefenderOrigin = weakGameEntity3.GlobalPosition;
			}
			else
			{
				this.MidDefenderOrigin = (siegeLane2.DefenderOrigin.AsVec2.IsNonZero() ? siegeLane2.DefenderOrigin.GetGroundVec3() : Vec3.Zero);
			}
			WeakGameEntity weakGameEntity4 = mission2.Scene.FindWeakEntityWithTag("middle_attacker_origin");
			if (weakGameEntity4.IsValid)
			{
				this.MiddleAttackerOrigin = weakGameEntity4.GlobalPosition;
			}
			else
			{
				this.MiddleAttackerOrigin = (siegeLane2.AttackerOrigin.AsVec2.IsNonZero() ? siegeLane2.AttackerOrigin.GetGroundVec3() : Vec3.Zero);
			}
			WeakGameEntity weakGameEntity5 = mission2.Scene.FindWeakEntityWithTag("right_defender_origin");
			if (weakGameEntity5.IsValid)
			{
				this.RightDefenderOrigin = weakGameEntity5.GlobalPosition;
			}
			else
			{
				this.RightDefenderOrigin = (siegeLane2.DefenderOrigin.AsVec2.IsNonZero() ? siegeLane2.DefenderOrigin.GetGroundVec3() : Vec3.Zero);
			}
			WeakGameEntity weakGameEntity6 = mission2.Scene.FindWeakEntityWithTag("right_attacker_origin");
			if (weakGameEntity6.IsValid)
			{
				this.RightAttackerOrigin = weakGameEntity6.GlobalPosition;
			}
			else
			{
				this.RightAttackerOrigin = (siegeLane2.AttackerOrigin.AsVec2.IsNonZero() ? siegeLane2.AttackerOrigin.GetGroundVec3() : Vec3.Zero);
			}
			this.LeftToMidDir = (this.MiddleAttackerOrigin.AsVec2 - this.LeftDefenderOrigin.AsVec2).Normalized();
			this.MidToLeftDir = (this.LeftAttackerOrigin.AsVec2 - this.MidDefenderOrigin.AsVec2).Normalized();
			this.MidToRightDir = (this.RightAttackerOrigin.AsVec2 - this.MidDefenderOrigin.AsVec2).Normalized();
			this.RightToMidDir = (this.MiddleAttackerOrigin.AsVec2 - this.RightDefenderOrigin.AsVec2).Normalized();
			this._leftRegionMemberCount = new QueryData<int>(() => this.LocateAttackers(SiegeQuerySystem.RegionEnum.Left), 5f);
			this._leftCloseAttackerCount = new QueryData<int>(() => this.LocateAttackers(SiegeQuerySystem.RegionEnum.LeftClose), 5f);
			this._middleRegionMemberCount = new QueryData<int>(() => this.LocateAttackers(SiegeQuerySystem.RegionEnum.Middle), 5f);
			this._middleCloseAttackerCount = new QueryData<int>(() => this.LocateAttackers(SiegeQuerySystem.RegionEnum.MiddleClose), 5f);
			this._rightRegionMemberCount = new QueryData<int>(() => this.LocateAttackers(SiegeQuerySystem.RegionEnum.Right), 5f);
			this._rightCloseAttackerCount = new QueryData<int>(() => this.LocateAttackers(SiegeQuerySystem.RegionEnum.RightClose), 5f);
			this._insideAttackerCount = new QueryData<int>(() => this.LocateAttackers(SiegeQuerySystem.RegionEnum.Inside), 5f);
			this._leftDefenderCount = new QueryData<int>(() => mission.GetNearbyAllyAgentsCount(this.LeftDefenderOrigin.AsVec2, 10f, defenderTeam), 5f);
			this._middleDefenderCount = new QueryData<int>(() => mission.GetNearbyAllyAgentsCount(this.MidDefenderOrigin.AsVec2, 10f, defenderTeam), 5f);
			this._rightDefenderCount = new QueryData<int>(() => mission.GetNearbyAllyAgentsCount(this.RightDefenderOrigin.AsVec2, 10f, defenderTeam), 5f);
			this.DefenderLeftToDefenderMidDir = (this.MidDefenderOrigin.AsVec2 - this.LeftDefenderOrigin.AsVec2).Normalized();
			this.DefenderMidToDefenderRightDir = (this.RightDefenderOrigin.AsVec2 - this.MidDefenderOrigin.AsVec2).Normalized();
			this.InitializeTelemetryScopeNames();
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0004AEB8 File Offset: 0x000490B8
		private int LocateAttackers(SiegeQuerySystem.RegionEnum region)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			foreach (Agent agent in this._attackerTeam.ActiveAgents)
			{
				Vec2 vec = agent.Position.AsVec2 - this.LeftDefenderOrigin.AsVec2;
				Vec2 vec2 = agent.Position.AsVec2 - this.MidDefenderOrigin.AsVec2;
				Vec2 vec3 = agent.Position.AsVec2 - this.RightDefenderOrigin.AsVec2;
				if (vec.Normalize() < 15f && Math.Abs(agent.Position.z - this.LeftDefenderOrigin.z) <= 3f)
				{
					num2++;
					num++;
				}
				else
				{
					if (vec.DotProduct(this.LeftToMidDir) >= 0f && vec.DotProduct(this.LeftToMidDir.RightVec()) >= 0f)
					{
						num++;
					}
					else if (vec2.DotProduct(this.MidToLeftDir) >= 0f && vec2.DotProduct(this.MidToLeftDir.RightVec()) >= 0f)
					{
						num++;
					}
					if (vec3.Normalize() < 15f && Math.Abs(agent.Position.z - this.RightDefenderOrigin.z) <= 3f)
					{
						num6++;
						num5++;
					}
					else
					{
						if (vec3.DotProduct(this.RightToMidDir) >= 0f && vec3.DotProduct(this.RightToMidDir.LeftVec()) >= 0f)
						{
							num5++;
						}
						else if (vec2.DotProduct(this.MidToRightDir) >= 0f && vec2.DotProduct(this.MidToRightDir.LeftVec()) >= 0f)
						{
							num5++;
						}
						if (vec2.Normalize() < 15f && Math.Abs(agent.Position.z - this.MidDefenderOrigin.z) <= 3f)
						{
							num4++;
							num3++;
						}
						else
						{
							if ((vec2.DotProduct(this.MidToLeftDir) < 0f || vec2.DotProduct(this.MidToLeftDir.RightVec()) < 0f || vec.DotProduct(this.LeftToMidDir) < 0f || vec.DotProduct(this.LeftToMidDir.RightVec()) < 0f) && (vec2.DotProduct(this.MidToRightDir) < 0f || vec2.DotProduct(this.MidToRightDir.LeftVec()) < 0f || vec3.DotProduct(this.RightToMidDir) < 0f || vec3.DotProduct(this.RightToMidDir.LeftVec()) < 0f))
							{
								num3++;
							}
							if (agent.GetCurrentNavigationFaceId() % 10 == 1)
							{
								num7++;
							}
						}
					}
				}
			}
			float currentTime = Mission.Current.CurrentTime;
			this._leftRegionMemberCount.SetValue(num, currentTime);
			this._leftCloseAttackerCount.SetValue(num2, currentTime);
			this._middleRegionMemberCount.SetValue(num3, currentTime);
			this._middleCloseAttackerCount.SetValue(num4, currentTime);
			this._rightRegionMemberCount.SetValue(num5, currentTime);
			this._rightCloseAttackerCount.SetValue(num6, currentTime);
			this._insideAttackerCount.SetValue(num7, currentTime);
			switch (region)
			{
			case SiegeQuerySystem.RegionEnum.Left:
				return num;
			case SiegeQuerySystem.RegionEnum.LeftClose:
				return num2;
			case SiegeQuerySystem.RegionEnum.Middle:
				return num3;
			case SiegeQuerySystem.RegionEnum.MiddleClose:
				return num4;
			case SiegeQuerySystem.RegionEnum.Right:
				return num5;
			case SiegeQuerySystem.RegionEnum.RightClose:
				return num6;
			case SiegeQuerySystem.RegionEnum.Inside:
				return num7;
			default:
				return 0;
			}
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0004B2C0 File Offset: 0x000494C0
		public void Expire()
		{
			this._leftRegionMemberCount.Expire();
			this._leftCloseAttackerCount.Expire();
			this._middleRegionMemberCount.Expire();
			this._middleCloseAttackerCount.Expire();
			this._rightRegionMemberCount.Expire();
			this._rightCloseAttackerCount.Expire();
			this._insideAttackerCount.Expire();
			this._leftDefenderCount.Expire();
			this._middleDefenderCount.Expire();
			this._rightDefenderCount.Expire();
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0004B33B File Offset: 0x0004953B
		private void InitializeTelemetryScopeNames()
		{
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0004B340 File Offset: 0x00049540
		public int DeterminePositionAssociatedSide(Vec3 position)
		{
			float num = position.AsVec2.DistanceSquared(this.LeftDefenderOrigin.AsVec2);
			float num2 = position.AsVec2.DistanceSquared(this.MidDefenderOrigin.AsVec2);
			float num3 = position.AsVec2.DistanceSquared(this.RightDefenderOrigin.AsVec2);
			FormationAI.BehaviorSide behaviorSide;
			if (num < num2 && num < num3)
			{
				behaviorSide = FormationAI.BehaviorSide.Left;
			}
			else if (num3 < num2)
			{
				behaviorSide = FormationAI.BehaviorSide.Right;
			}
			else
			{
				behaviorSide = FormationAI.BehaviorSide.Middle;
			}
			FormationAI.BehaviorSide behaviorSide2 = FormationAI.BehaviorSide.BehaviorSideNotSet;
			switch (behaviorSide)
			{
			case FormationAI.BehaviorSide.Left:
				if ((position.AsVec2 - this.LeftDefenderOrigin.AsVec2).Normalized().DotProduct(this.DefenderLeftToDefenderMidDir) > 0f)
				{
					behaviorSide2 = FormationAI.BehaviorSide.Middle;
				}
				break;
			case FormationAI.BehaviorSide.Middle:
				if ((position.AsVec2 - this.MidDefenderOrigin.AsVec2).Normalized().DotProduct(this.DefenderMidToDefenderRightDir) > 0f)
				{
					behaviorSide2 = FormationAI.BehaviorSide.Right;
				}
				else
				{
					behaviorSide2 = FormationAI.BehaviorSide.Left;
				}
				break;
			case FormationAI.BehaviorSide.Right:
				if ((position.AsVec2 - this.RightDefenderOrigin.AsVec2).Normalized().DotProduct(this.DefenderMidToDefenderRightDir) < 0f)
				{
					behaviorSide2 = FormationAI.BehaviorSide.Middle;
				}
				break;
			}
			int num4 = 1 << (int)behaviorSide;
			if (behaviorSide2 != FormationAI.BehaviorSide.BehaviorSideNotSet)
			{
				num4 |= 1 << (int)behaviorSide2;
			}
			return num4;
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0004B4BE File Offset: 0x000496BE
		public static bool AreSidesRelated(FormationAI.BehaviorSide side, int connectedSides)
		{
			return ((1 << (int)side) & connectedSides) != 0;
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0004B4CC File Offset: 0x000496CC
		public static int SideDistance(int connectedSides, int side)
		{
			while (connectedSides != 0 && side != 0)
			{
				connectedSides >>= 1;
				side >>= 1;
			}
			int i = ((connectedSides != 0) ? connectedSides : side);
			int num = 0;
			while (i > 0)
			{
				num++;
				if ((i & 1) == 1)
				{
					break;
				}
				i >>= 1;
			}
			return num;
		}

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001474 RID: 5236 RVA: 0x0004B50A File Offset: 0x0004970A
		public Vec3 LeftDefenderOrigin { get; }

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001475 RID: 5237 RVA: 0x0004B512 File Offset: 0x00049712
		public Vec3 MidDefenderOrigin { get; }

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001476 RID: 5238 RVA: 0x0004B51A File Offset: 0x0004971A
		public Vec3 RightDefenderOrigin { get; }

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06001477 RID: 5239 RVA: 0x0004B522 File Offset: 0x00049722
		public Vec3 LeftAttackerOrigin { get; }

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06001478 RID: 5240 RVA: 0x0004B52A File Offset: 0x0004972A
		public Vec3 MiddleAttackerOrigin { get; }

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06001479 RID: 5241 RVA: 0x0004B532 File Offset: 0x00049732
		public Vec3 RightAttackerOrigin { get; }

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x0600147A RID: 5242 RVA: 0x0004B53A File Offset: 0x0004973A
		public Vec2 LeftToMidDir { get; }

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x0600147B RID: 5243 RVA: 0x0004B542 File Offset: 0x00049742
		public Vec2 MidToLeftDir { get; }

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x0600147C RID: 5244 RVA: 0x0004B54A File Offset: 0x0004974A
		public Vec2 MidToRightDir { get; }

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x0600147D RID: 5245 RVA: 0x0004B552 File Offset: 0x00049752
		public Vec2 RightToMidDir { get; }

		// Token: 0x04000545 RID: 1349
		private const float LaneProximityDistance = 15f;

		// Token: 0x04000546 RID: 1350
		private readonly Team _attackerTeam;

		// Token: 0x04000547 RID: 1351
		public Vec2 DefenderLeftToDefenderMidDir;

		// Token: 0x04000548 RID: 1352
		public Vec2 DefenderMidToDefenderRightDir;

		// Token: 0x04000549 RID: 1353
		private readonly QueryData<int> _leftRegionMemberCount;

		// Token: 0x0400054A RID: 1354
		private readonly QueryData<int> _leftCloseAttackerCount;

		// Token: 0x0400054B RID: 1355
		private readonly QueryData<int> _middleRegionMemberCount;

		// Token: 0x0400054C RID: 1356
		private readonly QueryData<int> _middleCloseAttackerCount;

		// Token: 0x0400054D RID: 1357
		private readonly QueryData<int> _rightRegionMemberCount;

		// Token: 0x0400054E RID: 1358
		private readonly QueryData<int> _rightCloseAttackerCount;

		// Token: 0x0400054F RID: 1359
		private readonly QueryData<int> _insideAttackerCount;

		// Token: 0x04000550 RID: 1360
		private readonly QueryData<int> _leftDefenderCount;

		// Token: 0x04000551 RID: 1361
		private readonly QueryData<int> _middleDefenderCount;

		// Token: 0x04000552 RID: 1362
		private readonly QueryData<int> _rightDefenderCount;

		// Token: 0x020004D6 RID: 1238
		private enum RegionEnum
		{
			// Token: 0x04001C6A RID: 7274
			Left,
			// Token: 0x04001C6B RID: 7275
			LeftClose,
			// Token: 0x04001C6C RID: 7276
			Middle,
			// Token: 0x04001C6D RID: 7277
			MiddleClose,
			// Token: 0x04001C6E RID: 7278
			Right,
			// Token: 0x04001C6F RID: 7279
			RightClose,
			// Token: 0x04001C70 RID: 7280
			Inside
		}
	}
}
