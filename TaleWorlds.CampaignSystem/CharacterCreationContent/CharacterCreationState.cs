using System;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
	// Token: 0x02000218 RID: 536
	public class CharacterCreationState : PlayerGameState
	{
		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x060020B7 RID: 8375 RVA: 0x00093DA4 File Offset: 0x00091FA4
		// (set) Token: 0x060020B8 RID: 8376 RVA: 0x00093DAC File Offset: 0x00091FAC
		public CharacterCreationManager CharacterCreationManager
		{
			get
			{
				return this._characterCreationManager;
			}
			private set
			{
				this._characterCreationManager = value;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x060020B9 RID: 8377 RVA: 0x00093DB5 File Offset: 0x00091FB5
		// (set) Token: 0x060020BA RID: 8378 RVA: 0x00093DBD File Offset: 0x00091FBD
		public ICharacterCreationStateHandler Handler
		{
			get
			{
				return this._handler;
			}
			set
			{
				this._handler = value;
			}
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x00093DC6 File Offset: 0x00091FC6
		public CharacterCreationState()
		{
			this.CharacterCreationManager = new CharacterCreationManager(this);
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x00093DDA File Offset: 0x00091FDA
		protected override void OnInitialize()
		{
			base.OnInitialize();
			Game.Current.GameStateManager.RegisterActiveStateDisableRequest(this);
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x00093DF2 File Offset: 0x00091FF2
		protected override void OnActivate()
		{
			base.OnActivate();
			this.CharacterCreationManager.OnStateActivated();
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x00093E08 File Offset: 0x00092008
		public void FinalizeCharacterCreationState()
		{
			Game.Current.GameStateManager.UnregisterActiveStateDisableRequest(this);
			Game.Current.GameStateManager.CleanAndPushState(Game.Current.GameStateManager.CreateState<MapState>(), 0);
			PartyBase.MainParty.SetVisualAsDirty();
			Hero.MainHero.Gold = 1000;
			MobileParty.MainParty.ItemRoster.AddToCounts(DefaultItems.Grain, 1);
			ICharacterCreationStateHandler handler = this._handler;
			if (handler != null)
			{
				handler.OnCharacterCreationFinalized();
			}
			CampaignEventDispatcher.Instance.OnCharacterCreationIsOver();
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x00093E8E File Offset: 0x0009208E
		public void Refresh()
		{
			ICharacterCreationStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnRefresh();
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x00093EA0 File Offset: 0x000920A0
		public void OnStageActivated(CharacterCreationStageBase stage)
		{
			ICharacterCreationStateHandler handler = this._handler;
			if (handler == null)
			{
				return;
			}
			handler.OnStageCreated(stage);
		}

		// Token: 0x04000985 RID: 2437
		private CharacterCreationManager _characterCreationManager;

		// Token: 0x04000986 RID: 2438
		private ICharacterCreationStateHandler _handler;
	}
}
