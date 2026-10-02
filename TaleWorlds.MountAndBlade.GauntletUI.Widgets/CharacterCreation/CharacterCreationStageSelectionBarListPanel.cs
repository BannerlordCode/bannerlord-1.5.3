using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterCreation
{
	// Token: 0x0200018A RID: 394
	public class CharacterCreationStageSelectionBarListPanel : ListPanel
	{
		// Token: 0x06001485 RID: 5253 RVA: 0x00038013 File Offset: 0x00036213
		public CharacterCreationStageSelectionBarListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x0003803C File Offset: 0x0003623C
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			this.RefreshButtonList();
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0003804C File Offset: 0x0003624C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.BarFillWidget != null && this.TotalStagesCount != 0 && this._buttonsInitialized && this.CurrentStageIndex != -1)
			{
				this.BarFillWidget.ScaledSuggestedWidth = this.BarCanvasWidget.Size.X - this._stageButtonsList[this._stageButtonsList.Count - 1 - this.CurrentStageIndex].LocalPosition.X;
			}
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x000380C8 File Offset: 0x000362C8
		private void RefreshButtonList()
		{
			if (!this._buttonsInitialized)
			{
				this._stageButtonsList = new List<ButtonWidget>();
				if (base.HasChild(this.StageButtonTemplate))
				{
					base.RemoveChild(this.StageButtonTemplate);
				}
				base.RemoveAllChildren();
				if (this.StageButtonTemplate != null && this.EmptyButtonBrush != null && this.FullButtonBrush != null && this.FullBrightButtonBrush != null)
				{
					if (this.TotalStagesCount == 0)
					{
						this.BarCanvasWidget.IsVisible = false;
						base.IsVisible = false;
					}
					else
					{
						for (int i = 0; i < this.TotalStagesCount; i++)
						{
							ButtonWidget buttonWidget = new ButtonWidget(base.Context);
							base.AddChild(buttonWidget);
							buttonWidget.Brush = this.StageButtonTemplate.ReadOnlyBrush;
							bool flag = false;
							if (i == this.CurrentStageIndex)
							{
								buttonWidget.Brush = base.EventManager.Context.Brushes.First<Brush>((Brush b) => b.Name == this.FullBrightButtonBrush);
							}
							else if (i <= this.OpenedStageIndex || (this.OpenedStageIndex == -1 && i < this.CurrentStageIndex))
							{
								buttonWidget.Brush = base.EventManager.Context.Brushes.First<Brush>((Brush b) => b.Name == this.FullButtonBrush);
								flag = true;
							}
							else
							{
								buttonWidget.Brush = base.EventManager.Context.Brushes.First<Brush>((Brush b) => b.Name == this.EmptyButtonBrush);
							}
							buttonWidget.DoNotAcceptEvents = !flag;
							buttonWidget.SuggestedHeight = this.StageButtonTemplate.SuggestedHeight;
							buttonWidget.SuggestedWidth = this.StageButtonTemplate.SuggestedWidth;
							buttonWidget.DoNotPassEventsToChildren = this.StageButtonTemplate.DoNotPassEventsToChildren;
							buttonWidget.ClickEventHandlers.Add(new Action<Widget>(this.OnStageSelection));
							this._stageButtonsList.Add(buttonWidget);
						}
					}
					this._buttonsInitialized = true;
				}
			}
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0003829C File Offset: 0x0003649C
		private void OnStageSelection(Widget stageButton)
		{
			int num = this._stageButtonsList.IndexOf(stageButton as ButtonWidget);
			base.EventFired("OnStageSelection", new object[] { num });
		}

		// Token: 0x17000745 RID: 1861
		// (get) Token: 0x0600148A RID: 5258 RVA: 0x000382D5 File Offset: 0x000364D5
		// (set) Token: 0x0600148B RID: 5259 RVA: 0x000382DD File Offset: 0x000364DD
		[Editor(false)]
		public ButtonWidget StageButtonTemplate
		{
			get
			{
				return this._stageButtonTemplate;
			}
			set
			{
				if (this._stageButtonTemplate != value)
				{
					this._stageButtonTemplate = value;
					base.OnPropertyChanged<ButtonWidget>(value, "StageButtonTemplate");
					if (value != null)
					{
						base.RemoveChild(value);
					}
				}
			}
		}

		// Token: 0x17000746 RID: 1862
		// (get) Token: 0x0600148C RID: 5260 RVA: 0x00038305 File Offset: 0x00036505
		// (set) Token: 0x0600148D RID: 5261 RVA: 0x0003830D File Offset: 0x0003650D
		[Editor(false)]
		public Widget BarFillWidget
		{
			get
			{
				return this._barFillWidget;
			}
			set
			{
				if (this._barFillWidget != value)
				{
					this._barFillWidget = value;
					base.OnPropertyChanged<Widget>(value, "BarFillWidget");
				}
			}
		}

		// Token: 0x17000747 RID: 1863
		// (get) Token: 0x0600148E RID: 5262 RVA: 0x0003832B File Offset: 0x0003652B
		// (set) Token: 0x0600148F RID: 5263 RVA: 0x00038333 File Offset: 0x00036533
		[Editor(false)]
		public Widget BarCanvasWidget
		{
			get
			{
				return this._barCanvasWidget;
			}
			set
			{
				if (this._barCanvasWidget != value)
				{
					this._barCanvasWidget = value;
					base.OnPropertyChanged<Widget>(value, "BarCanvasWidget");
				}
			}
		}

		// Token: 0x17000748 RID: 1864
		// (get) Token: 0x06001490 RID: 5264 RVA: 0x00038351 File Offset: 0x00036551
		// (set) Token: 0x06001491 RID: 5265 RVA: 0x00038359 File Offset: 0x00036559
		[Editor(false)]
		public int CurrentStageIndex
		{
			get
			{
				return this._currentStageIndex;
			}
			set
			{
				if (this._currentStageIndex != value)
				{
					this._currentStageIndex = value;
					base.OnPropertyChanged(value, "CurrentStageIndex");
					this._buttonsInitialized = false;
				}
			}
		}

		// Token: 0x17000749 RID: 1865
		// (get) Token: 0x06001492 RID: 5266 RVA: 0x0003837E File Offset: 0x0003657E
		// (set) Token: 0x06001493 RID: 5267 RVA: 0x00038386 File Offset: 0x00036586
		[Editor(false)]
		public int TotalStagesCount
		{
			get
			{
				return this._totalStagesCount;
			}
			set
			{
				if (this._totalStagesCount != value)
				{
					this._totalStagesCount = value;
					base.OnPropertyChanged(value, "TotalStagesCount");
				}
			}
		}

		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06001494 RID: 5268 RVA: 0x000383A4 File Offset: 0x000365A4
		// (set) Token: 0x06001495 RID: 5269 RVA: 0x000383AC File Offset: 0x000365AC
		[Editor(false)]
		public int OpenedStageIndex
		{
			get
			{
				return this._openedStageIndex;
			}
			set
			{
				if (this._openedStageIndex != value)
				{
					this._openedStageIndex = value;
					base.OnPropertyChanged(value, "OpenedStageIndex");
				}
			}
		}

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06001496 RID: 5270 RVA: 0x000383CA File Offset: 0x000365CA
		// (set) Token: 0x06001497 RID: 5271 RVA: 0x000383D2 File Offset: 0x000365D2
		[Editor(false)]
		public string FullButtonBrush
		{
			get
			{
				return this._fullButtonBrush;
			}
			set
			{
				if (this._fullButtonBrush != value)
				{
					this._fullButtonBrush = value;
					base.OnPropertyChanged<string>(value, "FullButtonBrush");
				}
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06001498 RID: 5272 RVA: 0x000383F5 File Offset: 0x000365F5
		// (set) Token: 0x06001499 RID: 5273 RVA: 0x000383FD File Offset: 0x000365FD
		[Editor(false)]
		public string EmptyButtonBrush
		{
			get
			{
				return this._emptyButtonBrush;
			}
			set
			{
				if (this._emptyButtonBrush != value)
				{
					this._emptyButtonBrush = value;
					base.OnPropertyChanged<string>(value, "EmptyButtonBrush");
				}
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x0600149A RID: 5274 RVA: 0x00038420 File Offset: 0x00036620
		// (set) Token: 0x0600149B RID: 5275 RVA: 0x00038428 File Offset: 0x00036628
		[Editor(false)]
		public string FullBrightButtonBrush
		{
			get
			{
				return this._fullBrightButtonBrush;
			}
			set
			{
				if (this._fullBrightButtonBrush != value)
				{
					this._fullBrightButtonBrush = value;
					base.OnPropertyChanged<string>(value, "FullBrightButtonBrush");
				}
			}
		}

		// Token: 0x0400095A RID: 2394
		private List<ButtonWidget> _stageButtonsList = new List<ButtonWidget>();

		// Token: 0x0400095B RID: 2395
		private bool _buttonsInitialized;

		// Token: 0x0400095C RID: 2396
		private ButtonWidget _stageButtonTemplate;

		// Token: 0x0400095D RID: 2397
		private int _currentStageIndex = -1;

		// Token: 0x0400095E RID: 2398
		private int _totalStagesCount = -1;

		// Token: 0x0400095F RID: 2399
		private int _openedStageIndex = -1;

		// Token: 0x04000960 RID: 2400
		private string _fullButtonBrush;

		// Token: 0x04000961 RID: 2401
		private string _emptyButtonBrush;

		// Token: 0x04000962 RID: 2402
		private string _fullBrightButtonBrush;

		// Token: 0x04000963 RID: 2403
		private Widget _barFillWidget;

		// Token: 0x04000964 RID: 2404
		private Widget _barCanvasWidget;
	}
}
