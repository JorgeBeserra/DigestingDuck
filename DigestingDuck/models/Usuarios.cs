using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigestingDuck.models
{
    public class Usuarios : RealmObject
    {
        [PrimaryKey]
        public long Id { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Avatar { get; set; }
        public DateTimeOffset UltimoAcessoDataHora { get; set; }
        
    }
}
