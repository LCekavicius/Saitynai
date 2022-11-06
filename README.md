# Sprendžiamo uždavinio aprašymas
## Sistemos paskirtis
Projekto tikslas - Sukurti informacinę sistemą kuri padėtų įmonėms lengviau valdyti ir sekti produkcinę darbo veiklą.

Veikimo principas - Informacinę sistemą sudarys naudotojo sąsaja, kuri bus pasiekiama per internetinę aplikaciją, ir aplikacijų programavimo sąsaja (API)

Sistemos administratorius galės pridėti įmonę bei ją atstovaujančius vadovus. Vadovas gali registruoti įmonės darbuotojus jiem sukuriant paskyras. Tai pat, kūrti produkcinius užsakymus, jiems priskirti reikalingus darbus ir šiuos prisegti darbuotojams. Darbuotojai galės matyti visus jiems priskirtus darbus bei juos pradėti, stabdyti, užbaigti.

## Funkciniai reikalavimai
Neprisijungęs naudotojas galės:
1. Peržiūrėti prisijungimo puslapį
2. Prisijungti

Sistemos administratorius galės:
1. Peržiūrėti įmonių sąrašą
2. Sukūrti naują įmonę ir ją pridėti prie sąrašo
3. Peržiūrėti įmonių duomenis

Įmonės vadovas galės:
1. Sukūrti produkcinį užsakymą
2. Peržiūrėti produkcinių užsakymų sąrašą
3. Kūrti darbo tipus
4. Priskirti užsakymui reikalingus darbus
5. Priskirti darbus darbuotojams
6. Sukūrti darbuotojų paskyras

Darbuotojas galės:
1. Peržiūrėti jam priskirtus darbus (nepradėtus, pradėtus, užbaigtus)
2. Pradėti nepradėtus darbus
3. Stabdyti pradėtus darbus
4. Užbaigti pradėtus darbus

# Sistemos architektūra

Sistemos sudedamosios dalys:
1. Kliento pusė (Front-end) - bus realizuojama naudojant Blazor karkasą
2. Serverio pusė (Back-end) - bus realizuojama naudojant .NET Core 6. Duomenų bazė: MySQL.

Paveikslely pavaizduota kuriamos sistemos diegimo diagrama. Sistema bus patalptinta serveryje, kiekviena jos dalis bus tame paciame serveryje. Internetine aplikacija pasiekiama per HTTP protokolą. 

![PSed_diagram](https://user-images.githubusercontent.com/79587416/192154936-b31d9cf5-cf81-4e87-b116-4efd7408dc07.png)

