using System;
using System.Collections.Generic;
using System.Text;

namespace xx
{

    public interface ISerde
    {
        ushort GetTypeid();
        void Write(ObjManager om, Data data);
        int Read(ObjManager om, DataReader data);
    }

    public delegate ISerde TypeIdCreatorFunc();

    public class ObjManager
    {
        static TypeIdCreatorFunc[] typeIdCreatorMappings = new TypeIdCreatorFunc[ushort.MaxValue + 1];
        static Dictionary<Type, ushort> typeTypeIdMappings = new Dictionary<Type, ushort>();

        public static void Register<T>(ushort typeid) where T : ISerde, new()
        {
            if (typeTypeIdMappings.ContainsKey(typeof(T)))
                throw new IndexOutOfRangeException($"repeat typeid:{typeid} check [TypeId]");
            typeIdCreatorMappings[typeid] = () => { return new T(); };
        }

        public static ISerde Create(int typeid) => typeIdCreatorMappings[typeid]();

        Lazy<Dictionary<ISerde, uint>> _ptrStore = new Lazy<Dictionary<ISerde, uint>>(System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        Lazy<List<ISerde>> _idxStore = new Lazy<List<ISerde>>(System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        public Dictionary<ISerde, uint> PtrStore => _ptrStore.Value;
        public List<ISerde> IdxStore => _idxStore.Value;

        #region WriteBase
        public void WriteTo(Data data, byte b)        
           => data.WriteFiexd(b);        

        public void WriteTo(Data data, sbyte b)        
           => data.WriteFiexd(b);        

        public void WriteTo(Data data, bool b)        
           => data.WriteFiexd(b);        

        public void WriteTo(Data data, short b)        
           => data.WriteVarInteger(b);        

        public void WriteTo(Data data, ushort b)        
           => data.WriteVarInteger(b);        

        public void WriteTo(Data data, int b)        
           => data.WriteVarInteger(b);        

        public void WriteTo(Data data, uint b)        
           =>data.WriteVarInteger(b);        

        public void WriteTo(Data data, ulong b)        
            =>data.WriteVarInteger(b);    

        public void WriteTo(Data data, long b)        
            =>data.WriteVarInteger(b);      

        public void WriteTo(Data data, float b)        
           => data.WriteFiexd(b);

        public void WriteTo(Data data, double b)        
           => data.WriteFiexd(b);

        public void WriteTo(Data data, byte? b)
        {
            if (b.HasValue)
            {
                data.WriteFiexd((byte)1);
                data.WriteFiexd(b.Value);
            }
            else
                data.WriteFiexd((byte)0);
        }

        public void WriteTo(Data data, sbyte? b)
        {
            if (b.HasValue)
            {
                data.WriteFiexd((byte)1);
                data.WriteFiexd(b.Value);
            }
            else
                data.WriteFiexd((byte)0);
        }

        public void WriteTo(Data data, bool? b)
        {
            if (b.HasValue)
            {
                data.WriteFiexd((byte)1);
                data.WriteFiexd(b.Value);
            }
            else
                data.WriteFiexd((byte)0);
        }

        public void WriteTo(Data data, short? b)
        {
            if (b.HasValue)
            {
                data.WriteFiexd((byte)1);
                data.WriteVarInteger(b.Value);
            }
            else
                data.WriteFiexd((byte)0);
        }

        public void WriteTo(Data data, ushort? b)
        {
            if (b.HasValue)
            {
                data.WriteFiexd((byte)1);
                data.WriteVarInteger(b.Value);
            }
            else
                data.WriteFiexd((byte)0);
        }

        public void WriteTo(Data data, int? b)
        {
            if (b.HasValue)
            {
                data.WriteFiexd((byte)1);
                data.WriteVarInteger(b.Value);
            }
            else
                data.WriteFiexd((byte)0);
        }

        public void WriteTo(Data data, uint? b)
        {
            if (b.HasValue)
            {
                data.WriteFiexd((byte)1);
                data.WriteVarInteger(b.Value);
            }
            else
                data.WriteFiexd((byte)0);
        }

        public void WriteTo(Data data, ulong? b)
        {
            if (b.HasValue)
            {
                data.WriteFiexd((byte)1);
                data.WriteVarInteger(b.Value);
            }
            else
                data.WriteFiexd((byte)0);
        }

        public void WriteTo(Data data, long? b)
        {
            if (b.HasValue)
            {
                data.WriteFiexd((byte)1);
                data.WriteVarInteger(b.Value);
            }
            else
                data.WriteFiexd((byte)0);
        }
      

        public void WriteTo(Data data, float? b)
        {
            if (b.HasValue)
            {
                data.WriteFiexd((byte)1);
                data.WriteFiexd(b.Value);
            }
            else
                data.WriteFiexd((byte)0);
        }

        public void WriteTo(Data data, double? b)
        {
            if (b.HasValue)
            {
                data.WriteFiexd((byte)1);
                data.WriteFiexd(b.Value);
            }
            else
                data.WriteFiexd((byte)0);
        }

        public void WriteTo(Data data, string b)
        {
            b = b ?? "";
            data.WriteVarInteger(b);
        }

        #endregion

        #region WriteClass
        public void WriteTo<T>(Data data,T v) where T : class,ISerde,new()
        {
            this.PtrStore.Clear();
            WriteObj(data, v);
        }


        public void WriteObj<T>(Data data, T v) where T : class, ISerde, new()
        {
            if (v is null)
                data.WriteFiexd((byte)0);
            else
            {
                if (!PtrStore.TryGetValue(v, out var offset))
                {
                    var typeid = v.GetTypeid();
                    offset = Convert.ToUInt32(PtrStore.Count + 1);
                    PtrStore.Add(v, offset);
                    data.WriteVarInteger(offset);
                    data.WriteVarInteger(typeid);
                    v.Write(this, data);
                }
                else
                    data.WriteVarInteger(offset);
            }
        }
        #endregion

        #region WriteArray
        public void WriteTo(Data data,byte[] v)
        {
            data.WriteVarInteger((uint)v.Length);
            data.WriteBuf(v);
        }

        public void WriteTo<T>(Data data,T[] v) where T : class, ISerde, new()
        {
            this.PtrStore.Clear();

            data.WriteVarInteger((uint)v.Length);
            foreach (var p in v)
                WriteObj(data, p);
        }


        #endregion

        #region WriteList

        public void WriteTo<S>(Data data,List<S> v) where S:class,ISerde,new()
        {
            this.PtrStore.Clear();

            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                WriteObj(data, p);           
        }

        #region ListBase

        public void WriteTo(Data data, List<byte> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                data.WriteFiexd(p);
        }

        public void WriteTo(Data data, List<sbyte> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                data.WriteFiexd(p);
        }

        public void WriteTo(Data data, List<bool> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                data.WriteFiexd(p);
        }

        public void WriteTo(Data data, List<short> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                data.WriteVarInteger(p);
        }

        public void WriteTo(Data data, List<ushort> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                data.WriteVarInteger(p);
        }

        public void WriteTo(Data data, List<int> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                data.WriteVarInteger(p);
        }

        public void WriteTo(Data data, List<uint> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                data.WriteVarInteger(p);
        }

        public void WriteTo(Data data, List<long> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                data.WriteVarInteger(p);
        }

        public void WriteTo(Data data, List<ulong> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                data.WriteVarInteger(p);
        }

        public void WriteTo(Data data, List<double> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                data.WriteFiexd(p);
        }

        public void WriteTo(Data data, List<float> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                data.WriteFiexd(p);
        }

        public void WriteTo(Data data, List<string> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                this.WriteTo(data, p);
        }

        #endregion

        #region ListBase_Option

        public void WriteTo(Data data, List<byte?> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                WriteTo(data,p);
        }

        public void WriteTo(Data data, List<sbyte?> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                WriteTo(data, p);
        }

        public void WriteTo(Data data, List<bool?> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                WriteTo(data, p);
        }

        public void WriteTo(Data data, List<short?> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                WriteTo(data, p);
        }

        public void WriteTo(Data data, List<ushort?> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                WriteTo(data, p);
        }

        public void WriteTo(Data data, List<int?> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                WriteTo(data, p);
        }

        public void WriteTo(Data data, List<uint?> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                WriteTo(data, p);
        }

        public void WriteTo(Data data, List<long?> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                WriteTo(data, p);
        }

        public void WriteTo(Data data, List<ulong?> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                WriteTo(data, p);
        }

        public void WriteTo(Data data, List<double?> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                WriteTo(data, p);
        }

        public void WriteTo(Data data, List<float?> v)
        {
            data.WriteVarInteger((uint)v.Count);
            foreach (var p in v)
                WriteTo(data, p);
        }

        #endregion
        #endregion

        #region ReadBase
        public int ReadFrom(DataReader data, out byte v)
            => data.ReadFiexd(out v);

        public int ReadFrom(DataReader data, out sbyte v)
          => data.ReadFiexd(out v);

        public int ReadFrom(DataReader data, out bool v)
          => data.ReadFiexd(out v);

        public int ReadFrom(DataReader data, out short v)
          => data.ReadVarInteger(out v);

        public int ReadFrom(DataReader data, out ushort v)
          => data.ReadVarInteger(out v);

        public int ReadFrom(DataReader data, out int v)
          => data.ReadVarInteger(out v);

        public int ReadFrom(DataReader data, out uint v)
          => data.ReadVarInteger(out v);

        public int ReadFrom(DataReader data, out long v)
          => data.ReadVarInteger(out v);

        public int ReadFrom(DataReader data, out ulong v)
          => data.ReadVarInteger(out v);

        public int ReadFrom(DataReader data, out float v)
          => data.ReadFiexd(out v);

        public int ReadFrom(DataReader data, out double v)
          => data.ReadFiexd(out v);

        public int ReadFrom(DataReader data, out string v)        
          => data.ReadVarInteger(out v);

        public int ReadFrom(DataReader data, out byte? v)
        {
            var err = data.ReadFiexd(out byte have);
            if (err == 0)
            {
                if (have == 1)
                {
                    var r = data.ReadFiexd(out byte b);
                    v = b;
                    return r;
                }
                else
                {
                    v = null;
                    return 0;
                }
            }
            else
            {
                v = null;
                return err;
            }
        }

        public int ReadFrom(DataReader data, out sbyte? v)
        {
            var err = data.ReadFiexd(out byte have);
            if (err == 0)
            {
                if (have == 1)
                {
                    var r = data.ReadFiexd(out sbyte b);
                    v = b;
                    return r;
                }
                else
                {
                    v = null;
                    return 0;
                }
            }
            else
            {
                v = null;
                return err;
            }
        }

        public int ReadFrom(DataReader data, out bool? v)
        {
            var err = data.ReadFiexd(out byte have);
            if (err == 0)
            {
                if (have == 1)
                {
                    var r = data.ReadFiexd(out bool b);
                    v = b;
                    return r;
                }
                else
                {
                    v = null;
                    return 0;
                }
            }
            else
            {
                v = null;
                return err;
            }
        }

        public int ReadFrom(DataReader data, out short? v)
        {
            var err = data.ReadFiexd(out byte have);
            if (err == 0)
            {
                if (have == 1)
                {
                    var r = data.ReadVarInteger(out short b);
                    v = b;
                    return r;
                }
                else
                {
                    v = null;
                    return 0;
                }
            }
            else
            {
                v = null;
                return err;
            }
        }

        public int ReadFrom(DataReader data, out ushort? v)
        {
            var err = data.ReadFiexd(out byte have);
            if (err == 0)
            {
                if (have == 1)
                {
                    var r = data.ReadVarInteger(out ushort b);
                    v = b;
                    return r;
                }
                else
                {
                    v = null;
                    return 0;
                }
            }
            else
            {
                v = null;
                return err;
            }
        }

        public int ReadFrom(DataReader data, out int? v)
        {
            var err = data.ReadFiexd(out byte have);
            if (err == 0)
            {
                if (have == 1)
                {
                    var r = data.ReadVarInteger(out int b);
                    v = b;
                    return r;
                }
                else
                {
                    v = null;
                    return 0;
                }
            }
            else
            {
                v = null;
                return err;
            }
        }

        public int ReadFrom(DataReader data, out uint? v)
        {
            var err = data.ReadFiexd(out byte have);
            if (err == 0)
            {
                if (have == 1)
                {
                    var r = data.ReadVarInteger(out uint b);
                    v = b;
                    return r;
                }
                else
                {
                    v = null;
                    return 0;
                }
            }
            else
            {
                v = null;
                return err;
            }
        }

        public int ReadFrom(DataReader data, out long? v)
        {
            var err = data.ReadFiexd(out byte have);
            if (err == 0)
            {
                if (have == 1)
                {
                    var r = data.ReadVarInteger(out long b);
                    v = b;
                    return r;
                }
                else
                {
                    v = null;
                    return 0;
                }
            }
            else
            {
                v = null;
                return err;
            }
        }

        public int ReadFrom(DataReader data, out ulong? v)
        {
            var err = data.ReadFiexd(out byte have);
            if (err == 0)
            {
                if (have == 1)
                {
                    var r = data.ReadVarInteger(out ulong b);
                    v = b;
                    return r;
                }
                else
                {
                    v = null;
                    return 0;
                }
            }
            else
            {
                v = null;
                return err;
            }
        }

        public int ReadFrom(DataReader data, out float? v)
        {
            var err = data.ReadFiexd(out byte have);
            if (err == 0)
            {
                if (have == 1)
                {
                    var r = data.ReadFiexd(out float b);
                    v = b;
                    return r;
                }
                else
                {
                    v = null;
                    return 0;
                }
            }
            else
            {
                v = null;
                return err;
            }
        }

        public int ReadFrom(DataReader data, out double? v)
        {
            var err = data.ReadFiexd(out byte have);
            if (err == 0)
            {
                if (have == 1)
                {
                    var r = data.ReadFiexd(out double b);
                    v = b;
                    return r;
                }
                else
                {
                    v = null;
                    return 0;
                }
            }
            else
            {
                v = null;
                return err;
            }
        }

        #endregion

        #region ReadClass

        public int ReadFrom(DataReader data,out ISerde v)
        {
            IdxStore.Clear();
            return ReadISerde(data, out v);

        }


        public int ReadISerde(DataReader data, out ISerde v)
        {
            v = default;
            int err;
            if ((err = data.ReadVarInteger(out uint offset)) == 0)
            {
                if (offset == 0)
                    return 0;
               
                if(offset == IdxStore.Count+1)
                {
                    if ((err = data.ReadVarInteger(out ushort typeid)) == 0)
                    {
                        v = Create(typeid);
                        if (v != null)
                        {
                            IdxStore.Add(v);
                            return v.Read(this, data);
                        }
                        else
                            throw new KeyNotFoundException($"create obj not found typeid:{typeid}");
                    }
                }
                else
                {
                    if(offset> IdxStore.Count)
                        throw new KeyNotFoundException($"offset :{offset} error");

                    v = IdxStore[(int)offset - 1];
                    if (v != null)
                        return 0;
                    else
                        throw new KeyNotFoundException($"offset :{offset} obj not as ISerde");
                }
            }

            return err;
        }

        public int ReadFrom<T>(DataReader data, out T v) where T : class, ISerde, new()
        {
            IdxStore.Clear();
            return ReadObj(data, out v);
        }

        public int ReadObj<T>(DataReader data,out T v) where T : class, ISerde, new()
        {
            v = null;
            int err;
            if ((err= data.ReadVarInteger(out uint offset)) == 0)
            {
                if (offset == 0)
                    return 0;

                if (offset == IdxStore.Count + 1)
                {
                    if ((err = data.ReadVarInteger(out ushort typeid)) == 0)
                    {
                        v = Create(typeid) as T;
                        if (v != null)
                        {
                            IdxStore.Add(v);
                            return v.Read(this, data);                            
                        }
                        else
                            throw new KeyNotFoundException($"create obj not found typeid:{typeid}");
                    }
                }
                else
                {
                    if (offset > IdxStore.Count)
                        throw new KeyNotFoundException($"offset :{offset} error");

                    v = IdxStore[(int)offset - 1] as T;
                    if (v != null)
                        return 0;
                    else
                        throw new KeyNotFoundException($"offset :{offset} obj not as ISerde");
                }
            }
          
            return err;
        }
        #endregion

        #region ReadArray
        public int ReadFrom(DataReader data,out byte[] v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);

            if (err == 0)
            {
                if (len > data.Length - data.Offset)
                    return -1001;

                v = new byte[len];
                return data.ReadBuf(v, 0, v.Length);
            }
            else
                return err;
        }

        public int ReadFrom<T>(DataReader data, out T[] v) where T : class, ISerde, new()
        {
            IdxStore.Clear();
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                if (len > data.Length - data.Offset)
                    return -1002;

                v = new T[len];
                for (int i = 0; i < len; i++)
                {
                    if ((err = ReadObj(data, out T x)) == 0)
                        v[i] = x;
                }
            }

            return err;
        }

