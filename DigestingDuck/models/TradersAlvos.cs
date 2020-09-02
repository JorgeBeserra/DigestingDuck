using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigestingDuck.models
{
    public class TradersAlvos : RealmObject
    {
        [PrimaryKey]
        public int Id { get; set; }
        public long UserId { get; set; }
    }
}
