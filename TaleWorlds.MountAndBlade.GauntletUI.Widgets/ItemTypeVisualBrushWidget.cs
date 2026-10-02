using System;
using System.Linq;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x0200002C RID: 44
	public class ItemTypeVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000243 RID: 579 RVA: 0x0000815B File Offset: 0x0000635B
		public ItemTypeVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00008164 File Offset: 0x00006364
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._isInitialized)
			{
				string brushName = null;
				if (!string.IsNullOrEmpty(this.ItemTypeAsString))
				{
					string itemTypeAsString = this.ItemTypeAsString;
					uint num = <PrivateImplementationDetails>.ComputeStringHash(itemTypeAsString);
					if (num <= 2039097040U)
					{
						if (num <= 947870807U)
						{
							if (num <= 784896431U)
							{
								if (num != 677454334U)
								{
									if (num != 784896431U)
									{
										goto IL_027D;
									}
									if (!(itemTypeAsString == "Banner"))
									{
										goto IL_027D;
									}
								}
								else if (!(itemTypeAsString == "Spear"))
								{
									goto IL_027D;
								}
							}
							else if (num != 810547195U)
							{
								if (num != 947870807U)
								{
									goto IL_027D;
								}
								if (!(itemTypeAsString == "Mace"))
								{
									goto IL_027D;
								}
							}
							else if (!(itemTypeAsString == "None"))
							{
								goto IL_027D;
							}
						}
						else if (num <= 1842662042U)
						{
							if (num != 1041399898U)
							{
								if (num != 1842662042U)
								{
									goto IL_027D;
								}
								if (!(itemTypeAsString == "Stone"))
								{
									goto IL_027D;
								}
							}
							else if (!(itemTypeAsString == "Mount"))
							{
								goto IL_027D;
							}
						}
						else if (num != 1894730868U)
						{
							if (num != 2039097040U)
							{
								goto IL_027D;
							}
							if (!(itemTypeAsString == "Shield"))
							{
								goto IL_027D;
							}
						}
						else if (!(itemTypeAsString == "Javelin"))
						{
							goto IL_027D;
						}
					}
					else if (num <= 3440297014U)
					{
						if (num <= 2665595067U)
						{
							if (num != 2233436357U)
							{
								if (num != 2665595067U)
								{
									goto IL_027D;
								}
								if (!(itemTypeAsString == "Axe"))
								{
									goto IL_027D;
								}
							}
							else if (!(itemTypeAsString == "PickUp"))
							{
								goto IL_027D;
							}
						}
						else if (num != 3332997230U)
						{
							if (num != 3440297014U)
							{
								goto IL_027D;
							}
							if (!(itemTypeAsString == "Sword"))
							{
								goto IL_027D;
							}
						}
						else if (!(itemTypeAsString == "ThrowingKnife"))
						{
							goto IL_027D;
						}
					}
					else if (num <= 3687274959U)
					{
						if (num != 3637216139U)
						{
							if (num != 3687274959U)
							{
								goto IL_027D;
							}
							if (!(itemTypeAsString == "Ammo"))
							{
								goto IL_027D;
							}
						}
						else if (!(itemTypeAsString == "Bow"))
						{
							goto IL_027D;
						}
					}
					else if (num != 3778748927U)
					{
						if (num != 4282369777U)
						{
							goto IL_027D;
						}
						if (!(itemTypeAsString == "Crossbow"))
						{
							goto IL_027D;
						}
					}
					else if (!(itemTypeAsString == "ThrowingAxe"))
					{
						goto IL_027D;
					}
					brushName = "Item.Type.Icon." + this.ItemTypeAsString;
					goto IL_02AB;
					IL_027D:
					Debug.FailedAssert("Unidentified item type to show type for: " + this.ItemTypeAsString, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\ItemTypeVisualBrushWidget.cs", "OnLateUpdate", 66);
				}
				else
				{
					brushName = "Item.Type.Icon.None";
				}
				IL_02AB:
				if (!string.IsNullOrEmpty(brushName))
				{
					base.Brush = base.Context.Brushes.SingleOrDefault<Brush>((Brush b) => b.Name == brushName);
				}
				this._isInitialized = true;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000245 RID: 581 RVA: 0x00008452 File Offset: 0x00006652
		// (set) Token: 0x06000246 RID: 582 RVA: 0x0000845A File Offset: 0x0000665A
		[Editor(false)]
		public string ItemTypeAsString
		{
			get
			{
				return this._itemTypeAsString;
			}
			set
			{
				if (value != this._itemTypeAsString)
				{
					this._itemTypeAsString = value;
					base.OnPropertyChanged<string>(value, "ItemTypeAsString");
					this._isInitialized = false;
				}
			}
		}

		// Token: 0x0400010D RID: 269
		private const string ItemTypeBrushNameBase = "Item.Type.Icon.";

		// Token: 0x0400010E RID: 270
		private bool _isInitialized;

		// Token: 0x0400010F RID: 271
		private string _itemTypeAsString;
	}
}
