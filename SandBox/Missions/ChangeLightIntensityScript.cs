using System;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace SandBox.Missions
{
	// Token: 0x02000058 RID: 88
	public class ChangeLightIntensityScript : ScriptComponentBehavior
	{
		// Token: 0x0600037D RID: 893 RVA: 0x000144F6 File Offset: 0x000126F6
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick;
		}

		// Token: 0x0600037E RID: 894 RVA: 0x000144F9 File Offset: 0x000126F9
		protected override void OnTick(float dt)
		{
			if (this._state == ChangeLightIntensityScript.State.None)
			{
				this._state = ChangeLightIntensityScript.State.Start;
			}
			this.OnTickInternal(dt);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00014511 File Offset: 0x00012711
		protected override void OnEditorTick(float dt)
		{
			this.OnTickInternal(dt);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x0001451C File Offset: 0x0001271C
		private void OnTickInternal(float dt)
		{
			switch (this._state)
			{
			case ChangeLightIntensityScript.State.Start:
				this._lightComponent = base.GameEntity.GetLight();
				this._initialIntensityCacheForPreviewButton = this._lightComponent.Intensity;
				if (this._waitBeforeChangeAsSeconds > 0f)
				{
					this._state = ChangeLightIntensityScript.State.WaitBeforeChange;
					return;
				}
				this._state = ChangeLightIntensityScript.State.ChangingIntensity;
				return;
			case ChangeLightIntensityScript.State.WaitBeforeChange:
				this._currentTimeDt += dt;
				if (this._currentTimeDt >= this._waitBeforeChangeAsSeconds)
				{
					this._state = ChangeLightIntensityScript.State.ChangingIntensity;
					return;
				}
				break;
			case ChangeLightIntensityScript.State.ChangingIntensity:
			{
				float num = 1f * this._changeSpeed;
				this._currentChangeAmount += num;
				this._lightComponent.Intensity += num;
				if (this._currentChangeAmount >= this._changeAmount)
				{
					this._state = ChangeLightIntensityScript.State.End;
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000145F0 File Offset: 0x000127F0
		protected override void OnEditorVariableChanged(string variableName)
		{
			base.OnEditorVariableChanged(variableName);
			if (variableName == "Preview")
			{
				if (this._state != ChangeLightIntensityScript.State.None && this._state != ChangeLightIntensityScript.State.End)
				{
					Debug.FailedAssert("The intensity change is already started, please click the \"Reset\" button first!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox\\Missions\\ChangeLightIntensityScript.cs", "OnEditorVariableChanged", 108);
				}
				else
				{
					this._currentChangeAmount = 0f;
					this._currentTimeDt = 0f;
					this._state = ChangeLightIntensityScript.State.Start;
				}
			}
			if (variableName == "Reset")
			{
				this._lightComponent.Intensity = this._initialIntensityCacheForPreviewButton;
				this._currentChangeAmount = 0f;
				this._currentTimeDt = 0f;
				this._state = ChangeLightIntensityScript.State.None;
			}
		}

		// Token: 0x040001C2 RID: 450
		[EditableScriptComponentVariable(true, "WaitBeforeChangeAsSeconds")]
		private float _waitBeforeChangeAsSeconds;

		// Token: 0x040001C3 RID: 451
		[EditableScriptComponentVariable(true, "ChangeAmount")]
		private float _changeAmount = 20f;

		// Token: 0x040001C4 RID: 452
		[EditableScriptComponentVariable(true, "ChangeSpeed")]
		private float _changeSpeed = 1f;

		// Token: 0x040001C5 RID: 453
		private ChangeLightIntensityScript.State _state;

		// Token: 0x040001C6 RID: 454
		private float _currentChangeAmount;

		// Token: 0x040001C7 RID: 455
		private float _currentTimeDt;

		// Token: 0x040001C8 RID: 456
		public SimpleButton Preview;

		// Token: 0x040001C9 RID: 457
		public SimpleButton Reset;

		// Token: 0x040001CA RID: 458
		private float _initialIntensityCacheForPreviewButton;

		// Token: 0x040001CB RID: 459
		private Light _lightComponent;

		// Token: 0x02000165 RID: 357
		private enum State
		{
			// Token: 0x040006E0 RID: 1760
			None,
			// Token: 0x040006E1 RID: 1761
			Start,
			// Token: 0x040006E2 RID: 1762
			WaitBeforeChange,
			// Token: 0x040006E3 RID: 1763
			ChangingIntensity,
			// Token: 0x040006E4 RID: 1764
			End
		}
	}
}
