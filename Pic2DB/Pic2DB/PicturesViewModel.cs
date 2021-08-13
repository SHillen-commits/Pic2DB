using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Pic2DB
{
    public class PicturesViewModel
    {
        public ObservableCollection<Pictures> pictures { get; set; }
        public PicturesDataBase database;

        public PicturesViewModel()
        {
            pictures = new ObservableCollection<Pictures>();
            InitDBAndLoadPictures();
        }

        private async void InitDBAndLoadPictures()
        {
            database = await PicturesDataBase.Instance;
            //int i = await database.DeleteItemsAsync();

            RetrievePicturesFromDBAndDisplay();
        }

        public async void RetrievePicturesFromDBAndDisplay()
        {
            List<Pictures> pics = await database.GetItemsAsync();
            pictures.Clear();
            Debug.WriteLine("pics in LoadPictures: " + pics.Count);
            foreach (Pictures pic in pics)
            {
                ImageSource imgSrc = ImageSource.FromFile(pic.photoPath);
                pic.imgSrc = imgSrc;
                pictures.Add(pic);
            }
            Debug.WriteLine("pictures.Add in LoadPictures: " + pictures.Count);
        }

        /// <summary>
        /// Warning: Drops the whole database, for testing purposes only!
        /// </summary>
        public async void DeleteAllPictures ()
        {
            database.DeleteItemsAsync().Wait();
            RetrievePicturesFromDBAndDisplay();

        }

        private ObservableCollection<Pictures> GetTestData()
        {
            return new ObservableCollection<Pictures>
            {
                new Pictures { Name="Marie" , photoPath=@"https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/images-images/local-sml.png" },
                new Pictures { Name="Josue" , photoPath=@"https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/images-images/local-sml.png" },
                new Pictures { Name="Maritza", photoPath=@"File: C:\Users\SHI\AppData\Local\Packages\7c4a7640-5cf1-47d5-8f65-898291e9eb07_fv24t9v6w4nb8\LocalCache\Unbenannt.PNG" }
            };
        }

        internal void AddPicture(string Name, string imgPath)
        {
            Pictures tmpPic = new Pictures { Name = Name, photoPath = imgPath };            
            
            Task<int> saveTask = database.SaveItemAsync(tmpPic);
            saveTask.ContinueWith(async antecedent =>
            {
                Debug.WriteLine($"Number of rows updated during AddPicture: {antecedent.Result}.");

                RetrievePicturesFromDBAndDisplay();
            }
            );
        }
    }
}