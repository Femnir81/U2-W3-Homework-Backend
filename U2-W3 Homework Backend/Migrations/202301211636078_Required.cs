namespace U2_W3_Homework_Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Required : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pizze", "Foto", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pizze", "Foto", c => c.String(nullable: false));
        }
    }
}
