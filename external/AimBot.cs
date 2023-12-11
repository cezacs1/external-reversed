using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SharpDX;

// Token: 0x02000002 RID: 2
public class AimBot
{
	// Token: 0x06000004 RID: 4 RVA: 0x00002064 File Offset: 0x00000264
	public static bool IsKeyPressed(Keys keys)
	{
		return NativeImport.GetAsyncKeyState((int)keys) > 0;
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00002070 File Offset: 0x00000270
	public void Run()
	{
		if (!this.Active)
		{
			return;
		}
		List<Entity> list = (from entity in EntityManager.Instance.GetDrawEnemies()
		where entity.HeadBoneScreenPosition.X > 0f && entity.HeadBoneScreenPosition.X < 1920f && entity.HeadBoneScreenPosition.Y > 0f && entity.HeadBoneScreenPosition.Y < 1080f
		select entity).ToList<Entity>();
		Point position = Cursor.Position;
		Vector2 cursorPosition = new Vector2((float)position.X, (float)position.Y);
		list.Sort((Entity a, Entity b) => Vector2.Distance(a.HeadBoneScreenPosition, cursorPosition).CompareTo(Vector2.Distance(b.HeadBoneScreenPosition, cursorPosition)));
		if (list.Count < 1)
		{
			return;
		}
		this._nextTick = DateTime.Now.Ticks + 10000000L;
		this._lastAimEnemy = list[0];
		if (AimBot.IsKeyPressed(Keys.LButton))
		{
			Vector2 headBoneScreenPosition = this._lastAimEnemy.HeadBoneScreenPosition;
			if (Vector2.Distance(headBoneScreenPosition, cursorPosition) > 50f)
			{
				return;
			}
			int tx = (int)headBoneScreenPosition.X;
			int ty = (int)headBoneScreenPosition.Y;
			DeviceManager.Instance.MoveScreenPos(tx, ty);
		}
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002184 File Offset: 0x00000384
	private static void MouseEvent(uint mouseEvent, uint x, uint y)
	{
		NativeImport.mouse_event(mouseEvent, x, y, 0U, 0U);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002190 File Offset: 0x00000390
	internal static void MouseLeftDown()
	{
		AimBot.MouseEvent(2U, (uint)Cursor.Position.X, (uint)Cursor.Position.Y);
	}

	// Token: 0x06000008 RID: 8 RVA: 0x000021C4 File Offset: 0x000003C4
	internal static void MouseLeftUp()
	{
		AimBot.MouseEvent(4U, (uint)Cursor.Position.X, (uint)Cursor.Position.Y);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000021F8 File Offset: 0x000003F8
	internal static void MouseClickLeft()
	{
		AimBot.MouseLeftDown();
		AimBot.MouseLeftUp();
	}

	// Token: 0x04000001 RID: 1
	public static AimBot Instance;

	// Token: 0x04000002 RID: 2
	private Entity _lastAimEnemy;

	// Token: 0x04000003 RID: 3
	private long _nextTick;

	// Token: 0x04000004 RID: 4
	public bool Active = true;

	// Token: 0x04000005 RID: 5
	private const uint MOUSEEVENTF_LEFTDOWN = 2U;

	// Token: 0x04000006 RID: 6
	private const uint MOUSEEVENTF_LEFTUP = 4U;
}
