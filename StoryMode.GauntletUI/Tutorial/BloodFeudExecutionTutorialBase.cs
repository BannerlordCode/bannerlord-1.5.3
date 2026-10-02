using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SceneInformationPopupTypes;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000049 RID: 73
	public abstract class BloodFeudExecutionTutorialBase : TutorialItemBase
	{
		// Token: 0x0600015E RID: 350 RVA: 0x0000486A File Offset: 0x00002A6A
		protected BloodFeudExecutionTutorialBase()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = string.Empty;
			base.MouseRequired = true;
		}

		// Token: 0x0600015F RID: 351
		protected abstract bool IsRelevantToBloodFeudState(Hero victimHero);

		// Token: 0x06000160 RID: 352 RVA: 0x0000488C File Offset: 0x00002A8C
		private bool IsCurrentSceneNotificationRelevant()
		{
			HeroExecutionSceneNotificationData heroExecutionSceneNotificationData;
			return (heroExecutionSceneNotificationData = MBInformationManager.GetActiveSceneNotificationData() as HeroExecutionSceneNotificationData) != null && heroExecutionSceneNotificationData.IsPlayerExecutionPrompt && heroExecutionSceneNotificationData.Victim != null && this.IsRelevantToBloodFeudState(heroExecutionSceneNotificationData.Victim);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000048C5 File Offset: 0x00002AC5
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.SceneNotification;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000048C9 File Offset: 0x00002AC9
		public override bool IsConditionsMetForActivation()
		{
			if (this.IsCurrentSceneNotificationRelevant())
			{
				this._isShown = true;
				return true;
			}
			return false;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000048DD File Offset: 0x00002ADD
		public override bool IsConditionsMetForCompletion()
		{
			return this._isShown && !this.IsCurrentSceneNotificationRelevant();
		}

		// Token: 0x0400005F RID: 95
		private bool _isShown;
	}
}
