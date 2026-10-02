using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Menu.TownManagement
{
	// Token: 0x02000127 RID: 295
	public class VillageTypeVisualIconBrushWidget : BrushWidget
	{
		// Token: 0x06000FB8 RID: 4024 RVA: 0x0002B9C3 File Offset: 0x00029BC3
		public VillageTypeVisualIconBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000FB9 RID: 4025 RVA: 0x0002B9CC File Offset: 0x00029BCC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				switch (this.VillageType)
				{
				case 0:
					this.SetState("Default");
					break;
				case 1:
					this.SetState("EuropeHorseRanch");
					break;
				case 2:
					this.SetState("BattanianHorseRanch");
					break;
				case 3:
					this.SetState("SteppeHorseRanch");
					break;
				case 4:
					this.SetState("DesertHorseRanch");
					break;
				case 5:
					this.SetState("WheatFarm");
					break;
				case 6:
					this.SetState("Lumberjack");
					break;
				case 7:
					this.SetState("ClayMine");
					break;
				case 8:
					this.SetState("SaltMine");
					break;
				case 9:
					this.SetState("IronMine");
					break;
				case 10:
					this.SetState("Fisherman");
					break;
				case 11:
					this.SetState("CattleRange");
					break;
				case 12:
					this.SetState("SheepFarm");
					break;
				case 13:
					this.SetState("VineYard");
					break;
				case 14:
					this.SetState("FlaxPlant");
					break;
				case 15:
					this.SetState("DateFarm");
					break;
				case 16:
					this.SetState("OliveTrees");
					break;
				case 17:
					this.SetState("SilkPlant");
					break;
				case 18:
					this.SetState("SilverMine");
					break;
				default:
					Debug.FailedAssert("No workshop visual with this type: " + this.VillageType, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Map\\Menu\\TownManagement\\VillageTypeVisualIconBrushWidget.cs", "OnLateUpdate", 103);
					this.SetState("Default");
					break;
				}
				this._initialized = true;
			}
		}

		// Token: 0x17000597 RID: 1431
		// (get) Token: 0x06000FBA RID: 4026 RVA: 0x0002BB9C File Offset: 0x00029D9C
		// (set) Token: 0x06000FBB RID: 4027 RVA: 0x0002BBA4 File Offset: 0x00029DA4
		[Editor(false)]
		public int VillageType
		{
			get
			{
				return this._villageType;
			}
			set
			{
				if (this._villageType != value)
				{
					this._villageType = value;
					base.OnPropertyChanged(value, "VillageType");
				}
			}
		}

		// Token: 0x04000731 RID: 1841
		private bool _initialized;

		// Token: 0x04000732 RID: 1842
		private int _villageType;
	}
}
