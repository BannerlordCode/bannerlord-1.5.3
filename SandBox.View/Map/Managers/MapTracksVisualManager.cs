using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.View.Map.Visuals;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.View.Map.Managers
{
	// Token: 0x02000077 RID: 119
	public class MapTracksVisualManager : EntityVisualManagerBase<Track>
	{
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x00026E5E File Offset: 0x0002505E
		public static MapTracksVisualManager Current
		{
			get
			{
				return SandBoxViewSubModule.SandBoxViewVisualManager.GetEntityComponent<MapTracksVisualManager>();
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x00026E6A File Offset: 0x0002506A
		public override int Priority
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00026E70 File Offset: 0x00025070
		public MapTracksVisualManager()
		{
			this._trackVisuals = new Dictionary<Track, ValueTuple<TrackVisual, GameEntity>>();
			this._entityPool = new Stack<GameEntity>();
			this.PopulateEntityPool();
			this._parallelUpdateTrackColorsPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelUpdateTrackColors);
			this._parallelUpdateVisibleTracksPredicate = new TWParallel.ParallelForAuxPredicate(this.ParallelUpdateVisibleTracks);
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x00026ECA File Offset: 0x000250CA
		public override void OnVisualTick(MapScreen screen, float realDt, float dt)
		{
			if (this._tracksDirty)
			{
				this.UpdateTrackMesh();
				this._tracksDirty = false;
			}
			TWParallel.For(0, MapScreen.Instance.MapTracksCampaignBehavior.DetectedTracks.Count, this._parallelUpdateTrackColorsPredicate, 16);
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00026F03 File Offset: 0x00025103
		public override bool OnVisualIntersected(Ray mouseRay, UIntPtr[] intersectedEntityIDs, Intersection[] intersectionInfos, int entityCount, Vec3 worldMouseNear, Vec3 worldMouseFar, Vec3 terrainIntersectionPoint, ref MapEntityVisual hoveredVisual, ref MapEntityVisual selectedVisual)
		{
			if (hoveredVisual == null)
			{
				hoveredVisual = this.GetVisualOfEntity(this.GetTrackOnMouse(mouseRay, terrainIntersectionPoint));
			}
			return hoveredVisual != null;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00026F24 File Offset: 0x00025124
		public override void OnGameLoadFinished()
		{
			base.OnGameLoadFinished();
			foreach (Track track in MapScreen.Instance.MapTracksCampaignBehavior.DetectedTracks)
			{
				this.OnTrackDetected(track);
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00026F88 File Offset: 0x00025188
		public override MapEntityVisual<Track> GetVisualOfEntity(Track entity)
		{
			if (entity == null)
			{
				return null;
			}
			return this._trackVisuals[entity].Item1;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00026FA0 File Offset: 0x000251A0
		protected override void OnFinalize()
		{
			base.OnFinalize();
			foreach (GameEntity gameEntity in this._entityPool.ToList<GameEntity>())
			{
				gameEntity.Remove(111);
			}
			this._entityPool.Clear();
			this._trackVisuals.Clear();
			CampaignEventDispatcher.Instance.RemoveListeners(this);
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00027020 File Offset: 0x00025220
		protected override void OnInitialize()
		{
			base.OnInitialize();
			CampaignEvents.TrackDetectedEvent.AddNonSerializedListener(this, new Action<Track>(this.OnTrackDetected));
			CampaignEvents.TrackLostEvent.AddNonSerializedListener(this, new Action<Track>(this.OnTrackLost));
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00027058 File Offset: 0x00025258
		internal void ReleaseResources(Track track)
		{
			ValueTuple<TrackVisual, GameEntity> valueTuple;
			if (this._trackVisuals.TryGetValue(track, out valueTuple))
			{
				valueTuple.Item2.Remove(111);
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00027084 File Offset: 0x00025284
		private void OnTrackDetected(Track track)
		{
			this._tracksDirty = true;
			GameEntity gameEntity = this.GetGameEntity();
			gameEntity.SetVisibilityExcludeParents(true);
			this._trackVisuals.Add(track, new ValueTuple<TrackVisual, GameEntity>(new TrackVisual(track), gameEntity));
			SandBoxViewSubModule.VisualsOfEntities.Add(this._trackVisuals[track].Item2.Pointer, this._trackVisuals[track].Item1);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x000270F0 File Offset: 0x000252F0
		private void OnTrackLost(Track track)
		{
			this._tracksDirty = true;
			ValueTuple<TrackVisual, GameEntity> valueTuple = this._trackVisuals[track];
			this._trackVisuals.Remove(track);
			SandBoxViewSubModule.VisualsOfEntities.Remove(valueTuple.Item2.Pointer);
			this.ReleaseEntity(valueTuple.Item2);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00027140 File Offset: 0x00025340
		private void ParallelUpdateTrackColors(Track track)
		{
			(this._trackVisuals[track].Item2.GetComponentAtIndex(0, GameEntity.ComponentType.Decal) as Decal).SetFactor1(Campaign.Current.Models.MapTrackModel.GetTrackColor(track));
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0002717C File Offset: 0x0002537C
		private void ParallelUpdateTrackColors(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this.ParallelUpdateTrackColors(MapScreen.Instance.MapTracksCampaignBehavior.DetectedTracks[i]);
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x000271B0 File Offset: 0x000253B0
		private void UpdateTrackMesh()
		{
			TWParallel.For(0, MapScreen.Instance.MapTracksCampaignBehavior.DetectedTracks.Count, this._parallelUpdateVisibleTracksPredicate, 16);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x000271D4 File Offset: 0x000253D4
		private void UpdateTrackPoolPosition(Track track)
		{
			MatrixFrame matrixFrame = this.CalculateTrackFrame(track);
			this._trackVisuals[track].Item2.SetFrame(ref matrixFrame, true);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00027202 File Offset: 0x00025402
		private void ParallelUpdateVisibleTracks(Track track)
		{
			this._trackVisuals[track].Item2.SetVisibilityExcludeParents(true);
			this.UpdateTrackPoolPosition(track);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00027224 File Offset: 0x00025424
		private void ParallelUpdateVisibleTracks(int startInclusive, int endExclusive)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this.ParallelUpdateVisibleTracks(MapScreen.Instance.MapTracksCampaignBehavior.DetectedTracks[i]);
			}
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00027258 File Offset: 0x00025458
		private bool RaySphereIntersection(Ray ray, SphereData sphere, ref Vec3 intersectionPoint)
		{
			Vec3 origin = sphere.Origin;
			float radius = sphere.Radius;
			Vec3 vec = origin - ray.Origin;
			float num = Vec3.DotProduct(ray.Direction, vec);
			if (num > 0f)
			{
				Vec3 vec2 = ray.Origin + ray.Direction * num - origin;
				float num2 = radius * radius - vec2.LengthSquared;
				if (num2 >= 0f)
				{
					float num3 = MathF.Sqrt(num2);
					float num4 = num - num3;
					if (num4 >= 0f && num4 <= ray.MaxDistance)
					{
						intersectionPoint = ray.Origin + ray.Direction * num4;
						return true;
					}
					if (num4 < 0f)
					{
						intersectionPoint = ray.Origin;
						return true;
					}
				}
			}
			else if ((ray.Origin - origin).LengthSquared < radius * radius)
			{
				intersectionPoint = ray.Origin;
				return true;
			}
			return false;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0002735C File Offset: 0x0002555C
		private Track GetTrackOnMouse(Ray mouseRay, Vec3 mouseIntersectionPoint)
		{
			Track track = null;
			for (int i = 0; i < MapScreen.Instance.MapTracksCampaignBehavior.DetectedTracks.Count; i++)
			{
				Track track2 = MapScreen.Instance.MapTracksCampaignBehavior.DetectedTracks[i];
				float trackScale = Campaign.Current.Models.MapTrackModel.GetTrackScale(track2);
				MatrixFrame matrixFrame = this.CalculateTrackFrame(track2);
				float lengthSquared = (matrixFrame.origin - mouseIntersectionPoint).LengthSquared;
				if (lengthSquared < 0.1f)
				{
					float num = MathF.Sqrt(lengthSquared);
					this._trackSphere.Origin = matrixFrame.origin;
					this._trackSphere.Radius = 0.05f + num * 0.01f + trackScale;
					Vec3 vec = default(Vec3);
					if (this.RaySphereIntersection(mouseRay, this._trackSphere, ref vec))
					{
						track = track2;
						break;
					}
				}
			}
			return track;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0002743C File Offset: 0x0002563C
		private MatrixFrame CalculateTrackFrame(Track track)
		{
			Vec3 vec = track.Position.AsVec3();
			float scale = track.Scale;
			MatrixFrame identity = MatrixFrame.Identity;
			identity.origin = vec;
			float num;
			Vec3 vec2;
			Campaign.Current.MapSceneWrapper.GetTerrainHeightAndNormal(identity.origin.AsVec2, out num, out vec2);
			identity.rotation.u = vec2;
			Vec2 asVec = identity.rotation.f.AsVec2;
			asVec.RotateCCW(track.Direction);
			identity.rotation.f = new Vec3(asVec.x, asVec.y, identity.rotation.f.z, -1f);
			identity.rotation.s = Vec3.CrossProduct(identity.rotation.f, identity.rotation.u);
			identity.rotation.s.Normalize();
			identity.rotation.f = Vec3.CrossProduct(identity.rotation.u, identity.rotation.s);
			identity.rotation.f.Normalize();
			float num2 = scale;
			identity.rotation.s = identity.rotation.s * num2;
			identity.rotation.f = identity.rotation.f * num2;
			identity.rotation.u = identity.rotation.u * num2;
			return identity;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x000275B8 File Offset: 0x000257B8
		private GameEntity GetGameEntity()
		{
			Stack<GameEntity> entityPool = this._entityPool;
			if (entityPool.Count != 0)
			{
				return entityPool.Pop();
			}
			GameEntity gameEntity = GameEntity.Instantiate(base.MapScene, "map_track_arrow", MatrixFrame.Identity, true);
			gameEntity.SetVisibilityExcludeParents(false);
			return gameEntity;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000275F8 File Offset: 0x000257F8
		private void PopulateEntityPool()
		{
			for (int i = 0; i < 256; i++)
			{
				GameEntity gameEntity = GameEntity.Instantiate(base.MapScene, "map_track_arrow", MatrixFrame.Identity, true);
				gameEntity.SetVisibilityExcludeParents(false);
				this._entityPool.Push(gameEntity);
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0002763F File Offset: 0x0002583F
		private void ReleaseEntity(GameEntity e)
		{
			e.SetVisibilityExcludeParents(false);
			if (this._entityPool == null)
			{
				this._entityPool = new Stack<GameEntity>();
			}
			this._entityPool.Push(e);
		}

		// Token: 0x04000247 RID: 583
		private const string TrackPrefabName = "map_track_arrow";

		// Token: 0x04000248 RID: 584
		private const int DefaultObjectPoolCount = 256;

		// Token: 0x04000249 RID: 585
		private Dictionary<Track, ValueTuple<TrackVisual, GameEntity>> _trackVisuals;

		// Token: 0x0400024A RID: 586
		private SphereData _trackSphere;

		// Token: 0x0400024B RID: 587
		private bool _tracksDirty = true;

		// Token: 0x0400024C RID: 588
		private readonly TWParallel.ParallelForAuxPredicate _parallelUpdateTrackColorsPredicate;

		// Token: 0x0400024D RID: 589
		private readonly TWParallel.ParallelForAuxPredicate _parallelUpdateVisibleTracksPredicate;

		// Token: 0x0400024E RID: 590
		private Stack<GameEntity> _entityPool;
	}
}
