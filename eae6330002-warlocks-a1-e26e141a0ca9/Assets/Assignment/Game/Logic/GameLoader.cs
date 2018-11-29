using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour, ISceneLoader {

    [SerializeField]
    private Game game = null;

    #region ISceneLoader

    // TODO: implement, dummy load for now
    private float loadProgress = 0f;
    private float maxProgress = 5f;
    private string loadPhase = "Game";
    public IEnumerator Load() {
        loadProgress = 0f;
        while (loadProgress < maxProgress) {
            yield return new WaitForEndOfFrame();
            loadProgress += Time.deltaTime;
        }
        yield break;
    }

    public void OnSceneReady() {
        game.StartGame();
        Destroy(gameObject);
    }

    public float LoadProgress { get { return loadProgress / maxProgress; } }
    public string LoadPhase { get { return loadPhase; } }

    #endregion

    // manually trigger game start if scene launch in editor
#if UNITY_EDITOR
    void Start() {
        App app = FindObjectOfType<App>();
        if(app == null) {
            OnSceneReady();
        }
    }
#endif

}
