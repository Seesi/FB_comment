namespace FacebookComment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatingRegisterViewModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfiles", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfiles", "Email");
        }
    }
}
