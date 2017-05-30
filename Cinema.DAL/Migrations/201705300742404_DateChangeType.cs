namespace Cinema.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateChangeType : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Movies", "Year");
            AddColumn("dbo.Movies", "Year", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "Year");
            AddColumn("dbo.Movies", "Year", c => c.DateTime(nullable: false));
        }
    }
}
