using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudget.App.Models
{
    public class IconCategory
    {
        public string CategoryName { get; set; } = string.Empty;
        public List<IconItem> Icons { get; set; } = new List<IconItem>();
    }
}
