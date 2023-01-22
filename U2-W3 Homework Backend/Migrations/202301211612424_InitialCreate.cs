namespace U2_W3_Homework_Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ordini",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Quantita = c.Int(nullable: false),
                        IndirizzoSpedizione = c.String(),
                        Nota = c.String(),
                        OrdineConfermato = c.Boolean(nullable: false),
                        OrdineConsegnato = c.Boolean(nullable: false),
                        DataOrdine = c.DateTime(nullable: false),
                        IDPizze = c.Int(nullable: false),
                        IDUtenti = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Pizze", t => t.IDPizze)
                .ForeignKey("dbo.Utenti", t => t.IDUtenti)
                .Index(t => t.IDPizze)
                .Index(t => t.IDUtenti);
            
            CreateTable(
                "dbo.Pizze",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false),
                        Foto = c.String(nullable: false),
                        Prezzo = c.Decimal(nullable: false, precision: 15, scale: 2),
                        TempoPreparazione = c.Int(nullable: false),
                        Ingredienti = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Utenti",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Ruolo = c.String(),
                        Nome = c.String(nullable: false),
                        Cognome = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ordini", "IDUtenti", "dbo.Utenti");
            DropForeignKey("dbo.Ordini", "IDPizze", "dbo.Pizze");
            DropIndex("dbo.Ordini", new[] { "IDUtenti" });
            DropIndex("dbo.Ordini", new[] { "IDPizze" });
            DropTable("dbo.Utenti");
            DropTable("dbo.Pizze");
            DropTable("dbo.Ordini");
        }
    }
}
