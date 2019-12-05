using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Yutaka.Reflection
{
    public class Mapper<T>
    {
		protected readonly Dictionary<string, PropertyInfo> Properties;

		public Mapper()
		{
			Properties = typeof(T).GetProperties().ToDictionary(p => p.Name, p => p);
		}

		public void Map<TSource>(TSource source)
		{
			var instance = Activator.CreateInstance(typeof(T));
			var sourceType = typeof(TSource);
			var destType = typeof(T);
			Console.Write("\n");
			Console.Write("\n\t\tprivate static {0} To{0}({1} x)", destType, sourceType);

			//foreach (var p in Properties) {
			//	p.SetProperty(
			//		instance,
			//		Convert.Type(
			//			p.PropertyType,
			//			source.Items[Array.IndexOf(source.ItemsNames, p.Name)]);

			//}
		}
	}
}