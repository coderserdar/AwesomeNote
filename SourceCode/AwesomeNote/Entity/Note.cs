using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace AwesomeNote
{
    [Table]
    public class Note
    {
        [Column(IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int NoteId { get; set; }

        [Column(DbType = "INT NOT NULL",
            CanBeNull = false)]
        public int NoteFolderId { get; set; }

        [Column]
        public string NoteName { get; set; }

        [Column]
        // burada notun kendisi NoteDescription oluyor
        public string NoteDescription { get; set; }

        [Column]
        public DateTime CreationDate { get; set; }

        [Column]
        public DateTime ModificationDate { get; set; }

        [Column]
        public string NameCreation { get; set; }

        [Column]
        public string NameDescription { get; set; }

    }
}
