using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem.Naval;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace SandBox.Missions
{
	// Token: 0x0200005B RID: 91
	public class CivilianPortShipSpawnMissionLogic : MissionLogic
	{
		// Token: 0x0600038E RID: 910 RVA: 0x00014CF0 File Offset: 0x00012EF0
		public CivilianPortShipSpawnMissionLogic(List<Ship> mainPartyShips, List<Ship> townLordShips)
		{
			this._mainPartyShips = mainPartyShips;
			this._townLordShips = townLordShips;
		}

		// Token: 0x0600038F RID: 911 RVA: 0x00014D40 File Offset: 0x00012F40
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			foreach (GameEntity gameEntity in Mission.Current.Scene.FindEntitiesWithTag("shipyard_ship"))
			{
				this._shipyardShipSpawnPoints.Enqueue(gameEntity);
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00014DA8 File Offset: 0x00012FA8
		public override void EarlyStart()
		{
			base.EarlyStart();
			if (!this._shipyardShipSpawnPoints.IsEmpty<GameEntity>())
			{
				if (!this._mainPartyShips.IsEmpty<Ship>())
				{
					Ship randomElement = this._mainPartyShips.GetRandomElement<Ship>();
					this.SpawnShip(randomElement);
				}
				while (!this._shipyardShipSpawnPoints.IsEmpty<GameEntity>() && !this._townLordShips.IsEmpty<Ship>())
				{
					Ship randomElement2 = this._townLordShips.GetRandomElement<Ship>();
					this._townLordShips.Remove(randomElement2);
					this.SpawnShip(randomElement2);
				}
			}
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00014E24 File Offset: 0x00013024
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			foreach (KeyValuePair<GameEntity, MatrixFrame> keyValuePair in this._spawnedShipVisuals)
			{
				GameEntity key = keyValuePair.Key;
				MatrixFrame value = keyValuePair.Value;
				this.TickShipAnimation(dt, key, in value);
			}
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00014E90 File Offset: 0x00013090
		private void SpawnShip(Ship ship)
		{
			MissionShipObject @object = MBObjectManager.Instance.GetObject<MissionShipObject>(ship.ShipHull.MissionShipObjectId);
			ValueTuple<uint, uint> valueTuple;
			ShipHelper.TryGetSailColors(ship, out valueTuple, null);
			GameEntity gameEntity = VisualShipFactory.CreateVisualShip(@object.Prefab, base.Mission.Scene, ship.GetShipVisualSlotInfos(), ship.RandomValue, ship.HitPoints / ship.MaxSailHitPoints, valueTuple.Item1, valueTuple.Item2, true, ship.ShipHull.FloatingForceMultiplier, true);
			MatrixFrame globalFrame = this._shipyardShipSpawnPoints.Dequeue().GetGlobalFrame();
			float waterLevelAtPosition = base.Mission.Scene.GetWaterLevelAtPosition(globalFrame.origin.AsVec2, true, true);
			globalFrame.origin.z = waterLevelAtPosition;
			if (gameEntity != null)
			{
				gameEntity.SetFrame(ref globalFrame, true);
			}
			this._spawnedShipVisuals.Add(gameEntity, globalFrame);
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00014F5C File Offset: 0x0001315C
		private void TickShipAnimation(float dt, GameEntity shipVisualEntity, in MatrixFrame initialFrame)
		{
			if (shipVisualEntity == null)
			{
				return;
			}
			MatrixFrame frame = shipVisualEntity.GetFrame();
			Vec3 vec = shipVisualEntity.GetBoundingBoxMin() + new Vec3(5f, 5f, 0f, -1f);
			Vec3 vec2 = shipVisualEntity.GetBoundingBoxMax() - new Vec3(5f, 5f, 0f, -1f);
			Vec2[] array = new Vec2[32];
			for (int i = 0; i < 4; i++)
			{
				float num = (float)i / 3f;
				float num2 = MathF.Lerp(vec.x, vec2.x, num, 1E-05f);
				for (int j = 0; j < 8; j++)
				{
					float num3 = (float)j / 7f;
					float num4 = MathF.Lerp(vec.y, vec2.y, num3, 1E-05f);
					Vec3 vec3 = frame.origin + new Vec3(num2, num4, 0f, -1f);
					int num5 = i * 8 + j;
					array[num5] = vec3.AsVec2;
				}
			}
			Vec3 vec4 = Vec3.Zero;
			float num6 = 0f;
			float[] array2 = new float[array.Length];
			Vec3[] array3 = new Vec3[array.Length];
			base.Mission.Scene.GetBulkWaterLevelAtPositions(array, ref array2, ref array3);
			for (int k = 0; k < array3.Length; k++)
			{
				Vec3 vec5 = array3[k];
				vec4 += vec5;
				num6 += array2[k];
			}
			vec4.Normalize();
			num6 /= (float)array3.Length;
			MatrixFrame matrixFrame = initialFrame;
			matrixFrame.origin.z = num6 + 0.5f;
			Mat3 identity = Mat3.Identity;
			identity.u = vec4;
			identity.u.Normalize();
			identity.s = Vec3.CrossProduct(Vec3.Forward, identity.u);
			identity.s.Normalize();
			identity.f = Vec3.CrossProduct(identity.u, identity.s);
			identity.f.Normalize();
			matrixFrame.rotation = identity;
			MatrixFrame matrixFrame2 = MatrixFrame.Slerp(in frame, in matrixFrame, dt * 1.5f);
			shipVisualEntity.SetFrame(ref matrixFrame2, true);
		}

		// Token: 0x040001D1 RID: 465
		private const string ShipyardShipSpawnPointTag = "shipyard_ship";

		// Token: 0x040001D2 RID: 466
		private Queue<GameEntity> _shipyardShipSpawnPoints = new Queue<GameEntity>();

		// Token: 0x040001D3 RID: 467
		private List<Ship> _mainPartyShips = new List<Ship>();

		// Token: 0x040001D4 RID: 468
		private List<Ship> _townLordShips = new List<Ship>();

		// Token: 0x040001D5 RID: 469
		private Dictionary<GameEntity, MatrixFrame> _spawnedShipVisuals = new Dictionary<GameEntity, MatrixFrame>();
	}
}
