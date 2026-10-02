using System;
using System.Collections.Generic;

namespace TaleWorlds.GauntletUI
{
	// Token: 0x02000036 RID: 54
	public class VisualDefinition
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0000FB5E File Offset: 0x0000DD5E
		// (set) Token: 0x060003B2 RID: 946 RVA: 0x0000FB66 File Offset: 0x0000DD66
		public string Name { get; private set; }

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x0000FB6F File Offset: 0x0000DD6F
		// (set) Token: 0x060003B4 RID: 948 RVA: 0x0000FB77 File Offset: 0x0000DD77
		public float TransitionDuration { get; private set; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060003B5 RID: 949 RVA: 0x0000FB80 File Offset: 0x0000DD80
		// (set) Token: 0x060003B6 RID: 950 RVA: 0x0000FB88 File Offset: 0x0000DD88
		public float DelayOnBegin { get; private set; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x0000FB91 File Offset: 0x0000DD91
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x0000FB99 File Offset: 0x0000DD99
		public AnimationInterpolation.Type EaseType { get; private set; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x0000FBA2 File Offset: 0x0000DDA2
		// (set) Token: 0x060003BA RID: 954 RVA: 0x0000FBAA File Offset: 0x0000DDAA
		public AnimationInterpolation.Function EaseFunction { get; private set; }

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0000FBB3 File Offset: 0x0000DDB3
		// (set) Token: 0x060003BC RID: 956 RVA: 0x0000FBBB File Offset: 0x0000DDBB
		public Dictionary<string, VisualState> VisualStates { get; private set; }

		// Token: 0x060003BD RID: 957 RVA: 0x0000FBC4 File Offset: 0x0000DDC4
		public VisualDefinition(string name, float transitionDuration, float delayOnBegin, AnimationInterpolation.Type easeType, AnimationInterpolation.Function easeFunction)
		{
			this.Name = name;
			this.TransitionDuration = transitionDuration;
			this.DelayOnBegin = delayOnBegin;
			this.EaseType = easeType;
			this.EaseFunction = easeFunction;
			this.VisualStates = new Dictionary<string, VisualState>();
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0000FBFC File Offset: 0x0000DDFC
		public void AddVisualState(VisualState visualState)
		{
			this.VisualStates.Add(visualState.State, visualState);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x0000FC10 File Offset: 0x0000DE10
		public VisualState GetVisualState(string state)
		{
			if (this.VisualStates.ContainsKey(state))
			{
				return this.VisualStates[state];
			}
			return null;
		}
	}
}
