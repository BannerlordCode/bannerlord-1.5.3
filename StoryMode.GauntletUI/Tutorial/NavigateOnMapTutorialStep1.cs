using System;
using SandBox.GauntletUI.Tutorial;
using SandBox.View.Map;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.Core;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000024 RID: 36
	[Tutorial("NavigateOnMapTutorialStep1")]
	public class NavigateOnMapTutorialStep1 : TutorialItemBase
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00003533 File Offset: 0x00001733
		public NavigateOnMapTutorialStep1()
		{
			base.Placement = TutorialItemVM.ItemPlacements.Right;
			base.HighlightedVisualElementID = string.Empty;
			base.MouseRequired = true;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x0000355F File Offset: 0x0000175F
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.MapWindow;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00003562 File Offset: 0x00001762
		public override bool IsConditionsMetForActivation()
		{
			return !TutorialHelper.TownMenuIsOpen && !TutorialHelper.VillageMenuIsOpen && TutorialHelper.CurrentContext == TutorialContexts.MapWindow;
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x0000357C File Offset: 0x0000177C
		public override void OnMainMapCameraMove(MapScreen.MainMapCameraMoveEvent obj)
		{
			base.OnMainMapCameraMove(obj);
			this._movedPosition = obj.PositionChanged || this._movedPosition;
			this._movedRotation = obj.RotationChanged || this._movedRotation;
			if (this._movedRotation && this._movedPosition && this._completionTime == DateTime.MinValue)
			{
				this._completionTime = TutorialHelper.CurrentTime;
			}
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000035EC File Offset: 0x000017EC
		public override bool IsConditionsMetForCompletion()
		{
			return !(this._completionTime == DateTime.MinValue) && (TutorialHelper.CurrentTime - this._completionTime).TotalSeconds > 2.0;
		}

		// Token: 0x04000030 RID: 48
		private bool _movedPosition;

		// Token: 0x04000031 RID: 49
		private bool _movedRotation;

		// Token: 0x04000032 RID: 50
		private const float _delayInSeconds = 2f;

		// Token: 0x04000033 RID: 51
		private DateTime _completionTime = DateTime.MinValue;
	}
}
