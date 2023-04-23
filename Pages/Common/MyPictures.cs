using GameX1.Data;
using Microsoft.AspNetCore.Components;

namespace Stage1Base
{
    public class MyPictures : ComponentBase
    {
        
        [Inject]
        public NavigationManager? navManager { get; set; }

        public List<Picture>? pictures { get; set; }


        protected override Task OnInitializedAsync()
        {
            pictures = new();

            using (var context = new DataContext())
            {
                pictures = context.Pictures.ToList();
            }


            return base.OnInitializedAsync();
        }


        //Back / Cancel
        public void Return()
        {
            navManager!.NavigateTo("/stage1");
        }
    }
}
