using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core.ViewModelCollection.BannerEditor
{
	// Token: 0x0200002E RID: 46
	public class BannerViewModel : ViewModel
	{
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060001EC RID: 492 RVA: 0x000060A4 File Offset: 0x000042A4
		public Banner Banner { get; }

		// Token: 0x060001ED RID: 493 RVA: 0x000060AC File Offset: 0x000042AC
		public BannerViewModel(Banner banner)
		{
			this.Banner = banner;
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060001EE RID: 494 RVA: 0x000060BB File Offset: 0x000042BB
		// (set) Token: 0x060001EF RID: 495 RVA: 0x000060C8 File Offset: 0x000042C8
		[DataSourceProperty]
		public string BannerCode
		{
			get
			{
				return this.Banner.BannerCode;
			}
			set
			{
				if (value != this.Banner.BannerCode)
				{
					this.Banner.Deserialize(value);
					base.OnPropertyChangedWithValue<string>(value, "BannerCode");
				}
			}
		}
	}
}
