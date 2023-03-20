namespace AppCore.Records.Bases
{
    /*
    IModifiedBy
    CreateDate (DateTime), CreatedBy (string), UpdateDate (DateTime), UpdatedBy (string) 
    */
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
