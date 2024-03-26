using BaseDemo.Util;
using BaseDemo.Util.Extensions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace ReactiveDemo.ViewModels.MainWindow.SearchFileKeyWord
{
    public class SearchFileKeyWordViewModel : ViewModelBase
    {
        #region Field



        #endregion

        #region PrivateProperty

        private DispatcherTimer _timer = new DispatcherTimer();

        private DateTime _start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        #endregion

        #region ReactiveCommand

        public ReactiveCommand OpenLocalFileCommand { get; set; }

        public AsyncReactiveCommand SearchCommand { get; set; }

        public ReactiveCommand CancelCommand { get; set; }

        #endregion

        #region ReactiveProperty

        public ReactiveProperty<string> SelectedPath { get; set; }

        public ReactiveProperty<string> KeyWord { get; set; }

        public ReactiveProperty<string> Result { get; set; }

        public ReactiveProperty<bool> IsProgress { get; set; }

        public ReactiveProperty<double> Process { get; set; }

        public ReactiveProperty<string> SearchTime { get; set; }

        #endregion

        #region Request



        #endregion

        #region Events



        #endregion

        #region Override

        protected override void InitData()
        {
            base.InitData();

            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += RaiseTime;
        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();

            SelectedPath = new ReactiveProperty<string>().AddTo(DisposablePool);
            KeyWord = new ReactiveProperty<string>(string.Empty).AddTo(DisposablePool);
            Result = new ReactiveProperty<string>(string.Empty).AddTo(DisposablePool);
            IsProgress = new ReactiveProperty<bool>().AddTo(DisposablePool);
            Process = new ReactiveProperty<double>().AddTo(DisposablePool);
            SearchTime = new ReactiveProperty<string>().AddTo(DisposablePool);
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            OpenLocalFileCommand = new ReactiveCommand().AddTo(DisposablePool);
            OpenLocalFileCommand.Subscribe(OpenLocalFile).AddTo(DisposablePool);

            SearchCommand = new AsyncReactiveCommand().AddTo(DisposablePool);
            SearchCommand.Subscribe(Search).AddTo(DisposablePool);

            CancelCommand = new ReactiveCommand().AddTo(DisposablePool);
            CancelCommand.Subscribe(CancelSearch).AddTo(DisposablePool);
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Method

        private void CloseWindow()
        {
            FinishInteraction?.Invoke();
        }

        private void CancelSearch()
        {
            _cancellationTokenSource.Cancel();
        }

        private void OpenLocalFile()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                var result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    SelectedPath.Value = fbd.SelectedPath;
                }
            }
        }

        private void RaiseTime(object sender, EventArgs e)
        {
            _start = _start.AddSeconds(1);
            SearchTime.Value = $"{_start.Hour}:{_start.Minute}:{_start.Second}";
        }

        private async Task Search()
        {
            try
            {
                IsProgress.Value = true;
                Result.Value = string.Empty;
                Process.Value = 0.0;
                _timer.Start();
                _cancellationTokenSource = new CancellationTokenSource();

                var fileList = new List<string>();

                if (File.Exists(SelectedPath.Value))
                {
                    if (ZipUtil.IsZipFile(SelectedPath.Value))
                    {
                        return;
                    }

                    fileList.Add(SelectedPath.Value);
                }
                else if (Directory.Exists(SelectedPath.Value))
                {
                    var files = Directory.GetFiles(SelectedPath.Value);

                    foreach (string file in files)
                    {
                        if (ZipUtil.IsZipFile(file))
                        {
                            continue;
                        }

                        fileList.Add(file);
                    }
                }
                else
                {
                    Console.WriteLine("The path does not exist or is invalid.");
                }

                fileList = fileList.OrderBy(o => o).ToList();
                ConcurrentDictionary<string, int> searchResultList = new ConcurrentDictionary<string, int>();

                //var index = 0;
                //foreach (var filePath in fileList)
                //{
                //    var lineNum = await FindKeywordInFileAsync(filePath, KeyWord.Value);
                //    if (lineNum != -1)
                //    {
                //        searchResultList.TryAdd(Path.GetFileName(filePath), lineNum);
                //    }

                //    index++;

                //    Process.Value = index * 1.0 / fileList.Count * 1.0 * 100;
                //}

                //Parallel.ForEach(fileList, async filePath => {
                //    var lineNum = await FindKeywordInFileAsync(filePath, KeyWord.Value);
                //    if (lineNum != -1)
                //    {
                //        searchResultList.TryAdd(Path.GetFileName(filePath), lineNum);
                //    }
                //});

                SemaphoreSlim semaphore = new SemaphoreSlim(100);
                var index = 0;
                foreach (string filePath in fileList)
                {
                    if (_cancellationTokenSource.IsCancellationRequested) continue;

                    await semaphore.WaitAsync();

                    ThreadPool.QueueUserWorkItem(async state =>
                    {
                        try
                        {
                            var lineNum = await FindKeywordInFileAsync(filePath, KeyWord.Value, _cancellationTokenSource.Token);
                            if (lineNum != -1)
                            {
                                searchResultList.TryAdd(Path.GetFileName(filePath), lineNum);
                            }
                        }
                        catch(OperationCanceledException cancelEx)
                        {

                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    });

                    index++;
                    Process.Value = index * 1.0 / fileList.Count * 1.0 * 100;
                }

                semaphore.Release(10);

                if (searchResultList.IsEmpty)
                {
                    Result.Value = $"Key Word not exist!";
                }
                else
                {
                    foreach (var searchResult in searchResultList)
                    {
                        Result.Value += $"FileName: {searchResult.Key}, Line: {searchResult.Value}\r\n";
                    }
                }
            }
            catch(Exception e)
            {
                // do nothing
            }
            finally
            {
                IsProgress.Value = false;
                _timer.Stop();
                _start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            }
        }

        public async Task<int> FindKeywordInFileAsync(string fileName, string keyword, CancellationToken cancellationToken)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line;
                    int lineNumber = 0;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            return -1;
                        }

                        lineNumber++;
                        if (line.Contains(keyword))
                        {
                            return lineNumber;
                        }
                    }

                }
            }

            return -1;
        }

        #endregion
    }
}
