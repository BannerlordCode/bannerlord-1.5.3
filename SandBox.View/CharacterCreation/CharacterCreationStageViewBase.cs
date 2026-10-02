using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;
using TaleWorlds.ScreenSystem;

namespace SandBox.View.CharacterCreation
{
	// Token: 0x02000081 RID: 129
	public abstract class CharacterCreationStageViewBase : ICharacterCreationStageListener
	{
		// Token: 0x0600058B RID: 1419 RVA: 0x00029704 File Offset: 0x00027904
		protected CharacterCreationStageViewBase(ControlCharacterCreationStage affirmativeAction, ControlCharacterCreationStage negativeAction, ControlCharacterCreationStage refreshAction, ControlCharacterCreationStageReturnInt getCurrentStageIndexAction, ControlCharacterCreationStageReturnInt getTotalStageCountAction, ControlCharacterCreationStageReturnInt getFurthestIndexAction, ControlCharacterCreationStageWithInt goToIndexAction)
		{
			this._affirmativeAction = affirmativeAction;
			this._negativeAction = negativeAction;
			this._refreshAction = refreshAction;
			this._getTotalStageCountAction = getTotalStageCountAction;
			this._getCurrentStageIndexAction = getCurrentStageIndexAction;
			this._getFurthestIndexAction = getFurthestIndexAction;
			this._goToIndexAction = goToIndexAction;
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0002976B File Offset: 0x0002796B
		public virtual void SetGenericScene(Scene scene)
		{
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0002976D File Offset: 0x0002796D
		protected virtual void OnRefresh()
		{
			this._refreshAction();
		}

		// Token: 0x0600058E RID: 1422
		public abstract IEnumerable<ScreenLayer> GetLayers();

		// Token: 0x0600058F RID: 1423
		public abstract void NextStage();

		// Token: 0x06000590 RID: 1424
		public abstract void PreviousStage();

		// Token: 0x06000591 RID: 1425 RVA: 0x0002977A File Offset: 0x0002797A
		void ICharacterCreationStageListener.OnStageFinalize()
		{
			this.OnFinalize();
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00029782 File Offset: 0x00027982
		protected virtual void OnFinalize()
		{
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00029784 File Offset: 0x00027984
		public virtual void Tick(float dt)
		{
		}

		// Token: 0x06000594 RID: 1428
		public abstract int GetVirtualStageCount();

		// Token: 0x06000595 RID: 1429 RVA: 0x00029786 File Offset: 0x00027986
		public virtual void GoToIndex(int index)
		{
			this._goToIndexAction(index);
		}

		// Token: 0x06000596 RID: 1430
		public abstract void LoadEscapeMenuMovie();

		// Token: 0x06000597 RID: 1431
		public abstract void ReleaseEscapeMenuMovie();

		// Token: 0x06000598 RID: 1432 RVA: 0x00029794 File Offset: 0x00027994
		public void HandleEscapeMenu(CharacterCreationStageViewBase view, ScreenLayer screenLayer)
		{
			if (screenLayer.Input.IsHotKeyReleased("ToggleEscapeMenu"))
			{
				if (this._isEscapeOpen)
				{
					this.RemoveEscapeMenu(view);
					return;
				}
				this.OpenEscapeMenu(view);
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x000297BF File Offset: 0x000279BF
		private void OpenEscapeMenu(CharacterCreationStageViewBase view)
		{
			view.LoadEscapeMenuMovie();
			this._isEscapeOpen = true;
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x000297CE File Offset: 0x000279CE
		private void RemoveEscapeMenu(CharacterCreationStageViewBase view)
		{
			view.ReleaseEscapeMenuMovie();
			this._isEscapeOpen = false;
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x000297E0 File Offset: 0x000279E0
		public List<EscapeMenuItemVM> GetEscapeMenuItems(CharacterCreationStageViewBase view)
		{
			TextObject characterCreationDisabledReason = GameTexts.FindText("str_pause_menu_disabled_hint", "CharacterCreation");
			List<EscapeMenuItemVM> list = new List<EscapeMenuItemVM>();
			list.Add(new EscapeMenuItemVM(new TextObject("{=5Saniypu}Resume", null), delegate(object o)
			{
				this.RemoveEscapeMenu(view);
			}, null, () => new Tuple<bool, TextObject>(false, null), true));
			list.Add(new EscapeMenuItemVM(new TextObject("{=PXT6aA4J}Campaign Options", null), delegate(object o)
			{
			}, null, () => new Tuple<bool, TextObject>(true, characterCreationDisabledReason), false));
			list.Add(new EscapeMenuItemVM(new TextObject("{=NqarFr4P}Options", null), delegate(object o)
			{
			}, null, () => new Tuple<bool, TextObject>(true, characterCreationDisabledReason), false));
			list.Add(new EscapeMenuItemVM(new TextObject("{=bV75iwKa}Save", null), delegate(object o)
			{
			}, null, () => new Tuple<bool, TextObject>(true, characterCreationDisabledReason), false));
			list.Add(new EscapeMenuItemVM(new TextObject("{=e0KdfaNe}Save As", null), delegate(object o)
			{
			}, null, () => new Tuple<bool, TextObject>(true, characterCreationDisabledReason), false));
			list.Add(new EscapeMenuItemVM(new TextObject("{=9NuttOBC}Load", null), delegate(object o)
			{
			}, null, () => new Tuple<bool, TextObject>(true, characterCreationDisabledReason), false));
			list.Add(new EscapeMenuItemVM(new TextObject("{=AbEh2y8o}Save And Exit", null), delegate(object o)
			{
			}, null, () => new Tuple<bool, TextObject>(true, characterCreationDisabledReason), false));
			list.Add(new EscapeMenuItemVM(new TextObject("{=RamV6yLM}Exit to Main Menu", null), delegate(object o)
			{
				this.RemoveEscapeMenu(view);
				view.OnFinalize();
				MBGameManager.EndGame();
			}, null, () => new Tuple<bool, TextObject>(false, null), false));
			return list;
		}

		// Token: 0x0400029A RID: 666
		protected readonly ControlCharacterCreationStage _affirmativeAction;

		// Token: 0x0400029B RID: 667
		protected readonly ControlCharacterCreationStage _negativeAction;

		// Token: 0x0400029C RID: 668
		protected readonly ControlCharacterCreationStage _refreshAction;

		// Token: 0x0400029D RID: 669
		protected readonly ControlCharacterCreationStageReturnInt _getTotalStageCountAction;

		// Token: 0x0400029E RID: 670
		protected readonly ControlCharacterCreationStageReturnInt _getCurrentStageIndexAction;

		// Token: 0x0400029F RID: 671
		protected readonly ControlCharacterCreationStageReturnInt _getFurthestIndexAction;

		// Token: 0x040002A0 RID: 672
		protected readonly ControlCharacterCreationStageWithInt _goToIndexAction;

		// Token: 0x040002A1 RID: 673
		protected readonly Vec3 _cameraPosition = new Vec3(6.45f, 4.35f, 1.6f, -1f);

		// Token: 0x040002A2 RID: 674
		private bool _isEscapeOpen;
	}
}
