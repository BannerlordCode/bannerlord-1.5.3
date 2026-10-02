using System;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.GameMenu.Overlay
{
	// Token: 0x020000BE RID: 190
	public class GameMenuOverlayActionVM : StringItemWithEnabledAndHintVM
	{
		// Token: 0x06001271 RID: 4721 RVA: 0x0004AD74 File Offset: 0x00048F74
		public GameMenuOverlayActionVM(Action<object> onExecute, string item, bool isEnabled, object identifier, TextObject hint = null)
			: base(onExecute, item, isEnabled, identifier, hint)
		{
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001272 RID: 4722 RVA: 0x0004AD83 File Offset: 0x00048F83
		// (set) Token: 0x06001273 RID: 4723 RVA: 0x0004AD8B File Offset: 0x00048F8B
		[DataSourceProperty]
		public bool IsHiglightEnabled
		{
			get
			{
				return this._isHiglightEnabled;
			}
			set
			{
				if (value != this._isHiglightEnabled)
				{
					this._isHiglightEnabled = value;
					base.OnPropertyChangedWithValue(value, "IsHiglightEnabled");
				}
			}
		}

		// Token: 0x04000860 RID: 2144
		private bool _isHiglightEnabled;
	}
}
