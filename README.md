# KrakenEasy-BETA-1.8

KrakenEasy es una plataforma de estadísticas de poker utilizado para aumentar las probablidades de ganar en dicho juego o casino. Actualmente hay dos formas de usar esta herramienta, visualizar estadísticas durante la partida de poker o reproducir una mano anterior y analizar las jugadas tuyas con la de los oponentes.


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
