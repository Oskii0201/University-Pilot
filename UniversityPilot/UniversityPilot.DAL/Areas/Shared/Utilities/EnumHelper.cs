using System.ComponentModel;
using System.Reflection;

namespace UniversityPilot.DAL.Areas.Shared.Utilities
{
    public static class EnumHelper
    {
        public static TEnum ParseEnumFromDescription<TEnum>(string description) where TEnum : Enum
        {
            foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null && attribute.Description == description)
                {
                    return (TEnum)field.GetValue(null);
                }
            }

            throw new ArgumentException($"No matching enum value found for description: {description}", nameof(description));
        }

        public static TEnum ParseEnumFromDescriptionOrDefault<TEnum>(string description, TEnum defaultValue) where TEnum : Enum
        {
            foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attribute = field.GetCustomAttribute<DescriptionAttribute>();
                if (attribute != null && attribute.Description == description)
                {
                    return (TEnum)field.GetValue(null);
                }
            }

            return defaultValue;
        }
    }
}