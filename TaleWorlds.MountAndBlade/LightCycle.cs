using System;
using System.Collections.Generic;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200033C RID: 828
	public class LightCycle : ScriptComponentBehavior
	{
		// Token: 0x06002ECC RID: 11980 RVA: 0x000B5208 File Offset: 0x000B3408
		private void SetVisibility()
		{
			Light light = base.GameEntity.GetLight();
			float timeOfDay = base.Scene.TimeOfDay;
			this.visibility = timeOfDay < 6f || timeOfDay > 20f || base.Scene.IsAtmosphereIndoor || this.alwaysBurn;
			if (light != null)
			{
				light.SetVisibility(this.visibility);
			}
			foreach (WeakGameEntity weakGameEntity in base.GameEntity.GetChildren())
			{
				weakGameEntity.SetVisibilityExcludeParents(this.visibility);
			}
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x000B52C0 File Offset: 0x000B34C0
		protected internal override void OnInit()
		{
			base.OnInit();
			this.SetVisibility();
			if (!this.visibility)
			{
				List<WeakGameEntity> list = new List<WeakGameEntity>();
				base.GameEntity.GetChildrenRecursive(ref list);
				for (int i = list.Count - 1; i >= 0; i--)
				{
					base.Scene.RemoveEntity(list[i], 0);
				}
				base.GameEntity.RemoveScriptComponent(base.ScriptComponent.Pointer, 0);
			}
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x000B5337 File Offset: 0x000B3537
		protected internal override void OnEditorTick(float dt)
		{
			this.SetVisibility();
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x000B533F File Offset: 0x000B353F
		protected internal override bool MovesEntity()
		{
			return false;
		}

		// Token: 0x0400128C RID: 4748
		public bool alwaysBurn;

		// Token: 0x0400128D RID: 4749
		private bool visibility;
	}
}
