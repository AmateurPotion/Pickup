using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Pickup.Utils.Tags
{
    public class TagCompound : IEnumerable<KeyValuePair<string, object>>, ICloneable
    {
        private readonly Dictionary<string, object> m_Dict = new();

        public T Get<T>(string key)
        {
            if (TryGet<T>(key, out var obj)) return obj;
            if (obj != null) return obj;

            try
            {
                return TagIO.Deserialize<T>(null);
            }
            catch (Exception ex)
            {
                throw new IOException(
                    $"NBT Deserialization (type={typeof(T)},entry={TagPrinter.Print(new KeyValuePair<string, object>(key, null))})",
                    ex);
            }
        }

        public bool TryGet<T>(string key, out T value)
        {
            if (!m_Dict.TryGetValue(key, out var tag))
            {
                value = default(T);
                return false;
            }

            try
            {
                value = TagIO.Deserialize<T>(tag);
                return true;
            }
            catch (Exception ex)
            {
                throw new IOException(
                    $"NBT Deserialization (type={typeof(T)},entry={TagPrinter.Print(new KeyValuePair<string, object>(key, tag))})",
                    ex);
            }
        }

        public void Set(string key, object value, bool replace = false)
        {
            if (value == null)
            {
                Remove(key);
            }
            else
            {
                object obj;
                try
                {
                    obj = TagIO.Serialize(value);
                }
                catch (IOException ex)
                {
                    var str = "value=" + value;
                    if (value.GetType().ToString() != value.ToString())
                        str = "type=" + value.GetType() + "," + str;
                    throw new IOException($"NBT Serialization (key={key},{str})", ex);
                }

                if (replace) m_Dict[key] = obj;
                else m_Dict.Add(key, obj);
            }
        }

        public bool ContainsKey(string key) => m_Dict.ContainsKey(key);

        public bool Remove(string key) => m_Dict.Remove(key);
        public byte GetByte(string key) => Get<byte>(key);
        public short GetShort(string key) => Get<short>(key);
        public int GetInt(string key) => Get<int>(key);
        public long GetLong(string key) => Get<long>(key);
        public float GetFloat(string key) => Get<float>(key);
        public double GetDouble(string key) => Get<double>(key);
        public byte[] GetByteArray(string key) => Get<byte[]>(key);
        public int[] GetIntArray(string key) => Get<int[]>(key);
        public string GetString(string key) => Get<string>(key);
        public IList<T> GetList<T>(string key) => Get<List<T>>(key);
        public TagCompound GetCompound(string key) => Get<TagCompound>(key);
        public bool GetBool(string key) => Get<bool>(key);

        public short GetAsShort(string key)
        {
            var obj = Get<object>(key);
            return obj as short? ?? (obj as byte?).GetValueOrDefault();
        }

        public int GetAsInt(string key)
        {
            var obj = Get<object>(key);
            return obj as int? ?? (obj as short?) ?? (int)(obj as byte?).GetValueOrDefault();
        }

        public long GetAsLong(string key)
        {
            var obj = Get<object>(key);
            return obj as long? ??
                   (obj as int? ?? (obj as short?) ?? (int)(obj as byte?).GetValueOrDefault());
        }

        public double GetAsDouble(string key)
        {
            var obj = Get<object>(key);
            return obj as double? ?? (obj as float?).GetValueOrDefault();
        }

        public object Clone()
        {
            var tagCompound = new TagCompound();
            foreach (var keyValuePair in this)
                tagCompound.Set(keyValuePair.Key, TagIO.Clone(keyValuePair.Value));
            return tagCompound;
        }

        public override string ToString() => TagPrinter.Print(this);

        public object this[string key]
        {
            get => Get<object>(key);
            set => Set(key, value, true);
        }

        public void Add(string key, object value) => Set(key, value);
        public void Add(KeyValuePair<string, object> entry) => Set(entry.Key, entry.Value);
        public void Clear() => m_Dict.Clear();
        public int count => m_Dict.Count;

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => m_Dict.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}