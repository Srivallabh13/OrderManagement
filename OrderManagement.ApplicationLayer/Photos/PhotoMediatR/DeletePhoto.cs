using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using OrderManagement.ApplicationLayer.Photos.Interfaces;
using OrderManagement.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.ApplicationLayer.Photos.PhotoMediatR
{
    public class DeletePhoto
    {
        public class Command : IRequest<Unit>
        {
            public string Id { get; set; }
        }
        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly OrderDbContext _context;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly IHttpContextAccessor _httpContext;

            public Handler(OrderDbContext context, IPhotoAccessor photoAccessor, IHttpContextAccessor httpContext)
            {
                _context = context;
                _photoAccessor = photoAccessor;
                _httpContext = httpContext;
            }
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                string name = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.UserName == name);

                if (user == null) throw new Exception("User doesn't exist");

                var photo = user.Photos.FirstOrDefault(x=>x.Id == request.Id);

                if (photo == null) throw new Exception("Photo doesn't exist");

                var result = await _photoAccessor.DeletePhotoFromCloudinary(photo.Id);

                if (result == null) throw new Exception("Problem deleting the photo");
                user.ImageUrl = null;
                    user.Photos.Remove(photo);
                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;
                throw new Exception("Problem deleting the photo");
            }
        }
    }
}
