using GameX1.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace Stage1Base
{
    public class SavePicture : ComponentBase
    {
        
        [Inject]
        public NavigationManager? navManager { get; set; }

        public Picture? picture { get; set; }


        [Parameter]
        public string? PictureUrl
        {
            get;
            set;
        }

        [Parameter]
        public int Id
        {
            get;
            set;
        }


        protected override Task OnInitializedAsync()
        {
            picture = new();

            //restore url forward slash and question mark
            PictureUrl = PictureUrl!.Replace("U+2215", "/").Replace("0x3F", "?");

            return base.OnInitializedAsync();
        }


        //save picture to SQLite DB
        public async Task HandleSubmit()
        {
            //save picture to database for user
            using (var context = new DataContext())
            {

                var pic = new Picture()
                {
                    Name = picture!.Name,
                    Url = PictureUrl,
                    Power = picture.Power,
                    Age = picture.Age
                };

                await context.Pictures.AddAsync(pic);

                await context.SaveChangesAsync();
            }

            //navigate to game screen
            navManager!.NavigateTo("/stage1", true);
        }


        //Back / Cancel
        public void Return()
        {
            navManager!.NavigateTo("/stage1");
        }
    }
}
