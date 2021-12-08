namespace SELearning.Shared.Toast;

public delegate void OnDismissHandler();

public partial class ToastNotification {
    public DateTime Created { get; init; } = DateTime.Now;
    public string Title { get; init; }
    public string Body { get; init; }
    public ToastType Type { get; init; }

    private IList<OnDismissHandler> _onDismissHandlers = new List<OnDismissHandler>();

    public ToastNotification(string title, string body, ToastType type) {
        Title = title;
        Body = body;
        Type = type;
    }

    public void AddDismissHandler(OnDismissHandler handler) {
        _onDismissHandlers.Add(handler);
    }

    public void Dismiss() {
        foreach (var handler in _onDismissHandlers) {
            handler.Invoke();
        }
    }
}