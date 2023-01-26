# Ćwiczenie 1
1. Przygotuj ten projekt aplikacji konsolowej do połączenia z lokalną instancją MongoDB wg [opisu](https://record-it.gitbook.io/mongodb-c-driver/konfiguracja-polaczenia/projekt-aplikacji-konsolowej).
 - nazwa bazy `cityApp`
 - nazwa kolekcji miast `cities`,
 - nazwa kolekcji krajów `countries`
2. Zdefiniuj w `Program.cs` metodę  statyczną `CreateCountryDocument`, która zbuduje `BsonDocument` na podstawie
recordu `Country` (patrz plik `GeoNames.cs`). Nazwy właściwości dokumentu zapisz za pomocą snake case np. pole rekordu featureClass zapisz jako feature_class.
3. Zdefiniuj także drugą metodę `CreateCityDocument` do tworzenie dokumentu z opisem miasta.
3. Metodami `LoadCities` i `LoadCountries` pobierz listy miast i krajów i utwórz dla każdego rekordu document opisujący miasto i kraj w odpowiedniej kolekcji 
4. Listę dokumentów zapisz do MongoDB, sprawdź czy baza i kolekcja zostały utworzone oraz czy w kolekcji cities znajdują się miasta.
5. Cały kod ćwiczenia umieść w metodzie `Exercise1`

# Ćwiczenie 2
Wykonaj update każdego kraju w kolekcji 'countries' aby :
1. W polu `neighbours` zmiast tablicy kodów  krajów była tablica indeksów `_id` krajów o tym kodzie,
2. W polu `capital` zamiast nazwy stolicy wstaw cały dokument z kolekcji `cities`.
3. Kod ćwiczenia umieść w metodzie `Exercise2`.

# Ćwiczenie 3
Za pomocą operacji masowych wykonaj poniższe zmiany:
1. Usuń z kolekcji `cities` miasta, których populacja jest zerowa.
2. Wykonaj update każdego miasta w kolekcji 'cities', aby posiadało pole 'country_id' z identyfikatorem kraju, w którym miasto się znajduje.
3. Kod ćwiczenia umieść w metodzie `Exercise3`.

# Ćwiczenie 4
Zdefiniuj klasę encji `CityEntity` dla miasta i `CountryEntity` do kraju. W klasie zdefiniuj także metodę `toString`.
Napisz kod wyświetlający polskie miasta za pomocą encji.
Kod ćwiczenia umieść w metodzie `Exercise4`.

# Ćwiczenie 5
Dodaj w klasie `CityEntity` właściwość typu  `CountryEntity` o nazwie `Country`, która nie jest mapowana na pole dokumentu w bazie.
Napisz kod, który ustawia tę właściwość po odczycie danego miasta encją kraju, w którym to miasto się znajduje.
Kod ćwiczenia umieść w metodzie `Exercise5`.

# Cwiczenie 6
Napisz kod, który pobierze polskie miasta o liczbie ludności powyżej 400 tys. w postaci projekcji:
 - nazwą miasta
 - współrzędnymi geograficznymi
 - populacją
Kod ćwiczenia umieść w metodzie `Exercise6`.

# Ćwiczenie 7
1. Wykonaj grupowanie krajów wg kontynetów, aby otrzymać słownik z nazwą kontynetru jako klucz i listą krajów jako wartość. 
2. Wykonaj grupowanie miast wg kontynentów, aby otrzymać listę krotek z nazwą kontynentu i liczbą miast na tym kontynecie.
3. Wyświetl wynik grupowania.
4. Kod ćwiczenia umieść w metodzie `Exercise6`.

# Ćwiczenie 7
1. Zdefiniuj repozytorium generyczne, które zawiera zależność do klienta MongoDB i zawiera metody oraz parametry konstruktora z nazwą bazy i kolekcji.
 - zapis encji do kolekcji ze zwrotem `_id`
 - usunięcie encji na podstawie `_id`
 - update encji na podstawie obiektu klasy encji,
 - odczyt wszystkich encji ze stronicowaniem - numer strony, rozmiar strony, porządek sortowania
 - odczyt jednej encji na podstawie `_id`
Repozytorium umieść w osobnej klasie, która może być też w osobnym folderze, namespac'ie wg własnego uznania.
Kod korzystający z serwisu umieść w metodzie `Exercise7`.



