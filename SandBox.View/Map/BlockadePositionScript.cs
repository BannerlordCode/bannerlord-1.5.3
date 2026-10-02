using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.View.Map
{
	// Token: 0x02000040 RID: 64
	public class BlockadePositionScript : ScriptComponentBehavior
	{
		// Token: 0x06000206 RID: 518 RVA: 0x00013BB7 File Offset: 0x00011DB7
		protected override void OnEditorTick(float dt)
		{
			if (this.IsVisualizationEnabled)
			{
				this.VisualizeArcs();
			}
		}

		// Token: 0x06000207 RID: 519 RVA: 0x00013BC8 File Offset: 0x00011DC8
		private void VisualizeArcs()
		{
			if (this._pointsOfArcs == null || !this.IsRandomizationEnabled)
			{
				this._pointsOfArcs = this.GetBlockadeArc(this.MaximumNumberOfShips, out this._center);
			}
			if (this._pointsOfArcs != null)
			{
				foreach (List<Vec3> list in this._pointsOfArcs)
				{
					foreach (Vec3 vec in list)
					{
					}
				}
			}
		}

		// Token: 0x06000208 RID: 520 RVA: 0x00013C78 File Offset: 0x00011E78
		protected override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "RefreshVisualization")
			{
				this._pointsOfArcs = this.GetBlockadeArc(this.MaximumNumberOfShips, out this._center);
				if (!this._shipEntities.IsEmpty<GameEntity>())
				{
					Utilities.DeleteEntitiesInEditorScene(this._shipEntities);
				}
				this._shipEntities.Clear();
				if (this.IsShipVisualizationEnabled)
				{
					foreach (List<Vec3> list in this._pointsOfArcs)
					{
						foreach (Vec3 vec in list)
						{
							Vec2 vec2 = vec.AsVec2 - this._center.AsVec2;
							MatrixFrame identity = MatrixFrame.Identity;
							identity.origin = vec;
							float num = vec2.AngleBetween(identity.rotation.f.AsVec2);
							identity.Rotate(1.5707964f - num, in Vec3.Up);
							identity.rotation.ApplyScaleLocal(this.ShipScaleFactor);
							GameEntity gameEntity = VisualShipFactory.CreateVisualShipForCampaign(this.MissionShipId, base.GameEntity.Scene, new List<ShipVisualSlotInfo>(), 0, "", uint.MaxValue, uint.MaxValue);
							if (gameEntity == null)
							{
								break;
							}
							gameEntity.SetVisibilityExcludeParents(true);
							gameEntity.SetFrame(ref identity, true);
							this._shipEntities.Add(gameEntity);
						}
					}
				}
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x00013E34 File Offset: 0x00012034
		public List<List<Vec3>> GetBlockadeArc(int totalNumberOfShips, out Vec3 center)
		{
			int num = this.MaximumNumberOfShips;
			if (totalNumberOfShips < num)
			{
				num = totalNumberOfShips;
			}
			List<List<Vec3>> list = new List<List<Vec3>>();
			WeakGameEntity firstChildEntityWithTag = base.GameEntity.GetFirstChildEntityWithTag("Blockade_Arc_Start");
			WeakGameEntity firstChildEntityWithTag2 = base.GameEntity.GetFirstChildEntityWithTag("Blockade_Arc_End");
			center = Vec3.Invalid;
			if (firstChildEntityWithTag == null || firstChildEntityWithTag2 == null)
			{
				return list;
			}
			center = this.FindCenterOfCircle(firstChildEntityWithTag2.GlobalPosition, firstChildEntityWithTag.GlobalPosition);
			Vec3 vec = firstChildEntityWithTag2.GlobalPosition - center;
			vec.Normalize();
			Vec3 vec2 = vec;
			int i = this.NumberOfArcs;
			float num2 = firstChildEntityWithTag2.GlobalPosition.Distance(center) / (float)this.NumberOfArcs;
			int num3 = 0;
			float num4 = this.DistanceBetweenShips;
			while (i > 0)
			{
				int num5 = MathF.Round(this.Angle * (float)i / this.DistanceBetweenShips);
				if (num - num3 < num5)
				{
					num4 *= (float)num5 / (float)(num - num3);
					num5 = num - num3;
				}
				vec2.RotateAboutZ(num4 / (float)(i * 2));
				List<Vec3> list2 = new List<Vec3>();
				for (int j = 0; j < num5; j++)
				{
					float num6 = MBRandom.RandomFloatRanged(-this.DistanceRandomizationOnArcs, this.DistanceRandomizationOnArcs);
					float num7 = MBRandom.RandomFloatRanged(0f, this.DistanceRandomizationBetweenArcs);
					Vec3 vec3 = center + vec2 * (float)i * num2;
					vec2.RotateAboutZ(num4 / (float)i);
					if (this.IsRandomizationEnabled)
					{
						vec3 += vec2 * num7;
						vec2.RotateAboutZ(num6);
					}
					list2.Add(vec3);
					num3++;
					if (num3 >= num)
					{
						break;
					}
				}
				list.Add(list2);
				if (num3 >= num)
				{
					return list;
				}
				vec2 = vec;
				i--;
			}
			return list;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0001401C File Offset: 0x0001221C
		private Vec3 FindCenterOfCircle(Vec3 arcPointStart, Vec3 arcPointEnd)
		{
			Vec3 vec = arcPointEnd + (arcPointStart - arcPointEnd) / 2f;
			Vec3 vec2 = (arcPointStart - arcPointEnd) / 2f;
			float num = arcPointEnd.Distance(vec);
			float num2 = num / MathF.Tan(this.Angle / 2f);
			return new Vec3(vec.X + num2 * vec2.Y / num, vec.Y - num2 * vec2.X / num, arcPointStart.Z, -1f);
		}

		// Token: 0x04000110 RID: 272
		public int MaximumNumberOfShips = 12;

		// Token: 0x04000111 RID: 273
		public int NumberOfArcs = 4;

		// Token: 0x04000112 RID: 274
		public float DistanceBetweenShips = 0.7853982f;

		// Token: 0x04000113 RID: 275
		public float DistanceRandomizationOnArcs = 0.1f;

		// Token: 0x04000114 RID: 276
		public float DistanceRandomizationBetweenArcs = 0.1f;

		// Token: 0x04000115 RID: 277
		public float Angle = 1.5707964f;

		// Token: 0x04000116 RID: 278
		public string MissionShipId = "dromon_ship_nested";

		// Token: 0x04000117 RID: 279
		public float ShipScaleFactor = 0.052f;

		// Token: 0x04000118 RID: 280
		public bool IsVisualizationEnabled;

		// Token: 0x04000119 RID: 281
		public bool IsRandomizationEnabled;

		// Token: 0x0400011A RID: 282
		public bool IsShipVisualizationEnabled;

		// Token: 0x0400011B RID: 283
		public SimpleButton RefreshVisualization;

		// Token: 0x0400011C RID: 284
		private List<List<Vec3>> _pointsOfArcs;

		// Token: 0x0400011D RID: 285
		private Vec3 _center;

		// Token: 0x0400011E RID: 286
		private List<GameEntity> _shipEntities = new List<GameEntity>();
	}
}
