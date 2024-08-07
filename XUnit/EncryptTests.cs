using Avalonia.Headless;
using Avalonia.Headless.XUnit;
using Avalonia.Input;
using TouchWater.ViewModels;
using TouchWater.Views;
using Xunit;

namespace TouchWater.Headless.XUnit;

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
        window.NCharsUpDown.Value = 20;

        // Or directly to the control:
        window.WebsiteTextbox.Text = "testing";
        window.CharsetTextbox.Text = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890-=_+,;:<>";
        window.EncryptTextbox.Text = "the big round sheep";

        // Raise click event on the button:
        window.WeaklyEncrypt.Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        // Copy the result output to verify reversible
        string output = window.EncryptOutput.Text;
        window.EncryptTextbox.Text = output;

        // Clears the previous output
        window.ClearButton.Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        // Raise click event on the button:
        window.WeaklyEncrypt.Focus();
        window.KeyPress(Key.Enter, RawInputModifiers.None);

        Assert.Equal("the big round sheep", window.EncryptOutput.Text);
    }
}