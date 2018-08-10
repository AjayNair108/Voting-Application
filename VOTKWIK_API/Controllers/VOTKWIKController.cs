using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VOTKWIK_API.POCO;
using VOTKWIK_API.Response;
using VOTKWIK_API.SMS;
using VOTKWIK_DAL.Entities;
using VOTKWIK_DAL.Model;

namespace VOTKWIK_API.Controllers
{
    public class VOTKWIKController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage Get()
        {
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private VOTKWIKContext db = new VOTKWIKContext();


        #region  GET methods to validate
        [Route("VOTKWIK/{adharNo}/AuthenticateUser")]
        public HttpResponseMessage GetAuthenticatedUser(string adharNo, [FromUri] string contactNumber)
        {
            var Rsult = false;
            if (!string.IsNullOrEmpty(adharNo) && !string.IsNullOrEmpty(contactNumber))
                Rsult = (from a in db.Users.Where(x => x.AdharNo == adharNo.Trim()).ToList()
                         join b in db.UserDetails.Where(x => x.ContactNumber == "+" + contactNumber.Trim())
            on a.UserId equals b.UserId
                         where a.IsActive && b.IsActive
                         select a
                    ).Distinct().Any();

            if (Rsult == true)
                return new HttpResponseMessage(HttpStatusCode.OK);
            else
                return new HttpResponseMessage(HttpStatusCode.NoContent);
        }

        [Route("VOTKWIK/{ballotNo}/AuthenticateBallot")]
        public HttpResponseMessage GetAuthenticatedBallotNum(string ballotNo)
        {
            var Rsult = false;
            DateTime urrentDateTime = DateTime.Now;
            if (!string.IsNullOrEmpty(ballotNo))
                Rsult = db.UserSystemDetails.Where(x => x.TokenNoBroadCast == ballotNo.Trim() && urrentDateTime >= x.StarteDate && urrentDateTime <= x.EndDate).Any();

            if (Rsult == true)
                return new HttpResponseMessage(HttpStatusCode.OK);
            else
                return new HttpResponseMessage(HttpStatusCode.NotAcceptable);
        }
        #endregion

        #region Get Method for Booth Result

        [Route("VOTKWIK/{boothNo}/GetResult")]
        public List<BoothResult> GetBoothResult(string boothNo)
        {
            List<BoothResult> lstBoothResult;
            if (!string.IsNullOrEmpty(boothNo))
            {
                var objuserSystemDetail = db.UserSystemDetails.Where(x => x.TokenNoBroadCast == boothNo).SingleOrDefault();
                var lstCandidateBallotDetails = from a in db.CandidateBallotDetails.Where(x => x.IsActive && x.UserSystemDetailID == objuserSystemDetail.UserSystemDetailID) select a;
                if (objuserSystemDetail != null)
                {
                    lstBoothResult = (from Cd in db.CandidateDetails.Where(x => x.IsActive)
                                      join cbd in lstCandidateBallotDetails
                                      on Cd.CandidateDetailId equals cbd.CandidateDetailId
                                      into ps
                                      from b in ps.DefaultIfEmpty()
                                      select new BoothResult
                                      {
                                          //CandidateName = Cd.CandidateName == null ? "" : Cd.CandidateName,
                                          VoteCount = b.VoteCount == null ? 0 : b.VoteCount
                                      }).ToList();
                    return lstBoothResult;
                }
            }
            lstBoothResult = new List<BoothResult>();
            return lstBoothResult;
        }
        #endregion

        #region  POST methods -- To post the data into database

        [Route("VOTKWIK/{userId}/AddCandidate")]
        public HttpResponseMessage PostCandidate(long userId, [FromBody]List<CandidateDetailPOCO> lstCandidateDetail)
        {
            if (lstCandidateDetail != null && lstCandidateDetail.Count() > 0)
            {
                foreach (var objCandidateDetail in lstCandidateDetail)
                {
                    try
                    {
                        var user = db.Users.Where(x => x.UserName == objCandidateDetail.CandidateName.Trim()).SingleOrDefault();
                        if (user != null)
                        {
                            db.CandidateDetails.Add(new CandidateDetail()
                            {
                                CandidateID = user == null ? 0 : user.UserId,
                                UserId = userId,
                                IsActive = true
                            });
                            db.SaveChanges();

                            var userDetails = db.UserDetails.Where(x => x.UserId == user.UserId).SingleOrDefault();

                            //Notify Candidate 

                            var message = "This is to inform you that you are elected as candidate for TestElection";
                            var result = new BoSms().SendSMS(user.UserName, userDetails.ContactNumber, message);
                        }
                    }
                    catch (Exception ex)
                    {
                        return new HttpResponseMessage(HttpStatusCode.Ambiguous);
                    }
                }
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            return new HttpResponseMessage(HttpStatusCode.NotAcceptable);
        }

        [Route("VOTKWIK/{userId}/AddSystemDetails")]
        public HttpResponseMessage PostSystemDetails(long userId, [FromBody]UserSystemDetail objUserSystemDetail)
        {
            if (userId > 0 && objUserSystemDetail != null)
            {
                try
                {
                    objUserSystemDetail.IsActive = true;
                    objUserSystemDetail.IsLocalAdmin = true;
                    db.UserSystemDetails.Add(objUserSystemDetail);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotModified);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.OK);
        }


        [Route("VOTKWIK/{userId}/AddVote")]
        public HttpResponseMessage PostVote(long userId, [FromUri]string ballotNum, [FromUri] long candidateId)
        {
            if (userId > 0 && !string.IsNullOrEmpty(ballotNum))
            {
                try
                {
                    var objUserSystemDetails = db.UserSystemDetails.Where(x => x.TokenNoBroadCast == ballotNum).Distinct().SingleOrDefault();
                    db.VoterDetails.Add(
                             new VoterDetail()
                             {
                                 UserId = userId,
                                 UserSystemDetailID = objUserSystemDetails.UserSystemDetailID,
                                 IsActive = true
                             }
                        );

                    var objCandidateBallotDetails = db.CandidateBallotDetails.Where(x => x.CandidateDetailId == candidateId).SingleOrDefault();

                    if (objCandidateBallotDetails == null)
                    {
                        db.CandidateBallotDetails.Add(
                              new CandidateBallotDetail()
                              {
                                  CandidateDetailId = candidateId,
                                  IsActive = true,
                                  VoteCount = 1,
                                  UserSystemDetailID = objUserSystemDetails.UserSystemDetailID
                              }
                          );
                    }
                    else
                    {
                        objCandidateBallotDetails.VoteCount += 1;
                        db.Entry(objCandidateBallotDetails).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
                catch (Exception ex)
                {
                    return new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
                }
            }
            return new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
        }

        #endregion

        #region User Add
        [Route("VOTKWIK/User")]
        public HttpResponseMessage PostUser([FromBody]User objuser)
        {
            try
            {
                db.Users.Add(objuser);
                db.SaveChanges();
                var message = "Congratulation! you can use VOTWIK app to vote.";
                var result = new BoSms().SendSMS(objuser.UserName, objuser.UserDetails.SingleOrDefault(x => x.UserId == objuser.UserId).ContactNumber, message);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
        #endregion
    }
}
