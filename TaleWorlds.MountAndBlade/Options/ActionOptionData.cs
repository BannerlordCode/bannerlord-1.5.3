using System;
using TaleWorlds.Engine.Options;

namespace TaleWorlds.MountAndBlade.Options
{
	// Token: 0x0200039F RID: 927
	public class ActionOptionData : IOptionData
	{
		// Token: 0x170009EF RID: 2543
		// (get) Token: 0x06003559 RID: 13657 RVA: 0x000DD141 File Offset: 0x000DB341
		// (set) Token: 0x0600355A RID: 13658 RVA: 0x000DD149 File Offset: 0x000DB349
		public Action OnAction { get; private set; }

		// Token: 0x0600355B RID: 13659 RVA: 0x000DD152 File Offset: 0x000DB352
		public ActionOptionData(ManagedOptions.ManagedOptionsType managedType, Action onAction)
		{
			this._managedType = managedType;
			this.OnAction = onAction;
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x000DD168 File Offset: 0x000DB368
		public ActionOptionData(NativeOptions.NativeOptionsType nativeType, Action onAction)
		{
			this._nativeType = nativeType;
			this.OnAction = onAction;
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x000DD17E File Offset: 0x000DB37E
		public ActionOptionData(string optionTypeId, Action onAction)
		{
			this._actionOptionTypeId = optionTypeId;
			this._nativeType = NativeOptions.NativeOptionsType.None;
			this.OnAction = onAction;
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x000DD19B File Offset: 0x000DB39B
		public void Commit()
		{
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x000DD19D File Offset: 0x000DB39D
		public float GetDefaultValue()
		{
			return 0f;
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x000DD1A4 File Offset: 0x000DB3A4
		public object GetOptionType()
		{
			if (this._nativeType != NativeOptions.NativeOptionsType.None)
			{
				return this._nativeType;
			}
			if (this._managedType != ManagedOptions.ManagedOptionsType.Language)
			{
				return this._managedType;
			}
			return this._actionOptionTypeId;
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x000DD1D5 File Offset: 0x000DB3D5
		public float GetValue(bool forceRefresh)
		{
			return 0f;
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x000DD1DC File Offset: 0x000DB3DC
		public bool IsNative()
		{
			return this._nativeType != NativeOptions.NativeOptionsType.None;
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x000DD1EA File Offset: 0x000DB3EA
		public void SetValue(float value)
		{
		}

		// Token: 0x06003564 RID: 13668 RVA: 0x000DD1EC File Offset: 0x000DB3EC
		public bool IsAction()
		{
			return this._nativeType == NativeOptions.NativeOptionsType.None && this._managedType == ManagedOptions.ManagedOptionsType.Language;
		}

		// Token: 0x06003565 RID: 13669 RVA: 0x000DD202 File Offset: 0x000DB402
		public ValueTuple<string, bool> GetIsDisabledAndReasonID()
		{
			return new ValueTuple<string, bool>(string.Empty, false);
		}

		// Token: 0x040016C8 RID: 5832
		private ManagedOptions.ManagedOptionsType _managedType;

		// Token: 0x040016C9 RID: 5833
		private NativeOptions.NativeOptionsType _nativeType;

		// Token: 0x040016CA RID: 5834
		private string _actionOptionTypeId;
	}
}
