using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Pic2DB
{
    [Table("Pictures")]
    public class Pictures
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }

        public string photoPath { get; set; }

        [Ignore]
        public ImageSource imgSrc { get; set; }
}
}
