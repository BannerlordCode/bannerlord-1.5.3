using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer
{
	// Token: 0x0200008E RID: 142
	public class FormationIndicatorMissionView : MissionView
	{
		// Token: 0x0600055A RID: 1370 RVA: 0x00027028 File Offset: 0x00025228
		public override void AfterStart()
		{
			base.AfterStart();
			this.mission = Mission.Current;
			this._indicators = new FormationIndicatorMissionView.Indicator[this.mission.Teams.Count, 9];
			for (int i = 0; i < this.mission.Teams.Count; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					this._indicators[i, j] = new FormationIndicatorMissionView.Indicator
					{
						missionScreen = base.MissionScreen
					};
				}
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x000270AC File Offset: 0x000252AC
		private GameEntity CreateBannerEntity(Formation formation)
		{
			GameEntity gameEntity = GameEntity.CreateEmpty(this.mission.Scene, true, true, true);
			gameEntity.EntityFlags |= EntityFlags.NoOcclusionCulling;
			uint num = 4278190080U;
			uint num2;
			if (formation.Team.IsPlayerAlly)
			{
				num2 = 2130747904U;
			}
			else
			{
				num2 = 2144798212U;
			}
			gameEntity.AddMultiMesh(MetaMesh.GetCopy("billboard_unit_mesh", true, false), true);
			gameEntity.GetFirstMesh().Color = uint.MaxValue;
			Material formationMaterial = Material.GetFromResource("formation_icon").CreateCopy();
			if (formation.Team != null)
			{
				Banner banner = ((formation.Captain != null) ? formation.Captain.Origin.Banner : formation.Team.Banner);
				if (banner != null)
				{
					Action<Texture> action = delegate(Texture tex)
					{
						formationMaterial.SetTexture(Material.MBTextureType.DiffuseMap2, tex);
					};
					Banner banner2 = banner;
					BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
					banner2.GetTableauTextureLarge(in bannerDebugInfo, action);
				}
				else
				{
					Texture fromResource = Texture.GetFromResource("plain_red");
					formationMaterial.SetTexture(Material.MBTextureType.DiffuseMap2, fromResource);
				}
			}
			else
			{
				Texture fromResource2 = Texture.GetFromResource("plain_red");
				formationMaterial.SetTexture(Material.MBTextureType.DiffuseMap2, fromResource2);
			}
			int num3 = (int)(formation.FormationIndex % FormationClass.NumberOfDefaultFormations);
			gameEntity.GetFirstMesh().SetMaterial(formationMaterial);
			gameEntity.GetFirstMesh().Color = num2;
			gameEntity.GetFirstMesh().Color2 = num;
			gameEntity.GetFirstMesh().SetVectorArgument(0f, 1f, (float)num3, 1f);
			return gameEntity;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x00027220 File Offset: 0x00025420
		private int GetFormationTeamIndex(Formation formation)
		{
			int num;
			if (this.mission.Teams.Count > 2 && (formation.Team == this.mission.AttackerAllyTeam || formation.Team == this.mission.DefenderAllyTeam))
			{
				num = ((this.mission.Teams.Count == 3) ? 2 : ((formation.Team == this.mission.DefenderAllyTeam) ? 2 : 3));
			}
			else
			{
				num = (int)formation.Team.Side;
			}
			return num;
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x000272A4 File Offset: 0x000254A4
		public override void OnMissionScreenTick(float dt)
		{
			this.OnMissionTick(dt);
			bool flag;
			if (base.Input.IsGameKeyDown(5))
			{
				flag = false;
				this._isEnabled = false;
			}
			else
			{
				flag = false;
			}
			IEnumerable<Formation> enumerable = this.mission.Teams.SelectMany<Team, Formation>((Team t) => t.FormationsIncludingEmpty);
			if (flag)
			{
				IEnumerable<Formation> enumerable2 = this.mission.Teams.SelectMany<Team, Formation>((Team t) => t.FormationsIncludingEmpty.Where<Formation>((Formation f) => f.CountOfUnits > 0));
				foreach (Formation formation in enumerable2)
				{
					int formationTeamIndex = this.GetFormationTeamIndex(formation);
					FormationIndicatorMissionView.Indicator indicator = this._indicators[formationTeamIndex, (int)formation.FormationIndex];
					indicator.DetermineIndicatorState(dt, formation.CachedMedianPosition.GetGroundVec3());
					if (indicator.indicatorEntity == null)
					{
						if (indicator.indicatorVisible)
						{
							GameEntity gameEntity = this.CreateBannerEntity(formation);
							gameEntity.SetFrame(ref indicator.indicatorFrame, true);
							gameEntity.SetVisibilityExcludeParents(true);
							this._indicators[formationTeamIndex, (int)formation.FormationIndex].indicatorEntity = gameEntity;
							gameEntity.SetAlpha(0f);
							this._indicators[formationTeamIndex, (int)formation.FormationIndex].indicatorAlpha = 0f;
						}
					}
					else
					{
						if (indicator.indicatorEntity.IsVisibleIncludeParents() != indicator.indicatorVisible)
						{
							if (!indicator.indicatorVisible)
							{
								if (indicator.indicatorAlpha > 0f)
								{
									indicator.indicatorAlpha -= 0.01f;
									if (indicator.indicatorAlpha < 0f)
									{
										indicator.indicatorAlpha = 0f;
									}
									indicator.indicatorEntity.SetAlpha(indicator.indicatorAlpha);
								}
								else
								{
									indicator.indicatorEntity.SetVisibilityExcludeParents(indicator.indicatorVisible);
								}
							}
							else
							{
								indicator.indicatorEntity.SetVisibilityExcludeParents(indicator.indicatorVisible);
							}
						}
						if (indicator.indicatorVisible && indicator.indicatorAlpha < 1f)
						{
							indicator.indicatorAlpha += 0.01f;
							if (indicator.indicatorAlpha > 1f)
							{
								indicator.indicatorAlpha = 1f;
							}
							indicator.indicatorEntity.SetAlpha(indicator.indicatorAlpha);
						}
						if (!indicator._isMovingTooFast && indicator.indicatorEntity.IsVisibleIncludeParents())
						{
							indicator.indicatorEntity.SetFrame(ref indicator.indicatorFrame, true);
						}
					}
				}
				using (IEnumerator<Formation> enumerator = enumerable.Except<Formation>(enumerable2).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Formation formation2 = enumerator.Current;
						if (formation2.FormationIndex >= FormationClass.NumberOfRegularFormations)
						{
							break;
						}
						FormationIndicatorMissionView.Indicator indicator2 = this._indicators[this.GetFormationTeamIndex(formation2), (int)formation2.FormationIndex];
						if (indicator2.indicatorEntity != null && indicator2.indicatorEntity.IsVisibleIncludeParents())
						{
							indicator2.indicatorEntity.SetVisibilityExcludeParents(false);
							indicator2.indicatorVisible = false;
						}
					}
					return;
				}
			}
			if (this._isEnabled)
			{
				foreach (Formation formation3 in enumerable)
				{
					int formationTeamIndex2 = this.GetFormationTeamIndex(formation3);
					FormationIndicatorMissionView.Indicator indicator3 = this._indicators[formationTeamIndex2, (int)formation3.FormationIndex];
					if (indicator3 != null && indicator3.indicatorEntity != null)
					{
						indicator3.indicatorAlpha = 0f;
						indicator3.indicatorEntity.SetVisibilityExcludeParents(false);
					}
				}
				this._isEnabled = false;
			}
		}

		// Token: 0x040002FC RID: 764
		private FormationIndicatorMissionView.Indicator[,] _indicators;

		// Token: 0x040002FD RID: 765
		private Mission mission;

		// Token: 0x040002FE RID: 766
		private bool _isEnabled;

		// Token: 0x020000E9 RID: 233
		public class Indicator
		{
			// Token: 0x0600067E RID: 1662 RVA: 0x0002B7B8 File Offset: 0x000299B8
			private Vec3? GetCurrentPosition()
			{
				if (Mission.Current.MainAgent != null)
				{
					return new Vec3?(Mission.Current.MainAgent.AgentVisuals.GetGlobalFrame().origin + new Vec3(0f, 0f, 1f, -1f));
				}
				if (this.missionScreen.CombatCamera != null)
				{
					return new Vec3?(this.missionScreen.CombatCamera.Position);
				}
				return null;
			}

			// Token: 0x0600067F RID: 1663 RVA: 0x0002B840 File Offset: 0x00029A40
			public void DetermineIndicatorState(float dt, Vec3 position)
			{
				Mission mission = Mission.Current;
				Vec3? currentPosition = this.GetCurrentPosition();
				if (currentPosition == null)
				{
					this.indicatorVisible = false;
					return;
				}
				if (this.firstTime)
				{
					this.prevIndicatorPosition = position;
					this.nextIndicatorPosition = position;
					this.firstTime = false;
				}
				Vec3 vec;
				if (this.nextIndicatorPosition.Distance(this.prevIndicatorPosition) / 0.5f > 30f)
				{
					vec = position;
					this._isMovingTooFast = true;
				}
				else
				{
					vec = Vec3.Lerp(this.prevIndicatorPosition, this.nextIndicatorPosition, MBMath.ClampFloat(this._drawIndicatorElapsedTime / 0.5f, 0f, 1f));
				}
				float num = currentPosition.Value.Distance(vec);
				if (this._drawIndicatorElapsedTime < 0.5f)
				{
					this._drawIndicatorElapsedTime += dt;
				}
				else
				{
					this.prevIndicatorPosition = this.nextIndicatorPosition;
					this.nextIndicatorPosition = position;
					this._isSeenByPlayer = num < 60f && (num < 15f || mission.Scene.CheckPointCanSeePoint(currentPosition.Value, position, null) || mission.Scene.CheckPointCanSeePoint(currentPosition.Value + new Vec3(0f, 0f, 2f, -1f), position + new Vec3(0f, 0f, 2f, -1f), null));
					this._drawIndicatorElapsedTime = 0f;
				}
				if (this._isSeenByPlayer)
				{
					this.indicatorVisible = false;
					if (!this._isMovingTooFast && this.indicatorEntity != null && this.indicatorEntity.IsVisibleIncludeParents())
					{
						float num2 = MBMath.ClampFloat(num * 0.02f, 1f, 10f) * 3f;
						MatrixFrame identity = MatrixFrame.Identity;
						identity.origin = vec;
						identity.origin.z = identity.origin.z + num2 * 0.75f;
						identity.rotation.ApplyScaleLocal(num2);
						this.indicatorFrame = identity;
					}
					return;
				}
				float num3 = MBMath.ClampFloat(num * 0.02f, 1f, 10f) * 3f;
				MatrixFrame identity2 = MatrixFrame.Identity;
				identity2.origin = vec;
				identity2.origin.z = identity2.origin.z + num3 * 0.75f;
				identity2.rotation.ApplyScaleLocal(num3);
				this.indicatorFrame = identity2;
				if (!this._isMovingTooFast)
				{
					this.indicatorVisible = true;
					return;
				}
				if (this.indicatorEntity != null && this.indicatorEntity.IsVisibleIncludeParents())
				{
					this.indicatorVisible = false;
					return;
				}
				this._isMovingTooFast = false;
				this.indicatorVisible = true;
			}

			// Token: 0x0400040D RID: 1037
			public MissionScreen missionScreen;

			// Token: 0x0400040E RID: 1038
			public bool indicatorVisible;

			// Token: 0x0400040F RID: 1039
			public MatrixFrame indicatorFrame;

			// Token: 0x04000410 RID: 1040
			public bool firstTime = true;

			// Token: 0x04000411 RID: 1041
			public GameEntity indicatorEntity;

			// Token: 0x04000412 RID: 1042
			public Vec3 nextIndicatorPosition;

			// Token: 0x04000413 RID: 1043
			public Vec3 prevIndicatorPosition;

			// Token: 0x04000414 RID: 1044
			public float indicatorAlpha = 1f;

			// Token: 0x04000415 RID: 1045
			private float _drawIndicatorElapsedTime;

			// Token: 0x04000416 RID: 1046
			private const float IndicatorExpireTime = 0.5f;

			// Token: 0x04000417 RID: 1047
			private bool _isSeenByPlayer = true;

			// Token: 0x04000418 RID: 1048
			internal bool _isMovingTooFast;
		}
	}
}
