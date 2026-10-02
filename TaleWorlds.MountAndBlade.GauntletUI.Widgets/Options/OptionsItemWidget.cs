using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options
{
	// Token: 0x02000077 RID: 119
	public class OptionsItemWidget : Widget
	{
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x0001306E File Offset: 0x0001126E
		// (set) Token: 0x0600066A RID: 1642 RVA: 0x00013076 File Offset: 0x00011276
		public Widget BooleanOption { get; set; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x0001307F File Offset: 0x0001127F
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x00013087 File Offset: 0x00011287
		public Widget NumericOption { get; set; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x00013090 File Offset: 0x00011290
		// (set) Token: 0x0600066E RID: 1646 RVA: 0x00013098 File Offset: 0x00011298
		public Widget StringOption { get; set; }

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x000130A1 File Offset: 0x000112A1
		// (set) Token: 0x06000670 RID: 1648 RVA: 0x000130A9 File Offset: 0x000112A9
		public Widget GameKeyOption { get; set; }

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x000130B2 File Offset: 0x000112B2
		// (set) Token: 0x06000672 RID: 1650 RVA: 0x000130BA File Offset: 0x000112BA
		public Widget ActionOption { get; set; }

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x000130C3 File Offset: 0x000112C3
		// (set) Token: 0x06000674 RID: 1652 RVA: 0x000130CB File Offset: 0x000112CB
		public Widget InputOption { get; set; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x000130D4 File Offset: 0x000112D4
		// (set) Token: 0x06000676 RID: 1654 RVA: 0x000130DC File Offset: 0x000112DC
		public AnimatedDropdownWidget DropdownWidget
		{
			get
			{
				return this._dropdownWidget;
			}
			set
			{
				if (value != this._dropdownWidget)
				{
					this._dropdownWidget = value;
				}
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x000130EE File Offset: 0x000112EE
		// (set) Token: 0x06000678 RID: 1656 RVA: 0x000130F6 File Offset: 0x000112F6
		public ButtonWidget BooleanToggleButtonWidget
		{
			get
			{
				return this._booleanToggleButtonWidget;
			}
			set
			{
				if (value != this._booleanToggleButtonWidget)
				{
					this._booleanToggleButtonWidget = value;
				}
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00013108 File Offset: 0x00011308
		public OptionsItemWidget(UIContext context)
			: base(context)
		{
			this._optionTypeID = -1;
			this._graphicsSprites = new List<Sprite>();
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001312C File Offset: 0x0001132C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.SetCurrentScreenWidget(this.FindScreenWidget(base.ParentWidget));
				if (this.ImageIDs != null)
				{
					for (int i = 0; i < this.ImageIDs.Length; i++)
					{
						if (this.ImageIDs[i] != string.Empty)
						{
							Sprite sprite = base.Context.SpriteData.GetSprite(this.ImageIDs[i]);
							this._graphicsSprites.Add(sprite);
						}
					}
				}
				this.RefreshVisibilityOfSubItems();
				this.ResetNavigationIndices();
				this._initialized = true;
			}
			if (!this._eventsRegistered)
			{
				this.RegisterHoverEvents();
				this._eventsRegistered = true;
			}
			if (this._isEnabledStateDirty)
			{
				Widget currentOptionWidget = this.GetCurrentOptionWidget();
				if (currentOptionWidget != null)
				{
					currentOptionWidget.ApplyActionToAllChildrenRecursive(delegate(Widget child)
					{
						child.IsEnabled = this.IsOptionEnabled;
					});
				}
				this._isEnabledStateDirty = false;
			}
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00013201 File Offset: 0x00011401
		protected override void OnHoverBegin()
		{
			base.OnHoverBegin();
			this.SetCurrentOption(false, false, -1);
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00013212 File Offset: 0x00011412
		protected override void OnHoverEnd()
		{
			base.OnHoverEnd();
			this.ResetCurrentOption();
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00013220 File Offset: 0x00011420
		private OptionsScreenWidget FindScreenWidget(Widget parent)
		{
			OptionsScreenWidget optionsScreenWidget;
			if ((optionsScreenWidget = parent as OptionsScreenWidget) != null)
			{
				return optionsScreenWidget;
			}
			if (parent == null)
			{
				return null;
			}
			return this.FindScreenWidget(parent.ParentWidget);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0001324C File Offset: 0x0001144C
		private void SetCurrentOption(bool fromHoverOverDropdown, bool fromBooleanSelection, int hoverDropdownItemIndex = -1)
		{
			if (this._optionTypeID == 3)
			{
				Sprite sprite;
				if (fromHoverOverDropdown)
				{
					sprite = ((this._graphicsSprites.Count > hoverDropdownItemIndex) ? this._graphicsSprites[hoverDropdownItemIndex] : null);
				}
				else
				{
					sprite = ((this._graphicsSprites.Count > this.DropdownWidget.CurrentSelectedIndex && this.DropdownWidget.CurrentSelectedIndex >= 0) ? this._graphicsSprites[this.DropdownWidget.CurrentSelectedIndex] : null);
				}
				OptionsScreenWidget screenWidget = this._screenWidget;
				if (screenWidget == null)
				{
					return;
				}
				screenWidget.SetCurrentOption(this, sprite);
				return;
			}
			else if (this._optionTypeID == 0)
			{
				int num = (this.BooleanToggleButtonWidget.IsSelected ? 0 : 1);
				Sprite sprite2 = ((this._graphicsSprites.Count > num) ? this._graphicsSprites[num] : null);
				OptionsScreenWidget screenWidget2 = this._screenWidget;
				if (screenWidget2 == null)
				{
					return;
				}
				screenWidget2.SetCurrentOption(this, sprite2);
				return;
			}
			else
			{
				OptionsScreenWidget screenWidget3 = this._screenWidget;
				if (screenWidget3 == null)
				{
					return;
				}
				screenWidget3.SetCurrentOption(this, null);
				return;
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00013337 File Offset: 0x00011537
		public void SetCurrentScreenWidget(OptionsScreenWidget screenWidget)
		{
			this._screenWidget = screenWidget;
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00013340 File Offset: 0x00011540
		private void ResetCurrentOption()
		{
			OptionsScreenWidget screenWidget = this._screenWidget;
			if (screenWidget == null)
			{
				return;
			}
			screenWidget.SetCurrentOption(null, null);
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00013354 File Offset: 0x00011554
		private void RegisterHoverEvents()
		{
			base.ApplyActionToAllChildrenRecursive(delegate(Widget child)
			{
				child.boolPropertyChanged += this.Child_PropertyChanged;
			});
			if (this.OptionTypeID == 0)
			{
				this.BooleanToggleButtonWidget.boolPropertyChanged += this.BooleanOption_PropertyChanged;
				return;
			}
			if (this.OptionTypeID == 3)
			{
				this._dropdownExtensionParentWidget = this.DropdownWidget.DropdownClipWidget;
				this._dropdownExtensionParentWidget.ApplyActionToAllChildrenRecursive(delegate(Widget child)
				{
					child.boolPropertyChanged += this.DropdownItem_PropertyChanged1;
				});
			}
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x000133C4 File Offset: 0x000115C4
		private void BooleanOption_PropertyChanged(PropertyOwnerObject childWidget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsSelected")
			{
				this.SetCurrentOption(false, true, -1);
			}
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x000133DC File Offset: 0x000115DC
		private void Child_PropertyChanged(PropertyOwnerObject childWidget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsHovered")
			{
				if (propertyValue)
				{
					this.SetCurrentOption(false, false, -1);
					return;
				}
				this.ResetCurrentOption();
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00013400 File Offset: 0x00011600
		private void DropdownItem_PropertyChanged1(PropertyOwnerObject childWidget, string propertyName, bool propertyValue)
		{
			if (propertyName == "IsHovered")
			{
				if (propertyValue)
				{
					Widget widget = childWidget as Widget;
					this.SetCurrentOption(true, false, widget.ParentWidget.GetChildIndex(widget));
					return;
				}
				this.ResetCurrentOption();
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00013440 File Offset: 0x00011640
		private void RefreshVisibilityOfSubItems()
		{
			this.BooleanOption.IsVisible = this.OptionTypeID == 0;
			this.NumericOption.IsVisible = this.OptionTypeID == 1;
			this.StringOption.IsVisible = this.OptionTypeID == 3;
			this.GameKeyOption.IsVisible = this.OptionTypeID == 2;
			this.InputOption.IsVisible = this.OptionTypeID == 4;
			if (this.ActionOption != null)
			{
				this.ActionOption.IsVisible = this.OptionTypeID == 5;
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x000134D0 File Offset: 0x000116D0
		private Widget GetCurrentOptionWidget()
		{
			switch (this.OptionTypeID)
			{
			case 0:
				return this.BooleanOption;
			case 1:
				return this.NumericOption;
			case 2:
				return this.StringOption;
			case 3:
				return this.GameKeyOption;
			case 4:
				return this.InputOption;
			case 5:
				return this.ActionOption;
			default:
				return null;
			}
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00013530 File Offset: 0x00011730
		private void ResetNavigationIndices()
		{
			if (base.GamepadNavigationIndex == -1)
			{
				return;
			}
			bool flag = false;
			Widget booleanOption = this.BooleanOption;
			if (booleanOption != null && booleanOption.IsVisible)
			{
				this.BooleanOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
				flag = true;
			}
			else
			{
				Widget numericOption = this.NumericOption;
				if (numericOption != null && numericOption.IsVisible)
				{
					this.NumericOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
					flag = true;
				}
				else
				{
					Widget stringOption = this.StringOption;
					if (stringOption != null && stringOption.IsVisible)
					{
						this.StringOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
						flag = true;
					}
					else
					{
						Widget gameKeyOption = this.GameKeyOption;
						if (gameKeyOption != null && gameKeyOption.IsVisible)
						{
							this.GameKeyOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
							flag = true;
						}
						else
						{
							Widget inputOption = this.InputOption;
							if (inputOption != null && inputOption.IsVisible)
							{
								this.InputOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
								flag = true;
							}
							else
							{
								Widget actionOption = this.ActionOption;
								if (actionOption != null && actionOption.IsVisible)
								{
									this.ActionOption.GamepadNavigationIndex = base.GamepadNavigationIndex;
									flag = true;
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				base.GamepadNavigationIndex = -1;
				return;
			}
			Debug.FailedAssert("No option type is visible for: " + base.GetType().Name, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Options\\OptionsItemWidget.cs", "ResetNavigationIndices", 310);
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00013677 File Offset: 0x00011877
		protected override void OnGamepadNavigationIndexUpdated(int newIndex)
		{
			if (this._initialized)
			{
				this.ResetNavigationIndices();
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x00013687 File Offset: 0x00011887
		// (set) Token: 0x0600068A RID: 1674 RVA: 0x0001368F File Offset: 0x0001188F
		public int OptionTypeID
		{
			get
			{
				return this._optionTypeID;
			}
			set
			{
				if (this._optionTypeID != value)
				{
					this._optionTypeID = value;
					base.OnPropertyChanged(value, "OptionTypeID");
				}
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x000136AD File Offset: 0x000118AD
		// (set) Token: 0x0600068C RID: 1676 RVA: 0x000136B5 File Offset: 0x000118B5
		public bool IsOptionEnabled
		{
			get
			{
				return this._isOptionEnabled;
			}
			set
			{
				if (this._isOptionEnabled != value)
				{
					this._isOptionEnabled = value;
					base.OnPropertyChanged(value, "IsOptionEnabled");
					this._isEnabledStateDirty = true;
				}
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x000136DA File Offset: 0x000118DA
		// (set) Token: 0x0600068E RID: 1678 RVA: 0x000136E2 File Offset: 0x000118E2
		public string OptionTitle
		{
			get
			{
				return this._optionTitle;
			}
			set
			{
				if (this._optionTitle != value)
				{
					this._optionTitle = value;
				}
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x000136F9 File Offset: 0x000118F9
		// (set) Token: 0x06000690 RID: 1680 RVA: 0x00013701 File Offset: 0x00011901
		public string[] ImageIDs
		{
			get
			{
				return this._imageIDs;
			}
			set
			{
				if (this._imageIDs != value)
				{
					this._imageIDs = value;
				}
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x00013713 File Offset: 0x00011913
		// (set) Token: 0x06000692 RID: 1682 RVA: 0x0001371B File Offset: 0x0001191B
		public string OptionDescription
		{
			get
			{
				return this._optionDescription;
			}
			set
			{
				if (this._optionDescription != value)
				{
					this._optionDescription = value;
				}
			}
		}

		// Token: 0x040002C1 RID: 705
		private ButtonWidget _booleanToggleButtonWidget;

		// Token: 0x040002C2 RID: 706
		private AnimatedDropdownWidget _dropdownWidget;

		// Token: 0x040002C3 RID: 707
		private OptionsScreenWidget _screenWidget;

		// Token: 0x040002C4 RID: 708
		private Widget _dropdownExtensionParentWidget;

		// Token: 0x040002C5 RID: 709
		private bool _eventsRegistered;

		// Token: 0x040002C6 RID: 710
		private bool _initialized;

		// Token: 0x040002C7 RID: 711
		private List<Sprite> _graphicsSprites;

		// Token: 0x040002C8 RID: 712
		private bool _isEnabledStateDirty = true;

		// Token: 0x040002C9 RID: 713
		private int _optionTypeID;

		// Token: 0x040002CA RID: 714
		private string _optionDescription;

		// Token: 0x040002CB RID: 715
		private string _optionTitle;

		// Token: 0x040002CC RID: 716
		private string[] _imageIDs;

		// Token: 0x040002CD RID: 717
		private bool _isOptionEnabled;
	}
}
