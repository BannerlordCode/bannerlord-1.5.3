using System;
using SandBox.View.Map;
using SandBox.ViewModelCollection.Map.Incidents;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Incidents;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x0200003A RID: 58
	[OverrideView(typeof(MapIncidentView))]
	public class GauntletMapIncidentView : MapIncidentView
	{
		// Token: 0x060002B6 RID: 694 RVA: 0x00010614 File Offset: 0x0000E814
		public GauntletMapIncidentView(Incident incident)
			: base(incident)
		{
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0001061D File Offset: 0x0000E81D
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
			}
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x00010639 File Offset: 0x0000E839
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, false);
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x00010658 File Offset: 0x0000E858
		protected override void CreateLayout()
		{
			base.CreateLayout();
			if (this.Incident == null)
			{
				Debug.FailedAssert("Failed to start incident view", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.GauntletUI\\Map\\GauntletMapIncidentView.cs", "CreateLayout", 60);
				return;
			}
			this._controlModeBeforeIncident = Campaign.Current.TimeControlMode;
			this._controlModeLockBeforeIncident = Campaign.Current.TimeControlModeLock;
			Campaign.Current.TimeControlMode = CampaignTimeControlMode.Stop;
			Campaign.Current.SetTimeControlModeLock(true);
			MBCommon.PauseGameEngine();
			this._dataSource = new MapIncidentVM(this.Incident, new Action(this.OnCloseView));
			this._dataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._gauntletLayer = new GauntletLayer("MapIncidents", 203, false);
			this._gauntletLayer.LoadMovie("MapIncident", this._dataSource);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			base.Layer = this._gauntletLayer;
			base.MapScreen.AddLayer(base.Layer);
			this._spriteCategory = UIResourceManager.LoadSpriteCategory("ui_map_incidents");
			base.Layer.IsFocusLayer = true;
			ScreenManager.TrySetFocus(base.Layer);
			base.MapScreen.SetIsMapIncidentActive(true);
			this.PlayIncidentSound();
		}

		// Token: 0x060002BA RID: 698 RVA: 0x000107B0 File Offset: 0x0000E9B0
		protected override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			this.Tick();
		}

		// Token: 0x060002BB RID: 699 RVA: 0x000107BF File Offset: 0x0000E9BF
		protected override void OnIdleTick(float dt)
		{
			base.OnIdleTick(dt);
			this.Tick();
		}

		// Token: 0x060002BC RID: 700 RVA: 0x000107CE File Offset: 0x0000E9CE
		protected override void OnMenuModeTick(float dt)
		{
			base.OnMenuModeTick(dt);
			this.Tick();
		}

		// Token: 0x060002BD RID: 701 RVA: 0x000107E0 File Offset: 0x0000E9E0
		private void Tick()
		{
			if (this._dataSource != null && this._gauntletLayer.Input.IsHotKeyReleased("Confirm") && this._dataSource.CanConfirm)
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._dataSource.ExecuteConfirm();
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0001082E File Offset: 0x0000EA2E
		protected override bool IsOpeningEscapeMenuOnFocusChangeAllowed()
		{
			return false;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x00010831 File Offset: 0x0000EA31
		private void OnCloseView()
		{
			base.MapScreen.RemoveMapView(this);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00010840 File Offset: 0x0000EA40
		protected override void OnFinalize()
		{
			base.OnFinalize();
			if (MBCommon.IsPaused)
			{
				MBCommon.UnPauseGameEngine();
			}
			if (base.Layer != null)
			{
				this._spriteCategory.Unload();
				this._dataSource.OnFinalize();
				this._dataSource = null;
				base.Layer.IsFocusLayer = false;
				ScreenManager.TryLoseFocus(base.Layer);
				base.MapScreen.RemoveLayer(base.Layer);
				base.MapScreen.SetIsMapIncidentActive(false);
				Campaign.Current.TimeControlMode = this._controlModeBeforeIncident;
				Campaign.Current.SetTimeControlModeLock(this._controlModeLockBeforeIncident);
				return;
			}
			if (this._dataSource != null || this._spriteCategory != null)
			{
				Debug.FailedAssert("Incident view is was not propertly initialized", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.GauntletUI\\Map\\GauntletMapIncidentView.cs", "OnFinalize", 166);
				MapIncidentVM dataSource = this._dataSource;
				if (dataSource != null)
				{
					dataSource.OnFinalize();
				}
				SpriteCategory spriteCategory = this._spriteCategory;
				if (spriteCategory == null)
				{
					return;
				}
				spriteCategory.Unload();
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00010924 File Offset: 0x0000EB24
		private void PlayIncidentSound()
		{
			string text = "";
			string typeId = this.Incident.TypeId;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(typeId);
			if (num <= 2183768032U)
			{
				if (num <= 802596536U)
				{
					if (num != 189238540U)
					{
						if (num != 421159677U)
						{
							if (num == 802596536U)
							{
								if (typeId == "DreamsSongsAndSigns")
								{
									text = "event:/ui/encounter/dreams_signs";
								}
							}
						}
						else if (typeId == "HuntingForaging")
						{
							text = "event:/ui/encounter/hunting_foraging";
						}
					}
					else if (typeId == "TroopSettlementRelation")
					{
						text = "event:/ui/encounter/troop_settlement";
					}
				}
				else if (num <= 1147955296U)
				{
					if (num != 868532207U)
					{
						if (num == 1147955296U)
						{
							if (typeId == "FiefManagement")
							{
								text = "event:/ui/encounter/fief";
							}
						}
					}
					else if (typeId == "AnimalIllness")
					{
						text = "event:/ui/encounter/sick_animals";
					}
				}
				else if (num != 2173941516U)
				{
					if (num == 2183768032U)
					{
						if (typeId == "PartyCampLife")
						{
							text = "event:/ui/encounter/camp";
						}
					}
				}
				else if (typeId == "Siege")
				{
					text = "event:/ui/encounter/siege";
				}
			}
			else if (num <= 2949999526U)
			{
				if (num != 2734782823U)
				{
					if (num != 2771884965U)
					{
						if (num == 2949999526U)
						{
							if (typeId == "PlightOfCivilians")
							{
								text = "event:/ui/encounter/plight";
							}
						}
					}
					else if (typeId == "PostBattle")
					{
						text = "event:/ui/encounter/post_battle";
					}
				}
				else if (typeId == "Profit")
				{
					text = "event:/ui/encounter/profit";
				}
			}
			else if (num <= 3794521132U)
			{
				if (num != 3234036840U)
				{
					if (num == 3794521132U)
					{
						if (typeId == "Workshop")
						{
							text = "event:/ui/encounter/workshops";
						}
					}
				}
				else if (typeId == "FoodConsumption")
				{
					text = "event:/ui/encounter/food_spoil";
				}
			}
			else if (num != 3829598375U)
			{
				if (num == 4193304736U)
				{
					if (typeId == "HardTravel")
					{
						text = "event:/ui/encounter/hard_travel";
					}
				}
			}
			else if (typeId == "Illness")
			{
				text = "event:/ui/encounter/illness";
			}
			if (!string.IsNullOrEmpty(text))
			{
				UISoundsHelper.PlayUISound(text);
			}
		}

		// Token: 0x040000FE RID: 254
		private MapIncidentVM _dataSource;

		// Token: 0x040000FF RID: 255
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000100 RID: 256
		private SpriteCategory _spriteCategory;

		// Token: 0x04000101 RID: 257
		private bool _controlModeLockBeforeIncident;

		// Token: 0x04000102 RID: 258
		private CampaignTimeControlMode _controlModeBeforeIncident;
	}
}
