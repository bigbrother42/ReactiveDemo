using Newtonsoft.Json.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ReactiveDemo.ViewModels.MainWindow.DeepSeek
{
    public class DeepSeekDemoViewModel : ViewModelBase
    {
        #region Field

        private static readonly string apiKey = "sk-989bef8c6eb8452b86363aa5007dce9f";
        private static readonly string apiUrl = "https://api.deepseek.com";

        #endregion

        #region PrivateProperty



        #endregion

        #region ReactiveCommand

        public AsyncReactiveCommand CallAPICommand { get; set; }

        #endregion

        #region ReactiveProperty



        #endregion

        #region Request

        

        #endregion

        #region Events



        #endregion

        #region Override

        protected override void InitData()
        {
            base.InitData();

        }

        protected override void RegisterProperties()
        {
            base.RegisterProperties();
        }

        protected override void RegisterCommands()
        {
            base.RegisterCommands();

            CallAPICommand = new AsyncReactiveCommand().AddTo(DisposablePool);
            CallAPICommand.Subscribe(CallAPIMethodAsync).AddTo(DisposablePool);
        }

        protected override void RegisterPubEvents()
        {
            base.RegisterPubEvents();
        }

        #endregion

        #region Method

        private async Task CallAPIMethodAsync()
        {
            string searchTerm = "4+5等于多少？";

            try
            {
                var response = await CallDeepSeekApiAsync(searchTerm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private async Task<String> CallDeepSeekApiAsync(string searchTerm)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {apiKey}");
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                var requestData = new
                {
                    prompt = searchTerm,
                    max_tokens = 50
                };

                var jsonContent = JsonConvert.SerializeObject(requestData);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{apiUrl}", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return responseJson;
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API call failed: {response.StatusCode}\n{errorResponse}");
                }
            }
        }

        #endregion
    }
}
