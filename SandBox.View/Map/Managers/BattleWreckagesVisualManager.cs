using System;
using System.Collections.Generic;
using SandBox.View.Map.Visuals;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.BattleWreckages;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.View.Map.Managers
{
	// Token: 0x02000074 RID: 116
	public class BattleWreckagesVisualManager : EntityVisualManagerBase<BattleWreckage>
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000509 RID: 1289 RVA: 0x0002686E File Offset: 0x00024A6E
		public static BattleWreckagesVisualManager Current
		{
			get
			{
				return SandBoxViewSubModule.SandBoxViewVisualManager.GetEntityComponent<BattleWreckagesVisualManager>();
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x0002687A File Offset: 0x00024A7A
		public override int Priority
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00026880 File Offset: 0x00024A80
		public override MapEntityVisual<BattleWreckage> GetVisualOfEntity(BattleWreckage entity)
		{
			BattleWreckageVisual battleWreckageVisual;
			if (this._visuals.TryGetValue(entity, out battleWreckageVisual))
			{
				return battleWreckageVisual;
			}
			return null;
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x000268A0 File Offset: 0x00024AA0
		protected override void OnInitialize()
		{
			this.RegisterEvents();
			foreach (BattleWreckage battleWreckage in Campaign.Current.Wreckages)
			{
				this.OnBattleWreckageCreated(battleWreckage);
			}
			this._circleDecalHover = MapScreen.DecalEntity.Create(base.MapScene, "map_circle_decal", "OuterPointTarget");
			this._circleDecalTarget = MapScreen.DecalEntity.Create(base.MapScene, "map_circle_decal", "OuterPointTarget");
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00026934 File Offset: 0x00024B34
		private void RegisterEvents()
		{
			CampaignEvents.MapInteractableCreated.AddNonSerializedListener(this, new Action<IInteractablePoint>(this.OnInteractableCreated));
			CampaignEvents.MapInteractableDestroyed.AddNonSerializedListener(this, new Action<IInteractablePoint>(this.OnInteractableDestroyed));
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00026964 File Offset: 0x00024B64
		public override void OnTick(float realDt, float dt)
		{
			TWParallel.For(0, this._visualsFlattened.Count, delegate(int startInclusive, int endExclusive)
			{
				for (int j = startInclusive; j < endExclusive; j++)
				{
					this._visualsFlattened[j].Tick(dt, realDt);
				}
			}, 16);
			foreach (KeyValuePair<BattleWreckage, BattleWreckageVisual> keyValuePair in this._visuals)
			{
				if (keyValuePair.Value.HasVisibilityChanged())
				{
					keyValuePair.Value.OnVisibilityChanged();
					if (!this._fadingVisuals.Contains(keyValuePair.Value))
					{
						this._fadingVisuals.Add(keyValuePair.Value);
					}
				}
			}
			for (int i = this._fadingVisuals.Count - 1; i >= 0; i--)
			{
				this._fadingVisuals[i].TickFadingState(realDt);
				if (!this._fadingVisuals[i].IsFading)
				{
					this._fadingVisuals.RemoveAt(i);
				}
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00026A78 File Offset: 0x00024C78
		public override void OnVisualTick(MapScreen screen, float realDt, float dt)
		{
			BattleWreckageVisual battleWreckageVisual = null;
			BattleWreckageVisual battleWreckageVisual2 = null;
			BattleWreckage battleWreckage;
			BattleWreckageVisual battleWreckageVisual3;
			if ((battleWreckage = MobileParty.MainParty.Ai.AiBehaviorInteractable as BattleWreckage) != null)
			{
				battleWreckageVisual2 = (BattleWreckageVisual)this.GetVisualOfEntity(battleWreckage);
			}
			else if ((battleWreckageVisual3 = screen.CurrentVisualOfTooltip as BattleWreckageVisual) != null)
			{
				battleWreckageVisual = battleWreckageVisual3;
			}
			if (battleWreckageVisual2 != null)
			{
				MatrixFrame identity = MatrixFrame.Identity;
				identity.origin = battleWreckageVisual2.GetVisualPosition();
				Vec3 vec = Vec3.One * battleWreckageVisual2.WreckageTypeCoefficient;
				identity.Scale(in vec);
				this._circleDecalTarget.GameEntity.SetVisibilityExcludeParents(true);
				this._circleDecalTarget.Decal.SetVectorArgument(0.166f, 1f, 0.83f, 0f);
				this._circleDecalTarget.Decal.SetFactor1Linear(4291596077U);
				this._circleDecalTarget.GameEntity.SetGlobalFrame(in identity, true);
			}
			else
			{
				this._circleDecalTarget.GameEntity.SetVisibilityExcludeParents(false);
			}
			if (battleWreckageVisual != null && battleWreckageVisual != battleWreckageVisual2)
			{
				MatrixFrame identity2 = MatrixFrame.Identity;
				identity2.origin = battleWreckageVisual.GetVisualPosition();
				Vec3 vec = Vec3.One * battleWreckageVisual.WreckageTypeCoefficient;
				identity2.Scale(in vec);
				this._circleDecalHover.GameEntity.SetVisibilityExcludeParents(true);
				this._circleDecalHover.Decal.SetVectorArgument(0.166f, 1f, 0.83f, 0f);
				this._circleDecalHover.Decal.SetFactor1Linear(4291596077U);
				this._circleDecalHover.GameEntity.SetGlobalFrame(in identity2, true);
				return;
			}
			this._circleDecalHover.GameEntity.SetVisibilityExcludeParents(false);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x00026C14 File Offset: 0x00024E14
		public override bool OnVisualIntersected(Ray mouseRay, UIntPtr[] intersectedEntityIDs, Intersection[] intersectionInfos, int entityCount, Vec3 worldMouseNear, Vec3 worldMouseFar, Vec3 terrainIntersectionPoint, ref MapEntityVisual hoveredVisual, ref MapEntityVisual selectedVisual)
		{
			for (int i = entityCount - 1; i >= 0; i--)
			{
				UIntPtr uintPtr = intersectedEntityIDs[i];
				MapEntityVisual mapEntityVisual;
				if (uintPtr != UIntPtr.Zero && MapScreen.VisualsOfEntities.TryGetValue(uintPtr, out mapEntityVisual) && mapEntityVisual is BattleWreckageVisual && mapEntityVisual.IsVisibleOrFadingOut())
				{
					hoveredVisual = mapEntityVisual;
					selectedVisual = mapEntityVisual;
				}
			}
			return selectedVisual != null;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00026C70 File Offset: 0x00024E70
		private void OnInteractableCreated(IInteractablePoint point)
		{
			BattleWreckage battleWreckage;
			if ((battleWreckage = point as BattleWreckage) != null)
			{
				this.OnBattleWreckageCreated(battleWreckage);
			}
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00026C90 File Offset: 0x00024E90
		private void OnInteractableDestroyed(IInteractablePoint point)
		{
			BattleWreckage battleWreckage;
			if ((battleWreckage = point as BattleWreckage) != null)
			{
				this.OnBattleWreckageDestroyed(battleWreckage);
			}
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00026CB0 File Offset: 0x00024EB0
		private void OnBattleWreckageCreated(BattleWreckage battleWreckage)
		{
			BattleWreckageVisual battleWreckageVisual = new BattleWreckageVisual(battleWreckage);
			battleWreckageVisual.OnStartup();
			if (!this._visuals.ContainsKey(battleWreckageVisual.MapEntity))
			{
				this._visuals.Add(battleWreckageVisual.MapEntity, battleWreckageVisual);
				this._visualsFlattened.Add(battleWreckageVisual);
			}
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x00026CFC File Offset: 0x00024EFC
		private void OnBattleWreckageDestroyed(BattleWreckage battleWreckage)
		{
			BattleWreckageVisual battleWreckageVisual = (BattleWreckageVisual)this.GetVisualOfEntity(battleWreckage);
			if (battleWreckageVisual != null)
			{
				this._fadingVisuals.Remove(battleWreckageVisual);
				this._visualsFlattened.Remove(battleWreckageVisual);
				battleWreckageVisual.OnRemoved();
			}
			this._visuals.Remove(battleWreckage);
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00026D48 File Offset: 0x00024F48
		protected override void OnFinalize()
		{
			base.OnFinalize();
			foreach (KeyValuePair<BattleWreckage, BattleWreckageVisual> keyValuePair in this._visuals)
			{
				keyValuePair.Value.OnRemoved();
			}
			this._visuals.Clear();
			this._fadingVisuals.Clear();
			this._visualsFlattened.Clear();
		}

		// Token: 0x0400023F RID: 575
		private readonly Dictionary<BattleWreckage, BattleWreckageVisual> _visuals = new Dictionary<BattleWreckage, BattleWreckageVisual>();

		// Token: 0x04000240 RID: 576
		private readonly List<BattleWreckageVisual> _visualsFlattened = new List<BattleWreckageVisual>();

		// Token: 0x04000241 RID: 577
		private readonly List<BattleWreckageVisual> _fadingVisuals = new List<BattleWreckageVisual>();

		// Token: 0x04000242 RID: 578
		private MapScreen.DecalEntity _circleDecalHover;

		// Token: 0x04000243 RID: 579
		private MapScreen.DecalEntity _circleDecalTarget;

		// Token: 0x04000244 RID: 580
		private const string CircleDecalMaterialName = "map_circle_decal";

		// Token: 0x04000245 RID: 581
		private const string CircleDecalEntityName = "OuterPointTarget";
	}
}
