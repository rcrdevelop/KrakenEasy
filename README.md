# KrakenEasy-BETA

KrakenEasy es una plataforma de estadísticas de poker utilizado para aumentar las probablidades de ganar en dicho juego o casino. Actualmente hay dos formas de usar esta herramienta, visualizar estadísticas durante la partida de poker o reproducir una mano anterior y analizar las jugadas tuyas con la de los oponentes.


#### Version independiente
	Este modo fue realizado como experimental para la aprobacion de la recoleccion de datos y funciones.
* Los servicios están separados por funciones independientes de work services lo cual posiblemente cree conflicto en alguno de sus funcionamientos.
* KrakenHUD es un proyecto de pruebas para las interfaces de estadísticas que funciona a traves de un contenedor.
* La base de datos es el enfoque principal por lo que maneja todos los procesos del sistema, realiza registro de ventanas, ficheros, partidas y ubicacion de las carpetas de los casinos.

#### Version actual
	Se delegan todos los procesos realizados de la base de datos a variables BSON(variable de mongodb) con valores estaticos que consuman menos recursos y las probabilidades de fallos en ejecucion.
* Los servicios estan secuenciados en el mismo Work Services interrumpiendo cualquier proceso hijo que continue luego de este a diferencia de la otra version.

## Modulos

### Servicios

Aquí se encuentran todas las funciones independientes que se ejecutan en segundo plano como un work services. Actualmente este servicio es llamado como cualquier otra clase sin embargo se tendrá que instalar como un dll para su correcta ejecución.

### Hands

Este modulo es utilizado para aplicar los análisis y almacenamiento de los ficheros que dan la información de cada partida de poker.

### Casinos

Contiene los parámetros de cada casino al momento de interpretar sus ficheros, cada casino es independiente y puede modificar la estructura de la información como desee, por lo que este funciona como estándar para manejar la información.

### KrakenBD

En esta sección están los métodos de registro ya sea en base de datos o estática para la comunicación del sistema, HUDS(ventanas con estadísticas), registros de partidas y ficheros.

### HUDS

Los HUDS son ventanas que contienen estadísticas del jugador que esta represente, actualmente existen 3 tipos de HUDS y estas deberán aparecer cuando ha pasado una mano de juego o si estas leyendo una mano en el Replayer.

### RePlayer

Como su nombre lo indica en un reproductor de manos que permite al espectador mejorar su método de juego e interpretar jugadas anteriores.


### Flujo de trabajo
   
   1. Al iniciar Kraken se inicializa la base de datos al igual que los variables BSON.
   2. El Work Services es iniciado esperando alguna mesa que se encuentra almacenada como fichero en la carpeta KrakenHands que es creada cuando es iniciado dicho servicio, se copian todos los archivos de los casinos que contienen las manos de las mesas jugadas.
   3. Cuando se identifica la condicion para que los HUDS aparezcan se consulta la ultima mano jugada de la base de datos y son ubicados los jugadores que se deben analizar para poder mostrar las estadisticas.
   4. Los HUDS desaparecen en cuanto la mesa lo hace, si el casino o el programa se detiene sucede lo mismo.

   Nota: para verificar si se registra informacion en la base de datos utilice MongoCompass, la conexion es la default, con darle al boton "connect" sera mas que suficiente para entrar.

## Setup Desarrollo

##### Equipo 1 

- 4 gb de ram frecuencia 1900 MHZ.
- Procesador AMD A10-7800.
- Tarjeta grafica integrada Radeon R7.
- Windows 10 Pro x64 Version 20H2. 
