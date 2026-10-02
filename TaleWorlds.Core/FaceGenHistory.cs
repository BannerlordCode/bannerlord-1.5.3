using System;
using System.Collections.Generic;

namespace TaleWorlds.Core
{
	// Token: 0x0200005D RID: 93
	public class FaceGenHistory
	{
		// Token: 0x06000732 RID: 1842 RVA: 0x00018ECC File Offset: 0x000170CC
		public FaceGenHistory(List<UndoRedoKey> undoCommands, List<UndoRedoKey> redoCommands, Dictionary<string, float> initialValues)
		{
			this.UndoCommands = undoCommands;
			this.RedoCommands = redoCommands;
			this.InitialValues = initialValues;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00018EE9 File Offset: 0x000170E9
		public void ClearHistory()
		{
			this.UndoCommands.Clear();
			this.RedoCommands.Clear();
			this.InitialValues.Clear();
		}

		// Token: 0x0400039F RID: 927
		public readonly List<UndoRedoKey> UndoCommands;

		// Token: 0x040003A0 RID: 928
		public readonly List<UndoRedoKey> RedoCommands;

		// Token: 0x040003A1 RID: 929
		public readonly Dictionary<string, float> InitialValues;
	}
}
