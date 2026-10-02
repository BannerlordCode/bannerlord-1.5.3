using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Scoreboard
{
	// Token: 0x02000055 RID: 85
	public class ScoreboardBattleRewardsWidget : Widget
	{
		// Token: 0x060004A7 RID: 1191 RVA: 0x0000EAC9 File Offset: 0x0000CCC9
		public ScoreboardBattleRewardsWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000EAE8 File Offset: 0x0000CCE8
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (this._isAnimationActive)
			{
				this.UpdateAnimation(dt);
			}
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0000EB00 File Offset: 0x0000CD00
		public void StartAnimation()
		{
			this._isAnimationActive = true;
			this._animationTimePassed = 0f;
			this._animationLastItemIndex = -1;
			this.ItemContainer.SetState("Opened");
			for (int i = 0; i < this.ItemContainer.ChildCount; i++)
			{
				Widget child = this.ItemContainer.GetChild(i);
				child.IsVisible = false;
				child.AddState("Opened");
			}
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0000EB6C File Offset: 0x0000CD6C
		public void Reset()
		{
			for (int i = 0; i < this.ItemContainer.ChildCount; i++)
			{
				this.ItemContainer.GetChild(i).IsVisible = false;
			}
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0000EBA4 File Offset: 0x0000CDA4
		private void UpdateAnimation(float dt)
		{
			if (this._animationTimePassed >= this.AnimationDelay + this.AnimationInterval * (float)this.ItemContainer.ChildCount)
			{
				return;
			}
			if (this._animationTimePassed >= this.AnimationDelay)
			{
				int num = MathF.Floor((this._animationTimePassed - this.AnimationDelay) / this.AnimationInterval);
				if (num != this._animationLastItemIndex && num < this.ItemContainer.ChildCount)
				{
					for (int i = this._animationLastItemIndex + 1; i <= num; i++)
					{
						Widget child = this.ItemContainer.GetChild(i);
						child.IsVisible = true;
						child.SetState("Opened");
					}
				}
			}
			this._animationTimePassed += dt;
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x0000EC51 File Offset: 0x0000CE51
		// (set) Token: 0x060004AD RID: 1197 RVA: 0x0000EC59 File Offset: 0x0000CE59
		[Editor(false)]
		public float AnimationDelay
		{
			get
			{
				return this._animationDelay;
			}
			set
			{
				if (this._animationDelay != value)
				{
					this._animationDelay = value;
					base.OnPropertyChanged(value, "AnimationDelay");
				}
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x0000EC77 File Offset: 0x0000CE77
		// (set) Token: 0x060004AF RID: 1199 RVA: 0x0000EC7F File Offset: 0x0000CE7F
		[Editor(false)]
		public float AnimationInterval
		{
			get
			{
				return this._animationInterval;
			}
			set
			{
				if (this._animationInterval != value)
				{
					this._animationInterval = value;
					base.OnPropertyChanged(value, "AnimationInterval");
				}
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x0000EC9D File Offset: 0x0000CE9D
		// (set) Token: 0x060004B1 RID: 1201 RVA: 0x0000ECA5 File Offset: 0x0000CEA5
		[Editor(false)]
		public Widget ItemContainer
		{
			get
			{
				return this._itemContainer;
			}
			set
			{
				if (this._itemContainer != value)
				{
					this._itemContainer = value;
					base.OnPropertyChanged<Widget>(value, "ItemContainer");
				}
			}
		}

		// Token: 0x040001FA RID: 506
		private bool _isAnimationActive;

		// Token: 0x040001FB RID: 507
		private float _animationTimePassed;

		// Token: 0x040001FC RID: 508
		private int _animationLastItemIndex;

		// Token: 0x040001FD RID: 509
		private float _animationDelay = 1f;

		// Token: 0x040001FE RID: 510
		private float _animationInterval = 0.25f;

		// Token: 0x040001FF RID: 511
		private Widget _itemContainer;
	}
}
