namespace Dreadborne.Scene;

internal sealed class SceneManager
{
    private readonly Stack<IScene> _sceneStack;

    public SceneManager() => _sceneStack = new();

    public void AddScene(IScene scene)
    {
        scene.Awake(Game.Content);
        _sceneStack.Push(scene);
    }

    public void RemoveScene() => _sceneStack.Pop();

    public IScene GetCurrentScene() => _sceneStack.Peek();
}