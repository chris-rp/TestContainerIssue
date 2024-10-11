using PetaPoco;

namespace TCITests.Stuff;
[TableName("SomeTable"), PrimaryKey("Id")]
public class SomeTablePoco
{
    public int Id { get; set; }
    public string Name { get; set; }
}
