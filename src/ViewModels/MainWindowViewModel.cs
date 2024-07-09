using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations;

namespace TouchWater.ViewModels
{
	public class MainWindowViewModel : ViewModelBase
	{
		public ICommand LoadWebsite { get; }
		public ICommand OpenDataFolder { get; }
		public ICommand FillEnglishCharset { get; }
		public ICommand FillMalayalamCharset { get; }
		public ICommand FillCharset { get; }
		public ICommand StrongEncrypt { get; }
		public ICommand WeakEncrypt { get; }
		public ICommand DecryptWeak { get; }
		public ICommand CopyResults { get; }

		public ICommand DoAsyncCommand { get; }

		/// <summary>
		///  This collection will store what the computer said
		/// </summary>
		public ObservableCollection<string> PostEncrypt { get; } = new ObservableCollection<string>();

		public string? Website {
			get => _Website;
			set => this.RaiseAndSetIfChanged(ref _Website, value?.Replace("\r\n", "\n")?.Replace("\n", ""));
		}

		public int NChars {
			get => _NChars;
			set => this.RaiseAndSetIfChanged(ref _NChars, int.Clamp(value, 1, 9999999));
		}

		public string? CharacterSet {
			get => _CharacterSet;
			set => this.RaiseAndSetIfChanged(ref _CharacterSet, value?.Replace("\r\n", "\n")?.Replace("\n", ""));
		}

		public string? EncryptInput {
			get => _EncryptInput;
			set => this.RaiseAndSetIfChanged(ref _EncryptInput, value);
		}

		private string? _Website;
		private int _NChars = 24;
		private string? _CharacterSet;
		private string? _EncryptInput;
		private string malay = "കഖഗഘങചഛജഝടഠഡഢണതഥദഹസപശബഭളറഴ";
		private string english = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890-=_+,;:<>";


		// We use the constructor to initialize the Commands.
		public MainWindowViewModel()
		{
			// We initiate our Commands using ReactiveCommand.Create...
			// see: https://www.reactiveui.net/docs/handbook/commands/

			IObservable<bool> website = this.WhenAnyValue(vm => vm.Website, (name) => !string.IsNullOrWhiteSpace(name));
			IObservable<bool> nchars = this.WhenAnyValue(vm => vm.NChars, (val) => int.IsPositive(val));
			IObservable<bool> charset = this.WhenAnyValue(vm => vm.CharacterSet, (name) => !string.IsNullOrWhiteSpace(name));
			IObservable<bool> allGood = this.WhenAnyValue(
				vm => vm.Website, vm => vm.NChars, vm => vm.CharacterSet, vm => vm.EncryptInput,
				(web, nch, cha, inp) => 
					!string.IsNullOrWhiteSpace(web) && 
					int.IsPositive(nch) && 
					!string.IsNullOrWhiteSpace(cha) && 
					!string.IsNullOrWhiteSpace(inp) && 
					cha.Length >= 2
			);

			LoadWebsite = ReactiveCommand.Create(() => PrepareFile(Website), website);
			OpenDataFolder = ReactiveCommand.Create(OpenFolder);

			FillEnglishCharset = ReactiveCommand.Create(() => CharacterSet = english);
			FillMalayalamCharset = ReactiveCommand.Create(() => CharacterSet = malay);
			FillCharset = ReactiveCommand.Create<string>(name => PrepareFile(name), website);

			StrongEncrypt = ReactiveCommand.Create(() => DoPrimary(), allGood);
			WeakEncrypt = ReactiveCommand.Create(() => DoAlt(), allGood);
			DecryptWeak = ReactiveCommand.Create(() => AddToConvo(Encrypt.Undo(EncryptInput, Website, CharacterSet, NChars)), allGood);

			CopyResults = ReactiveCommand.Create(() => Clippy.Paste(PostEncrypt));
			DoAsyncCommand = ReactiveCommand.CreateFromTask(DoAsync);
		}

		private void PrepareFile() {
			PrepareFile(Website);
		}

		private void PrepareFile(string? file) {
			Method data = LookupSubfolder.Key(Website);

			if (! MethodUtils.IsNull(data)) {
				CharacterSet = data.key;
				NChars = data.num;
			}
		}

		private void OpenFolder() {
			string lastError = "";
			try {
				ProcessStartInfo startInfo = new ProcessStartInfo {
					Arguments = Folder.Get(),
					FileName = "explorer.exe"
				};

				Process.Start(startInfo);
			} catch (Exception e) {
				lastError = e.Message;
			}
		}

		private void DoPrimary() {
			AddToConvo(Encrypt.Primary(EncryptInput, Website, CharacterSet, NChars));
		}

		private void DoAlt() {
			AddToConvo(Encrypt.Alt(EncryptInput, Website, CharacterSet, NChars));
		}

		private void DoUndo() {
			AddToConvo(Encrypt.Undo(EncryptInput, Website, CharacterSet, NChars));
		}

		// Just a helper to add content to PostEncrypt
		private void AddToConvo(string content) {
			PostEncrypt.Add(content);
		}

		// This method is an async Task because opening the pod bay doors can take long time.
		// We don't want our UI to become unresponsive.
		private async Task DoAsync() {
			PostEncrypt.Clear();
			AddToConvo( "Preparing to open the Pod Bay...");
			// wait a second
			await Task.Delay(1000);

			AddToConvo("Pod Bay is open to space!");
		}
	}
}
