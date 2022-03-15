using System;
using System.Collections.Generic;
using System.Linq;
using Wing.Model;
using Wing.Result;

namespace Wing.Dashboard.Helper
{
    public static class Paged
    {
        public static PageResult<List<TResult>> ToPage<TSource, TResult>(this List<TSource> data,
                                                                        Func<TSource, bool> where,
                                                                        PageModel dto,
                                                                        Action<TSource, List<TResult>> addItem)
                                                                where TSource : class, new()
                                                                where TResult : class, new()
        {
            var result = new PageResult<List<TResult>>();
            if (data == null || data.Count == 0)
            {
                result.TotalCount = 0;
                result.Items = null;
                return result;
            }

            var whereResult = new List<TSource>();
            if (where == null)
            {
                whereResult = data;
            }
            else
            {
                foreach (var item in data)
                {
                    if (where(item))
                    {
                        whereResult.Add(item);
                    }
                }
            }

            result.TotalCount = whereResult.Count();
            result.Items = new List<TResult>();
            var i = 0;
            foreach (var item in whereResult)
            {
                if (result.Items.Count >= dto.PageSize)
                {
                    break;
                }

                i++;
                if (i > dto.PageSize * (dto.PageIndex - 1) && i <= dto.PageSize * dto.PageIndex)
                {
                    addItem(item, result.Items);
                }
            }

            return result;
        }

        public static PageResult<Dictionary<TKey, TValue>> ToPage<TKey, TValue>(this Dictionary<TKey, TValue> data,
                                                                        Func<KeyValuePair<TKey, TValue>, bool> where,
                                                                        PageModel dto,
                                                                        Action<KeyValuePair<TKey, TValue>, Dictionary<TKey, TValue>> addItem)
        {
            var result = new PageResult<Dictionary<TKey, TValue>>();
            if (data == null || data.Count == 0)
            {
                result.TotalCount = 0;
                result.Items = null;
                return result;
            }

            var whereResult = new Dictionary<TKey, TValue>();
            if (where == null)
            {
                whereResult = data;
            }
            else
            {
                foreach (var item in data)
                {
                    if (where(item))
                    {
                        whereResult.Add(item.Key, item.Value);
                    }
                }
            }

            result.TotalCount = whereResult.Count();
            result.Items = new Dictionary<TKey, TValue>();
            var i = 0;
            foreach (var item in whereResult)
            {
                if (result.Items.Count >= dto.PageSize)
                {
                    break;
                }

                i++;
                if (i > dto.PageSize * (dto.PageIndex - 1) && i <= dto.PageSize * dto.PageIndex)
                {
                    addItem(item, result.Items);
                }
            }

            return result;
        }
    }
}
