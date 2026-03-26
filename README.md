# apbd-cw3 - wypozyczalnia sprzetu

### opis funkcjonalnosci
Aplikacja sluzy do zarzadzania wypozyczalnia sprzetu (jakiego chcemy):
- zarzadzanie magazynem
- system do sledzenia wypozyczen
- zarzadzanie klientami (widzimy czy ktos jest studentem albo mozemy dostosowac max liczbe urzadzen dla wybranego typu)
- kalkulator kar
- zarzadzanie wypozyczeniami i zwrotami
- podsumowanie (raporty stanu magazynu)

przykladowe wykorzystanie: wypozyczalnia sprzetu filmowego lub sportowego.

### 1. dlaczego podzielilem aplikacje w ten sposob?
warstwy modeli, logiki biznesowej i obslugi interfejsu (w moim przypadku terminala) to klasyczna architektura warstwowa (model, serwis, wizualna) dostosowana pod aplikacje konsolowa.


Pomaga to latwo poruszac sie miedzy warstwami danych, logiki i widoku dla klienta.

### 2. funkcjonalnosc i decyzje
aplikacja w terminalu pozwala wypozyczac rozne sprzety i dostosowac/dodac kolejne przedmioty poprzez dziedziczenie np. nowe typy sprzetu czy uzytkownikow, co umozliwia skalowalnosc i customizacje dla potencjalnego klienta.
postawilem na architekture obiektowa pozwalajaca na zmiany bez modyfikacji glownego kodu.

chcialem zeby w kodzie byly wylapywane jak najczesciej bledy i edge casey - chroni to nasza aplikacje przed glupimi pomyslami klientow, ktorzy testujac limity, co mogloby spowodowac problemy z jej dzialaniem, co prowadziloby do zniszczenia doswiadczenia przez uzytkowania.

### 3. proby zadbania o kohezje, coupling i odpowiedzialnosc klas
a) kohezja - w skrocie dazymy do wysokiej kohezji, ktora przeklada sie na zasade single responsibility. Oznacza to, ze nasze metody/klasy posiadaja jedno przewidziane zadanie, ktore ewentualnie mozna modyfikowac, ale nie dodajac skrajnie nowych funkcjonalnosci.<br>
przyklad u mnie w kodzie to np. interfejs ipenaltycalculator, ktory jedyne co robi to matematyczne obliczanie kary na podstawie podanych dat.<br>jesli z kolei mowimy o klasach modeli, to nie przechowujemy tam logiki biznesowej, a jedynie stany i podstawowe zachowania dotyczace obiektu. 

b) coupling - staralem sie zachowac jego luzna wersje poprzez oddzielenie warstwy danych, logiki i wizualnej, sluzy to reuzywalnosci kodu, a takze pozwala szybciej wprowadzac zmiany, nie powodujac hardcodowania zaleznosci w kodzie. <br>przykladowo moja klasa rentallogic nie korzysta z wypisywania niczego w konsoli, bo odbywa sie to w mainie (program.cs).<br> kolejnym przykladem jest tez uzywanie di (dependency injection) i wstrzykiwanie interfejsu kalkulatora przez konstruktor, zamiast hardkodowac go poprezz new wewnatrz klasy

### 4. krotko o gitflow
poczatkowe initial commity robilem na mainie, po czym przeszedlem na deva, na ktorym na biezaco commitowalem zmiany. <br>po kazdej ukonczonej warstwie wrzucalem zmiany na maina po wczesniejszym przetestowaniu i upewnieniu sie, ze wszystko dziala. <br>dodatkowo stworzylem brancha na readme, ktory jest odpowiedzialny tylko za aktualizowanie dokumentacji, a takze rozwijanie opisu projektu oraz poszczegolnych jego funkcjonalnosci i jego od razu wrzucalem na main bo nie ma w tym przypadku sensu warstwowego mergowania.