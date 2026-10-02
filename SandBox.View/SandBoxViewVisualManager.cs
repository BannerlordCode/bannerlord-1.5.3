using System;
using System.Collections.Generic;
using SandBox.View.Map;
using SandBox.View.Map.Visuals;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.View
{
	// Token: 0x0200000E RID: 14
	public class SandBoxViewVisualManager
	{
		// Token: 0x06000062 RID: 98 RVA: 0x0000410C File Offset: 0x0000230C
		public SandBoxViewVisualManager()
		{
			this._components = new EntitySystem<CampaignEntityVisualComponent>();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004120 File Offset: 0x00002320
		public static void VisualTick(MapScreen screen, float realDt, float dt)
		{
			foreach (CampaignEntityVisualComponent campaignEntityVisualComponent in SandBoxViewSubModule.SandBoxViewVisualManager.GetComponents())
			{
				campaignEntityVisualComponent.OnVisualTick(screen, realDt, dt);
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00004178 File Offset: 0x00002378
		public static void OnTick(float realDt, float dt)
		{
			foreach (CampaignEntityVisualComponent campaignEntityVisualComponent in SandBoxViewSubModule.SandBoxViewVisualManager.GetComponents())
			{
				campaignEntityVisualComponent.OnTick(realDt, dt);
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000041D0 File Offset: 0x000023D0
		public static void ClearVisualMemory()
		{
			foreach (CampaignEntityVisualComponent campaignEntityVisualComponent in SandBoxViewSubModule.SandBoxViewVisualManager.GetComponents())
			{
				campaignEntityVisualComponent.ClearVisualMemory();
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00004224 File Offset: 0x00002424
		public static void OnFrameTick(float dt)
		{
			foreach (CampaignEntityVisualComponent campaignEntityVisualComponent in SandBoxViewSubModule.SandBoxViewVisualManager.GetComponents())
			{
				campaignEntityVisualComponent.OnFrameTick(dt);
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000427C File Offset: 0x0000247C
		public static bool OnMouseClick(MapEntityVisual visualOfSelectedEntity, Vec3 intersectionPoint, PathFaceRecord mouseOverFaceIndex, bool isDoubleClick)
		{
			bool flag = false;
			foreach (CampaignEntityVisualComponent campaignEntityVisualComponent in SandBoxViewSubModule.SandBoxViewVisualManager.GetComponents())
			{
				flag |= campaignEntityVisualComponent.OnMouseClick(visualOfSelectedEntity, intersectionPoint, mouseOverFaceIndex, isDoubleClick);
			}
			return flag;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000042DC File Offset: 0x000024DC
		public static void OnGameLoadFinished()
		{
			foreach (CampaignEntityVisualComponent campaignEntityVisualComponent in SandBoxViewSubModule.SandBoxViewVisualManager.GetComponents())
			{
				campaignEntityVisualComponent.OnGameLoadFinished();
			}
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00004330 File Offset: 0x00002530
		public TComponent GetEntityComponent<TComponent>() where TComponent : CampaignEntityVisualComponent
		{
			EntitySystem<CampaignEntityVisualComponent> components = this._components;
			if (components == null)
			{
				return default(TComponent);
			}
			return components.GetComponent<TComponent>();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004356 File Offset: 0x00002556
		public TComponent AddEntityComponent<TComponent>() where TComponent : CampaignEntityVisualComponent, new()
		{
			TComponent tcomponent = this._components.AddComponent<TComponent>();
			this.SortComponents();
			return tcomponent;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004369 File Offset: 0x00002569
		public void RemoveEntityComponent<TComponent>() where TComponent : CampaignEntityVisualComponent
		{
			this._components.RemoveComponent<TComponent>();
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004376 File Offset: 0x00002576
		public void Finalize<TComponent>(TComponent component) where TComponent : CampaignEntityVisualComponent
		{
			this._components.Finalize(component);
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00004389 File Offset: 0x00002589
		public void RemoveEntityComponent<TComponent>(TComponent component) where TComponent : CampaignEntityVisualComponent
		{
			this._components.RemoveComponent(component);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000439C File Offset: 0x0000259C
		public List<TComponent> GetComponents<TComponent>() where TComponent : CampaignEntityVisualComponent
		{
			return this._components.GetComponents<TComponent>();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000043A9 File Offset: 0x000025A9
		public MBList<CampaignEntityVisualComponent> GetComponents()
		{
			return this._components.GetComponents();
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000043B6 File Offset: 0x000025B6
		private void SortComponents()
		{
			this._components.SortComponents<CampaignEntityVisualComponent>(SandBoxViewVisualManager._comparisonDelegate);
		}

		// Token: 0x04000016 RID: 22
		private EntitySystem<CampaignEntityVisualComponent> _components;

		// Token: 0x04000017 RID: 23
		private static readonly Comparison<CampaignEntityVisualComponent> _comparisonDelegate = (CampaignEntityVisualComponent x, CampaignEntityVisualComponent y) => x.Priority.CompareTo(y.Priority);
	}
}
