using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem
{
	// Token: 0x0200008B RID: 139
	public struct ExplainedNumber
	{
		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x0600118A RID: 4490 RVA: 0x0005527C File Offset: 0x0005347C
		public float ResultNumber
		{
			get
			{
				return MathF.Clamp(this._unclampedResultNumber, this.LimitMinValue, this.LimitMaxValue);
			}
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x0600118B RID: 4491 RVA: 0x00055295 File Offset: 0x00053495
		public int RoundedResultNumber
		{
			get
			{
				return MathF.Round(this.ResultNumber);
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x0600118C RID: 4492 RVA: 0x000552A2 File Offset: 0x000534A2
		// (set) Token: 0x0600118D RID: 4493 RVA: 0x000552AA File Offset: 0x000534AA
		public float BaseNumber { get; private set; }

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x0600118E RID: 4494 RVA: 0x000552B3 File Offset: 0x000534B3
		public bool IncludeDescriptions
		{
			get
			{
				return this._explainer != null;
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x0600118F RID: 4495 RVA: 0x000552BE File Offset: 0x000534BE
		public float LimitMinValue
		{
			get
			{
				if (this._limitMinValue == null)
				{
					return float.MinValue;
				}
				return this._limitMinValue.Value;
			}
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001190 RID: 4496 RVA: 0x000552DF File Offset: 0x000534DF
		public float LimitMaxValue
		{
			get
			{
				if (this._limitMaxValue == null)
				{
					return float.MaxValue;
				}
				return this._limitMaxValue.Value;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001191 RID: 4497 RVA: 0x00055300 File Offset: 0x00053500
		// (set) Token: 0x06001192 RID: 4498 RVA: 0x00055308 File Offset: 0x00053508
		public float SumOfFactors { get; private set; }

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001193 RID: 4499 RVA: 0x00055311 File Offset: 0x00053511
		private float _unclampedResultNumber
		{
			get
			{
				return this.BaseNumber + this.BaseNumber * this.SumOfFactors;
			}
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x00055328 File Offset: 0x00053528
		public ExplainedNumber(float baseNumber = 0f, bool includeDescriptions = false, TextObject baseText = null)
		{
			this.BaseNumber = baseNumber;
			this._explainer = (includeDescriptions ? new ExplainedNumber.StatExplainer() : null);
			this.SumOfFactors = 0f;
			this._limitMinValue = new float?(float.MinValue);
			this._limitMaxValue = new float?(float.MaxValue);
			if (this._explainer != null && !this.BaseNumber.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				this._explainer.AddLine((baseText ?? ExplainedNumber.BaseText).ToString(), this.BaseNumber, ExplainedNumber.StatExplainer.OperationType.Base);
			}
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x000553B8 File Offset: 0x000535B8
		public string GetExplanations()
		{
			if (this._explainer == null)
			{
				return "";
			}
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(16, "GetExplanations");
			foreach (ValueTuple<string, float> valueTuple in this._explainer.GetLines(this.BaseNumber, this._unclampedResultNumber, null, null, null))
			{
				string text = string.Format("{0} : {1}{2:0.##}\n", valueTuple.Item1, (valueTuple.Item2 > 0.001f) ? "+" : "", valueTuple.Item2);
				mbstringBuilder.Append<string>(text);
			}
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x00055484 File Offset: 0x00053684
		[return: TupleElementNames(new string[] { "name", "number" })]
		public List<ValueTuple<string, float>> GetLines()
		{
			if (this._explainer == null)
			{
				return new List<ValueTuple<string, float>>();
			}
			return this._explainer.GetLines(this.BaseNumber, this._unclampedResultNumber, null, null, null);
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x000554B0 File Offset: 0x000536B0
		public void AddFromExplainedNumber(ExplainedNumber explainedNumber, TextObject baseText)
		{
			if (explainedNumber._explainer != null && this._explainer != null)
			{
				TextObject textObject = new TextObject("{=HKoLNyIm}{BASE} Maximum", null);
				TextObject textObject2 = new TextObject("{=0Fliz2vk}{BASE} Minimum", null);
				textObject.SetTextVariable("BASE", baseText);
				textObject2.SetTextVariable("BASE", baseText);
				foreach (ValueTuple<string, float> valueTuple in explainedNumber._explainer.GetLines(explainedNumber.BaseNumber, explainedNumber._unclampedResultNumber, baseText, textObject, textObject2))
				{
					this._explainer.AddLine(valueTuple.Item1, valueTuple.Item2, ExplainedNumber.StatExplainer.OperationType.Add);
				}
			}
			this.BaseNumber += explainedNumber.ResultNumber;
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x00055588 File Offset: 0x00053788
		public void SubtractFromExplainedNumber(ExplainedNumber explainedNumber, TextObject baseText)
		{
			if (explainedNumber._explainer != null && this._explainer != null)
			{
				TextObject textObject = new TextObject("{=HKoLNyIm}{BASE} Maximum", null);
				TextObject textObject2 = new TextObject("{=0Fliz2vk}{BASE} Minimum", null);
				textObject.SetTextVariable("BASE", baseText);
				textObject2.SetTextVariable("BASE", baseText);
				foreach (ValueTuple<string, float> valueTuple in explainedNumber._explainer.GetLines(explainedNumber.BaseNumber, explainedNumber._unclampedResultNumber, baseText, textObject, textObject2))
				{
					this._explainer.AddLine(valueTuple.Item1, -valueTuple.Item2, ExplainedNumber.StatExplainer.OperationType.Add);
				}
			}
			this.BaseNumber -= explainedNumber.ResultNumber;
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x00055660 File Offset: 0x00053860
		public void Add(float value, TextObject description = null, TextObject variable = null)
		{
			if (value.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return;
			}
			this.BaseNumber += value;
			if (this._explainer != null && description != null && !value.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				if (variable != null)
				{
					description.SetTextVariable("A0", variable);
				}
				this._explainer.AddLine(description.ToString(), value, ExplainedNumber.StatExplainer.OperationType.Add);
			}
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x000556DC File Offset: 0x000538DC
		public void AddFactor(float value, TextObject description = null)
		{
			if (value.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				return;
			}
			this.SumOfFactors += value;
			if (description != null && this._explainer != null && !value.ApproximatelyEqualsTo(0f, 1E-05f))
			{
				this._explainer.AddLine(description.ToString(), MathF.Round(value, 3) * 100f, ExplainedNumber.StatExplainer.OperationType.Multiply);
			}
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0005574C File Offset: 0x0005394C
		public void LimitMin(float minValue)
		{
			this._limitMinValue = new float?(minValue);
			if (this._explainer != null)
			{
				this._explainer.AddLine(ExplainedNumber.LimitMinText.ToString(), minValue, ExplainedNumber.StatExplainer.OperationType.LimitMin);
			}
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x00055779 File Offset: 0x00053979
		public void LimitMax(float maxValue, TextObject description = null)
		{
			this._limitMaxValue = new float?(maxValue);
			if (this._explainer != null)
			{
				this._explainer.AddLine((description ?? ExplainedNumber.LimitMaxText).ToString(), maxValue, ExplainedNumber.StatExplainer.OperationType.LimitMax);
			}
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x000557AB File Offset: 0x000539AB
		public void Clamp(float minValue, float maxValue)
		{
			this.LimitMin(minValue);
			this.LimitMax(maxValue, null);
		}

		// Token: 0x0400056F RID: 1391
		private static readonly TextObject LimitMinText = new TextObject("{=GNalaRaN}Minimum", null);

		// Token: 0x04000570 RID: 1392
		private static readonly TextObject LimitMaxText = new TextObject("{=cfjTtxWv}Maximum", null);

		// Token: 0x04000571 RID: 1393
		private static readonly TextObject BaseText = new TextObject("{=basevalue}Base", null);

		// Token: 0x04000573 RID: 1395
		private float? _limitMinValue;

		// Token: 0x04000574 RID: 1396
		private float? _limitMaxValue;

		// Token: 0x04000575 RID: 1397
		private ExplainedNumber.StatExplainer _explainer;

		// Token: 0x0200056E RID: 1390
		private class StatExplainer
		{
			// Token: 0x17000F5B RID: 3931
			// (get) Token: 0x0600500D RID: 20493 RVA: 0x0018E58E File Offset: 0x0018C78E
			// (set) Token: 0x0600500E RID: 20494 RVA: 0x0018E596 File Offset: 0x0018C796
			public List<ExplainedNumber.StatExplainer.ExplanationLine> Lines { get; private set; } = new List<ExplainedNumber.StatExplainer.ExplanationLine>();

			// Token: 0x17000F5C RID: 3932
			// (get) Token: 0x0600500F RID: 20495 RVA: 0x0018E59F File Offset: 0x0018C79F
			// (set) Token: 0x06005010 RID: 20496 RVA: 0x0018E5A7 File Offset: 0x0018C7A7
			public ExplainedNumber.StatExplainer.ExplanationLine? BaseLine { get; private set; }

			// Token: 0x17000F5D RID: 3933
			// (get) Token: 0x06005011 RID: 20497 RVA: 0x0018E5B0 File Offset: 0x0018C7B0
			// (set) Token: 0x06005012 RID: 20498 RVA: 0x0018E5B8 File Offset: 0x0018C7B8
			public ExplainedNumber.StatExplainer.ExplanationLine? LimitMinLine { get; private set; }

			// Token: 0x17000F5E RID: 3934
			// (get) Token: 0x06005013 RID: 20499 RVA: 0x0018E5C1 File Offset: 0x0018C7C1
			// (set) Token: 0x06005014 RID: 20500 RVA: 0x0018E5C9 File Offset: 0x0018C7C9
			public ExplainedNumber.StatExplainer.ExplanationLine? LimitMaxLine { get; private set; }

			// Token: 0x06005015 RID: 20501 RVA: 0x0018E5D4 File Offset: 0x0018C7D4
			[return: TupleElementNames(new string[] { "name", "number" })]
			public List<ValueTuple<string, float>> GetLines(float baseNumber, float unclampedResultNumber, TextObject overrideBaseLineText = null, TextObject overrideMaximumLineText = null, TextObject overrideMinimumLineText = null)
			{
				List<ValueTuple<string, float>> list = new List<ValueTuple<string, float>>();
				if (this.BaseLine != null)
				{
					list.Add(new ValueTuple<string, float>((overrideBaseLineText != null) ? overrideBaseLineText.ToString() : this.BaseLine.Value.Name, this.BaseLine.Value.Number));
				}
				foreach (ExplainedNumber.StatExplainer.ExplanationLine explanationLine in this.Lines)
				{
					float num = explanationLine.Number;
					if (explanationLine.OperationType == ExplainedNumber.StatExplainer.OperationType.Multiply)
					{
						num = baseNumber * num * 0.01f;
					}
					list.Add(new ValueTuple<string, float>(explanationLine.Name, num));
				}
				if (this.LimitMinLine != null && this.LimitMinLine.Value.Number > unclampedResultNumber)
				{
					list.Add(new ValueTuple<string, float>((overrideMinimumLineText != null) ? overrideMinimumLineText.ToString() : this.LimitMinLine.Value.Name, this.LimitMinLine.Value.Number - unclampedResultNumber));
				}
				if (this.LimitMaxLine != null && this.LimitMaxLine.Value.Number < unclampedResultNumber)
				{
					list.Add(new ValueTuple<string, float>((overrideMaximumLineText != null) ? overrideMaximumLineText.ToString() : this.LimitMaxLine.Value.Name, this.LimitMaxLine.Value.Number - unclampedResultNumber));
				}
				return list;
			}

			// Token: 0x06005016 RID: 20502 RVA: 0x0018E784 File Offset: 0x0018C984
			public void AddLine(string name, float number, ExplainedNumber.StatExplainer.OperationType opType)
			{
				ExplainedNumber.StatExplainer.ExplanationLine explanationLine = new ExplainedNumber.StatExplainer.ExplanationLine(name, number, opType);
				if (opType == ExplainedNumber.StatExplainer.OperationType.Add || opType == ExplainedNumber.StatExplainer.OperationType.Multiply)
				{
					int num = -1;
					for (int i = 0; i < this.Lines.Count; i++)
					{
						if (this.Lines[i].Name.Equals(name) && this.Lines[i].OperationType == opType)
						{
							num = i;
							break;
						}
					}
					if (num < 0)
					{
						this.Lines.Add(explanationLine);
						return;
					}
					explanationLine = new ExplainedNumber.StatExplainer.ExplanationLine(name, number + this.Lines[num].Number, opType);
					this.Lines[num] = explanationLine;
					return;
				}
				else
				{
					if (opType == ExplainedNumber.StatExplainer.OperationType.Base)
					{
						this.BaseLine = new ExplainedNumber.StatExplainer.ExplanationLine?(explanationLine);
						return;
					}
					if (opType == ExplainedNumber.StatExplainer.OperationType.LimitMin)
					{
						this.LimitMinLine = new ExplainedNumber.StatExplainer.ExplanationLine?(explanationLine);
						return;
					}
					if (opType == ExplainedNumber.StatExplainer.OperationType.LimitMax)
					{
						this.LimitMaxLine = new ExplainedNumber.StatExplainer.ExplanationLine?(explanationLine);
					}
					return;
				}
			}

			// Token: 0x020008FE RID: 2302
			public enum OperationType
			{
				// Token: 0x040026D6 RID: 9942
				Base,
				// Token: 0x040026D7 RID: 9943
				Add,
				// Token: 0x040026D8 RID: 9944
				Multiply,
				// Token: 0x040026D9 RID: 9945
				LimitMin,
				// Token: 0x040026DA RID: 9946
				LimitMax
			}

			// Token: 0x020008FF RID: 2303
			public readonly struct ExplanationLine
			{
				// Token: 0x06006CB7 RID: 27831 RVA: 0x001DC542 File Offset: 0x001DA742
				public ExplanationLine(string name, float number, ExplainedNumber.StatExplainer.OperationType operationType)
				{
					this.Name = name;
					this.Number = number;
					this.OperationType = operationType;
				}

				// Token: 0x040026DB RID: 9947
				public readonly float Number;

				// Token: 0x040026DC RID: 9948
				public readonly string Name;

				// Token: 0x040026DD RID: 9949
				public readonly ExplainedNumber.StatExplainer.OperationType OperationType;
			}
		}
	}
}
