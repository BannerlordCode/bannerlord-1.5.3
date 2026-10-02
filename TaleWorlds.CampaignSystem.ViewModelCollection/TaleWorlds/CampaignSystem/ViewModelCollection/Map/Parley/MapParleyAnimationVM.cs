using System;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.Parley
{
	// Token: 0x02000037 RID: 55
	public class MapParleyAnimationVM : ViewModel
	{
		// Token: 0x0600057F RID: 1407 RVA: 0x0001E16E File Offset: 0x0001C36E
		public MapParleyAnimationVM(PartyBase parleyedParty, float animationDuration)
		{
			this._parleyedParty = parleyedParty;
			this.AnimationDuration = animationDuration;
			this.RefreshValues();
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001E19B File Offset: 0x0001C39B
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ParleyTextObj.SetTextVariable("PARTY_NAME", this._parleyedParty.Name);
			this.ParleyText = this.ParleyTextObj.ToString();
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001E1D0 File Offset: 0x0001C3D0
		public override void OnFinalize()
		{
			base.OnFinalize();
			this._parleyedParty = null;
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x0001E1DF File Offset: 0x0001C3DF
		// (set) Token: 0x06000583 RID: 1411 RVA: 0x0001E1E7 File Offset: 0x0001C3E7
		[DataSourceProperty]
		public string ParleyText
		{
			get
			{
				return this._parleyText;
			}
			set
			{
				if (this._parleyText != value)
				{
					this._parleyText = value;
					base.OnPropertyChangedWithValue<string>(value, "ParleyText");
				}
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000584 RID: 1412 RVA: 0x0001E20A File Offset: 0x0001C40A
		// (set) Token: 0x06000585 RID: 1413 RVA: 0x0001E212 File Offset: 0x0001C412
		[DataSourceProperty]
		public float AnimationDuration
		{
			get
			{
				return this._animationDuration;
			}
			set
			{
				if (this._animationDuration != value)
				{
					this._animationDuration = value;
					base.OnPropertyChangedWithValue(value, "AnimationDuration");
				}
			}
		}

		// Token: 0x04000259 RID: 601
		private readonly TextObject ParleyTextObj = new TextObject("{=LZbHWkCB}Parleying with {PARTY_NAME}", null);

		// Token: 0x0400025A RID: 602
		private PartyBase _parleyedParty;

		// Token: 0x0400025B RID: 603
		private string _parleyText;

		// Token: 0x0400025C RID: 604
		private float _animationDuration;
	}
}
