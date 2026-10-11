using System;
using System.Collections.Generic;
using System.Text.Json;

namespace SchoolProject
{
    internal class MainProgram
    {
        static void Main(string[] args)
        {
            while (true)
            {
                //Toto mi pomohl vygenerovat Gemini pro, promt: Jak v C# vytvořím novou složku v adresáři, kde se mi automaticky ukládá json a jak pak k ní najdu cestu.
                string defaultSoubor = Directory.GetCurrentDirectory();
                string souborZalozek = Path.Combine(defaultSoubor, "MojeZalozky");
                if (!Directory.Exists(souborZalozek))
                {
                    Directory.CreateDirectory(souborZalozek);
                }

                //Zde se vypíší všechny už vytvořené záložky uživatelem nebo se dá uživateli informace o tom, že zatím žádné záložky nebyli vytvořeny.
                string[] soubory = Directory.GetFiles(souborZalozek, "*.json");

                if (soubory.Length == 0)
                {
                    Console.WriteLine("Zatím nemáte žádné záložky.\n");
                }
                else
                {
                    Console.WriteLine("Existující záložky:");
                    for (int i = 0; i < soubory.Length; i++)
                    {
                        Console.WriteLine($"{Path.GetFileNameWithoutExtension(soubory[i])}");
                    }
                    Console.WriteLine("\n-----------------------------------------");
                }
                

                //Uživatel si vymyslí nebo zadá už vymyšlenej název záložky pro úkládaní podobného druhu informací.
                //Vstup je zároveň opatřen aby uživatel nemohl zadat název záložky kratší než 3 znaky.
                Console.WriteLine("Pokud si přeješ vypnout program napiš [0]");
                string zalozka = Funkce.Velikost("Zadej jméno záložky: ", 3);
                if (zalozka == "0")                                                 
                    break;

                //Vytvoření listu a souboru, který se pojmenuje podle názvu záložky.
                string nazevSouboru = Path.Combine(souborZalozek, $"{zalozka}" + ".json");
                List<Ukol> todolist = new();

                if (File.Exists(nazevSouboru))  //Vyčtení dat ze souboru pokud existuje, jinak o tom dá vědět uživateli.
                {
                    string nacitaniSouboru = File.ReadAllText(nazevSouboru);
                    todolist = JsonSerializer.Deserialize<List<Ukol>>(nacitaniSouboru) ?? new();
                    Console.WriteLine($"Na vaše jméno máte uložene {todolist.Count} úkoly");
                }
                else
                {
                    Console.WriteLine("Zatím nemáte žádné úkoly");
                }

                int akce;
                bool behProgramu = true;

                //HLAVNÍ PROGRAM
                while (behProgramu)
                {
                    bool behAkce = true;
                    Console.Clear();

                    //MENU
                    Console.WriteLine("====================");
                    Console.WriteLine("||   TO-DO LIST   ||");
                    Console.WriteLine("====================");
                    Console.WriteLine($"| Záložka {zalozka}");
                    Console.WriteLine("--------------------");
                    Console.WriteLine("| Přehled      [1] |");
                    Console.WriteLine("--------------------");
                    Console.WriteLine("| Přidat       [2] |");
                    Console.WriteLine("| Spravovat    [3] |");
                    Console.WriteLine("--------------------");
                    Console.WriteLine("| KONEC        [0] |");
                    Console.WriteLine("--------------------\n");
                    Console.Write("Zadej svou akci: ");
                    akce = (Console.ReadKey().KeyChar);

                    //PODPROGRAMY
                    while (behAkce)
                    {
                        switch (akce)
                        {
                            case '0': //Slouží pro ukončování podprogramu.
                                behAkce = false;
                                behProgramu = false;
                                Console.Clear();
                                break;
                            case '1':   //VNITŘEK PŘEHLEDU - Zde se uživateli vypíší všechny uložené údaje.
                                Console.Clear();

                                Console.WriteLine("===================================");
                                Console.WriteLine("||            Přehled            ||");
                                Console.WriteLine("===================================");

                                if (todolist.Count == 0)    //Oznámení pro uživatele pokud ještě žádný list nevytvořil.
                                {
                                    Console.WriteLine("\n* Do této záložky zatím nebyly přidány žádne úkoly *");
                                }
                                else
                                {
                                    Console.WriteLine($"Počet uložených úkolu: {todolist.Count}\n");
                                    Console.WriteLine("-----------------------------------");
                                    foreach (Ukol u in todolist)    //Výpis listu.
                                    {
                                        string datum = u.Termin.HasValue ? u.Termin.Value.ToString("d.M.yyyy") : "neni";
                                        string dulezitost = u.Dulezitost ? "DŮLEŽITÝ!" : "nedůležity";
                                        Console.WriteLine($"Předmět  : {u.Predmet}");
                                        Console.WriteLine($"Status   : {u.Status}");
                                        Console.WriteLine($"Termín   : {datum}");
                                        Console.WriteLine($"Priorita : {dulezitost}");
                                        Console.WriteLine("-----------------------------------");
                                    }
                                }

                                if (Funkce.GoBack("\nPro vráceni do Menu stiskni [0]"))     //Vyvolání funkce pro návrat do menu po stisknutí "0".
                                {
                                    behAkce = false;
                                }
                                break;
                            case '2':   //VNITŘEK ZÁPISU
                                while (true)
                                {
                                    Console.Clear();

                                    Console.WriteLine("===================================");
                                    Console.WriteLine("||       Přidat nový zápis       ||");
                                    Console.WriteLine("===================================");

                                    //PŘEDMĚT - Uživatel zadá název předmětu, který si chce zaznamenat, a jako u jména musí být delší jak 3 znaky.
                                    Console.WriteLine("\n* Pokud už nebudeš chtít nic přidat a vrátit se do Menu stiskni [0] *\n");
                                    string predmet = Funkce.Velikost("Zadej název úkolu: ", 3);
                                    if (predmet == "0")     //Návrat do menu.
                                    {
                                        behAkce = false;
                                        break;
                                    }

                                    //STATUS - Přes enum má uživatel na výber ze tří možností v jakém stádiu se úkol nachází.
                                    Status status;
                                    while (true)
                                    {
                                        Console.Clear();
                                        Console.Write("Jaký je status předmětu? čeká | probíhá | hotovo: ");
                                        string volbastatus = (Console.ReadLine() ?? "").ToLower();
                                        if (Enum.TryParse(volbastatus, out status))
                                            break;
                                        else
                                            Console.WriteLine("Zadal jsi neplatnou odpoveď, zkus to znovu.\n");
                                        Console.ReadKey();
                                    }

                                    //TERMÍN - Zde uživatel zadává termín do kdy má být úkol splněn nebo kdy byl splněn.
                                    // - Vstup je opatřen aby uživatel musel zadat datum přesně v tomto formátu (DD.MM.YYYY).
                                    // - Dále je tam opatření, když v předchozím kroku uživatel vybere že úkol čeká nebo probíhá, tak se nemůže zadat datum co už bylo, kromě statusu hotovo.
                                    DateTime? datum = null;
                                    while (true)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\nPokud datum nevyžadujete napište 'není'.");
                                        Console.Write("\nDo kdy je termín úkolu? (D.M.YYYY): ");
                                        string volbaTermin = (Console.ReadLine() ?? "").ToLower();

                                        if (!DateTime.TryParseExact(volbaTermin, "d.M.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime novyDatum))
                                        {
                                            if (volbaTermin == "není")
                                            {
                                                datum = null;
                                                break;
                                            }

                                            Console.WriteLine("Zadal jsi neplatný datum, zkus to znovu.\n");
                                            Console.ReadKey();
                                            continue;
                                        }
                                        else if ((novyDatum.Date > DateTime.Today) || (status == Status.hotovo))
                                        {
                                            datum = novyDatum;
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Zadal jsi neplatný datum, zkus to znovu.\n");
                                            Console.ReadKey();
                                        }
                                    }

                                    //DŮLEŽITOST - Zde má uživatel na výběr jenom 2 možnosti a to ano pokud je úkol důležitý nebo ne pokud není.
                                    // - Vrací se true nebo false, který se pak ve výpisu přepíše na DŮLEŽITÝ! nebo nedůležitý.
                                    // - Opatřeno aby uživatel nemohl zadat nic krom "ano" nebo "ne".
                                    bool dulezitost;
                                    while (true)
                                    {
                                        Console.Clear();
                                        Console.Write("\nJe tento úkol důležitý? Napiš ano/ne: ");
                                        string volbaDulezitost = (Console.ReadLine() ?? "").ToLower();
                                        if (volbaDulezitost == "ano")
                                        {
                                            dulezitost = true;
                                            break;
                                        }
                                        else if (volbaDulezitost == "ne")
                                        {
                                            dulezitost = false;
                                            break;
                                        }
                                        else
                                            Console.WriteLine("Musíš zadat buď 'ano' nebo 'ne'.");
                                        Console.ReadKey();
                                    }

                                    todolist.Add(new Ukol   //Uložení všech údajů do listu.
                                    {
                                        Predmet = predmet,
                                        Status = status,
                                        Termin = datum,
                                        Dulezitost = dulezitost
                                    });

                                    Console.WriteLine("\n* Úkol byl úspešně přidán! *\n");
                                    Console.ReadKey();
                                }
                                string jsonText = JsonSerializer.Serialize(todolist);   //Zde se list převede do .json a uloží se jako nový soubor se všemi údaji.
                                File.WriteAllText(nazevSouboru, jsonText);

                                break;
                            case '3':   //VNITŘEK SPRAVOVÁNÍ - Zde si uživatel podle lokálního ID může smazat uložené úkoly, které se následně uloží do nového listu.
                                while (true)
                                {
                                    Console.Clear();

                                    Console.WriteLine("===================================");
                                    Console.WriteLine("||           Spravovat           ||");
                                    Console.WriteLine("===================================\n");

                                    if (todolist.Count == 0)   //Oznámení pro uživatele pokud ještě žádný list nevytvořil.
                                    {
                                        Console.WriteLine("* Do této záložky zatím nebyly přidány žádné úkoly *");
                                        if (Funkce.GoBack("\nPro vrácení do Menu stiskni [0]"))
                                        {
                                            behAkce = false;
                                        }
                                        break;
                                    }

                                    for (int i = 0; i < todolist.Count; i++)
                                    {
                                        string datum = todolist[i].Termin ?. ToString("d.M.yyyy") ?? "není";
                                        Console.WriteLine($"[{i + 1}] {todolist[i].Predmet} — {datum}");
                                    }

                                    Console.Write($"\nZadej číslo úkolu 1 až {todolist.Count}, který chceš smazat nebo [0] pro návrat do Menu: ");      //Uživatel vybere ID úkolu, který chce smazat.
                                    if (!int.TryParse(Console.ReadLine(), out int volba) || volba < 0 || volba > todolist.Count)                        //Pokud zadá 0 vrátí se do menu, cokoliv jiného nahlásí chybu.
                                    {
                                        Console.Write($"Tento úkol neexistuje, zadej 1 až {todolist.Count}");
                                        Console.ReadKey();
                                        continue;
                                    }

                                    if (volba == 0)     //návrat do menu.
                                    {
                                        behAkce = false;
                                        break;
                                    }

                                    Console.WriteLine($"\nÚkol '{todolist[volba - 1].Predmet}' byl smazán.");
                                    todolist.RemoveAt(volba - 1);                                           //Zde se smaže požadovaný úkol.
                                    File.WriteAllText(nazevSouboru, JsonSerializer.Serialize(todolist));    //Nový list se smazaným úkolem se uloží jako nový.
                                    Console.WriteLine("\nSeznam úkolů byl uložen");
                                    Console.ReadKey();
                                    break;
                                }
                                break;
                        default:
                            Console.WriteLine(" - Zadal jsi neplatnou akci.");  //Hlášení pokud uživatel zadal neplatnou volbu v Menu.
                            Console.ReadKey();
                            behAkce = false;
                            break;
                        }
                    }
                }
            }
        }
    }
}