using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace xx
{
    public class DataReader
    {
        public static bool IsBigEndian { get; set; }

        byte[] buff;     
        int len;
        int offset;


        /// <summary>
        /// 长度
        /// </summary>
        public int Length => len;

        /// <summary>
        /// 写入偏移量
        /// </summary>
        public int Offset
        {
            get => offset;
            set
            {
                offset = value;
            }
        }


        public DataReader(byte[] data)
        {            
            this.buff = data;
            this.len = data.Length;
        }

        public DataReader(byte[] data,int length)
        {
            this.buff = data;
            this.len = length;
        }

        #region Read
        /// <summary>
        /// 读 定长buf 到 tar. 返回非 0 则读取失败
        /// </summary>      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadBuf(byte[] data, int index, int length)
        {
            if (offset + length > len) return -1;
            Buffer.BlockCopy(buff, offset, data, index, length);
            offset += length;
            return 0;
        }

        /// <summary>
        /// 从指定下标 读 定长buf. 不改变 offset. 返回非 0 则读取失败
        /// </summary>      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadBufAt(int idx,byte[] data,int index,int length)
        {
            if (idx + length > len) return -2;
            Buffer.BlockCopy(buff, idx, data, index, length);
            return 0;
        }

        /// <summary>
        /// 读取一个字节
        /// </summary>      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexd(out byte v)
        {
            if (offset + sizeof(byte) > len)
            {
                v = 0;
                return -3;
            }

            v = buff[offset];
            offset += 1;
            return 0;
        }

        /// <summary>
        /// 读取一个sbyte
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexd(out sbyte v)
        {
            byte b;
            var r= ReadFiexd(out b);
            v = (sbyte)b;
            return r;
        }

        /// <summary>
        /// 读取一个BOOL
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexd(out bool v)
        {
            byte b;
            var r = ReadFiexd(out b);
            v = b == 1;
            return r;
        }

        /// <summary>
        /// 读取一个 u16
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexd(out ushort v)
        {
            if (offset + sizeof(ushort) > len)
            {
                v = 0;
                return -4;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[offset]))
                {
                    v = *(ushort*)numRef;
                    if (IsBigEndian)
                        v =BinaryPrimitives.ReverseEndianness(v);
                    offset += sizeof(ushort);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 i16
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexd(out short v)
        {
            if (offset + sizeof(short) > len)
            {
                v = 0;
                return -5;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[offset]))
                {
                    v = *(short*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);
                    offset += sizeof(short);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 u32
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexd(out uint v)
        {
            if (offset + sizeof(uint) > len)
            {
                v = 0;
                return -6;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[offset]))
                {
                    v = *(uint*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);
                    offset += sizeof(uint);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 i32
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexd(out int v)
        {
            if (offset + sizeof(int) > len)
            {
                v = 0;
                return -7;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[offset]))
                {
                    v = *(int*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);
                    offset += sizeof(int);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 u64
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexd(out ulong v)
        {
            if (offset + sizeof(ulong) > len)
            {
                v = 0;
                return -8;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[offset]))
                {
                    v = *(ulong*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);
                    offset += sizeof(ulong);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 i64
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexd(out long v)
        {
            if (offset + sizeof(long) > len)
            {
                v = 0;
                return -9;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[offset]))
                {
                    v = *(long*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);
                    offset += sizeof(long);
                    return 0;
                }
            }
        }


        /// <summary>
        /// 读取一个 float
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexd(out float v)
        {
            if (offset + sizeof(float) > len)
            {
                v = 0;
                return -10;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[offset]))
                {
                    var x = *(uint*)numRef;
                    uint p = IsBigEndian ? BinaryPrimitives.ReverseEndianness(x) : x;
                    v= *((float*)&p); 
                    offset += sizeof(float);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 float
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexd(out double v)
        {
            if (offset + sizeof(float) > len)
            {
                v = 0;
                return -11;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[offset]))
                {
                    var x = *(ulong*)numRef;
                    ulong p = IsBigEndian ? BinaryPrimitives.ReverseEndianness(x) : x;
                    v= *(double*)&p;
                    offset += sizeof(double);
                    return 0;
                }
            }
        }




        #endregion

        #region ReadAt

        /// <summary>
        /// 读取一个字节
        /// </summary>      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexdAt(int idx,out byte v)
        {
            if (idx + sizeof(byte) > len)
            {
                v = 0;
                return -103;
            }

            v = buff[idx];           
            return 0;
        }


        /// <summary>
        /// 读取一个sbyte
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexdAt(int idx, out sbyte v)
        {
            byte b;
            var r = ReadFiexdAt(idx,out b);
            v = (sbyte)b;
            return r;
        }

        /// <summary>
        /// 读取一个BOOL
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexdAt(int idx,out bool v)
        {
            byte b;
            var r = ReadFiexdAt(idx,out b);
            v = b == 1;
            return r;
        }


        /// <summary>
        /// 读取一个 u16
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexdAt(int idx,out ushort v)
        {
            if (idx + sizeof(ushort) > len)
            {
                v = 0;
                return -104;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[idx]))
                {
                    v = *(ushort*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);                    
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 i16
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexdAt(int idx,out short v)
        {
            if (idx + sizeof(short) > len)
            {
                v = 0;
                return -105;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[idx]))
                {
                    v = *(short*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);                   
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 u32
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexdAt(int idx, out uint v)
        {
            if (idx + sizeof(uint) > len)
            {
                v = 0;
                return -106;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[idx]))
                {
                    v = *(uint*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);                   
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 i32
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexdAt(int idx, out int v)
        {
            if (idx + sizeof(int) > len)
            {
                v = 0;
                return -107;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[idx]))
                {
                    v = *(int*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);                 
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 u64
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexdAt(int idx, out ulong v)
        {
            if (idx + sizeof(ulong) > len)
            {
                v = 0;
                return -108;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[idx]))
                {
                    v = *(ulong*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);                   
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 i64
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexdAt(int idx, out long v)
        {
            if (idx + sizeof(long) > len)
            {
                v = 0;
                return -109;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[idx]))
                {
                    v = *(long*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);                   
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 float
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexdAt(int idx, out float v)
        {
            if (idx + sizeof(float) > len)
            {
                v = 0;
                return -110;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[idx]))
                {
                    v = *(float*)numRef;                   
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 float
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFiexdAt(int idx, out double v)
        {
            if (idx + sizeof(float) > len)
            {
                v = 0;
                return -111;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[idx]))
                {
                    v = *(double*)numRef;                   
                    return 0;
                }
            }
        }
        #endregion

        #region ReadVar
        /// <summary>
        /// 读取变长 u16
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public  int ReadVarInteger(out ushort v)
        {
            v = 0;
            for (int shift = 0; shift < sizeof(ushort)*8; shift+=7)
            {
                if (offset == len)
                    return -201;
                uint b = buff[offset++];
                v|=(ushort)((b & 0x7Fu) << shift);
                if ((b & 0x80) == 0)                
                    return 0;                
            }

            return -202;
        }

        /// <summary>
        /// 读取变长 i64
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadVarInteger(out short v)
        {          
            var r = ReadVarInteger(out ushort b);
            v = ZigZagDecode(b);
            return r;
        }

        /// <summary>
        /// 读取变长 u32
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadVarInteger(out uint v)
        {
            v = 0;
            for (int shift = 0; shift < sizeof(uint) * 8; shift += 7)
            {
                if (offset == len)
                    return -211;
                uint b = buff[offset++];
                v |= ((b & 0x7Fu) << shift);
                if ((b & 0x80) == 0)
                    return 0;
            }

            return -212;
        }


        /// <summary>
        /// 读取变长 i32
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadVarInteger(out int v)
        {
            var r = ReadVarInteger(out uint b);
            v = ZigZagDecode(b);
            return r;
        }


        /// <summary>
        /// 读取变长 u64
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadVarInteger(out ulong v)
        {
            v = 0;
            for (int shift = 0; shift < sizeof(ulong) * 8; shift += 7)
            {
                if (offset == len)
                    return -221;
                ulong b = buff[offset++];
                v |= ((b & 0x7Fu) << shift);
                if ((b & 0x80) == 0)
                    return 0;
            }

            return -222;
        }


        /// <summary>
        /// 读取变长 i64
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadVarInteger(out long v)
        {
            var r = ReadVarInteger(out ulong b);
            v = ZigZagDecode(b);
            return r;
        }

        /// <summary>
        /// 读取变长字符串
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadVarInteger(out string v)
        {
            v = "";

            if (ReadVarInteger(out uint len) == 0)
            {
                if (len > 0)
                {
                    if (len > this.len - offset)
                        return -333;

                    byte[] data = new byte[len];
                    if (ReadBuf(data, 0, data.Length) == 0)
                    {
                        v = System.Text.Encoding.UTF8.GetString(data);
                        return 0;
                    }
                }
                else
                {                  
                    return 0;
                }
            }           
            return -300;
        }


        #endregion

        /// <summary>
        /// 下标访问
        /// </summary>       
        public byte this[int index] => buff[index];

        // 等效代码： if( (v & 1) > 0 ) return -(v + 1) / 2; else return v / 2;

        public static short ZigZagDecode(ushort v) { return (short)((short)(v >> 1) ^ (-(short)(v & 1))); }

        public static int ZigZagDecode(uint v) { return (int)(v >> 1) ^ (-(int)(v & 1)); }

        public static long ZigZagDecode(ulong v) { return (long)(v >> 1) ^ (-(long)(v & 1)); }
    }
}
