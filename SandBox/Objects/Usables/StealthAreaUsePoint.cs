using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Objects.Usables
{
	// Token: 0x02000050 RID: 80
	public class StealthAreaUsePoint : UsableMissionObject
	{
		// Token: 0x060002EE RID: 750 RVA: 0x00010C52 File Offset: 0x0000EE52
		public StealthAreaUsePoint()
			: base(false)
		{
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00010C64 File Offset: 0x0000EE64
		protected override void OnInit()
		{
			base.OnInit();
			this._isAlreadyUsed = false;
			this.ActionMessage = GameTexts.FindText(string.IsNullOrEmpty(this.ActionStringId) ? "str_call_troops" : this.ActionStringId, null);
			this.ActionMessage.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			this.DescriptionMessage = (string.IsNullOrEmpty(this.DescriptionStringId) ? TextObject.GetEmpty() : GameTexts.FindText(this.DescriptionStringId, null));
			foreach (WeakGameEntity weakGameEntity in base.GameEntity.GetChildren())
			{
				foreach (WeakGameEntity weakGameEntity2 in weakGameEntity.GetChildren())
				{
					if (weakGameEntity2.Name.Equals("highlight_pointer_glow_ground"))
					{
						this._highlightGameEntity = weakGameEntity2;
						break;
					}
				}
				if (this._highlightGameEntity != null)
				{
					break;
				}
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00010D98 File Offset: 0x0000EF98
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return this.DescriptionMessage;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00010DA0 File Offset: 0x0000EFA0
		public override void OnUse(Agent userAgent, sbyte agentBoneIndex)
		{
			base.OnUse(userAgent, agentBoneIndex);
			if (!this.IsInCombat())
			{
				if (userAgent.IsMainAgent)
				{
					string text = "event:/mission/combat/pickup_arrows";
					Vec3 position = userAgent.Position;
					SoundManager.StartOneShotEvent(text, in position);
					this._isAlreadyUsed = true;
					this._highlightGameEntity.SetVisibilityExcludeParents(false);
					userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
				}
				this.DisableAgentAIs();
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00010DFA File Offset: 0x0000EFFA
		public override void OnUseStopped(Agent userAgent, bool isSuccessful, int preferenceIndex)
		{
			base.OnUseStopped(userAgent, isSuccessful, preferenceIndex);
			if (this.LockUserFrames || this.LockUserPositions)
			{
				userAgent.ClearTargetFrame();
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00010E1C File Offset: 0x0000F01C
		public void DisableAgentAIs()
		{
			foreach (Agent agent in Mission.Current.Agents)
			{
				if (agent.IsActive() && agent.IsAIControlled)
				{
					agent.SetIsAIPaused(true);
					WorldPosition worldPosition = new WorldPosition(Mission.Current.Scene, agent.Position);
					agent.SetScriptedPosition(ref worldPosition, false, Agent.AIScriptedFrameFlags.None);
				}
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00010EA4 File Offset: 0x0000F0A4
		public override bool IsDisabledForAgent(Agent agent)
		{
			return !agent.IsMainAgent && (this._isAlreadyUsed || !this._isEnabled);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00010EC3 File Offset: 0x0000F0C3
		public override bool IsUsableByAgent(Agent userAgent)
		{
			return userAgent.IsMainAgent && !this._isAlreadyUsed && this._isEnabled && !this.IsInCombat();
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00010EE8 File Offset: 0x0000F0E8
		private bool IsInCombat()
		{
			bool flag = false;
			foreach (Agent agent in Mission.Current.AllAgents)
			{
				if (agent.IsActive())
				{
					Agent.AIStateFlag aistateFlag = Agent.AIStateFlag.Alarmed;
					if ((agent.AIStateFlags & aistateFlag) == aistateFlag)
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00010F54 File Offset: 0x0000F154
		public void EnableStealthAreaUsePoint()
		{
			if (!this._isEnabled)
			{
				string text = "event:/ui/notification/quest_update";
				Vec3 globalPosition = base.GameEntity.GlobalPosition;
				SoundManager.StartOneShotEvent(text, in globalPosition);
			}
			this._highlightGameEntity.SetVisibilityExcludeParents(true);
			this._isEnabled = true;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00010F98 File Offset: 0x0000F198
		public void DisableStealthAreaUsePoint()
		{
			this._isEnabled = false;
			this._highlightGameEntity.SetVisibilityExcludeParents(false);
		}

		// Token: 0x04000149 RID: 329
		private const string HighlightEntityName = "highlight_pointer_glow_ground";

		// Token: 0x0400014A RID: 330
		private bool _isEnabled = true;

		// Token: 0x0400014B RID: 331
		private bool _isAlreadyUsed;

		// Token: 0x0400014C RID: 332
		private WeakGameEntity _highlightGameEntity;

		// Token: 0x0400014D RID: 333
		public string ActionStringId;

		// Token: 0x0400014E RID: 334
		public string DescriptionStringId;
	}
}
