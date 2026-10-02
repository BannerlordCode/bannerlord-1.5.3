using System;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.Nameplate
{
	// Token: 0x02000018 RID: 24
	public class NameplateVM : ViewModel
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000A254 File Offset: 0x00008454
		// (set) Token: 0x06000245 RID: 581 RVA: 0x0000A25C File Offset: 0x0000845C
		public double Scale { get; set; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000A265 File Offset: 0x00008465
		// (set) Token: 0x06000247 RID: 583 RVA: 0x0000A26D File Offset: 0x0000846D
		public int NameplateOrder { get; set; }

		// Token: 0x06000249 RID: 585 RVA: 0x0000A27E File Offset: 0x0000847E
		protected void OnTutorialNotificationElementChanged(TutorialNotificationElementChangeEvent obj)
		{
			this.RefreshTutorialStatus(((obj != null) ? obj.NewNotificationElementID : null) ?? string.Empty);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000A29B File Offset: 0x0000849B
		public virtual void RefreshDynamicProperties(bool forceUpdate)
		{
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000A29D File Offset: 0x0000849D
		public virtual void RefreshPosition()
		{
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000A29F File Offset: 0x0000849F
		public virtual void RefreshRelationStatus()
		{
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000A2A1 File Offset: 0x000084A1
		public virtual void RefreshTutorialStatus(string newTutorialHighlightElementID)
		{
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000A2A3 File Offset: 0x000084A3
		// (set) Token: 0x0600024F RID: 591 RVA: 0x0000A2AB File Offset: 0x000084AB
		public string FactionColor
		{
			get
			{
				return this._factionColor;
			}
			set
			{
				if (value != this._factionColor)
				{
					this._factionColor = value;
					base.OnPropertyChangedWithValue<string>(value, "FactionColor");
				}
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000A2CE File Offset: 0x000084CE
		// (set) Token: 0x06000251 RID: 593 RVA: 0x0000A2D6 File Offset: 0x000084D6
		public float DistanceToCamera
		{
			get
			{
				return this._distanceToCamera;
			}
			set
			{
				if (value != this._distanceToCamera)
				{
					this._distanceToCamera = value;
					base.OnPropertyChangedWithValue(value, "DistanceToCamera");
				}
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000A2F4 File Offset: 0x000084F4
		// (set) Token: 0x06000253 RID: 595 RVA: 0x0000A2FC File Offset: 0x000084FC
		public bool IsVisibleOnMap
		{
			get
			{
				return this._isVisibleOnMap;
			}
			set
			{
				if (value != this._isVisibleOnMap)
				{
					this._isVisibleOnMap = value;
					base.OnPropertyChangedWithValue(value, "IsVisibleOnMap");
				}
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000A31A File Offset: 0x0000851A
		// (set) Token: 0x06000255 RID: 597 RVA: 0x0000A322 File Offset: 0x00008522
		public bool IsTargetedByTutorial
		{
			get
			{
				return this._isTargetedByTutorial;
			}
			set
			{
				if (value != this._isTargetedByTutorial)
				{
					this._isTargetedByTutorial = value;
					base.OnPropertyChangedWithValue(value, "IsTargetedByTutorial");
					base.OnPropertyChanged("ShouldShowFullName");
					base.OnPropertyChanged("IsTracked");
				}
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000A356 File Offset: 0x00008556
		// (set) Token: 0x06000257 RID: 599 RVA: 0x0000A35E File Offset: 0x0000855E
		public Vec2 Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position != value)
				{
					this._position = value;
					base.OnPropertyChangedWithValue(value, "Position");
				}
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000A381 File Offset: 0x00008581
		// (set) Token: 0x06000259 RID: 601 RVA: 0x0000A389 File Offset: 0x00008589
		public bool CanParley
		{
			get
			{
				return this._canParley;
			}
			set
			{
				if (value != this._canParley)
				{
					this._canParley = value;
					base.OnPropertyChangedWithValue(value, "CanParley");
				}
			}
		}

		// Token: 0x04000109 RID: 265
		protected bool _bindIsTargetedByTutorial;

		// Token: 0x0400010A RID: 266
		private Vec2 _position;

		// Token: 0x0400010B RID: 267
		private bool _isVisibleOnMap;

		// Token: 0x0400010C RID: 268
		private string _factionColor;

		// Token: 0x0400010D RID: 269
		private bool _isTargetedByTutorial;

		// Token: 0x0400010E RID: 270
		private float _distanceToCamera;

		// Token: 0x0400010F RID: 271
		private bool _canParley;

		// Token: 0x02000084 RID: 132
		protected enum NameplateSize
		{
			// Token: 0x040003AE RID: 942
			Small,
			// Token: 0x040003AF RID: 943
			Normal,
			// Token: 0x040003B0 RID: 944
			Big
		}
	}
}
