using Omu.ValueInjecter;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JehovaJireh.Data.Mappings
{
	public class IgnoreCaseInjection : ConventionInjection
	{
		protected override bool Match(ConventionInfo c)
		{
			return String.Compare(c.SourceProp.Name, c.TargetProp.Name,
								  StringComparison.OrdinalIgnoreCase) == 0;
		}
	}

	public class NoNullsInjection : ConventionInjection
	{
		protected override bool Match(ConventionInfo c)
		{
			return c.SourceProp.Name == c.TargetProp.Name
					&& c.SourceProp.Value != null;
		}
	}

	public class OrderPatientInjection : ConventionInjection
	{
		protected override bool Match(ConventionInfo c)
		{
			string[] p = new string[] { "Age", "DOB", "Height", "HIPPASignatureOnFile", "Sex", "Weight" };

			return (c.SourceProp.Name == c.TargetProp.Name
					&& c.SourceProp.Name.StartsWith("Guarantor"))
					|| (c.SourceProp.Name == c.TargetProp.Name
					&& c.SourceProp.Name.StartsWith("Patient")
					&& p.Any(c.SourceProp.Name.EndsWith))
					|| (c.SourceProp.Name == c.TargetProp.Name
					&& c.SourceProp.Name.StartsWith("OrderingPhysican"));
		}
	}

	public class SmartConventionInjection : ValueInjection
	{
		private class Path
		{
			public IDictionary<string, string> MatchingProps { get; set; }
		}

		private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<KeyValuePair<Type, Type>, Path>> WasLearned = new ConcurrentDictionary<Type, ConcurrentDictionary<KeyValuePair<Type, Type>, Path>>();

		protected virtual void SetValue(PropertyDescriptor prop, object component, object value)
		{
               prop.SetValue(component, value);
		}

		protected virtual object GetValue(PropertyDescriptor prop, object component)
		{
			return prop.GetValue(component);
		}

		protected virtual bool Match(SmartConventionInfo c)
		{
			return c.SourceProp.Name == c.TargetProp.Name && c.SourceProp.PropertyType == c.TargetProp.PropertyType;
		}

		protected virtual void ExecuteMatch(SmartMatchInfo mi)
		{
			SetValue(mi.TargetProp, mi.Target, GetValue(mi.SourceProp, mi.Source));
		}

		private Path Learn(object source, object target)
		{
			Path path = null;
			var sourceProps = source.GetProps();
			var targetProps = target.GetProps();
			var smartConventionInfo = new SmartConventionInfo
			{
				SourceType = source.GetType(),
				TargetType = target.GetType()
			};

			for (var i = 0; i < sourceProps.Count; i++)
			{
				var sourceProp = sourceProps[i];
				smartConventionInfo.SourceProp = sourceProp;

				for (var j = 0; j < targetProps.Count; j++)
				{
					var targetProp = targetProps[j];
					smartConventionInfo.TargetProp = targetProp;

					if (!Match(smartConventionInfo)) continue;
					if (path == null)
						path = new Path
						{
							MatchingProps = new Dictionary<string, string> { { smartConventionInfo.SourceProp.Name, smartConventionInfo.TargetProp.Name } }
						};
					else path.MatchingProps.Add(smartConventionInfo.SourceProp.Name, smartConventionInfo.TargetProp.Name);
				}
			}
			return path;
		}

		protected override void Inject(object source, object target)
		{
			var sourceProps = source.GetProps();
			var targetProps = target.GetProps();

			var cacheEntry = WasLearned.GetOrAdd(GetType(), new ConcurrentDictionary<KeyValuePair<Type, Type>, Path>());

			var path = cacheEntry.GetOrAdd(new KeyValuePair<Type, Type>(source.GetType(), target.GetType()), pair => Learn(source, target));

			if (path == null) return;

			foreach (var pair in path.MatchingProps)
			{
				var sourceProp = sourceProps.GetByName(pair.Key);
				var targetProp = targetProps.GetByName(pair.Value);
				ExecuteMatch(new SmartMatchInfo
				{
					Source = source,
					Target = target,
					SourceProp = sourceProp,
					TargetProp = targetProp
				});
			}
		}
	}

	public class SmartConventionInfo
	{
		public Type SourceType { get; set; }
		public Type TargetType { get; set; }

		public PropertyDescriptor SourceProp { get; set; }
		public PropertyDescriptor TargetProp { get; set; }
	}

	public class SmartMatchInfo
	{
		public PropertyDescriptor SourceProp { get; set; }
		public PropertyDescriptor TargetProp { get; set; }
		public object Source { get; set; }
		public object Target { get; set; }
	}
	// Define other methods and classes here
	public class DeepCloneInjection : SmartConventionInjection
	{
		protected override bool Match(SmartConventionInfo c)
		{
			return c.SourceProp.Name == c.TargetProp.Name;
		}

		protected override void ExecuteMatch(SmartMatchInfo mi)
		{
			var sourceVal = GetValue(mi.SourceProp, mi.Source);
			//if (sourceVal == null) return;

			//for value types and string just return the value as is
			if (mi.SourceProp.PropertyType.IsValueType || mi.SourceProp.PropertyType == typeof(string) || sourceVal == null)
			{
				SetValue(mi.TargetProp, mi.Target, sourceVal);
				return;
			}

			//handle arrays
			if (mi.SourceProp.PropertyType.IsArray)
			{
				var arr = sourceVal as Array;
				var arrayClone = arr.Clone() as Array;

				for (var index = 0; index < arr.Length; index++)
				{
					var arriVal = arr.GetValue(index);
					if (arriVal.GetType().IsValueType || arriVal.GetType() == typeof(string)) continue;
					arrayClone.SetValue(Activator.CreateInstance(arriVal.GetType()).InjectFrom<DeepCloneInjection>(arriVal), index);
				}
				SetValue(mi.TargetProp, mi.Target, arrayClone);
				return;
			}

			if (mi.SourceProp.PropertyType.IsGenericType)
			{
				//handle IEnumerable<> also ICollection<> IList<> List<>
				if (mi.SourceProp.PropertyType.GetGenericTypeDefinition().GetInterfaces().Contains(typeof(IEnumerable)))
				{
					var genericArgument = mi.TargetProp.PropertyType.GetGenericArguments()[0];

					var tlist = typeof(List<>).MakeGenericType(genericArgument);

					var list = Activator.CreateInstance(tlist);

					if (genericArgument.IsValueType || genericArgument == typeof(string))
					{
						var addRange = tlist.GetMethod("AddRange");
						addRange.Invoke(list, new[] { sourceVal });
					}
					else
					{
						var addMethod = tlist.GetMethod("Add");
						foreach (var o in sourceVal as IEnumerable)
						{
							addMethod.Invoke(list, new[] { Activator.CreateInstance(genericArgument).InjectFrom<DeepCloneInjection>(o) });
						}
					}
					SetValue(mi.TargetProp, mi.Target, list);
					return;
				}

				throw new NotImplementedException(string.Format("deep clonning for generic type {0} is not implemented", mi.SourceProp.Name));
			}

			//for simple object types create a new instace and apply the clone injection on it
			SetValue(mi.TargetProp, mi.Target, Activator.CreateInstance(mi.TargetProp.PropertyType).InjectFrom<DeepCloneInjection>(sourceVal));
		}
	}
}
