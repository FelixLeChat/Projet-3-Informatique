using System;
using System.Net;
using Helper.Http;
using Models.Token;
using PrincessAPI.Infrastructure;

namespace PrincessAPI.Services
{
    /// <summary>
    /// Provide a way to use Dependency injection to access the user Token in any controller that
    /// </summary>
    public class AbstractService
    {
        protected UserToken UserToken { get; set; }
        protected AbstractService(UserToken userToken)
        {
            UserToken = userToken;
        }

        protected void TrySaveDb(SystemDBContext db)
        {
            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                string message = "";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        message += string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                    }
                }
                throw HttpResponseExceptionHelper.Create(message, HttpStatusCode.BadRequest);
            }
        }
    }
}