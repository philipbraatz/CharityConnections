using System;

namespace Doorfail.DataConnection
{
    public class Guard
    {
        public static void ThrowIfNull(object param)
        {
            var props = param.GetType().GetProperties();
            foreach (var prop in props)
            {
                var name = prop.Name;
                var val = prop.GetValue(param, null);

                _ = val ?? throw new ArgumentNullException(name);
            }
        }
    }
}
