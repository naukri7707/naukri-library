using System;
using System.Linq.Expressions;

namespace Naukri.Reflection
{
    /// <summary>
    /// 轉型成<see cref="TResult"/>
    /// </summary>
    /// <typeparam name="TResult">目標型態</typeparam>
    public static class CastTo<TResult>
    {
        private static class Caster<TSource>
        {
            // 轉型用委派
            public static readonly Func<TSource, TResult> Cast = GetDelegate();

            private static Func<TSource, TResult> GetDelegate()
            {
                var instanceParam = Expression.Parameter(typeof(TSource));
                return
                    Expression.Lambda<Func<TSource, TResult>>
                    (
                        Expression.ConvertChecked
                        (
                            instanceParam,
                            typeof(TResult)
                        ),
                        instanceParam
                    ).Compile();
            }
        }

        /// <summary>
        /// 將 <see cref="TSource"/> 轉型為 <see cref="TResult"/>
        /// </summary>
        /// <typeparam name="TSource">原始型態</typeparam>
        public static TResult From<TSource>(TSource src)
        {
            return Caster<TSource>.Cast(src);
        }

    }
}