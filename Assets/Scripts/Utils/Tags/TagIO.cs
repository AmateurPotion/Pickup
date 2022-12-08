#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace Pickup.Utils.Tags
{
    public static class TagIO
    {
        private static readonly PayloadHandler[] PayloadHandlers = new PayloadHandler[12]
        {
            null!,
            new PayloadHandler<byte>(r => r.ReadByte(), (w, v) => w.Write(v)),
            new PayloadHandler<short>(r => r.ReadInt16(), (w, v) => w.Write(v)),
            new PayloadHandler<int>(r => r.ReadInt32(), (w, v) => w.Write(v)),
            new PayloadHandler<long>(r => r.ReadInt64(), (w, v) => w.Write(v)),
            new PayloadHandler<float>(r => r.ReadSingle(), (w, v) => w.Write(v)),
            new PayloadHandler<double>(r => r.ReadDouble(), (w, v) => w.Write(v)),
            new ClassPayloadHandler<byte[]>(r => r.ReadBytes(r.ReadInt32()), (w, v) =>
            {
                w.Write(v.Length);
                w.Write(v);
            }, v => (byte[])v.Clone(), (Func<byte[]>)(Array.Empty<byte>)),
            new ClassPayloadHandler<string>(r => Encoding.UTF8.GetString(r.ReadBytes(r.ReadInt16())), (w, v) =>
            {
                byte[] bytes = Encoding.UTF8.GetBytes(v);
                w.Write((short)bytes.Length);
                w.Write(bytes);
            }, v => v, (Func<string>)(() => "")),
            new ClassPayloadHandler<IList>((r => GetHandler(r.ReadByte()).ReadList(r, r.ReadInt32())), (w, v) =>
            {
                int payloadId;
                try
                {
                    payloadId = GetPayloadId(v.GetType().GetGenericArguments()[0]);
                }
                catch (IOException)
                {
                    throw new IOException("Invalid NBT list type: " + v.GetType());
                }

                w.Write((byte)payloadId);
                w.Write(v.Count);
                PayloadHandlers[payloadId].WriteList(w, v);
            }, (v =>
            {
                try
                {
                    return GetHandler(GetPayloadId(v.GetType().GetGenericArguments()[0])).CloneList(v);
                }
                catch (IOException)
                {
                    throw new IOException("Invalid NBT list type: " + v.GetType());
                }
            })),
            new ClassPayloadHandler<TagCompound>(r =>
            {
                var tagCompound = new TagCompound();
                while (ReadTag(r, out var name) is { } obj)
                    tagCompound.Set(name, obj);
                return tagCompound;
            }, (w, v) =>
            {
                foreach (KeyValuePair<string, object> keyValuePair in v)
                {
                    if (keyValuePair.Value != null)
                        WriteTag(keyValuePair.Key, keyValuePair.Value, w);
                }

                w.Write((byte)0);
            }, v => (TagCompound)v.Clone(), (Func<TagCompound>)(() => new TagCompound())),
            new ClassPayloadHandler<int[]>(r =>
            {
                var numArray = new int[r.ReadInt32()];
                for (var index = 0; index < numArray.Length; ++index)
                    numArray[index] = r.ReadInt32();
                return numArray;
            }, (w, v) =>
            {
                w.Write(v.Length);
                foreach (var num in v)
                    w.Write(num);
            }, v => (int[])v.Clone(), (Func<int[]>)(Array.Empty<int>))
        };

        private static readonly Dictionary<Type, int> PayloadIDs = Enumerable.Range(1, PayloadHandlers.Length - 1)
            .ToDictionary((Func<int, Type>)(i => PayloadHandlers[i].payloadType));

        private static readonly PayloadHandler<string> StringHandler = (PayloadHandler<string>)PayloadHandlers[8];

        private static PayloadHandler GetHandler(int id)
        {
            if (id < 1 || id >= PayloadHandlers.Length) throw new IOException("Invalid NBT payload id: " + id);

            return PayloadHandlers[id];
        }

        private static int GetPayloadId(Type t)
        {
            if (PayloadIDs.TryGetValue(t, out var payloadId)) return payloadId;
            if (typeof(IList).IsAssignableFrom(t)) return 9;

            throw new IOException($"Invalid NBT payload type '{t}'");
        }

        public static object Serialize(object value)
        {
            var type1 = value.GetType();
            if (TagSerializer.TryGetSerializer(type1, out var serializer))
                return serializer.Serialize(value);
            if (GetPayloadId(type1) != 9)
                return value;
            var list = (IList)value;
            var type2 = type1.GetElementType() ?? type1.GetGenericArguments()[0];
            if (TagSerializer.TryGetSerializer(type2, out serializer))
                return serializer.SerializeList(list);
            if (GetPayloadId(type2) != 9)
                return list;
            var listList = new List<IList>(list.Count);
            listList.AddRange(from object? obj in list select (IList)Serialize(obj));
            return listList;
        }

        public static T Deserialize<T>(object? tag) => tag is T obj ? obj : (T)Deserialize(typeof(T), tag);

        public static object Deserialize(Type type, object? tag)
        {
            if (type.IsInstanceOfType(tag))
                if (tag != null)
                    return tag;
            if (TagSerializer.TryGetSerializer(type, out var serializer))
            {
                tag ??= Deserialize(serializer.tagType, null);
                return serializer.Deserialize(tag);
            }

            if (tag == null && !type.IsArray)
            {
                if (type.GetGenericArguments().Length == 0)
                    return GetHandler(GetPayloadId(type)).Default();
                if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    return Activator.CreateInstance(type);
            }

            if (tag is null or IList || type.IsArray)
            {
                if (type.IsArray)
                {
                    var elementType = type.GetElementType() ?? throw new InvalidOperationException();
                    if (tag == null)
                        return Array.CreateInstance(elementType, 0);
                    var list = (IList)tag;
                    if (TagSerializer.TryGetSerializer(elementType, out serializer))
                    {
                        IList instance = Array.CreateInstance(elementType, list.Count);
                        for (int index = 0; index < list.Count; ++index)
                            instance[index] = serializer.Deserialize(list[index]);
                        return instance;
                    }

                    IList instance1 = Array.CreateInstance(elementType, list.Count);
                    for (int index = 0; index < list.Count; ++index)
                        instance1[index] = Deserialize(elementType, list[index]);
                    return instance1;
                }

                if (type.GetGenericArguments().Length == 1)
                {
                    var genericArgument = type.GetGenericArguments()[0];
                    var type1 = typeof(List<>).MakeGenericType(genericArgument);
                    if (type.IsAssignableFrom(type1))
                    {
                        if (tag == null)
                            return Activator.CreateInstance(type1);
                        if (TagSerializer.TryGetSerializer(genericArgument, out serializer))
                            return serializer.DeserializeList((IList)tag);
                        var list = (IList)tag;
                        var instance = (IList)Activator.CreateInstance(type1, list.Count);
                        foreach (var tag1 in list)
                            instance.Add(Deserialize(genericArgument, tag1));
                        return instance;
                    }
                }
            }

            if (tag == null)
            {
                throw new IOException($"Invalid NBT payload type '{type}'");
            }

            throw new InvalidCastException($"Unable to cast object of type '{tag.GetType()}' to type '{type}'");
        }

        public static T Clone<T>(T o) where T : notnull => (T)GetHandler(GetPayloadId(o.GetType())).Clone(o);

        public static object? ReadTag(BinaryReader r, out string? name)
        {
            int index = r.ReadByte();
            if (index == 0)
            {
                name = null;
                return null;
            }

            name = StringHandler.reader(r);
            return PayloadHandlers[index].Read(r);
        }

        public static void WriteTag(string name, object tag, BinaryWriter w)
        {
            int payloadId = GetPayloadId(tag.GetType());
            w.Write((byte)payloadId);
            StringHandler.writer(w, name);
            PayloadHandlers[payloadId].Write(w, tag);
        }

        public static TagCompound FromFile(string path, bool compressed = true)
        {
            try
            {
                using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                    return FromStream(stream, compressed);
            }
            catch (IOException ex)
            {
                throw new IOException("Failed to read NBT file: " + path, ex);
            }
        }

        public static TagCompound FromStream(Stream stream, bool compressed = true)
        {
            if (compressed) stream = new GZipStream(stream, CompressionMode.Decompress);

            return Read(new BigEndianReader(stream));
        }

        public static TagCompound Read(BinaryReader reader) => ReadTag(reader, out _) is TagCompound tagCompound
            ? tagCompound
            : throw new IOException("Root tag not a TagCompound");

        public static void ToFile(TagCompound root, string path, bool compress = true)
        {
            try
            {
                using Stream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                ToStream(root, stream, compress);
            }
            catch (IOException ex)
            {
                throw new IOException("Failed to read NBT file: " + path, ex);
            }
        }

        public static void ToStream(TagCompound root, Stream stream, bool compress = true)
        {
            if (compress)
                stream = new GZipStream(stream, CompressionMode.Compress, true);
            Write(root, new BigEndianWriter(stream));
            if (!compress)
                return;
            stream.Close();
        }

        public static void Write(TagCompound root, BinaryWriter writer) => WriteTag("", root, writer);

        private abstract class PayloadHandler
        {
            public abstract Type payloadType { get; }

            public abstract object Default();

            public abstract object Read(BinaryReader r);

            public abstract void Write(BinaryWriter w, object v);

            public abstract IList ReadList(BinaryReader r, int size);

            public abstract void WriteList(BinaryWriter w, IList list);

            public abstract object Clone(object o);

            public abstract IList CloneList(IList list);
        }

        private class PayloadHandler<T> : PayloadHandler where T : notnull
        {
            internal Func<BinaryReader, T> reader;
            internal Action<BinaryWriter, T> writer;

            public PayloadHandler(Func<BinaryReader, T> reader, Action<BinaryWriter, T> writer)
            {
                this.reader = reader;
                this.writer = writer;
            }

            public override Type payloadType => typeof(T);

            public override object Read(BinaryReader r) => reader(r);

            public override void Write(BinaryWriter w, object v) => writer(w, (T)v);

            public override IList ReadList(BinaryReader r, int size)
            {
                List<T> objList = new List<T>(size);
                for (int index = 0; index < size; ++index)
                    objList.Add(reader(r));
                return objList;
            }

            public override void WriteList(BinaryWriter w, IList list)
            {
                foreach (T obj in list)
                    writer(w, obj);
            }

            public override object Clone(object o) => o;

            public override IList CloneList(IList list) => CloneList((IList<T>)list);

            public virtual IList CloneList(IList<T> list) => new List<T>(list);

            public override object Default() => default(T);
        }

        private class ClassPayloadHandler<T> : PayloadHandler<T> where T : class
        {
            private readonly Func<T, T> m_Clone;
            private Func<T>? m_MakeDefault;

            public ClassPayloadHandler(
                Func<BinaryReader, T> reader,
                Action<BinaryWriter, T> writer,
                Func<T, T> clone,
                Func<T>? makeDefault = null)
                : base(reader, writer)
            {
                m_Clone = clone;
                m_MakeDefault = makeDefault;
            }

            public override object Clone(object o) => m_Clone((T)o);
            public override IList CloneList(IList<T> list) => list.Select(m_Clone).ToList();
            
            public override object Default() => m_MakeDefault();
        }
    }
}