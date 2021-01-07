# ADN Mutante - API REST
Evalúa una cadena de ADN compuesta por sus bases nitrogenadas(A,C,G,T). Si encuentra 2 o más veces las coincidencias de 4 caracteres iguales de forma vertical, horizontal o diagonal, confirma que el ADN analizado corresponde a un mutante.

## Technologies
Proyecto desarrollado con:
* C# .Net Core 3.1
* SQLite 


## Endpoints

### Stats
Devuelve cantidad de mutantes, humanos y el ratio entre ellos.

#### Request

`GET /stats/`

   curl -X GET "https://localhost:44380/stats" -H  "accept: */*"

#### Response Headers

 content-length: 54 
 content-type: application/json; charset=utf-8 
 date: Thu07 Jan 2021 19:13:43 GMT 
 server: Microsoft-IIS/10.0 
 x-powered-by: ASP.NET 
    
 ### Response Body   

  {"count_mutant_dna":1,"count_human_dna":4,"ratio":0.25}

## Mutant
Evalúa si es mutante o no pasandole un Array de Strings. Cada fila del Array debe tener la misma cantidad de caracteres y las únicas posibles letras válidas son "A", "C", "G" o "T" . En caso de repetir una cadena de ADN ya guardada en la base de datos, devolverá si es o no mutante pero no lo guardará en la base de datos nuevamente.

### Request

`POST /mutant/`

    curl -X POST "https://localhost:44380/mutant" -H  "accept: */*" -H  "Content-Type: application/json-patch+json" -d "{\"dna\":[\"ATTCGA\",\"CAGTGC\",\"TTTCTC\",\"AGAAGT\",\"CCACTA\",\"TCACTG\"],\"isMutant\":true}"

### Response Headers

 content-length: 133 
 content-type: application/problem+json; charset=utf-8 
 date: Thu07 Jan 2021 19:16:33 GMT 
 server: Microsoft-IIS/10.0 
 x-powered-by: ASP.NET 
    
 ### Response Body
 Devuelve estas posibles respuestas:
 
* 403-Forbidden en caso de que no sea mutante
* 200-Ok Si es mutante
* 400- Cuándo la cadena de ADN recibida no es válida

# Para ejecutar los endpoint

## Swagger
URL de la versión productiva:
* Swagger para probar los endpoint: https://adnmutante.azurewebsites.net/swagger/index.html

### /mutant 
Se le pasa una cadena de caracteres en este formato: "ATGCGA","CAGTGC","TTATGT","AGAAGG","CCCCTA","TCACTG"
Cada fila debe tener la misma cantidad de caracteres y únicamente los siguientes caracteres: A, C, G, T

### /stats 
Al ejectularlo, devolverá en un json los siguientes datos:
* cantidad de humanos
* cantidad de mutantes
* ratio

URL para probar los endpoints en localhost luego de levantar la aplicación con IISExpress:
* https://localhost:44380/swagger/index.html
