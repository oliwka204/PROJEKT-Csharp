using System.Text.Json; //do JSON
using System.IO; //do operacji na plikach
using System;

public class Miejsce //klasa reprezentująca pojedyncze miejsce parkingowe
{
    public int Wiersz { get; set; } //właściwość przechowująca numer wiersza miejsca parkingowego
    public int Kolumna { get; set; } //właściwość przechowująca numer kolumny miejsca parkingowego
    public bool CzyZajete { get; set; } //właściwość określająca, czy miejsce jest zajęte
    public string NrRej { get; set; } //właściwość przechowująca numer rejestracyjny auta zaparkowanego na miejscu
}
public class  LogWpisu
{
    public DateTime Czas { get; set; } //właściwość przechowująca czas zdarzenia
    public string Akcja { get; set; } //właściwość przechowująca opis akcji (np. "Przyjazd", "Odjazd")    
    public string NrRej { get; set; } //właściwość przechowująca numer rejestracyjny auta
}
public abstract class Pojazd
{
       public string NrRej { get; set; } //właściwość przechowująca numer rejestracyjny pojazdu
       public List<(int wiersz, int kolumna)> Miejsca { get; set; } //właściwość przechowująca listę miejsc zajmowanych przez pojazd
         
       public Pojazd(string nrRej)
       {
           NrRej = nrRej;
           Miejsca = new List<(int, int)>();
    }
    public abstract void WyswietlInfo(); //metoda abstrakcyjna do wyświetlania informacji o pojeździe

}
public class  Motocykl : Pojazd 
{
    public Motocykl(string NrRej, int wiersz, int kolumna) : base(NrRej)
    {
        Miejsca.Add((wiersz, kolumna)); //motocykl zajmuje jedno miejsce
    }   
    public override void WyswietlInfo()
    {
        Console.WriteLine($"Motocykl - Nr Rej: {NrRej}, Miejsce: " +
            $"({Miejsca[0].wiersz}, {Miejsca[0].kolumna})");
    }
}
public class Samochod : Pojazd
{
    public Samochod(string nrRej, int wiersz, int kolumna) : base(nrRej)
    {
        Miejsca.Add((wiersz, kolumna)); //pierwsze miejsce
        Miejsca.Add((wiersz, kolumna + 1)); //drugie miejsce obok pierwszego, ponieważ samochód zajmuje dwa miejsca
    }
    public override void WyswietlInfo()
    {
        Console.WriteLine($"Samochód - Nr Rej: {NrRej}, Miejsca: " +
            $"({Miejsca[0].wiersz}, {Miejsca[0].kolumna}) i ({Miejsca[1].wiersz}, {Miejsca[1].kolumna})");
    }
}
public class Autobus : Pojazd 
{ 
public Autobus(string nrRej, int wiersz, int kolumna) : base(nrRej)
    {
        for (int w=0;w<2;w++) //autobus zajmuje dwa wiersze
        {
            for (int k=0; k<2; k++) //i dwie kolumny
            {
                Miejsca.Add((wiersz + w, kolumna + k)); //autobus zajmuje 4 miejsca w układzie kwadratu 2x2
            }
        }
    }
    public override void WyswietlInfo()
    {
        Console.WriteLine($"Autobus - Nr Rej: {NrRej}, Miejsca: " +
            $"({Miejsca[0].wiersz}, {Miejsca[0].kolumna}), " +
            $"({Miejsca[1].wiersz}, {Miejsca[1].kolumna}), " +
            $"({Miejsca[2].wiersz}, {Miejsca[2].kolumna}), " +
            $"({Miejsca[3].wiersz}, {Miejsca[3].kolumna})");
    }   
}

public class Ciezarowka : Pojazd
{
    public Ciezarowka(string nrRej, int wiersz, int kolumna) : base(nrRej)
    {
        //ciężarówka zajmuje 3 miejsca w jednym wierszu (obok siebie)
        for (int k = 0; k < 3; k++)
        {
            Miejsca.Add((wiersz, kolumna + k));
        }
    }

