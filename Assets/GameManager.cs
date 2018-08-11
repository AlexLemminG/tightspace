using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public ControlledParticle controlledParticle;
	public Particle particlePrefab;
	public Boom boomPrefab;
	public Boom timeToSpawnEffectPrefab;
	public float downTimeToSpawn = 3f;
	public ScoreView scoreView;
	public TMPro.TMP_Text bestScoreTextMesh;
	public GameObject restartButton;
	static float bestScore;
	public static GameManager instance{
		get{
			return FindObjectOfType<GameManager>();
		}
	}

	int ballsCount = 0;
	int scoreMultiplier = 1;
	float baseScore;
	Camera camera;

	void Awake(){

		camera = Camera.main;
		Physics2D.autoSimulation = false;
		UpdateBestScore();
	}
	float downTime = 0f;
	bool gameover = false;
	bool readyToSpawn = false;
	bool charging = false;
	const int innerFieldLayer = 1 << 4;
	void Update()
	{
		if(gameover){
            scoreView.UpdateScore(baseScore);
			return;
		}
		var wasReadyToSpawn = readyToSpawn;
		readyToSpawn = downTime >= downTimeToSpawn;
		if(!wasReadyToSpawn && readyToSpawn){
			Instantiate(timeToSpawnEffectPrefab);
		}
        ballsCount = FindObjectsOfType<Particle>().Length;
        scoreMultiplier = 1 << ballsCount-1;
		if (Input.GetMouseButton(0))
		{
            var mouseRay = camera.ScreenPointToRay(Input.mousePosition);
            var pos = Vector3.ProjectOnPlane(mouseRay.origin, Vector3.forward);
			var innerCollider = Physics2D.OverlapPoint(pos, innerFieldLayer);
			if(innerCollider != null){            
                controlledParticle.gameObject.SetActive(true);
                downTime += Time.deltaTime;
                controlledParticle.transform.position = pos;
                scoreView.UpdateScore(baseScore, downTime, scoreMultiplier, disabled: !readyToSpawn);
			}else{
				if(controlledParticle.gameObject.activeSelf){
                    controlledParticle.transform.position = pos;
					OnControlledParticleCollision(controlledParticle.transform.position);
				}
			}
		}
		else{
            scoreView.UpdateScore(baseScore);
		}
        if (Input.GetMouseButtonUp(0))
        {
			var velocity = controlledParticle.GetAvarageVelocity();
            controlledParticle.gameObject.SetActive(false);
			if(readyToSpawn){
				var particle = Instantiate(particlePrefab, controlledParticle.transform.position, controlledParticle.transform.rotation);
				particle.initialVelocity = velocity;
				SumUpScore(downTime * scoreMultiplier);
			}
			downTime = 0f;
        }
	}
	void SumUpScore(float addition){
		baseScore += addition;
        scoreView.SumUpAnimation();
		UpdateBestScore();
	}
	void UpdateBestScore(){
		bestScore = Mathf.Max(bestScore, baseScore);
        bestScoreTextMesh.text = string.Format("{0:0.0}", bestScore);
    
	}
	void FixedUpdate(){
		if (!gameover)
		{
			Physics2D.Simulate(Time.fixedDeltaTime);
		}
	}
	public void OnControlledParticleCollision(Vector3 position){
		Instantiate(boomPrefab, position, Quaternion.identity);
		Debug.Log("GameOver");
		gameover = true;
		restartButton.SetActive(true);
		SumUpScore(downTime * scoreMultiplier);
		foreach(var p in FindObjectsOfType<Particle>()){
			p.RevertPos();
		}
	}
	public void Restart(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
