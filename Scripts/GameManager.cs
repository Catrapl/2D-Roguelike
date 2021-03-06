using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public float LevelStartDelay = 2f;
	public float turnDelay = 0.1f;
	public static GameManager instance = null;
	public int playerFoodPoints=100;
	[HideInInspector] public bool playerTurn = true;

	private Text levelText;
	private GameObject levelImage;
	private BoardManager boardScript;
	private int level = 0;
	private List<Enemy> enemies;
	private bool enemiesMoving;
	private bool doingSetup = true;


	// Use this for initialization
	void Awake ()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
		enemies = new List<Enemy> ();
		boardScript = GetComponent<BoardManager>();


	}
	/*void OnLevelWasLoaded(int index)
	{
		//Add one to our level number.
		level++;
		//Call InitGame to initialize our level.
		InitGame();
	}*/


	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		level++;
		InitGame ();
	}

	void OnEnable()
	{
		//Tell our ‘OnLevelFinishedLoading’ function to start listening for a scene change event as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}


	void OnDisable()
	{
		//Tell our ‘OnLevelFinishedLoading’ function to stop listening for a scene change event as soon as this script is disabled. 
		//Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;}

	void InitGame()
	{
		doingSetup = true;
		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text>();
		levelText.text = "Day:" + level;
		levelImage.SetActive (true);
		Invoke ("HideLevelImage", LevelStartDelay);

		enemies.Clear ();
		boardScript.SetupScene (level);
	}

	private void HideLevelImage ()
	{
		levelImage.SetActive (false);
		doingSetup = false;
	}



	// Update is called once per frame
	void Update ()
	{
		if (playerTurn || enemiesMoving || doingSetup)
			return;

		StartCoroutine (MoveEnemies ());


	
	}

	public void AddEnemyToList (Enemy script)
	{
		enemies.Add (script);

	}

	public void GameOver()
	{
		levelText.text = "After " + level + " days, you starved";
		levelImage.SetActive (true);
		enabled = false;
	}


	IEnumerator MoveEnemies()
	{
		enemiesMoving = true;
		yield return new WaitForSeconds (turnDelay);
		if (enemies.Count == 0) 
		{
			yield return new WaitForSeconds (turnDelay);
		}

		for (int i = 0; i < enemies.Count; i++)
		{
			enemies [i].MoveEnemy ();
			yield return new WaitForSeconds (enemies[i].moveTime);

		}

		playerTurn = true;
		enemiesMoving = false;

	}

	
}
