using System;

class Program
{
    static void Main(string[] args)
    {
        Parking parking = new Parking(5, 6);
        bool dziala = true;

        while (dziala)
        {
            Console.WriteLine("\n=== MENU OBSŁUGI PARKINGU ===");
            Console.WriteLine("1. Dodaj pojazd");
            Console.WriteLine("2. Usuń pojazd");
            Console.WriteLine("3. Wyświetl status parkingu");
            Console.WriteLine("4. Zapisz do pliku");
            Console.WriteLine("5. Wczytaj z pliku");
            Console.WriteLine("6. Wyświetl wszystkie pojazdy");
            Console.WriteLine("7. Wyszukaj pojazd po nr rejestracyjnym");
            Console.WriteLine("8. Wyświetl historię (logi)");
            Console.WriteLine("9. Edytuj nr rejestracyjny");
            Console.WriteLine("10. Wyjście");
            Console.Write("Wybierz opcję: ");

            string wybor = Console.ReadLine();

            switch (wybor)
            {
                case "1":
                    try
                    {
                        Console.WriteLine("Jaki typ pojazdu?");
                        Console.WriteLine("1. Motocykl (1 miejsce)");
                        Console.WriteLine("2. Samochód (2 miejsca)");
                        Console.WriteLine("3. Autobus (4 miejsca 2x2)");
                        Console.WriteLine("4. Ciężarówka (3 miejsca)");
                        Console.Write("Wybór: ");
                        string typ = Console.ReadLine();

                        Console.Write("Nr rejestracyjny: ");
                        string nrRej = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(nrRej))
                        {
                            Console.WriteLine("Nr rejestracyjny nie może być pusty!");
                            break;
                        }

                        Console.Write($"Wiersz (0-{parking.LiczbaWierszy - 1}): ");
                        int w = int.Parse(Console.ReadLine());

                        Console.Write($"Kolumna (0-{parking.LiczbaKolumn - 1}): ");
                        int k = int.Parse(Console.ReadLine());

                        if (w < 0 || w >= parking.LiczbaWierszy || k < 0 || k >= parking.LiczbaKolumn)
                        {
                            Console.WriteLine(" Współrzędne poza parkingiem!");
                            break;
                        }

                        Pojazd pojazd = null;

                        if (typ == "1")
                        {
                            pojazd = new Motocykl(nrRej, w, k);
                        }
                        else if (typ == "2")
                        {
                            if (k + 1 >= parking.LiczbaKolumn)
                            {
                                Console.WriteLine(" Samochód nie zmieści się (potrzebuje 2 kolumny)!");
                                break;
                            }
                            pojazd = new Samochod(nrRej, w, k);
                        }
                        else if (typ == "3")
                        {
                            if (w + 1 >= parking.LiczbaWierszy || k + 1 >= parking.LiczbaKolumn)
                            {
                                Console.WriteLine(" Autobus nie zmieści się (potrzebuje 2x2 miejsca)!");
                                break;
                            }
                            pojazd = new Autobus(nrRej, w, k);
                        }
                        else if (typ == "4")
                        {
                            if (k + 2 >= parking.LiczbaKolumn)
                            {
                                Console.WriteLine(" Ciężarówka nie zmieści się (potrzebuje 3 kolumny)!");
                                break;
                            }
                            pojazd = new Ciezarowka(nrRej, w, k);
                        }
                        else
                        {
                            Console.WriteLine(" Nieprawidłowy typ pojazdu!");
                            break;
                        }

                        if (pojazd != null)
                        {
                            if (parking.DodajPojazd(pojazd))
                                Console.WriteLine("Pojazd dodany pomyślnie!");
                            else
                                Console.WriteLine(" Miejsce zajęte! Wybierz inne współrzędne.");
                        }
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine(" Błąd! Wprowadź poprawne liczby!");
                    }
              
                    break;

                case "2":
                    Console.Write("Nr rejestracyjny pojazdu do usunięcia: ");
                    string nrDel = Console.ReadLine();
                    if (parking.UsunPojazd(nrDel))
                        Console.WriteLine(" Pojazd usunięty!");
                    else
                        Console.WriteLine(" Nie znaleziono pojazdu o takim numerze!");
                    break;

                case "3":
                    parking.WyswietlStatusParking();
                    break;

                case "4":
                    parking.ZapiszDoPliku("parking.json");
                    break;

                case "5":
                    parking.WczytajZPliku("parking.json");
                    break;

                case "6":
                    Console.WriteLine("=== ZAPARKOWANE POJAZDY ===");
                    if (parking.Pojazdy.Count == 0)
                    {
                        Console.WriteLine("Parking jest pusty!");
                    }
                    else
                    {
                        foreach (var p in parking.Pojazdy)
                        {
                            p.WyswietlInfo();
                        }
                    }
                    break;

                case "7":
                    Console.Write("Podaj nr rejestracyjny do wyszukania: ");
                    string nrSzukaj = Console.ReadLine();
                    parking.WyszukajPojazdPoNrRej(nrSzukaj);
                    break;

                case "8":
                    parking.WyswietlLogi();
                    break;

                case "9":
                    Console.Write("Podaj aktualny nr rejestracyjny: ");
                    string staryNr = Console.ReadLine();
                    Console.Write("Podaj nowy nr rejestracyjny: ");
                    string nowyNr = Console.ReadLine();
                    parking.EdytujNrRej(staryNr, nowyNr);
                    break;

                case "10":
                    dziala = false;
                    Console.WriteLine("Do widzenia!");
                    break;

                default:
                    Console.WriteLine(" Nieprawidłowa opcja! Wybierz 1-10.");
                    break;
            }
        }
    }
}