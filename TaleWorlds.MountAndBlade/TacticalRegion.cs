using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000370 RID: 880
	public class TacticalRegion : MissionObject
	{
		// Token: 0x17000980 RID: 2432
		// (get) Token: 0x060032BC RID: 12988 RVA: 0x000CFAC0 File Offset: 0x000CDCC0
		// (set) Token: 0x060032BD RID: 12989 RVA: 0x000CFAC8 File Offset: 0x000CDCC8
		public WorldPosition Position
		{
			get
			{
				return this._position;
			}
			set
			{
				this._position = value;
			}
		}

		// Token: 0x17000981 RID: 2433
		// (get) Token: 0x060032BE RID: 12990 RVA: 0x000CFAD1 File Offset: 0x000CDCD1
		// (set) Token: 0x060032BF RID: 12991 RVA: 0x000CFAD9 File Offset: 0x000CDCD9
		public List<TacticalPosition> LinkedTacticalPositions
		{
			get
			{
				return this._linkedTacticalPositions;
			}
			set
			{
				this._linkedTacticalPositions = value;
			}
		}

		// Token: 0x060032C0 RID: 12992 RVA: 0x000CFAE4 File Offset: 0x000CDCE4
		protected internal override void OnInit()
		{
			base.OnInit();
			this._position = new WorldPosition(base.GameEntity.GetScenePointer(), base.GameEntity.GlobalPosition);
		}

		// Token: 0x060032C1 RID: 12993 RVA: 0x000CFB20 File Offset: 0x000CDD20
		public override void AfterMissionStart()
		{
			base.AfterMissionStart();
			this._linkedTacticalPositions = new List<TacticalPosition>();
			this._linkedTacticalPositions = (from c in base.GameEntity.GetChildren()
				where c.HasScriptOfType<TacticalPosition>()
				select c.GetFirstScriptOfType<TacticalPosition>()).ToList<TacticalPosition>();
		}

		// Token: 0x060032C2 RID: 12994 RVA: 0x000CFBA0 File Offset: 0x000CDDA0
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this._position = new WorldPosition(base.GameEntity.GetScenePointer(), UIntPtr.Zero, base.GameEntity.GlobalPosition, false);
		}

		// Token: 0x060032C3 RID: 12995 RVA: 0x000CFBE0 File Offset: 0x000CDDE0
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this._position.SetVec3(UIntPtr.Zero, base.GameEntity.GlobalPosition, false);
			if (this.IsEditorDebugRingVisible)
			{
				MBEditor.HelpersEnabled();
			}
		}

		// Token: 0x0400158C RID: 5516
		public bool IsEditorDebugRingVisible;

		// Token: 0x0400158D RID: 5517
		private WorldPosition _position;

		// Token: 0x0400158E RID: 5518
		public float radius = 1f;

		// Token: 0x0400158F RID: 5519
		private List<TacticalPosition> _linkedTacticalPositions;

		// Token: 0x04001590 RID: 5520
		public TacticalRegion.TacticalRegionTypeEnum tacticalRegionType;

		// Token: 0x02000651 RID: 1617
		public enum TacticalRegionTypeEnum
		{
			// Token: 0x040021B5 RID: 8629
			Forest,
			// Token: 0x040021B6 RID: 8630
			DifficultTerrain,
			// Token: 0x040021B7 RID: 8631
			Opening
		}
	}
}
