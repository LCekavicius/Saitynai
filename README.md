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

# API specifikacija

JWT kodavimo algoritmas: HS256

JWT data:

```{
  "jti": "00e933cc-ed35-4366-9dc5-3243e895762f",
  "sub": "e66bac6b-095d-4829-8eb4-3077536abf01",
  "companyId": "0",
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": [
    "Representative",
    "Admin",
    "Worker"
  ],
  "exp": 1671646492,
  "iss": "TestIssuer",
  "aud": "TrustedClient"
}
```
| Atsakymo formatas | JSON |
| :---------------- | :--- |
| Norma ribota?     | Ne   |

## Auth API metodai
### Priregistruoti darbuotoja
| API metodas                | Register_Worker (POST)                                                                                                       |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| Paskirtis                  | Uzregistruoti imones darbuotoja                                                                                              |
| Endpoint'o kelias          | /api/registerworker                                                                                                          |
| Uzklausos struktura        | { "userName": string "email": string "password": string "companyId": int }                    |
| Uzklausos header           | Authorization: Bearer {token}                                                                                                |
| Reikalaujami Role          | Representative                                                                                                               |
| Atsakymo kodas             | 201 - Created                                                                                                                |
| Atsakymo struktura         | { "id": string, "userName": string, "email": string, "companyId": int } | |
| Kiti galimi atsakymo kodai | 400 - Bad Request ( Kai toks email arba username jau egzistuoja ), 401 - Unauthorized (Kai nera paduodamas jwt, arba jwt esanti role netinkama) |

### Priregistruoti Imones Atstova
| API metodas                | Register_Representative (POST)                                                                                               |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| Paskirtis                  | Uzregistruoti imones atstova                                                                                              |
| Endpoint'o kelias          | /api/registerrepresentative                                                                                                          |
| Uzklausos struktura        | { "userName": string "email": string "password": string "companyId": int }                     |
| Uzklausos header           | Authorization: Bearer {token}                                                                                                |
| Reikalaujami Role          | Admin                                                                                                               |
| Atsakymo kodas             | 201 - Created                                                                                                                |
| Atsakymo struktura         | { "id": string, "userName": string, "email": string, "companyId": int } |
| Kiti galimi atsakymo kodai | 400 - Bad Request ( Kai toks email arba username jau egzistuoja ), 401 - Unauthorized (Kai nera paduodamas jwt, arba jwt esanti role netinkama) |

### Prisijungti
| API metodas                | Login (POST)                                                                                                       |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| Paskirtis                  | Sistemos naudotojui prisijungti ir gauti authToken                                                                           |
| Endpoint'o kelias          | /api/login                                                                                                          |
| Uzklausos struktura        | { "userName": string, "password": string }                  |
| Uzklausos header           | -                                                                                               |
| Reikalaujami Role          | -                                                                                                               |
| Atsakymo kodas             | 200 - Ok                                                                                                                |
| Atsakymo struktura         | { "accessToken": {token} } |
| Kiti galimi atsakymo kodai | 400 - Bad Request ( Kai paduotas username nerandamas arba slaptazodis neatitinka )  |

## Imonių API metodai
### Gauti visas uzregistruotas imones
| API metodas                | GetMany (GET)                                                                                                       |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| Paskirtis                  | Gauti visas uzregistruotas imones                                                                                            |
| Endpoint'o kelias          | /api/companies                                                                                                          |
| Uzklausos struktura        | -                     |
| Uzklausos header           | Authorization: Bearer {token}                                                                                                |
| Reikalaujami Role          | Admin                                                                                                               |
| Atsakymo kodas             | 200 - Ok                                                                                                                |
| Atsakymo struktura         | [ {"id": int, "name": string, "creationDate": date } ] |
| Kiti galimi atsakymo kodai | 401 - Unauthorized (Kai nera paduodamas jwt, arba jwt esanti role netinkama) |

### Gauti viena uzregistruota imone
| API metodas                | Get (GET)                                                                                                       |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| Paskirtis                  | Gauti viena uzregistruota imone                                                                                           |
| Endpoint'o kelias          | /api/companies/{companyId}                                                                                                |
| Uzklausos struktura        | -                     |
| Uzklausos header           | Authorization: Bearer {token}                                                                                                |
| Reikalaujami Role          | -                                                                                                               |
| Atsakymo kodas             | 200 - Ok                                                                                                                |
| Atsakymo struktura         | {"id": int, "name": string, "creationDate": date } |
| Kiti galimi atsakymo kodai | 401 - Unauthorized (Kai nera paduodamas jwt), 404 - Not Found (kai imone nerasta pagal id), 403 - Forbid (kai Uzklausoje paduotas imones id sutampa su uzsakovo imones, kuriai jis priklauso, id) |

