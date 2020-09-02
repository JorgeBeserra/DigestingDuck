using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigestingDuck.models
{
    public class Configuracao : RealmObject
    {
        [PrimaryKey]
        public int Id { get; set; }
        public DateTimeOffset UltimaExecucao { get; set; }
        public string Serial { get; set; }
        public long UltimoUsuario { get; set; }
        public bool LembrarDados { get; set; }
    }
}
