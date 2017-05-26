namespace Cinema.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SQL_Movie_Users_FK : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE Movies ADD CONSTRAINT FK_Movies_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)");
        }
        
        public override void Down()
        {
            Sql("ALTER TABLE Movies DROP CONSTRAINT FK_Movies_AspNetUsers; ");
        }
    }
}
