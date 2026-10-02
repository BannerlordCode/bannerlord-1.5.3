using System;
using System.Collections.Generic;
using SandBox.View.Map.Visuals;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.View.Map.Managers
{
	// Token: 0x02000079 RID: 121
	public class MobilePartyVisualManager : EntityVisualManagerBase<PartyBase>
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x00027BC0 File Offset: 0x00025DC0
		public override int Priority
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x00027BC4 File Offset: 0x00025DC4
		public static MobilePartyVisualManager Current
		{
			get
			{
				return SandBoxViewSubModule.SandBoxViewVisualManager.GetEntityComponent<MobilePartyVisualManager>();
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x00027BD0 File Offset: 0x00025DD0
		public override void OnTick(float realDt, float dt)
		{
			this._dirtyPartyVisualCount = -1;
			TWParallel.For(0, this._visualsFlattened.Count, delegate(int startInclusive, int endExclusive)
			{
				for (int k = startInclusive; k < endExclusive; k++)
				{
					this._visualsFlattened[k].Tick(dt, realDt, ref this._dirtyPartyVisualCount, ref this._dirtyPartiesList);
				}
			}, 16);
			for (int i = 0; i < this._dirtyPartyVisualCount + 1; i++)
			{
				this._dirtyPartiesList[i].ValidateIsDirty();
			}
			for (int j = this._fadingPartiesFlatten.Count - 1; j >= 0; j--)
			{
				this._fadingPartiesFlatten[j].TickFadingState(realDt, dt);
			}
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x00027C74 File Offset: 0x00025E74
		public override void ClearVisualMemory()
		{
			foreach (MobilePartyVisual mobilePartyVisual in this._visualsFlattened)
			{
				mobilePartyVisual.ClearVisualMemory();
			}
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x00027CC4 File Offset: 0x00025EC4
		public override void OnVisualTick(MapScreen screen, float realDt, float dt)
		{
			base.OnVisualTick(screen, realDt, dt);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x00027CD0 File Offset: 0x00025ED0
		public override bool OnVisualIntersected(Ray mouseRay, UIntPtr[] intersectedEntityIDs, Intersection[] intersectionInfos, int entityCount, Vec3 worldMouseNear, Vec3 worldMouseFar, Vec3 terrainIntersectionPoint, ref MapEntityVisual hoveredVisual, ref MapEntityVisual selectedVisual)
		{
			for (int i = entityCount - 1; i >= 0; i--)
			{
				UIntPtr uintPtr = intersectedEntityIDs[i];
				MapEntityVisual mapEntityVisual;
				MobilePartyVisual mobilePartyVisual;
				if (uintPtr != UIntPtr.Zero && MapScreen.VisualsOfEntities.TryGetValue(uintPtr, out mapEntityVisual) && (mobilePartyVisual = mapEntityVisual as MobilePartyVisual) != null && mapEntityVisual.IsVisibleOrFadingOut() && (!mobilePartyVisual.MapEntity.IsMobile || mobilePartyVisual.MapEntity.MobileParty.IsMainParty || !mobilePartyVisual.MapEntity.MobileParty.IsInRaftState))
				{
					if (!mobilePartyVisual.IsMainEntity || !Hero.MainHero.IsPrisoner)
					{
						hoveredVisual = mapEntityVisual.AttachedTo ?? mapEntityVisual;
					}
					if (!mapEntityVisual.IsMainEntity && (mapEntityVisual.AttachedTo == null || !mapEntityVisual.AttachedTo.IsMainEntity))
					{
						selectedVisual = mapEntityVisual.AttachedTo ?? mapEntityVisual;
					}
				}
			}
			return selectedVisual != null;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x00027DB4 File Offset: 0x00025FB4
		public override MapEntityVisual<PartyBase> GetVisualOfEntity(PartyBase partyBase)
		{
			MobileParty mobileParty = partyBase.MobileParty;
			if (mobileParty != null && !mobileParty.IsCurrentlyAtSea)
			{
				MobilePartyVisual mobilePartyVisual;
				this._partiesAndVisuals.TryGetValue(partyBase, out mobilePartyVisual);
				return mobilePartyVisual;
			}
			return null;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00027DEC File Offset: 0x00025FEC
		protected override void OnFinalize()
		{
			foreach (MobilePartyVisual mobilePartyVisual in this._partiesAndVisuals.Values)
			{
				mobilePartyVisual.ReleaseResources();
			}
			CampaignEventDispatcher.Instance.RemoveListeners(this);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00027E4C File Offset: 0x0002604C
		protected override void OnInitialize()
		{
			base.OnInitialize();
			foreach (MobileParty mobileParty in MobileParty.All)
			{
				this.AddNewPartyVisualForParty(mobileParty, true);
			}
			CampaignEvents.MobilePartyCreated.AddNonSerializedListener(this, new Action<MobileParty>(this.OnMobilePartyCreated));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x00027ED4 File Offset: 0x000260D4
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			this.RemovePartyVisualForParty(mobileParty);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x00027EDD File Offset: 0x000260DD
		private void OnMobilePartyCreated(MobileParty mobileParty)
		{
			this.AddNewPartyVisualForParty(mobileParty, false);
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00027EE7 File Offset: 0x000260E7
		public MobilePartyVisual GetPartyVisual(PartyBase partyBase)
		{
			return this._partiesAndVisuals[partyBase];
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x00027EF5 File Offset: 0x000260F5
		internal void RegisterFadingVisual(MobilePartyVisual visual)
		{
			if (!this._fadingPartiesSet.Contains(visual))
			{
				this._fadingPartiesFlatten.Add(visual);
				this._fadingPartiesSet.Add(visual);
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x00027F20 File Offset: 0x00026120
		internal void UnRegisterFadingVisual(MobilePartyVisual visual)
		{
			if (this._fadingPartiesSet.Contains(visual))
			{
				int num = this._fadingPartiesFlatten.IndexOf(visual);
				this._fadingPartiesFlatten[num] = this._fadingPartiesFlatten[this._fadingPartiesFlatten.Count - 1];
				this._fadingPartiesFlatten.Remove(this._fadingPartiesFlatten[this._fadingPartiesFlatten.Count - 1]);
				this._fadingPartiesSet.Remove(visual);
			}
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00027FA0 File Offset: 0x000261A0
		private void AddNewPartyVisualForParty(MobileParty mobileParty, bool shouldTick = false)
		{
			if (!mobileParty.IsGarrison && !mobileParty.IsMilitia && !this._partiesAndVisuals.ContainsKey(mobileParty.Party))
			{
				MobilePartyVisual mobilePartyVisual = new MobilePartyVisual(mobileParty.Party);
				mobilePartyVisual.OnStartup();
				this._partiesAndVisuals.Add(mobileParty.Party, mobilePartyVisual);
				this._visualsFlattened.Add(mobilePartyVisual);
				if (shouldTick)
				{
					mobilePartyVisual.Tick(0.1f, 0.1f, ref this._dirtyPartyVisualCount, ref this._dirtyPartiesList);
				}
			}
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x00028020 File Offset: 0x00026220
		private void RemovePartyVisualForParty(MobileParty mobileParty)
		{
			MobilePartyVisual mobilePartyVisual;
			if (this._partiesAndVisuals.TryGetValue(mobileParty.Party, out mobilePartyVisual))
			{
				mobilePartyVisual.OnPartyRemoved();
				this._visualsFlattened.Remove(mobilePartyVisual);
				this._partiesAndVisuals.Remove(mobileParty.Party);
			}
		}

		// Token: 0x04000265 RID: 613
		private readonly Dictionary<PartyBase, MobilePartyVisual> _partiesAndVisuals = new Dictionary<PartyBase, MobilePartyVisual>();

		// Token: 0x04000266 RID: 614
		private readonly List<MobilePartyVisual> _visualsFlattened = new List<MobilePartyVisual>();

		// Token: 0x04000267 RID: 615
		private int _dirtyPartyVisualCount;

		// Token: 0x04000268 RID: 616
		private MobilePartyVisual[] _dirtyPartiesList = new MobilePartyVisual[2500];

		// Token: 0x04000269 RID: 617
		private readonly List<MobilePartyVisual> _fadingPartiesFlatten = new List<MobilePartyVisual>();

		// Token: 0x0400026A RID: 618
		private readonly HashSet<MobilePartyVisual> _fadingPartiesSet = new HashSet<MobilePartyVisual>();
	}
}
