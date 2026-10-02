using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000215 RID: 533
	public class DefaultDeploymentPlan
	{
		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x06001EDE RID: 7902 RVA: 0x0006A774 File Offset: 0x00068974
		public bool SpawnWithHorses
		{
			get
			{
				return this._spawnWithHorses;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x06001EDF RID: 7903 RVA: 0x0006A77C File Offset: 0x0006897C
		public int PlanCount
		{
			get
			{
				return this._planCount;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x06001EE0 RID: 7904 RVA: 0x0006A784 File Offset: 0x00068984
		// (set) Token: 0x06001EE1 RID: 7905 RVA: 0x0006A78C File Offset: 0x0006898C
		public bool IsPlanMade { get; private set; }

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x06001EE2 RID: 7906 RVA: 0x0006A795 File Offset: 0x00068995
		// (set) Token: 0x06001EE3 RID: 7907 RVA: 0x0006A79D File Offset: 0x0006899D
		public float SpawnPathOffset { get; private set; }

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06001EE4 RID: 7908 RVA: 0x0006A7A6 File Offset: 0x000689A6
		// (set) Token: 0x06001EE5 RID: 7909 RVA: 0x0006A7AE File Offset: 0x000689AE
		public float TargetOffset { get; private set; }

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06001EE6 RID: 7910 RVA: 0x0006A7B7 File Offset: 0x000689B7
		public bool IsSafeToDeploy
		{
			get
			{
				return this.SafetyScore >= 50f;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06001EE7 RID: 7911 RVA: 0x0006A7C9 File Offset: 0x000689C9
		// (set) Token: 0x06001EE8 RID: 7912 RVA: 0x0006A7D1 File Offset: 0x000689D1
		public float SafetyScore { get; private set; }

		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06001EE9 RID: 7913 RVA: 0x0006A7DC File Offset: 0x000689DC
		public int FootTroopCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < 11; i++)
				{
					num += this._formationFootTroopCounts[i];
				}
				return num;
			}
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06001EEA RID: 7914 RVA: 0x0006A804 File Offset: 0x00068A04
		public int MountedTroopCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < 11; i++)
				{
					num += this._formationMountedTroopCounts[i];
				}
				return num;
			}
		}

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06001EEB RID: 7915 RVA: 0x0006A82C File Offset: 0x00068A2C
		public int TroopCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < 11; i++)
				{
					num += this._formationFootTroopCounts[i] + this._formationMountedTroopCounts[i];
				}
				return num;
			}
		}

		// Token: 0x17000640 RID: 1600
		// (get) Token: 0x06001EEC RID: 7916 RVA: 0x0006A85D File Offset: 0x00068A5D
		public Vec3 MeanPosition
		{
			get
			{
				return this._meanPosition;
			}
		}

		// Token: 0x06001EED RID: 7917 RVA: 0x0006A865 File Offset: 0x00068A65
		public static DefaultDeploymentPlan CreateInitialPlan(Mission mission, Team team)
		{
			return new DefaultDeploymentPlan(mission, team, false, null, 0f);
		}

		// Token: 0x06001EEE RID: 7918 RVA: 0x0006A875 File Offset: 0x00068A75
		public static DefaultDeploymentPlan CreateReinforcementPlan(Mission mission, Team team)
		{
			return new DefaultDeploymentPlan(mission, team, true, null, 0f);
		}

		// Token: 0x06001EEF RID: 7919 RVA: 0x0006A885 File Offset: 0x00068A85
		public static DefaultDeploymentPlan CreateReinforcementPlanWithSpawnPath(Mission mission, Team team, SpawnPathData spawnPathData, float reinforcementOffset)
		{
			return new DefaultDeploymentPlan(mission, team, true, spawnPathData, reinforcementOffset);
		}

		// Token: 0x06001EF0 RID: 7920 RVA: 0x0006A894 File Offset: 0x00068A94
		private DefaultDeploymentPlan(Mission mission, Team team, bool isReinforcement, SpawnPathData spawnPathData, float spawnPathReinforcementOffset = 0f)
		{
			this._mission = mission;
			this._planCount = 0;
			this.Team = team;
			this.IsReinforcement = isReinforcement;
			this.SpawnPathData = spawnPathData;
			int num = 11;
			this._formationPlans = new DefaultFormationDeploymentPlan[num];
			this._formationFootTroopCounts = new int[num];
			this._formationMountedTroopCounts = new int[num];
			this._meanPosition = Vec3.Zero;
			this.IsPlanMade = false;
			this.SpawnPathOffset = 0f;
			this.SpawnPathReinforcementOffset = spawnPathReinforcementOffset;
			this.TargetOffset = 0f;
			this.SafetyScore = 100f;
			for (int i = 0; i < this._formationPlans.Length; i++)
			{
				FormationClass formationClass = (FormationClass)i;
				this._formationPlans[i] = new DefaultFormationDeploymentPlan(formationClass);
			}
			for (int j = 0; j < 4; j++)
			{
				this._deploymentFlanks[j] = new SortedList<FormationDeploymentOrder, DefaultFormationDeploymentPlan>(FormationDeploymentOrder.GetComparer());
			}
			this.ClearAddedTroops();
			this.ClearPlan();
		}

		// Token: 0x06001EF1 RID: 7921 RVA: 0x0006A986 File Offset: 0x00068B86
		public void SetSpawnWithHorses(bool value)
		{
			this._spawnWithHorses = value;
		}

		// Token: 0x06001EF2 RID: 7922 RVA: 0x0006A98F File Offset: 0x00068B8F
		public void MakeDeploymentPlan(FormationSceneSpawnEntry[,] formationSceneSpawnEntries = null)
		{
			this.PlanFormationDimensions();
			if (this._mission.HasSpawnPath)
			{
				this.PlanFieldBattleDeploymentFromSpawnPath();
			}
			else if (this._mission.IsFieldBattle)
			{
				this.PlanFieldBattleDeploymentFromSceneData(formationSceneSpawnEntries);
			}
			else
			{
				this.PlanBattleDeploymentFromSceneData(formationSceneSpawnEntries);
			}
			this.ComputeMeanPosition();
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x0006A9D0 File Offset: 0x00068BD0
		public void ClearPlan()
		{
			DefaultFormationDeploymentPlan[] formationPlans = this._formationPlans;
			for (int i = 0; i < formationPlans.Length; i++)
			{
				formationPlans[i].Clear();
			}
			SortedList<FormationDeploymentOrder, DefaultFormationDeploymentPlan>[] deploymentFlanks = this._deploymentFlanks;
			for (int i = 0; i < deploymentFlanks.Length; i++)
			{
				deploymentFlanks[i].Clear();
			}
			this.IsPlanMade = false;
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x0006AA20 File Offset: 0x00068C20
		public void ClearAddedTroops()
		{
			for (int i = 0; i < 11; i++)
			{
				this._formationFootTroopCounts[i] = 0;
				this._formationMountedTroopCounts[i] = 0;
			}
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x0006AA4C File Offset: 0x00068C4C
		public void AddTroops(FormationClass formationClass, int footTroopCount, int mountedTroopCount)
		{
			if (footTroopCount + mountedTroopCount > 0 && formationClass < FormationClass.NumberOfAllFormationsWithUnset)
			{
				this._formationFootTroopCounts[(int)formationClass] += footTroopCount;
				this._formationMountedTroopCounts[(int)formationClass] += mountedTroopCount;
			}
		}

		// Token: 0x06001EF6 RID: 7926 RVA: 0x0006AA88 File Offset: 0x00068C88
		public DefaultFormationDeploymentPlan GetFormationPlan(FormationClass fClass)
		{
			return this._formationPlans[(int)fClass];
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x0006AA94 File Offset: 0x00068C94
		public bool GetFormationDeploymentFrame(FormationClass fClass, out MatrixFrame frame)
		{
			DefaultFormationDeploymentPlan formationPlan = this.GetFormationPlan(fClass);
			if (formationPlan.HasFrame())
			{
				frame = formationPlan.GetFrame();
				return true;
			}
			frame = MatrixFrame.Identity;
			return false;
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x0006AACC File Offset: 0x00068CCC
		public bool GetFirstValidFormationFrame(out MatrixFrame frame, bool checkDimensions)
		{
			foreach (DefaultFormationDeploymentPlan defaultFormationDeploymentPlan in this._formationPlans)
			{
				if (defaultFormationDeploymentPlan.HasFrame() && (!checkDimensions || defaultFormationDeploymentPlan.HasDimensions))
				{
					frame = defaultFormationDeploymentPlan.GetFrame();
					return true;
				}
			}
			frame = MatrixFrame.Identity;
			return false;
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x0006AB20 File Offset: 0x00068D20
		public MatrixFrame ComputeFormationsCenterFrameAndExtents(bool ignoreDimensionlessFormations, out Vec2 halfExtents)
		{
			MatrixFrame matrixFrame;
			this.GetFirstValidFormationFrame(out matrixFrame, true);
			float num = float.MinValue;
			float num2 = float.MaxValue;
			float num3 = float.MaxValue;
			float num4 = float.MinValue;
			foreach (DefaultFormationDeploymentPlan defaultFormationDeploymentPlan in this._formationPlans)
			{
				if (defaultFormationDeploymentPlan.HasFrame() && (!ignoreDimensionlessFormations || defaultFormationDeploymentPlan.HasDimensions))
				{
					MatrixFrame frame = defaultFormationDeploymentPlan.GetFrame();
					MatrixFrame matrixFrame2 = matrixFrame.TransformToLocal(in frame);
					float num5 = defaultFormationDeploymentPlan.PlannedWidth * 0.5f;
					float plannedDepth = defaultFormationDeploymentPlan.PlannedDepth;
					float num6 = MathF.Abs(matrixFrame2.rotation.s.x) * num5 + MathF.Abs(matrixFrame2.rotation.f.x) * plannedDepth;
					float num7 = MathF.Abs(matrixFrame2.rotation.s.y) * num5;
					float num8 = MathF.Abs(matrixFrame2.rotation.s.y) * num5 + MathF.Abs(matrixFrame2.rotation.f.y) * plannedDepth;
					float num9 = matrixFrame2.origin.y + num7;
					float num10 = matrixFrame2.origin.y - num8;
					float num11 = matrixFrame2.origin.x - num6;
					float num12 = matrixFrame2.origin.x + num6;
					num = MathF.Max(num, num9);
					num2 = MathF.Min(num2, num10);
					num3 = MathF.Min(num3, num11);
					num4 = MathF.Max(num4, num12);
				}
			}
			float num13 = (num3 + num4) * 0.5f;
			float num14 = (num2 + num) * 0.5f;
			halfExtents = new Vec2((num4 - num3) / 2f, (num - num2) / 2f);
			Vec3 vec = new Vec3(num13, num14, 0f, -1f);
			Vec3 vec2 = matrixFrame.TransformToParent(in vec);
			vec2.z = Mission.Current.Scene.GetTerrainHeight(vec2.AsVec2, true);
			return new MatrixFrame(in matrixFrame.rotation, in vec2);
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x0006AD30 File Offset: 0x00068F30
		public bool IsPlanSuitableForFormations(ValueTuple<int, int>[] troopDataPerFormationClass)
		{
			if (troopDataPerFormationClass.Length == 11)
			{
				for (int i = 0; i < 11; i++)
				{
					FormationClass formationClass = (FormationClass)i;
					DefaultFormationDeploymentPlan formationPlan = this.GetFormationPlan(formationClass);
					ValueTuple<int, int> valueTuple = troopDataPerFormationClass[i];
					if (formationPlan.PlannedFootTroopCount != valueTuple.Item1 || formationPlan.PlannedMountedTroopCount != valueTuple.Item2)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x0006AD84 File Offset: 0x00068F84
		public void UpdateSafetyScore()
		{
			if (this._mission.Teams == null)
			{
				return;
			}
			float num = 100f;
			Team team = ((this.Team.Side == BattleSideEnum.Attacker) ? this._mission.Teams.Defender : ((this.Team.Side == BattleSideEnum.Defender) ? this._mission.Teams.Attacker : null));
			if (team != null)
			{
				foreach (Formation formation in team.FormationsIncludingEmpty)
				{
					if (formation.CountOfUnits > 0)
					{
						float num2 = this._meanPosition.AsVec2.Distance(formation.CachedAveragePosition);
						if (num >= num2)
						{
							num = num2;
						}
					}
				}
			}
			team = ((this.Team.Side == BattleSideEnum.Attacker) ? this._mission.Teams.DefenderAlly : ((this.Team.Side == BattleSideEnum.Defender) ? this._mission.Teams.AttackerAlly : null));
			if (team != null)
			{
				foreach (Formation formation2 in team.FormationsIncludingEmpty)
				{
					if (formation2.CountOfUnits > 0)
					{
						float num3 = this._meanPosition.AsVec2.Distance(formation2.CachedAveragePosition);
						if (num >= num3)
						{
							num = num3;
						}
					}
				}
			}
			this.SafetyScore = num;
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x0006AF0C File Offset: 0x0006910C
		public void SetSpawnPathOffset(float pathOffset = 0f, float targetOffset = 0f)
		{
			this.SpawnPathOffset = pathOffset;
			this.TargetOffset = targetOffset;
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x0006AF1C File Offset: 0x0006911C
		public WorldFrame GetFrameFromFormationSpawnEntity(GameEntity formationSpawnEntity, float depthOffset = 0f)
		{
			MatrixFrame globalFrame = formationSpawnEntity.GetGlobalFrame();
			globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			WorldPosition worldPosition = new WorldPosition(this._mission.Scene, UIntPtr.Zero, globalFrame.origin, false);
			WorldPosition worldPosition2 = worldPosition;
			if (depthOffset != 0f)
			{
				worldPosition2.SetVec2(worldPosition2.AsVec2 - depthOffset * globalFrame.rotation.f.AsVec2);
				if (!worldPosition2.IsValid || worldPosition2.GetNavMesh() == UIntPtr.Zero)
				{
					worldPosition2 = worldPosition;
				}
			}
			return new WorldFrame(globalFrame.rotation, worldPosition2);
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x0006AFB8 File Offset: 0x000691B8
		private void PlanFieldBattleDeploymentFromSpawnPath()
		{
			bool flag = this.FootTroopCount > 0;
			for (int i = 0; i < this._formationPlans.Length; i++)
			{
				FormationClass formationClass = (FormationClass)i;
				int num = this._formationFootTroopCounts[i] + this._formationMountedTroopCounts[i];
				if (num > 0 || (!this.IsReinforcement && (formationClass == FormationClass.NumberOfRegularFormations || formationClass == FormationClass.Bodyguard)))
				{
					DefaultFormationDeploymentPlan defaultFormationDeploymentPlan = this._formationPlans[i];
					FormationDeploymentFlank defaultFlank = defaultFormationDeploymentPlan.GetDefaultFlank(num, flag, this._spawnWithHorses);
					int num2 = ((num > 0 || formationClass == FormationClass.NumberOfRegularFormations) ? 0 : 1);
					FormationDeploymentOrder flankDeploymentOrder = defaultFormationDeploymentPlan.GetFlankDeploymentOrder(num2);
					this._deploymentFlanks[(int)defaultFlank].Add(flankDeploymentOrder, defaultFormationDeploymentPlan);
				}
			}
			float num3 = this.ComputeHorizontalCenterOffset();
			SpawnPathData initialSpawnPathData = this._mission.GetInitialSpawnPathData(this.Team.Side);
			Vec2 asVec;
			Vec2 vec;
			if (this.IsReinforcement)
			{
				MatrixFrame matrixFrame = this.SpawnPathData.GetSpawnFrame(this.SpawnPathReinforcementOffset, true, SpawnPathData.SearchDirection.Forward);
				asVec = matrixFrame.origin.AsVec2;
				matrixFrame = initialSpawnPathData.GetCenterFrame();
				vec = (matrixFrame.origin.AsVec2 - asVec).Normalized();
			}
			else
			{
				initialSpawnPathData.GetSpawnPathFrameFacingTarget(this.SpawnPathOffset, this.TargetOffset, false, out asVec, out vec, true, 0.2f);
			}
			this.DeployFlanks(asVec, vec, num3);
			SortedList<FormationDeploymentOrder, DefaultFormationDeploymentPlan>[] deploymentFlanks = this._deploymentFlanks;
			for (int j = 0; j < deploymentFlanks.Length; j++)
			{
				deploymentFlanks[j].Clear();
			}
			this.IsPlanMade = true;
			this._planCount++;
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x0006B138 File Offset: 0x00069338
		private void PlanFieldBattleDeploymentFromSceneData(FormationSceneSpawnEntry[,] formationSceneSpawnEntries)
		{
			if (formationSceneSpawnEntries == null || formationSceneSpawnEntries.GetLength(0) != 2 || formationSceneSpawnEntries.GetLength(1) != this._formationPlans.Length)
			{
				return;
			}
			int side = (int)this.Team.Side;
			int num = ((this.Team.Side == BattleSideEnum.Attacker) ? 0 : 1);
			Dictionary<GameEntity, float> dictionary = new Dictionary<GameEntity, float>();
			bool flag = !this.IsReinforcement;
			for (int i = 0; i < this._formationPlans.Length; i++)
			{
				DefaultFormationDeploymentPlan defaultFormationDeploymentPlan = this._formationPlans[i];
				FormationSceneSpawnEntry formationSceneSpawnEntry = formationSceneSpawnEntries[side, i];
				FormationSceneSpawnEntry formationSceneSpawnEntry2 = formationSceneSpawnEntries[num, i];
				GameEntity gameEntity = (flag ? formationSceneSpawnEntry.SpawnEntity : formationSceneSpawnEntry.ReinforcementSpawnEntity);
				GameEntity gameEntity2 = (flag ? formationSceneSpawnEntry2.SpawnEntity : formationSceneSpawnEntry2.ReinforcementSpawnEntity);
				if (gameEntity != null && gameEntity2 != null)
				{
					WorldFrame worldFrame = this.ComputeFieldBattleDeploymentFrameForFormation(defaultFormationDeploymentPlan, gameEntity, gameEntity2, ref dictionary);
					defaultFormationDeploymentPlan.SetFrame(in worldFrame);
				}
				else
				{
					defaultFormationDeploymentPlan.SetFrame(in WorldFrame.Invalid);
				}
				defaultFormationDeploymentPlan.SetSpawnClass(formationSceneSpawnEntry.FormationClass);
			}
			this.IsPlanMade = true;
			this._planCount++;
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x0006B25C File Offset: 0x0006945C
		private void PlanBattleDeploymentFromSceneData(FormationSceneSpawnEntry[,] formationSceneSpawnEntries)
		{
			if (formationSceneSpawnEntries == null || formationSceneSpawnEntries.GetLength(0) != 2 || formationSceneSpawnEntries.GetLength(1) != this._formationPlans.Length)
			{
				return;
			}
			int side = (int)this.Team.Side;
			Dictionary<GameEntity, float> dictionary = new Dictionary<GameEntity, float>();
			bool flag = !this.IsReinforcement;
			for (int i = 0; i < this._formationPlans.Length; i++)
			{
				DefaultFormationDeploymentPlan defaultFormationDeploymentPlan = this._formationPlans[i];
				FormationSceneSpawnEntry formationSceneSpawnEntry = formationSceneSpawnEntries[side, i];
				GameEntity gameEntity = (flag ? formationSceneSpawnEntry.SpawnEntity : formationSceneSpawnEntry.ReinforcementSpawnEntity);
				if (gameEntity != null)
				{
					float andUpdateSpawnDepth = this.GetAndUpdateSpawnDepth(ref dictionary, gameEntity, defaultFormationDeploymentPlan);
					DefaultFormationDeploymentPlan defaultFormationDeploymentPlan2 = defaultFormationDeploymentPlan;
					WorldFrame frameFromFormationSpawnEntity = this.GetFrameFromFormationSpawnEntity(gameEntity, andUpdateSpawnDepth);
					defaultFormationDeploymentPlan2.SetFrame(in frameFromFormationSpawnEntity);
				}
				else
				{
					defaultFormationDeploymentPlan.SetFrame(in WorldFrame.Invalid);
				}
				defaultFormationDeploymentPlan.SetSpawnClass(formationSceneSpawnEntry.FormationClass);
			}
			this.IsPlanMade = true;
			this._planCount++;
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x0006B344 File Offset: 0x00069544
		private void PlanFormationDimensions()
		{
			for (int i = 0; i < this._formationPlans.Length; i++)
			{
				int num = this._formationFootTroopCounts[i];
				int num2 = this._formationMountedTroopCounts[i];
				int num3 = num + num2;
				DefaultFormationDeploymentPlan defaultFormationDeploymentPlan = this._formationPlans[i];
				if (num3 > 0)
				{
					bool flag = DefaultMissionDeploymentPlan.HasSignificantMountedTroops(num, num2);
					ValueTuple<float, float> formationSpawnWidthAndDepth = DefaultDeploymentPlan.GetFormationSpawnWidthAndDepth(defaultFormationDeploymentPlan.Class, num3, flag, !this._spawnWithHorses);
					float item = formationSpawnWidthAndDepth.Item1;
					float item2 = formationSpawnWidthAndDepth.Item2;
					defaultFormationDeploymentPlan.SetPlannedDimensions(item, item2);
					defaultFormationDeploymentPlan.SetPlannedTroopCount(num, num2);
				}
			}
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x0006B3CC File Offset: 0x000695CC
		private void DeployFlanks(Vec2 deployPosition, Vec2 deployDirection, float horizontalCenterOffset)
		{
			ValueTuple<float, float> valueTuple = this.PlanFlankDeployment(FormationDeploymentFlank.Front, deployPosition, deployDirection, 0f, horizontalCenterOffset);
			float item = valueTuple.Item1;
			float num = valueTuple.Item2;
			num += 3f;
			float item2 = this.PlanFlankDeployment(FormationDeploymentFlank.Rear, deployPosition, deployDirection, num, horizontalCenterOffset).Item1;
			float num2 = MathF.Max(item, item2);
			float num3 = this.ComputeFlankDepth(FormationDeploymentFlank.Front, true);
			num3 += 3f;
			float num4 = this.ComputeFlankWidth(FormationDeploymentFlank.Left);
			float num5 = horizontalCenterOffset + 2f + 0.5f * (num2 + num4);
			this.PlanFlankDeployment(FormationDeploymentFlank.Left, deployPosition, deployDirection, num3, num5);
			float num6 = this.ComputeFlankWidth(FormationDeploymentFlank.Right);
			float num7 = horizontalCenterOffset - (2f + 0.5f * (num2 + num6));
			this.PlanFlankDeployment(FormationDeploymentFlank.Right, deployPosition, deployDirection, num3, num7);
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x0006B488 File Offset: 0x00069688
		private void ComputeMeanPosition()
		{
			this._meanPosition = Vec3.Zero;
			Vec2 vec = Vec2.Zero;
			int num = 0;
			foreach (DefaultFormationDeploymentPlan defaultFormationDeploymentPlan in this._formationPlans)
			{
				if (defaultFormationDeploymentPlan.HasFrame())
				{
					vec += defaultFormationDeploymentPlan.GetPosition().AsVec2;
					num++;
				}
			}
			if (num > 0)
			{
				vec = new Vec2(vec.X / (float)num, vec.Y / (float)num);
				float num2 = 0f;
				Mission.Current.Scene.GetHeightAtPoint(vec, BodyFlags.None, ref num2);
				this._meanPosition = new Vec3(vec, num2, -1f);
			}
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x0006B534 File Offset: 0x00069734
		[return: TupleElementNames(new string[] { "flankWidth", "flankDepth" })]
		private ValueTuple<float, float> PlanFlankDeployment(FormationDeploymentFlank flankFlank, Vec2 deployPosition, Vec2 deployDirection, float verticalOffset = 0f, float horizontalOffset = 0f)
		{
			Mat3 identity = Mat3.Identity;
			identity.RotateAboutUp(deployDirection.RotationInRadians);
			float num = 0f;
			float num2 = 0f;
			Vec2 vec = deployDirection.LeftVec();
			WorldPosition worldPosition = new WorldPosition(this._mission.Scene, UIntPtr.Zero, deployPosition.ToVec3(0f), false);
			foreach (KeyValuePair<FormationDeploymentOrder, DefaultFormationDeploymentPlan> keyValuePair in this._deploymentFlanks[(int)flankFlank])
			{
				DefaultFormationDeploymentPlan value = keyValuePair.Value;
				Vec2 vec2 = worldPosition.AsVec2 - (num2 + verticalOffset) * deployDirection + horizontalOffset * vec;
				Vec3 lastPointOnNavigationMeshFromWorldPositionToDestination = this._mission.Scene.GetLastPointOnNavigationMeshFromWorldPositionToDestination(ref worldPosition, vec2);
				WorldPosition worldPosition2 = new WorldPosition(this._mission.Scene, UIntPtr.Zero, lastPointOnNavigationMeshFromWorldPositionToDestination, false);
				WorldFrame worldFrame = new WorldFrame(identity, worldPosition2);
				value.SetFrame(in worldFrame);
				float num3 = value.PlannedDepth + 3f;
				num2 += num3;
				num = MathF.Max(num, value.PlannedWidth);
			}
			num2 = MathF.Max(num2 - 3f, 0f);
			return new ValueTuple<float, float>(num, num2);
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x0006B680 File Offset: 0x00069880
		private WorldFrame ComputeFieldBattleDeploymentFrameForFormation(DefaultFormationDeploymentPlan formationPlan, GameEntity formationSceneEntity, GameEntity counterSideFormationSceneEntity, ref Dictionary<GameEntity, float> spawnDepths)
		{
			Vec3 globalPosition = formationSceneEntity.GlobalPosition;
			Vec2 asVec = (counterSideFormationSceneEntity.GlobalPosition - globalPosition).AsVec2;
			asVec.Normalize();
			float andUpdateSpawnDepth = this.GetAndUpdateSpawnDepth(ref spawnDepths, formationSceneEntity, formationPlan);
			WorldPosition worldPosition = new WorldPosition(this._mission.Scene, UIntPtr.Zero, globalPosition, false);
			worldPosition.SetVec2(worldPosition.AsVec2 - andUpdateSpawnDepth * asVec);
			Mat3 identity = Mat3.Identity;
			identity.RotateAboutUp(asVec.RotationInRadians);
			return new WorldFrame(identity, worldPosition);
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x0006B710 File Offset: 0x00069910
		private float ComputeFlankWidth(FormationDeploymentFlank flank)
		{
			float num = 0f;
			foreach (KeyValuePair<FormationDeploymentOrder, DefaultFormationDeploymentPlan> keyValuePair in this._deploymentFlanks[(int)flank])
			{
				num = MathF.Max(num, keyValuePair.Value.PlannedWidth);
			}
			return num;
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x0006B774 File Offset: 0x00069974
		private float ComputeFlankDepth(FormationDeploymentFlank flank, bool countPositiveNumTroops = false)
		{
			float num = 0f;
			foreach (KeyValuePair<FormationDeploymentOrder, DefaultFormationDeploymentPlan> keyValuePair in this._deploymentFlanks[(int)flank])
			{
				if (!countPositiveNumTroops)
				{
					num += keyValuePair.Value.PlannedDepth + 3f;
				}
				else if (keyValuePair.Value.PlannedTroopCount > 0)
				{
					num += keyValuePair.Value.PlannedDepth + 3f;
				}
			}
			num -= 3f;
			return num;
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x0006B80C File Offset: 0x00069A0C
		private float ComputeHorizontalCenterOffset()
		{
			float num = MathF.Max(this.ComputeFlankWidth(FormationDeploymentFlank.Front), this.ComputeFlankWidth(FormationDeploymentFlank.Rear));
			float num2 = this.ComputeFlankWidth(FormationDeploymentFlank.Left);
			float num3 = this.ComputeFlankWidth(FormationDeploymentFlank.Right);
			float num4 = num / 2f + num2 + 2f;
			return (num / 2f + num3 + 2f - num4) / 2f;
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x0006B864 File Offset: 0x00069A64
		private float GetAndUpdateSpawnDepth(ref Dictionary<GameEntity, float> spawnDepths, GameEntity spawnEntity, DefaultFormationDeploymentPlan formationPlan)
		{
			float num;
			bool flag = spawnDepths.TryGetValue(spawnEntity, out num);
			float num2 = (formationPlan.HasDimensions ? (formationPlan.PlannedDepth + 3f) : 0f);
			if (!flag)
			{
				num = 0f;
				spawnDepths[spawnEntity] = num2;
			}
			else if (formationPlan.HasDimensions)
			{
				spawnDepths[spawnEntity] = num + num2;
			}
			return num;
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x0006B8C0 File Offset: 0x00069AC0
		public static ValueTuple<float, float> GetFormationSpawnWidthAndDepth(FormationClass formationNo, int troopCount, bool hasMountedTroops, bool considerCavalryAsInfantry = false)
		{
			bool flag = !considerCavalryAsInfantry && hasMountedTroops;
			float defaultUnitDiameter = Formation.GetDefaultUnitDiameter(flag);
			int unitSpacingOf = ArrangementOrder.GetUnitSpacingOf(ArrangementOrder.ArrangementOrderEnum.Line);
			float num = (flag ? Formation.CavalryInterval(unitSpacingOf) : Formation.InfantryInterval(unitSpacingOf));
			float num2 = (flag ? Formation.CavalryDistance(unitSpacingOf) : Formation.InfantryDistance(unitSpacingOf));
			float num3 = (float)MathF.Max(0, troopCount - 1) * (num + defaultUnitDiameter) + defaultUnitDiameter;
			float num4 = (flag ? 18f : 9f);
			int num5 = (int)(num3 / MathF.Sqrt(num4 * (float)troopCount + 1f));
			num5 = MathF.Max(1, num5);
			float num6 = (float)troopCount / (float)num5;
			float num7 = MathF.Max(0f, num6 - 1f) * (num + defaultUnitDiameter) + defaultUnitDiameter;
			float num8 = (float)(num5 - 1) * (num2 + defaultUnitDiameter) + defaultUnitDiameter;
			return new ValueTuple<float, float>(num7, num8);
		}

		// Token: 0x04000A90 RID: 2704
		public const float VerticalFormationGap = 3f;

		// Token: 0x04000A91 RID: 2705
		public const float HorizontalFormationGap = 2f;

		// Token: 0x04000A92 RID: 2706
		public const float MaxSafetyScore = 100f;

		// Token: 0x04000A93 RID: 2707
		public readonly Team Team;

		// Token: 0x04000A94 RID: 2708
		public readonly bool IsReinforcement;

		// Token: 0x04000A95 RID: 2709
		public readonly SpawnPathData SpawnPathData;

		// Token: 0x04000A96 RID: 2710
		public readonly float SpawnPathReinforcementOffset;

		// Token: 0x04000A9B RID: 2715
		private readonly Mission _mission;

		// Token: 0x04000A9C RID: 2716
		private int _planCount;

		// Token: 0x04000A9D RID: 2717
		private bool _spawnWithHorses;

		// Token: 0x04000A9E RID: 2718
		private readonly int[] _formationMountedTroopCounts;

		// Token: 0x04000A9F RID: 2719
		private readonly int[] _formationFootTroopCounts;

		// Token: 0x04000AA0 RID: 2720
		private Vec3 _meanPosition;

		// Token: 0x04000AA1 RID: 2721
		private readonly DefaultFormationDeploymentPlan[] _formationPlans;

		// Token: 0x04000AA2 RID: 2722
		private readonly SortedList<FormationDeploymentOrder, DefaultFormationDeploymentPlan>[] _deploymentFlanks = new SortedList<FormationDeploymentOrder, DefaultFormationDeploymentPlan>[4];
	}
}
