using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000D2 RID: 210
	public class MultiplayerClassLoadoutTroopTupleVisualWidget : Widget
	{
		// Token: 0x06000AE8 RID: 2792 RVA: 0x0001E9D1 File Offset: 0x0001CBD1
		public MultiplayerClassLoadoutTroopTupleVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0001E9DC File Offset: 0x0001CBDC
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				base.Sprite = base.Context.SpriteData.GetSprite("MPClassLoadout\\TroopTupleImages\\" + this.TroopTypeCode + "1");
				base.Sprite = base.Sprite;
				base.SuggestedWidth = (float)base.Sprite.Width;
				base.SuggestedHeight = (float)base.Sprite.Height;
				this._initialized = true;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000AEA RID: 2794 RVA: 0x0001EA5A File Offset: 0x0001CC5A
		// (set) Token: 0x06000AEB RID: 2795 RVA: 0x0001EA62 File Offset: 0x0001CC62
		public string FactionCode
		{
			get
			{
				return this._factionCode;
			}
			set
			{
				if (value != this._factionCode)
				{
					this._factionCode = value;
					base.OnPropertyChanged<string>(value, "FactionCode");
				}
			}
		}

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x0001EA85 File Offset: 0x0001CC85
		// (set) Token: 0x06000AED RID: 2797 RVA: 0x0001EA8D File Offset: 0x0001CC8D
		public string TroopTypeCode
		{
			get
			{
				return this._troopTypeCode;
			}
			set
			{
				if (value != this._troopTypeCode)
				{
					this._troopTypeCode = value;
					base.OnPropertyChanged<string>(value, "TroopTypeCode");
				}
			}
		}

		// Token: 0x040004F4 RID: 1268
		private bool _initialized;

		// Token: 0x040004F5 RID: 1269
		private string _factionCode;

		// Token: 0x040004F6 RID: 1270
		private string _troopTypeCode;
	}
}
