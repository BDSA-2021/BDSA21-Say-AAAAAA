using System.Timers;

namespace SELearning.Shared.Toast;

public delegate void OnDismissHandler();

public partial class ToastNotification : IDisposable
{
    public DateTime Created { get; init; } = DateTime.Now;
    public string Title { get; init; }
    public string Body { get; init; }
    public ToastType Type { get; init; }

    private int _duration { get; init; }
    private Timer _dismissTimer { get; init; }

    private IList<OnDismissHandler> _onDismissHandlers = new List<OnDismissHandler>();

    public ToastNotification(string title, string body, ToastType type)
    {
        Title = title;
        Body = body;
        Type = type;
    }

    /// <summary>
    /// Creates an auto dismissing toast notification. It will dismiss after the given duration.
    /// </summary>
    /// <param name="Duration">Amount of miliseconds that should pass before dismissing</param>
    public ToastNotification(string title, string body, ToastType type, int Duration)
        : this(title, body, type)
    {
        _duration = Duration;
        _dismissTimer = new Timer();
        _dismissTimer.Interval = Duration;
        _dismissTimer.Elapsed += (object sender, ElapsedEventArgs e) =>
        {
            Dismiss();
        };
        _dismissTimer.Start();
    }


    public void AddDismissHandler(OnDismissHandler handler)
    {
        _onDismissHandlers.Add(handler);
    }

    public void Dismiss()
    {
        foreach (var handler in _onDismissHandlers)
        {
            handler.Invoke();
        }
    }

    public void Dispose()
    {
        _dismissTimer.Dispose();
    }

    public static ToastNotification CreateNoAccessToastNotification() =>
        new("Access not available!", "Unfortunately, our authorization handler was not able " +
            "to find your access token. Please contact the admin", ToastType.Error, 10_000);

    public static ToastNotification CreateGenericErrorToastNotification(string message) =>
        new("Fatal error!", $"An error occurred: {message}", ToastType.Error, 10_000);

    public static ToastNotification CreateUnauthorized(string message) =>
        new("Unauthorized!", $"You are not authorized to {message}", ToastType.Error, 10_000);

    public static ToastNotification CreateLoggedError(string message)
    {
        System.Console.WriteLine(message);
        return CreateGenericErrorToastNotification(message);
    }
}
