using System;
using System.Numerics;
using TaleWorlds.GauntletUI.GamepadNavigation;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.GauntletUI.BaseTypes
{
	// Token: 0x02000067 RID: 103
	public class SliderWidget : ImageWidget
	{
		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0001E65F File Offset: 0x0001C85F
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x0001E667 File Offset: 0x0001C867
		public bool UpdateValueOnScroll { get; set; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x0001E670 File Offset: 0x0001C870
		private float _holdTimeToStartMovement
		{
			get
			{
				return 0.3f;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000704 RID: 1796 RVA: 0x0001E677 File Offset: 0x0001C877
		private float _dynamicIncrement
		{
			get
			{
				if (this.MaxValueFloat - this.MinValueFloat <= 2f)
				{
					return 0.1f;
				}
				return 1f;
			}
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0001E698 File Offset: 0x0001C898
		public SliderWidget(UIContext context)
			: base(context)
		{
			this.SliderArea = this;
			this._firstFrame = true;
			base.FrictionEnabled = true;
			base.UsedNavigationMovements = GamepadNavigationTypes.Horizontal;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0001E6D0 File Offset: 0x0001C8D0
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			bool flag = false;
			base.IsUsingNavigation = false;
			if (!this.Locked)
			{
				if (!base.IsPressed)
				{
					Widget handle = this.Handle;
					if (handle == null || !handle.IsPressed)
					{
						Widget handleExtension = this.HandleExtension;
						if (handleExtension == null || !handleExtension.IsPressed)
						{
							goto IL_01A7;
						}
					}
				}
				if (base.EventManager.IsControllerActive && base.IsRecursivelyVisible() && base.EventManager.GetIsHitThisFrame())
				{
					float num = 0f;
					if (Input.IsKeyDown(InputKey.ControllerLLeft))
					{
						num = -1f;
					}
					else if (Input.IsKeyDown(InputKey.ControllerLRight))
					{
						num = 1f;
					}
					if (num != 0f)
					{
						num *= (this.IsDiscrete ? ((float)this.DiscreteIncrementInterval) : this._dynamicIncrement);
						if (this._downStartTime == -1f)
						{
							this._downStartTime = base.Context.EventManager.Time;
							this.ValueFloat = MathF.Clamp(this._valueFloat + num, this.MinValueFloat, this.MaxValueFloat);
							flag = true;
						}
						else if (this._holdTimeToStartMovement < base.Context.EventManager.Time - this._downStartTime)
						{
							this.ValueFloat = MathF.Clamp(this._valueFloat + num, this.MinValueFloat, this.MaxValueFloat);
							flag = true;
						}
					}
					else
					{
						this._downStartTime = -1f;
					}
					base.IsUsingNavigation = true;
				}
				if (!this._handleClicked)
				{
					this._handleClicked = true;
					this.UpdateLocalClickPosition();
					this._handleClickOffset = base.EventManager.MousePosition - this.Handle.AreaRect.GetCenter();
				}
				this.HandleMouseMove();
				goto IL_01CB;
			}
			IL_01A7:
			this._downStartTime = -1f;
			this._handleClickOffset = Vector2.Zero;
			this._handleClicked = false;
			this._valueChangedByMouse = false;
			IL_01CB:
			this.UpdateScrollBar();
			this.UpdateHandleLength();
			Widget handle2 = this.Handle;
			if (handle2 != null)
			{
				handle2.SetState(base.CurrentState);
			}
			if (this._snapCursorToHandle)
			{
				Vector2 center = this.Handle.AreaRect.GetCenter();
				Input.SetMousePosition((int)center.X, (int)center.Y);
				this._snapCursorToHandle = false;
			}
			if (flag && Input.MouseMoveX == 0f && Input.MouseMoveY == 0f)
			{
				this._snapCursorToHandle = true;
			}
			this._firstFrame = false;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0001E928 File Offset: 0x0001CB28
		protected override void OnParallelUpdate(float dt)
		{
			base.OnParallelUpdate(dt);
			if (this.Filler != null)
			{
				float num = 1f;
				if (MathF.Abs(this.MaxValueFloat - this.MinValueFloat) > 1E-45f)
				{
					num = (this._valueFloat - this.MinValueFloat) / (this.MaxValueFloat - this.MinValueFloat);
				}
				this.Filler.HorizontalAlignment = HorizontalAlignment.Left;
				if (this.AlignmentAxis == AlignmentAxis.Horizontal)
				{
					this.Filler.WidthSizePolicy = SizePolicy.Fixed;
					this.Filler.ScaledSuggestedWidth = this.SliderArea.Size.X * num;
				}
				else
				{
					this.Filler.HeightSizePolicy = SizePolicy.Fixed;
					this.Filler.ScaledSuggestedHeight = this.SliderArea.Size.Y * num;
				}
				this.Filler.DoNotAcceptEvents = true;
				this.Filler.DoNotPassEventsToChildren = true;
			}
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0001EA04 File Offset: 0x0001CC04
		protected internal override void OnMousePressed()
		{
			if (this.Handle != null && this.Handle.IsVisible)
			{
				base.IsPressed = true;
				base.EventFired("MousePressed", Array.Empty<object>());
				this.UpdateLocalClickPosition();
				base.OnPropertyChanged<string>("MouseDown", "OnMousePressed");
				this.HandleMouseMove();
			}
		}

		// Token: 0x06000709 RID: 1801 RVA: 0x0001EA5C File Offset: 0x0001CC5C
		protected internal override void OnMouseReleased(bool isFromInput)
		{
			if (this.Handle != null)
			{
				base.IsPressed = false;
				base.EventFired("MouseReleased", Array.Empty<object>());
				if (this.UpdateValueOnRelease)
				{
					base.OnPropertyChanged(this._valueFloat, "ValueFloat");
					base.OnPropertyChanged(this.ValueInt, "ValueInt");
					this.OnValueFloatChanged(this._valueFloat);
				}
			}
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0001EABE File Offset: 0x0001CCBE
		protected internal override void OnMouseMove()
		{
			base.OnMouseMove();
			if (base.IsPressed)
			{
				this.HandleMouseMove();
			}
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0001EAD4 File Offset: 0x0001CCD4
		protected internal virtual void OnValueIntChanged(int value)
		{
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0001EAD6 File Offset: 0x0001CCD6
		protected internal virtual void OnValueFloatChanged(float value)
		{
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001EAD8 File Offset: 0x0001CCD8
		private void UpdateScrollBar()
		{
			if (!this._firstFrame && base.IsVisible)
			{
				this.UpdateHandleByValue();
			}
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0001EAF0 File Offset: 0x0001CCF0
		private void UpdateLocalClickPosition()
		{
			Vector2 mousePosition = base.EventManager.MousePosition;
			this._localClickPos = this.Handle.AreaRect.TransformScreenPositionToLocal(in mousePosition);
			if (this._localClickPos.X < 0f || this._localClickPos.X > this.Handle.Size.X)
			{
				this._localClickPos.X = this.Handle.Size.X / 2f;
			}
			if (this._localClickPos.Y < -5f)
			{
				this._localClickPos.Y = -5f;
				return;
			}
			if (this._localClickPos.Y > this.Handle.Size.Y + 5f)
			{
				this._localClickPos.Y = this.Handle.Size.Y + 5f;
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0001EBD8 File Offset: 0x0001CDD8
		private void HandleMouseMove()
		{
			if (base.EventManager.IsControllerActive && Input.MouseMoveX == 0f && Input.MouseMoveY == 0f)
			{
				return;
			}
			if (this.Handle != null && !this.Locked)
			{
				Vector2 vector = base.EventManager.MousePosition - this._localClickPos;
				float num = this.GetValue(vector, this.AlignmentAxis);
				float num2;
				float num3;
				if (this.AlignmentAxis == AlignmentAxis.Horizontal)
				{
					float x = base.ParentWidget.GlobalPosition.X;
					num2 = x + base.Left;
					num3 = x + base.Right;
					num3 -= this.Handle.Size.X;
					Widget handleExtension = this.HandleExtension;
					if (handleExtension != null && handleExtension.IsPressed)
					{
						num -= this._handleClickOffset.X;
					}
				}
				else
				{
					float y = base.ParentWidget.GlobalPosition.Y;
					num2 = y + base.Top;
					num3 = y + base.Bottom;
					num3 -= this.Handle.Size.Y;
				}
				if (Mathf.Abs(num3 - num2) < 1E-05f)
				{
					this.ValueFloat = 0f;
					return;
				}
				if (num < num2)
				{
					num = num2;
				}
				if (num > num3)
				{
					num = num3;
				}
				float num4 = (num - num2) / (num3 - num2);
				this._valueChangedByMouse = true;
				this.ValueFloat = this.MinValueFloat + (this.MaxValueFloat - this.MinValueFloat) * num4;
			}
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0001ED30 File Offset: 0x0001CF30
		private void UpdateHandleByValue()
		{
			if (this._valueFloat < this.MinValueFloat)
			{
				this.ValueFloat = this.MinValueFloat;
			}
			if (this._valueFloat > this.MaxValueFloat)
			{
				this.ValueFloat = this.MaxValueFloat;
			}
			float num = 1f;
			if (MathF.Abs(this.MaxValueFloat - this.MinValueFloat) > 1E-45f)
			{
				num = (this._valueFloat - this.MinValueFloat) / (this.MaxValueFloat - this.MinValueFloat);
				if (this.ReverseDirection)
				{
					num = 1f - num;
				}
			}
			if (this.Handle != null)
			{
				if (this.AlignmentAxis == AlignmentAxis.Horizontal)
				{
					this.Handle.HorizontalAlignment = HorizontalAlignment.Left;
					this.Handle.VerticalAlignment = VerticalAlignment.Center;
					float num2 = this.SliderArea.Size.X;
					num2 -= this.Handle.Size.X;
					this.Handle.ScaledPositionXOffset = num2 * num;
					this.Handle.ScaledPositionYOffset = 0f;
				}
				else
				{
					this.Handle.HorizontalAlignment = HorizontalAlignment.Center;
					this.Handle.VerticalAlignment = VerticalAlignment.Bottom;
					float num3 = this.SliderArea.Size.Y;
					num3 -= this.Handle.Size.Y;
					this.Handle.ScaledPositionYOffset = -1f * num3 * (1f - num);
					this.Handle.ScaledPositionXOffset = 0f;
				}
				if (this.HandleExtension != null)
				{
					this.HandleExtension.HorizontalAlignment = this.Handle.HorizontalAlignment;
					this.HandleExtension.VerticalAlignment = this.Handle.VerticalAlignment;
					this.HandleExtension.ScaledPositionXOffset = this.Handle.ScaledPositionXOffset;
					this.HandleExtension.ScaledPositionYOffset = this.Handle.ScaledPositionYOffset;
				}
			}
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0001EEF4 File Offset: 0x0001D0F4
		private void UpdateHandleLength()
		{
			if (this.Handle != null && !this.DoNotUpdateHandleSize && this.IsDiscrete && this.Handle.WidthSizePolicy == SizePolicy.Fixed)
			{
				if (this.AlignmentAxis == AlignmentAxis.Horizontal)
				{
					this.Handle.SuggestedWidth = Mathf.Clamp(base.SuggestedWidth / (this.MaxValueFloat + 1f), 50f, base.SuggestedWidth / 2f);
					return;
				}
				if (this.AlignmentAxis == AlignmentAxis.Vertical)
				{
					this.Handle.SuggestedHeight = Mathf.Clamp(base.SuggestedHeight / (this.MaxValueFloat + 1f), 50f, base.SuggestedHeight / 2f);
				}
			}
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0001EFA9 File Offset: 0x0001D1A9
		private float GetValue(Vector2 value, AlignmentAxis alignmentAxis)
		{
			if (alignmentAxis == AlignmentAxis.Horizontal)
			{
				return value.X;
			}
			return value.Y;
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0001EFBC File Offset: 0x0001D1BC
		protected override bool OnPreviewMouseScroll()
		{
			if (this.UpdateValueOnScroll && !this.Locked)
			{
				float num = base.EventManager.DeltaMouseScroll * 0.004f;
				this.ValueFloat = MathF.Clamp(this._valueFloat + this._dynamicIncrement * num, this.MinValueFloat, this.MaxValueFloat);
			}
			return base.OnPreviewMouseScroll();
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x0001F017 File Offset: 0x0001D217
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x0001F01F File Offset: 0x0001D21F
		[Editor(false)]
		public bool IsDiscrete
		{
			get
			{
				return this._isDiscrete;
			}
			set
			{
				if (this._isDiscrete != value)
				{
					this._isDiscrete = value;
					base.OnPropertyChanged(this._isDiscrete, "IsDiscrete");
				}
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x0001F042 File Offset: 0x0001D242
		// (set) Token: 0x06000717 RID: 1815 RVA: 0x0001F04A File Offset: 0x0001D24A
		[Editor(false)]
		public bool Locked
		{
			get
			{
				return this._locked;
			}
			set
			{
				if (this._locked != value)
				{
					this._locked = value;
					base.OnPropertyChanged(this._locked, "Locked");
				}
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x0001F06D File Offset: 0x0001D26D
		// (set) Token: 0x06000719 RID: 1817 RVA: 0x0001F075 File Offset: 0x0001D275
		[Editor(false)]
		public bool UpdateValueOnRelease
		{
			get
			{
				return this._updateValueOnRelease;
			}
			set
			{
				if (this._updateValueOnRelease != value)
				{
					this._updateValueOnRelease = value;
					base.OnPropertyChanged(this._updateValueOnRelease, "UpdateValueOnRelease");
				}
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x0001F098 File Offset: 0x0001D298
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x0001F0A3 File Offset: 0x0001D2A3
		[Editor(false)]
		public bool UpdateValueContinuously
		{
			get
			{
				return !this._updateValueOnRelease;
			}
			set
			{
				if (this.UpdateValueContinuously != value)
				{
					this._updateValueOnRelease = !value;
					base.OnPropertyChanged(this._updateValueOnRelease, "UpdateValueContinuously");
				}
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x0001F0C9 File Offset: 0x0001D2C9
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x0001F0D1 File Offset: 0x0001D2D1
		[Editor(false)]
		public AlignmentAxis AlignmentAxis { get; set; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x0001F0DA File Offset: 0x0001D2DA
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x0001F0E2 File Offset: 0x0001D2E2
		[Editor(false)]
		public bool ReverseDirection { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x0001F0EB File Offset: 0x0001D2EB
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x0001F0F3 File Offset: 0x0001D2F3
		[Editor(false)]
		public Widget Filler { get; set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x0001F0FC File Offset: 0x0001D2FC
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x0001F104 File Offset: 0x0001D304
		[Editor(false)]
		public Widget HandleExtension { get; set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x0001F10D File Offset: 0x0001D30D
		// (set) Token: 0x06000725 RID: 1829 RVA: 0x0001F118 File Offset: 0x0001D318
		[Editor(false)]
		public float ValueFloat
		{
			get
			{
				return this._valueFloat;
			}
			set
			{
				if (MathF.Abs(this._valueFloat - value) > 1E-05f)
				{
					float valueFloat = this._valueFloat;
					if (this.MinValueFloat <= this.MaxValueFloat)
					{
						if (this._valueFloat < this.MinValueFloat)
						{
							this._valueFloat = this.MinValueFloat;
						}
						if (this._valueFloat > this.MaxValueFloat)
						{
							this._valueFloat = this.MaxValueFloat;
						}
						if (this.IsDiscrete)
						{
							if (value >= (float)this.MaxValueInt)
							{
								this._valueFloat = (float)this.MaxValueInt;
							}
							else
							{
								float num = Mathf.Floor((value - (float)this.MinValueInt) / (float)this.DiscreteIncrementInterval);
								this._valueFloat = num * (float)this.DiscreteIncrementInterval + (float)this.MinValueInt;
							}
						}
						else
						{
							this._valueFloat = value;
						}
						this.UpdateHandleByValue();
						if (MathF.Abs(this._valueFloat - valueFloat) > 1E-05f && ((this.UpdateValueOnRelease && !base.IsPressed) || !this.UpdateValueOnRelease))
						{
							base.OnPropertyChanged(this._valueFloat, "ValueFloat");
							base.OnPropertyChanged(this.ValueInt, "ValueInt");
							this.OnValueFloatChanged(this._valueFloat);
						}
					}
				}
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x0001F240 File Offset: 0x0001D440
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x0001F24D File Offset: 0x0001D44D
		[Editor(false)]
		public int ValueInt
		{
			get
			{
				return MathF.Round(this.ValueFloat);
			}
			set
			{
				this.ValueFloat = (float)value;
				this.OnValueIntChanged(this.ValueInt);
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0001F263 File Offset: 0x0001D463
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x0001F26B File Offset: 0x0001D46B
		[Editor(false)]
		public float MinValueFloat { get; set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x0001F274 File Offset: 0x0001D474
		// (set) Token: 0x0600072B RID: 1835 RVA: 0x0001F27C File Offset: 0x0001D47C
		[Editor(false)]
		public float MaxValueFloat { get; set; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x0001F285 File Offset: 0x0001D485
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x0001F292 File Offset: 0x0001D492
		[Editor(false)]
		public int MinValueInt
		{
			get
			{
				return MathF.Round(this.MinValueFloat);
			}
			set
			{
				this.MinValueFloat = (float)value;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x0001F29C File Offset: 0x0001D49C
		// (set) Token: 0x0600072F RID: 1839 RVA: 0x0001F2A9 File Offset: 0x0001D4A9
		[Editor(false)]
		public int MaxValueInt
		{
			get
			{
				return MathF.Round(this.MaxValueFloat);
			}
			set
			{
				this.MaxValueFloat = (float)value;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x0001F2B3 File Offset: 0x0001D4B3
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x0001F2BB File Offset: 0x0001D4BB
		public int DiscreteIncrementInterval { get; set; } = 1;

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x0001F2C4 File Offset: 0x0001D4C4
		// (set) Token: 0x06000733 RID: 1843 RVA: 0x0001F2CC File Offset: 0x0001D4CC
		[Editor(false)]
		public bool DoNotUpdateHandleSize { get; set; }

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x0001F2D5 File Offset: 0x0001D4D5
		// (set) Token: 0x06000735 RID: 1845 RVA: 0x0001F2E0 File Offset: 0x0001D4E0
		[Editor(false)]
		public Widget Handle
		{
			get
			{
				return this._handle;
			}
			set
			{
				if (this._handle != value)
				{
					this._handle = value;
					this.UpdateHandleByValue();
					if (this._handle != null)
					{
						this._handle.ExtendCursorAreaLeft = 6f;
						this._handle.ExtendCursorAreaRight = 6f;
						this._handle.ExtendCursorAreaTop = 3f;
						this._handle.ExtendCursorAreaBottom = 3f;
					}
				}
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x0001F34B File Offset: 0x0001D54B
		// (set) Token: 0x06000737 RID: 1847 RVA: 0x0001F353 File Offset: 0x0001D553
		[Editor(false)]
		public Widget SliderArea { get; set; }

		// Token: 0x0400034D RID: 845
		private bool _firstFrame;

		// Token: 0x0400034E RID: 846
		public float HandleRatio;

		// Token: 0x0400034F RID: 847
		protected bool _handleClicked;

		// Token: 0x04000350 RID: 848
		protected bool _valueChangedByMouse;

		// Token: 0x04000351 RID: 849
		private float _downStartTime = -1f;

		// Token: 0x04000352 RID: 850
		private Vector2 _handleClickOffset;

		// Token: 0x04000353 RID: 851
		private bool _snapCursorToHandle;

		// Token: 0x04000354 RID: 852
		private bool _locked;

		// Token: 0x04000355 RID: 853
		private bool _isDiscrete;

		// Token: 0x04000356 RID: 854
		private bool _updateValueOnRelease;

		// Token: 0x04000357 RID: 855
		private Vector2 _localClickPos;

		// Token: 0x04000358 RID: 856
		private float _valueFloat;

		// Token: 0x04000359 RID: 857
		private Widget _handle;
	}
}
