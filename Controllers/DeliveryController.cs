using ConvicartAdminApp.Data;
using ConvicartAdminApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConvicartAdminApp.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly ConvicartWarehouseContext _context;

        public DeliveryController(ConvicartWarehouseContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin, DeliveryPartner")]
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders.ToListAsync();
            return View(orders);
        }



        [HttpPost]
        [Authorize(Roles = "DeliveryPartner")]
        public async Task<IActionResult> AcceptDelivery(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null && order.Status == "OrderPlaced")
            {
                order.Status = "Delivery In Progress";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Orders));
        }

        [HttpPost]
        [Authorize(Roles = "DeliveryPartner")]
        public async Task<IActionResult> MarkAsDelivered(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null && order.Status == "Delivery In Progress")
            {
                order.Status = "Delivered";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Orders));
        }
    }
}
