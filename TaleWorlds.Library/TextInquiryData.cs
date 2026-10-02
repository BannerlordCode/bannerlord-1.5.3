using System;

namespace TaleWorlds.Library
{
	// Token: 0x0200003E RID: 62
	public class TextInquiryData
	{
		// Token: 0x06000203 RID: 515 RVA: 0x00007A50 File Offset: 0x00005C50
		public TextInquiryData(string titleText, string text, bool isAffirmativeOptionShown, bool isNegativeOptionShown, string affirmativeText, string negativeText, Action<string> affirmativeAction, Action negativeAction, bool shouldInputBeObfuscated = false, Func<string, Tuple<bool, string>> textCondition = null, string soundEventPath = "", string defaultInputText = "")
		{
			this.TitleText = titleText;
			this.Text = text;
			this.IsAffirmativeOptionShown = isAffirmativeOptionShown;
			this.IsNegativeOptionShown = isNegativeOptionShown;
			this.AffirmativeText = affirmativeText;
			this.NegativeText = negativeText;
			this.AffirmativeAction = affirmativeAction;
			this.NegativeAction = negativeAction;
			this.TextCondition = textCondition;
			this.IsInputObfuscated = shouldInputBeObfuscated;
			this.SoundEventPath = soundEventPath;
			this.DefaultInputText = defaultInputText;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00007ACC File Offset: 0x00005CCC
		public bool HasSameContentWith(object other)
		{
			TextInquiryData textInquiryData;
			return (textInquiryData = other as TextInquiryData) != null && (this.TitleText == textInquiryData.TitleText && this.Text == textInquiryData.Text && this.IsAffirmativeOptionShown == textInquiryData.IsAffirmativeOptionShown && this.IsNegativeOptionShown == textInquiryData.IsNegativeOptionShown && this.AffirmativeText == textInquiryData.AffirmativeText && this.NegativeText == textInquiryData.NegativeText && this.AffirmativeAction == textInquiryData.AffirmativeAction && this.NegativeAction == textInquiryData.NegativeAction && this.TextCondition == textInquiryData.TextCondition && this.IsInputObfuscated == textInquiryData.IsInputObfuscated && this.SoundEventPath == textInquiryData.SoundEventPath) && this.DefaultInputText == textInquiryData.DefaultInputText;
		}

		// Token: 0x040000CC RID: 204
		public string TitleText;

		// Token: 0x040000CD RID: 205
		public string Text = "";

		// Token: 0x040000CE RID: 206
		public readonly bool IsAffirmativeOptionShown;

		// Token: 0x040000CF RID: 207
		public readonly bool IsNegativeOptionShown;

		// Token: 0x040000D0 RID: 208
		public readonly bool IsInputObfuscated;

		// Token: 0x040000D1 RID: 209
		public readonly string AffirmativeText;

		// Token: 0x040000D2 RID: 210
		public readonly string NegativeText;

		// Token: 0x040000D3 RID: 211
		public readonly string SoundEventPath;

		// Token: 0x040000D4 RID: 212
		public readonly string DefaultInputText;

		// Token: 0x040000D5 RID: 213
		public readonly Action<string> AffirmativeAction;

		// Token: 0x040000D6 RID: 214
		public readonly Action NegativeAction;

		// Token: 0x040000D7 RID: 215
		public readonly Func<string, Tuple<bool, string>> TextCondition;
	}
}
