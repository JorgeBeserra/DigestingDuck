using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DigestingDuck.Models
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
