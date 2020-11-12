﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Library.Database
{
    public static class ExspressionFilter
    {
        public static IOrderedQueryable<T> OrderByExp<T>(this IQueryable<T> source, string property)
        {
            var sortProperty = typeof(T).GetProperty(property);
            var parametrExpression = Expression.Parameter(typeof(T), "x"); //параметр
            Expression propertyExpression = Expression.Property(parametrExpression, property); //свойство параметра

            var lambda = Expression.Lambda(propertyExpression, parametrExpression); //нетипизированная лямбда

            var methods = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static);//все публичные и статические методы типа Queryable 
            var orderByMethod = methods.Where(m => m.Name == "OrderBy" && m.GetParameters().Count() == 2).First();
            orderByMethod = orderByMethod.MakeGenericMethod(typeof(T), propertyExpression.Type);//типизированный OrderBy

            var result = (IOrderedQueryable<T>)orderByMethod.Invoke(null, new object[] { source, lambda });

            return result;
        }

        public static IQueryable<T> WhereExp<T>(this IQueryable<T> source, string propertyName, string propertyValue)
        {
            var parameterExp = Expression.Parameter(typeof(T), "type");
            var propertyExp = Expression.Property(parameterExp, propertyName);

            MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var someValue = Expression.Constant(propertyValue, typeof(string));
            var containsMethodExp = Expression.Call(propertyExp, method, someValue);

            var lambda = Expression.Lambda<Func<T, bool>>(containsMethodExp, parameterExp);
            return source.Where(lambda);
        }
    }
}
