using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ModelVerifyDemo.Filter
{
    public class PandaActionAttribute : IActionFilter
    {
        /// <summary>
        /// 在执行操作方法之前调用
        /// </summary>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //请求参数去空格(模型绑定无效)
            ParameterDescriptor[] parameters = filterContext.ActionDescriptor.GetParameters();
            foreach (ParameterDescriptor parameter in parameters)
            {
                if (parameter.ParameterType == typeof(string))
                {
                    filterContext.ActionParameters[parameter.ParameterName] = (filterContext.ActionParameters[parameter.ParameterName] as string)?.Trim();
                }
            }

            //模型验证
            //if (actionContext.ModelState.IsValid == false)
            //{
            //    // Return the validation errors in the response body.
            //    // 在响应体中返回验证错误
            //    var errors = new Dictionary<string, IEnumerable<string>>();
            //    foreach (KeyValuePair<string, ModelState> keyValue in actionContext.ModelState)
            //    {
            //        errors[keyValue.Key] = keyValue.Value.Errors.Select(e => e.ErrorMessage);
            //    }

            //    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            //}
        }

        /// <summary>
        /// 在执行操作方法后调用
        /// </summary>
        /// <param name="filterContext">筛选器上下文</param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //不需要，留空
        }
    }
}