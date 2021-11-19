using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //-- Static Singleton
    private static GameController _instance;
    public static GameController Instance => _instance;

    //-- References to other Controller Scripts
    public ObjectPool ObjectPool { get; set; }
    public Inputs Inputs { get; set; }
    public GameUI GameUI { get; set; }

    //-- Object Lists
    public List<GameObject> ActiveGameObjects;  //-- List of currently active Game Objects at game start. We loop through this to reset the game.
    public List<GameObject> SpawnedObjects;     //-- List of objects that are spawned during playthrough. Used for garbage collection when resetting the game.
    

    void Awake()
    {
        Random.InitState(1); //-- Seeds the Random class with a unique seed. Same seed will always give us the same random values every time the game starts. Useful for f.ex generating a MineCraft map with a world seed.
        if (_instance == null) _instance = this;
        ActiveGameObjects = new List<GameObject>();

        ObjectPool = new ObjectPool();  //-- The Object Pool class is not attached to a GameObject, so we Instantiate it just like a regular class and keep a reference to it
        Inputs = GetComponent<Inputs>();
    }

    public void RestartGame()
    {
        RemoveActiveProjectiles();

        EnableAllObjects();

        #region Local Methods
        void EnableAllObjects()
        {
            foreach (var activeObject in ActiveGameObjects)
            {
                var restartInterface = activeObject.GetComponent<IRestart>();
                    restartInterface?.Respawn(); //-- If the object implements the IRestart interface, call the Respawn method.
            }
        }

        void RemoveActiveProjectiles()
        {
            foreach (var projectileObject in SpawnedObjects)
            {
                Destroy(projectileObject);
            }
            SpawnedObjects.Clear();
        }
        #endregion
    }
}
