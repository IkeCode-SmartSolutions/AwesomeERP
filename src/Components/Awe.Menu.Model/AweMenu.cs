﻿using Awe.Core.Crypto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Awe.Menu
{
    public class AweMenu
    {
        public string Parent { get; set; }
        public string Title { get; set; }
        public string Hint { get; set; }
        public int Order { get; set; }
        public string RouteName { get; set; }

        public string ControllerName
        {
            get
            {
                var split = RouteName.Split('#');
                if (split.Length > 1)
                {
                    return split[0].Replace("Controller", string.Empty).Substring(1);
                }
                else return "";
            }
        }

        public string ActionName
        {
            get
            {
                var split = RouteName.Split('#');
                if (split.Length > 1)
                {
                    return split[1];
                }
                else return "";
            }
        }

        public string Icon { get; set; }
        public List<AweMenu> Children { get; set; }

        public AweMenu()
        {

        }

        public AweMenu(AweMenu menu)
        {
            Parent = menu.Parent;
            Title = menu.Title;
            Hint = menu.Hint;
            Order = menu.Order;
            RouteName = menu.RouteName;
            Icon = menu.Icon;
            Children = menu.Children;
        }

        private AweMenu(string parent, string title, string hint, int order = 0, string icon = "")
        {
            Parent = parent;
            Title = title;
            Hint = hint;
            Order = order;
            Icon = icon;
        }

        public AweMenu(string routeName, string parent, string title, string hint, int order = 0, string icon = "")
            : this(parent, title, hint, order, icon)
        {
            RouteName = routeName;
        }

        public AweMenu(string controller, string action, string parent, string title, string hint, int order = 0, string icon = "")
            : this(parent, title, hint, order, icon)
        {
            RouteName = $"${controller}#{action}";
        }
    }
}
