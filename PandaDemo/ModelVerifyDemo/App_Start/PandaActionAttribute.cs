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