    public override void WyswietlInfo()
    {
        Console.WriteLine($"Ciężarówka - Nr Rej: {NrRej}, Miejsca: " +
            $"({Miejsca[0].wiersz}, {Miejsca[0].kolumna}), " +
            $"({Miejsca[1].wiersz}, {Miejsca[1].kolumna}), " +
            $"({Miejsca[2].wiersz}, {Miejsca[2].kolumna})");
    }
}



public class Parking
{
    public Miejsce[,] Miejsca { get; set; } //macierz miejsc parkingowych(prostokątna tablica 2D)
    public List<Pojazd> Pojazdy { get; set; } //lista pojazdów aktualnie zaparkowanych
    public List<LogWpisu> Logi { get; set; } //lista /hustoria zdarzeń parkingowych(przyjazdy/odjazdy z datą i czasem)
    public int LiczbaWierszy { get; set; } //właściwość przechowująca liczbę wierszy miejsc parkingowych
    public int LiczbaKolumn { get; set; } //właściwość przechowująca liczbę kolumn miejsc parkingowych

    public Parking(int wiersze, int kolumny)
    {
        LiczbaWierszy = wiersze;
        LiczbaKolumn = kolumny;
        Miejsca = new Miejsce[wiersze, kolumny];
        Pojazdy = new List<Pojazd>();
        Logi = new List<LogWpisu>();

        //inicjalizacja wszystkich miejsc parkingowych jako wolne
        for (int w = 0; w < wiersze; w++)
        {
            for (int k = 0; k < kolumny; k++)
            {
                Miejsca[w, k] = new Miejsce { Wiersz = w, Kolumna = k, CzyZajete = false };
            }
        }
    }
    public bool DodajPojazd(Pojazd pojazd)
    {
        //sprawdzenie czy wszystkie miejsca są wolne
        foreach (var miejsce in pojazd.Miejsca)
        {
            if (Miejsca[miejsce.wiersz, miejsce.kolumna].CzyZajete)
            {
                return false; //jeśli choć jedno miejsce jest zajęte, zwróć false

            }
        }
        //oznmacz miejsca jako zajęte
        foreach (var miejsce in pojazd.Miejsca)
        {
            Miejsca[miejsce.wiersz, miejsce.kolumna].CzyZajete = true;
            Miejsca[miejsce.wiersz, miejsce.kolumna].NrRej = pojazd.NrRej;
        }
        //dodaj pojazd do listy zaparkowanych pojazdów
        Pojazdy.Add(pojazd);

        //dodaj wpis do logu
        Logi.Add(new LogWpisu { Czas = DateTime.Now, Akcja = "Przyjazd", NrRej = pojazd.NrRej });

        return true; //zwróć true, jeśli pojazd został pomyślnie dodany
    }
    public bool UsunPojazd(string nrRej)
    {
        Pojazd pojazd = Pojazdy.Find(p => p.NrRej == nrRej); //znajdź pojazd po numerze rejestracyjnym

        if (pojazd == null)
        {
            return false; //jeśli pojazd o takim numerze rej. nie został znaleziony, zwróć false
        }
        //zwolnij miejsca 
        foreach (var miejsce in pojazd.Miejsca)
        {
            Miejsca[miejsce.wiersz, miejsce.kolumna].CzyZajete = false;
            Miejsca[miejsce.wiersz, miejsce.kolumna].NrRej = null;
        }
        Pojazdy.Remove(pojazd); //usuń pojazd z listy zaparkowanych pojazdów

        //dodaj wpis do logu
        Logi.Add(new LogWpisu { Czas = DateTime.Now, Akcja = "Odjazd", NrRej = pojazd.NrRej });

        return true; //zwróć true, jeśli pojazd został pomyślnie usunięty
    }


