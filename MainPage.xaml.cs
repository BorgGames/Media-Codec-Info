namespace UWP_Codec_Info {
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    using Windows.Media.Core;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage: Page {
        public MainPage() {
            this.InitializeComponent();
        }

        async Task EnumerateCodecs() {
            try {
                var codecQuery = new CodecQuery();
                var videoEncoders = await codecQuery.FindAllAsync(CodecKind.Video, CodecCategory.Encoder, "");
                var audioEncoders = await codecQuery.FindAllAsync(CodecKind.Audio, CodecCategory.Encoder, "");
                var videoDecoders = await codecQuery.FindAllAsync(CodecKind.Video, CodecCategory.Decoder, "");
                var audioDecoders = await codecQuery.FindAllAsync(CodecKind.Audio, CodecCategory.Decoder, "");
                var allCodecs = videoEncoders.Concat(audioEncoders).Concat(videoDecoders).Concat(audioDecoders);
                this.DataContext = new ObservableCollection<CodecViewModel>(allCodecs.Select(c => new CodecViewModel(c)));
            } catch (TypeInitializationException) {
                this.DataContext = new ObservableCollection<CodecViewModel> {
                    new CodecViewModel {
                        Category = CodecCategory.Encoder,
                        DisplayName = "Windows N editions are not supported",
                        Features = string.Empty,
                        Kind = CodecKind.Audio,
                    },
                };
            }
        }

        async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            await this.EnumerateCodecs();
        }
    }
}
