using System;
using Xunit;

namespace xxlibtest
{
    public class TestData
    {
        [Fact]
        public void test_fill()
        {
            using var data = new xx.Data();
            data.Fill(new byte[] { 1, 2, 3, 4 });
        
            Assert.True(data.Length == 4);

            for (int i = 0; i < 4; i++)            
                Assert.True(data[i] == i + 1);

         

            data.WriteBuf(new byte[] { 5, 6, 7, 8 }, 0, 4);         
            Assert.True(data.Length == 8);

            for (int i = 0; i < 8; i++)
                Assert.True(data[i] == i + 1);
        }




        [Fact]
        public void test_write_buff()
        {
            {
                using var data = new xx.Data(cap: 0);
                data.WriteBuf(new byte[] { 1, 2, 3, 4 }, 0, 4);
             
                Assert.True(data.Length == 4);              
               
                for (int i = 0; i < 4; i++)
                    Assert.True(data[i] == i + 1);

                data.WriteBuf(new byte[] { 5, 6, 7, 8 }, 0, 4);               
                Assert.True(data.Length == 8);
               
                for (int i = 0; i < 8; i++)
                    Assert.True(data[i] == i + 1);

                data.WriteFiexd((byte)9);
                data.WriteFiexd((byte)10);
                data.WriteFiexd((byte)11);


                for (int i = 0; i < 11; i++)
                    Assert.True(data[i] == i + 1);

                var buff = data.ToData();

                for (int i = 0; i < 11; i++)
                    Assert.True(buff[i] == i + 1);
            }
            {
                using var data = new xx.Data(cap: 200);  
                data.WriteBuf(new byte[] { 1, 2, 3, 4 }, 0, 4);                
                Assert.True(data.Length == 4);              
                for (int i = 0; i < 4; i++)
                    Assert.True(data[i] == i + 1);

                data.WriteBuf(new byte[] { 5, 6, 7, 8 }, 0, 4);              
                Assert.True(data.Length == 8);
              
                for (int i = 0; i < 8; i++)
                    Assert.True(data[i] == i + 1);
            }

         
        }


        [Fact]
        public void test_write_fixed()
        {
            {
                using var data = new xx.Data();
                data.WriteFiexd((byte)1);
                data.WriteFiexd((byte)2);
                data.WriteFiexd((byte)3);
                data.WriteFiexd((byte)4);
                Assert.True(data.Length == 4);

                for (int i = 0; i < 4; i++)
                    Assert.True(data[i] == i + 1);

                data.WriteFiexd((sbyte)5);
                data.WriteFiexd((sbyte)6);
                data.WriteFiexd((sbyte)7);
                data.WriteFiexd((sbyte)8);

                for (int i = 0; i < 8; i++)
                    Assert.True(data[i] == i + 1);

            }

            {
                using var data = new xx.Data();
                data.WriteFiexd(ushort.MaxValue);

                Assert.True(data[0] ==255);
                Assert.True(data[1] == 255);
                Assert.True(data.Length == 2);
            }
            {
                using var data = new xx.Data();
                data.WriteFiexd(short.MaxValue);

                Assert.True(data[0] == 255);
                Assert.True(data[1] == 127);
                Assert.True(data.Length == 2);
            }

            {
                using var data = new xx.Data();
                data.WriteFiexd(uint.MaxValue);

                Assert.True(data[0] == 255);
                Assert.True(data[1] == 255);
                Assert.True(data[2] == 255);
                Assert.True(data[3] == 255);
                Assert.True(data.Length == 4);
            }
            {
                using var data = new xx.Data();
                data.WriteFiexd(int.MaxValue);

                Assert.True(data[0] == 255);
                Assert.True(data[1] == 255);
                Assert.True(data[2] == 255);
                Assert.True(data[3] == 127);
                Assert.True(data.Length == 4);
            }


            {
                using var data = new xx.Data();
                data.WriteFiexd(ulong.MaxValue);

                Assert.True(data[0] == 255);
                Assert.True(data[1] == 255);
                Assert.True(data[2] == 255);
                Assert.True(data[3] == 255);
                Assert.True(data[4] == 255);
                Assert.True(data[5] == 255);
                Assert.True(data[6] == 255);
                Assert.True(data[7] == 255);
                Assert.True(data.Length == 8);
            }
            {
                using var data = new xx.Data();
                data.WriteFiexd(long.MaxValue);

                Assert.True(data[0] == 255);
                Assert.True(data[1] == 255);
                Assert.True(data[2] == 255);
                Assert.True(data[3] == 255);
                Assert.True(data[4] == 255);
                Assert.True(data[5] == 255);
                Assert.True(data[6] == 255);
                Assert.True(data[7] == 127);
                Assert.True(data.Length == 8);
            }
            {
                using var data = new xx.Data();

                for (int i = 0; i < 10000; i++)
                {
                    data.WriteFiexd(int.MaxValue);
                }

                var buff = data.ToArray();           

            }

        }

