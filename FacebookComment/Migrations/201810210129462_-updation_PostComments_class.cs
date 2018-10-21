namespace FacebookComment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updation_PostComments_class : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PostComments", "CommentedBy", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PostComments", "CommentedBy", c => c.Int(nullable: false));
        }
    }
}
