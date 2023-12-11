using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

// Token: 0x0200001F RID: 31
public class VAMemory
{
	// Token: 0x060000AC RID: 172
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint dwSize, out IntPtr lpNumberOfBytesRead);

	// Token: 0x060000AD RID: 173
	[DllImport("kernel32.dll")]
	private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, uint lpNumberOfBytesWritten);

	// Token: 0x060000AE RID: 174
	[DllImport("kernel32.dll")]
	private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

	// Token: 0x060000AF RID: 175
	[DllImport("kernel32.dll")]
	private static extern bool CloseHandle(IntPtr hObject);

	// Token: 0x060000B0 RID: 176
	[DllImport("kernel32.dll")]
	private static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

	// Token: 0x060000B1 RID: 177
	[DllImport("kernel32.dll", SetLastError = true)]
	private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004740 File Offset: 0x00002940
	// (set) Token: 0x060000B3 RID: 179 RVA: 0x00004748 File Offset: 0x00002948
	public string processName { get; set; }

	// Token: 0x060000B4 RID: 180 RVA: 0x00004754 File Offset: 0x00002954
	public VAMemory()
	{
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00004794 File Offset: 0x00002994
	public VAMemory(string pProcessName)
	{
		this.processName = pProcessName;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x000047EC File Offset: 0x000029EC
	public bool CheckProcess()
	{
		if (this.processName == null)
		{
			MessageBox.Show("Programmer, define process name first!");
			return false;
		}
		this.mainProcess = Process.GetProcessesByName(this.processName);
		if (this.mainProcess.Length == 0)
		{
			this.ErrorProcessNotFound(this.processName);
			return false;
		}
		this.processHandle = VAMemory.OpenProcess(2035711U, false, this.mainProcess[0].Id);
		if (!(this.processHandle == IntPtr.Zero))
		{
			return true;
		}
		this.ErrorProcessNotFound(this.processName);
		return false;
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00004888 File Offset: 0x00002A88
	public byte[] ReadByteArray(IntPtr pOffset, uint pSize)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		byte[] result;
		try
		{
			byte[] array = new byte[pSize];
			IntPtr zero = IntPtr.Zero;
			VAMemory.ReadProcessMemory(this.processHandle, pOffset, array, pSize, out zero);
			result = array;
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadByteArray" + ex.ToString());
			}
			result = new byte[1];
		}
		return result;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00004914 File Offset: 0x00002B14
	public string ReadStringUnicode(IntPtr pOffset, uint pSize)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		string result;
		try
		{
			result = Encoding.Unicode.GetString(this.ReadByteArray(pOffset, pSize), 0, (int)pSize);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadStringUnicode" + ex.ToString());
			}
			result = "";
		}
		return result;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00004994 File Offset: 0x00002B94
	public string ReadStringASCII(IntPtr pOffset, uint pSize)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		string result;
		try
		{
			result = Encoding.ASCII.GetString(this.ReadByteArray(pOffset, pSize), 0, (int)pSize);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadStringASCII" + ex.ToString());
			}
			result = "";
		}
		return result;
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00004A14 File Offset: 0x00002C14
	public char ReadChar(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		char result;
		try
		{
			result = BitConverter.ToChar(this.ReadByteArray(pOffset, 1U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadChar" + ex.ToString());
			}
			result = ' ';
		}
		return result;
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00004A8C File Offset: 0x00002C8C
	public bool ReadBoolean(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = BitConverter.ToBoolean(this.ReadByteArray(pOffset, 1U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadByte" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00004B04 File Offset: 0x00002D04
	public byte ReadByte(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		byte result;
		try
		{
			result = this.ReadByteArray(pOffset, 1U)[0];
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadByte" + ex.ToString());
			}
			result = 0;
		}
		return result;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00004B78 File Offset: 0x00002D78
	public short ReadInt16(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		short result;
		try
		{
			result = BitConverter.ToInt16(this.ReadByteArray(pOffset, 2U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadInt16" + ex.ToString());
			}
			result = 0;
		}
		return result;
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00004BF0 File Offset: 0x00002DF0
	public short ReadShort(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		short result;
		try
		{
			result = BitConverter.ToInt16(this.ReadByteArray(pOffset, 2U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadInt16" + ex.ToString());
			}
			result = 0;
		}
		return result;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00004C68 File Offset: 0x00002E68
	public int ReadInt32(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		int result;
		try
		{
			result = BitConverter.ToInt32(this.ReadByteArray(pOffset, 4U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadInt32" + ex.ToString());
			}
			result = 0;
		}
		return result;
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00004CE0 File Offset: 0x00002EE0
	public IntPtr ReadIntPtr(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		IntPtr result;
		try
		{
			result = (IntPtr)BitConverter.ToInt32(this.ReadByteArray(pOffset, 4U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadIntPtr" + ex.ToString());
			}
			result = IntPtr.Zero;
		}
		return result;
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00004D60 File Offset: 0x00002F60
	public int ReadInteger(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		int result;
		try
		{
			result = BitConverter.ToInt32(this.ReadByteArray(pOffset, 4U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadInteger" + ex.ToString());
			}
			result = 0;
		}
		return result;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00004DD8 File Offset: 0x00002FD8
	public long ReadInt64(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		long result;
		try
		{
			result = BitConverter.ToInt64(this.ReadByteArray(pOffset, 8U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadInt64" + ex.ToString());
			}
			result = 0L;
		}
		return result;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x00004E50 File Offset: 0x00003050
	public long ReadLong(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		long result;
		try
		{
			result = BitConverter.ToInt64(this.ReadByteArray(pOffset, 8U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadLong" + ex.ToString());
			}
			result = 0L;
		}
		return result;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x00004EC8 File Offset: 0x000030C8
	public ushort ReadUInt16(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		ushort result;
		try
		{
			result = BitConverter.ToUInt16(this.ReadByteArray(pOffset, 2U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadUInt16" + ex.ToString());
			}
			result = 0;
		}
		return result;
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00004F40 File Offset: 0x00003140
	public ushort ReadUShort(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		ushort result;
		try
		{
			result = BitConverter.ToUInt16(this.ReadByteArray(pOffset, 2U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadUShort" + ex.ToString());
			}
			result = 0;
		}
		return result;
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x00004FB8 File Offset: 0x000031B8
	public uint ReadUInt32(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		uint result;
		try
		{
			result = BitConverter.ToUInt32(this.ReadByteArray(pOffset, 4U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadUInt32" + ex.ToString());
			}
			result = 0U;
		}
		return result;
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00005030 File Offset: 0x00003230
	public uint ReadUInteger(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		uint result;
		try
		{
			result = BitConverter.ToUInt32(this.ReadByteArray(pOffset, 4U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadUInteger" + ex.ToString());
			}
			result = 0U;
		}
		return result;
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x000050A8 File Offset: 0x000032A8
	public ulong ReadUInt64(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		ulong result;
		try
		{
			result = BitConverter.ToUInt64(this.ReadByteArray(pOffset, 8U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadUInt64" + ex.ToString());
			}
			result = 0UL;
		}
		return result;
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00005120 File Offset: 0x00003320
	public long ReadULong(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		long result;
		try
		{
			result = (long)BitConverter.ToUInt64(this.ReadByteArray(pOffset, 8U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadULong" + ex.ToString());
			}
			result = 0L;
		}
		return result;
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00005198 File Offset: 0x00003398
	public float ReadFloat(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		float result;
		try
		{
			result = BitConverter.ToSingle(this.ReadByteArray(pOffset, 4U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadFloat" + ex.ToString());
			}
			result = 0f;
		}
		return result;
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00005214 File Offset: 0x00003414
	public double ReadDouble(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		double result;
		try
		{
			result = BitConverter.ToDouble(this.ReadByteArray(pOffset, 8U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadDouble" + ex.ToString());
			}
			result = 0.0;
		}
		return result;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00005294 File Offset: 0x00003494
	public Vector2 ReadVector2(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		Vector2 result;
		try
		{
			result = new Vector2(BitConverter.ToSingle(this.ReadByteArray(pOffset, 4U), 0), BitConverter.ToSingle(this.ReadByteArray(pOffset + 4, 4U), 0));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadVector2" + ex.ToString());
			}
			result = new Vector2(0f, 0f);
		}
		return result;
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00005334 File Offset: 0x00003534
	public Vector3 ReadVector3(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		Vector3 result;
		try
		{
			result = new Vector3(BitConverter.ToSingle(this.ReadByteArray(pOffset, 4U), 0), BitConverter.ToSingle(this.ReadByteArray(pOffset + 4, 4U), 0), BitConverter.ToSingle(this.ReadByteArray(pOffset + 8, 4U), 0));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadVector3" + ex.ToString());
			}
			result = new Vector3(0f, 0f, 0f);
		}
		return result;
	}

	// Token: 0x060000CE RID: 206 RVA: 0x000053EC File Offset: 0x000035EC
	public byte[] ReadProtectedByteArray(IntPtr pOffset, uint pSize)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		byte[] result;
		try
		{
			uint flNewProtect;
			VAMemory.VirtualProtectEx(this.processHandle, pOffset, (UIntPtr)pSize, 64U, out flNewProtect);
			byte[] array = new byte[pSize];
			IntPtr zero = IntPtr.Zero;
			VAMemory.ReadProcessMemory(this.processHandle, pOffset, array, pSize, out zero);
			VAMemory.VirtualProtectEx(this.processHandle, pOffset, (UIntPtr)pSize, flNewProtect, out flNewProtect);
			result = array;
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadByteArray" + ex.ToString());
			}
			result = new byte[1];
		}
		return result;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x000054A8 File Offset: 0x000036A8
	public string ReadProtectedStringUnicode(IntPtr pOffset, uint pSize)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		string result;
		try
		{
			result = Encoding.Unicode.GetString(this.ReadProtectedByteArray(pOffset, pSize), 0, (int)pSize);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadStringUnicode" + ex.ToString());
			}
			result = "";
		}
		return result;
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00005528 File Offset: 0x00003728
	public string ReadProtectedStringASCII(IntPtr pOffset, uint pSize)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		string result;
		try
		{
			result = Encoding.ASCII.GetString(this.ReadProtectedByteArray(pOffset, pSize), 0, (int)pSize);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadStringASCII" + ex.ToString());
			}
			result = "";
		}
		return result;
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x000055A8 File Offset: 0x000037A8
	public char ReadProtectedChar(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		char result;
		try
		{
			result = BitConverter.ToChar(this.ReadProtectedByteArray(pOffset, 1U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadChar" + ex.ToString());
			}
			result = ' ';
		}
		return result;
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00005620 File Offset: 0x00003820
	public bool ReadProtectedBoolean(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = BitConverter.ToBoolean(this.ReadProtectedByteArray(pOffset, 1U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadByte" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00005698 File Offset: 0x00003898
	public byte ReadProtectedByte(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		byte result;
		try
		{
			result = this.ReadProtectedByteArray(pOffset, 1U)[0];
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadByte" + ex.ToString());
			}
			result = 0;
		}
		return result;
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x0000570C File Offset: 0x0000390C
	public short ReadProtectedInt16(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		short result;
		try
		{
			result = BitConverter.ToInt16(this.ReadProtectedByteArray(pOffset, 2U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadInt16" + ex.ToString());
			}
			result = 0;
		}
		return result;
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x00005784 File Offset: 0x00003984
	public short ReadProtectedShort(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		short result;
		try
		{
			result = BitConverter.ToInt16(this.ReadProtectedByteArray(pOffset, 2U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadInt16" + ex.ToString());
			}
			result = 0;
		}
		return result;
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x000057FC File Offset: 0x000039FC
	public int ReadProtectedInt32(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		int result;
		try
		{
			result = BitConverter.ToInt32(this.ReadProtectedByteArray(pOffset, 4U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadInt32" + ex.ToString());
			}
			result = 0;
		}
		return result;
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00005874 File Offset: 0x00003A74
	public IntPtr ReadProtectedIntPtr(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		IntPtr result;
		try
		{
			result = (IntPtr)BitConverter.ToInt32(this.ReadProtectedByteArray(pOffset, 4U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadProtecedIntPtr" + ex.ToString());
			}
			result = IntPtr.Zero;
		}
		return result;
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x000058F4 File Offset: 0x00003AF4
	public int ReadProtectedInteger(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		int result;
		try
		{
			result = BitConverter.ToInt32(this.ReadProtectedByteArray(pOffset, 4U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadInteger" + ex.ToString());
			}
			result = 0;
		}
		return result;
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x0000596C File Offset: 0x00003B6C
	public long ReadProtectedInt64(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		long result;
		try
		{
			result = BitConverter.ToInt64(this.ReadProtectedByteArray(pOffset, 8U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadInt64" + ex.ToString());
			}
			result = 0L;
		}
		return result;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x000059E4 File Offset: 0x00003BE4
	public long ReadProtectedLong(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		long result;
		try
		{
			result = BitConverter.ToInt64(this.ReadProtectedByteArray(pOffset, 8U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadLong" + ex.ToString());
			}
			result = 0L;
		}
		return result;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00005A5C File Offset: 0x00003C5C
	public ushort ReadProtectedUInt16(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		ushort result;
		try
		{
			result = BitConverter.ToUInt16(this.ReadProtectedByteArray(pOffset, 2U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadUInt16" + ex.ToString());
			}
			result = 0;
		}
		return result;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00005AD4 File Offset: 0x00003CD4
	public ushort ReadProtectedUShort(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		ushort result;
		try
		{
			result = BitConverter.ToUInt16(this.ReadProtectedByteArray(pOffset, 2U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadUShort" + ex.ToString());
			}
			result = 0;
		}
		return result;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00005B4C File Offset: 0x00003D4C
	public uint ReadProtectedUInt32(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		uint result;
		try
		{
			result = BitConverter.ToUInt32(this.ReadProtectedByteArray(pOffset, 4U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadUInt32" + ex.ToString());
			}
			result = 0U;
		}
		return result;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00005BC4 File Offset: 0x00003DC4
	public uint ReadProtectedUInteger(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		uint result;
		try
		{
			result = BitConverter.ToUInt32(this.ReadProtectedByteArray(pOffset, 4U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadUInteger" + ex.ToString());
			}
			result = 0U;
		}
		return result;
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00005C3C File Offset: 0x00003E3C
	public ulong ReadProtectedUInt64(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		ulong result;
		try
		{
			result = BitConverter.ToUInt64(this.ReadProtectedByteArray(pOffset, 8U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadUInt64" + ex.ToString());
			}
			result = 0UL;
		}
		return result;
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00005CB4 File Offset: 0x00003EB4
	public long ReadProtectedULong(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		long result;
		try
		{
			result = (long)BitConverter.ToUInt64(this.ReadProtectedByteArray(pOffset, 8U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadULong" + ex.ToString());
			}
			result = 0L;
		}
		return result;
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00005D2C File Offset: 0x00003F2C
	public float ReadProtectedFloat(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		float result;
		try
		{
			result = BitConverter.ToSingle(this.ReadProtectedByteArray(pOffset, 4U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadFloat" + ex.ToString());
			}
			result = 0f;
		}
		return result;
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00005DA8 File Offset: 0x00003FA8
	public double ReadProtectedDouble(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		double result;
		try
		{
			result = BitConverter.ToDouble(this.ReadProtectedByteArray(pOffset, 8U), 0);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadDouble" + ex.ToString());
			}
			result = 0.0;
		}
		return result;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00005E28 File Offset: 0x00004028
	public Vector2 ReadProtectedVector2(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		Vector2 result;
		try
		{
			result = new Vector2(BitConverter.ToSingle(this.ReadProtectedByteArray(pOffset, 4U), 0), BitConverter.ToSingle(this.ReadProtectedByteArray(pOffset + 4, 4U), 0));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadFloat" + ex.ToString());
			}
			result = new Vector2(0f, 0f);
		}
		return result;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00005EC8 File Offset: 0x000040C8
	public Vector3 ReadProtectedVector3(IntPtr pOffset)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		Vector3 result;
		try
		{
			result = new Vector3(BitConverter.ToSingle(this.ReadProtectedByteArray(pOffset, 4U), 0), BitConverter.ToSingle(this.ReadProtectedByteArray(pOffset + 4, 4U), 0), BitConverter.ToSingle(this.ReadProtectedByteArray(pOffset + 8, 4U), 0));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: ReadFloat" + ex.ToString());
			}
			result = new Vector3(0f, 0f, 0f);
		}
		return result;
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00005F80 File Offset: 0x00004180
	public bool WriteByteArray(IntPtr pOffset, byte[] pBytes)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = VAMemory.WriteProcessMemory(this.processHandle, pOffset, pBytes, (uint)pBytes.Length, 0U);
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteByteArray" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00005FFC File Offset: 0x000041FC
	public bool WriteStringUnicode(IntPtr pOffset, string pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, Encoding.Unicode.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteStringUnicode" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00006078 File Offset: 0x00004278
	public bool WriteStringASCII(IntPtr pOffset, string pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, Encoding.ASCII.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteStringASCII" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x000060F4 File Offset: 0x000042F4
	public bool WriteBoolean(IntPtr pOffset, bool pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteBoolean" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x0000616C File Offset: 0x0000436C
	public bool WriteChar(IntPtr pOffset, char pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteChar" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x000061E4 File Offset: 0x000043E4
	public bool WriteByte(IntPtr pOffset, byte pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes((short)pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteByte" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x0000625C File Offset: 0x0000445C
	public bool WriteInt16(IntPtr pOffset, short pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteInt16" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000EC RID: 236 RVA: 0x000062D4 File Offset: 0x000044D4
	public bool WriteShort(IntPtr pOffset, short pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteShort" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x0000634C File Offset: 0x0000454C
	public bool WriteInt32(IntPtr pOffset, int pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteInt32" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000EE RID: 238 RVA: 0x000063C4 File Offset: 0x000045C4
	public bool WriteInteger(IntPtr pOffset, int pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteInt" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000EF RID: 239 RVA: 0x0000643C File Offset: 0x0000463C
	public bool WriteInt64(IntPtr pOffset, long pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteInt64" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x000064B4 File Offset: 0x000046B4
	public bool WriteLong(IntPtr pOffset, long pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteLong" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x0000652C File Offset: 0x0000472C
	public bool WriteUInt16(IntPtr pOffset, ushort pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteUInt16" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x000065A4 File Offset: 0x000047A4
	public bool WriteUShort(IntPtr pOffset, ushort pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteShort" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x0000661C File Offset: 0x0000481C
	public bool WriteUInt32(IntPtr pOffset, uint pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteUInt32" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00006694 File Offset: 0x00004894
	public bool WriteUInteger(IntPtr pOffset, uint pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteUInt" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x0000670C File Offset: 0x0000490C
	public bool WriteUInt64(IntPtr pOffset, ulong pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteUInt64" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00006784 File Offset: 0x00004984
	public bool WriteULong(IntPtr pOffset, ulong pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteULong" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x000067FC File Offset: 0x000049FC
	public bool WriteFloat(IntPtr pOffset, float pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteFloat" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00006874 File Offset: 0x00004A74
	public bool WriteDouble(IntPtr pOffset, double pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteDouble" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x000068EC File Offset: 0x00004AEC
	public bool WriteVector2(IntPtr pOffset, Vector2 pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			bool flag = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData.X));
			bool flag2 = this.WriteByteArray(pOffset + 4, BitConverter.GetBytes(pData.Y));
			if (flag && flag2)
			{
				result = true;
			}
			else
			{
				result = false;
			}
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteFloat" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00006990 File Offset: 0x00004B90
	public bool WriteVector3(IntPtr pOffset, Vector3 pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			bool flag = this.WriteByteArray(pOffset, BitConverter.GetBytes(pData.X));
			bool flag2 = this.WriteByteArray(pOffset + 4, BitConverter.GetBytes(pData.Y));
			bool flag3 = this.WriteByteArray(pOffset + 8, BitConverter.GetBytes(pData.Z));
			if (flag && flag2 && flag3)
			{
				result = true;
			}
			else
			{
				result = false;
			}
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteFloat" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00006A50 File Offset: 0x00004C50
	public bool WriteProtectedByteArray(IntPtr pOffset, byte[] pBytes)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			uint flNewProtect;
			VAMemory.VirtualProtectEx(this.processHandle, pOffset, (UIntPtr)((ulong)((long)pBytes.Length)), 64U, out flNewProtect);
			bool flag = VAMemory.WriteProcessMemory(this.processHandle, pOffset, pBytes, (uint)pBytes.Length, 0U);
			VAMemory.VirtualProtectEx(this.processHandle, pOffset, (UIntPtr)((ulong)((long)pBytes.Length)), flNewProtect, out flNewProtect);
			result = flag;
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteByteArray" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00006AFC File Offset: 0x00004CFC
	public bool WriteProtectedStringUnicode(IntPtr pOffset, string pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, Encoding.Unicode.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteStringUnicode" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00006B78 File Offset: 0x00004D78
	public bool WriteProtectedStringASCII(IntPtr pOffset, string pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, Encoding.ASCII.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteStringASCII" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00006BF4 File Offset: 0x00004DF4
	public bool WriteProtectedBoolean(IntPtr pOffset, bool pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteBoolean" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00006C6C File Offset: 0x00004E6C
	public bool WriteProtectedChar(IntPtr pOffset, char pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteChar" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00006CE4 File Offset: 0x00004EE4
	public bool WriteProtectedByte(IntPtr pOffset, byte pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes((short)pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteByte" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00006D5C File Offset: 0x00004F5C
	public bool WriteProtectedInt16(IntPtr pOffset, short pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteInt16" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00006DD4 File Offset: 0x00004FD4
	public bool WriteProtectedShort(IntPtr pOffset, short pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteShort" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00006E4C File Offset: 0x0000504C
	public bool WriteProtectedInt32(IntPtr pOffset, int pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteInt32" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00006EC4 File Offset: 0x000050C4
	public bool WriteProtectedInteger(IntPtr pOffset, int pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteInt" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00006F3C File Offset: 0x0000513C
	public bool WriteProtectedInt64(IntPtr pOffset, long pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteInt64" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00006FB4 File Offset: 0x000051B4
	public bool WriteProtectedLong(IntPtr pOffset, long pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteLong" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x06000107 RID: 263 RVA: 0x0000702C File Offset: 0x0000522C
	public bool WriteProtectedUInt16(IntPtr pOffset, ushort pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteUInt16" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x06000108 RID: 264 RVA: 0x000070A4 File Offset: 0x000052A4
	public bool WriteProtectedUShort(IntPtr pOffset, ushort pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteShort" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x06000109 RID: 265 RVA: 0x0000711C File Offset: 0x0000531C
	public bool WriteProtectedUInt32(IntPtr pOffset, uint pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteUInt32" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00007194 File Offset: 0x00005394
	public bool WriteProtectedUInteger(IntPtr pOffset, uint pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteUInt" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x0600010B RID: 267 RVA: 0x0000720C File Offset: 0x0000540C
	public bool WriteProtectedUInt64(IntPtr pOffset, ulong pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteUInt64" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00007284 File Offset: 0x00005484
	public bool WriteProtectedULong(IntPtr pOffset, ulong pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteULong" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x0600010D RID: 269 RVA: 0x000072FC File Offset: 0x000054FC
	public bool WriteProtectedFloat(IntPtr pOffset, float pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteFloat" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00007374 File Offset: 0x00005574
	public bool WriteProtectedDouble(IntPtr pOffset, double pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			result = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData));
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteDouble" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x0600010F RID: 271 RVA: 0x000073EC File Offset: 0x000055EC
	public bool WriteProtectedVector2(IntPtr pOffset, Vector2 pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			bool flag = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData.X));
			bool flag2 = this.WriteProtectedByteArray(pOffset + 4, BitConverter.GetBytes(pData.Y));
			if (flag && flag2)
			{
				result = true;
			}
			else
			{
				result = false;
			}
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteFloat" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00007490 File Offset: 0x00005690
	public bool WriteProtectedVector3(IntPtr pOffset, Vector3 pData)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			bool flag = this.WriteProtectedByteArray(pOffset, BitConverter.GetBytes(pData.X));
			bool flag2 = this.WriteProtectedByteArray(pOffset + 4, BitConverter.GetBytes(pData.Y));
			bool flag3 = this.WriteProtectedByteArray(pOffset + 8, BitConverter.GetBytes(pData.Z));
			if (flag && flag2 && flag3)
			{
				result = true;
			}
			else
			{
				result = false;
			}
		}
		catch (Exception ex)
		{
			if (VAMemory.debugMode)
			{
				Console.WriteLine("Error: WriteFloat" + ex.ToString());
			}
			result = false;
		}
		return result;
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00007550 File Offset: 0x00005750
	private bool DumpMemory()
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			this.m_vDumpedRegion = new byte[this.m_vSize];
			IntPtr zero = IntPtr.Zero;
			if (!VAMemory.ReadProcessMemory(this.processHandle, this.m_vAddress, this.m_vDumpedRegion, this.m_vSize, out zero))
			{
				result = false;
			}
			else
			{
				result = true;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("VAM: DUMPMEM: " + ex.ToString());
			result = false;
		}
		return result;
	}

	// Token: 0x06000112 RID: 274 RVA: 0x000075F4 File Offset: 0x000057F4
	private bool GetModuleInfos(string moduleName)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		bool result;
		try
		{
			if (this.mainProcess.Length != 0)
			{
				foreach (object obj in this.mainProcess[0].Modules)
				{
					ProcessModule processModule = (ProcessModule)obj;
					if (processModule.ModuleName == moduleName)
					{
						this.m_vAddress = processModule.BaseAddress;
						this.m_vSize = (uint)processModule.ModuleMemorySize;
						return true;
					}
				}
				result = true;
			}
			else
			{
				result = false;
			}
		}
		catch (Exception)
		{
			result = false;
		}
		return result;
	}

	// Token: 0x06000113 RID: 275 RVA: 0x000076D8 File Offset: 0x000058D8
	private bool MaskCheck(int nOffset, byte[] btPattern, string strMask)
	{
		for (int i = 0; i < btPattern.Length; i++)
		{
			if (strMask[i] != '?' && strMask[i] == 'x' && btPattern[i] != this.m_vDumpedRegion[nOffset + i])
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000114 RID: 276 RVA: 0x0000772C File Offset: 0x0000592C
	public IntPtr FindRawPattern(string moduleName, string bPattern)
	{
		return this.FindPattern(moduleName, bPattern, new List<int>
		{
			0
		}, 0, false, false);
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00007754 File Offset: 0x00005954
	public IntPtr FindPatternOffset(string moduleName, string bPattern, int iOffset, int iExtra, bool bRelative)
	{
		return this.FindPattern(moduleName, bPattern, new List<int>
		{
			iOffset
		}, iExtra, bRelative, true);
	}

	// Token: 0x06000116 RID: 278 RVA: 0x00007780 File Offset: 0x00005980
	public IntPtr FindPatternOffset(string moduleName, string bPattern, List<int> iOffset, int iExtra, bool bRelative)
	{
		return this.FindPattern(moduleName, bPattern, iOffset, iExtra, bRelative, true);
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00007790 File Offset: 0x00005990
	public IntPtr FindPattern(string moduleName, string bPattern, int iOffset, int iExtra, bool bRelative, bool bSubtract)
	{
		return this.FindPattern(moduleName, bPattern, new List<int>
		{
			iOffset
		}, iExtra, bRelative, bSubtract);
	}

	// Token: 0x06000118 RID: 280 RVA: 0x000077BC File Offset: 0x000059BC
	public IntPtr FindPattern(string moduleName, string bPattern, List<int> iOffset, int iExtra, bool bRelative, bool bSubtract)
	{
		IntPtr result;
		try
		{
			string text = "";
			char c = ' ';
			foreach (char c2 in bPattern)
			{
				if (c2 == '?')
				{
					text += "?";
				}
				else if (c2 != ' ' && c != ' ')
				{
					text += "x";
				}
				c = c2;
			}
			string[] array = bPattern.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
			List<string> list = new List<string>();
			foreach (string text2 in array)
			{
				if (text2 == "?")
				{
					list.Add("FF");
				}
				else
				{
					list.Add(text2);
				}
			}
			string[] array3 = list.ToArray();
			List<byte> list2 = new List<byte>();
			string[] array2 = array3;
			for (int i = 0; i < array2.Length; i++)
			{
				byte item = Convert.ToByte(array2[i], 16);
				list2.Add(item);
			}
			byte[] btPattern = list2.ToArray();
			result = this.FindPattern(moduleName, btPattern, text, iOffset, iExtra, bRelative, bSubtract);
		}
		catch (Exception ex)
		{
			Console.WriteLine("VAM: FindPattern: " + ex.ToString());
			result = IntPtr.Zero;
		}
		return result;
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00007928 File Offset: 0x00005B28
	public IntPtr FindPattern(string moduleName, byte[] btPattern, string strMask, List<int> iOffset, int iExtra, bool bRelative, bool bSubtract)
	{
		IntPtr zero;
		try
		{
			if (!this.GetModuleInfos(moduleName))
			{
				zero = IntPtr.Zero;
			}
			else if ((this.m_vDumpedRegion == null || this.m_vDumpedRegion.Length == 0) && !this.DumpMemory())
			{
				zero = IntPtr.Zero;
			}
			else if (strMask.Length != btPattern.Length)
			{
				zero = IntPtr.Zero;
			}
			else
			{
				for (int i = 0; i < this.m_vDumpedRegion.Length; i++)
				{
					if (this.MaskCheck(i, btPattern, strMask))
					{
						int num = this.m_vAddress.ToInt32() + i;
						if (bRelative)
						{
							foreach (int num2 in iOffset)
							{
								num += num2;
								num = this.ReadInt32((IntPtr)num);
							}
						}
						num += iExtra;
						if (bSubtract)
						{
							num -= this.m_vAddress.ToInt32();
						}
						this.ResetRegion();
						return (IntPtr)num;
					}
				}
				zero = IntPtr.Zero;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("VAM: FindPattern: " + ex.ToString());
			zero = IntPtr.Zero;
		}
		return zero;
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00007AA4 File Offset: 0x00005CA4
	public IntPtr GetModuleBaseAddress(string moduleName)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		IntPtr zero;
		try
		{
			if (this.mainProcess.Length != 0)
			{
				foreach (object obj in this.mainProcess[0].Modules)
				{
					ProcessModule processModule = (ProcessModule)obj;
					if (processModule.ModuleName == moduleName)
					{
						return processModule.BaseAddress;
					}
				}
				zero = IntPtr.Zero;
			}
			else
			{
				zero = IntPtr.Zero;
			}
		}
		catch (Exception)
		{
			zero = IntPtr.Zero;
		}
		return zero;
	}

	// Token: 0x0600011B RID: 283 RVA: 0x00007B80 File Offset: 0x00005D80
	public int GetModuleSize(string moduleName)
	{
		if (this.processHandle == IntPtr.Zero)
		{
			this.CheckProcess();
		}
		int result;
		try
		{
			if (this.mainProcess.Length != 0)
			{
				foreach (object obj in this.mainProcess[0].Modules)
				{
					ProcessModule processModule = (ProcessModule)obj;
					if (processModule.ModuleName == moduleName)
					{
						return processModule.ModuleMemorySize;
					}
				}
				result = 0;
			}
			else
			{
				result = 0;
			}
		}
		catch (Exception)
		{
			result = 0;
		}
		return result;
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00007C50 File Offset: 0x00005E50
	public void ResetRegion()
	{
		this.m_vDumpedRegion = null;
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00007C5C File Offset: 0x00005E5C
	private string GetPropName(int dwAddress)
	{
		int value = this.ReadInt32((IntPtr)dwAddress + this.m_pVarName);
		return new UTF8Encoding().GetString(this.ReadByteArray((IntPtr)value, 128U));
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00007CA0 File Offset: 0x00005EA0
	private int GetDataTable(int dwAddress)
	{
		return this.ReadInt32((IntPtr)dwAddress + this.m_pDataTable);
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00007CBC File Offset: 0x00005EBC
	private int GetOffset(int dwAddress)
	{
		return this.ReadInt32((IntPtr)dwAddress + this.m_iOffset);
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00007CD8 File Offset: 0x00005ED8
	private int GetPropById(int dwAddress, int iIndex)
	{
		return this.ReadInt32((IntPtr)dwAddress + this.m_pProps) + this.nCRecvPropSize * iIndex;
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00007CFC File Offset: 0x00005EFC
	private int GetPropCount(int dwAddress)
	{
		return this.ReadInt32((IntPtr)dwAddress + this.m_nProps);
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00007D18 File Offset: 0x00005F18
	private string GetTableName(int dwAddress)
	{
		int value = this.ReadInt32((IntPtr)dwAddress + this.m_pNetTableName);
		return new UTF8Encoding().GetString(this.ReadByteArray((IntPtr)value, 128U));
	}

	// Token: 0x06000123 RID: 291 RVA: 0x00007D5C File Offset: 0x00005F5C
	private int GetTable(int dwAddress)
	{
		return this.ReadInt32((IntPtr)dwAddress + this.m_pRecvTable);
	}

	// Token: 0x06000124 RID: 292 RVA: 0x00007D78 File Offset: 0x00005F78
	private int GetNextClass(int dwAddress)
	{
		return this.ReadInt32((IntPtr)dwAddress + this.m_pNext);
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00007D94 File Offset: 0x00005F94
	private int ScanTable(int dwTableAddr, string lpVarName, int dwLevel)
	{
		for (int i = 0; i < this.GetPropCount(dwTableAddr); i++)
		{
			int propById = this.GetPropById(dwTableAddr, i);
			if (propById != 0)
			{
				string propName = this.GetPropName(propById);
				if (propName != null && !char.IsDigit(propName[0]))
				{
					int offset = this.GetOffset(propById);
					if (propName.ToLower().StartsWith(lpVarName.ToLower()))
					{
						return dwLevel + offset;
					}
					int dataTable = this.GetDataTable(propById);
					if (dataTable != 0)
					{
						int num = this.ScanTable(dataTable, lpVarName, dwLevel + offset);
						if (num != 0)
						{
							return num;
						}
					}
				}
			}
		}
		return 0;
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00007E3C File Offset: 0x0000603C
	private string normalize(string myStr)
	{
		string text = "";
		foreach (char c in myStr)
		{
			if (char.IsControl(c) && c != '_')
			{
				return text;
			}
			text += c.ToString();
		}
		return text;
	}

	// Token: 0x06000127 RID: 295 RVA: 0x00007E9C File Offset: 0x0000609C
	public int FindNetvar(int dwStart, string lpClassName, string lpVarName)
	{
		for (int dwAddress = dwStart; dwAddress != 0; dwAddress = this.GetNextClass(dwAddress))
		{
			int table = this.GetTable(dwAddress);
			string text = this.GetTableName(table);
			text = this.normalize(text);
			if (text != null && !char.IsDigit(text[0]) && text.ToLower().Equals(lpClassName.ToLower()))
			{
				return this.ScanTable(table, lpVarName, 0);
			}
		}
		return 0;
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00007F10 File Offset: 0x00006110
	public void SetStartAddress(int startAddr)
	{
		this.dwStartAddress = startAddr;
	}

	// Token: 0x06000129 RID: 297 RVA: 0x00007F1C File Offset: 0x0000611C
	public int FindNetvar(int dwStart, string lpClassName, string lpVarName, int iExtra)
	{
		return this.FindNetvar(dwStart, lpClassName, lpVarName) + iExtra;
	}

	// Token: 0x0600012A RID: 298 RVA: 0x00007F2C File Offset: 0x0000612C
	public int FindNetvar(string lpClassName, string lpVarName)
	{
		if (this.dwStartAddress != 0)
		{
			return this.FindNetvar(this.dwStartAddress, lpClassName, lpVarName);
		}
		return 0;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x00007F4C File Offset: 0x0000614C
	public int FindNetvar(string lpClassName, string lpVarName, int iExtra)
	{
		if (this.dwStartAddress != 0)
		{
			return this.FindNetvar(this.dwStartAddress, lpClassName, lpVarName) + iExtra;
		}
		return 0;
	}

	// Token: 0x0600012C RID: 300 RVA: 0x00007F6C File Offset: 0x0000616C
	private void ErrorProcessNotFound(string pProcessName)
	{
		MessageBox.Show(this.processName + " is not running or has not been found. Please check and try again", "Process Not Found", MessageBoxButtons.OK, MessageBoxIcon.Hand);
	}

	// Token: 0x040000EA RID: 234
	public static bool debugMode;

	// Token: 0x040000EB RID: 235
	private Process[] mainProcess;

	// Token: 0x040000EC RID: 236
	private IntPtr processHandle;

	// Token: 0x040000ED RID: 237
	private byte[] m_vDumpedRegion;

	// Token: 0x040000EE RID: 238
	private IntPtr m_vAddress;

	// Token: 0x040000EF RID: 239
	private uint m_vSize;

	// Token: 0x040000F0 RID: 240
	private int dwStartAddress;

	// Token: 0x040000F1 RID: 241
	private int nCRecvPropSize = 60;

	// Token: 0x040000F2 RID: 242
	private int m_pVarName;

	// Token: 0x040000F3 RID: 243
	private int m_pDataTable = 40;

	// Token: 0x040000F4 RID: 244
	private int m_iOffset = 44;

	// Token: 0x040000F5 RID: 245
	private int m_pProps;

	// Token: 0x040000F6 RID: 246
	private int m_nProps = 4;

	// Token: 0x040000F7 RID: 247
	private int m_pNetTableName = 12;

	// Token: 0x040000F8 RID: 248
	private int m_pRecvTable = 12;

	// Token: 0x040000F9 RID: 249
	private int m_pNext = 16;

	// Token: 0x0200027B RID: 635
	[Flags]
	private enum ProcessAccessFlags : uint
	{
		// Token: 0x04001DDA RID: 7642
		All = 2035711U,
		// Token: 0x04001DDB RID: 7643
		Terminate = 1U,
		// Token: 0x04001DDC RID: 7644
		CreateThread = 2U,
		// Token: 0x04001DDD RID: 7645
		VMOperation = 8U,
		// Token: 0x04001DDE RID: 7646
		VMRead = 16U,
		// Token: 0x04001DDF RID: 7647
		VMWrite = 32U,
		// Token: 0x04001DE0 RID: 7648
		DupHandle = 64U,
		// Token: 0x04001DE1 RID: 7649
		SetInformation = 512U,
		// Token: 0x04001DE2 RID: 7650
		QueryInformation = 1024U,
		// Token: 0x04001DE3 RID: 7651
		Synchronize = 1048576U
	}

	// Token: 0x0200027C RID: 636
	private enum VirtualMemoryProtection : uint
	{
		// Token: 0x04001DE5 RID: 7653
		PAGE_NOACCESS = 1U,
		// Token: 0x04001DE6 RID: 7654
		PAGE_READONLY,
		// Token: 0x04001DE7 RID: 7655
		PAGE_READWRITE = 4U,
		// Token: 0x04001DE8 RID: 7656
		PAGE_WRITECOPY = 8U,
		// Token: 0x04001DE9 RID: 7657
		PAGE_EXECUTE = 16U,
		// Token: 0x04001DEA RID: 7658
		PAGE_EXECUTE_READ = 32U,
		// Token: 0x04001DEB RID: 7659
		PAGE_EXECUTE_READWRITE = 64U,
		// Token: 0x04001DEC RID: 7660
		PAGE_EXECUTE_WRITECOPY = 128U,
		// Token: 0x04001DED RID: 7661
		PAGE_GUARD = 256U,
		// Token: 0x04001DEE RID: 7662
		PAGE_NOCACHE = 512U,
		// Token: 0x04001DEF RID: 7663
		PROCESS_ALL_ACCESS = 2035711U
	}
}
