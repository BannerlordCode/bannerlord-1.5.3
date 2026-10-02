using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000029 RID: 41
	public class InitialMenuAnimControllerWidget : Widget
	{
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600021A RID: 538 RVA: 0x00007B72 File Offset: 0x00005D72
		// (set) Token: 0x0600021B RID: 539 RVA: 0x00007B7A File Offset: 0x00005D7A
		public bool IsAnimEnabled { get; set; }

		// Token: 0x0600021C RID: 540 RVA: 0x00007B83 File Offset: 0x00005D83
		public InitialMenuAnimControllerWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00007B8C File Offset: 0x00005D8C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.IsAnimEnabled)
			{
				if (!this._isInitialized)
				{
					Widget optionsList = this.OptionsList;
					bool flag;
					if (optionsList == null)
					{
						flag = false;
					}
					else
					{
						List<Widget> children = optionsList.Children;
						int? num = ((children != null) ? new int?(children.Count) : null);
						int num2 = 0;
						flag = (num.GetValueOrDefault() > num2) & (num != null);
					}
					if (flag)
					{
						this.OptionsList.Children.ForEach(delegate(Widget x)
						{
							x.SetGlobalAlphaRecursively(0f);
						});
						this._totalOptionCount = this.OptionsList.Children.Count;
						this._isInitialized = true;
					}
				}
				if (this._isInitialized && !this._isFinalized && this.OptionsList != null)
				{
					this._timer += dt;
					if (this._timer >= this.InitialWaitTime + (float)this._currentOptionIndex * this.WaitTimeBetweenOptions)
					{
						Widget child = this.OptionsList.GetChild(this._currentOptionIndex);
						if (child != null)
						{
							child.SetState("Activated");
						}
						this._currentOptionIndex++;
					}
					for (int i = 0; i < this._currentOptionIndex; i++)
					{
						float num3 = this.InitialWaitTime + this.WaitTimeBetweenOptions * (float)i;
						float num4 = num3 + this.OptionFadeInTime;
						Widget child2 = this.OptionsList.GetChild(i);
						if (this._timer < num4)
						{
							float num5 = MathF.Clamp((this._timer - num3) / (num4 - num3), 0f, 1f);
							if (child2 != null)
							{
								child2.SetGlobalAlphaRecursively(num5);
							}
						}
						else if (child2 != null)
						{
							child2.SetGlobalAlphaRecursively(1f);
						}
					}
					this._isFinalized = this._timer > this.InitialWaitTime + this.WaitTimeBetweenOptions * (float)(this._totalOptionCount - 1) + this.OptionFadeInTime;
				}
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600021E RID: 542 RVA: 0x00007D73 File Offset: 0x00005F73
		// (set) Token: 0x0600021F RID: 543 RVA: 0x00007D7B File Offset: 0x00005F7B
		[Editor(false)]
		public Widget OptionsList
		{
			get
			{
				return this._optionsList;
			}
			set
			{
				if (this._optionsList != value)
				{
					this._optionsList = value;
					base.OnPropertyChanged<Widget>(value, "OptionsList");
				}
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000220 RID: 544 RVA: 0x00007D99 File Offset: 0x00005F99
		// (set) Token: 0x06000221 RID: 545 RVA: 0x00007DA1 File Offset: 0x00005FA1
		[Editor(false)]
		public float InitialWaitTime
		{
			get
			{
				return this._initialWaitTime;
			}
			set
			{
				if (this._initialWaitTime != value)
				{
					this._initialWaitTime = value;
					base.OnPropertyChanged(value, "InitialWaitTime");
				}
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00007DBF File Offset: 0x00005FBF
		// (set) Token: 0x06000223 RID: 547 RVA: 0x00007DC7 File Offset: 0x00005FC7
		[Editor(false)]
		public float WaitTimeBetweenOptions
		{
			get
			{
				return this._waitTimeBetweenOptions;
			}
			set
			{
				if (this._waitTimeBetweenOptions != value)
				{
					this._waitTimeBetweenOptions = value;
					base.OnPropertyChanged(value, "WaitTimeBetweenOptions");
				}
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000224 RID: 548 RVA: 0x00007DE5 File Offset: 0x00005FE5
		// (set) Token: 0x06000225 RID: 549 RVA: 0x00007DED File Offset: 0x00005FED
		[Editor(false)]
		public float OptionFadeInTime
		{
			get
			{
				return this._optionFadeInTime;
			}
			set
			{
				if (this._optionFadeInTime != value)
				{
					this._optionFadeInTime = value;
					base.OnPropertyChanged(value, "OptionFadeInTime");
				}
			}
		}

		// Token: 0x040000FA RID: 250
		private bool _isInitialized;

		// Token: 0x040000FB RID: 251
		private bool _isFinalized;

		// Token: 0x040000FC RID: 252
		private int _currentOptionIndex;

		// Token: 0x040000FD RID: 253
		private int _totalOptionCount;

		// Token: 0x040000FE RID: 254
		private float _timer;

		// Token: 0x040000FF RID: 255
		private Widget _optionsList;

		// Token: 0x04000100 RID: 256
		private float _initialWaitTime;

		// Token: 0x04000101 RID: 257
		private float _waitTimeBetweenOptions;

		// Token: 0x04000102 RID: 258
		private float _optionFadeInTime;
	}
}
