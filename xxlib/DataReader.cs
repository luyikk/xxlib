﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace xx
{
    public class DataReader
    {
        public static bool IsBigEndian { get; set; }

        Lazy<List<ISerde>> _idxStore = new Lazy<List<ISerde>>(System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        public List<ISerde> IdxStore => _idxStore.Value;

        byte[] buff;
        int len;
        int position;
        int offset;

        /// <summary>
        /// 长度
        /// </summary>
        public int Length => len - offset;

        public int BuffOffset => offset;

        public int Position => position;

        /// <summary>
        /// 写入偏移量
        /// </summary>
        public int Offset
        {
            get => position - offset;
            set
            {
                position = offset + value;
            }
        }


        public DataReader(byte[] data)
        {
            this.buff = data;
            this.len = data.Length;
        }

        public DataReader(byte[] data, int offset, int length)
        {
            this.buff = data;
            this.position = offset;
            this.offset = offset;
            this.len = offset + length;
        }

        #region Read
        /// <summary>
        /// 读 定长buf 到 tar. 返回非 0 则读取失败
        /// </summary>      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadBuf(byte[] data, int index, int length)
        {
            if (position + length > len) return -1;
            Buffer.BlockCopy(buff, position, data, index, length);
            position += length;
            return 0;
        }

        /// <summary>
        /// 从指定下标 读 定长buf. 不改变 offset. 返回非 0 则读取失败
        /// </summary>      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadBufAt(int idx, byte[] data, int index, int length)
        {
            if (idx + length > len) return -2;
            Buffer.BlockCopy(buff, idx, data, index, length);
            return 0;
        }

        /// <summary>
        /// 读取一个字节
        /// </summary>      
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFixed(out byte v)
        {
            if (position + sizeof(byte) > len)
            {
                v = 0;
                return -3;
            }

            v = buff[position];
            position += 1;
            return 0;
        }

        /// <summary>
        /// 读取一个sbyte
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFixed(out sbyte v)
        {
            byte b;
            var r = ReadFixed(out b);
            v = (sbyte)b;
            return r;
        }

        /// <summary>
        /// 读取一个BOOL
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFixed(out bool v)
        {
            byte b;
            var r = ReadFixed(out b);
            v = b == 1;
            return r;
        }

        /// <summary>
        /// 读取一个 u16
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFixed(out ushort v)
        {
            if (position + sizeof(ushort) > len)
            {
                v = 0;
                return -4;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[position]))
                {
                    v = *(ushort*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);
                    position += sizeof(ushort);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 i16
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFixed(out short v)
        {
            if (position + sizeof(short) > len)
            {
                v = 0;
                return -5;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[position]))
                {
                    v = *(short*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);
                    position += sizeof(short);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 u32
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFixed(out uint v)
        {
            if (position + sizeof(uint) > len)
            {
                v = 0;
                return -6;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[position]))
                {
                    v = *(uint*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);
                    position += sizeof(uint);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 i32
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFixed(out int v)
        {
            if (position + sizeof(int) > len)
            {
                v = 0;
                return -7;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[position]))
                {
                    v = *(int*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);
                    position += sizeof(int);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 u64
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFixed(out ulong v)
        {
            if (position + sizeof(ulong) > len)
            {
                v = 0;
                return -8;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[position]))
                {
                    v = *(ulong*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);
                    position += sizeof(ulong);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 i64
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFixed(out long v)
        {
            if (position + sizeof(long) > len)
            {
                v = 0;
                return -9;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[position]))
                {
                    v = *(long*)numRef;
                    if (IsBigEndian)
                        v = BinaryPrimitives.ReverseEndianness(v);
                    position += sizeof(long);
                    return 0;
                }
            }
        }


        /// <summary>
        /// 读取一个 float
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFixed(out float v)
        {
            if (position + sizeof(float) > len)
            {
                v = 0;
                return -10;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[position]))
                {
                    var x = *(uint*)numRef;
                    uint p = IsBigEndian ? BinaryPrimitives.ReverseEndianness(x) : x;
                    v = *((float*)&p);
                    position += sizeof(float);
                    return 0;
                }
            }
        }

        /// <summary>
        /// 读取一个 float
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFixed(out double v)
        {
            if (position + sizeof(float) > len)
            {
                v = 0;
                return -11;
            }

            unsafe
            {
                fixed (byte* numRef = &(buff[position]))
                {
                    var x = *(ulong*)numRef;
                    ulong p = IsBigEndian ? BinaryPrimitives.ReverseEndianness(x) : x;
                    v = *(double*)&p;
                    position += sizeof(double);
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
        public int ReadFixedAt(int idx, out byte v)
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
        public int ReadFixedAt(int idx, out sbyte v)
        {
            byte b;
            var r = ReadFixedAt(idx, out b);
            v = (sbyte)b;
            return r;
        }

        /// <summary>
        /// 读取一个BOOL
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFixedAt(int idx, out bool v)
        {
            byte b;
            var r = ReadFixedAt(idx, out b);
            v = b == 1;
            return r;
        }


        /// <summary>
        /// 读取一个 u16
        /// </summary>    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadFixedAt(int idx, out ushort v)
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
        public int ReadFixedAt(int idx, out short v)
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
        public int ReadFixedAt(int idx, out uint v)
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
        public int ReadFixedAt(int idx, out int v)
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
        public int ReadFixedAt(int idx, out ulong v)
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
        public int ReadFixedAt(int idx, out long v)
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
        public int ReadFixedAt(int idx, out float v)
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
        public int ReadFixedAt(int idx, out double v)
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
        public int ReadVarInteger(out ushort v)
        {
            v = 0;
            for (int shift = 0; shift < sizeof(ushort) * 8; shift += 7)
            {
                if (position == len)
                    return -201;
                uint b = buff[position++];
                v |= (ushort)((b & 0x7Fu) << shift);
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
                if (position == len)
                    return -211;
                uint b = buff[position++];
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
                if (position == len)
                    return -221;
                ulong b = buff[position++];
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
                    if (len > this.len - position)
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short ZigZagDecode(ushort v) { return (short)((short)(v >> 1) ^ (-(short)(v & 1))); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ZigZagDecode(uint v) { return (int)(v >> 1) ^ (-(int)(v & 1)); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ZigZagDecode(ulong v) { return (long)(v >> 1) ^ (-(long)(v & 1)); }
    }
}
