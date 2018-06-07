using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JustSaying.Models;

namespace Publisher
{
    public class BulkUpload : Message
    {
        public BulkUpload(int orderInfo)
        {
            OrderInfo = orderInfo;
        }
        public int OrderInfo { get; private set; }
    }
}
