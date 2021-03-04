using System;
using xx;

namespace xxlib
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                var data = new Data();
                data.Fill(new byte[] { 1, 2, 3, 4, 5, 6 });
                foreach (var p in data)
                {
                    Console.WriteLine(p);
                }

                for (int i = 0; i < 6; i++)
                {
                    Console.WriteLine(data[i]);
                }
            }
            {
                var data = new Data(reserveLen:100);
                data.Fill(new byte[] { 1, 2, 3, 4, 5, 6 });
                foreach (var p in data)
                {
                    Console.WriteLine(p);
                }

                for (int i = 0; i < 6; i++)
                {
                    Console.WriteLine(data[i]);
                }

            }

            {
                {
                    var data = new Data();
                    data.Fill(new byte[] { 1, 2, 3, 4 });
                    data.WriteBuf(new byte[] { 5, 6, 7, 8 }, 0, 4);
                    foreach (var p in data)
                        Console.WriteLine(p);
                }
                {
                    var data = new Data(cap:2);                 
                    data.WriteBuf(new byte[] { 5, 6, 7, 8 }, 0, 4);
                    foreach (var p in data)
                        Console.WriteLine(p);
                }

            }
        }
    }
}
