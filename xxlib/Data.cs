using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace xx
{
    public class Data : IDisposable
    {

        public static bool IsBigEndian { get; set; }


        Lazy<Dictionary<ISerde, uint>> _ptrStore = new Lazy<Dictionary<ISerde, uint>>(System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
    
        public Dictionary<ISerde, uint> PtrStore => _ptrStore.Value;       
        
        byte[] buff;
        int len;
        int cap;
         
        /// <summary>
        /// 长度
        /// </summary>
        public int Length => len;
        /// <summary>
        /// 分配大小
        /// </summary>
        public int Cap => cap;

        public Data(int cap = 256)
        {
            var siz = EnsureCapcity(cap);
            buff = new byte[siz];
            this.cap = siz;
        }

        public Data(byte[] data)
        {
            this.cap = data.Length;
            buff = data;
        }


        /// <summary>
        /// 重新设置大小
        /// </summary>
        /// <param name="newSize"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reserve(int newSize)
        {
            if (newSize < cap) return;

            var siz = EnsureCapcity(newSize);
            if (buff is null)
                buff = new byte[siz];
            else
                Array.Resize(ref buff, siz);
          
            cap = siz;
        }

        /// <summary>
        /// 重新设置长度
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Resize(int newLen)
        {
            if (newLen > cap)
                Reserve(newLen);
            var rtv = len;
            len = newLen;
            return rtv;
        }

        /// <summary>
        /// 通过 初始化列表 填充内容. 填充前会先 Clear.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Fill(byte[] data)
        {
            Clear();
            Reserve(data.Length);
            unsafe
            {
                foreach (var b in data)
                    buff[len++] = b;
            }
        }


        #region Write

        /// <summary>
        /// 写入一段二进制
        /// </summary>        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBuf(byte[] data, int index, int length)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (len + length > cap)
            {
                var write_index = Resize(len + length);
                Buffer.BlockCopy(data, index, buff, write_index, length);
            }
            else
            {
                Buffer.BlockCopy(data, index, buff, len, length);
            }

            len += length;
        }

        /// <summary>
        /// 写入一段二进制
        /// </summary>        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBuf(byte[] data) => WriteBuf(data, 0, data.Length);


        /// <summary>
        /// 写入定长 byte
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixed(byte v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (len + 1 > cap)
                Reserve(len + 1);

            buff[len] = v;
            len += 1;

        }

        /// <summary>
        /// 写入定长 sbyte
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixed(sbyte v) => WriteFixed((byte)v);

        /// <summary>
        /// 写入bool
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixed(bool v)
            => WriteFixed(v ? (byte)1 : (byte)0);

        /// <summary>
        /// 写入定长 ushort
        /// </summary>   
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixed(ushort v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (IsBigEndian)
                v = BinaryPrimitives.ReverseEndianness(v);

            if (len + sizeof(ushort) > cap)
                Reserve(len + sizeof(ushort));

            var p =  len;
            buff[p] = (byte)v;
            buff[p+1] = (byte)(v >> 8);
            len += sizeof(short);
        }

        /// <summary>
        /// 写入定长 short
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixed(short v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (IsBigEndian)
                v = BinaryPrimitives.ReverseEndianness(v);

            if (len + sizeof(short) > cap)
                Reserve(len + sizeof(short));

            var p = len;
            buff[p] = (byte)v;
            buff[p + 1] = (byte)(v >> 8);
            len += sizeof(short);

        }

        /// <summary>
        /// 写入定长 uint
        /// </summary>   
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixed(uint v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (IsBigEndian)
                v = BinaryPrimitives.ReverseEndianness(v);

            if (len + sizeof(uint) > cap)
                Reserve(len + sizeof(uint));

            var p = len;
            buff[p] = (byte)v;
            buff[p + 1] = (byte)(v >> 8);
            buff[p + 2] = (byte)(v >> 16);
            buff[p + 3] = (byte)(v >> 24);
            len += sizeof(uint);

        }

        /// <summary>
        /// 写入定长 int
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixed(int v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (IsBigEndian)
                v = BinaryPrimitives.ReverseEndianness(v);

            if (len + sizeof(int) > cap)
                Reserve(len + sizeof(int));

            var p = len;
            buff[p] = (byte)v;
            buff[p + 1] = (byte)(v >> 8);
            buff[p + 2] = (byte)(v >> 16);
            buff[p + 3] = (byte)(v >> 24);
            len += sizeof(uint);

        }

        /// <summary>
        /// 写入定长 ulong
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixed(ulong v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (IsBigEndian)
                v = BinaryPrimitives.ReverseEndianness(v);

            if (len + sizeof(ulong) > cap)
                Reserve(len + sizeof(ulong));

            var p = len;
            buff[p] = (byte)v;
            buff[p + 1] = (byte)(v >> 8);
            buff[p + 2] = (byte)(v >> 16);
            buff[p + 3] = (byte)(v >> 24);
            buff[p + 4] = (byte)(v >> 32);
            buff[p + 5] = (byte)(v >> 40);
            buff[p + 6] = (byte)(v >> 48);
            buff[p + 7] = (byte)(v >> 56);
            len += sizeof(ulong);

        }

        /// <summary>
        /// 写入定长 long
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixed(long v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (IsBigEndian)
                v = BinaryPrimitives.ReverseEndianness(v);

            if (len + sizeof(long) > cap)
                Reserve(len + sizeof(long));

            var p = len;
            buff[p] = (byte)v;
            buff[p + 1] = (byte)(v >> 8);
            buff[p + 2] = (byte)(v >> 16);
            buff[p + 3] = (byte)(v >> 24);
            buff[p + 4] = (byte)(v >> 32);
            buff[p + 5] = (byte)(v >> 40);
            buff[p + 6] = (byte)(v >> 48);
            buff[p + 7] = (byte)(v >> 56);
            len += sizeof(ulong);
        }

        /// <summary>
        /// 写入定长 float
        /// </summary>   
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixed(float v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (len + sizeof(float) > cap)
                Reserve(len + sizeof(float));

            var x = new FloatingInteger { f = v };
            unsafe
            {
                var p = len;

                if (IsBigEndian)
                {
                    buff[p] = x.b3;
                    buff[p + 1] = x.b2;
                    buff[p + 2] = x.b1;
                    buff[p + 3] = x.b0;
                }
                else
                {
                    buff[p] = x.b0;
                    buff[p + 1] = x.b1;
                    buff[p + 2] = x.b2;
                    buff[p + 3] = x.b3;
                }
                len += 4;
            }
        }

        /// <summary>
        /// 写入定长 double
        /// </summary>      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixed(double v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (len + sizeof(double) > cap)
                Reserve(len + sizeof(double));

            var x = new FloatingInteger { d = v };
            unsafe
            {
                var p =  len;
                if (IsBigEndian)
                {
                    buff[p] = x.b7;
                    buff[p + 1] = x.b6;
                    buff[p + 2] = x.b5;
                    buff[p + 3] = x.b4;
                    buff[p + 4] = x.b3;
                    buff[p + 5] = x.b2;
                    buff[p + 6] = x.b1;
                    buff[p + 7] = x.b0;
                }
                else
                {
                    buff[p] = x.b0;
                    buff[p + 1] = x.b1;
                    buff[p + 2] = x.b2;
                    buff[p + 3] = x.b3;
                    buff[p + 4] = x.b4;
                    buff[p + 5] = x.b5;
                    buff[p + 6] = x.b6;
                    buff[p + 7] = x.b7;
                }
                len += 8;
            }
        }

        #endregion

        #region WriteAt

        /// <summary>
        /// 写入一段二进制
        /// </summary>        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void WriteBufAt(int idx, byte[] data, int index, int length)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (idx+ length > len)
                Resize(len + idx + length);
           
            Buffer.BlockCopy(data, index, buff, idx, length);

        }

        /// <summary>
        /// 写入一段二进制
        /// </summary>        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBufAt(int idx,byte[] data) => WriteBufAt(idx,data, 0, data.Length);

        /// <summary>
        /// 写入定长 byte
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixedAt(int idx, byte v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (idx + 1 > len)
                Resize(idx + 1);

            buff[idx] = v;

        }

        /// <summary>
        /// 写入定长 sbyte
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixedAt(int idx, sbyte v) => WriteFixedAt(idx,(byte)v);

        /// <summary>
        /// 写入bool
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixedAt(int idx, bool v)
            => WriteFixedAt(idx,v ? (byte)1 : (byte)0);

        /// <summary>
        /// 写入定长 ushort
        /// </summary>   
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixedAt(int idx, ushort v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (IsBigEndian)
                v = BinaryPrimitives.ReverseEndianness(v);

            if (idx + sizeof(ushort) > len)
                Resize(idx + sizeof(ushort));

            buff[idx] = (byte)v;
            buff[idx + 1] = (byte)(v >> 8);

        }

        /// <summary>
        /// 写入定长 short
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixedAt(int idx, short v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (IsBigEndian)
                v = BinaryPrimitives.ReverseEndianness(v);

            if (idx + sizeof(short) > len)
                Resize(idx + sizeof(short));

            buff[idx] = (byte)v;
            buff[idx + 1] = (byte)(v >> 8);

        }

        /// <summary>
        /// 写入定长 uint
        /// </summary>   
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixedAt(int idx, uint v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (IsBigEndian)
                v = BinaryPrimitives.ReverseEndianness(v);

            if (idx + sizeof(uint) > len)
                Resize(idx + sizeof(uint));


            buff[idx] = (byte)v;
            buff[idx + 1] = (byte)(v >> 8);
            buff[idx + 2] = (byte)(v >> 16);
            buff[idx + 3] = (byte)(v >> 24);

        }

        /// <summary>
        /// 写入定长 int
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixedAt(int idx, int v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (IsBigEndian)
                v = BinaryPrimitives.ReverseEndianness(v);

            if (idx + sizeof(int) > len)
                Resize(idx + sizeof(int));


            buff[idx] = (byte)v;
            buff[idx + 1] = (byte)(v >> 8);
            buff[idx + 2] = (byte)(v >> 16);
            buff[idx + 3] = (byte)(v >> 24);

        }

        /// <summary>
        /// 写入定长 ulong
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixedAt(int idx, ulong v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (IsBigEndian)
                v = BinaryPrimitives.ReverseEndianness(v);
            if (idx + sizeof(ulong) > len)
                Resize(idx + sizeof(ulong));


            buff[idx] = (byte)v;
            buff[idx + 1] = (byte)(v >> 8);
            buff[idx + 2] = (byte)(v >> 16);
            buff[idx + 3] = (byte)(v >> 24);
            buff[idx + 4] = (byte)(v >> 32);
            buff[idx + 5] = (byte)(v >> 40);
            buff[idx + 6] = (byte)(v >> 48);
            buff[idx + 7] = (byte)(v >> 56);

        }

        /// <summary>
        /// 写入定长 long
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixedAt(int idx, long v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (IsBigEndian)
                v = BinaryPrimitives.ReverseEndianness(v);

            if (idx + sizeof(long) > len)
                Resize(idx + sizeof(long));


            buff[idx] = (byte)v;
            buff[idx + 1] = (byte)(v >> 8);
            buff[idx + 2] = (byte)(v >> 16);
            buff[idx + 3] = (byte)(v >> 24);
            buff[idx + 4] = (byte)(v >> 32);
            buff[idx + 5] = (byte)(v >> 40);
            buff[idx + 6] = (byte)(v >> 48);
            buff[idx + 7] = (byte)(v >> 56);

        }

        /// <summary>
        /// 写入定长 float
        /// </summary>   
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixedAt(int idx, float v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (idx + sizeof(float) > len)
                Resize(idx + sizeof(float));

            var x = new FloatingInteger { f = v };

            buff[idx] = x.b0;
            buff[idx + 1] = x.b1;
            buff[idx + 2] = x.b2;
            buff[idx + 3] = x.b3;

        }

        /// <summary>
        /// 写入定长 double
        /// </summary>      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteFixedAt(int idx, double v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (idx + sizeof(double) > len)
                Resize(idx + sizeof(double));

            var x = new FloatingInteger { d = v };

            buff[idx] = x.b0;
            buff[idx + 1] = x.b1;
            buff[idx + 2] = x.b2;
            buff[idx + 3] = x.b3;
            buff[idx + 4] = x.b4;
            buff[idx + 5] = x.b5;
            buff[idx + 6] = x.b6;
            buff[idx + 7] = x.b7;

        }

        #endregion

        #region WriteVar   

        /// <summary>
        /// 变长写入i64
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteVarInteger(short v) => WriteVarInteger(ZigZagEncode(v));

        /// <summary>
        /// 变长写入u16
        /// </summary>      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteVarInteger(ushort v) => WriteVarInteger((ulong)v);

        /// <summary>
        /// 变长写入i32
        /// </summary>
        /// <param name="v"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteVarInteger(int v) => WriteVarInteger(ZigZagEncode(v));

        /// <summary>
        /// 变长写入u32
        /// </summary>      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteVarInteger(uint v) => WriteVarInteger((ulong)v);

        /// <summary>
        /// 变长写入i64
        /// </summary>     
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteVarInteger(long v) => WriteVarInteger(ZigZagEncode(v));

        /// <summary>
        /// 变长写入u64
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteVarInteger(ulong v)
        {
            if (ischecknull())
                throw new ObjectDisposedException("Data");

            if (len + ComputeRawVarint64Size(v) > cap)
                Reserve(len + ComputeRawVarint64Size(v));

            unsafe
            {
                while (v >= 1 << 7)
                {
                    buff[len++] = (byte)(v & 0x7fu | 0x80u);
                    v >>= 7;
                };

                buff[len++] = (byte)v;
            }
        }

        /// <summary>
        /// 变长写入字符串
        /// </summary>       
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteVarInteger(string v)
        {
            var buff= System.Text.Encoding.UTF8.GetBytes(v);
            WriteVarInteger((uint)buff.Length);
            WriteBuf(buff);
        }


        #endregion



        #region other
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool ischecknull() => cap == 0;
       
        /// <summary>
        /// 清空DATA
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {          
            len= 0;            
        }



        /// <summary>
        /// 转换成byte[] 同时清空
        /// </summary>
        /// <returns>(data,len)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public (byte[],int) ToArray()
        {           
            var len = this.len;
            Clear();
            return (buff,len);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] ToData()
        {          
            var len = this.len;
            Clear();
            var data = new byte[len];
            Buffer.BlockCopy(buff, 0, data, 0, len);
            return data;
        }


        /// <summary>
        /// 传入len 返回2的倍数
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int EnsureCapcity(int value)
        {
            if (value < 0)
                throw new OverflowException("value <0");

            int newCapacity = Math.Max(value, 256);
            if (newCapacity < cap * 2)
                newCapacity = cap * 2;

            if ((uint)(cap * 2) > 0x7FFFFFC7)
                newCapacity = Math.Max(value, 0x7FFFFFC7);

            return newCapacity;

        }

        public void Dispose()
        {
            buff = null;
            cap = 0;
            Clear();
        }

        /// <summary>
        /// 下标访问
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>      
        public byte this[int index] {
            get
            {
                unsafe
                {
                    return buff[index];
                }
            }
        }



        // 负转正：利用单数来存负数，双数来存正数
        // 等效代码： if( v < 0 ) return -v * 2 - 1; else return v * 2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ZigZagEncode(short v) { return (ushort)((v << 1) ^ (v >> 15)); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ZigZagEncode(int v) { return (uint)((v << 1) ^ (v >> 31)); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ZigZagEncode(long v) { return (ulong)((v << 1) ^ (v >> 63)); }



        /// <summary>
        /// 求u64  变长序列化后的长度
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ComputeRawVarint64Size(ulong v)
        {
            if ((v & (ulong.MaxValue << 7)) == 0)
                return 1;
            if ((v & (ulong.MaxValue << 14)) == 0)
                return 2;
            if ((v & (ulong.MaxValue << 21)) == 0)
                return 3;
            if ((v & (ulong.MaxValue << 28)) == 0)
                return 4;
            if ((v & (ulong.MaxValue << 35)) == 0)
                return 5;
            if ((v & (ulong.MaxValue << 42)) == 0)
                return 6;
            if ((v & (ulong.MaxValue << 49)) == 0)
                return 7;
            if ((v & (ulong.MaxValue << 56)) == 0)
                return 8;
            if ((v & (ulong.MaxValue << 63)) == 0)
                return 9;
            return 10;
        }

        #endregion
    }

    /// <summary>
    /// 用于浮点到各长度整型的快速转换 
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 8, CharSet = CharSet.Ansi)]
    public struct FloatingInteger
    {
        [FieldOffset(0)] public double d;
        [FieldOffset(0)] public ulong ul;
        [FieldOffset(0)] public float f;
        [FieldOffset(0)] public uint u;
        [FieldOffset(0)] public byte b0;
        [FieldOffset(1)] public byte b1;
        [FieldOffset(2)] public byte b2;
        [FieldOffset(3)] public byte b3;
        [FieldOffset(4)] public byte b4;
        [FieldOffset(5)] public byte b5;
        [FieldOffset(6)] public byte b6;
        [FieldOffset(7)] public byte b7;
    }

    public static class BinaryPrimitives
    {
        /// <summary>
        /// This is a no-op and added only for consistency.
        /// This allows the caller to read a struct of numeric primitives and reverse each field
        /// rather than having to skip sbyte fields.
        /// </summary>       
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReverseEndianness(sbyte value) => value;

        /// <summary>
        /// Reverses a primitive value - performs an endianness swap
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ReverseEndianness(short value) => (short)ReverseEndianness((ushort)value);

        /// <summary>
        /// Reverses a primitive value - performs an endianness swap
        /// </summary>     
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ReverseEndianness(int value) => (int)ReverseEndianness((uint)value);

        /// <summary>
        /// Reverses a primitive value - performs an endianness swap
        /// </summary>      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ReverseEndianness(long value) => (long)ReverseEndianness((ulong)value);

        /// <summary>
        /// This is a no-op and added only for consistency.
        /// This allows the caller to read a struct of numeric primitives and reverse each field
        /// rather than having to skip byte fields.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ReverseEndianness(byte value) => value;

        /// <summary>
        /// Reverses a primitive value - performs an endianness swap
        /// </summary>      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort ReverseEndianness(ushort value)
        {
            // Don't need to AND with 0xFF00 or 0x00FF since the final
            // cast back to ushort will clear out all bits above [ 15 .. 00 ].
            // This is normally implemented via "movzx eax, ax" on the return.
            // Alternatively, the compiler could elide the movzx instruction
            // entirely if it knows the caller is only going to access "ax"
            // instead of "eax" / "rax" when the function returns.

            return (ushort)((value >> 8) + (value << 8));
        }

        /// <summary>
        /// Reverses a primitive value - performs an endianness swap
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint ReverseEndianness(uint value)
        {
            // This takes advantage of the fact that the JIT can detect
            // ROL32 / ROR32 patterns and output the correct intrinsic.
            //
            // Input: value = [ ww xx yy zz ]
            //
            // First line generates : [ ww xx yy zz ]
            //                      & [ 00 FF 00 FF ]
            //                      = [ 00 xx 00 zz ]
            //             ROR32(8) = [ zz 00 xx 00 ]
            //
            // Second line generates: [ ww xx yy zz ]
            //                      & [ FF 00 FF 00 ]
            //                      = [ ww 00 yy 00 ]
            //             ROL32(8) = [ 00 yy 00 ww ]
            //
            //                (sum) = [ zz yy xx ww ]
            //
            // Testing shows that throughput increases if the AND
            // is performed before the ROL / ROR.

            return RotateRight(value & 0x00FF00FFu, 8) // xx zz
                + RotateLeft(value & 0xFF00FF00u, 8); // ww yy
        }

        /// <summary>
        /// Reverses a primitive value - performs an endianness swap
        /// </summary>       
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ReverseEndianness(ulong value)
        {
            // Operations on 32-bit values have higher throughput than
            // operations on 64-bit values, so decompose.

            return ((ulong)ReverseEndianness((uint)value) << 32)
                + ReverseEndianness((uint)(value >> 32));
        }

        #region other


        /// <summary>
        /// Rotates the specified value left by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROL.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]       
        public static uint RotateLeft(uint value, int offset)
            => (value << offset) | (value >> (32 - offset));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]     
        public static ulong RotateLeft(ulong value, int offset)
         => (value << offset) | (value >> (64 - offset));

        /// <summary>
        /// Rotates the specified value right by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROR.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..31] is treated as congruent mod 32.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]    
        public static uint RotateRight(uint value, int offset)
            => (value >> offset) | (value << (32 - offset));

        /// <summary>
        /// Rotates the specified value right by the specified number of bits.
        /// Similar in behavior to the x86 instruction ROR.
        /// </summary>
        /// <param name="value">The value to rotate.</param>
        /// <param name="offset">The number of bits to rotate by.
        /// Any value outside the range [0..63] is treated as congruent mod 64.</param>
        /// <returns>The rotated value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]     
        public static ulong RotateRight(ulong value, int offset)
            => (value >> offset) | (value << (64 - offset));
        #endregion
    }
}
