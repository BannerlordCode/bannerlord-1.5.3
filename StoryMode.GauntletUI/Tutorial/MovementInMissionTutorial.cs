using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000017 RID: 23
	[Tutorial("MovementInMissionTutorial")]
	public class MovementInMissionTutorial : TutorialItemBase
	{
		// Token: 0x0600006E RID: 110 RVA: 0x00002C9E File Offset: 0x00000E9E
		public MovementInMissionTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = string.Empty;
			base.MouseRequired = false;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00002CBF File Offset: 0x00000EBF
		public override bool IsConditionsMetForCompletion()
		{
			return this._playerMovedBackward && this._playerMovedLeft && this._playerMovedRight && this._playerMovedForward;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00002CE4 File Offset: 0x00000EE4
		public override void OnPlayerMovementFlagChanged(MissionPlayerMovementFlagsChangeEvent obj)
		{
			base.OnPlayerMovementFlagChanged(obj);
			this._playerMovedRight = this._playerMovedRight || (obj.MovementFlag & Agent.MovementControlFlag.StrafeRight) == Agent.MovementControlFlag.StrafeRight;
			this._playerMovedLeft = this._playerMovedLeft || (obj.MovementFlag & Agent.MovementControlFlag.StrafeLeft) == Agent.MovementControlFlag.StrafeLeft;
			this._playerMovedForward = this._playerMovedForward || (obj.MovementFlag & Agent.MovementControlFlag.Forward) == Agent.MovementControlFlag.Forward;
			this._playerMovedBackward = this._playerMovedBackward || (obj.MovementFlag & Agent.MovementControlFlag.Backward) == Agent.MovementControlFlag.Backward;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00002D68 File Offset: 0x00000F68
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00002D6B File Offset: 0x00000F6B
		public override bool IsConditionsMetForActivation()
		{
			return Mission.Current != null && Mission.Current.Mode != MissionMode.Deployment && !TutorialHelper.PlayerIsInAConversation && TutorialHelper.CurrentContext == TutorialContexts.Mission;
		}

		// Token: 0x0400001A RID: 26
		private bool _playerMovedForward;

		// Token: 0x0400001B RID: 27
		private bool _playerMovedBackward;

		// Token: 0x0400001C RID: 28
		private bool _playerMovedLeft;

		// Token: 0x0400001D RID: 29
		private bool _playerMovedRight;
	}
}
