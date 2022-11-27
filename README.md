# Jump, Sonic, jump!

"Jump, Sonic, jump!" es el nombre de mi prototipo para la segunda Práctica de Evaluación Continua (PEC2) de la asignatura Programación de Videojuegos 2D del Máster Universitario en Diseño y Programación de Videojuegos de la UOC.

El objetivo de la práctica era desarollar un juego en 2D que replicara el mundo 1-1 de "Super Marios Bros." (NES) utilizando los conocimientos adquiridos en el estudio del segundo módulo de la asignatura y realizando investigación por cuenta propia.

## Vídeo explicativo



## Versión jugable

El prototipo puede jugarse aquí:

[Juega a "Jump, Sonic, Jump!" de Ragart en itch.io](https://itch.io/embed-upload/6902566?color=333333)

## Repositorio en GitLab

[UOC - M7.456 - PEC2 en GitLab](https://gitlab.com/ragart-uoc/m7.456/uoc-m7.456-pec2)

## Cómo jugar

El objetivo del juego es llegar hasta el final del nivel, recogiendo monedas y evitando que los oponentes .

Los controles son los siguientes:

- Flecha izquierda: moverse hacia la izquierda
- Flecha derecha: moverse hacia la derecha
- Flecha superior: saltar
- Flecha inferior: agacharse (elemento estético)

## Desarrollo

A efectos de cumplir lo solicitado en las instrucciones, el prototipo incluye lo siguiente:

- Dos escenas: una escena que muestra la información del jugador (vidas, puntos, etc.) y otra en la que se desarrolla el juego.
- El escenario de juego se ha estructurado utilizando tilemaps, mediante dos palettes (una para el overworld y otra para el underworld).
- Se han añadido animaciones y sonidos para todas las acciones del personaje jugador.
- La cámara se desplaza con el personaje jugador y lo impide volver a atrás.
- Los personajes oponentes también tienen animaciones y sonidos, se desplazan horizontalmente y mueren si el personaje jugador salta sobre ellos, utilizando una mezcla de colliders y de raycast.
- Se han implementado varios tipos de bloque: los estándar, que el personaje jugador puede romper cuando es grande; los sorpresa, que contienen monedas o powerups; los que contienen múltiples monedas; y, finalmente, los invisibles.
- Se han implementado dos powerups: las setas que hacen que el personaje jugador crezca y cuente con un punto de daño adicional y las setas que proporcionan una vida extra.
- Se ha añadido un HUD con información sobre los puntos, las monedas y el tiempo restante. Si el tiempo llega a cero, el jugador pierde.
- La escena de juego cuenta con música de fondo y sonidos propios.
- Además de las animaciones proporcionadas por el componente Animator, también se han creado algunas animaciones en tiempo de ejecución mediante las posiciones y el Lerp.

## Problemas conocidos

- A veces el personaje jugador salta demasiado alto debido a una detección incorrecta del nivel del suelo.

## Créditos

### Super Mario Bros™
- Todos los elementos pertenecientes al juego Super Mario Bros.™ son propiedad de Nintendo Company, Ltd.

### Sonic the Hedgehog™
- Todos los elementos pertenecientes al juego Sonic the Hedgehog™ son propiedad de Sega Corporation​.

### Fuentes
- "Super Mario Bros. NES Font" - TheWolfBunny64 - https://www.deviantart.com/thewolfbunny64/art/Super-Mario-Bros-NES-Font-812840651
- "Deathblood" - Creativework69 Studio - https://www.dafont.com/es/deathblood-2.font

### Imágenes y animaciones
- "Super Mario Bros. (NES) - Tileset" - Superjustinbros - https://www.spriters-resource.com/nes/supermariobros/sheet/52571/
- "Super Mario Bros. (NES) - Enemies & Bosses" - Superjustinbros - https://www.spriters-resource.com/nes/supermariobros/sheet/52570/
- "Super Mario Bros. (NES) - Items, Objects and NPCs" - Superjustinbros - https://www.spriters-resource.com/nes/supermariobros/sheet/52569/
- "Super Mario Bros. (NES) - Title Screen, HUD, and Miscellaneous" - Superjustinbros - https://www.spriters-resource.com/nes/supermariobros/sheet/56929/
- "Super Mario Bros. 3 (NES) - Enemies" - Fleepa & Superjustinbros - https://www.spriters-resource.com/nes/supermariobros3/sheet/25685/
- "Sonic the Hedgehog (Genesis) - Playable characters" - Paraemon & Triangly - https://www.spriters-resource.com/genesis_32x_scd/sonicth1/sheet/21628/
- "Sonic the Hedgehog (Genesis) - Common objects" - Paraemon - https://www.spriters-resource.com/genesis_32x_scd/sonicth1/sheet/27193/
- "Sonig the Hedgehog (Genesis) - Credits" - Paraemon - https://www.spriters-resource.com/genesis_32x_scd/sonicth1/sheet/151744/

## Referencias

### Unity - General
- "Importing a .unityPackage" - Unity Answers - https://answers.unity.com/questions/10813/importing-a-unitypackage.html
- "Box2D" - https://box2d.org/

### Tilemaps
- "TILEMAPS in Unity" - Brackeys - https://www.youtube.com/watch?v=ryISV_nH8qw
- "Tilemap" - Unity - https://docs.unity3d.com/Manual/class-Tilemap.html
- "Tilemap Collider 2D" - Unity - https://docs.unity3d.com/Manual/class-TilemapCollider2D.html
- "Adding Colliders to Tilemaps in Unity" - Jon Jenkins - https://jon-jenkins.medium.com/adding-colliders-to-tilemaps-in-unity-96e996908145
- "Easy 2D Tilemap Collisions | Unity 2018 Tutorial" - Chris’ Tutorials - https://www.youtube.com/watch?v=VDdKv0DgY5I
- "Unity - Fixing Gaps Between Tiles" - gamesplusjames - https://www.youtube.com/watch?v=QW53YIjhQsA
- "Random bumps using Tilemap Collider 2D?" - Reddit - https://www.reddit.com/r/Unity2D/comments/funphc/random_bumps_using_tilemap_collider_2d/

### Raycast
- "Unity Raycast 2D What Is It And How To Use It" - Romeo Violini - https://gamedevelopertips.com/unity-raycast-2d-what-is-it-and-how-to-use-it/
- "Unity 2D: Checking if a Character or Object is on the Ground using Raycasts" - Kyle Banks - https://kylewbanks.com/blog/unity-2d-checking-if-a-character-or-object-is-on-the-ground-using-raycasts

### Movimiento y control  del personaje
- "Input.GetAxisRaw" - Unity - https://docs.unity3d.com/ScriptReference/Input.GetAxisRaw.html
- "How to jump in Unity (with or without physics)" - John Leonard French - https://gamedevbeginner.com/how-to-jump-in-unity-with-or-without-physics/
- "Variable jump height in Unity jumping to random heights" - StackOverflow - https://stackoverflow.com/questions/73454484/variable-jump-height-in-unity-jumping-to-random-heights
- "How to check if the transform position of the x is increasing or decreasing?" - Unity Answers - https://answers.unity.com/questions/1508627/how-to-check-of-the-transform-position-of-the-x-is.html
- "How to get an object to follow you along a certain axis?" - Unity Answers - https://answers.unity.com/questions/886164/how-to-get-an-object-to-follow-you-along-a-certain.html
- "How to give the camera’s edge collision in Unity" - StackOverflow - https://stackoverflow.com/questions/58941259/how-to-give-the-cameras-edge-collision-in-unity
- "The right way to Lerp in Unity (with examples)" - John Leonard French - https://gamedevbeginner.com/the-right-way-to-lerp-in-unity-with-examples/
- "Help using Lerp inside of a coroutine" - Unity Answers - https://answers.unity.com/questions/1502190/help-using-lerp-inside-of-a-coroutine.html

### Animaciones
- "2D Animation in Unity (Tutorial)" - Brackeys - https://www.youtube.com/watch?v=hkaysu1Z-N8
- "How do you make an UI animation longer than one second?" - Unity Answers - https://answers.unity.com/questions/1794223/how-do-you-make-an-ui-animation-longer-than-one-se.html
- "How can I play an animation backwards?" - Unity Forums - https://forum.unity.com/threads/how-can-i-play-an-animation-backwards.498287/
- "How can I mirror 2d animation" - Unity Answers - https://answers.unity.com/questions/1023689/how-can-i-mirror-2d-animation.html

### Partículas
- "Unity Tutorial: Breakable Blocks with Unity Events (Mario style)" - PitiIT - https://www.youtube.com/watch?v=0WReDQ8ZSIA

### Otras
- "Annex D Documentation comments" - Microsoft - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
- "The right way to pause a game in Unity" - John Leonard French - https://gamedevbeginner.com/the-right-way-to-pause-the-game-in-unity/
- "Regarding 4:3 and 8:7. . . ." - NesDev - https://forums.nesdev.org/viewtopic.php?t=23885
- "Super Mario Brothers - World 1-1 Labeled Map" - Rick N. Burns - https://nesmaps.com/maps/SuperMarioBrothers/SuperMarioBrosWorld1-1Map.html