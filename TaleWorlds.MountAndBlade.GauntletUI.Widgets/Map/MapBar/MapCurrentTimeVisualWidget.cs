using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapBar
{
	// Token: 0x02000130 RID: 304
	public class MapCurrentTimeVisualWidget : Widget
	{
		// Token: 0x06000FFA RID: 4090 RVA: 0x0002C470 File Offset: 0x0002A670
		public MapCurrentTimeVisualWidget(UIContext context)
			: base(context)
		{
			base.AddState("Disabled");
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x0002C484 File Offset: 0x0002A684
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (base.IsDisabled)
			{
				this.SetState("Disabled");
				return;
			}
			this.SetState("Default");
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			switch (this.CurrentTimeState)
			{
			case 0:
			case 6:
				flag3 = true;
				break;
			case 1:
			case 3:
				flag = true;
				break;
			case 2:
			case 4:
			case 5:
				flag2 = true;
				break;
			}
			this.PlayButton.IsSelected = flag;
			this.FastForwardButton.IsSelected = flag2;
			this.PauseButton.IsSelected = flag3;
		}

		// Token: 0x170005AB RID: 1451
		// (get) Token: 0x06000FFC RID: 4092 RVA: 0x0002C516 File Offset: 0x0002A716
		// (set) Token: 0x06000FFD RID: 4093 RVA: 0x0002C51E File Offset: 0x0002A71E
		[Editor(false)]
		public int CurrentTimeState
		{
			get
			{
				return this._currenTimeState;
			}
			set
			{
				if (this._currenTimeState != value)
				{
					this._currenTimeState = value;
					base.OnPropertyChanged(value, "CurrentTimeState");
				}
			}
		}

		// Token: 0x170005AC RID: 1452
		// (get) Token: 0x06000FFE RID: 4094 RVA: 0x0002C53C File Offset: 0x0002A73C
		// (set) Token: 0x06000FFF RID: 4095 RVA: 0x0002C544 File Offset: 0x0002A744
		[Editor(false)]
		public ButtonWidget FastForwardButton
		{
			get
			{
				return this._fastForwardButton;
			}
			set
			{
				if (this._fastForwardButton != value)
				{
					this._fastForwardButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "FastForwardButton");
				}
			}
		}

		// Token: 0x170005AD RID: 1453
		// (get) Token: 0x06001000 RID: 4096 RVA: 0x0002C562 File Offset: 0x0002A762
		// (set) Token: 0x06001001 RID: 4097 RVA: 0x0002C56A File Offset: 0x0002A76A
		[Editor(false)]
		public ButtonWidget PlayButton
		{
			get
			{
				return this._playButton;
			}
			set
			{
				if (this._playButton != value)
				{
					this._playButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "PlayButton");
				}
			}
		}

		// Token: 0x170005AE RID: 1454
		// (get) Token: 0x06001002 RID: 4098 RVA: 0x0002C588 File Offset: 0x0002A788
		// (set) Token: 0x06001003 RID: 4099 RVA: 0x0002C590 File Offset: 0x0002A790
		[Editor(false)]
		public ButtonWidget PauseButton
		{
			get
			{
				return this._pauseButton;
			}
			set
			{
				if (this._pauseButton != value)
				{
					this._pauseButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "PauseButton");
				}
			}
		}

		// Token: 0x0400074B RID: 1867
		private int _currenTimeState;

		// Token: 0x0400074C RID: 1868
		private ButtonWidget _fastForwardButton;

		// Token: 0x0400074D RID: 1869
		private ButtonWidget _playButton;

		// Token: 0x0400074E RID: 1870
		private ButtonWidget _pauseButton;
	}
}
