using System;

namespace Pickup.Utils.Tags
{
    internal class TagSerializableSerializer<T> : TagSerializer<T, TagCompound> where T : ITagSerializable
    {
        private readonly Func<TagCompound, T> m_Deserializer;

        public TagSerializableSerializer()
        {
            var type = typeof(T);
            var field = type.GetField("DESERIALIZER");
            if (!(field != null)) return;

            if (field.FieldType != typeof(Func<TagCompound, T>))
            {
                throw new ArgumentException($"Invalid deserializer field type {field.FieldType} in {type.FullName} expected {typeof(Func<TagCompound, T>)}.");
            }

            m_Deserializer = (Func<TagCompound, T>)field.GetValue(null);
        }

        public override TagCompound Serialize(T value)
        {
            var tagCompound = value.SerializeData();
            tagCompound["<type>"] = value.GetType().FullName;
            return tagCompound;
        }

        public override T Deserialize(TagCompound tag)
        {
            if (tag.ContainsKey("<type>") && tag.GetString("<type>") != this.type.FullName)
            {
                var type = GetType(tag.GetString("<type>"));
                if (type != null && base.type.IsAssignableFrom(type) && TryGetSerializer(type, out var serializer)) return (T)serializer.Deserialize(tag);
            }

            if (m_Deserializer == null)
                throw new ArgumentException("Missing deserializer for type '" + type.FullName + "'.");
            return m_Deserializer(tag);
        }
    }
}