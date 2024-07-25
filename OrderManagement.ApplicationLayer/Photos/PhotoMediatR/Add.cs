using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OrderManagement.ApplicationLayer.Photos.Interfaces;
using OrderManagement.DataAccess;
using OrderManagement.DomainLayer.Entities;
using System.Security.Claims;

namespace OrderManagement.ApplicationLayer.Photos.PhotoMediatR
{
    public class Add
    {
        public class Command : IRequest<Photo>
        {
            public IFormFile File { get; set; }
        }
        public class Handler : IRequestHandler<Command, Photo>
        {
            private readonly OrderDbContext _context;
            private readonly IPhotoAccessor photoAccessor;
            private readonly IHttpContextAccessor _httpContext;

            public Handler(OrderDbContext context, IPhotoAccessor photoAccessor, IHttpContextAccessor httpContext)
            {
                _context = context;
                _httpContext = httpContext;
                this.photoAccessor = photoAccessor;
            }
            public async Task<Photo> Handle(Command request, CancellationToken cancellationToken)
            {
                string name = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                 var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.UserName == name);

                 if (user == null) return null;

                var photoUploadResult = await photoAccessor.AddPhoto(request.File);
                var photo = new Photo
                {
                    Url = photoUploadResult.Url,
                    Id = photoUploadResult.PublicId
                };
                user.ImageUrl = photo.Url;
                if(user.Photos.Count>0)
                {
                    foreach(Photo image in user.Photos)
                    {
                        await photoAccessor.DeletePhotoFromCloudinary(image.Id);
                        _context.Photos.Remove(image);
                    }
                    user.Photos.Clear();
                }
                user.Photos.Add(photo);
                var result = await _context.SaveChangesAsync() > 0;

                if (result) return photo;

                throw new Exception("Problem Adding photo");
            }
        }
    }
}