### Gauti visus imones darbuotojus
| API metodas                | GetAllCompanyWorkers (GET)                                                                                                       |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| Paskirtis                  | Gauti visus imones darbuotojus                                                                                          |
| Endpoint'o kelias          | /api/companies/{companyId}/GetAllCompanyWorkers                                                                           |
| Uzklausos struktura        | -                     |
| Uzklausos header           | Authorization: Bearer {token}                                                                                                |
| Reikalaujami Role          | -                                                                                                               |
| Atsakymo kodas             | 200 - Ok                                                                                                                |
| Atsakymo struktura         | [ {"id": string, "userName": string, "email": string, "companyId": int } ] |
| Kiti galimi atsakymo kodai | 401 - Unauthorized (Kai nera paduodamas jwt), 404 - Not Found (kai imone nerasta pagal id), 403 - Forbid (kai Uzklausoje paduotas imones id sutampa su uzsakovo imones, kuriai jis priklauso, id) |

### Gauti visus imones darbus
| API metodas                | GetAllWorks (GET)                                                                                                       |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| Paskirtis                  | Gauti visus imones darbus                                                                                          |
| Endpoint'o kelias          | /api/companies/{companyId}/GetAllWorks                                                                           |
| Uzklausos struktura        | -                     |
| Uzklausos header           | Authorization: Bearer {token}                                                                                                |
| Reikalaujami Role          | -                                                                                                               |
| Atsakymo kodas             | 200 - Ok                                                                                                                |
| Atsakymo struktura         | [ {"id": int, "type": string, "description": string, "creationDate": Date, "modifedDate": date, "startDateTime": date, "endDateTime": date, "IsPaused": boolean, "productionOrderId": int, "userId": string} ] |
| Kiti galimi atsakymo kodai | 401 - Unauthorized (Kai nera paduodamas jwt), 404 - Not Found (kai imone nerasta pagal id), 403 - Forbid (kai Uzklausoje paduotas imones id sutampa su uzsakovo imones, kuriai jis priklauso, id) |

### Sukurti imone
| API metodas                | Create (POST)                                                                                                       |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| Paskirtis                  | Sukurti imone                                                                                        |
| Endpoint'o kelias          | /api/companies                                                                           |
| Uzklausos struktura        | { "Name": string }                    |
| Uzklausos header           | Authorization: Bearer {token}                                                                                                |
| Reikalaujami Role          | Admin                                                                                                               |
| Atsakymo kodas             | 201 - Created                                                                                                                |
| Atsakymo struktura         |  { "id": int, "name": string, "creationDate": date } |
| Kiti galimi atsakymo kodai | 401 - Unauthorized (Kai nera paduodamas jwt ar kai jwt esanti role nesutamp su reikalaujancia ) |

### Redaguoti imone
| API metodas                | Update (PUT)                                                                                                       |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| Paskirtis                  | Redaguoti imones duomenis                                                                                        |
| Endpoint'o kelias          | /api/companies/{companyId}                                                                           |
| Uzklausos struktura        | { "Name": string }                    |
| Uzklausos header           | Authorization: Bearer {token}                                                                                                |
| Reikalaujami Role          | Admin                                                                                                               |
| Atsakymo kodas             | 200 - Ok                                                                                                                |
| Atsakymo struktura         | { "id": int, "name": string, "creationDate": date } |
| Kiti galimi atsakymo kodai | 401 - Unauthorized (Kai nera paduodamas jwt ar kai jwt esanti role nesutamp su reikalaujancia ) |

### Istrinti imone
| API metodas                | Delete (DELETE)                                                                                                       |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| Paskirtis                  | Istrinti Imone                                                                                        |
| Endpoint'o kelias          | /api/companies/{companyId}                                                                           |
| Uzklausos struktura        | -                    |
| Uzklausos header           | Authorization: Bearer {token}                                                                                                |
| Reikalaujami Role          | Admin                                                                                                               |
| Atsakymo kodas             | 204 - No Content                                                                                                                |
| Atsakymo struktura         | - |
| Kiti galimi atsakymo kodai | 401 - Unauthorized (Kai nera paduodamas jwt ar kai jwt esanti role nesutamp su reikalaujancia ) |

## Production Orders API metodai
### Gauti visus imones sukurtus gamybos uzsakymus
| API metodas                | GetMany (GET)                                                                                                       |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| Paskirtis                  | Gauti visus vienos imones gamybos uzsakymus                                                                                           |
| Endpoint'o kelias          | /api/companies/{companyId}/productionorders                                                                                     |
| Uzklausos struktura        | -                     |
| Uzklausos header           | Authorization: Bearer {token}                                                                                                |
| Reikalaujami Role          | -                                                                                                               |
| Atsakymo kodas             | 200 - Ok                                                                                                                |
| Atsakymo struktura         | [ {"id": int, "productName": string, "creationDate": date, "modifiedDate" : date, "Startdatetime": date, "Enddatetime": date, "companyId": int } ] |
| Kiti galimi atsakymo kodai | 401 - Unauthorized (Kai nera paduodamas jwt), 403 - Forbidden (kai uzsakovo imones id nesutampa su uzklausoje pateiktu imones id) |

