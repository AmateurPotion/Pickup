#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Pickup.Utils.Tags {
    public abstract class TagSerializer
    {
      private static readonly IDictionary<Type, TagSerializer> Serializers = new Dictionary<Type, TagSerializer>();
      private static readonly Dictionary<string, Type> TypeNameCache = new Dictionary<string, Type>();

      public abstract Type type { get; }

      public abstract Type tagType { get; }

      public abstract object Serialize(object value);

      public abstract object Deserialize(object tag);

      public abstract IList SerializeList(IList value);

      public abstract IList DeserializeList(IList value);

      static TagSerializer() => Reload();

      internal static void Reload()
      {
        Serializers.Clear();
        TypeNameCache.Clear();
      }

      public static bool TryGetSerializer(Type type, [NotNullWhen(true)] out TagSerializer? serializer)
      {
        if (Serializers.TryGetValue(type, out serializer))
          return true;
        if (type.IsArray && type.GetArrayRank() > 1)
        {
          Serializers[type] = serializer = new MultiDimArraySerializer(type);
          return true;
        }
        if (!typeof (ITagSerializable).IsAssignableFrom(type))
          return false;
        Type type1 = typeof (TagSerializableSerializer<>).MakeGenericType(type);
        Serializers[type] = serializer = (TagSerializer) Activator.CreateInstance(type1);
        return true;
      }

      internal static void AddSerializer(TagSerializer serializer) => Serializers.Add(serializer.type, serializer);

      public static Type? GetType(string name)
      {
        if (TypeNameCache.TryGetValue(name, out var type1)) return type1;
        
        var type2 = Type.GetType(name);
        if (type2 != null) return TypeNameCache[name] = type2;
        
        /*
        foreach (var mod in Terraria.ModLoader.ModLoader.Mods)
        {
          Type type3 = mod.Code?.GetType(name);
          if (type3 != null)
            return TypeNameCache[name] = type3;
        }
        */
        return null;
      }

      protected void Register() => AddSerializer(this);
      // public void SetupContent() => this.SetStaticDefaults();
    }
    
    public abstract class TagSerializer<T, S> : TagSerializer where T : notnull where S : notnull
    {
      public override Type type => typeof (T);
      public override Type tagType => typeof (S);
      public abstract S Serialize(T value);
      public abstract T Deserialize(S tag);
      public override object Serialize(object value) => Serialize((T) value);
      public override object Deserialize(object tag) => Deserialize((S) tag);
      public override IList SerializeList(IList value) => ((IEnumerable<T>) value).Select(Serialize).ToList();
      public override IList DeserializeList(IList value) => ((IEnumerable<S>) value).Select(Deserialize).ToList();
    }
}
