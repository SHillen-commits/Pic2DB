using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Pic2DB
{
    public partial class MainPage : ContentPage
    {
        private PicturesViewModel picViewModel = new PicturesViewModel();
        private string PhotoPath;

        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = picViewModel;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //TodoItemDatabase database = await TodoItemDatabase.Instance;
            //List<Pictures> pics = await picViewModel.database.GetItemsAsync();
            //this.collectionView.ItemsSource = pics;
        }

        private void ToolbarBtnAdd_Clicked(object sender, EventArgs e)
        {
            _ = AddPhotoFromFile(); //database.DeleteItemsAsync().Wait();  
        }

        private void ToolbarBtnDeleteAll_Clicked(object sender, EventArgs e)
        {
            picViewModel.DeleteAllPictures();
        }

        async Task AddPhotoFromFile()
        {
            FileResult photo = await MediaPicker.PickPhotoAsync();
            if (photo != null)
            {
                await LoadPhotoAsync(photo);

                string photoName = photo.FileName;
                string photoFullPath = photo.FullPath;
                ImageSource imgSource = ImageSource.FromFile(PhotoPath);

                string result = await DisplayPromptAsync("Titel eingeben", "");
                if ((result != null)&&(result != ""))
                    photoName = result;

                picViewModel.AddPicture(photoName, PhotoPath);
            }
        }

        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            {
                if (!File.Exists(newFile))
                {
                    using (var newStream = File.OpenWrite(newFile))
                    await stream.CopyToAsync(newStream);
                }
                   
                PhotoPath = newFile;
            }
        }
    }
}