### Gauti viena imones sukurta gamybos uzsakyma
| API metodas                | Get (GET)                                                                                                       |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| Paskirtis                  | Gauti visus vienos imones gamybos uzsakymus                                                                                           |
| Endpoint'o kelias          | /api/companies/{companyId}/productionorders/{productionOrderId}                                                                  |
| Uzklausos struktura        | -                     |
| Uzklausos header           | Authorization: Bearer {token}                                                                                                |
| Reikalaujami Role          | -                                                                                                               |
| Atsakymo kodas             | 200 - Ok                                                                                                                |
| Atsakymo struktura         | {"id": int, "productName": string, "creationDate": date, "modifiedDate" : date, "Startdatetime": date, "Enddatetime": date, "companyId": int } |
| Kiti galimi atsakymo kodai | 401 - Unauthorized (Kai nera paduodamas jwt), 403 - Forbidden (kai uzsakovo imones id nesutampa su uzklausoje pateiktu imones id), 404 - (kai uzsakymas, su paduotu id, nerandamas) |

### Sukurti gamybos uzsakyma
| API metodas                | Create (POST)                                                                                                       |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| Paskirtis                  | Sukurti gamybos uzsakyma                                                                        |
| Endpoint'o kelias          | /api/companies/{companyId}/productionorders                                                                          |
| Uzklausos struktura        | { "productName": string, "description": string }                    |
| Uzklausos header           | Authorization: Bearer {token}                                                                                                |
| Reikalaujami Role          | Representative                                                                                                  |
| Atsakymo kodas             | 201 - Created                                                                                                                |
| Atsakymo struktura         | {"id": int, "productName": string, "creationDate": date, "modifiedDate" : date, "Startdatetime": date, "Enddatetime": date, "companyId": int } |
| Kiti galimi atsakymo kodai | 401 - Unauthorized (Kai nera paduodamas jwt ar kai jwt esanti role nesutamp su reikalaujancia), 403 - Forbidden (kai uzsakovo imones id nesutampa su uzklausoje pateiktu imones id), 404 - (Kai paduotas companyId neegzistuoja duomenu bazeje) |

### Redaguoti gamybos uzsakyma
| API metodas                | Update (PUT)                                                                                                       |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| Paskirtis                  | Redaguoti gamybos uzsakymo duomenis                                                                                        |
| Endpoint'o kelias          | /api/companies/{companyId}/productionOrders/{productionOrderId} |
| Uzklausos struktura        | { "productName": string, "startDateTime": date, "endDateTime": date }                    |
| Uzklausos header           | Authorization: Bearer {token}                                                                                                |
| Reikalaujami Role          | Representative                                                                                                               |
| Atsakymo kodas             | 200 - Ok                                                                                                                |
| Atsakymo struktura         | { "id": int, "name": string, "creationDate": date } |
| Kiti galimi atsakymo kodai | 401 - Unauthorized (Kai nera paduodamas jwt ar kai jwt esanti role nesutamp su reikalaujancia), 403 - Forbidden (kai uzsakovo imones id nesutampa su uzklausoje pateiktu imones id), 404 - (Kai paduotas companyId arba productionOrderId neegzistuoja duomenu bazeje) |

### Istrinti gamybos uzsakyma
| API metodas                | Delete (DELETE)                                                                                                       |
| :------------------------- | :--------------------------------------------------------------------------------------------------------------------------- |
| Paskirtis                  | Istrinti gamybos uzsakyma                                                                                        |
| Endpoint'o kelias          | /api/companies/{companyId}/productionOrders/{productionOrderId} |
| Uzklausos struktura        | -                    |
| Uzklausos header           | Authorization: Bearer {token}                                                                                                |
| Reikalaujami Role          | Representative                                                                                                               |
| Atsakymo kodas             | 204 - No Content                                                                                                                |
| Atsakymo struktura         | - |
| Kiti galimi atsakymo kodai | 401 - Unauthorized (Kai nera paduodamas jwt ar kai jwt esanti role nesutamp su reikalaujancia), 403 - Forbidden (kai uzsakovo imones id nesutampa su uzklausoje pateiktu imones id), 404 - (Kai paduotas companyId arba productionOrderId neegzistuoja duomenu bazeje) |
