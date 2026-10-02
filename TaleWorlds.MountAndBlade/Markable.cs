using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000355 RID: 853
	public class Markable : ScriptComponentBehavior
	{
		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06003064 RID: 12388 RVA: 0x000BFB42 File Offset: 0x000BDD42
		// (set) Token: 0x06003065 RID: 12389 RVA: 0x000BFB4A File Offset: 0x000BDD4A
		private bool MarkerActive
		{
			get
			{
				return this._markerActive;
			}
			set
			{
				if (this._markerActive != value)
				{
					this._markerActive = value;
					base.SetScriptComponentToTick(this.GetTickRequirement());
				}
			}
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x000BFB68 File Offset: 0x000BDD68
		protected internal override void OnInit()
		{
			base.OnInit();
			this._marker = TaleWorlds.Engine.GameEntity.Instantiate(Mission.Current.Scene, "highlight_beam", base.GameEntity.GetGlobalFrame(), true);
			this.DeactivateMarker(true);
			this._destructibleComponent = base.GameEntity.GetFirstScriptOfType<DestructableComponent>();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x000BFBCB File Offset: 0x000BDDCB
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (this.MarkerActive)
			{
				return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x000BFBE4 File Offset: 0x000BDDE4
		protected internal override void OnTick(float dt)
		{
			if (this.MarkerActive)
			{
				if (this._destructibleComponent != null && this._destructibleComponent.IsDestroyed)
				{
					if (this._markerVisible)
					{
						this.DisableMarkerActivation();
						return;
					}
				}
				else if (this._markerVisible)
				{
					if (Mission.Current.CurrentTime - this._markerEventBeginningTime > this._markerActiveDuration)
					{
						this.DeactivateMarker(false);
						return;
					}
				}
				else if (!this._markerVisible && Mission.Current.CurrentTime - this._markerEventBeginningTime > this._markerPassiveDuration)
				{
					this.ActivateMarkerFor(this._markerActiveDuration, this._markerPassiveDuration);
				}
			}
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x000BFC7C File Offset: 0x000BDE7C
		public void DisableMarkerActivation()
		{
			this.MarkerActive = false;
			this.DeactivateMarker(false);
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x000BFC8C File Offset: 0x000BDE8C
		public void ActivateMarkerFor(float activeSeconds, float passiveSeconds)
		{
			if (this._destructibleComponent == null || !this._destructibleComponent.IsDestroyed)
			{
				this.MarkerActive = true;
				this._markerVisible = true;
				this._markerEventBeginningTime = Mission.Current.CurrentTime;
				this._markerActiveDuration = activeSeconds;
				this._markerPassiveDuration = passiveSeconds;
				this._marker.SetVisibilityExcludeParents(true);
				this._marker.BurstEntityParticle(true);
			}
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x000BFCF2 File Offset: 0x000BDEF2
		private void DeactivateMarker(bool onInitCalled = false)
		{
			this._markerVisible = false;
			this._marker.SetVisibilityExcludeParents(false);
			this._markerEventBeginningTime = ((!onInitCalled) ? Mission.Current.CurrentTime : 0f);
		}

		// Token: 0x0600306C RID: 12396 RVA: 0x000BFD21 File Offset: 0x000BDF21
		public void ResetPassiveDurationTimer()
		{
			if (!this._markerVisible && this.MarkerActive)
			{
				this._markerEventBeginningTime = Mission.Current.CurrentTime;
			}
		}

		// Token: 0x040013E8 RID: 5096
		public string MarkerPrefabName = "highlight_beam";

		// Token: 0x040013E9 RID: 5097
		private GameEntity _marker;

		// Token: 0x040013EA RID: 5098
		private DestructableComponent _destructibleComponent;

		// Token: 0x040013EB RID: 5099
		private bool _markerActive;

		// Token: 0x040013EC RID: 5100
		private bool _markerVisible;

		// Token: 0x040013ED RID: 5101
		private float _markerEventBeginningTime;

		// Token: 0x040013EE RID: 5102
		private float _markerActiveDuration;

		// Token: 0x040013EF RID: 5103
		private float _markerPassiveDuration;
	}
}
