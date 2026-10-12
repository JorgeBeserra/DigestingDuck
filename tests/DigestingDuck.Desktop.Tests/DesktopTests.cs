using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Headless.NUnit;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using DigestingDuck.Desktop;
using NUnit.Framework;

[assembly: AvaloniaTestApplication(typeof(DigestingDuck.Tests.UiBuilder))]
namespace DigestingDuck.Tests;
public static class UiBuilder
{
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>().UseSkia()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions { UseHeadlessDrawing = false });
}
public class DesktopTests
{
    [AvaloniaTest]
    public async Task AllPagesRenderAndEmptyCredentialsDoNotContactTheBroker()
    {
        var directory = Path.Combine(Path.GetTempPath(), "digestingduck-ui-"+Guid.NewGuid().ToString("N"));
        var window = new MainWindow(directory) { Width = 1180, Height = 790 };
        window.Show();
        var tabs = window.GetVisualDescendants().OfType<TabControl>().Single();
        Assert.That(tabs.Items.Count, Is.EqualTo(8));
        for (var i = 0; i < tabs.Items.Count; i++)
        {
            tabs.SelectedIndex = i;
            Dispatcher.UIThread.RunJobs();
            using var frame = window.CaptureRenderedFrame() ?? throw new InvalidOperationException("Frame not rendered");
            Assert.That(frame.PixelSize.Width, Is.EqualTo(1180));
            Assert.That(frame.PixelSize.Height, Is.EqualTo(790));
        }
        tabs.SelectedIndex = 1;
        Dispatcher.UIThread.RunJobs();
        var login = window.GetVisualDescendants().OfType<Button>().Single(x => x.Content?.ToString() == "Conectar");
        login.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        Dispatcher.UIThread.RunJobs();
        Assert.That(window.GetVisualDescendants().OfType<TextBlock>().Any(x => x.Text == "Verifique o registro"), Is.True);
        var closed = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        window.Closed += (_, _) => closed.TrySetResult();
        window.Close();
        await closed.Task.WaitAsync(TimeSpan.FromSeconds(5));
        if (Directory.Exists(directory)) Directory.Delete(directory,true);
    }
}
