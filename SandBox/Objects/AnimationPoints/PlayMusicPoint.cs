using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

namespace SandBox.Objects.AnimationPoints
{
	// Token: 0x02000055 RID: 85
	public class PlayMusicPoint : AnimationPoint
	{
		// Token: 0x06000363 RID: 867 RVA: 0x00013E8F File Offset: 0x0001208F
		protected override void OnInit()
		{
			base.OnInit();
			base.IsDisabledForPlayers = true;
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00013EAC File Offset: 0x000120AC
		public void StartLoop(SoundEvent trackEvent)
		{
			this._trackEvent = trackEvent;
			if (base.HasUser && MBActionSet.CheckActionAnimationClipExists(base.UserAgent.ActionSet, in this.LoopStartActionCode))
			{
				base.UserAgent.SetActionChannel(0, in this.LoopStartActionCode, true, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00013F17 File Offset: 0x00012117
		public void EndLoop()
		{
			if (this._trackEvent != null)
			{
				this._trackEvent = null;
				this.ChangeInstrument(null);
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00013F2F File Offset: 0x0001212F
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.HasUser)
			{
				return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00013F48 File Offset: 0x00012148
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._trackEvent != null && base.HasUser && MBActionSet.CheckActionAnimationClipExists(base.UserAgent.ActionSet, in this.LoopStartActionCode))
			{
				base.UserAgent.SetActionChannel(0, in this.LoopStartActionCode, this._hasInstrumentAttached, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
			}
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00013FC0 File Offset: 0x000121C0
		public override void OnUseStopped(Agent userAgent, bool isSuccessful, int preferenceIndex)
		{
			base.OnUseStopped(userAgent, isSuccessful, preferenceIndex);
			this.DefaultActionCode = ActionIndexCache.act_none;
			this.EndLoop();
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00013FDC File Offset: 0x000121DC
		public void ChangeInstrument(Tuple<InstrumentData, float> instrument)
		{
			InstrumentData instrumentData = ((instrument != null) ? instrument.Item1 : null);
			if (this._instrumentData != instrumentData)
			{
				this._instrumentData = instrumentData;
				if (base.HasUser && base.UserAgent.IsActive())
				{
					if (base.UserAgent.IsSitting())
					{
						this.LoopStartAction = ((instrumentData == null) ? "act_sit_1" : instrumentData.SittingAction);
					}
					else
					{
						this.LoopStartAction = ((instrumentData == null) ? "act_stand_1" : instrumentData.StandingAction);
						this.ArriveAction = "";
					}
					this.ActionSpeed = ((instrument != null) ? instrument.Item2 : 1f);
					this.SetActionCodes();
					base.ClearAssignedItems();
					base.UserAgent.SetActionChannel(0, in this.LoopStartActionCode, false, (AnimFlags)((long)Math.Min(base.UserAgent.GetCurrentActionPriority(0), 73)), 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true);
					if (this._instrumentData != null)
					{
						foreach (ValueTuple<HumanBone, string> valueTuple in this._instrumentData.InstrumentEntities)
						{
							AnimationPoint.ItemForBone itemForBone = new AnimationPoint.ItemForBone(valueTuple.Item1, valueTuple.Item2, true);
							base.AssignItemToBone(itemForBone);
						}
						base.AddItemsToAgent();
						this._hasInstrumentAttached = !this._instrumentData.IsDataWithoutInstrument;
					}
				}
			}
		}

		// Token: 0x040001B3 RID: 435
		private InstrumentData _instrumentData;

		// Token: 0x040001B4 RID: 436
		private SoundEvent _trackEvent;

		// Token: 0x040001B5 RID: 437
		private bool _hasInstrumentAttached;
	}
}
