using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;

namespace Library.Common
{
    public class AuthorityHelper
    {
        /// <summary>
        /// 读取xml导航权限
        /// </summary> 
        /// <returns>IList<MenuEntity></returns>
        public IList<MenuEntity> ReadAuthorityAll()
        {
            IList<MenuEntity> menuEntityList = new List<MenuEntity>();

            var path = AppDomain.CurrentDomain.BaseDirectory + "authority.xml";
            XDocument xdoc = XDocument.Load(path);
            var elements = xdoc.Root.Elements();
            foreach (var item in elements)
            {
                var menuEntity = new MenuEntity();
                menuEntity = this.GetEntityByElement(item);

                //子导航 
                var subElements = item.Elements("menu");
                foreach (var subItem in subElements)
                {
                    var subMenuEntity = new MenuEntity();
                    subMenuEntity = this.GetEntityByElement(subItem);
                    menuEntity.SubMenuEntitys.Add(subMenuEntity);
                }

                foreach (var aut in menuEntity.Authority)
                {
                    foreach (var subItem in menuEntity.SubMenuEntitys)
                    {
                        var subMenu = (MenuEntity)subItem;
                        var subAut = subMenu.Authority.FindAll(m => m.UserId == aut.UserId);
                        var count = subAut.Count;
                        if (count < 1)
                        {
                            subMenu.Authority.Add(aut);
                        }
                    }
                }

                menuEntityList.Add(menuEntity);
            }

            return menuEntityList;
        }

        /// <summary>
        /// 导航过滤
        /// </summary>
        /// <param name="list">导航集合</param>
        /// <param name="userId">用户Id</param>
        /// <param name="currentUrl">当前链接</param>
        /// <returns>IList<MenuEntity></returns>
        public IList<MenuEntity> AuthorityFilterByUser(IList<MenuEntity> list, string userId, string currentUrl)
        {
            var newList = new List<MenuEntity>();

            if (list == null || string.IsNullOrWhiteSpace(userId))
            {
                return newList;
            }

            //是否首页选中  
            var isIndexActive = true;

            foreach (var menu in list)
            {
                if (menu.Visible)
                {
                    //主菜单
                    var newMenu = new MenuEntity() { Id = menu.Id, Icon = menu.Icon, Name = menu.Name, Url = menu.Url, Visible = menu.Visible, Authority = menu.Authority, SubMenuEntitys = new List<object>() };

                    foreach (var sub in menu.SubMenuEntitys)
                    {
                        var submenu = (MenuEntity)sub;
                        submenu.IsCurrent = this.UrlMath(submenu.Url, currentUrl);

                        if (submenu.IsCurrent)
                        {
                            newMenu.IsCurrent = true;
                            isIndexActive = false;
                        }

                        var auts = submenu.Authority.FindAll(m => m.UserId == userId && m.IsAuthorize);
                        if (auts.Count >= 1)
                        {
                            newMenu.SubMenuEntitys.Add(submenu);
                        }
                    }

                    if (newMenu.SubMenuEntitys.Count >= 1 || this.UrlMath(newMenu.Url, "/Home/Index"))
                    {
                        newList.Add(newMenu);
                    }
                }
            }

            if (isIndexActive)
            {
                if (newList.Count > 0)
                {
                    newList[0].IsCurrent = true;
                }
            }

            return newList;
        }

        /// <summary>
        /// 判断用户是否有权限访问Action
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="controller">控制器名称</param>
        /// <param name="action">Action名称</param>
        /// <returns>结果</returns> 
        public bool HasActionPower(string userId, string controller, string action)
        {
            var res = false;

            var url = "/" + controller + "/" + action;
            var listMenu = ReadAuthorityAll();
            var listMenuF = AuthorityFilterByUser(listMenu, userId, url);
            foreach (var menu in listMenuF)
            {
                if (this.UrlMath(menu.Url, url))
                {
                    return true;
                }

                foreach (var sub in menu.SubMenuEntitys)
                {
                    var submenu = (MenuEntity)sub;
                    if (this.UrlMath(submenu.Url, url))
                    {
                        return true;
                    }
                }
            }

            return res;
        }

        public bool HasActionPower2(string userId, string controller, string action)
        {
            var isAuthorize = false;

            var url = "/" + controller + "/" + action;
            var path = AppDomain.CurrentDomain.BaseDirectory + "authority.xml";
            var xpath = string.Format("//menu[@url='{0}']/authority[@userId='{1}']", url, userId);
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(path);
            var node = xdoc.SelectSingleNode(xpath);
            if (node == null)
            {
                xpath = string.Format("//menu[@url='{0}']", url);
                node = xdoc.SelectSingleNode(xpath);
                if (node != null)
                {
                    node = node.ParentNode.SelectSingleNode(string.Format("authority[@userId='{0}']", userId));
                    var authorize = node.Attributes["isAuthorize"].Value;
                    bool.TryParse(authorize, out isAuthorize);
                }
                else
                {
                    isAuthorize = false;
                }

            }
            else
            {
                var authorize = node.Attributes["isAuthorize"].Value;
                bool.TryParse(authorize, out isAuthorize);
            }

            return isAuthorize;
        }
         
        /// <summary>
        /// XML节点转Entity
        /// </summary>
        /// <param name="element">节点</param>
        /// <returns>MenuEntity</returns>
        private MenuEntity GetEntityByElement(XElement element)
        {
            var menuEntity = new MenuEntity();
            menuEntity.Authority = new List<Authority>();
            menuEntity.SubMenuEntitys = new List<object>();

            menuEntity.Id = element.Attribute("id").Value;
            menuEntity.Name = element.Attribute("name").Value;
            menuEntity.Url = element.Attribute("url").Value;
            menuEntity.Icon = element.Attribute("icon").Value;
            menuEntity.Visible = Convert.ToBoolean(element.Attribute("visible").Value);

            //授权信息 
            var authoritys = element.Elements("authority");
            foreach (var autItem in authoritys)
            {
                var authority = new Authority();
                authority.UserId = autItem.Attribute("userId").Value;
                authority.IsAuthorize = Convert.ToBoolean(autItem.Attribute("isAuthorize").Value);
                menuEntity.Authority.Add(authority);
            }

            return menuEntity;
        }

        /// <summary>
        /// 正则表达式匹配url
        /// </summary>
        /// <param name="ckUrl">需要验证的url</param>
        /// <param name="url">拼接表达式的url</param>
        /// <returns>bool</returns>
        private bool UrlMath(string ckUrl, string url)
        {
            Regex reg = new Regex(string.Format(@"\w*{0}$", url));
            return reg.IsMatch(ckUrl);
        }
    }

    /// <summary>
    /// 一级导航实体类
    /// </summary>
    public class MenuEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 导航名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 导航URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 是否当前导航
        /// </summary>
        public bool IsCurrent { get; set; }

        /// <summary>
        /// 子导航集合
        /// </summary>
        public IList<Object> SubMenuEntitys { get; set; }

        /// <summary>
        /// 授权人员
        /// </summary>
        public List<Authority> Authority { get; set; }

    }

    /// <summary>
    /// 授权实体
    /// </summary>
    public class Authority
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 是否授权
        /// </summary>
        public bool IsAuthorize { get; set; }

    }

}
