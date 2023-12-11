using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

// Token: 0x02000004 RID: 4
public class DeviceManager
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x0600000B RID: 11 RVA: 0x00002214 File Offset: 0x00000414
	public static DeviceManager Instance
	{
		get
		{
			if (DeviceManager._instance == null)
			{
				DeviceManager._instance = new DeviceManager();
			}
			return DeviceManager._instance;
		}
	}

	// Token: 0x0600000C RID: 12
	[DllImport("user32.dll", SetLastError = true)]
	private static extern uint SendInput(uint nInputs, ref DeviceManager.Input pInputs, int cbSize);

	// Token: 0x0600000D RID: 13 RVA: 0x00002230 File Offset: 0x00000430
	public void MoveScreenPos(int tx, int ty)
	{
		Point position = Cursor.Position;
		int num = tx - position.X;
		int num2 = ty - position.Y;
		if ((double)Math.Abs(num) < 0.01 && (double)Math.Abs(num2) < 0.01)
		{
			return;
		}
		this.MoveMouse(num, num2);
		Thread.Sleep(1);
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002294 File Offset: 0x00000494
	private void MoveMouse(int dx, int dy)
	{
		DeviceManager.Input input = new DeviceManager.Input
		{
			type = 0U,
			u = new DeviceManager.InputUnion
			{
				mi = new DeviceManager.MOUSEINPUT
				{
					dx = dx,
					dy = dy,
					mouseData = 0U,
					dwFlags = 1U,
					time = 0U,
					dwExtraInfo = IntPtr.Zero
				}
			}
		};
		if (DeviceManager.SendInput(1U, ref input, Marshal.SizeOf(typeof(DeviceManager.Input))) == 0U)
		{
			throw new Exception("SendInput failed with code: " + Marshal.GetLastWin32Error().ToString());
		}
	}

	// Token: 0x04000018 RID: 24
	protected static DeviceManager _instance;

	// Token: 0x04000019 RID: 25
	private const uint InputMouse = 0U;

	// Token: 0x0400001A RID: 26
	private const uint MouseeventfMove = 1U;

	// Token: 0x02000268 RID: 616
	private struct Input
	{
		// Token: 0x04001DA3 RID: 7587
		public uint type;

		// Token: 0x04001DA4 RID: 7588
		public DeviceManager.InputUnion u;
	}

	// Token: 0x02000269 RID: 617
	[StructLayout(LayoutKind.Explicit)]
	private struct InputUnion
	{
		// Token: 0x04001DA5 RID: 7589
		[FieldOffset(0)]
		public DeviceManager.MOUSEINPUT mi;
	}

	// Token: 0x0200026A RID: 618
	private struct MOUSEINPUT
	{
		// Token: 0x04001DA6 RID: 7590
		public int dx;

		// Token: 0x04001DA7 RID: 7591
		public int dy;

		// Token: 0x04001DA8 RID: 7592
		public uint mouseData;

		// Token: 0x04001DA9 RID: 7593
		public uint dwFlags;

		// Token: 0x04001DAA RID: 7594
		public uint time;

		// Token: 0x04001DAB RID: 7595
		public IntPtr dwExtraInfo;
	}
}
