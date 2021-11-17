using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace ForceUtf8_2022
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "3.0.5", IconResourceID = 400)]
    [Guid(ForceUtf8_2022Package.PackageGuidString)]
    [ProvideAutoLoad("{ADFC4E65-0397-11D1-9F4E-00A0C911004F}", PackageAutoLoadFlags.BackgroundLoad)] // UIContextGuids.EmptySolution
    [ProvideAutoLoad("{ADFC4E64-0397-11D1-9F4E-00A0C911004F}", PackageAutoLoadFlags.BackgroundLoad)] // UIContextGuids.NoSolution
    [ProvideAutoLoad("{F1536EF8-92EC-443C-9ED7-FDADF150DA82}", PackageAutoLoadFlags.BackgroundLoad)] // UIContextGuids.SolutionExists
    public sealed class ForceUtf8_2022Package : AsyncPackage
    {
        public const string PackageGuidString = "e0107d16-b8b8-41b4-9974-3378f4058059";

        #region Package Members

        private DocumentEvents documentEvents;
        // 是否保存为带BOM头的UTF-8格式
        private bool withBOM = true;

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            var dte = await this.GetServiceAsync(typeof(DTE)) as DTE;
            if (dte != null)
            {
                documentEvents = dte.Events.DocumentEvents;
                documentEvents.DocumentSaved += DocumentEvents_DocumentSaved;
            }
        }

        private void DocumentEvents_DocumentSaved(Document document)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (document.Kind != "{8E7B96A8-E33D-11D0-A6D5-00C04FB67F6A}")
            {
                return;
            }

            var path = document.FullName;
            var stream = new FileStream(path, FileMode.Open);
            var reader = new StreamReader(stream, Encoding.Default, true);

            reader.Read();
            if (reader.CurrentEncoding != Encoding.Default)
            {
                stream.Close();
                return;
            }

            try
            {
                stream.Position = 0;
                // 尝试以UTF-8解码，失败进入异常，将文本转为UTF-8保存
                reader = new StreamReader(stream, new UTF8Encoding(withBOM, true));
                var text = reader.ReadToEnd();
                stream.Close();

                // UTF-8编码的文件，写一次将其转为UTF8 with BOM
                // UTF-8 with BOM编码的文件，在前面判断编码的时候就会被过滤掉，不需要处理，也不会变为UTF-8
                if (withBOM)
                {
                    File.WriteAllText(path, text, new UTF8Encoding(true));
                }
            }
            catch (DecoderFallbackException)
            {
                stream.Position = 0;
                reader = new StreamReader(stream, Encoding.Default);
                var text = reader.ReadToEnd();
                stream.Close();
                File.WriteAllText(path, text, new UTF8Encoding(withBOM));
            }
        }

        #endregion
    }
}
