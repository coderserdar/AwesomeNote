using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Windows.Media.Imaging;

namespace AwesomeNote
{
    [Table]
    public class NoteFolder
    {
        [Column(IsPrimaryKey = true,
IsDbGenerated = true,
DbType = "INT NOT NULL Identity",
CanBeNull = false)]
        public int NoteFolderId { get; set; }
        [Column]
        public string NoteFolderName { get; set; }
        [Column]
        public string NoteFolderDescription { get; set; }
        [Column]
        public bool IsPasswordProtected { get; set; }
        [Column]
        public string NoteFolderPassword { get; set; }
        [Column]
        public string FontFamily { get; set; }
        [Column]
        public string FontSize { get; set; }
        [Column]
        public string NoteOrderBy { get; set; }
        [Column]
        public string NoteOrderStyle { get; set; }
        [Column]
        public DateTime CreationDate { get; set; }

        [Column]
        public DateTime ModificationDate { get; set; }

        [Column]
        public int NoteCount { get; set; }

        [Column]
        public string NameCount { get; set; }

        [Column (DbType = "Image", UpdateCheck = UpdateCheck.Never)]
        public byte[] FolderBackground { get; set; }
        //[Column]
        //public string FontStyle { get; set; }
        //[Column]
        //public string FontWeight { get; set; }
    }
}
