namespace UWP_Codec_Info {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Windows.Media.Core;

    public class CodecViewModel {
        public CodecKind Kind { get; set; }
        public string DisplayName { get; set; }
        public CodecCategory Category { get; set; }
        public string Features { get; set; }

        public CodecViewModel(CodecInfo codecInfo) {
            this.Kind = codecInfo.Kind;
            this.DisplayName = codecInfo.DisplayName;
            this.Category = codecInfo.Category;
            this.Features = string.Join(Environment.NewLine, codecInfo.Subtypes.Select(SubtypeToFeature));
        }

        static string SubtypeToFeature(string subtype)
            => subtype2name.TryGetValue(subtype, out string feature) ? feature : subtype;

        static readonly Dictionary<string, string> subtype2name = new Dictionary<string, string>() { };

        static CodecViewModel() {
            var knownSubtypes = typeof(CodecSubtypes).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (var knownSubtype in knownSubtypes) {
                string featureName = knownSubtype.Name;
                if (featureName.StartsWith("VideoFormat"))
                    featureName = featureName.Substring("VideoFormat".Length).ToUpperInvariant();
                if (featureName.StartsWith("AudioFormat"))
                    featureName = featureName.Substring("AudioFormat".Length).ToUpperInvariant();
                string subtype = (string)knownSubtype.GetValue(null);
                subtype2name.Add(subtype, featureName);
            }
        }

        public CodecViewModel() { }
    }
}
