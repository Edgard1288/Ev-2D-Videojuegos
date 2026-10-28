# Princess Peach:  SIN PIEDAD
#### por Edgard García 

## Objetivo
Desarrollar un videojuego 2d Plataformas donde el personaje que en este caso es la princesa Peach le dispara a los enemigos con una escopeta, puede dar doble saltos y tiene vida

## Plataforma
PLATAFORMA: Windows
Motor: Unity

## Instrucciones de Ejecución

Descargar el archivo comprimido .zip del proyecto compilado

Extraer el contenido en una carpeta local

Ejecutar el archivo PrincessPeachSinPiedad.exe

## Controles

| Acción | Tecla / Botón |
| :--- | :--- |
| **Moverse (Izquierda / Derecha)** | `A` / `D` o `Flechas Direccionales` |
| **Saltar** | `Barra Espaciadora` |
| **Apuntar** | `Movimiento del Cursor` |
| **Disparar Escopeta** | `Clic Izquierdo` o `Ctrl Izquierdo` |

## Riesgos y Mitigaciones

| Riesgo Detectado | Mitigación Implementada |
| :--- | :--- |
| **Descuadre de UI:** La interfaz se desposiciona en diferentes pantallas. | Configuración de `Canvas Scaler` en *Scale With Screen Size* (1920x1080) con anclas centralizadas. |
| **Cortes en el Parallax:** El fondo deja huecos o espacios vacíos al avanzar. | Duplicado de imágenes laterales y script ajustado al ancho del sprite para repetición en bucle. |
| **Baja de Rendimiento (Lag):** Acumulación de balas en memoria tras disparar. | Destrucción automática de perdigones al impactar o tras un tiempo límite. |

## Configuración Clave

Plataforma: Permite jugar cómodamente con teclado y mouse para apuntar el arma

Audio: Se utilizó AudioManager con el patrón Singleton y DontDestroyOnLoad para mantener la música de fondo fluida y continua al cambiar entre la escena del Menú y el Nivel

Físicas: Rigidbody2D y BoxCollider2D para el movimiento del personaje, detección de suelo, rebote de plataformas y sistema de físicas en perdigones/enemigos

Publico: Está pensado para jugadores de PC que buscan un juego indie de acción rápido y chistoso

## Dependencias 
Windows 10/11

Unity 6 (6000.2.3f1)

Packages: Universal Render Pipeline (URP), 2D Tilemap Editor, TextMeshPro, Input Manager.

## Roles

Todo desarrollado por Edgard García
