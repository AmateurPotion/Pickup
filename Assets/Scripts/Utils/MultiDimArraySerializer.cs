#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pickup.Utils.Tags;

namespace Pickup.Utils
{
    public class MultiDimArraySerializer : TagSerializer<Array, TagCompound>
    {
        private const string Ranks = "ranks";
        private const string List = "list";

        public readonly Type arrayType;
        public readonly Type elementType;
        public readonly int arrayRank;

        public MultiDimArraySerializer(Type arrayType)
        {
            this.arrayType = arrayType.IsArray
                ? arrayType
                : throw new ArgumentException("Must be an array type", nameof(arrayType));
            elementType = arrayType.GetElementType() ?? throw new InvalidOperationException();
            arrayRank = arrayType.GetArrayRank();
        }

        public override TagCompound Serialize(Array array)
        {
            if (array.Length == 0) return ToTagCompound(array);
            
            var targetType = TagIO.Serialize(array.GetValue(new int[array.Rank])).GetType();
            return ToTagCompound(array, targetType,
                TagIO.Serialize);
        }

        public override Array Deserialize(TagCompound tag)
        {
            return FromTagCompound(tag, arrayType,
                (Converter)(e => TagIO.Deserialize(elementType, e)));
        }

        public override IList SerializeList(IList list)
        {
            var tagCompoundList = new List<TagCompound>(list.Count);
            tagCompoundList.AddRange(from Array array in list select Serialize(array));
            return tagCompoundList;
        }

        public override IList DeserializeList(IList list)
        {
            var tagCompoundList = (IList<TagCompound>)list;
            var instance = (IList)Activator.CreateInstance(
                typeof(List<>).MakeGenericType(arrayType),
                tagCompoundList.Count);
            foreach (var tag in tagCompoundList)
                instance.Add(Deserialize(tag));
            return instance;
        }

        public static TagCompound ToTagCompound(
            Array array,
            Type? elementType = null,
            Converter? converter = null)
        {
            var numArray = new int[array.Rank];
            for (var dimension = 0; dimension < numArray.Length; ++dimension)
                numArray[dimension] = array.GetLength(dimension);
            return new TagCompound
            {
                ["ranks"] = numArray,
                ["list"] = ToList(array, elementType, converter)
            };
        }

        public static IList ToList(
            Array array,
            Type? elementType = null,
            Converter? converter = null)
        {
            var type1 = array.GetType();
            var type2 = typeof(List<>);
            var typeArray = new Type[1];
            var type3 = elementType ?? type1.GetElementType();

            typeArray[0] = type3 ?? throw new InvalidOperationException();
            var instance = (IList)Activator.CreateInstance(type2.MakeGenericType(typeArray), array.Length);
            
            foreach (object element in array)
                instance.Add(converter != null ? converter(element) : element);
            
            return instance;
        }

        public static Array FromTagCompound(
            TagCompound tag,
            Type arrayType,
            Converter? converter = null)
        {
            var elementType = (arrayType.IsArray
                ? arrayType.GetElementType()
                : throw new ArgumentException("Must be an array type", nameof(arrayType))) ?? throw new InvalidOperationException();
            return !tag.TryGet("ranks", out int[] arrayRanks)
                ? Array.CreateInstance(elementType, new int[arrayType.GetArrayRank()])
                : FromList(tag.Get<List<object>>("list"),
                    arrayRanks, elementType, converter);
        }

        public static Array FromList(
            IList list,
            int[] arrayRanks,
            Type? elementType = null,
            Converter? converter = null)
        {
            if (arrayRanks.Length == 0)
                throw new ArgumentException("Array rank must be greater than 0");
            if (list.Count != arrayRanks.Aggregate(1,
                    (Func<int, int, int>)((current, length) => current * length)))
                throw new ArgumentException("List length does not match array length");
            var type = list.GetType();
            elementType ??= type.GetElementType();
            if (elementType == null)
            {
                var genericArguments = type.GetGenericArguments();
                elementType = genericArguments.Length == 1
                    ? genericArguments[0]
                    : throw new ArgumentException("IList type must have exactly one generic argument");
            }

            Array instance = Array.CreateInstance(elementType, arrayRanks);
            int[] numArray1 = new int[arrayRanks.Length];
            foreach (object obj in list)
            {
                object element = obj;
                for (int index = numArray1.Length - 1; index >= 0 && numArray1[index] >= arrayRanks[index]; --index)
                {
                    if (index != 0)
                    {
                        numArray1[index] = 0;
                        ++numArray1[index - 1];
                    }
                    else
                        goto label_23;
                }

                if (converter != null)
                    element = converter(element);
                instance.SetValue(element, numArray1);
                ++numArray1[^1];
            }

            label_23:
            return instance;
        }

        public delegate object Converter(object element);
    }
}