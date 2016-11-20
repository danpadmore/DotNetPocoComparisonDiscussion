using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DotNetPocoComparisonDiscussion
{
    public class PlainOldClrOjbect
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// A property that was added in a newer versino
        /// </summary>
        public string NewlyAddedProperty { get; set; }

        /// <summary>
        /// This method carries risks of developer forgetting to extend it with new properties
        /// </summary>
        public bool IsEqualByManualComparison(PlainOldClrOjbect other)
        {
            if (other == null)
                return false;

            return other.Id == Id
                && other.Name == Name;
            // Developer then forgets to add NewlyAddedProperty to comparison
        }

        /// <summary>
        /// This method incurs performance penalty for serialization
        /// </summary>
        public bool IsEqualBySerializedStringComparison(PlainOldClrOjbect other)
        {
            var serializer = new DataContractJsonSerializer(GetType());

            using (var sourceStream = new MemoryStream())
            using (var otherStream = new MemoryStream())
            {
                serializer.WriteObject(sourceStream, this);
                var sourceSerialized = Encoding.Default.GetString(sourceStream.ToArray());

                serializer.WriteObject(otherStream, other);
                var otherSerialized = Encoding.Default.GetString(otherStream.ToArray());

                return (sourceSerialized == otherSerialized);
            }
        }

        /// <summary>
        /// This method incurs performance penalty for reflection
        /// </summary>
        public bool IsEqualByReflection(PlainOldClrOjbect other)
        {
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var source = property.GetValue(this);
                var otherValue = property.GetValue(other);

                if (source == null && otherValue != null)
                    return false;

                if (source != null && otherValue == null)
                    return false;

                if (!source.Equals(otherValue))
                    return false;
            }

            return true;
        }
    }
}
