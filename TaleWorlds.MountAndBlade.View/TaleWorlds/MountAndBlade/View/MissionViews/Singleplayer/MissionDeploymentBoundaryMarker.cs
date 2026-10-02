using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View.Tableaus.Thumbnails;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer
{
	// Token: 0x02000093 RID: 147
	public class MissionDeploymentBoundaryMarker : MissionView
	{
		// Token: 0x06000567 RID: 1383 RVA: 0x00027797 File Offset: 0x00025997
		public MissionDeploymentBoundaryMarker(string prefabName, float markerInterval = 2f)
		{
			this._prefabName = prefabName;
			this.MarkerInterval = Math.Max(markerInterval, 0.0001f);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000277CC File Offset: 0x000259CC
		public override void AfterStart()
		{
			base.AfterStart();
			for (int i = 0; i < 2; i++)
			{
				this._boundaryMarkersPerSide[i] = new Dictionary<string, List<GameEntity>>();
			}
			this._boundaryMarkersRemoved = false;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x000277FF File Offset: 0x000259FF
		protected override void OnEndMission()
		{
			base.OnEndMission();
			this.TryRemoveBoundaryMarkers();
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00027810 File Offset: 0x00025A10
		public override void OnDeploymentPlanMade(Team team, bool isFirstPlan)
		{
			if (team.IsPlayerTeam || team == base.Mission.PlayerEnemyTeam)
			{
				bool flag = base.Mission.DeploymentPlan.HasDeploymentBoundaries(team);
				if (isFirstPlan && flag)
				{
					foreach (ValueTuple<string, MBList<Vec2>> valueTuple in base.Mission.DeploymentPlan.GetDeploymentBoundaries(team))
					{
						this.AddBoundaryMarkerForSide(team.Side, new KeyValuePair<string, ICollection<Vec2>>(valueTuple.Item1, valueTuple.Item2));
					}
				}
			}
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x000278B4 File Offset: 0x00025AB4
		public override void OnRemoveBehavior()
		{
			this.TryRemoveBoundaryMarkers();
			base.OnRemoveBehavior();
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x000278C4 File Offset: 0x00025AC4
		private void AddBoundaryMarkerForSide(BattleSideEnum side, KeyValuePair<string, ICollection<Vec2>> boundary)
		{
			string key = boundary.Key;
			if (!this._boundaryMarkersPerSide[(int)side].ContainsKey(key))
			{
				Banner banner = ((side == BattleSideEnum.Attacker) ? base.Mission.AttackerTeam.Banner : ((side == BattleSideEnum.Defender) ? base.Mission.DefenderTeam.Banner : null));
				List<GameEntity> list = new List<GameEntity>();
				List<Vec2> list2 = boundary.Value.ToList<Vec2>();
				for (int i = 0; i < list2.Count; i++)
				{
					this.MarkLine(new Vec3(list2[i], 0f, -1f), new Vec3(list2[(i + 1) % list2.Count], 0f, -1f), list, banner);
				}
				this._boundaryMarkersPerSide[(int)side][key] = list;
			}
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00027998 File Offset: 0x00025B98
		private void TryRemoveBoundaryMarkers()
		{
			if (!this._boundaryMarkersRemoved)
			{
				for (int i = 0; i < 2; i++)
				{
					foreach (string text in this._boundaryMarkersPerSide[i].Keys.ToList<string>())
					{
						this.RemoveBoundaryMarker(text, (BattleSideEnum)i);
					}
				}
				this._boundaryMarkersRemoved = true;
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x00027A14 File Offset: 0x00025C14
		private void RemoveBoundaryMarker(string boundaryName, BattleSideEnum side)
		{
			List<GameEntity> list;
			if (this._boundaryMarkersPerSide[(int)side].TryGetValue(boundaryName, out list))
			{
				foreach (GameEntity gameEntity in list)
				{
					gameEntity.Remove(103);
				}
				this._boundaryMarkersPerSide[(int)side].Remove(boundaryName);
			}
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x00027A84 File Offset: 0x00025C84
		protected virtual void MarkLine(Vec3 startPoint, Vec3 endPoint, List<GameEntity> boundary, Banner banner = null)
		{
			Scene scene = base.Mission.Scene;
			Vec3 vec = endPoint - startPoint;
			float length = vec.Length;
			Vec3 vec2 = vec;
			vec2.Normalize();
			vec2 *= this.MarkerInterval;
			for (float num = 0f; num < length; num += this.MarkerInterval)
			{
				MatrixFrame identity = MatrixFrame.Identity;
				identity.rotation.RotateAboutUp(vec.RotationZ + 1.5707964f);
				identity.origin = startPoint;
				if (!scene.GetHeightAtPoint(identity.origin.AsVec2, BodyFlags.CommonCollisionExcludeFlagsForCombat, ref identity.origin.z))
				{
					identity.origin.z = 0f;
				}
				identity.origin.z = identity.origin.z - 0.5f;
				Vec3 vec3 = Vec3.One * 0.4f;
				identity.Scale(in vec3);
				GameEntity gameEntity = this.MakeEntity(banner);
				gameEntity.SetFrame(ref identity, true);
				boundary.Add(gameEntity);
				startPoint += vec2;
			}
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00027B98 File Offset: 0x00025D98
		private GameEntity MakeEntity(Banner banner = null)
		{
			Scene scene = base.Mission.Scene;
			if (this._cachedEntity == null)
			{
				this._cachedEntity = GameEntity.Instantiate(null, this._prefabName, false, true, "");
			}
			GameEntity gameEntity = GameEntity.CopyFrom(scene, this._cachedEntity, true, true);
			gameEntity.SetMobility(GameEntity.Mobility.Dynamic);
			if (banner != null)
			{
				Mesh firstMesh = gameEntity.GetFirstMesh();
				Material material = firstMesh.GetMaterial();
				Material tableauMaterial = material.CreateCopy();
				BannerDebugInfo bannerDebugInfo = BannerDebugInfo.CreateManual(base.GetType().Name);
				banner.GetTableauTextureSmall(in bannerDebugInfo, delegate(Texture tex)
				{
					tableauMaterial.SetTexture(Material.MBTextureType.DiffuseMap2, tex);
				});
				firstMesh.SetMaterial(tableauMaterial);
			}
			return gameEntity;
		}

		// Token: 0x04000301 RID: 769
		public const string AttackerStaticDeploymentBoundaryName = "walk_area";

		// Token: 0x04000302 RID: 770
		public const string DefenderStaticDeploymentBoundaryName = "deployment_castle_boundary";

		// Token: 0x04000303 RID: 771
		public readonly float MarkerInterval;

		// Token: 0x04000304 RID: 772
		protected readonly Dictionary<string, List<GameEntity>>[] _boundaryMarkersPerSide = new Dictionary<string, List<GameEntity>>[2];

		// Token: 0x04000305 RID: 773
		protected readonly string _prefabName;

		// Token: 0x04000306 RID: 774
		protected GameEntity _cachedEntity;

		// Token: 0x04000307 RID: 775
		protected bool _boundaryMarkersRemoved = true;
	}
}
