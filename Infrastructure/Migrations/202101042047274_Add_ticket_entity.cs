namespace Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ticket_entity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "ticket.Tickets",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SessionSeat_SessionId = c.Int(nullable: false),
                        SessionSeat_SeatRow = c.Int(nullable: false),
                        SessionSeat_SeatNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("session.SessionSeats", t => new { t.SessionSeat_SessionId, t.SessionSeat_SeatRow, t.SessionSeat_SeatNumber })
                .Index(t => new { t.SessionSeat_SessionId, t.SessionSeat_SeatRow, t.SessionSeat_SeatNumber });

            RenameColumn("session.SessionSeats", "Ticket", "TicketId");
        }

        public override void Down()
        {
            RenameColumn("session.SessionSeats", "TicketId", "Ticket");
            DropForeignKey("ticket.Tickets", new[] { "SessionSeat_SessionId", "SessionSeat_SeatRow", "SessionSeat_SeatNumber" }, "session.SessionSeats");
            DropIndex("ticket.Tickets", new[] { "SessionSeat_SessionId", "SessionSeat_SeatRow", "SessionSeat_SeatNumber" });
            DropTable("ticket.Tickets");
        }
    }
}