        [Fact]
        public void test_write_fixed_at()
        {
            {
                using var data = new xx.Data();
                data.Fill( new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
                data.WriteBufAt(2, new byte[] { 1, 2, 3, 4 });
                Assert.True(data.Length == 8);
                data.WriteFiexdAt(8, uint.MaxValue);
                Assert.True(data.Length == 12);

                var buff = data.ToData();
                Assert.Equal(buff, new byte[] { 0, 0, 1, 2, 3, 4, 0, 0,255,255,255,255});

            }
            

            {
                using var data = new xx.Data(cap:2);
                data.WriteBufAt(300, new byte[] { 1, 2, 3, 4 });
                Assert.True(data.Length == 304);
                Assert.True(data[300] == 1);
                Assert.True(data[301] == 2);
                Assert.True(data[302] == 3);
                Assert.True(data[303] == 4);
            }
          
            {
                using var data = new xx.Data();
                data.Fill(new byte[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 });
                data.WriteFiexdAt(0, byte.MaxValue);
                data.WriteFiexdAt(1, ushort.MaxValue);
                data.WriteFiexdAt(3, uint.MaxValue);
                data.WriteFiexdAt(7, ulong.MaxValue);
                Assert.True(data.Length == 15);
                var buff = data.ToData();
                Assert.Equal(buff, new byte[] { 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255, 255,255,255,255 });
            }
           
        }


        [Fact]
        public void test_write_var()
        {
            {
                using var data = new xx.Data();
                data.WriteVarInteger((ulong)10000000);
                data.WriteVarInteger((uint)10000000);
                data.WriteVarInteger((ulong)10000000);
                var len = xx.Data.ComputeRawVarint64Size(10000000);
                Assert.True(data.Length == len*3);
            }

            {
                using var data = new xx.Data();
                data.WriteVarInteger((long)10000000);
                data.WriteVarInteger((int)10000000);
                data.WriteVarInteger((long)10000000);
                var len= xx.Data.ComputeRawVarint64Size(xx.Data.ZigZagEncode(10000000));
                Assert.True(data.Length == len*3);
            }
        }

        [Fact]
        public void test_read_buff()
        {
            {
                var data = new xx.DataReader(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 });
                var buff = new byte[100];
                Assert.True(data.ReadBuf(buff, 0, 10) == 0);
            }
            {
                using var data = new xx.Data();
                data.WriteFiexd((byte)1);
                data.WriteFiexd((sbyte)2);
                data.WriteFiexd((short)3);
                data.WriteFiexd((ushort)4);
                data.WriteFiexd((int)5);
                data.WriteFiexd((uint)6);
                data.WriteFiexd((long)7);
                data.WriteFiexd((ulong)8);
                data.WriteFiexd(true);
                data.WriteFiexd(false);
                data.WriteFiexd(0.5f);
                data.WriteFiexd(0.55);

                var (buff, len) = data.ToArray();
                var read = new xx.DataReader(buff, len);
                Assert.True(read.ReadFiexd(out byte a) == 0);
                Assert.Equal(1, a);

                Assert.True(read.ReadFiexd(out sbyte b) == 0);
                Assert.Equal(2, b);

                Assert.True(read.ReadFiexd(out short c) == 0);
                Assert.Equal(3, c);

                Assert.True(read.ReadFiexd(out ushort d) == 0);
                Assert.Equal(4, d);

                Assert.True(read.ReadFiexd(out int e) == 0);
                Assert.Equal(5, e);

                Assert.True(read.ReadFiexd(out uint f) == 0);
                Assert.Equal((uint)6, f);

                Assert.True(read.ReadFiexd(out long g) == 0);
                Assert.Equal(7, g);

                Assert.True(read.ReadFiexd(out ulong h) == 0);
                Assert.Equal((ulong)8, h);

                Assert.True(read.ReadFiexd(out bool i) == 0);
                Assert.True(i);

                Assert.True(read.ReadFiexd(out bool j) == 0);
                Assert.False(j);

                Assert.True(read.ReadFiexd(out float k) == 0);
                Assert.Equal(0.5f, k);

                Assert.True(read.ReadFiexd(out double l) == 0);
                Assert.Equal(0.55, l);

            }
            {
                using var data = new xx.Data();
                data.WriteFiexd((byte)1);
                data.WriteFiexd((sbyte)2);
                data.WriteFiexd((short)3);
                data.WriteFiexd((ushort)4);
                data.WriteFiexd((int)5);
                data.WriteFiexd((uint)6);
                data.WriteFiexd((long)7);
                data.WriteFiexd((ulong)8);
                data.WriteFiexd(true);
                data.WriteFiexd(false);
                data.WriteFiexd(0.5f);
                data.WriteFiexd(0.55);

                var (buff, len) = data.ToArray();
                var read = new xx.DataReader(buff, len);
                Assert.True(read.ReadFiexdAt(0,out byte a) == 0);
                Assert.Equal(1, a);

                Assert.True(read.ReadFiexdAt(1,out sbyte b) == 0);
                Assert.Equal(2, b);

                Assert.True(read.ReadFiexdAt(2,out short c) == 0);
                Assert.Equal(3, c);

                Assert.True(read.ReadFiexdAt(4,out ushort d) == 0);
                Assert.Equal(4, d);

                Assert.True(read.ReadFiexdAt(6,out int e) == 0);
                Assert.Equal(5, e);

                Assert.True(read.ReadFiexdAt(10,out uint f) == 0);
                Assert.Equal((uint)6, f);

                Assert.True(read.ReadFiexdAt(14,out long g) == 0);
                Assert.Equal(7, g);

                Assert.True(read.ReadFiexdAt(22,out ulong h) == 0);
                Assert.Equal((ulong)8, h);

                Assert.True(read.ReadFiexdAt(30,out bool i) == 0);
                Assert.True(i);

                Assert.True(read.ReadFiexdAt(31,out bool j) == 0);
                Assert.False(j);

                Assert.True(read.ReadFiexdAt(32,out float k) == 0);
                Assert.Equal(0.5f, k);

                Assert.True(read.ReadFiexdAt(36,out double l) == 0);
                Assert.Equal(0.55, l);

                Assert.True(read.Offset == 0);
            }
            {
                using var data = new xx.Data();
                data.WriteVarInteger((short)1);
                data.WriteVarInteger((ushort)2);
                data.WriteVarInteger((int)3);
                data.WriteVarInteger((uint)4);
                data.WriteVarInteger((long)6);
                data.WriteVarInteger((ulong)200);
                data.WriteVarInteger((int)5);
                data.WriteBuf(new byte[] { 1, 2, 3, 4, 5 },0,5);

                var (buff, len) = data.ToArray();
                var read = new xx.DataReader(buff, len);
                Assert.True(read.ReadVarInteger(out short a) == 0);
                Assert.Equal(1, a);

                Assert.True(read.ReadVarInteger(out ushort b) == 0);
                Assert.Equal(2, b);

                Assert.True(read.ReadVarInteger(out int c) == 0);
                Assert.Equal(3, c);

                Assert.True(read.ReadVarInteger(out uint d) == 0);
                Assert.Equal((uint)4, d);

                Assert.True(read.ReadVarInteger(out long e) == 0);
                Assert.Equal(6, e);

                Assert.True(read.ReadVarInteger(out ulong f) == 0);
                Assert.Equal((ulong)200, f);

                Assert.True(read.ReadVarInteger(out int g) == 0);
                Assert.Equal(5, g);

            }
            {
                using var data = new xx.Data();
                data.WriteVarInteger("123123123");
                data.WriteVarInteger("321321321");
                data.WriteVarInteger("");
                var (buff, len) = data.ToArray();
                var read = new xx.DataReader(buff, len);
                Assert.True(read.ReadVarInteger(out string v) == 0);
                Assert.Equal("123123123", v);

                Assert.True(read.ReadVarInteger(out string v2) == 0);
                Assert.Equal("321321321", v2);

                Assert.True(read.ReadVarInteger(out string v3) == 0);
                Assert.Equal("", v3);
            }
            {

                using var data = new xx.Data();
                for (int i = 0; i < 100000; i++)                
                    data.WriteFiexd(i);

                var (buff, len) = data.ToArray();
                var read = new xx.DataReader(buff, len);

                for (int i = 0; i < 100000; i++)
                {
                    Assert.True(read.ReadFiexd(out int v) == 0);
                    Assert.Equal(i, v);
                }

            }
        }
    }
}
