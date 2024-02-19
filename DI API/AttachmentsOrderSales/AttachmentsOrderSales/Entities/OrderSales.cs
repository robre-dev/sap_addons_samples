using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttachmentsOrderSales.Entities
{
    public class OrderSales
    {
        public int DocEntry { get; set; }

        public DateTime DocDate { get; set; }

        public string CardCode { get; set; }

        public List<OrderSalesDetails> OrderSalesDetails { get; set; }

        public List<Atachments> Atachments { get; set; }

       
    }
}
