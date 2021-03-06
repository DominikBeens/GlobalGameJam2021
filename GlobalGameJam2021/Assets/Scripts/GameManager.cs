﻿using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using DB.SimpleFramework.SimpleAudioManager;

public class GameManager : Singleton<GameManager> {

    [SerializeField] private string menuScene = "Menu";
    [SerializeField] private string gameScene = "Game";
    [SerializeField] private AudioClip gameAudio;

    private List<AsyncOperation> loadOperations = new List<AsyncOperation>();
    private int currentLevel;

    public Camera Camera { get; private set; }

    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize() {
        SimpleAudioManager.Initialize();
        SimpleAudioManager.Play2D(gameAudio, 0.3f).SetLoop(true);
        yield return new WaitForSeconds(0.1f);
        yield return LoadMenuRoutine();
    }

    private IEnumerator LoadMenuRoutine() {
        yield return LoadSceneRoutine(menuScene);
    }

    private IEnumerator LoadLevelRoutine(int level) {
        currentLevel = level;
        BoardManager.Instance.SetCurrentLevel(level);
        yield return LoadSceneRoutine(gameScene);
        Camera = Camera.main;
        BoardManager.Instance.BuildLevel();
    }

    private IEnumerator LoadSceneRoutine(string sceneName) {
        loadOperations.Clear();
        loadOperations.Add(SceneManager.LoadSceneAsync(sceneName));
        while (!IsDoneLoading()) {
            yield return null;
        }
    }

    public void LoadLevel(int level) {
        StartCoroutine(LoadLevelRoutine(level));
    }

    public void LoadLevel(Level level) {
        int index = BoardManager.Instance.Level.IndexOf(level);
        if (index <= -1) { return; }
        StartCoroutine(LoadLevelRoutine(index));
    }

    public void RestartLevel() {
        StartCoroutine(LoadLevelRoutine(currentLevel));
    }

    public void LoadMenu() {
        StartCoroutine(LoadMenuRoutine());
    }

    private bool IsDoneLoading() {
        return loadOperations.All(x => x?.isDone ?? true);
    }
}
