using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Menu.TownManagement
{
	// Token: 0x0200010C RID: 268
	public class DevelopmentNameTextWidget : TextWidget
	{
		// Token: 0x06000E5F RID: 3679 RVA: 0x00027C2C File Offset: 0x00025E2C
		public DevelopmentNameTextWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x00027C47 File Offset: 0x00025E47
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this.IsInQueue)
			{
				this.SetState(base.ParentWidget.CurrentState);
			}
			else
			{
				this.SetState("Selected");
			}
			this.HandleAnim(dt);
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x00027C80 File Offset: 0x00025E80
		private void HandleAnim(float dt)
		{
			switch (this._currentState)
			{
			case DevelopmentNameTextWidget.AnimState.Start:
				this._currentAlphaTarget = 0f;
				this._currentState = DevelopmentNameTextWidget.AnimState.DownName;
				break;
			case DevelopmentNameTextWidget.AnimState.DownName:
				if ((double)base.ReadOnlyBrush.TextAlphaFactor < 0.01)
				{
					this._currentAlphaTarget = 1f;
					base.Text = this.MaxText;
					this._currentState = DevelopmentNameTextWidget.AnimState.UpMax;
				}
				break;
			case DevelopmentNameTextWidget.AnimState.UpMax:
				if ((double)base.ReadOnlyBrush.TextAlphaFactor > 0.99)
				{
					this._currentAlphaTarget = 0f;
					this._currentState = DevelopmentNameTextWidget.AnimState.StayMax;
					this._stayMaxTotalTime = 0f;
				}
				break;
			case DevelopmentNameTextWidget.AnimState.StayMax:
				this._stayMaxTotalTime += dt;
				if (this._stayMaxTotalTime >= this.MaxTextStayTime)
				{
					this._currentAlphaTarget = 0f;
					this._currentState = DevelopmentNameTextWidget.AnimState.DownMax;
				}
				break;
			case DevelopmentNameTextWidget.AnimState.DownMax:
				if ((double)base.ReadOnlyBrush.TextAlphaFactor < 0.01)
				{
					this._currentAlphaTarget = 1f;
					this._currentState = DevelopmentNameTextWidget.AnimState.UpName;
					base.Text = this.NameText;
				}
				break;
			case DevelopmentNameTextWidget.AnimState.UpName:
				if ((double)base.ReadOnlyBrush.TextAlphaFactor > 0.99)
				{
					this._currentState = DevelopmentNameTextWidget.AnimState.Idle;
					base.Text = this.NameText;
				}
				break;
			}
			if (this._currentState != DevelopmentNameTextWidget.AnimState.Idle && this._currentState != DevelopmentNameTextWidget.AnimState.StayMax)
			{
				base.Brush.TextAlphaFactor = Mathf.Lerp(base.ReadOnlyBrush.TextAlphaFactor, this._currentAlphaTarget, dt * 15f);
			}
		}

		// Token: 0x06000E62 RID: 3682 RVA: 0x00027E18 File Offset: 0x00026018
		public void StartMaxTextAnimation()
		{
			DevelopmentNameTextWidget.AnimState currentState = this._currentState;
			if (currentState > DevelopmentNameTextWidget.AnimState.StayMax)
			{
				int num = currentState - DevelopmentNameTextWidget.AnimState.DownMax;
				this._currentState = DevelopmentNameTextWidget.AnimState.Start;
			}
		}

		// Token: 0x1700051F RID: 1311
		// (get) Token: 0x06000E63 RID: 3683 RVA: 0x00027E3D File Offset: 0x0002603D
		// (set) Token: 0x06000E64 RID: 3684 RVA: 0x00027E45 File Offset: 0x00026045
		[Editor(false)]
		public string MaxText
		{
			get
			{
				return this._maxText;
			}
			set
			{
				if (this._maxText != value)
				{
					this._maxText = value;
					base.OnPropertyChanged<string>(value, "MaxText");
				}
			}
		}

		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x00027E68 File Offset: 0x00026068
		// (set) Token: 0x06000E66 RID: 3686 RVA: 0x00027E70 File Offset: 0x00026070
		[Editor(false)]
		public float MaxTextStayTime
		{
			get
			{
				return this._maxTextStayTime;
			}
			set
			{
				if (this._maxTextStayTime != value)
				{
					this._maxTextStayTime = value;
					base.OnPropertyChanged(value, "MaxTextStayTime");
				}
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x06000E67 RID: 3687 RVA: 0x00027E8E File Offset: 0x0002608E
		// (set) Token: 0x06000E68 RID: 3688 RVA: 0x00027E96 File Offset: 0x00026096
		[Editor(false)]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (this._nameText != value)
				{
					this._nameText = value;
					base.OnPropertyChanged<string>(value, "NameText");
					base.Text = this.NameText;
				}
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x06000E69 RID: 3689 RVA: 0x00027EC5 File Offset: 0x000260C5
		// (set) Token: 0x06000E6A RID: 3690 RVA: 0x00027ECD File Offset: 0x000260CD
		[Editor(false)]
		public bool IsInQueue
		{
			get
			{
				return this._isInQueue;
			}
			set
			{
				if (this._isInQueue != value)
				{
					this._isInQueue = value;
					base.OnPropertyChanged(value, "IsInQueue");
				}
			}
		}

		// Token: 0x04000688 RID: 1672
		private float _currentAlphaTarget;

		// Token: 0x04000689 RID: 1673
		private float _stayMaxTotalTime;

		// Token: 0x0400068A RID: 1674
		private DevelopmentNameTextWidget.AnimState _currentState = DevelopmentNameTextWidget.AnimState.Idle;

		// Token: 0x0400068B RID: 1675
		private float _maxTextStayTime = 1f;

		// Token: 0x0400068C RID: 1676
		private bool _isInQueue;

		// Token: 0x0400068D RID: 1677
		private string _maxText;

		// Token: 0x0400068E RID: 1678
		private string _nameText;

		// Token: 0x020001C9 RID: 457
		public enum AnimState
		{
			// Token: 0x04000A47 RID: 2631
			Start,
			// Token: 0x04000A48 RID: 2632
			DownName,
			// Token: 0x04000A49 RID: 2633
			UpMax,
			// Token: 0x04000A4A RID: 2634
			StayMax,
			// Token: 0x04000A4B RID: 2635
			DownMax,
			// Token: 0x04000A4C RID: 2636
			UpName,
			// Token: 0x04000A4D RID: 2637
			Idle
		}
	}
}
