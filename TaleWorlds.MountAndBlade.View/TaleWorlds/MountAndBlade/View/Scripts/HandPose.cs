using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.Scripts
{
	// Token: 0x02000065 RID: 101
	public class HandPose : ScriptComponentBehavior
	{
		// Token: 0x060003EB RID: 1003 RVA: 0x0001DB6A File Offset: 0x0001BD6A
		protected override void OnEditorInit()
		{
			base.OnEditorInit();
			if (Game.Current == null)
			{
				this._editorGameManager = new EditorGameManager();
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001DB84 File Offset: 0x0001BD84
		protected override void OnEditorTick(float dt)
		{
			if (!this._isFinished && this._editorGameManager != null)
			{
				this._isFinished = !this._editorGameManager.DoLoadingForGameManager();
			}
			if (Game.Current != null && !this._initiliazed)
			{
				AnimationSystemData animationSystemData = Game.Current.DefaultMonster.FillAnimationSystemData(MBActionSet.GetActionSet(Game.Current.DefaultMonster.ActionSetCode), 1f, false);
				base.GameEntity.CreateSkeletonWithActionSet(ref animationSystemData);
				base.GameEntity.CopyComponentsToSkeleton();
				base.GameEntity.Skeleton.SetAgentActionChannel(0, in ActionIndexCache.act_tableau_hand_armor_pose, 0f, -0.2f, true, 0f);
				base.GameEntity.Skeleton.TickAnimationsAndForceUpdate(0.01f, base.GameEntity.GetGlobalFrame(), true);
				base.GameEntity.Skeleton.Freeze(false);
				base.GameEntity.Skeleton.TickAnimationsAndForceUpdate(0.001f, base.GameEntity.GetGlobalFrame(), false);
				base.GameEntity.Skeleton.SetAnimationParameterAtChannel(0, MBMath.ClampFloat(0f, 0f, 1f));
				base.GameEntity.Skeleton.SetUptoDate(false);
				base.GameEntity.Skeleton.Freeze(true);
				this._initiliazed = true;
			}
			if (this._initiliazed)
			{
				base.GameEntity.Skeleton.Freeze(false);
				base.GameEntity.Skeleton.TickAnimationsAndForceUpdate(0.001f, base.GameEntity.GetGlobalFrame(), false);
				base.GameEntity.Skeleton.SetAnimationParameterAtChannel(0, MBMath.ClampFloat(0f, 0f, 1f));
				base.GameEntity.Skeleton.SetUptoDate(false);
				base.GameEntity.Skeleton.Freeze(true);
			}
		}

		// Token: 0x0400024D RID: 589
		private MBGameManager _editorGameManager;

		// Token: 0x0400024E RID: 590
		private bool _initiliazed;

		// Token: 0x0400024F RID: 591
		private bool _isFinished;
	}
}
