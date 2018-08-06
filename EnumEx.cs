using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrape.Utility
{


        public static class EnumEx
        {
            public static T GetValueFromDescription<T>(string description)
            {
                var type = typeof(T);
                if (!type.IsEnum) throw new InvalidOperationException();
                foreach (var field in type.GetFields())
                {
                    var attribute = Attribute.GetCustomAttribute(field,
                        typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attribute != null)
                    {
                        if (attribute.Description == description)
                            return (T)field.GetValue(null);
                    }
                    else
                    {
                        if (field.Name == description)
                            return (T)field.GetValue(null);
                    }
                }
                throw new ArgumentException("Not found.", "description");
                // or return default(T);
            }


        public static T Parse<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }


    }

}
