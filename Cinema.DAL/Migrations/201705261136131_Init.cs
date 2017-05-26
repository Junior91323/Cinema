namespace Cinema.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Movies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Year = c.DateTime(nullable: false),
                        Producer = c.String(),
                        PosterURL = c.String(),
                        UserId = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifyDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Movies");
        }
    }
}
