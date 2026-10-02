using System;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.ViewModelCollection.Barter;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.TwoDimension;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000033 RID: 51
	public class GauntletMapConversationBarterView
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000266 RID: 614 RVA: 0x0000F02A File Offset: 0x0000D22A
		// (set) Token: 0x06000267 RID: 615 RVA: 0x0000F032 File Offset: 0x0000D232
		public bool IsCreated { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000268 RID: 616 RVA: 0x0000F03B File Offset: 0x0000D23B
		// (set) Token: 0x06000269 RID: 617 RVA: 0x0000F043 File Offset: 0x0000D243
		public bool IsActive { get; private set; }

		// Token: 0x0600026A RID: 618 RVA: 0x0000F04C File Offset: 0x0000D24C
		public GauntletMapConversationBarterView(GauntletLayer layer, GauntletMapConversationBarterView.OnBarterActiveStateChanged onActiveStateChanged)
		{
			this._gauntletLayer = layer;
			this._onActiveStateChanged = onActiveStateChanged;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000F064 File Offset: 0x0000D264
		public void CreateBarterView(BarterData args)
		{
			this._barterDataSource = new BarterVM(args);
			this._barterDataSource.SetResetInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Reset"));
			this._barterDataSource.SetDoneInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Confirm"));
			this._barterDataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
			GauntletMapConversationBarterView.OnBarterActiveStateChanged onActiveStateChanged = this._onActiveStateChanged;
			if (onActiveStateChanged != null)
			{
				onActiveStateChanged(true);
			}
			this._barterCategory = UIResourceManager.GetSpriteCategory("ui_barter");
			this.Activate();
			this.IsCreated = true;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000F10C File Offset: 0x0000D30C
		public void DestroyBarterView()
		{
			this.Deactivate();
			this._barterDataSource.OnFinalize();
			this._barterDataSource = null;
			this._barterCategory = null;
			GauntletMapConversationBarterView.OnBarterActiveStateChanged onActiveStateChanged = this._onActiveStateChanged;
			if (onActiveStateChanged != null)
			{
				onActiveStateChanged(false);
			}
			BarterItemVM.IsFiveStackModifierActive = false;
			BarterItemVM.IsEntireStackModifierActive = false;
			this.IsCreated = false;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000F160 File Offset: 0x0000D360
		public void Activate()
		{
			this._barterMovie = this._gauntletLayer.LoadMovie("BarterScreen", this._barterDataSource);
			this._barterCategory.Load();
			GauntletMapConversationBarterView.OnBarterActiveStateChanged onActiveStateChanged = this._onActiveStateChanged;
			if (onActiveStateChanged != null)
			{
				onActiveStateChanged(true);
			}
			this.IsActive = true;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000F1AD File Offset: 0x0000D3AD
		public void Deactivate()
		{
			this._gauntletLayer.ReleaseMovie(this._barterMovie);
			this._barterCategory.Unload();
			this.IsActive = false;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000F1D4 File Offset: 0x0000D3D4
		public void TickInput()
		{
			if (this._gauntletLayer.Input.IsHotKeyReleased("Exit"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._barterDataSource.ExecuteCancel();
				return;
			}
			if (this._gauntletLayer.Input.IsHotKeyReleased("Confirm"))
			{
				BarterVM barterDataSource = this._barterDataSource;
				if (barterDataSource != null && !barterDataSource.IsOfferDisabled)
				{
					UISoundsHelper.PlayUISound("event:/ui/default");
					this._barterDataSource.ExecuteOffer();
					return;
				}
			}
			if (this._gauntletLayer.Input.IsHotKeyReleased("Reset"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._barterDataSource.ExecuteReset();
			}
		}

		// Token: 0x040000D7 RID: 215
		private readonly GauntletLayer _gauntletLayer;

		// Token: 0x040000D8 RID: 216
		private readonly GauntletMapConversationBarterView.OnBarterActiveStateChanged _onActiveStateChanged;

		// Token: 0x040000D9 RID: 217
		private SpriteCategory _barterCategory;

		// Token: 0x040000DA RID: 218
		private BarterVM _barterDataSource;

		// Token: 0x040000DB RID: 219
		private GauntletMovieIdentifier _barterMovie;

		// Token: 0x02000082 RID: 130
		// (Invoke) Token: 0x06000467 RID: 1127
		public delegate void OnBarterActiveStateChanged(bool isBarterActive);
	}
}
