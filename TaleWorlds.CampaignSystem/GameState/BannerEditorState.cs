using System;
using TaleWorlds.Core;

namespace TaleWorlds.CampaignSystem.GameState
{
	// Token: 0x020003A0 RID: 928
	public class BannerEditorState : GameState
	{
		// Token: 0x17000CAF RID: 3247
		// (get) Token: 0x06003697 RID: 13975 RVA: 0x000DF0D6 File Offset: 0x000DD2D6
		public override bool IsMenuState
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x06003698 RID: 13976 RVA: 0x000DF0D9 File Offset: 0x000DD2D9
		// (set) Token: 0x06003699 RID: 13977 RVA: 0x000DF0E1 File Offset: 0x000DD2E1
		public IBannerEditorStateHandler Handler
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

		// Token: 0x0600369A RID: 13978 RVA: 0x000DF0EA File Offset: 0x000DD2EA
		public BannerEditorState()
		{
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x000DF0F2 File Offset: 0x000DD2F2
		public BannerEditorState(Action endAction)
		{
			this._onEndAction = endAction;
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x000DF101 File Offset: 0x000DD301
		public Clan GetClan()
		{
			return Clan.PlayerClan;
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x000DF108 File Offset: 0x000DD308
		public CharacterObject GetCharacter()
		{
			return CharacterObject.PlayerCharacter;
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x000DF10F File Offset: 0x000DD30F
		protected override void OnFinalize()
		{
			base.OnFinalize();
			Action onEndAction = this._onEndAction;
			if (onEndAction == null)
			{
				return;
			}
			onEndAction();
		}

		// Token: 0x04000F58 RID: 3928
		private IBannerEditorStateHandler _handler;

		// Token: 0x04000F59 RID: 3929
		private Action _onEndAction;
	}
}
