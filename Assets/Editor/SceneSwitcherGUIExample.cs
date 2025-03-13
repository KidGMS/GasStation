using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
[InitializeOnLoad]
public class SceneSwitcherGUIExample {
  private static readonly string WindowTitle = "Switcher";
  private static readonly string SceneIntroPath = "Assets/Scenes/Intro.unity";
  private static readonly string SceneLoaderPath = "Assets/Scenes/Loader.unity";
  private static readonly string SceneGamePath = "Assets/Scenes/Game.unity";
  private static readonly string SceneMainPath = "Assets/Scenes/Main.unity";
  private static readonly string CollapseIconPath = "Assets/Sprite/Icon/collapse.png";
  private static readonly string ExpandIconPath = "Assets/Sprite/Icon/expand.png";
  private static readonly string CloseIconPath = "Assets/Sprite/Icon/close.png";
  private static readonly string ButtonNameSceneIntro = "Intro";
  private static readonly string ButtonNameSceneLoader = "Loader";
  private static readonly string ButtonNameSceneGame = "Game";
  private static readonly string ButtonNameSceneMain = "Main";
  private static readonly string WindowPosX = "WindowPosX";
  private static readonly string WindowPosY = "WindowPosY";
  private static readonly string WindowWidth = "WindowWidth";
  private static readonly string WindowHeight = "WindowHeight";
  private static readonly string WindowCollapsed = "WindowCollapsed";
  private static readonly string WindowVisible = "WindowVisible";

  private static readonly int WindowID = 123456;
  private static readonly int OpenWidowSize = 130;
  private static readonly int CloseWidowSize = 50;
  private static readonly Vector2 ControlButtonSize = new Vector2(20, 20);
  private static readonly Vector2 MainButtonSize = new Vector2(60, 20);

  private static Rect _windowRect = new Rect(100, 100, 60, 50);
  private static bool _windowVisible = true;
  private static bool _isCollapsed;
  private static Texture2D _collapseTexture;
  private static Texture2D _expandTexture;
  private static Texture2D _closeTexture;

  [MenuItem("Tools/Scene Switcher")]
  public static void ToggleWindow() {
    _windowVisible = !_windowVisible;
  }

  private static void LoadTextures() {
    _collapseTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(CollapseIconPath);
    _expandTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(ExpandIconPath);
    _closeTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(CloseIconPath);
  }

  private static void OnSceneGUI (SceneView sceneView) {
    if (!_windowVisible) {
      return;
    }

    Handles.BeginGUI();

    _windowRect = GUILayout.Window(WindowID, _windowRect, DrawSceneWindow, WindowTitle);

    Handles.EndGUI();
  }

  private static void DrawSceneWindow (int id) {
    if (Event.current.type == EventType.Layout) {
      SaveWindowPosition();
    }

    GUILayout.BeginHorizontal();

    if (GUILayout.Button(_isCollapsed ? _expandTexture : _collapseTexture, GUILayout.Width(ControlButtonSize.x), GUILayout.Height(ControlButtonSize.y))) {
      _isCollapsed = !_isCollapsed;
      _windowRect.height = _isCollapsed ? CloseWidowSize : OpenWidowSize;
    }

    if (GUILayout.Button(_closeTexture, GUILayout.Width(ControlButtonSize.x), GUILayout.Height(ControlButtonSize.y))) {
      _windowVisible = false;
    }

    GUILayout.EndHorizontal();

    if (!_isCollapsed) {
      DrawButton(ButtonNameSceneIntro, SceneIntroPath);
      DrawButton(ButtonNameSceneLoader, SceneLoaderPath);
      DrawButton(ButtonNameSceneGame, SceneGamePath);
      DrawButton(ButtonNameSceneMain, SceneMainPath);
    }

    GUI.DragWindow();
  }

  private static void DrawButton (string buttonName, string pathName) {
    if (GUILayout.Button(buttonName, GUILayout.Width(MainButtonSize.x), GUILayout.Height(MainButtonSize.y))) {
      EditorSceneManager.OpenScene(pathName);
    }
  }

  private static void SaveWindowPosition() {
    EditorPrefs.SetFloat(WindowPosX, _windowRect.x);
    EditorPrefs.SetFloat(WindowPosY, _windowRect.y);
    EditorPrefs.SetFloat(WindowWidth, _windowRect.width);
    EditorPrefs.SetFloat(WindowHeight, _windowRect.height);
    EditorPrefs.SetBool(WindowCollapsed, _isCollapsed);
    EditorPrefs.SetBool(WindowVisible, _windowVisible);
  }

  private static void LoadWindowPosition() {
    if (!EditorPrefs.HasKey(WindowPosX) || !EditorPrefs.HasKey(WindowPosY) || !EditorPrefs.HasKey(WindowWidth) || !EditorPrefs.HasKey(WindowHeight)
        || !EditorPrefs.HasKey(WindowCollapsed) || !EditorPrefs.HasKey(WindowVisible)) {
      return;
    }

    _windowRect.x = EditorPrefs.GetFloat(WindowPosX);
    _windowRect.y = EditorPrefs.GetFloat(WindowPosY);
    _windowRect.width = EditorPrefs.GetFloat(WindowWidth);
    _windowRect.height = EditorPrefs.GetFloat(WindowHeight);
    _isCollapsed = EditorPrefs.GetBool(WindowCollapsed);
    _windowVisible = EditorPrefs.GetBool(WindowVisible);
  }

  static SceneSwitcherGUIExample() {
    SceneView.duringSceneGui += OnSceneGUI;
    LoadTextures();
    LoadWindowPosition();
  }

  private void OnDestroy() {
    SaveWindowPosition();
  }
}