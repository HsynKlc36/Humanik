using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanIK.BUSINESS.Concrete
{
    public static class EmailCreator
    {
        //public static string Create(string name, string lastName, string companyName)
        //{
        //    string mail;
        //    if ((name.Length + lastName.Length) > 20)
        //    {
        //        mail = name[0] + lastName + "@";

        //        if (companyName.Length<=20)
        //            mail += companyName.Replace(" ", string.Empty) + ".com";
        //        else
        //            mail += companyName.Replace(" ", string.Empty).Substring(0,20) + ".com";

        //        return mail.ToLower();
        //    }
        //    else
        //    {

        //        mail = name + "." + lastName + "@";
        //        if (companyName.Length<= 20)
        //            mail += companyName.Replace(" ", string.Empty) + ".com";

        //        else
        //            mail += companyName.Replace(" ", string.Empty).Substring(0,20) + ".com";


        //        return mail.ToLower();

        //    }

        //}

        public static string Create(string name, string lastName, string mailExtension)
        {
            string mail;
            if ((name.Length + lastName.Length) > 20)
            {
                mail = name[0] + lastName + "@";

                if (mailExtension.Length <= 20)
                    mail += mailExtension.Replace(" ", string.Empty);
                else
                    mail += mailExtension.Replace(" ", string.Empty).Substring(0, 20);

                return mail.ToLower();
            }
            else
            {

                mail = name + "." + lastName + "@";
                if (mailExtension.Length <= 20)
                    mail += mailExtension.Replace(" ", string.Empty);

                else
                    mail += mailExtension.Replace(" ", string.Empty).Substring(0, 20);


                return mail.ToGlobal().ToLower().Replace('ı', 'i');

            }
        }
    }
}
