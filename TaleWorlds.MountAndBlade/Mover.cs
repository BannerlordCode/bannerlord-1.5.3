using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200033F RID: 831
	public class Mover : ScriptComponentBehavior
	{
		// Token: 0x06002EE0 RID: 12000 RVA: 0x000B598C File Offset: 0x000B3B8C
		protected internal override void OnEditorTick(float dt)
		{
			if (!base.GameEntity.EntityFlags.HasAnyFlag(EntityFlags.IsHelper))
			{
				if (this._moverGhost == null && this._pathname != "")
				{
					this.CreateOrUpdateMoverGhost();
				}
				if (this._tracker != null && this._tracker.IsValid)
				{
					if (this._moveGhost)
					{
						this._tracker.Advance(this._speed * dt);
						if (this._tracker.TotalDistanceTraveled >= this._tracker.GetPathLength())
						{
							this._tracker.Reset();
						}
					}
					else
					{
						this._tracker.Advance(0f);
					}
					MatrixFrame currentFrame = this._tracker.CurrentFrame;
					this._moverGhost.SetFrame(ref currentFrame, true);
				}
			}
		}

		// Token: 0x06002EE1 RID: 12001 RVA: 0x000B5A5C File Offset: 0x000B3C5C
		protected internal override void OnEditorVariableChanged(string variableName)
		{
			if (variableName == "_pathname")
			{
				this.CreateOrUpdateMoverGhost();
				return;
			}
			if (variableName == "_moveGhost")
			{
				if (!this._moveGhost)
				{
					this._moverGhost.SetVisibilityExcludeParents(false);
					return;
				}
				this._moverGhost.SetVisibilityExcludeParents(true);
			}
		}

		// Token: 0x06002EE2 RID: 12002 RVA: 0x000B5AAC File Offset: 0x000B3CAC
		private void CreateOrUpdateMoverGhost()
		{
			Path pathWithName = base.GameEntity.Scene.GetPathWithName(this._pathname);
			if (pathWithName != null)
			{
				this._tracker = new PathTracker(pathWithName, Vec3.One);
				this._tracker.Reset();
				base.GameEntity.SetLocalPosition(this._tracker.CurrentFrame.origin);
				if (this._moverGhost == null)
				{
					this._moverGhost = TaleWorlds.Engine.GameEntity.CopyFrom(base.GameEntity.Scene, base.GameEntity, true, true);
					this._moverGhost.EntityFlags |= EntityFlags.IsHelper | EntityFlags.DontSaveToScene | EntityFlags.DoNotTick;
					this._moverGhost.SetAlpha(0.2f);
					return;
				}
				this._moverGhost.SetLocalPosition(this._tracker.CurrentFrame.origin);
			}
		}

		// Token: 0x06002EE3 RID: 12003 RVA: 0x000B5B8C File Offset: 0x000B3D8C
		protected internal override void OnInit()
		{
			base.OnInit();
			Path pathWithName = base.GameEntity.Scene.GetPathWithName(this._pathname);
			if (pathWithName != null)
			{
				this._tracker = new PathTracker(pathWithName, Vec3.One);
				this._tracker.Reset();
				base.GameEntity.SetLocalPosition(this._tracker.CurrentFrame.origin);
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06002EE4 RID: 12004 RVA: 0x000B5C08 File Offset: 0x000B3E08
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (base.GameEntity.IsVisibleIncludeParents())
			{
				return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
			}
			return base.GetTickRequirement();
		}

		// Token: 0x06002EE5 RID: 12005 RVA: 0x000B5C34 File Offset: 0x000B3E34
		protected internal override void OnTick(float dt)
		{
			if (Mission.Current.Mode == MissionMode.Battle && this._tracker != null && this._tracker.IsValid && this._tracker.TotalDistanceTraveled < this._tracker.GetPathLength())
			{
				this._tracker.Advance(this._speed * dt);
				MatrixFrame currentFrame = this._tracker.CurrentFrame;
				base.GameEntity.SetFrame(ref currentFrame, true);
			}
		}

		// Token: 0x040012A4 RID: 4772
		[EditorVisibleScriptComponentVariable(true)]
		private string _pathname = "";

		// Token: 0x040012A5 RID: 4773
		[EditorVisibleScriptComponentVariable(true)]
		private float _speed;

		// Token: 0x040012A6 RID: 4774
		[EditorVisibleScriptComponentVariable(true)]
		private bool _moveGhost;

		// Token: 0x040012A7 RID: 4775
		private GameEntity _moverGhost;

		// Token: 0x040012A8 RID: 4776
		private PathTracker _tracker;
	}
}
