using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews
{
	// Token: 0x02000073 RID: 115
	public class MissionFormationTargetSelectionHandler : MissionView
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600045A RID: 1114 RVA: 0x000206CC File Offset: 0x0001E8CC
		// (remove) Token: 0x0600045B RID: 1115 RVA: 0x00020704 File Offset: 0x0001E904
		public event Action<Formation> OnFormationFocused;

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x00020739 File Offset: 0x0001E939
		public static float MinDistanceForFocusCheck
		{
			get
			{
				if (!GameNetwork.IsMultiplayer)
				{
					return 8f;
				}
				return 4.8f;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0002074D File Offset: 0x0001E94D
		private Camera ActiveCamera
		{
			get
			{
				return base.MissionScreen.CustomCamera ?? base.MissionScreen.CombatCamera;
			}
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0002076C File Offset: 0x0001E96C
		public MissionFormationTargetSelectionHandler()
		{
			this._visibilityResultCache = new Dictionary<Formation, ValueTuple<bool, float, float>>();
			this._markerVisibilityCache = new Dictionary<Formation, bool>();
			this._expiredVisibilityKeys = new List<Formation>();
			this._visibilityConfig = new MissionFormationTargetSelectionHandler.VisibilityConfig
			{
				Mode = MultiplayerOptions.FormationTargetingVisibilityModes.Disabled,
				Threshold = 0,
				AppliesAtCloseRange = false
			};
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x0002080A File Offset: 0x0001EA0A
		public void SetVisibilityConfig(MissionFormationTargetSelectionHandler.VisibilityConfig config)
		{
			this._visibilityConfig = config;
			this._visibilityResultCache.Clear();
			this._markerVisibilityCache.Clear();
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x0002082C File Offset: 0x0001EA2C
		public override void OnPreDisplayMissionTick(float dt)
		{
			base.OnPreDisplayMissionTick(dt);
			this._distanceCache.Clear();
			this._focusedFormation = null;
			this._markerVisibilityCache.Clear();
			this._elapsedTime += dt;
			this.PruneExpiredVisibilityResults();
			Mission mission = base.Mission;
			if (((mission != null) ? mission.Teams : null) != null)
			{
				if (!this._isTargetingDisabled)
				{
					Vec3 position = this.ActiveCamera.Position;
					this._centerOfScreen.x = Screen.RealScreenResolutionWidth / 2f;
					this._centerOfScreen.y = Screen.RealScreenResolutionHeight / 2f;
					bool flag = this._visibilityConfig.Mode > MultiplayerOptions.FormationTargetingVisibilityModes.Disabled;
					MatrixFrame identity = MatrixFrame.Identity;
					Vec3 vec = Vec3.Zero;
					if (flag)
					{
						this.ActiveCamera.GetViewProjMatrix(ref identity);
						vec = this.ActiveCamera.Position;
					}
					for (int i = 0; i < base.Mission.Teams.Count; i++)
					{
						Team team = base.Mission.Teams[i];
						if (!team.IsPlayerAlly)
						{
							for (int j = 0; j < team.FormationsIncludingEmpty.Count; j++)
							{
								Formation formation = team.FormationsIncludingEmpty[j];
								if (formation.CountOfUnits > 0)
								{
									bool flag2;
									float num;
									this.TryGetFormationDistanceToCenter(formation, position, out flag2, out num);
									if (flag2)
									{
										this._distanceCache.Add(new ValueTuple<Formation, float>(formation, num));
									}
									if (flag)
									{
										bool flag3 = formation.CachedMedianPosition.AsVec2.Distance(position.AsVec2) < 1000f;
										this._markerVisibilityCache[formation] = flag3 && this.IsFormationVisibleEnough(formation, ref identity, vec);
									}
								}
							}
						}
					}
				}
				if (this._distanceCache.Count == 0)
				{
					Action<Formation> onFormationFocused = this.OnFormationFocused;
					if (onFormationFocused == null)
					{
						return;
					}
					onFormationFocused(null);
					return;
				}
				else
				{
					this._distanceCache.Sort(new Comparison<ValueTuple<Formation, float>>(MissionFormationTargetSelectionHandler.CompareByDistanceToScreenCenter));
					MatrixFrame identity2 = MatrixFrame.Identity;
					Vec3 vec2 = Vec3.Zero;
					bool flag4 = this._visibilityConfig.Mode > MultiplayerOptions.FormationTargetingVisibilityModes.Disabled;
					if (flag4)
					{
						this.ActiveCamera.GetViewProjMatrix(ref identity2);
						vec2 = this.ActiveCamera.Position;
					}
					for (int k = 0; k < this._distanceCache.Count; k++)
					{
						ValueTuple<Formation, float> valueTuple = this._distanceCache[k];
						Formation item = valueTuple.Item1;
						if (valueTuple.Item2 >= this.MaxDistanceToCenterForFocus)
						{
							break;
						}
						if (!flag4 || this.IsFormationVisibleEnough(item, ref identity2, vec2))
						{
							this._focusedFormation = item;
							break;
						}
					}
					Action<Formation> onFormationFocused2 = this.OnFormationFocused;
					if (onFormationFocused2 == null)
					{
						return;
					}
					onFormationFocused2(this._focusedFormation);
				}
			}
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00020AD2 File Offset: 0x0001ECD2
		private static int CompareByDistanceToScreenCenter(ValueTuple<Formation, float> a, ValueTuple<Formation, float> b)
		{
			return a.Item2.CompareTo(b.Item2);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00020AE8 File Offset: 0x0001ECE8
		private void TryGetFormationDistanceToCenter(Formation formation, Vec3 cameraPosition, out bool isFormationFocusable, out float distanceToScreenCenter)
		{
			WorldPosition cachedMedianPosition = formation.CachedMedianPosition;
			float num = cachedMedianPosition.AsVec2.Distance(cameraPosition.AsVec2);
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			MBWindowManager.WorldToScreenInsideUsableArea(this.ActiveCamera, cachedMedianPosition.GetGroundVec3() + new Vec3(0f, 0f, 3f, -1f), ref num2, ref num3, ref num4);
			bool flag = num4 <= 0f;
			if (num >= 1000f || num <= MissionFormationTargetSelectionHandler.MinDistanceForFocusCheck || flag)
			{
				distanceToScreenCenter = 2.1474836E+09f;
				isFormationFocusable = false;
				return;
			}
			isFormationFocusable = true;
			distanceToScreenCenter = new Vec2(num2, num3).Distance(this._centerOfScreen);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x00020BB0 File Offset: 0x0001EDB0
		private bool IsFormationVisibleEnough(Formation formation, ref MatrixFrame viewProjection, Vec3 cameraOrigin)
		{
			ValueTuple<bool, float, float> valueTuple;
			if (this._visibilityResultCache.TryGetValue(formation, out valueTuple) && this._elapsedTime - valueTuple.Item3 < 0.2f)
			{
				return valueTuple.Item1;
			}
			float num;
			bool flag = this.ComputeFormationVisibility(formation, ref viewProjection, cameraOrigin, out num);
			this._visibilityResultCache[formation] = new ValueTuple<bool, float, float>(flag, num, this._elapsedTime);
			return flag;
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00020C10 File Offset: 0x0001EE10
		public float GetFormationVisibilityRatio(Formation formation)
		{
			ValueTuple<bool, float, float> valueTuple;
			if (this._visibilityResultCache.TryGetValue(formation, out valueTuple) && this._elapsedTime - valueTuple.Item3 < 0.2f)
			{
				return valueTuple.Item2;
			}
			return 1f;
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00020C50 File Offset: 0x0001EE50
		public MissionFormationTargetSelectionHandler.FormationMarkerVisibility GetFormationMarkerVisibility(Formation formation)
		{
			if (this._isTargetingDisabled || this._visibilityConfig.Mode == MultiplayerOptions.FormationTargetingVisibilityModes.Disabled)
			{
				return MissionFormationTargetSelectionHandler.FormationMarkerVisibility.NotEvaluated;
			}
			bool flag;
			if (!this._markerVisibilityCache.TryGetValue(formation, out flag))
			{
				return MissionFormationTargetSelectionHandler.FormationMarkerVisibility.NotEvaluated;
			}
			if (!flag)
			{
				return MissionFormationTargetSelectionHandler.FormationMarkerVisibility.Hidden;
			}
			return MissionFormationTargetSelectionHandler.FormationMarkerVisibility.Visible;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00020C8C File Offset: 0x0001EE8C
		private void PruneExpiredVisibilityResults()
		{
			if (this._visibilityResultCache.Count == 0)
			{
				return;
			}
			this._expiredVisibilityKeys.Clear();
			foreach (KeyValuePair<Formation, ValueTuple<bool, float, float>> keyValuePair in this._visibilityResultCache)
			{
				if (this._elapsedTime - keyValuePair.Value.Item3 >= 0.2f)
				{
					this._expiredVisibilityKeys.Add(keyValuePair.Key);
				}
			}
			for (int i = 0; i < this._expiredVisibilityKeys.Count; i++)
			{
				this._visibilityResultCache.Remove(this._expiredVisibilityKeys[i]);
			}
			this._expiredVisibilityKeys.Clear();
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00020D58 File Offset: 0x0001EF58
		private bool ComputeFormationVisibility(Formation formation, ref MatrixFrame viewProjection, Vec3 cameraOrigin, out float visibilityRatio)
		{
			visibilityRatio = 1f;
			int countOfUnits = formation.CountOfUnits;
			if (countOfUnits == 0)
			{
				visibilityRatio = 0f;
				return false;
			}
			MBReadOnlyList<IFormationUnit> unitsWithoutLooseDetachedOnes = formation.UnitsWithoutLooseDetachedOnes;
			MBReadOnlyList<Agent> detachedUnits = formation.DetachedUnits;
			int count = unitsWithoutLooseDetachedOnes.Count;
			int count2 = detachedUnits.Count;
			int num = count + count2;
			int num2 = ((num <= 24) ? 1 : ((num + 24 - 1) / 24));
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < num; i += num2)
			{
				Agent agent;
				if (i < count)
				{
					agent = (Agent)unitsWithoutLooseDetachedOnes[i];
				}
				else
				{
					agent = detachedUnits[i - count];
				}
				if (agent.IsActive())
				{
					Vec3 chestGlobalPosition = agent.GetChestGlobalPosition();
					Vec3 vec = chestGlobalPosition;
					vec.w = 1f;
					Vec3 vec2 = vec * viewProjection;
					if (vec2.w <= 0f)
					{
						num3++;
					}
					else
					{
						float num5 = vec2.x / vec2.w;
						float num6 = vec2.y / vec2.w;
						if (num5 < -1f || num5 > 1f || num6 < -1f || num6 > 1f)
						{
							num3++;
						}
						else if (!base.Mission.Scene.CheckPointCanSeePoint(cameraOrigin, chestGlobalPosition, null))
						{
							num3++;
						}
						else
						{
							num3++;
							num4++;
						}
					}
				}
			}
			if (num3 == 0)
			{
				visibilityRatio = 0f;
				return false;
			}
			visibilityRatio = (float)num4 / (float)num3;
			MultiplayerOptions.FormationTargetingVisibilityModes mode = this._visibilityConfig.Mode;
			bool flag;
			if (mode != MultiplayerOptions.FormationTargetingVisibilityModes.Percentage)
			{
				flag = mode != MultiplayerOptions.FormationTargetingVisibilityModes.AbsoluteCount || visibilityRatio * (float)countOfUnits >= (float)this._visibilityConfig.Threshold;
			}
			else
			{
				flag = visibilityRatio * 100f >= (float)this._visibilityConfig.Threshold;
			}
			return flag;
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00020F38 File Offset: 0x0001F138
		public void SetIsFormationTargetingDisabled(bool isDisabled)
		{
			if (this._isTargetingDisabled != isDisabled)
			{
				this._isTargetingDisabled = isDisabled;
				if (isDisabled)
				{
					this._distanceCache.Clear();
					this._focusedFormation = null;
					this._visibilityResultCache.Clear();
					this._markerVisibilityCache.Clear();
					Action<Formation> onFormationFocused = this.OnFormationFocused;
					if (onFormationFocused == null)
					{
						return;
					}
					onFormationFocused(null);
				}
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x00020F94 File Offset: 0x0001F194
		public override void OnRemoveBehavior()
		{
			this._distanceCache.Clear();
			this._focusedFormation = null;
			this._visibilityResultCache.Clear();
			this._markerVisibilityCache.Clear();
			this._expiredVisibilityKeys.Clear();
			this.OnFormationFocused = null;
			base.OnRemoveBehavior();
		}

		// Token: 0x04000284 RID: 644
		public const float MaxDistanceForFocusCheck = 1000f;

		// Token: 0x04000285 RID: 645
		public readonly float MaxDistanceToCenterForFocus = 70f * (Screen.RealScreenResolutionHeight / 1080f);

		// Token: 0x04000286 RID: 646
		private const int MaxSampledUnitsPerFormation = 24;

		// Token: 0x04000287 RID: 647
		private const float VisibilityResultCacheDuration = 0.2f;

		// Token: 0x04000288 RID: 648
		[TupleElementNames(new string[] { "Formation", "DistanceToScreenCenter" })]
		private readonly List<ValueTuple<Formation, float>> _distanceCache = new List<ValueTuple<Formation, float>>();

		// Token: 0x04000289 RID: 649
		private Formation _focusedFormation;

		// Token: 0x0400028A RID: 650
		private Vec2 _centerOfScreen = new Vec2(Screen.RealScreenResolutionWidth / 2f, Screen.RealScreenResolutionHeight / 2f);

		// Token: 0x0400028B RID: 651
		private MissionFormationTargetSelectionHandler.VisibilityConfig _visibilityConfig;

		// Token: 0x0400028C RID: 652
		[TupleElementNames(new string[] { "IsVisible", "VisibilityRatio", "ComputedAtTime" })]
		private readonly Dictionary<Formation, ValueTuple<bool, float, float>> _visibilityResultCache;

		// Token: 0x0400028D RID: 653
		private readonly Dictionary<Formation, bool> _markerVisibilityCache;

		// Token: 0x0400028E RID: 654
		private float _elapsedTime;

		// Token: 0x0400028F RID: 655
		private bool _isTargetingDisabled;

		// Token: 0x04000290 RID: 656
		private readonly List<Formation> _expiredVisibilityKeys;

		// Token: 0x020000DB RID: 219
		public enum FormationMarkerVisibility
		{
			// Token: 0x040003E7 RID: 999
			NotEvaluated,
			// Token: 0x040003E8 RID: 1000
			Hidden,
			// Token: 0x040003E9 RID: 1001
			Visible
		}

		// Token: 0x020000DC RID: 220
		public struct VisibilityConfig
		{
			// Token: 0x040003EA RID: 1002
			public MultiplayerOptions.FormationTargetingVisibilityModes Mode;

			// Token: 0x040003EB RID: 1003
			public int Threshold;

			// Token: 0x040003EC RID: 1004
			public bool AppliesAtCloseRange;
		}
	}
}
