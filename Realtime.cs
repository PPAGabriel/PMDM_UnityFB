using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Unity.VisualScripting;

public class Realtime : MonoBehaviour
{
    // conexion con Firebase
    private FirebaseApp _app;
    // Singleton de la Base de Datos
    private FirebaseDatabase _db;
    // referencia a la 'coleccion' Clientes
    private DatabaseReference _refClientes;
    // referencia a un cliente en concreto
    private DatabaseReference _refAA002;
    // GameObject a modificar
    public PlayerController player1;
    // contador para update
    private float _i;
	// referencia a moneda
	private DatabaseReference _refMoneda;
    //referencia a los pickups
    public GameObject pickupPrefab;
    
    
    // Start is called before the first frame update
    void Start()
    {
		//obtenemos el player1
		player1=GameObject.Find("Player").GetComponent<PlayerController>();
        
        // realizamos la conexion a Firebase
        _app = Conexion();
        
        // obtenemos el Singleton de la base de datos
        _db = FirebaseDatabase.DefaultInstance;
        
        // Obtenemos la referencia a TODA la base de datos
        // DatabaseReference reference = db.RootReference;
        
        // Definimos la referencia a Clientes
        _refClientes = _db.GetReference("Jugadores");

		//Definimos la referencia de la moneda
        _refMoneda = _db.GetReference("prefabs");

		//Definimos la referencia a jugador1
        
        // Recogemos todos los valores de Clientes
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

        // Añadimos un nodo
        AltaDevice();
    }
    
    // realizamos la conexion a Firebase
    // devolvemos una instancia de esta aplicacion
    FirebaseApp Conexion()
    {
        FirebaseApp firebaseApp = null;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                firebaseApp = FirebaseApp.DefaultInstance;

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                firebaseApp = null;
            }
        });
            
        return firebaseApp;
    }
    
    // evento
    // escalo objeto en la escena
    void HandleValueChanged(object sender, ValueChangedEventArgs args) {
        if (args.DatabaseError != null) {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Mostramos lo resultados
        MuestroJugador(args.Snapshot);
    }

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
    
    // muestro un jugador
    void MuestroJugador(DataSnapshot jugador)
    {
        foreach (var resultado in jugador.Children) // jugador
        {
            Debug.LogFormat("{0}:{1}", resultado.Key, resultado.Value);
        }
    }


    // doy de alta un nodo con un identificador unico
    void AltaDevice()
    {
        _refClientes.Child(SystemInfo.deviceUniqueIdentifier).Child("nombre").SetValueAsync("Mi dispositivo");
    }
    
    // Update is called once per frame
    void Update()
    {
		//Actualizo la base de datos cada frame
		_refClientes.Child(SystemInfo.deviceUniqueIdentifier).Child("puntos").SetValueAsync(player1.count);
    }
}