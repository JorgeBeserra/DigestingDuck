using MongoDB.Bson;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigestingDuck.Models
{
    public class SinaisLista : RealmObject
    {
        [PrimaryKey]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public bool Status { get; set; }
        public string Descricao { get; set; }
        public IList<SinaisSinal> ListOfSinais { get; }

        [Ignored]
        public string StatusImage
        {
            get
            {
                if (Status)
                    return "../Images/check.png";
                return "../Images/uncheck.png";
            }
        }
    }
}
