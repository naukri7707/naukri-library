using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Naukri.Reflection
{
    #region -- Property --

    /// <summary>
    /// Getter 委派
    /// </summary>
    /// <typeparam name="TObject">實例型態</typeparam>
    /// <typeparam name="TResult">取值型態</typeparam>
    /// <param name="target">目標實例</param>
    /// <returns></returns>
    public delegate TResult GetterDelegate<TObject, TResult>(TObject target);

    /// <summary>
    /// Setter 委派
    /// </summary>
    /// <typeparam name="TObject">實例型態</typeparam>
    /// <typeparam name="TValue">賦值型態</typeparam>
    /// <param name="target">目標實例</param>
    /// <param name="value"></param>
    /// <returns></returns>
    public delegate void SetterDelegate<TObject, TValue>(TObject target, TValue value);

    public static class FastReflection
    {
        public static GetterDelegate<TObject, TValue> CreateGetterDelegate<TObject, TValue>(this PropertyInfo self)
        {
            var objectType = typeof(TObject);
            var valueType = typeof(TValue);

            var instanceParam = Expression.Parameter(objectType);

            return
                Expression.Lambda<GetterDelegate<TObject, TValue>>( // 建立方法
                    Expression.Convert(                             // 先用 Call 取值 再用 Convert 轉成目標型態
                        Expression.Call(instanceParam, self.GetGetMethod()),
                        valueType
                        ),
                    instanceParam
                    ).Compile();
        }

        public static SetterDelegate<TObject, TValue> CreateSetterDelegate<TObject, TValue>(this PropertyInfo self)
        {
            var objectType = typeof(TObject);
            var valueType = typeof(TValue);

            var instanceParam = Expression.Parameter(objectType);
            var argumentParam = Expression.Parameter(valueType);

            return
                Expression.Lambda<SetterDelegate<TObject, TValue>>(
                    Expression.Call(                                // 先用 Convert 轉型值 再用 Call 賦值
                        instanceParam,
                        self.GetSetMethod(),
                        Expression.Convert(argumentParam, valueType)
                        ),
                    instanceParam,
                    argumentParam
                    ).Compile();
        }

        #endregion

        #region -- Method --

        #region -- Action --

        public static Action<TObject> CreatActionDelegate<TObject>(this MethodInfo self)
        {
            var objectType = typeof(TObject);

            var instance = Expression.Parameter(objectType);

            return
                Expression.Lambda<Action<TObject>>(
                    Expression.Call(
                        instance,
                        self
                        ),
                    instance
                    ).Compile();
        }

        public static Action<TObject, TParam> CreatActionDelegate<TObject, TParam>(this MethodInfo self)
        {
            var objectType = typeof(TObject);
            var paramType = typeof(TParam);

            var instanceParam = Expression.Parameter(objectType);
            var argumentParam = Expression.Parameter(paramType);

            return
                Expression.Lambda<Action<TObject, TParam>>(
                    Expression.Call(                        // 先用 Convert 轉型值 再用 Call 賦值
                        instanceParam,
                        self,
                        Expression.Convert(argumentParam, paramType)
                        ),
                    instanceParam,
                    argumentParam
                    ).Compile();
        }

        public static Action<TObject, TParam1, TParam2> CreatActionDelegate<TObject, TParam1, TParam2>(this MethodInfo self)
        {
            var (argumentParams, convertArgumentParams) = CreateExpressionList(typeof(TObject), typeof(TParam1), typeof(TParam2));
            return
                Expression.Lambda<Action<TObject, TParam1, TParam2>>(
                    Expression.Call(
                        argumentParams[0],
                        self,
                        convertArgumentParams.Skip(1)
                        ),
                    argumentParams
                    ).Compile();
        }

        public static Action<TObject, TParam1, TParam2, TParam3> CreatActionDelegate<TObject, TParam1, TParam2, TParam3>(this MethodInfo self)
        {
            var (argumentParams, convertArgumentParams) = CreateExpressionList(typeof(TObject), typeof(TParam1), typeof(TParam2), typeof(TParam3));
            return
                Expression.Lambda<Action<TObject, TParam1, TParam2, TParam3>>(
                    Expression.Call(
                        argumentParams[0],
                        self,
                        convertArgumentParams.Skip(1)
                        ),
                    argumentParams
                    ).Compile();
        }

        public static Action<TObject, TParam1, TParam2, TParam3, TParam4> CreatActionDelegate<TObject, TParam1, TParam2, TParam3, TParam4>(this MethodInfo self)
        {
            var (argumentParams, convertArgumentParams) = CreateExpressionList(typeof(TObject), typeof(TParam1), typeof(TParam2), typeof(TParam3), typeof(TParam4));
            return
                Expression.Lambda<Action<TObject, TParam1, TParam2, TParam3, TParam4>>(
                    Expression.Call(
                        argumentParams[0],
                        self,
                        convertArgumentParams.Skip(1)
                        ),
                    argumentParams
                    ).Compile();
        }

        public static Action<TObject, TParam1, TParam2, TParam3, TParam4, TParam5> CreatActionDelegate<TObject, TParam1, TParam2, TParam3, TParam4, TParam5>(this MethodInfo self)
        {
            var (argumentParams, convertArgumentParams) = CreateExpressionList(typeof(TObject), typeof(TParam1), typeof(TParam2), typeof(TParam3), typeof(TParam4), typeof(TParam5));
            return
                Expression.Lambda<Action<TObject, TParam1, TParam2, TParam3, TParam4, TParam5>>(
                    Expression.Call(
                        argumentParams[0],
                        self,
                        convertArgumentParams.Skip(1)
                        ),
                    argumentParams
                    ).Compile();
        }

        #endregion

        #region -- Func --
        public static Func<TObject, TResult> CreatFuncDelegate<TObject, TResult>(this MethodInfo self)
        {
            var objectType = typeof(TObject);
            var resultType = typeof(TResult);

            var instanceParam = Expression.Parameter(objectType);

            return
                Expression.Lambda<Func<TObject, TResult>>(
                    Expression.Convert(
                        Expression.Call(                        // 先用 Convert 轉型值 再用 Call 賦值
                            instanceParam,
                            self
                            ),
                        resultType
                        ),
                    instanceParam
                    ).Compile();
        }

        public static Func<TObject, TParam1, TResult> CreatFuncDelegate<TObject, TParam1, TResult>(this MethodInfo self)
        {
            var (argumentParams, convertArgumentParams) = CreateExpressionList(typeof(TObject), typeof(TParam1));
            var resultType = typeof(TResult);
            return
                Expression.Lambda<Func<TObject, TParam1, TResult>>(
                    Expression.Convert(
                        Expression.Call(
                            argumentParams[0],
                            self,
                            convertArgumentParams.Skip(1)
                            ),
                        resultType
                        ),
                    argumentParams
                    ).Compile();
        }

        public static Func<TObject, TParam1, TParam2, TResult> CreatFuncDelegate<TObject, TParam1, TParam2, TResult>(this MethodInfo self)
        {
            var (argumentParams, convertArgumentParams) = CreateExpressionList(typeof(TObject), typeof(TParam1), typeof(TParam2));
            var resultType = typeof(TResult);
            return
                Expression.Lambda<Func<TObject, TParam1, TParam2, TResult>>(
                    Expression.Convert(
                        Expression.Call(
                            argumentParams[0],
                            self,
                            convertArgumentParams.Skip(1)
                            ),
                        resultType
                        ),
                    argumentParams
                    ).Compile();
        }

        public static Func<TObject, TParam1, TParam2, TParam3, TResult> CreatFuncDelegate<TObject, TParam1, TParam2, TParam3, TResult>(this MethodInfo self)
        {
            var (argumentParams, convertArgumentParams) = CreateExpressionList(typeof(TObject), typeof(TParam1), typeof(TParam2), typeof(TParam3));
            var resultType = typeof(TResult);
            return
               Expression.Lambda<Func<TObject, TParam1, TParam2, TParam3, TResult>>(
                   Expression.Convert(
                       Expression.Call(
                           argumentParams[0],
                           self,
                           convertArgumentParams.Skip(1)
                           ),
                       resultType
                       ),
                   argumentParams
                   ).Compile();
        }

        public static Func<TObject, TParam1, TParam2, TParam3, TParam4, TResult> CreatFuncDelegate<TObject, TParam1, TParam2, TParam3, TParam4, TResult>(this MethodInfo self)
        {
            var (argumentParams, convertArgumentParams) = CreateExpressionList(typeof(TObject), typeof(TParam1), typeof(TParam2), typeof(TParam3), typeof(TParam4));
            var resultType = typeof(TResult);
            return
               Expression.Lambda<Func<TObject, TParam1, TParam2, TParam3, TParam4, TResult>>(
                   Expression.Convert(
                       Expression.Call(
                           argumentParams[0],
                           self,
                           convertArgumentParams.Skip(1)
                           ),
                       resultType
                       ),
                   argumentParams
                   ).Compile();
        }

        #endregion

        public static (ParameterExpression[] argumentParams, UnaryExpression[] convertArgumentParams) CreateExpressionList(params Type[] types)
        {
            var argumentParams = types.Select(it => Expression.Parameter(it)).ToArray();
            var convertArgumentParams = argumentParams.Select(it => Expression.Convert(it, it.Type)).ToArray();
            return (argumentParams, convertArgumentParams);
        }

        #endregion
    }
}
