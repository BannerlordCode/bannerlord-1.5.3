using System;
using SandBox.View.Map.Visuals;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.View.Map
{
	// Token: 0x02000041 RID: 65
	public class CampaignEntityVisualComponent : IEntityComponent
	{
		// Token: 0x0600020C RID: 524 RVA: 0x00014117 File Offset: 0x00012317
		public virtual void OnVisualTick(MapScreen screen, float realDt, float dt)
		{
		}

		// Token: 0x0600020D RID: 525 RVA: 0x00014119 File Offset: 0x00012319
		public virtual bool OnMouseClick(MapEntityVisual visualOfSelectedEntity, Vec3 intersectionPoint, PathFaceRecord mouseOverFaceIndex, bool isDoubleClick)
		{
			return false;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0001411C File Offset: 0x0001231C
		public virtual bool OnVisualIntersected(Ray mouseRay, UIntPtr[] intersectedEntityIDs, Intersection[] intersectionInfos, int entityCount, Vec3 worldMouseNear, Vec3 worldMouseFar, Vec3 terrainIntersectionPoint, ref MapEntityVisual hoveredVisual, ref MapEntityVisual selectedVisual)
		{
			return false;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0001411F File Offset: 0x0001231F
		public virtual void OnFrameTick(float dt)
		{
		}

		// Token: 0x06000210 RID: 528 RVA: 0x00014121 File Offset: 0x00012321
		public virtual void OnGameLoadFinished()
		{
		}

		// Token: 0x06000211 RID: 529 RVA: 0x00014123 File Offset: 0x00012323
		public virtual void OnTick(float realDt, float dt)
		{
		}

		// Token: 0x06000212 RID: 530 RVA: 0x00014125 File Offset: 0x00012325
		public virtual void ClearVisualMemory()
		{
		}

		// Token: 0x06000213 RID: 531 RVA: 0x00014127 File Offset: 0x00012327
		void IEntityComponent.OnInitialize()
		{
			this.OnInitialize();
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0001412F File Offset: 0x0001232F
		void IEntityComponent.OnFinalize()
		{
			this.OnFinalize();
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00014137 File Offset: 0x00012337
		protected virtual void OnInitialize()
		{
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00014139 File Offset: 0x00012339
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0001413B File Offset: 0x0001233B
		public virtual int Priority
		{
			get
			{
				return 0;
			}
		}
	}
}
