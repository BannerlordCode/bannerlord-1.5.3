using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard
{
	// Token: 0x02000092 RID: 146
	public class MultiplayerScoreboardEndOfBattlePanelWidget : Widget
	{
		// Token: 0x06000810 RID: 2064 RVA: 0x00017990 File Offset: 0x00015B90
		public MultiplayerScoreboardEndOfBattlePanelWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x000179A4 File Offset: 0x00015BA4
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._isFinished || !this._isStarted)
			{
				return;
			}
			this._timePassed += dt;
			if (this._timePassed >= this.SecondDelay)
			{
				this._isFinished = true;
				this.SetState("Opened");
				base.Context.TwoDimensionContext.PlaySound(this._openedSoundEvent);
				return;
			}
			if (this._timePassed >= this.FirstDelay && !this._isPreStateFinished)
			{
				this._isPreStateFinished = true;
				this.SetState("PreOpened");
			}
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00017A36 File Offset: 0x00015C36
		public void StartAnimation()
		{
			this._isStarted = true;
			this._isFinished = false;
			this._isPreStateFinished = false;
			this._timePassed = 0f;
			base.AddState("PreOpened");
			base.AddState("Opened");
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00017A6E File Offset: 0x00015C6E
		private void Reset()
		{
			this._isStarted = false;
			this._isPreStateFinished = false;
			this._isFinished = false;
			this.SetState("Default");
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x00017A90 File Offset: 0x00015C90
		private void AvailableUpdated()
		{
			if (this.IsAvailable)
			{
				this.StartAnimation();
				return;
			}
			this.Reset();
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x00017AA7 File Offset: 0x00015CA7
		// (set) Token: 0x06000816 RID: 2070 RVA: 0x00017AAF File Offset: 0x00015CAF
		[Editor(false)]
		public bool IsAvailable
		{
			get
			{
				return this._isAvailable;
			}
			set
			{
				if (value != this._isAvailable)
				{
					this._isAvailable = value;
					base.OnPropertyChanged(value, "IsAvailable");
					this.AvailableUpdated();
				}
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x00017AD3 File Offset: 0x00015CD3
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x00017ADB File Offset: 0x00015CDB
		[Editor(false)]
		public float FirstDelay
		{
			get
			{
				return this._firstDelay;
			}
			set
			{
				if (value != this._firstDelay)
				{
					this._firstDelay = value;
					base.OnPropertyChanged(value, "FirstDelay");
				}
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x00017AF9 File Offset: 0x00015CF9
		// (set) Token: 0x0600081A RID: 2074 RVA: 0x00017B01 File Offset: 0x00015D01
		[Editor(false)]
		public float SecondDelay
		{
			get
			{
				return this._secondDelay;
			}
			set
			{
				if (value != this._secondDelay)
				{
					this._secondDelay = value;
					base.OnPropertyChanged(value, "SecondDelay");
				}
			}
		}

		// Token: 0x04000393 RID: 915
		private bool _isStarted;

		// Token: 0x04000394 RID: 916
		private bool _isPreStateFinished;

		// Token: 0x04000395 RID: 917
		private bool _isFinished;

		// Token: 0x04000396 RID: 918
		private float _timePassed;

		// Token: 0x04000397 RID: 919
		private string _openedSoundEvent = "panels/scoreboard_flags";

		// Token: 0x04000398 RID: 920
		private bool _isAvailable;

		// Token: 0x04000399 RID: 921
		private float _firstDelay;

		// Token: 0x0400039A RID: 922
		private float _secondDelay;
	}
}
