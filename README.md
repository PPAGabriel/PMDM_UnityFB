# Unity con FireBase

## Configurar Firebase en Unity

**1. Crear un proyecto en Firebase:**
 
Debes ingresar a la consola de Firebase y crear un nuevo proyecto. Para ello, debes ingresar a la consola de Firebase y hacer clic en el botón "Agregar proyecto". Luego, debes seguir los pasos que se te indican para crear un nuevo proyecto.

![img.png](media%2Fimg.png)

**2. Agregar una aplicación a tu proyecto:**

Una vez que hayas creado tu proyecto, debes agregar una aplicación a tu proyecto. Para ello, debes hacer clic en el botón "Agregar aplicación" y seleccionar la plataforma en la que deseas agregar tu aplicación. En este caso, seleccionaremos la plataforma Android.

![img_1.png](media%2Fimg_1.png)

**3. Agregar el archivo google-services.json a tu proyecto de Unity:**

Después de agregar tu aplicación, Firebase te proporcionará un archivo `google-services.json` que debes agregar a tu proyecto de Unity. Para ello, debes descargar el archivo `google-services.json` y agregarlo a la carpeta `Assets` de tu proyecto de Unity.

![img_2.png](media%2Fimg_2.png)

**4. Agregar el SDK de Firebase a tu proyecto de Unity:**

Después de agregar el archivo `google-services.json` a tu proyecto de Unity, debes agregar el SDK de Firebase (descomprimido) a tu proyecto de Unity. Para ello, debes descargar el SDK de Firebase y agregarlo a la carpeta `Assets` de tu proyecto de Unity.

![img_3.png](media%2Fimg_3.png)

## Estructura JSON en FireBase:

En la sección de Realtime Database, podemos interactuar con el formato que trabaja la base de datos de FireBase.
![img_4.png](media%2Fimg_4.png)

El formato JSON es un formato de intercambio de datos que es fácil de leer y escribir para los humanos y fácil de analizar y generar para las máquinas. JSON es un formato de texto que es completamente independiente del lenguaje pero utiliza convenciones que son familiares para los programadores de la familia de lenguajes C, incluidos C, C++, C#, Java, JavaScript, Perl, Python y muchos otros. Estas propiedades hacen que JSON sea un lenguaje ideal para el intercambio de datos.

---

## En el inicio del juego, recoger las posiciones de varios elementos (los prefab) y posicionarlos en función de los datos de la base de datos

Inicialmente, para poder interactuar con la base de datos creamos un objeto vacío con un Script relacionado, que en este caso lo llamaremos 'Realtime.cs'. Este script se encargará de recoger los datos de la base de datos y posicionar los elementos en función de estos datos.

Ahora bien, para hacer referencia a una variable de nuestra base de datos:

```csharp
		//Definimos la referencia de la moneda
        _refMoneda = _db.GetReference("prefabs");

        
        // Recogemos todos los valores de Prefabs
        _refMoneda.GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted) {
                    // Handle the error...
                }
                else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    // mostramos los datos
                    RecorrePickUps(snapshot);
                    //Debug.Log(snapshot.value);
                }
            });
```

En este caso, la variable `_refMoneda` hace referencia a la base de datos de FireBase, en la cual se encuentra la información de los elementos que queremos posicionar en el juego. 

```csharp
    // recorro un snapshot de un nivel
    void RecorrePickUps(DataSnapshot snapshot)
    {
        foreach(var resultado in snapshot.Children) // Monedas
        {
            
            float x=0,y=0,z=0;
            Debug.LogFormat("Key = {0}", resultado.Key);  // "Key= prefabXX"
            foreach(var levels in resultado.Children)
            {
                if (levels.Key == "x")
                {
                    x= float.Parse(levels.Value.ToString());
                }

                if (levels.Key == "y")
                {
                    y= float.Parse(levels.Value.ToString());
                }
                
                if (levels.Key == "z")
                {
                    z= float.Parse(levels.Value.ToString());
                }
            }
            SpawnPickup(x, y, z);
        }
    }
    
    public void SpawnPickup(float x, float y, float z)
    {
        // Usaran las coordenadas que se le den
        Vector3 spawnPosition = new Vector3(x, y, z);
        // P
        Instantiate(pickupPrefab, spawnPosition, Quaternion.identity);
    }
```

En este caso, la función `RecorrePickUps` recorre los datos de la base de datos y los almacena en las variables `x`, `y` y `z`. Luego, la función `SpawnPickup` se encarga de posicionar los elementos en función de estos datos.

De esta manera, al iniciar el juego, los elementos se posicionarán en función de los datos de la base de datos.

Antes de iniciar:

![img_5.png](media%2Fimg_5.png)

Después de iniciar:

![img_6.png](media%2Fimg_6.png)

---

## Actualizar datos en la base de datos segun avance el juego, por ejemplo, los puntos recogidos, las vidas, etc. Esto tiene que ser particular para cada jugador. Pensar que el juego lo juegan muchos jugadores y comparten la base de datos

Haciendo referencia a nuestro PlayerController, tenemos acceso a la variable "count", de manera que la utilicemos para modificar nuestra base de datos.

>>  public PlayerController player1;

>[!IMPORTANT]
> La variable debe ser pública para tener acceso a ella en nuestro Realtime.cs

```csharp
       void Start()
    {
		//obtenemos el player1
		player1=GameObject.Find("Player").GetComponent<PlayerController>();
        ...
        ...
            // Update is called once per frame
    void Update()
    {
		//Actualizo la base de datos cada frame
		_refClientes.Child(SystemInfo.deviceUniqueIdentifier).Child("puntos").SetValueAsync(player1.count);
    }
```

En este caso, la variable `_refClientes` hace referencia a la base de datos de FireBase, en la cual se encuentra la información de los elementos que queremos modificar. Al realizarlo en nuestro 'Update', se actualiza la base de datos cada frame.

Como resultado, tenemos el siguiente:

Sin colisionar con un bloque:

![img_7.png](media%2Fimg_7.png)

Al colisionar con los bloques:

![img_8.png](media%2Fimg_8.png)

---

# Espero esta información sea de ayuda! :smile:


