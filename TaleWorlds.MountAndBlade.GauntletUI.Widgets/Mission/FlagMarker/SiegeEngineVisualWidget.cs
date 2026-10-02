using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Mission.FlagMarker
{
	// Token: 0x02000102 RID: 258
	public class SiegeEngineVisualWidget : Widget
	{
		// Token: 0x06000DF0 RID: 3568 RVA: 0x000264CA File Offset: 0x000246CA
		public SiegeEngineVisualWidget(UIContext context)
			: base(context)
		{
			this._fallbackSprite = this.GetSprite("BlankWhiteCircle");
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x000264F0 File Offset: 0x000246F0
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._hasVisualSet && this.EngineID != string.Empty && this.OutlineWidget != null && this.IconWidget != null)
			{
				string text = string.Empty;
				string engineID = this.EngineID;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(engineID);
				if (num <= 1241455715U)
				{
					if (num <= 712590611U)
					{
						if (num != 6339497U)
						{
							if (num != 695812992U)
							{
								if (num != 712590611U)
								{
									goto IL_01F8;
								}
								if (!(engineID == "siege_tower_level2"))
								{
									goto IL_01F8;
								}
							}
							else if (!(engineID == "siege_tower_level3"))
							{
								goto IL_01F8;
							}
						}
						else
						{
							if (!(engineID == "ladder"))
							{
								goto IL_01F8;
							}
							text = "ladder";
							goto IL_01F8;
						}
					}
					else if (num != 729368230U)
					{
						if (num != 808481256U)
						{
							if (num != 1241455715U)
							{
								goto IL_01F8;
							}
							if (!(engineID == "ram"))
							{
								goto IL_01F8;
							}
							text = "battering_ram";
							goto IL_01F8;
						}
						else
						{
							if (!(engineID == "fire_ballista"))
							{
								goto IL_01F8;
							}
							goto IL_01D2;
						}
					}
					else if (!(engineID == "siege_tower_level1"))
					{
						goto IL_01F8;
					}
					text = "siege_tower";
					goto IL_01F8;
				}
				if (num <= 1839032341U)
				{
					if (num != 1748194790U)
					{
						if (num != 1820818168U)
						{
							if (num != 1839032341U)
							{
								goto IL_01F8;
							}
							if (!(engineID == "trebuchet"))
							{
								goto IL_01F8;
							}
							text = "trebuchet";
							goto IL_01F8;
						}
						else if (!(engineID == "fire_onager"))
						{
							goto IL_01F8;
						}
					}
					else if (!(engineID == "fire_catapult"))
					{
						goto IL_01F8;
					}
				}
				else if (num != 1898442385U)
				{
					if (num != 2806198843U)
					{
						if (num != 4036530155U)
						{
							goto IL_01F8;
						}
						if (!(engineID == "ballista"))
						{
							goto IL_01F8;
						}
						goto IL_01D2;
					}
					else if (!(engineID == "onager"))
					{
						goto IL_01F8;
					}
				}
				else if (!(engineID == "catapult"))
				{
					goto IL_01F8;
				}
				text = "catapult";
				goto IL_01F8;
				IL_01D2:
				text = "ballista";
				IL_01F8:
				this.OutlineWidget.Sprite = ((text == string.Empty) ? this._fallbackSprite : this.GetSprite("MPHud\\SiegeMarkers\\" + text + "_outline"));
				this.IconWidget.Sprite = ((text == string.Empty) ? this._fallbackSprite : this.GetSprite("MPHud\\SiegeMarkers\\" + text));
				this._hasVisualSet = true;
			}
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x00026763 File Offset: 0x00024963
		private Sprite GetSprite(string path)
		{
			return base.Context.SpriteData.GetSprite(path);
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06000DF3 RID: 3571 RVA: 0x00026776 File Offset: 0x00024976
		// (set) Token: 0x06000DF4 RID: 3572 RVA: 0x0002677E File Offset: 0x0002497E
		[Editor(false)]
		public string EngineID
		{
			get
			{
				return this._engineID;
			}
			set
			{
				if (value != this._engineID)
				{
					this._engineID = value;
					base.OnPropertyChanged<string>(value, "EngineID");
				}
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06000DF5 RID: 3573 RVA: 0x000267A1 File Offset: 0x000249A1
		// (set) Token: 0x06000DF6 RID: 3574 RVA: 0x000267A9 File Offset: 0x000249A9
		public Widget OutlineWidget
		{
			get
			{
				return this._outlineWidget;
			}
			set
			{
				if (this._outlineWidget != value)
				{
					this._outlineWidget = value;
					base.OnPropertyChanged<Widget>(value, "OutlineWidget");
				}
			}
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06000DF7 RID: 3575 RVA: 0x000267C7 File Offset: 0x000249C7
		// (set) Token: 0x06000DF8 RID: 3576 RVA: 0x000267CF File Offset: 0x000249CF
		public Widget IconWidget
		{
			get
			{
				return this._iconWidget;
			}
			set
			{
				if (this._iconWidget != value)
				{
					this._iconWidget = value;
					base.OnPropertyChanged<Widget>(value, "IconWidget");
				}
			}
		}

		// Token: 0x04000654 RID: 1620
		private bool _hasVisualSet;

		// Token: 0x04000655 RID: 1621
		private Sprite _fallbackSprite;

		// Token: 0x04000656 RID: 1622
		private const string SpritePathPrefix = "MPHud\\SiegeMarkers\\";

		// Token: 0x04000657 RID: 1623
		private string _engineID = string.Empty;

		// Token: 0x04000658 RID: 1624
		private Widget _outlineWidget;

		// Token: 0x04000659 RID: 1625
		private Widget _iconWidget;
	}
}
