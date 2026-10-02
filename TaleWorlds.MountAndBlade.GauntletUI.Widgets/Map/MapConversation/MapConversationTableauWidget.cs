using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapConversation
{
	// Token: 0x0200012B RID: 299
	public class MapConversationTableauWidget : TextureWidget
	{
		// Token: 0x06000FD0 RID: 4048 RVA: 0x0002BE25 File Offset: 0x0002A025
		public MapConversationTableauWidget(UIContext context)
			: base(context)
		{
			base.TextureProviderName = "MapConversationTextureProvider";
			this._isRenderRequestedPreviousFrame = false;
			base.UpdateTextureWidget();
			base.EventManager.AddAfterFinalizedCallback(new Action(this.OnEventManagerIsFinalized));
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0002BE5D File Offset: 0x0002A05D
		private void OnEventManagerIsFinalized()
		{
			if (!base.SetForClearNextFrame)
			{
				TextureProvider textureProvider = base.TextureProvider;
				if (textureProvider != null)
				{
					textureProvider.SetProperty("IsReleased", true);
				}
				TextureProvider textureProvider2 = base.TextureProvider;
				if (textureProvider2 == null)
				{
					return;
				}
				textureProvider2.Clear(false);
			}
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x0002BE94 File Offset: 0x0002A094
		public override void OnClearTextureProvider()
		{
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x0002BE96 File Offset: 0x0002A096
		// (set) Token: 0x06000FD4 RID: 4052 RVA: 0x0002BEA0 File Offset: 0x0002A0A0
		[Editor(false)]
		public object Data
		{
			get
			{
				return this._data;
			}
			set
			{
				if (value != this._data)
				{
					this._data = value;
					base.OnPropertyChanged<object>(value, "Data");
					base.SetTextureProviderProperty("IsEnabled", this._data != null);
					base.SetTextureProviderProperty("Data", value);
					if (this._data != null)
					{
						this._isRenderRequestedPreviousFrame = true;
					}
				}
			}
		}

		// Token: 0x0400073A RID: 1850
		private object _data;
	}
}
