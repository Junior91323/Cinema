namespace Cinema.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Movie_UserId : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movies", "UserId", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "UserId", c => c.String());
        }
    }
}
