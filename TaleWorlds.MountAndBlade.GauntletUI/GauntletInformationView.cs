using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI
{
	// Token: 0x02000013 RID: 19
	public class GauntletInformationView : GlobalLayer
	{
		// Token: 0x0600009C RID: 156 RVA: 0x000051A8 File Offset: 0x000033A8
		private GauntletInformationView()
		{
			this._layerAsGauntletLayer = new GauntletLayer("Tooltip", 115000, false);
			InformationManager.OnShowTooltip += this.OnShowTooltip;
			InformationManager.OnHideTooltip += this.OnHideTooltip;
			InformationManager.IsAnyTooltipActiveInternal = (InformationManager.IsAnyTooltipActiveDelegate)Delegate.Combine(InformationManager.IsAnyTooltipActiveInternal, new InformationManager.IsAnyTooltipActiveDelegate(this.OnGetIsAnyTooltipActive));
			base.Layer = this._layerAsGauntletLayer;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00005220 File Offset: 0x00003420
		public static void Initialize()
		{
			if (GauntletInformationView._current == null)
			{
				GauntletInformationView._current = new GauntletInformationView();
				ScreenManager.AddGlobalLayer(GauntletInformationView._current, false);
				PropertyBasedTooltipVM.AddKeyType("MapClick", () => GauntletInformationView._current.GetKey("MapHotKeyCategory", "MapClick"));
				PropertyBasedTooltipVM.AddKeyType("FollowModifier", () => GauntletInformationView._current.GetKey("MapHotKeyCategory", "MapFollowModifier"));
				PropertyBasedTooltipVM.AddKeyType("ExtendModifier", () => GauntletInformationView._current.GetExtendTooltipKeyText());
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000052C8 File Offset: 0x000034C8
		public static void OnFinalize()
		{
			if (GauntletInformationView._current != null)
			{
				InformationManager.OnShowTooltip -= GauntletInformationView._current.OnShowTooltip;
				InformationManager.OnHideTooltip -= GauntletInformationView._current.OnHideTooltip;
				InformationManager.IsAnyTooltipActiveInternal = (InformationManager.IsAnyTooltipActiveDelegate)Delegate.Remove(InformationManager.IsAnyTooltipActiveInternal, new InformationManager.IsAnyTooltipActiveDelegate(GauntletInformationView._current.OnGetIsAnyTooltipActive));
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000532C File Offset: 0x0000352C
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._dataSource != null && (Input.IsKeyDown(InputKey.LeftAlt) || Input.IsKeyDown(InputKey.RightAlt) || Input.IsKeyDown(InputKey.ControllerLBumper)))
			{
				this._gamepadTooltipExtendTimer += dt;
			}
			else
			{
				this._gamepadTooltipExtendTimer = 0f;
			}
			if (this._dataSource != null)
			{
				this._dataSource.Tick(dt);
				this._dataSource.IsExtended = (Input.IsGamepadActive ? (this._gamepadTooltipExtendTimer > 0.18f) : (this._gamepadTooltipExtendTimer > 0f));
			}
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000053C6 File Offset: 0x000035C6
		private string GetExtendTooltipKeyText()
		{
			if (Input.IsGamepadActive)
			{
				return this.GetKey("MapHotKeyCategory", "MapFollowModifier");
			}
			return Game.Current.GameTextManager.FindText("str_game_key_text", "anyalt").ToString();
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000053FE File Offset: 0x000035FE
		private string GetKey(string categoryId, string keyId)
		{
			return Game.Current.GameTextManager.GetHotKeyGameText(categoryId, keyId).ToString();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00005416 File Offset: 0x00003616
		private string GetKey(string categoryId, int keyId)
		{
			return Game.Current.GameTextManager.GetHotKeyGameText(categoryId, keyId).ToString();
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005430 File Offset: 0x00003630
		private void OnShowTooltip(Type type, object[] args)
		{
			this.OnHideTooltip();
			InformationManager.TooltipRegistry tooltipRegistry;
			if (InformationManager.RegisteredTypes.TryGetValue(type, out tooltipRegistry))
			{
				try
				{
					this._dataSource = Activator.CreateInstance(tooltipRegistry.TooltipType, new object[] { type, args }) as TooltipBaseVM;
					this._movie = this._layerAsGauntletLayer.LoadMovie(tooltipRegistry.MovieName, this._dataSource);
					return;
				}
				catch (Exception ex)
				{
					Debug.FailedAssert(string.Format("Failed to display tooltip of type: {0}. Exception: {1}", type.FullName, ex), "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\GauntletInformationView.cs", "OnShowTooltip", 112);
					return;
				}
			}
			Debug.FailedAssert("Unable to show tooltip. Either the given type or the corresponding tooltip type is not added to TooltipMappingProvider. Given type: " + type.FullName, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI\\GauntletInformationView.cs", "OnShowTooltip", 117);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000054EC File Offset: 0x000036EC
		private void OnHideTooltip()
		{
			TooltipBaseVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnFinalize();
			}
			if (this._movie != null)
			{
				this._layerAsGauntletLayer.ReleaseMovie(this._movie);
			}
			this._dataSource = null;
			this._movie = null;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005526 File Offset: 0x00003726
		private void OnGetIsAnyTooltipActive(out bool isAnyTooltipActive, out bool isAnyTooltipExtended)
		{
			TooltipBaseVM dataSource = this._dataSource;
			isAnyTooltipActive = dataSource != null && dataSource.IsActive;
			TooltipBaseVM dataSource2 = this._dataSource;
			isAnyTooltipExtended = dataSource2 != null && dataSource2.IsExtended;
		}

		// Token: 0x04000063 RID: 99
		private TooltipBaseVM _dataSource;

		// Token: 0x04000064 RID: 100
		private GauntletMovieIdentifier _movie;

		// Token: 0x04000065 RID: 101
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x04000066 RID: 102
		private static GauntletInformationView _current;

		// Token: 0x04000067 RID: 103
		private const float _tooltipExtendTreshold = 0.18f;

		// Token: 0x04000068 RID: 104
		private float _gamepadTooltipExtendTimer;
	}
}
