using GameX1.Data;
using GameX1.Pages.Common;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

namespace Stage1Base
{
    public class Stage1Base : ComponentBase
    {
        private string _apiName = "Picture/";
        private HttpClient? client;


        [Inject]
        public NavigationManager? navManager { get; set; }

        public List<Picture>? Pictures { get; set; }

        public string? CurrentPictureURL { get; set; }

        public int CurrentPictureId { get; set; }

        public int CurrentPictureIndex { get; set; }

        public int ConfigMaxPictureStageCount { get; set; } = 0;

        public bool DisableNextButton { get; set; } = false;

        protected override async Task<Task> OnInitializedAsync()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7131/api/")
            };

            CurrentPictureIndex = 0;

            Pictures = new List<Picture>();

            //first get the max PictureId from ViewdPicture Table frrom user database
            int externalPictureId = 0;

            ////code here for fetch external id from user database
            using (var context = new DataContext())
            {
                try
                {
                    externalPictureId = context.ViewedPicture.Max(p => p.PictureId);
                }
                catch { }
            }

            //get 5 pictures from API using the Max ExternalPictureId
            Pictures = await GetPicturesFromAPI(externalPictureId);
            await SetNextPicture();

            return base.OnInitializedAsync();
        }

        private async Task<List<Picture>> GetPicturesFromAPI(int externalPictureId)
        {
            //get 5 pictures from API using the Max ExternalPictureId
            var response = await client!.GetAsync(_apiName + "get-multiple/" + externalPictureId);

            if (response.IsSuccessStatusCode)
            {
                List<Picture> pictureList = await response.Content.ReadAsAsync<List<Picture>>();

                if (pictureList.Count > 0)
                {
                    Pictures!.AddRange(pictureList);
                    return pictureList;
                }
            }

            return new List<Picture>();

        }

        public async Task LoadNextPicture()
        {
            CurrentPictureIndex++;
            if (Pictures != null && CurrentPictureIndex < Pictures.Count)
            {
                await SetNextPicture();
            }
            else
            {
                //fetch more pictures from API
                List<Picture> pictureList = await GetPicturesFromAPI(CurrentPictureId);

                if (pictureList!.Count > 0)
                {
                    //add pictures to UI PictureList
                    Pictures!.AddRange(pictureList);
                    await SetNextPicture();
                }
                else
                {
                    //no more pictures to show.
                    //disable next button.
                    DisableNextButton = true;
                }
            }
        }


        /// <summary>
        /// Sets the next picture to UI and saves the viewed picture to users viwed pics table
        /// </summary>
        private async Task SetNextPicture()
        {
            //set next picture
            CurrentPictureURL = Pictures![CurrentPictureIndex].Url;
            CurrentPictureId = Pictures![CurrentPictureIndex].PictureId; //this is the Id from API side, which becomes external id in UI database

            //save picture to viewed picture table
            using (var context = new DataContext())
            {
                var pic = new ViewedPicture()
                {
                    PictureId = CurrentPictureId
                };

                await context.ViewedPicture.AddAsync(pic);
                await context.SaveChangesAsync();
            }
        }

        public void LoadPreviousPicture()
        {
            if (CurrentPictureIndex > 0 && Pictures != null)
            {
                CurrentPictureIndex--;
                CurrentPictureURL = Pictures[CurrentPictureIndex].Url;
                CurrentPictureId = Pictures[CurrentPictureIndex].PictureId;
            }
        }


        //redirect to SavePicture page
        public void SavePicture()
        {
            //replace forward slashes and question mark with unicode values
            string cleanUrl = CurrentPictureURL!.Replace("/", "U+2215").Replace("?", "0x3F");

            navManager!.NavigateTo("/save-picture/" + cleanUrl + "&id=" + CurrentPictureId);
        }
    }
}
