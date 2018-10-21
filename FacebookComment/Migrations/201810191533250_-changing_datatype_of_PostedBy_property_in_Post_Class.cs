namespace FacebookComment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changing_datatype_of_PostedBy_property_in_Post_Class : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "PostedBy", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "PostedBy", c => c.Int(nullable: false));
        }
    }
}
