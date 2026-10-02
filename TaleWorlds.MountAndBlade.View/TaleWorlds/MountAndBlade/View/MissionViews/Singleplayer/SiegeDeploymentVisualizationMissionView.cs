using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer
{
	// Token: 0x0200009F RID: 159
	public class SiegeDeploymentVisualizationMissionView : MissionView
	{
		// Token: 0x06000582 RID: 1410 RVA: 0x00027D6C File Offset: 0x00025F6C
		public override void AfterStart()
		{
			base.AfterStart();
			this._deploymentPoints = (from dp in Mission.Current.ActiveMissionObjects.FindAllWithType<DeploymentPoint>()
				where !dp.IsDisabled
				select dp).ToList<DeploymentPoint>();
			foreach (DeploymentPoint deploymentPoint in this._deploymentPoints)
			{
				deploymentPoint.OnDeploymentPointTypeDetermined += this.OnDeploymentPointStateSet;
				deploymentPoint.OnDeploymentStateChanged += this.OnDeploymentStateChanged;
			}
			this._deploymentPointsVisible = true;
			Mission.Current.GetMissionBehavior<SiegeDeploymentMissionController>();
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00027E30 File Offset: 0x00026030
		public override void OnDeploymentFinished()
		{
			base.OnDeploymentFinished();
			this.TryRemoveDeploymentVisibilities();
			Mission.Current.RemoveMissionBehavior(this);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00027E49 File Offset: 0x00026049
		protected override void OnEndMission()
		{
			base.OnEndMission();
			this.TryRemoveDeploymentVisibilities();
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00027E57 File Offset: 0x00026057
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00027E60 File Offset: 0x00026060
		private void TryRemoveDeploymentVisibilities()
		{
			if (this._deploymentPointsVisible)
			{
				foreach (DeploymentPoint deploymentPoint in this._deploymentPoints)
				{
					this.RemoveDeploymentVisibility(deploymentPoint);
					deploymentPoint.OnDeploymentStateChanged -= this.OnDeploymentStateChanged;
				}
				this._deploymentPointsVisible = false;
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x00027ED4 File Offset: 0x000260D4
		private void RemoveDeploymentVisibility(DeploymentPoint deploymentPoint)
		{
			switch (deploymentPoint.GetDeploymentPointType())
			{
			case DeploymentPoint.DeploymentPointType.BatteringRam:
				this.HideDeploymentBanners(deploymentPoint, true);
				this.SetGhostVisibility(deploymentPoint, false);
				return;
			case DeploymentPoint.DeploymentPointType.TowerLadder:
				this.HideDeploymentBanners(deploymentPoint, true);
				this.SetGhostVisibility(deploymentPoint, false);
				this.SetDeploymentTargetContourState(deploymentPoint, false);
				this.SetLaddersUpState(deploymentPoint, false);
				this.SetLightState(deploymentPoint, false);
				return;
			case DeploymentPoint.DeploymentPointType.Breach:
				this.HideDeploymentBanners(deploymentPoint, true);
				this.SetDeploymentTargetContourState(deploymentPoint, false);
				this.SetLightState(deploymentPoint, false);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00027F50 File Offset: 0x00026150
		private static string GetSelectorStateDescription()
		{
			string text = "";
			for (int i = 1; i < 1023; i *= 2)
			{
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & i) > 0)
				{
					string text2 = text;
					string text3 = " ";
					SiegeDeploymentVisualizationMissionView.DeploymentVisualizationPreference deploymentVisualizationPreference = (SiegeDeploymentVisualizationMissionView.DeploymentVisualizationPreference)i;
					text = text2 + text3 + deploymentVisualizationPreference.ToString();
				}
			}
			return text;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00027F99 File Offset: 0x00026199
		[CommandLineFunctionality.CommandLineArgumentFunction("set_deployment_visualization_selector", "mission")]
		public static string SetDeploymentVisualizationSelector(List<string> strings)
		{
			if (strings.Count == 1 && int.TryParse(strings[0], out SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector))
			{
				return "Enabled deployment visualization options are:" + SiegeDeploymentVisualizationMissionView.GetSelectorStateDescription();
			}
			return "Format is \"mission.set_deployment_visualization_selector [integer > 0]\".";
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x00027FCC File Offset: 0x000261CC
		private void OnDeploymentStateChanged(DeploymentPoint deploymentPoint, SynchedMissionObject targetObject)
		{
			this.OnDeploymentPointStateSet(deploymentPoint);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00027FD8 File Offset: 0x000261D8
		private void OnDeploymentPointStateSet(DeploymentPoint deploymentPoint)
		{
			switch (deploymentPoint.GetDeploymentPointState())
			{
			case DeploymentPoint.DeploymentPointState.NotDeployed:
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 1) > 0)
				{
					if (deploymentPoint.GetDeploymentPointType() == DeploymentPoint.DeploymentPointType.BatteringRam)
					{
						if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 2) > 0)
						{
							this.ShowLineFromDeploymentPointToTarget(deploymentPoint);
						}
						if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 4) > 0)
						{
							this.CreateArcPoints(deploymentPoint);
						}
						if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 8) > 0)
						{
							this.ShowDeploymentBanners(deploymentPoint);
						}
						if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 16) > 0)
						{
							this.ShowPath(deploymentPoint);
						}
						if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 32) > 0)
						{
							this.SetGhostVisibility(deploymentPoint, true);
							return;
						}
					}
				}
				else if (deploymentPoint.GetDeploymentPointType() == DeploymentPoint.DeploymentPointType.BatteringRam)
				{
					this.HideDeploymentBanners(deploymentPoint, false);
				}
				break;
			case DeploymentPoint.DeploymentPointState.BatteringRam:
			case DeploymentPoint.DeploymentPointState.SiegeTower:
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 2) > 0)
				{
					this.ShowLineFromDeploymentPointToTarget(deploymentPoint);
				}
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 4) > 0)
				{
					this.CreateArcPoints(deploymentPoint);
				}
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 8) > 0)
				{
					this.ShowDeploymentBanners(deploymentPoint);
				}
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 16) > 0)
				{
					this.ShowPath(deploymentPoint);
				}
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 32) > 0)
				{
					this.SetGhostVisibility(deploymentPoint, true);
				}
				this.SetLaddersUpState(deploymentPoint, false);
				this.SetLightState(deploymentPoint, false);
				return;
			case DeploymentPoint.DeploymentPointState.SiegeLadder:
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 2) > 0)
				{
					this.ShowLineFromDeploymentPointToTarget(deploymentPoint);
				}
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 4) > 0)
				{
					this.CreateArcPoints(deploymentPoint);
				}
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 8) > 0)
				{
					this.ShowDeploymentBanners(deploymentPoint);
				}
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 64) > 0)
				{
					this.SetDeploymentTargetContourState(deploymentPoint, true);
				}
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 128) > 0)
				{
					this.SetLaddersUpState(deploymentPoint, true);
				}
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 256) > 0)
				{
					this.SetLightState(deploymentPoint, true);
					return;
				}
				break;
			case DeploymentPoint.DeploymentPointState.Breach:
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 2) > 0)
				{
					this.ShowLineFromDeploymentPointToTarget(deploymentPoint);
				}
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 4) > 0)
				{
					this.CreateArcPoints(deploymentPoint);
				}
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 8) > 0)
				{
					this.ShowDeploymentBanners(deploymentPoint);
				}
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 64) > 0)
				{
					this.SetDeploymentTargetContourState(deploymentPoint, true);
				}
				if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 256) > 0)
				{
					this.SetLightState(deploymentPoint, true);
					return;
				}
				break;
			default:
				return;
			}
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x000281C8 File Offset: 0x000263C8
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			foreach (DeploymentPoint deploymentPoint in this._deploymentPoints)
			{
				switch (deploymentPoint.GetDeploymentPointState())
				{
				case DeploymentPoint.DeploymentPointState.NotDeployed:
					if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 1) > 0 && deploymentPoint.GetDeploymentPointType() == DeploymentPoint.DeploymentPointType.BatteringRam)
					{
						if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 2) > 0)
						{
							this.ShowLineFromDeploymentPointToTarget(deploymentPoint);
						}
						if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 4) > 0)
						{
							this.ShowArcFromDeploymentPointToTarget(deploymentPoint);
						}
						if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 16) > 0)
						{
							this.ShowPath(deploymentPoint);
						}
					}
					break;
				case DeploymentPoint.DeploymentPointState.BatteringRam:
				case DeploymentPoint.DeploymentPointState.SiegeTower:
					if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 2) > 0)
					{
						this.ShowLineFromDeploymentPointToTarget(deploymentPoint);
					}
					if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 4) > 0)
					{
						this.ShowArcFromDeploymentPointToTarget(deploymentPoint);
					}
					if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 16) > 0)
					{
						this.ShowPath(deploymentPoint);
					}
					break;
				case DeploymentPoint.DeploymentPointState.SiegeLadder:
					if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 2) > 0)
					{
						this.ShowLineFromDeploymentPointToTarget(deploymentPoint);
					}
					if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 4) > 0)
					{
						this.ShowArcFromDeploymentPointToTarget(deploymentPoint);
					}
					break;
				case DeploymentPoint.DeploymentPointState.Breach:
					if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 2) > 0)
					{
						this.ShowLineFromDeploymentPointToTarget(deploymentPoint);
					}
					if ((SiegeDeploymentVisualizationMissionView.deploymentVisualizerSelector & 4) > 0)
					{
						this.ShowArcFromDeploymentPointToTarget(deploymentPoint);
					}
					break;
				}
			}
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00028320 File Offset: 0x00026520
		private void ShowLineFromDeploymentPointToTarget(DeploymentPoint deploymentPoint)
		{
			deploymentPoint.GetDeploymentOrigin();
			Vec3 deploymentTargetPosition = deploymentPoint.DeploymentTargetPosition;
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00028330 File Offset: 0x00026530
		private List<Vec3> CreateArcPoints(DeploymentPoint deploymentPoint)
		{
			Vec3 deploymentOrigin = deploymentPoint.GetDeploymentOrigin();
			Vec3 deploymentTargetPosition = deploymentPoint.DeploymentTargetPosition;
			float num = (deploymentTargetPosition - deploymentOrigin).Length / 3f;
			List<Vec3> list = new List<Vec3>();
			int num2 = 0;
			while ((float)num2 < num)
			{
				Vec3 vec = MBMath.Lerp(deploymentOrigin, deploymentTargetPosition, (float)num2 / num, 0f);
				float num3 = 8f - MathF.Pow(MathF.Abs((float)num2 - num * 0.5f) / (num * 0.5f), 1.2f) * 8f;
				vec.z += num3;
				list.Add(vec);
				num2++;
			}
			list.Add(deploymentTargetPosition);
			return list;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x000283DC File Offset: 0x000265DC
		private void ShowArcFromDeploymentPointToTarget(DeploymentPoint deploymentPoint)
		{
			Vec3 deploymentTargetPosition = deploymentPoint.DeploymentTargetPosition;
			List<Vec3> list;
			this._deploymentArcs.TryGetValue(deploymentPoint, out list);
			if (list == null || list[list.Count - 1] != deploymentTargetPosition)
			{
				list = this.CreateArcPoints(deploymentPoint);
			}
			Vec3 vec = Vec3.Invalid;
			foreach (Vec3 vec2 in list)
			{
				bool isValid = vec.IsValid;
				vec = vec2;
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00028468 File Offset: 0x00026668
		private void ShowDeploymentBanners(DeploymentPoint deploymentPoint)
		{
			Vec3 deploymentOrigin = deploymentPoint.GetDeploymentOrigin();
			Vec3 deploymentTargetPosition = deploymentPoint.DeploymentTargetPosition;
			ValueTuple<GameEntity, GameEntity> valueTuple;
			this._deploymentBanners.TryGetValue(deploymentPoint, out valueTuple);
			if (valueTuple.Item1 == null || valueTuple.Item2 == null)
			{
				valueTuple = this.CreateBanners(deploymentPoint);
			}
			GameEntity item = this._deploymentBanners[deploymentPoint].Item1;
			MatrixFrame matrixFrame = MatrixFrame.Identity;
			matrixFrame.origin = deploymentOrigin;
			matrixFrame.origin.z = matrixFrame.origin.z + 7.5f;
			matrixFrame.rotation.ApplyScaleLocal(10f);
			MatrixFrame matrixFrame2 = matrixFrame;
			item.SetFrame(ref matrixFrame2, true);
			item.SetVisibilityExcludeParents(true);
			item.SetAlpha(1f);
			GameEntity item2 = this._deploymentBanners[deploymentPoint].Item2;
			matrixFrame = MatrixFrame.Identity;
			matrixFrame.origin = deploymentTargetPosition;
			matrixFrame.origin.z = matrixFrame.origin.z + 7.5f;
			matrixFrame.rotation.ApplyScaleLocal(10f);
			MatrixFrame matrixFrame3 = matrixFrame;
			item2.SetFrame(ref matrixFrame3, true);
			item2.SetVisibilityExcludeParents(true);
			item2.SetAlpha(1f);
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x0002857C File Offset: 0x0002677C
		private void HideDeploymentBanners(DeploymentPoint deploymentPoint, bool isRemoving = false)
		{
			ValueTuple<GameEntity, GameEntity> valueTuple;
			this._deploymentBanners.TryGetValue(deploymentPoint, out valueTuple);
			if (valueTuple.Item1 != null && valueTuple.Item2 != null)
			{
				if (isRemoving)
				{
					valueTuple.Item1.Remove(104);
					valueTuple.Item2.Remove(105);
					return;
				}
				valueTuple.Item1.SetVisibilityExcludeParents(false);
				valueTuple.Item2.SetVisibilityExcludeParents(false);
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x000285EC File Offset: 0x000267EC
		private ValueTuple<GameEntity, GameEntity> CreateBanners(DeploymentPoint deploymentPoint)
		{
			GameEntity gameEntity = this.CreateBannerEntity(false);
			gameEntity.SetVisibilityExcludeParents(false);
			GameEntity gameEntity2 = this.CreateBannerEntity(true);
			gameEntity2.SetVisibilityExcludeParents(false);
			ValueTuple<GameEntity, GameEntity> valueTuple = new ValueTuple<GameEntity, GameEntity>(gameEntity, gameEntity2);
			this._deploymentBanners.Add(deploymentPoint, valueTuple);
			return valueTuple;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00028630 File Offset: 0x00026830
		private GameEntity CreateBannerEntity(bool isTargetEntity)
		{
			GameEntity gameEntity = GameEntity.CreateEmpty(Mission.Current.Scene, true, true, true);
			gameEntity.EntityFlags |= EntityFlags.NoOcclusionCulling;
			uint num = 4278190080U;
			uint num2;
			if (!isTargetEntity)
			{
				num2 = 2141323264U;
			}
			else
			{
				num2 = 2131100887U;
			}
			gameEntity.AddMultiMesh(MetaMesh.GetCopy("billboard_unit_mesh", true, false), true);
			gameEntity.GetFirstMesh().Color = uint.MaxValue;
			Material material = Material.GetFromResource("formation_icon").CreateCopy();
			if (isTargetEntity)
			{
				Texture fromResource = Texture.GetFromResource("plain_yellow");
				material.SetTexture(Material.MBTextureType.DiffuseMap2, fromResource);
			}
			else
			{
				Texture fromResource2 = Texture.GetFromResource("plain_blue");
				material.SetTexture(Material.MBTextureType.DiffuseMap2, fromResource2);
			}
			gameEntity.GetFirstMesh().SetMaterial(material);
			gameEntity.GetFirstMesh().Color = num2;
			gameEntity.GetFirstMesh().Color2 = num;
			gameEntity.GetFirstMesh().SetVectorArgument(0f, 1f, 0f, 1f);
			return gameEntity;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00028717 File Offset: 0x00026917
		private void ShowPath(DeploymentPoint deploymentPoint)
		{
			(deploymentPoint.GetWeaponsUnder().FirstOrDefault<SynchedMissionObject>((SynchedMissionObject wu) => wu is IMoveableSiegeWeapon) as IMoveableSiegeWeapon).HighlightPath();
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x0002874D File Offset: 0x0002694D
		private void SetGhostVisibility(DeploymentPoint deploymentPoint, bool isVisible)
		{
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00028750 File Offset: 0x00026950
		private void SetDeploymentTargetContourState(DeploymentPoint deploymentPoint, bool isHighlighted)
		{
			DeploymentPoint.DeploymentPointState deploymentPointState = deploymentPoint.GetDeploymentPointState();
			if (deploymentPointState == DeploymentPoint.DeploymentPointState.SiegeLadder)
			{
				using (List<SiegeLadder>.Enumerator enumerator = deploymentPoint.GetAssociatedSiegeLadders().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SiegeLadder siegeLadder = enumerator.Current;
						if (isHighlighted)
						{
							siegeLadder.GameEntity.SetContourColor(new uint?(4289622555U), true);
						}
						else
						{
							siegeLadder.GameEntity.SetContourColor(null, true);
						}
					}
					return;
				}
			}
			if (deploymentPointState == DeploymentPoint.DeploymentPointState.Breach)
			{
				if (isHighlighted)
				{
					deploymentPoint.AssociatedWallSegment.GameEntity.SetContourColor(new uint?(4289622555U), true);
					return;
				}
				deploymentPoint.AssociatedWallSegment.GameEntity.SetContourColor(null, true);
			}
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00028824 File Offset: 0x00026A24
		private void SetLaddersUpState(DeploymentPoint deploymentPoint, bool isRaised)
		{
			foreach (SiegeLadder siegeLadder in deploymentPoint.GetAssociatedSiegeLadders())
			{
				siegeLadder.SetUpStateVisibility(isRaised);
			}
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00028878 File Offset: 0x00026A78
		private void SetLightState(DeploymentPoint deploymentPoint, bool isVisible)
		{
			GameEntity gameEntity;
			this._deploymentLights.TryGetValue(deploymentPoint, out gameEntity);
			if (gameEntity != null)
			{
				gameEntity.SetVisibilityExcludeParents(isVisible);
				return;
			}
			if (isVisible)
			{
				this.CreateLight(deploymentPoint);
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x000288B0 File Offset: 0x00026AB0
		private void CreateLight(DeploymentPoint deploymentPoint)
		{
			MatrixFrame identity = MatrixFrame.Identity;
			identity.origin = deploymentPoint.DeploymentTargetPosition + new Vec3(0f, 0f, (deploymentPoint.GetDeploymentPointType() == DeploymentPoint.DeploymentPointType.TowerLadder) ? 10f : 3f, -1f);
			identity.rotation.RotateAboutSide(1.5707964f);
			Vec3 vec = new Vec3(5f, 5f, 5f, -1f);
			identity.Scale(in vec);
			GameEntity gameEntity = GameEntity.Instantiate(Mission.Current.Scene, "aserai_keep_interior_a_light_shaft_a", identity, true);
			this._deploymentLights.Add(deploymentPoint, gameEntity);
		}

		// Token: 0x04000308 RID: 776
		private static int deploymentVisualizerSelector;

		// Token: 0x04000309 RID: 777
		private List<DeploymentPoint> _deploymentPoints;

		// Token: 0x0400030A RID: 778
		private bool _deploymentPointsVisible;

		// Token: 0x0400030B RID: 779
		private Dictionary<DeploymentPoint, List<Vec3>> _deploymentArcs = new Dictionary<DeploymentPoint, List<Vec3>>();

		// Token: 0x0400030C RID: 780
		private Dictionary<DeploymentPoint, ValueTuple<GameEntity, GameEntity>> _deploymentBanners = new Dictionary<DeploymentPoint, ValueTuple<GameEntity, GameEntity>>();

		// Token: 0x0400030D RID: 781
		private Dictionary<DeploymentPoint, GameEntity> _deploymentLights = new Dictionary<DeploymentPoint, GameEntity>();

		// Token: 0x0400030E RID: 782
		private const uint EntityHighlightColor = 4289622555U;

		// Token: 0x020000ED RID: 237
		public enum DeploymentVisualizationPreference
		{
			// Token: 0x04000420 RID: 1056
			ShowUndeployed = 1,
			// Token: 0x04000421 RID: 1057
			Line,
			// Token: 0x04000422 RID: 1058
			Arc = 4,
			// Token: 0x04000423 RID: 1059
			Banner = 8,
			// Token: 0x04000424 RID: 1060
			Path = 16,
			// Token: 0x04000425 RID: 1061
			Ghost = 32,
			// Token: 0x04000426 RID: 1062
			Contour = 64,
			// Token: 0x04000427 RID: 1063
			LiftLadders = 128,
			// Token: 0x04000428 RID: 1064
			Light = 256,
			// Token: 0x04000429 RID: 1065
			AllEnabled = 1023
		}
	}
}
