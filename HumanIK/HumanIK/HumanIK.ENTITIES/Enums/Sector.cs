using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.ENTITIES.Enums
{
    public enum Sector
    {
        Eğitim=1,
        [Display(Name = "Elektrik-Elektronik")]
        Elektrik, 
        Finans, 
        Gıda, 
        İnşaat, 
        Maden, 
        Metal, 
        Otomotiv, 
        Seramik,
        Polimer,
        İletişim,
        Tekstil,
        Teknoloji,
        [Display(Name = "Ulaştırma,Lojistik ve Haberleşme")]
        Ulaştırma,
        Kimya,
        [Display(Name = "Sağlık ve Sosyal Hizmetler")]
        Sağlık,
        [Display(Name = "Turizm, Konaklama, Yiyecek-İçecek Hizmetleri")]
        Turizm
    }
}
