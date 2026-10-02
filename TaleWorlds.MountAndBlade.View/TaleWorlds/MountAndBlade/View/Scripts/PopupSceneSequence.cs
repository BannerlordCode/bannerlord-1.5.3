using System;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade.View.Scripts
{
	// Token: 0x0200005F RID: 95
	public class PopupSceneSequence : ScriptComponentBehavior
	{
		// Token: 0x060003A7 RID: 935 RVA: 0x0001B795 File Offset: 0x00019995
		public void InitializeWithAgentVisuals(AgentVisuals visuals)
		{
			this._agentVisuals = visuals;
			this._time = 0f;
			this._triggered = false;
			this._state = 0;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0001B7BF File Offset: 0x000199BF
		protected override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0001B7D3 File Offset: 0x000199D3
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x060003AB RID: 939 RVA: 0x0001B7E0 File Offset: 0x000199E0
		protected override void OnTick(float dt)
		{
			this._time += dt;
			if (!this._triggered)
			{
				if (this._state == 0 && this._time >= this.InitialActivationTime)
				{
					this._triggered = true;
					this.OnInitialState();
				}
				if (this._state == 1 && this._time >= this.PositiveActivationTime)
				{
					this._triggered = true;
					this.OnPositiveState();
				}
				if (this._state == 2 && this._time >= this.NegativeActivationTime)
				{
					this._triggered = true;
					this.OnNegativeState();
				}
			}
		}

		// Token: 0x060003AC RID: 940 RVA: 0x0001B86E File Offset: 0x00019A6E
		public virtual void OnInitialState()
		{
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0001B870 File Offset: 0x00019A70
		public virtual void OnPositiveState()
		{
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001B872 File Offset: 0x00019A72
		public virtual void OnNegativeState()
		{
		}

		// Token: 0x060003AF RID: 943 RVA: 0x0001B874 File Offset: 0x00019A74
		public void SetInitialState()
		{
			this._triggered = false;
			this._state = 0;
			this._time = 0f;
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x0001B88F File Offset: 0x00019A8F
		public void SetPositiveState()
		{
			this._triggered = false;
			this._state = 1;
			this._time = 0f;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0001B8AA File Offset: 0x00019AAA
		public void SetNegativeState()
		{
			this._triggered = false;
			this._state = 2;
			this._time = 0f;
		}

		// Token: 0x0400020D RID: 525
		public float InitialActivationTime;

		// Token: 0x0400020E RID: 526
		public float PositiveActivationTime;

		// Token: 0x0400020F RID: 527
		public float NegativeActivationTime;

		// Token: 0x04000210 RID: 528
		protected AgentVisuals _agentVisuals;

		// Token: 0x04000211 RID: 529
		protected float _time;

		// Token: 0x04000212 RID: 530
		protected bool _triggered;

		// Token: 0x04000213 RID: 531
		protected int _state;
	}
}