        #endregion

        #region ReadList
        public int ReadFrom<T>(DataReader data, out List<T> v) where T : class, ISerde, new()
        {
            IdxStore.Clear();
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<T>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = ReadObj(data, out T x)) == 0)
                        v.Add(x);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data,out List<byte> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<byte>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = data.ReadFiexd(out byte b)) == 0)
                        v.Add(b);
                }                
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<sbyte> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<sbyte>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = data.ReadFiexd(out sbyte b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<bool> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<bool>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = data.ReadFiexd(out bool b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<short> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<short>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = data.ReadVarInteger(out short b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<ushort> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<ushort>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = data.ReadVarInteger(out ushort b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<int> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<int>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = data.ReadVarInteger(out int b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<uint> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<uint>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = data.ReadVarInteger(out uint b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<long> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<long>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = data.ReadVarInteger(out long b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<ulong> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<ulong>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = data.ReadVarInteger(out ulong b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<string> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<string>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = ReadFrom(data,out string b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }




        public int ReadFrom(DataReader data, out List<byte?> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<byte?>();
                for (int i = 0; i < len; i++)
                {
                    if ((err =ReadFrom(data,out byte? b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<sbyte?> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<sbyte?>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = ReadFrom(data, out sbyte? b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<bool?> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<bool?>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = ReadFrom(data,out bool? b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<short?> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<short?>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = ReadFrom(data,out short? b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<ushort?> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<ushort?>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = ReadFrom(data,out ushort? b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<int?> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<int?>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = ReadFrom(data,out int? b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<uint?> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<uint?>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = ReadFrom(data,out uint? b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<long?> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<long?>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = ReadFrom(data,out long? b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }

        public int ReadFrom(DataReader data, out List<ulong?> v)
        {
            v = null;
            var err = data.ReadVarInteger(out uint len);
            if (err == 0)
            {
                v = new List<ulong?>();
                for (int i = 0; i < len; i++)
                {
                    if ((err = ReadFrom(data,out ulong? b)) == 0)
                        v.Add(b);
                }
            }
            return err;
        }


        #endregion
    }
}