    public bool EdytujNrRej(string staryNrRej, string nowyNrRej)
    {
        // Znajdź pojazd po starym numerze rejestracyjnym
        Pojazd pojazd = Pojazdy.Find(p => p.NrRej.ToUpper() == staryNrRej.ToUpper());

        if (pojazd == null)
        {
            Console.WriteLine($" Nie znaleziono pojazdu o numerze: {staryNrRej}");
            return false;
        }

        // Sprawdź czy nowy numer nie jest pusty
        if (string.IsNullOrWhiteSpace(nowyNrRej))
        {
            Console.WriteLine(" Nowy numer rejestracyjny nie może być pusty!");
            return false;
        }

        // Zaktualizuj numer rejestracyjny w obiekcie pojazdu
        pojazd.NrRej = nowyNrRej;

        // Zaktualizuj numer rejestracyjny we wszystkich miejscach zajmowanych przez pojazd
        foreach (var miejsce in pojazd.Miejsca)
        {
            Miejsca[miejsce.wiersz, miejsce.kolumna].NrRej = nowyNrRej;
        }

        // Dodaj wpis do logu
        Logi.Add(new LogWpisu
        {
            Czas = DateTime.Now,
            Akcja = "Edycja nr rej",
            NrRej = $"{staryNrRej} → {nowyNrRej}"
        });

        Console.WriteLine($" Zaktualizowano nr rejestracyjny z {staryNrRej} na {nowyNrRej}");
        return true;
    }
    public void WyswietlStatusParking()
    {
        Console.WriteLine("AKTUALNY STATUS PARKINGU: ");
        //nagłówek z numerami kolumn
        Console.Write("   ");//wciecie dla nr wiersza
        for (int k = 0; k < LiczbaKolumn; k++)
        {
            Console.Write($" {k} "); //numery kolumn
        }
        Console.WriteLine();

        for (int w = 0; w < LiczbaWierszy; w++)
        {
            Console.Write($" {w} "); //numer wiersza

            for (int k = 0; k < LiczbaKolumn; k++)
            {
                if (Miejsca[w, k].CzyZajete)
                {
                    Console.Write("[x]"); //oznaczenie miejsca zajętego
                }
                else
                {
                    Console.Write("[]"); //oznaczenie miejsca wolnego
                }
            }
            Console.WriteLine();

            //przejazd co drugi wiersz (po wierszach parzystych: 0, 2, 4...)
            if (w % 2 == 1 && w < LiczbaWierszy - 1)
            {
                Console.Write("   "); //wcięcie
                for (int k = 0; k < LiczbaKolumn; k++)
                {
                    Console.Write("---"); //linia przejazdu
                }
                Console.WriteLine();
            }
        }
    }
    public void ZapiszDoPliku(string nazwaPliku)
    {
        // Przerzuć tablicę 2D na listę
        List<Miejsce> listaMiejsc = new List<Miejsce>();
        for (int w = 0; w < LiczbaWierszy; w++)
        {
            for (int k = 0; k < LiczbaKolumn; k++)
            {
                listaMiejsc.Add(Miejsca[w, k]);
            }
        }

        // Stwórz listę pojazdów z informacją o typie
        var listaPojazdowDoZapisu = new List<object>();
        foreach (var pojazd in Pojazdy)
        {
            listaPojazdowDoZapisu.Add(new
            {
                Typ = pojazd.GetType().Name, // Nazwa klasy: "Motocykl", "Samochod", itp.
                NrRej = pojazd.NrRej,
                Miejsca = pojazd.Miejsca
            });
        }

        // Pakuje dane do zapisu
        var dane = new
        {
            LiczbaWierszy = this.LiczbaWierszy,
            LiczbaKolumn = this.LiczbaKolumn,
            Miejsca = listaMiejsc,
            Pojazdy = listaPojazdowDoZapisu, 
            Logi = this.Logi
        };

        // Zapis do JSON
        string json = JsonSerializer.Serialize(dane, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(nazwaPliku, json);
        Console.WriteLine($" Sukces! Zapisano do pliku: {nazwaPliku}");
    }

    public void WczytajZPliku(string nazwaPliku)
    {
        // Sprawdzenie czy plik istnieje
        if (!File.Exists(nazwaPliku))
        {
            Console.WriteLine($" Błąd! Plik {nazwaPliku} nie istnieje!");
            return;
        }

        // Wczytaj JSON z pliku
        string json = File.ReadAllText(nazwaPliku);

        // Zamiana JSON na obiekt tymczasowy
        var dane = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

        // Odczyt wymiarów parkingu
        int wiersze = dane["LiczbaWierszy"].GetInt32();
        int kolumny = dane["LiczbaKolumn"].GetInt32();
        LiczbaWierszy = wiersze;
        LiczbaKolumn = kolumny;

        // Odtworzenie tablicy miejsc
        Miejsca = new Miejsce[wiersze, kolumny];
        var listaMiejsc = JsonSerializer.Deserialize<List<Miejsce>>(dane["Miejsca"].GetRawText());

        foreach (var miejsce in listaMiejsc)
        {
            Miejsca[miejsce.Wiersz, miejsce.Kolumna] = miejsce;
        }

       
        Pojazdy = new List<Pojazd>(); // Wyczyść starą listę

        if (dane.ContainsKey("Pojazdy"))
        {
            var listaPojazdow = JsonSerializer.Deserialize<List<JsonElement>>(dane["Pojazdy"].GetRawText());

            foreach (var pojazdJson in listaPojazdow)
            {
                string typ = pojazdJson.GetProperty("Typ").GetString();
                string nrRej = pojazdJson.GetProperty("NrRej").GetString();
                var miejsca = JsonSerializer.Deserialize<List<(int wiersz, int kolumna)>>(
                    pojazdJson.GetProperty("Miejsca").GetRawText());

                // Odtwórz obiekt pojazdu na podstawie typu
                Pojazd pojazd = null;

                if (typ == "Motocykl")
                    pojazd = new Motocykl(nrRej, miejsca[0].wiersz, miejsca[0].kolumna);
                else if (typ == "Samochod")
                    pojazd = new Samochod(nrRej, miejsca[0].wiersz, miejsca[0].kolumna);
                else if (typ == "Autobus")
                    pojazd = new Autobus(nrRej, miejsca[0].wiersz, miejsca[0].kolumna);
                else if (typ == "Ciezarowka")
                    pojazd = new Ciezarowka(nrRej, miejsca[0].wiersz, miejsca[0].kolumna);

                if (pojazd != null)
                    Pojazdy.Add(pojazd);
            }
        }

        // Odczyt logów
        Logi = JsonSerializer.Deserialize<List<LogWpisu>>(dane["Logi"].GetRawText());
        Console.WriteLine($"Sukces! Wczytano z pliku: {nazwaPliku}");
    }


    public void WyszukajPojazdPoNrRej(string nrRej)
    {
        //wyszukanie pojazdu po numerze rejestracyjnym
        Pojazd pojazd = Pojazdy.Find(p => p.NrRej.ToUpper() == nrRej.ToUpper());

        if (pojazd == null)
        {
            Console.WriteLine($" Nie znaleziono pojazdu o numerze: {nrRej}");
        }
        else
        {
            Console.WriteLine(" Znaleziono pojazd:");
            pojazd.WyswietlInfo();
        }
    }

    public void WyswietlLogi()
    {
        //wyświetlenie całej historii (logi przyjazd/odjazd z datą i czasem)
        Console.WriteLine(" HISTORIA PARKINGU (LOGI) ");

        if (Logi.Count == 0)
        {
            Console.WriteLine("Brak wpisów w historii.");
            return;
        }

        foreach (var log in Logi)
        {
            Console.WriteLine($"[{log.Czas:yyyy-MM-dd HH:mm:ss}] {log.Akcja} - Nr Rej: {log.NrRej}");
        }
    }
}