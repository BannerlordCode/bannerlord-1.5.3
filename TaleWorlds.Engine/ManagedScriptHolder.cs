using System;
using System.Collections.Generic;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200005D RID: 93
	public sealed class ManagedScriptHolder : DotNetObject
	{
		// Token: 0x0600092A RID: 2346 RVA: 0x00008229 File Offset: 0x00006429
		[EngineCallback(null, false)]
		internal static ManagedScriptHolder CreateManagedScriptHolder()
		{
			return new ManagedScriptHolder();
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00008230 File Offset: 0x00006430
		public ManagedScriptHolder()
		{
			this.TickComponentsParallelAuxMTPredicate = new TWParallel.ParallelForWithDtAuxPredicate(this.TickComponentsParallelAuxMT);
			this.TickComponentsParallel2AuxMTPredicate = new TWParallel.ParallelForWithDtAuxPredicate(this.TickComponentsParallel2AuxMT);
			this.TickComponentsParallel3AuxMTPredicate = new TWParallel.ParallelForWithDtAuxPredicate(this.TickComponentsParallel3AuxMT);
			this.TickComponentsOccasionallyParallelAuxMTPredicate = new TWParallel.ParallelForWithDtAuxPredicate(this.TickComponentsOccasionallyParallelAuxMT);
			this.TickComponentsFixedParallelAuxMTPredicate = new TWParallel.ParallelForWithDtAuxPredicate(this.TickComponentsFixedParallelAuxMT);
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x0000831F File Offset: 0x0000651F
		[EngineCallback(null, false)]
		public void SetScriptComponentHolder(ScriptComponentBehavior sc)
		{
			sc.SetOwnerManagedScriptHolder(this);
			this._toTickForEditor.AddToRec(sc);
			sc.SetScriptComponentToTick(sc.GetTickRequirement());
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00008340 File Offset: 0x00006540
		private ManagedScriptHolder.BehaviorTickRecord GetRecordFromEnum(ScriptComponentBehavior.TickRequirement tickRecEnum)
		{
			if (tickRecEnum <= ScriptComponentBehavior.TickRequirement.TickParallel2)
			{
				switch (tickRecEnum)
				{
				case ScriptComponentBehavior.TickRequirement.TickOccasionally:
					return this._toTickOccasionally;
				case ScriptComponentBehavior.TickRequirement.Tick:
					return this._toTick;
				case ScriptComponentBehavior.TickRequirement.TickOccasionally | ScriptComponentBehavior.TickRequirement.Tick:
					break;
				case ScriptComponentBehavior.TickRequirement.TickParallel:
					return this._toParallelTick;
				default:
					if (tickRecEnum == ScriptComponentBehavior.TickRequirement.TickParallel2)
					{
						return this._toParallelTick2;
					}
					break;
				}
			}
			else
			{
				if (tickRecEnum == ScriptComponentBehavior.TickRequirement.FixedTick)
				{
					return this._toFixedTick;
				}
				if (tickRecEnum == ScriptComponentBehavior.TickRequirement.FixedParallelTick)
				{
					return this._toFixedParallelTick;
				}
				if (tickRecEnum == ScriptComponentBehavior.TickRequirement.TickParallel3)
				{
					return this._toParallelTick3;
				}
			}
			return null;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x000083B4 File Offset: 0x000065B4
		public void UpdateTickRequirement(ScriptComponentBehavior sc, ScriptComponentBehavior.TickRequirement oldTickRequirement, ScriptComponentBehavior.TickRequirement newTickRequirement)
		{
			foreach (ScriptComponentBehavior.TickRequirement tickRequirement in ManagedScriptHolder.TickRequirementEnumValues)
			{
				if (newTickRequirement.HasAnyFlag(tickRequirement) != oldTickRequirement.HasAnyFlag(tickRequirement))
				{
					ManagedScriptHolder.BehaviorTickRecord recordFromEnum = this.GetRecordFromEnum(tickRequirement);
					if (oldTickRequirement.HasAnyFlag(tickRequirement))
					{
						recordFromEnum.RemoveFromRec(sc, true);
					}
					else
					{
						recordFromEnum.AddToRec(sc);
					}
				}
			}
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x0000840C File Offset: 0x0000660C
		[EngineCallback(null, false)]
		public void RemoveScriptComponentFromAllTickLists(ScriptComponentBehavior sc)
		{
			object addRemoveLockObject = this.AddRemoveLockObject;
			lock (addRemoveLockObject)
			{
				sc.SetScriptComponentToTickMT(ScriptComponentBehavior.TickRequirement.None);
				this._toTickForEditor.RemoveFromRec(sc, true);
			}
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0000845C File Offset: 0x0000665C
		[EngineCallback(null, false)]
		internal int GetNumberOfScripts()
		{
			return this._toTick.ScriptComponents.Count;
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00008470 File Offset: 0x00006670
		private void TickComponentsParallelAuxMT(int startInclusive, int endExclusive, float dt)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this._toParallelTick.ScriptComponents[i].OnTickParallel(dt);
			}
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x000084A0 File Offset: 0x000066A0
		private void TickComponentsParallel2AuxMT(int startInclusive, int endExclusive, float dt)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this._toParallelTick2.ScriptComponents[i].OnTickParallel2(dt);
			}
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x000084D0 File Offset: 0x000066D0
		private void TickComponentsParallel3AuxMT(int startInclusive, int endExclusive, float dt)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this._toParallelTick3.ScriptComponents[i].OnTickParallel3(dt);
			}
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x00008500 File Offset: 0x00006700
		private void TickComponentsOccasionallyParallelAuxMT(int startInclusive, int endExclusive, float dt)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this._toTickOccasionally.ScriptComponents[i].OnTickOccasionally(dt);
			}
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00008530 File Offset: 0x00006730
		private void TickComponentsFixedParallelAuxMT(int startInclusive, int endExclusive, float dt)
		{
			for (int i = startInclusive; i < endExclusive; i++)
			{
				this._toFixedParallelTick.ScriptComponents[i].OnParallelFixedTick(dt);
			}
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00008560 File Offset: 0x00006760
		[EngineCallback(null, false)]
		internal void FixedTickComponents(float fixedDt)
		{
			this._toFixedParallelTick.TickRec();
			TWParallel.For(0, this._toFixedParallelTick.ScriptComponents.Count, fixedDt, this.TickComponentsFixedParallelAuxMTPredicate, 1);
			this._toFixedTick.TickRec();
			foreach (ScriptComponentBehavior scriptComponentBehavior in this._toFixedTick.ScriptComponents)
			{
				scriptComponentBehavior.OnFixedTick(fixedDt);
			}
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x000085EC File Offset: 0x000067EC
		[EngineCallback(null, false)]
		internal void TickComponents(float dt)
		{
			this._toParallelTick.TickRec();
			TWParallel.For(0, this._toParallelTick.ScriptComponents.Count, dt, this.TickComponentsParallelAuxMTPredicate, 1);
			this._toParallelTick2.TickRec();
			TWParallel.For(0, this._toParallelTick2.ScriptComponents.Count, dt, this.TickComponentsParallel2AuxMTPredicate, 8);
			this._toParallelTick3.TickRec();
			TWParallel.For(0, this._toParallelTick3.ScriptComponents.Count, dt, this.TickComponentsParallel3AuxMTPredicate, 8);
			this._toTick.TickRec();
			foreach (ScriptComponentBehavior scriptComponentBehavior in this._toTick.ScriptComponents)
			{
				scriptComponentBehavior.OnTick(dt);
			}
			this._nextIndexToTickOccasionally = MathF.Max(0, this._nextIndexToTickOccasionally - this._toTickOccasionally.GetWillBeRemovedCount());
			this._toTickOccasionally.TickRec();
			int num = this._toTickOccasionally.ScriptComponents.Count / 10 + 1;
			int num2 = Math.Min(this._nextIndexToTickOccasionally + num, this._toTickOccasionally.ScriptComponents.Count);
			if (this._nextIndexToTickOccasionally < num2)
			{
				TWParallel.For(this._nextIndexToTickOccasionally, num2, dt, this.TickComponentsOccasionallyParallelAuxMTPredicate, 8);
				this._nextIndexToTickOccasionally = ((num2 >= this._toTickOccasionally.ScriptComponents.Count) ? 0 : num2);
				return;
			}
			this._nextIndexToTickOccasionally = 0;
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x00008768 File Offset: 0x00006968
		[EngineCallback(null, false)]
		internal void TickComponentsEditor(float dt)
		{
			this._toTickForEditor.TickRec();
			for (int i = 0; i < this._toTickForEditor.ScriptComponents.Count; i++)
			{
				this._toTickForEditor.ScriptComponents[i].OnEditorTick(dt);
			}
		}

		// Token: 0x040000D9 RID: 217
		private static readonly ScriptComponentBehavior.TickRequirement[] TickRequirementEnumValues = (ScriptComponentBehavior.TickRequirement[])Enum.GetValues(typeof(ScriptComponentBehavior.TickRequirement));

		// Token: 0x040000DA RID: 218
		public object AddRemoveLockObject = new object();

		// Token: 0x040000DB RID: 219
		private readonly ManagedScriptHolder.BehaviorTickRecord _toTick = new ManagedScriptHolder.BehaviorTickRecord(512);

		// Token: 0x040000DC RID: 220
		private readonly ManagedScriptHolder.BehaviorTickRecord _toParallelTick = new ManagedScriptHolder.BehaviorTickRecord(64);

		// Token: 0x040000DD RID: 221
		private readonly ManagedScriptHolder.BehaviorTickRecord _toParallelTick2 = new ManagedScriptHolder.BehaviorTickRecord(512);

		// Token: 0x040000DE RID: 222
		private readonly ManagedScriptHolder.BehaviorTickRecord _toParallelTick3 = new ManagedScriptHolder.BehaviorTickRecord(512);

		// Token: 0x040000DF RID: 223
		private readonly ManagedScriptHolder.BehaviorTickRecord _toTickOccasionally = new ManagedScriptHolder.BehaviorTickRecord(512);

		// Token: 0x040000E0 RID: 224
		private readonly ManagedScriptHolder.BehaviorTickRecord _toTickForEditor = new ManagedScriptHolder.BehaviorTickRecord(512);

		// Token: 0x040000E1 RID: 225
		private readonly ManagedScriptHolder.BehaviorTickRecord _toFixedParallelTick = new ManagedScriptHolder.BehaviorTickRecord(64);

		// Token: 0x040000E2 RID: 226
		private readonly ManagedScriptHolder.BehaviorTickRecord _toFixedTick = new ManagedScriptHolder.BehaviorTickRecord(32);

		// Token: 0x040000E3 RID: 227
		private int _nextIndexToTickOccasionally;

		// Token: 0x040000E4 RID: 228
		private readonly TWParallel.ParallelForWithDtAuxPredicate TickComponentsParallelAuxMTPredicate;

		// Token: 0x040000E5 RID: 229
		private readonly TWParallel.ParallelForWithDtAuxPredicate TickComponentsParallel2AuxMTPredicate;

		// Token: 0x040000E6 RID: 230
		private readonly TWParallel.ParallelForWithDtAuxPredicate TickComponentsParallel3AuxMTPredicate;

		// Token: 0x040000E7 RID: 231
		private readonly TWParallel.ParallelForWithDtAuxPredicate TickComponentsFixedParallelAuxMTPredicate;

		// Token: 0x040000E8 RID: 232
		private readonly TWParallel.ParallelForWithDtAuxPredicate TickComponentsOccasionallyParallelAuxMTPredicate;

		// Token: 0x020000C5 RID: 197
		private class BehaviorTickRecord
		{
			// Token: 0x170000D2 RID: 210
			// (get) Token: 0x0600101E RID: 4126 RVA: 0x00014A23 File Offset: 0x00012C23
			public List<ScriptComponentBehavior> ScriptComponents
			{
				get
				{
					return this._scriptComponents;
				}
			}

			// Token: 0x0600101F RID: 4127 RVA: 0x00014A2B File Offset: 0x00012C2B
			public BehaviorTickRecord(int initialCapacity)
			{
				this._scriptComponents = new List<ScriptComponentBehavior>(initialCapacity);
				this._addTo = new List<ScriptComponentBehavior>();
				this._removeFrom = new List<ScriptComponentBehavior>();
			}

			// Token: 0x06001020 RID: 4128 RVA: 0x00014A58 File Offset: 0x00012C58
			internal void AddToRec(ScriptComponentBehavior sc)
			{
				int num = this._removeFrom.IndexOf(sc);
				if (num != -1)
				{
					this._removeFrom.RemoveAt(num);
					return;
				}
				this._addTo.Add(sc);
			}

			// Token: 0x06001021 RID: 4129 RVA: 0x00014A90 File Offset: 0x00012C90
			internal void RemoveFromRec(ScriptComponentBehavior sc, bool checkForDoubleRemove = true)
			{
				int num = this._addTo.IndexOf(sc);
				if (num != -1)
				{
					this._addTo.RemoveAt(num);
					return;
				}
				if (this._removeFrom.IndexOf(sc) == -1 && (!checkForDoubleRemove || this.ScriptComponents.IndexOf(sc) != -1))
				{
					this._removeFrom.Add(sc);
				}
			}

			// Token: 0x06001022 RID: 4130 RVA: 0x00014AE8 File Offset: 0x00012CE8
			internal void TickRec()
			{
				foreach (ScriptComponentBehavior scriptComponentBehavior in this._removeFrom)
				{
					this.ScriptComponents.Remove(scriptComponentBehavior);
				}
				this._removeFrom.Clear();
				foreach (ScriptComponentBehavior scriptComponentBehavior2 in this._addTo)
				{
					this.ScriptComponents.Add(scriptComponentBehavior2);
				}
				this._addTo.Clear();
			}

			// Token: 0x06001023 RID: 4131 RVA: 0x00014BA0 File Offset: 0x00012DA0
			internal bool ContainsOrToBeAdded(ScriptComponentBehavior sc)
			{
				return (this.ScriptComponents.Contains(sc) || this._addTo.Contains(sc)) && !this._removeFrom.Contains(sc);
			}

			// Token: 0x06001024 RID: 4132 RVA: 0x00014BCF File Offset: 0x00012DCF
			internal int GetWillBeRemovedCount()
			{
				return this._removeFrom.Count;
			}

			// Token: 0x040003EE RID: 1006
			private readonly List<ScriptComponentBehavior> _scriptComponents;

			// Token: 0x040003EF RID: 1007
			private readonly List<ScriptComponentBehavior> _addTo;

			// Token: 0x040003F0 RID: 1008
			private readonly List<ScriptComponentBehavior> _removeFrom;
		}
	}
}
