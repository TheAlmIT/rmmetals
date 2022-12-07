namespace RM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddName : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.myusers", "Address", c => c.String());

            //AddColumn("dbo.myusers", "Address2", c => c.String());

            AddColumn("dbo.myusers", "Approved", c => c.Boolean());

        }

        public override void Down()
        {
            //DropColumn("dbo.myusers", "Address");

            //DropColumn("dbo.myusers", "Address2");

            AddColumn("dbo.myusers", "Approved", c => c.Boolean());


        }
    }
}
