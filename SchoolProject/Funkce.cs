using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SchoolProject
{
    public class Funkce
    {
        public static bool GoBack(string dotaz)     //Tato funkce kontroluje a vrací true pokud bylo zadáno "0".
        {
            Console.Write(dotaz);
            char back = (Console.ReadKey().KeyChar);
            if (back == '0')
            {
                return true;
            }
            return false;
        }

        public static string Velikost(string dotaz, int min)    //Tato funkce kontroluje jestli je vstup alespoň 3 znaky dlouhý nebo se zadalo "0".
        {
            while (true)
            {
                Console.Write(dotaz);
                string vstup = (Console.ReadLine() ?? "").Trim();

                if (vstup == "0")
                    return "0";
                else if (vstup.Length >= min)
                    return vstup;

                Console.WriteLine($"Musite zadat alespon {min} znaky");
            }
        }
    }
}