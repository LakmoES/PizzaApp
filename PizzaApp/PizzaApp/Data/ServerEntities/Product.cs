using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data.ServerEntities
{
    public class Product
    {
        public int id { get; set; }
        public string title { get; set; }
        public string measure { get; set; }
        public int category { get; set; }
        public Decimal cost { get; set; }
        public int available { get; set; }
        public int advertising  { get; set; }
        public override string ToString()
        {
            return String.Format("[{0}] {1} {2} " + Environment.NewLine + "категория: {3}" + Environment.NewLine + "{4} грн" + Environment.NewLine + "Доступно:{5}" + Environment.NewLine + "Акционный:{6}", this.id, this.title, this.measure, this.category, this.cost, this.available, this.advertising);
        }
    }
}
