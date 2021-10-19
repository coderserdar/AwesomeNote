using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeNote
{
    public class NoteFolderDataContext : DataContext
    {
        public const string ConnectionString = @"Data Source=isostore:/NoteAndFolders.sdf";
        public NoteFolderDataContext(string connectionString)
            : base(connectionString) { }
        public Table<NoteFolder> NoteFolders;
        public Table<Note> Notes;
        public Table<AppSettings> AppSettings;
    }
}
