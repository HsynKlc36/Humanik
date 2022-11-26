using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.ENTITIES.Enums
{
    public enum Title
    {
        Anonim=1, Limited, Kollektif, Komandit, Kooperatif //view lerde input değilde enum select ile seçim yaparken enum değerlerini 1 den başlatmak oluşabilecek hataları önleyebilir. özellikle 0'dan başlarsa  boş olarak görünebilir(empty) ve notempty kolonlarında bu durum hataya neden olabilir.
    }
}
