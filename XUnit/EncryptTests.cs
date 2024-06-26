using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using TestableApp.ViewModels;
using TestableApp.Views;
using Xunit;

namespace TestableApp.Headless.XUnit;

public class EncryptTests
{
    [AvaloniaFact]
    public void Caesar1()
    {
        var window = new MainWindow
        {
            DataContext = new MainWindowViewModel()
        };

        window.Show();

        // Set values to the input boxes by simulating text input:
        window.NChars.Focus();
        window.KeyTextInput("20");

        // Or directly to the control:
        window.Website.Text = "testing";
        window.CharacterSet.Text = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890-=_+,;:<>";
        window.PostEncrypt.Text = "the big round sheep.";

        // Raise click event on the button:
        window.AddButton.Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        Assert.Equal("eaq Riv RutEy gtA Rrvvqd Rtyqp", window.ResultBox.Text);
    }
}