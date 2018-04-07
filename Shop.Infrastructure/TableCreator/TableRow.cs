namespace Shop.Infrastructure.TableCreator
{
    using System.Collections.Generic;

    public class TableRow
    {
        public int Id { get; set; }

        public List<string> Cells { get; set; }
    }
}