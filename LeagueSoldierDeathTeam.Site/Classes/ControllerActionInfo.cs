using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Web.Mvc;

namespace LeagueSoldierDeathTeam.Site.Classes
{
	public class ControllerActionInfo
	{
		private static readonly int WordControllerLength = "Controller".Length;

		public string ControllerName { get; private set; }
		public string ActionName { get; private set; }
		public RouteValueDictionary RouteValues { get; private set; }

		private ControllerActionInfo()
		{ }

		public static ControllerActionInfo Create<TController>(Expression<Action<TController>> action, IDictionary<string, object> dRouteValues)
			where TController : Controller
		{
			var routeValues = dRouteValues == null ? null : new RouteValueDictionary(dRouteValues);
			return Create(action, routeValues);
		}

		public static ControllerActionInfo Create<TController>(Expression<Action<TController>> action, object oRouteValues)
			where TController : Controller
		{
			var routeValues = oRouteValues == null ? null : new RouteValueDictionary(oRouteValues);
			return Create(action, routeValues);
		}

		public static ControllerActionInfo Create<TController>(Expression<Action<TController>> action, RouteValueDictionary routeValues = null)
			where TController : Controller
		{
			var controllerName = typeof(TController).Name;
			controllerName = controllerName.Substring(0, controllerName.Length - WordControllerLength);

			var method = (MethodCallExpression)action.Body;
			var actionName = method.Method.Name;

			if (method.Arguments.Count > 0)
			{
				var parameters = method.Method.GetParameters();
				if (routeValues == null)
					routeValues = new RouteValueDictionary();
				for (var i = 0; i < parameters.Length; i++)
				{
					var name = parameters[i].Name;
					var type = parameters[i].ParameterType;
					if (type.IsValueType || type == typeof(string))
					{
						if (routeValues.ContainsKey(name))
							continue;
						var value = GetArgumentValue(method.Arguments[i]);
						if (value != null)
							routeValues.Add(name, value);
					}
					else
						if (type == typeof(IList<int>))
						{
							var list = (IList<int>)(CachedExpressionCompiler.Evaluate(method.Arguments[i]));
							if (list == null)
								continue;
							for (var j = 0; j < list.Count; j++)
								routeValues.Add(string.Concat(name, "[", j, "]"), list[j]);
						}
						else
						{
							var value = GetArgumentValue(method.Arguments[i]);
							if (value == null)
								continue;
							var objectValues = new RouteValueDictionary(value);
							foreach (var pair in objectValues.Where(pair => !routeValues.ContainsKey(pair.Key)))
							{
								if (pair.Value == null || pair.Value is IDictionary)
									continue;
								//routeValues.Add(pair.Key, pair.Value);
								if (pair.Value is IEnumerable && !(pair.Value is string))
								{
									var index = 0;
									foreach (var val in (IEnumerable)pair.Value)
									{
										routeValues.Add(string.Format("{0}[{1}]", pair.Key, index), val);
										index++;
									}
								}
								else
									routeValues.Add(pair.Key, pair.Value);
							}
						}
				}
			}

			return new ControllerActionInfo
			{
				ControllerName = controllerName,
				ActionName = actionName,
				RouteValues = routeValues,
			};
		}

		private static object GetArgumentValue(Expression argument)
		{
			var constant = argument as ConstantExpression;
			return constant == null ? CachedExpressionCompiler.Evaluate(argument) : constant.Value;
		}
	}
}