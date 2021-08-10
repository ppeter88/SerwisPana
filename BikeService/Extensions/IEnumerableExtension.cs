using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeService.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> items, int selectedValue, int whichModel)
        {
            if (items == null)
            {
                return null;
            }
            /*Employee*/
            if(whichModel == 1)
            {
                return from item in items
                       select new SelectListItem
                       {
                           Text = item.GetPropertyValue("Name") + " " + item.GetPropertyValue("Surname"),
                           Value = item.GetPropertyValue("Id"),
                           Selected = item.GetPropertyValue("Id").Equals(selectedValue.ToString())
                       };
            }
            /*Customer*/
            if (whichModel == 2)
            {
                return from item in items
                       select new SelectListItem
                       {
                           Text = item.GetPropertyValue("Name") + " -> " + item.GetPropertyValue("Phone"),
                           Value = item.GetPropertyValue("Id"),
                           Selected = item.GetPropertyValue("Id").Equals(selectedValue.ToString())
                       };
            }

            return null;
            
        }
    }
}
