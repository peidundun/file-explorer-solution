using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Extensions.UrlHelper
{
    public static partial class UrlHelperExtensions
    {
        /// <summary>
        /// Generates a fully qualified URL to an action method by using the specified action name, controller name and
        /// route values.
        /// </summary>
        /// <param name="url">The URL helper.</param>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">The route values.</param>
        /// <returns>The absolute URL.</returns>
        public static string AbsoluteAction(
            this IUrlHelper urlHelper,
            string actionName,
            string controllerName,
            object routeValues = null)
        {
            return urlHelper.Action(actionName, controllerName, routeValues, urlHelper.ActionContext.HttpContext.Request.Scheme);
        }

        /// <summary>
        /// Generates a fully qualified URL to an action method by using the specified action name, controller name and
        /// route values.
        /// </summary>
        /// <param name="url">The URL helper.</param>
        /// <returns>The absolute URL.</returns>
        public static string AbsoluteContent(
            this IUrlHelper urlHelper,
            string contentPath)
        {
            if (contentPath == null)
                return null;

            var request = urlHelper.ActionContext.HttpContext.Request;
            var url = contentPath;
            if (contentPath.StartsWith("/"))
            {
                url = new Uri(new Uri(request.Scheme + "://" + request.Host.Value), contentPath).ToString();
            }
            return url;
        }
    }
}
